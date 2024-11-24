using System;
using TUNING;
using UnityEngine;

// Token: 0x02000387 RID: 903
public class HighEnergyParticleSpawnerConfig : IBuildingConfig
{
	// Token: 0x06000ECC RID: 3788 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x0017AA70 File Offset: 0x00178C70
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HighEnergyParticleSpawner";
		int width = 1;
		int height = 2;
		string anim = "radiation_collector_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 1);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.DiseaseCellVisName = "RadiationSickness";
		buildingDef.UtilityOutputOffset = CellOffset.none;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HighEnergyParticleSpawner");
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x0017AB60 File Offset: 0x00178D60
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		Prioritizable.AddRef(go);
		go.AddOrGet<HighEnergyParticleStorage>().capacity = 500f;
		go.AddOrGet<LoopingSounds>();
		HighEnergyParticleSpawner highEnergyParticleSpawner = go.AddOrGet<HighEnergyParticleSpawner>();
		highEnergyParticleSpawner.minLaunchInterval = 2f;
		highEnergyParticleSpawner.radiationSampleRate = 0.2f;
		highEnergyParticleSpawner.minSlider = 50;
		highEnergyParticleSpawner.maxSlider = 500;
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000AA3 RID: 2723
	public const string ID = "HighEnergyParticleSpawner";

	// Token: 0x04000AA4 RID: 2724
	public const float MIN_LAUNCH_INTERVAL = 2f;

	// Token: 0x04000AA5 RID: 2725
	public const float RADIATION_SAMPLE_RATE = 0.2f;

	// Token: 0x04000AA6 RID: 2726
	public const float HEP_PER_RAD = 0.1f;

	// Token: 0x04000AA7 RID: 2727
	public const int MIN_SLIDER = 50;

	// Token: 0x04000AA8 RID: 2728
	public const int MAX_SLIDER = 500;

	// Token: 0x04000AA9 RID: 2729
	public const float DISABLED_CONSUMPTION_RATE = 1f;
}
