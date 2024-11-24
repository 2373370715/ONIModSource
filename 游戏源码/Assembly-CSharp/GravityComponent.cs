using System;
using UnityEngine;

// Token: 0x0200139A RID: 5018
public struct GravityComponent
{
	// Token: 0x06006733 RID: 26419 RVA: 0x002D3B0C File Offset: 0x002D1D0C
	public GravityComponent(Transform transform, System.Action on_landed, Vector2 initial_velocity, bool land_on_fake_floors, bool mayLeaveWorld)
	{
		this.transform = transform;
		this.elapsedTime = 0f;
		this.velocity = initial_velocity;
		this.onLanded = on_landed;
		this.landOnFakeFloors = land_on_fake_floors;
		this.mayLeaveWorld = mayLeaveWorld;
		this.collider2D = transform.GetComponent<KCollider2D>();
		this.extents = GravityComponent.GetExtents(this.collider2D);
	}

	// Token: 0x06006734 RID: 26420 RVA: 0x002D3B68 File Offset: 0x002D1D68
	public static float GetGroundOffset(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.bounds.extents.y - collider.offset.y;
		}
		return 0f;
	}

	// Token: 0x06006735 RID: 26421 RVA: 0x000E37B4 File Offset: 0x000E19B4
	public static float GetGroundOffset(GravityComponent gravityComponent)
	{
		if (gravityComponent.collider2D != null)
		{
			return gravityComponent.extents.y - gravityComponent.collider2D.offset.y;
		}
		return 0f;
	}

	// Token: 0x06006736 RID: 26422 RVA: 0x002D3BA4 File Offset: 0x002D1DA4
	public static Vector2 GetExtents(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.bounds.extents;
		}
		return Vector2.zero;
	}

	// Token: 0x06006737 RID: 26423 RVA: 0x000E37E6 File Offset: 0x000E19E6
	public static Vector2 GetOffset(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.offset;
		}
		return Vector2.zero;
	}

	// Token: 0x04004D85 RID: 19845
	public Transform transform;

	// Token: 0x04004D86 RID: 19846
	public Vector2 velocity;

	// Token: 0x04004D87 RID: 19847
	public float elapsedTime;

	// Token: 0x04004D88 RID: 19848
	public System.Action onLanded;

	// Token: 0x04004D89 RID: 19849
	public bool landOnFakeFloors;

	// Token: 0x04004D8A RID: 19850
	public bool mayLeaveWorld;

	// Token: 0x04004D8B RID: 19851
	public Vector2 extents;

	// Token: 0x04004D8C RID: 19852
	public KCollider2D collider2D;
}
