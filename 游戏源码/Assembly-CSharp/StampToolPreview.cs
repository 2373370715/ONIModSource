using System;
using System.Collections;
using UnityEngine;

// Token: 0x02001459 RID: 5209
public class StampToolPreview
{
	// Token: 0x06006C1D RID: 27677 RVA: 0x000E70B2 File Offset: 0x000E52B2
	public StampToolPreview(InterfaceTool tool, params IStampToolPreviewPlugin[] plugins)
	{
		this.context = new StampToolPreviewContext();
		this.context.previewParent = new GameObject("StampToolPreview::Preview").transform;
		this.context.tool = tool;
		this.plugins = plugins;
	}

	// Token: 0x06006C1E RID: 27678 RVA: 0x000E70F2 File Offset: 0x000E52F2
	public IEnumerator Setup(TemplateContainer stampTemplate)
	{
		this.Cleanup();
		this.context.stampTemplate = stampTemplate;
		if (this.plugins != null)
		{
			IStampToolPreviewPlugin[] array = this.plugins;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Setup(this.context);
			}
		}
		yield return null;
		if (this.context.frameAfterSetupFn != null)
		{
			this.context.frameAfterSetupFn();
		}
		yield break;
	}

	// Token: 0x06006C1F RID: 27679 RVA: 0x002E53C8 File Offset: 0x002E35C8
	public void Refresh(int originCell)
	{
		if (this.context.stampTemplate == null)
		{
			return;
		}
		if (originCell == this.prevOriginCell)
		{
			return;
		}
		this.prevOriginCell = originCell;
		if (!Grid.IsValidCell(originCell))
		{
			return;
		}
		if (this.context.refreshFn != null)
		{
			this.context.refreshFn(originCell);
		}
		this.context.previewParent.transform.SetPosition(Grid.CellToPosCBC(originCell, this.context.tool.visualizerLayer));
		this.context.previewParent.gameObject.SetActive(true);
	}

	// Token: 0x06006C20 RID: 27680 RVA: 0x000E7108 File Offset: 0x000E5308
	public void OnErrorChange(string error)
	{
		if (this.context.onErrorChangeFn != null)
		{
			this.context.onErrorChangeFn(error);
		}
	}

	// Token: 0x06006C21 RID: 27681 RVA: 0x000E7128 File Offset: 0x000E5328
	public void OnPlace()
	{
		if (this.context.onPlaceFn != null)
		{
			this.context.onPlaceFn();
		}
	}

	// Token: 0x06006C22 RID: 27682 RVA: 0x002E5460 File Offset: 0x002E3660
	public void Cleanup()
	{
		if (this.context.cleanupFn != null)
		{
			this.context.cleanupFn();
		}
		this.prevOriginCell = Grid.InvalidCell;
		this.context.stampTemplate = null;
		this.context.frameAfterSetupFn = null;
		this.context.refreshFn = null;
		this.context.onPlaceFn = null;
		this.context.cleanupFn = null;
	}

	// Token: 0x0400511E RID: 20766
	private IStampToolPreviewPlugin[] plugins;

	// Token: 0x0400511F RID: 20767
	private StampToolPreviewContext context;

	// Token: 0x04005120 RID: 20768
	private int prevOriginCell;
}
