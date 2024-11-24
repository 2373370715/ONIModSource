using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020011B0 RID: 4528
public class MoltDropperMonitor : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>
{
	// Token: 0x06005C63 RID: 23651 RVA: 0x0029AEAC File Offset: 0x002990AC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.EventHandler(GameHashes.NewDay, (MoltDropperMonitor.Instance smi) => GameClock.Instance, delegate(MoltDropperMonitor.Instance smi)
		{
			smi.spawnedThisCycle = false;
		});
		this.satisfied.UpdateTransition(this.drop, (MoltDropperMonitor.Instance smi, float dt) => smi.ShouldDropElement(), UpdateRate.SIM_4000ms, false);
		this.drop.DefaultState(this.drop.dropping);
		this.drop.dropping.EnterTransition(this.drop.complete, (MoltDropperMonitor.Instance smi) => !smi.def.synchWithBehaviour).ToggleBehaviour(GameTags.Creatures.ReadyToMolt, (MoltDropperMonitor.Instance smi) => true, delegate(MoltDropperMonitor.Instance smi)
		{
			smi.GoTo(this.drop.complete);
		});
		this.drop.complete.Enter(delegate(MoltDropperMonitor.Instance smi)
		{
			smi.Drop();
		}).TriggerOnEnter(GameHashes.Molt, null).EventTransition(GameHashes.NewDay, (MoltDropperMonitor.Instance smi) => GameClock.Instance, this.satisfied, null);
	}

	// Token: 0x04004147 RID: 16711
	public StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter droppedThisCycle = new StateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.BoolParameter(false);

	// Token: 0x04004148 RID: 16712
	public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State satisfied;

	// Token: 0x04004149 RID: 16713
	public MoltDropperMonitor.DropStates drop;

	// Token: 0x020011B1 RID: 4529
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400414A RID: 16714
		public bool synchWithBehaviour;

		// Token: 0x0400414B RID: 16715
		public string onGrowDropID;

		// Token: 0x0400414C RID: 16716
		public float massToDrop;

		// Token: 0x0400414D RID: 16717
		public string amountName;

		// Token: 0x0400414E RID: 16718
		public Func<MoltDropperMonitor.Instance, bool> isReadyToMolt;
	}

	// Token: 0x020011B2 RID: 4530
	public class DropStates : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State
	{
		// Token: 0x0400414F RID: 16719
		public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State dropping;

		// Token: 0x04004150 RID: 16720
		public GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.State complete;
	}

	// Token: 0x020011B3 RID: 4531
	public new class Instance : GameStateMachine<MoltDropperMonitor, MoltDropperMonitor.Instance, IStateMachineTarget, MoltDropperMonitor.Def>.GameInstance
	{
		// Token: 0x06005C68 RID: 23656 RVA: 0x0029B038 File Offset: 0x00299238
		public Instance(IStateMachineTarget master, MoltDropperMonitor.Def def) : base(master, def)
		{
			if (!string.IsNullOrEmpty(def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(def.amountName).Lookup(base.smi.gameObject);
				amountInstance.OnMaxValueReached = (System.Action)Delegate.Combine(amountInstance.OnMaxValueReached, new System.Action(this.OnAmountMaxValueReached));
			}
		}

		// Token: 0x06005C69 RID: 23657 RVA: 0x000DC526 File Offset: 0x000DA726
		private void OnAmountMaxValueReached()
		{
			this.lastTineAmountReachedMax = GameClock.Instance.GetTime();
		}

		// Token: 0x06005C6A RID: 23658 RVA: 0x0029B0A0 File Offset: 0x002992A0
		protected override void OnCleanUp()
		{
			if (!string.IsNullOrEmpty(base.def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(base.def.amountName).Lookup(base.smi.gameObject);
				amountInstance.OnMaxValueReached = (System.Action)Delegate.Remove(amountInstance.OnMaxValueReached, new System.Action(this.OnAmountMaxValueReached));
			}
			base.OnCleanUp();
		}

		// Token: 0x06005C6B RID: 23659 RVA: 0x000DC538 File Offset: 0x000DA738
		public bool ShouldDropElement()
		{
			return base.def.isReadyToMolt(this);
		}

		// Token: 0x06005C6C RID: 23660 RVA: 0x0029B110 File Offset: 0x00299310
		public void Drop()
		{
			GameObject gameObject = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, base.def.onGrowDropID, Grid.SceneLayer.Ore);
			gameObject.SetActive(true);
			gameObject.GetComponent<PrimaryElement>().Mass = base.def.massToDrop;
			this.spawnedThisCycle = true;
			this.timeOfLastDrop = GameClock.Instance.GetTime();
			if (!string.IsNullOrEmpty(base.def.amountName))
			{
				AmountInstance amountInstance = Db.Get().Amounts.Get(base.def.amountName).Lookup(base.smi.gameObject);
				amountInstance.value = amountInstance.GetMin();
			}
		}

		// Token: 0x06005C6D RID: 23661 RVA: 0x0029B1B4 File Offset: 0x002993B4
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

		// Token: 0x04004151 RID: 16721
		[MyCmpGet]
		public KPrefabID prefabID;

		// Token: 0x04004152 RID: 16722
		[Serialize]
		public bool spawnedThisCycle;

		// Token: 0x04004153 RID: 16723
		[Serialize]
		public float timeOfLastDrop;

		// Token: 0x04004154 RID: 16724
		[Serialize]
		public float lastTineAmountReachedMax;
	}
}
