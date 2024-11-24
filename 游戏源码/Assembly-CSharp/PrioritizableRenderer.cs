using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ACA RID: 2762
public class PrioritizableRenderer
{
	// Token: 0x1700022F RID: 559
	// (get) Token: 0x060033C0 RID: 13248 RVA: 0x000C1BF3 File Offset: 0x000BFDF3
	// (set) Token: 0x060033C1 RID: 13249 RVA: 0x000C1BFB File Offset: 0x000BFDFB
	public PrioritizeTool currentTool
	{
		get
		{
			return this.tool;
		}
		set
		{
			this.tool = value;
		}
	}

	// Token: 0x060033C2 RID: 13250 RVA: 0x00207864 File Offset: 0x00205A64
	public PrioritizableRenderer()
	{
		this.layer = LayerMask.NameToLayer("UI");
		Shader shader = Shader.Find("Klei/Prioritizable");
		Texture2D texture = Assets.GetTexture("priority_overlay_atlas");
		this.material = new Material(shader);
		this.material.SetTexture(Shader.PropertyToID("_MainTex"), texture);
		this.prioritizables = new List<Prioritizable>();
		this.mesh = new Mesh();
		this.mesh.name = "Prioritizables";
		this.mesh.MarkDynamic();
	}

	// Token: 0x060033C3 RID: 13251 RVA: 0x000C1C04 File Offset: 0x000BFE04
	public void Cleanup()
	{
		this.material = null;
		this.vertices = null;
		this.uvs = null;
		this.prioritizables = null;
		this.triangles = null;
		UnityEngine.Object.DestroyImmediate(this.mesh);
		this.mesh = null;
	}

	// Token: 0x060033C4 RID: 13252 RVA: 0x002078F0 File Offset: 0x00205AF0
	public void RenderEveryTick()
	{
		using (new KProfiler.Region("PrioritizableRenderer", null))
		{
			if (!(GameScreenManager.Instance == null))
			{
				if (!(SimDebugView.Instance == null) && !(SimDebugView.Instance.GetMode() != OverlayModes.Priorities.ID))
				{
					this.prioritizables.Clear();
					Vector2I vector2I;
					Vector2I vector2I2;
					Grid.GetVisibleExtents(out vector2I, out vector2I2);
					int height = vector2I2.y - vector2I.y;
					int width = vector2I2.x - vector2I.x;
					Extents extents = new Extents(vector2I.x, vector2I.y, width, height);
					List<ScenePartitionerEntry> list = new List<ScenePartitionerEntry>();
					GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.prioritizableObjects, list);
					foreach (ScenePartitionerEntry scenePartitionerEntry in list)
					{
						Prioritizable prioritizable = (Prioritizable)scenePartitionerEntry.obj;
						if (prioritizable != null && prioritizable.showIcon && prioritizable.IsPrioritizable() && this.tool.IsActiveLayer(this.tool.GetFilterLayerFromGameObject(prioritizable.gameObject)) && prioritizable.GetMyWorldId() == ClusterManager.Instance.activeWorldId)
						{
							this.prioritizables.Add(prioritizable);
						}
					}
					if (this.prioritizableCount != this.prioritizables.Count)
					{
						this.prioritizableCount = this.prioritizables.Count;
						this.vertices = new Vector3[4 * this.prioritizableCount];
						this.uvs = new Vector2[4 * this.prioritizableCount];
						this.triangles = new int[6 * this.prioritizableCount];
					}
					if (this.prioritizableCount != 0)
					{
						for (int i = 0; i < this.prioritizables.Count; i++)
						{
							Prioritizable prioritizable2 = this.prioritizables[i];
							Vector3 vector = Vector3.zero;
							KAnimControllerBase component = prioritizable2.GetComponent<KAnimControllerBase>();
							if (component != null)
							{
								vector = component.GetWorldPivot();
							}
							else
							{
								vector = prioritizable2.transform.GetPosition();
							}
							vector.x += prioritizable2.iconOffset.x;
							vector.y += prioritizable2.iconOffset.y;
							Vector2 vector2 = new Vector2(0.2f, 0.3f) * prioritizable2.iconScale;
							float z = -5f;
							int num = 4 * i;
							this.vertices[num] = new Vector3(vector.x - vector2.x, vector.y - vector2.y, z);
							this.vertices[1 + num] = new Vector3(vector.x - vector2.x, vector.y + vector2.y, z);
							this.vertices[2 + num] = new Vector3(vector.x + vector2.x, vector.y - vector2.y, z);
							this.vertices[3 + num] = new Vector3(vector.x + vector2.x, vector.y + vector2.y, z);
							float num2 = 0.1f;
							PrioritySetting masterPriority = prioritizable2.GetMasterPriority();
							float num3 = -1f;
							if (masterPriority.priority_class >= PriorityScreen.PriorityClass.high)
							{
								num3 += 9f;
							}
							if (masterPriority.priority_class >= PriorityScreen.PriorityClass.topPriority)
							{
								num3 += 0f;
							}
							num3 += (float)masterPriority.priority_value;
							float num4 = num2 * num3;
							float num5 = 0f;
							float num6 = num2;
							float num7 = 1f;
							this.uvs[num] = new Vector2(num4, num5);
							this.uvs[1 + num] = new Vector2(num4, num5 + num7);
							this.uvs[2 + num] = new Vector2(num4 + num6, num5);
							this.uvs[3 + num] = new Vector2(num4 + num6, num5 + num7);
							int num8 = 6 * i;
							this.triangles[num8] = num;
							this.triangles[1 + num8] = num + 1;
							this.triangles[2 + num8] = num + 2;
							this.triangles[3 + num8] = num + 2;
							this.triangles[4 + num8] = num + 1;
							this.triangles[5 + num8] = num + 3;
						}
						this.mesh.Clear();
						this.mesh.vertices = this.vertices;
						this.mesh.uv = this.uvs;
						this.mesh.SetTriangles(this.triangles, 0);
						this.mesh.RecalculateBounds();
						Graphics.DrawMesh(this.mesh, Vector3.zero, Quaternion.identity, this.material, this.layer, GameScreenManager.Instance.worldSpaceCanvas.GetComponent<Canvas>().worldCamera, 0, null, false, false);
					}
				}
			}
		}
	}

	// Token: 0x040022D0 RID: 8912
	private Mesh mesh;

	// Token: 0x040022D1 RID: 8913
	private int layer;

	// Token: 0x040022D2 RID: 8914
	private Material material;

	// Token: 0x040022D3 RID: 8915
	private int prioritizableCount;

	// Token: 0x040022D4 RID: 8916
	private Vector3[] vertices;

	// Token: 0x040022D5 RID: 8917
	private Vector2[] uvs;

	// Token: 0x040022D6 RID: 8918
	private int[] triangles;

	// Token: 0x040022D7 RID: 8919
	private List<Prioritizable> prioritizables;

	// Token: 0x040022D8 RID: 8920
	private PrioritizeTool tool;
}
