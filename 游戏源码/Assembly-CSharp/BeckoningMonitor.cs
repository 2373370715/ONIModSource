using System;
using Klei.AI;
using UnityEngine;

public class BeckoningMonitor : GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>
{
	public class Def : BaseDef
	{
		public float caloriesPerCycle;

		public string effectId = "MooWellFed";

		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Beckoning.Id);
		}
	}

	public new class Instance : GameInstance
	{
		private AmountInstance beckoning;

		[MyCmpGet]
		private Effects effects;

		[MyCmpGet]
		public KSelectable kselectable;

		private Guid beckoningBlockedHandle;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			beckoning = Db.Get().Amounts.Beckoning.Lookup(base.gameObject);
		}

		private bool IsSpaceVisible()
		{
			int num = Grid.PosToCell(this);
			if (Grid.IsValidCell(num))
			{
				return Grid.ExposedToSunlight[num] > 0;
			}
			return false;
		}

		private bool IsBeckoningAvailable()
		{
			return base.smi.beckoning.value >= base.smi.beckoning.GetMax();
		}

		public bool IsReadyToBeckon()
		{
			if (IsBeckoningAvailable())
			{
				return IsSpaceVisible();
			}
			return false;
		}

		public void UpdateBlockedStatusItem()
		{
			bool flag = IsSpaceVisible();
			if (!flag && IsBeckoningAvailable() && beckoningBlockedHandle == Guid.Empty)
			{
				beckoningBlockedHandle = kselectable.AddStatusItem(Db.Get().CreatureStatusItems.BeckoningBlocked);
			}
			else if (flag)
			{
				beckoningBlockedHandle = kselectable.RemoveStatusItem(beckoningBlockedHandle);
			}
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
	}

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = root;
		root.EventHandler(GameHashes.CaloriesConsumed, delegate(Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.WantsToBeckon, (Instance smi) => smi.IsReadyToBeckon()).Update(delegate(Instance smi, float dt)
		{
			smi.UpdateBlockedStatusItem();
		}, UpdateRate.SIM_1000ms);
	}
}
