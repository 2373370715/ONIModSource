using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000FBD RID: 4029
[AddComponentMenu("KMonoBehaviour/scripts/StorageLocker")]
public class StorageLocker : KMonoBehaviour, IUserControlledCapacity
{
	// Token: 0x0600518C RID: 20876 RVA: 0x000D520C File Offset: 0x000D340C
	protected override void OnPrefabInit()
	{
		this.Initialize(false);
	}

	// Token: 0x0600518D RID: 20877 RVA: 0x002724C0 File Offset: 0x002706C0
	protected void Initialize(bool use_logic_meter)
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("StorageLocker", 35);
		ChoreType fetch_chore_type = Db.Get().ChoreTypes.Get(this.choreTypeID);
		this.filteredStorage = new FilteredStorage(this, null, this, use_logic_meter, fetch_chore_type);
		base.Subscribe<StorageLocker>(-905833192, StorageLocker.OnCopySettingsDelegate);
	}

	// Token: 0x0600518E RID: 20878 RVA: 0x0027251C File Offset: 0x0027071C
	protected override void OnSpawn()
	{
		this.filteredStorage.FilterChanged();
		if (this.nameable != null && !this.lockerName.IsNullOrWhiteSpace())
		{
			this.nameable.SetName(this.lockerName);
		}
		base.Trigger(-1683615038, null);
	}

	// Token: 0x0600518F RID: 20879 RVA: 0x000D5215 File Offset: 0x000D3415
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
	}

	// Token: 0x06005190 RID: 20880 RVA: 0x0027256C File Offset: 0x0027076C
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		StorageLocker component = gameObject.GetComponent<StorageLocker>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06005191 RID: 20881 RVA: 0x000D5222 File Offset: 0x000D3422
	public void UpdateForbiddenTag(Tag game_tag, bool forbidden)
	{
		if (forbidden)
		{
			this.filteredStorage.RemoveForbiddenTag(game_tag);
			return;
		}
		this.filteredStorage.AddForbiddenTag(game_tag);
	}

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x06005192 RID: 20882 RVA: 0x000D5240 File Offset: 0x000D3440
	// (set) Token: 0x06005193 RID: 20883 RVA: 0x000D5258 File Offset: 0x000D3458
	public virtual float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, base.GetComponent<Storage>().capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x06005194 RID: 20884 RVA: 0x000D1D7D File Offset: 0x000CFF7D
	public float AmountStored
	{
		get
		{
			return base.GetComponent<Storage>().MassStored();
		}
	}

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06005195 RID: 20885 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06005196 RID: 20886 RVA: 0x000D1D8A File Offset: 0x000CFF8A
	public float MaxCapacity
	{
		get
		{
			return base.GetComponent<Storage>().capacityKg;
		}
	}

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06005197 RID: 20887 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06005198 RID: 20888 RVA: 0x000C8D02 File Offset: 0x000C6F02
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x04003914 RID: 14612
	private LoggerFS log;

	// Token: 0x04003915 RID: 14613
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04003916 RID: 14614
	[Serialize]
	public string lockerName = "";

	// Token: 0x04003917 RID: 14615
	protected FilteredStorage filteredStorage;

	// Token: 0x04003918 RID: 14616
	[MyCmpGet]
	private UserNameable nameable;

	// Token: 0x04003919 RID: 14617
	public string choreTypeID = Db.Get().ChoreTypes.StorageFetch.Id;

	// Token: 0x0400391A RID: 14618
	private static readonly EventSystem.IntraObjectHandler<StorageLocker> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<StorageLocker>(delegate(StorageLocker component, object data)
	{
		component.OnCopySettings(data);
	});
}
