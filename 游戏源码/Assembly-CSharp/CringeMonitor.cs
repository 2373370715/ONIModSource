public class CringeMonitor : GameStateMachine<CringeMonitor, CringeMonitor.Instance>
{
	public new class Instance : GameInstance
	{
		private StatusItem statusItem;

		public Instance(IStateMachineTarget master)
			: base(master)
		{
		}

		public void SetCringeSourceData(object data)
		{
			string name = (string)data;
			statusItem = new StatusItem("CringeSource", name, null, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, allow_multiples: false, OverlayModes.None.ID);
		}

		public Reactable GetReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "Cringe", Db.Get().ChoreTypes.EmoteHighPriority, 0f, 0f);
			selfEmoteReactable.SetEmote(Db.Get().Emotes.Minion.Cringe);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		public StatusItem GetStatusItem()
		{
			return statusItem;
		}
	}

	public State idle;

	public State cringe;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = idle;
		idle.EventHandler(GameHashes.Cringe, TriggerCringe);
		cringe.ToggleReactable((Instance smi) => smi.GetReactable()).ToggleStatusItem((Instance smi) => smi.GetStatusItem()).ScheduleGoTo(3f, idle);
	}

	private void TriggerCringe(Instance smi, object data)
	{
		if (!smi.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
		{
			smi.SetCringeSourceData(data);
			smi.GoTo(cringe);
		}
	}
}
