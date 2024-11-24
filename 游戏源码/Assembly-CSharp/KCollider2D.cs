using System;
using UnityEngine;

// Token: 0x02000A74 RID: 2676
public abstract class KCollider2D : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x170001FA RID: 506
	// (get) Token: 0x0600314F RID: 12623 RVA: 0x000BFFF1 File Offset: 0x000BE1F1
	// (set) Token: 0x06003150 RID: 12624 RVA: 0x000BFFF9 File Offset: 0x000BE1F9
	public Vector2 offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			this._offset = value;
			this.MarkDirty(false);
		}
	}

	// Token: 0x06003151 RID: 12625 RVA: 0x000C0009 File Offset: 0x000BE209
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoRegisterSimRender = false;
	}

	// Token: 0x06003152 RID: 12626 RVA: 0x000C0018 File Offset: 0x000BE218
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(base.transform, new Action<Transform, bool>(KCollider2D.OnMovementStateChanged));
		this.MarkDirty(true);
	}

	// Token: 0x06003153 RID: 12627 RVA: 0x000C0043 File Offset: 0x000BE243
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(base.transform, new Action<Transform, bool>(KCollider2D.OnMovementStateChanged));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06003154 RID: 12628 RVA: 0x001FECE8 File Offset: 0x001FCEE8
	public void MarkDirty(bool force = false)
	{
		bool flag = force || this.partitionerEntry.IsValid();
		if (!flag)
		{
			return;
		}
		Extents extents = this.GetExtents();
		if (!force && this.cachedExtents.x == extents.x && this.cachedExtents.y == extents.y && this.cachedExtents.width == extents.width && this.cachedExtents.height == extents.height)
		{
			return;
		}
		this.cachedExtents = extents;
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (flag)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add(base.name, this, this.cachedExtents, GameScenePartitioner.Instance.collisionLayer, null);
		}
	}

	// Token: 0x06003155 RID: 12629 RVA: 0x000C0077 File Offset: 0x000BE277
	private void OnMovementStateChanged(bool is_moving)
	{
		if (is_moving)
		{
			this.MarkDirty(false);
			SimAndRenderScheduler.instance.Add(this, false);
			return;
		}
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06003156 RID: 12630 RVA: 0x000C009B File Offset: 0x000BE29B
	private static void OnMovementStateChanged(Transform transform, bool is_moving)
	{
		transform.GetComponent<KCollider2D>().OnMovementStateChanged(is_moving);
	}

	// Token: 0x06003157 RID: 12631 RVA: 0x000C00A9 File Offset: 0x000BE2A9
	public void RenderEveryTick(float dt)
	{
		this.MarkDirty(false);
	}

	// Token: 0x06003158 RID: 12632
	public abstract bool Intersects(Vector2 pos);

	// Token: 0x06003159 RID: 12633
	public abstract Extents GetExtents();

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x0600315A RID: 12634
	public abstract Bounds bounds { get; }

	// Token: 0x0400213A RID: 8506
	[SerializeField]
	public Vector2 _offset;

	// Token: 0x0400213B RID: 8507
	private Extents cachedExtents;

	// Token: 0x0400213C RID: 8508
	private HandleVector<int>.Handle partitionerEntry;
}
