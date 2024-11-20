using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

public class ConversationMonitor : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.TopicDiscussed, delegate(ConversationMonitor.Instance smi, object obj)
		{
			smi.OnTopicDiscussed(obj);
		}).EventHandler(GameHashes.TopicDiscovered, delegate(ConversationMonitor.Instance smi, object obj)
		{
			smi.OnTopicDiscovered(obj);
		});
	}

	private const int MAX_RECENT_TOPICS = 5;

	private const int MAX_FAVOURITE_TOPICS = 5;

	private const float FAVOURITE_CHANCE = 0.033333335f;

	private const float LEARN_CHANCE = 0.33333334f;

	public class Def : StateMachine.BaseDef
	{
	}

	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>.GameInstance
	{
		public Instance(IStateMachineTarget master, ConversationMonitor.Def def) : base(master, def)
		{
			this.recentTopics = new Queue<string>();
			this.favouriteTopics = new List<string>
			{
				ConversationMonitor.Instance.randomTopics[UnityEngine.Random.Range(0, ConversationMonitor.Instance.randomTopics.Count)]
			};
			this.personalTopics = new List<string>();
		}

		public string GetATopic()
		{
			int maxExclusive = this.recentTopics.Count + this.favouriteTopics.Count * 2 + this.personalTopics.Count;
			int num = UnityEngine.Random.Range(0, maxExclusive);
			if (num < this.recentTopics.Count)
			{
				return this.recentTopics.Dequeue();
			}
			num -= this.recentTopics.Count;
			if (num < this.favouriteTopics.Count)
			{
				return this.favouriteTopics[num];
			}
			num -= this.favouriteTopics.Count;
			if (num < this.favouriteTopics.Count)
			{
				return this.favouriteTopics[num];
			}
			num -= this.favouriteTopics.Count;
			if (num < this.personalTopics.Count)
			{
				return this.personalTopics[num];
			}
			return "";
		}

		public void OnTopicDiscovered(object data)
		{
			string item = (string)data;
			if (!this.recentTopics.Contains(item))
			{
				this.recentTopics.Enqueue(item);
				if (this.recentTopics.Count > 5)
				{
					string topic = this.recentTopics.Dequeue();
					this.TryMakeFavouriteTopic(topic);
				}
			}
		}

		public void OnTopicDiscussed(object data)
		{
			string data2 = (string)data;
			if (UnityEngine.Random.value < 0.33333334f)
			{
				this.OnTopicDiscovered(data2);
			}
		}

		private void TryMakeFavouriteTopic(string topic)
		{
			if (UnityEngine.Random.value < 0.033333335f)
			{
				if (this.favouriteTopics.Count < 5)
				{
					this.favouriteTopics.Add(topic);
					return;
				}
				this.favouriteTopics[UnityEngine.Random.Range(0, this.favouriteTopics.Count)] = topic;
			}
		}

		[Serialize]
		private Queue<string> recentTopics;

		[Serialize]
		private List<string> favouriteTopics;

		private List<string> personalTopics;

		private static readonly List<string> randomTopics = new List<string>
		{
			"Headquarters"
		};
	}
}
