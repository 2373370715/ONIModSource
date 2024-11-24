using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x020020CE RID: 8398
	public class Brush
	{
		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x0600B295 RID: 45717 RVA: 0x00113F82 File Offset: 0x00112182
		// (set) Token: 0x0600B296 RID: 45718 RVA: 0x00113F8A File Offset: 0x0011218A
		public int Id { get; private set; }

		// Token: 0x0600B297 RID: 45719 RVA: 0x004379BC File Offset: 0x00435BBC
		public Brush(int id, string name, Material material, Mask mask, List<Brush> active_brushes, List<Brush> dirty_brushes, int width_in_tiles, MaterialPropertyBlock property_block)
		{
			this.Id = id;
			this.material = material;
			this.mask = mask;
			this.mesh = new DynamicMesh(name, new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, 0f)));
			this.activeBrushes = active_brushes;
			this.dirtyBrushes = dirty_brushes;
			this.layer = LayerMask.NameToLayer("World");
			this.widthInTiles = width_in_tiles;
			this.propertyBlock = property_block;
		}

		// Token: 0x0600B298 RID: 45720 RVA: 0x00113F93 File Offset: 0x00112193
		public void Add(int tile_idx)
		{
			this.tiles.Add(tile_idx);
			if (!this.dirty)
			{
				this.dirtyBrushes.Add(this);
				this.dirty = true;
			}
		}

		// Token: 0x0600B299 RID: 45721 RVA: 0x00113FBD File Offset: 0x001121BD
		public void Remove(int tile_idx)
		{
			this.tiles.Remove(tile_idx);
			if (!this.dirty)
			{
				this.dirtyBrushes.Add(this);
				this.dirty = true;
			}
		}

		// Token: 0x0600B29A RID: 45722 RVA: 0x00113FE7 File Offset: 0x001121E7
		public void SetMaskOffset(int offset)
		{
			this.mask.SetOffset(offset);
		}

		// Token: 0x0600B29B RID: 45723 RVA: 0x00437A4C File Offset: 0x00435C4C
		public void Refresh()
		{
			bool flag = this.mesh.Meshes.Length != 0;
			int count = this.tiles.Count;
			int vertex_count = count * 4;
			int triangle_count = count * 6;
			this.mesh.Reserve(vertex_count, triangle_count);
			if (this.mesh.SetTriangles)
			{
				int num = 0;
				for (int i = 0; i < count; i++)
				{
					this.mesh.AddTriangle(num);
					this.mesh.AddTriangle(2 + num);
					this.mesh.AddTriangle(1 + num);
					this.mesh.AddTriangle(1 + num);
					this.mesh.AddTriangle(2 + num);
					this.mesh.AddTriangle(3 + num);
					num += 4;
				}
			}
			foreach (int num2 in this.tiles)
			{
				float num3 = (float)(num2 % this.widthInTiles);
				float num4 = (float)(num2 / this.widthInTiles);
				float z = 0f;
				this.mesh.AddVertex(new Vector3(num3 - 0.5f, num4 - 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 + 0.5f, num4 - 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 - 0.5f, num4 + 0.5f, z));
				this.mesh.AddVertex(new Vector3(num3 + 0.5f, num4 + 0.5f, z));
			}
			if (this.mesh.SetUVs)
			{
				for (int j = 0; j < count; j++)
				{
					this.mesh.AddUV(this.mask.UV0);
					this.mesh.AddUV(this.mask.UV1);
					this.mesh.AddUV(this.mask.UV2);
					this.mesh.AddUV(this.mask.UV3);
				}
			}
			this.dirty = false;
			this.mesh.Commit();
			if (this.mesh.Meshes.Length != 0)
			{
				if (!flag)
				{
					this.activeBrushes.Add(this);
					return;
				}
			}
			else if (flag)
			{
				this.activeBrushes.Remove(this);
			}
		}

		// Token: 0x0600B29C RID: 45724 RVA: 0x00437CA8 File Offset: 0x00435EA8
		public void Render()
		{
			Vector3 position = new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.Ground));
			this.mesh.Render(position, Quaternion.identity, this.material, this.layer, this.propertyBlock);
		}

		// Token: 0x0600B29D RID: 45725 RVA: 0x00113FF5 File Offset: 0x001121F5
		public void SetMaterial(Material material, MaterialPropertyBlock property_block)
		{
			this.material = material;
			this.propertyBlock = property_block;
		}

		// Token: 0x04008D16 RID: 36118
		private bool dirty;

		// Token: 0x04008D17 RID: 36119
		private Material material;

		// Token: 0x04008D18 RID: 36120
		private int layer;

		// Token: 0x04008D19 RID: 36121
		private HashSet<int> tiles = new HashSet<int>();

		// Token: 0x04008D1A RID: 36122
		private List<Brush> activeBrushes;

		// Token: 0x04008D1B RID: 36123
		private List<Brush> dirtyBrushes;

		// Token: 0x04008D1C RID: 36124
		private int widthInTiles;

		// Token: 0x04008D1D RID: 36125
		private Mask mask;

		// Token: 0x04008D1E RID: 36126
		private DynamicMesh mesh;

		// Token: 0x04008D1F RID: 36127
		private MaterialPropertyBlock propertyBlock;
	}
}
