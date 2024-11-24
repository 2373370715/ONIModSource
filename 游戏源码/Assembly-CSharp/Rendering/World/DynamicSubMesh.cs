using System;
using UnityEngine;

namespace Rendering.World
{
	// Token: 0x020020D0 RID: 8400
	public class DynamicSubMesh
	{
		// Token: 0x0600B2A6 RID: 45734 RVA: 0x00437F48 File Offset: 0x00436148
		public DynamicSubMesh(string name, Bounds bounds, int idx_offset)
		{
			this.IdxOffset = idx_offset;
			this.Mesh = new Mesh();
			this.Mesh.name = name;
			this.Mesh.bounds = bounds;
			this.Mesh.MarkDynamic();
		}

		// Token: 0x0600B2A7 RID: 45735 RVA: 0x00437FB4 File Offset: 0x004361B4
		public void Reserve(int vertex_count, int triangle_count)
		{
			if (vertex_count > this.Vertices.Length)
			{
				this.Vertices = new Vector3[vertex_count];
				this.UVs = new Vector2[vertex_count];
				this.SetUVs = true;
			}
			else
			{
				this.SetUVs = false;
			}
			if (this.Triangles.Length != triangle_count)
			{
				this.Triangles = new int[triangle_count];
				this.SetTriangles = true;
				return;
			}
			this.SetTriangles = false;
		}

		// Token: 0x0600B2A8 RID: 45736 RVA: 0x00114041 File Offset: 0x00112241
		public bool AreTrianglesFull()
		{
			return this.Triangles.Length == this.TriangleIdx;
		}

		// Token: 0x0600B2A9 RID: 45737 RVA: 0x00114053 File Offset: 0x00112253
		public bool AreVerticesFull()
		{
			return this.Vertices.Length == this.VertexIdx;
		}

		// Token: 0x0600B2AA RID: 45738 RVA: 0x00114065 File Offset: 0x00112265
		public bool AreUVsFull()
		{
			return this.UVs.Length == this.UVIdx;
		}

		// Token: 0x0600B2AB RID: 45739 RVA: 0x0043801C File Offset: 0x0043621C
		public void Commit()
		{
			if (this.SetTriangles)
			{
				this.Mesh.Clear();
			}
			this.Mesh.vertices = this.Vertices;
			if (this.SetUVs || this.SetTriangles)
			{
				this.Mesh.uv = this.UVs;
			}
			if (this.SetTriangles)
			{
				this.Mesh.triangles = this.Triangles;
			}
			this.VertexIdx = 0;
			this.UVIdx = 0;
			this.TriangleIdx = 0;
		}

		// Token: 0x0600B2AC RID: 45740 RVA: 0x0043809C File Offset: 0x0043629C
		public void AddTriangle(int triangle)
		{
			int[] triangles = this.Triangles;
			int triangleIdx = this.TriangleIdx;
			this.TriangleIdx = triangleIdx + 1;
			triangles[triangleIdx] = triangle + this.IdxOffset;
		}

		// Token: 0x0600B2AD RID: 45741 RVA: 0x004380CC File Offset: 0x004362CC
		public void AddUV(Vector2 uv)
		{
			Vector2[] uvs = this.UVs;
			int uvidx = this.UVIdx;
			this.UVIdx = uvidx + 1;
			uvs[uvidx] = uv;
		}

		// Token: 0x0600B2AE RID: 45742 RVA: 0x004380F8 File Offset: 0x004362F8
		public void AddVertex(Vector3 vertex)
		{
			Vector3[] vertices = this.Vertices;
			int vertexIdx = this.VertexIdx;
			this.VertexIdx = vertexIdx + 1;
			vertices[vertexIdx] = vertex;
		}

		// Token: 0x0600B2AF RID: 45743 RVA: 0x00438124 File Offset: 0x00436324
		public void Render(Vector3 position, Quaternion rotation, Material material, int layer, MaterialPropertyBlock property_block)
		{
			Graphics.DrawMesh(this.Mesh, position, rotation, material, layer, null, 0, property_block, false, false);
		}

		// Token: 0x04008D2F RID: 36143
		public Vector3[] Vertices = new Vector3[0];

		// Token: 0x04008D30 RID: 36144
		public Vector2[] UVs = new Vector2[0];

		// Token: 0x04008D31 RID: 36145
		public int[] Triangles = new int[0];

		// Token: 0x04008D32 RID: 36146
		public Mesh Mesh;

		// Token: 0x04008D33 RID: 36147
		public bool SetUVs;

		// Token: 0x04008D34 RID: 36148
		public bool SetTriangles;

		// Token: 0x04008D35 RID: 36149
		private int VertexIdx;

		// Token: 0x04008D36 RID: 36150
		private int UVIdx;

		// Token: 0x04008D37 RID: 36151
		private int TriangleIdx;

		// Token: 0x04008D38 RID: 36152
		private int IdxOffset;
	}
}
