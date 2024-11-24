using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001AA1 RID: 6817
public class BuildMenuBuildingsScreen : KIconToggleMenu
{
	// Token: 0x06008E90 RID: 36496 RVA: 0x000EAD7D File Offset: 0x000E8F7D
	public override float GetSortKey()
	{
		return 8f;
	}

	// Token: 0x06008E91 RID: 36497 RVA: 0x00370CE0 File Offset: 0x0036EEE0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateBuildableStates();
		Game.Instance.Subscribe(-107300940, new Action<object>(this.OnResearchComplete));
		base.onSelect += this.OnClickBuilding;
		Game.Instance.Subscribe(-1190690038, new Action<object>(this.OnBuildToolDeactivated));
	}

	// Token: 0x06008E92 RID: 36498 RVA: 0x00370D44 File Offset: 0x0036EF44
	public void Configure(HashedString category, IList<BuildMenu.BuildingInfo> building_infos)
	{
		this.ClearButtons();
		this.SetHasFocus(true);
		List<KIconToggleMenu.ToggleInfo> list = new List<KIconToggleMenu.ToggleInfo>();
		string text = HashCache.Get().Get(category).ToUpper();
		text = text.Replace(" ", "");
		this.titleLabel.text = Strings.Get("STRINGS.UI.NEWBUILDCATEGORIES." + text + ".BUILDMENUTITLE");
		foreach (BuildMenu.BuildingInfo buildingInfo in building_infos)
		{
			BuildingDef def = Assets.GetBuildingDef(buildingInfo.id);
			if (def.ShouldShowInBuildMenu() && def.IsAvailable())
			{
				KIconToggleMenu.ToggleInfo item = new KIconToggleMenu.ToggleInfo(def.Name, new BuildMenuBuildingsScreen.UserData(def, PlanScreen.RequirementsState.Tech), def.HotKey, () => def.GetUISprite("ui", false));
				list.Add(item);
			}
		}
		base.Setup(list);
		for (int i = 0; i < this.toggleInfo.Count; i++)
		{
			this.RefreshToggle(this.toggleInfo[i]);
		}
		int num = 0;
		using (IEnumerator enumerator2 = this.gridSizer.transform.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				if (((Transform)enumerator2.Current).gameObject.activeSelf)
				{
					num++;
				}
			}
		}
		this.gridSizer.constraintCount = Mathf.Min(num, 3);
		int num2 = Mathf.Min(num, this.gridSizer.constraintCount);
		int num3 = (num + this.gridSizer.constraintCount - 1) / this.gridSizer.constraintCount;
		int num4 = num2 - 1;
		int num5 = num3 - 1;
		Vector2 vector = new Vector2((float)num2 * this.gridSizer.cellSize.x + (float)num4 * this.gridSizer.spacing.x + (float)this.gridSizer.padding.left + (float)this.gridSizer.padding.right, (float)num3 * this.gridSizer.cellSize.y + (float)num5 * this.gridSizer.spacing.y + (float)this.gridSizer.padding.top + (float)this.gridSizer.padding.bottom);
		this.contentSizeLayout.minWidth = vector.x;
		this.contentSizeLayout.minHeight = vector.y;
	}

	// Token: 0x06008E93 RID: 36499 RVA: 0x000FD369 File Offset: 0x000FB569
	private void ConfigureToolTip(ToolTip tooltip, BuildingDef def)
	{
		tooltip.ClearMultiStringTooltip();
		tooltip.AddMultiStringTooltip(def.Name, this.buildingToolTipSettings.BuildButtonName);
		tooltip.AddMultiStringTooltip(def.Effect, this.buildingToolTipSettings.BuildButtonDescription);
	}

	// Token: 0x06008E94 RID: 36500 RVA: 0x00371004 File Offset: 0x0036F204
	public void CloseRecipe(bool playSound = false)
	{
		if (playSound)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
		}
		ToolMenu.Instance.ClearSelection();
		this.DeactivateBuildTools();
		if (PlayerController.Instance.ActiveTool == PrebuildTool.Instance)
		{
			SelectTool.Instance.Activate();
		}
		this.selectedBuilding = null;
		this.onBuildingSelected(this.selectedBuilding);
	}

	// Token: 0x06008E95 RID: 36501 RVA: 0x0037106C File Offset: 0x0036F26C
	private void RefreshToggle(KIconToggleMenu.ToggleInfo info)
	{
		if (info == null || info.toggle == null)
		{
			return;
		}
		BuildingDef def = (info.userData as BuildMenuBuildingsScreen.UserData).def;
		TechItem techItem = Db.Get().TechItems.TryGet(def.PrefabID);
		bool flag = DebugHandler.InstantBuildMode || techItem == null || techItem.IsComplete();
		bool flag2 = flag || techItem == null || techItem.ParentTech.ArePrerequisitesComplete();
		KToggle toggle = info.toggle;
		if (toggle.gameObject.activeSelf != flag2)
		{
			toggle.gameObject.SetActive(flag2);
		}
		if (toggle.bgImage == null)
		{
			return;
		}
		Image image = toggle.bgImage.GetComponentsInChildren<Image>()[1];
		Sprite uisprite = def.GetUISprite("ui", false);
		image.sprite = uisprite;
		image.SetNativeSize();
		image.rectTransform().sizeDelta /= 4f;
		ToolTip component = toggle.gameObject.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		string text = def.Name;
		string effect = def.Effect;
		if (def.HotKey != global::Action.NumActions)
		{
			text += GameUtil.GetHotkeyString(def.HotKey);
		}
		component.AddMultiStringTooltip(text, this.buildingToolTipSettings.BuildButtonName);
		component.AddMultiStringTooltip(effect, this.buildingToolTipSettings.BuildButtonDescription);
		LocText componentInChildren = toggle.GetComponentInChildren<LocText>();
		if (componentInChildren != null)
		{
			componentInChildren.text = def.Name;
		}
		PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
		int num = (requirementsState == PlanScreen.RequirementsState.Complete) ? 1 : 0;
		ImageToggleState.State state;
		if (def == this.selectedBuilding && (requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode))
		{
			state = ImageToggleState.State.Active;
		}
		else
		{
			state = ((requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode) ? ImageToggleState.State.Inactive : ImageToggleState.State.Disabled);
		}
		if (def == this.selectedBuilding && state == ImageToggleState.State.Disabled)
		{
			state = ImageToggleState.State.DisabledActive;
		}
		else if (state == ImageToggleState.State.Disabled)
		{
			state = ImageToggleState.State.Disabled;
		}
		toggle.GetComponent<ImageToggleState>().SetState(state);
		Material material;
		Color color;
		if (requirementsState == PlanScreen.RequirementsState.Complete || DebugHandler.InstantBuildMode)
		{
			material = this.defaultUIMaterial;
			color = Color.white;
		}
		else
		{
			material = this.desaturatedUIMaterial;
			Color color3;
			if (!flag)
			{
				Graphic graphic = image;
				Color color2 = new Color(1f, 1f, 1f, 0.15f);
				graphic.color = color2;
				color3 = color2;
			}
			else
			{
				color3 = new Color(1f, 1f, 1f, 0.6f);
			}
			color = color3;
		}
		if (image.material != material)
		{
			image.material = material;
			image.color = color;
		}
		Image fgImage = toggle.gameObject.GetComponent<KToggle>().fgImage;
		fgImage.gameObject.SetActive(false);
		if (!flag)
		{
			fgImage.sprite = this.Overlay_NeedTech;
			fgImage.gameObject.SetActive(true);
			string newString = string.Format(UI.PRODUCTINFO_REQUIRESRESEARCHDESC, techItem.ParentTech.Name);
			component.AddMultiStringTooltip("\n", this.buildingToolTipSettings.ResearchRequirement);
			component.AddMultiStringTooltip(newString, this.buildingToolTipSettings.ResearchRequirement);
			return;
		}
		if (requirementsState != PlanScreen.RequirementsState.Complete)
		{
			fgImage.gameObject.SetActive(false);
			component.AddMultiStringTooltip("\n", this.buildingToolTipSettings.ResearchRequirement);
			string newString2 = UI.PRODUCTINFO_MISSINGRESOURCES_HOVER;
			component.AddMultiStringTooltip(newString2, this.buildingToolTipSettings.ResearchRequirement);
			foreach (Recipe.Ingredient ingredient in def.CraftRecipe.Ingredients)
			{
				string newString3 = string.Format("{0}{1}: {2}", "• ", ingredient.tag.ProperName(), GameUtil.GetFormattedMass(ingredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				component.AddMultiStringTooltip(newString3, this.buildingToolTipSettings.ResearchRequirement);
			}
			component.AddMultiStringTooltip("", this.buildingToolTipSettings.ResearchRequirement);
		}
	}

	// Token: 0x06008E96 RID: 36502 RVA: 0x000FD39F File Offset: 0x000FB59F
	public void ClearUI()
	{
		this.Show(false);
		this.ClearButtons();
	}

	// Token: 0x06008E97 RID: 36503 RVA: 0x0037145C File Offset: 0x0036F65C
	private void ClearButtons()
	{
		foreach (KToggle ktoggle in this.toggles)
		{
			ktoggle.gameObject.SetActive(false);
			ktoggle.gameObject.transform.SetParent(null);
			UnityEngine.Object.DestroyImmediate(ktoggle.gameObject);
		}
		if (this.toggles != null)
		{
			this.toggles.Clear();
		}
		if (this.toggleInfo != null)
		{
			this.toggleInfo.Clear();
		}
	}

	// Token: 0x06008E98 RID: 36504 RVA: 0x003714F4 File Offset: 0x0036F6F4
	private void OnClickBuilding(KIconToggleMenu.ToggleInfo toggle_info)
	{
		BuildMenuBuildingsScreen.UserData userData = toggle_info.userData as BuildMenuBuildingsScreen.UserData;
		this.OnSelectBuilding(userData.def);
	}

	// Token: 0x06008E99 RID: 36505 RVA: 0x0037151C File Offset: 0x0036F71C
	private void OnSelectBuilding(BuildingDef def)
	{
		PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
		if (requirementsState - PlanScreen.RequirementsState.Materials <= 1)
		{
			if (def != this.selectedBuilding)
			{
				this.selectedBuilding = def;
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
			}
			else
			{
				this.selectedBuilding = null;
				this.ClearSelection();
				this.CloseRecipe(true);
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Deselect", false));
			}
		}
		else
		{
			this.selectedBuilding = null;
			this.ClearSelection();
			this.CloseRecipe(true);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
		}
		this.onBuildingSelected(this.selectedBuilding);
	}

	// Token: 0x06008E9A RID: 36506 RVA: 0x003715C0 File Offset: 0x0036F7C0
	public void UpdateBuildableStates()
	{
		if (this.toggleInfo == null || this.toggleInfo.Count <= 0)
		{
			return;
		}
		BuildingDef buildingDef = null;
		foreach (KIconToggleMenu.ToggleInfo toggleInfo in this.toggleInfo)
		{
			this.RefreshToggle(toggleInfo);
			BuildMenuBuildingsScreen.UserData userData = toggleInfo.userData as BuildMenuBuildingsScreen.UserData;
			BuildingDef def = userData.def;
			if (def.IsAvailable())
			{
				PlanScreen.RequirementsState requirementsState = BuildMenu.Instance.BuildableState(def);
				if (requirementsState != userData.requirementsState)
				{
					if (def == BuildMenu.Instance.SelectedBuildingDef)
					{
						buildingDef = def;
					}
					this.RefreshToggle(toggleInfo);
					userData.requirementsState = requirementsState;
				}
			}
		}
		if (buildingDef != null)
		{
			BuildMenu.Instance.RefreshProductInfoScreen(buildingDef);
		}
	}

	// Token: 0x06008E9B RID: 36507 RVA: 0x000FD3AE File Offset: 0x000FB5AE
	private void OnResearchComplete(object data)
	{
		this.UpdateBuildableStates();
	}

	// Token: 0x06008E9C RID: 36508 RVA: 0x00371694 File Offset: 0x0036F894
	private void DeactivateBuildTools()
	{
		InterfaceTool activeTool = PlayerController.Instance.ActiveTool;
		if (activeTool != null)
		{
			Type type = activeTool.GetType();
			if (type == typeof(BuildTool) || typeof(BaseUtilityBuildTool).IsAssignableFrom(type) || typeof(PrebuildTool).IsAssignableFrom(type))
			{
				activeTool.DeactivateTool(null);
			}
		}
	}

	// Token: 0x06008E9D RID: 36509 RVA: 0x003716FC File Offset: 0x0036F8FC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.mouseOver && base.ConsumeMouseScroll && !e.TryConsume(global::Action.ZoomIn))
		{
			e.TryConsume(global::Action.ZoomOut);
		}
		if (!this.HasFocus)
		{
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			Game.Instance.Trigger(288942073, null);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			return;
		}
		base.OnKeyDown(e);
		if (!e.Consumed)
		{
			global::Action action = e.GetAction();
			if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
			{
				e.TryConsume(action);
			}
		}
	}

	// Token: 0x06008E9E RID: 36510 RVA: 0x00371780 File Offset: 0x0036F980
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!this.HasFocus)
		{
			return;
		}
		if (this.selectedBuilding != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
			Game.Instance.Trigger(288942073, null);
			return;
		}
		base.OnKeyUp(e);
		if (!e.Consumed)
		{
			global::Action action = e.GetAction();
			if (action >= global::Action.BUILD_MENU_START_INTERCEPT)
			{
				e.TryConsume(action);
			}
		}
	}

	// Token: 0x06008E9F RID: 36511 RVA: 0x003717F8 File Offset: 0x0036F9F8
	public override void Close()
	{
		ToolMenu.Instance.ClearSelection();
		this.DeactivateBuildTools();
		if (PlayerController.Instance.ActiveTool == PrebuildTool.Instance)
		{
			SelectTool.Instance.Activate();
		}
		this.selectedBuilding = null;
		this.ClearButtons();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06008EA0 RID: 36512 RVA: 0x000FD3B6 File Offset: 0x000FB5B6
	public override void SetHasFocus(bool has_focus)
	{
		base.SetHasFocus(has_focus);
		if (this.focusIndicator != null)
		{
			this.focusIndicator.color = (has_focus ? this.focusedColour : this.unfocusedColour);
		}
	}

	// Token: 0x06008EA1 RID: 36513 RVA: 0x000FD3EE File Offset: 0x000FB5EE
	private void OnBuildToolDeactivated(object data)
	{
		this.CloseRecipe(false);
	}

	// Token: 0x04006B6F RID: 27503
	[SerializeField]
	private Image focusIndicator;

	// Token: 0x04006B70 RID: 27504
	[SerializeField]
	private Color32 focusedColour;

	// Token: 0x04006B71 RID: 27505
	[SerializeField]
	private Color32 unfocusedColour;

	// Token: 0x04006B72 RID: 27506
	public Action<BuildingDef> onBuildingSelected;

	// Token: 0x04006B73 RID: 27507
	[SerializeField]
	private LocText titleLabel;

	// Token: 0x04006B74 RID: 27508
	[SerializeField]
	private BuildMenuBuildingsScreen.BuildingToolTipSettings buildingToolTipSettings;

	// Token: 0x04006B75 RID: 27509
	[SerializeField]
	private LayoutElement contentSizeLayout;

	// Token: 0x04006B76 RID: 27510
	[SerializeField]
	private GridLayoutGroup gridSizer;

	// Token: 0x04006B77 RID: 27511
	[SerializeField]
	private Sprite Overlay_NeedTech;

	// Token: 0x04006B78 RID: 27512
	[SerializeField]
	private Material defaultUIMaterial;

	// Token: 0x04006B79 RID: 27513
	[SerializeField]
	private Material desaturatedUIMaterial;

	// Token: 0x04006B7A RID: 27514
	private BuildingDef selectedBuilding;

	// Token: 0x02001AA2 RID: 6818
	[Serializable]
	public struct BuildingToolTipSettings
	{
		// Token: 0x04006B7B RID: 27515
		public TextStyleSetting BuildButtonName;

		// Token: 0x04006B7C RID: 27516
		public TextStyleSetting BuildButtonDescription;

		// Token: 0x04006B7D RID: 27517
		public TextStyleSetting MaterialRequirement;

		// Token: 0x04006B7E RID: 27518
		public TextStyleSetting ResearchRequirement;
	}

	// Token: 0x02001AA3 RID: 6819
	[Serializable]
	public struct BuildingNameTextSetting
	{
		// Token: 0x04006B7F RID: 27519
		public TextStyleSetting ActiveSelected;

		// Token: 0x04006B80 RID: 27520
		public TextStyleSetting ActiveDeselected;

		// Token: 0x04006B81 RID: 27521
		public TextStyleSetting InactiveSelected;

		// Token: 0x04006B82 RID: 27522
		public TextStyleSetting InactiveDeselected;
	}

	// Token: 0x02001AA4 RID: 6820
	private class UserData
	{
		// Token: 0x06008EA3 RID: 36515 RVA: 0x000FD3FF File Offset: 0x000FB5FF
		public UserData(BuildingDef def, PlanScreen.RequirementsState state)
		{
			this.def = def;
			this.requirementsState = state;
		}

		// Token: 0x04006B83 RID: 27523
		public BuildingDef def;

		// Token: 0x04006B84 RID: 27524
		public PlanScreen.RequirementsState requirementsState;
	}
}
