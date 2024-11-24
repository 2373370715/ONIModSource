using Klei.AI;
using TUNING;
using UnityEngine;

public class RanchStationConfig : IBuildingConfig
{
	public const string ID = "RanchStation";

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("RanchStation", 2, 3, "rancherstation_kanim", 30, 30f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER1, decor: BUILDINGS.DECOR.NONE);
		obj.ViewMode = OverlayModes.Rooms.ID;
		obj.Overheatable = false;
		obj.AudioCategory = "Metal";
		obj.AudioSize = "large";
		obj.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		return obj;
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.RanchStationType);
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		RanchStation.Def def = go.AddOrGetDef<RanchStation.Def>();
		def.IsCritterEligibleToBeRanchedCb = (GameObject creature_go, RanchStation.Instance ranch_station_smi) => !creature_go.GetComponent<Effects>().HasEffect("Ranched");
		def.OnRanchCompleteCb = delegate(GameObject creature_go)
		{
			RanchStation.Instance targetRanchStation = creature_go.GetSMI<RanchableMonitor.Instance>().TargetRanchStation;
			RancherChore.RancherChoreStates.Instance sMI = targetRanchStation.GetSMI<RancherChore.RancherChoreStates.Instance>();
			GameObject go2 = targetRanchStation.GetSMI<RancherChore.RancherChoreStates.Instance>().sm.rancher.Get(sMI);
			float num = 1f + go2.GetAttributes().Get(Db.Get().Attributes.Ranching.Id).GetTotalValue() * 0.1f;
			creature_go.GetComponent<Effects>().Add("Ranched", should_save: true).timeRemaining *= num;
			AmountInstance amountInstance = Db.Get().Amounts.HitPoints.Lookup(creature_go);
			amountInstance?.ApplyDelta(amountInstance.GetMax() - amountInstance.value + 1f);
		};
		def.RanchedPreAnim = "grooming_pre";
		def.RanchedLoopAnim = "grooming_loop";
		def.RanchedPstAnim = "grooming_pst";
		def.WorkTime = 12f;
		def.GetTargetRanchCell = delegate(RanchStation.Instance smi)
		{
			int result = Grid.InvalidCell;
			if (!smi.IsNullOrStopped())
			{
				result = Grid.CellRight(Grid.PosToCell(smi.transform.GetPosition()));
			}
			return result;
		};
		RoomTracker roomTracker = go.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.CreaturePen.Id;
		roomTracker.requirement = RoomTracker.Requirement.Required;
		go.AddOrGet<SkillPerkMissingComplainer>().requiredSkillPerk = Db.Get().SkillPerks.CanUseRanchStation.Id;
		Prioritizable.AddRef(go);
	}
}
