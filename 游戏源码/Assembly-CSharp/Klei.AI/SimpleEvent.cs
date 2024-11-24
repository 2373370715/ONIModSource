using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI;

public class SimpleEvent : GameplayEvent<SimpleEvent.StatesInstance>
{
	public class States : GameplayEventStateMachine<States, StatesInstance, GameplayEventManager, SimpleEvent>
	{
		public State ending;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;
			ending.ReturnSuccess();
		}

		public override EventInfoData GenerateEventPopupData(StatesInstance smi)
		{
			EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
			eventInfoData.minions = smi.minions;
			eventInfoData.artifact = smi.artifact;
			EventInfoData.Option option = eventInfoData.AddOption(smi.gameplayEvent.buttonText);
			option.callback = delegate
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
				foreach (Tuple<string, string> textParameter in smi.textParameters)
				{
					eventInfoData.SetTextParameter(textParameter.first, textParameter.second);
				}
			}
			return eventInfoData;
		}
	}

	public class StatesInstance : GameplayEventStateMachine<States, StatesInstance, GameplayEventManager, SimpleEvent>.GameplayEventStateMachineInstance
	{
		public GameObject[] minions;

		public GameObject artifact;

		public List<Tuple<string, string>> textParameters;

		public System.Action callback;

		public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SimpleEvent simpleEvent)
			: base(master, eventInstance, simpleEvent)
		{
		}

		public void SetTextParameter(string key, string value)
		{
			if (textParameters == null)
			{
				textParameters = new List<Tuple<string, string>>();
			}
			textParameters.Add(new Tuple<string, string>(key, value));
		}

		public void ShowEventPopup()
		{
			EventInfoScreen.ShowPopup(base.smi.sm.GenerateEventPopupData(base.smi));
		}
	}

	private string buttonText;

	private string buttonTooltip;

	public SimpleEvent(string id, string title, string description, string animFileName, string buttonText = null, string buttonTooltip = null)
		: base(id, 0, 0)
	{
		base.title = title;
		base.description = description;
		this.buttonText = buttonText;
		this.buttonTooltip = buttonTooltip;
		base.animFileName = animFileName;
	}

	public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
	{
		return new StatesInstance(manager, eventInstance, this);
	}
}
