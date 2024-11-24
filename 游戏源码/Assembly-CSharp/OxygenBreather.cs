using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200168D RID: 5773
[RequireComponent(typeof(Health))]
[AddComponentMenu("KMonoBehaviour/scripts/OxygenBreather")]
public class OxygenBreather : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000780 RID: 1920
	// (get) Token: 0x06007734 RID: 30516 RVA: 0x000EE42B File Offset: 0x000EC62B
	public float ConsumptionRate
	{
		get
		{
			if (this.airConsumptionRate != null)
			{
				return this.airConsumptionRate.GetTotalValue();
			}
			return 0f;
		}
	}

	// Token: 0x17000781 RID: 1921
	// (get) Token: 0x06007735 RID: 30517 RVA: 0x000EE446 File Offset: 0x000EC646
	public float CO2EmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.co2Accumulator);
		}
	}

	// Token: 0x17000782 RID: 1922
	// (get) Token: 0x06007736 RID: 30518 RVA: 0x000EE45D File Offset: 0x000EC65D
	public HandleVector<int>.Handle O2Accumulator
	{
		get
		{
			return this.o2Accumulator;
		}
	}

	// Token: 0x06007737 RID: 30519 RVA: 0x000EE465 File Offset: 0x000EC665
	protected override void OnPrefabInit()
	{
		GameUtil.SubscribeToTags<OxygenBreather>(this, OxygenBreather.OnDeadTagAddedDelegate, true);
	}

	// Token: 0x06007738 RID: 30520 RVA: 0x000EE473 File Offset: 0x000EC673
	public bool IsLowOxygenAtMouthCell()
	{
		return this.GetOxygenPressure(this.mouthCell) < this.lowOxygenThreshold;
	}

	// Token: 0x06007739 RID: 30521 RVA: 0x0030D594 File Offset: 0x0030B794
	protected override void OnSpawn()
	{
		this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup(this);
		this.o2Accumulator = Game.Instance.accumulators.Add("O2", this);
		this.co2Accumulator = Game.Instance.accumulators.Add("CO2", this);
		KSelectable component = base.GetComponent<KSelectable>();
		component.AddStatusItem(Db.Get().DuplicantStatusItems.BreathingO2, this);
		component.AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, this);
		this.temperature = Db.Get().Amounts.Temperature.Lookup(this);
		NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
	}

	// Token: 0x0600773A RID: 30522 RVA: 0x000EE489 File Offset: 0x000EC689
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.o2Accumulator);
		Game.Instance.accumulators.Remove(this.co2Accumulator);
		this.SetGasProvider(null);
		base.OnCleanUp();
	}

	// Token: 0x0600773B RID: 30523 RVA: 0x000EE4C4 File Offset: 0x000EC6C4
	public void Consume(Sim.MassConsumedCallback mass_consumed)
	{
		if (this.onSimConsume != null)
		{
			this.onSimConsume(mass_consumed);
		}
	}

	// Token: 0x0600773C RID: 30524 RVA: 0x0030D654 File Offset: 0x0030B854
	public void Sim200ms(float dt)
	{
		if (!base.gameObject.HasTag(GameTags.Dead))
		{
			float num = this.airConsumptionRate.GetTotalValue() * dt;
			bool flag = this.gasProvider.ConsumeGas(this, num);
			if (flag)
			{
				if (this.gasProvider.ShouldEmitCO2())
				{
					float num2 = num * this.O2toCO2conversion;
					Game.Instance.accumulators.Accumulate(this.co2Accumulator, num2);
					this.accumulatedCO2 += num2;
					if (this.accumulatedCO2 >= this.minCO2ToEmit)
					{
						this.accumulatedCO2 -= this.minCO2ToEmit;
						Vector3 position = base.transform.GetPosition();
						Vector3 vector = position;
						vector.x += (this.facing.GetFacing() ? (-this.mouthOffset.x) : this.mouthOffset.x);
						vector.y += this.mouthOffset.y;
						vector.z -= 0.5f;
						if (Mathf.FloorToInt(vector.x) != Mathf.FloorToInt(position.x))
						{
							vector.x = Mathf.Floor(position.x) + (this.facing.GetFacing() ? 0.01f : 0.99f);
						}
						CO2Manager.instance.SpawnBreath(vector, this.minCO2ToEmit, this.temperature.value, this.facing.GetFacing());
					}
				}
				else if (this.gasProvider.ShouldStoreCO2())
				{
					Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
					if (equippable != null)
					{
						float num3 = num * this.O2toCO2conversion;
						Game.Instance.accumulators.Accumulate(this.co2Accumulator, num3);
						this.accumulatedCO2 += num3;
						if (this.accumulatedCO2 >= this.minCO2ToEmit)
						{
							this.accumulatedCO2 -= this.minCO2ToEmit;
							equippable.GetComponent<Storage>().AddGasChunk(SimHashes.CarbonDioxide, this.minCO2ToEmit, this.temperature.value, byte.MaxValue, 0, false, true);
						}
					}
				}
			}
			if (flag != this.hasAir)
			{
				this.hasAirTimer.Start();
				if (this.hasAirTimer.TryStop(2f))
				{
					this.hasAir = flag;
					base.Trigger(-933153513, this.hasAir);
					return;
				}
			}
			else
			{
				this.hasAirTimer.Stop();
			}
		}
	}

	// Token: 0x0600773D RID: 30525 RVA: 0x000EE4DA File Offset: 0x000EC6DA
	private void OnDeath(object data)
	{
		base.enabled = false;
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().DuplicantStatusItems.BreathingO2, false);
		component.RemoveStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, false);
	}

	// Token: 0x0600773E RID: 30526 RVA: 0x0030D8C4 File Offset: 0x0030BAC4
	private int GetMouthCellAtCell(int cell, CellOffset[] offsets)
	{
		float num = 0f;
		int result = cell;
		foreach (CellOffset offset in offsets)
		{
			int num2 = Grid.OffsetCell(cell, offset);
			float oxygenPressure = this.GetOxygenPressure(num2);
			if (oxygenPressure > num && oxygenPressure > this.noOxygenThreshold)
			{
				num = oxygenPressure;
				result = num2;
			}
		}
		return result;
	}

	// Token: 0x17000783 RID: 1923
	// (get) Token: 0x0600773F RID: 30527 RVA: 0x0030D91C File Offset: 0x0030BB1C
	public int mouthCell
	{
		get
		{
			int cell = Grid.PosToCell(this);
			return this.GetMouthCellAtCell(cell, this.breathableCells);
		}
	}

	// Token: 0x06007740 RID: 30528 RVA: 0x000EE516 File Offset: 0x000EC716
	public bool IsBreathableElementAtCell(int cell, CellOffset[] offsets = null)
	{
		return this.GetBreathableElementAtCell(cell, offsets) != SimHashes.Vacuum;
	}

	// Token: 0x06007741 RID: 30529 RVA: 0x0030D940 File Offset: 0x0030BB40
	public SimHashes GetBreathableElementAtCell(int cell, CellOffset[] offsets = null)
	{
		if (offsets == null)
		{
			offsets = this.breathableCells;
		}
		int mouthCellAtCell = this.GetMouthCellAtCell(cell, offsets);
		if (!Grid.IsValidCell(mouthCellAtCell))
		{
			return SimHashes.Vacuum;
		}
		Element element = Grid.Element[mouthCellAtCell];
		if (!element.IsGas || !element.HasTag(GameTags.Breathable) || Grid.Mass[mouthCellAtCell] <= this.noOxygenThreshold)
		{
			return SimHashes.Vacuum;
		}
		return element.id;
	}

	// Token: 0x17000784 RID: 1924
	// (get) Token: 0x06007742 RID: 30530 RVA: 0x000EE52A File Offset: 0x000EC72A
	public bool IsUnderLiquid
	{
		get
		{
			return Grid.Element[this.mouthCell].IsLiquid;
		}
	}

	// Token: 0x17000785 RID: 1925
	// (get) Token: 0x06007743 RID: 30531 RVA: 0x000EE53D File Offset: 0x000EC73D
	public bool IsSuffocating
	{
		get
		{
			return !this.hasAir;
		}
	}

	// Token: 0x17000786 RID: 1926
	// (get) Token: 0x06007744 RID: 30532 RVA: 0x000EE548 File Offset: 0x000EC748
	public SimHashes GetBreathableElement
	{
		get
		{
			return this.GetBreathableElementAtCell(Grid.PosToCell(this), null);
		}
	}

	// Token: 0x17000787 RID: 1927
	// (get) Token: 0x06007745 RID: 30533 RVA: 0x000EE557 File Offset: 0x000EC757
	public bool IsBreathableElement
	{
		get
		{
			return this.IsBreathableElementAtCell(Grid.PosToCell(this), null);
		}
	}

	// Token: 0x06007746 RID: 30534 RVA: 0x000EE566 File Offset: 0x000EC766
	private float GetOxygenPressure(int cell)
	{
		if (Grid.IsValidCell(cell) && Grid.Element[cell].HasTag(GameTags.Breathable))
		{
			return Grid.Mass[cell];
		}
		return 0f;
	}

	// Token: 0x06007747 RID: 30535 RVA: 0x000EE594 File Offset: 0x000EC794
	public OxygenBreather.IGasProvider GetGasProvider()
	{
		return this.gasProvider;
	}

	// Token: 0x06007748 RID: 30536 RVA: 0x000EE59C File Offset: 0x000EC79C
	public void SetGasProvider(OxygenBreather.IGasProvider gas_provider)
	{
		if (this.gasProvider != null)
		{
			this.gasProvider.OnClearOxygenBreather(this);
		}
		this.gasProvider = gas_provider;
		if (this.gasProvider != null)
		{
			this.gasProvider.OnSetOxygenBreather(this);
		}
	}

	// Token: 0x0400591D RID: 22813
	public static CellOffset[] DEFAULT_BREATHABLE_OFFSETS = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(1, 1),
		new CellOffset(-1, 1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x0400591E RID: 22814
	public float O2toCO2conversion = 0.5f;

	// Token: 0x0400591F RID: 22815
	public float lowOxygenThreshold;

	// Token: 0x04005920 RID: 22816
	public float noOxygenThreshold;

	// Token: 0x04005921 RID: 22817
	public Vector2 mouthOffset;

	// Token: 0x04005922 RID: 22818
	[Serialize]
	public float accumulatedCO2;

	// Token: 0x04005923 RID: 22819
	[SerializeField]
	public float minCO2ToEmit = 0.3f;

	// Token: 0x04005924 RID: 22820
	private bool hasAir = true;

	// Token: 0x04005925 RID: 22821
	private Timer hasAirTimer = new Timer();

	// Token: 0x04005926 RID: 22822
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04005927 RID: 22823
	[MyCmpGet]
	private Facing facing;

	// Token: 0x04005928 RID: 22824
	private HandleVector<int>.Handle o2Accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04005929 RID: 22825
	private HandleVector<int>.Handle co2Accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x0400592A RID: 22826
	private AmountInstance temperature;

	// Token: 0x0400592B RID: 22827
	private AttributeInstance airConsumptionRate;

	// Token: 0x0400592C RID: 22828
	public CellOffset[] breathableCells;

	// Token: 0x0400592D RID: 22829
	public Action<Sim.MassConsumedCallback> onSimConsume;

	// Token: 0x0400592E RID: 22830
	private OxygenBreather.IGasProvider gasProvider;

	// Token: 0x0400592F RID: 22831
	private static readonly EventSystem.IntraObjectHandler<OxygenBreather> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<OxygenBreather>(GameTags.Dead, delegate(OxygenBreather component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x0200168E RID: 5774
	public interface IGasProvider
	{
		// Token: 0x0600774B RID: 30539
		void OnSetOxygenBreather(OxygenBreather oxygen_breather);

		// Token: 0x0600774C RID: 30540
		void OnClearOxygenBreather(OxygenBreather oxygen_breather);

		// Token: 0x0600774D RID: 30541
		bool ConsumeGas(OxygenBreather oxygen_breather, float amount);

		// Token: 0x0600774E RID: 30542
		bool ShouldEmitCO2();

		// Token: 0x0600774F RID: 30543
		bool ShouldStoreCO2();

		// Token: 0x06007750 RID: 30544
		bool IsLowOxygen();
	}
}
