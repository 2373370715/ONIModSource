using System;
using TemplateClasses;
using UnityEngine;

public class StampToolPreview_SolidLiquidGas : IStampToolPreviewPlugin
{
	public static Material solidMaterial;

	public static Material liquidMaterial;

	public static Material gasMaterial;

	public void Setup(StampToolPreviewContext context)
	{
		SetupMaterials(context);
		using HashSetPool<int, StampToolPreview_SolidLiquidGas>.PooledHashSet pooledHashSet = PoolsFor<StampToolPreview_SolidLiquidGas>.AllocateHashSet<int>();
		if (context.stampTemplate.buildings != null)
		{
			foreach (Prefab building in context.stampTemplate.buildings)
			{
				if (building.IsNullOrDestroyed())
				{
					continue;
				}
				GameObject prefab = Assets.GetPrefab(building.id);
				if (prefab.IsNullOrDestroyed())
				{
					continue;
				}
				Building component = prefab.GetComponent<Building>();
				if (!component.IsNullOrDestroyed() && component.Def.IsTilePiece)
				{
					pooledHashSet.Add(CellHash(building.location_x, building.location_y));
				}
				MakeBaseSolid.Def def = prefab.GetDef<MakeBaseSolid.Def>();
				if (!def.IsNullOrDestroyed())
				{
					CellOffset[] solidOffsets = def.solidOffsets;
					for (int i = 0; i < solidOffsets.Length; i++)
					{
						CellOffset cellOffset = solidOffsets[i];
						pooledHashSet.Add(CellHash(building.location_x + cellOffset.x, building.location_y + cellOffset.y));
					}
				}
			}
		}
		if (context.stampTemplate.cells == null)
		{
			return;
		}
		for (int j = 0; j < context.stampTemplate.cells.Count; j++)
		{
			Cell cell = context.stampTemplate.cells[j];
			if (cell.IsNullOrDestroyed() || pooledHashSet.Contains(CellHash(cell.location_x, cell.location_y)))
			{
				continue;
			}
			Element element = ElementLoader.FindElementByHash(cell.element);
			Material material;
			string text;
			switch ((element != null) ? new Element.State?(element.state & Element.State.Solid) : null)
			{
			case Element.State.Solid:
				material = solidMaterial;
				text = "Solid";
				break;
			case Element.State.Liquid:
				material = liquidMaterial;
				text = "Liquid";
				break;
			case Element.State.Gas:
				material = gasMaterial;
				text = "Gas";
				break;
			case Element.State.Vacuum:
				material = gasMaterial;
				text = "Vacuum";
				break;
			default:
				continue;
			}
			StampToolPreviewUtil.MakeQuad(out var gameObject, out var meshRenderer, 1f);
			gameObject.transform.SetParent(context.previewParent, worldPositionStays: false);
			gameObject.transform.localPosition = new Vector3(cell.location_x, (float)cell.location_y + Grid.HalfCellSizeInMeters);
			context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, (System.Action)delegate
			{
				if (!gameObject.IsNullOrDestroyed())
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			});
			gameObject.name = "TilePlacer (" + text + ")";
			meshRenderer.material = material;
		}
	}

	private void SetupMaterials(StampToolPreviewContext context)
	{
		if (solidMaterial.IsNullOrDestroyed())
		{
			solidMaterial = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_solid"));
			solidMaterial.name = "Solid (" + solidMaterial.name + ")";
		}
		if (liquidMaterial.IsNullOrDestroyed())
		{
			liquidMaterial = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_liquid"));
			liquidMaterial.name = "Liquid (" + liquidMaterial.name + ")";
		}
		if (gasMaterial.IsNullOrDestroyed())
		{
			gasMaterial = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_gas"));
			gasMaterial.name = "Gas (" + gasMaterial.name + ")";
		}
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, (Action<string>)delegate(string error)
		{
			Color c2 = ((error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK);
			if (!solidMaterial.IsNullOrDestroyed())
			{
				solidMaterial.color = WithAlpha(c2, 1f);
			}
			if (!liquidMaterial.IsNullOrDestroyed())
			{
				liquidMaterial.color = WithAlpha(c2, 1f);
			}
			if (!gasMaterial.IsNullOrDestroyed())
			{
				gasMaterial.color = WithAlpha(c2, 1f);
			}
		});
		static Color WithAlpha(Color c, float a)
		{
			return new Color(c.r, c.g, c.b, a);
		}
	}

	private static int CellHash(int x, int y)
	{
		return x + y * 10000;
	}
}
