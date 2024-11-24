using STRINGS;
using UnityEngine;

public class TrappedStates : GameStateMachine<TrappedStates, TrappedStates.Instance, IStateMachineTarget, TrappedStates.Def>
{
	public class Def : BaseDef
	{
	}

	public interface ITrapStateAnimationInstructions
	{
		string GetTrappedAnimationName();
	}

	public new class Instance : GameInstance
	{
		public static readonly Chore.Precondition IsTrapped = new Chore.Precondition
		{
			id = "IsTrapped",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Trapped);
			}
		};

		public Instance(Chore<Instance> chore, Def def)
			: base((IStateMachineTarget)chore, def)
		{
			chore.AddPrecondition(IsTrapped);
		}
	}

	public const string DEFAULT_TRAPPED_ANIM_NAME = "trapped";

	private State trapped;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = trapped;
		root.ToggleStatusItem(CREATURES.STATUSITEMS.TRAPPED.NAME, CREATURES.STATUSITEMS.TRAPPED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		trapped.Enter(delegate(Instance smi)
		{
			Navigator component = smi.GetComponent<Navigator>();
			if (component.IsValidNavType(NavType.Floor))
			{
				component.SetCurrentNavType(NavType.Floor);
			}
		}).ToggleTag(GameTags.Creatures.Deliverable).PlayAnim(GetTrappedAnimName, KAnim.PlayMode.Loop)
			.TagTransition(GameTags.Trapped, null, on_remove: true);
	}

	public static string GetTrappedAnimName(Instance smi)
	{
		string result = "trapped";
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		Pickupable component = smi.gameObject.GetComponent<Pickupable>();
		GameObject gameObject = ((component != null) ? component.storage.gameObject : Grid.Objects[cell, 1]);
		if (gameObject != null)
		{
			if (gameObject.GetComponent<ITrapStateAnimationInstructions>() != null)
			{
				string trappedAnimationName = gameObject.GetComponent<ITrapStateAnimationInstructions>().GetTrappedAnimationName();
				if (trappedAnimationName != null)
				{
					return trappedAnimationName;
				}
			}
			if (gameObject.GetSMI<ITrapStateAnimationInstructions>() != null)
			{
				string trappedAnimationName2 = gameObject.GetSMI<ITrapStateAnimationInstructions>().GetTrappedAnimationName();
				if (trappedAnimationName2 != null)
				{
					return trappedAnimationName2;
				}
			}
		}
		Trappable component2 = smi.gameObject.GetComponent<Trappable>();
		if (component2 != null && component2.HasTag(GameTags.Creatures.Swimmer) && Grid.IsValidCell(cell) && !Grid.IsLiquid(cell))
		{
			result = "trapped_onLand";
		}
		return result;
	}
}
