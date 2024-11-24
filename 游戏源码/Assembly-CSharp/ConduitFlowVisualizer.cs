using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02001AB9 RID: 6841
public class ConduitFlowVisualizer
{
	// Token: 0x06008F59 RID: 36697 RVA: 0x00375C80 File Offset: 0x00373E80
	public ConduitFlowVisualizer(ConduitFlow flow_manager, Game.ConduitVisInfo vis_info, EventReference overlay_sound, ConduitFlowVisualizer.Tuning tuning)
	{
		this.flowManager = flow_manager;
		this.visInfo = vis_info;
		this.overlaySound = overlay_sound;
		this.tuning = tuning;
		this.movingBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
		this.staticBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
		ConduitFlowVisualizer.RenderMeshTask.Ball.InitializeResources();
	}

	// Token: 0x06008F5A RID: 36698 RVA: 0x000FDA5F File Offset: 0x000FBC5F
	public void FreeResources()
	{
		this.movingBallMesh.Cleanup();
		this.staticBallMesh.Cleanup();
	}

	// Token: 0x06008F5B RID: 36699 RVA: 0x00375D0C File Offset: 0x00373F0C
	private float CalculateMassScale(float mass)
	{
		float t = (mass - this.visInfo.overlayMassScaleRange.x) / (this.visInfo.overlayMassScaleRange.y - this.visInfo.overlayMassScaleRange.x);
		return Mathf.Lerp(this.visInfo.overlayMassScaleValues.x, this.visInfo.overlayMassScaleValues.y, t);
	}

	// Token: 0x06008F5C RID: 36700 RVA: 0x00375D74 File Offset: 0x00373F74
	private Color32 GetContentsColor(Element element, Color32 default_color)
	{
		if (element != null)
		{
			Color c = element.substance.conduitColour;
			c.a = 128f;
			return c;
		}
		return default_color;
	}

	// Token: 0x06008F5D RID: 36701 RVA: 0x000FDA77 File Offset: 0x000FBC77
	private Color32 GetTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.tint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayTintName);
	}

	// Token: 0x06008F5E RID: 36702 RVA: 0x000FDAA7 File Offset: 0x000FBCA7
	private Color32 GetInsulatedTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.insulatedTint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayInsulatedTintName);
	}

	// Token: 0x06008F5F RID: 36703 RVA: 0x000FDAD7 File Offset: 0x000FBCD7
	private Color32 GetRadiantTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.radiantTint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayRadiantTintName);
	}

	// Token: 0x06008F60 RID: 36704 RVA: 0x00375DAC File Offset: 0x00373FAC
	private Color32 GetCellTintColour(int cell)
	{
		Color32 result;
		if (this.insulatedCells.Contains(cell))
		{
			result = this.GetInsulatedTintColour();
		}
		else if (this.radiantCells.Contains(cell))
		{
			result = this.GetRadiantTintColour();
		}
		else
		{
			result = this.GetTintColour();
		}
		return result;
	}

	// Token: 0x06008F61 RID: 36705 RVA: 0x00375DF0 File Offset: 0x00373FF0
	public void Render(float z, int render_layer, float lerp_percent, bool trigger_audio = false)
	{
		this.animTime += (double)Time.deltaTime;
		if (trigger_audio)
		{
			if (this.audioInfo == null)
			{
				this.audioInfo = new List<ConduitFlowVisualizer.AudioInfo>();
			}
			for (int i = 0; i < this.audioInfo.Count; i++)
			{
				ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
				audioInfo.distance = float.PositiveInfinity;
				audioInfo.position = Vector3.zero;
				audioInfo.blobCount = (audioInfo.blobCount + 1) % 10;
				this.audioInfo[i] = audioInfo;
			}
		}
		if (this.tuning.renderMesh)
		{
			this.RenderMesh(z, render_layer, lerp_percent, trigger_audio);
		}
		if (trigger_audio)
		{
			this.TriggerAudio();
		}
	}

	// Token: 0x06008F62 RID: 36706 RVA: 0x00375EA4 File Offset: 0x003740A4
	private void RenderMesh(float z, int render_layer, float lerp_percent, bool trigger_audio)
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I min = new Vector2I(Mathf.Max(0, visibleArea.Min.x - 1), Mathf.Max(0, visibleArea.Min.y - 1));
		Vector2I max = new Vector2I(Mathf.Min(Grid.WidthInCells - 1, visibleArea.Max.x + 1), Mathf.Min(Grid.HeightInCells - 1, visibleArea.Max.y + 1));
		ConduitFlowVisualizer.RenderMeshContext renderMeshContext = new ConduitFlowVisualizer.RenderMeshContext(this, lerp_percent, min, max);
		if (renderMeshContext.visible_conduits.Count == 0)
		{
			renderMeshContext.Finish();
			return;
		}
		ConduitFlowVisualizer.render_mesh_job.Reset(renderMeshContext);
		int num = Mathf.Max(1, (int)((float)(renderMeshContext.visible_conduits.Count / CPUBudget.coreCount) / 1.5f));
		int num2 = Mathf.Max(1, renderMeshContext.visible_conduits.Count / num);
		for (int num3 = 0; num3 != num2; num3++)
		{
			int num4 = num3 * num;
			int end = (num3 == num2 - 1) ? renderMeshContext.visible_conduits.Count : (num4 + num);
			ConduitFlowVisualizer.render_mesh_job.Add(new ConduitFlowVisualizer.RenderMeshTask(num4, end));
		}
		GlobalJobManager.Run(ConduitFlowVisualizer.render_mesh_job);
		float z2 = 0f;
		if (this.showContents)
		{
			z2 = 1f;
		}
		float w = (float)((int)(this.animTime / (1.0 / (double)this.tuning.framesPerSecond)) % (int)this.tuning.spriteCount) * (1f / this.tuning.spriteCount);
		this.movingBallMesh.Begin();
		this.movingBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
		this.movingBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
		this.movingBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z2, w));
		this.movingBallMesh.SetVector("_Highlight", new Vector4((float)this.highlightColour.r / 255f, (float)this.highlightColour.g / 255f, (float)this.highlightColour.b / 255f, 0f));
		this.staticBallMesh.Begin();
		this.staticBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
		this.staticBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
		this.staticBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z2, 0f));
		this.staticBallMesh.SetVector("_Highlight", new Vector4((float)this.highlightColour.r / 255f, (float)this.highlightColour.g / 255f, (float)this.highlightColour.b / 255f, 0f));
		Vector3 position = CameraController.Instance.transform.GetPosition();
		ConduitFlowVisualizer visualizer = trigger_audio ? this : null;
		for (int num5 = 0; num5 != ConduitFlowVisualizer.render_mesh_job.Count; num5++)
		{
			ConduitFlowVisualizer.render_mesh_job.GetWorkItem(num5).Finish(this.movingBallMesh, this.staticBallMesh, position, visualizer);
		}
		this.movingBallMesh.End(z, this.layer);
		this.staticBallMesh.End(z, this.layer);
		renderMeshContext.Finish();
		ConduitFlowVisualizer.render_mesh_job.Reset(null);
	}

	// Token: 0x06008F63 RID: 36707 RVA: 0x000FDB07 File Offset: 0x000FBD07
	public void ColourizePipeContents(bool show_contents, bool move_to_overlay_layer)
	{
		this.showContents = show_contents;
		this.layer = ((show_contents && move_to_overlay_layer) ? LayerMask.NameToLayer("MaskedOverlay") : 0);
	}

	// Token: 0x06008F64 RID: 36708 RVA: 0x0037623C File Offset: 0x0037443C
	private void AddAudioSource(ConduitFlow.Conduit conduit, Vector3 camera_pos)
	{
		using (new KProfiler.Region("AddAudioSource", null))
		{
			UtilityNetwork network = this.flowManager.GetNetwork(conduit);
			if (network != null)
			{
				Vector3 vector = Grid.CellToPosCCC(conduit.GetCell(this.flowManager), Grid.SceneLayer.Building);
				float num = Vector3.SqrMagnitude(vector - camera_pos);
				bool flag = false;
				for (int i = 0; i < this.audioInfo.Count; i++)
				{
					ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
					if (audioInfo.networkID == network.id)
					{
						if (num < audioInfo.distance)
						{
							audioInfo.distance = num;
							audioInfo.position = vector;
							this.audioInfo[i] = audioInfo;
						}
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					ConduitFlowVisualizer.AudioInfo item = default(ConduitFlowVisualizer.AudioInfo);
					item.networkID = network.id;
					item.position = vector;
					item.distance = num;
					item.blobCount = 0;
					this.audioInfo.Add(item);
				}
			}
		}
	}

	// Token: 0x06008F65 RID: 36709 RVA: 0x00376354 File Offset: 0x00374554
	private void TriggerAudio()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		CameraController instance = CameraController.Instance;
		int num = 0;
		List<ConduitFlowVisualizer.AudioInfo> list = new List<ConduitFlowVisualizer.AudioInfo>();
		for (int i = 0; i < this.audioInfo.Count; i++)
		{
			if (instance.IsVisiblePos(this.audioInfo[i].position))
			{
				list.Add(this.audioInfo[i]);
				num++;
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			ConduitFlowVisualizer.AudioInfo audioInfo = list[j];
			if (audioInfo.distance != float.PositiveInfinity)
			{
				Vector3 position = audioInfo.position;
				position.z = 0f;
				EventInstance instance2 = SoundEvent.BeginOneShot(this.overlaySound, position, 1f, false);
				instance2.setParameterByName("blobCount", (float)audioInfo.blobCount, false);
				instance2.setParameterByName("networkCount", (float)num, false);
				SoundEvent.EndOneShot(instance2);
			}
		}
	}

	// Token: 0x06008F66 RID: 36710 RVA: 0x000FDB28 File Offset: 0x000FBD28
	public void AddThermalConductivity(int cell, float conductivity)
	{
		if (conductivity < 1f)
		{
			this.insulatedCells.Add(cell);
			return;
		}
		if (conductivity > 1f)
		{
			this.radiantCells.Add(cell);
		}
	}

	// Token: 0x06008F67 RID: 36711 RVA: 0x000FDB55 File Offset: 0x000FBD55
	public void RemoveThermalConductivity(int cell, float conductivity)
	{
		if (conductivity < 1f)
		{
			this.insulatedCells.Remove(cell);
			return;
		}
		if (conductivity > 1f)
		{
			this.radiantCells.Remove(cell);
		}
	}

	// Token: 0x06008F68 RID: 36712 RVA: 0x000FDB82 File Offset: 0x000FBD82
	public void SetHighlightedCell(int cell)
	{
		this.highlightedCell = cell;
	}

	// Token: 0x04006C0F RID: 27663
	private ConduitFlow flowManager;

	// Token: 0x04006C10 RID: 27664
	private EventReference overlaySound;

	// Token: 0x04006C11 RID: 27665
	private bool showContents;

	// Token: 0x04006C12 RID: 27666
	private double animTime;

	// Token: 0x04006C13 RID: 27667
	private int layer;

	// Token: 0x04006C14 RID: 27668
	private static Vector2 GRID_OFFSET = new Vector2(0.5f, 0.5f);

	// Token: 0x04006C15 RID: 27669
	private List<ConduitFlowVisualizer.AudioInfo> audioInfo;

	// Token: 0x04006C16 RID: 27670
	private HashSet<int> insulatedCells = new HashSet<int>();

	// Token: 0x04006C17 RID: 27671
	private HashSet<int> radiantCells = new HashSet<int>();

	// Token: 0x04006C18 RID: 27672
	private Game.ConduitVisInfo visInfo;

	// Token: 0x04006C19 RID: 27673
	private ConduitFlowVisualizer.ConduitFlowMesh movingBallMesh;

	// Token: 0x04006C1A RID: 27674
	private ConduitFlowVisualizer.ConduitFlowMesh staticBallMesh;

	// Token: 0x04006C1B RID: 27675
	private int highlightedCell = -1;

	// Token: 0x04006C1C RID: 27676
	private Color32 highlightColour = new Color(0.2f, 0.2f, 0.2f, 0.2f);

	// Token: 0x04006C1D RID: 27677
	private ConduitFlowVisualizer.Tuning tuning;

	// Token: 0x04006C1E RID: 27678
	private static WorkItemCollection<ConduitFlowVisualizer.RenderMeshTask, ConduitFlowVisualizer.RenderMeshContext> render_mesh_job = new WorkItemCollection<ConduitFlowVisualizer.RenderMeshTask, ConduitFlowVisualizer.RenderMeshContext>();

	// Token: 0x02001ABA RID: 6842
	[Serializable]
	public class Tuning
	{
		// Token: 0x04006C1F RID: 27679
		public bool renderMesh;

		// Token: 0x04006C20 RID: 27680
		public float size;

		// Token: 0x04006C21 RID: 27681
		public float spriteCount;

		// Token: 0x04006C22 RID: 27682
		public float framesPerSecond;

		// Token: 0x04006C23 RID: 27683
		public Texture2D backgroundTexture;

		// Token: 0x04006C24 RID: 27684
		public Texture2D foregroundTexture;
	}

	// Token: 0x02001ABB RID: 6843
	private class ConduitFlowMesh
	{
		// Token: 0x06008F6B RID: 36715 RVA: 0x00376448 File Offset: 0x00374648
		public ConduitFlowMesh()
		{
			this.mesh = new Mesh();
			this.mesh.name = "ConduitMesh";
			this.material = new Material(Shader.Find("Klei/ConduitBall"));
		}

		// Token: 0x06008F6C RID: 36716 RVA: 0x003764B8 File Offset: 0x003746B8
		public void AddQuad(Vector2 pos, Color32 color, float size, float is_foreground, float highlight, Vector2I uvbl, Vector2I uvtl, Vector2I uvbr, Vector2I uvtr)
		{
			float num = size * 0.5f;
			this.positions.Add(new Vector3(pos.x - num, pos.y - num, 0f));
			this.positions.Add(new Vector3(pos.x - num, pos.y + num, 0f));
			this.positions.Add(new Vector3(pos.x + num, pos.y - num, 0f));
			this.positions.Add(new Vector3(pos.x + num, pos.y + num, 0f));
			this.uvs.Add(new Vector4((float)uvbl.x, (float)uvbl.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvtl.x, (float)uvtl.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvbr.x, (float)uvbr.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvtr.x, (float)uvtr.y, is_foreground, highlight));
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.triangles.Add(this.quadIndex * 4);
			this.triangles.Add(this.quadIndex * 4 + 1);
			this.triangles.Add(this.quadIndex * 4 + 2);
			this.triangles.Add(this.quadIndex * 4 + 2);
			this.triangles.Add(this.quadIndex * 4 + 1);
			this.triangles.Add(this.quadIndex * 4 + 3);
			this.quadIndex++;
		}

		// Token: 0x06008F6D RID: 36717 RVA: 0x000FDBAB File Offset: 0x000FBDAB
		public void SetTexture(string id, Texture2D texture)
		{
			this.material.SetTexture(id, texture);
		}

		// Token: 0x06008F6E RID: 36718 RVA: 0x000FDBBA File Offset: 0x000FBDBA
		public void SetVector(string id, Vector4 data)
		{
			this.material.SetVector(id, data);
		}

		// Token: 0x06008F6F RID: 36719 RVA: 0x000FDBC9 File Offset: 0x000FBDC9
		public void Begin()
		{
			this.positions.Clear();
			this.uvs.Clear();
			this.triangles.Clear();
			this.colors.Clear();
			this.quadIndex = 0;
		}

		// Token: 0x06008F70 RID: 36720 RVA: 0x003766AC File Offset: 0x003748AC
		public void End(float z, int layer)
		{
			this.mesh.Clear();
			this.mesh.SetVertices(this.positions);
			this.mesh.SetUVs(0, this.uvs);
			this.mesh.SetColors(this.colors);
			this.mesh.SetTriangles(this.triangles, 0, false);
			Graphics.DrawMesh(this.mesh, new Vector3(ConduitFlowVisualizer.GRID_OFFSET.x, ConduitFlowVisualizer.GRID_OFFSET.y, z - 0.1f), Quaternion.identity, this.material, layer);
		}

		// Token: 0x06008F71 RID: 36721 RVA: 0x000FDBFE File Offset: 0x000FBDFE
		public void Cleanup()
		{
			UnityEngine.Object.Destroy(this.mesh);
			this.mesh = null;
			UnityEngine.Object.Destroy(this.material);
			this.material = null;
		}

		// Token: 0x04006C25 RID: 27685
		private Mesh mesh;

		// Token: 0x04006C26 RID: 27686
		private Material material;

		// Token: 0x04006C27 RID: 27687
		private List<Vector3> positions = new List<Vector3>();

		// Token: 0x04006C28 RID: 27688
		private List<Vector4> uvs = new List<Vector4>();

		// Token: 0x04006C29 RID: 27689
		private List<int> triangles = new List<int>();

		// Token: 0x04006C2A RID: 27690
		private List<Color32> colors = new List<Color32>();

		// Token: 0x04006C2B RID: 27691
		private int quadIndex;
	}

	// Token: 0x02001ABC RID: 6844
	private struct AudioInfo
	{
		// Token: 0x04006C2C RID: 27692
		public int networkID;

		// Token: 0x04006C2D RID: 27693
		public int blobCount;

		// Token: 0x04006C2E RID: 27694
		public float distance;

		// Token: 0x04006C2F RID: 27695
		public Vector3 position;
	}

	// Token: 0x02001ABD RID: 6845
	private class RenderMeshContext
	{
		// Token: 0x06008F72 RID: 36722 RVA: 0x00376744 File Offset: 0x00374944
		public RenderMeshContext(ConduitFlowVisualizer outer, float lerp_percent, Vector2I min, Vector2I max)
		{
			this.outer = outer;
			this.lerp_percent = lerp_percent;
			this.visible_conduits = ListPool<int, ConduitFlowVisualizer>.Allocate();
			this.visible_conduits.Capacity = Math.Max(outer.flowManager.soaInfo.NumEntries, this.visible_conduits.Capacity);
			for (int num = 0; num != outer.flowManager.soaInfo.NumEntries; num++)
			{
				Vector2I vector2I = Grid.CellToXY(outer.flowManager.soaInfo.GetCell(num));
				if (min <= vector2I && vector2I <= max)
				{
					this.visible_conduits.Add(num);
				}
			}
		}

		// Token: 0x06008F73 RID: 36723 RVA: 0x000FDC24 File Offset: 0x000FBE24
		public void Finish()
		{
			this.visible_conduits.Recycle();
		}

		// Token: 0x04006C30 RID: 27696
		public ListPool<int, ConduitFlowVisualizer>.PooledList visible_conduits;

		// Token: 0x04006C31 RID: 27697
		public ConduitFlowVisualizer outer;

		// Token: 0x04006C32 RID: 27698
		public float lerp_percent;
	}

	// Token: 0x02001ABE RID: 6846
	private struct RenderMeshTask : IWorkItem<ConduitFlowVisualizer.RenderMeshContext>
	{
		// Token: 0x06008F74 RID: 36724 RVA: 0x003767EC File Offset: 0x003749EC
		public RenderMeshTask(int start, int end)
		{
			this.start = start;
			this.end = end;
			int capacity = end - start;
			this.moving_balls = ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.Allocate();
			this.moving_balls.Capacity = capacity;
			this.static_balls = ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.Allocate();
			this.static_balls.Capacity = capacity;
			this.moving_conduits = ListPool<ConduitFlow.Conduit, ConduitFlowVisualizer.RenderMeshTask>.Allocate();
			this.moving_conduits.Capacity = capacity;
		}

		// Token: 0x06008F75 RID: 36725 RVA: 0x00376850 File Offset: 0x00374A50
		public void Run(ConduitFlowVisualizer.RenderMeshContext context)
		{
			Element element = null;
			for (int num = this.start; num != this.end; num++)
			{
				ConduitFlow.Conduit conduit = context.outer.flowManager.soaInfo.GetConduit(context.visible_conduits[num]);
				ConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(context.outer.flowManager);
				ConduitFlow.ConduitContents initialContents = conduit.GetInitialContents(context.outer.flowManager);
				if (lastFlowInfo.contents.mass > 0f)
				{
					int cell = conduit.GetCell(context.outer.flowManager);
					int cellFromDirection = ConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
					Vector2I vector2I = Grid.CellToXY(cell);
					Vector2I vector2I2 = Grid.CellToXY(cellFromDirection);
					Vector2 vector = (cell == -1) ? vector2I : Vector2.Lerp(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), context.lerp_percent);
					Color32 cellTintColour = context.outer.GetCellTintColour(cell);
					Color32 cellTintColour2 = context.outer.GetCellTintColour(cellFromDirection);
					Color32 color = Color32.Lerp(cellTintColour, cellTintColour2, context.lerp_percent);
					bool highlight = false;
					if (context.outer.showContents)
					{
						if (lastFlowInfo.contents.mass >= initialContents.mass)
						{
							this.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(lastFlowInfo.direction, vector, color, context.outer.tuning.size, false, false));
						}
						if (element == null || lastFlowInfo.contents.element != element.id)
						{
							element = ElementLoader.FindElementByHash(lastFlowInfo.contents.element);
						}
					}
					else
					{
						element = null;
						highlight = (Grid.PosToCell(new Vector3(vector.x + ConduitFlowVisualizer.GRID_OFFSET.x, vector.y + ConduitFlowVisualizer.GRID_OFFSET.y, 0f)) == context.outer.highlightedCell);
					}
					Color32 contentsColor = context.outer.GetContentsColor(element, color);
					float num2 = 1f;
					if (context.outer.showContents || lastFlowInfo.contents.mass < initialContents.mass)
					{
						num2 = context.outer.CalculateMassScale(lastFlowInfo.contents.mass);
					}
					this.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(lastFlowInfo.direction, vector, contentsColor, context.outer.tuning.size * num2, true, highlight));
					this.moving_conduits.Add(conduit);
				}
				if (initialContents.mass > lastFlowInfo.contents.mass && initialContents.mass > 0f)
				{
					int cell2 = conduit.GetCell(context.outer.flowManager);
					Vector2 pos = Grid.CellToXY(cell2);
					float mass = initialContents.mass - lastFlowInfo.contents.mass;
					bool highlight2 = false;
					Color32 cellTintColour3 = context.outer.GetCellTintColour(cell2);
					float num3 = context.outer.CalculateMassScale(mass);
					if (context.outer.showContents)
					{
						this.static_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(ConduitFlow.FlowDirections.None, pos, cellTintColour3, context.outer.tuning.size * num3, false, false));
						if (element == null || initialContents.element != element.id)
						{
							element = ElementLoader.FindElementByHash(initialContents.element);
						}
					}
					else
					{
						element = null;
						highlight2 = (cell2 == context.outer.highlightedCell);
					}
					Color32 contentsColor2 = context.outer.GetContentsColor(element, cellTintColour3);
					this.static_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(ConduitFlow.FlowDirections.None, pos, contentsColor2, context.outer.tuning.size * num3, true, highlight2));
				}
			}
		}

		// Token: 0x06008F76 RID: 36726 RVA: 0x00376BF4 File Offset: 0x00374DF4
		public void Finish(ConduitFlowVisualizer.ConduitFlowMesh moving_ball_mesh, ConduitFlowVisualizer.ConduitFlowMesh static_ball_mesh, Vector3 camera_pos, ConduitFlowVisualizer visualizer)
		{
			for (int num = 0; num != this.moving_balls.Count; num++)
			{
				this.moving_balls[num].Consume(moving_ball_mesh);
			}
			this.moving_balls.Recycle();
			for (int num2 = 0; num2 != this.static_balls.Count; num2++)
			{
				this.static_balls[num2].Consume(static_ball_mesh);
			}
			this.static_balls.Recycle();
			if (visualizer != null)
			{
				foreach (ConduitFlow.Conduit conduit in this.moving_conduits)
				{
					visualizer.AddAudioSource(conduit, camera_pos);
				}
			}
			this.moving_conduits.Recycle();
		}

		// Token: 0x04006C33 RID: 27699
		private ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.PooledList moving_balls;

		// Token: 0x04006C34 RID: 27700
		private ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.PooledList static_balls;

		// Token: 0x04006C35 RID: 27701
		private ListPool<ConduitFlow.Conduit, ConduitFlowVisualizer.RenderMeshTask>.PooledList moving_conduits;

		// Token: 0x04006C36 RID: 27702
		private int start;

		// Token: 0x04006C37 RID: 27703
		private int end;

		// Token: 0x02001ABF RID: 6847
		public struct Ball
		{
			// Token: 0x06008F77 RID: 36727 RVA: 0x000FDC31 File Offset: 0x000FBE31
			public Ball(ConduitFlow.FlowDirections direction, Vector2 pos, Color32 color, float size, bool foreground, bool highlight)
			{
				this.pos = pos;
				this.size = size;
				this.color = color;
				this.direction = direction;
				this.foreground = foreground;
				this.highlight = highlight;
			}

			// Token: 0x06008F78 RID: 36728 RVA: 0x00376CC8 File Offset: 0x00374EC8
			public static void InitializeResources()
			{
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.None] = new ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack
				{
					bl = new Vector2I(0, 0),
					tl = new Vector2I(0, 1),
					br = new Vector2I(1, 0),
					tr = new Vector2I(1, 1)
				};
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Left] = new ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack
				{
					bl = new Vector2I(0, 0),
					tl = new Vector2I(0, 1),
					br = new Vector2I(1, 0),
					tr = new Vector2I(1, 1)
				};
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Right] = ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Left];
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Up] = new ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack
				{
					bl = new Vector2I(1, 0),
					tl = new Vector2I(0, 0),
					br = new Vector2I(1, 1),
					tr = new Vector2I(0, 1)
				};
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Down] = ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Up];
			}

			// Token: 0x06008F79 RID: 36729 RVA: 0x000FDC60 File Offset: 0x000FBE60
			private static ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack GetUVPack(ConduitFlow.FlowDirections direction)
			{
				return ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[direction];
			}

			// Token: 0x06008F7A RID: 36730 RVA: 0x00376DD0 File Offset: 0x00374FD0
			public void Consume(ConduitFlowVisualizer.ConduitFlowMesh mesh)
			{
				ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack uvpack = ConduitFlowVisualizer.RenderMeshTask.Ball.GetUVPack(this.direction);
				mesh.AddQuad(this.pos, this.color, this.size, (float)(this.foreground ? 1 : 0), (float)(this.highlight ? 1 : 0), uvpack.bl, uvpack.tl, uvpack.br, uvpack.tr);
			}

			// Token: 0x04006C38 RID: 27704
			private Vector2 pos;

			// Token: 0x04006C39 RID: 27705
			private float size;

			// Token: 0x04006C3A RID: 27706
			private Color32 color;

			// Token: 0x04006C3B RID: 27707
			private ConduitFlow.FlowDirections direction;

			// Token: 0x04006C3C RID: 27708
			private bool foreground;

			// Token: 0x04006C3D RID: 27709
			private bool highlight;

			// Token: 0x04006C3E RID: 27710
			private static Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack> uv_packs = new Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack>();

			// Token: 0x02001AC0 RID: 6848
			private class UVPack
			{
				// Token: 0x04006C3F RID: 27711
				public Vector2I bl;

				// Token: 0x04006C40 RID: 27712
				public Vector2I tl;

				// Token: 0x04006C41 RID: 27713
				public Vector2I br;

				// Token: 0x04006C42 RID: 27714
				public Vector2I tr;
			}
		}
	}
}
