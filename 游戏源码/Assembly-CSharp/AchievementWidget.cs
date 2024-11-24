using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using FMOD.Studio;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

// Token: 0x02001BC1 RID: 7105
[AddComponentMenu("KMonoBehaviour/scripts/AchievementWidget")]
public class AchievementWidget : KMonoBehaviour
{
	// Token: 0x060093B1 RID: 37809 RVA: 0x00100461 File Offset: 0x000FE661
	protected override void OnSpawn()
	{
		base.OnSpawn();
		MultiToggle component = base.GetComponent<MultiToggle>();
		component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
		{
			this.ExpandAchievement();
		}));
	}

	// Token: 0x060093B2 RID: 37810 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void Update()
	{
	}

	// Token: 0x060093B3 RID: 37811 RVA: 0x00100490 File Offset: 0x000FE690
	private void ExpandAchievement()
	{
		if (SaveGame.Instance != null)
		{
			this.progressParent.gameObject.SetActive(!this.progressParent.gameObject.activeSelf);
		}
	}

	// Token: 0x060093B4 RID: 37812 RVA: 0x001004C2 File Offset: 0x000FE6C2
	public void ActivateNewlyAchievedFlourish(float delay = 1f)
	{
		base.StartCoroutine(this.Flourish(delay));
	}

	// Token: 0x060093B5 RID: 37813 RVA: 0x001004D2 File Offset: 0x000FE6D2
	private IEnumerator Flourish(float startDelay)
	{
		this.SetNeverAchieved();
		Canvas canvas = base.GetComponent<Canvas>();
		if (canvas == null)
		{
			canvas = base.gameObject.AddComponent<Canvas>();
		}
		yield return SequenceUtil.WaitForSecondsRealtime(startDelay);
		KScrollRect component = base.transform.parent.parent.GetComponent<KScrollRect>();
		float num = 1.1f;
		float smoothAutoScrollTarget = 1f + base.transform.localPosition.y * num / component.content.rect.height;
		component.SetSmoothAutoScrollTarget(smoothAutoScrollTarget);
		yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
		canvas.overrideSorting = true;
		canvas.sortingOrder = 30;
		GameObject icon = base.GetComponent<HierarchyReferences>().GetReference<Image>("icon").transform.parent.gameObject;
		foreach (KBatchedAnimController kbatchedAnimController in this.sparks)
		{
			if (kbatchedAnimController.transform.parent != icon.transform.parent)
			{
				kbatchedAnimController.GetComponent<KBatchedAnimController>().TintColour = new Color(1f, 0.86f, 0.56f, 1f);
				kbatchedAnimController.transform.SetParent(icon.transform.parent);
				kbatchedAnimController.transform.SetSiblingIndex(icon.transform.GetSiblingIndex());
				kbatchedAnimController.GetComponent<KBatchedAnimCanvasRenderer>().compare = CompareFunction.Always;
			}
		}
		HierarchyReferences component2 = base.GetComponent<HierarchyReferences>();
		component2.GetReference<Image>("iconBG").color = this.color_dark_red;
		component2.GetReference<Image>("iconBorder").color = this.color_gold;
		component2.GetReference<Image>("icon").color = this.color_gold;
		bool colorChanged = false;
		EventInstance instance = KFMOD.BeginOneShot(GlobalAssets.GetSound("AchievementUnlocked", false), Vector3.zero, 1f);
		int num2 = Mathf.RoundToInt(MathUtil.Clamp(1f, 7f, startDelay - startDelay % 1f / 1f)) - 1;
		instance.setParameterByName("num_achievements", (float)num2, false);
		KFMOD.EndOneShot(instance);
		for (float i = 0f; i < 1.2f; i += Time.unscaledDeltaTime)
		{
			icon.transform.localScale = Vector3.one * this.flourish_iconScaleCurve.Evaluate(i);
			this.sheenTransform.anchoredPosition = new Vector2(this.flourish_sheenPositionCurve.Evaluate(i), this.sheenTransform.anchoredPosition.y);
			if (i > 1f && !colorChanged)
			{
				colorChanged = true;
				KBatchedAnimController[] array = this.sparks;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].Play("spark", KAnim.PlayMode.Once, 1f, 0f);
				}
				this.SetAchievedNow();
			}
			yield return SequenceUtil.WaitForNextFrame;
		}
		icon.transform.localScale = Vector3.one;
		this.CompleteFlourish();
		for (float i = 0f; i < 0.6f; i += Time.unscaledDeltaTime)
		{
			yield return SequenceUtil.WaitForNextFrame;
		}
		base.transform.localScale = Vector3.one;
		yield break;
	}

	// Token: 0x060093B6 RID: 37814 RVA: 0x0038F9F8 File Offset: 0x0038DBF8
	public void CompleteFlourish()
	{
		Canvas canvas = base.GetComponent<Canvas>();
		if (canvas == null)
		{
			canvas = base.gameObject.AddComponent<Canvas>();
		}
		canvas.overrideSorting = false;
	}

	// Token: 0x060093B7 RID: 37815 RVA: 0x0038FA28 File Offset: 0x0038DC28
	public void SetAchievedNow()
	{
		base.GetComponent<MultiToggle>().ChangeState(1);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_red;
		component.GetReference<Image>("iconBorder").color = this.color_gold;
		component.GetReference<Image>("icon").color = this.color_gold;
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = Color.white;
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.ACHIEVED_THIS_COLONY_TOOLTIP);
	}

	// Token: 0x060093B8 RID: 37816 RVA: 0x0038FAC0 File Offset: 0x0038DCC0
	public void SetAchievedBefore()
	{
		base.GetComponent<MultiToggle>().ChangeState(1);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_red;
		component.GetReference<Image>("iconBorder").color = this.color_gold;
		component.GetReference<Image>("icon").color = this.color_gold;
		LocText[] componentsInChildren = base.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = Color.white;
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.ACHIEVED_OTHER_COLONY_TOOLTIP);
	}

	// Token: 0x060093B9 RID: 37817 RVA: 0x0038FB58 File Offset: 0x0038DD58
	public void SetNeverAchieved()
	{
		base.GetComponent<MultiToggle>().ChangeState(2);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_grey;
		component.GetReference<Image>("iconBorder").color = this.color_grey;
		component.GetReference<Image>("icon").color = this.color_grey;
		foreach (LocText locText in base.GetComponentsInChildren<LocText>())
		{
			locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.6f);
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.NOT_ACHIEVED_EVER);
	}

	// Token: 0x060093BA RID: 37818 RVA: 0x0038FC18 File Offset: 0x0038DE18
	public void SetNotAchieved()
	{
		base.GetComponent<MultiToggle>().ChangeState(2);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_grey;
		component.GetReference<Image>("iconBorder").color = this.color_grey;
		component.GetReference<Image>("icon").color = this.color_grey;
		foreach (LocText locText in base.GetComponentsInChildren<LocText>())
		{
			locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.6f);
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.NOT_ACHIEVED_THIS_COLONY);
	}

	// Token: 0x060093BB RID: 37819 RVA: 0x0038FCD8 File Offset: 0x0038DED8
	public void SetFailed()
	{
		base.GetComponent<MultiToggle>().ChangeState(2);
		HierarchyReferences component = base.GetComponent<HierarchyReferences>();
		component.GetReference<Image>("iconBG").color = this.color_dark_grey;
		component.GetReference<Image>("iconBG").SetAlpha(0.5f);
		component.GetReference<Image>("iconBorder").color = this.color_grey;
		component.GetReference<Image>("iconBorder").SetAlpha(0.5f);
		component.GetReference<Image>("icon").color = this.color_grey;
		component.GetReference<Image>("icon").SetAlpha(0.5f);
		foreach (LocText locText in base.GetComponentsInChildren<LocText>())
		{
			locText.color = new Color(locText.color.r, locText.color.g, locText.color.b, 0.25f);
		}
		this.ConfigureToolTip(base.GetComponent<ToolTip>(), COLONY_ACHIEVEMENTS.FAILED_THIS_COLONY);
	}

	// Token: 0x060093BC RID: 37820 RVA: 0x0038FDD8 File Offset: 0x0038DFD8
	private void ConfigureToolTip(ToolTip tooltip, string status)
	{
		tooltip.ClearMultiStringTooltip();
		tooltip.AddMultiStringTooltip(status, null);
		if (SaveGame.Instance != null && !this.progressParent.gameObject.activeSelf)
		{
			tooltip.AddMultiStringTooltip(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXPAND_TOOLTIP, null);
		}
		if (DlcManager.IsDlcId(this.dlcIdFrom))
		{
			tooltip.AddMultiStringTooltip(string.Format(COLONY_ACHIEVEMENTS.DLC_ACHIEVEMENT, DlcManager.GetDlcTitle(this.dlcIdFrom)), null);
		}
	}

	// Token: 0x060093BD RID: 37821 RVA: 0x0038FE54 File Offset: 0x0038E054
	public void ShowProgress(ColonyAchievementStatus achievement)
	{
		if (this.progressParent == null)
		{
			return;
		}
		this.numRequirementsDisplayed = 0;
		for (int i = 0; i < achievement.Requirements.Count; i++)
		{
			ColonyAchievementRequirement colonyAchievementRequirement = achievement.Requirements[i];
			if (colonyAchievementRequirement is CritterTypesWithTraits)
			{
				this.ShowCritterChecklist(colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is DupesCompleteChoreInExoSuitForCycles)
			{
				this.ShowDupesInExoSuitsRequirement(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is DupesVsSolidTransferArmFetch)
			{
				this.ShowArmsOutPeformingDupesRequirement(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is ProduceXEngeryWithoutUsingYList)
			{
				this.ShowEngeryWithoutUsing(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is MinimumMorale)
			{
				this.ShowMinimumMoraleRequirement(achievement.success, colonyAchievementRequirement);
			}
			else if (colonyAchievementRequirement is SurviveARocketWithMinimumMorale)
			{
				this.ShowRocketMoraleRequirement(achievement.success, colonyAchievementRequirement);
			}
			else
			{
				this.ShowRequirement(achievement.success, colonyAchievementRequirement);
			}
		}
	}

	// Token: 0x060093BE RID: 37822 RVA: 0x0038FF34 File Offset: 0x0038E134
	private HierarchyReferences GetNextRequirementWidget()
	{
		GameObject gameObject;
		if (this.progressParent.childCount <= this.numRequirementsDisplayed)
		{
			gameObject = global::Util.KInstantiateUI(this.requirementPrefab, this.progressParent.gameObject, true);
		}
		else
		{
			gameObject = this.progressParent.GetChild(this.numRequirementsDisplayed).gameObject;
			gameObject.SetActive(true);
		}
		this.numRequirementsDisplayed++;
		return gameObject.GetComponent<HierarchyReferences>();
	}

	// Token: 0x060093BF RID: 37823 RVA: 0x001004E8 File Offset: 0x000FE6E8
	private void SetDescription(string str, HierarchyReferences refs)
	{
		refs.GetReference<LocText>("Desc").SetText(str);
	}

	// Token: 0x060093C0 RID: 37824 RVA: 0x001004FB File Offset: 0x000FE6FB
	private void SetIcon(Sprite sprite, Color color, HierarchyReferences refs)
	{
		Image reference = refs.GetReference<Image>("Icon");
		reference.sprite = sprite;
		reference.color = color;
		reference.gameObject.SetActive(true);
	}

	// Token: 0x060093C1 RID: 37825 RVA: 0x00100521 File Offset: 0x000FE721
	private void ShowIcon(bool show, HierarchyReferences refs)
	{
		refs.GetReference<Image>("Icon").gameObject.SetActive(show);
	}

	// Token: 0x060093C2 RID: 37826 RVA: 0x0038FFA0 File Offset: 0x0038E1A0
	private void ShowRequirement(bool succeed, ColonyAchievementRequirement req)
	{
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		bool flag = req.Success() || succeed;
		bool flag2 = req.Fail();
		if (flag && !flag2)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		else if (flag2)
		{
			this.SetIcon(this.statusFailureIcon, Color.red, nextRequirementWidget);
		}
		else
		{
			this.ShowIcon(false, nextRequirementWidget);
		}
		this.SetDescription(req.GetProgress(flag), nextRequirementWidget);
	}

	// Token: 0x060093C3 RID: 37827 RVA: 0x0039000C File Offset: 0x0038E20C
	private void ShowCritterChecklist(ColonyAchievementRequirement req)
	{
		CritterTypesWithTraits critterTypesWithTraits = req as CritterTypesWithTraits;
		if (req == null)
		{
			return;
		}
		foreach (KeyValuePair<Tag, bool> keyValuePair in critterTypesWithTraits.critterTypesToCheck)
		{
			HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
			if (keyValuePair.Value)
			{
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			else
			{
				this.ShowIcon(false, nextRequirementWidget);
			}
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.TAME_A_CRITTER, keyValuePair.Key.Name.ProperName()), nextRequirementWidget);
		}
	}

	// Token: 0x060093C4 RID: 37828 RVA: 0x003900C0 File Offset: 0x0038E2C0
	private void ShowArmsOutPeformingDupesRequirement(bool succeed, ColonyAchievementRequirement req)
	{
		DupesVsSolidTransferArmFetch dupesVsSolidTransferArmFetch = req as DupesVsSolidTransferArmFetch;
		if (dupesVsSolidTransferArmFetch == null)
		{
			return;
		}
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		if (succeed)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ARM_PERFORMANCE, succeed ? dupesVsSolidTransferArmFetch.numCycles : dupesVsSolidTransferArmFetch.currentCycleCount, dupesVsSolidTransferArmFetch.numCycles), nextRequirementWidget);
		if (!succeed)
		{
			Dictionary<int, int> fetchDupeChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchDupeChoreDeliveries;
			Dictionary<int, int> fetchAutomatedChoreDeliveries = SaveGame.Instance.ColonyAchievementTracker.fetchAutomatedChoreDeliveries;
			int num = 0;
			fetchDupeChoreDeliveries.TryGetValue(GameClock.Instance.GetCycle(), out num);
			int num2 = 0;
			fetchAutomatedChoreDeliveries.TryGetValue(GameClock.Instance.GetCycle(), out num2);
			nextRequirementWidget = this.GetNextRequirementWidget();
			if ((float)num < (float)num2 * dupesVsSolidTransferArmFetch.percentage)
			{
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			else
			{
				this.ShowIcon(false, nextRequirementWidget);
			}
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ARM_VS_DUPE_FETCHES, "SolidTransferArm", num2, num), nextRequirementWidget);
		}
	}

	// Token: 0x060093C5 RID: 37829 RVA: 0x003901D4 File Offset: 0x0038E3D4
	private void ShowDupesInExoSuitsRequirement(bool succeed, ColonyAchievementRequirement req)
	{
		DupesCompleteChoreInExoSuitForCycles dupesCompleteChoreInExoSuitForCycles = req as DupesCompleteChoreInExoSuitForCycles;
		if (dupesCompleteChoreInExoSuitForCycles == null)
		{
			return;
		}
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		if (succeed)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		else
		{
			this.ShowIcon(false, nextRequirementWidget);
		}
		this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXOSUIT_CYCLES, succeed ? dupesCompleteChoreInExoSuitForCycles.numCycles : dupesCompleteChoreInExoSuitForCycles.currentCycleStreak, dupesCompleteChoreInExoSuitForCycles.numCycles), nextRequirementWidget);
		if (!succeed)
		{
			nextRequirementWidget = this.GetNextRequirementWidget();
			int num = dupesCompleteChoreInExoSuitForCycles.GetNumberOfDupesForCycle(GameClock.Instance.GetCycle());
			if (num >= Components.LiveMinionIdentities.Count)
			{
				num = Components.LiveMinionIdentities.Count;
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.EXOSUIT_THIS_CYCLE, num, Components.LiveMinionIdentities.Count), nextRequirementWidget);
		}
	}

	// Token: 0x060093C6 RID: 37830 RVA: 0x003902BC File Offset: 0x0038E4BC
	private void ShowEngeryWithoutUsing(bool succeed, ColonyAchievementRequirement req)
	{
		ProduceXEngeryWithoutUsingYList produceXEngeryWithoutUsingYList = req as ProduceXEngeryWithoutUsingYList;
		if (req == null)
		{
			return;
		}
		HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
		float productionAmount = produceXEngeryWithoutUsingYList.GetProductionAmount(succeed);
		this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.GENERATE_POWER, GameUtil.GetFormattedRoundedJoules(productionAmount), GameUtil.GetFormattedRoundedJoules(produceXEngeryWithoutUsingYList.amountToProduce * 1000f)), nextRequirementWidget);
		if (succeed)
		{
			this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
		}
		else
		{
			this.ShowIcon(false, nextRequirementWidget);
		}
		foreach (Tag key in produceXEngeryWithoutUsingYList.disallowedBuildings)
		{
			nextRequirementWidget = this.GetNextRequirementWidget();
			if (Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(key))
			{
				this.SetIcon(this.statusFailureIcon, Color.red, nextRequirementWidget);
			}
			else
			{
				this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
			}
			BuildingDef buildingDef = Assets.GetBuildingDef(key.Name);
			this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.NO_BUILDING, buildingDef.Name), nextRequirementWidget);
		}
	}

	// Token: 0x060093C7 RID: 37831 RVA: 0x003903E4 File Offset: 0x0038E5E4
	private void ShowMinimumMoraleRequirement(bool success, ColonyAchievementRequirement req)
	{
		MinimumMorale minimumMorale = req as MinimumMorale;
		if (minimumMorale == null)
		{
			return;
		}
		if (success)
		{
			this.ShowRequirement(success, req);
			return;
		}
		foreach (object obj in Components.MinionAssignablesProxy)
		{
			GameObject targetGameObject = ((MinionAssignablesProxy)obj).GetTargetGameObject();
			if (targetGameObject != null && !targetGameObject.HasTag(GameTags.Dead))
			{
				AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(targetGameObject.GetComponent<MinionModifiers>());
				if (attributeInstance != null)
				{
					HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
					if (attributeInstance.GetTotalValue() >= (float)minimumMorale.minimumMorale)
					{
						this.SetIcon(this.statusSuccessIcon, Color.green, nextRequirementWidget);
					}
					else
					{
						this.ShowIcon(false, nextRequirementWidget);
					}
					this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.MORALE, targetGameObject.GetProperName(), attributeInstance.GetTotalDisplayValue()), nextRequirementWidget);
				}
			}
		}
	}

	// Token: 0x060093C8 RID: 37832 RVA: 0x003904F0 File Offset: 0x0038E6F0
	private void ShowRocketMoraleRequirement(bool success, ColonyAchievementRequirement req)
	{
		SurviveARocketWithMinimumMorale surviveARocketWithMinimumMorale = req as SurviveARocketWithMinimumMorale;
		if (surviveARocketWithMinimumMorale == null)
		{
			return;
		}
		if (success)
		{
			this.ShowRequirement(success, req);
			return;
		}
		foreach (KeyValuePair<int, int> keyValuePair in SaveGame.Instance.ColonyAchievementTracker.cyclesRocketDupeMoraleAboveRequirement)
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(keyValuePair.Key);
			if (world != null)
			{
				HierarchyReferences nextRequirementWidget = this.GetNextRequirementWidget();
				this.SetDescription(string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SURVIVE_SPACE, new object[]
				{
					surviveARocketWithMinimumMorale.minimumMorale,
					keyValuePair.Value,
					surviveARocketWithMinimumMorale.numberOfCycles,
					world.GetProperName()
				}), nextRequirementWidget);
			}
		}
	}

	// Token: 0x040072A3 RID: 29347
	private Color color_dark_red = new Color(0.28235295f, 0.16078432f, 0.14901961f);

	// Token: 0x040072A4 RID: 29348
	private Color color_gold = new Color(1f, 0.63529414f, 0.28627452f);

	// Token: 0x040072A5 RID: 29349
	private Color color_dark_grey = new Color(0.21568628f, 0.21568628f, 0.21568628f);

	// Token: 0x040072A6 RID: 29350
	private Color color_grey = new Color(0.6901961f, 0.6901961f, 0.6901961f);

	// Token: 0x040072A7 RID: 29351
	[SerializeField]
	private RectTransform sheenTransform;

	// Token: 0x040072A8 RID: 29352
	public AnimationCurve flourish_iconScaleCurve;

	// Token: 0x040072A9 RID: 29353
	public AnimationCurve flourish_sheenPositionCurve;

	// Token: 0x040072AA RID: 29354
	public KBatchedAnimController[] sparks;

	// Token: 0x040072AB RID: 29355
	[SerializeField]
	private RectTransform progressParent;

	// Token: 0x040072AC RID: 29356
	[SerializeField]
	private GameObject requirementPrefab;

	// Token: 0x040072AD RID: 29357
	[SerializeField]
	private Sprite statusSuccessIcon;

	// Token: 0x040072AE RID: 29358
	[SerializeField]
	private Sprite statusFailureIcon;

	// Token: 0x040072AF RID: 29359
	private int numRequirementsDisplayed;

	// Token: 0x040072B0 RID: 29360
	public string dlcIdFrom;
}
