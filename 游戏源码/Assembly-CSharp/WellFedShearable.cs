using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class WellFedShearable : GameStateMachine<WellFedShearable, WellFedShearable.Instance, IStateMachineTarget, WellFedShearable.Def>
{
	public class Def : BaseDef, IGameObjectEffectDescriptor
	{
		public string effectId;

		public float caloriesPerCycle;

		public float growthDurationCycles;

		public int levelCount;

		public Tag itemDroppedOnShear;

		public float dropMass;

		public override void Configure(GameObject prefab)
		{
			prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
		}

		public List<Descriptor> GetDescriptors(GameObject obj)
		{
			return new List<Descriptor>
			{
				new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(growthDurationCycles * 600f)), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_FED.Replace("{Item}", itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(dropMass)).Replace("{Time}", GameUtil.GetFormattedCycles(growthDurationCycles * 600f)))
			};
		}
	}

	public new class Instance : GameInstance, IShearable
	{
		[MyCmpGet]
		private Effects effects;

		public AmountInstance scaleGrowth;

		public int currentScaleLevel = -1;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(base.gameObject);
			scaleGrowth.value = scaleGrowth.GetMax();
		}

		public bool IsFullyGrown()
		{
			return currentScaleLevel == base.def.levelCount;
		}

		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			EffectInstance effectInstance = effects.Get(base.smi.def.effectId);
			if (effectInstance == null)
			{
				effectInstance = effects.Add(base.smi.def.effectId, should_save: true);
			}
			effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.caloriesPerCycle * 600f;
		}

		public void Shear()
		{
			PrimaryElement component = base.smi.GetComponent<PrimaryElement>();
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.def.itemDroppedOnShear));
			gameObject.transform.SetPosition(Grid.CellToPosCCC(Grid.CellLeft(Grid.PosToCell(this)), Grid.SceneLayer.Ore));
			PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
			component2.Temperature = component.Temperature;
			component2.Mass = base.def.dropMass;
			component2.AddDisease(component.DiseaseIdx, component.DiseaseCount, "Shearing");
			gameObject.SetActive(value: true);
			Vector2 initial_velocity = new Vector2(Random.Range(-1f, 1f) * 1f, Random.value * 2f + 2f);
			if (GameComps.Fallers.Has(gameObject))
			{
				GameComps.Fallers.Remove(gameObject);
			}
			GameComps.Fallers.Add(gameObject, initial_velocity);
			scaleGrowth.value = 0f;
			UpdateScales(this, 0f);
		}
	}

	public State growing;

	public State fullyGrown;

	private static HashedString[] SCALE_SYMBOL_NAMES = new HashedString[5] { "scale_0", "scale_1", "scale_2", "scale_3", "scale_4" };

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = growing;
		root.Enter(delegate(Instance smi)
		{
			UpdateScales(smi, 0f);
		}).Update(UpdateScales, UpdateRate.SIM_1000ms).EventHandler(GameHashes.CaloriesConsumed, delegate(Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		});
		growing.Enter(delegate(Instance smi)
		{
			UpdateScales(smi, 0f);
		}).Transition(fullyGrown, AreScalesFullyGrown, UpdateRate.SIM_1000ms);
		fullyGrown.Enter(delegate(Instance smi)
		{
			UpdateScales(smi, 0f);
		}).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (Instance smi) => smi.HasTag(GameTags.Creatures.CanMolt)).EventTransition(GameHashes.Molt, growing, GameStateMachine<WellFedShearable, Instance, IStateMachineTarget, Def>.Not(AreScalesFullyGrown))
			.Transition(growing, GameStateMachine<WellFedShearable, Instance, IStateMachineTarget, Def>.Not(AreScalesFullyGrown), UpdateRate.SIM_1000ms);
	}

	private static bool AreScalesFullyGrown(Instance smi)
	{
		return smi.scaleGrowth.value >= smi.scaleGrowth.GetMax();
	}

	private static void UpdateScales(Instance smi, float dt)
	{
		int num = (int)((float)smi.def.levelCount * smi.scaleGrowth.value / 100f);
		if (smi.currentScaleLevel != num)
		{
			KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < SCALE_SYMBOL_NAMES.Length; i++)
			{
				bool is_visible = i <= num - 1;
				component.SetSymbolVisiblity(SCALE_SYMBOL_NAMES[i], is_visible);
			}
			smi.currentScaleLevel = num;
		}
	}
}
