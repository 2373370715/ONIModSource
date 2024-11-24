using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FA4 RID: 8100
public class PlanterSideScreen : ReceptacleSideScreen
{
	// Token: 0x17000AF2 RID: 2802
	// (get) Token: 0x0600AB1E RID: 43806 RVA: 0x00408B5C File Offset: 0x00406D5C
	// (set) Token: 0x0600AB1F RID: 43807 RVA: 0x00408BB4 File Offset: 0x00406DB4
	private Tag selectedSubspecies
	{
		get
		{
			if (!this.entityPreviousSubSelectionMap.ContainsKey((PlantablePlot)this.targetReceptacle))
			{
				this.entityPreviousSubSelectionMap.Add((PlantablePlot)this.targetReceptacle, Tag.Invalid);
			}
			return this.entityPreviousSubSelectionMap[(PlantablePlot)this.targetReceptacle];
		}
		set
		{
			if (!this.entityPreviousSubSelectionMap.ContainsKey((PlantablePlot)this.targetReceptacle))
			{
				this.entityPreviousSubSelectionMap.Add((PlantablePlot)this.targetReceptacle, Tag.Invalid);
			}
			this.entityPreviousSubSelectionMap[(PlantablePlot)this.targetReceptacle] = value;
			this.selectedDepositObjectAdditionalTag = value;
		}
	}

	// Token: 0x0600AB20 RID: 43808 RVA: 0x00408C14 File Offset: 0x00406E14
	private void LoadTargetSubSpeciesRequest()
	{
		PlantablePlot plantablePlot = (PlantablePlot)this.targetReceptacle;
		Tag tag = Tag.Invalid;
		if (plantablePlot.requestedEntityTag != Tag.Invalid && plantablePlot.requestedEntityTag != GameTags.Empty)
		{
			tag = plantablePlot.requestedEntityTag;
		}
		else if (this.selectedEntityToggle != null)
		{
			tag = this.depositObjectMap[this.selectedEntityToggle].tag;
		}
		if (DlcManager.FeaturePlantMutationsEnabled() && tag.IsValid)
		{
			MutantPlant component = Assets.GetPrefab(tag).GetComponent<MutantPlant>();
			if (component == null)
			{
				this.selectedSubspecies = Tag.Invalid;
				return;
			}
			if (plantablePlot.requestedEntityAdditionalFilterTag != Tag.Invalid && plantablePlot.requestedEntityAdditionalFilterTag != GameTags.Empty)
			{
				this.selectedSubspecies = plantablePlot.requestedEntityAdditionalFilterTag;
				return;
			}
			if (this.selectedSubspecies == Tag.Invalid)
			{
				PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo;
				if (PlantSubSpeciesCatalog.Instance.GetOriginalSubSpecies(component.SpeciesID, out subSpeciesInfo))
				{
					this.selectedSubspecies = subSpeciesInfo.ID;
				}
				plantablePlot.requestedEntityAdditionalFilterTag = this.selectedSubspecies;
			}
		}
	}

	// Token: 0x0600AB21 RID: 43809 RVA: 0x0010F2AB File Offset: 0x0010D4AB
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<PlantablePlot>() != null;
	}

	// Token: 0x0600AB22 RID: 43810 RVA: 0x0010F2B9 File Offset: 0x0010D4B9
	protected override void ToggleClicked(ReceptacleToggle toggle)
	{
		this.LoadTargetSubSpeciesRequest();
		base.ToggleClicked(toggle);
		this.UpdateState(null);
	}

	// Token: 0x0600AB23 RID: 43811 RVA: 0x0010F2CF File Offset: 0x0010D4CF
	protected void MutationToggleClicked(GameObject toggle)
	{
		this.selectedSubspecies = this.subspeciesToggles[toggle];
		this.UpdateState(null);
	}

	// Token: 0x0600AB24 RID: 43812 RVA: 0x0010F2EA File Offset: 0x0010D4EA
	protected override void UpdateState(object data)
	{
		base.UpdateState(data);
		this.requestSelectedEntityBtn.onClick += this.RefreshSubspeciesToggles;
		this.RefreshSubspeciesToggles();
	}

	// Token: 0x0600AB25 RID: 43813 RVA: 0x0010F310 File Offset: 0x0010D510
	private IEnumerator ExpandMutations()
	{
		LayoutElement le = this.mutationViewport.GetComponent<LayoutElement>();
		float num = 94f;
		float travelPerSecond = num / 0.33f;
		while (le.minHeight < 118f)
		{
			float num2 = le.minHeight;
			float num3 = Time.unscaledDeltaTime * travelPerSecond;
			num2 = Mathf.Min(num2 + num3, 118f);
			le.minHeight = num2;
			le.preferredHeight = num2;
			yield return new WaitForEndOfFrame();
		}
		this.mutationPanelCollapsed = false;
		this.activeAnimationRoutine = null;
		yield return 0;
		yield break;
	}

	// Token: 0x0600AB26 RID: 43814 RVA: 0x0010F31F File Offset: 0x0010D51F
	private IEnumerator CollapseMutations()
	{
		LayoutElement le = this.mutationViewport.GetComponent<LayoutElement>();
		float num = -94f;
		float travelPerSecond = num / 0.33f;
		while (le.minHeight > 24f)
		{
			float num2 = le.minHeight;
			float num3 = Time.unscaledDeltaTime * travelPerSecond;
			num2 = Mathf.Max(num2 + num3, 24f);
			le.minHeight = num2;
			le.preferredHeight = num2;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.mutationPanelCollapsed = true;
		this.activeAnimationRoutine = null;
		yield return SequenceUtil.WaitForNextFrame;
		yield break;
	}

	// Token: 0x0600AB27 RID: 43815 RVA: 0x00408D2C File Offset: 0x00406F2C
	private void RefreshSubspeciesToggles()
	{
		foreach (KeyValuePair<GameObject, Tag> keyValuePair in this.subspeciesToggles)
		{
			UnityEngine.Object.Destroy(keyValuePair.Key);
		}
		this.subspeciesToggles.Clear();
		if (!PlantSubSpeciesCatalog.Instance.AnyNonOriginalDiscovered)
		{
			this.mutationPanel.SetActive(false);
			return;
		}
		this.mutationPanel.SetActive(true);
		foreach (GameObject obj in this.blankMutationObjects)
		{
			UnityEngine.Object.Destroy(obj);
		}
		this.blankMutationObjects.Clear();
		this.selectSpeciesPrompt.SetActive(false);
		if (this.selectedDepositObjectTag.IsValid)
		{
			Tag plantID = Assets.GetPrefab(this.selectedDepositObjectTag).GetComponent<PlantableSeed>().PlantID;
			List<PlantSubSpeciesCatalog.SubSpeciesInfo> allSubSpeciesForSpecies = PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(plantID);
			if (allSubSpeciesForSpecies != null)
			{
				foreach (PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo in allSubSpeciesForSpecies)
				{
					GameObject option = Util.KInstantiateUI(this.mutationOption, this.mutationContainer, true);
					option.GetComponentInChildren<LocText>().text = subSpeciesInfo.GetNameWithMutations(plantID.ProperName(), PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(subSpeciesInfo.ID), false);
					MultiToggle component = option.GetComponent<MultiToggle>();
					component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
					{
						this.MutationToggleClicked(option);
					}));
					option.GetComponent<ToolTip>().SetSimpleTooltip(subSpeciesInfo.GetMutationsTooltip());
					this.subspeciesToggles.Add(option, subSpeciesInfo.ID);
				}
				for (int i = allSubSpeciesForSpecies.Count; i < 5; i++)
				{
					this.blankMutationObjects.Add(Util.KInstantiateUI(this.blankMutationOption, this.mutationContainer, true));
				}
				if (!this.selectedSubspecies.IsValid || !this.subspeciesToggles.ContainsValue(this.selectedSubspecies))
				{
					this.selectedSubspecies = allSubSpeciesForSpecies[0].ID;
				}
			}
		}
		else
		{
			this.selectSpeciesPrompt.SetActive(true);
		}
		ICollection<Pickupable> collection = new List<Pickupable>();
		bool flag = base.CheckReceptacleOccupied();
		bool flag2 = this.targetReceptacle.GetActiveRequest != null;
		WorldContainer myWorld = this.targetReceptacle.GetMyWorld();
		collection = myWorld.worldInventory.GetPickupables(this.selectedDepositObjectTag, myWorld.IsModuleInterior);
		foreach (KeyValuePair<GameObject, Tag> keyValuePair2 in this.subspeciesToggles)
		{
			float num = 0f;
			bool flag3 = PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(keyValuePair2.Value);
			if (collection != null)
			{
				foreach (Pickupable pickupable in collection)
				{
					if (pickupable.HasTag(keyValuePair2.Value))
					{
						num += pickupable.GetComponent<PrimaryElement>().Units;
					}
				}
			}
			if (num > 0f && flag3)
			{
				keyValuePair2.Key.GetComponent<MultiToggle>().ChangeState((keyValuePair2.Value == this.selectedSubspecies) ? 1 : 0);
			}
			else
			{
				keyValuePair2.Key.GetComponent<MultiToggle>().ChangeState((keyValuePair2.Value == this.selectedSubspecies) ? 3 : 2);
			}
			LocText componentInChildren = keyValuePair2.Key.GetComponentInChildren<LocText>();
			componentInChildren.text += string.Format(" ({0})", num);
			if (flag || flag2)
			{
				if (keyValuePair2.Value == this.selectedSubspecies)
				{
					keyValuePair2.Key.SetActive(true);
					keyValuePair2.Key.GetComponent<MultiToggle>().ChangeState(1);
				}
				else
				{
					keyValuePair2.Key.SetActive(false);
				}
			}
			else
			{
				keyValuePair2.Key.SetActive(this.selectedEntityToggle != null);
			}
		}
		bool flag4 = !flag && !flag2 && this.selectedEntityToggle != null && this.subspeciesToggles.Count >= 1;
		if (flag4 && this.mutationPanelCollapsed)
		{
			if (this.activeAnimationRoutine != null)
			{
				base.StopCoroutine(this.activeAnimationRoutine);
			}
			this.activeAnimationRoutine = base.StartCoroutine(this.ExpandMutations());
			return;
		}
		if (!flag4 && !this.mutationPanelCollapsed)
		{
			if (this.activeAnimationRoutine != null)
			{
				base.StopCoroutine(this.activeAnimationRoutine);
			}
			this.activeAnimationRoutine = base.StartCoroutine(this.CollapseMutations());
		}
	}

	// Token: 0x0600AB28 RID: 43816 RVA: 0x0040926C File Offset: 0x0040746C
	protected override Sprite GetEntityIcon(Tag prefabTag)
	{
		PlantableSeed component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
		if (component != null)
		{
			return base.GetEntityIcon(new Tag(component.PlantID));
		}
		return base.GetEntityIcon(prefabTag);
	}

	// Token: 0x0600AB29 RID: 43817 RVA: 0x004092A8 File Offset: 0x004074A8
	protected override string GetEntityName(Tag prefabTag)
	{
		PlantableSeed component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
		if (component != null)
		{
			return Assets.GetPrefab(component.PlantID).GetProperName();
		}
		return base.GetEntityName(prefabTag);
	}

	// Token: 0x0600AB2A RID: 43818 RVA: 0x004092E4 File Offset: 0x004074E4
	protected override string GetEntityTooltip(Tag prefabTag)
	{
		PlantableSeed component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
		return string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANT_TOGGLE_TOOLTIP, this.GetEntityName(prefabTag), component.domesticatedDescription, base.GetAvailableAmount(prefabTag));
	}

	// Token: 0x0600AB2B RID: 43819 RVA: 0x00409328 File Offset: 0x00407528
	protected override void SetResultDescriptions(GameObject seed_or_plant)
	{
		string text = "";
		GameObject gameObject = seed_or_plant;
		PlantableSeed component = seed_or_plant.GetComponent<PlantableSeed>();
		List<Descriptor> list = new List<Descriptor>();
		bool flag = true;
		if (seed_or_plant.GetComponent<MutantPlant>() != null && this.selectedDepositObjectAdditionalTag != Tag.Invalid)
		{
			flag = PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.selectedDepositObjectAdditionalTag);
		}
		if (!flag)
		{
			text = CREATURES.PLANT_MUTATIONS.UNIDENTIFIED + "\n\n" + CREATURES.PLANT_MUTATIONS.UNIDENTIFIED_DESC;
		}
		else if (component != null)
		{
			list = component.GetDescriptors(component.gameObject);
			if (this.targetReceptacle.rotatable != null && this.targetReceptacle.Direction != component.direction)
			{
				if (component.direction == SingleEntityReceptacle.ReceptacleDirection.Top)
				{
					text += UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_FLOOR;
				}
				else if (component.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
				{
					text += UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_WALL;
				}
				else if (component.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
				{
					text += UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_CEILING;
				}
				text += "\n\n";
			}
			gameObject = Assets.GetPrefab(component.PlantID);
			MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
			if (component2 != null && this.selectedDepositObjectAdditionalTag.IsValid)
			{
				PlantSubSpeciesCatalog.SubSpeciesInfo subSpecies = PlantSubSpeciesCatalog.Instance.GetSubSpecies(component.PlantID, this.selectedDepositObjectAdditionalTag);
				component2.DummySetSubspecies(subSpecies.mutationIDs);
			}
			if (!string.IsNullOrEmpty(component.domesticatedDescription))
			{
				text += component.domesticatedDescription;
			}
		}
		else
		{
			InfoDescription component3 = gameObject.GetComponent<InfoDescription>();
			if (component3)
			{
				text += component3.description;
			}
		}
		this.descriptionLabel.SetText(text);
		List<Descriptor> plantLifeCycleDescriptors = GameUtil.GetPlantLifeCycleDescriptors(gameObject);
		if (plantLifeCycleDescriptors.Count > 0 && flag)
		{
			this.HarvestDescriptorPanel.SetDescriptors(plantLifeCycleDescriptors);
			this.HarvestDescriptorPanel.gameObject.SetActive(true);
		}
		else
		{
			this.HarvestDescriptorPanel.gameObject.SetActive(false);
		}
		List<Descriptor> plantRequirementDescriptors = GameUtil.GetPlantRequirementDescriptors(gameObject);
		if (list.Count > 0)
		{
			GameUtil.IndentListOfDescriptors(list, 1);
			plantRequirementDescriptors.InsertRange(plantRequirementDescriptors.Count, list);
		}
		if (plantRequirementDescriptors.Count > 0 && flag)
		{
			this.RequirementsDescriptorPanel.SetDescriptors(plantRequirementDescriptors);
			this.RequirementsDescriptorPanel.gameObject.SetActive(true);
		}
		else
		{
			this.RequirementsDescriptorPanel.gameObject.SetActive(false);
		}
		List<Descriptor> plantEffectDescriptors = GameUtil.GetPlantEffectDescriptors(gameObject);
		if (plantEffectDescriptors.Count > 0 && flag)
		{
			this.EffectsDescriptorPanel.SetDescriptors(plantEffectDescriptors);
			this.EffectsDescriptorPanel.gameObject.SetActive(true);
			return;
		}
		this.EffectsDescriptorPanel.gameObject.SetActive(false);
	}

	// Token: 0x0600AB2C RID: 43820 RVA: 0x004095D0 File Offset: 0x004077D0
	protected override bool AdditionalCanDepositTest()
	{
		bool flag = false;
		if (this.selectedDepositObjectTag.IsValid)
		{
			if (DlcManager.FeaturePlantMutationsEnabled())
			{
				flag = PlantSubSpeciesCatalog.Instance.IsValidPlantableSeed(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
			}
			else
			{
				flag = this.selectedDepositObjectTag.IsValid;
			}
		}
		PlantablePlot plantablePlot = this.targetReceptacle as PlantablePlot;
		WorldContainer myWorld = this.targetReceptacle.GetMyWorld();
		return flag && plantablePlot.ValidPlant && myWorld.worldInventory.GetCountWithAdditionalTag(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag, myWorld.IsModuleInterior) > 0;
	}

	// Token: 0x0600AB2D RID: 43821 RVA: 0x0010F32E File Offset: 0x0010D52E
	public override void SetTarget(GameObject target)
	{
		this.selectedDepositObjectTag = Tag.Invalid;
		this.selectedDepositObjectAdditionalTag = Tag.Invalid;
		base.SetTarget(target);
		this.LoadTargetSubSpeciesRequest();
		this.RefreshSubspeciesToggles();
	}

	// Token: 0x0600AB2E RID: 43822 RVA: 0x00409660 File Offset: 0x00407860
	protected override void RestoreSelectionFromOccupant()
	{
		base.RestoreSelectionFromOccupant();
		PlantablePlot plantablePlot = (PlantablePlot)this.targetReceptacle;
		Tag tag = Tag.Invalid;
		Tag tag2 = Tag.Invalid;
		bool flag = false;
		if (plantablePlot.Occupant != null)
		{
			tag = plantablePlot.Occupant.GetComponent<SeedProducer>().seedInfo.seedId;
			MutantPlant component = plantablePlot.Occupant.GetComponent<MutantPlant>();
			if (component != null)
			{
				tag2 = component.SubSpeciesID;
			}
		}
		else if (plantablePlot.GetActiveRequest != null)
		{
			tag = plantablePlot.requestedEntityTag;
			tag2 = plantablePlot.requestedEntityAdditionalFilterTag;
			this.selectedDepositObjectTag = tag;
			this.selectedDepositObjectAdditionalTag = tag2;
			flag = true;
		}
		if (tag != Tag.Invalid)
		{
			if (!this.entityPreviousSelectionMap.ContainsKey(plantablePlot) || flag)
			{
				int value = 0;
				foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
				{
					if (keyValuePair.Value.tag == tag)
					{
						value = this.entityToggles.IndexOf(keyValuePair.Key);
					}
				}
				if (!this.entityPreviousSelectionMap.ContainsKey(plantablePlot))
				{
					this.entityPreviousSelectionMap.Add(plantablePlot, -1);
				}
				this.entityPreviousSelectionMap[plantablePlot] = value;
			}
			if (!this.entityPreviousSubSelectionMap.ContainsKey(plantablePlot))
			{
				this.entityPreviousSubSelectionMap.Add(plantablePlot, Tag.Invalid);
			}
			if (this.entityPreviousSubSelectionMap[plantablePlot] == Tag.Invalid || flag)
			{
				this.entityPreviousSubSelectionMap[plantablePlot] = tag2;
			}
		}
	}

	// Token: 0x0400867E RID: 34430
	public DescriptorPanel RequirementsDescriptorPanel;

	// Token: 0x0400867F RID: 34431
	public DescriptorPanel HarvestDescriptorPanel;

	// Token: 0x04008680 RID: 34432
	public DescriptorPanel EffectsDescriptorPanel;

	// Token: 0x04008681 RID: 34433
	public GameObject mutationPanel;

	// Token: 0x04008682 RID: 34434
	public GameObject mutationViewport;

	// Token: 0x04008683 RID: 34435
	public GameObject mutationContainer;

	// Token: 0x04008684 RID: 34436
	public GameObject mutationOption;

	// Token: 0x04008685 RID: 34437
	public GameObject blankMutationOption;

	// Token: 0x04008686 RID: 34438
	public GameObject selectSpeciesPrompt;

	// Token: 0x04008687 RID: 34439
	private bool mutationPanelCollapsed = true;

	// Token: 0x04008688 RID: 34440
	public Dictionary<GameObject, Tag> subspeciesToggles = new Dictionary<GameObject, Tag>();

	// Token: 0x04008689 RID: 34441
	private List<GameObject> blankMutationObjects = new List<GameObject>();

	// Token: 0x0400868A RID: 34442
	private Dictionary<PlantablePlot, Tag> entityPreviousSubSelectionMap = new Dictionary<PlantablePlot, Tag>();

	// Token: 0x0400868B RID: 34443
	private Coroutine activeAnimationRoutine;

	// Token: 0x0400868C RID: 34444
	private const float EXPAND_DURATION = 0.33f;

	// Token: 0x0400868D RID: 34445
	private const float EXPAND_MIN = 24f;

	// Token: 0x0400868E RID: 34446
	private const float EXPAND_MAX = 118f;
}
