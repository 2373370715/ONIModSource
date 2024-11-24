using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class DeathStates : GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>
{
	// Token: 0x0600051F RID: 1311 RVA: 0x00158940 File Offset: 0x00156B40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State state = this.loop;
		string name = CREATURES.STATUSITEMS.DEAD.NAME;
		string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter("EnableGravity", delegate(DeathStates.Instance smi)
		{
			smi.EnableGravityIfNecessary();
		}).Enter("Play Death Animations", delegate(DeathStates.Instance smi)
		{
			smi.PlayDeathAnimations();
		}).OnAnimQueueComplete(this.pst).ScheduleGoTo((DeathStates.Instance smi) => smi.def.DIE_ANIMATION_EXPIRATION_TIME, this.pst);
		this.pst.TriggerOnEnter(GameHashes.DeathAnimComplete, null).TriggerOnEnter(GameHashes.Died, null).Enter("Butcher", delegate(DeathStates.Instance smi)
		{
			if (smi.gameObject.GetComponent<Butcherable>() != null)
			{
				smi.GetComponent<Butcherable>().OnButcherComplete();
			}
		}).Enter("Destroy", delegate(DeathStates.Instance smi)
		{
			smi.gameObject.AddTag(GameTags.Dead);
			smi.gameObject.DeleteObject();
		}).BehaviourComplete(GameTags.Creatures.Die, false);
	}

	// Token: 0x040003C7 RID: 967
	private GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State loop;

	// Token: 0x040003C8 RID: 968
	public GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.State pst;

	// Token: 0x02000160 RID: 352
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040003C9 RID: 969
		public float DIE_ANIMATION_EXPIRATION_TIME = 4f;
	}

	// Token: 0x02000161 RID: 353
	public new class Instance : GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>.GameInstance
	{
		// Token: 0x06000522 RID: 1314 RVA: 0x000A7FEF File Offset: 0x000A61EF
		public Instance(Chore<DeathStates.Instance> chore, DeathStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Die);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00158AA0 File Offset: 0x00156CA0
		public void EnableGravityIfNecessary()
		{
			if (base.HasTag(GameTags.Creatures.Flyer) && !base.HasTag(GameTags.Stored))
			{
				GameComps.Gravities.Add(base.smi.gameObject, Vector2.zero, delegate()
				{
					base.smi.DisableGravity();
				});
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000A8013 File Offset: 0x000A6213
		public void DisableGravity()
		{
			if (GameComps.Gravities.Has(base.smi.gameObject))
			{
				GameComps.Gravities.Remove(base.smi.gameObject);
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00158AF0 File Offset: 0x00156CF0
		public void PlayDeathAnimations()
		{
			if (base.gameObject.HasTag(GameTags.PreventDeadAnimation))
			{
				return;
			}
			KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("Death", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}
}
