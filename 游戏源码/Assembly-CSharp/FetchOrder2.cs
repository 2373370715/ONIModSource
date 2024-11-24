using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020012F5 RID: 4853
public class FetchOrder2
{
	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x06006398 RID: 25496 RVA: 0x000E0FA3 File Offset: 0x000DF1A3
	// (set) Token: 0x06006399 RID: 25497 RVA: 0x000E0FAB File Offset: 0x000DF1AB
	public float TotalAmount { get; set; }

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x0600639A RID: 25498 RVA: 0x000E0FB4 File Offset: 0x000DF1B4
	// (set) Token: 0x0600639B RID: 25499 RVA: 0x000E0FBC File Offset: 0x000DF1BC
	public int PriorityMod { get; set; }

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x0600639C RID: 25500 RVA: 0x000E0FC5 File Offset: 0x000DF1C5
	// (set) Token: 0x0600639D RID: 25501 RVA: 0x000E0FCD File Offset: 0x000DF1CD
	public HashSet<Tag> Tags { get; protected set; }

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x0600639E RID: 25502 RVA: 0x000E0FD6 File Offset: 0x000DF1D6
	// (set) Token: 0x0600639F RID: 25503 RVA: 0x000E0FDE File Offset: 0x000DF1DE
	public FetchChore.MatchCriteria Criteria { get; protected set; }

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x060063A0 RID: 25504 RVA: 0x000E0FE7 File Offset: 0x000DF1E7
	// (set) Token: 0x060063A1 RID: 25505 RVA: 0x000E0FEF File Offset: 0x000DF1EF
	public Tag RequiredTag { get; protected set; }

	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x060063A2 RID: 25506 RVA: 0x000E0FF8 File Offset: 0x000DF1F8
	// (set) Token: 0x060063A3 RID: 25507 RVA: 0x000E1000 File Offset: 0x000DF200
	public Tag[] ForbiddenTags { get; protected set; }

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x060063A4 RID: 25508 RVA: 0x000E1009 File Offset: 0x000DF209
	// (set) Token: 0x060063A5 RID: 25509 RVA: 0x000E1011 File Offset: 0x000DF211
	public Storage Destination { get; set; }

	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x060063A6 RID: 25510 RVA: 0x000E101A File Offset: 0x000DF21A
	// (set) Token: 0x060063A7 RID: 25511 RVA: 0x000E1022 File Offset: 0x000DF222
	private float UnfetchedAmount
	{
		get
		{
			return this._UnfetchedAmount;
		}
		set
		{
			this._UnfetchedAmount = value;
			this.Assert(this._UnfetchedAmount <= this.TotalAmount, "_UnfetchedAmount <= TotalAmount");
			this.Assert(this._UnfetchedAmount >= 0f, "_UnfetchedAmount >= 0");
		}
	}

	// Token: 0x060063A8 RID: 25512 RVA: 0x002BC24C File Offset: 0x002BA44C
	public FetchOrder2(ChoreType chore_type, HashSet<Tag> tags, FetchChore.MatchCriteria criteria, Tag required_tag, Tag[] forbidden_tags, Storage destination, float amount, Operational.State operationalRequirementDEPRECATED = Operational.State.None, int priorityMod = 0)
	{
		if (amount <= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				string.Format("FetchOrder2 {0} is requesting {1} {2} to {3}", new object[]
				{
					chore_type.Id,
					tags,
					amount,
					(destination != null) ? destination.name : "to nowhere"
				})
			});
		}
		this.choreType = chore_type;
		this.Tags = tags;
		this.Criteria = criteria;
		this.RequiredTag = required_tag;
		this.ForbiddenTags = forbidden_tags;
		this.Destination = destination;
		this.TotalAmount = amount;
		this.UnfetchedAmount = amount;
		this.PriorityMod = priorityMod;
		this.operationalRequirement = operationalRequirementDEPRECATED;
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x060063A9 RID: 25513 RVA: 0x002BC318 File Offset: 0x002BA518
	public bool InProgress
	{
		get
		{
			bool result = false;
			using (List<FetchChore>.Enumerator enumerator = this.Chores.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.InProgress())
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
	}

	// Token: 0x060063AA RID: 25514 RVA: 0x000E1062 File Offset: 0x000DF262
	private void IssueTask()
	{
		if (this.UnfetchedAmount > 0f)
		{
			this.SetFetchTask(this.UnfetchedAmount);
			this.UnfetchedAmount = 0f;
		}
	}

	// Token: 0x060063AB RID: 25515 RVA: 0x002BC374 File Offset: 0x002BA574
	public void SetPriorityMod(int priorityMod)
	{
		this.PriorityMod = priorityMod;
		for (int i = 0; i < this.Chores.Count; i++)
		{
			this.Chores[i].SetPriorityMod(this.PriorityMod);
		}
	}

	// Token: 0x060063AC RID: 25516 RVA: 0x002BC3B8 File Offset: 0x002BA5B8
	private void SetFetchTask(float amount)
	{
		FetchChore fetchChore = new FetchChore(this.choreType, this.Destination, amount, this.Tags, this.Criteria, this.RequiredTag, this.ForbiddenTags, null, true, new Action<Chore>(this.OnFetchChoreComplete), new Action<Chore>(this.OnFetchChoreBegin), new Action<Chore>(this.OnFetchChoreEnd), this.operationalRequirement, this.PriorityMod);
		fetchChore.validateRequiredTagOnTagChange = this.validateRequiredTagOnTagChange;
		this.Chores.Add(fetchChore);
	}

	// Token: 0x060063AD RID: 25517 RVA: 0x002BC43C File Offset: 0x002BA63C
	private void OnFetchChoreEnd(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		if (this.Chores.Contains(fetchChore))
		{
			this.UnfetchedAmount += fetchChore.amount;
			fetchChore.Cancel("FetchChore Redistribution");
			this.Chores.Remove(fetchChore);
			this.IssueTask();
		}
	}

	// Token: 0x060063AE RID: 25518 RVA: 0x002BC490 File Offset: 0x002BA690
	private void OnFetchChoreComplete(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		this.Chores.Remove(fetchChore);
		if (this.Chores.Count == 0 && this.OnComplete != null)
		{
			this.OnComplete(this, fetchChore.fetchTarget);
		}
	}

	// Token: 0x060063AF RID: 25519 RVA: 0x002BC4D8 File Offset: 0x002BA6D8
	private void OnFetchChoreBegin(Chore chore)
	{
		FetchChore fetchChore = (FetchChore)chore;
		this.UnfetchedAmount += fetchChore.originalAmount - fetchChore.amount;
		this.IssueTask();
		if (this.OnBegin != null)
		{
			this.OnBegin(this, fetchChore.fetchTarget);
		}
	}

	// Token: 0x060063B0 RID: 25520 RVA: 0x002BC528 File Offset: 0x002BA728
	public void Cancel(string reason)
	{
		while (this.Chores.Count > 0)
		{
			FetchChore fetchChore = this.Chores[0];
			fetchChore.Cancel(reason);
			this.Chores.Remove(fetchChore);
		}
	}

	// Token: 0x060063B1 RID: 25521 RVA: 0x000E1088 File Offset: 0x000DF288
	public void Suspend(string reason)
	{
		global::Debug.LogError("UNIMPLEMENTED!");
	}

	// Token: 0x060063B2 RID: 25522 RVA: 0x000E1088 File Offset: 0x000DF288
	public void Resume(string reason)
	{
		global::Debug.LogError("UNIMPLEMENTED!");
	}

	// Token: 0x060063B3 RID: 25523 RVA: 0x002BC568 File Offset: 0x002BA768
	public void Submit(Action<FetchOrder2, Pickupable> on_complete, bool check_storage_contents, Action<FetchOrder2, Pickupable> on_begin = null)
	{
		this.OnComplete = on_complete;
		this.OnBegin = on_begin;
		this.checkStorageContents = check_storage_contents;
		if (check_storage_contents)
		{
			Pickupable arg = null;
			this.UnfetchedAmount = this.GetRemaining(out arg);
			if (this.UnfetchedAmount > this.Destination.storageFullMargin)
			{
				this.IssueTask();
				return;
			}
			if (this.OnComplete != null)
			{
				this.OnComplete(this, arg);
				return;
			}
		}
		else
		{
			this.IssueTask();
		}
	}

	// Token: 0x060063B4 RID: 25524 RVA: 0x002BC5D4 File Offset: 0x002BA7D4
	public bool IsMaterialOnStorage(Storage storage, ref float amount, ref Pickupable out_item)
	{
		foreach (GameObject gameObject in this.Destination.items)
		{
			if (gameObject != null)
			{
				Pickupable component = gameObject.GetComponent<Pickupable>();
				if (component != null)
				{
					KPrefabID kprefabID = component.KPrefabID;
					foreach (Tag tag in this.Tags)
					{
						if (kprefabID.HasTag(tag))
						{
							amount = component.TotalAmount;
							out_item = component;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060063B5 RID: 25525 RVA: 0x002BC6A0 File Offset: 0x002BA8A0
	public float AmountWaitingToFetch()
	{
		if (!this.checkStorageContents)
		{
			float num = this.UnfetchedAmount;
			for (int i = 0; i < this.Chores.Count; i++)
			{
				num += this.Chores[i].AmountWaitingToFetch();
			}
			return num;
		}
		Pickupable pickupable;
		return this.GetRemaining(out pickupable);
	}

	// Token: 0x060063B6 RID: 25526 RVA: 0x002BC6F0 File Offset: 0x002BA8F0
	public float GetRemaining(out Pickupable out_item)
	{
		float num = this.TotalAmount;
		float num2 = 0f;
		out_item = null;
		if (this.IsMaterialOnStorage(this.Destination, ref num2, ref out_item))
		{
			num = Math.Max(num - num2, 0f);
		}
		return num;
	}

	// Token: 0x060063B7 RID: 25527 RVA: 0x002BC730 File Offset: 0x002BA930
	public bool IsComplete()
	{
		for (int i = 0; i < this.Chores.Count; i++)
		{
			if (!this.Chores[i].isComplete)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060063B8 RID: 25528 RVA: 0x002BC76C File Offset: 0x002BA96C
	private void Assert(bool condition, string message)
	{
		if (condition)
		{
			return;
		}
		string text = "FetchOrder error: " + message;
		if (this.Destination == null)
		{
			text += "\nDestination: None";
		}
		else
		{
			text = text + "\nDestination: " + this.Destination.name;
		}
		text = text + "\nTotal Amount: " + this.TotalAmount.ToString();
		text = text + "\nUnfetched Amount: " + this._UnfetchedAmount.ToString();
		global::Debug.LogError(text);
	}

	// Token: 0x04004731 RID: 18225
	public Action<FetchOrder2, Pickupable> OnComplete;

	// Token: 0x04004732 RID: 18226
	public Action<FetchOrder2, Pickupable> OnBegin;

	// Token: 0x04004737 RID: 18231
	public bool validateRequiredTagOnTagChange;

	// Token: 0x0400473B RID: 18235
	public List<FetchChore> Chores = new List<FetchChore>();

	// Token: 0x0400473C RID: 18236
	private ChoreType choreType;

	// Token: 0x0400473D RID: 18237
	private float _UnfetchedAmount;

	// Token: 0x0400473E RID: 18238
	private bool checkStorageContents;

	// Token: 0x0400473F RID: 18239
	private Operational.State operationalRequirement = Operational.State.None;
}
