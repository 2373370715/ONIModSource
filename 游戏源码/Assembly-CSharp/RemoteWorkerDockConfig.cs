using System;
using TUNING;
using UnityEngine;

// Token: 0x0200053E RID: 1342
public class RemoteWorkerDockConfig : IBuildingConfig
{
	// Token: 0x060017B7 RID: 6071 RVA: 0x0019B5F4 File Offset: 0x001997F4
	public override BuildingDef CreateBuildingDef()
	{
		string id = RemoteWorkerDockConfig.ID;
		int width = 1;
		int height = 2;
		string anim = "remote_work_dock_kanim";
		int hitpoints = 100;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.InputConduitType = ConduitType.Liquid;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.UtilityInputOffset = new CellOffset(0, 1);
		buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		return buildingDef;
	}

	// Token: 0x060017B8 RID: 6072 RVA: 0x000AFFFC File Offset: 0x000AE1FC
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AddVisualizer(go);
	}

	// Token: 0x060017B9 RID: 6073 RVA: 0x000B000D File Offset: 0x000AE20D
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		this.AddVisualizer(go);
	}

	// Token: 0x060017BA RID: 6074 RVA: 0x0019B6B4 File Offset: 0x001998B4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<RemoteWorkerDock>();
		go.AddOrGet<RemoteWorkerDockAnimSM>();
		go.AddOrGet<Storage>();
		go.AddOrGet<Operational>();
		go.AddOrGet<UserNameable>();
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Liquid;
		conduitConsumer.capacityTag = GameTags.LubricatingOil;
		conduitConsumer.capacityKG = 50f;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.elementFilter = new SimHashes[]
		{
			SimHashes.LiquidGunk
		};
		this.AddVisualizer(go);
		go.AddOrGet<RangeVisualizer>();
	}

	// Token: 0x060017BB RID: 6075 RVA: 0x000A5F37 File Offset: 0x000A4137
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x060017BC RID: 6076 RVA: 0x0019B73C File Offset: 0x0019993C
	private void AddVisualizer(GameObject prefab)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.RangeMin.x = -12;
		rangeVisualizer.RangeMin.y = 0;
		rangeVisualizer.RangeMax.x = 12;
		rangeVisualizer.RangeMax.y = 0;
		rangeVisualizer.OriginOffset = default(Vector2I);
		rangeVisualizer.BlockingTileVisible = false;
		prefab.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = new Func<int, bool>(RemoteWorkerDockConfig.DockPathBlockingCB);
		};
	}

	// Token: 0x060017BD RID: 6077 RVA: 0x0019B7C0 File Offset: 0x001999C0
	public static bool DockPathBlockingCB(int cell)
	{
		int num = Grid.CellAbove(cell);
		int num2 = Grid.CellBelow(cell);
		return num == Grid.InvalidCell || num2 == Grid.InvalidCell || (!Grid.Foundation[num2] && !Grid.Solid[num2]) || (Grid.Solid[cell] || Grid.Solid[num]);
	}

	// Token: 0x04000F55 RID: 3925
	public static string ID = "RemoteWorkerDock";

	// Token: 0x04000F56 RID: 3926
	public const float NEW_WORKER_DELAY_SECONDS = 2f;

	// Token: 0x04000F57 RID: 3927
	public const int WORK_RANGE = 12;

	// Token: 0x04000F58 RID: 3928
	public const float LUBRICANT_CAPACITY_KG = 50f;

	// Token: 0x04000F59 RID: 3929
	public const string ON_EMPTY_ANIM = "on_empty";

	// Token: 0x04000F5A RID: 3930
	public const string ON_FULL_ANIM = "on_full";

	// Token: 0x04000F5B RID: 3931
	public const string OFF_EMPTY_ANIM = "off_empty";

	// Token: 0x04000F5C RID: 3932
	public const string OFF_FULL_ANIM = "off_full";

	// Token: 0x04000F5D RID: 3933
	public const string NEW_WORKER_ANIM = "new_worker";
}
