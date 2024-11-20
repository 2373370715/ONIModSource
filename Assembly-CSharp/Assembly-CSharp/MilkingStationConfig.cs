using System;
using Klei.AI;
using TUNING;
using UnityEngine;

public class MilkingStationConfig : IBuildingConfig
{
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MilkingStation";
		int width = 2;
		int height = 4;
		string anim = "milking_station_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0],
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Plastic"
		};
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier, 0.2f);
		buildingDef.ViewMode = OverlayModes.Rooms.ID;
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
		buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
		buildingDef.Overheatable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		buildingDef.OutputConduitType = ConduitType.Liquid;
		buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
		return buildingDef;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType, false);
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = MooTuning.MILK_AMOUNT_AT_MILKING * 2f;
		storage.showInUI = true;
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		go.AddOrGet<SkillPerkMissingComplainer>().requiredSkillPerk = Db.Get().SkillPerks.CanUseMilkingStation.Id;
		RanchStation.Def ranch_station = go.AddOrGetDef<RanchStation.Def>();
		ranch_station.IsCritterEligibleToBeRanchedCb = ((GameObject creature_go, RanchStation.Instance ranch_station_smi) => !(creature_go.PrefabID() != "Moo") && creature_go.GetComponent<KPrefabID>().HasTag(GameTags.Creatures.RequiresMilking));
		ranch_station.RancherInteractAnim = "anim_interacts_milking_station_kanim";
		ranch_station.RanchedPreAnim = "mooshake_pre";
		ranch_station.RanchedLoopAnim = "mooshake_loop";
		ranch_station.RanchedPstAnim = "mooshake_pst";
		ranch_station.WorkTime = 20f;
		ranch_station.CreatureRanchingStatusItem = Db.Get().CreatureStatusItems.GettingMilked;
		ranch_station.GetTargetRanchCell = delegate(RanchStation.Instance smi)
		{
			int result = Grid.InvalidCell;
			if (!smi.IsNullOrStopped())
			{
				result = Grid.PosToCell(smi.transform.GetPosition());
			}
			return result;
		};
		ranch_station.OnRanchCompleteCb = delegate(GameObject creature_go)
		{
			RanchStation.Instance targetRanchStation = creature_go.GetSMI<RanchableMonitor.Instance>().TargetRanchStation;
			AmountInstance amountInstance = creature_go.GetAmounts().Get(Db.Get().Amounts.MilkProduction.Id);
			if (amountInstance.value > 0f)
			{
				float mass = amountInstance.value * (MooTuning.MILK_AMOUNT_AT_MILKING / amountInstance.GetMax());
				targetRanchStation.GetComponent<Storage>().AddLiquid(SimHashes.Milk, mass, 310.15f, byte.MaxValue, 0, false, true);
				amountInstance.SetValue(0f);
			}
			creature_go.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.RequiresMilking);
		};
		ranch_station.OnRanchWorkTick = delegate(GameObject creature_go, float dt, Workable workable)
		{
			if (creature_go.GetComponent<KAnimControllerBase>().CurrentAnim.name == ranch_station.RanchedPstAnim)
			{
				StateMachine.Instance ranchStation = creature_go.GetSMI<RanchedStates.Instance>().GetRanchStation();
				AmountInstance amountInstance = creature_go.GetAmounts().Get(Db.Get().Amounts.MilkProduction.Id);
				float num = amountInstance.GetMax() * dt / workable.workTime;
				float mass = num * (MooTuning.MILK_AMOUNT_AT_MILKING / amountInstance.GetMax());
				float temperature = creature_go.GetComponent<PrimaryElement>().Temperature;
				ranchStation.GetComponent<Storage>().AddLiquid(SimHashes.Milk, mass, temperature, byte.MaxValue, 0, false, true);
				amountInstance.ApplyDelta(-num);
			}
		};
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
	}

	public const string ID = "MilkingStation";
}
