using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001728 RID: 5928
[SkipSaveFileSerialization]
public class RadiationEater : StateMachineComponent<RadiationEater.StatesInstance>
{
	// Token: 0x06007A20 RID: 31264 RVA: 0x000F036D File Offset: 0x000EE56D
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001729 RID: 5929
	public class StatesInstance : GameStateMachine<RadiationEater.States, RadiationEater.StatesInstance, RadiationEater, object>.GameInstance
	{
		// Token: 0x06007A22 RID: 31266 RVA: 0x000F0382 File Offset: 0x000EE582
		public StatesInstance(RadiationEater master) : base(master)
		{
			this.radiationEating = new AttributeModifier(Db.Get().Attributes.RadiationRecovery.Id, TRAITS.RADIATION_EATER_RECOVERY, DUPLICANTS.TRAITS.RADIATIONEATER.NAME, false, false, true);
		}

		// Token: 0x06007A23 RID: 31267 RVA: 0x00317B68 File Offset: 0x00315D68
		public void OnEatRads(float radsEaten)
		{
			float delta = Mathf.Abs(radsEaten) * TRAITS.RADS_TO_CALS;
			base.smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.Calories).ApplyDelta(delta);
		}

		// Token: 0x04005B9F RID: 23455
		public AttributeModifier radiationEating;
	}

	// Token: 0x0200172A RID: 5930
	public class States : GameStateMachine<RadiationEater.States, RadiationEater.StatesInstance, RadiationEater>
	{
		// Token: 0x06007A24 RID: 31268 RVA: 0x00317BB4 File Offset: 0x00315DB4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier("Radiation Eating", (RadiationEater.StatesInstance smi) => smi.radiationEating, null).EventHandler(GameHashes.RadiationRecovery, delegate(RadiationEater.StatesInstance smi, object data)
			{
				float radsEaten = (float)data;
				smi.OnEatRads(radsEaten);
			});
		}
	}
}
