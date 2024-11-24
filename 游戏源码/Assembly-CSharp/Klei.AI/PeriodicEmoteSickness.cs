using UnityEngine;

namespace Klei.AI;

public class PeriodicEmoteSickness : Sickness.SicknessComponent
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, SicknessInstance, object>.GameInstance
	{
		public PeriodicEmoteSickness periodicEmoteSickness;

		public StatesInstance(SicknessInstance master, PeriodicEmoteSickness periodicEmoteSickness)
			: base(master)
		{
			this.periodicEmoteSickness = periodicEmoteSickness;
		}

		public Reactable GetReactable()
		{
			return new SelfEmoteReactable(base.master.gameObject, "PeriodicEmoteSickness", Db.Get().ChoreTypes.Emote, 0f, periodicEmoteSickness.cooldown).SetEmote(periodicEmoteSickness.emote).SetOverideAnimSet("anim_sneeze_kanim");
		}
	}

	public class States : GameStateMachine<States, StatesInstance, SicknessInstance>
	{
		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;
			root.ToggleReactable((StatesInstance smi) => smi.GetReactable());
		}
	}

	private Emote emote;

	private float cooldown;

	public PeriodicEmoteSickness(Emote emote, float cooldown)
	{
		this.emote = emote;
		this.cooldown = cooldown;
	}

	public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
	{
		StatesInstance statesInstance = new StatesInstance(diseaseInstance, this);
		statesInstance.StartSM();
		return statesInstance;
	}

	public override void OnCure(GameObject go, object instance_data)
	{
		((StatesInstance)instance_data).StopSM("Cured");
	}
}
