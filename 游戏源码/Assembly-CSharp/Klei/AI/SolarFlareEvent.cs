using System;
using STRINGS;

namespace Klei.AI
{
	// Token: 0x02003B77 RID: 15223
	public class SolarFlareEvent : GameplayEvent<SolarFlareEvent.StatesInstance>
	{
		// Token: 0x0600EA5D RID: 59997 RVA: 0x0013CA2D File Offset: 0x0013AC2D
		public SolarFlareEvent() : base("SolarFlareEvent", 0, 0)
		{
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.SOLAR_FLARE.DESCRIPTION;
		}

		// Token: 0x0600EA5E RID: 59998 RVA: 0x0013CA5C File Offset: 0x0013AC5C
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SolarFlareEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400E5BA RID: 58810
		public const string ID = "SolarFlareEvent";

		// Token: 0x0400E5BB RID: 58811
		public const float DURATION = 7f;

		// Token: 0x02003B78 RID: 15224
		public class StatesInstance : GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600EA5F RID: 59999 RVA: 0x0013CA66 File Offset: 0x0013AC66
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SolarFlareEvent solarFlareEvent) : base(master, eventInstance, solarFlareEvent)
			{
			}
		}

		// Token: 0x02003B79 RID: 15225
		public class States : GameplayEventStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, SolarFlareEvent>
		{
			// Token: 0x0600EA60 RID: 60000 RVA: 0x0013CA71 File Offset: 0x0013AC71
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.idle;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.idle.DoNothing();
				this.start.ScheduleGoTo(7f, this.finished);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600EA61 RID: 60001 RVA: 0x004C9F90 File Offset: 0x004C8190
			public override EventInfoData GenerateEventPopupData(SolarFlareEvent.StatesInstance smi)
			{
				return new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName)
				{
					location = GAMEPLAY_EVENTS.LOCATIONS.SUN,
					whenDescription = GAMEPLAY_EVENTS.TIMES.NOW
				};
			}

			// Token: 0x0400E5BC RID: 58812
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State idle;

			// Token: 0x0400E5BD RID: 58813
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State start;

			// Token: 0x0400E5BE RID: 58814
			public GameStateMachine<SolarFlareEvent.States, SolarFlareEvent.StatesInstance, GameplayEventManager, object>.State finished;
		}
	}
}
