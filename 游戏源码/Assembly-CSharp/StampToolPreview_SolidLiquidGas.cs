using System;
using System.Runtime.CompilerServices;
using TemplateClasses;
using UnityEngine;

// Token: 0x02001465 RID: 5221
public class StampToolPreview_SolidLiquidGas : IStampToolPreviewPlugin
{
	// Token: 0x06006C4A RID: 27722 RVA: 0x002E6580 File Offset: 0x002E4780
	public void Setup(StampToolPreviewContext context)
	{
		this.SetupMaterials(context);
		using (HashSetPool<int, StampToolPreview_SolidLiquidGas>.PooledHashSet pooledHashSet = PoolsFor<StampToolPreview_SolidLiquidGas>.AllocateHashSet<int>())
		{
			if (context.stampTemplate.buildings != null)
			{
				foreach (Prefab prefab in context.stampTemplate.buildings)
				{
					if (!prefab.IsNullOrDestroyed())
					{
						GameObject prefab2 = Assets.GetPrefab(prefab.id);
						if (!prefab2.IsNullOrDestroyed())
						{
							Building component = prefab2.GetComponent<Building>();
							if (!component.IsNullOrDestroyed() && component.Def.IsTilePiece)
							{
								pooledHashSet.Add(StampToolPreview_SolidLiquidGas.CellHash(prefab.location_x, prefab.location_y));
							}
							MakeBaseSolid.Def def = prefab2.GetDef<MakeBaseSolid.Def>();
							if (!def.IsNullOrDestroyed())
							{
								foreach (CellOffset cellOffset in def.solidOffsets)
								{
									pooledHashSet.Add(StampToolPreview_SolidLiquidGas.CellHash(prefab.location_x + cellOffset.x, prefab.location_y + cellOffset.y));
								}
							}
						}
					}
				}
			}
			if (context.stampTemplate.cells != null)
			{
				for (int j = 0; j < context.stampTemplate.cells.Count; j++)
				{
					Cell cell = context.stampTemplate.cells[j];
					if (!cell.IsNullOrDestroyed() && !pooledHashSet.Contains(StampToolPreview_SolidLiquidGas.CellHash(cell.location_x, cell.location_y)))
					{
						Element element = ElementLoader.FindElementByHash(cell.element);
						Element.State? state;
						if (element == null)
						{
							state = null;
						}
						else
						{
							state = new Element.State?(element.state & Element.State.Solid);
						}
						if (state != null)
						{
							Material material;
							string str;
							switch (state.GetValueOrDefault())
							{
							case Element.State.Vacuum:
								material = StampToolPreview_SolidLiquidGas.gasMaterial;
								str = "Vacuum";
								break;
							case Element.State.Gas:
								material = StampToolPreview_SolidLiquidGas.gasMaterial;
								str = "Gas";
								break;
							case Element.State.Liquid:
								material = StampToolPreview_SolidLiquidGas.liquidMaterial;
								str = "Liquid";
								break;
							case Element.State.Solid:
								material = StampToolPreview_SolidLiquidGas.solidMaterial;
								str = "Solid";
								break;
							default:
								goto IL_2B7;
							}
							MeshRenderer meshRenderer;
							GameObject gameObject;
							StampToolPreviewUtil.MakeQuad(out gameObject, out meshRenderer, 1f, null);
							gameObject.transform.SetParent(context.previewParent, false);
							gameObject.transform.localPosition = new Vector3((float)cell.location_x, (float)cell.location_y + Grid.HalfCellSizeInMeters);
							context.cleanupFn = (System.Action)Delegate.Combine(context.cleanupFn, new System.Action(delegate()
							{
								if (gameObject.IsNullOrDestroyed())
								{
									return;
								}
								UnityEngine.Object.Destroy(gameObject);
							}));
							gameObject.name = "TilePlacer (" + str + ")";
							meshRenderer.material = material;
						}
					}
					IL_2B7:;
				}
			}
		}
	}

	// Token: 0x06006C4B RID: 27723 RVA: 0x002E68A4 File Offset: 0x002E4AA4
	private void SetupMaterials(StampToolPreviewContext context)
	{
		if (StampToolPreview_SolidLiquidGas.solidMaterial.IsNullOrDestroyed())
		{
			StampToolPreview_SolidLiquidGas.solidMaterial = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_solid"));
			StampToolPreview_SolidLiquidGas.solidMaterial.name = "Solid (" + StampToolPreview_SolidLiquidGas.solidMaterial.name + ")";
		}
		if (StampToolPreview_SolidLiquidGas.liquidMaterial.IsNullOrDestroyed())
		{
			StampToolPreview_SolidLiquidGas.liquidMaterial = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_liquid"));
			StampToolPreview_SolidLiquidGas.liquidMaterial.name = "Liquid (" + StampToolPreview_SolidLiquidGas.liquidMaterial.name + ")";
		}
		if (StampToolPreview_SolidLiquidGas.gasMaterial.IsNullOrDestroyed())
		{
			StampToolPreview_SolidLiquidGas.gasMaterial = StampToolPreviewUtil.MakeMaterial(Assets.GetTexture("stamptool_vis_gas"));
			StampToolPreview_SolidLiquidGas.gasMaterial.name = "Gas (" + StampToolPreview_SolidLiquidGas.gasMaterial.name + ")";
		}
		context.onErrorChangeFn = (Action<string>)Delegate.Combine(context.onErrorChangeFn, new Action<string>(delegate(string error)
		{
			Color c = (error != null) ? StampToolPreviewUtil.COLOR_ERROR : StampToolPreviewUtil.COLOR_OK;
			if (!StampToolPreview_SolidLiquidGas.solidMaterial.IsNullOrDestroyed())
			{
				StampToolPreview_SolidLiquidGas.solidMaterial.color = StampToolPreview_SolidLiquidGas.<SetupMaterials>g__WithAlpha|4_1(c, 1f);
			}
			if (!StampToolPreview_SolidLiquidGas.liquidMaterial.IsNullOrDestroyed())
			{
				StampToolPreview_SolidLiquidGas.liquidMaterial.color = StampToolPreview_SolidLiquidGas.<SetupMaterials>g__WithAlpha|4_1(c, 1f);
			}
			if (!StampToolPreview_SolidLiquidGas.gasMaterial.IsNullOrDestroyed())
			{
				StampToolPreview_SolidLiquidGas.gasMaterial.color = StampToolPreview_SolidLiquidGas.<SetupMaterials>g__WithAlpha|4_1(c, 1f);
			}
		}));
	}

	// Token: 0x06006C4C RID: 27724 RVA: 0x000E7287 File Offset: 0x000E5487
	private static int CellHash(int x, int y)
	{
		return x + y * 10000;
	}

	// Token: 0x06006C4E RID: 27726 RVA: 0x000E7292 File Offset: 0x000E5492
	[CompilerGenerated]
	internal static Color <SetupMaterials>g__WithAlpha|4_1(Color c, float a)
	{
		return new Color(c.r, c.g, c.b, a);
	}

	// Token: 0x0400513D RID: 20797
	public static Material solidMaterial;

	// Token: 0x0400513E RID: 20798
	public static Material liquidMaterial;

	// Token: 0x0400513F RID: 20799
	public static Material gasMaterial;
}
