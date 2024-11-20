using System;
using TUNING;
using UnityEngine;

namespace ShinseiSuperHeater
{
	// Token: 0x02000004 RID: 4
	internal class AirConfig : SpaceHeaterConfig
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002100 File Offset: 0x00000300
		public override BuildingDef CreateBuildingDef()
		{
			string id = "ShinseiIndustrialAirHeater";
			int width = 2;
			int height = 2;
			string anim = "spaceheater_kanim";
			int hitpoints = 30;
			float construction_time = 30f;
			float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
			string[] all_METALS = MATERIALS.ALL_METALS;
			float melting_point = 2873.15f;
			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 200f;
			buildingDef.ExhaustKilowattsWhenActive = -400f;
			buildingDef.SelfHeatKilowattsWhenActive = 0f;
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.OverheatTemperature = 773.15f;
			return buildingDef;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021D6 File Offset: 0x000003D6
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<LoopingSounds>();
			go.AddOrGet<SpaceHeater>().targetTemperature = 773.15f;
			go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 73.15f;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021F0 File Offset: 0x000003F0
		public static Color32 Color()
		{
			return new Color32(byte.MaxValue, 47, 15, byte.MaxValue);
		}

		// Token: 0x04000001 RID: 1
		public new const string ID = "ShinseiIndustrialAirHeater";
	}
}
