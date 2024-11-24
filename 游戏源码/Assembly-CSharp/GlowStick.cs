using Klei.AI;
using STRINGS;
using TUNING;

[SkipSaveFileSerialization]
public class GlowStick : StateMachineComponent<GlowStick.StatesInstance>
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, GlowStick, object>.GameInstance
	{
		[MyCmpAdd]
		private RadiationEmitter _radiationEmitter;

		public AttributeModifier radiationResistance;

		public AttributeModifier luminescenceModifier;

		public StatesInstance(GlowStick master)
			: base(master)
		{
			_radiationEmitter.emitRads = 100f;
			_radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			_radiationEmitter.emitRate = 0.5f;
			_radiationEmitter.emitRadiusX = 3;
			_radiationEmitter.emitRadiusY = 3;
			radiationResistance = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, TRAITS.GLOWSTICK_RADIATION_RESISTANCE, DUPLICANTS.TRAITS.GLOWSTICK.NAME);
			luminescenceModifier = new AttributeModifier(Db.Get().Attributes.Luminescence.Id, TRAITS.GLOWSTICK_LUX_VALUE, DUPLICANTS.TRAITS.GLOWSTICK.NAME);
		}
	}

	public class States : GameStateMachine<States, StatesInstance, GlowStick>
	{
		public override void InitializeStates(out BaseState default_state)
		{
			default_state = root;
			root.ToggleComponent<RadiationEmitter>().ToggleAttributeModifier("Radiation Resistance", (StatesInstance smi) => smi.radiationResistance).ToggleAttributeModifier("Luminescence Modifier", (StatesInstance smi) => smi.luminescenceModifier);
		}
	}

	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}
}
