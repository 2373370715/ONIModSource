﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000409 RID: 1033
public class MassageTableConfig : IBuildingConfig
{
	// Token: 0x0600117A RID: 4474 RVA: 0x00183F1C File Offset: 0x0018211C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MassageTable";
		int width = 2;
		int height = 2;
		string anim = "masseur_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.Overheatable = true;
		buildingDef.EnergyConsumptionWhenActive = 240f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x0600117B RID: 4475 RVA: 0x00183FAC File Offset: 0x001821AC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.DeStressingBuilding, false);
		MassageTable massageTable = go.AddOrGet<MassageTable>();
		massageTable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_masseur_kanim")
		};
		massageTable.stressModificationValue = -30f;
		massageTable.roomStressModificationValue = -60f;
		massageTable.workLayer = Grid.SceneLayer.BuildingFront;
		Ownable ownable = go.AddOrGet<Ownable>();
		ownable.slotID = Db.Get().AssignableSlots.MassageTable.Id;
		ownable.canBePublic = true;
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.MassageClinic.Id;
		roomTracker.requirement = RoomTracker.Requirement.Recommended;
	}

	// Token: 0x0600117C RID: 4476 RVA: 0x000ADE17 File Offset: 0x000AC017
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.GetComponent<KAnimControllerBase>().initialAnim = "off";
		go.AddOrGet<CopyBuildingSettings>();
	}

	// Token: 0x04000BE6 RID: 3046
	public const string ID = "MassageTable";
}
