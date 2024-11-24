using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CB8 RID: 7352
[AddComponentMenu("KMonoBehaviour/scripts/DropDown")]
public class DropDown : KMonoBehaviour
{
	// Token: 0x17000A20 RID: 2592
	// (get) Token: 0x06009986 RID: 39302 RVA: 0x00103F1E File Offset: 0x0010211E
	// (set) Token: 0x06009987 RID: 39303 RVA: 0x00103F26 File Offset: 0x00102126
	public bool open { get; private set; }

	// Token: 0x17000A21 RID: 2593
	// (get) Token: 0x06009988 RID: 39304 RVA: 0x00103F2F File Offset: 0x0010212F
	public List<IListableOption> Entries
	{
		get
		{
			return this.entries;
		}
	}

	// Token: 0x06009989 RID: 39305 RVA: 0x003B5728 File Offset: 0x003B3928
	public void Initialize(IEnumerable<IListableOption> contentKeys, Action<IListableOption, object> onEntrySelectedAction, Func<IListableOption, IListableOption, object, int> sortFunction = null, Action<DropDownEntry, object> refreshAction = null, bool displaySelectedValueWhenClosed = true, object targetData = null)
	{
		this.targetData = targetData;
		this.sortFunction = sortFunction;
		this.onEntrySelectedAction = onEntrySelectedAction;
		this.displaySelectedValueWhenClosed = displaySelectedValueWhenClosed;
		this.rowRefreshAction = refreshAction;
		this.ChangeContent(contentKeys);
		this.openButton.ClearOnClick();
		this.openButton.onClick += delegate()
		{
			this.OnClick();
		};
		this.canvasScaler = GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>();
	}

	// Token: 0x0600998A RID: 39306 RVA: 0x00103F37 File Offset: 0x00102137
	public void CustomizeEmptyRow(string txt, Sprite icon)
	{
		this.emptyRowLabel = txt;
		this.emptyRowSprite = icon;
	}

	// Token: 0x0600998B RID: 39307 RVA: 0x00103F47 File Offset: 0x00102147
	public void OnClick()
	{
		if (!this.open)
		{
			this.Open();
			return;
		}
		this.Close();
	}

	// Token: 0x0600998C RID: 39308 RVA: 0x003B579C File Offset: 0x003B399C
	public void ChangeContent(IEnumerable<IListableOption> contentKeys)
	{
		this.entries.Clear();
		foreach (IListableOption item in contentKeys)
		{
			this.entries.Add(item);
		}
		this.built = false;
	}

	// Token: 0x0600998D RID: 39309 RVA: 0x003B57FC File Offset: 0x003B39FC
	private void Update()
	{
		if (!this.open)
		{
			return;
		}
		if (!Input.GetMouseButtonDown(0) && Input.GetAxis("Mouse ScrollWheel") == 0f && !KInputManager.steamInputInterpreter.GetSteamInputActionIsDown(global::Action.MouseLeft))
		{
			return;
		}
		float canvasScale = this.canvasScaler.GetCanvasScale();
		if (this.scrollRect.rectTransform().GetPosition().x + this.scrollRect.rectTransform().sizeDelta.x * canvasScale < KInputManager.GetMousePos().x || this.scrollRect.rectTransform().GetPosition().x > KInputManager.GetMousePos().x || this.scrollRect.rectTransform().GetPosition().y - this.scrollRect.rectTransform().sizeDelta.y * canvasScale > KInputManager.GetMousePos().y || this.scrollRect.rectTransform().GetPosition().y < KInputManager.GetMousePos().y)
		{
			this.Close();
		}
	}

	// Token: 0x0600998E RID: 39310 RVA: 0x003B5900 File Offset: 0x003B3B00
	private void Build(List<IListableOption> contentKeys)
	{
		this.built = true;
		for (int i = this.contentContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.contentContainer.GetChild(i));
		}
		this.rowLookup.Clear();
		if (this.addEmptyRow)
		{
			this.emptyRow = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
			this.emptyRow.GetComponent<KButton>().onClick += delegate()
			{
				this.onEntrySelectedAction(null, this.targetData);
				if (this.displaySelectedValueWhenClosed)
				{
					this.selectedLabel.text = (this.emptyRowLabel ?? UI.DROPDOWN.NONE);
				}
				this.Close();
			};
			string text = this.emptyRowLabel ?? UI.DROPDOWN.NONE;
			this.emptyRow.GetComponent<DropDownEntry>().label.text = text;
			if (this.emptyRowSprite != null)
			{
				this.emptyRow.GetComponent<DropDownEntry>().image.sprite = this.emptyRowSprite;
			}
		}
		for (int j = 0; j < contentKeys.Count; j++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
			IListableOption id = contentKeys[j];
			gameObject.GetComponent<DropDownEntry>().entryData = id;
			gameObject.GetComponent<KButton>().onClick += delegate()
			{
				this.onEntrySelectedAction(id, this.targetData);
				if (this.displaySelectedValueWhenClosed)
				{
					this.selectedLabel.text = id.GetProperName();
				}
				this.Close();
			};
			this.rowLookup.Add(id, gameObject);
		}
		this.RefreshEntries();
		this.Close();
		this.scrollRect.gameObject.transform.SetParent(this.targetDropDownContainer.transform);
		this.scrollRect.gameObject.SetActive(false);
	}

	// Token: 0x0600998F RID: 39311 RVA: 0x003B5AA0 File Offset: 0x003B3CA0
	private void RefreshEntries()
	{
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			DropDownEntry component = keyValuePair.Value.GetComponent<DropDownEntry>();
			component.label.text = keyValuePair.Key.GetProperName();
			if (component.portrait != null && keyValuePair.Key is IAssignableIdentity)
			{
				component.portrait.SetIdentityObject(keyValuePair.Key as IAssignableIdentity, true);
			}
		}
		if (this.sortFunction != null)
		{
			this.entries.Sort((IListableOption a, IListableOption b) => this.sortFunction(a, b, this.targetData));
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.rowLookup[this.entries[i]].transform.SetAsFirstSibling();
			}
			if (this.emptyRow != null)
			{
				this.emptyRow.transform.SetAsFirstSibling();
			}
		}
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair2 in this.rowLookup)
		{
			DropDownEntry component2 = keyValuePair2.Value.GetComponent<DropDownEntry>();
			this.rowRefreshAction(component2, this.targetData);
		}
		if (this.emptyRow != null)
		{
			this.rowRefreshAction(this.emptyRow.GetComponent<DropDownEntry>(), this.targetData);
		}
	}

	// Token: 0x06009990 RID: 39312 RVA: 0x00103F5E File Offset: 0x0010215E
	protected override void OnCleanUp()
	{
		Util.KDestroyGameObject(this.scrollRect);
		base.OnCleanUp();
	}

	// Token: 0x06009991 RID: 39313 RVA: 0x003B5C44 File Offset: 0x003B3E44
	public void Open()
	{
		if (this.open)
		{
			return;
		}
		if (!this.built)
		{
			this.Build(this.entries);
		}
		else
		{
			this.RefreshEntries();
		}
		this.open = true;
		this.scrollRect.gameObject.SetActive(true);
		this.scrollRect.rectTransform().localScale = Vector3.one;
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			keyValuePair.Value.SetActive(true);
		}
		float num = Mathf.Max(32f, this.rowEntryPrefab.GetComponent<LayoutElement>().preferredHeight);
		this.scrollRect.rectTransform().sizeDelta = new Vector2(this.scrollRect.rectTransform().sizeDelta.x, num * (float)Mathf.Min(this.contentContainer.childCount, 8));
		Vector3 vector = this.dropdownAlignmentTarget.TransformPoint(this.dropdownAlignmentTarget.rect.x, this.dropdownAlignmentTarget.rect.y, 0f);
		Vector2 v = new Vector2(Mathf.Min(0f, (float)Screen.width - (vector.x + (this.rowEntryPrefab.GetComponent<LayoutElement>().minWidth * this.canvasScaler.GetCanvasScale() + DropDown.edgePadding.x))), -Mathf.Min(0f, vector.y - (this.scrollRect.rectTransform().sizeDelta.y * this.canvasScaler.GetCanvasScale() + DropDown.edgePadding.y)));
		vector += v;
		this.scrollRect.rectTransform().SetPosition(vector);
	}

	// Token: 0x06009992 RID: 39314 RVA: 0x003B5E28 File Offset: 0x003B4028
	public void Close()
	{
		if (!this.open)
		{
			return;
		}
		this.open = false;
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			keyValuePair.Value.SetActive(false);
		}
		this.scrollRect.SetActive(false);
	}

	// Token: 0x040077C2 RID: 30658
	public GameObject targetDropDownContainer;

	// Token: 0x040077C3 RID: 30659
	public LocText selectedLabel;

	// Token: 0x040077C5 RID: 30661
	public KButton openButton;

	// Token: 0x040077C6 RID: 30662
	public Transform contentContainer;

	// Token: 0x040077C7 RID: 30663
	public GameObject scrollRect;

	// Token: 0x040077C8 RID: 30664
	public RectTransform dropdownAlignmentTarget;

	// Token: 0x040077C9 RID: 30665
	public GameObject rowEntryPrefab;

	// Token: 0x040077CA RID: 30666
	public bool addEmptyRow = true;

	// Token: 0x040077CB RID: 30667
	private static Vector2 edgePadding = new Vector2(8f, 8f);

	// Token: 0x040077CC RID: 30668
	public object targetData;

	// Token: 0x040077CD RID: 30669
	private List<IListableOption> entries = new List<IListableOption>();

	// Token: 0x040077CE RID: 30670
	private Action<IListableOption, object> onEntrySelectedAction;

	// Token: 0x040077CF RID: 30671
	private Action<DropDownEntry, object> rowRefreshAction;

	// Token: 0x040077D0 RID: 30672
	public Dictionary<IListableOption, GameObject> rowLookup = new Dictionary<IListableOption, GameObject>();

	// Token: 0x040077D1 RID: 30673
	private Func<IListableOption, IListableOption, object, int> sortFunction;

	// Token: 0x040077D2 RID: 30674
	private GameObject emptyRow;

	// Token: 0x040077D3 RID: 30675
	private string emptyRowLabel;

	// Token: 0x040077D4 RID: 30676
	private Sprite emptyRowSprite;

	// Token: 0x040077D5 RID: 30677
	private bool built;

	// Token: 0x040077D6 RID: 30678
	private bool displaySelectedValueWhenClosed = true;

	// Token: 0x040077D7 RID: 30679
	private const int ROWS_BEFORE_SCROLL = 8;

	// Token: 0x040077D8 RID: 30680
	private KCanvasScaler canvasScaler;
}
