using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001A3A RID: 6714
[SerializationConfig(MemberSerialization.OptIn)]
public class WaterCooler : StateMachineComponent<WaterCooler.StatesInstance>, IApproachable, IGameObjectEffectDescriptor, FewOptionSideScreen.IFewOptionSideScreen
{
	// Token: 0x1700092A RID: 2346
	// (get) Token: 0x06008BF3 RID: 35827 RVA: 0x000FB854 File Offset: 0x000F9A54
	// (set) Token: 0x06008BF4 RID: 35828 RVA: 0x00361540 File Offset: 0x0035F740
	public Tag ChosenBeverage
	{
		get
		{
			return this.chosenBeverage;
		}
		set
		{
			if (this.chosenBeverage != value)
			{
				this.chosenBeverage = value;
				base.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
				this.storage.DropAll(false, false, default(Vector3), true, null);
			}
		}
	}

	// Token: 0x06008BF5 RID: 35829 RVA: 0x0036158C File Offset: 0x0035F78C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new SocialGatheringPointWorkable[this.socializeOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.socializeOffsets[i]), Grid.SceneLayer.Move);
			SocialGatheringPointWorkable socialGatheringPointWorkable = ChoreHelpers.CreateLocator("WaterCoolerWorkable", pos).AddOrGet<SocialGatheringPointWorkable>();
			socialGatheringPointWorkable.specificEffect = "Socialized";
			socialGatheringPointWorkable.SetWorkTime(this.workTime);
			this.workables[i] = socialGatheringPointWorkable;
		}
		this.chores = new Chore[this.socializeOffsets.Length];
		Extents extents = new Extents(Grid.PosToCell(this), this.socializeOffsets);
		this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WaterCooler", this, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
		base.Subscribe<WaterCooler>(-1697596308, WaterCooler.OnStorageChangeDelegate);
		base.smi.StartSM();
	}

	// Token: 0x06008BF6 RID: 35830 RVA: 0x003616CC File Offset: 0x0035F8CC
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
		this.CancelDrinkChores();
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06008BF7 RID: 35831 RVA: 0x00361730 File Offset: 0x0035F930
	public void UpdateDrinkChores(bool force = true)
	{
		if (!force && !this.choresDirty)
		{
			return;
		}
		float num = this.storage.GetMassAvailable(this.ChosenBeverage);
		int num2 = 0;
		for (int i = 0; i < this.socializeOffsets.Length; i++)
		{
			CellOffset offset = this.socializeOffsets[i];
			Chore chore = this.chores[i];
			if (num2 < this.choreCount && this.IsOffsetValid(offset) && num >= 1f)
			{
				num2++;
				num -= 1f;
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = new WaterCoolerChore(this, this.workables[i], null, null, new Action<Chore>(this.OnChoreEnd));
				}
			}
			else if (chore != null)
			{
				chore.Cancel("invalid");
				this.chores[i] = null;
			}
		}
		this.choresDirty = false;
	}

	// Token: 0x06008BF8 RID: 35832 RVA: 0x00361810 File Offset: 0x0035FA10
	public void CancelDrinkChores()
	{
		for (int i = 0; i < this.socializeOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (chore != null)
			{
				chore.Cancel("cancelled");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x06008BF9 RID: 35833 RVA: 0x00361850 File Offset: 0x0035FA50
	private bool IsOffsetValid(CellOffset offset)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(this), offset);
		int anchor_cell = Grid.CellBelow(cell);
		return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, false);
	}

	// Token: 0x06008BFA RID: 35834 RVA: 0x000FB85C File Offset: 0x000F9A5C
	private void OnChoreEnd(Chore chore)
	{
		this.choresDirty = true;
	}

	// Token: 0x06008BFB RID: 35835 RVA: 0x000FB85C File Offset: 0x000F9A5C
	private void OnCellChanged(object data)
	{
		this.choresDirty = true;
	}

	// Token: 0x06008BFC RID: 35836 RVA: 0x000FB85C File Offset: 0x000F9A5C
	private void OnStorageChange(object data)
	{
		this.choresDirty = true;
	}

	// Token: 0x06008BFD RID: 35837 RVA: 0x000FB865 File Offset: 0x000F9A65
	public CellOffset[] GetOffsets()
	{
		return this.drinkOffsets;
	}

	// Token: 0x06008BFE RID: 35838 RVA: 0x000BCAC8 File Offset: 0x000BACC8
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06008BFF RID: 35839 RVA: 0x002B73A0 File Offset: 0x002B55A0
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06008C00 RID: 35840 RVA: 0x00361878 File Offset: 0x0035FA78
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Socialized", true);
		foreach (global::Tuple<Tag, string> tuple in WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS)
		{
			this.AddRequirementDesc(list, tuple.first, 1f);
		}
		return list;
	}

	// Token: 0x06008C01 RID: 35841 RVA: 0x003618F8 File Offset: 0x0035FAF8
	public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
	{
		Effect.CreateTooltip(Db.Get().effects.Get("DuplicantGotMilk"), true, "\n    • ", true);
		FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string text = Strings.Get("STRINGS.BUILDINGS.PREFABS.WATERCOOLER.OPTION_TOOLTIPS." + WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first.ToString().ToUpper());
			if (!WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].second.IsNullOrWhiteSpace())
			{
				text = text + "\n\n" + Effect.CreateTooltip(Db.Get().effects.Get(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].second), false, "\n    • ", true);
			}
			array[i] = new FewOptionSideScreen.IFewOptionSideScreen.Option(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first, ElementLoader.GetElement(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first).name, Def.GetUISprite(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first, "ui", false), text);
		}
		return array;
	}

	// Token: 0x06008C02 RID: 35842 RVA: 0x000FB86D File Offset: 0x000F9A6D
	public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
	{
		this.ChosenBeverage = option.tag;
	}

	// Token: 0x06008C03 RID: 35843 RVA: 0x000FB87B File Offset: 0x000F9A7B
	public Tag GetSelectedOption()
	{
		return this.ChosenBeverage;
	}

	// Token: 0x04006961 RID: 26977
	public const float DRINK_MASS = 1f;

	// Token: 0x04006962 RID: 26978
	public const string SPECIFIC_EFFECT = "Socialized";

	// Token: 0x04006963 RID: 26979
	public CellOffset[] socializeOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(2, 0),
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04006964 RID: 26980
	public int choreCount = 2;

	// Token: 0x04006965 RID: 26981
	public float workTime = 5f;

	// Token: 0x04006966 RID: 26982
	private CellOffset[] drinkOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04006967 RID: 26983
	public static Action<GameObject, GameObject> OnDuplicantDrank;

	// Token: 0x04006968 RID: 26984
	private Chore[] chores;

	// Token: 0x04006969 RID: 26985
	private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x0400696A RID: 26986
	private SocialGatheringPointWorkable[] workables;

	// Token: 0x0400696B RID: 26987
	[MyCmpGet]
	private Storage storage;

	// Token: 0x0400696C RID: 26988
	public bool choresDirty;

	// Token: 0x0400696D RID: 26989
	[Serialize]
	private Tag chosenBeverage = GameTags.Water;

	// Token: 0x0400696E RID: 26990
	private static readonly EventSystem.IntraObjectHandler<WaterCooler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<WaterCooler>(delegate(WaterCooler component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001A3B RID: 6715
	public class States : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler>
	{
		// Token: 0x06008C06 RID: 35846 RVA: 0x00361AA8 File Offset: 0x0035FCA8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery, false).PlayAnim("off");
			this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.dispensing, (WaterCooler.StatesInstance smi) => smi.HasMinimumMass(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.dispensing, (WaterCooler.StatesInstance smi) => smi.HasMinimumMass()).PlayAnim("off");
			this.dispensing.Enter("StartMeter", delegate(WaterCooler.StatesInstance smi)
			{
				smi.StartMeter();
			}).Enter("Set Active", delegate(WaterCooler.StatesInstance smi)
			{
				smi.SetOperationalActiveState(true);
			}).Enter("UpdateDrinkChores.force", delegate(WaterCooler.StatesInstance smi)
			{
				smi.master.UpdateDrinkChores(true);
			}).Update("UpdateDrinkChores", delegate(WaterCooler.StatesInstance smi, float dt)
			{
				smi.master.UpdateDrinkChores(true);
			}, UpdateRate.SIM_200ms, false).Exit("CancelDrinkChores", delegate(WaterCooler.StatesInstance smi)
			{
				smi.master.CancelDrinkChores();
			}).Exit("Set Inactive", delegate(WaterCooler.StatesInstance smi)
			{
				smi.SetOperationalActiveState(false);
			}).TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (WaterCooler.StatesInstance smi) => !smi.HasMinimumMass()).PlayAnim("working");
		}

		// Token: 0x0400696F RID: 26991
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State unoperational;

		// Token: 0x04006970 RID: 26992
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State waitingfordelivery;

		// Token: 0x04006971 RID: 26993
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State dispensing;
	}

	// Token: 0x02001A3D RID: 6717
	public class StatesInstance : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.GameInstance
	{
		// Token: 0x06008C13 RID: 35859 RVA: 0x00361CA4 File Offset: 0x0035FEA4
		public StatesInstance(WaterCooler smi) : base(smi)
		{
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_bottle", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_bottle"
			});
			this.storage = base.master.GetComponent<Storage>();
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		}

		// Token: 0x06008C14 RID: 35860 RVA: 0x00361D0C File Offset: 0x0035FF0C
		public void Drink(GameObject druplicant, bool triggerOnDrinkCallback = true)
		{
			if (!this.HasMinimumMass())
			{
				return;
			}
			Tag tag = this.storage.items[0].PrefabID();
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float num2;
			this.storage.ConsumeAndGetDisease(tag, 1f, out num, out diseaseInfo, out num2);
			GermExposureMonitor.Instance smi = druplicant.GetSMI<GermExposureMonitor.Instance>();
			if (smi != null)
			{
				smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, tag, Sickness.InfectionVector.Digestion);
			}
			Effects component = druplicant.GetComponent<Effects>();
			if (tag == SimHashes.Milk.CreateTag())
			{
				component.Add("DuplicantGotMilk", true);
			}
			if (triggerOnDrinkCallback)
			{
				Action<GameObject, GameObject> onDuplicantDrank = WaterCooler.OnDuplicantDrank;
				if (onDuplicantDrank == null)
				{
					return;
				}
				onDuplicantDrank(druplicant, base.gameObject);
			}
		}

		// Token: 0x06008C15 RID: 35861 RVA: 0x00361DB4 File Offset: 0x0035FFB4
		private void OnStorageChange(object data)
		{
			float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
			this.meter.SetPositionPercent(positionPercent);
		}

		// Token: 0x06008C16 RID: 35862 RVA: 0x000FB8FB File Offset: 0x000F9AFB
		public void SetOperationalActiveState(bool isActive)
		{
			this.operational.SetActive(isActive, false);
		}

		// Token: 0x06008C17 RID: 35863 RVA: 0x00361DEC File Offset: 0x0035FFEC
		public void StartMeter()
		{
			PrimaryElement primaryElement = this.storage.FindFirstWithMass(base.smi.master.ChosenBeverage, 0f);
			if (primaryElement == null)
			{
				return;
			}
			this.meter.SetSymbolTint(new KAnimHashedString("meter_water"), primaryElement.Element.substance.colour);
			this.OnStorageChange(null);
		}

		// Token: 0x06008C18 RID: 35864 RVA: 0x000FB90A File Offset: 0x000F9B0A
		public bool HasMinimumMass()
		{
			return this.storage.GetMassAvailable(ElementLoader.GetElement(base.smi.master.ChosenBeverage).id) >= 1f;
		}

		// Token: 0x0400697C RID: 27004
		[MyCmpGet]
		private Operational operational;

		// Token: 0x0400697D RID: 27005
		private Storage storage;

		// Token: 0x0400697E RID: 27006
		private MeterController meter;
	}
}
