using System;
using Klei.AI;
using UnityEngine;

public class EmoteChore : Chore<EmoteChore.StatesInstance>
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, EmoteChore, object>.GameInstance
	{
		public KAnimFile animFile;

		public HashedString[] emoteAnims;

		public KAnim.PlayMode mode = KAnim.PlayMode.Once;

		public StatesInstance(EmoteChore master, GameObject emoter, Emote emote, KAnim.PlayMode mode, int emoteIterations, bool flip_x)
			: base(master)
		{
			this.mode = mode;
			animFile = emote.AnimSet;
			emote.CollectStepAnims(out emoteAnims, emoteIterations);
			base.sm.emoter.Set(emoter, base.smi);
		}

		public StatesInstance(EmoteChore master, GameObject emoter, HashedString animFile, HashedString[] anims, KAnim.PlayMode mode, bool flip_x)
			: base(master)
		{
			this.mode = mode;
			this.animFile = Assets.GetAnim(animFile);
			emoteAnims = anims;
			base.sm.emoter.Set(emoter, base.smi);
		}
	}

	public class States : GameStateMachine<States, StatesInstance, EmoteChore>
	{
		public TargetParameter emoter;

		public State finish;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;
			Target(emoter);
			root.ToggleAnims((StatesInstance smi) => smi.animFile).PlayAnims((StatesInstance smi) => smi.emoteAnims, (StatesInstance smi) => smi.mode).ScheduleGoTo(10f, finish)
				.OnAnimQueueComplete(finish);
			finish.ReturnSuccess();
		}
	}

	private Func<StatusItem> getStatusItem;

	private SelfEmoteReactable reactable;

	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, Emote emote, int emoteIterations = 1, Func<StatusItem> get_status_item = null)
		: base(chore_type, target, target.GetComponent<ChoreProvider>(), run_until_complete: false, (Action<Chore>)null, (Action<Chore>)null, (Action<Chore>)null, PriorityScreen.PriorityClass.compulsory, 5, is_preemptable: false, allow_in_context_menu: true, 0, add_to_daily_report: false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StatesInstance(this, target.gameObject, emote, KAnim.PlayMode.Once, emoteIterations, flip_x: false);
		getStatusItem = get_status_item;
	}

	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, Emote emote, KAnim.PlayMode play_mode, int emoteIterations = 1, bool flip_x = false)
		: base(chore_type, target, target.GetComponent<ChoreProvider>(), run_until_complete: false, (Action<Chore>)null, (Action<Chore>)null, (Action<Chore>)null, PriorityScreen.PriorityClass.compulsory, 5, is_preemptable: false, allow_in_context_menu: true, 0, add_to_daily_report: false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StatesInstance(this, target.gameObject, emote, play_mode, emoteIterations, flip_x);
	}

	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString animFile, HashedString[] anims, Func<StatusItem> get_status_item = null)
		: base(chore_type, target, target.GetComponent<ChoreProvider>(), run_until_complete: false, (Action<Chore>)null, (Action<Chore>)null, (Action<Chore>)null, PriorityScreen.PriorityClass.compulsory, 5, is_preemptable: false, allow_in_context_menu: true, 0, add_to_daily_report: false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StatesInstance(this, target.gameObject, animFile, anims, KAnim.PlayMode.Once, flip_x: false);
		getStatusItem = get_status_item;
	}

	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString animFile, HashedString[] anims, KAnim.PlayMode play_mode, bool flip_x = false)
		: base(chore_type, target, target.GetComponent<ChoreProvider>(), run_until_complete: false, (Action<Chore>)null, (Action<Chore>)null, (Action<Chore>)null, PriorityScreen.PriorityClass.compulsory, 5, is_preemptable: false, allow_in_context_menu: true, 0, add_to_daily_report: false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StatesInstance(this, target.gameObject, animFile, anims, play_mode, flip_x);
	}

	protected override StatusItem GetStatusItem()
	{
		if (getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return getStatusItem();
	}

	public override string ToString()
	{
		if (base.smi.animFile != null)
		{
			return "EmoteChore<" + base.smi.animFile.GetData().name + ">";
		}
		HashedString hashedString = base.smi.emoteAnims[0];
		return "EmoteChore<" + hashedString.ToString() + ">";
	}

	public void PairReactable(SelfEmoteReactable reactable)
	{
		this.reactable = reactable;
	}

	protected new virtual void End(string reason)
	{
		if (reactable != null)
		{
			reactable.PairEmote(null);
			reactable.Cleanup();
			reactable = null;
		}
		base.End(reason);
	}
}
