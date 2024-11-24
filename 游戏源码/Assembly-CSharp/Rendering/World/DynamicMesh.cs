using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x020020CF RID: 8399
	public class DynamicMesh
	{
		// Token: 0x0600B29E RID: 45726 RVA: 0x00114005 File Offset: 0x00112205
		public DynamicMesh(string name, Bounds bounds)
		{
			this.Name = name;
			this.Bounds = bounds;
		}

		// Token: 0x0600B29F RID: 45727 RVA: 0x00437CF0 File Offset: 0x00435EF0
		public void Reserve(int vertex_count, int triangle_count)
		{
			if (vertex_count > this.VertexCount)
			{
				this.SetUVs = true;
			}
			else
			{
				this.SetUVs = false;
			}
			if (this.TriangleCount != triangle_count)
			{
				this.SetTriangles = true;
			}
			else
			{
				this.SetTriangles = false;
			}
			int num = (int)Mathf.Ceil((float)triangle_count / (float)DynamicMesh.TrianglesPerMesh);
			if (num != this.Meshes.Length)
			{
				this.Meshes = new DynamicSubMesh[num];
				for (int i = 0; i < this.Meshes.Length; i++)
				{
					int idx_offset = -i * DynamicMesh.VerticesPerMesh;
					this.Meshes[i] = new DynamicSubMesh(this.Name, this.Bounds, idx_offset);
				}
				this.SetUVs = true;
				this.SetTriangles = true;
			}
			for (int j = 0; j < this.Meshes.Length; j++)
			{
				if (j == this.Meshes.Length - 1)
				{
					this.Meshes[j].Reserve(vertex_count % DynamicMesh.VerticesPerMesh, triangle_count % DynamicMesh.TrianglesPerMesh);
				}
				else
				{
					this.Meshes[j].Reserve(DynamicMesh.VerticesPerMesh, DynamicMesh.TrianglesPerMesh);
				}
			}
			this.VertexCount = vertex_count;
			this.TriangleCount = triangle_count;
		}

		// Token: 0x0600B2A0 RID: 45728 RVA: 0x00437DFC File Offset: 0x00435FFC
		public void Commit()
		{
			DynamicSubMesh[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].Commit();
			}
			this.TriangleMeshIdx = 0;
			this.UVMeshIdx = 0;
			this.VertexMeshIdx = 0;
		}

		// Token: 0x0600B2A1 RID: 45729 RVA: 0x00437E3C File Offset: 0x0043603C
		public void AddTriangle(int triangle)
		{
			if (this.Meshes[this.TriangleMeshIdx].AreTrianglesFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.TriangleMeshIdx + 1;
				this.TriangleMeshIdx = num;
				object obj = meshes[num];
			}
			this.Meshes[this.TriangleMeshIdx].AddTriangle(triangle);
		}

		// Token: 0x0600B2A2 RID: 45730 RVA: 0x00437E8C File Offset: 0x0043608C
		public void AddUV(Vector2 uv)
		{
			DynamicSubMesh dynamicSubMesh = this.Meshes[this.UVMeshIdx];
			if (dynamicSubMesh.AreUVsFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.UVMeshIdx + 1;
				this.UVMeshIdx = num;
				dynamicSubMesh = meshes[num];
			}
			dynamicSubMesh.AddUV(uv);
		}

		// Token: 0x0600B2A3 RID: 45731 RVA: 0x00437ED0 File Offset: 0x004360D0
		public void AddVertex(Vector3 vertex)
		{
			DynamicSubMesh dynamicSubMesh = this.Meshes[this.VertexMeshIdx];
			if (dynamicSubMesh.AreVerticesFull())
			{
				DynamicSubMesh[] meshes = this.Meshes;
				int num = this.VertexMeshIdx + 1;
				this.VertexMeshIdx = num;
				dynamicSubMesh = meshes[num];
			}
			dynamicSubMesh.AddVertex(vertex);
		}

		// Token: 0x0600B2A4 RID: 45732 RVA: 0x00437F14 File Offset: 0x00436114
		public void Render(Vector3 position, Quaternion rotation, Material material, int layer, MaterialPropertyBlock property_block)
		{
			DynamicSubMesh[] meshes = this.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				meshes[i].Render(position, rotation, material, layer, property_block);
			}
		}

		// Token: 0x04008D20 RID: 36128
		private static int TrianglesPerMesh = 65004;

		// Token: 0x04008D21 RID: 36129
		private static int VerticesPerMesh = 4 * DynamicMesh.TrianglesPerMesh / 6;

		// Token: 0x04008D22 RID: 36130
		public bool SetUVs;

		// Token: 0x04008D23 RID: 36131
		public bool SetTriangles;

		// Token: 0x04008D24 RID: 36132
		public string Name;

		// Token: 0x04008D25 RID: 36133
		public Bounds Bounds;

		// Token: 0x04008D26 RID: 36134
		public DynamicSubMesh[] Meshes = new DynamicSubMesh[0];

		// Token: 0x04008D27 RID: 36135
		private int VertexCount;

		// Token: 0x04008D28 RID: 36136
		private int TriangleCount;

		// Token: 0x04008D29 RID: 36137
		private int VertexIdx;

		// Token: 0x04008D2A RID: 36138
		private int UVIdx;

		// Token: 0x04008D2B RID: 36139
		private int TriangleIdx;

		// Token: 0x04008D2C RID: 36140
		private int TriangleMeshIdx;

		// Token: 0x04008D2D RID: 36141
		private int VertexMeshIdx;

		// Token: 0x04008D2E RID: 36142
		private int UVMeshIdx;
	}
}
