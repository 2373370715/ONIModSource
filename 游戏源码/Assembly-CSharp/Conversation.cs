using System;
using System.Collections.Generic;

// Token: 0x0200110E RID: 4366
public class Conversation
{
	// Token: 0x04003F34 RID: 16180
	public List<MinionIdentity> minions = new List<MinionIdentity>();

	// Token: 0x04003F35 RID: 16181
	public MinionIdentity lastTalked;

	// Token: 0x04003F36 RID: 16182
	public ConversationType conversationType;

	// Token: 0x04003F37 RID: 16183
	public float lastTalkedTime;

	// Token: 0x04003F38 RID: 16184
	public Conversation.Topic lastTopic;

	// Token: 0x04003F39 RID: 16185
	public int numUtterances;

	// Token: 0x0200110F RID: 4367
	public enum ModeType
	{
		// Token: 0x04003F3B RID: 16187
		Query,
		// Token: 0x04003F3C RID: 16188
		Statement,
		// Token: 0x04003F3D RID: 16189
		Agreement,
		// Token: 0x04003F3E RID: 16190
		Disagreement,
		// Token: 0x04003F3F RID: 16191
		Musing,
		// Token: 0x04003F40 RID: 16192
		Satisfaction,
		// Token: 0x04003F41 RID: 16193
		Nominal,
		// Token: 0x04003F42 RID: 16194
		Dissatisfaction,
		// Token: 0x04003F43 RID: 16195
		Stressing,
		// Token: 0x04003F44 RID: 16196
		Segue,
		// Token: 0x04003F45 RID: 16197
		End
	}

	// Token: 0x02001110 RID: 4368
	public class Mode
	{
		// Token: 0x06005987 RID: 22919 RVA: 0x000DA49C File Offset: 0x000D869C
		public Mode(Conversation.ModeType type, string voice, string icon, string mouth, string anim, bool newTopic = false)
		{
			this.type = type;
			this.voice = voice;
			this.mouth = mouth;
			this.anim = anim;
			this.icon = icon;
			this.newTopic = newTopic;
		}

		// Token: 0x04003F46 RID: 16198
		public Conversation.ModeType type;

		// Token: 0x04003F47 RID: 16199
		public string voice;

		// Token: 0x04003F48 RID: 16200
		public string mouth;

		// Token: 0x04003F49 RID: 16201
		public string anim;

		// Token: 0x04003F4A RID: 16202
		public string icon;

		// Token: 0x04003F4B RID: 16203
		public bool newTopic;
	}

	// Token: 0x02001111 RID: 4369
	public class Topic
	{
		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06005988 RID: 22920 RVA: 0x002919A0 File Offset: 0x0028FBA0
		public static Dictionary<int, Conversation.Mode> Modes
		{
			get
			{
				if (Conversation.Topic._modes == null)
				{
					Conversation.Topic._modes = new Dictionary<int, Conversation.Mode>();
					foreach (Conversation.Mode mode in Conversation.Topic.modeList)
					{
						Conversation.Topic._modes[(int)mode.type] = mode;
					}
				}
				return Conversation.Topic._modes;
			}
		}

		// Token: 0x06005989 RID: 22921 RVA: 0x000DA4D1 File Offset: 0x000D86D1
		public Topic(string topic, Conversation.ModeType mode)
		{
			this.topic = topic;
			this.mode = mode;
		}

		// Token: 0x04003F4C RID: 16204
		public static List<Conversation.Mode> modeList = new List<Conversation.Mode>
		{
			new Conversation.Mode(Conversation.ModeType.Query, "conversation_question", "mode_query", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Statement, "conversation_answer", "mode_statement", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Agreement, "conversation_answer", "mode_agreement", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Disagreement, "conversation_answer", "mode_disagreement", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Musing, "conversation_short", "mode_musing", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Satisfaction, "conversation_short", "mode_satisfaction", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Nominal, "conversation_short", "mode_nominal", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Dissatisfaction, "conversation_short", "mode_dissatisfaction", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Stressing, "conversation_short", "mode_stressing", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Segue, "conversation_question", "mode_segue", SpeechMonitor.PREFIX_HAPPY, "happy", true)
		};

		// Token: 0x04003F4D RID: 16205
		private static Dictionary<int, Conversation.Mode> _modes;

		// Token: 0x04003F4E RID: 16206
		public string topic;

		// Token: 0x04003F4F RID: 16207
		public Conversation.ModeType mode;
	}
}
