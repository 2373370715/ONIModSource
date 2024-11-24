using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Delaunay.Geo;
using Klei;
using KSerialization;
using ProcGen;
using ProcGenGame;
using TemplateClasses;
using TUNING;
using UnityEngine;

// Token: 0x02001A4D RID: 6733
[SerializationConfig(MemberSerialization.OptIn)]
public class WorldContainer : KMonoBehaviour
{
	// Token: 0x17000936 RID: 2358
	// (get) Token: 0x06008C7E RID: 35966 RVA: 0x000FBCD5 File Offset: 0x000F9ED5
	// (set) Token: 0x06008C7F RID: 35967 RVA: 0x000FBCDD File Offset: 0x000F9EDD
	[Serialize]
	public WorldInventory worldInventory { get; private set; }

	// Token: 0x17000937 RID: 2359
	// (get) Token: 0x06008C80 RID: 35968 RVA: 0x000FBCE6 File Offset: 0x000F9EE6
	// (set) Token: 0x06008C81 RID: 35969 RVA: 0x000FBCEE File Offset: 0x000F9EEE
	public Dictionary<Tag, float> materialNeeds { get; private set; }

	// Token: 0x17000938 RID: 2360
	// (get) Token: 0x06008C82 RID: 35970 RVA: 0x000FBCF7 File Offset: 0x000F9EF7
	public bool IsModuleInterior
	{
		get
		{
			return this.isModuleInterior;
		}
	}

	// Token: 0x17000939 RID: 2361
	// (get) Token: 0x06008C83 RID: 35971 RVA: 0x000FBCFF File Offset: 0x000F9EFF
	public bool IsDiscovered
	{
		get
		{
			return this.isDiscovered || DebugHandler.RevealFogOfWar;
		}
	}

	// Token: 0x1700093A RID: 2362
	// (get) Token: 0x06008C84 RID: 35972 RVA: 0x000FBD10 File Offset: 0x000F9F10
	public bool IsStartWorld
	{
		get
		{
			return this.isStartWorld;
		}
	}

	// Token: 0x1700093B RID: 2363
	// (get) Token: 0x06008C85 RID: 35973 RVA: 0x000FBD18 File Offset: 0x000F9F18
	public bool IsDupeVisited
	{
		get
		{
			return this.isDupeVisited;
		}
	}

	// Token: 0x1700093C RID: 2364
	// (get) Token: 0x06008C86 RID: 35974 RVA: 0x000FBD20 File Offset: 0x000F9F20
	public float DupeVisitedTimestamp
	{
		get
		{
			return this.dupeVisitedTimestamp;
		}
	}

	// Token: 0x1700093D RID: 2365
	// (get) Token: 0x06008C87 RID: 35975 RVA: 0x000FBD28 File Offset: 0x000F9F28
	public float DiscoveryTimestamp
	{
		get
		{
			return this.discoveryTimestamp;
		}
	}

	// Token: 0x1700093E RID: 2366
	// (get) Token: 0x06008C88 RID: 35976 RVA: 0x000FBD30 File Offset: 0x000F9F30
	public bool IsRoverVisted
	{
		get
		{
			return this.isRoverVisited;
		}
	}

	// Token: 0x1700093F RID: 2367
	// (get) Token: 0x06008C89 RID: 35977 RVA: 0x000FBD38 File Offset: 0x000F9F38
	public bool IsSurfaceRevealed
	{
		get
		{
			return this.isSurfaceRevealed;
		}
	}

	// Token: 0x17000940 RID: 2368
	// (get) Token: 0x06008C8A RID: 35978 RVA: 0x000FBD40 File Offset: 0x000F9F40
	public Dictionary<string, int> SunlightFixedTraits
	{
		get
		{
			return this.sunlightFixedTraits;
		}
	}

	// Token: 0x17000941 RID: 2369
	// (get) Token: 0x06008C8B RID: 35979 RVA: 0x000FBD48 File Offset: 0x000F9F48
	public Dictionary<string, int> NorthernLightsFixedTraits
	{
		get
		{
			return this.northernLightsFixedTraits;
		}
	}

	// Token: 0x17000942 RID: 2370
	// (get) Token: 0x06008C8C RID: 35980 RVA: 0x000FBD50 File Offset: 0x000F9F50
	public Dictionary<string, int> CosmicRadiationFixedTraits
	{
		get
		{
			return this.cosmicRadiationFixedTraits;
		}
	}

	// Token: 0x17000943 RID: 2371
	// (get) Token: 0x06008C8D RID: 35981 RVA: 0x000FBD58 File Offset: 0x000F9F58
	public List<string> Biomes
	{
		get
		{
			return this.m_subworldNames;
		}
	}

	// Token: 0x17000944 RID: 2372
	// (get) Token: 0x06008C8E RID: 35982 RVA: 0x000FBD60 File Offset: 0x000F9F60
	public List<string> GeneratedBiomes
	{
		get
		{
			return this.m_generatedSubworlds;
		}
	}

	// Token: 0x17000945 RID: 2373
	// (get) Token: 0x06008C8F RID: 35983 RVA: 0x000FBD68 File Offset: 0x000F9F68
	public List<string> WorldTraitIds
	{
		get
		{
			return this.m_worldTraitIds;
		}
	}

	// Token: 0x17000946 RID: 2374
	// (get) Token: 0x06008C90 RID: 35984 RVA: 0x000FBD70 File Offset: 0x000F9F70
	public List<string> StoryTraitIds
	{
		get
		{
			return this.m_storyTraitIds;
		}
	}

	// Token: 0x17000947 RID: 2375
	// (get) Token: 0x06008C91 RID: 35985 RVA: 0x0036307C File Offset: 0x0036127C
	public AlertStateManager.Instance AlertManager
	{
		get
		{
			if (this.m_alertManager == null)
			{
				StateMachineController component = base.GetComponent<StateMachineController>();
				this.m_alertManager = component.GetSMI<AlertStateManager.Instance>();
			}
			global::Debug.Assert(this.m_alertManager != null, "AlertStateManager should never be null.");
			return this.m_alertManager;
		}
	}

	// Token: 0x06008C92 RID: 35986 RVA: 0x000FBD78 File Offset: 0x000F9F78
	public void AddTopPriorityPrioritizable(Prioritizable prioritizable)
	{
		if (!this.yellowAlertTasks.Contains(prioritizable))
		{
			this.yellowAlertTasks.Add(prioritizable);
		}
		this.RefreshHasTopPriorityChore();
	}

	// Token: 0x06008C93 RID: 35987 RVA: 0x003630C0 File Offset: 0x003612C0
	public void RemoveTopPriorityPrioritizable(Prioritizable prioritizable)
	{
		for (int i = this.yellowAlertTasks.Count - 1; i >= 0; i--)
		{
			if (this.yellowAlertTasks[i] == prioritizable || this.yellowAlertTasks[i].Equals(null))
			{
				this.yellowAlertTasks.RemoveAt(i);
			}
		}
		this.RefreshHasTopPriorityChore();
	}

	// Token: 0x17000948 RID: 2376
	// (get) Token: 0x06008C94 RID: 35988 RVA: 0x000FBD9A File Offset: 0x000F9F9A
	// (set) Token: 0x06008C95 RID: 35989 RVA: 0x000FBDA2 File Offset: 0x000F9FA2
	public int ParentWorldId { get; private set; }

	// Token: 0x06008C96 RID: 35990 RVA: 0x000FBDAB File Offset: 0x000F9FAB
	public ICollection<int> GetChildWorldIds()
	{
		return this.m_childWorlds;
	}

	// Token: 0x06008C97 RID: 35991 RVA: 0x00363120 File Offset: 0x00361320
	private void OnWorldRemoved(object data)
	{
		int num = (data is int) ? ((int)data) : 255;
		if (num != 255)
		{
			this.m_childWorlds.Remove(num);
		}
	}

	// Token: 0x06008C98 RID: 35992 RVA: 0x00363158 File Offset: 0x00361358
	private void OnWorldParentChanged(object data)
	{
		WorldParentChangedEventArgs worldParentChangedEventArgs = data as WorldParentChangedEventArgs;
		if (worldParentChangedEventArgs == null)
		{
			return;
		}
		if (worldParentChangedEventArgs.world.ParentWorldId == this.id)
		{
			this.m_childWorlds.Add(worldParentChangedEventArgs.world.id);
		}
		if (worldParentChangedEventArgs.lastParentId == this.ParentWorldId)
		{
			this.m_childWorlds.Remove(worldParentChangedEventArgs.world.id);
		}
	}

	// Token: 0x06008C99 RID: 35993 RVA: 0x003631C0 File Offset: 0x003613C0
	public Quadrant[] GetQuadrantOfCell(int cell, int depth = 1)
	{
		Vector2 vector = new Vector2((float)this.WorldSize.x * Grid.CellSizeInMeters, (float)this.worldSize.y * Grid.CellSizeInMeters);
		Vector2 vector2 = Grid.CellToPos2D(Grid.XYToCell(this.WorldOffset.x, this.WorldOffset.y));
		Vector2 vector3 = Grid.CellToPos2D(cell);
		Quadrant[] array = new Quadrant[depth];
		Vector2 vector4 = new Vector2(vector2.x, (float)this.worldOffset.y + vector.y);
		Vector2 vector5 = new Vector2(vector2.x + vector.x, (float)this.worldOffset.y);
		for (int i = 0; i < depth; i++)
		{
			float num = vector5.x - vector4.x;
			float num2 = vector4.y - vector5.y;
			float num3 = num * 0.5f;
			float num4 = num2 * 0.5f;
			if (vector3.x >= vector4.x + num3 && vector3.y >= vector5.y + num4)
			{
				array[i] = Quadrant.NE;
			}
			if (vector3.x >= vector4.x + num3 && vector3.y < vector5.y + num4)
			{
				array[i] = Quadrant.SE;
			}
			if (vector3.x < vector4.x + num3 && vector3.y < vector5.y + num4)
			{
				array[i] = Quadrant.SW;
			}
			if (vector3.x < vector4.x + num3 && vector3.y >= vector5.y + num4)
			{
				array[i] = Quadrant.NW;
			}
			switch (array[i])
			{
			case Quadrant.NE:
				vector4.x += num3;
				vector5.y += num4;
				break;
			case Quadrant.NW:
				vector5.x -= num3;
				vector5.y += num4;
				break;
			case Quadrant.SW:
				vector4.y -= num4;
				vector5.x -= num3;
				break;
			case Quadrant.SE:
				vector4.x += num3;
				vector4.y -= num4;
				break;
			}
		}
		return array;
	}

	// Token: 0x06008C9A RID: 35994 RVA: 0x000FBDB3 File Offset: 0x000F9FB3
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.ParentWorldId = this.id;
	}

	// Token: 0x06008C9B RID: 35995 RVA: 0x003633F0 File Offset: 0x003615F0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.worldInventory = base.GetComponent<WorldInventory>();
		this.materialNeeds = new Dictionary<Tag, float>();
		ClusterManager.Instance.RegisterWorldContainer(this);
		Game.Instance.Subscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
	}

	// Token: 0x06008C9C RID: 35996 RVA: 0x00363460 File Offset: 0x00361660
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.gameObject.AddOrGet<InfoDescription>().DescriptionLocString = this.worldDescription;
		this.RefreshHasTopPriorityChore();
		this.UpgradeFixedTraits();
		this.RefreshFixedTraits();
		if (DlcManager.IsPureVanilla())
		{
			this.isStartWorld = true;
			this.isDupeVisited = true;
		}
	}

	// Token: 0x06008C9D RID: 35997 RVA: 0x003634B0 File Offset: 0x003616B0
	protected override void OnCleanUp()
	{
		SaveGame.Instance.materialSelectorSerializer.WipeWorldSelectionData(this.id);
		Game.Instance.Unsubscribe(880851192, new Action<object>(this.OnWorldParentChanged));
		ClusterManager.Instance.Unsubscribe(-1078710002, new Action<object>(this.OnWorldRemoved));
		base.OnCleanUp();
	}

	// Token: 0x06008C9E RID: 35998 RVA: 0x00363510 File Offset: 0x00361710
	private void UpgradeFixedTraits()
	{
		if (this.sunlightFixedTrait == null || this.sunlightFixedTrait == "")
		{
			new Dictionary<int, string>
			{
				{
					160000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
				},
				{
					0,
					FIXEDTRAITS.SUNLIGHT.NAME.NONE
				},
				{
					10000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW
				},
				{
					20000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW
				},
				{
					30000,
					FIXEDTRAITS.SUNLIGHT.NAME.LOW
				},
				{
					35000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW
				},
				{
					40000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED
				},
				{
					50000,
					FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH
				},
				{
					60000,
					FIXEDTRAITS.SUNLIGHT.NAME.HIGH
				},
				{
					80000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH
				},
				{
					120000,
					FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH
				}
			}.TryGetValue(this.sunlight, out this.sunlightFixedTrait);
		}
		if (this.cosmicRadiationFixedTrait == null || this.cosmicRadiationFixedTrait == "")
		{
			new Dictionary<int, string>
			{
				{
					0,
					FIXEDTRAITS.COSMICRADIATION.NAME.NONE
				},
				{
					6,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW
				},
				{
					12,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW
				},
				{
					18,
					FIXEDTRAITS.COSMICRADIATION.NAME.LOW
				},
				{
					21,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW
				},
				{
					25,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED
				},
				{
					31,
					FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH
				},
				{
					37,
					FIXEDTRAITS.COSMICRADIATION.NAME.HIGH
				},
				{
					50,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH
				},
				{
					75,
					FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH
				}
			}.TryGetValue(this.cosmicRadiation, out this.cosmicRadiationFixedTrait);
		}
	}

	// Token: 0x06008C9F RID: 35999 RVA: 0x000FBDC1 File Offset: 0x000F9FC1
	private void RefreshFixedTraits()
	{
		this.sunlight = this.GetSunlightValueFromFixedTrait();
		this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
		this.northernlights = this.GetNorthernlightValueFromFixedTrait();
	}

	// Token: 0x06008CA0 RID: 36000 RVA: 0x000FBDE7 File Offset: 0x000F9FE7
	private void RefreshHasTopPriorityChore()
	{
		if (this.AlertManager != null)
		{
			this.AlertManager.SetHasTopPriorityChore(this.yellowAlertTasks.Count > 0);
		}
	}

	// Token: 0x06008CA1 RID: 36001 RVA: 0x000FBE0A File Offset: 0x000FA00A
	public List<string> GetSeasonIds()
	{
		return this.m_seasonIds;
	}

	// Token: 0x06008CA2 RID: 36002 RVA: 0x000FBE12 File Offset: 0x000FA012
	public bool IsRedAlert()
	{
		return this.m_alertManager.IsRedAlert();
	}

	// Token: 0x06008CA3 RID: 36003 RVA: 0x000FBE1F File Offset: 0x000FA01F
	public bool IsYellowAlert()
	{
		return this.m_alertManager.IsYellowAlert();
	}

	// Token: 0x06008CA4 RID: 36004 RVA: 0x000FBE2C File Offset: 0x000FA02C
	public string GetRandomName()
	{
		if (!this.overrideName.IsNullOrWhiteSpace())
		{
			return Strings.Get(this.overrideName);
		}
		return GameUtil.GenerateRandomWorldName(this.nameTables);
	}

	// Token: 0x06008CA5 RID: 36005 RVA: 0x000FBE57 File Offset: 0x000FA057
	public void SetID(int id)
	{
		this.id = id;
		this.ParentWorldId = id;
	}

	// Token: 0x06008CA6 RID: 36006 RVA: 0x003636B4 File Offset: 0x003618B4
	public void SetParentIdx(int parentIdx)
	{
		this.parentChangeArgs.lastParentId = this.ParentWorldId;
		this.parentChangeArgs.world = this;
		this.ParentWorldId = parentIdx;
		Game.Instance.Trigger(880851192, this.parentChangeArgs);
		this.parentChangeArgs.lastParentId = 255;
	}

	// Token: 0x17000949 RID: 2377
	// (get) Token: 0x06008CA7 RID: 36007 RVA: 0x000FBE67 File Offset: 0x000FA067
	public Vector2 minimumBounds
	{
		get
		{
			return new Vector2((float)this.worldOffset.x, (float)this.worldOffset.y);
		}
	}

	// Token: 0x1700094A RID: 2378
	// (get) Token: 0x06008CA8 RID: 36008 RVA: 0x000FBE86 File Offset: 0x000FA086
	public Vector2 maximumBounds
	{
		get
		{
			return new Vector2((float)(this.worldOffset.x + (this.worldSize.x - 1)), (float)(this.worldOffset.y + (this.worldSize.y - 1)));
		}
	}

	// Token: 0x1700094B RID: 2379
	// (get) Token: 0x06008CA9 RID: 36009 RVA: 0x000FBEC1 File Offset: 0x000FA0C1
	public Vector2I WorldSize
	{
		get
		{
			return this.worldSize;
		}
	}

	// Token: 0x1700094C RID: 2380
	// (get) Token: 0x06008CAA RID: 36010 RVA: 0x000FBEC9 File Offset: 0x000FA0C9
	public Vector2I WorldOffset
	{
		get
		{
			return this.worldOffset;
		}
	}

	// Token: 0x1700094D RID: 2381
	// (get) Token: 0x06008CAB RID: 36011 RVA: 0x000FBED1 File Offset: 0x000FA0D1
	public bool FullyEnclosedBorder
	{
		get
		{
			return this.fullyEnclosedBorder;
		}
	}

	// Token: 0x1700094E RID: 2382
	// (get) Token: 0x06008CAC RID: 36012 RVA: 0x000FBED9 File Offset: 0x000FA0D9
	public int Height
	{
		get
		{
			return this.worldSize.y;
		}
	}

	// Token: 0x1700094F RID: 2383
	// (get) Token: 0x06008CAD RID: 36013 RVA: 0x000FBEE6 File Offset: 0x000FA0E6
	public int Width
	{
		get
		{
			return this.worldSize.x;
		}
	}

	// Token: 0x06008CAE RID: 36014 RVA: 0x000FBEF3 File Offset: 0x000FA0F3
	public void SetDiscovered(bool reveal_surface = false)
	{
		if (!this.isDiscovered)
		{
			this.discoveryTimestamp = GameUtil.GetCurrentTimeInCycles();
		}
		this.isDiscovered = true;
		if (reveal_surface)
		{
			this.LookAtSurface();
		}
		Game.Instance.Trigger(-521212405, this);
	}

	// Token: 0x06008CAF RID: 36015 RVA: 0x000FBF28 File Offset: 0x000FA128
	public void SetDupeVisited()
	{
		if (!this.isDupeVisited)
		{
			this.dupeVisitedTimestamp = GameUtil.GetCurrentTimeInCycles();
			this.isDupeVisited = true;
			Game.Instance.Trigger(-434755240, this);
		}
	}

	// Token: 0x06008CB0 RID: 36016 RVA: 0x000FBF54 File Offset: 0x000FA154
	public void SetRoverLanded()
	{
		this.isRoverVisited = true;
	}

	// Token: 0x06008CB1 RID: 36017 RVA: 0x000FBF5D File Offset: 0x000FA15D
	public void SetRocketInteriorWorldDetails(int world_id, Vector2I size, Vector2I offset)
	{
		this.SetID(world_id);
		this.fullyEnclosedBorder = true;
		this.worldOffset = offset;
		this.worldSize = size;
		this.isDiscovered = true;
		this.isModuleInterior = true;
		this.m_seasonIds = new List<string>();
	}

	// Token: 0x06008CB2 RID: 36018 RVA: 0x0036370C File Offset: 0x0036190C
	private static int IsClockwise(Vector2 first, Vector2 second, Vector2 origin)
	{
		if (first == second)
		{
			return 0;
		}
		Vector2 vector = first - origin;
		Vector2 vector2 = second - origin;
		float num = Mathf.Atan2(vector.x, vector.y);
		float num2 = Mathf.Atan2(vector2.x, vector2.y);
		if (num < num2)
		{
			return 1;
		}
		if (num > num2)
		{
			return -1;
		}
		if (vector.sqrMagnitude >= vector2.sqrMagnitude)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x06008CB3 RID: 36019 RVA: 0x00363778 File Offset: 0x00361978
	public void PlaceInteriorTemplate(string template_name, System.Action callback)
	{
		TemplateContainer template = TemplateCache.GetTemplate(template_name);
		Vector2 pos = new Vector2((float)(this.worldSize.x / 2 + this.worldOffset.x), (float)(this.worldSize.y / 2 + this.worldOffset.y));
		TemplateLoader.Stamp(template, pos, callback);
		float val = template.info.size.X / 2f;
		float val2 = template.info.size.Y / 2f;
		float num = Math.Max(val, val2);
		GridVisibility.Reveal((int)pos.x, (int)pos.y, (int)num + 3 + 5, num + 3f);
		WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
		this.overworldCell = new WorldDetailSave.OverworldCell();
		List<Vector2> list = new List<Vector2>(template.cells.Count);
		foreach (Prefab prefab in template.buildings)
		{
			if (prefab.id == "RocketWallTile")
			{
				Vector2 vector = new Vector2((float)prefab.location_x + pos.x, (float)prefab.location_y + pos.y);
				if (vector.x > pos.x)
				{
					vector.x += 0.5f;
				}
				if (vector.y > pos.y)
				{
					vector.y += 0.5f;
				}
				list.Add(vector);
			}
		}
		list.Sort((Vector2 v1, Vector2 v2) => WorldContainer.IsClockwise(v1, v2, pos));
		Polygon polygon = new Polygon(list);
		this.overworldCell.poly = polygon;
		this.overworldCell.zoneType = SubWorld.ZoneType.RocketInterior;
		this.overworldCell.tags = new TagSet
		{
			WorldGenTags.RocketInterior
		};
		clusterDetailSave.overworldCells.Add(this.overworldCell);
		for (int i = 0; i < this.worldSize.y; i++)
		{
			for (int j = 0; j < this.worldSize.x; j++)
			{
				Vector2I vector2I = new Vector2I(this.worldOffset.x + j, this.worldOffset.y + i);
				int num2 = Grid.XYToCell(vector2I.x, vector2I.y);
				if (polygon.Contains(new Vector2((float)vector2I.x, (float)vector2I.y)))
				{
					SimMessages.ModifyCellWorldZone(num2, 14);
					global::World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.RocketInterior;
				}
				else
				{
					SimMessages.ModifyCellWorldZone(num2, 7);
					global::World.Instance.zoneRenderData.worldZoneTypes[num2] = SubWorld.ZoneType.Space;
				}
			}
		}
	}

	// Token: 0x06008CB4 RID: 36020 RVA: 0x000FBF94 File Offset: 0x000FA194
	private int GetDefaultValueForFixedTraitCategory(Dictionary<string, int> traitCategory)
	{
		if (traitCategory == this.northernLightsFixedTraits)
		{
			return FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;
		}
		if (traitCategory == this.sunlightFixedTraits)
		{
			return FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;
		}
		if (traitCategory == this.cosmicRadiationFixedTraits)
		{
			return FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;
		}
		return 0;
	}

	// Token: 0x06008CB5 RID: 36021 RVA: 0x000FBFC4 File Offset: 0x000FA1C4
	private string GetDefaultFixedTraitFor(Dictionary<string, int> traitCategory)
	{
		if (traitCategory == this.northernLightsFixedTraits)
		{
			return FIXEDTRAITS.NORTHERNLIGHTS.NAME.DEFAULT;
		}
		if (traitCategory == this.sunlightFixedTraits)
		{
			return FIXEDTRAITS.SUNLIGHT.NAME.DEFAULT;
		}
		if (traitCategory == this.cosmicRadiationFixedTraits)
		{
			return FIXEDTRAITS.COSMICRADIATION.NAME.DEFAULT;
		}
		return null;
	}

	// Token: 0x06008CB6 RID: 36022 RVA: 0x00363A80 File Offset: 0x00361C80
	private string GetFixedTraitsFor(Dictionary<string, int> traitCategory, WorldGen world)
	{
		foreach (string text in world.Settings.world.fixedTraits)
		{
			if (traitCategory.ContainsKey(text))
			{
				return text;
			}
		}
		return this.GetDefaultFixedTraitFor(traitCategory);
	}

	// Token: 0x06008CB7 RID: 36023 RVA: 0x000FBFF4 File Offset: 0x000FA1F4
	private int GetFixedTraitValueForTrait(Dictionary<string, int> traitCategory, ref string trait)
	{
		if (trait == null)
		{
			trait = this.GetDefaultFixedTraitFor(traitCategory);
		}
		if (traitCategory.ContainsKey(trait))
		{
			return traitCategory[trait];
		}
		return this.GetDefaultValueForFixedTraitCategory(traitCategory);
	}

	// Token: 0x06008CB8 RID: 36024 RVA: 0x000FC01D File Offset: 0x000FA21D
	private string GetNorthernlightFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.northernLightsFixedTraits, world);
	}

	// Token: 0x06008CB9 RID: 36025 RVA: 0x000FC02C File Offset: 0x000FA22C
	private string GetSunlightFromFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.sunlightFixedTraits, world);
	}

	// Token: 0x06008CBA RID: 36026 RVA: 0x000FC03B File Offset: 0x000FA23B
	private string GetCosmicRadiationFromFixedTraits(WorldGen world)
	{
		return this.GetFixedTraitsFor(this.cosmicRadiationFixedTraits, world);
	}

	// Token: 0x06008CBB RID: 36027 RVA: 0x000FC04A File Offset: 0x000FA24A
	private int GetNorthernlightValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.northernLightsFixedTraits, ref this.northernLightFixedTrait);
	}

	// Token: 0x06008CBC RID: 36028 RVA: 0x000FC05E File Offset: 0x000FA25E
	private int GetSunlightValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.sunlightFixedTraits, ref this.sunlightFixedTrait);
	}

	// Token: 0x06008CBD RID: 36029 RVA: 0x000FC072 File Offset: 0x000FA272
	private int GetCosmicRadiationValueFromFixedTrait()
	{
		return this.GetFixedTraitValueForTrait(this.cosmicRadiationFixedTraits, ref this.cosmicRadiationFixedTrait);
	}

	// Token: 0x06008CBE RID: 36030 RVA: 0x00363AEC File Offset: 0x00361CEC
	public void SetWorldDetails(WorldGen world)
	{
		if (world != null)
		{
			this.fullyEnclosedBorder = (world.Settings.GetBoolSetting("DrawWorldBorder") && world.Settings.GetBoolSetting("DrawWorldBorderOverVacuum"));
			this.worldOffset = world.GetPosition();
			this.worldSize = world.GetSize();
			this.isDiscovered = world.isStartingWorld;
			this.isStartWorld = world.isStartingWorld;
			this.worldName = world.Settings.world.filePath;
			this.nameTables = world.Settings.world.nameTables;
			this.worldTags = ((world.Settings.world.worldTags != null) ? world.Settings.world.worldTags.ToArray().ToTagArray() : new Tag[0]);
			this.worldDescription = world.Settings.world.description;
			this.worldType = world.Settings.world.name;
			this.isModuleInterior = world.Settings.world.moduleInterior;
			this.m_seasonIds = new List<string>(world.Settings.world.seasons);
			this.m_generatedSubworlds = world.Settings.world.generatedSubworlds;
			this.northernLightFixedTrait = this.GetNorthernlightFixedTraits(world);
			this.sunlightFixedTrait = this.GetSunlightFromFixedTraits(world);
			this.cosmicRadiationFixedTrait = this.GetCosmicRadiationFromFixedTraits(world);
			this.sunlight = this.GetSunlightValueFromFixedTrait();
			this.northernlights = this.GetNorthernlightValueFromFixedTrait();
			this.cosmicRadiation = this.GetCosmicRadiationValueFromFixedTrait();
			this.currentCosmicIntensity = (float)this.cosmicRadiation;
			HashSet<string> hashSet = new HashSet<string>();
			foreach (string text in world.Settings.world.generatedSubworlds)
			{
				text = text.Substring(0, text.LastIndexOf('/'));
				text = text.Substring(text.LastIndexOf('/') + 1, text.Length - (text.LastIndexOf('/') + 1));
				hashSet.Add(text);
			}
			this.m_subworldNames = hashSet.ToList<string>();
			this.m_worldTraitIds = new List<string>();
			this.m_worldTraitIds.AddRange(world.Settings.GetWorldTraitIDs());
			this.m_storyTraitIds = new List<string>();
			this.m_storyTraitIds.AddRange(world.Settings.GetStoryTraitIDs());
			return;
		}
		this.fullyEnclosedBorder = false;
		this.worldOffset = Vector2I.zero;
		this.worldSize = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
		this.isDiscovered = true;
		this.isStartWorld = true;
		this.isDupeVisited = true;
		this.m_seasonIds = new List<string>
		{
			Db.Get().GameplaySeasons.MeteorShowers.Id
		};
	}

	// Token: 0x06008CBF RID: 36031 RVA: 0x00363DC4 File Offset: 0x00361FC4
	public bool ContainsPoint(Vector2 point)
	{
		return point.x >= (float)this.worldOffset.x && point.y >= (float)this.worldOffset.y && point.x < (float)(this.worldOffset.x + this.worldSize.x) && point.y < (float)(this.worldOffset.y + this.worldSize.y);
	}

	// Token: 0x06008CC0 RID: 36032 RVA: 0x00363E3C File Offset: 0x0036203C
	public void LookAtSurface()
	{
		if (!this.IsDupeVisited)
		{
			this.RevealSurface();
		}
		Vector3? vector = this.SetSurfaceCameraPos();
		if (ClusterManager.Instance.activeWorldId == this.id && vector != null)
		{
			CameraController.Instance.SnapTo(vector.Value);
		}
	}

	// Token: 0x06008CC1 RID: 36033 RVA: 0x00363E8C File Offset: 0x0036208C
	public void RevealSurface()
	{
		if (this.isSurfaceRevealed)
		{
			return;
		}
		this.isSurfaceRevealed = true;
		for (int i = 0; i < this.worldSize.x; i++)
		{
			for (int j = this.worldSize.y - 1; j >= 0; j--)
			{
				int cell = Grid.XYToCell(i + this.worldOffset.x, j + this.worldOffset.y);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell) || Grid.IsLiquid(cell))
				{
					break;
				}
				GridVisibility.Reveal(i + this.worldOffset.X, j + this.worldOffset.y, 7, 1f);
			}
		}
	}

	// Token: 0x06008CC2 RID: 36034 RVA: 0x00363F38 File Offset: 0x00362138
	private Vector3? SetSurfaceCameraPos()
	{
		if (SaveGame.Instance != null)
		{
			int num = (int)this.maximumBounds.y;
			for (int i = 0; i < this.worldSize.X; i++)
			{
				for (int j = this.worldSize.y - 1; j >= 0; j--)
				{
					int num2 = j + this.worldOffset.y;
					int num3 = Grid.XYToCell(i + this.worldOffset.x, num2);
					if (Grid.IsValidCell(num3) && (Grid.Solid[num3] || Grid.IsLiquid(num3)))
					{
						num = Math.Min(num, num2);
						break;
					}
				}
			}
			int num4 = (num + this.worldOffset.y + this.worldSize.y) / 2;
			Vector3 vector = new Vector3((float)(this.WorldOffset.x + this.Width / 2), (float)num4, 0f);
			SaveGame.Instance.GetComponent<UserNavigation>().SetWorldCameraStartPosition(this.id, vector);
			return new Vector3?(vector);
		}
		return null;
	}

	// Token: 0x06008CC3 RID: 36035 RVA: 0x0036404C File Offset: 0x0036224C
	public void EjectAllDupes(Vector3 spawn_pos)
	{
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.GetWorldItems(this.id, false))
		{
			minionIdentity.transform.SetLocalPosition(spawn_pos);
		}
	}

	// Token: 0x06008CC4 RID: 36036 RVA: 0x003640B0 File Offset: 0x003622B0
	public void SpacePodAllDupes(AxialI sourceLocation, SimHashes podElement)
	{
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.GetWorldItems(this.id, false))
		{
			if (!minionIdentity.HasTag(GameTags.Dead))
			{
				Vector3 position = new Vector3(-1f, -1f, 0f);
				GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab("EscapePod"), position);
				gameObject.GetComponent<PrimaryElement>().SetElement(podElement, true);
				gameObject.SetActive(true);
				gameObject.GetComponent<MinionStorage>().SerializeMinion(minionIdentity.gameObject);
				TravellingCargoLander.StatesInstance smi = gameObject.GetSMI<TravellingCargoLander.StatesInstance>();
				smi.StartSM();
				smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
			}
		}
	}

	// Token: 0x06008CC5 RID: 36037 RVA: 0x00364188 File Offset: 0x00362388
	public void DestroyWorldBuildings(out HashSet<int> noRefundTiles)
	{
		this.TransferBuildingMaterials(out noRefundTiles);
		foreach (ClustercraftInteriorDoor cmp in Components.ClusterCraftInteriorDoors.GetWorldItems(this.id, false))
		{
			cmp.DeleteObject();
		}
		this.ClearWorldZones();
	}

	// Token: 0x06008CC6 RID: 36038 RVA: 0x000FC086 File Offset: 0x000FA286
	public void TransferResourcesToParentWorld(Vector3 spawn_pos, HashSet<int> noRefundTiles)
	{
		this.TransferPickupables(spawn_pos);
		this.TransferLiquidsSolidsAndGases(spawn_pos, noRefundTiles);
	}

	// Token: 0x06008CC7 RID: 36039 RVA: 0x003641F0 File Offset: 0x003623F0
	public void TransferResourcesToDebris(AxialI sourceLocation, HashSet<int> noRefundTiles, SimHashes debrisContainerElement)
	{
		List<Storage> list = new List<Storage>();
		this.TransferPickupablesToDebris(ref list, debrisContainerElement);
		this.TransferLiquidsSolidsAndGasesToDebris(ref list, noRefundTiles, debrisContainerElement);
		foreach (Storage cmp in list)
		{
			RailGunPayload.StatesInstance smi = cmp.GetSMI<RailGunPayload.StatesInstance>();
			smi.StartSM();
			smi.Travel(sourceLocation, ClusterUtil.ClosestVisibleAsteroidToLocation(sourceLocation).Location);
		}
	}

	// Token: 0x06008CC8 RID: 36040 RVA: 0x0036426C File Offset: 0x0036246C
	private void TransferBuildingMaterials(out HashSet<int> noRefundTiles)
	{
		HashSet<int> retTemplateFoundationCells = new HashSet<int>();
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.completeBuildings, pooledList);
		Action<int> <>9__0;
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			BuildingComplete buildingComplete = scenePartitionerEntry.obj as BuildingComplete;
			if (buildingComplete != null)
			{
				Deconstructable component = buildingComplete.GetComponent<Deconstructable>();
				if (component != null && !buildingComplete.HasTag(GameTags.NoRocketRefund))
				{
					PrimaryElement component2 = buildingComplete.GetComponent<PrimaryElement>();
					float temperature = component2.Temperature;
					byte diseaseIdx = component2.DiseaseIdx;
					int diseaseCount = component2.DiseaseCount;
					int num = 0;
					while (num < component.constructionElements.Length && buildingComplete.Def.Mass.Length > num)
					{
						Element element = ElementLoader.GetElement(component.constructionElements[num]);
						if (element != null)
						{
							element.substance.SpawnResource(buildingComplete.transform.GetPosition(), buildingComplete.Def.Mass[num], temperature, diseaseIdx, diseaseCount, false, false, false);
						}
						else
						{
							GameObject prefab = Assets.GetPrefab(component.constructionElements[num]);
							int num2 = 0;
							while ((float)num2 < buildingComplete.Def.Mass[num])
							{
								GameUtil.KInstantiate(prefab, buildingComplete.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
								num2++;
							}
						}
						num++;
					}
				}
				SimCellOccupier component3 = buildingComplete.GetComponent<SimCellOccupier>();
				if (component3 != null && component3.doReplaceElement)
				{
					Building building = buildingComplete;
					Action<int> callback;
					if ((callback = <>9__0) == null)
					{
						callback = (<>9__0 = delegate(int cell)
						{
							retTemplateFoundationCells.Add(cell);
						});
					}
					building.RunOnArea(callback);
				}
				Storage component4 = buildingComplete.GetComponent<Storage>();
				if (component4 != null)
				{
					component4.DropAll(false, false, default(Vector3), true, null);
				}
				PlantablePlot component5 = buildingComplete.GetComponent<PlantablePlot>();
				if (component5 != null)
				{
					SeedProducer seedProducer = (component5.Occupant != null) ? component5.Occupant.GetComponent<SeedProducer>() : null;
					if (seedProducer != null)
					{
						seedProducer.DropSeed(null);
					}
				}
				buildingComplete.DeleteObject();
			}
		}
		pooledList.Clear();
		noRefundTiles = retTemplateFoundationCells;
	}

	// Token: 0x06008CC9 RID: 36041 RVA: 0x003644FC File Offset: 0x003626FC
	private void TransferPickupables(Vector3 pos)
	{
		int cell = Grid.PosToCell(pos);
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			if (scenePartitionerEntry.obj != null)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable != null)
				{
					pickupable.gameObject.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06008CCA RID: 36042 RVA: 0x003645C8 File Offset: 0x003627C8
	private void TransferLiquidsSolidsAndGases(Vector3 pos, HashSet<int> noRefundTiles)
	{
		int num = (int)this.minimumBounds.x;
		while ((float)num <= this.maximumBounds.x)
		{
			int num2 = (int)this.minimumBounds.y;
			while ((float)num2 <= this.maximumBounds.y)
			{
				int num3 = Grid.XYToCell(num, num2);
				if (!noRefundTiles.Contains(num3))
				{
					Element element = Grid.Element[num3];
					if (element != null && !element.IsVacuum)
					{
						element.substance.SpawnResource(pos, Grid.Mass[num3], Grid.Temperature[num3], Grid.DiseaseIdx[num3], Grid.DiseaseCount[num3], false, false, false);
					}
				}
				num2++;
			}
			num++;
		}
	}

	// Token: 0x06008CCB RID: 36043 RVA: 0x00364680 File Offset: 0x00362880
	private void TransferPickupablesToDebris(ref List<Storage> debrisObjects, SimHashes debrisContainerElement)
	{
		ListPool<ScenePartitionerEntry, ClusterManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, ClusterManager>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)this.minimumBounds.x, (int)this.minimumBounds.y, this.Width, this.Height, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			if (scenePartitionerEntry.obj != null)
			{
				Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
				if (pickupable != null)
				{
					if (pickupable.KPrefabID.HasTag(GameTags.BaseMinion))
					{
						global::Util.KDestroyGameObject(pickupable.gameObject);
					}
					else
					{
						pickupable.PrimaryElement.Units = (float)Mathf.Max(1, Mathf.RoundToInt(pickupable.PrimaryElement.Units * 0.5f));
						if ((debrisObjects.Count == 0 || debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) && pickupable.PrimaryElement.Mass > 0f)
						{
							debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement));
						}
						Storage storage = debrisObjects[debrisObjects.Count - 1];
						while (pickupable.PrimaryElement.Mass > storage.RemainingCapacity())
						{
							Pickupable pickupable2 = pickupable.Take(storage.RemainingCapacity());
							storage.Store(pickupable2.gameObject, false, false, true, false);
							storage = CraftModuleInterface.SpawnRocketDebris(" from World Objects", debrisContainerElement);
							debrisObjects.Add(storage);
						}
						if (pickupable.PrimaryElement.Mass > 0f)
						{
							storage.Store(pickupable.gameObject, false, false, true, false);
						}
					}
				}
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06008CCC RID: 36044 RVA: 0x00364858 File Offset: 0x00362A58
	private void TransferLiquidsSolidsAndGasesToDebris(ref List<Storage> debrisObjects, HashSet<int> noRefundTiles, SimHashes debrisContainerElement)
	{
		int num = (int)this.minimumBounds.x;
		while ((float)num <= this.maximumBounds.x)
		{
			int num2 = (int)this.minimumBounds.y;
			while ((float)num2 <= this.maximumBounds.y)
			{
				int num3 = Grid.XYToCell(num, num2);
				if (!noRefundTiles.Contains(num3))
				{
					Element element = Grid.Element[num3];
					if (element != null && !element.IsVacuum)
					{
						float num4 = Grid.Mass[num3];
						num4 *= 0.5f;
						if ((debrisObjects.Count == 0 || debrisObjects[debrisObjects.Count - 1].RemainingCapacity() == 0f) && num4 > 0f)
						{
							debrisObjects.Add(CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement));
						}
						Storage storage = debrisObjects[debrisObjects.Count - 1];
						while (num4 > 0f)
						{
							float num5 = Mathf.Min(num4, storage.RemainingCapacity());
							num4 -= num5;
							storage.AddOre(element.id, num5, Grid.Temperature[num3], Grid.DiseaseIdx[num3], Grid.DiseaseCount[num3], false, true);
							if (num4 > 0f)
							{
								storage = CraftModuleInterface.SpawnRocketDebris(" from World Tiles", debrisContainerElement);
								debrisObjects.Add(storage);
							}
						}
					}
				}
				num2++;
			}
			num++;
		}
	}

	// Token: 0x06008CCD RID: 36045 RVA: 0x003649C0 File Offset: 0x00362BC0
	public void CancelChores()
	{
		for (int i = 0; i < 45; i++)
		{
			int num = (int)this.minimumBounds.x;
			while ((float)num <= this.maximumBounds.x)
			{
				int num2 = (int)this.minimumBounds.y;
				while ((float)num2 <= this.maximumBounds.y)
				{
					int cell = Grid.XYToCell(num, num2);
					GameObject gameObject = Grid.Objects[cell, i];
					if (gameObject != null)
					{
						gameObject.Trigger(2127324410, true);
					}
					num2++;
				}
				num++;
			}
		}
		List<Chore> list;
		GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(this.id, out list);
		int num3 = 0;
		while (list != null && num3 < list.Count)
		{
			Chore chore = list[num3];
			if (chore != null && chore.target != null && !chore.isNull)
			{
				chore.Cancel("World destroyed");
			}
			num3++;
		}
		List<FetchChore> list2;
		GlobalChoreProvider.Instance.fetchMap.TryGetValue(this.id, out list2);
		int num4 = 0;
		while (list2 != null && num4 < list2.Count)
		{
			FetchChore fetchChore = list2[num4];
			if (fetchChore != null && fetchChore.target != null && !fetchChore.isNull)
			{
				fetchChore.Cancel("World destroyed");
			}
			num4++;
		}
	}

	// Token: 0x06008CCE RID: 36046 RVA: 0x00364B18 File Offset: 0x00362D18
	public void ClearWorldZones()
	{
		if (this.overworldCell != null)
		{
			WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
			int num = -1;
			for (int i = 0; i < SaveLoader.Instance.clusterDetailSave.overworldCells.Count; i++)
			{
				WorldDetailSave.OverworldCell overworldCell = SaveLoader.Instance.clusterDetailSave.overworldCells[i];
				if (overworldCell.zoneType == this.overworldCell.zoneType && overworldCell.tags != null && this.overworldCell.tags != null && overworldCell.tags.ContainsAll(this.overworldCell.tags) && overworldCell.poly.bounds == this.overworldCell.poly.bounds)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				clusterDetailSave.overworldCells.RemoveAt(num);
			}
		}
		int num2 = (int)this.minimumBounds.y;
		while ((float)num2 <= this.maximumBounds.y)
		{
			int num3 = (int)this.minimumBounds.x;
			while ((float)num3 <= this.maximumBounds.x)
			{
				SimMessages.ModifyCellWorldZone(Grid.XYToCell(num3, num2), byte.MaxValue);
				num3++;
			}
			num2++;
		}
	}

	// Token: 0x06008CCF RID: 36047 RVA: 0x00364C50 File Offset: 0x00362E50
	public int GetSafeCell()
	{
		if (this.IsModuleInterior)
		{
			using (List<RocketControlStation>.Enumerator enumerator = Components.RocketControlStations.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RocketControlStation rocketControlStation = enumerator.Current;
					if (rocketControlStation.GetMyWorldId() == this.id)
					{
						return Grid.PosToCell(rocketControlStation);
					}
				}
				goto IL_A2;
			}
		}
		foreach (Telepad telepad in Components.Telepads.Items)
		{
			if (telepad.GetMyWorldId() == this.id)
			{
				return Grid.PosToCell(telepad);
			}
		}
		IL_A2:
		return Grid.XYToCell(this.worldOffset.x + this.worldSize.x / 2, this.worldOffset.y + this.worldSize.y / 2);
	}

	// Token: 0x06008CD0 RID: 36048 RVA: 0x000FC097 File Offset: 0x000FA297
	public string GetStatus()
	{
		return ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResultStatus(this.id);
	}

	// Token: 0x040069B0 RID: 27056
	[Serialize]
	public int id = -1;

	// Token: 0x040069B1 RID: 27057
	[Serialize]
	public Tag prefabTag;

	// Token: 0x040069B4 RID: 27060
	[Serialize]
	private Vector2I worldOffset;

	// Token: 0x040069B5 RID: 27061
	[Serialize]
	private Vector2I worldSize;

	// Token: 0x040069B6 RID: 27062
	[Serialize]
	private bool fullyEnclosedBorder;

	// Token: 0x040069B7 RID: 27063
	[Serialize]
	private bool isModuleInterior;

	// Token: 0x040069B8 RID: 27064
	[Serialize]
	private WorldDetailSave.OverworldCell overworldCell;

	// Token: 0x040069B9 RID: 27065
	[Serialize]
	private bool isDiscovered;

	// Token: 0x040069BA RID: 27066
	[Serialize]
	private bool isStartWorld;

	// Token: 0x040069BB RID: 27067
	[Serialize]
	private bool isDupeVisited;

	// Token: 0x040069BC RID: 27068
	[Serialize]
	private float dupeVisitedTimestamp = -1f;

	// Token: 0x040069BD RID: 27069
	[Serialize]
	private float discoveryTimestamp = -1f;

	// Token: 0x040069BE RID: 27070
	[Serialize]
	private bool isRoverVisited;

	// Token: 0x040069BF RID: 27071
	[Serialize]
	private bool isSurfaceRevealed;

	// Token: 0x040069C0 RID: 27072
	[Serialize]
	public string worldName;

	// Token: 0x040069C1 RID: 27073
	[Serialize]
	public string[] nameTables;

	// Token: 0x040069C2 RID: 27074
	[Serialize]
	public Tag[] worldTags;

	// Token: 0x040069C3 RID: 27075
	[Serialize]
	public string overrideName;

	// Token: 0x040069C4 RID: 27076
	[Serialize]
	public string worldType;

	// Token: 0x040069C5 RID: 27077
	[Serialize]
	public string worldDescription;

	// Token: 0x040069C6 RID: 27078
	[Serialize]
	public int northernlights = FIXEDTRAITS.NORTHERNLIGHTS.DEFAULT_VALUE;

	// Token: 0x040069C7 RID: 27079
	[Serialize]
	public int sunlight = FIXEDTRAITS.SUNLIGHT.DEFAULT_VALUE;

	// Token: 0x040069C8 RID: 27080
	[Serialize]
	public int cosmicRadiation = FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

	// Token: 0x040069C9 RID: 27081
	[Serialize]
	public float currentSunlightIntensity;

	// Token: 0x040069CA RID: 27082
	[Serialize]
	public float currentCosmicIntensity = (float)FIXEDTRAITS.COSMICRADIATION.DEFAULT_VALUE;

	// Token: 0x040069CB RID: 27083
	[Serialize]
	public string sunlightFixedTrait;

	// Token: 0x040069CC RID: 27084
	[Serialize]
	public string cosmicRadiationFixedTrait;

	// Token: 0x040069CD RID: 27085
	[Serialize]
	public string northernLightFixedTrait;

	// Token: 0x040069CE RID: 27086
	[Serialize]
	public int fixedTraitsUpdateVersion = 1;

	// Token: 0x040069CF RID: 27087
	private Dictionary<string, int> sunlightFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.SUNLIGHT.NAME.NONE,
			FIXEDTRAITS.SUNLIGHT.NONE
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_LOW,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_LOW,
			FIXEDTRAITS.SUNLIGHT.VERY_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.LOW,
			FIXEDTRAITS.SUNLIGHT.LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED_LOW,
			FIXEDTRAITS.SUNLIGHT.MED_LOW
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED,
			FIXEDTRAITS.SUNLIGHT.MED
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.MED_HIGH,
			FIXEDTRAITS.SUNLIGHT.MED_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.HIGH,
			FIXEDTRAITS.SUNLIGHT.HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_HIGH
		},
		{
			FIXEDTRAITS.SUNLIGHT.NAME.VERY_VERY_VERY_HIGH,
			FIXEDTRAITS.SUNLIGHT.VERY_VERY_VERY_HIGH
		}
	};

	// Token: 0x040069D0 RID: 27088
	private Dictionary<string, int> northernLightsFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.NORTHERNLIGHTS.NAME.NONE,
			FIXEDTRAITS.NORTHERNLIGHTS.NONE
		},
		{
			FIXEDTRAITS.NORTHERNLIGHTS.NAME.ENABLED,
			FIXEDTRAITS.NORTHERNLIGHTS.ENABLED
		}
	};

	// Token: 0x040069D1 RID: 27089
	private Dictionary<string, int> cosmicRadiationFixedTraits = new Dictionary<string, int>
	{
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.NONE,
			FIXEDTRAITS.COSMICRADIATION.NONE
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_LOW,
			FIXEDTRAITS.COSMICRADIATION.VERY_VERY_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_LOW,
			FIXEDTRAITS.COSMICRADIATION.VERY_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.LOW,
			FIXEDTRAITS.COSMICRADIATION.LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED_LOW,
			FIXEDTRAITS.COSMICRADIATION.MED_LOW
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED,
			FIXEDTRAITS.COSMICRADIATION.MED
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.MED_HIGH,
			FIXEDTRAITS.COSMICRADIATION.MED_HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.HIGH,
			FIXEDTRAITS.COSMICRADIATION.HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_HIGH,
			FIXEDTRAITS.COSMICRADIATION.VERY_HIGH
		},
		{
			FIXEDTRAITS.COSMICRADIATION.NAME.VERY_VERY_HIGH,
			FIXEDTRAITS.COSMICRADIATION.VERY_VERY_HIGH
		}
	};

	// Token: 0x040069D2 RID: 27090
	[Serialize]
	private List<string> m_seasonIds;

	// Token: 0x040069D3 RID: 27091
	[Serialize]
	private List<string> m_subworldNames;

	// Token: 0x040069D4 RID: 27092
	[Serialize]
	private List<string> m_worldTraitIds;

	// Token: 0x040069D5 RID: 27093
	[Serialize]
	private List<string> m_storyTraitIds;

	// Token: 0x040069D6 RID: 27094
	[Serialize]
	private List<string> m_generatedSubworlds;

	// Token: 0x040069D7 RID: 27095
	private WorldParentChangedEventArgs parentChangeArgs = new WorldParentChangedEventArgs();

	// Token: 0x040069D8 RID: 27096
	[MySmiReq]
	private AlertStateManager.Instance m_alertManager;

	// Token: 0x040069D9 RID: 27097
	private List<Prioritizable> yellowAlertTasks = new List<Prioritizable>();

	// Token: 0x040069DB RID: 27099
	private List<int> m_childWorlds = new List<int>();
}
