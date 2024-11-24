using System;
using UnityEngine;

// Token: 0x02000986 RID: 2438
[AddComponentMenu("KMonoBehaviour/scripts/AnimEventHandler")]
public class AnimEventHandler : KMonoBehaviour
{
	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06002C2F RID: 11311 RVA: 0x001EB398 File Offset: 0x001E9598
	// (remove) Token: 0x06002C30 RID: 11312 RVA: 0x001EB3D0 File Offset: 0x001E95D0
	private event AnimEventHandler.SetPos onWorkTargetSet;

	// Token: 0x06002C31 RID: 11313 RVA: 0x000BC9E9 File Offset: 0x000BABE9
	public int GetCachedCell()
	{
		return this.pickupable.cachedCell;
	}

	// Token: 0x06002C32 RID: 11314 RVA: 0x001EB408 File Offset: 0x001E9608
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cachedTransform = base.transform;
		this.pickupable = base.GetComponent<Pickupable>();
		foreach (KBatchedAnimTracker kbatchedAnimTracker in base.GetComponentsInChildren<KBatchedAnimTracker>(true))
		{
			if (kbatchedAnimTracker.useTargetPoint)
			{
				this.onWorkTargetSet += kbatchedAnimTracker.SetTarget;
			}
		}
		this.baseOffset = this.animCollider.offset;
		AnimEventHandlerManager.Instance.Add(this);
	}

	// Token: 0x06002C33 RID: 11315 RVA: 0x000BC9F6 File Offset: 0x000BABF6
	protected override void OnCleanUp()
	{
		AnimEventHandlerManager.Instance.Remove(this);
	}

	// Token: 0x06002C34 RID: 11316 RVA: 0x000BCA03 File Offset: 0x000BAC03
	protected override void OnForcedCleanUp()
	{
		this.navigator = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06002C35 RID: 11317 RVA: 0x000BCA12 File Offset: 0x000BAC12
	public HashedString GetContext()
	{
		return this.context;
	}

	// Token: 0x06002C36 RID: 11318 RVA: 0x000BCA1A File Offset: 0x000BAC1A
	public void UpdateWorkTarget(Vector3 pos)
	{
		if (this.onWorkTargetSet != null)
		{
			this.onWorkTargetSet(pos);
		}
	}

	// Token: 0x06002C37 RID: 11319 RVA: 0x000BCA30 File Offset: 0x000BAC30
	public void SetContext(HashedString context)
	{
		this.context = context;
	}

	// Token: 0x06002C38 RID: 11320 RVA: 0x000BCA39 File Offset: 0x000BAC39
	public void SetTargetPos(Vector3 target_pos)
	{
		this.targetPos = target_pos;
	}

	// Token: 0x06002C39 RID: 11321 RVA: 0x000BCA42 File Offset: 0x000BAC42
	public Vector3 GetTargetPos()
	{
		return this.targetPos;
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x000BCA4A File Offset: 0x000BAC4A
	public void ClearContext()
	{
		this.context = default(HashedString);
	}

	// Token: 0x06002C3B RID: 11323 RVA: 0x001EB484 File Offset: 0x001E9684
	public void UpdateOffset()
	{
		Vector3 pivotSymbolPosition = this.controller.GetPivotSymbolPosition();
		Vector3 vector = this.navigator.NavGrid.GetNavTypeData(this.navigator.CurrentNavType).animControllerOffset;
		Vector3 position = this.cachedTransform.position;
		Vector2 vector2 = new Vector2(this.baseOffset.x + pivotSymbolPosition.x - position.x - vector.x, this.baseOffset.y + pivotSymbolPosition.y - position.y + vector.y);
		if (this.animCollider.offset != vector2)
		{
			this.animCollider.offset = vector2;
		}
	}

	// Token: 0x04001DB0 RID: 7600
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x04001DB1 RID: 7601
	[MyCmpGet]
	private KBoxCollider2D animCollider;

	// Token: 0x04001DB2 RID: 7602
	[MyCmpGet]
	private Navigator navigator;

	// Token: 0x04001DB3 RID: 7603
	private Pickupable pickupable;

	// Token: 0x04001DB4 RID: 7604
	private Vector3 targetPos;

	// Token: 0x04001DB5 RID: 7605
	public Transform cachedTransform;

	// Token: 0x04001DB7 RID: 7607
	public Vector2 baseOffset;

	// Token: 0x04001DB8 RID: 7608
	private HashedString context;

	// Token: 0x02000987 RID: 2439
	// (Invoke) Token: 0x06002C3E RID: 11326
	private delegate void SetPos(Vector3 pos);
}
