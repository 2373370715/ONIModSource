using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200146E RID: 5230
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/JetSuitTank")]
public class JetSuitTank : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06006C77 RID: 27767 RVA: 0x000E738B File Offset: 0x000E558B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.amount = 25f;
		base.Subscribe<JetSuitTank>(-1617557748, JetSuitTank.OnEquippedDelegate);
		base.Subscribe<JetSuitTank>(-170173755, JetSuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06006C78 RID: 27768 RVA: 0x000E73C0 File Offset: 0x000E55C0
	public float PercentFull()
	{
		return this.amount / 25f;
	}

	// Token: 0x06006C79 RID: 27769 RVA: 0x000E73CE File Offset: 0x000E55CE
	public bool IsEmpty()
	{
		return this.amount <= 0f;
	}

	// Token: 0x06006C7A RID: 27770 RVA: 0x000E73E0 File Offset: 0x000E55E0
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x06006C7B RID: 27771 RVA: 0x000E73F2 File Offset: 0x000E55F2
	public bool NeedsRecharging()
	{
		return this.PercentFull() < 0.25f;
	}

	// Token: 0x06006C7C RID: 27772 RVA: 0x002E8064 File Offset: 0x002E6264
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.JETSUIT_TANK, GameUtil.GetFormattedMass(this.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x06006C7D RID: 27773 RVA: 0x002E80A8 File Offset: 0x002E62A8
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		this.jetSuitMonitor = new JetSuitMonitor.Instance(this, equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject());
		this.jetSuitMonitor.StartSM();
		if (this.IsEmpty())
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().AddTag(GameTags.JetSuitOutOfFuel);
		}
	}

	// Token: 0x06006C7E RID: 27774 RVA: 0x002E8120 File Offset: 0x002E6320
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.JetSuitOutOfFuel);
			NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), null, false);
			Navigator component = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<Navigator>();
			if (component && component.CurrentNavType == NavType.Hover)
			{
				component.SetCurrentNavType(NavType.Floor);
			}
		}
		if (this.jetSuitMonitor != null)
		{
			this.jetSuitMonitor.StopSM("Removed jetsuit tank");
			this.jetSuitMonitor = null;
		}
	}

	// Token: 0x0400514F RID: 20815
	[MyCmpGet]
	private ElementEmitter elementConverter;

	// Token: 0x04005150 RID: 20816
	[Serialize]
	public float amount;

	// Token: 0x04005151 RID: 20817
	public const float FUEL_CAPACITY = 25f;

	// Token: 0x04005152 RID: 20818
	public const float FUEL_BURN_RATE = 0.1f;

	// Token: 0x04005153 RID: 20819
	public const float CO2_EMITTED_PER_FUEL_BURNED = 3f;

	// Token: 0x04005154 RID: 20820
	public const float EMIT_TEMPERATURE = 473.15f;

	// Token: 0x04005155 RID: 20821
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x04005156 RID: 20822
	private JetSuitMonitor.Instance jetSuitMonitor;

	// Token: 0x04005157 RID: 20823
	private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>(delegate(JetSuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04005158 RID: 20824
	private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>(delegate(JetSuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
