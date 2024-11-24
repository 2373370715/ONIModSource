using System;
using UnityEngine;

// Token: 0x020017DB RID: 6107
public class SweepBotTrappedMonitor : GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>
{
	// Token: 0x06007DAB RID: 32171 RVA: 0x00327634 File Offset: 0x00325834
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notTrapped;
		this.notTrapped.Update(delegate(SweepBotTrappedMonitor.Instance smi, float dt)
		{
			StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
			Storage storage = smi2.sm.sweepLocker.Get(smi2);
			Navigator component = smi.master.GetComponent<Navigator>();
			if (storage == null)
			{
				smi.GoTo(this.death);
				return;
			}
			if ((smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) || smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.UnloadBehaviour)) && !component.CanReach(Grid.PosToCell(storage), SweepBotTrappedMonitor.defaultOffsets))
			{
				smi.GoTo(this.trapped);
			}
		}, UpdateRate.SIM_1000ms, false);
		this.trapped.ToggleBehaviour(GameTags.Robots.Behaviours.TrappedBehaviour, (SweepBotTrappedMonitor.Instance data) => true, null).ToggleStatusItem(Db.Get().RobotStatusItems.CantReachStation, null, Db.Get().StatusItemCategories.Main).Update(delegate(SweepBotTrappedMonitor.Instance smi, float dt)
		{
			StorageUnloadMonitor.Instance smi2 = smi.master.gameObject.GetSMI<StorageUnloadMonitor.Instance>();
			Storage storage = smi2.sm.sweepLocker.Get(smi2);
			Navigator component = smi.master.GetComponent<Navigator>();
			if (storage == null)
			{
				smi.GoTo(this.death);
			}
			else if ((!smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.RechargeBehaviour) && !smi.master.gameObject.HasTag(GameTags.Robots.Behaviours.UnloadBehaviour)) || component.CanReach(Grid.PosToCell(storage), SweepBotTrappedMonitor.defaultOffsets))
			{
				smi.GoTo(this.notTrapped);
			}
			if (storage != null && component.CanReach(Grid.PosToCell(storage), SweepBotTrappedMonitor.defaultOffsets))
			{
				smi.GoTo(this.notTrapped);
				return;
			}
			if (storage == null)
			{
				smi.GoTo(this.death);
			}
		}, UpdateRate.SIM_1000ms, false);
		this.death.Enter(delegate(SweepBotTrappedMonitor.Instance smi)
		{
			smi.master.gameObject.GetComponent<OrnamentReceptacle>().OrderRemoveOccupant();
			smi.master.gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("death");
		}).OnAnimQueueComplete(this.destroySelf);
		this.destroySelf.Enter(delegate(SweepBotTrappedMonitor.Instance smi)
		{
			Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, smi.master.transform.position, 0f);
			foreach (Storage storage in smi.master.gameObject.GetComponents<Storage>())
			{
				for (int j = storage.items.Count - 1; j >= 0; j--)
				{
					GameObject gameObject = storage.Drop(storage.items[j], true);
					if (gameObject != null)
					{
						if (GameComps.Fallers.Has(gameObject))
						{
							GameComps.Fallers.Remove(gameObject);
						}
						GameComps.Fallers.Add(gameObject, new Vector2((float)UnityEngine.Random.Range(-5, 5), 8f));
					}
				}
			}
			PrimaryElement component = smi.master.GetComponent<PrimaryElement>();
			component.Element.substance.SpawnResource(Grid.CellToPosCCC(Grid.PosToCell(smi.master.gameObject), Grid.SceneLayer.Ore), SweepBotConfig.MASS, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, false, false);
			Util.KDestroyGameObject(smi.master.gameObject);
		});
	}

	// Token: 0x04005F30 RID: 24368
	public static CellOffset[] defaultOffsets = new CellOffset[]
	{
		new CellOffset(0, 0)
	};

	// Token: 0x04005F31 RID: 24369
	public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State notTrapped;

	// Token: 0x04005F32 RID: 24370
	public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State trapped;

	// Token: 0x04005F33 RID: 24371
	public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State death;

	// Token: 0x04005F34 RID: 24372
	public GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.State destroySelf;

	// Token: 0x020017DC RID: 6108
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020017DD RID: 6109
	public new class Instance : GameStateMachine<SweepBotTrappedMonitor, SweepBotTrappedMonitor.Instance, IStateMachineTarget, SweepBotTrappedMonitor.Def>.GameInstance
	{
		// Token: 0x06007DB1 RID: 32177 RVA: 0x000F2CFE File Offset: 0x000F0EFE
		public Instance(IStateMachineTarget master, SweepBotTrappedMonitor.Def def) : base(master, def)
		{
		}
	}
}
