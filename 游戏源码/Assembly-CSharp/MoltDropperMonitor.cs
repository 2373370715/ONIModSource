using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

public class MoltDropperMonitor : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>
{
	public class Def : BaseDef
	{
		public bool synchWithBehaviour;

		public string onGrowDropID;

		public float massToDrop;

		public string amountName;

		public Func<Instance, bool> isReadyToMolt;
	}

	public class DropStates : State
	{
		public State dropping;

		public State complete;
	}

	public new class Instance : GameInstance
	{
		[MyCmpGet]
		public KPrefabID prefabID;

		[Serialize]
		public bool spawnedThisCycle;

		[Serialize]
		public float timeOfLastDrop;

		[Serialize]
		public float lastTineAmountReachedMax;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			if (!string.IsNullOrEmpty(def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(def.amountName).Lookup(base.smi.gameObject);
				amountInstance.OnMaxValueReached = (System.Action)Delegate.Combine(amountInstance.OnMaxValueReached, new System.Action(OnAmountMaxValueReached));
			}
		}

		private void OnAmountMaxValueReached()
		{
			lastTineAmountReachedMax = GameClock.Instance.GetTime();
		}

		protected override void OnCleanUp()
		{
			if (!string.IsNullOrEmpty(base.def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(base.def.amountName).Lookup(base.smi.gameObject);
				amountInstance.OnMaxValueReached = (System.Action)Delegate.Remove(amountInstance.OnMaxValueReached, new System.Action(OnAmountMaxValueReached));
			}
			base.OnCleanUp();
		}

		public bool ShouldDropElement()
		{
			return base.def.isReadyToMolt(this);
		}

		public void Drop()
		{
			GameObject obj = Scenario.SpawnPrefab(GetDropSpawnLocation(), 0, 0, base.def.onGrowDropID);
			obj.SetActive(value: true);
			obj.GetComponent<PrimaryElement>().Mass = base.def.massToDrop;
			spawnedThisCycle = true;
			timeOfLastDrop = GameClock.Instance.GetTime();
			if (!string.IsNullOrEmpty(base.def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(base.def.amountName).Lookup(base.smi.gameObject);
				amountInstance.value = amountInstance.GetMin();
			}
		}

		private int GetDropSpawnLocation()
		{
			int num = Grid.PosToCell(base.gameObject);
			int num2 = Grid.CellAbove(num);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				return num2;
			}
			return num;
		}
	}

	public BoolParameter droppedThisCycle = new BoolParameter(default_value: false);

	public State satisfied;

	public DropStates drop;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = satisfied;
		root.EventHandler(GameHashes.NewDay, (Instance smi) => GameClock.Instance, delegate(Instance smi)
		{
			smi.spawnedThisCycle = false;
		});
		satisfied.UpdateTransition(drop, (Instance smi, float dt) => smi.ShouldDropElement(), UpdateRate.SIM_4000ms);
		drop.DefaultState(drop.dropping);
		drop.dropping.EnterTransition(drop.complete, (Instance smi) => !smi.def.synchWithBehaviour).ToggleBehaviour(GameTags.Creatures.ReadyToMolt, (Instance smi) => true, delegate(Instance smi)
		{
			smi.GoTo(drop.complete);
		});
		drop.complete.Enter(delegate(Instance smi)
		{
			smi.Drop();
		}).TriggerOnEnter(GameHashes.Molt).EventTransition(GameHashes.NewDay, (Instance smi) => GameClock.Instance, satisfied);
	}
}
