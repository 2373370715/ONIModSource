using Database;
using KSerialization;
using TUNING;

public class EquippableBalloon : StateMachineComponent<EquippableBalloon.StatesInstance>
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, EquippableBalloon, object>.GameInstance
	{
		[Serialize]
		public float transitionTime;

		[Serialize]
		public string facadeAnim;

		[Serialize]
		public string symbolID;

		public StatesInstance(EquippableBalloon master)
			: base(master)
		{
		}
	}

	public class States : GameStateMachine<States, StatesInstance, EquippableBalloon>
	{
		public State destroy;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;
			base.serializable = SerializeType.Both_DEPRECATED;
			root.Transition(destroy, (StatesInstance smi) => GameClock.Instance.GetTime() >= smi.transitionTime);
			destroy.Enter(delegate(StatesInstance smi)
			{
				smi.master.GetComponent<Equippable>().Unassign();
			});
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		ApplyBalloonOverrideToBalloonFx();
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	public void SetBalloonOverride(BalloonOverrideSymbol balloonOverride)
	{
		base.smi.facadeAnim = balloonOverride.animFileID;
		base.smi.symbolID = balloonOverride.animFileSymbolID;
		ApplyBalloonOverrideToBalloonFx();
	}

	public void ApplyBalloonOverrideToBalloonFx()
	{
		Equippable component = GetComponent<Equippable>();
		if (component.IsNullOrDestroyed() || component.assignee.IsNullOrDestroyed())
		{
			return;
		}
		Ownables soleOwner = component.assignee.GetSoleOwner();
		if (!soleOwner.IsNullOrDestroyed())
		{
			BalloonFX.Instance sMI = ((KMonoBehaviour)soleOwner.GetComponent<MinionAssignablesProxy>().target).GetSMI<BalloonFX.Instance>();
			if (!sMI.IsNullOrDestroyed())
			{
				new BalloonOverrideSymbol(base.smi.facadeAnim, base.smi.symbolID).ApplyTo(sMI);
			}
		}
	}
}
