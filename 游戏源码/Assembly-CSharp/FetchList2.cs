using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012EA RID: 4842
public class FetchList2 : IFetchList
{
	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x0600634F RID: 25423 RVA: 0x000E0DA2 File Offset: 0x000DEFA2
	// (set) Token: 0x06006350 RID: 25424 RVA: 0x000E0DAA File Offset: 0x000DEFAA
	public bool ShowStatusItem
	{
		get
		{
			return this.bShowStatusItem;
		}
		set
		{
			this.bShowStatusItem = value;
		}
	}

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06006351 RID: 25425 RVA: 0x000E0DB3 File Offset: 0x000DEFB3
	public bool IsComplete
	{
		get
		{
			return this.FetchOrders.Count == 0;
		}
	}

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06006352 RID: 25426 RVA: 0x002BA598 File Offset: 0x002B8798
	public bool InProgress
	{
		get
		{
			if (this.FetchOrders.Count < 0)
			{
				return false;
			}
			bool result = false;
			using (List<FetchOrder2>.Enumerator enumerator = this.FetchOrders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.InProgress)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06006353 RID: 25427 RVA: 0x000E0DC3 File Offset: 0x000DEFC3
	// (set) Token: 0x06006354 RID: 25428 RVA: 0x000E0DCB File Offset: 0x000DEFCB
	public Storage Destination { get; private set; }

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x06006355 RID: 25429 RVA: 0x000E0DD4 File Offset: 0x000DEFD4
	// (set) Token: 0x06006356 RID: 25430 RVA: 0x000E0DDC File Offset: 0x000DEFDC
	public int PriorityMod { get; private set; }

	// Token: 0x06006357 RID: 25431 RVA: 0x002BA604 File Offset: 0x002B8804
	public FetchList2(Storage destination, ChoreType chore_type)
	{
		this.Destination = destination;
		this.choreType = chore_type;
	}

	// Token: 0x06006358 RID: 25432 RVA: 0x002BA670 File Offset: 0x002B8870
	public void SetPriorityMod(int priorityMod)
	{
		this.PriorityMod = priorityMod;
		for (int i = 0; i < this.FetchOrders.Count; i++)
		{
			this.FetchOrders[i].SetPriorityMod(this.PriorityMod);
		}
	}

	// Token: 0x06006359 RID: 25433 RVA: 0x002BA6B4 File Offset: 0x002B88B4
	public void Add(HashSet<Tag> tags, Tag requiredTag, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		foreach (Tag key in tags)
		{
			if (!this.MinimumAmount.ContainsKey(key))
			{
				this.MinimumAmount[key] = amount;
			}
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, tags, FetchChore.MatchCriteria.MatchID, requiredTag, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x0600635A RID: 25434 RVA: 0x002BA744 File Offset: 0x002B8944
	public void Add(HashSet<Tag> tags, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		foreach (Tag key in tags)
		{
			if (!this.MinimumAmount.ContainsKey(key))
			{
				this.MinimumAmount[key] = amount;
			}
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, tags, FetchChore.MatchCriteria.MatchID, Tag.Invalid, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x0600635B RID: 25435 RVA: 0x002BA7D8 File Offset: 0x002B89D8
	public void Add(Tag tag, Tag[] forbidden_tags = null, float amount = 1f, Operational.State operationalRequirementDEPRECATED = Operational.State.None)
	{
		if (!this.MinimumAmount.ContainsKey(tag))
		{
			this.MinimumAmount[tag] = amount;
		}
		FetchOrder2 item = new FetchOrder2(this.choreType, new HashSet<Tag>
		{
			tag
		}, FetchChore.MatchCriteria.MatchTags, Tag.Invalid, forbidden_tags, this.Destination, amount, operationalRequirementDEPRECATED, this.PriorityMod);
		this.FetchOrders.Add(item);
	}

	// Token: 0x0600635C RID: 25436 RVA: 0x002BA83C File Offset: 0x002B8A3C
	public float GetMinimumAmount(Tag tag)
	{
		float result = 0f;
		this.MinimumAmount.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x0600635D RID: 25437 RVA: 0x000E0DE5 File Offset: 0x000DEFE5
	private void OnFetchOrderComplete(FetchOrder2 fetch_order, Pickupable fetched_item)
	{
		this.FetchOrders.Remove(fetch_order);
		if (this.FetchOrders.Count == 0)
		{
			if (this.OnComplete != null)
			{
				this.OnComplete();
			}
			FetchListStatusItemUpdater.instance.RemoveFetchList(this);
			this.ClearStatus();
		}
	}

	// Token: 0x0600635E RID: 25438 RVA: 0x002BA860 File Offset: 0x002B8A60
	public void Cancel(string reason)
	{
		FetchListStatusItemUpdater.instance.RemoveFetchList(this);
		this.ClearStatus();
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Cancel(reason);
		}
	}

	// Token: 0x0600635F RID: 25439 RVA: 0x002BA8C4 File Offset: 0x002B8AC4
	public void UpdateRemaining()
	{
		this.Remaining.Clear();
		for (int i = 0; i < this.FetchOrders.Count; i++)
		{
			FetchOrder2 fetchOrder = this.FetchOrders[i];
			foreach (Tag key in fetchOrder.Tags)
			{
				float num = 0f;
				this.Remaining.TryGetValue(key, out num);
				this.Remaining[key] = num + fetchOrder.AmountWaitingToFetch();
			}
		}
	}

	// Token: 0x06006360 RID: 25440 RVA: 0x000E0E25 File Offset: 0x000DF025
	public Dictionary<Tag, float> GetRemaining()
	{
		return this.Remaining;
	}

	// Token: 0x06006361 RID: 25441 RVA: 0x002BA96C File Offset: 0x002B8B6C
	public Dictionary<Tag, float> GetRemainingMinimum()
	{
		Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			foreach (Tag key in fetchOrder.Tags)
			{
				dictionary[key] = this.MinimumAmount[key];
			}
		}
		foreach (GameObject gameObject in this.Destination.items)
		{
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component != null)
				{
					KPrefabID kprefabID = component.KPrefabID;
					if (dictionary.ContainsKey(kprefabID.PrefabTag))
					{
						dictionary[kprefabID.PrefabTag] = Math.Max(dictionary[kprefabID.PrefabTag] - component.TotalAmount, 0f);
					}
					foreach (Tag key2 in kprefabID.Tags)
					{
						if (dictionary.ContainsKey(key2))
						{
							dictionary[key2] = Math.Max(dictionary[key2] - component.TotalAmount, 0f);
						}
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x06006362 RID: 25442 RVA: 0x002BAB24 File Offset: 0x002B8D24
	public void Suspend(string reason)
	{
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Suspend(reason);
		}
	}

	// Token: 0x06006363 RID: 25443 RVA: 0x002BAB78 File Offset: 0x002B8D78
	public void Resume(string reason)
	{
		foreach (FetchOrder2 fetchOrder in this.FetchOrders)
		{
			fetchOrder.Resume(reason);
		}
	}

	// Token: 0x06006364 RID: 25444 RVA: 0x002BABCC File Offset: 0x002B8DCC
	public void Submit(System.Action on_complete, bool check_storage_contents)
	{
		this.OnComplete = on_complete;
		foreach (FetchOrder2 fetchOrder in this.FetchOrders.GetRange(0, this.FetchOrders.Count))
		{
			fetchOrder.Submit(new Action<FetchOrder2, Pickupable>(this.OnFetchOrderComplete), check_storage_contents, null);
		}
		if (!this.IsComplete && this.ShowStatusItem)
		{
			FetchListStatusItemUpdater.instance.AddFetchList(this);
		}
	}

	// Token: 0x06006365 RID: 25445 RVA: 0x002BAC60 File Offset: 0x002B8E60
	private void ClearStatus()
	{
		if (this.Destination != null)
		{
			KSelectable component = this.Destination.GetComponent<KSelectable>();
			if (component != null)
			{
				this.waitingForMaterialsHandle = component.RemoveStatusItem(this.waitingForMaterialsHandle, false);
				this.materialsUnavailableHandle = component.RemoveStatusItem(this.materialsUnavailableHandle, false);
				this.materialsUnavailableForRefillHandle = component.RemoveStatusItem(this.materialsUnavailableForRefillHandle, false);
			}
		}
	}

	// Token: 0x06006366 RID: 25446 RVA: 0x002BACCC File Offset: 0x002B8ECC
	public void UpdateStatusItem(MaterialsStatusItem status_item, ref Guid handle, bool should_add)
	{
		bool flag = handle != Guid.Empty;
		if (should_add != flag)
		{
			if (should_add)
			{
				KSelectable component = this.Destination.GetComponent<KSelectable>();
				if (component != null)
				{
					handle = component.AddStatusItem(status_item, this);
					GameScheduler.Instance.Schedule("Digging Tutorial", 2f, delegate(object obj)
					{
						Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Digging, true);
					}, null, null);
					return;
				}
			}
			else
			{
				KSelectable component2 = this.Destination.GetComponent<KSelectable>();
				if (component2 != null)
				{
					handle = component2.RemoveStatusItem(handle, false);
				}
			}
		}
	}

	// Token: 0x04004703 RID: 18179
	private System.Action OnComplete;

	// Token: 0x04004706 RID: 18182
	private ChoreType choreType;

	// Token: 0x04004707 RID: 18183
	public Guid waitingForMaterialsHandle = Guid.Empty;

	// Token: 0x04004708 RID: 18184
	public Guid materialsUnavailableForRefillHandle = Guid.Empty;

	// Token: 0x04004709 RID: 18185
	public Guid materialsUnavailableHandle = Guid.Empty;

	// Token: 0x0400470A RID: 18186
	public Dictionary<Tag, float> MinimumAmount = new Dictionary<Tag, float>();

	// Token: 0x0400470B RID: 18187
	public List<FetchOrder2> FetchOrders = new List<FetchOrder2>();

	// Token: 0x0400470C RID: 18188
	private Dictionary<Tag, float> Remaining = new Dictionary<Tag, float>();

	// Token: 0x0400470D RID: 18189
	private bool bShowStatusItem = true;
}
