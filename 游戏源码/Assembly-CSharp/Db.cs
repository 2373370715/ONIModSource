using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Database;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000B82 RID: 2946
public class Db : EntityModifierSet
{
	// Token: 0x06003837 RID: 14391 RVA: 0x0021A7D0 File Offset: 0x002189D0
	public static string GetPath(string dlcId, string folder)
	{
		string result;
		if (dlcId == "")
		{
			result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath, folder));
		}
		else
		{
			string contentDirectoryName = DlcManager.GetContentDirectoryName(dlcId);
			result = FileSystem.Normalize(Path.Combine(Application.streamingAssetsPath, "dlc", contentDirectoryName, folder));
		}
		return result;
	}

	// Token: 0x06003838 RID: 14392 RVA: 0x000C467F File Offset: 0x000C287F
	public static Db Get()
	{
		if (Db._Instance == null)
		{
			Db._Instance = Resources.Load<Db>("Db");
			Db._Instance.Initialize();
		}
		return Db._Instance;
	}

	// Token: 0x06003839 RID: 14393 RVA: 0x000C46AC File Offset: 0x000C28AC
	public static BuildingFacades GetBuildingFacades()
	{
		return Db.Get().Permits.BuildingFacades;
	}

	// Token: 0x0600383A RID: 14394 RVA: 0x000C46BD File Offset: 0x000C28BD
	public static ArtableStages GetArtableStages()
	{
		return Db.Get().Permits.ArtableStages;
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x000C46CE File Offset: 0x000C28CE
	public static EquippableFacades GetEquippableFacades()
	{
		return Db.Get().Permits.EquippableFacades;
	}

	// Token: 0x0600383C RID: 14396 RVA: 0x000C46DF File Offset: 0x000C28DF
	public static StickerBombs GetStickerBombs()
	{
		return Db.Get().Permits.StickerBombs;
	}

	// Token: 0x0600383D RID: 14397 RVA: 0x000C46F0 File Offset: 0x000C28F0
	public static MonumentParts GetMonumentParts()
	{
		return Db.Get().Permits.MonumentParts;
	}

	// Token: 0x0600383E RID: 14398 RVA: 0x0021A81C File Offset: 0x00218A1C
	public override void Initialize()
	{
		base.Initialize();
		this.Urges = new Urges();
		this.AssignableSlots = new AssignableSlots();
		this.StateMachineCategories = new StateMachineCategories();
		this.Personalities = new Personalities();
		this.Faces = new Faces();
		this.Shirts = new Shirts();
		this.Expressions = new Expressions(this.Root);
		this.Emotes = new Emotes(this.Root);
		this.Thoughts = new Thoughts(this.Root);
		this.Dreams = new Dreams(this.Root);
		this.Deaths = new Deaths(this.Root);
		this.StatusItemCategories = new StatusItemCategories(this.Root);
		this.TechTreeTitles = new TechTreeTitles(this.Root);
		this.TechTreeTitles.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
		this.Techs = new Techs(this.Root);
		this.TechItems = new TechItems(this.Root);
		this.Techs.Init();
		this.Techs.Load(DlcManager.IsExpansion1Active() ? this.researchTreeFileExpansion1 : this.researchTreeFileVanilla);
		this.TechItems.Init();
		this.Accessories = new Accessories(this.Root);
		this.AccessorySlots = new AccessorySlots(this.Root);
		this.ScheduleBlockTypes = new ScheduleBlockTypes(this.Root);
		this.ScheduleGroups = new ScheduleGroups(this.Root);
		this.RoomTypeCategories = new RoomTypeCategories(this.Root);
		this.RoomTypes = new RoomTypes(this.Root);
		this.ArtifactDropRates = new ArtifactDropRates(this.Root);
		this.SpaceDestinationTypes = new SpaceDestinationTypes(this.Root);
		this.Diseases = new Diseases(this.Root, false);
		this.Sicknesses = new Database.Sicknesses(this.Root);
		this.SkillPerks = new SkillPerks(this.Root);
		this.SkillGroups = new SkillGroups(this.Root);
		this.Skills = new Skills(this.Root);
		this.ColonyAchievements = new ColonyAchievements(this.Root);
		this.MiscStatusItems = new MiscStatusItems(this.Root);
		this.CreatureStatusItems = new CreatureStatusItems(this.Root);
		this.BuildingStatusItems = new BuildingStatusItems(this.Root);
		this.RobotStatusItems = new RobotStatusItems(this.Root);
		this.ChoreTypes = new ChoreTypes(this.Root);
		this.Quests = new Quests(this.Root);
		this.GameplayEvents = new GameplayEvents(this.Root);
		this.GameplaySeasons = new GameplaySeasons(this.Root);
		this.Stories = new Stories(this.Root);
		if (DlcManager.FeaturePlantMutationsEnabled())
		{
			this.PlantMutations = new PlantMutations(this.Root);
		}
		this.OrbitalTypeCategories = new OrbitalTypeCategories(this.Root);
		this.ArtableStatuses = new ArtableStatuses(this.Root);
		this.Permits = new PermitResources(this.Root);
		Effect effect = new Effect("CenterOfAttention", DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, DUPLICANTS.MODIFIERS.CENTEROFATTENTION.TOOLTIP, 0f, true, true, false, null, -1f, 0f, null, "");
		effect.Add(new AttributeModifier("StressDelta", -0.008333334f, DUPLICANTS.MODIFIERS.CENTEROFATTENTION.NAME, false, false, true));
		this.effects.Add(effect);
		this.Spices = new Spices(this.Root);
		this.CollectResources(this.Root, this.ResourceTable);
	}

	// Token: 0x0600383F RID: 14399 RVA: 0x000C4701 File Offset: 0x000C2901
	public void PostProcess()
	{
		this.Techs.PostProcess();
		this.Permits.PostProcess();
	}

	// Token: 0x06003840 RID: 14400 RVA: 0x0021ABC0 File Offset: 0x00218DC0
	private void CollectResources(Resource resource, List<Resource> resource_table)
	{
		if (resource.Guid != null)
		{
			resource_table.Add(resource);
		}
		ResourceSet resourceSet = resource as ResourceSet;
		if (resourceSet != null)
		{
			for (int i = 0; i < resourceSet.Count; i++)
			{
				this.CollectResources(resourceSet.GetResource(i), resource_table);
			}
		}
	}

	// Token: 0x06003841 RID: 14401 RVA: 0x0021AC0C File Offset: 0x00218E0C
	public ResourceType GetResource<ResourceType>(ResourceGuid guid) where ResourceType : Resource
	{
		Resource resource = this.ResourceTable.FirstOrDefault((Resource s) => s.Guid == guid);
		if (resource == null)
		{
			string str = "Could not find resource: ";
			ResourceGuid guid2 = guid;
			global::Debug.LogWarning(str + ((guid2 != null) ? guid2.ToString() : null));
			return default(ResourceType);
		}
		ResourceType resourceType = (ResourceType)((object)resource);
		if (resourceType == null)
		{
			global::Debug.LogError(string.Concat(new string[]
			{
				"Resource type mismatch for resource: ",
				resource.Id,
				"\nExpecting Type: ",
				typeof(ResourceType).Name,
				"\nGot Type: ",
				resource.GetType().Name
			}));
			return default(ResourceType);
		}
		return resourceType;
	}

	// Token: 0x06003842 RID: 14402 RVA: 0x000C4719 File Offset: 0x000C2919
	public void ResetProblematicDbs()
	{
		this.Emotes.ResetProblematicReferences();
	}

	// Token: 0x0400262E RID: 9774
	private static Db _Instance;

	// Token: 0x0400262F RID: 9775
	public TextAsset researchTreeFileVanilla;

	// Token: 0x04002630 RID: 9776
	public TextAsset researchTreeFileExpansion1;

	// Token: 0x04002631 RID: 9777
	public Diseases Diseases;

	// Token: 0x04002632 RID: 9778
	public Database.Sicknesses Sicknesses;

	// Token: 0x04002633 RID: 9779
	public Urges Urges;

	// Token: 0x04002634 RID: 9780
	public AssignableSlots AssignableSlots;

	// Token: 0x04002635 RID: 9781
	public StateMachineCategories StateMachineCategories;

	// Token: 0x04002636 RID: 9782
	public Personalities Personalities;

	// Token: 0x04002637 RID: 9783
	public Faces Faces;

	// Token: 0x04002638 RID: 9784
	public Shirts Shirts;

	// Token: 0x04002639 RID: 9785
	public Expressions Expressions;

	// Token: 0x0400263A RID: 9786
	public Emotes Emotes;

	// Token: 0x0400263B RID: 9787
	public Thoughts Thoughts;

	// Token: 0x0400263C RID: 9788
	public Dreams Dreams;

	// Token: 0x0400263D RID: 9789
	public BuildingStatusItems BuildingStatusItems;

	// Token: 0x0400263E RID: 9790
	public MiscStatusItems MiscStatusItems;

	// Token: 0x0400263F RID: 9791
	public CreatureStatusItems CreatureStatusItems;

	// Token: 0x04002640 RID: 9792
	public RobotStatusItems RobotStatusItems;

	// Token: 0x04002641 RID: 9793
	public StatusItemCategories StatusItemCategories;

	// Token: 0x04002642 RID: 9794
	public Deaths Deaths;

	// Token: 0x04002643 RID: 9795
	public ChoreTypes ChoreTypes;

	// Token: 0x04002644 RID: 9796
	public TechItems TechItems;

	// Token: 0x04002645 RID: 9797
	public AccessorySlots AccessorySlots;

	// Token: 0x04002646 RID: 9798
	public Accessories Accessories;

	// Token: 0x04002647 RID: 9799
	public ScheduleBlockTypes ScheduleBlockTypes;

	// Token: 0x04002648 RID: 9800
	public ScheduleGroups ScheduleGroups;

	// Token: 0x04002649 RID: 9801
	public RoomTypeCategories RoomTypeCategories;

	// Token: 0x0400264A RID: 9802
	public RoomTypes RoomTypes;

	// Token: 0x0400264B RID: 9803
	public ArtifactDropRates ArtifactDropRates;

	// Token: 0x0400264C RID: 9804
	public SpaceDestinationTypes SpaceDestinationTypes;

	// Token: 0x0400264D RID: 9805
	public SkillPerks SkillPerks;

	// Token: 0x0400264E RID: 9806
	public SkillGroups SkillGroups;

	// Token: 0x0400264F RID: 9807
	public Skills Skills;

	// Token: 0x04002650 RID: 9808
	public ColonyAchievements ColonyAchievements;

	// Token: 0x04002651 RID: 9809
	public Quests Quests;

	// Token: 0x04002652 RID: 9810
	public GameplayEvents GameplayEvents;

	// Token: 0x04002653 RID: 9811
	public GameplaySeasons GameplaySeasons;

	// Token: 0x04002654 RID: 9812
	public PlantMutations PlantMutations;

	// Token: 0x04002655 RID: 9813
	public Spices Spices;

	// Token: 0x04002656 RID: 9814
	public Techs Techs;

	// Token: 0x04002657 RID: 9815
	public TechTreeTitles TechTreeTitles;

	// Token: 0x04002658 RID: 9816
	public OrbitalTypeCategories OrbitalTypeCategories;

	// Token: 0x04002659 RID: 9817
	public PermitResources Permits;

	// Token: 0x0400265A RID: 9818
	public ArtableStatuses ArtableStatuses;

	// Token: 0x0400265B RID: 9819
	public Stories Stories;

	// Token: 0x02000B83 RID: 2947
	[Serializable]
	public class SlotInfo : Resource
	{
	}
}
