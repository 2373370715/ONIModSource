using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;
using Sicknesses = Database.Sicknesses;

public class Db : EntityModifierSet {
    private static Db                     _Instance;
    public         Accessories            Accessories;
    public         AccessorySlots         AccessorySlots;
    public         ArtableStatuses        ArtableStatuses;
    public         ArtifactDropRates      ArtifactDropRates;
    public         AssignableSlots        AssignableSlots;
    public         BuildingStatusItems    BuildingStatusItems;
    public         ChoreTypes             ChoreTypes;
    public         ColonyAchievements     ColonyAchievements;
    public         CreatureStatusItems    CreatureStatusItems;
    public         Deaths                 Deaths;
    public         Diseases               Diseases;
    public         Dreams                 Dreams;
    public         Emotes                 Emotes;
    public         Expressions            Expressions;
    public         Faces                  Faces;
    public         GameplayEvents         GameplayEvents;
    public         GameplaySeasons        GameplaySeasons;
    public         MiscStatusItems        MiscStatusItems;
    public         OrbitalTypeCategories  OrbitalTypeCategories;
    public         PermitResources        Permits;
    public         Personalities          Personalities;
    public         PlantMutations         PlantMutations;
    public         Quests                 Quests;
    public         TextAsset              researchTreeFileExpansion1;
    public         TextAsset              researchTreeFileVanilla;
    public         RobotStatusItems       RobotStatusItems;
    public         RoomTypeCategories     RoomTypeCategories;
    public         RoomTypes              RoomTypes;
    public         ScheduleBlockTypes     ScheduleBlockTypes;
    public         ScheduleGroups         ScheduleGroups;
    public         Shirts                 Shirts;
    public         Sicknesses             Sicknesses;
    public         SkillGroups            SkillGroups;
    public         SkillPerks             SkillPerks;
    public         Skills                 Skills;
    public         SpaceDestinationTypes  SpaceDestinationTypes;
    public         Spices                 Spices;
    public         StateMachineCategories StateMachineCategories;
    public         StatusItemCategories   StatusItemCategories;
    public         Stories                Stories;
    public         TechItems              TechItems;
    public         Techs                  Techs;
    public         TechTreeTitles         TechTreeTitles;
    public         Thoughts               Thoughts;
    public         Urges                  Urges;

    public static string GetPath(string dlcId, string folder) {
        string result;
        if (dlcId == "")
            result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath, folder));
        else {
            var contentDirectoryName = DlcManager.GetContentDirectoryName(dlcId);
            result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath,
                                                       "dlc",
                                                       contentDirectoryName,
                                                       folder));
        }

        return result;
    }

    public static Db Get() {
        if (_Instance == null) {
            _Instance = Resources.Load<Db>("Db");
            _Instance.Initialize();
        }

        return _Instance;
    }

    public static BuildingFacades   GetBuildingFacades()   { return Get().Permits.BuildingFacades; }
    public static ArtableStages     GetArtableStages()     { return Get().Permits.ArtableStages; }
    public static EquippableFacades GetEquippableFacades() { return Get().Permits.EquippableFacades; }
    public static StickerBombs      GetStickerBombs()      { return Get().Permits.StickerBombs; }
    public static MonumentParts     GetMonumentParts()     { return Get().Permits.MonumentParts; }

    public override void Initialize() {
        base.Initialize();
        Urges                  = new Urges();
        AssignableSlots        = new AssignableSlots();
        StateMachineCategories = new StateMachineCategories();
        Personalities          = new Personalities();
        Faces                  = new Faces();
        Shirts                 = new Shirts();
        Expressions            = new Expressions(Root);
        Emotes                 = new Emotes(Root);
        Thoughts               = new Thoughts(Root);
        Dreams                 = new Dreams(Root);
        Deaths                 = new Deaths(Root);
        StatusItemCategories   = new StatusItemCategories(Root);
        TechTreeTitles         = new TechTreeTitles(Root);
        TechTreeTitles.Load(DlcManager.IsExpansion1Active() ? researchTreeFileExpansion1 : researchTreeFileVanilla);
        Techs     = new Techs(Root);
        TechItems = new TechItems(Root);
        Techs.Init();
        Techs.Load(DlcManager.IsExpansion1Active() ? researchTreeFileExpansion1 : researchTreeFileVanilla);
        TechItems.Init();
        Accessories           = new Accessories(Root);
        AccessorySlots        = new AccessorySlots(Root);
        ScheduleBlockTypes    = new ScheduleBlockTypes(Root);
        ScheduleGroups        = new ScheduleGroups(Root);
        RoomTypeCategories    = new RoomTypeCategories(Root);
        RoomTypes             = new RoomTypes(Root);
        ArtifactDropRates     = new ArtifactDropRates(Root);
        SpaceDestinationTypes = new SpaceDestinationTypes(Root);
        Diseases              = new Diseases(Root);
        Sicknesses            = new Sicknesses(Root);
        SkillPerks            = new SkillPerks(Root);
        SkillGroups           = new SkillGroups(Root);
        Skills                = new Skills(Root);
        ColonyAchievements    = new ColonyAchievements(Root);
        MiscStatusItems       = new MiscStatusItems(Root);
        CreatureStatusItems   = new CreatureStatusItems(Root);
        BuildingStatusItems   = new BuildingStatusItems(Root);
        RobotStatusItems      = new RobotStatusItems(Root);
        ChoreTypes            = new ChoreTypes(Root);
        Quests                = new Quests(Root);
        GameplayEvents        = new GameplayEvents(Root);
        GameplaySeasons       = new GameplaySeasons(Root);
        Stories               = new Stories(Root);
        if (DlcManager.FeaturePlantMutationsEnabled()) PlantMutations = new PlantMutations(Root);
        OrbitalTypeCategories = new OrbitalTypeCategories(Root);
        ArtableStatuses       = new ArtableStatuses(Root);
        Permits               = new PermitResources(Root);
        var effect = new Effect("CenterOfAttention",
                                DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME,
                                DUPLICANTS.MODIFIERS.CENTEROFATTENTION.TOOLTIP,
                                0f,
                                true,
                                true,
                                false);

        effect.Add(new AttributeModifier("StressDelta", -0.008333334f, DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME));
        effects.Add(effect);
        Spices = new Spices(Root);
        CollectResources(Root, ResourceTable);
    }

    public void PostProcess() {
        Techs.PostProcess();
        Permits.PostProcess();
    }

    private void CollectResources(Resource resource, List<Resource> resource_table) {
        if (resource.Guid != null) resource_table.Add(resource);
        var resourceSet = resource as ResourceSet;
        if (resourceSet != null)
            for (var i = 0; i < resourceSet.Count; i++)
                CollectResources(resourceSet.GetResource(i), resource_table);
    }

    public ResourceType GetResource<ResourceType>(ResourceGuid guid) where ResourceType : Resource {
        var resource = ResourceTable.FirstOrDefault(s => s.Guid == guid);
        if (resource == null) {
            var str   = "Could not find resource: ";
            var guid2 = guid;
            Debug.LogWarning(str + (guid2 != null ? guid2.ToString() : null));
            return default(ResourceType);
        }

        var resourceType = (ResourceType)resource;
        if (resourceType == null) {
            Debug.LogError(string.Concat("Resource type mismatch for resource: ",
                                         resource.Id,
                                         "\nExpecting Type: ",
                                         typeof(ResourceType).Name,
                                         "\nGot Type: ",
                                         resource.GetType().Name));

            return default(ResourceType);
        }

        return resourceType;
    }

    public void ResetProblematicDbs() { Emotes.ResetProblematicReferences(); }

    [Serializable]
    public class SlotInfo : Resource { }
}