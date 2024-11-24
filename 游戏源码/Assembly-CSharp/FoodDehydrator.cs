using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D7F RID: 3455
public class FoodDehydrator : GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>
{
	// Token: 0x060043B9 RID: 17337 RVA: 0x00245EF4 File Offset: 0x002440F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		this.waitingForFuelStatus.resolveStringCallback = ((string str, object obj) => string.Format(str, FOODDEHYDRATORTUNING.FUEL_TAG.ProperName(), GameUtil.GetFormattedMass(5.0000005f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")));
		default_state = this.waitingForFuel;
		this.waitingForFuel.Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, false);
		}).EventTransition(GameHashes.OnStorageChange, this.working, (FoodDehydrator.StatesInstance smi) => smi.GetAvailableFuel() >= 5.0000005f).ToggleStatusItem(this.waitingForFuelStatus, null);
		this.working.Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.complexFabricator.SetQueueDirty();
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, true);
		}).EventHandler(GameHashes.FabricatorOrdersUpdated, delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.UpdateFoodSymbol();
		}).EnterTransition(this.requestEmpty, (FoodDehydrator.StatesInstance smi) => smi.RequiresEmptying()).EventTransition(GameHashes.OnStorageChange, this.waitingForFuel, (FoodDehydrator.StatesInstance smi) => smi.GetAvailableFuel() <= 0f).EventHandlerTransition(GameHashes.FabricatorOrderCompleted, this.requestEmpty, (FoodDehydrator.StatesInstance smi, object data) => smi.RequiresEmptying()).EventHandler(GameHashes.FabricatorOrderStarted, delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.UpdateFoodSymbol();
		});
		this.requestEmpty.ToggleRecurringChore(new Func<FoodDehydrator.StatesInstance, Chore>(this.CreateChore), (FoodDehydrator.StatesInstance smi) => smi.RequiresEmptying()).EventHandlerTransition(GameHashes.OnStorageChange, this.working, (FoodDehydrator.StatesInstance smi, object data) => !smi.RequiresEmptying()).Enter(delegate(FoodDehydrator.StatesInstance smi)
		{
			smi.operational.SetFlag(FoodDehydrator.foodDehydratorFlag, false);
			smi.UpdateFoodSymbol();
		}).ToggleStatusItem(Db.Get().BuildingStatusItems.AwaitingEmptyBuilding, null);
	}

	// Token: 0x060043BA RID: 17338 RVA: 0x00246140 File Offset: 0x00244340
	private Chore CreateChore(FoodDehydrator.StatesInstance smi)
	{
		WorkChore<FoodDehydratorWorkableEmpty> workChore = new WorkChore<FoodDehydratorWorkableEmpty>(Db.Get().ChoreTypes.FoodFetch, smi.master.GetComponent<FoodDehydratorWorkableEmpty>(), null, true, new Action<Chore>(smi.OnEmptyComplete), null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		return workChore;
	}

	// Token: 0x04002E70 RID: 11888
	private StatusItem waitingForFuelStatus = new StatusItem("waitingForFuelStatus", BUILDING.STATUSITEMS.ENOUGH_FUEL.NAME, BUILDING.STATUSITEMS.ENOUGH_FUEL.TOOLTIP, "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);

	// Token: 0x04002E71 RID: 11889
	private static readonly Operational.Flag foodDehydratorFlag = new Operational.Flag("food_dehydrator", Operational.Flag.Type.Requirement);

	// Token: 0x04002E72 RID: 11890
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State waitingForFuel;

	// Token: 0x04002E73 RID: 11891
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State working;

	// Token: 0x04002E74 RID: 11892
	private GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.State requestEmpty;

	// Token: 0x02000D80 RID: 3456
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x060043BD RID: 17341 RVA: 0x002461E8 File Offset: 0x002443E8
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			List<Descriptor> list = new List<Descriptor>();
			Descriptor item = new Descriptor(UI.BUILDINGEFFECTS.FOOD_DEHYDRATOR_WATER_OUTPUT, UI.BUILDINGEFFECTS.TOOLTIPS.FOOD_DEHYDRATOR_WATER_OUTPUT, Descriptor.DescriptorType.Effect, false);
			list.Add(item);
			return list;
		}
	}

	// Token: 0x02000D81 RID: 3457
	public class StatesInstance : GameStateMachine<FoodDehydrator, FoodDehydrator.StatesInstance, IStateMachineTarget, FoodDehydrator.Def>.GameInstance
	{
		// Token: 0x060043BF RID: 17343 RVA: 0x000CBCC2 File Offset: 0x000C9EC2
		public StatesInstance(IStateMachineTarget master, FoodDehydrator.Def def) : base(master, def)
		{
			this.SetupFoodSymbol();
		}

		// Token: 0x060043C0 RID: 17344 RVA: 0x000CBCD2 File Offset: 0x000C9ED2
		public float GetAvailableFuel()
		{
			return this.complexFabricator.inStorage.GetMassAvailable(FOODDEHYDRATORTUNING.FUEL_TAG);
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x000CBCE9 File Offset: 0x000C9EE9
		public bool RequiresEmptying()
		{
			return !this.complexFabricator.outStorage.IsEmpty();
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x00246220 File Offset: 0x00244420
		public void OnEmptyComplete(Chore obj)
		{
			Vector3 position = Grid.CellToPosLCC(Grid.PosToCell(this), Grid.SceneLayer.Ore);
			this.complexFabricator.outStorage.DropAll(position, false, true, default(Vector3), true, null);
		}

		// Token: 0x060043C3 RID: 17347 RVA: 0x0024625C File Offset: 0x0024445C
		public void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "food_symbol");
			gameObject.SetActive(false);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			bool flag;
			Vector3 position = component.GetSymbolTransform(FoodDehydrator.StatesInstance.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			component.SetSymbolVisiblity(FoodDehydrator.StatesInstance.HASH_FOOD, false);
			this.foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
			KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = new HashedString("food");
			kbatchedAnimTracker.offset = Vector3.zero;
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x00246344 File Offset: 0x00244544
		public void UpdateFoodSymbol()
		{
			ComplexRecipe currentWorkingOrder = this.complexFabricator.CurrentWorkingOrder;
			if (this.complexFabricator.CurrentWorkingOrder != null)
			{
				this.foodKBAC.gameObject.SetActive(true);
				GameObject prefab = Assets.GetPrefab(currentWorkingOrder.ingredients[this.foodIngredientIdx].material);
				this.foodKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			if (this.complexFabricator.outStorage.items.Count > 0)
			{
				this.foodKBAC.gameObject.SetActive(true);
				this.foodKBAC.SwapAnims(this.complexFabricator.outStorage.items[0].GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
			this.foodKBAC.gameObject.SetActive(false);
		}

		// Token: 0x04002E75 RID: 11893
		[MyCmpReq]
		public Operational operational;

		// Token: 0x04002E76 RID: 11894
		[MyCmpReq]
		public ComplexFabricator complexFabricator;

		// Token: 0x04002E77 RID: 11895
		private static string HASH_FOOD = "food";

		// Token: 0x04002E78 RID: 11896
		private KBatchedAnimController foodKBAC;

		// Token: 0x04002E79 RID: 11897
		private int foodIngredientIdx;
	}
}
