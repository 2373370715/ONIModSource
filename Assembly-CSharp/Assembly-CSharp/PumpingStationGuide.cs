﻿using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/PumpingStationGuide")]
public class PumpingStationGuide : KMonoBehaviour, IRenderEveryTick
{
		protected override void OnSpawn()
	{
		base.OnSpawn();
		this.parentController = this.parent.GetComponent<KBatchedAnimController>();
		this.guideController = base.GetComponent<KBatchedAnimController>();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

		public void RefreshPosition()
	{
		if (this.guideController != null && this.guideController.IsMoving)
		{
			this.guideController.SetDirty();
		}
	}

		private void RefreshTint()
	{
		this.guideController.TintColour = this.parentController.TintColour;
	}

		private void RefreshDepthAvailable()
	{
		int depthAvailable = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell(this), this.parent);
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
				PumpingStationGuide.OccupyArea(this.parent, depthAvailable);
			}
			this.previousDepthAvailable = depthAvailable;
		}
	}

		public void RenderEveryTick(float dt)
	{
		this.RefreshPosition();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

		public static void OccupyArea(GameObject go, int depth_available)
	{
		int cell = Grid.PosToCell(go.transform.GetPosition());
		for (int i = 1; i <= 4; i++)
		{
			int key = Grid.OffsetCell(cell, 0, -i);
			int key2 = Grid.OffsetCell(cell, 1, -i);
			if (i <= depth_available)
			{
				Grid.ObjectLayers[1][key] = go;
				Grid.ObjectLayers[1][key2] = go;
			}
			else
			{
				if (Grid.ObjectLayers[1].ContainsKey(key) && Grid.ObjectLayers[1][key] == go)
				{
					Grid.ObjectLayers[1][key] = null;
				}
				if (Grid.ObjectLayers[1].ContainsKey(key2) && Grid.ObjectLayers[1][key2] == go)
				{
					Grid.ObjectLayers[1][key2] = null;
				}
			}
		}
	}

		public static int GetDepthAvailable(int root_cell, GameObject pump)
	{
		int num = 4;
		int result = 0;
		for (int i = 1; i <= num; i++)
		{
			int num2 = Grid.OffsetCell(root_cell, 0, -i);
			int num3 = Grid.OffsetCell(root_cell, 1, -i);
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || !Grid.IsValidCell(num3) || Grid.Solid[num3] || (Grid.ObjectLayers[1].ContainsKey(num2) && !(Grid.ObjectLayers[1][num2] == null) && !(Grid.ObjectLayers[1][num2] == pump)) || (Grid.ObjectLayers[1].ContainsKey(num3) && !(Grid.ObjectLayers[1][num3] == null) && !(Grid.ObjectLayers[1][num3] == pump)))
			{
				break;
			}
			result = i;
		}
		return result;
	}

		private int previousDepthAvailable = -1;

		public GameObject parent;

		public bool occupyTiles;

		private KBatchedAnimController parentController;

		private KBatchedAnimController guideController;
}
