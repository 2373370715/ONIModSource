using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000F81 RID: 3969
[AddComponentMenu("KMonoBehaviour/Workable/SingleEntityReceptacle")]
public class SingleEntityReceptacle : Workable, IRender1000ms
{
	// Token: 0x1700047B RID: 1147
	// (get) Token: 0x06005050 RID: 20560 RVA: 0x000D45FA File Offset: 0x000D27FA
	public FetchChore GetActiveRequest
	{
		get
		{
			return this.fetchChore;
		}
	}

	// Token: 0x1700047C RID: 1148
	// (get) Token: 0x06005051 RID: 20561 RVA: 0x000D4602 File Offset: 0x000D2802
	// (set) Token: 0x06005052 RID: 20562 RVA: 0x000D4629 File Offset: 0x000D2829
	protected GameObject occupyingObject
	{
		get
		{
			if (this.occupyObjectRef.Get() != null)
			{
				return this.occupyObjectRef.Get().gameObject;
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				this.occupyObjectRef.Set(null);
				return;
			}
			this.occupyObjectRef.Set(value.GetComponent<KSelectable>());
		}
	}

	// Token: 0x1700047D RID: 1149
	// (get) Token: 0x06005053 RID: 20563 RVA: 0x000D4652 File Offset: 0x000D2852
	public GameObject Occupant
	{
		get
		{
			return this.occupyingObject;
		}
	}

	// Token: 0x1700047E RID: 1150
	// (get) Token: 0x06005054 RID: 20564 RVA: 0x000D465A File Offset: 0x000D285A
	public IReadOnlyList<Tag> possibleDepositObjectTags
	{
		get
		{
			return this.possibleDepositTagsList;
		}
	}

	// Token: 0x06005055 RID: 20565 RVA: 0x000D4662 File Offset: 0x000D2862
	public bool HasDepositTag(Tag tag)
	{
		return this.possibleDepositTagsList.Contains(tag);
	}

	// Token: 0x06005056 RID: 20566 RVA: 0x0026E57C File Offset: 0x0026C77C
	public bool IsValidEntity(GameObject candidate)
	{
		KPrefabID component = candidate.GetComponent<KPrefabID>();
		if (!SaveLoader.Instance.IsDlcListActiveForCurrentSave(component.requiredDlcIds))
		{
			return false;
		}
		IReceptacleDirection component2 = candidate.GetComponent<IReceptacleDirection>();
		bool flag = this.rotatable != null || component2 == null || component2.Direction == this.Direction;
		int num = 0;
		while (flag && num < this.additionalCriteria.Count)
		{
			flag = this.additionalCriteria[num](candidate);
			num++;
		}
		return flag;
	}

	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x06005057 RID: 20567 RVA: 0x000D4670 File Offset: 0x000D2870
	public SingleEntityReceptacle.ReceptacleDirection Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x06005058 RID: 20568 RVA: 0x000BC8FA File Offset: 0x000BAAFA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005059 RID: 20569 RVA: 0x0026E5FC File Offset: 0x0026C7FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.occupyingObject != null)
		{
			this.PositionOccupyingObject();
			this.SubscribeToOccupant();
		}
		this.UpdateStatusItem();
		if (this.occupyingObject == null && !this.requestedEntityTag.IsValid)
		{
			this.requestedEntityAdditionalFilterTag = null;
		}
		if (this.occupyingObject == null && this.requestedEntityTag.IsValid)
		{
			this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		}
		base.Subscribe<SingleEntityReceptacle>(-592767678, SingleEntityReceptacle.OnOperationalChangedDelegate);
	}

	// Token: 0x0600505A RID: 20570 RVA: 0x000D4678 File Offset: 0x000D2878
	public void AddDepositTag(Tag t)
	{
		this.possibleDepositTagsList.Add(t);
	}

	// Token: 0x0600505B RID: 20571 RVA: 0x000D4686 File Offset: 0x000D2886
	public void AddAdditionalCriteria(Func<GameObject, bool> criteria)
	{
		this.additionalCriteria.Add(criteria);
	}

	// Token: 0x0600505C RID: 20572 RVA: 0x000D4694 File Offset: 0x000D2894
	public void SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection d)
	{
		this.direction = d;
	}

	// Token: 0x0600505D RID: 20573 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void SetPreview(Tag entityTag, bool solid = false)
	{
	}

	// Token: 0x0600505E RID: 20574 RVA: 0x000D469D File Offset: 0x000D289D
	public virtual void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		this.requestedEntityTag = entityTag;
		this.requestedEntityAdditionalFilterTag = additionalFilterTag;
		this.CreateFetchChore(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		this.SetPreview(entityTag, true);
		this.UpdateStatusItem();
	}

	// Token: 0x0600505F RID: 20575 RVA: 0x000D46CD File Offset: 0x000D28CD
	public void Render1000ms(float dt)
	{
		this.UpdateStatusItem();
	}

	// Token: 0x06005060 RID: 20576 RVA: 0x0026E694 File Offset: 0x0026C894
	protected virtual void UpdateStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.Occupant != null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, null, null);
			return;
		}
		if (this.fetchChore == null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNeed, null);
			return;
		}
		bool flag = this.fetchChore.fetcher != null;
		WorldContainer myWorld = this.GetMyWorld();
		if (!flag && myWorld != null)
		{
			foreach (Tag tag in this.fetchChore.tags)
			{
				if (myWorld.worldInventory.GetTotalAmount(tag, true) > 0f)
				{
					if (myWorld.worldInventory.GetTotalAmount(this.requestedEntityAdditionalFilterTag, true) > 0f || this.requestedEntityAdditionalFilterTag == Tag.Invalid)
					{
						flag = true;
						break;
					}
					break;
				}
			}
		}
		if (flag)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemAwaitingDelivery, null);
			return;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNoneAvailable, null);
	}

	// Token: 0x06005061 RID: 20577 RVA: 0x0026E7E8 File Offset: 0x0026C9E8
	protected void CreateFetchChore(Tag entityTag, Tag additionalRequiredTag)
	{
		if (this.fetchChore == null && entityTag.IsValid && entityTag != GameTags.Empty)
		{
			this.fetchChore = new FetchChore(this.choreType, this.storage, 1f, new HashSet<Tag>
			{
				entityTag
			}, FetchChore.MatchCriteria.MatchID, (additionalRequiredTag.IsValid && additionalRequiredTag != GameTags.Empty) ? additionalRequiredTag : Tag.Invalid, null, null, true, new Action<Chore>(this.OnFetchComplete), delegate(Chore chore)
			{
				this.UpdateStatusItem();
			}, delegate(Chore chore)
			{
				this.UpdateStatusItem();
			}, Operational.State.Functional, 0);
			MaterialNeeds.UpdateNeed(this.requestedEntityTag, 1f, base.gameObject.GetMyWorldId());
			this.UpdateStatusItem();
		}
	}

	// Token: 0x06005062 RID: 20578 RVA: 0x000D46D5 File Offset: 0x000D28D5
	public virtual void OrderRemoveOccupant()
	{
		this.ClearOccupant();
	}

	// Token: 0x06005063 RID: 20579 RVA: 0x0026E8B0 File Offset: 0x0026CAB0
	protected virtual void ClearOccupant()
	{
		if (this.occupyingObject)
		{
			this.UnsubscribeFromOccupant();
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
		this.occupyingObject = null;
		this.UpdateActive();
		this.UpdateStatusItem();
		base.Trigger(-731304873, this.occupyingObject);
	}

	// Token: 0x06005064 RID: 20580 RVA: 0x0026E90C File Offset: 0x0026CB0C
	public void CancelActiveRequest()
	{
		if (this.fetchChore != null)
		{
			MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, base.gameObject.GetMyWorldId());
			this.fetchChore.Cancel("User canceled");
			this.fetchChore = null;
		}
		this.requestedEntityTag = Tag.Invalid;
		this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		this.UpdateStatusItem();
		this.SetPreview(Tag.Invalid, false);
	}

	// Token: 0x06005065 RID: 20581 RVA: 0x0026E97C File Offset: 0x0026CB7C
	private void OnOccupantDestroyed(object data)
	{
		this.occupyingObject = null;
		this.ClearOccupant();
		if (this.autoReplaceEntity && this.requestedEntityTag.IsValid && this.requestedEntityTag != GameTags.Empty)
		{
			this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		}
	}

	// Token: 0x06005066 RID: 20582 RVA: 0x000D46DD File Offset: 0x000D28DD
	protected virtual void SubscribeToOccupant()
	{
		if (this.occupyingObject != null)
		{
			base.Subscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
		}
	}

	// Token: 0x06005067 RID: 20583 RVA: 0x000D470B File Offset: 0x000D290B
	protected virtual void UnsubscribeFromOccupant()
	{
		if (this.occupyingObject != null)
		{
			base.Unsubscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
		}
	}

	// Token: 0x06005068 RID: 20584 RVA: 0x0026E9D0 File Offset: 0x0026CBD0
	private void OnFetchComplete(Chore chore)
	{
		if (this.fetchChore == null)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} OnFetchComplete fetchChore null", new object[]
			{
				base.gameObject
			});
			return;
		}
		if (this.fetchChore.fetchTarget == null)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} OnFetchComplete fetchChore.fetchTarget null", new object[]
			{
				base.gameObject
			});
			return;
		}
		this.OnDepositObject(this.fetchChore.fetchTarget.gameObject);
	}

	// Token: 0x06005069 RID: 20585 RVA: 0x000D4738 File Offset: 0x000D2938
	public void ForceDeposit(GameObject depositedObject)
	{
		if (this.occupyingObject != null)
		{
			this.ClearOccupant();
		}
		this.OnDepositObject(depositedObject);
	}

	// Token: 0x0600506A RID: 20586 RVA: 0x0026EA50 File Offset: 0x0026CC50
	private void OnDepositObject(GameObject depositedObject)
	{
		this.SetPreview(Tag.Invalid, false);
		MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, base.gameObject.GetMyWorldId());
		KBatchedAnimController component = depositedObject.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.GetBatchInstanceData().ClearOverrideTransformMatrix();
		}
		this.occupyingObject = this.SpawnOccupyingObject(depositedObject);
		if (this.occupyingObject != null)
		{
			this.ConfigureOccupyingObject(this.occupyingObject);
			this.occupyingObject.SetActive(true);
			this.PositionOccupyingObject();
			this.SubscribeToOccupant();
		}
		else
		{
			global::Debug.LogWarning(base.gameObject.name + " EntityReceptacle did not spawn occupying entity.");
		}
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("receptacle filled");
			this.fetchChore = null;
		}
		if (!this.autoReplaceEntity)
		{
			this.requestedEntityTag = Tag.Invalid;
			this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		}
		this.UpdateActive();
		this.UpdateStatusItem();
		if (this.destroyEntityOnDeposit)
		{
			Util.KDestroyGameObject(depositedObject);
		}
		base.Trigger(-731304873, this.occupyingObject);
	}

	// Token: 0x0600506B RID: 20587 RVA: 0x000B1FA8 File Offset: 0x000B01A8
	protected virtual GameObject SpawnOccupyingObject(GameObject depositedEntity)
	{
		return depositedEntity;
	}

	// Token: 0x0600506C RID: 20588 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void ConfigureOccupyingObject(GameObject source)
	{
	}

	// Token: 0x0600506D RID: 20589 RVA: 0x0026EB64 File Offset: 0x0026CD64
	protected virtual void PositionOccupyingObject()
	{
		if (this.rotatable != null)
		{
			this.occupyingObject.transform.SetPosition(base.gameObject.transform.GetPosition() + this.rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition));
		}
		else
		{
			this.occupyingObject.transform.SetPosition(base.gameObject.transform.GetPosition() + this.occupyingObjectRelativePosition);
		}
		KBatchedAnimController component = this.occupyingObject.GetComponent<KBatchedAnimController>();
		component.enabled = false;
		component.enabled = true;
	}

	// Token: 0x0600506E RID: 20590 RVA: 0x0026EBFC File Offset: 0x0026CDFC
	protected void UpdateActive()
	{
		if (this.Equals(null) || this == null || base.gameObject.Equals(null) || base.gameObject == null)
		{
			return;
		}
		if (this.operational != null)
		{
			this.operational.SetActive(this.operational.IsOperational && this.occupyingObject != null, false);
		}
	}

	// Token: 0x0600506F RID: 20591 RVA: 0x000D4755 File Offset: 0x000D2955
	protected override void OnCleanUp()
	{
		this.CancelActiveRequest();
		this.UnsubscribeFromOccupant();
		base.OnCleanUp();
	}

	// Token: 0x06005070 RID: 20592 RVA: 0x000D4769 File Offset: 0x000D2969
	private void OnOperationalChanged(object data)
	{
		this.UpdateActive();
		if (this.occupyingObject)
		{
			this.occupyingObject.Trigger(this.operational.IsOperational ? 1628751838 : 960378201, null);
		}
	}

	// Token: 0x04003805 RID: 14341
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x04003806 RID: 14342
	[MyCmpReq]
	protected Storage storage;

	// Token: 0x04003807 RID: 14343
	[MyCmpGet]
	public Rotatable rotatable;

	// Token: 0x04003808 RID: 14344
	protected FetchChore fetchChore;

	// Token: 0x04003809 RID: 14345
	public ChoreType choreType = Db.Get().ChoreTypes.Fetch;

	// Token: 0x0400380A RID: 14346
	[Serialize]
	public bool autoReplaceEntity;

	// Token: 0x0400380B RID: 14347
	[Serialize]
	public Tag requestedEntityTag;

	// Token: 0x0400380C RID: 14348
	[Serialize]
	public Tag requestedEntityAdditionalFilterTag;

	// Token: 0x0400380D RID: 14349
	[Serialize]
	protected Ref<KSelectable> occupyObjectRef = new Ref<KSelectable>();

	// Token: 0x0400380E RID: 14350
	[SerializeField]
	private List<Tag> possibleDepositTagsList = new List<Tag>();

	// Token: 0x0400380F RID: 14351
	[SerializeField]
	private List<Func<GameObject, bool>> additionalCriteria = new List<Func<GameObject, bool>>();

	// Token: 0x04003810 RID: 14352
	[SerializeField]
	protected bool destroyEntityOnDeposit;

	// Token: 0x04003811 RID: 14353
	[SerializeField]
	protected SingleEntityReceptacle.ReceptacleDirection direction;

	// Token: 0x04003812 RID: 14354
	public Vector3 occupyingObjectRelativePosition = new Vector3(0f, 1f, 3f);

	// Token: 0x04003813 RID: 14355
	protected StatusItem statusItemAwaitingDelivery;

	// Token: 0x04003814 RID: 14356
	protected StatusItem statusItemNeed;

	// Token: 0x04003815 RID: 14357
	protected StatusItem statusItemNoneAvailable;

	// Token: 0x04003816 RID: 14358
	private static readonly EventSystem.IntraObjectHandler<SingleEntityReceptacle> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SingleEntityReceptacle>(delegate(SingleEntityReceptacle component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x02000F82 RID: 3970
	public enum ReceptacleDirection
	{
		// Token: 0x04003818 RID: 14360
		Top,
		// Token: 0x04003819 RID: 14361
		Side,
		// Token: 0x0400381A RID: 14362
		Bottom
	}
}
