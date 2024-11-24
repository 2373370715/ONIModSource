using System;
using UnityEngine;

// Token: 0x02000ACD RID: 2765
public class RangedAttackable : AttackableBase
{
	// Token: 0x060033CE RID: 13262 RVA: 0x000BC8FA File Offset: 0x000BAAFA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060033CF RID: 13263 RVA: 0x000C1CF0 File Offset: 0x000BFEF0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.preferUnreservedCell = true;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x060033D0 RID: 13264 RVA: 0x000BCAC8 File Offset: 0x000BACC8
	public new int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x060033D1 RID: 13265 RVA: 0x00208060 File Offset: 0x00206260
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(0f, 0.5f, 0.5f, 0.15f);
		foreach (CellOffset offset in base.GetOffsets())
		{
			Gizmos.DrawCube(new Vector3(0.5f, 0.5f, 0f) + Grid.CellToPos(Grid.OffsetCell(Grid.PosToCell(base.gameObject), offset)), Vector3.one);
		}
	}
}
