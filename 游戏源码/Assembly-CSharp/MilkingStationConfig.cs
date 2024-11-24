using Klei.AI;
using TUNING;
using UnityEngine;

public class MilkingStationConfig : IBuildingConfig
{
	public const string ID = "MilkingStation";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("MilkingStation", 2, 4, "milking_station_kanim", 30, 60f, new float[2]
		{
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0],
			BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
		}, new string[2] { "RefinedMetal", "Plastic" }, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER1, decor: BUILDINGS.DECOR.PENALTY.TIER2);
		obj.ViewMode = OverlayModes.Rooms.ID;
		obj.OutputConduitType = ConduitType.Liquid;
		obj.UtilityOutputOffset = new CellOffset(1, 0);
		obj.ViewMode = OverlayModes.LiquidConduits.ID;
		obj.Overheatable = false;
		obj.AudioCategory = "Metal";
		obj.AudioSize = "large";
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		obj.OutputConduitType = ConduitType.Liquid;
		obj.UtilityOutputOffset = new CellOffset(1, 1);
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType);
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
		ranch_station.IsCritterEligibleToBeRanchedCb = (GameObject creature_go, RanchStation.Instance ranch_station_smi) => !(creature_go.PrefabID() != "Moo") && creature_go.GetComponent<KPrefabID>().HasTag(GameTags.Creatures.RequiresMilking);
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
			AmountInstance amountInstance2 = creature_go.GetAmounts().Get(Db.Get().Amounts.MilkProduction.Id);
			if (amountInstance2.value > 0f)
			{
				float mass2 = amountInstance2.value * (MooTuning.MILK_AMOUNT_AT_MILKING / amountInstance2.GetMax());
				targetRanchStation.GetComponent<Storage>().AddLiquid(SimHashes.Milk, mass2, 310.15f, byte.MaxValue, 0);
				amountInstance2.SetValue(0f);
			}
			creature_go.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.RequiresMilking);
		};
		ranch_station.OnRanchWorkTick = delegate(GameObject creature_go, float dt, Workable workable)
		{
			if (creature_go.GetComponent<KAnimControllerBase>().CurrentAnim.name == ranch_station.RanchedPstAnim)
			{
				RanchStation.Instance ranchStation = creature_go.GetSMI<RanchedStates.Instance>().GetRanchStation();
				AmountInstance amountInstance = creature_go.GetAmounts().Get(Db.Get().Amounts.MilkProduction.Id);
				float num = amountInstance.GetMax() * dt / workable.workTime;
				float mass = num * (MooTuning.MILK_AMOUNT_AT_MILKING / amountInstance.GetMax());
				float temperature = creature_go.GetComponent<PrimaryElement>().Temperature;
				ranchStation.GetComponent<Storage>().AddLiquid(SimHashes.Milk, mass, temperature, byte.MaxValue, 0);
				amountInstance.ApplyDelta(0f - num);
			}
		};
		ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
		conduitDispenser.conduitType = ConduitType.Liquid;
		conduitDispenser.alwaysDispense = true;
		conduitDispenser.elementFilter = null;
	}
}
