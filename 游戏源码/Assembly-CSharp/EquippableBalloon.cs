using System;
using Database;
using KSerialization;
using TUNING;

// Token: 0x020012B6 RID: 4790
public class EquippableBalloon : StateMachineComponent<EquippableBalloon.StatesInstance>
{
	// Token: 0x06006277 RID: 25207 RVA: 0x000E0404 File Offset: 0x000DE604
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
	}

	// Token: 0x06006278 RID: 25208 RVA: 0x000E0427 File Offset: 0x000DE627
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.ApplyBalloonOverrideToBalloonFx();
	}

	// Token: 0x06006279 RID: 25209 RVA: 0x000E0440 File Offset: 0x000DE640
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600627A RID: 25210 RVA: 0x000E0448 File Offset: 0x000DE648
	public void SetBalloonOverride(BalloonOverrideSymbol balloonOverride)
	{
		base.smi.facadeAnim = balloonOverride.animFileID;
		base.smi.symbolID = balloonOverride.animFileSymbolID;
		this.ApplyBalloonOverrideToBalloonFx();
	}

	// Token: 0x0600627B RID: 25211 RVA: 0x002B6CDC File Offset: 0x002B4EDC
	public void ApplyBalloonOverrideToBalloonFx()
	{
		Equippable component = base.GetComponent<Equippable>();
		if (!component.IsNullOrDestroyed() && !component.assignee.IsNullOrDestroyed())
		{
			Ownables soleOwner = component.assignee.GetSoleOwner();
			if (soleOwner.IsNullOrDestroyed())
			{
				return;
			}
			BalloonFX.Instance smi = ((KMonoBehaviour)soleOwner.GetComponent<MinionAssignablesProxy>().target).GetSMI<BalloonFX.Instance>();
			if (!smi.IsNullOrDestroyed())
			{
				new BalloonOverrideSymbol(base.smi.facadeAnim, base.smi.symbolID).ApplyTo(smi);
			}
		}
	}

	// Token: 0x020012B7 RID: 4791
	public class StatesInstance : GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.GameInstance
	{
		// Token: 0x0600627D RID: 25213 RVA: 0x000E047A File Offset: 0x000DE67A
		public StatesInstance(EquippableBalloon master) : base(master)
		{
		}

		// Token: 0x0400461C RID: 17948
		[Serialize]
		public float transitionTime;

		// Token: 0x0400461D RID: 17949
		[Serialize]
		public string facadeAnim;

		// Token: 0x0400461E RID: 17950
		[Serialize]
		public string symbolID;
	}

	// Token: 0x020012B8 RID: 4792
	public class States : GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon>
	{
		// Token: 0x0600627E RID: 25214 RVA: 0x002B6D5C File Offset: 0x002B4F5C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Transition(this.destroy, (EquippableBalloon.StatesInstance smi) => GameClock.Instance.GetTime() >= smi.transitionTime, UpdateRate.SIM_200ms);
			this.destroy.Enter(delegate(EquippableBalloon.StatesInstance smi)
			{
				smi.master.GetComponent<Equippable>().Unassign();
			});
		}

		// Token: 0x0400461F RID: 17951
		public GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.State destroy;
	}
}
