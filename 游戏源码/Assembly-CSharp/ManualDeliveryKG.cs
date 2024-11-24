using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020014C7 RID: 5319
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ManualDeliveryKG")]
public class ManualDeliveryKG : KMonoBehaviour, ISim1000ms
{
	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x06006ED2 RID: 28370 RVA: 0x000E89B9 File Offset: 0x000E6BB9
	public bool IsPaused
	{
		get
		{
			return this.paused;
		}
	}

	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x06006ED3 RID: 28371 RVA: 0x000E89C1 File Offset: 0x000E6BC1
	public float Capacity
	{
		get
		{
			return this.capacity;
		}
	}

	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x06006ED4 RID: 28372 RVA: 0x000E89C9 File Offset: 0x000E6BC9
	// (set) Token: 0x06006ED5 RID: 28373 RVA: 0x000E89D1 File Offset: 0x000E6BD1
	public Tag RequestedItemTag
	{
		get
		{
			return this.requestedItemTag;
		}
		set
		{
			this.requestedItemTag = value;
			this.AbortDelivery("Requested Item Tag Changed");
		}
	}

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x06006ED6 RID: 28374 RVA: 0x000E89E5 File Offset: 0x000E6BE5
	// (set) Token: 0x06006ED7 RID: 28375 RVA: 0x000E89ED File Offset: 0x000E6BED
	public Tag[] ForbiddenTags
	{
		get
		{
			return this.forbiddenTags;
		}
		set
		{
			this.forbiddenTags = value;
			this.AbortDelivery("Forbidden Tags Changed");
		}
	}

	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x06006ED8 RID: 28376 RVA: 0x000E8A01 File Offset: 0x000E6C01
	public Storage DebugStorage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x06006ED9 RID: 28377 RVA: 0x000E8A09 File Offset: 0x000E6C09
	public FetchList2 DebugFetchList
	{
		get
		{
			return this.fetchList;
		}
	}

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x06006EDA RID: 28378 RVA: 0x000E8A11 File Offset: 0x000E6C11
	private float MassStoredPerUnit
	{
		get
		{
			return this.storage.GetMassAvailable(this.requestedItemTag) / this.MassPerUnit;
		}
	}

	// Token: 0x06006EDB RID: 28379 RVA: 0x002F0284 File Offset: 0x002EE484
	protected override void OnSpawn()
	{
		base.OnSpawn();
		DebugUtil.Assert(this.choreTypeIDHash.IsValid, "ManualDeliveryKG Must have a valid chore type specified!", base.name);
		if (this.allowPause)
		{
			base.Subscribe<ManualDeliveryKG>(493375141, ManualDeliveryKG.OnRefreshUserMenuDelegate);
			base.Subscribe<ManualDeliveryKG>(-111137758, ManualDeliveryKG.OnRefreshUserMenuDelegate);
		}
		base.Subscribe<ManualDeliveryKG>(-592767678, ManualDeliveryKG.OnOperationalChangedDelegate);
		if (this.storage != null)
		{
			this.SetStorage(this.storage);
		}
		Prioritizable.AddRef(base.gameObject);
		if (this.userPaused && this.allowPause)
		{
			this.OnPause();
		}
	}

	// Token: 0x06006EDC RID: 28380 RVA: 0x000E8A2B File Offset: 0x000E6C2B
	protected override void OnCleanUp()
	{
		this.AbortDelivery("ManualDeliverKG destroyed");
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06006EDD RID: 28381 RVA: 0x002F0328 File Offset: 0x002EE528
	public void SetStorage(Storage storage)
	{
		if (this.storage != null)
		{
			this.storage.Unsubscribe(this.onStorageChangeSubscription);
			this.onStorageChangeSubscription = -1;
		}
		this.AbortDelivery("storage pointer changed");
		this.storage = storage;
		if (this.storage != null && base.isSpawned)
		{
			global::Debug.Assert(this.onStorageChangeSubscription == -1);
			this.onStorageChangeSubscription = this.storage.Subscribe<ManualDeliveryKG>(-1697596308, ManualDeliveryKG.OnStorageChangedDelegate);
		}
	}

	// Token: 0x06006EDE RID: 28382 RVA: 0x000E8A49 File Offset: 0x000E6C49
	public void Pause(bool pause, string reason)
	{
		if (this.paused != pause)
		{
			this.paused = pause;
			if (pause)
			{
				this.AbortDelivery(reason);
			}
		}
	}

	// Token: 0x06006EDF RID: 28383 RVA: 0x000E8A65 File Offset: 0x000E6C65
	public void Sim1000ms(float dt)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x06006EE0 RID: 28384 RVA: 0x000E8A6D File Offset: 0x000E6C6D
	[ContextMenu("UpdateDeliveryState")]
	public void UpdateDeliveryState()
	{
		if (!this.requestedItemTag.IsValid)
		{
			return;
		}
		if (this.storage == null)
		{
			return;
		}
		this.UpdateFetchList();
	}

	// Token: 0x06006EE1 RID: 28385 RVA: 0x002F03AC File Offset: 0x002EE5AC
	public void RequestDelivery()
	{
		if (this.fetchList != null)
		{
			return;
		}
		float massStoredPerUnit = this.MassStoredPerUnit;
		if (massStoredPerUnit < this.capacity)
		{
			this.CreateFetchChore(massStoredPerUnit);
		}
	}

	// Token: 0x06006EE2 RID: 28386 RVA: 0x002F03DC File Offset: 0x002EE5DC
	private void CreateFetchChore(float stored_mass)
	{
		float num = this.capacity - stored_mass;
		num = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, num);
		if (this.RoundFetchAmountToInt)
		{
			num = (float)((int)num);
			if (num < 0.1f)
			{
				return;
			}
		}
		ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.choreTypeIDHash);
		this.fetchList = new FetchList2(this.storage, byHash);
		this.fetchList.ShowStatusItem = this.ShowStatusItem;
		this.fetchList.MinimumAmount[this.requestedItemTag] = Mathf.Max(PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT, this.MinimumMass);
		FetchList2 fetchList = this.fetchList;
		Tag tag = this.requestedItemTag;
		float amount = num;
		fetchList.Add(tag, this.forbiddenTags, amount, Operational.State.None);
		this.fetchList.Submit(new System.Action(this.OnFetchComplete), false);
	}

	// Token: 0x06006EE3 RID: 28387 RVA: 0x002F04A8 File Offset: 0x002EE6A8
	private void OnFetchComplete()
	{
		if (this.FillToCapacity && this.storage != null)
		{
			float amountAvailable = this.storage.GetAmountAvailable(this.requestedItemTag);
			if (amountAvailable < this.capacity)
			{
				this.CreateFetchChore(amountAvailable);
			}
		}
	}

	// Token: 0x06006EE4 RID: 28388 RVA: 0x002F04F0 File Offset: 0x002EE6F0
	private void UpdateFetchList()
	{
		if (this.paused)
		{
			return;
		}
		if (this.fetchList != null && this.fetchList.IsComplete)
		{
			this.fetchList = null;
		}
		if (!(this.operational == null) && !this.operational.MeetsRequirements(this.operationalRequirement))
		{
			if (this.fetchList != null)
			{
				this.fetchList.Cancel("Operational requirements");
				this.fetchList = null;
				return;
			}
		}
		else if (this.fetchList == null && this.MassStoredPerUnit < this.refillMass)
		{
			this.RequestDelivery();
		}
	}

	// Token: 0x06006EE5 RID: 28389 RVA: 0x000E8A92 File Offset: 0x000E6C92
	public void AbortDelivery(string reason)
	{
		if (this.fetchList != null)
		{
			FetchList2 fetchList = this.fetchList;
			this.fetchList = null;
			fetchList.Cancel(reason);
		}
	}

	// Token: 0x06006EE6 RID: 28390 RVA: 0x000E8A65 File Offset: 0x000E6C65
	protected void OnStorageChanged(object data)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x06006EE7 RID: 28391 RVA: 0x000E8AAF File Offset: 0x000E6CAF
	private void OnPause()
	{
		this.userPaused = true;
		this.Pause(true, "Forbid manual delivery");
	}

	// Token: 0x06006EE8 RID: 28392 RVA: 0x000E8AC4 File Offset: 0x000E6CC4
	private void OnResume()
	{
		this.userPaused = false;
		this.Pause(false, "Allow manual delivery");
	}

	// Token: 0x06006EE9 RID: 28393 RVA: 0x002F0580 File Offset: 0x002EE780
	private void OnRefreshUserMenu(object data)
	{
		if (!this.allowPause)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.paused) ? new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME, new System.Action(this.OnPause), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_move_to_storage", UI.USERMENUACTIONS.MANUAL_DELIVERY.NAME_OFF, new System.Action(this.OnResume), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.MANUAL_DELIVERY.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06006EEA RID: 28394 RVA: 0x000E8A65 File Offset: 0x000E6C65
	private void OnOperationalChanged(object data)
	{
		this.UpdateDeliveryState();
	}

	// Token: 0x040052C5 RID: 21189
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040052C6 RID: 21190
	[SerializeField]
	private Storage storage;

	// Token: 0x040052C7 RID: 21191
	[SerializeField]
	public Tag requestedItemTag;

	// Token: 0x040052C8 RID: 21192
	private Tag[] forbiddenTags;

	// Token: 0x040052C9 RID: 21193
	[SerializeField]
	public float capacity = 100f;

	// Token: 0x040052CA RID: 21194
	[SerializeField]
	public float refillMass = 10f;

	// Token: 0x040052CB RID: 21195
	[SerializeField]
	public float MinimumMass = 10f;

	// Token: 0x040052CC RID: 21196
	[SerializeField]
	public bool RoundFetchAmountToInt;

	// Token: 0x040052CD RID: 21197
	[SerializeField]
	public float MassPerUnit = 1f;

	// Token: 0x040052CE RID: 21198
	[SerializeField]
	public bool FillToCapacity;

	// Token: 0x040052CF RID: 21199
	[SerializeField]
	public Operational.State operationalRequirement;

	// Token: 0x040052D0 RID: 21200
	[SerializeField]
	public bool allowPause;

	// Token: 0x040052D1 RID: 21201
	[SerializeField]
	private bool paused;

	// Token: 0x040052D2 RID: 21202
	[SerializeField]
	public HashedString choreTypeIDHash;

	// Token: 0x040052D3 RID: 21203
	[Serialize]
	private bool userPaused;

	// Token: 0x040052D4 RID: 21204
	public bool ShowStatusItem = true;

	// Token: 0x040052D5 RID: 21205
	private FetchList2 fetchList;

	// Token: 0x040052D6 RID: 21206
	private int onStorageChangeSubscription = -1;

	// Token: 0x040052D7 RID: 21207
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040052D8 RID: 21208
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x040052D9 RID: 21209
	private static readonly EventSystem.IntraObjectHandler<ManualDeliveryKG> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<ManualDeliveryKG>(delegate(ManualDeliveryKG component, object data)
	{
		component.OnStorageChanged(data);
	});
}
