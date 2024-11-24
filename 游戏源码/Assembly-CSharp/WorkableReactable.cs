using System;
using UnityEngine;

// Token: 0x02000813 RID: 2067
public class WorkableReactable : Reactable
{
	// Token: 0x06002509 RID: 9481 RVA: 0x001CB620 File Offset: 0x001C9820
	public WorkableReactable(Workable workable, HashedString id, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable.gameObject, id, chore_type, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
	{
		this.workable = workable;
		this.allowedDirection = allowed_direction;
	}

	// Token: 0x0600250A RID: 9482 RVA: 0x001CB664 File Offset: 0x001C9864
	public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
	{
		if (this.workable == null)
		{
			return false;
		}
		if (this.reactor != null)
		{
			return false;
		}
		Brain component = new_reactor.GetComponent<Brain>();
		if (component == null)
		{
			return false;
		}
		if (!component.IsRunning())
		{
			return false;
		}
		Navigator component2 = new_reactor.GetComponent<Navigator>();
		if (component2 == null)
		{
			return false;
		}
		if (!component2.IsMoving())
		{
			return false;
		}
		if (this.allowedDirection == WorkableReactable.AllowedDirection.Any)
		{
			return true;
		}
		Facing component3 = new_reactor.GetComponent<Facing>();
		if (component3 == null)
		{
			return false;
		}
		bool facing = component3.GetFacing();
		return (!facing || this.allowedDirection != WorkableReactable.AllowedDirection.Right) && (facing || this.allowedDirection != WorkableReactable.AllowedDirection.Left);
	}

	// Token: 0x0600250B RID: 9483 RVA: 0x000B8220 File Offset: 0x000B6420
	protected override void InternalBegin()
	{
		this.worker = this.reactor.GetComponent<WorkerBase>();
		this.worker.StartWork(new WorkerBase.StartWorkInfo(this.workable));
	}

	// Token: 0x0600250C RID: 9484 RVA: 0x000B8249 File Offset: 0x000B6449
	public override void Update(float dt)
	{
		if (this.worker.GetWorkable() == null)
		{
			base.End();
			return;
		}
		if (this.worker.Work(dt) != WorkerBase.WorkResult.InProgress)
		{
			base.End();
		}
	}

	// Token: 0x0600250D RID: 9485 RVA: 0x000B827A File Offset: 0x000B647A
	protected override void InternalEnd()
	{
		if (this.worker != null)
		{
			this.worker.StopWork();
		}
	}

	// Token: 0x0600250E RID: 9486 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void InternalCleanup()
	{
	}

	// Token: 0x0400190E RID: 6414
	protected Workable workable;

	// Token: 0x0400190F RID: 6415
	private WorkerBase worker;

	// Token: 0x04001910 RID: 6416
	public WorkableReactable.AllowedDirection allowedDirection;

	// Token: 0x02000814 RID: 2068
	public enum AllowedDirection
	{
		// Token: 0x04001912 RID: 6418
		Any,
		// Token: 0x04001913 RID: 6419
		Left,
		// Token: 0x04001914 RID: 6420
		Right
	}
}
