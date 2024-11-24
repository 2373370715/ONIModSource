using System;

// Token: 0x0200154A RID: 5450
public class CringeMonitor : GameStateMachine<CringeMonitor, CringeMonitor.Instance>
{
	// Token: 0x06007186 RID: 29062 RVA: 0x002FAA9C File Offset: 0x002F8C9C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.Cringe, new GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback(this.TriggerCringe));
		this.cringe.ToggleReactable((CringeMonitor.Instance smi) => smi.GetReactable()).ToggleStatusItem((CringeMonitor.Instance smi) => smi.GetStatusItem(), null).ScheduleGoTo(3f, this.idle);
	}

	// Token: 0x06007187 RID: 29063 RVA: 0x000EA4FE File Offset: 0x000E86FE
	private void TriggerCringe(CringeMonitor.Instance smi, object data)
	{
		if (smi.GetComponent<KPrefabID>().HasTag(GameTags.Suit))
		{
			return;
		}
		smi.SetCringeSourceData(data);
		smi.GoTo(this.cringe);
	}

	// Token: 0x040054C4 RID: 21700
	public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State idle;

	// Token: 0x040054C5 RID: 21701
	public GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.State cringe;

	// Token: 0x0200154B RID: 5451
	public new class Instance : GameStateMachine<CringeMonitor, CringeMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007189 RID: 29065 RVA: 0x000EA52E File Offset: 0x000E872E
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600718A RID: 29066 RVA: 0x002FAB30 File Offset: 0x002F8D30
		public void SetCringeSourceData(object data)
		{
			string name = (string)data;
			this.statusItem = new StatusItem("CringeSource", name, null, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
		}

		// Token: 0x0600718B RID: 29067 RVA: 0x002FAB6C File Offset: 0x002F8D6C
		public Reactable GetReactable()
		{
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "Cringe", Db.Get().ChoreTypes.EmoteHighPriority, 0f, 0f, float.PositiveInfinity, 0f);
			selfEmoteReactable.SetEmote(Db.Get().Emotes.Minion.Cringe);
			selfEmoteReactable.preventChoreInterruption = true;
			return selfEmoteReactable;
		}

		// Token: 0x0600718C RID: 29068 RVA: 0x000EA537 File Offset: 0x000E8737
		public StatusItem GetStatusItem()
		{
			return this.statusItem;
		}

		// Token: 0x040054C6 RID: 21702
		private StatusItem statusItem;
	}
}
