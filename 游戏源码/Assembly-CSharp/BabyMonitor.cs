using System;
using Klei;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class BabyMonitor : GameStateMachine<BabyMonitor, BabyMonitor.Instance, IStateMachineTarget, BabyMonitor.Def>
{
	public class Def : BaseDef
	{
		public Tag adultPrefab;

		public string onGrowDropID;

		public bool forceAdultNavType;

		public float adultThreshold = 5f;

		public Action<GameObject> configureAdultOnMaturation;
	}

	public new class Instance : GameInstance
	{
		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public void SpawnAdult()
		{
			Vector3 position = base.smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.smi.def.adultPrefab), position);
			gameObject.SetActive(value: true);
			if (!base.smi.gameObject.HasTag(GameTags.Creatures.PreventGrowAnimation))
			{
				gameObject.GetSMI<AnimInterruptMonitor.Instance>().PlayAnim("growup_pst");
			}
			if (base.smi.def.onGrowDropID != null)
			{
				Util.KInstantiate(Assets.GetPrefab(base.smi.def.onGrowDropID), position).SetActive(value: true);
			}
			foreach (AmountInstance amount in base.gameObject.GetAmounts())
			{
				AmountInstance amountInstance = amount.amount.Lookup(gameObject);
				if (amountInstance != null)
				{
					float num = amount.value / amount.GetMax();
					amountInstance.value = num * amountInstance.GetMax();
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
				SelectTool.Instance.Select(gameObject.GetComponent<KSelectable>());
			}
			base.smi.gameObject.Trigger(663420073, gameObject);
			base.smi.gameObject.DeleteObject();
			if (base.def.configureAdultOnMaturation != null)
			{
				base.def.configureAdultOnMaturation(gameObject);
			}
		}
	}

	public State baby;

	public State spawnadult;

	public Effect babyEffect;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = baby;
		root.Enter(AddBabyEffect);
		baby.Transition(spawnadult, IsReadyToSpawnAdult, UpdateRate.SIM_4000ms);
		spawnadult.ToggleBehaviour(GameTags.Creatures.Behaviours.GrowUpBehaviour, (Instance smi) => true);
		babyEffect = new Effect("IsABaby", CREATURES.MODIFIERS.BABY.NAME, CREATURES.MODIFIERS.BABY.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: false);
		babyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Metabolism.Id, -0.9f, CREATURES.MODIFIERS.BABY.NAME, is_multiplier: true));
		babyEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 5f, CREATURES.MODIFIERS.BABY.NAME));
	}

	private static void AddBabyEffect(Instance smi)
	{
		smi.Get<Effects>().Add(smi.sm.babyEffect, should_save: false);
	}

	private static bool IsReadyToSpawnAdult(Instance smi)
	{
		AmountInstance amountInstance = Db.Get().Amounts.Age.Lookup(smi.gameObject);
		float num = smi.def.adultThreshold;
		if (GenericGameSettings.instance.acceleratedLifecycle)
		{
			num = 0.005f;
		}
		return amountInstance.value > num;
	}
}
