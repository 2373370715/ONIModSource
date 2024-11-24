using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x020012CB RID: 4811
public class CellEventLogger : EventLogger<CellEventInstance, CellEvent>
{
	// Token: 0x060062D5 RID: 25301 RVA: 0x000E08CA File Offset: 0x000DEACA
	public static void DestroyInstance()
	{
		CellEventLogger.Instance = null;
	}

	// Token: 0x060062D6 RID: 25302 RVA: 0x000E08D2 File Offset: 0x000DEAD2
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void LogCallbackSend(int cell, int callback_id)
	{
		if (callback_id != -1)
		{
			this.CallbackToCellMap[callback_id] = cell;
		}
	}

	// Token: 0x060062D7 RID: 25303 RVA: 0x002B7A2C File Offset: 0x002B5C2C
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void LogCallbackReceive(int callback_id)
	{
		int invalidCell = Grid.InvalidCell;
		this.CallbackToCellMap.TryGetValue(callback_id, out invalidCell);
	}

	// Token: 0x060062D8 RID: 25304 RVA: 0x002B7A50 File Offset: 0x002B5C50
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		CellEventLogger.Instance = this;
		this.SimMessagesSolid = (base.AddEvent(new CellSolidEvent("SimMessageSolid", "Sim Message", false, true)) as CellSolidEvent);
		this.SimCellOccupierDestroy = (base.AddEvent(new CellSolidEvent("SimCellOccupierClearSolid", "Sim Cell Occupier Destroy", false, true)) as CellSolidEvent);
		this.SimCellOccupierForceSolid = (base.AddEvent(new CellSolidEvent("SimCellOccupierForceSolid", "Sim Cell Occupier Force Solid", false, true)) as CellSolidEvent);
		this.SimCellOccupierSolidChanged = (base.AddEvent(new CellSolidEvent("SimCellOccupierSolidChanged", "Sim Cell Occupier Solid Changed", false, true)) as CellSolidEvent);
		this.DoorOpen = (base.AddEvent(new CellElementEvent("DoorOpen", "Door Open", true, true)) as CellElementEvent);
		this.DoorClose = (base.AddEvent(new CellElementEvent("DoorClose", "Door Close", true, true)) as CellElementEvent);
		this.Excavator = (base.AddEvent(new CellElementEvent("Excavator", "Excavator", true, true)) as CellElementEvent);
		this.DebugTool = (base.AddEvent(new CellElementEvent("DebugTool", "Debug Tool", true, true)) as CellElementEvent);
		this.SandBoxTool = (base.AddEvent(new CellElementEvent("SandBoxTool", "Sandbox Tool", true, true)) as CellElementEvent);
		this.TemplateLoader = (base.AddEvent(new CellElementEvent("TemplateLoader", "Template Loader", true, true)) as CellElementEvent);
		this.Scenario = (base.AddEvent(new CellElementEvent("Scenario", "Scenario", true, true)) as CellElementEvent);
		this.SimCellOccupierOnSpawn = (base.AddEvent(new CellElementEvent("SimCellOccupierOnSpawn", "Sim Cell Occupier OnSpawn", true, true)) as CellElementEvent);
		this.SimCellOccupierDestroySelf = (base.AddEvent(new CellElementEvent("SimCellOccupierDestroySelf", "Sim Cell Occupier Destroy Self", true, true)) as CellElementEvent);
		this.WorldGapManager = (base.AddEvent(new CellElementEvent("WorldGapManager", "World Gap Manager", true, true)) as CellElementEvent);
		this.ReceiveElementChanged = (base.AddEvent(new CellElementEvent("ReceiveElementChanged", "Sim Message", false, false)) as CellElementEvent);
		this.ObjectSetSimOnSpawn = (base.AddEvent(new CellElementEvent("ObjectSetSimOnSpawn", "Object set sim on spawn", true, true)) as CellElementEvent);
		this.DecompositionDirtyWater = (base.AddEvent(new CellElementEvent("DecompositionDirtyWater", "Decomposition dirty water", true, true)) as CellElementEvent);
		this.SendCallback = (base.AddEvent(new CellCallbackEvent("SendCallback", true, true)) as CellCallbackEvent);
		this.ReceiveCallback = (base.AddEvent(new CellCallbackEvent("ReceiveCallback", false, true)) as CellCallbackEvent);
		this.Dig = (base.AddEvent(new CellDigEvent(true)) as CellDigEvent);
		this.WorldDamageDelayedSpawnFX = (base.AddEvent(new CellAddRemoveSubstanceEvent("WorldDamageDelayedSpawnFX", "World Damage Delayed Spawn FX", false)) as CellAddRemoveSubstanceEvent);
		this.OxygenModifierSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("OxygenModifierSimUpdate", "Oxygen Modifier SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.LiquidChunkOnStore = (base.AddEvent(new CellAddRemoveSubstanceEvent("LiquidChunkOnStore", "Liquid Chunk On Store", false)) as CellAddRemoveSubstanceEvent);
		this.FallingWaterAddToSim = (base.AddEvent(new CellAddRemoveSubstanceEvent("FallingWaterAddToSim", "Falling Water Add To Sim", false)) as CellAddRemoveSubstanceEvent);
		this.ExploderOnSpawn = (base.AddEvent(new CellAddRemoveSubstanceEvent("ExploderOnSpawn", "Exploder OnSpawn", false)) as CellAddRemoveSubstanceEvent);
		this.ExhaustSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("ExhaustSimUpdate", "Exhaust SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.ElementConsumerSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementConsumerSimUpdate", "Element Consumer SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.SublimatesEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("SublimatesEmit", "Sublimates Emit", false)) as CellAddRemoveSubstanceEvent);
		this.Mop = (base.AddEvent(new CellAddRemoveSubstanceEvent("Mop", "Mop", false)) as CellAddRemoveSubstanceEvent);
		this.OreMelted = (base.AddEvent(new CellAddRemoveSubstanceEvent("OreMelted", "Ore Melted", false)) as CellAddRemoveSubstanceEvent);
		this.ConstructTile = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConstructTile", "ConstructTile", false)) as CellAddRemoveSubstanceEvent);
		this.Dumpable = (base.AddEvent(new CellAddRemoveSubstanceEvent("Dympable", "Dumpable", false)) as CellAddRemoveSubstanceEvent);
		this.Cough = (base.AddEvent(new CellAddRemoveSubstanceEvent("Cough", "Cough", false)) as CellAddRemoveSubstanceEvent);
		this.Meteor = (base.AddEvent(new CellAddRemoveSubstanceEvent("Meteor", "Meteor", false)) as CellAddRemoveSubstanceEvent);
		this.ElementChunkTransition = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementChunkTransition", "Element Chunk Transition", false)) as CellAddRemoveSubstanceEvent);
		this.OxyrockEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("OxyrockEmit", "Oxyrock Emit", false)) as CellAddRemoveSubstanceEvent);
		this.BleachstoneEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("BleachstoneEmit", "Bleachstone Emit", false)) as CellAddRemoveSubstanceEvent);
		this.UnstableGround = (base.AddEvent(new CellAddRemoveSubstanceEvent("UnstableGround", "Unstable Ground", false)) as CellAddRemoveSubstanceEvent);
		this.ConduitFlowEmptyConduit = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConduitFlowEmptyConduit", "Conduit Flow Empty Conduit", false)) as CellAddRemoveSubstanceEvent);
		this.ConduitConsumerWrongElement = (base.AddEvent(new CellAddRemoveSubstanceEvent("ConduitConsumerWrongElement", "Conduit Consumer Wrong Element", false)) as CellAddRemoveSubstanceEvent);
		this.OverheatableMeltingDown = (base.AddEvent(new CellAddRemoveSubstanceEvent("OverheatableMeltingDown", "Overheatable MeltingDown", false)) as CellAddRemoveSubstanceEvent);
		this.FabricatorProduceMelted = (base.AddEvent(new CellAddRemoveSubstanceEvent("FabricatorProduceMelted", "Fabricator Produce Melted", false)) as CellAddRemoveSubstanceEvent);
		this.PumpSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("PumpSimUpdate", "Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.WallPumpSimUpdate = (base.AddEvent(new CellAddRemoveSubstanceEvent("WallPumpSimUpdate", "Wall Pump SimUpdate", false)) as CellAddRemoveSubstanceEvent);
		this.Vomit = (base.AddEvent(new CellAddRemoveSubstanceEvent("Vomit", "Vomit", false)) as CellAddRemoveSubstanceEvent);
		this.Tears = (base.AddEvent(new CellAddRemoveSubstanceEvent("Tears", "Tears", false)) as CellAddRemoveSubstanceEvent);
		this.Pee = (base.AddEvent(new CellAddRemoveSubstanceEvent("Pee", "Pee", false)) as CellAddRemoveSubstanceEvent);
		this.AlgaeHabitat = (base.AddEvent(new CellAddRemoveSubstanceEvent("AlgaeHabitat", "AlgaeHabitat", false)) as CellAddRemoveSubstanceEvent);
		this.CO2FilterOxygen = (base.AddEvent(new CellAddRemoveSubstanceEvent("CO2FilterOxygen", "CO2FilterOxygen", false)) as CellAddRemoveSubstanceEvent);
		this.ToiletEmit = (base.AddEvent(new CellAddRemoveSubstanceEvent("ToiletEmit", "ToiletEmit", false)) as CellAddRemoveSubstanceEvent);
		this.ElementEmitted = (base.AddEvent(new CellAddRemoveSubstanceEvent("ElementEmitted", "Element Emitted", false)) as CellAddRemoveSubstanceEvent);
		this.CO2ManagerFixedUpdate = (base.AddEvent(new CellModifyMassEvent("CO2ManagerFixedUpdate", "CO2Manager FixedUpdate", false)) as CellModifyMassEvent);
		this.EnvironmentConsumerFixedUpdate = (base.AddEvent(new CellModifyMassEvent("EnvironmentConsumerFixedUpdate", "EnvironmentConsumer FixedUpdate", false)) as CellModifyMassEvent);
		this.ExcavatorShockwave = (base.AddEvent(new CellModifyMassEvent("ExcavatorShockwave", "Excavator Shockwave", false)) as CellModifyMassEvent);
		this.OxygenBreatherSimUpdate = (base.AddEvent(new CellModifyMassEvent("OxygenBreatherSimUpdate", "Oxygen Breather SimUpdate", false)) as CellModifyMassEvent);
		this.CO2ScrubberSimUpdate = (base.AddEvent(new CellModifyMassEvent("CO2ScrubberSimUpdate", "CO2Scrubber SimUpdate", false)) as CellModifyMassEvent);
		this.RiverSourceSimUpdate = (base.AddEvent(new CellModifyMassEvent("RiverSourceSimUpdate", "RiverSource SimUpdate", false)) as CellModifyMassEvent);
		this.RiverTerminusSimUpdate = (base.AddEvent(new CellModifyMassEvent("RiverTerminusSimUpdate", "RiverTerminus SimUpdate", false)) as CellModifyMassEvent);
		this.DebugToolModifyMass = (base.AddEvent(new CellModifyMassEvent("DebugToolModifyMass", "DebugTool ModifyMass", false)) as CellModifyMassEvent);
		this.EnergyGeneratorModifyMass = (base.AddEvent(new CellModifyMassEvent("EnergyGeneratorModifyMass", "EnergyGenerator ModifyMass", false)) as CellModifyMassEvent);
		this.SolidFilterEvent = (base.AddEvent(new CellSolidFilterEvent("SolidFilterEvent", true)) as CellSolidFilterEvent);
	}

	// Token: 0x0400464F RID: 17999
	public static CellEventLogger Instance;

	// Token: 0x04004650 RID: 18000
	public CellSolidEvent SimMessagesSolid;

	// Token: 0x04004651 RID: 18001
	public CellSolidEvent SimCellOccupierDestroy;

	// Token: 0x04004652 RID: 18002
	public CellSolidEvent SimCellOccupierForceSolid;

	// Token: 0x04004653 RID: 18003
	public CellSolidEvent SimCellOccupierSolidChanged;

	// Token: 0x04004654 RID: 18004
	public CellElementEvent DoorOpen;

	// Token: 0x04004655 RID: 18005
	public CellElementEvent DoorClose;

	// Token: 0x04004656 RID: 18006
	public CellElementEvent Excavator;

	// Token: 0x04004657 RID: 18007
	public CellElementEvent DebugTool;

	// Token: 0x04004658 RID: 18008
	public CellElementEvent SandBoxTool;

	// Token: 0x04004659 RID: 18009
	public CellElementEvent TemplateLoader;

	// Token: 0x0400465A RID: 18010
	public CellElementEvent Scenario;

	// Token: 0x0400465B RID: 18011
	public CellElementEvent SimCellOccupierOnSpawn;

	// Token: 0x0400465C RID: 18012
	public CellElementEvent SimCellOccupierDestroySelf;

	// Token: 0x0400465D RID: 18013
	public CellElementEvent WorldGapManager;

	// Token: 0x0400465E RID: 18014
	public CellElementEvent ReceiveElementChanged;

	// Token: 0x0400465F RID: 18015
	public CellElementEvent ObjectSetSimOnSpawn;

	// Token: 0x04004660 RID: 18016
	public CellElementEvent DecompositionDirtyWater;

	// Token: 0x04004661 RID: 18017
	public CellElementEvent LaunchpadDesolidify;

	// Token: 0x04004662 RID: 18018
	public CellCallbackEvent SendCallback;

	// Token: 0x04004663 RID: 18019
	public CellCallbackEvent ReceiveCallback;

	// Token: 0x04004664 RID: 18020
	public CellDigEvent Dig;

	// Token: 0x04004665 RID: 18021
	public CellAddRemoveSubstanceEvent WorldDamageDelayedSpawnFX;

	// Token: 0x04004666 RID: 18022
	public CellAddRemoveSubstanceEvent SublimatesEmit;

	// Token: 0x04004667 RID: 18023
	public CellAddRemoveSubstanceEvent OxygenModifierSimUpdate;

	// Token: 0x04004668 RID: 18024
	public CellAddRemoveSubstanceEvent LiquidChunkOnStore;

	// Token: 0x04004669 RID: 18025
	public CellAddRemoveSubstanceEvent FallingWaterAddToSim;

	// Token: 0x0400466A RID: 18026
	public CellAddRemoveSubstanceEvent ExploderOnSpawn;

	// Token: 0x0400466B RID: 18027
	public CellAddRemoveSubstanceEvent ExhaustSimUpdate;

	// Token: 0x0400466C RID: 18028
	public CellAddRemoveSubstanceEvent ElementConsumerSimUpdate;

	// Token: 0x0400466D RID: 18029
	public CellAddRemoveSubstanceEvent ElementChunkTransition;

	// Token: 0x0400466E RID: 18030
	public CellAddRemoveSubstanceEvent OxyrockEmit;

	// Token: 0x0400466F RID: 18031
	public CellAddRemoveSubstanceEvent BleachstoneEmit;

	// Token: 0x04004670 RID: 18032
	public CellAddRemoveSubstanceEvent UnstableGround;

	// Token: 0x04004671 RID: 18033
	public CellAddRemoveSubstanceEvent ConduitFlowEmptyConduit;

	// Token: 0x04004672 RID: 18034
	public CellAddRemoveSubstanceEvent ConduitConsumerWrongElement;

	// Token: 0x04004673 RID: 18035
	public CellAddRemoveSubstanceEvent OverheatableMeltingDown;

	// Token: 0x04004674 RID: 18036
	public CellAddRemoveSubstanceEvent FabricatorProduceMelted;

	// Token: 0x04004675 RID: 18037
	public CellAddRemoveSubstanceEvent PumpSimUpdate;

	// Token: 0x04004676 RID: 18038
	public CellAddRemoveSubstanceEvent WallPumpSimUpdate;

	// Token: 0x04004677 RID: 18039
	public CellAddRemoveSubstanceEvent Vomit;

	// Token: 0x04004678 RID: 18040
	public CellAddRemoveSubstanceEvent Tears;

	// Token: 0x04004679 RID: 18041
	public CellAddRemoveSubstanceEvent Pee;

	// Token: 0x0400467A RID: 18042
	public CellAddRemoveSubstanceEvent AlgaeHabitat;

	// Token: 0x0400467B RID: 18043
	public CellAddRemoveSubstanceEvent CO2FilterOxygen;

	// Token: 0x0400467C RID: 18044
	public CellAddRemoveSubstanceEvent ToiletEmit;

	// Token: 0x0400467D RID: 18045
	public CellAddRemoveSubstanceEvent ElementEmitted;

	// Token: 0x0400467E RID: 18046
	public CellAddRemoveSubstanceEvent Mop;

	// Token: 0x0400467F RID: 18047
	public CellAddRemoveSubstanceEvent OreMelted;

	// Token: 0x04004680 RID: 18048
	public CellAddRemoveSubstanceEvent ConstructTile;

	// Token: 0x04004681 RID: 18049
	public CellAddRemoveSubstanceEvent Dumpable;

	// Token: 0x04004682 RID: 18050
	public CellAddRemoveSubstanceEvent Cough;

	// Token: 0x04004683 RID: 18051
	public CellAddRemoveSubstanceEvent Meteor;

	// Token: 0x04004684 RID: 18052
	public CellModifyMassEvent CO2ManagerFixedUpdate;

	// Token: 0x04004685 RID: 18053
	public CellModifyMassEvent EnvironmentConsumerFixedUpdate;

	// Token: 0x04004686 RID: 18054
	public CellModifyMassEvent ExcavatorShockwave;

	// Token: 0x04004687 RID: 18055
	public CellModifyMassEvent OxygenBreatherSimUpdate;

	// Token: 0x04004688 RID: 18056
	public CellModifyMassEvent CO2ScrubberSimUpdate;

	// Token: 0x04004689 RID: 18057
	public CellModifyMassEvent RiverSourceSimUpdate;

	// Token: 0x0400468A RID: 18058
	public CellModifyMassEvent RiverTerminusSimUpdate;

	// Token: 0x0400468B RID: 18059
	public CellModifyMassEvent DebugToolModifyMass;

	// Token: 0x0400468C RID: 18060
	public CellModifyMassEvent EnergyGeneratorModifyMass;

	// Token: 0x0400468D RID: 18061
	public CellSolidFilterEvent SolidFilterEvent;

	// Token: 0x0400468E RID: 18062
	public Dictionary<int, int> CallbackToCellMap = new Dictionary<int, int>();
}
