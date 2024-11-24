using System;
using UnityEngine;

// Token: 0x020013D3 RID: 5075
[AddComponentMenu("KMonoBehaviour/scripts/GridVisibility")]
public class GridVisibility : KMonoBehaviour
{
	// Token: 0x060067FB RID: 26619 RVA: 0x002D5DBC File Offset: 0x002D3FBC
	protected override void OnSpawn()
	{
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "GridVisibility.OnSpawn");
		this.OnCellChange();
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld != null && !base.gameObject.HasTag(GameTags.Stored))
		{
			myWorld.SetDiscovered(false);
		}
	}

	// Token: 0x060067FC RID: 26620 RVA: 0x002D5E20 File Offset: 0x002D4020
	private void OnCellChange()
	{
		if (base.gameObject.HasTag(GameTags.Dead))
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		if (!Grid.Revealed[num])
		{
			int baseX;
			int baseY;
			Grid.PosToXY(base.transform.GetPosition(), out baseX, out baseY);
			GridVisibility.Reveal(baseX, baseY, this.radius, this.innerRadius);
			Grid.Revealed[num] = true;
		}
		FogOfWarMask.ClearMask(num);
	}

	// Token: 0x060067FD RID: 26621 RVA: 0x002D5E98 File Offset: 0x002D4098
	public static void Reveal(int baseX, int baseY, int radius, float innerRadius)
	{
		int num = (int)Grid.WorldIdx[baseY * Grid.WidthInCells + baseX];
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				int num2 = baseY + i;
				int num3 = baseX + j;
				if (num2 >= 0 && Grid.HeightInCells - 1 >= num2 && num3 >= 0 && Grid.WidthInCells - 1 >= num3)
				{
					int num4 = num2 * Grid.WidthInCells + num3;
					if (Grid.Visible[num4] < 255 && num == (int)Grid.WorldIdx[num4])
					{
						Vector2 vector = new Vector2((float)j, (float)i);
						float num5 = Mathf.Lerp(1f, 0f, (vector.magnitude - innerRadius) / ((float)radius - innerRadius));
						Grid.Reveal(num4, (byte)(255f * num5), false);
					}
				}
			}
		}
	}

	// Token: 0x060067FE RID: 26622 RVA: 0x000E4172 File Offset: 0x000E2372
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
	}

	// Token: 0x04004E7A RID: 20090
	public int radius = 18;

	// Token: 0x04004E7B RID: 20091
	public float innerRadius = 16.5f;
}
