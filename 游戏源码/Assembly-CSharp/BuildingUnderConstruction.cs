using System;
using UnityEngine;

// Token: 0x02000C86 RID: 3206
public class BuildingUnderConstruction : Building
{
	// Token: 0x06003DAB RID: 15787 RVA: 0x00231F3C File Offset: 0x0023013C
	protected override void OnPrefabInit()
	{
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(this.Def.SceneLayer);
		base.transform.SetPosition(position);
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Construction"));
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Rotatable component2 = base.GetComponent<Rotatable>();
		if (component != null && component2 == null)
		{
			component.Offset = this.Def.GetVisualizerOffset();
		}
		KBoxCollider2D component3 = base.GetComponent<KBoxCollider2D>();
		if (component3 != null)
		{
			Vector3 visualizerOffset = this.Def.GetVisualizerOffset();
			component3.offset += new Vector2(visualizerOffset.x, visualizerOffset.y);
		}
		base.OnPrefabInit();
	}

	// Token: 0x06003DAC RID: 15788 RVA: 0x00232008 File Offset: 0x00230208
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.Def.IsTilePiece)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.Def.RunOnArea(cell, base.Orientation, delegate(int c)
			{
				TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer);
			});
		}
		base.RegisterBlockTileRenderer();
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x000C7FCF File Offset: 0x000C61CF
	protected override void OnCleanUp()
	{
		base.UnregisterBlockTileRenderer();
		base.OnCleanUp();
	}

	// Token: 0x040029FD RID: 10749
	[MyCmpAdd]
	private KSelectable selectable;

	// Token: 0x040029FE RID: 10750
	[MyCmpAdd]
	private SaveLoadRoot saveLoadRoot;

	// Token: 0x040029FF RID: 10751
	[MyCmpAdd]
	private KPrefabID kPrefabID;
}
