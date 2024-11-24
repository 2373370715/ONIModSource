using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TemplateClasses;
using UnityEngine;

// Token: 0x02001190 RID: 4496
public class Growing : StateMachineComponent<Growing.StatesInstance>, IGameObjectEffectDescriptor, IManageGrowingStates
{
	// Token: 0x17000572 RID: 1394
	// (get) Token: 0x06005BB2 RID: 23474 RVA: 0x000DBCB3 File Offset: 0x000D9EB3
	private Crop crop
	{
		get
		{
			if (this._crop == null)
			{
				this._crop = base.GetComponent<Crop>();
			}
			return this._crop;
		}
	}

	// Token: 0x06005BB3 RID: 23475 RVA: 0x00298960 File Offset: 0x00296B60
	protected override void OnPrefabInit()
	{
		Amounts amounts = base.gameObject.GetAmounts();
		this.maturity = amounts.Get(Db.Get().Amounts.Maturity);
		this.oldAge = amounts.Add(new AmountInstance(Db.Get().Amounts.OldAge, base.gameObject));
		this.oldAge.maxAttribute.ClearModifiers();
		this.oldAge.maxAttribute.Add(new AttributeModifier(Db.Get().Amounts.OldAge.maxAttribute.Id, this.maxAge, null, false, false, true));
		base.OnPrefabInit();
		base.Subscribe<Growing>(1119167081, Growing.OnNewGameSpawnDelegate);
		base.Subscribe<Growing>(1272413801, Growing.ResetGrowthDelegate);
	}

	// Token: 0x06005BB4 RID: 23476 RVA: 0x000DBCD5 File Offset: 0x000D9ED5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		base.gameObject.AddTag(GameTags.GrowingPlant);
	}

	// Token: 0x06005BB5 RID: 23477 RVA: 0x00298A2C File Offset: 0x00296C2C
	private void OnNewGameSpawn(object data)
	{
		Prefab prefab = (Prefab)data;
		if (prefab.amounts != null)
		{
			foreach (Prefab.template_amount_value template_amount_value in prefab.amounts)
			{
				if (template_amount_value.id == this.maturity.amount.Id && template_amount_value.value == this.GetMaxMaturity())
				{
					return;
				}
			}
		}
		this.maturity.SetValue(this.maturity.maxAttribute.GetTotalValue() * UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x06005BB6 RID: 23478 RVA: 0x00298ABC File Offset: 0x00296CBC
	public void OverrideMaturityLevel(float percent)
	{
		float value = this.maturity.GetMax() * percent;
		this.maturity.SetValue(value);
	}

	// Token: 0x06005BB7 RID: 23479 RVA: 0x000DBCF8 File Offset: 0x000D9EF8
	public bool ReachedNextHarvest()
	{
		return this.PercentOfCurrentHarvest() >= 1f;
	}

	// Token: 0x06005BB8 RID: 23480 RVA: 0x000DBD0A File Offset: 0x000D9F0A
	public bool IsGrown()
	{
		return this.maturity.value == this.maturity.GetMax();
	}

	// Token: 0x06005BB9 RID: 23481 RVA: 0x000DBD24 File Offset: 0x000D9F24
	public bool CanGrow()
	{
		return !this.IsGrown();
	}

	// Token: 0x06005BBA RID: 23482 RVA: 0x000DBD2F File Offset: 0x000D9F2F
	public bool IsGrowing()
	{
		return this.maturity.GetDelta() > 0f;
	}

	// Token: 0x06005BBB RID: 23483 RVA: 0x000DBD43 File Offset: 0x000D9F43
	public void ClampGrowthToHarvest()
	{
		this.maturity.value = this.maturity.GetMax();
	}

	// Token: 0x06005BBC RID: 23484 RVA: 0x000DBD5B File Offset: 0x000D9F5B
	public float GetMaxMaturity()
	{
		return this.maturity.GetMax();
	}

	// Token: 0x06005BBD RID: 23485 RVA: 0x000DBD68 File Offset: 0x000D9F68
	public float PercentOfCurrentHarvest()
	{
		return this.maturity.value / this.maturity.GetMax();
	}

	// Token: 0x06005BBE RID: 23486 RVA: 0x000DBD81 File Offset: 0x000D9F81
	public float TimeUntilNextHarvest()
	{
		return (this.maturity.GetMax() - this.maturity.value) / this.maturity.GetDelta();
	}

	// Token: 0x06005BBF RID: 23487 RVA: 0x000DBDA6 File Offset: 0x000D9FA6
	public float DomesticGrowthTime()
	{
		return this.maturity.GetMax() / base.smi.baseGrowingRate.Value;
	}

	// Token: 0x06005BC0 RID: 23488 RVA: 0x000DBDC4 File Offset: 0x000D9FC4
	public float WildGrowthTime()
	{
		return this.maturity.GetMax() / base.smi.wildGrowingRate.Value;
	}

	// Token: 0x06005BC1 RID: 23489 RVA: 0x000DBD68 File Offset: 0x000D9F68
	public float PercentGrown()
	{
		return this.maturity.value / this.maturity.GetMax();
	}

	// Token: 0x06005BC2 RID: 23490 RVA: 0x000DBDE2 File Offset: 0x000D9FE2
	public void ResetGrowth(object data = null)
	{
		this.maturity.value = 0f;
	}

	// Token: 0x06005BC3 RID: 23491 RVA: 0x000DBDF4 File Offset: 0x000D9FF4
	public float PercentOldAge()
	{
		if (!this.shouldGrowOld)
		{
			return 0f;
		}
		return this.oldAge.value / this.oldAge.GetMax();
	}

	// Token: 0x06005BC4 RID: 23492 RVA: 0x00298AE4 File Offset: 0x00296CE4
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Klei.AI.Attribute maxAttribute = Db.Get().Amounts.Maturity.maxAttribute;
		list.Add(new Descriptor(go.GetComponent<Modifiers>().GetPreModifiedAttributeDescription(maxAttribute), go.GetComponent<Modifiers>().GetPreModifiedAttributeToolTip(maxAttribute), Descriptor.DescriptorType.Requirement, false));
		return list;
	}

	// Token: 0x06005BC5 RID: 23493 RVA: 0x00298B30 File Offset: 0x00296D30
	public void ConsumeMass(float mass_to_consume)
	{
		float value = this.maturity.value;
		mass_to_consume = Mathf.Min(mass_to_consume, value);
		this.maturity.value = this.maturity.value - mass_to_consume;
		base.gameObject.Trigger(-1793167409, null);
	}

	// Token: 0x06005BC6 RID: 23494 RVA: 0x00298B7C File Offset: 0x00296D7C
	public void ConsumeGrowthUnits(float units_to_consume, float unit_maturity_ratio)
	{
		float num = units_to_consume / unit_maturity_ratio;
		global::Debug.Assert(num <= this.maturity.value);
		this.maturity.value -= num;
		base.gameObject.Trigger(-1793167409, null);
	}

	// Token: 0x06005BC7 RID: 23495 RVA: 0x000DBE1B File Offset: 0x000DA01B
	public Crop GetGropComponent()
	{
		return base.GetComponent<Crop>();
	}

	// Token: 0x040040C3 RID: 16579
	public float GROWTH_RATE = 0.0016666667f;

	// Token: 0x040040C4 RID: 16580
	public float WILD_GROWTH_RATE = 0.00041666668f;

	// Token: 0x040040C5 RID: 16581
	public bool shouldGrowOld = true;

	// Token: 0x040040C6 RID: 16582
	public float maxAge = 2400f;

	// Token: 0x040040C7 RID: 16583
	private AmountInstance maturity;

	// Token: 0x040040C8 RID: 16584
	private AmountInstance oldAge;

	// Token: 0x040040C9 RID: 16585
	[MyCmpGet]
	private WiltCondition wiltCondition;

	// Token: 0x040040CA RID: 16586
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040040CB RID: 16587
	[MyCmpReq]
	private Modifiers modifiers;

	// Token: 0x040040CC RID: 16588
	[MyCmpReq]
	private ReceptacleMonitor rm;

	// Token: 0x040040CD RID: 16589
	private Crop _crop;

	// Token: 0x040040CE RID: 16590
	private static readonly EventSystem.IntraObjectHandler<Growing> OnNewGameSpawnDelegate = new EventSystem.IntraObjectHandler<Growing>(delegate(Growing component, object data)
	{
		component.OnNewGameSpawn(data);
	});

	// Token: 0x040040CF RID: 16591
	private static readonly EventSystem.IntraObjectHandler<Growing> ResetGrowthDelegate = new EventSystem.IntraObjectHandler<Growing>(delegate(Growing component, object data)
	{
		component.ResetGrowth(data);
	});

	// Token: 0x02001191 RID: 4497
	public class StatesInstance : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.GameInstance
	{
		// Token: 0x06005BCA RID: 23498 RVA: 0x00298BC8 File Offset: 0x00296DC8
		public StatesInstance(Growing master) : base(master)
		{
			this.baseGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, master.GROWTH_RATE, CREATURES.STATS.MATURITY.GROWING, false, false, true);
			this.wildGrowingRate = new AttributeModifier(master.maturity.deltaAttribute.Id, master.WILD_GROWTH_RATE, CREATURES.STATS.MATURITY.GROWINGWILD, false, false, true);
			this.getOldRate = new AttributeModifier(master.oldAge.deltaAttribute.Id, master.shouldGrowOld ? 1f : 0f, null, false, false, true);
		}

		// Token: 0x06005BCB RID: 23499 RVA: 0x000DBE89 File Offset: 0x000DA089
		public bool IsGrown()
		{
			return base.master.IsGrown();
		}

		// Token: 0x06005BCC RID: 23500 RVA: 0x000DBE96 File Offset: 0x000DA096
		public bool ReachedNextHarvest()
		{
			return base.master.ReachedNextHarvest();
		}

		// Token: 0x06005BCD RID: 23501 RVA: 0x000DBEA3 File Offset: 0x000DA0A3
		public void ClampGrowthToHarvest()
		{
			base.master.ClampGrowthToHarvest();
		}

		// Token: 0x06005BCE RID: 23502 RVA: 0x000DBEB0 File Offset: 0x000DA0B0
		public bool IsWilting()
		{
			return base.master.wiltCondition != null && base.master.wiltCondition.IsWilting();
		}

		// Token: 0x06005BCF RID: 23503 RVA: 0x00298C6C File Offset: 0x00296E6C
		public bool IsSleeping()
		{
			CropSleepingMonitor.Instance smi = base.master.GetSMI<CropSleepingMonitor.Instance>();
			return smi != null && smi.IsSleeping();
		}

		// Token: 0x06005BD0 RID: 23504 RVA: 0x000DBED7 File Offset: 0x000DA0D7
		public bool CanExitStalled()
		{
			return !this.IsWilting() && !this.IsSleeping();
		}

		// Token: 0x040040D0 RID: 16592
		public AttributeModifier baseGrowingRate;

		// Token: 0x040040D1 RID: 16593
		public AttributeModifier wildGrowingRate;

		// Token: 0x040040D2 RID: 16594
		public AttributeModifier getOldRate;
	}

	// Token: 0x02001192 RID: 4498
	public class States : GameStateMachine<Growing.States, Growing.StatesInstance, Growing>
	{
		// Token: 0x06005BD1 RID: 23505 RVA: 0x00298C90 File Offset: 0x00296E90
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.growing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.growing.EventTransition(GameHashes.Wilt, this.stalled, (Growing.StatesInstance smi) => smi.IsWilting()).EventTransition(GameHashes.CropSleep, this.stalled, (Growing.StatesInstance smi) => smi.IsSleeping()).EventTransition(GameHashes.ReceptacleMonitorChange, this.growing.planted, (Growing.StatesInstance smi) => smi.master.rm.Replanted).EventTransition(GameHashes.ReceptacleMonitorChange, this.growing.wild, (Growing.StatesInstance smi) => !smi.master.rm.Replanted).EventTransition(GameHashes.PlanterStorage, this.growing.planted, (Growing.StatesInstance smi) => smi.master.rm.Replanted).EventTransition(GameHashes.PlanterStorage, this.growing.wild, (Growing.StatesInstance smi) => !smi.master.rm.Replanted).TriggerOnEnter(GameHashes.Grow, null).Update("CheckGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (smi.ReachedNextHarvest())
				{
					smi.GoTo(this.grown);
				}
			}, UpdateRate.SIM_4000ms, false).ToggleStatusItem(Db.Get().CreatureStatusItems.Growing, (Growing.StatesInstance smi) => smi.master.GetComponent<IManageGrowingStates>()).Enter(delegate(Growing.StatesInstance smi)
			{
				GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State state = smi.master.rm.Replanted ? this.growing.planted : this.growing.wild;
				smi.GoTo(state);
			});
			this.growing.wild.ToggleAttributeModifier("GrowingWild", (Growing.StatesInstance smi) => smi.wildGrowingRate, null);
			this.growing.planted.ToggleAttributeModifier("Growing", (Growing.StatesInstance smi) => smi.baseGrowingRate, null);
			this.stalled.EventTransition(GameHashes.WiltRecover, this.growing, (Growing.StatesInstance smi) => smi.CanExitStalled()).EventTransition(GameHashes.CropWakeUp, this.growing, (Growing.StatesInstance smi) => smi.CanExitStalled());
			this.grown.DefaultState(this.grown.idle).TriggerOnEnter(GameHashes.Grow, null).Update("CheckNotGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (!smi.ReachedNextHarvest())
				{
					smi.GoTo(this.growing);
				}
			}, UpdateRate.SIM_4000ms, false).ToggleAttributeModifier("GettingOld", (Growing.StatesInstance smi) => smi.getOldRate, null).Enter(delegate(Growing.StatesInstance smi)
			{
				smi.ClampGrowthToHarvest();
			}).Exit(delegate(Growing.StatesInstance smi)
			{
				smi.master.oldAge.SetValue(0f);
			});
			this.grown.idle.Update("CheckNotGrown", delegate(Growing.StatesInstance smi, float dt)
			{
				if (smi.master.shouldGrowOld && smi.master.oldAge.value >= smi.master.oldAge.GetMax())
				{
					smi.GoTo(this.grown.try_self_harvest);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.grown.try_self_harvest.Enter(delegate(Growing.StatesInstance smi)
			{
				Harvestable component = smi.master.GetComponent<Harvestable>();
				if (component && component.CanBeHarvested)
				{
					bool harvestWhenReady = component.harvestDesignatable.HarvestWhenReady;
					component.ForceCancelHarvest(null);
					component.Harvest();
					if (harvestWhenReady && component != null)
					{
						component.harvestDesignatable.SetHarvestWhenReady(true);
					}
				}
				smi.master.maturity.SetValue(0f);
				smi.master.oldAge.SetValue(0f);
			}).GoTo(this.grown.idle);
		}

		// Token: 0x040040D3 RID: 16595
		public Growing.States.GrowingStates growing;

		// Token: 0x040040D4 RID: 16596
		public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State stalled;

		// Token: 0x040040D5 RID: 16597
		public Growing.States.GrownStates grown;

		// Token: 0x02001193 RID: 4499
		public class GrowingStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
		{
			// Token: 0x040040D6 RID: 16598
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State wild;

			// Token: 0x040040D7 RID: 16599
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State planted;
		}

		// Token: 0x02001194 RID: 4500
		public class GrownStates : GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State
		{
			// Token: 0x040040D8 RID: 16600
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State idle;

			// Token: 0x040040D9 RID: 16601
			public GameStateMachine<Growing.States, Growing.StatesInstance, Growing, object>.State try_self_harvest;
		}
	}
}
