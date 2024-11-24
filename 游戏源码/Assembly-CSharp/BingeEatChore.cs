using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200066C RID: 1644
public class BingeEatChore : Chore<BingeEatChore.StatesInstance>
{
	// Token: 0x06001DD8 RID: 7640 RVA: 0x001B0AC8 File Offset: 0x001AECC8
	public BingeEatChore(IStateMachineTarget target, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.BingeEat, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new BingeEatChore.StatesInstance(this, target.gameObject);
		base.Subscribe(1121894420, new Action<object>(this.OnEat));
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x001B0B28 File Offset: 0x001AED28
	private void OnEat(object data)
	{
		Edible edible = (Edible)data;
		if (edible != null)
		{
			base.smi.sm.bingeremaining.Set(Mathf.Max(0f, base.smi.sm.bingeremaining.Get(base.smi) - edible.unitsConsumed), base.smi, false);
		}
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x000B3B6D File Offset: 0x000B1D6D
	public override void Cleanup()
	{
		base.Cleanup();
		base.Unsubscribe(1121894420, new Action<object>(this.OnEat));
	}

	// Token: 0x0200066D RID: 1645
	public class StatesInstance : GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.GameInstance
	{
		// Token: 0x06001DDB RID: 7643 RVA: 0x000B3B8C File Offset: 0x000B1D8C
		public StatesInstance(BingeEatChore master, GameObject eater) : base(master)
		{
			base.sm.eater.Set(eater, base.smi, false);
			base.sm.bingeremaining.Set(2f, base.smi, false);
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x001B0B90 File Offset: 0x001AED90
		public void FindFood()
		{
			Navigator component = base.GetComponent<Navigator>();
			int num = int.MaxValue;
			Edible edible = null;
			if (base.sm.bingeremaining.Get(base.smi) <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				this.GoTo(base.sm.eat_pst);
				return;
			}
			foreach (Edible edible2 in Components.Edibles.Items)
			{
				if (!edible2.HasTag(GameTags.Dehydrated) && !(edible2 == null) && !(edible2 == base.sm.ediblesource.Get<Edible>(base.smi)) && !edible2.isBeingConsumed)
				{
					Pickupable component2 = edible2.GetComponent<Pickupable>();
					if (component2.UnreservedAmount > 0f && component2.CouldBePickedUpByMinion(base.gameObject) && !component2.HasTag(GameTags.StoredPrivate))
					{
						int navigationCost = component.GetNavigationCost(edible2);
						if (navigationCost != -1 && navigationCost < num)
						{
							num = navigationCost;
							edible = edible2;
						}
					}
				}
			}
			base.sm.ediblesource.Set(edible, base.smi);
			base.sm.requestedfoodunits.Set(base.sm.bingeremaining.Get(base.smi), base.smi, false);
			if (edible == null)
			{
				this.GoTo(base.sm.cantFindFood);
				return;
			}
			this.GoTo(base.sm.fetch);
		}

		// Token: 0x06001DDD RID: 7645 RVA: 0x000B3BCB File Offset: 0x000B1DCB
		public bool IsBingeEating()
		{
			return base.sm.isBingeEating.Get(base.smi);
		}
	}

	// Token: 0x0200066E RID: 1646
	public class States : GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore>
	{
		// Token: 0x06001DDE RID: 7646 RVA: 0x001B0D28 File Offset: 0x001AEF28
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.findfood;
			base.Target(this.eater);
			this.bingeEatingEffect = new Effect("Binge_Eating", DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, DUPLICANTS.MODIFIERS.BINGE_EATING.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.bingeEatingEffect.Add(new AttributeModifier(Db.Get().Attributes.Decor.Id, -30f, DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, false, false, true));
			this.bingeEatingEffect.Add(new AttributeModifier("CaloriesDelta", -6666.6665f, DUPLICANTS.MODIFIERS.BINGE_EATING.NAME, false, false, true));
			Db.Get().effects.Add(this.bingeEatingEffect);
			this.root.ToggleEffect((BingeEatChore.StatesInstance smi) => this.bingeEatingEffect);
			this.noTarget.GoTo(this.finish);
			this.eat_pst.ToggleAnims("anim_eat_overeat_kanim", 0f).PlayAnim("working_pst").OnAnimQueueComplete(this.finish);
			this.finish.Enter(delegate(BingeEatChore.StatesInstance smi)
			{
				smi.StopSM("complete/no more food");
			});
			this.findfood.Enter("FindFood", delegate(BingeEatChore.StatesInstance smi)
			{
				smi.FindFood();
			});
			this.fetch.InitializeStates(this.eater, this.ediblesource, this.ediblechunk, this.requestedfoodunits, this.actualfoodunits, this.eat, this.cantFindFood);
			this.eat.ToggleAnims("anim_eat_overeat_kanim", 0f).QueueAnim("working_loop", true, null).Enter(delegate(BingeEatChore.StatesInstance smi)
			{
				this.isBingeEating.Set(true, smi, false);
			}).DoEat(this.ediblechunk, this.actualfoodunits, this.findfood, this.findfood).Exit("ClearIsBingeEating", delegate(BingeEatChore.StatesInstance smi)
			{
				this.isBingeEating.Set(false, smi, false);
			});
			this.cantFindFood.ToggleAnims("anim_interrupt_binge_eat_kanim", 0f).PlayAnim("interrupt_binge_eat").OnAnimQueueComplete(this.noTarget);
		}

		// Token: 0x040012B1 RID: 4785
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter eater;

		// Token: 0x040012B2 RID: 4786
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter ediblesource;

		// Token: 0x040012B3 RID: 4787
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.TargetParameter ediblechunk;

		// Token: 0x040012B4 RID: 4788
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.BoolParameter isBingeEating;

		// Token: 0x040012B5 RID: 4789
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter requestedfoodunits;

		// Token: 0x040012B6 RID: 4790
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter actualfoodunits;

		// Token: 0x040012B7 RID: 4791
		public StateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FloatParameter bingeremaining;

		// Token: 0x040012B8 RID: 4792
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State noTarget;

		// Token: 0x040012B9 RID: 4793
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State findfood;

		// Token: 0x040012BA RID: 4794
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State eat;

		// Token: 0x040012BB RID: 4795
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State eat_pst;

		// Token: 0x040012BC RID: 4796
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State cantFindFood;

		// Token: 0x040012BD RID: 4797
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.State finish;

		// Token: 0x040012BE RID: 4798
		public GameStateMachine<BingeEatChore.States, BingeEatChore.StatesInstance, BingeEatChore, object>.FetchSubState fetch;

		// Token: 0x040012BF RID: 4799
		private Effect bingeEatingEffect;
	}
}
