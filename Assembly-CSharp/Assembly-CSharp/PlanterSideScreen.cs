using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class PlanterSideScreen : ReceptacleSideScreen {
    private const    float            EXPAND_DURATION = 0.33f;
    private const    float            EXPAND_MIN      = 24f;
    private const    float            EXPAND_MAX      = 118f;
    private          Coroutine        activeAnimationRoutine;
    private readonly List<GameObject> blankMutationObjects = new List<GameObject>();
    public           GameObject       blankMutationOption;
    public           DescriptorPanel  EffectsDescriptorPanel;

    private readonly Dictionary<PlantablePlot, Tag>
        entityPreviousSubSelectionMap = new Dictionary<PlantablePlot, Tag>();

    public  DescriptorPanel             HarvestDescriptorPanel;
    public  GameObject                  mutationContainer;
    public  GameObject                  mutationOption;
    public  GameObject                  mutationPanel;
    private bool                        mutationPanelCollapsed = true;
    public  GameObject                  mutationViewport;
    public  DescriptorPanel             RequirementsDescriptorPanel;
    public  GameObject                  selectSpeciesPrompt;
    public  Dictionary<GameObject, Tag> subspeciesToggles = new Dictionary<GameObject, Tag>();

    private Tag selectedSubspecies {
        get {
            if (!entityPreviousSubSelectionMap.ContainsKey((PlantablePlot)targetReceptacle))
                entityPreviousSubSelectionMap.Add((PlantablePlot)targetReceptacle, Tag.Invalid);

            return entityPreviousSubSelectionMap[(PlantablePlot)targetReceptacle];
        }
        set {
            if (!entityPreviousSubSelectionMap.ContainsKey((PlantablePlot)targetReceptacle))
                entityPreviousSubSelectionMap.Add((PlantablePlot)targetReceptacle, Tag.Invalid);

            entityPreviousSubSelectionMap[(PlantablePlot)targetReceptacle] = value;
            selectedDepositObjectAdditionalTag                             = value;
        }
    }

    private void LoadTargetSubSpeciesRequest() {
        var plantablePlot = (PlantablePlot)targetReceptacle;
        var tag           = Tag.Invalid;
        if (plantablePlot.requestedEntityTag != Tag.Invalid && plantablePlot.requestedEntityTag != GameTags.Empty)
            tag                                    = plantablePlot.requestedEntityTag;
        else if (selectedEntityToggle != null) tag = depositObjectMap[selectedEntityToggle].tag;

        if (DlcManager.FeaturePlantMutationsEnabled() && tag.IsValid) {
            var component = Assets.GetPrefab(tag).GetComponent<MutantPlant>();
            if (component == null) {
                selectedSubspecies = Tag.Invalid;
                return;
            }

            if (plantablePlot.requestedEntityAdditionalFilterTag != Tag.Invalid &&
                plantablePlot.requestedEntityAdditionalFilterTag != GameTags.Empty) {
                selectedSubspecies = plantablePlot.requestedEntityAdditionalFilterTag;
                return;
            }

            if (selectedSubspecies == Tag.Invalid) {
                PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo;
                if (PlantSubSpeciesCatalog.Instance.GetOriginalSubSpecies(component.SpeciesID, out subSpeciesInfo))
                    selectedSubspecies = subSpeciesInfo.ID;

                plantablePlot.requestedEntityAdditionalFilterTag = selectedSubspecies;
            }
        }
    }

    public override bool IsValidForTarget(GameObject target) { return target.GetComponent<PlantablePlot>() != null; }

    protected override void ToggleClicked(ReceptacleToggle toggle) {
        LoadTargetSubSpeciesRequest();
        base.ToggleClicked(toggle);
        UpdateState(null);
    }

    protected void MutationToggleClicked(GameObject toggle) {
        selectedSubspecies = subspeciesToggles[toggle];
        UpdateState(null);
    }

    protected override void UpdateState(object data) {
        base.UpdateState(data);
        requestSelectedEntityBtn.onClick += RefreshSubspeciesToggles;
        RefreshSubspeciesToggles();
    }

    private IEnumerator ExpandMutations() {
        var le              = mutationViewport.GetComponent<LayoutElement>();
        var num             = 94f;
        var travelPerSecond = num / 0.33f;
        while (le.minHeight < 118f) {
            var num2 = le.minHeight;
            var num3 = Time.unscaledDeltaTime * travelPerSecond;
            num2               = Mathf.Min(num2 + num3, 118f);
            le.minHeight       = num2;
            le.preferredHeight = num2;
            yield return new WaitForEndOfFrame();
        }

        mutationPanelCollapsed = false;
        activeAnimationRoutine = null;
        yield return 0;
    }

    private IEnumerator CollapseMutations() {
        var le              = mutationViewport.GetComponent<LayoutElement>();
        var num             = -94f;
        var travelPerSecond = num / 0.33f;
        while (le.minHeight > 24f) {
            var num2 = le.minHeight;
            var num3 = Time.unscaledDeltaTime * travelPerSecond;
            num2               = Mathf.Max(num2 + num3, 24f);
            le.minHeight       = num2;
            le.preferredHeight = num2;
            yield return SequenceUtil.WaitForEndOfFrame;
        }

        mutationPanelCollapsed = true;
        activeAnimationRoutine = null;
        yield return SequenceUtil.WaitForNextFrame;
    }

    private void RefreshSubspeciesToggles() {
        foreach (var keyValuePair in subspeciesToggles) Destroy(keyValuePair.Key);
        subspeciesToggles.Clear();
        if (!PlantSubSpeciesCatalog.Instance.AnyNonOriginalDiscovered) {
            mutationPanel.SetActive(false);
            return;
        }

        mutationPanel.SetActive(true);
        foreach (var obj in blankMutationObjects) Destroy(obj);
        blankMutationObjects.Clear();
        selectSpeciesPrompt.SetActive(false);
        if (selectedDepositObjectTag.IsValid) {
            var plantID = Assets.GetPrefab(selectedDepositObjectTag).GetComponent<PlantableSeed>().PlantID;
            var allSubSpeciesForSpecies = PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(plantID);
            if (allSubSpeciesForSpecies != null) {
                foreach (var subSpeciesInfo in allSubSpeciesForSpecies) {
                    var option = Util.KInstantiateUI(mutationOption, mutationContainer, true);
                    option.GetComponentInChildren<LocText>().text
                        = subSpeciesInfo.GetNameWithMutations(plantID.ProperName(),
                                                              PlantSubSpeciesCatalog.Instance
                                                                  .IsSubSpeciesIdentified(subSpeciesInfo.ID),
                                                              false);

                    var component = option.GetComponent<MultiToggle>();
                    component.onClick = (System.Action)Delegate.Combine(component.onClick,
                                                                        new System.Action(delegate {
                                                                            MutationToggleClicked(option);
                                                                        }));

                    option.GetComponent<ToolTip>().SetSimpleTooltip(subSpeciesInfo.GetMutationsTooltip());
                    subspeciesToggles.Add(option, subSpeciesInfo.ID);
                }

                for (var i = allSubSpeciesForSpecies.Count; i < 5; i++)
                    blankMutationObjects.Add(Util.KInstantiateUI(blankMutationOption, mutationContainer, true));

                if (!selectedSubspecies.IsValid || !subspeciesToggles.ContainsValue(selectedSubspecies))
                    selectedSubspecies = allSubSpeciesForSpecies[0].ID;
            }
        } else
            selectSpeciesPrompt.SetActive(true);

        ICollection<Pickupable> collection = new List<Pickupable>();
        var                     flag       = CheckReceptacleOccupied();
        var                     flag2      = targetReceptacle.GetActiveRequest != null;
        var                     myWorld    = targetReceptacle.GetMyWorld();
        collection = myWorld.worldInventory.GetPickupables(selectedDepositObjectTag, myWorld.IsModuleInterior);
        foreach (var keyValuePair2 in subspeciesToggles) {
            var num   = 0f;
            var flag3 = PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(keyValuePair2.Value);
            if (collection != null)
                foreach (var pickupable in collection)
                    if (pickupable.HasTag(keyValuePair2.Value))
                        num += pickupable.GetComponent<PrimaryElement>().Units;

            if (num > 0f && flag3)
                keyValuePair2.Key.GetComponent<MultiToggle>()
                             .ChangeState(keyValuePair2.Value == selectedSubspecies ? 1 : 0);
            else
                keyValuePair2.Key.GetComponent<MultiToggle>()
                             .ChangeState(keyValuePair2.Value == selectedSubspecies ? 3 : 2);

            var componentInChildren = keyValuePair2.Key.GetComponentInChildren<LocText>();
            componentInChildren.text += string.Format(" ({0})", num);
            if (flag || flag2) {
                if (keyValuePair2.Value == selectedSubspecies) {
                    keyValuePair2.Key.SetActive(true);
                    keyValuePair2.Key.GetComponent<MultiToggle>().ChangeState(1);
                } else
                    keyValuePair2.Key.SetActive(false);
            } else
                keyValuePair2.Key.SetActive(selectedEntityToggle != null);
        }

        var flag4 = !flag && !flag2 && selectedEntityToggle != null && subspeciesToggles.Count >= 1;
        if (flag4 && mutationPanelCollapsed) {
            if (activeAnimationRoutine != null) StopCoroutine(activeAnimationRoutine);
            activeAnimationRoutine = StartCoroutine(ExpandMutations());
            return;
        }

        if (!flag4 && !mutationPanelCollapsed) {
            if (activeAnimationRoutine != null) StopCoroutine(activeAnimationRoutine);
            activeAnimationRoutine = StartCoroutine(CollapseMutations());
        }
    }

    protected override Sprite GetEntityIcon(Tag prefabTag) {
        var component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
        if (component != null) return base.GetEntityIcon(new Tag(component.PlantID));

        return base.GetEntityIcon(prefabTag);
    }

    protected override string GetEntityName(Tag prefabTag) {
        var component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
        if (component != null) return Assets.GetPrefab(component.PlantID).GetProperName();

        return base.GetEntityName(prefabTag);
    }

    protected override string GetEntityTooltip(Tag prefabTag) {
        var component = Assets.GetPrefab(prefabTag).GetComponent<PlantableSeed>();
        return string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.PLANT_TOGGLE_TOOLTIP,
                             GetEntityName(prefabTag),
                             component.domesticatedDescription,
                             GetAvailableAmount(prefabTag));
    }

    protected override void SetResultDescriptions(GameObject seed_or_plant) {
        var text       = "";
        var gameObject = seed_or_plant;
        var component  = seed_or_plant.GetComponent<PlantableSeed>();
        var list       = new List<Descriptor>();
        var flag       = true;
        if (seed_or_plant.GetComponent<MutantPlant>() != null && selectedDepositObjectAdditionalTag != Tag.Invalid)
            flag = PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(selectedDepositObjectAdditionalTag);

        if (!flag)
            text = CREATURES.PLANT_MUTATIONS.UNIDENTIFIED + "\n\n" + CREATURES.PLANT_MUTATIONS.UNIDENTIFIED_DESC;
        else if (component != null) {
            list = component.GetDescriptors(component.gameObject);
            if (targetReceptacle.rotatable != null && targetReceptacle.Direction != component.direction) {
                if (component.direction == SingleEntityReceptacle.ReceptacleDirection.Top)
                    text += UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_FLOOR;
                else if (component.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
                    text += UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_WALL;
                else if (component.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
                    text += UI.UISIDESCREENS.PLANTERSIDESCREEN.ROTATION_NEED_CEILING;

                text += "\n\n";
            }

            gameObject = Assets.GetPrefab(component.PlantID);
            var component2 = gameObject.GetComponent<MutantPlant>();
            if (component2 != null && selectedDepositObjectAdditionalTag.IsValid) {
                var subSpecies
                    = PlantSubSpeciesCatalog.Instance.GetSubSpecies(component.PlantID,
                                                                    selectedDepositObjectAdditionalTag);

                component2.DummySetSubspecies(subSpecies.mutationIDs);
            }

            if (!string.IsNullOrEmpty(component.domesticatedDescription)) text += component.domesticatedDescription;
        } else {
            var component3       = gameObject.GetComponent<InfoDescription>();
            if (component3) text += component3.description;
        }

        descriptionLabel.SetText(text);
        var plantLifeCycleDescriptors = GameUtil.GetPlantLifeCycleDescriptors(gameObject);
        if (plantLifeCycleDescriptors.Count > 0 && flag) {
            HarvestDescriptorPanel.SetDescriptors(plantLifeCycleDescriptors);
            HarvestDescriptorPanel.gameObject.SetActive(true);
        } else
            HarvestDescriptorPanel.gameObject.SetActive(false);

        var plantRequirementDescriptors = GameUtil.GetPlantRequirementDescriptors(gameObject);
        if (list.Count > 0) {
            GameUtil.IndentListOfDescriptors(list);
            plantRequirementDescriptors.InsertRange(plantRequirementDescriptors.Count, list);
        }

        if (plantRequirementDescriptors.Count > 0 && flag) {
            RequirementsDescriptorPanel.SetDescriptors(plantRequirementDescriptors);
            RequirementsDescriptorPanel.gameObject.SetActive(true);
        } else
            RequirementsDescriptorPanel.gameObject.SetActive(false);

        var plantEffectDescriptors = GameUtil.GetPlantEffectDescriptors(gameObject);
        if (plantEffectDescriptors.Count > 0 && flag) {
            EffectsDescriptorPanel.SetDescriptors(plantEffectDescriptors);
            EffectsDescriptorPanel.gameObject.SetActive(true);
            return;
        }

        EffectsDescriptorPanel.gameObject.SetActive(false);
    }

    protected override bool AdditionalCanDepositTest() {
        var flag = false;
        if (selectedDepositObjectTag.IsValid) {
            if (DlcManager.FeaturePlantMutationsEnabled())
                flag = PlantSubSpeciesCatalog.Instance.IsValidPlantableSeed(selectedDepositObjectTag,
                                                                            selectedDepositObjectAdditionalTag);
            else
                flag = selectedDepositObjectTag.IsValid;
        }

        var plantablePlot = targetReceptacle as PlantablePlot;
        var myWorld       = targetReceptacle.GetMyWorld();
        return flag                     &&
               plantablePlot.ValidPlant &&
               myWorld.worldInventory.GetCountWithAdditionalTag(selectedDepositObjectTag,
                                                                selectedDepositObjectAdditionalTag,
                                                                myWorld.IsModuleInterior) >
               0;
    }

    public override void SetTarget(GameObject target) {
        selectedDepositObjectTag           = Tag.Invalid;
        selectedDepositObjectAdditionalTag = Tag.Invalid;
        base.SetTarget(target);
        LoadTargetSubSpeciesRequest();
        RefreshSubspeciesToggles();
    }

    protected override void RestoreSelectionFromOccupant() {
        base.RestoreSelectionFromOccupant();
        var plantablePlot = (PlantablePlot)targetReceptacle;
        var tag           = Tag.Invalid;
        var tag2          = Tag.Invalid;
        var flag          = false;
        if (plantablePlot.Occupant != null) {
            tag = plantablePlot.Occupant.GetComponent<SeedProducer>().seedInfo.seedId;
            var component               = plantablePlot.Occupant.GetComponent<MutantPlant>();
            if (component != null) tag2 = component.SubSpeciesID;
        } else if (plantablePlot.GetActiveRequest != null) {
            tag                                = plantablePlot.requestedEntityTag;
            tag2                               = plantablePlot.requestedEntityAdditionalFilterTag;
            selectedDepositObjectTag           = tag;
            selectedDepositObjectAdditionalTag = tag2;
            flag                               = true;
        }

        if (tag != Tag.Invalid) {
            if (!entityPreviousSelectionMap.ContainsKey(plantablePlot) || flag) {
                var value = 0;
                foreach (var keyValuePair in depositObjectMap)
                    if (keyValuePair.Value.tag == tag)
                        value = entityToggles.IndexOf(keyValuePair.Key);

                if (!entityPreviousSelectionMap.ContainsKey(plantablePlot))
                    entityPreviousSelectionMap.Add(plantablePlot, -1);

                entityPreviousSelectionMap[plantablePlot] = value;
            }

            if (!entityPreviousSubSelectionMap.ContainsKey(plantablePlot))
                entityPreviousSubSelectionMap.Add(plantablePlot, Tag.Invalid);

            if (entityPreviousSubSelectionMap[plantablePlot] == Tag.Invalid || flag)
                entityPreviousSubSelectionMap[plantablePlot] = tag2;
        }
    }
}