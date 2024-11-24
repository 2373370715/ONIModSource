using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F53 RID: 3923
[AddComponentMenu("KMonoBehaviour/scripts/Refrigerator")]
public class Refrigerator : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x06004F56 RID: 20310 RVA: 0x000D3B5B File Offset: 0x000D1D5B
	protected override void OnPrefabInit()
	{
		this.filteredStorage = new FilteredStorage(this, new Tag[]
		{
			GameTags.Compostable
		}, this, true, Db.Get().ChoreTypes.FoodFetch);
	}

	// Token: 0x06004F57 RID: 20311 RVA: 0x0026B080 File Offset: 0x00269280
	protected override void OnSpawn()
	{
		base.GetComponent<KAnimControllerBase>().Play("off", KAnim.PlayMode.Once, 1f, 0f);
		FoodStorage component = base.GetComponent<FoodStorage>();
		component.FilteredStorage = this.filteredStorage;
		component.SpicedFoodOnly = component.SpicedFoodOnly;
		this.filteredStorage.FilterChanged();
		this.UpdateLogicCircuit();
		base.Subscribe<Refrigerator>(-905833192, Refrigerator.OnCopySettingsDelegate);
		base.Subscribe<Refrigerator>(-1697596308, Refrigerator.UpdateLogicCircuitCBDelegate);
		base.Subscribe<Refrigerator>(-592767678, Refrigerator.UpdateLogicCircuitCBDelegate);
	}

	// Token: 0x06004F58 RID: 20312 RVA: 0x000D3B8C File Offset: 0x000D1D8C
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x06004F59 RID: 20313 RVA: 0x000D3B99 File Offset: 0x000D1D99
	public bool IsActive()
	{
		return this.operational.IsActive;
	}

	// Token: 0x06004F5A RID: 20314 RVA: 0x0026B110 File Offset: 0x00269310
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		Refrigerator component = gameObject.GetComponent<Refrigerator>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x17000471 RID: 1137
	// (get) Token: 0x06004F5B RID: 20315 RVA: 0x000D3BA6 File Offset: 0x000D1DA6
	// (set) Token: 0x06004F5C RID: 20316 RVA: 0x000D3BBE File Offset: 0x000D1DBE
	public float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, this.storage.capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
			this.UpdateLogicCircuit();
		}
	}

	// Token: 0x17000472 RID: 1138
	// (get) Token: 0x06004F5D RID: 20317 RVA: 0x000D3BD8 File Offset: 0x000D1DD8
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000473 RID: 1139
	// (get) Token: 0x06004F5E RID: 20318 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000474 RID: 1140
	// (get) Token: 0x06004F5F RID: 20319 RVA: 0x000D3BE5 File Offset: 0x000D1DE5
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000475 RID: 1141
	// (get) Token: 0x06004F60 RID: 20320 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000476 RID: 1142
	// (get) Token: 0x06004F61 RID: 20321 RVA: 0x000C8D02 File Offset: 0x000C6F02
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x06004F62 RID: 20322 RVA: 0x000D3BF2 File Offset: 0x000D1DF2
	private void UpdateLogicCircuitCB(object data)
	{
		this.UpdateLogicCircuit();
	}

	// Token: 0x06004F63 RID: 20323 RVA: 0x0026B14C File Offset: 0x0026934C
	private void UpdateLogicCircuit()
	{
		bool flag = this.filteredStorage.IsFull();
		bool isOperational = this.operational.IsOperational;
		bool flag2 = flag && isOperational;
		this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, flag2 ? 1 : 0);
		this.filteredStorage.SetLogicMeter(flag2);
	}

	// Token: 0x04003768 RID: 14184
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003769 RID: 14185
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400376A RID: 14186
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x0400376B RID: 14187
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x0400376C RID: 14188
	private FilteredStorage filteredStorage;

	// Token: 0x0400376D RID: 14189
	private static readonly EventSystem.IntraObjectHandler<Refrigerator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Refrigerator>(delegate(Refrigerator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400376E RID: 14190
	private static readonly EventSystem.IntraObjectHandler<Refrigerator> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<Refrigerator>(delegate(Refrigerator component, object data)
	{
		component.UpdateLogicCircuitCB(data);
	});
}
