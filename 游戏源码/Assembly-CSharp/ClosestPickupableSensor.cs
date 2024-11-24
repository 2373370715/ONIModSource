using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000826 RID: 2086
public abstract class ClosestPickupableSensor<T> : Sensor where T : Component
{
	// Token: 0x0600254D RID: 9549 RVA: 0x000B85E3 File Offset: 0x000B67E3
	public ClosestPickupableSensor(Sensors sensors, Tag itemSearchTag, bool shouldStartActive) : base(sensors, shouldStartActive)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.itemSearchTag = itemSearchTag;
	}

	// Token: 0x0600254E RID: 9550 RVA: 0x000B860B File Offset: 0x000B680B
	public T GetItem()
	{
		return this.item;
	}

	// Token: 0x0600254F RID: 9551 RVA: 0x000B8613 File Offset: 0x000B6813
	public int GetItemNavCost()
	{
		if (!(this.item == null))
		{
			return this.itemNavCost;
		}
		return int.MaxValue;
	}

	// Token: 0x06002550 RID: 9552 RVA: 0x001CBF88 File Offset: 0x001CA188
	public override void Update()
	{
		HashSet<Tag> forbiddenTagSet = base.GetComponent<ConsumableConsumer>().forbiddenTagSet;
		int maxValue = int.MaxValue;
		Pickupable pickupable = this.FindClosestPickupable(base.GetComponent<Storage>(), forbiddenTagSet, out maxValue, this.itemSearchTag, this.requiredTags);
		bool flag = this.itemInReachButNotPermitted;
		T t = default(T);
		bool flag2 = false;
		if (pickupable != null)
		{
			t = pickupable.GetComponent<T>();
			flag2 = true;
			flag = false;
		}
		else
		{
			int num;
			flag = (this.FindClosestPickupable(base.GetComponent<Storage>(), new HashSet<Tag>(), out num, this.itemSearchTag, this.requiredTags) != null);
		}
		if (t != this.item || this.isThereAnyItemAvailable != flag2)
		{
			this.item = t;
			this.itemNavCost = maxValue;
			this.isThereAnyItemAvailable = flag2;
			this.itemInReachButNotPermitted = flag;
			this.ItemChanged();
		}
	}

	// Token: 0x06002551 RID: 9553 RVA: 0x001CC05C File Offset: 0x001CA25C
	public Pickupable FindClosestPickupable(Storage destination, HashSet<Tag> exclude_tags, out int cost, Tag categoryTag, Tag[] otherRequiredTags = null)
	{
		ICollection<Pickupable> pickupables = base.gameObject.GetMyWorld().worldInventory.GetPickupables(categoryTag, false);
		if (pickupables == null)
		{
			cost = int.MaxValue;
			return null;
		}
		if (otherRequiredTags == null)
		{
			otherRequiredTags = new Tag[]
			{
				categoryTag
			};
		}
		Pickupable result = null;
		int num = int.MaxValue;
		foreach (Pickupable pickupable in pickupables)
		{
			if (FetchManager.IsFetchablePickup_Exclude(pickupable.KPrefabID, pickupable.storage, pickupable.UnreservedAmount, exclude_tags, otherRequiredTags, destination))
			{
				int navigationCost = pickupable.GetNavigationCost(this.navigator, pickupable.cachedCell);
				if (navigationCost != -1 && navigationCost < num)
				{
					result = pickupable;
					num = navigationCost;
				}
			}
		}
		cost = num;
		return result;
	}

	// Token: 0x06002552 RID: 9554 RVA: 0x000B8634 File Offset: 0x000B6834
	public virtual void ItemChanged()
	{
		Action<T> onItemChanged = this.OnItemChanged;
		if (onItemChanged == null)
		{
			return;
		}
		onItemChanged(this.item);
	}

	// Token: 0x04001931 RID: 6449
	public Action<T> OnItemChanged;

	// Token: 0x04001932 RID: 6450
	protected T item;

	// Token: 0x04001933 RID: 6451
	protected int itemNavCost = int.MaxValue;

	// Token: 0x04001934 RID: 6452
	protected Tag itemSearchTag;

	// Token: 0x04001935 RID: 6453
	protected Tag[] requiredTags;

	// Token: 0x04001936 RID: 6454
	protected bool isThereAnyItemAvailable;

	// Token: 0x04001937 RID: 6455
	protected bool itemInReachButNotPermitted;

	// Token: 0x04001938 RID: 6456
	private Navigator navigator;
}
