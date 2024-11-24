using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001275 RID: 4725
[SerializationConfig(MemberSerialization.OptIn)]
public class EightDirectionUtil
{
	// Token: 0x060060CF RID: 24783 RVA: 0x000B7C34 File Offset: 0x000B5E34
	public static int GetDirectionIndex(EightDirection direction)
	{
		return (int)direction;
	}

	// Token: 0x060060D0 RID: 24784 RVA: 0x000DF2A4 File Offset: 0x000DD4A4
	public static EightDirection AngleToDirection(int angle)
	{
		return (EightDirection)Mathf.Floor((float)angle / 45f);
	}

	// Token: 0x060060D1 RID: 24785 RVA: 0x000DF2B4 File Offset: 0x000DD4B4
	public static Vector3 GetNormal(EightDirection direction)
	{
		return EightDirectionUtil.normals[EightDirectionUtil.GetDirectionIndex(direction)];
	}

	// Token: 0x060060D2 RID: 24786 RVA: 0x000DF2C6 File Offset: 0x000DD4C6
	public static float GetAngle(EightDirection direction)
	{
		return (float)(45 * EightDirectionUtil.GetDirectionIndex(direction));
	}

	// Token: 0x040044B5 RID: 17589
	public static readonly Vector3[] normals = new Vector3[]
	{
		Vector3.up,
		(Vector3.up + Vector3.left).normalized,
		Vector3.left,
		(Vector3.down + Vector3.left).normalized,
		Vector3.down,
		(Vector3.down + Vector3.right).normalized,
		Vector3.right,
		(Vector3.up + Vector3.right).normalized
	};
}
