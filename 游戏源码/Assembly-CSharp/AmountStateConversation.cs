using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02001115 RID: 4373
public class AmountStateConversation : ConversationType
{
	// Token: 0x0600599B RID: 22939 RVA: 0x000DA527 File Offset: 0x000D8727
	public AmountStateConversation()
	{
		this.id = "AmountStateConversation";
	}

	// Token: 0x0600599C RID: 22940 RVA: 0x000DA53A File Offset: 0x000D873A
	public override void NewTarget(MinionIdentity speaker)
	{
		this.target = AmountStateConversation.targets[UnityEngine.Random.Range(0, AmountStateConversation.targets.Count)];
	}

	// Token: 0x0600599D RID: 22941 RVA: 0x00291ED0 File Offset: 0x002900D0
	public override Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		if (lastTopic == null)
		{
			return new Conversation.Topic(this.target, Conversation.ModeType.Query);
		}
		List<Conversation.ModeType> list = AmountStateConversation.transitions[lastTopic.mode];
		Conversation.ModeType modeType = list[UnityEngine.Random.Range(0, list.Count)];
		if (modeType == Conversation.ModeType.Statement)
		{
			Conversation.ModeType modeForAmount = this.GetModeForAmount(speaker, this.target);
			return new Conversation.Topic(this.target, modeForAmount);
		}
		return new Conversation.Topic(this.target, modeType);
	}

	// Token: 0x0600599E RID: 22942 RVA: 0x00291F3C File Offset: 0x0029013C
	public override Sprite GetSprite(string topic)
	{
		if (Db.Get().Amounts.Exists(topic))
		{
			return Assets.GetSprite(Db.Get().Amounts.Get(topic).thoughtSprite);
		}
		if (Db.Get().Attributes.Exists(topic))
		{
			return Assets.GetSprite(Db.Get().Attributes.Get(topic).thoughtSprite);
		}
		return null;
	}

	// Token: 0x0600599F RID: 22943 RVA: 0x00291FB0 File Offset: 0x002901B0
	private Conversation.ModeType GetModeForAmount(MinionIdentity speaker, string target)
	{
		if (target == Db.Get().Amounts.Stress.Id)
		{
			AmountInstance amountInstance = Db.Get().Amounts.Stress.Lookup(speaker);
			float num = amountInstance.value / amountInstance.GetMax();
			if (num < 0.1f)
			{
				return Conversation.ModeType.Satisfaction;
			}
			if (num > 0.6f)
			{
				return Conversation.ModeType.Dissatisfaction;
			}
		}
		else if (target == Db.Get().Attributes.QualityOfLife.Id)
		{
			AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(speaker);
			AttributeInstance attributeInstance2 = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(speaker);
			float num2 = attributeInstance.GetTotalValue() - attributeInstance2.GetTotalValue();
			if (num2 > 0f)
			{
				return Conversation.ModeType.Satisfaction;
			}
			if (num2 < 0f)
			{
				return Conversation.ModeType.Dissatisfaction;
			}
		}
		else if (target == Db.Get().Amounts.HitPoints.Id)
		{
			AmountInstance amountInstance2 = Db.Get().Amounts.HitPoints.Lookup(speaker);
			float num3 = amountInstance2.value / amountInstance2.GetMax();
			if (num3 >= 1f)
			{
				return Conversation.ModeType.Satisfaction;
			}
			if (num3 < 0.8f)
			{
				return Conversation.ModeType.Dissatisfaction;
			}
		}
		else if (target == Db.Get().Amounts.Calories.Id)
		{
			AmountInstance amountInstance3 = Db.Get().Amounts.Calories.Lookup(speaker);
			if (amountInstance3 == null)
			{
				return Conversation.ModeType.Query;
			}
			float num4 = amountInstance3.value / amountInstance3.GetMax();
			if (num4 > 0.85f)
			{
				return Conversation.ModeType.Satisfaction;
			}
			if (num4 < 0.5f)
			{
				return Conversation.ModeType.Dissatisfaction;
			}
		}
		else if (target == Db.Get().Amounts.Stamina.Id)
		{
			AmountInstance amountInstance4 = Db.Get().Amounts.Stamina.Lookup(speaker);
			if (amountInstance4 == null)
			{
				return Conversation.ModeType.Query;
			}
			float num5 = amountInstance4.value / amountInstance4.GetMax();
			if (num5 > 0.5f)
			{
				return Conversation.ModeType.Satisfaction;
			}
			if (num5 < 0.2f)
			{
				return Conversation.ModeType.Dissatisfaction;
			}
		}
		else if (target == Db.Get().Amounts.ImmuneLevel.Id)
		{
			AmountInstance amountInstance5 = Db.Get().Amounts.ImmuneLevel.Lookup(speaker);
			float num6 = amountInstance5.value / amountInstance5.GetMax();
			if (num6 > 0.9f)
			{
				return Conversation.ModeType.Satisfaction;
			}
			if (num6 < 0.5f)
			{
				return Conversation.ModeType.Dissatisfaction;
			}
		}
		return Conversation.ModeType.Nominal;
	}

	// Token: 0x04003F54 RID: 16212
	public static Dictionary<Conversation.ModeType, List<Conversation.ModeType>> transitions = new Dictionary<Conversation.ModeType, List<Conversation.ModeType>>
	{
		{
			Conversation.ModeType.Query,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Statement
			}
		},
		{
			Conversation.ModeType.Satisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Agreement,
				Conversation.ModeType.Statement
			}
		},
		{
			Conversation.ModeType.Nominal,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Musing,
				Conversation.ModeType.Statement
			}
		},
		{
			Conversation.ModeType.Dissatisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Disagreement,
				Conversation.ModeType.Statement
			}
		},
		{
			Conversation.ModeType.Agreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Disagreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Musing,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		}
	};

	// Token: 0x04003F55 RID: 16213
	public static List<string> targets = new List<string>
	{
		"Stress",
		"QualityOfLife",
		"HitPoints",
		"Calories",
		"Stamina",
		"ImmuneLevel"
	};
}
