using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001490 RID: 5264
[SerializationConfig(MemberSerialization.OptIn)]
public class LeadSuitTank : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06006D22 RID: 27938 RVA: 0x000E7950 File Offset: 0x000E5B50
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LeadSuitTank>(-1617557748, LeadSuitTank.OnEquippedDelegate);
		base.Subscribe<LeadSuitTank>(-170173755, LeadSuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06006D23 RID: 27939 RVA: 0x000E797A File Offset: 0x000E5B7A
	public float PercentFull()
	{
		return this.batteryCharge;
	}

	// Token: 0x06006D24 RID: 27940 RVA: 0x000E7982 File Offset: 0x000E5B82
	public bool IsEmpty()
	{
		return this.batteryCharge <= 0f;
	}

	// Token: 0x06006D25 RID: 27941 RVA: 0x000E7994 File Offset: 0x000E5B94
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x06006D26 RID: 27942 RVA: 0x000E79A6 File Offset: 0x000E5BA6
	public bool NeedsRecharging()
	{
		return this.PercentFull() <= 0.25f;
	}

	// Token: 0x06006D27 RID: 27943 RVA: 0x002EABD0 File Offset: 0x002E8DD0
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.LEADSUIT_BATTERY, GameUtil.GetFormattedPercent(this.PercentFull() * 100f, GameUtil.TimeSlice.None));
		list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x06006D28 RID: 27944 RVA: 0x002EAC14 File Offset: 0x002E8E14
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitBatteryDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		this.leadSuitMonitor = new LeadSuitMonitor.Instance(this, equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject());
		this.leadSuitMonitor.StartSM();
		if (this.NeedsRecharging())
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().AddTag(GameTags.SuitBatteryLow);
		}
	}

	// Token: 0x06006D29 RID: 27945 RVA: 0x002EAC8C File Offset: 0x002E8E8C
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.SuitBatteryLow);
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.SuitBatteryOut);
			NameDisplayScreen.Instance.SetSuitBatteryDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), null, false);
		}
		if (this.leadSuitMonitor != null)
		{
			this.leadSuitMonitor.StopSM("Removed leadsuit tank");
			this.leadSuitMonitor = null;
		}
	}

	// Token: 0x040051DD RID: 20957
	[Serialize]
	public float batteryCharge = 1f;

	// Token: 0x040051DE RID: 20958
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x040051DF RID: 20959
	public float batteryDuration = 200f;

	// Token: 0x040051E0 RID: 20960
	public float coolingOperationalTemperature = 333.15f;

	// Token: 0x040051E1 RID: 20961
	public Tag coolantTag;

	// Token: 0x040051E2 RID: 20962
	private LeadSuitMonitor.Instance leadSuitMonitor;

	// Token: 0x040051E3 RID: 20963
	private static readonly EventSystem.IntraObjectHandler<LeadSuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<LeadSuitTank>(delegate(LeadSuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x040051E4 RID: 20964
	private static readonly EventSystem.IntraObjectHandler<LeadSuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<LeadSuitTank>(delegate(LeadSuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
