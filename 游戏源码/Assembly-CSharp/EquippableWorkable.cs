using System;
using KSerialization;
using UnityEngine;

// Token: 0x020012BB RID: 4795
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/EquippableWorkable")]
public class EquippableWorkable : Workable, ISaveLoadable
{
	// Token: 0x0600628C RID: 25228 RVA: 0x002B6DD8 File Offset: 0x002B4FD8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Equipping;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_equip_clothing_kanim")
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x0600628D RID: 25229 RVA: 0x000E05B5 File Offset: 0x000DE7B5
	public global::QualityLevel GetQuality()
	{
		return this.quality;
	}

	// Token: 0x0600628E RID: 25230 RVA: 0x000E05BD File Offset: 0x000DE7BD
	public void SetQuality(global::QualityLevel level)
	{
		this.quality = level;
	}

	// Token: 0x0600628F RID: 25231 RVA: 0x000E05C6 File Offset: 0x000DE7C6
	protected override void OnSpawn()
	{
		base.SetWorkTime(1.5f);
		this.equippable.OnAssign += this.RefreshChore;
	}

	// Token: 0x06006290 RID: 25232 RVA: 0x002B6E28 File Offset: 0x002B5028
	private void CreateChore()
	{
		global::Debug.Assert(this.chore == null, "chore should be null");
		this.chore = new EquipChore(this);
		Chore chore = this.chore;
		chore.onExit = (Action<Chore>)Delegate.Combine(chore.onExit, new Action<Chore>(this.OnChoreExit));
	}

	// Token: 0x06006291 RID: 25233 RVA: 0x000E05EA File Offset: 0x000DE7EA
	private void OnChoreExit(Chore chore)
	{
		if (!chore.isComplete)
		{
			this.RefreshChore(this.currentTarget);
		}
	}

	// Token: 0x06006292 RID: 25234 RVA: 0x000E0600 File Offset: 0x000DE800
	public void CancelChore(string reason = "")
	{
		if (this.chore != null)
		{
			this.chore.Cancel(reason);
			Prioritizable.RemoveRef(this.equippable.gameObject);
			this.chore = null;
		}
	}

	// Token: 0x06006293 RID: 25235 RVA: 0x000E062D File Offset: 0x000DE82D
	private void RefreshChore(IAssignableIdentity target)
	{
		if (this.chore != null)
		{
			this.CancelChore("Equipment Reassigned");
		}
		this.currentTarget = target;
		if (target != null && !target.GetSoleOwner().GetComponent<Equipment>().IsEquipped(this.equippable))
		{
			this.CreateChore();
		}
	}

	// Token: 0x06006294 RID: 25236 RVA: 0x002B6E7C File Offset: 0x002B507C
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.equippable.assignee != null)
		{
			Ownables soleOwner = this.equippable.assignee.GetSoleOwner();
			if (soleOwner)
			{
				soleOwner.GetComponent<Equipment>().Equip(this.equippable);
				Prioritizable.RemoveRef(this.equippable.gameObject);
				this.chore = null;
			}
		}
	}

	// Token: 0x06006295 RID: 25237 RVA: 0x000E066A File Offset: 0x000DE86A
	protected override void OnStopWork(WorkerBase worker)
	{
		this.workTimeRemaining = this.GetWorkTime();
		base.OnStopWork(worker);
	}

	// Token: 0x04004625 RID: 17957
	[MyCmpReq]
	private Equippable equippable;

	// Token: 0x04004626 RID: 17958
	private Chore chore;

	// Token: 0x04004627 RID: 17959
	private IAssignableIdentity currentTarget;

	// Token: 0x04004628 RID: 17960
	private global::QualityLevel quality;
}
