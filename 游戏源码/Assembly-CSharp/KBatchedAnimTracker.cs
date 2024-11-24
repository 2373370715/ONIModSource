using System;
using UnityEngine;

// Token: 0x02000925 RID: 2341
public class KBatchedAnimTracker : MonoBehaviour
{
	// Token: 0x06002A4D RID: 10829 RVA: 0x001D93B8 File Offset: 0x001D75B8
	private void Start()
	{
		if (this.controller == null)
		{
			Transform parent = base.transform.parent;
			while (parent != null)
			{
				this.controller = parent.GetComponent<KBatchedAnimController>();
				if (this.controller != null)
				{
					break;
				}
				parent = parent.parent;
			}
		}
		if (this.controller == null)
		{
			global::Debug.Log("Controller Null for tracker on " + base.gameObject.name, base.gameObject);
			base.enabled = false;
			return;
		}
		this.controller.onAnimEnter += this.OnAnimStart;
		this.controller.onAnimComplete += this.OnAnimStop;
		this.controller.onLayerChanged += this.OnLayerChanged;
		this.forceUpdate = true;
		if (this.myAnim != null)
		{
			return;
		}
		this.myAnim = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController kbatchedAnimController = this.myAnim;
		kbatchedAnimController.getPositionDataFunctionInUse = (Func<Vector4>)Delegate.Combine(kbatchedAnimController.getPositionDataFunctionInUse, new Func<Vector4>(this.MyAnimGetPosition));
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x001D94D0 File Offset: 0x001D76D0
	private Vector4 MyAnimGetPosition()
	{
		if (this.myAnim != null && this.controller != null && this.controller.transform == this.myAnim.transform.parent)
		{
			Vector3 pivotSymbolPosition = this.myAnim.GetPivotSymbolPosition();
			return new Vector4(pivotSymbolPosition.x - this.controller.Offset.x, pivotSymbolPosition.y - this.controller.Offset.y, pivotSymbolPosition.x, pivotSymbolPosition.y);
		}
		return base.transform.GetPosition();
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x001D9578 File Offset: 0x001D7778
	private void OnDestroy()
	{
		if (this.controller != null)
		{
			this.controller.onAnimEnter -= this.OnAnimStart;
			this.controller.onAnimComplete -= this.OnAnimStop;
			this.controller.onLayerChanged -= this.OnLayerChanged;
			this.controller = null;
		}
		if (this.myAnim != null)
		{
			KBatchedAnimController kbatchedAnimController = this.myAnim;
			kbatchedAnimController.getPositionDataFunctionInUse = (Func<Vector4>)Delegate.Remove(kbatchedAnimController.getPositionDataFunctionInUse, new Func<Vector4>(this.MyAnimGetPosition));
		}
		this.myAnim = null;
	}

	// Token: 0x06002A50 RID: 10832 RVA: 0x001D961C File Offset: 0x001D781C
	private void LateUpdate()
	{
		if (this.controller != null && (this.controller.IsVisible() || this.forceAlwaysVisible || this.forceUpdate))
		{
			this.UpdateFrame();
		}
		if (!this.alive)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06002A51 RID: 10833 RVA: 0x000BB881 File Offset: 0x000B9A81
	public void SetAnimControllers(KBatchedAnimController controller, KBatchedAnimController parentController)
	{
		this.myAnim = controller;
		this.controller = parentController;
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x001D966C File Offset: 0x001D786C
	private void UpdateFrame()
	{
		this.forceUpdate = false;
		bool flag = false;
		if (this.controller.CurrentAnim != null)
		{
			Matrix2x3 symbolLocalTransform = this.controller.GetSymbolLocalTransform(this.symbol, out flag);
			Vector3 position = this.controller.transform.GetPosition();
			if (flag && (this.previousMatrix != symbolLocalTransform || position != this.previousPosition || (this.useTargetPoint && this.targetPoint != this.previousTargetPoint) || (this.matchParentOffset && this.myAnim.Offset != this.controller.Offset)))
			{
				this.previousMatrix = symbolLocalTransform;
				this.previousPosition = position;
				Matrix2x3 overrideTransformMatrix = ((this.useTargetPoint || this.myAnim == null) ? this.controller.GetTransformMatrix() : this.controller.GetTransformMatrix(new Vector2(this.myAnim.animWidth * this.myAnim.animScale, -this.myAnim.animHeight * this.myAnim.animScale))) * symbolLocalTransform;
				float z = base.transform.GetPosition().z;
				base.transform.SetPosition(overrideTransformMatrix.MultiplyPoint(this.offset));
				if (this.useTargetPoint)
				{
					this.previousTargetPoint = this.targetPoint;
					Vector3 position2 = base.transform.GetPosition();
					position2.z = 0f;
					Vector3 vector = this.targetPoint - position2;
					float num = Vector3.Angle(vector, Vector3.right);
					if (vector.y < 0f)
					{
						num = 360f - num;
					}
					base.transform.localRotation = Quaternion.identity;
					base.transform.RotateAround(position2, new Vector3(0f, 0f, 1f), num);
					float sqrMagnitude = vector.sqrMagnitude;
					this.myAnim.GetBatchInstanceData().SetClipRadius(base.transform.GetPosition().x, base.transform.GetPosition().y, sqrMagnitude, true);
				}
				else
				{
					Vector3 v = this.controller.FlipX ? Vector3.left : Vector3.right;
					Vector3 v2 = this.controller.FlipY ? Vector3.down : Vector3.up;
					base.transform.up = overrideTransformMatrix.MultiplyVector(v2);
					base.transform.right = overrideTransformMatrix.MultiplyVector(v);
					if (this.myAnim != null)
					{
						KBatchedAnimInstanceData batchInstanceData = this.myAnim.GetBatchInstanceData();
						if (batchInstanceData != null)
						{
							batchInstanceData.SetOverrideTransformMatrix(overrideTransformMatrix);
						}
					}
				}
				base.transform.SetPosition(new Vector3(base.transform.GetPosition().x, base.transform.GetPosition().y, z));
				if (this.matchParentOffset)
				{
					this.myAnim.Offset = this.controller.Offset;
				}
				this.myAnim.SetDirty();
			}
		}
		if (this.myAnim != null && flag != this.myAnim.enabled && this.synchronizeEnabledState)
		{
			this.myAnim.enabled = flag;
		}
	}

	// Token: 0x06002A53 RID: 10835 RVA: 0x000BB891 File Offset: 0x000B9A91
	[ContextMenu("ForceAlive")]
	private void OnAnimStart(HashedString name)
	{
		this.alive = true;
		base.enabled = true;
		this.forceUpdate = true;
	}

	// Token: 0x06002A54 RID: 10836 RVA: 0x000BB8A8 File Offset: 0x000B9AA8
	private void OnAnimStop(HashedString name)
	{
		if (!this.forceAlwaysAlive)
		{
			this.alive = false;
		}
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x000BB8B9 File Offset: 0x000B9AB9
	private void OnLayerChanged(int layer)
	{
		this.myAnim.SetLayer(layer);
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x000BB8C7 File Offset: 0x000B9AC7
	public void SetTarget(Vector3 target)
	{
		this.targetPoint = target;
		this.targetPoint.z = 0f;
	}

	// Token: 0x04001C14 RID: 7188
	public KBatchedAnimController controller;

	// Token: 0x04001C15 RID: 7189
	public Vector3 offset = Vector3.zero;

	// Token: 0x04001C16 RID: 7190
	public HashedString symbol;

	// Token: 0x04001C17 RID: 7191
	public Vector3 targetPoint = Vector3.zero;

	// Token: 0x04001C18 RID: 7192
	public Vector3 previousTargetPoint;

	// Token: 0x04001C19 RID: 7193
	public bool useTargetPoint;

	// Token: 0x04001C1A RID: 7194
	public bool fadeOut = true;

	// Token: 0x04001C1B RID: 7195
	public bool forceAlwaysVisible;

	// Token: 0x04001C1C RID: 7196
	public bool matchParentOffset;

	// Token: 0x04001C1D RID: 7197
	public bool forceAlwaysAlive;

	// Token: 0x04001C1E RID: 7198
	private bool alive = true;

	// Token: 0x04001C1F RID: 7199
	private bool forceUpdate;

	// Token: 0x04001C20 RID: 7200
	private Matrix2x3 previousMatrix;

	// Token: 0x04001C21 RID: 7201
	private Vector3 previousPosition;

	// Token: 0x04001C22 RID: 7202
	public bool synchronizeEnabledState = true;

	// Token: 0x04001C23 RID: 7203
	[SerializeField]
	private KBatchedAnimController myAnim;
}
