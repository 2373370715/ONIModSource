using System;
using UnityEngine;

// Token: 0x02000C6A RID: 3178
[AddComponentMenu("KMonoBehaviour/scripts/BuildingAttachPoint")]
public class BuildingAttachPoint : KMonoBehaviour
{
	// Token: 0x06003CF9 RID: 15609 RVA: 0x000C7733 File Offset: 0x000C5933
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.BuildingAttachPoints.Add(this);
		this.TryAttachEmptyHardpoints();
	}

	// Token: 0x06003CFA RID: 15610 RVA: 0x000BFD08 File Offset: 0x000BDF08
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003CFB RID: 15611 RVA: 0x0022FAD4 File Offset: 0x0022DCD4
	private void TryAttachEmptyHardpoints()
	{
		for (int i = 0; i < this.points.Length; i++)
		{
			if (!(this.points[i].attachedBuilding != null))
			{
				bool flag = false;
				int num = 0;
				while (num < Components.AttachableBuildings.Count && !flag)
				{
					if (Components.AttachableBuildings[num].attachableToTag == this.points[i].attachableType && Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.points[i].position) == Grid.PosToCell(Components.AttachableBuildings[num]))
					{
						this.points[i].attachedBuilding = Components.AttachableBuildings[num];
						flag = true;
					}
					num++;
				}
			}
		}
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x0022FBAC File Offset: 0x0022DDAC
	public bool AcceptsAttachment(Tag type, int cell)
	{
		int cell2 = Grid.PosToCell(base.gameObject);
		for (int i = 0; i < this.points.Length; i++)
		{
			if (Grid.OffsetCell(cell2, this.points[i].position) == cell && this.points[i].attachableType == type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003CFD RID: 15613 RVA: 0x000C774C File Offset: 0x000C594C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BuildingAttachPoints.Remove(this);
	}

	// Token: 0x04002987 RID: 10631
	public BuildingAttachPoint.HardPoint[] points = new BuildingAttachPoint.HardPoint[0];

	// Token: 0x02000C6B RID: 3179
	[Serializable]
	public struct HardPoint
	{
		// Token: 0x06003CFF RID: 15615 RVA: 0x000C7773 File Offset: 0x000C5973
		public HardPoint(CellOffset position, Tag attachableType, AttachableBuilding attachedBuilding)
		{
			this.position = position;
			this.attachableType = attachableType;
			this.attachedBuilding = attachedBuilding;
		}

		// Token: 0x04002988 RID: 10632
		public CellOffset position;

		// Token: 0x04002989 RID: 10633
		public Tag attachableType;

		// Token: 0x0400298A RID: 10634
		public AttachableBuilding attachedBuilding;
	}
}
