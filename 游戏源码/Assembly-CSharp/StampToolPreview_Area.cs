using System;
using TemplateClasses;
using UnityEngine;

// Token: 0x0200145C RID: 5212
public class StampToolPreview_Area : IStampToolPreviewPlugin
{
	// Token: 0x06006C2C RID: 27692 RVA: 0x002E5798 File Offset: 0x002E3998
	public void Setup(StampToolPreviewContext context)
	{
		if (StampToolPreview_Area.material == null)
		{
			StampToolPreview_Area.material = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_background"));
			StampToolPreview_Area.material.name = "Area (" + StampToolPreview_Area.material.name + ")";
		}
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, new Action<string>(delegate(string error)
		{
			Color color = (error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK;
			color.a = 1f;
			StampToolPreview_Area.material.color = color;
		}));
		for (int i = 0; i < context.stampTemplate.cells.Count; i++)
		{
			Cell cell = context.stampTemplate.cells[i];
			MeshRenderer meshRenderer;
			GameObject gameObject;
			StampToolPreviewUtil.MakeQuad(out gameObject, out meshRenderer, 1f, null);
			gameObject.name = "AreaPlacer";
			gameObject.transform.SetParent(context.previewParent, false);
			gameObject.transform.localPosition = new Vector3((float)cell.location_x, (float)cell.location_y + Grid.HalfCellSizeInMeters);
			context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, new System.Action(delegate()
			{
				if (!gameObject.IsNullOrDestroyed())
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}));
			meshRenderer.sharedMaterial = StampToolPreview_Area.material;
		}
	}

	// Token: 0x0400512B RID: 20779
	public static Material material;
}
