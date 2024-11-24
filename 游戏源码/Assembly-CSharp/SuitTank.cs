using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020019D2 RID: 6610
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SuitTank")]
public class SuitTank : KMonoBehaviour, IGameObjectEffectDescriptor, OxygenBreather.IGasProvider
{
	// Token: 0x060089A8 RID: 35240 RVA: 0x000FA295 File Offset: 0x000F8495
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SuitTank>(-1617557748, SuitTank.OnEquippedDelegate);
		base.Subscribe<SuitTank>(-170173755, SuitTank.OnUnequippedDelegate);
	}

	// Token: 0x060089A9 RID: 35241 RVA: 0x003582AC File Offset: 0x003564AC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.amount != 0f)
		{
			this.storage.AddGasChunk(SimHashes.Oxygen, this.amount, base.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0, false, true);
			this.amount = 0f;
		}
	}

	// Token: 0x060089AA RID: 35242 RVA: 0x000FA2BF File Offset: 0x000F84BF
	public float GetTankAmount()
	{
		if (this.storage == null)
		{
			this.storage = base.GetComponent<Storage>();
		}
		return this.storage.GetMassAvailable(this.elementTag);
	}

	// Token: 0x060089AB RID: 35243 RVA: 0x000FA2EC File Offset: 0x000F84EC
	public float PercentFull()
	{
		return this.GetTankAmount() / this.capacity;
	}

	// Token: 0x060089AC RID: 35244 RVA: 0x000FA2FB File Offset: 0x000F84FB
	public bool IsEmpty()
	{
		return this.GetTankAmount() <= 0f;
	}

	// Token: 0x060089AD RID: 35245 RVA: 0x000FA30D File Offset: 0x000F850D
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x060089AE RID: 35246 RVA: 0x000FA31F File Offset: 0x000F851F
	public bool NeedsRecharging()
	{
		return this.PercentFull() < 0.25f;
	}

	// Token: 0x060089AF RID: 35247 RVA: 0x00358304 File Offset: 0x00356504
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.elementTag == GameTags.Breathable)
		{
			string text = this.underwaterSupport ? string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK_UNDERWATER, GameUtil.GetFormattedMass(this.GetTankAmount(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")) : string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK, GameUtil.GetFormattedMass(this.GetTankAmount(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x060089B0 RID: 35248 RVA: 0x00358388 File Offset: 0x00356588
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		GameObject targetGameObject = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		OxygenBreather component = targetGameObject.GetComponent<OxygenBreather>();
		if (component != null)
		{
			component.SetGasProvider(this);
		}
		targetGameObject.AddTag(GameTags.HasSuitTank);
	}

	// Token: 0x060089B1 RID: 35249 RVA: 0x003583EC File Offset: 0x003565EC
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), false);
			GameObject targetGameObject = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			OxygenBreather component = targetGameObject.GetComponent<OxygenBreather>();
			if (component != null)
			{
				component.SetGasProvider(new GasBreatherFromWorldProvider());
			}
			targetGameObject.RemoveTag(GameTags.HasSuitTank);
		}
	}

	// Token: 0x060089B2 RID: 35250 RVA: 0x000FA32E File Offset: 0x000F852E
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suitSuffocationMonitor = new SuitSuffocationMonitor.Instance(oxygen_breather, this);
		this.suitSuffocationMonitor.StartSM();
	}

	// Token: 0x060089B3 RID: 35251 RVA: 0x000FA348 File Offset: 0x000F8548
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.suitSuffocationMonitor.StopSM("Removed suit tank");
		this.suitSuffocationMonitor = null;
	}

	// Token: 0x060089B4 RID: 35252 RVA: 0x0035845C File Offset: 0x0035665C
	public bool ConsumeGas(OxygenBreather oxygen_breather, float gas_consumed)
	{
		if (this.IsEmpty())
		{
			return false;
		}
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		this.storage.ConsumeAndGetDisease(this.elementTag, gas_consumed, out num, out diseaseInfo, out num2);
		Game.Instance.accumulators.Accumulate(oxygen_breather.O2Accumulator, num);
		ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, -num, oxygen_breather.GetProperName(), null);
		base.Trigger(608245985, base.gameObject);
		return true;
	}

	// Token: 0x060089B5 RID: 35253 RVA: 0x000FA361 File Offset: 0x000F8561
	public bool ShouldEmitCO2()
	{
		return !base.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
	}

	// Token: 0x060089B6 RID: 35254 RVA: 0x000FA376 File Offset: 0x000F8576
	public bool ShouldStoreCO2()
	{
		return base.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
	}

	// Token: 0x060089B7 RID: 35255 RVA: 0x000FA388 File Offset: 0x000F8588
	public bool IsLowOxygen()
	{
		return this.IsEmpty();
	}

	// Token: 0x060089B8 RID: 35256 RVA: 0x003584C8 File Offset: 0x003566C8
	[ContextMenu("SetToRefillAmount")]
	public void SetToRefillAmount()
	{
		float tankAmount = this.GetTankAmount();
		float num = 0.25f * this.capacity;
		if (tankAmount > num)
		{
			this.storage.ConsumeIgnoringDisease(this.elementTag, tankAmount - num);
		}
	}

	// Token: 0x060089B9 RID: 35257 RVA: 0x000FA390 File Offset: 0x000F8590
	[ContextMenu("Empty")]
	public void Empty()
	{
		this.storage.ConsumeIgnoringDisease(this.elementTag, this.GetTankAmount());
	}

	// Token: 0x060089BA RID: 35258 RVA: 0x000FA3A9 File Offset: 0x000F85A9
	[ContextMenu("Fill Tank")]
	public void FillTank()
	{
		this.Empty();
		this.storage.AddGasChunk(SimHashes.Oxygen, this.capacity, 15f, 0, 0, false, false);
	}

	// Token: 0x0400679A RID: 26522
	[Serialize]
	public string element;

	// Token: 0x0400679B RID: 26523
	[Serialize]
	public float amount;

	// Token: 0x0400679C RID: 26524
	public Tag elementTag;

	// Token: 0x0400679D RID: 26525
	[MyCmpReq]
	public Storage storage;

	// Token: 0x0400679E RID: 26526
	public float capacity;

	// Token: 0x0400679F RID: 26527
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x040067A0 RID: 26528
	public bool underwaterSupport;

	// Token: 0x040067A1 RID: 26529
	private SuitSuffocationMonitor.Instance suitSuffocationMonitor;

	// Token: 0x040067A2 RID: 26530
	private static readonly EventSystem.IntraObjectHandler<SuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>(delegate(SuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x040067A3 RID: 26531
	private static readonly EventSystem.IntraObjectHandler<SuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>(delegate(SuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
