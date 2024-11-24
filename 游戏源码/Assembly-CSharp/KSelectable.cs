using System;
using UnityEngine;

// Token: 0x02000A75 RID: 2677
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/KSelectable")]
public class KSelectable : KMonoBehaviour
{
	// Token: 0x170001FC RID: 508
	// (get) Token: 0x0600315C RID: 12636 RVA: 0x000C00B2 File Offset: 0x000BE2B2
	public bool IsSelected
	{
		get
		{
			return this.selected;
		}
	}

	// Token: 0x170001FD RID: 509
	// (get) Token: 0x0600315D RID: 12637 RVA: 0x000C00BA File Offset: 0x000BE2BA
	// (set) Token: 0x0600315E RID: 12638 RVA: 0x000C00CC File Offset: 0x000BE2CC
	public bool IsSelectable
	{
		get
		{
			return this.selectable && base.isActiveAndEnabled;
		}
		set
		{
			this.selectable = value;
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x0600315F RID: 12639 RVA: 0x000C00D5 File Offset: 0x000BE2D5
	public bool DisableSelectMarker
	{
		get
		{
			return this.disableSelectMarker;
		}
	}

	// Token: 0x06003160 RID: 12640 RVA: 0x001FEDA4 File Offset: 0x001FCFA4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.statusItemGroup = new StatusItemGroup(base.gameObject);
		base.GetComponent<KPrefabID>() != null;
		if (this.entityName == null || this.entityName.Length <= 0)
		{
			this.SetName(base.name);
		}
		if (this.entityGender == null)
		{
			this.entityGender = "NB";
		}
	}

	// Token: 0x06003161 RID: 12641 RVA: 0x001FEE0C File Offset: 0x001FD00C
	public virtual string GetName()
	{
		if (this.entityName == null || this.entityName == "" || this.entityName.Length <= 0)
		{
			global::Debug.Log("Warning Item has blank name!", base.gameObject);
			return base.name;
		}
		return this.entityName;
	}

	// Token: 0x06003162 RID: 12642 RVA: 0x000C00DD File Offset: 0x000BE2DD
	public void SetStatusIndicatorOffset(Vector3 offset)
	{
		if (this.statusItemGroup == null)
		{
			return;
		}
		this.statusItemGroup.SetOffset(offset);
	}

	// Token: 0x06003163 RID: 12643 RVA: 0x000C00F4 File Offset: 0x000BE2F4
	public void SetName(string name)
	{
		this.entityName = name;
	}

	// Token: 0x06003164 RID: 12644 RVA: 0x000C00FD File Offset: 0x000BE2FD
	public void SetGender(string Gender)
	{
		this.entityGender = Gender;
	}

	// Token: 0x06003165 RID: 12645 RVA: 0x001FEE60 File Offset: 0x001FD060
	public float GetZoom()
	{
		Bounds bounds = Util.GetBounds(base.gameObject);
		return 1.05f * Mathf.Max(bounds.extents.x, bounds.extents.y);
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x001FEE9C File Offset: 0x001FD09C
	public Vector3 GetPortraitLocation()
	{
		return Util.GetBounds(base.gameObject).center;
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x001FEEBC File Offset: 0x001FD0BC
	private void ClearHighlight()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.HighlightColour = new Color(0f, 0f, 0f, 0f);
		}
		base.Trigger(-1201923725, false);
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x001FEF10 File Offset: 0x001FD110
	private void ApplyHighlight(float highlight)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.HighlightColour = new Color(highlight, highlight, highlight, highlight);
		}
		base.Trigger(-1201923725, true);
	}

	// Token: 0x06003169 RID: 12649 RVA: 0x001FEF54 File Offset: 0x001FD154
	public void Select()
	{
		this.selected = true;
		this.ClearHighlight();
		this.ApplyHighlight(0.2f);
		base.Trigger(-1503271301, true);
		if (base.GetComponent<LoopingSounds>() != null)
		{
			base.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		if (base.transform.GetComponentInParent<LoopingSounds>() != null)
		{
			base.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			int childCount2 = base.transform.GetChild(i).childCount;
			for (int j = 0; j < childCount2; j++)
			{
				if (base.transform.GetChild(i).transform.GetChild(j).GetComponent<LoopingSounds>() != null)
				{
					base.transform.GetChild(i).transform.GetChild(j).GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
				}
			}
		}
		this.UpdateWorkerSelection(this.selected);
		this.UpdateWorkableSelection(this.selected);
	}

	// Token: 0x0600316A RID: 12650 RVA: 0x001FF06C File Offset: 0x001FD26C
	public void Unselect()
	{
		if (this.selected)
		{
			this.selected = false;
			this.ClearHighlight();
			base.Trigger(-1503271301, false);
		}
		if (base.GetComponent<LoopingSounds>() != null)
		{
			base.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		if (base.transform.GetComponentInParent<LoopingSounds>() != null)
		{
			base.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<LoopingSounds>() != null)
			{
				transform.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
			}
		}
		this.UpdateWorkerSelection(this.selected);
		this.UpdateWorkableSelection(this.selected);
	}

	// Token: 0x0600316B RID: 12651 RVA: 0x000C0106 File Offset: 0x000BE306
	public void Hover(bool playAudio)
	{
		this.ClearHighlight();
		if (!DebugHandler.HideUI)
		{
			this.ApplyHighlight(0.25f);
		}
		if (playAudio)
		{
			this.PlayHoverSound();
		}
	}

	// Token: 0x0600316C RID: 12652 RVA: 0x000C0129 File Offset: 0x000BE329
	private void PlayHoverSound()
	{
		if (CellSelectionObject.IsSelectionObject(base.gameObject))
		{
			return;
		}
		UISounds.PlaySound(UISounds.Sound.Object_Mouseover);
	}

	// Token: 0x0600316D RID: 12653 RVA: 0x000C013F File Offset: 0x000BE33F
	public void Unhover()
	{
		if (!this.selected)
		{
			this.ClearHighlight();
		}
	}

	// Token: 0x0600316E RID: 12654 RVA: 0x000C014F File Offset: 0x000BE34F
	public Guid ToggleStatusItem(StatusItem status_item, bool on, object data = null)
	{
		if (on)
		{
			return this.AddStatusItem(status_item, data);
		}
		return this.RemoveStatusItem(status_item, false);
	}

	// Token: 0x0600316F RID: 12655 RVA: 0x000C0165 File Offset: 0x000BE365
	public Guid ToggleStatusItem(StatusItem status_item, Guid guid, bool show, object data = null)
	{
		if (show)
		{
			if (guid != Guid.Empty)
			{
				return guid;
			}
			return this.AddStatusItem(status_item, data);
		}
		else
		{
			if (guid != Guid.Empty)
			{
				return this.RemoveStatusItem(guid, false);
			}
			return guid;
		}
	}

	// Token: 0x06003170 RID: 12656 RVA: 0x000C019A File Offset: 0x000BE39A
	public Guid SetStatusItem(StatusItemCategory category, StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		return this.statusItemGroup.SetStatusItem(category, status_item, data);
	}

	// Token: 0x06003171 RID: 12657 RVA: 0x000C01B8 File Offset: 0x000BE3B8
	public Guid ReplaceStatusItem(Guid guid, StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		if (guid != Guid.Empty)
		{
			this.statusItemGroup.RemoveStatusItem(guid, false);
		}
		return this.AddStatusItem(status_item, data);
	}

	// Token: 0x06003172 RID: 12658 RVA: 0x000C01EB File Offset: 0x000BE3EB
	public Guid AddStatusItem(StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		return this.statusItemGroup.AddStatusItem(status_item, data, null);
	}

	// Token: 0x06003173 RID: 12659 RVA: 0x000C0209 File Offset: 0x000BE409
	public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		this.statusItemGroup.RemoveStatusItem(status_item, immediate);
		return Guid.Empty;
	}

	// Token: 0x06003174 RID: 12660 RVA: 0x000C022C File Offset: 0x000BE42C
	public Guid RemoveStatusItem(Guid guid, bool immediate = false)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		this.statusItemGroup.RemoveStatusItem(guid, immediate);
		return Guid.Empty;
	}

	// Token: 0x06003175 RID: 12661 RVA: 0x000C024F File Offset: 0x000BE44F
	public bool HasStatusItem(StatusItem status_item)
	{
		return this.statusItemGroup != null && this.statusItemGroup.HasStatusItem(status_item);
	}

	// Token: 0x06003176 RID: 12662 RVA: 0x000C0267 File Offset: 0x000BE467
	public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
	{
		return this.statusItemGroup.GetStatusItem(category);
	}

	// Token: 0x06003177 RID: 12663 RVA: 0x000C0275 File Offset: 0x000BE475
	public StatusItemGroup GetStatusItemGroup()
	{
		return this.statusItemGroup;
	}

	// Token: 0x06003178 RID: 12664 RVA: 0x001FF164 File Offset: 0x001FD364
	public void UpdateWorkerSelection(bool selected)
	{
		Workable[] components = base.GetComponents<Workable>();
		if (components.Length != 0)
		{
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].worker != null && components[i].GetComponent<LoopingSounds>() != null)
				{
					components[i].GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
				}
			}
		}
	}

	// Token: 0x06003179 RID: 12665 RVA: 0x001FF1B8 File Offset: 0x001FD3B8
	public void UpdateWorkableSelection(bool selected)
	{
		WorkerBase component = base.GetComponent<WorkerBase>();
		if (component != null && component.GetWorkable() != null)
		{
			Workable workable = base.GetComponent<WorkerBase>().GetWorkable();
			if (workable.GetComponent<LoopingSounds>() != null)
			{
				workable.GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
			}
		}
	}

	// Token: 0x0600317A RID: 12666 RVA: 0x000C027D File Offset: 0x000BE47D
	protected override void OnLoadLevel()
	{
		this.OnCleanUp();
		base.OnLoadLevel();
	}

	// Token: 0x0600317B RID: 12667 RVA: 0x001FF20C File Offset: 0x001FD40C
	protected override void OnCleanUp()
	{
		if (this.statusItemGroup != null)
		{
			this.statusItemGroup.Destroy();
			this.statusItemGroup = null;
		}
		if (this.selected && SelectTool.Instance != null)
		{
			if (SelectTool.Instance.selected == this)
			{
				SelectTool.Instance.Select(null, true);
			}
			else
			{
				this.Unselect();
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x0400213D RID: 8509
	private const float hoverHighlight = 0.25f;

	// Token: 0x0400213E RID: 8510
	private const float selectHighlight = 0.2f;

	// Token: 0x0400213F RID: 8511
	public string entityName;

	// Token: 0x04002140 RID: 8512
	public string entityGender;

	// Token: 0x04002141 RID: 8513
	private bool selected;

	// Token: 0x04002142 RID: 8514
	[SerializeField]
	private bool selectable = true;

	// Token: 0x04002143 RID: 8515
	[SerializeField]
	private bool disableSelectMarker;

	// Token: 0x04002144 RID: 8516
	private StatusItemGroup statusItemGroup;
}
