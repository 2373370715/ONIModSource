using UnityEngine;

public class KAnimLayering
{
	private bool isForeground;

	private KAnimControllerBase controller;

	private KAnimControllerBase foregroundController;

	private KAnimLink link;

	private Grid.SceneLayer layer = Grid.SceneLayer.BuildingFront;

	public static readonly KAnimHashedString UI = new KAnimHashedString("ui");

	public KAnimLayering(KAnimControllerBase controller, Grid.SceneLayer layer)
	{
		this.controller = controller;
		this.layer = layer;
	}

	public void SetLayer(Grid.SceneLayer layer)
	{
		this.layer = layer;
		if (foregroundController != null)
		{
			TransformExtensions.SetLocalPosition(position: new Vector3(0f, 0f, Grid.GetLayerZ(layer) - controller.gameObject.transform.GetPosition().z - 0.1f), transform: foregroundController.transform);
		}
	}

	public void SetIsForeground(bool is_foreground)
	{
		isForeground = is_foreground;
	}

	public bool GetIsForeground()
	{
		return isForeground;
	}

	public KAnimLink GetLink()
	{
		return link;
	}

	private static bool IsAnimLayered(KAnimFile[] anims)
	{
		foreach (KAnimFile kAnimFile in anims)
		{
			if (kAnimFile == null)
			{
				continue;
			}
			KAnimFileData data = kAnimFile.GetData();
			if (data.build == null)
			{
				continue;
			}
			KAnim.Build.Symbol[] symbols = data.build.symbols;
			for (int j = 0; j < symbols.Length; j++)
			{
				if (((uint)symbols[j].flags & 8u) != 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void HideSymbolsInternal()
	{
		KAnimFile[] animFiles = controller.AnimFiles;
		foreach (KAnimFile kAnimFile in animFiles)
		{
			if (kAnimFile == null)
			{
				continue;
			}
			KAnimFileData data = kAnimFile.GetData();
			if (data.build == null)
			{
				continue;
			}
			KAnim.Build.Symbol[] symbols = data.build.symbols;
			for (int j = 0; j < symbols.Length; j++)
			{
				if ((symbols[j].flags & 8) != 0 != isForeground && !(symbols[j].hash == UI))
				{
					controller.SetSymbolVisiblity(symbols[j].hash, is_visible: false);
				}
			}
		}
	}

	public void HideSymbols()
	{
		if (EntityPrefabs.Instance == null || isForeground)
		{
			return;
		}
		KAnimFile[] animFiles = controller.AnimFiles;
		bool flag = IsAnimLayered(animFiles);
		if (flag && layer != Grid.SceneLayer.NoLayer)
		{
			bool num = foregroundController == null;
			if (num)
			{
				GameObject gameObject = Util.KInstantiate(EntityPrefabs.Instance.ForegroundLayer, controller.gameObject);
				gameObject.name = controller.name + "_fg";
				foregroundController = gameObject.GetComponent<KAnimControllerBase>();
				link = new KAnimLink(controller, foregroundController);
			}
			foregroundController.AnimFiles = animFiles;
			foregroundController.GetLayering().SetIsForeground(is_foreground: true);
			foregroundController.initialAnim = controller.initialAnim;
			Dirty();
			KAnimSynchronizer synchronizer = controller.GetSynchronizer();
			if (num)
			{
				synchronizer.Add(foregroundController);
			}
			else
			{
				foregroundController.GetComponent<KBatchedAnimController>().SwapAnims(foregroundController.AnimFiles);
			}
			synchronizer.Sync(foregroundController);
			TransformExtensions.SetLocalPosition(position: new Vector3(0f, 0f, Grid.GetLayerZ(layer) - controller.gameObject.transform.GetPosition().z - 0.1f), transform: foregroundController.gameObject.transform);
			foregroundController.gameObject.SetActive(value: true);
		}
		else if (!flag && foregroundController != null)
		{
			controller.GetSynchronizer().Remove(foregroundController);
			foregroundController.gameObject.DeleteObject();
			link = null;
		}
		if (foregroundController != null)
		{
			HideSymbolsInternal();
			foregroundController.GetLayering()?.HideSymbolsInternal();
		}
	}

	public void RefreshForegroundBatchGroup()
	{
		if (!(foregroundController == null))
		{
			foregroundController.GetComponent<KBatchedAnimController>().SwapAnims(foregroundController.AnimFiles);
		}
	}

	public void Dirty()
	{
		if (!(foregroundController == null))
		{
			foregroundController.Offset = controller.Offset;
			foregroundController.Pivot = controller.Pivot;
			foregroundController.Rotation = controller.Rotation;
			foregroundController.FlipX = controller.FlipX;
			foregroundController.FlipY = controller.FlipY;
		}
	}
}
