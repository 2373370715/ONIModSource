using System;
using TUNING;
using UnityEngine;

namespace ShinseiSuperHeater
{
	// Token: 0x02000006 RID: 6
	internal class WaterConfig : LiquidHeaterConfig
	{
		// Token: 0x0600000B RID: 11 RVA: 0x000022A0 File Offset: 0x000004A0
		public override BuildingDef CreateBuildingDef()
		{
			string id = "ShinseiSuperHeater";
			int width = 4;
			int height = 1;
			string anim = "boiler_kanim";
			int hitpoints = 30;
			float construction_time = 30f;
			float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
			string[] all_METALS = MATERIALS.ALL_METALS;
			float melting_point = 3200f;
			BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.Floodable = false;
			buildingDef.EnergyConsumptionWhenActive = 600f;
			buildingDef.ExhaustKilowattsWhenActive = -4000f;
			buildingDef.SelfHeatKilowattsWhenActive = 0f;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.AudioCategory = "SolidMetal";
			buildingDef.OverheatTemperature = 773.15f;
			buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
			return buildingDef;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002380 File Offset: 0x00000580
		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<LoopingSounds>();
			SpaceHeater spaceHeater = go.AddOrGet<SpaceHeater>();
			go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 73.15f;
			spaceHeater.SetLiquidHeater();
			spaceHeater.targetTemperature = 773.15f;
			spaceHeater.minimumCellMass = 0f;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000023BC File Offset: 0x000005BC
		public static Color32 Color()
		{
			return new Color32(byte.MaxValue, 47, 15, byte.MaxValue);
		}

		// Token: 0x04000002 RID: 2
		public new const string ID = "ShinseiSuperHeater";
	}
}
