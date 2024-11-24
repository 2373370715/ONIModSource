using UnityEngine;

public class MorbRoverMakerKeepsake : GameStateMachine<MorbRoverMakerKeepsake, MorbRoverMakerKeepsake.Instance, IStateMachineTarget, MorbRoverMakerKeepsake.Def>
{
	public class Def : BaseDef
	{
		public Vector2 OperationalRandomnessRange = new Vector2(120f, 600f);
	}

	public new class Instance : GameInstance
	{
		public float NextActivationTime = -1f;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public void CalculateNextActivationTime()
		{
			float time = GameClock.Instance.GetTime();
			float minInclusive = time + base.def.OperationalRandomnessRange.x;
			float maxInclusive = time + base.def.OperationalRandomnessRange.y;
			NextActivationTime = Random.Range(minInclusive, maxInclusive);
		}
	}

	public const string SILENT_ANIMATION_NAME = "silent";

	public const string TALKING_ANIMATION_NAME = "idle";

	public State silent;

	public State talking;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = silent;
		silent.PlayAnim("silent").Enter(CalculateNextActivationTime).Update(TimerUpdate);
		talking.PlayAnim("idle").OnAnimQueueComplete(silent);
	}

	public static void CalculateNextActivationTime(Instance smi)
	{
		smi.CalculateNextActivationTime();
	}

	public static void TimerUpdate(Instance smi, float dt)
	{
		if (GameClock.Instance.GetTime() > smi.NextActivationTime)
		{
			smi.GoTo(smi.sm.talking);
		}
	}
}
