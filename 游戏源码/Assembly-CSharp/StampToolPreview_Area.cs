using System;
using TemplateClasses;
using UnityEngine;

public class StampToolPreview_Area : IStampToolPreviewPlugin
{
	public static Material material;

	public void Setup(StampToolPreviewContext context)
	{
		if (material == null)
		{
			material = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_background"));
			material.name = "Area (" + material.name + ")";
		}
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, (Action<string>)delegate(string error)
		{
			Color color = ((error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
			color.a = 1f;
			material.color = color;
		});
		for (int i = 0; i < context.stampTemplate.cells.Count; i++)
		{
			Cell cell = context.stampTemplate.cells[i];
			StampToolPreviewUtil.MakeQuad(out var gameObject, out var meshRenderer, 1f);
			gameObject.name = "AreaPlacer";
			gameObject.transform.SetParent(context.previewParent, worldPositionStays: false);
			gameObject.transform.localPosition = new Vector3(cell.location_x, (float)cell.location_y + Grid.HalfCellSizeInMeters);
			context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, (System.Action)delegate
			{
				if (!gameObject.IsNullOrDestroyed())
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			});
			meshRenderer.sharedMaterial = material;
		}
	}
}
