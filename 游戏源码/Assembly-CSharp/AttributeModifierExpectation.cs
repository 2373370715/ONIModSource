using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020012D6 RID: 4822
public class AttributeModifierExpectation : Expectation
{
	// Token: 0x0600630D RID: 25357 RVA: 0x002B88C0 File Offset: 0x002B6AC0
	public AttributeModifierExpectation(string id, string name, string description, AttributeModifier modifier, Sprite icon) : base(id, name, description, delegate(MinionResume resume)
	{
		resume.GetAttributes().Get(modifier.AttributeId).Add(modifier);
	}, delegate(MinionResume resume)
	{
		resume.GetAttributes().Get(modifier.AttributeId).Remove(modifier);
	})
	{
		this.modifier = modifier;
		this.icon = icon;
	}

	// Token: 0x040046AA RID: 18090
	public AttributeModifier modifier;

	// Token: 0x040046AB RID: 18091
	public Sprite icon;
}
