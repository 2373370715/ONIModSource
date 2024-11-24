using System;
using UnityEngine;

// Token: 0x02000C21 RID: 3105
public class ArtifactModule : SingleEntityReceptacle, IRenderEveryTick
{
	// Token: 0x06003B32 RID: 15154 RVA: 0x0022A0E4 File Offset: 0x002282E4
	protected override void OnSpawn()
	{
		this.craft = this.module.CraftInterface.GetComponent<Clustercraft>();
		if (this.craft.Status == Clustercraft.CraftStatus.InFlight && base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(false);
		}
		base.OnSpawn();
		base.Subscribe(705820818, new Action<object>(this.OnEnterSpace));
		base.Subscribe(-1165815793, new Action<object>(this.OnExitSpace));
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x000C6351 File Offset: 0x000C4551
	public void RenderEveryTick(float dt)
	{
		this.ArtifactTrackModulePosition();
	}

	// Token: 0x06003B34 RID: 15156 RVA: 0x0022A168 File Offset: 0x00228368
	private void ArtifactTrackModulePosition()
	{
		this.occupyingObjectRelativePosition = this.animController.Offset + Vector3.up * 0.5f + new Vector3(0f, 0f, -1f);
		if (base.occupyingObject != null)
		{
			this.PositionOccupyingObject();
		}
	}

	// Token: 0x06003B35 RID: 15157 RVA: 0x000C6359 File Offset: 0x000C4559
	private void OnEnterSpace(object data)
	{
		if (base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(false);
		}
	}

	// Token: 0x06003B36 RID: 15158 RVA: 0x000C6375 File Offset: 0x000C4575
	private void OnExitSpace(object data)
	{
		if (base.occupyingObject != null)
		{
			base.occupyingObject.SetActive(true);
		}
	}

	// Token: 0x04002878 RID: 10360
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x04002879 RID: 10361
	[MyCmpReq]
	private RocketModuleCluster module;

	// Token: 0x0400287A RID: 10362
	private Clustercraft craft;
}
