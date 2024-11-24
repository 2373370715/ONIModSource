using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E95 RID: 3733
public class MassageTable : RelaxationPoint, IGameObjectEffectDescriptor, IActivationRangeTarget
{
	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x06004B1C RID: 19228 RVA: 0x000D08C2 File Offset: 0x000CEAC2
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.MASSAGETABLE.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x06004B1D RID: 19229 RVA: 0x000D08CE File Offset: 0x000CEACE
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.MASSAGETABLE.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x06004B1E RID: 19230 RVA: 0x000D08DA File Offset: 0x000CEADA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<MassageTable>(-905833192, MassageTable.OnCopySettingsDelegate);
	}

	// Token: 0x06004B1F RID: 19231 RVA: 0x0025D5BC File Offset: 0x0025B7BC
	private void OnCopySettings(object data)
	{
		MassageTable component = ((GameObject)data).GetComponent<MassageTable>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x06004B20 RID: 19232 RVA: 0x0025D5F8 File Offset: 0x0025B7F8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < MassageTable.EffectsRemoved.Length; i++)
		{
			string effect_id = MassageTable.EffectsRemoved[i];
			component.Remove(effect_id);
		}
	}

	// Token: 0x06004B21 RID: 19233 RVA: 0x0025D634 File Offset: 0x0025B834
	public new List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		if (MassageTable.EffectsRemoved.Length != 0)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE, Descriptor.DescriptorType.Effect);
			list.Add(item2);
			for (int i = 0; i < MassageTable.EffectsRemoved.Length; i++)
			{
				string text = MassageTable.EffectsRemoved[i];
				string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string arg2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".CAUSE");
				Descriptor item3 = default(Descriptor);
				item3.IncreaseIndent();
				item3.SetupDescriptor("• " + string.Format(UI.BUILDINGEFFECTS.REMOVEDEFFECT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, arg2), Descriptor.DescriptorType.Effect);
				list.Add(item3);
			}
		}
		return list;
	}

	// Token: 0x06004B22 RID: 19234 RVA: 0x0025D794 File Offset: 0x0025B994
	protected override WorkChore<RelaxationPoint> CreateWorkChore()
	{
		WorkChore<RelaxationPoint> workChore = new WorkChore<RelaxationPoint>(Db.Get().ChoreTypes.StressHeal, this, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		workChore.AddPrecondition(MassageTable.IsStressAboveActivationRange, this);
		return workChore;
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x06004B23 RID: 19235 RVA: 0x000D08F3 File Offset: 0x000CEAF3
	// (set) Token: 0x06004B24 RID: 19236 RVA: 0x000D08FB File Offset: 0x000CEAFB
	public float ActivateValue
	{
		get
		{
			return this.activateValue;
		}
		set
		{
			this.activateValue = value;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x06004B25 RID: 19237 RVA: 0x000D0904 File Offset: 0x000CEB04
	// (set) Token: 0x06004B26 RID: 19238 RVA: 0x000D090C File Offset: 0x000CEB0C
	public float DeactivateValue
	{
		get
		{
			return this.stopStressingValue;
		}
		set
		{
			this.stopStressingValue = value;
		}
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x06004B27 RID: 19239 RVA: 0x000A65EC File Offset: 0x000A47EC
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x06004B28 RID: 19240 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06004B29 RID: 19241 RVA: 0x000C8A64 File Offset: 0x000C6C64
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06004B2A RID: 19242 RVA: 0x000D0915 File Offset: 0x000CEB15
	public string ActivationRangeTitleText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.NAME;
		}
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06004B2B RID: 19243 RVA: 0x000D0921 File Offset: 0x000CEB21
	public string ActivateSliderLabelText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.ACTIVATE;
		}
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x06004B2C RID: 19244 RVA: 0x000D092D File Offset: 0x000CEB2D
	public string DeactivateSliderLabelText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.DEACTIVATE;
		}
	}

	// Token: 0x04003411 RID: 13329
	[Serialize]
	private float activateValue = 50f;

	// Token: 0x04003412 RID: 13330
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoreBack"
	};

	// Token: 0x04003413 RID: 13331
	private static readonly EventSystem.IntraObjectHandler<MassageTable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<MassageTable>(delegate(MassageTable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003414 RID: 13332
	private static readonly Chore.Precondition IsStressAboveActivationRange = new Chore.Precondition
	{
		id = "IsStressAboveActivationRange",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_STRESS_ABOVE_ACTIVATION_RANGE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			IActivationRangeTarget activationRangeTarget = (IActivationRangeTarget)data;
			return Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject).value >= activationRangeTarget.ActivateValue;
		}
	};
}
