using Klei.AI;

public class HeatImmunityProvider : EffectImmunityProviderStation<HeatImmunityProvider.Instance>
{
	public new class Def : EffectImmunityProviderStation<Instance>.Def
	{
	}

	public new class Instance : BaseInstance
	{
		public Instance(IStateMachineTarget master, Def def)
			: base(master, (EffectImmunityProviderStation<Instance>.Def)def)
		{
		}

		protected override void ApplyImmunityEffect(Effects target)
		{
			target.Add("RefreshingTouch", should_save: true);
		}
	}

	public const string PROVIDED_IMMUNITY_EFFECT_NAME = "RefreshingTouch";
}
