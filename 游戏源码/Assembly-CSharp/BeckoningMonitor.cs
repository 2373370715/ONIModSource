using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200112A RID: 4394
public class BeckoningMonitor : GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>
{
	// Token: 0x060059F8 RID: 23032 RVA: 0x00293804 File Offset: 0x00291A04
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(BeckoningMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.WantsToBeckon, (BeckoningMonitor.Instance smi) => smi.IsReadyToBeckon(), null).Update(delegate(BeckoningMonitor.Instance smi, float dt)
		{
			smi.UpdateBlockedStatusItem();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x0200112B RID: 4395
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060059FA RID: 23034 RVA: 0x000DA974 File Offset: 0x000D8B74
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Beckoning.Id);
		}

		// Token: 0x04003F7C RID: 16252
		public float caloriesPerCycle;

		// Token: 0x04003F7D RID: 16253
		public string effectId = "MooWellFed";
	}

	// Token: 0x0200112C RID: 4396
	public new class Instance : GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>.GameInstance
	{
		// Token: 0x060059FC RID: 23036 RVA: 0x000DA9AD File Offset: 0x000D8BAD
		public Instance(IStateMachineTarget master, BeckoningMonitor.Def def) : base(master, def)
		{
			this.beckoning = Db.Get().Amounts.Beckoning.Lookup(base.gameObject);
		}

		// Token: 0x060059FD RID: 23037 RVA: 0x0029389C File Offset: 0x00291A9C
		private bool IsSpaceVisible()
		{
			int num = Grid.PosToCell(this);
			return Grid.IsValidCell(num) && Grid.ExposedToSunlight[num] > 0;
		}

		// Token: 0x060059FE RID: 23038 RVA: 0x000DA9D7 File Offset: 0x000D8BD7
		private bool IsBeckoningAvailable()
		{
			return base.smi.beckoning.value >= base.smi.beckoning.GetMax();
		}

		// Token: 0x060059FF RID: 23039 RVA: 0x000DA9FE File Offset: 0x000D8BFE
		public bool IsReadyToBeckon()
		{
			return this.IsBeckoningAvailable() && this.IsSpaceVisible();
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x002938C8 File Offset: 0x00291AC8
		public void UpdateBlockedStatusItem()
		{
			bool flag = this.IsSpaceVisible();
			if (!flag && this.IsBeckoningAvailable() && this.beckoningBlockedHandle == Guid.Empty)
			{
				this.beckoningBlockedHandle = this.kselectable.AddStatusItem(Db.Get().CreatureStatusItems.BeckoningBlocked, null);
				return;
			}
			if (flag)
			{
				this.beckoningBlockedHandle = this.kselectable.RemoveStatusItem(this.beckoningBlockedHandle, false);
			}
		}

		// Token: 0x06005A01 RID: 23041 RVA: 0x00293938 File Offset: 0x00291B38
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			EffectInstance effectInstance = this.effects.Get(base.smi.def.effectId);
			if (effectInstance == null)
			{
				effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.caloriesPerCycle * 600f;
		}

		// Token: 0x04003F7E RID: 16254
		private AmountInstance beckoning;

		// Token: 0x04003F7F RID: 16255
		[MyCmpGet]
		private Effects effects;

		// Token: 0x04003F80 RID: 16256
		[MyCmpGet]
		public KSelectable kselectable;

		// Token: 0x04003F81 RID: 16257
		private Guid beckoningBlockedHandle;
	}
}
