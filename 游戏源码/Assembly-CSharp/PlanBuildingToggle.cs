using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001EA1 RID: 7841
public class PlanBuildingToggle : KToggle
{
	// Token: 0x0600A481 RID: 42113 RVA: 0x003E6CB0 File Offset: 0x003E4EB0
	public void Config(BuildingDef def, PlanScreen planScreen, HashedString buildingCategory)
	{
		this.def = def;
		this.planScreen = planScreen;
		this.buildingCategory = buildingCategory;
		this.techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		this.gameSubscriptions.Add(Game.Instance.Subscribe(-107300940, new Action<object>(this.CheckResearch)));
		this.gameSubscriptions.Add(Game.Instance.Subscribe(-1948169901, new Action<object>(this.CheckResearch)));
		this.gameSubscriptions.Add(Game.Instance.Subscribe(1557339983, new Action<object>(this.CheckResearch)));
		this.sprite = def.GetUISprite("ui", false);
		base.onClick += delegate()
		{
			PlanScreen.Instance.OnSelectBuilding(this.gameObject, def, null);
			this.RefreshDisplay();
		};
		if (TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(def.PrefabID))
		{
			Strings.TryGet("STRINGS.UI.NEWBUILDCATEGORIES." + TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[def.PrefabID].ToUpper() + ".NAME", out this.subcategoryName);
		}
		else
		{
			global::Debug.LogWarning("Building " + def.PrefabID + " has not been added to plan screen subcategory organization in BuildingTuning.cs");
		}
		this.CheckResearch(null);
		this.Refresh();
	}

	// Token: 0x0600A482 RID: 42114 RVA: 0x003E6E24 File Offset: 0x003E5024
	protected override void OnDestroy()
	{
		if (Game.Instance != null)
		{
			foreach (int id in this.gameSubscriptions)
			{
				Game.Instance.Unsubscribe(id);
			}
		}
		this.gameSubscriptions.Clear();
		base.OnDestroy();
	}

	// Token: 0x0600A483 RID: 42115 RVA: 0x0010AC2C File Offset: 0x00108E2C
	private void CheckResearch(object data = null)
	{
		this.researchComplete = PlanScreen.TechRequirementsMet(this.techItem);
	}

	// Token: 0x0600A484 RID: 42116 RVA: 0x003E6E9C File Offset: 0x003E509C
	public bool CheckBuildingPassesSearchFilter(Def building)
	{
		if (BuildingGroupScreen.SearchIsEmpty)
		{
			return this.StandardDisplayFilter();
		}
		string text = BuildingGroupScreen.Instance.inputField.text;
		string text2 = UI.StripLinkFormatting(building.Name).ToLower();
		text = text.ToUpper();
		return text2.ToUpper().Contains(text) || (this.subcategoryName != null && this.subcategoryName.String.ToUpper().Contains(text));
	}

	// Token: 0x0600A485 RID: 42117 RVA: 0x003E6F10 File Offset: 0x003E5110
	private bool StandardDisplayFilter()
	{
		return (this.researchComplete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive) && (this.planScreen.ActiveCategoryToggleInfo == null || this.buildingCategory == (HashedString)this.planScreen.ActiveCategoryToggleInfo.userData);
	}

	// Token: 0x0600A486 RID: 42118 RVA: 0x003E6F6C File Offset: 0x003E516C
	public bool Refresh()
	{
		bool flag;
		if (BuildingGroupScreen.SearchIsEmpty)
		{
			flag = this.StandardDisplayFilter();
		}
		else
		{
			flag = this.CheckBuildingPassesSearchFilter(this.def);
		}
		bool result = false;
		if (base.gameObject.activeSelf != flag)
		{
			base.gameObject.SetActive(flag);
			result = true;
		}
		if (!base.gameObject.activeSelf)
		{
			return result;
		}
		this.PositionTooltip();
		this.RefreshLabel();
		this.RefreshDisplay();
		return result;
	}

	// Token: 0x0600A487 RID: 42119 RVA: 0x003E6FD8 File Offset: 0x003E51D8
	public void SwitchViewMode(bool listView)
	{
		this.text.gameObject.SetActive(!listView);
		this.text_listView.gameObject.SetActive(listView);
		this.buildingIcon.gameObject.SetActive(!listView);
		this.buildingIcon_listView.gameObject.SetActive(listView);
	}

	// Token: 0x0600A488 RID: 42120 RVA: 0x003E7030 File Offset: 0x003E5230
	private void RefreshLabel()
	{
		if (this.text != null)
		{
			this.text.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
			this.text_listView.fontSize = (float)(ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
			this.text.text = this.def.Name;
			this.text_listView.text = this.def.Name;
		}
	}

	// Token: 0x0600A489 RID: 42121 RVA: 0x003E70B8 File Offset: 0x003E52B8
	private void RefreshDisplay()
	{
		PlanScreen.RequirementsState buildableState = PlanScreen.Instance.GetBuildableState(this.def);
		bool flag = buildableState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
		bool flag2 = base.gameObject == PlanScreen.Instance.SelectedBuildingGameObject;
		if (flag2 && flag)
		{
			this.toggle.ChangeState(1);
		}
		else if (!flag2 && flag)
		{
			this.toggle.ChangeState(0);
		}
		else if (flag2 && !flag)
		{
			this.toggle.ChangeState(3);
		}
		else if (!flag2 && !flag)
		{
			this.toggle.ChangeState(2);
		}
		this.RefreshBuildingButtonIconAndColors(flag);
		this.RefreshFG(buildableState);
	}

	// Token: 0x0600A48A RID: 42122 RVA: 0x003E7168 File Offset: 0x003E5368
	private void PositionTooltip()
	{
		this.tooltip.overrideParentObject = (PlanScreen.Instance.ProductInfoScreen.gameObject.activeSelf ? PlanScreen.Instance.ProductInfoScreen.rectTransform() : PlanScreen.Instance.buildingGroupsRoot);
		this.tooltip.tooltipPivot = Vector2.zero;
		this.tooltip.parentPositionAnchor = new Vector2(1f, 0f);
		this.tooltip.tooltipPositionOffset = new Vector2(4f, 0f);
		this.tooltip.ClearMultiStringTooltip();
		string name = this.def.Name;
		string effect = this.def.Effect;
		this.tooltip.AddMultiStringTooltip(name, PlanScreen.Instance.buildingToolTipSettings.BuildButtonName);
		this.tooltip.AddMultiStringTooltip(effect, PlanScreen.Instance.buildingToolTipSettings.BuildButtonDescription);
	}

	// Token: 0x0600A48B RID: 42123 RVA: 0x003E7250 File Offset: 0x003E5450
	private void RefreshBuildingButtonIconAndColors(bool buttonAvailable)
	{
		if (this.sprite == null)
		{
			this.sprite = PlanScreen.Instance.defaultBuildingIconSprite;
		}
		this.buildingIcon.sprite = this.sprite;
		this.buildingIcon.SetNativeSize();
		this.buildingIcon_listView.sprite = this.sprite;
		float d = ScreenResolutionMonitor.UsingGamepadUIMode() ? 3.25f : 4f;
		this.buildingIcon.rectTransform().sizeDelta /= d;
		Material material = buttonAvailable ? PlanScreen.Instance.defaultUIMaterial : PlanScreen.Instance.desaturatedUIMaterial;
		if (this.buildingIcon.material != material)
		{
			this.buildingIcon.material = material;
			this.buildingIcon_listView.material = material;
		}
	}

	// Token: 0x0600A48C RID: 42124 RVA: 0x003E7320 File Offset: 0x003E5520
	private void RefreshFG(PlanScreen.RequirementsState requirementsState)
	{
		if (requirementsState == PlanScreen.RequirementsState.Tech)
		{
			this.fgImage.sprite = PlanScreen.Instance.Overlay_NeedTech;
			this.fgImage.gameObject.SetActive(true);
		}
		else
		{
			this.fgImage.gameObject.SetActive(false);
		}
		string tooltipForRequirementsState = PlanScreen.GetTooltipForRequirementsState(this.def, requirementsState);
		if (tooltipForRequirementsState != null)
		{
			this.tooltip.AddMultiStringTooltip("\n", PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
			this.tooltip.AddMultiStringTooltip(tooltipForRequirementsState, PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
		}
	}

	// Token: 0x0400808B RID: 32907
	private BuildingDef def;

	// Token: 0x0400808C RID: 32908
	private HashedString buildingCategory;

	// Token: 0x0400808D RID: 32909
	private TechItem techItem;

	// Token: 0x0400808E RID: 32910
	private List<int> gameSubscriptions = new List<int>();

	// Token: 0x0400808F RID: 32911
	private bool researchComplete;

	// Token: 0x04008090 RID: 32912
	private Sprite sprite;

	// Token: 0x04008091 RID: 32913
	[SerializeField]
	private MultiToggle toggle;

	// Token: 0x04008092 RID: 32914
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x04008093 RID: 32915
	[SerializeField]
	private LocText text;

	// Token: 0x04008094 RID: 32916
	[SerializeField]
	private LocText text_listView;

	// Token: 0x04008095 RID: 32917
	[SerializeField]
	private Image buildingIcon;

	// Token: 0x04008096 RID: 32918
	[SerializeField]
	private Image buildingIcon_listView;

	// Token: 0x04008097 RID: 32919
	[SerializeField]
	private Image fgIcon;

	// Token: 0x04008098 RID: 32920
	[SerializeField]
	private PlanScreen planScreen;

	// Token: 0x04008099 RID: 32921
	private StringEntry subcategoryName;
}
