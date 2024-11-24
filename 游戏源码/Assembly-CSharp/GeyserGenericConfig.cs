using System;
using System.Collections.Generic;
using Klei;
using TUNING;
using UnityEngine;

// Token: 0x020002A0 RID: 672
public class GeyserGenericConfig : IMultiEntityConfig
{
	// Token: 0x06000A17 RID: 2583 RVA: 0x0016911C File Offset: 0x0016731C
	public List<GameObject> CreatePrefabs()
	{
		List<GameObject> list = new List<GameObject>();
		List<GeyserGenericConfig.GeyserPrefabParams> configs = this.GenerateConfigs();
		foreach (GeyserGenericConfig.GeyserPrefabParams geyserPrefabParams in configs)
		{
			list.Add(GeyserGenericConfig.CreateGeyser(geyserPrefabParams.id, geyserPrefabParams.anim, geyserPrefabParams.width, geyserPrefabParams.height, Strings.Get(geyserPrefabParams.nameStringKey), Strings.Get(geyserPrefabParams.descStringKey), geyserPrefabParams.geyserType.idHash, geyserPrefabParams.geyserType.geyserTemperature));
		}
		configs.RemoveAll((GeyserGenericConfig.GeyserPrefabParams x) => !x.isGenericGeyser);
		GameObject gameObject = EntityTemplates.CreateEntity("GeyserGeneric", "Random Geyser Spawner", true);
		gameObject.AddOrGet<SaveLoadRoot>();
		gameObject.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			int num = 0;
			if (SaveLoader.Instance.clusterDetailSave != null)
			{
				num = SaveLoader.Instance.clusterDetailSave.globalWorldSeed;
			}
			else
			{
				global::Debug.LogWarning("Could not load global world seed for geysers");
			}
			num = num + (int)inst.transform.GetPosition().x + (int)inst.transform.GetPosition().y;
			int index = new KRandom(num).Next(0, configs.Count);
			GameUtil.KInstantiate(Assets.GetPrefab(configs[index].id), inst.transform.GetPosition(), Grid.SceneLayer.BuildingBack, null, 0).SetActive(true);
			inst.DeleteObject();
		};
		list.Add(gameObject);
		return list;
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x00169244 File Offset: 0x00167444
	public static GameObject CreateGeyser(string id, string anim, int width, int height, string name, string desc, HashedString presetType, float geyserTemperature)
	{
		float mass = 2000f;
		EffectorValues tier = BUILDINGS.DECOR.BONUS.TIER1;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER6;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim(anim), "inactive", Grid.SceneLayer.BuildingBack, width, height, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.GeyserFeature
		}, 293f);
		gameObject.AddOrGet<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Katairite, true);
		component.Temperature = geyserTemperature;
		gameObject.AddOrGet<Prioritizable>();
		gameObject.AddOrGet<Uncoverable>();
		gameObject.AddOrGet<Geyser>().outputOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<GeyserConfigurator>().presetType = presetType;
		Studyable studyable = gameObject.AddOrGet<Studyable>();
		studyable.meterTrackerSymbol = "geotracker_target";
		studyable.meterAnim = "tracker";
		gameObject.AddOrGet<LoopingSounds>();
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_shake_LP", NOISE_POLLUTION.NOISY.TIER5);
		SoundEventVolumeCache.instance.AddVolume("geyser_side_steam_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER6);
		return gameObject;
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00169354 File Offset: 0x00167554
	private List<GeyserGenericConfig.GeyserPrefabParams> GenerateConfigs()
	{
		List<GeyserGenericConfig.GeyserPrefabParams> list = new List<GeyserGenericConfig.GeyserPrefabParams>();
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_kanim", 2, 4, new GeyserConfigurator.GeyserType("steam", SimHashes.Steam, GeyserConfigurator.GeyserShape.Gas, 383.15f, 1000f, 2000f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_steam_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_steam", SimHashes.Steam, GeyserConfigurator.GeyserShape.Gas, 773.15f, 500f, 1000f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_hot_kanim", 4, 2, new GeyserConfigurator.GeyserType("hot_water", SimHashes.Water, GeyserConfigurator.GeyserShape.Liquid, 368.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_slush_kanim", 4, 2, new GeyserConfigurator.GeyserType("slush_water", SimHashes.DirtyWater, GeyserConfigurator.GeyserShape.Liquid, 263.15f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 263f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_water_filthy_kanim", 4, 2, new GeyserConfigurator.GeyserType("filthy_water", SimHashes.DirtyWater, GeyserConfigurator.GeyserShape.Liquid, 303.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "").AddDisease(new SimUtil.DiseaseInfo
		{
			idx = Db.Get().Diseases.GetIndex("FoodPoisoning"),
			count = 20000
		}), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_cool_slush_kanim", 4, 2, new GeyserConfigurator.GeyserType("slush_salt_water", SimHashes.Brine, GeyserConfigurator.GeyserShape.Liquid, 263.15f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 263f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_salt_water_kanim", 4, 2, new GeyserConfigurator.GeyserType("salt_water", SimHashes.SaltWater, GeyserConfigurator.GeyserShape.Liquid, 368.15f, 2000f, 4000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_small_kanim", 3, 3, new GeyserConfigurator.GeyserType("small_volcano", SimHashes.Magma, GeyserConfigurator.GeyserShape.Molten, 2000f, 400f, 800f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_volcano_big_kanim", 3, 3, new GeyserConfigurator.GeyserType("big_volcano", SimHashes.Magma, GeyserConfigurator.GeyserShape.Molten, 2000f, 800f, 1600f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_co2_kanim", 4, 2, new GeyserConfigurator.GeyserType("liquid_co2", SimHashes.LiquidCarbonDioxide, GeyserConfigurator.GeyserShape.Liquid, 218f, 100f, 200f, 50f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 218f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_co2_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_co2", SimHashes.CarbonDioxide, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_hydrogen_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_hydrogen", SimHashes.Hydrogen, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_po2_hot_kanim", 2, 4, new GeyserConfigurator.GeyserType("hot_po2", SimHashes.ContaminatedOxygen, GeyserConfigurator.GeyserShape.Gas, 773.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_po2_slimy_kanim", 2, 4, new GeyserConfigurator.GeyserType("slimy_po2", SimHashes.ContaminatedOxygen, GeyserConfigurator.GeyserShape.Gas, 333.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "").AddDisease(new SimUtil.DiseaseInfo
		{
			idx = Db.Get().Diseases.GetIndex("SlimeLung"),
			count = 5000
		}), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_chlorine_kanim", 2, 4, new GeyserConfigurator.GeyserType("chlorine_gas", SimHashes.ChlorineGas, GeyserConfigurator.GeyserShape.Gas, 333.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_gas_methane_kanim", 2, 4, new GeyserConfigurator.GeyserType("methane", SimHashes.Methane, GeyserConfigurator.GeyserShape.Gas, 423.15f, 70f, 140f, 5f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_copper_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_copper", SimHashes.MoltenCopper, GeyserConfigurator.GeyserShape.Molten, 2500f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_iron_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_iron", SimHashes.MoltenIron, GeyserConfigurator.GeyserShape.Molten, 2800f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_gold_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_gold", SimHashes.MoltenGold, GeyserConfigurator.GeyserShape.Molten, 2900f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_aluminum_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_aluminum", SimHashes.MoltenAluminum, GeyserConfigurator.GeyserShape.Molten, 2000f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_tungsten_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_tungsten", SimHashes.MoltenTungsten, GeyserConfigurator.GeyserShape.Molten, 4000f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), false));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_niobium_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_niobium", SimHashes.MoltenNiobium, GeyserConfigurator.GeyserShape.Molten, 3500f, 800f, 1600f, 150f, 6000f, 12000f, 0.005f, 0.01f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), false));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_molten_cobalt_kanim", 3, 3, new GeyserConfigurator.GeyserType("molten_cobalt", SimHashes.MoltenCobalt, GeyserConfigurator.GeyserShape.Molten, 2500f, 200f, 400f, 150f, 480f, 1080f, 0.016666668f, 0.1f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_oil_kanim", 4, 2, new GeyserConfigurator.GeyserType("oil_drip", SimHashes.CrudeOil, GeyserConfigurator.GeyserShape.Liquid, 600f, 1f, 250f, 50f, 600f, 600f, 1f, 1f, 100f, 500f, 0.4f, 0.8f, 372.15f, ""), true));
		list.Add(new GeyserGenericConfig.GeyserPrefabParams("geyser_liquid_sulfur_kanim", 4, 2, new GeyserConfigurator.GeyserType("liquid_sulfur", SimHashes.LiquidSulfur, GeyserConfigurator.GeyserShape.Liquid, 438.34998f, 1000f, 2000f, 500f, 60f, 1140f, 0.1f, 0.9f, 15000f, 135000f, 0.4f, 0.8f, 372.15f, "EXPANSION1_ID"), true));
		list.RemoveAll((GeyserGenericConfig.GeyserPrefabParams geyser) => !geyser.geyserType.DlcID.IsNullOrWhiteSpace() && !DlcManager.IsContentSubscribed(geyser.geyserType.DlcID));
		return list;
	}

	// Token: 0x0400078B RID: 1931
	public const string ID = "GeyserGeneric";

	// Token: 0x0400078C RID: 1932
	public const string Steam = "steam";

	// Token: 0x0400078D RID: 1933
	public const string HotSteam = "hot_steam";

	// Token: 0x0400078E RID: 1934
	public const string HotWater = "hot_water";

	// Token: 0x0400078F RID: 1935
	public const string SlushWater = "slush_water";

	// Token: 0x04000790 RID: 1936
	public const string FilthyWater = "filthy_water";

	// Token: 0x04000791 RID: 1937
	public const string SlushSaltWater = "slush_salt_water";

	// Token: 0x04000792 RID: 1938
	public const string SaltWater = "salt_water";

	// Token: 0x04000793 RID: 1939
	public const string SmallVolcano = "small_volcano";

	// Token: 0x04000794 RID: 1940
	public const string BigVolcano = "big_volcano";

	// Token: 0x04000795 RID: 1941
	public const string LiquidCO2 = "liquid_co2";

	// Token: 0x04000796 RID: 1942
	public const string HotCO2 = "hot_co2";

	// Token: 0x04000797 RID: 1943
	public const string HotHydrogen = "hot_hydrogen";

	// Token: 0x04000798 RID: 1944
	public const string HotPO2 = "hot_po2";

	// Token: 0x04000799 RID: 1945
	public const string SlimyPO2 = "slimy_po2";

	// Token: 0x0400079A RID: 1946
	public const string ChlorineGas = "chlorine_gas";

	// Token: 0x0400079B RID: 1947
	public const string Methane = "methane";

	// Token: 0x0400079C RID: 1948
	public const string MoltenCopper = "molten_copper";

	// Token: 0x0400079D RID: 1949
	public const string MoltenIron = "molten_iron";

	// Token: 0x0400079E RID: 1950
	public const string MoltenGold = "molten_gold";

	// Token: 0x0400079F RID: 1951
	public const string MoltenAluminum = "molten_aluminum";

	// Token: 0x040007A0 RID: 1952
	public const string MoltenTungsten = "molten_tungsten";

	// Token: 0x040007A1 RID: 1953
	public const string MoltenNiobium = "molten_niobium";

	// Token: 0x040007A2 RID: 1954
	public const string MoltenCobalt = "molten_cobalt";

	// Token: 0x040007A3 RID: 1955
	public const string OilDrip = "oil_drip";

	// Token: 0x040007A4 RID: 1956
	public const string LiquidSulfur = "liquid_sulfur";

	// Token: 0x020002A1 RID: 673
	public struct GeyserPrefabParams
	{
		// Token: 0x06000A1D RID: 2589 RVA: 0x00169E44 File Offset: 0x00168044
		public GeyserPrefabParams(string anim, int width, int height, GeyserConfigurator.GeyserType geyserType, bool isGenericGeyser)
		{
			this.id = "GeyserGeneric_" + geyserType.id;
			this.anim = anim;
			this.width = width;
			this.height = height;
			this.nameStringKey = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + geyserType.id.ToUpper() + ".NAME");
			this.descStringKey = new StringKey("STRINGS.CREATURES.SPECIES.GEYSER." + geyserType.id.ToUpper() + ".DESC");
			this.geyserType = geyserType;
			this.isGenericGeyser = isGenericGeyser;
		}

		// Token: 0x040007A5 RID: 1957
		public string id;

		// Token: 0x040007A6 RID: 1958
		public string anim;

		// Token: 0x040007A7 RID: 1959
		public int width;

		// Token: 0x040007A8 RID: 1960
		public int height;

		// Token: 0x040007A9 RID: 1961
		public StringKey nameStringKey;

		// Token: 0x040007AA RID: 1962
		public StringKey descStringKey;

		// Token: 0x040007AB RID: 1963
		public GeyserConfigurator.GeyserType geyserType;

		// Token: 0x040007AC RID: 1964
		public bool isGenericGeyser;
	}

	// Token: 0x020002A2 RID: 674
	private static class TEMPERATURES
	{
		// Token: 0x040007AD RID: 1965
		public const float BELOW_FREEZING = 263.15f;

		// Token: 0x040007AE RID: 1966
		public const float DUPE_NORMAL = 303.15f;

		// Token: 0x040007AF RID: 1967
		public const float DUPE_HOT = 333.15f;

		// Token: 0x040007B0 RID: 1968
		public const float BELOW_BOILING = 368.15f;

		// Token: 0x040007B1 RID: 1969
		public const float ABOVE_BOILING = 383.15f;

		// Token: 0x040007B2 RID: 1970
		public const float HOT1 = 423.15f;

		// Token: 0x040007B3 RID: 1971
		public const float HOT2 = 773.15f;

		// Token: 0x040007B4 RID: 1972
		public const float MOLTEN_MAGMA = 2000f;
	}

	// Token: 0x020002A3 RID: 675
	public static class RATES
	{
		// Token: 0x040007B5 RID: 1973
		public const float GAS_SMALL_MIN = 40f;

		// Token: 0x040007B6 RID: 1974
		public const float GAS_SMALL_MAX = 80f;

		// Token: 0x040007B7 RID: 1975
		public const float GAS_NORMAL_MIN = 70f;

		// Token: 0x040007B8 RID: 1976
		public const float GAS_NORMAL_MAX = 140f;

		// Token: 0x040007B9 RID: 1977
		public const float GAS_BIG_MIN = 100f;

		// Token: 0x040007BA RID: 1978
		public const float GAS_BIG_MAX = 200f;

		// Token: 0x040007BB RID: 1979
		public const float LIQUID_SMALL_MIN = 500f;

		// Token: 0x040007BC RID: 1980
		public const float LIQUID_SMALL_MAX = 1000f;

		// Token: 0x040007BD RID: 1981
		public const float LIQUID_NORMAL_MIN = 1000f;

		// Token: 0x040007BE RID: 1982
		public const float LIQUID_NORMAL_MAX = 2000f;

		// Token: 0x040007BF RID: 1983
		public const float LIQUID_BIG_MIN = 2000f;

		// Token: 0x040007C0 RID: 1984
		public const float LIQUID_BIG_MAX = 4000f;

		// Token: 0x040007C1 RID: 1985
		public const float MOLTEN_NORMAL_MIN = 200f;

		// Token: 0x040007C2 RID: 1986
		public const float MOLTEN_NORMAL_MAX = 400f;

		// Token: 0x040007C3 RID: 1987
		public const float MOLTEN_BIG_MIN = 400f;

		// Token: 0x040007C4 RID: 1988
		public const float MOLTEN_BIG_MAX = 800f;

		// Token: 0x040007C5 RID: 1989
		public const float MOLTEN_HUGE_MIN = 800f;

		// Token: 0x040007C6 RID: 1990
		public const float MOLTEN_HUGE_MAX = 1600f;
	}

	// Token: 0x020002A4 RID: 676
	public static class MAX_PRESSURES
	{
		// Token: 0x040007C7 RID: 1991
		public const float GAS = 5f;

		// Token: 0x040007C8 RID: 1992
		public const float GAS_HIGH = 15f;

		// Token: 0x040007C9 RID: 1993
		public const float MOLTEN = 150f;

		// Token: 0x040007CA RID: 1994
		public const float LIQUID_SMALL = 50f;

		// Token: 0x040007CB RID: 1995
		public const float LIQUID = 500f;
	}

	// Token: 0x020002A5 RID: 677
	public static class ITERATIONS
	{
		// Token: 0x020002A6 RID: 678
		public static class INFREQUENT_MOLTEN
		{
			// Token: 0x040007CC RID: 1996
			public const float PCT_MIN = 0.005f;

			// Token: 0x040007CD RID: 1997
			public const float PCT_MAX = 0.01f;

			// Token: 0x040007CE RID: 1998
			public const float LEN_MIN = 6000f;

			// Token: 0x040007CF RID: 1999
			public const float LEN_MAX = 12000f;
		}

		// Token: 0x020002A7 RID: 679
		public static class FREQUENT_MOLTEN
		{
			// Token: 0x040007D0 RID: 2000
			public const float PCT_MIN = 0.016666668f;

			// Token: 0x040007D1 RID: 2001
			public const float PCT_MAX = 0.1f;

			// Token: 0x040007D2 RID: 2002
			public const float LEN_MIN = 480f;

			// Token: 0x040007D3 RID: 2003
			public const float LEN_MAX = 1080f;
		}
	}
}
