using System;
using UnityEngine;

// Token: 0x020012DD RID: 4829
public struct FallerComponent
{
	// Token: 0x0600631B RID: 25371 RVA: 0x002B8B28 File Offset: 0x002B6D28
	public FallerComponent(Transform transform, Vector2 initial_velocity)
	{
		this.transform = transform;
		this.transformInstanceId = transform.GetInstanceID();
		this.isFalling = false;
		this.initialVelocity = initial_velocity;
		this.partitionerEntry = default(HandleVector<int>.Handle);
		this.solidChangedCB = null;
		this.cellChangedCB = null;
		KCircleCollider2D component = transform.GetComponent<KCircleCollider2D>();
		if (component != null)
		{
			this.offset = component.radius;
			return;
		}
		KCollider2D component2 = transform.GetComponent<KCollider2D>();
		if (component2 != null)
		{
			this.offset = transform.GetPosition().y - component2.bounds.min.y;
			return;
		}
		this.offset = 0f;
	}

	// Token: 0x040046B6 RID: 18102
	public Transform transform;

	// Token: 0x040046B7 RID: 18103
	public int transformInstanceId;

	// Token: 0x040046B8 RID: 18104
	public bool isFalling;

	// Token: 0x040046B9 RID: 18105
	public float offset;

	// Token: 0x040046BA RID: 18106
	public Vector2 initialVelocity;

	// Token: 0x040046BB RID: 18107
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040046BC RID: 18108
	public Action<object> solidChangedCB;

	// Token: 0x040046BD RID: 18109
	public System.Action cellChangedCB;
}
