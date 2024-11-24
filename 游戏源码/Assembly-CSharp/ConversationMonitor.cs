using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200153F RID: 5439
public class ConversationMonitor : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>
{
	// Token: 0x06007165 RID: 29029 RVA: 0x002FA5AC File Offset: 0x002F87AC
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

	// Token: 0x040054AB RID: 21675
	private const int MAX_RECENT_TOPICS = 5;

	// Token: 0x040054AC RID: 21676
	private const int MAX_FAVOURITE_TOPICS = 5;

	// Token: 0x040054AD RID: 21677
	private const float FAVOURITE_CHANCE = 0.033333335f;

	// Token: 0x040054AE RID: 21678
	private const float LEARN_CHANCE = 0.33333334f;

	// Token: 0x02001540 RID: 5440
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001541 RID: 5441
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>.GameInstance
	{
		// Token: 0x06007168 RID: 29032 RVA: 0x002FA61C File Offset: 0x002F881C
		public Instance(IStateMachineTarget master, ConversationMonitor.Def def) : base(master, def)
		{
			this.recentTopics = new Queue<string>();
			this.favouriteTopics = new List<string>
			{
				ConversationMonitor.Instance.randomTopics[UnityEngine.Random.Range(0, ConversationMonitor.Instance.randomTopics.Count)]
			};
			this.personalTopics = new List<string>();
		}

		// Token: 0x06007169 RID: 29033 RVA: 0x002FA674 File Offset: 0x002F8874
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

		// Token: 0x0600716A RID: 29034 RVA: 0x002FA74C File Offset: 0x002F894C
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

		// Token: 0x0600716B RID: 29035 RVA: 0x002FA79C File Offset: 0x002F899C
		public void OnTopicDiscussed(object data)
		{
			string data2 = (string)data;
			if (UnityEngine.Random.value < 0.33333334f)
			{
				this.OnTopicDiscovered(data2);
			}
		}

		// Token: 0x0600716C RID: 29036 RVA: 0x002FA7C4 File Offset: 0x002F89C4
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

		// Token: 0x040054AF RID: 21679
		[Serialize]
		private Queue<string> recentTopics;

		// Token: 0x040054B0 RID: 21680
		[Serialize]
		private List<string> favouriteTopics;

		// Token: 0x040054B1 RID: 21681
		private List<string> personalTopics;

		// Token: 0x040054B2 RID: 21682
		private static readonly List<string> randomTopics = new List<string>
		{
			"Headquarters"
		};
	}
}
