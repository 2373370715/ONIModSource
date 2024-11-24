using System.Collections;
using UnityEngine;

public class StampToolPreview
{
	private IStampToolPreviewPlugin[] plugins;

	private StampToolPreviewContext context;

	private int prevOriginCell;

	public StampToolPreview(InterfaceTool tool, params IStampToolPreviewPlugin[] plugins)
	{
		context = new StampToolPreviewContext();
		context.previewParent = new GameObject("StampToolPreview::Preview").transform;
		context.tool = tool;
		this.plugins = plugins;
	}

	public IEnumerator Setup(TemplateContainer stampTemplate)
	{
		Cleanup();
		context.stampTemplate = stampTemplate;
		if (plugins != null)
		{
			IStampToolPreviewPlugin[] array = plugins;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Setup(context);
			}
		}
		yield return null;
		if (context.frameAfterSetupFn != null)
		{
			context.frameAfterSetupFn();
		}
	}

	public void Refresh(int originCell)
	{
		if (context.stampTemplate == null || originCell == prevOriginCell)
		{
			return;
		}
		prevOriginCell = originCell;
		if (Grid.IsValidCell(originCell))
		{
			if (context.refreshFn != null)
			{
				context.refreshFn(originCell);
			}
			context.previewParent.transform.SetPosition(Grid.CellToPosCBC(originCell, context.tool.visualizerLayer));
			context.previewParent.gameObject.SetActive(value: true);
		}
	}

	public void OnErrorChange(string error)
	{
		if (context.onErrorChangeFn != null)
		{
			context.onErrorChangeFn(error);
		}
	}

	public void OnPlace()
	{
		if (context.onPlaceFn != null)
		{
			context.onPlaceFn();
		}
	}

	public void Cleanup()
	{
		if (context.cleanupFn != null)
		{
			context.cleanupFn();
		}
		prevOriginCell = Grid.InvalidCell;
		context.stampTemplate = null;
		context.frameAfterSetupFn = null;
		context.refreshFn = null;
		context.onPlaceFn = null;
		context.cleanupFn = null;
	}
}
