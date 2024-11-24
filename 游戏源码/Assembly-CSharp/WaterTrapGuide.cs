using System;
using UnityEngine;

// Token: 0x02001A42 RID: 6722
public class WaterTrapGuide : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06008C2D RID: 35885 RVA: 0x000FB98E File Offset: 0x000F9B8E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.parentController = this.parent.GetComponent<KBatchedAnimController>();
		this.guideController = base.GetComponent<KBatchedAnimController>();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06008C2E RID: 35886 RVA: 0x000FB9BF File Offset: 0x000F9BBF
	private void RefreshTint()
	{
		this.guideController.TintColour = this.parentController.TintColour;
	}

	// Token: 0x06008C2F RID: 35887 RVA: 0x000FB9D7 File Offset: 0x000F9BD7
	public void RefreshPosition()
	{
		if (this.guideController != null && this.guideController.IsMoving)
		{
			this.guideController.SetDirty();
		}
	}

	// Token: 0x06008C30 RID: 35888 RVA: 0x00362510 File Offset: 0x00360710
	private void RefreshDepthAvailable()
	{
		int depthAvailable = WaterTrapGuide.GetDepthAvailable(Grid.PosToCell(this), this.parent);
		if (depthAvailable != this.previousDepthAvailable)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (depthAvailable == 0)
			{
				component.enabled = false;
			}
			else
			{
				component.enabled = true;
				component.Play(new HashedString("place_pipe" + depthAvailable.ToString()), KAnim.PlayMode.Once, 1f, 0f);
			}
			if (this.occupyTiles)
			{
				WaterTrapGuide.OccupyArea(this.parent, depthAvailable);
			}
			this.previousDepthAvailable = depthAvailable;
		}
	}

	// Token: 0x06008C31 RID: 35889 RVA: 0x000FB9FF File Offset: 0x000F9BFF
	public void RenderEveryTick(float dt)
	{
		this.RefreshPosition();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06008C32 RID: 35890 RVA: 0x00362594 File Offset: 0x00360794
	public static void OccupyArea(GameObject go, int depth_available)
	{
		int cell = Grid.PosToCell(go.transform.GetPosition());
		for (int i = 1; i <= 4; i++)
		{
			int key = Grid.OffsetCell(cell, 0, -i);
			if (i <= depth_available)
			{
				Grid.ObjectLayers[1][key] = go;
			}
			else if (Grid.ObjectLayers[1].ContainsKey(key) && Grid.ObjectLayers[1][key] == go)
			{
				Grid.ObjectLayers[1][key] = null;
			}
		}
	}

	// Token: 0x06008C33 RID: 35891 RVA: 0x00362614 File Offset: 0x00360814
	public static int GetDepthAvailable(int root_cell, GameObject pump)
	{
		int num = 4;
		int result = 0;
		for (int i = 1; i <= num; i++)
		{
			int num2 = Grid.OffsetCell(root_cell, 0, -i);
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || (Grid.ObjectLayers[1].ContainsKey(num2) && !(Grid.ObjectLayers[1][num2] == null) && !(Grid.ObjectLayers[1][num2] == pump)))
			{
				break;
			}
			result = i;
		}
		return result;
	}

	// Token: 0x0400698B RID: 27019
	private int previousDepthAvailable = -1;

	// Token: 0x0400698C RID: 27020
	public GameObject parent;

	// Token: 0x0400698D RID: 27021
	public bool occupyTiles;

	// Token: 0x0400698E RID: 27022
	private KBatchedAnimController parentController;

	// Token: 0x0400698F RID: 27023
	private KBatchedAnimController guideController;
}
