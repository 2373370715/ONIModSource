using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F42 RID: 3906
[AddComponentMenu("KMonoBehaviour/scripts/RationBox")]
public class RationBox : KMonoBehaviour, IUserControlledCapacity, IRender1000ms, IRottable
{
	// Token: 0x06004EE7 RID: 20199 RVA: 0x00269478 File Offset: 0x00267678
	protected override void OnPrefabInit()
	{
		this.filteredStorage = new FilteredStorage(this, new Tag[]
		{
			GameTags.Compostable
		}, this, false, Db.Get().ChoreTypes.FoodFetch);
		base.Subscribe<RationBox>(-592767678, RationBox.OnOperationalChangedDelegate);
		base.Subscribe<RationBox>(-905833192, RationBox.OnCopySettingsDelegate);
		DiscoveredResources.Instance.Discover("FieldRation".ToTag(), GameTags.Edible);
	}

	// Token: 0x06004EE8 RID: 20200 RVA: 0x000D36E6 File Offset: 0x000D18E6
	protected override void OnSpawn()
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(component.IsOperational, false);
		this.filteredStorage.FilterChanged();
	}

	// Token: 0x06004EE9 RID: 20201 RVA: 0x000D3705 File Offset: 0x000D1905
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x06004EEA RID: 20202 RVA: 0x000D3712 File Offset: 0x000D1912
	private void OnOperationalChanged(object data)
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(component.IsOperational, false);
	}

	// Token: 0x06004EEB RID: 20203 RVA: 0x002694F0 File Offset: 0x002676F0
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		RationBox component = gameObject.GetComponent<RationBox>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06004EEC RID: 20204 RVA: 0x000D3726 File Offset: 0x000D1926
	public void Render1000ms(float dt)
	{
		Rottable.SetStatusItems(this);
	}

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06004EED RID: 20205 RVA: 0x000D372E File Offset: 0x000D192E
	// (set) Token: 0x06004EEE RID: 20206 RVA: 0x000D3746 File Offset: 0x000D1946
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
		}
	}

	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x06004EEF RID: 20207 RVA: 0x000D375A File Offset: 0x000D195A
	public float AmountStored
	{
		get
		{
			return this.storage.MassStored();
		}
	}

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06004EF0 RID: 20208 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x000D3767 File Offset: 0x000D1967
	public float MaxCapacity
	{
		get
		{
			return this.storage.capacityKg;
		}
	}

	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x06004EF2 RID: 20210 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x06004EF3 RID: 20211 RVA: 0x000C8D02 File Offset: 0x000C6F02
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x06004EF4 RID: 20212 RVA: 0x000D3774 File Offset: 0x000D1974
	public float RotTemperature
	{
		get
		{
			return 277.15f;
		}
	}

	// Token: 0x1700046C RID: 1132
	// (get) Token: 0x06004EF5 RID: 20213 RVA: 0x000D377B File Offset: 0x000D197B
	public float PreserveTemperature
	{
		get
		{
			return 255.15f;
		}
	}

	// Token: 0x06004EF8 RID: 20216 RVA: 0x000C9F3A File Offset: 0x000C813A
	GameObject IRottable.get_gameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0400370B RID: 14091
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400370C RID: 14092
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x0400370D RID: 14093
	private FilteredStorage filteredStorage;

	// Token: 0x0400370E RID: 14094
	private static readonly EventSystem.IntraObjectHandler<RationBox> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<RationBox>(delegate(RationBox component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x0400370F RID: 14095
	private static readonly EventSystem.IntraObjectHandler<RationBox> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<RationBox>(delegate(RationBox component, object data)
	{
		component.OnCopySettings(data);
	});
}
