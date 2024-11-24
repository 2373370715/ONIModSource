using System;
using Klei.AI;

// Token: 0x02000A65 RID: 2661
public class HeatImmunityProvider : EffectImmunityProviderStation<HeatImmunityProvider.Instance>
{
	// Token: 0x04002118 RID: 8472
	public const string PROVIDED_IMMUNITY_EFFECT_NAME = "RefreshingTouch";

	// Token: 0x02000A66 RID: 2662
	public new class Def : EffectImmunityProviderStation<HeatImmunityProvider.Instance>.Def
	{
	}

	// Token: 0x02000A67 RID: 2663
	public new class Instance : EffectImmunityProviderStation<HeatImmunityProvider.Instance>.BaseInstance
	{
		// Token: 0x06003104 RID: 12548 RVA: 0x000BFD9D File Offset: 0x000BDF9D
		public Instance(IStateMachineTarget master, HeatImmunityProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x06003105 RID: 12549 RVA: 0x000BFDA7 File Offset: 0x000BDFA7
		protected override void ApplyImmunityEffect(Effects target)
		{
			target.Add("RefreshingTouch", true);
		}
	}
}
