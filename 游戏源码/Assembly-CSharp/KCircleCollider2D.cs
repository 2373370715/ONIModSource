using System;
using UnityEngine;

// Token: 0x02000A73 RID: 2675
public class KCircleCollider2D : KCollider2D
{
	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06003148 RID: 12616 RVA: 0x000BFFD9 File Offset: 0x000BE1D9
	// (set) Token: 0x06003149 RID: 12617 RVA: 0x000BFFE1 File Offset: 0x000BE1E1
	public float radius
	{
		get
		{
			return this._radius;
		}
		set
		{
			this._radius = value;
			base.MarkDirty(false);
		}
	}

	// Token: 0x0600314A RID: 12618 RVA: 0x001FEB38 File Offset: 0x001FCD38
	public override Extents GetExtents()
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = new Vector2(vector.x - this.radius, vector.y - this.radius);
		Vector2 vector3 = new Vector2(vector.x + this.radius, vector.y + this.radius);
		int width = (int)vector3.x - (int)vector2.x + 1;
		int height = (int)vector3.y - (int)vector2.y + 1;
		return new Extents((int)(vector.x - this._radius), (int)(vector.y - this._radius), width, height);
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x0600314B RID: 12619 RVA: 0x001FEBFC File Offset: 0x001FCDFC
	public override Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f), new Vector3(this._radius * 2f, this._radius * 2f, 0f));
		}
	}

	// Token: 0x0600314C RID: 12620 RVA: 0x001FEC60 File Offset: 0x001FCE60
	public override bool Intersects(Vector2 pos)
	{
		Vector3 position = base.transform.GetPosition();
		Vector2 b = new Vector2(position.x, position.y) + base.offset;
		return (pos - b).sqrMagnitude <= this._radius * this._radius;
	}

	// Token: 0x0600314D RID: 12621 RVA: 0x001FECB8 File Offset: 0x001FCEB8
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.bounds.center, this.radius);
	}

	// Token: 0x04002139 RID: 8505
	[SerializeField]
	private float _radius;
}
