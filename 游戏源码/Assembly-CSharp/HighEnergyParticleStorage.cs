using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020013F2 RID: 5106
public class HighEnergyParticleStorage : KMonoBehaviour, IStorage
{
	// Token: 0x170006B3 RID: 1715
	// (get) Token: 0x060068D1 RID: 26833 RVA: 0x000E4B14 File Offset: 0x000E2D14
	public float Particles
	{
		get
		{
			return this.particles;
		}
	}

	// Token: 0x170006B4 RID: 1716
	// (get) Token: 0x060068D2 RID: 26834 RVA: 0x000E4B1C File Offset: 0x000E2D1C
	// (set) Token: 0x060068D3 RID: 26835 RVA: 0x000E4B24 File Offset: 0x000E2D24
	public bool allowUIItemRemoval { get; set; }

	// Token: 0x060068D4 RID: 26836 RVA: 0x002D84AC File Offset: 0x002D66AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.autoStore)
		{
			HighEnergyParticlePort component = base.gameObject.GetComponent<HighEnergyParticlePort>();
			component.onParticleCapture = (HighEnergyParticlePort.OnParticleCapture)Delegate.Combine(component.onParticleCapture, new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture));
			component.onParticleCaptureAllowed = (HighEnergyParticlePort.OnParticleCaptureAllowed)Delegate.Combine(component.onParticleCaptureAllowed, new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed));
		}
		this.SetupStorageStatusItems();
	}

	// Token: 0x060068D5 RID: 26837 RVA: 0x000E4B2D File Offset: 0x000E2D2D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateLogicPorts();
	}

	// Token: 0x060068D6 RID: 26838 RVA: 0x002D851C File Offset: 0x002D671C
	private void UpdateLogicPorts()
	{
		if (this._logicPorts != null)
		{
			bool value = this.IsFull();
			this._logicPorts.SendSignal(this.PORT_ID, Convert.ToInt32(value));
		}
	}

	// Token: 0x060068D7 RID: 26839 RVA: 0x000E4B3B File Offset: 0x000E2D3B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.autoStore)
		{
			HighEnergyParticlePort component = base.gameObject.GetComponent<HighEnergyParticlePort>();
			component.onParticleCapture = (HighEnergyParticlePort.OnParticleCapture)Delegate.Remove(component.onParticleCapture, new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture));
		}
	}

	// Token: 0x060068D8 RID: 26840 RVA: 0x002D855C File Offset: 0x002D675C
	private void OnParticleCapture(HighEnergyParticle particle)
	{
		float num = Mathf.Min(particle.payload, this.capacity - this.particles);
		this.Store(num);
		particle.payload -= num;
		if (particle.payload > 0f)
		{
			base.gameObject.GetComponent<HighEnergyParticlePort>().Uncapture(particle);
		}
	}

	// Token: 0x060068D9 RID: 26841 RVA: 0x000E4B77 File Offset: 0x000E2D77
	private bool OnParticleCaptureAllowed(HighEnergyParticle particle)
	{
		return this.particles < this.capacity && this.receiverOpen;
	}

	// Token: 0x060068DA RID: 26842 RVA: 0x002D85B8 File Offset: 0x002D67B8
	private void DeltaParticles(float delta)
	{
		this.particles += delta;
		if (this.particles <= 0f)
		{
			base.Trigger(155636535, base.transform.gameObject);
		}
		base.Trigger(-1837862626, base.transform.gameObject);
		this.UpdateLogicPorts();
	}

	// Token: 0x060068DB RID: 26843 RVA: 0x002D8614 File Offset: 0x002D6814
	public float Store(float amount)
	{
		float num = Mathf.Min(amount, this.RemainingCapacity());
		this.DeltaParticles(num);
		return num;
	}

	// Token: 0x060068DC RID: 26844 RVA: 0x000E4B8F File Offset: 0x000E2D8F
	public float ConsumeAndGet(float amount)
	{
		amount = Mathf.Min(this.Particles, amount);
		this.DeltaParticles(-amount);
		return amount;
	}

	// Token: 0x060068DD RID: 26845 RVA: 0x000E4BA8 File Offset: 0x000E2DA8
	[ContextMenu("Trigger Stored Event")]
	public void DEBUG_TriggerStorageEvent()
	{
		base.Trigger(-1837862626, base.transform.gameObject);
	}

	// Token: 0x060068DE RID: 26846 RVA: 0x000E4BC0 File Offset: 0x000E2DC0
	[ContextMenu("Trigger Zero Event")]
	public void DEBUG_TriggerZeroEvent()
	{
		this.ConsumeAndGet(this.particles + 1f);
	}

	// Token: 0x060068DF RID: 26847 RVA: 0x000E4BD5 File Offset: 0x000E2DD5
	public float ConsumeAll()
	{
		return this.ConsumeAndGet(this.particles);
	}

	// Token: 0x060068E0 RID: 26848 RVA: 0x000E4BE3 File Offset: 0x000E2DE3
	public bool HasRadiation()
	{
		return this.Particles > 0f;
	}

	// Token: 0x060068E1 RID: 26849 RVA: 0x000AD332 File Offset: 0x000AB532
	public GameObject Drop(GameObject go, bool do_disease_transfer = true)
	{
		return null;
	}

	// Token: 0x060068E2 RID: 26850 RVA: 0x000E4BF2 File Offset: 0x000E2DF2
	public List<GameObject> GetItems()
	{
		return new List<GameObject>
		{
			base.gameObject
		};
	}

	// Token: 0x060068E3 RID: 26851 RVA: 0x000E4C05 File Offset: 0x000E2E05
	public bool IsFull()
	{
		return this.RemainingCapacity() <= 0f;
	}

	// Token: 0x060068E4 RID: 26852 RVA: 0x000E4C17 File Offset: 0x000E2E17
	public bool IsEmpty()
	{
		return this.Particles == 0f;
	}

	// Token: 0x060068E5 RID: 26853 RVA: 0x000E4C26 File Offset: 0x000E2E26
	public float Capacity()
	{
		return this.capacity;
	}

	// Token: 0x060068E6 RID: 26854 RVA: 0x000E4C2E File Offset: 0x000E2E2E
	public float RemainingCapacity()
	{
		return Mathf.Max(this.capacity - this.Particles, 0f);
	}

	// Token: 0x060068E7 RID: 26855 RVA: 0x000E4C47 File Offset: 0x000E2E47
	public bool ShouldShowInUI()
	{
		return this.showInUI;
	}

	// Token: 0x060068E8 RID: 26856 RVA: 0x000E4C4F File Offset: 0x000E2E4F
	public float GetAmountAvailable(Tag tag)
	{
		if (tag != GameTags.HighEnergyParticle)
		{
			return 0f;
		}
		return this.Particles;
	}

	// Token: 0x060068E9 RID: 26857 RVA: 0x000E4C6A File Offset: 0x000E2E6A
	public void ConsumeIgnoringDisease(Tag tag, float amount)
	{
		DebugUtil.DevAssert(tag == GameTags.HighEnergyParticle, "Consuming non-particle tag as amount", null);
		this.ConsumeAndGet(amount);
	}

	// Token: 0x060068EA RID: 26858 RVA: 0x002D8638 File Offset: 0x002D6838
	private void SetupStorageStatusItems()
	{
		if (HighEnergyParticleStorage.capacityStatusItem == null)
		{
			HighEnergyParticleStorage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			HighEnergyParticleStorage.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				HighEnergyParticleStorage highEnergyParticleStorage = (HighEnergyParticleStorage)data;
				string newValue = Util.FormatWholeNumber(highEnergyParticleStorage.particles);
				string newValue2 = Util.FormatWholeNumber(highEnergyParticleStorage.capacity);
				str = str.Replace("{Stored}", newValue);
				str = str.Replace("{Capacity}", newValue2);
				str = str.Replace("{Units}", UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES);
				return str;
			};
		}
		if (this.showCapacityStatusItem)
		{
			if (this.showCapacityAsMainStatus)
			{
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, HighEnergyParticleStorage.capacityStatusItem, this);
				return;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, HighEnergyParticleStorage.capacityStatusItem, this);
		}
	}

	// Token: 0x04004F13 RID: 20243
	[Serialize]
	[SerializeField]
	private float particles;

	// Token: 0x04004F14 RID: 20244
	public float capacity = float.MaxValue;

	// Token: 0x04004F15 RID: 20245
	public bool showInUI = true;

	// Token: 0x04004F16 RID: 20246
	public bool showCapacityStatusItem;

	// Token: 0x04004F17 RID: 20247
	public bool showCapacityAsMainStatus;

	// Token: 0x04004F19 RID: 20249
	public bool autoStore;

	// Token: 0x04004F1A RID: 20250
	[Serialize]
	public bool receiverOpen = true;

	// Token: 0x04004F1B RID: 20251
	[MyCmpGet]
	private LogicPorts _logicPorts;

	// Token: 0x04004F1C RID: 20252
	public string PORT_ID = "";

	// Token: 0x04004F1D RID: 20253
	private static StatusItem capacityStatusItem;
}
