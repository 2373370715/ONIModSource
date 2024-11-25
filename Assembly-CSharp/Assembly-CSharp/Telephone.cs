﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class Telephone : StateMachineComponent<Telephone.StatesInstance>, IGameObjectEffectDescriptor
{
		protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Components.Telephones.Add(this);
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

		protected override void OnCleanUp()
	{
		Components.Telephones.Remove(this);
		base.OnCleanUp();
	}

		public void AddModifierDescriptions(List<Descriptor> descs, string effect_id)
	{
		Effect effect = Db.Get().effects.Get(effect_id);
		string text;
		string text2;
		if (effect.Id == this.babbleEffect)
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_BABBLE;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_BABBLE_TOOLTIP;
		}
		else if (effect.Id == this.chatEffect)
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_CHAT;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_CHAT_TOOLTIP;
		}
		else
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_LONG_DISTANCE;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_LONG_DISTANCE_TOOLTIP;
		}
		foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
		{
			Descriptor item = new Descriptor(text.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()), text2.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Effect, false);
			item.IncreaseIndent();
			descs.Add(item);
		}
	}

		List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		this.AddModifierDescriptions(list, this.babbleEffect);
		this.AddModifierDescriptions(list, this.chatEffect);
		this.AddModifierDescriptions(list, this.longDistanceEffect);
		return list;
	}

		public void HangUp()
	{
		this.isInUse = false;
		this.wasAnswered = false;
		this.RemoveTag(GameTags.LongDistanceCall);
	}

		public string babbleEffect;

		public string chatEffect;

		public string longDistanceEffect;

		public string trackingEffect;

		public bool isInUse;

		public bool wasAnswered;

		public class States : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone>
	{
				public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			Telephone.States.CreateStatusItems();
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleRecurringChore(new Func<Telephone.StatesInstance, Chore>(this.CreateChore), null).Enter(delegate(Telephone.StatesInstance smi)
			{
				using (List<Telephone>.Enumerator enumerator = Components.Telephones.Items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.isInUse)
						{
							smi.GoTo(this.ready.speaker);
						}
					}
				}
			});
			this.ready.idle.WorkableStartTransition((Telephone.StatesInstance smi) => smi.master.GetComponent<TelephoneCallerWorkable>(), this.ready.calling.dial).TagTransition(GameTags.TelephoneRinging, this.ready.ringing, false).PlayAnim("off");
			this.ready.calling.ScheduleGoTo(15f, this.ready.talking.babbling);
			this.ready.calling.dial.PlayAnim("on_pre").OnAnimQueueComplete(this.ready.calling.animHack);
			this.ready.calling.animHack.ScheduleActionNextFrame("animHack_delay", delegate(Telephone.StatesInstance smi)
			{
				smi.GoTo(this.ready.calling.pre);
			});
			this.ready.calling.pre.PlayAnim("on").Enter(delegate(Telephone.StatesInstance smi)
			{
				this.RingAllTelephones(smi);
			}).OnAnimQueueComplete(this.ready.calling.wait);
			this.ready.calling.wait.PlayAnim("on", KAnim.PlayMode.Loop).Transition(this.ready.talking.chatting, (Telephone.StatesInstance smi) => smi.CallAnswered(), UpdateRate.SIM_4000ms);
			this.ready.ringing.PlayAnim("on_receiving", KAnim.PlayMode.Loop).Transition(this.ready.answer, (Telephone.StatesInstance smi) => smi.GetComponent<Telephone>().isInUse, UpdateRate.SIM_33ms).TagTransition(GameTags.TelephoneRinging, this.ready.speaker, true).ScheduleGoTo(15f, this.ready.speaker).Exit(delegate(Telephone.StatesInstance smi)
			{
				smi.GetComponent<Telephone>().RemoveTag(GameTags.TelephoneRinging);
			});
			this.ready.answer.PlayAnim("on_pre_loop_receiving").OnAnimQueueComplete(this.ready.talking.chatting);
			this.ready.talking.ScheduleGoTo(25f, this.ready.hangup).Enter(delegate(Telephone.StatesInstance smi)
			{
				this.UpdatePartyLine(smi);
			});
			this.ready.talking.babbling.PlayAnim("on_loop", KAnim.PlayMode.Loop).Transition(this.ready.talking.chatting, (Telephone.StatesInstance smi) => smi.CallAnswered(), UpdateRate.SIM_33ms).ToggleStatusItem(Telephone.States.babbling, null);
			this.ready.talking.chatting.PlayAnim("on_loop_pre").QueueAnim("on_loop", true, null).Transition(this.ready.talking.babbling, (Telephone.StatesInstance smi) => !smi.CallAnswered(), UpdateRate.SIM_33ms).ToggleStatusItem(Telephone.States.partyLine, null);
			this.ready.speaker.PlayAnim("on_loop_nobody", KAnim.PlayMode.Loop).Transition(this.ready, (Telephone.StatesInstance smi) => !smi.CallAnswered(), UpdateRate.SIM_4000ms).Transition(this.ready.answer, (Telephone.StatesInstance smi) => smi.GetComponent<Telephone>().isInUse, UpdateRate.SIM_33ms);
			this.ready.hangup.OnAnimQueueComplete(this.ready);
		}

				private Chore CreateChore(Telephone.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<TelephoneCallerWorkable>();
			WorkChore<TelephoneCallerWorkable> workChore = new WorkChore<TelephoneCallerWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

				public void UpdatePartyLine(Telephone.StatesInstance smi)
		{
			int myWorldId = smi.GetMyWorldId();
			bool flag = false;
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				telephone.RemoveTag(GameTags.TelephoneRinging);
				if (telephone.isInUse && myWorldId != telephone.GetMyWorldId())
				{
					flag = true;
					telephone.AddTag(GameTags.LongDistanceCall);
				}
			}
			Telephone component = smi.GetComponent<Telephone>();
			component.RemoveTag(GameTags.TelephoneRinging);
			if (flag)
			{
				component.AddTag(GameTags.LongDistanceCall);
			}
		}

				public void RingAllTelephones(Telephone.StatesInstance smi)
		{
			Telephone component = smi.master.GetComponent<Telephone>();
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				if (component != telephone && telephone.GetComponent<Operational>().IsOperational)
				{
					TelephoneCallerWorkable component2 = telephone.GetComponent<TelephoneCallerWorkable>();
					if (component2 != null && component2.worker == null)
					{
						telephone.AddTag(GameTags.TelephoneRinging);
					}
				}
			}
		}

				private static void CreateStatusItems()
		{
			if (Telephone.States.partyLine == null)
			{
				Telephone.States.partyLine = new StatusItem("PartyLine", BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO, "", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
				Telephone.States.partyLine.resolveStringCallback = delegate(string str, object obj)
				{
					Telephone component = ((Telephone.StatesInstance)obj).GetComponent<Telephone>();
					int num = 0;
					foreach (Telephone telephone in Components.Telephones.Items)
					{
						if (telephone.isInUse && telephone != component)
						{
							num++;
							if (num == 1)
							{
								str = str.Replace("{Asteroid}", telephone.GetMyWorld().GetProperName());
								str = str.Replace("{Duplicant}", telephone.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
							}
						}
					}
					if (num > 1)
					{
						str = string.Format(BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO_NUM, num);
					}
					return str;
				};
				Telephone.States.partyLine.resolveTooltipCallback = delegate(string str, object obj)
				{
					Telephone component = ((Telephone.StatesInstance)obj).GetComponent<Telephone>();
					foreach (Telephone telephone in Components.Telephones.Items)
					{
						if (telephone.isInUse && telephone != component)
						{
							string text = BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO;
							text = text.Replace("{Duplicant}", telephone.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
							text = text.Replace("{Asteroid}", telephone.GetMyWorld().GetProperName());
							str = str + text + "\n";
						}
					}
					return str;
				};
			}
			if (Telephone.States.babbling == null)
			{
				Telephone.States.babbling = new StatusItem("Babbling", BUILDING.STATUSITEMS.TELEPHONE.BABBLE.NAME, BUILDING.STATUSITEMS.TELEPHONE.BABBLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
				Telephone.States.babbling.resolveTooltipCallback = delegate(string str, object obj)
				{
					Telephone.StatesInstance statesInstance = (Telephone.StatesInstance)obj;
					str = str.Replace("{Duplicant}", statesInstance.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
					return str;
				};
			}
		}

				private GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State unoperational;

				private Telephone.States.ReadyStates ready;

				private static StatusItem partyLine;

				private static StatusItem babbling;

				public class ReadyStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
		{
						public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State idle;

						public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State ringing;

						public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State answer;

						public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State speaker;

						public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State hangup;

						public Telephone.States.ReadyStates.CallingStates calling;

						public Telephone.States.ReadyStates.TalkingStates talking;

						public class CallingStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
			{
								public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State dial;

								public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State animHack;

								public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State pre;

								public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State wait;
			}

						public class TalkingStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
			{
								public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State babbling;

								public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State chatting;
			}
		}
	}

		public class StatesInstance : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.GameInstance
	{
				public StatesInstance(Telephone smi) : base(smi)
		{
		}

				public bool CallAnswered()
		{
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				if (telephone.isInUse && telephone != base.smi.GetComponent<Telephone>())
				{
					telephone.wasAnswered = true;
					return true;
				}
			}
			return false;
		}

				public bool CallEnded()
		{
			using (List<Telephone>.Enumerator enumerator = Components.Telephones.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.isInUse)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
