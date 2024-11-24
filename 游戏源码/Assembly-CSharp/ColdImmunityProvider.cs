using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

public class ColdImmunityProvider : EffectImmunityProviderStation<ColdImmunityProvider.Instance>
{
	public new class Def : EffectImmunityProviderStation<Instance>.Def, IGameObjectEffectDescriptor
	{
		public override string[] DefaultAnims()
		{
			return new string[3] { "warmup_pre", "warmup_loop", "warmup_pst" };
		}

		public override string DefaultAnimFileName()
		{
			return "anim_warmup_kanim";
		}

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>
			{
				new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_TOOLTIP"))
			};
		}
	}

	public new class Instance : BaseInstance
	{
		public Instance(IStateMachineTarget master, Def def)
			: base(master, (EffectImmunityProviderStation<Instance>.Def)def)
		{
		}

		protected override void ApplyImmunityEffect(Effects target)
		{
			target.Add("WarmTouch", should_save: true);
		}
	}

	public const string PROVIDED_IMMUNITY_EFFECT_NAME = "WarmTouch";
}
