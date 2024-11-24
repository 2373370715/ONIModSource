using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001875 RID: 6261
[AddComponentMenu("KMonoBehaviour/scripts/SituationalAnim")]
public class SituationalAnim : KMonoBehaviour
{
	// Token: 0x06008194 RID: 33172 RVA: 0x00339DF0 File Offset: 0x00337FF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SituationalAnim.Situation situation = this.GetSituation();
		DebugUtil.LogArgs(new object[]
		{
			"Situation is",
			situation
		});
		this.SetAnimForSituation(situation);
	}

	// Token: 0x06008195 RID: 33173 RVA: 0x00339E30 File Offset: 0x00338030
	private void SetAnimForSituation(SituationalAnim.Situation situation)
	{
		foreach (global::Tuple<SituationalAnim.Situation, string> tuple in this.anims)
		{
			if ((tuple.first & situation) == tuple.first)
			{
				DebugUtil.LogArgs(new object[]
				{
					"Chose Anim",
					tuple.first,
					tuple.second
				});
				this.SetAnim(tuple.second);
				break;
			}
		}
	}

	// Token: 0x06008196 RID: 33174 RVA: 0x000F5376 File Offset: 0x000F3576
	private void SetAnim(string animName)
	{
		base.GetComponent<KBatchedAnimController>().Play(animName, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06008197 RID: 33175 RVA: 0x00339EC4 File Offset: 0x003380C4
	private SituationalAnim.Situation GetSituation()
	{
		SituationalAnim.Situation situation = (SituationalAnim.Situation)0;
		Extents extents = base.GetComponent<Building>().GetExtents();
		int x = extents.x;
		int num = extents.x + extents.width - 1;
		int y = extents.y;
		int num2 = extents.y + extents.height - 1;
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x, num, y - 1, y - 1), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Bottom;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x - 1, x - 1, y, num2), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Left;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x, num, num2 + 1, num2 + 1), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Top;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(num + 1, num + 1, y, num2), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Right;
		}
		return situation;
	}

	// Token: 0x06008198 RID: 33176 RVA: 0x000F5394 File Offset: 0x000F3594
	private bool DoesSatisfy(SituationalAnim.MustSatisfy result, SituationalAnim.MustSatisfy requirement)
	{
		if (requirement == SituationalAnim.MustSatisfy.All)
		{
			return result == SituationalAnim.MustSatisfy.All;
		}
		if (requirement == SituationalAnim.MustSatisfy.Any)
		{
			return result > SituationalAnim.MustSatisfy.None;
		}
		return result == SituationalAnim.MustSatisfy.None;
	}

	// Token: 0x06008199 RID: 33177 RVA: 0x00339F98 File Offset: 0x00338198
	private SituationalAnim.MustSatisfy GetSatisfactionForEdge(int minx, int maxx, int miny, int maxy)
	{
		bool flag = false;
		bool flag2 = true;
		for (int i = minx; i <= maxx; i++)
		{
			for (int j = miny; j <= maxy; j++)
			{
				int arg = Grid.XYToCell(i, j);
				if (this.test(arg))
				{
					flag = true;
				}
				else
				{
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			return SituationalAnim.MustSatisfy.All;
		}
		if (flag)
		{
			return SituationalAnim.MustSatisfy.Any;
		}
		return SituationalAnim.MustSatisfy.None;
	}

	// Token: 0x04006256 RID: 25174
	public List<global::Tuple<SituationalAnim.Situation, string>> anims;

	// Token: 0x04006257 RID: 25175
	public Func<int, bool> test;

	// Token: 0x04006258 RID: 25176
	public SituationalAnim.MustSatisfy mustSatisfy;

	// Token: 0x02001876 RID: 6262
	[Flags]
	public enum Situation
	{
		// Token: 0x0400625A RID: 25178
		Left = 1,
		// Token: 0x0400625B RID: 25179
		Right = 2,
		// Token: 0x0400625C RID: 25180
		Top = 4,
		// Token: 0x0400625D RID: 25181
		Bottom = 8
	}

	// Token: 0x02001877 RID: 6263
	public enum MustSatisfy
	{
		// Token: 0x0400625F RID: 25183
		None,
		// Token: 0x04006260 RID: 25184
		Any,
		// Token: 0x04006261 RID: 25185
		All
	}
}
