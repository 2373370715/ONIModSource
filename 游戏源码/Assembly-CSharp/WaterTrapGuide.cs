using UnityEngine;

public class WaterTrapGuide : KMonoBehaviour, IRenderEveryTick
{
	private int previousDepthAvailable = -1;

	public GameObject parent;

	public bool occupyTiles;

	private KBatchedAnimController parentController;

	private KBatchedAnimController guideController;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		parentController = parent.GetComponent<KBatchedAnimController>();
		guideController = GetComponent<KBatchedAnimController>();
		RefreshTint();
		RefreshDepthAvailable();
	}

	private void RefreshTint()
	{
		guideController.TintColour = parentController.TintColour;
	}

	public void RefreshPosition()
	{
		if (guideController != null && guideController.IsMoving)
		{
			guideController.SetDirty();
		}
	}

	private void RefreshDepthAvailable()
	{
		int depthAvailable = GetDepthAvailable(Grid.PosToCell(this), parent);
		if (depthAvailable != previousDepthAvailable)
		{
			KBatchedAnimController component = GetComponent<KBatchedAnimController>();
			if (depthAvailable == 0)
			{
				component.enabled = false;
			}
			else
			{
				component.enabled = true;
				component.Play(new HashedString("place_pipe" + depthAvailable));
			}
			if (occupyTiles)
			{
				OccupyArea(parent, depthAvailable);
			}
			previousDepthAvailable = depthAvailable;
		}
	}

	public void RenderEveryTick(float dt)
	{
		RefreshPosition();
		RefreshTint();
		RefreshDepthAvailable();
	}

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
}
