using STRINGS;
using UnityEngine;

public class DeathStates : GameStateMachine<DeathStates, DeathStates.Instance, IStateMachineTarget, DeathStates.Def>
{
	public class Def : BaseDef
	{
		public float DIE_ANIMATION_EXPIRATION_TIME = 4f;
	}

	public new class Instance : GameInstance
	{
		public Instance(Chore<Instance> chore, Def def)
			: base((IStateMachineTarget)chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Die);
		}

		public void EnableGravityIfNecessary()
		{
			if (HasTag(GameTags.Creatures.Flyer) && !HasTag(GameTags.Stored))
			{
				GameComps.Gravities.Add(base.smi.gameObject, Vector2.zero, delegate
				{
					base.smi.DisableGravity();
				});
			}
		}

		public void DisableGravity()
		{
			if (GameComps.Gravities.Has(base.smi.gameObject))
			{
				GameComps.Gravities.Remove(base.smi.gameObject);
			}
		}

		public void PlayDeathAnimations()
		{
			if (!base.gameObject.HasTag(GameTags.PreventDeadAnimation))
			{
				KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					component.Play("Death");
				}
			}
		}
	}

	private State loop;

	public State pst;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = loop;
		loop.ToggleStatusItem(CREATURES.STATUSITEMS.DEAD.NAME, CREATURES.STATUSITEMS.DEAD.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Enter("EnableGravity", delegate(Instance smi)
		{
			smi.EnableGravityIfNecessary();
		}).Enter("Play Death Animations", delegate(Instance smi)
		{
			smi.PlayDeathAnimations();
		})
			.OnAnimQueueComplete(pst)
			.ScheduleGoTo((Instance smi) => smi.def.DIE_ANIMATION_EXPIRATION_TIME, pst);
		pst.TriggerOnEnter(GameHashes.DeathAnimComplete).TriggerOnEnter(GameHashes.Died).Enter("Butcher", delegate(Instance smi)
		{
			if (smi.gameObject.GetComponent<Butcherable>() != null)
			{
				smi.GetComponent<Butcherable>().OnButcherComplete();
			}
		})
			.Enter("Destroy", delegate(Instance smi)
			{
				smi.gameObject.AddTag(GameTags.Dead);
				smi.gameObject.DeleteObject();
			})
			.BehaviourComplete(GameTags.Creatures.Die);
	}
}
