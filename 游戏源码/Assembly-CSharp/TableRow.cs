using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DD5 RID: 7637
[AddComponentMenu("KMonoBehaviour/scripts/TableRow")]
public class TableRow : KMonoBehaviour
{
	// Token: 0x06009F9C RID: 40860 RVA: 0x003D1CCC File Offset: 0x003CFECC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.selectMinionButton != null)
		{
			this.selectMinionButton.onClick += this.SelectMinion;
			this.selectMinionButton.onDoubleClick += this.SelectAndFocusMinion;
		}
	}

	// Token: 0x06009F9D RID: 40861 RVA: 0x00107D21 File Offset: 0x00105F21
	public GameObject GetScroller(string scrollerID)
	{
		return this.scrollers[scrollerID];
	}

	// Token: 0x06009F9E RID: 40862 RVA: 0x00107D2F File Offset: 0x00105F2F
	public GameObject GetScrollerBorder(string scrolledID)
	{
		return this.scrollerBorders[scrolledID];
	}

	// Token: 0x06009F9F RID: 40863 RVA: 0x003D1D1C File Offset: 0x003CFF1C
	public void SelectMinion()
	{
		MinionIdentity minionIdentity = this.minion as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		SelectTool.Instance.Select(minionIdentity.GetComponent<KSelectable>(), false);
	}

	// Token: 0x06009FA0 RID: 40864 RVA: 0x003D1D50 File Offset: 0x003CFF50
	public void SelectAndFocusMinion()
	{
		MinionIdentity minionIdentity = this.minion as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		SelectTool.Instance.SelectAndFocus(minionIdentity.transform.GetPosition(), minionIdentity.GetComponent<KSelectable>(), new Vector3(8f, 0f, 0f));
	}

	// Token: 0x06009FA1 RID: 40865 RVA: 0x003D1DA4 File Offset: 0x003CFFA4
	public void ConfigureAsWorldDivider(Dictionary<string, TableColumn> columns, TableScreen screen)
	{
		ScrollRect scroll_rect = base.gameObject.GetComponentInChildren<ScrollRect>();
		this.rowType = TableRow.RowType.WorldDivider;
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			if (keyValuePair.Value.scrollerID != "")
			{
				TableColumn value = keyValuePair.Value;
				break;
			}
		}
		scroll_rect.onValueChanged.AddListener(delegate(Vector2 <p0>)
		{
			if (screen.CheckScrollersDirty())
			{
				return;
			}
			screen.SetScrollersDirty(scroll_rect.horizontalNormalizedPosition);
		});
	}

	// Token: 0x06009FA2 RID: 40866 RVA: 0x003D1E50 File Offset: 0x003D0050
	public void ConfigureContent(IAssignableIdentity minion, Dictionary<string, TableColumn> columns, TableScreen screen)
	{
		this.minion = minion;
		KImage componentInChildren = base.GetComponentInChildren<KImage>(true);
		componentInChildren.colorStyleSetting = ((minion == null) ? this.style_setting_default : this.style_setting_minion);
		componentInChildren.ColorState = KImage.ColorSelector.Inactive;
		CanvasGroup component = base.GetComponent<CanvasGroup>();
		if (component != null && minion as StoredMinionIdentity != null)
		{
			component.alpha = 0.6f;
		}
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			GameObject gameObject;
			if (minion == null)
			{
				if (this.isDefault)
				{
					gameObject = keyValuePair.Value.GetDefaultWidget(base.gameObject);
				}
				else
				{
					gameObject = keyValuePair.Value.GetHeaderWidget(base.gameObject);
				}
			}
			else
			{
				gameObject = keyValuePair.Value.GetMinionWidget(base.gameObject);
			}
			this.widgets.Add(keyValuePair.Value, gameObject);
			keyValuePair.Value.widgets_by_row.Add(this, gameObject);
			if (keyValuePair.Value.scrollerID != "")
			{
				foreach (string text in keyValuePair.Value.screen.column_scrollers)
				{
					if (!(text != keyValuePair.Value.scrollerID))
					{
						if (!this.scrollers.ContainsKey(text))
						{
							GameObject gameObject2 = Util.KInstantiateUI(this.scrollerPrefab, base.gameObject, true);
							ScrollRect scroll_rect = gameObject2.GetComponent<ScrollRect>();
							this.scrollbar = gameObject2.GetComponentInChildren<Scrollbar>();
							scroll_rect.horizontalScrollbar = this.scrollbar;
							scroll_rect.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
							scroll_rect.onValueChanged.AddListener(delegate(Vector2 <p0>)
							{
								if (screen.CheckScrollersDirty())
								{
									return;
								}
								screen.SetScrollersDirty(scroll_rect.horizontalNormalizedPosition);
							});
							this.scrollers.Add(text, scroll_rect.content.gameObject);
							if (scroll_rect.content.transform.parent.Find("Border") != null)
							{
								this.scrollerBorders.Add(text, scroll_rect.content.transform.parent.Find("Border").gameObject);
							}
						}
						gameObject.transform.SetParent(this.scrollers[text].transform);
						this.scrollers[text].transform.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;
					}
				}
			}
		}
		this.RefreshColumns(columns);
		if (minion != null)
		{
			base.gameObject.name = minion.GetProperName();
		}
		else if (this.isDefault)
		{
			base.gameObject.name = "defaultRow";
		}
		if (this.selectMinionButton)
		{
			this.selectMinionButton.transform.SetAsLastSibling();
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.scrollerBorders)
		{
			RectTransform rectTransform = keyValuePair2.Value.rectTransform();
			float width = rectTransform.rect.width;
			keyValuePair2.Value.transform.SetParent(base.gameObject.transform);
			rectTransform.anchorMin = (rectTransform.anchorMax = new Vector2(0f, 1f));
			rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
			RectTransform rectTransform2 = this.scrollers[keyValuePair2.Key].transform.parent.rectTransform();
			Vector3 a = this.scrollers[keyValuePair2.Key].transform.parent.rectTransform().GetLocalPosition() - new Vector3(rectTransform2.sizeDelta.x / 2f, -1f * (rectTransform2.sizeDelta.y / 2f), 0f);
			a.y = 0f;
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 374f);
			rectTransform.SetLocalPosition(a + Vector3.up * rectTransform.GetLocalPosition().y + Vector3.up * -rectTransform.anchoredPosition.y);
		}
	}

	// Token: 0x06009FA3 RID: 40867 RVA: 0x003D2360 File Offset: 0x003D0560
	public void RefreshColumns(Dictionary<string, TableColumn> columns)
	{
		foreach (KeyValuePair<string, TableColumn> keyValuePair in columns)
		{
			if (keyValuePair.Value.on_load_action != null)
			{
				keyValuePair.Value.on_load_action(this.minion, keyValuePair.Value.widgets_by_row[this]);
			}
		}
	}

	// Token: 0x06009FA4 RID: 40868 RVA: 0x003D23E0 File Offset: 0x003D05E0
	public void RefreshScrollers()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.scrollers)
		{
			ScrollRect component = keyValuePair.Value.transform.parent.GetComponent<ScrollRect>();
			component.GetComponent<LayoutElement>().minWidth = Mathf.Min(768f, component.content.sizeDelta.x);
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.scrollerBorders)
		{
			RectTransform rectTransform = keyValuePair2.Value.rectTransform();
			rectTransform.sizeDelta = new Vector2(this.scrollers[keyValuePair2.Key].transform.parent.GetComponent<LayoutElement>().minWidth, rectTransform.sizeDelta.y);
		}
	}

	// Token: 0x06009FA5 RID: 40869 RVA: 0x003D24F0 File Offset: 0x003D06F0
	public GameObject GetWidget(TableColumn column)
	{
		if (this.widgets.ContainsKey(column) && this.widgets[column] != null)
		{
			return this.widgets[column];
		}
		global::Debug.LogWarning("Widget is null or row does not contain widget for column " + ((column != null) ? column.ToString() : null));
		return null;
	}

	// Token: 0x06009FA6 RID: 40870 RVA: 0x00107D3D File Offset: 0x00105F3D
	public IAssignableIdentity GetIdentity()
	{
		return this.minion;
	}

	// Token: 0x06009FA7 RID: 40871 RVA: 0x00107D45 File Offset: 0x00105F45
	public bool ContainsWidget(GameObject widget)
	{
		return this.widgets.ContainsValue(widget);
	}

	// Token: 0x06009FA8 RID: 40872 RVA: 0x003D254C File Offset: 0x003D074C
	public void Clear()
	{
		foreach (KeyValuePair<TableColumn, GameObject> keyValuePair in this.widgets)
		{
			keyValuePair.Key.widgets_by_row.Remove(this);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04007CE2 RID: 31970
	public TableRow.RowType rowType;

	// Token: 0x04007CE3 RID: 31971
	private IAssignableIdentity minion;

	// Token: 0x04007CE4 RID: 31972
	private Dictionary<TableColumn, GameObject> widgets = new Dictionary<TableColumn, GameObject>();

	// Token: 0x04007CE5 RID: 31973
	private Dictionary<string, GameObject> scrollers = new Dictionary<string, GameObject>();

	// Token: 0x04007CE6 RID: 31974
	private Dictionary<string, GameObject> scrollerBorders = new Dictionary<string, GameObject>();

	// Token: 0x04007CE7 RID: 31975
	public bool isDefault;

	// Token: 0x04007CE8 RID: 31976
	public KButton selectMinionButton;

	// Token: 0x04007CE9 RID: 31977
	[SerializeField]
	private ColorStyleSetting style_setting_default;

	// Token: 0x04007CEA RID: 31978
	[SerializeField]
	private ColorStyleSetting style_setting_minion;

	// Token: 0x04007CEB RID: 31979
	[SerializeField]
	private GameObject scrollerPrefab;

	// Token: 0x04007CEC RID: 31980
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x02001DD6 RID: 7638
	public enum RowType
	{
		// Token: 0x04007CEE RID: 31982
		Header,
		// Token: 0x04007CEF RID: 31983
		Default,
		// Token: 0x04007CF0 RID: 31984
		Minion,
		// Token: 0x04007CF1 RID: 31985
		StoredMinon,
		// Token: 0x04007CF2 RID: 31986
		WorldDivider
	}
}
