using System;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001126 RID: 4390
public class BabyMonitor : GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>
{
	// Token: 0x060059EE RID: 23022 RVA: 0x00293450 File Offset: 0x00291650
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.baby;
		this.root.Enter(new StateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State.Callback(BabyMonitor.AddBabyEffect));
		this.baby.Transition(this.spawnadult, new StateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.Transition.ConditionCallback(BabyMonitor.IsReadyToSpawnAdult), UpdateRate.SIM_4000ms);
		this.spawnadult.ToggleBehaviour(GameTags.Creatures.Behaviours.GrowUpBehaviour, (BabyMonitor.Instance smi) => true, null);
		this.babyEffect = new Effect("IsABaby", CREATURES.MODIFIERS.BABY.NAME, CREATURES.MODIFIERS.BABY.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		this.babyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -0.9f, CREATURES.MODIFIERS.BABY.NAME, true, false, true));
		this.babyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, CREATURES.MODIFIERS.BABY.NAME, false, false, true));
	}

	// Token: 0x060059EF RID: 23023 RVA: 0x000DA921 File Offset: 0x000D8B21
	private static void AddBabyEffect(BabyMonitor.Instance smi)
	{
		smi.Get<Effects>().Add(smi.sm.babyEffect, false);
	}

	// Token: 0x060059F0 RID: 23024 RVA: 0x00293578 File Offset: 0x00291778
	private static bool IsReadyToSpawnAdult(BabyMonitor.Instance smi)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Age.Lookup(smi.gameObject);
		float num = smi.def.adultThreshold;
		if (GenericGameSettings.instance.acceleratedLifecycle)
		{
			num = 0.005f;
		}
		return amountInstance.value > num;
	}

	// Token: 0x04003F72 RID: 16242
	public GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State baby;

	// Token: 0x04003F73 RID: 16243
	public GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.State spawnadult;

	// Token: 0x04003F74 RID: 16244
	public Effect babyEffect;

	// Token: 0x02001127 RID: 4391
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003F75 RID: 16245
		public Tag adultPrefab;

		// Token: 0x04003F76 RID: 16246
		public string onGrowDropID;

		// Token: 0x04003F77 RID: 16247
		public bool forceAdultNavType;

		// Token: 0x04003F78 RID: 16248
		public float adultThreshold = 5f;

		// Token: 0x04003F79 RID: 16249
		public Action<GameObject> configureAdultOnMaturation;
	}

	// Token: 0x02001128 RID: 4392
	public new class Instance : GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>.GameInstance
	{
		// Token: 0x060059F3 RID: 23027 RVA: 0x000DA956 File Offset: 0x000D8B56
		public Instance(IStateMachineTarget master, BabyMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x002935C8 File Offset: 0x002917C8
		public void SpawnAdult()
		{
			Vector3 position = base.smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.smi.def.adultPrefab), position);
			gameObject.SetActive(true);
			if (!base.smi.gameObject.HasTag(GameTags.Creatures.PreventGrowAnimation))
			{
				gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("growup_pst");
			}
			if (base.smi.def.onGrowDropID != null)
			{
				Util.KInstantiate(Assets.GetPrefab(base.smi.def.onGrowDropID), position).SetActive(true);
			}
			foreach (AmountInstance amountInstance in base.gameObject.GetAmounts())
			{
				AmountInstance amountInstance2 = amountInstance.amount.Lookup(gameObject);
				if (amountInstance2 != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					amountInstance2.value = num * amountInstance2.GetMax();
				}
			}
			EffectInstance effectInstance = base.gameObject.GetComponent<Effects>().Get("AteFromFeeder");
			if (effectInstance != null)
			{
				gameObject.GetComponent<Effects>().Add(effectInstance.effect, effectInstance.shouldSave).timeRemaining = effectInstance.timeRemaining;
			}
			if (!base.smi.def.forceAdultNavType)
			{
				Navigator component = base.smi.GetComponent<Navigator>();
				gameObject.GetComponent<Navigator>().SetCurrentNavType(component.CurrentNavType);
			}
			gameObject.Trigger(-2027483228, base.gameObject);
			KSelectable component2 = base.gameObject.GetComponent<KSelectable>();
			if (SelectTool.Instance != null && SelectTool.Instance.selected != null && SelectTool.Instance.selected == component2)
			{
				SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>(), false);
			}
			base.smi.gameObject.Trigger(663420073, gameObject);
			base.smi.gameObject.DeleteObject();
			if (base.def.configureAdultOnMaturation != null)
			{
				base.def.configureAdultOnMaturation(gameObject);
			}
		}
	}
}
