using System;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;

// Token: 0x02000537 RID: 1335
public class RailGunPayloadOpenerConfig : IBuildingConfig
{
	// Token: 0x06001795 RID: 6037 RVA: 0x000A5F1F File Offset: 0x000A411F
	public override string[] GetRequiredDlcIds()
	{
		return DlcManager.EXPANSION1;
	}

	// Token: 0x06001796 RID: 6038 RVA: 0x0019AB0C File Offset: 0x00198D0C
	public override BuildingDef CreateBuildingDef()
	{
		string id = "RailGunPayloadOpener";
		int width = 3;
		int height = 3;
		string anim = "railgun_emptier_kanim";
		int hitpoints = 250;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.DefaultAnimState = "on";
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 0.5f;
		return buildingDef;
	}

	// Token: 0x06001797 RID: 6039 RVA: 0x000AFF39 File Offset: 0x000AE139
	private void AttachPorts(GameObject go)
	{
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.liquidOutputPort;
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.gasOutputPort;
		go.AddComponent<ConduitSecondaryOutput>().portInfo = this.solidOutputPort;
	}

	// Token: 0x06001798 RID: 6040 RVA: 0x0019ABA4 File Offset: 0x00198DA4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		RailGunPayloadOpener railGunPayloadOpener = go.AddOrGet<RailGunPayloadOpener>();
		railGunPayloadOpener.liquidPortInfo = this.liquidOutputPort;
		railGunPayloadOpener.gasPortInfo = this.gasOutputPort;
		railGunPayloadOpener.solidPortInfo = this.solidOutputPort;
		railGunPayloadOpener.payloadStorage = go.AddComponent<Storage>();
		railGunPayloadOpener.payloadStorage.showInUI = true;
		railGunPayloadOpener.payloadStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		railGunPayloadOpener.payloadStorage.storageFilters = new List<Tag>
		{
			GameTags.RailGunPayloadEmptyable
		};
		railGunPayloadOpener.payloadStorage.capacityKg = 10f;
		railGunPayloadOpener.resourceStorage = go.AddComponent<Storage>();
		railGunPayloadOpener.resourceStorage.showInUI = true;
		railGunPayloadOpener.resourceStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		List<Tag> list = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD.Concat(STORAGEFILTERS.GASES).ToList<Tag>();
		list = list.Concat(STORAGEFILTERS.LIQUIDS).ToList<Tag>();
		railGunPayloadOpener.resourceStorage.storageFilters = list;
		railGunPayloadOpener.resourceStorage.capacityKg = 20000f;
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(railGunPayloadOpener.payloadStorage);
		manualDeliveryKG.RequestedItemTag = GameTags.RailGunPayloadEmptyable;
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.refillMass = 1f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.None;
	}

	// Token: 0x06001799 RID: 6041 RVA: 0x0019ACEC File Offset: 0x00198EEC
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<BuildingCellVisualizer>();
		DropAllWorkable dropAllWorkable = go.AddOrGet<DropAllWorkable>();
		dropAllWorkable.dropWorkTime = 90f;
		dropAllWorkable.choreTypeID = Db.Get().ChoreTypes.Fetch.Id;
		dropAllWorkable.ConfigureMultitoolContext("build", EffectConfigs.BuildSplashId);
		RequireInputs component = go.GetComponent<RequireInputs>();
		component.SetRequirements(true, false);
		component.requireConduitHasMass = false;
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x000AFF6E File Offset: 0x000AE16E
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		this.AttachPorts(go);
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x000AFF86 File Offset: 0x000AE186
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		this.AttachPorts(go);
		go.AddOrGet<BuildingCellVisualizer>();
	}

	// Token: 0x04000F44 RID: 3908
	public const string ID = "RailGunPayloadOpener";

	// Token: 0x04000F45 RID: 3909
	private ConduitPortInfo liquidOutputPort = new ConduitPortInfo(ConduitType.Liquid, new CellOffset(0, 0));

	// Token: 0x04000F46 RID: 3910
	private ConduitPortInfo gasOutputPort = new ConduitPortInfo(ConduitType.Gas, new CellOffset(1, 0));

	// Token: 0x04000F47 RID: 3911
	private ConduitPortInfo solidOutputPort = new ConduitPortInfo(ConduitType.Solid, new CellOffset(-1, 0));
}
