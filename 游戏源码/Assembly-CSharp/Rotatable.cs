using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000AF3 RID: 2803
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Rotatable")]
public class Rotatable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x1700023C RID: 572
	// (get) Token: 0x0600347E RID: 13438 RVA: 0x000C2373 File Offset: 0x000C0573
	public Orientation Orientation
	{
		get
		{
			return this.orientation;
		}
	}

	// Token: 0x0600347F RID: 13439 RVA: 0x0020A2E8 File Offset: 0x002084E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			this.SetSize(component.Def.WidthInCells, component.Def.HeightInCells);
		}
		this.OrientVisualizer(this.orientation);
		this.OrientCollider(this.orientation);
	}

	// Token: 0x06003480 RID: 13440 RVA: 0x0020A340 File Offset: 0x00208540
	public void SetSize(int width, int height)
	{
		this.width = width;
		this.height = height;
		if (width % 2 == 0)
		{
			this.pivot = new Vector3(-0.5f, 0.5f, 0f);
			this.visualizerOffset = new Vector3(0.5f, 0f, 0f);
			return;
		}
		this.pivot = new Vector3(0f, 0.5f, 0f);
		this.visualizerOffset = Vector3.zero;
	}

	// Token: 0x06003481 RID: 13441 RVA: 0x0020A3C0 File Offset: 0x002085C0
	public Orientation Rotate()
	{
		switch (this.permittedRotations)
		{
		case PermittedRotations.R90:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.R90 : Orientation.Neutral);
			break;
		case PermittedRotations.R360:
			this.orientation = (this.orientation + 1) % Orientation.NumRotations;
			break;
		case PermittedRotations.FlipH:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.FlipH : Orientation.Neutral);
			break;
		case PermittedRotations.FlipV:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.FlipV : Orientation.Neutral);
			break;
		}
		this.OrientVisualizer(this.orientation);
		return this.orientation;
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x000C237B File Offset: 0x000C057B
	public void SetOrientation(Orientation new_orientation)
	{
		this.orientation = new_orientation;
		this.OrientVisualizer(new_orientation);
		this.OrientCollider(new_orientation);
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x0020A44C File Offset: 0x0020864C
	public void Match(Rotatable other)
	{
		this.pivot = other.pivot;
		this.visualizerOffset = other.visualizerOffset;
		this.permittedRotations = other.permittedRotations;
		this.orientation = other.orientation;
		this.OrientVisualizer(this.orientation);
		this.OrientCollider(this.orientation);
	}

	// Token: 0x06003484 RID: 13444 RVA: 0x0020A4A4 File Offset: 0x002086A4
	public float GetVisualizerRotation()
	{
		PermittedRotations permittedRotations = this.permittedRotations;
		if (permittedRotations - PermittedRotations.R90 <= 1)
		{
			return -90f * (float)this.orientation;
		}
		return 0f;
	}

	// Token: 0x06003485 RID: 13445 RVA: 0x000C2392 File Offset: 0x000C0592
	public bool GetVisualizerFlipX()
	{
		return this.orientation == Orientation.FlipH;
	}

	// Token: 0x06003486 RID: 13446 RVA: 0x000C239D File Offset: 0x000C059D
	public bool GetVisualizerFlipY()
	{
		return this.orientation == Orientation.FlipV;
	}

	// Token: 0x06003487 RID: 13447 RVA: 0x0020A4D4 File Offset: 0x002086D4
	public Vector3 GetVisualizerPivot()
	{
		Vector3 result = this.pivot;
		Orientation orientation = this.orientation;
		if (orientation != Orientation.FlipH)
		{
			if (orientation != Orientation.FlipV)
			{
			}
		}
		else
		{
			result.x = -this.pivot.x;
		}
		return result;
	}

	// Token: 0x06003488 RID: 13448 RVA: 0x0020A510 File Offset: 0x00208710
	private Vector3 GetVisualizerOffset()
	{
		Orientation orientation = this.orientation;
		Vector3 result;
		if (orientation != Orientation.FlipH)
		{
			if (orientation != Orientation.FlipV)
			{
				result = this.visualizerOffset;
			}
			else
			{
				result = new Vector3(this.visualizerOffset.x, 1f, this.visualizerOffset.z);
			}
		}
		else
		{
			result = new Vector3(-this.visualizerOffset.x, this.visualizerOffset.y, this.visualizerOffset.z);
		}
		return result;
	}

	// Token: 0x06003489 RID: 13449 RVA: 0x0020A584 File Offset: 0x00208784
	private void OrientVisualizer(Orientation orientation)
	{
		float visualizerRotation = this.GetVisualizerRotation();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Pivot = this.GetVisualizerPivot();
		component.Rotation = visualizerRotation;
		component.Offset = this.GetVisualizerOffset();
		component.FlipX = this.GetVisualizerFlipX();
		component.FlipY = this.GetVisualizerFlipY();
		base.Trigger(-1643076535, this);
	}

	// Token: 0x0600348A RID: 13450 RVA: 0x0020A5E0 File Offset: 0x002087E0
	private void OrientCollider(Orientation orientation)
	{
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component == null)
		{
			return;
		}
		float num = 0.5f * (float)((this.width + 1) % 2);
		float num2 = 0f;
		switch (orientation)
		{
		case Orientation.R90:
			num2 = -90f;
			goto IL_11B;
		case Orientation.R180:
			num2 = -180f;
			goto IL_11B;
		case Orientation.R270:
			num2 = -270f;
			goto IL_11B;
		case Orientation.FlipH:
			component.offset = new Vector2(num + (float)(this.width % 2) - 1f, 0.5f * (float)this.height);
			component.size = new Vector2((float)this.width, (float)this.height);
			goto IL_11B;
		case Orientation.FlipV:
			component.offset = new Vector2(num, -0.5f * (float)(this.height - 2));
			component.size = new Vector2((float)this.width, (float)this.height);
			goto IL_11B;
		}
		component.offset = new Vector2(num, 0.5f * (float)this.height);
		component.size = new Vector2((float)this.width, (float)this.height);
		IL_11B:
		if (num2 != 0f)
		{
			Matrix2x3 n = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n2 = Matrix2x3.Rotate(num2 * 0.017453292f);
			Matrix2x3 matrix2x = Matrix2x3.Translate(this.pivot + new Vector3(num, 0f, 0f)) * n2 * n;
			Vector2 vector = new Vector2(-0.5f * (float)this.width, 0f);
			Vector2 vector2 = new Vector2(0.5f * (float)this.width, (float)this.height);
			Vector2 vector3 = new Vector2(0f, 0.5f * (float)this.height);
			vector = matrix2x.MultiplyPoint(vector);
			vector2 = matrix2x.MultiplyPoint(vector2);
			vector3 = matrix2x.MultiplyPoint(vector3);
			float num3 = Mathf.Min(vector.x, vector2.x);
			float num4 = Mathf.Max(vector.x, vector2.x);
			float num5 = Mathf.Min(vector.y, vector2.y);
			float num6 = Mathf.Max(vector.y, vector2.y);
			component.offset = vector3;
			component.size = new Vector2(num4 - num3, num6 - num5);
		}
	}

	// Token: 0x0600348B RID: 13451 RVA: 0x000C23A8 File Offset: 0x000C05A8
	public CellOffset GetRotatedCellOffset(CellOffset offset)
	{
		return Rotatable.GetRotatedCellOffset(offset, this.orientation);
	}

	// Token: 0x0600348C RID: 13452 RVA: 0x0020A868 File Offset: 0x00208A68
	public static CellOffset GetRotatedCellOffset(CellOffset offset, Orientation orientation)
	{
		switch (orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new CellOffset(offset.y, -offset.x);
		case Orientation.R180:
			return new CellOffset(-offset.x, -offset.y);
		case Orientation.R270:
			return new CellOffset(-offset.y, offset.x);
		case Orientation.FlipH:
			return new CellOffset(-offset.x, offset.y);
		case Orientation.FlipV:
			return new CellOffset(offset.x, -offset.y);
		}
	}

	// Token: 0x0600348D RID: 13453 RVA: 0x000C23B6 File Offset: 0x000C05B6
	public static CellOffset GetRotatedCellOffset(int x, int y, Orientation orientation)
	{
		return Rotatable.GetRotatedCellOffset(new CellOffset(x, y), orientation);
	}

	// Token: 0x0600348E RID: 13454 RVA: 0x000C23C5 File Offset: 0x000C05C5
	public Vector3 GetRotatedOffset(Vector3 offset)
	{
		return Rotatable.GetRotatedOffset(offset, this.orientation);
	}

	// Token: 0x0600348F RID: 13455 RVA: 0x0020A8F8 File Offset: 0x00208AF8
	public static Vector3 GetRotatedOffset(Vector3 offset, Orientation orientation)
	{
		switch (orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new Vector3(offset.y, -offset.x);
		case Orientation.R180:
			return new Vector3(-offset.x, -offset.y);
		case Orientation.R270:
			return new Vector3(-offset.y, offset.x);
		case Orientation.FlipH:
			return new Vector3(-offset.x, offset.y);
		case Orientation.FlipV:
			return new Vector3(offset.x, -offset.y);
		}
	}

	// Token: 0x06003490 RID: 13456 RVA: 0x0020A988 File Offset: 0x00208B88
	public Vector2I GetRotatedOffset(Vector2I offset)
	{
		switch (this.orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new Vector2I(offset.y, -offset.x);
		case Orientation.R180:
			return new Vector2I(-offset.x, -offset.y);
		case Orientation.R270:
			return new Vector2I(-offset.y, offset.x);
		case Orientation.FlipH:
			return new Vector2I(-offset.x, offset.y);
		case Orientation.FlipV:
			return new Vector2I(offset.x, -offset.y);
		}
	}

	// Token: 0x06003491 RID: 13457 RVA: 0x000C2373 File Offset: 0x000C0573
	public Orientation GetOrientation()
	{
		return this.orientation;
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06003492 RID: 13458 RVA: 0x000C23D3 File Offset: 0x000C05D3
	public bool IsRotated
	{
		get
		{
			return this.orientation > Orientation.Neutral;
		}
	}

	// Token: 0x0400238D RID: 9101
	[Serialize]
	[SerializeField]
	private Orientation orientation;

	// Token: 0x0400238E RID: 9102
	[SerializeField]
	private Vector3 pivot = Vector3.zero;

	// Token: 0x0400238F RID: 9103
	[SerializeField]
	private Vector3 visualizerOffset = Vector3.zero;

	// Token: 0x04002390 RID: 9104
	public PermittedRotations permittedRotations;

	// Token: 0x04002391 RID: 9105
	[SerializeField]
	private int width;

	// Token: 0x04002392 RID: 9106
	[SerializeField]
	private int height;
}
