using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class FoodDehydrator : GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>
{
	public class Def : BaseDef, IGameObjectEffectDescriptor
	{
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			Descriptor item = new Descriptor(UI.BUILDINGEFFECTS.FOOD_DEHYDRATOR_WATER_OUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.FOOD_DEHYDRATOR_WATER_OUTPUT);
			list.Add(item);
			return list;
		}
	}

	public class StatesInstance : GameInstance
	{
		[MyCmpReq]
		public Operational operational;

		[MyCmpReq]
		public ComplexFabricator complexFabricator;

		private static string HASH_FOOD = "food";

		private KBatchedAnimController foodKBAC;

		private int foodIngredientIdx;

		public StatesInstance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			SetupFoodSymbol();
		}

		public float GetAvailableFuel()
		{
			return complexFabricator.inStorage.GetMassAvailable(FOODDEHYDRATORTUNING.FUEL_TAG);
		}

		public bool RequiresEmptying()
		{
			return !complexFabricator.outStorage.IsEmpty();
		}

		public void OnEmptyComplete(Chore obj)
		{
			Vector3 position = Grid.CellToPosLCC(Grid.PosToCell(this), Grid.SceneLayer.Ore);
			complexFabricator.outStorage.DropAll(position, vent_gas: false, dump_liquid: true);
		}

		public void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "food_symbol");
			gameObject.SetActive(value: false);
			KBatchedAnimController component = GetComponent<KBatchedAnimController>();
			bool symbolVisible;
			Vector3 position = component.GetSymbolTransform(HASH_FOOD, out symbolVisible).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			foodKBAC.AnimFiles = new KAnimFile[1] { Assets.GetAnim("mushbar_kanim") };
			foodKBAC.initialAnim = "object";
			component.SetSymbolVisiblity(HASH_FOOD, is_visible: false);
			foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
			KBatchedAnimTracker kBatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
			kBatchedAnimTracker.symbol = new HashedString("food");
			kBatchedAnimTracker.offset = Vector3.zero;
		}

		public void UpdateFoodSymbol()
		{
			ComplexRecipe currentWorkingOrder = complexFabricator.CurrentWorkingOrder;
			if (complexFabricator.CurrentWorkingOrder != null)
			{
				foodKBAC.gameObject.SetActive(value: true);
				GameObject prefab = Assets.GetPrefab(currentWorkingOrder.ingredients[foodIngredientIdx].material);
				foodKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
				foodKBAC.Play("object", KAnim.PlayMode.Loop);
			}
			else if (complexFabricator.outStorage.items.Count > 0)
			{
				foodKBAC.gameObject.SetActive(value: true);
				foodKBAC.SwapAnims(complexFabricator.outStorage.items[0].GetComponent<KBatchedAnimController>().AnimFiles);
				foodKBAC.Play("object", KAnim.PlayMode.Loop);
			}
			else
			{
				foodKBAC.gameObject.SetActive(value: false);
			}
		}
	}

	private StatusItem waitingForFuelStatus = new StatusItem("waitingForFuelStatus", BUILDING.STATUSITEMS.ENOUGH_FUEL.NAME, BUILDING.STATUSITEMS.ENOUGH_FUEL.TOOLTIP, "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);

	private static readonly Operational.Flag foodDehydratorFlag = new Operational.Flag("food_dehydrator", Operational.Flag.Type.Requirement);

	private State waitingForFuel;

	private State working;

	private State requestEmpty;

	public override void InitializeStates(out BaseState default_state)
	{
		waitingForFuelStatus.resolveStringCallback = (string str, object obj) => string.Format(str, FOODDEHYDRATORTUNING.FUEL_TAG.ProperName(), GameUtil.GetFormattedMass(5.0000005f));
		default_state = waitingForFuel;
		waitingForFuel.Enter(delegate(StatesInstance smi)
		{
			smi.operational.SetFlag(foodDehydratorFlag, value: false);
		}).EventTransition(GameHashes.OnStorageChange, working, (StatesInstance smi) => smi.GetAvailableFuel() >= 5.0000005f).ToggleStatusItem(waitingForFuelStatus);
		working.Enter(delegate(StatesInstance smi)
		{
			smi.complexFabricator.SetQueueDirty();
			smi.operational.SetFlag(foodDehydratorFlag, value: true);
		}).EventHandler(GameHashes.FabricatorOrdersUpdated, delegate(StatesInstance smi)
		{
			smi.UpdateFoodSymbol();
		}).EnterTransition(requestEmpty, (StatesInstance smi) => smi.RequiresEmptying())
			.EventTransition(GameHashes.OnStorageChange, waitingForFuel, (StatesInstance smi) => smi.GetAvailableFuel() <= 0f)
			.EventHandlerTransition(GameHashes.FabricatorOrderCompleted, requestEmpty, (StatesInstance smi, object data) => smi.RequiresEmptying())
			.EventHandler(GameHashes.FabricatorOrderStarted, delegate(StatesInstance smi)
			{
				smi.UpdateFoodSymbol();
			});
		requestEmpty.ToggleRecurringChore(CreateChore, (StatesInstance smi) => smi.RequiresEmptying()).EventHandlerTransition(GameHashes.OnStorageChange, working, (StatesInstance smi, object data) => !smi.RequiresEmptying()).Enter(delegate(StatesInstance smi)
		{
			smi.operational.SetFlag(foodDehydratorFlag, value: false);
			smi.UpdateFoodSymbol();
		})
			.ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding);
	}

	private Chore CreateChore(StatesInstance smi)
	{
		WorkChore<FoodDehydratorWorkableEmpty> workChore = new WorkChore<FoodDehydratorWorkableEmpty>(Db.Get().ChoreTypes.FoodFetch, smi.master.GetComponent<FoodDehydratorWorkableEmpty>(), null, run_until_complete: true, smi.OnEmptyComplete, null, null, allow_in_red_alert: true, null, ignore_schedule_block: false, only_when_operational: false);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot);
		return workChore;
	}
}
