using System;
using System.Collections.Generic;
using ProcGen;
using ProcGenGame;
using STRINGS;
using UnityEngine;

// Token: 0x02001C69 RID: 7273
public class ColonyDestinationAsteroidBeltData
{
	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x0600978E RID: 38798 RVA: 0x001027F3 File Offset: 0x001009F3
	// (set) Token: 0x0600978F RID: 38799 RVA: 0x001027FB File Offset: 0x001009FB
	public float TargetScale { get; set; }

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x06009790 RID: 38800 RVA: 0x00102804 File Offset: 0x00100A04
	// (set) Token: 0x06009791 RID: 38801 RVA: 0x0010280C File Offset: 0x00100A0C
	public float Scale { get; set; }

	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x06009792 RID: 38802 RVA: 0x00102815 File Offset: 0x00100A15
	// (set) Token: 0x06009793 RID: 38803 RVA: 0x0010281D File Offset: 0x00100A1D
	public int seed { get; private set; }

	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x06009794 RID: 38804 RVA: 0x00102826 File Offset: 0x00100A26
	public string startWorldPath
	{
		get
		{
			return this.startWorld.filePath;
		}
	}

	// Token: 0x17000A0A RID: 2570
	// (get) Token: 0x06009795 RID: 38805 RVA: 0x00102833 File Offset: 0x00100A33
	// (set) Token: 0x06009796 RID: 38806 RVA: 0x0010283B File Offset: 0x00100A3B
	public Sprite sprite { get; private set; }

	// Token: 0x17000A0B RID: 2571
	// (get) Token: 0x06009797 RID: 38807 RVA: 0x00102844 File Offset: 0x00100A44
	// (set) Token: 0x06009798 RID: 38808 RVA: 0x0010284C File Offset: 0x00100A4C
	public int difficulty { get; private set; }

	// Token: 0x17000A0C RID: 2572
	// (get) Token: 0x06009799 RID: 38809 RVA: 0x00102855 File Offset: 0x00100A55
	public string startWorldName
	{
		get
		{
			return Strings.Get(this.startWorld.name);
		}
	}

	// Token: 0x17000A0D RID: 2573
	// (get) Token: 0x0600979A RID: 38810 RVA: 0x0010286C File Offset: 0x00100A6C
	public string properName
	{
		get
		{
			if (this.clusterLayout == null)
			{
				return "";
			}
			return this.clusterLayout.name;
		}
	}

	// Token: 0x17000A0E RID: 2574
	// (get) Token: 0x0600979B RID: 38811 RVA: 0x00102887 File Offset: 0x00100A87
	public string beltPath
	{
		get
		{
			if (this.clusterLayout == null)
			{
				return WorldGenSettings.ClusterDefaultName;
			}
			return this.clusterLayout.filePath;
		}
	}

	// Token: 0x17000A0F RID: 2575
	// (get) Token: 0x0600979C RID: 38812 RVA: 0x001028A2 File Offset: 0x00100AA2
	// (set) Token: 0x0600979D RID: 38813 RVA: 0x001028AA File Offset: 0x00100AAA
	public List<ProcGen.World> worlds { get; private set; }

	// Token: 0x17000A10 RID: 2576
	// (get) Token: 0x0600979E RID: 38814 RVA: 0x001028B3 File Offset: 0x00100AB3
	public ClusterLayout Layout
	{
		get
		{
			if (this.mutatedClusterLayout != null)
			{
				return this.mutatedClusterLayout.layout;
			}
			return this.clusterLayout;
		}
	}

	// Token: 0x17000A11 RID: 2577
	// (get) Token: 0x0600979F RID: 38815 RVA: 0x001028CF File Offset: 0x00100ACF
	public ProcGen.World GetStartWorld
	{
		get
		{
			return this.startWorld;
		}
	}

	// Token: 0x060097A0 RID: 38816 RVA: 0x003AC2B4 File Offset: 0x003AA4B4
	public ColonyDestinationAsteroidBeltData(string staringWorldName, int seed, string clusterPath)
	{
		this.startWorld = SettingsCache.worlds.GetWorldData(staringWorldName);
		this.Scale = (this.TargetScale = this.startWorld.iconScale);
		this.worlds = new List<ProcGen.World>();
		if (clusterPath != null)
		{
			this.clusterLayout = SettingsCache.clusterLayouts.GetClusterData(clusterPath);
		}
		this.ReInitialize(seed);
	}

	// Token: 0x060097A1 RID: 38817 RVA: 0x003AC330 File Offset: 0x003AA530
	public static Sprite GetUISprite(string filename)
	{
		if (filename.IsNullOrWhiteSpace())
		{
			filename = (DlcManager.FeatureClusterSpaceEnabled() ? "asteroid_sandstone_start_kanim" : "Asteroid_sandstone");
		}
		KAnimFile kanimFile;
		Assets.TryGetAnim(filename, out kanimFile);
		if (kanimFile != null)
		{
			return Def.GetUISpriteFromMultiObjectAnim(kanimFile, "ui", false, "");
		}
		return Assets.GetSprite(filename);
	}

	// Token: 0x060097A2 RID: 38818 RVA: 0x003AC390 File Offset: 0x003AA590
	public void ReInitialize(int seed)
	{
		this.seed = seed;
		this.paramDescriptors.Clear();
		this.traitDescriptors.Clear();
		this.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(this.startWorld.asteroidIcon);
		this.difficulty = this.clusterLayout.difficulty;
		this.mutatedClusterLayout = WorldgenMixing.DoWorldMixing(this.clusterLayout, seed, true, true);
		this.RemixClusterLayout();
	}

	// Token: 0x060097A3 RID: 38819 RVA: 0x003AC3FC File Offset: 0x003AA5FC
	public void RemixClusterLayout()
	{
		if (!WorldgenMixing.RefreshWorldMixing(this.mutatedClusterLayout, this.seed, true, true))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"World remix failed, using default cluster instead."
			});
			this.mutatedClusterLayout = new MutatedClusterLayout(this.clusterLayout);
		}
		this.worlds.Clear();
		for (int i = 0; i < this.Layout.worldPlacements.Count; i++)
		{
			if (i != this.Layout.startWorldIndex)
			{
				this.worlds.Add(SettingsCache.worlds.GetWorldData(this.Layout.worldPlacements[i].world));
			}
		}
	}

	// Token: 0x060097A4 RID: 38820 RVA: 0x001028D7 File Offset: 0x00100AD7
	public List<AsteroidDescriptor> GetParamDescriptors()
	{
		if (this.paramDescriptors.Count == 0)
		{
			this.paramDescriptors = this.GenerateParamDescriptors();
		}
		return this.paramDescriptors;
	}

	// Token: 0x060097A5 RID: 38821 RVA: 0x001028F8 File Offset: 0x00100AF8
	public List<AsteroidDescriptor> GetTraitDescriptors()
	{
		if (this.traitDescriptors.Count == 0)
		{
			this.traitDescriptors = this.GenerateTraitDescriptors();
		}
		return this.traitDescriptors;
	}

	// Token: 0x060097A6 RID: 38822 RVA: 0x003AC4A4 File Offset: 0x003AA6A4
	private List<AsteroidDescriptor> GenerateParamDescriptors()
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		if (this.clusterLayout != null && DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, Strings.Get(this.clusterLayout.name)), Strings.Get(this.clusterLayout.description), Color.white, null, null));
		}
		list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.PLANETNAME, this.startWorldName), null, Color.white, null, null));
		list.Add(new AsteroidDescriptor(Strings.Get(this.startWorld.description), null, Color.white, null, null));
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.MOONNAMES, Array.Empty<object>()), null, Color.white, null, null));
			foreach (ProcGen.World world in this.worlds)
			{
				list.Add(new AsteroidDescriptor(string.Format("{0}", Strings.Get(world.name)), Strings.Get(world.description), Color.white, null, null));
			}
		}
		int index = Mathf.Clamp(this.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
		global::Tuple<string, string, string> tuple = ColonyDestinationAsteroidBeltData.survivalOptions[index];
		list.Add(new AsteroidDescriptor(string.Format(WORLDS.SURVIVAL_CHANCE.TITLE, tuple.first, tuple.third), null, Color.white, null, null));
		return list;
	}

	// Token: 0x060097A7 RID: 38823 RVA: 0x003AC65C File Offset: 0x003AA85C
	private List<AsteroidDescriptor> GenerateTraitDescriptors()
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			ProcGen.World world = list2[i];
			if (DlcManager.IsExpansion1Active())
			{
				list.Add(new AsteroidDescriptor("", null, Color.white, null, null));
				list.Add(new AsteroidDescriptor(string.Format("<b>{0}</b>", Strings.Get(world.name)), null, Color.white, null, null));
			}
			List<WorldTrait> worldTraits = this.GetWorldTraits(world);
			foreach (WorldTrait worldTrait in worldTraits)
			{
				string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
				list.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", Strings.Get(worldTrait.name), worldTrait.colorHex), Strings.Get(worldTrait.description), global::Util.ColorFromHex(worldTrait.colorHex), null, associatedIcon));
			}
			if (worldTraits.Count == 0)
			{
				list.Add(new AsteroidDescriptor(WORLD_TRAITS.NO_TRAITS.NAME, WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, null, "NoTraits"));
			}
		}
		return list;
	}

	// Token: 0x060097A8 RID: 38824 RVA: 0x003AC7D8 File Offset: 0x003AA9D8
	public List<AsteroidDescriptor> GenerateTraitDescriptors(ProcGen.World singleWorld, bool includeDefaultTrait = true)
	{
		List<AsteroidDescriptor> list = new List<AsteroidDescriptor>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i] == singleWorld)
			{
				ProcGen.World singleWorld2 = list2[i];
				List<WorldTrait> worldTraits = this.GetWorldTraits(singleWorld2);
				foreach (WorldTrait worldTrait in worldTraits)
				{
					string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
					list.Add(new AsteroidDescriptor(string.Format("<color=#{1}>{0}</color>", Strings.Get(worldTrait.name), worldTrait.colorHex), Strings.Get(worldTrait.description), global::Util.ColorFromHex(worldTrait.colorHex), null, associatedIcon));
				}
				if (worldTraits.Count == 0 && includeDefaultTrait)
				{
					list.Add(new AsteroidDescriptor(WORLD_TRAITS.NO_TRAITS.NAME, WORLD_TRAITS.NO_TRAITS.DESCRIPTION, Color.white, null, "NoTraits"));
				}
			}
		}
		return list;
	}

	// Token: 0x060097A9 RID: 38825 RVA: 0x003AC920 File Offset: 0x003AAB20
	public List<WorldTrait> GetWorldTraits(ProcGen.World singleWorld)
	{
		List<WorldTrait> list = new List<WorldTrait>();
		List<ProcGen.World> list2 = new List<ProcGen.World>();
		list2.Add(this.startWorld);
		list2.AddRange(this.worlds);
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i] == singleWorld)
			{
				ProcGen.World world = list2[i];
				int num = this.seed;
				if (num > 0)
				{
					num += this.clusterLayout.worldPlacements.FindIndex((WorldPlacement x) => x.world == world.filePath);
				}
				foreach (string name in SettingsCache.GetRandomTraits(num, world))
				{
					WorldTrait cachedWorldTrait = SettingsCache.GetCachedWorldTrait(name, true);
					list.Add(cachedWorldTrait);
				}
			}
		}
		return list;
	}

	// Token: 0x040075A1 RID: 30113
	private ProcGen.World startWorld;

	// Token: 0x040075A2 RID: 30114
	private ClusterLayout clusterLayout;

	// Token: 0x040075A3 RID: 30115
	private MutatedClusterLayout mutatedClusterLayout;

	// Token: 0x040075A4 RID: 30116
	private List<AsteroidDescriptor> paramDescriptors = new List<AsteroidDescriptor>();

	// Token: 0x040075A5 RID: 30117
	private List<AsteroidDescriptor> traitDescriptors = new List<AsteroidDescriptor>();

	// Token: 0x040075A6 RID: 30118
	public static List<global::Tuple<string, string, string>> survivalOptions = new List<global::Tuple<string, string, string>>
	{
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.MOSTHOSPITABLE, "", "D2F40C"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.VERYHIGH, "", "7DE419"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.HIGH, "", "36D246"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.NEUTRAL, "", "63C2B7"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.LOW, "", "6A8EB1"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.VERYLOW, "", "937890"),
		new global::Tuple<string, string, string>(WORLDS.SURVIVAL_CHANCE.LEASTHOSPITABLE, "", "9636DF")
	};
}
