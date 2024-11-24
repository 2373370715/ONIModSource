using System;
using UnityEngine;

// Token: 0x02000A72 RID: 2674
public class KBoxCollider2D : KCollider2D
{
	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06003141 RID: 12609 RVA: 0x000BFFB9 File Offset: 0x000BE1B9
	// (set) Token: 0x06003142 RID: 12610 RVA: 0x000BFFC1 File Offset: 0x000BE1C1
	public Vector2 size
	{
		get
		{
			return this._size;
		}
		set
		{
			this._size = value;
			base.MarkDirty(false);
		}
	}

	// Token: 0x06003143 RID: 12611 RVA: 0x001FE890 File Offset: 0x001FCA90
	public override Extents GetExtents()
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = this.size * 0.9999f;
		Vector2 vector3 = new Vector2(vector.x - vector2.x * 0.5f, vector.y - vector2.y * 0.5f);
		Vector2 vector4 = new Vector2(vector.x + vector2.x * 0.5f, vector.y + vector2.y * 0.5f);
		Vector2I vector2I = new Vector2I((int)vector3.x, (int)vector3.y);
		Vector2I vector2I2 = new Vector2I((int)vector4.x, (int)vector4.y);
		int width = vector2I2.x - vector2I.x + 1;
		int height = vector2I2.y - vector2I.y + 1;
		return new Extents(vector2I.x, vector2I.y, width, height);
	}

	// Token: 0x06003144 RID: 12612 RVA: 0x001FE99C File Offset: 0x001FCB9C
	public override bool Intersects(Vector2 intersect_pos)
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = new Vector2(vector.x - this.size.x * 0.5f, vector.y - this.size.y * 0.5f);
		Vector2 vector3 = new Vector2(vector.x + this.size.x * 0.5f, vector.y + this.size.y * 0.5f);
		return intersect_pos.x >= vector2.x && intersect_pos.x <= vector3.x && intersect_pos.y >= vector2.y && intersect_pos.y <= vector3.y;
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06003145 RID: 12613 RVA: 0x001FEA88 File Offset: 0x001FCC88
	public override Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f), new Vector3(this._size.x, this._size.y, 0f));
		}
	}

	// Token: 0x06003146 RID: 12614 RVA: 0x001FEAEC File Offset: 0x001FCCEC
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(this.bounds.center, new Vector3(this._size.x, this._size.y, 0f));
	}

	// Token: 0x04002138 RID: 8504
	[SerializeField]
	private Vector2 _size;
}
