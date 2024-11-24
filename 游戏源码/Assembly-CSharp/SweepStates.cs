using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000552 RID: 1362
public class SweepStates : GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>
{
	// Token: 0x06001807 RID: 6151 RVA: 0x0019C8B0 File Offset: 0x0019AAB0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.beginPatrol;
		this.beginPatrol.Enter(delegate(SweepStates.Instance smi)
		{
			smi.sm.timeUntilBored.Set(30f, smi, false);
			smi.GoTo(this.moving);
			SweepStates.Instance smi2 = smi;
			smi2.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi2.OnStop, new Action<string, StateMachine.Status>(delegate(string data, StateMachine.Status status)
			{
				this.StopMoveSound(smi);
			}));
		});
		this.moving.Enter(delegate(SweepStates.Instance smi)
		{
		}).MoveTo((SweepStates.Instance smi) => this.GetNextCell(smi), this.pause, this.redirected, false).Update(delegate(SweepStates.Instance smi, float dt)
		{
			smi.sm.timeUntilBored.Set(smi.sm.timeUntilBored.Get(smi) - dt, smi, false);
			if (smi.sm.timeUntilBored.Get(smi) <= 0f)
			{
				smi.sm.bored.Set(true, smi, false);
				smi.sm.timeUntilBored.Set(30f, smi, false);
				smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("react_bored");
			}
			StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
			Storage storage = smi2.sm.sweepLocker.Get(smi2);
			if (storage != null && smi.sm.headingRight.Get(smi) == smi.master.transform.position.x > storage.transform.position.x)
			{
				Navigator component = smi.master.gameObject.GetComponent<Navigator>();
				if (component.GetNavigationCost(Grid.PosToCell(storage)) >= component.maxProbingRadius - 1)
				{
					smi.GoTo(smi.sm.emoteRedirected);
				}
			}
		}, UpdateRate.SIM_1000ms, false);
		this.emoteRedirected.Enter(delegate(SweepStates.Instance smi)
		{
			this.StopMoveSound(smi);
			int cell = Grid.PosToCell(smi.master.gameObject);
			if (Grid.IsCellOffsetValid(cell, this.headingRight.Get(smi) ? 1 : -1, -1) && !Grid.Solid[Grid.OffsetCell(cell, this.headingRight.Get(smi) ? 1 : -1, -1)])
			{
				smi.Play("gap", KAnim.PlayMode.Once);
			}
			else
			{
				smi.Play("bump", KAnim.PlayMode.Once);
			}
			this.headingRight.Set(!this.headingRight.Get(smi), smi, false);
		}).OnAnimQueueComplete(this.pause);
		this.redirected.StopMoving().GoTo(this.emoteRedirected);
		this.sweep.PlayAnim("pickup").ToggleEffect("BotSweeping").Enter(delegate(SweepStates.Instance smi)
		{
			this.StopMoveSound(smi);
			smi.sm.bored.Set(false, smi, false);
			smi.sm.timeUntilBored.Set(30f, smi, false);
		}).OnAnimQueueComplete(this.moving);
		this.pause.Enter(delegate(SweepStates.Instance smi)
		{
			if (Grid.IsLiquid(Grid.PosToCell(smi)))
			{
				smi.GoTo(this.mopping);
				return;
			}
			if (this.TrySweep(smi))
			{
				smi.GoTo(this.sweep);
				return;
			}
			smi.GoTo(this.moving);
		});
		this.mopping.PlayAnim("mop_pre", KAnim.PlayMode.Once).QueueAnim("mop_loop", true, null).ToggleEffect("BotMopping").Enter(delegate(SweepStates.Instance smi)
		{
			smi.sm.timeUntilBored.Set(30f, smi, false);
			smi.sm.bored.Set(false, smi, false);
			this.StopMoveSound(smi);
		}).Update(delegate(SweepStates.Instance smi, float dt)
		{
			if (smi.timeinstate > 16f || !Grid.IsLiquid(Grid.PosToCell(smi)))
			{
				smi.GoTo(this.moving);
				return;
			}
			this.TryMop(smi, dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x000B023D File Offset: 0x000AE43D
	public void StopMoveSound(SweepStates.Instance smi)
	{
		LoopingSounds component = smi.gameObject.GetComponent<LoopingSounds>();
		component.StopSound(GlobalAssets.GetSound("SweepBot_mvmt_lp", false));
		component.StopAllSounds();
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x0019CA24 File Offset: 0x0019AC24
	public void StartMoveSound(SweepStates.Instance smi)
	{
		LoopingSounds component = smi.gameObject.GetComponent<LoopingSounds>();
		if (!component.IsSoundPlaying(GlobalAssets.GetSound("SweepBot_mvmt_lp", false)))
		{
			component.StartSound(GlobalAssets.GetSound("SweepBot_mvmt_lp", false));
		}
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x0019CA64 File Offset: 0x0019AC64
	public void TryMop(SweepStates.Instance smi, float dt)
	{
		int cell = Grid.PosToCell(smi);
		if (Grid.IsLiquid(cell))
		{
			Moppable.MopCell(cell, Mathf.Min(Grid.Mass[cell], 10f * dt), delegate(Sim.MassConsumedCallback mass_cb_info, object data)
			{
				if (this == null)
				{
					return;
				}
				if (mass_cb_info.mass > 0f)
				{
					SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(ElementLoader.elements[(int)mass_cb_info.elemIdx], mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
					substanceChunk.transform.SetPosition(substanceChunk.transform.GetPosition() + new Vector3((UnityEngine.Random.value - 0.5f) * 0.5f, 0f, 0f));
					this.TryStore(substanceChunk.gameObject, smi);
				}
			});
		}
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x0019CAD8 File Offset: 0x0019ACD8
	public bool TrySweep(SweepStates.Instance smi)
	{
		int cell = Grid.PosToCell(smi);
		GameObject gameObject = Grid.Objects[cell, 3];
		if (gameObject != null)
		{
			ObjectLayerListItem nextItem = gameObject.GetComponent<Pickupable>().objectLayerListItem.nextItem;
			return nextItem != null && this.TryStore(nextItem.gameObject, smi);
		}
		return false;
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x0019CB28 File Offset: 0x0019AD28
	public bool TryStore(GameObject go, SweepStates.Instance smi)
	{
		Pickupable pickupable = go.GetComponent<Pickupable>();
		if (pickupable == null)
		{
			return false;
		}
		Storage storage = smi.master.gameObject.GetComponents<Storage>()[1];
		if (storage.IsFull())
		{
			return false;
		}
		if (pickupable != null && pickupable.absorbable)
		{
			SingleEntityReceptacle component = smi.master.GetComponent<SingleEntityReceptacle>();
			if (pickupable.gameObject == component.Occupant)
			{
				return false;
			}
			bool flag;
			if (pickupable.TotalAmount > 10f)
			{
				pickupable.GetComponent<EntitySplitter>();
				pickupable = EntitySplitter.Split(pickupable, Mathf.Min(10f, storage.RemainingCapacity()), null);
				smi.gameObject.GetAmounts().GetValue(Db.Get().Amounts.InternalBattery.Id);
				storage.Store(pickupable.gameObject, false, false, true, false);
				flag = true;
			}
			else
			{
				smi.gameObject.GetAmounts().GetValue(Db.Get().Amounts.InternalBattery.Id);
				storage.Store(pickupable.gameObject, false, false, true, false);
				flag = true;
			}
			if (flag)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x0019CC44 File Offset: 0x0019AE44
	public int GetNextCell(SweepStates.Instance smi)
	{
		int i = 0;
		int num = Grid.PosToCell(smi);
		int num2 = Grid.InvalidCell;
		if (!Grid.Solid[Grid.CellBelow(num)])
		{
			return Grid.InvalidCell;
		}
		if (Grid.Solid[num])
		{
			return Grid.InvalidCell;
		}
		while (i < 1)
		{
			num2 = (smi.sm.headingRight.Get(smi) ? Grid.CellRight(num) : Grid.CellLeft(num));
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || !Grid.IsValidCell(Grid.CellBelow(num2)) || !Grid.Solid[Grid.CellBelow(num2)])
			{
				break;
			}
			num = num2;
			i++;
		}
		if (num == Grid.PosToCell(smi))
		{
			return Grid.InvalidCell;
		}
		return num;
	}

	// Token: 0x04000F89 RID: 3977
	public const float TIME_UNTIL_BORED = 30f;

	// Token: 0x04000F8A RID: 3978
	public const string MOVE_LOOP_SOUND = "SweepBot_mvmt_lp";

	// Token: 0x04000F8B RID: 3979
	public StateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.BoolParameter headingRight;

	// Token: 0x04000F8C RID: 3980
	private StateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.FloatParameter timeUntilBored;

	// Token: 0x04000F8D RID: 3981
	public StateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.BoolParameter bored;

	// Token: 0x04000F8E RID: 3982
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State beginPatrol;

	// Token: 0x04000F8F RID: 3983
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State moving;

	// Token: 0x04000F90 RID: 3984
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State pause;

	// Token: 0x04000F91 RID: 3985
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State mopping;

	// Token: 0x04000F92 RID: 3986
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State redirected;

	// Token: 0x04000F93 RID: 3987
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State emoteRedirected;

	// Token: 0x04000F94 RID: 3988
	private GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.State sweep;

	// Token: 0x02000553 RID: 1363
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000554 RID: 1364
	public new class Instance : GameStateMachine<SweepStates, SweepStates.Instance, IStateMachineTarget, SweepStates.Def>.GameInstance
	{
		// Token: 0x06001817 RID: 6167 RVA: 0x000B034A File Offset: 0x000AE54A
		public Instance(Chore<SweepStates.Instance> chore, SweepStates.Def def) : base(chore, def)
		{
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x000B0354 File Offset: 0x000AE554
		public override void StartSM()
		{
			base.StartSM();
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().RobotStatusItems.Working, base.gameObject);
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x000B038C File Offset: 0x000AE58C
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().RobotStatusItems.Working, false);
		}
	}
}
