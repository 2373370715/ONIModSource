using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B73 RID: 15219
	public class SimpleEvent : GameplayEvent<SimpleEvent.StatesInstance>
	{
		// Token: 0x0600EA53 RID: 59987 RVA: 0x0013C950 File Offset: 0x0013AB50
		public SimpleEvent(string id, string title, string description, string animFileName, string buttonText = null, string buttonTooltip = null) : base(id, 0, 0)
		{
			this.title = title;
			this.description = description;
			this.buttonText = buttonText;
			this.buttonTooltip = buttonTooltip;
			this.animFileName = animFileName;
		}

		// Token: 0x0600EA54 RID: 59988 RVA: 0x0013C986 File Offset: 0x0013AB86
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SimpleEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400E5B2 RID: 58802
		private string buttonText;

		// Token: 0x0400E5B3 RID: 58803
		private string buttonTooltip;

		// Token: 0x02003B74 RID: 15220
		public class States : GameplayEventStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, SimpleEvent>
		{
			// Token: 0x0600EA55 RID: 59989 RVA: 0x0013C990 File Offset: 0x0013AB90
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.root;
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600EA56 RID: 59990 RVA: 0x004C9E7C File Offset: 0x004C807C
			public override EventInfoData GenerateEventPopupData(SimpleEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.minions = smi.minions;
				eventInfoData.artifact = smi.artifact;
				EventInfoData.Option option = eventInfoData.AddOption(smi.gameplayEvent.buttonText, null);
				option.callback = delegate()
				{
					if (smi.callback != null)
					{
						smi.callback();
					}
					smi.StopSM("SimpleEvent Finished");
				};
				option.tooltip = smi.gameplayEvent.buttonTooltip;
				if (smi.textParameters != null)
				{
					foreach (global::Tuple<string, string> tuple in smi.textParameters)
					{
						eventInfoData.SetTextParameter(tuple.first, tuple.second);
					}
				}
				return eventInfoData;
			}

			// Token: 0x0400E5B4 RID: 58804
			public GameStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, object>.State ending;
		}

		// Token: 0x02003B76 RID: 15222
		public class StatesInstance : GameplayEventStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, SimpleEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600EA5A RID: 59994 RVA: 0x0013C9DD File Offset: 0x0013ABDD
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SimpleEvent simpleEvent) : base(master, eventInstance, simpleEvent)
			{
			}

			// Token: 0x0600EA5B RID: 59995 RVA: 0x0013C9E8 File Offset: 0x0013ABE8
			public void SetTextParameter(string key, string value)
			{
				if (this.textParameters == null)
				{
					this.textParameters = new List<global::Tuple<string, string>>();
				}
				this.textParameters.Add(new global::Tuple<string, string>(key, value));
			}

			// Token: 0x0600EA5C RID: 59996 RVA: 0x0013CA0F File Offset: 0x0013AC0F
			public void ShowEventPopup()
			{
				EventInfoScreen.ShowPopup(base.smi.sm.GenerateEventPopupData(base.smi));
			}

			// Token: 0x0400E5B6 RID: 58806
			public GameObject[] minions;

			// Token: 0x0400E5B7 RID: 58807
			public GameObject artifact;

			// Token: 0x0400E5B8 RID: 58808
			public List<global::Tuple<string, string>> textParameters;

			// Token: 0x0400E5B9 RID: 58809
			public System.Action callback;
		}
	}
}
