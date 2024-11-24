using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x0200053D RID: 1341
public class RemoteWorkTerminalConfig : IBuildingConfig
{
	// Token: 0x060017B2 RID: 6066 RVA: 0x0019B498 File Offset: 0x00199698
	public override BuildingDef CreateBuildingDef()
	{
		string id = RemoteWorkTerminalConfig.ID;
		int width = 3;
		int height = 2;
		string anim = "remote_work_terminal_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.SelfHeatKilowattsWhenActive = 2f;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		return buildingDef;
	}

	// Token: 0x060017B3 RID: 6067 RVA: 0x0019B508 File Offset: 0x00199708
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddComponent<RemoteWorkTerminal>().workTime = float.PositiveInfinity;
		go.AddComponent<RemoteWorkTerminalSM>();
		go.AddOrGet<Operational>();
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 100f;
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Insulate
		});
		ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = RemoteWorkTerminalConfig.INPUT_MATERIAL;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.capacity = 10f;
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.Functional;
		ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
		elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
		{
			new ElementConverter.ConsumedElement(RemoteWorkTerminalConfig.INPUT_MATERIAL, 0.006666667f, true)
		};
		elementConverter.showDescriptors = false;
		go.AddOrGet<ElementConverterOperationalRequirement>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x060017B4 RID: 6068 RVA: 0x000AA527 File Offset: 0x000A8727
	public override string[] GetRequiredDlcIds()
	{
		return new string[]
		{
			"DLC3_ID"
		};
	}

	// Token: 0x04000F50 RID: 3920
	public static string ID = "RemoteWorkTerminal";

	// Token: 0x04000F51 RID: 3921
	public static readonly Tag INPUT_MATERIAL = new Tag("OrbitalResearchDatabank");

	// Token: 0x04000F52 RID: 3922
	public const float INPUT_CAPACITY = 10f;

	// Token: 0x04000F53 RID: 3923
	public const float INPUT_CONSUMPTION_RATE_PER_S = 0.006666667f;

	// Token: 0x04000F54 RID: 3924
	public const float INPUT_REFILL_RATIO = 0.5f;
}
