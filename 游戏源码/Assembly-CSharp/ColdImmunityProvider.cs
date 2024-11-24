using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x020009B2 RID: 2482
public class ColdImmunityProvider : EffectImmunityProviderStation<ColdImmunityProvider.Instance>
{
	// Token: 0x04001E9A RID: 7834
	public const string PROVIDED_IMMUNITY_EFFECT_NAME = "WarmTouch";

	// Token: 0x020009B3 RID: 2483
	public new class Def : EffectImmunityProviderStation<ColdImmunityProvider.Instance>.Def, IGameObjectEffectDescriptor
	{
		// Token: 0x06002D8B RID: 11659 RVA: 0x000BD796 File Offset: 0x000BB996
		public override string[] DefaultAnims()
		{
			return new string[]
			{
				"warmup_pre",
				"warmup_loop",
				"warmup_pst"
			};
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000BD7B6 File Offset: 0x000BB9B6
		public override string DefaultAnimFileName()
		{
			return "anim_warmup_kanim";
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x001F1154 File Offset: 0x001EF354
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_TOOLTIP"), Descriptor.DescriptorType.Effect, false)
			};
		}
	}

	// Token: 0x020009B4 RID: 2484
	public new class Instance : EffectImmunityProviderStation<ColdImmunityProvider.Instance>.BaseInstance
	{
		// Token: 0x06002D8F RID: 11663 RVA: 0x000BD7C5 File Offset: 0x000BB9C5
		public Instance(IStateMachineTarget master, ColdImmunityProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x000BD7CF File Offset: 0x000BB9CF
		protected override void ApplyImmunityEffect(Effects target)
		{
			target.Add("WarmTouch", true);
		}
	}
}
