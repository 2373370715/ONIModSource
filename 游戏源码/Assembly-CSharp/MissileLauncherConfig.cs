using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class MissileLauncherConfig : IBuildingConfig
{
	public const string ID = "MissileLauncher";

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef obj = BuildingTemplates.CreateBuildingDef("MissileLauncher", 3, 5, "missile_launcher_kanim", 250, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.ALL_METALS, 1600f, BuildLocationRule.OnFloor, noise: NOISE_POLLUTION.NOISY.TIER5, decor: TUNING.BUILDINGS.DECOR.NONE);
		obj.SceneLayer = Grid.SceneLayer.BuildingFront;
		obj.Floodable = false;
		obj.Overheatable = false;
		obj.AudioCategory = "Metal";
		obj.BaseTimeUntilRepair = 400f;
		obj.DefaultAnimState = "off";
		obj.RequiresPowerInput = true;
		obj.PowerInputOffset = new CellOffset(-1, 0);
		obj.EnergyConsumptionWhenActive = 240f;
		obj.InputConduitType = ConduitType.Solid;
		obj.UtilityInputOffset = new CellOffset(0, 0);
		obj.ViewMode = OverlayModes.SolidConveyor.ID;
		obj.ExhaustKilowattsWhenActive = 0.5f;
		obj.SelfHeatKilowattsWhenActive = 2f;
		return obj;
	}

	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		AddVisualizer(go);
	}

	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		AddVisualizer(go);
	}

	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGetDef<MissileLauncher.Def>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
		Storage storage = BuildingTemplates.CreateDefaultStorage(go);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.storageFilters = new List<Tag> { "MissileBasic" };
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
		storage.capacityKg = 300f;
		ManualDeliveryKG manualDeliveryKG = go.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.RequestedItemTag = "MissileBasic";
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.operationalRequirement = Operational.State.None;
		manualDeliveryKG.refillMass = 5f;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.capacity = storage.Capacity() / 10f;
		manualDeliveryKG.MassPerUnit = 10f;
		SolidConduitConsumer solidConduitConsumer = go.AddOrGet<SolidConduitConsumer>();
		solidConduitConsumer.alwaysConsume = true;
		solidConduitConsumer.capacityKG = storage.Capacity();
		AddVisualizer(go);
	}

	private void AddVisualizer(GameObject go)
	{
		RangeVisualizer rangeVisualizer = go.AddOrGet<RangeVisualizer>();
		rangeVisualizer.OriginOffset = MissileLauncher.Def.LaunchOffset.ToVector2I();
		rangeVisualizer.RangeMin.x = -MissileLauncher.Def.launchRange.x;
		rangeVisualizer.RangeMax.x = MissileLauncher.Def.launchRange.x;
		rangeVisualizer.RangeMin.y = 0;
		rangeVisualizer.RangeMax.y = MissileLauncher.Def.launchRange.y;
		rangeVisualizer.AllowLineOfSightInvalidCells = true;
		go.GetComponent<KPrefabID>().instantiateFn += delegate(GameObject go)
		{
			go.GetComponent<RangeVisualizer>().BlockingCb = IsCellSkyBlocked;
		};
	}

	public override void DoPostConfigureComplete(GameObject go)
	{
		SymbolOverrideControllerUtil.AddToPrefab(go);
		go.AddOrGet<TreeFilterable>().dropIncorrectOnFilterChange = false;
		FlatTagFilterable flatTagFilterable = go.AddOrGet<FlatTagFilterable>();
		flatTagFilterable.displayOnlyDiscoveredTags = false;
		flatTagFilterable.headerText = STRINGS.BUILDINGS.PREFABS.MISSILELAUNCHER.TARGET_SELECTION_HEADER;
	}

	public static bool IsCellSkyBlocked(int cell)
	{
		if (PlayerController.Instance != null)
		{
			int num = Grid.InvalidCell;
			BuildTool buildTool = PlayerController.Instance.ActiveTool as BuildTool;
			SelectTool selectTool = PlayerController.Instance.ActiveTool as SelectTool;
			if (buildTool != null)
			{
				num = buildTool.GetLastCell;
			}
			else if (selectTool != null)
			{
				num = Grid.PosToCell(selectTool.selected);
			}
			if (Grid.IsValidCell(cell) && Grid.IsValidCell(num) && Grid.WorldIdx[cell] == Grid.WorldIdx[num])
			{
				return Grid.Solid[cell];
			}
		}
		return false;
	}
}
