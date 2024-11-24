using System;
using UnityEngine;

// Token: 0x02001496 RID: 5270
[AddComponentMenu("KMonoBehaviour/scripts/LightShapePreview")]
public class LightShapePreview : KMonoBehaviour
{
	// Token: 0x06006D39 RID: 27961 RVA: 0x002EAFF4 File Offset: 0x002E91F4
	private void Update()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num != this.previousCell)
		{
			this.previousCell = num;
			LightGridManager.DestroyPreview();
			LightGridManager.CreatePreview(Grid.OffsetCell(num, this.offset), this.radius, this.shape, this.lux, this.width, this.direction);
		}
	}

	// Token: 0x06006D3A RID: 27962 RVA: 0x000E7A51 File Offset: 0x000E5C51
	protected override void OnCleanUp()
	{
		LightGridManager.DestroyPreview();
	}

	// Token: 0x040051F0 RID: 20976
	public float radius;

	// Token: 0x040051F1 RID: 20977
	public int lux;

	// Token: 0x040051F2 RID: 20978
	public int width;

	// Token: 0x040051F3 RID: 20979
	public DiscreteShadowCaster.Direction direction;

	// Token: 0x040051F4 RID: 20980
	public global::LightShape shape;

	// Token: 0x040051F5 RID: 20981
	public CellOffset offset;

	// Token: 0x040051F6 RID: 20982
	private int previousCell = -1;
}
