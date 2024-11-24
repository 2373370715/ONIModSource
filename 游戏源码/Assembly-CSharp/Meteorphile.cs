using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;

[SkipSaveFileSerialization]
public class Meteorphile : StateMachineComponent<Meteorphile.StatesInstance>
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, Meteorphile, object>.GameInstance
	{
		public StatesInstance(Meteorphile master)
			: base(master)
		{
		}

		public bool IsMeteors()
		{
			if (GameplayEventManager.Instance == null || base.master.kPrefabID.PrefabTag == GameTags.MinionSelectPreview)
			{
				return false;
			}
			int myWorldId = this.GetMyWorldId();
			List<GameplayEventInstance> results = new List<GameplayEventInstance>();
			GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(myWorldId, ref results);
			for (int i = 0; i < results.Count; i++)
			{
				if (results[i].smi is MeteorShowerEvent.StatesInstance statesInstance && statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
				{
					return true;
				}
			}
			return false;
		}
	}

	public class States : GameStateMachine<States, StatesInstance, Meteorphile>
	{
		public State idle;

		public State early;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = idle;
			root.TagTransition(GameTags.Dead, null);
			idle.Transition(early, (StatesInstance smi) => smi.IsMeteors());
			early.Enter("Meteors", delegate(StatesInstance smi)
			{
				smi.master.ApplyModifiers();
			}).Exit("NotMeteors", delegate(StatesInstance smi)
			{
				smi.master.RemoveModifiers();
			}).ToggleStatusItem(Db.Get().DuplicantStatusItems.Meteorphile)
				.ToggleExpression(Db.Get().Expressions.Happy)
				.Transition(idle, (StatesInstance smi) => !smi.IsMeteors());
		}
	}

	[MyCmpReq]
	private KPrefabID kPrefabID;

	private AttributeModifier[] attributeModifiers;

	protected override void OnSpawn()
	{
		attributeModifiers = new AttributeModifier[11]
		{
			new AttributeModifier("Construction", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME),
			new AttributeModifier("Digging", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME),
			new AttributeModifier("Machinery", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME),
			new AttributeModifier("Athletics", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME),
			new AttributeModifier("Learning", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME),
			new AttributeModifier("Cooking", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME),
			new AttributeModifier("Art", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME),
			new AttributeModifier("Strength", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME),
			new AttributeModifier("Caring", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME),
			new AttributeModifier("Botanist", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME),
			new AttributeModifier("Ranching", TRAITS.METEORPHILE_MODIFIER, DUPLICANTS.TRAITS.METEORPHILE.NAME)
		};
		base.smi.StartSM();
	}

	public void ApplyModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = attributeModifiers[i];
			attributes.Add(modifier);
		}
	}

	public void RemoveModifiers()
	{
		Attributes attributes = base.gameObject.GetAttributes();
		for (int i = 0; i < attributeModifiers.Length; i++)
		{
			AttributeModifier modifier = attributeModifiers[i];
			attributes.Remove(modifier);
		}
	}
}
