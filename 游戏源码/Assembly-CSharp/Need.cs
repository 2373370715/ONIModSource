using System;
using Klei.AI;

// Token: 0x02001643 RID: 5699
public abstract class Need : KMonoBehaviour
{
	// Token: 0x17000773 RID: 1907
	// (get) Token: 0x060075EB RID: 30187 RVA: 0x000ED75B File Offset: 0x000EB95B
	// (set) Token: 0x060075EC RID: 30188 RVA: 0x000ED763 File Offset: 0x000EB963
	public string Name { get; protected set; }

	// Token: 0x17000774 RID: 1908
	// (get) Token: 0x060075ED RID: 30189 RVA: 0x000ED76C File Offset: 0x000EB96C
	// (set) Token: 0x060075EE RID: 30190 RVA: 0x000ED774 File Offset: 0x000EB974
	public string ExpectationTooltip { get; protected set; }

	// Token: 0x17000775 RID: 1909
	// (get) Token: 0x060075EF RID: 30191 RVA: 0x000ED77D File Offset: 0x000EB97D
	// (set) Token: 0x060075F0 RID: 30192 RVA: 0x000ED785 File Offset: 0x000EB985
	public string Tooltip { get; protected set; }

	// Token: 0x060075F1 RID: 30193 RVA: 0x000ED78E File Offset: 0x000EB98E
	public Klei.AI.Attribute GetExpectationAttribute()
	{
		return this.expectationAttribute.Attribute;
	}

	// Token: 0x060075F2 RID: 30194 RVA: 0x000ED79B File Offset: 0x000EB99B
	protected void SetModifier(Need.ModifierType modifier)
	{
		if (this.currentStressModifier != modifier)
		{
			if (this.currentStressModifier != null)
			{
				this.UnapplyModifier(this.currentStressModifier);
			}
			if (modifier != null)
			{
				this.ApplyModifier(modifier);
			}
			this.currentStressModifier = modifier;
		}
	}

	// Token: 0x060075F3 RID: 30195 RVA: 0x003083F0 File Offset: 0x003065F0
	private void ApplyModifier(Need.ModifierType modifier)
	{
		if (modifier.modifier != null)
		{
			this.GetAttributes().Add(modifier.modifier);
		}
		if (modifier.statusItem != null)
		{
			base.GetComponent<KSelectable>().AddStatusItem(modifier.statusItem, null);
		}
		if (modifier.thought != null)
		{
			this.GetSMI<ThoughtGraph.Instance>().AddThought(modifier.thought);
		}
	}

	// Token: 0x060075F4 RID: 30196 RVA: 0x0030844C File Offset: 0x0030664C
	private void UnapplyModifier(Need.ModifierType modifier)
	{
		if (modifier.modifier != null)
		{
			this.GetAttributes().Remove(modifier.modifier);
		}
		if (modifier.statusItem != null)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(modifier.statusItem, false);
		}
		if (modifier.thought != null)
		{
			this.GetSMI<ThoughtGraph.Instance>().RemoveThought(modifier.thought);
		}
	}

	// Token: 0x0400586E RID: 22638
	protected AttributeInstance expectationAttribute;

	// Token: 0x0400586F RID: 22639
	protected Need.ModifierType stressBonus;

	// Token: 0x04005870 RID: 22640
	protected Need.ModifierType stressNeutral;

	// Token: 0x04005871 RID: 22641
	protected Need.ModifierType stressPenalty;

	// Token: 0x04005872 RID: 22642
	protected Need.ModifierType currentStressModifier;

	// Token: 0x02001644 RID: 5700
	protected class ModifierType
	{
		// Token: 0x04005876 RID: 22646
		public AttributeModifier modifier;

		// Token: 0x04005877 RID: 22647
		public StatusItem statusItem;

		// Token: 0x04005878 RID: 22648
		public Thought thought;
	}
}
