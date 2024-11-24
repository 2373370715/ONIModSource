using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

public class PlanBuildingToggle : KToggle
{
	private BuildingDef def;

	private HashedString buildingCategory;

	private TechItem techItem;

	private List<int> gameSubscriptions = new List<int>();

	private bool researchComplete;

	private Sprite sprite;

	[SerializeField]
	private MultiToggle toggle;

	[SerializeField]
	private ToolTip tooltip;

	[SerializeField]
	private LocText text;

	[SerializeField]
	private LocText text_listView;

	[SerializeField]
	private Image buildingIcon;

	[SerializeField]
	private Image buildingIcon_listView;

	[SerializeField]
	private Image fgIcon;

	[SerializeField]
	private PlanScreen planScreen;

	private StringEntry subcategoryName;

	public void Config(BuildingDef def, PlanScreen planScreen, HashedString buildingCategory)
	{
		this.def = def;
		this.planScreen = planScreen;
		this.buildingCategory = buildingCategory;
		techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		gameSubscriptions.Add(Game.Instance.Subscribe(-107300940, CheckResearch));
		gameSubscriptions.Add(Game.Instance.Subscribe(-1948169901, CheckResearch));
		gameSubscriptions.Add(Game.Instance.Subscribe(1557339983, CheckResearch));
		sprite = def.GetUISprite();
		base.onClick += delegate
		{
			PlanScreen.Instance.OnSelectBuilding(base.gameObject, def);
			RefreshDisplay();
		};
		if (TUNING.BUILDINGS.PLANSUBCATEGORYSORTING.ContainsKey(def.PrefabID))
		{
			Strings.TryGet("STRINGS.UI.NEWBUILDCATEGORIES." + TUNING.BUILDINGS.PLANSUBCATEGORYSORTING[def.PrefabID].ToUpper() + ".NAME", out subcategoryName);
		}
		else
		{
			Debug.LogWarning("Building " + def.PrefabID + " has not been added to plan screen subcategory organization in BuildingTuning.cs");
		}
		CheckResearch();
		Refresh();
	}

	protected override void OnDestroy()
	{
		if (Game.Instance != null)
		{
			foreach (int gameSubscription in gameSubscriptions)
			{
				Game.Instance.Unsubscribe(gameSubscription);
			}
		}
		gameSubscriptions.Clear();
		base.OnDestroy();
	}

	private void CheckResearch(object data = null)
	{
		researchComplete = PlanScreen.TechRequirementsMet(techItem);
	}

	public bool CheckBuildingPassesSearchFilter(Def building)
	{
		if (BuildingGroupScreen.SearchIsEmpty)
		{
			return StandardDisplayFilter();
		}
		string text = BuildingGroupScreen.Instance.inputField.text;
		string obj = UI.StripLinkFormatting(building.Name).ToLower();
		text = text.ToUpper();
		if (obj.ToUpper().Contains(text))
		{
			return true;
		}
		if (subcategoryName != null && subcategoryName.String.ToUpper().Contains(text))
		{
			return true;
		}
		return false;
	}

	private bool StandardDisplayFilter()
	{
		if (researchComplete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
		{
			if (planScreen.ActiveCategoryToggleInfo != null)
			{
				return buildingCategory == (HashedString)planScreen.ActiveCategoryToggleInfo.userData;
			}
			return true;
		}
		return false;
	}

	public bool Refresh()
	{
		bool flag = false;
		flag = ((!BuildingGroupScreen.SearchIsEmpty) ? CheckBuildingPassesSearchFilter(def) : StandardDisplayFilter());
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
		PositionTooltip();
		RefreshLabel();
		RefreshDisplay();
		return result;
	}

	public void SwitchViewMode(bool listView)
	{
		text.gameObject.SetActive(!listView);
		text_listView.gameObject.SetActive(listView);
		buildingIcon.gameObject.SetActive(!listView);
		buildingIcon_listView.gameObject.SetActive(listView);
	}

	private void RefreshLabel()
	{
		if (text != null)
		{
			text.fontSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
			text_listView.fontSize = (ScreenResolutionMonitor.UsingGamepadUIMode() ? PlanScreen.fontSizeBigMode : PlanScreen.fontSizeStandardMode);
			text.text = def.Name;
			text_listView.text = def.Name;
		}
	}

	private void RefreshDisplay()
	{
		PlanScreen.RequirementsState buildableState = PlanScreen.Instance.GetBuildableState(def);
		bool flag = buildableState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive;
		bool flag2 = base.gameObject == PlanScreen.Instance.SelectedBuildingGameObject;
		_ = 3;
		if (flag2 && flag)
		{
			toggle.ChangeState(1);
		}
		else if (!flag2 && flag)
		{
			toggle.ChangeState(0);
		}
		else if (flag2 && !flag)
		{
			toggle.ChangeState(3);
		}
		else if (!flag2 && !flag)
		{
			toggle.ChangeState(2);
		}
		RefreshBuildingButtonIconAndColors(flag);
		RefreshFG(buildableState);
	}

	private void PositionTooltip()
	{
		tooltip.overrideParentObject = (PlanScreen.Instance.ProductInfoScreen.gameObject.activeSelf ? PlanScreen.Instance.ProductInfoScreen.rectTransform() : PlanScreen.Instance.buildingGroupsRoot);
		tooltip.tooltipPivot = Vector2.zero;
		tooltip.parentPositionAnchor = new Vector2(1f, 0f);
		tooltip.tooltipPositionOffset = new Vector2(4f, 0f);
		tooltip.ClearMultiStringTooltip();
		string newString = def.Name;
		string effect = def.Effect;
		tooltip.AddMultiStringTooltip(newString, PlanScreen.Instance.buildingToolTipSettings.BuildButtonName);
		tooltip.AddMultiStringTooltip(effect, PlanScreen.Instance.buildingToolTipSettings.BuildButtonDescription);
	}

	private void RefreshBuildingButtonIconAndColors(bool buttonAvailable)
	{
		if (sprite == null)
		{
			sprite = PlanScreen.Instance.defaultBuildingIconSprite;
		}
		buildingIcon.sprite = sprite;
		buildingIcon.SetNativeSize();
		buildingIcon_listView.sprite = sprite;
		float num = (ScreenResolutionMonitor.UsingGamepadUIMode() ? 3.25f : 4f);
		buildingIcon.rectTransform().sizeDelta /= num;
		Material material = (buttonAvailable ? PlanScreen.Instance.defaultUIMaterial : PlanScreen.Instance.desaturatedUIMaterial);
		if (buildingIcon.material != material)
		{
			buildingIcon.material = material;
			buildingIcon_listView.material = material;
		}
	}

	private void RefreshFG(PlanScreen.RequirementsState requirementsState)
	{
		if (requirementsState == PlanScreen.RequirementsState.Tech)
		{
			fgImage.sprite = PlanScreen.Instance.Overlay_NeedTech;
			fgImage.gameObject.SetActive(value: true);
		}
		else
		{
			fgImage.gameObject.SetActive(value: false);
		}
		string tooltipForRequirementsState = PlanScreen.GetTooltipForRequirementsState(def, requirementsState);
		if (tooltipForRequirementsState != null)
		{
			tooltip.AddMultiStringTooltip("\n", PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
			tooltip.AddMultiStringTooltip(tooltipForRequirementsState, PlanScreen.Instance.buildingToolTipSettings.ResearchRequirement);
		}
	}
}
