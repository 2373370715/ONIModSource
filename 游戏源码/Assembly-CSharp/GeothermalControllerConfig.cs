using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000369 RID: 873
public class GeothermalControllerConfig : IEntityConfig
{
	// Token: 0x06000E27 RID: 3623 RVA: 0x00176E74 File Offset: 0x00175074
	public static List<GeothermalVent.ElementInfo> GetClearingEntombedVentReward()
	{
		return new List<GeothermalVent.ElementInfo>
		{
			new GeothermalVent.ElementInfo
			{
				isSolid = false,
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Steam).idx,
				mass = 100f,
				temperature = 1102f,
				diseaseIdx = byte.MaxValue,
				diseaseCount = 0
			},
			new GeothermalVent.ElementInfo
			{
				isSolid = true,
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Lead).idx,
				mass = 144f,
				temperature = 502f,
				diseaseIdx = byte.MaxValue,
				diseaseCount = 0
			}
		};
	}

	// Token: 0x06000E28 RID: 3624 RVA: 0x00176F38 File Offset: 0x00175138
	public static List<GeothermalControllerConfig.Impurity> GetImpurities()
	{
		return new List<GeothermalControllerConfig.Impurity>
		{
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.IgneousRock).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Granite).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Obsidian).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.SaltWater).idx,
				mass_kg = 320f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.DirtyWater).idx,
				mass_kg = 400f,
				required_temp_range = new MathUtil.MinMax(0f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Rust).idx,
				mass_kg = 125f,
				required_temp_range = new MathUtil.MinMax(330f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenLead).idx,
				mass_kg = 65f,
				required_temp_range = new MathUtil.MinMax(540f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.SulfurGas).idx,
				mass_kg = 30f,
				required_temp_range = new MathUtil.MinMax(700f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.SourGas).idx,
				mass_kg = 200f,
				required_temp_range = new MathUtil.MinMax(800f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.IronOre).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(850f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenAluminum).idx,
				mass_kg = 100f,
				required_temp_range = new MathUtil.MinMax(1200f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenCopper).idx,
				mass_kg = 100f,
				required_temp_range = new MathUtil.MinMax(1300f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenGold).idx,
				mass_kg = 100f,
				required_temp_range = new MathUtil.MinMax(1400f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Magma).idx,
				mass_kg = 75f,
				required_temp_range = new MathUtil.MinMax(1800f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Hydrogen).idx,
				mass_kg = 50f,
				required_temp_range = new MathUtil.MinMax(1800f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.MoltenIron).idx,
				mass_kg = 250f,
				required_temp_range = new MathUtil.MinMax(1900f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Wolframite).idx,
				mass_kg = 275f,
				required_temp_range = new MathUtil.MinMax(2000f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Fullerene).idx,
				mass_kg = 3f,
				required_temp_range = new MathUtil.MinMax(2500f, float.MaxValue)
			},
			new GeothermalControllerConfig.Impurity
			{
				elementIdx = ElementLoader.FindElementByHash(SimHashes.Niobium).idx,
				mass_kg = 5f,
				required_temp_range = new MathUtil.MinMax(2500f, float.MaxValue)
			}
		};
	}

	// Token: 0x06000E29 RID: 3625 RVA: 0x000AC4E8 File Offset: 0x000AA6E8
	public static float CalculateOutputTemperature(float inputTemperature)
	{
		if (inputTemperature < 1650f)
		{
			return Math.Min(1650f, inputTemperature + 150f);
		}
		return Math.Max(1650f, inputTemperature - 150f);
	}

	// Token: 0x06000E2A RID: 3626 RVA: 0x000A9B1E File Offset: 0x000A7D1E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_DLC_2;
	}

	// Token: 0x06000E2B RID: 3627 RVA: 0x00177490 File Offset: 0x00175690
	GameObject IEntityConfig.CreatePrefab()
	{
		string id = "GeothermalControllerEntity";
		string name = STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.EFFECT + "\n\n" + STRINGS.BUILDINGS.PREFABS.GEOTHERMALCONTROLLER.DESC;
		float mass = 100f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.PENALTY.TIER4;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_geoplant_kanim"), "off", Grid.SceneLayer.BuildingBack, 7, 8, tier, tier2, SimHashes.Unobtanium, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		gameObject.AddComponent<EntityCellVisualizer>();
		gameObject.AddComponent<GeothermalController>();
		gameObject.AddComponent<GeothermalPlantComponent>();
		gameObject.AddComponent<Operational>();
		gameObject.AddComponent<GeothermalController.ReconnectPipes>();
		gameObject.AddComponent<Notifier>();
		Storage storage = gameObject.AddComponent<Storage>();
		storage.showDescriptor = false;
		storage.showInUI = false;
		storage.capacityKg = 12000f;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate,
			Storage.StoredItemModifier.Seal
		});
		return gameObject;
	}

	// Token: 0x06000E2C RID: 3628 RVA: 0x000A5E40 File Offset: 0x000A4040
	void IEntityConfig.OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000E2D RID: 3629 RVA: 0x000A5E40 File Offset: 0x000A4040
	void IEntityConfig.OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000A16 RID: 2582
	public const string ID = "GeothermalControllerEntity";

	// Token: 0x04000A17 RID: 2583
	public const string KEEPSAKE_ID = "keepsake_geothermalplant";

	// Token: 0x04000A18 RID: 2584
	public const string COMPLETED_LORE_ENTRY_UNLOCK_ID = "notes_earthquake";

	// Token: 0x04000A19 RID: 2585
	private const string ANIM_FILE = "gravitas_geoplant_kanim";

	// Token: 0x04000A1A RID: 2586
	public const string OFFLINE_ANIM = "off";

	// Token: 0x04000A1B RID: 2587
	public const string ONLINE_ANIM = "on";

	// Token: 0x04000A1C RID: 2588
	public const string OBSTRUCTED_ANIM = "on";

	// Token: 0x04000A1D RID: 2589
	public const float WORKING_LOOP_DURATION_SECONDS = 16f;

	// Token: 0x04000A1E RID: 2590
	public const float HEATPUMP_CAPACITY_KG = 12000f;

	// Token: 0x04000A1F RID: 2591
	public const float OUTPUT_TARGET_TEMPERATURE = 1650f;

	// Token: 0x04000A20 RID: 2592
	public const float OUTPUT_DELTA_TEMPERATURE = 150f;

	// Token: 0x04000A21 RID: 2593
	public const float OUTPUT_PASSTHROUGH_RATIO = 0.92f;

	// Token: 0x04000A22 RID: 2594
	public static MathUtil.MinMax OUTPUT_VENT_WEIGHT_RANGE = new MathUtil.MinMax(43f, 57f);

	// Token: 0x04000A23 RID: 2595
	public static HashSet<Tag> STEEL_FETCH_TAGS = new HashSet<Tag>
	{
		GameTags.Steel
	};

	// Token: 0x04000A24 RID: 2596
	public const float STEEL_FETCH_QUANTITY_KG = 1200f;

	// Token: 0x04000A25 RID: 2597
	public const float RECONNECT_PUMP_CHORE_DURATION_SECONDS = 5f;

	// Token: 0x04000A26 RID: 2598
	public static HashedString RECONNECT_PUMP_ANIM_OVERRIDE = "anim_use_remote_kanim";

	// Token: 0x04000A27 RID: 2599
	public const string BAROMETER_ANIM = "meter";

	// Token: 0x04000A28 RID: 2600
	public const string BAROMETER_TARGET = "meter_target";

	// Token: 0x04000A29 RID: 2601
	public static string[] BAROMETER_SYMBOLS = new string[]
	{
		"meter_target"
	};

	// Token: 0x04000A2A RID: 2602
	public const string THERMOMETER_ANIM = "meter_temp";

	// Token: 0x04000A2B RID: 2603
	public const string THERMOMETER_TARGET = "meter_target";

	// Token: 0x04000A2C RID: 2604
	public static string[] THERMOMETER_SYMBOLS = new string[]
	{
		"meter_target"
	};

	// Token: 0x04000A2D RID: 2605
	public const float THERMOMETER_MIN_TEMP = 50f;

	// Token: 0x04000A2E RID: 2606
	public const float THERMOMETER_RANGE = 2450f;

	// Token: 0x04000A2F RID: 2607
	public static HashedString[] PRESSURE_ANIM_LOOPS = new HashedString[]
	{
		"pressure_loop",
		"high_pressure_loop",
		"high_pressure_loop2"
	};

	// Token: 0x04000A30 RID: 2608
	public static float[] PRESSURE_ANIM_THRESHOLDS = new float[]
	{
		0f,
		0.35f,
		0.85f
	};

	// Token: 0x04000A31 RID: 2609
	public const float CLEAR_ENTOMBED_VENT_THRESHOLD_TEMPERATURE = 602f;

	// Token: 0x0200036A RID: 874
	public struct Impurity
	{
		// Token: 0x04000A32 RID: 2610
		public ushort elementIdx;

		// Token: 0x04000A33 RID: 2611
		public float mass_kg;

		// Token: 0x04000A34 RID: 2612
		public MathUtil.MinMax required_temp_range;
	}
}
