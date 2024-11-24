using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x0200138F RID: 5007
[SkipSaveFileSerialization]
public class GlowStick : StateMachineComponent<GlowStick.StatesInstance>
{
	// Token: 0x06006702 RID: 26370 RVA: 0x000E359B File Offset: 0x000E179B
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001390 RID: 5008
	public class StatesInstance : GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick, object>.GameInstance
	{
		// Token: 0x06006704 RID: 26372 RVA: 0x002D3344 File Offset: 0x002D1544
		public StatesInstance(GlowStick master) : base(master)
		{
			this._radiationEmitter.emitRads = 100f;
			this._radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			this._radiationEmitter.emitRate = 0.5f;
			this._radiationEmitter.emitRadiusX = 3;
			this._radiationEmitter.emitRadiusY = 3;
			this.radiationResistance = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, TRAITS.GLOWSTICK_RADIATION_RESISTANCE, DUPLICANTS.TRAITS.GLOWSTICK.NAME, false, false, true);
			this.luminescenceModifier = new AttributeModifier(Db.Get().Attributes.Luminescence.Id, TRAITS.GLOWSTICK_LUX_VALUE, DUPLICANTS.TRAITS.GLOWSTICK.NAME, false, false, true);
		}

		// Token: 0x04004D5A RID: 19802
		[MyCmpAdd]
		private RadiationEmitter _radiationEmitter;

		// Token: 0x04004D5B RID: 19803
		public AttributeModifier radiationResistance;

		// Token: 0x04004D5C RID: 19804
		public AttributeModifier luminescenceModifier;
	}

	// Token: 0x02001391 RID: 5009
	public class States : GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick>
	{
		// Token: 0x06006705 RID: 26373 RVA: 0x002D3400 File Offset: 0x002D1600
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleComponent<RadiationEmitter>(false).ToggleAttributeModifier("Radiation Resistance", (GlowStick.StatesInstance smi) => smi.radiationResistance, null).ToggleAttributeModifier("Luminescence Modifier", (GlowStick.StatesInstance smi) => smi.luminescenceModifier, null);
		}
	}
}
