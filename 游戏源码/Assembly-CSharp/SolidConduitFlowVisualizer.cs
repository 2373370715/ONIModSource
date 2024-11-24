using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02001B45 RID: 6981
public class SolidConduitFlowVisualizer
{
	// Token: 0x0600929D RID: 37533 RVA: 0x003892AC File Offset: 0x003874AC
	public SolidConduitFlowVisualizer(SolidConduitFlow flow_manager, Game.ConduitVisInfo vis_info, EventReference overlay_sound, SolidConduitFlowVisualizer.Tuning tuning)
	{
		this.flowManager = flow_manager;
		this.visInfo = vis_info;
		this.overlaySound = overlay_sound;
		this.tuning = tuning;
		this.movingBallMesh = new SolidConduitFlowVisualizer.ConduitFlowMesh();
		this.staticBallMesh = new SolidConduitFlowVisualizer.ConduitFlowMesh();
	}

	// Token: 0x0600929E RID: 37534 RVA: 0x000FFBDC File Offset: 0x000FDDDC
	public void FreeResources()
	{
		this.movingBallMesh.Cleanup();
		this.staticBallMesh.Cleanup();
	}

	// Token: 0x0600929F RID: 37535 RVA: 0x00389328 File Offset: 0x00387528
	private float CalculateMassScale(float mass)
	{
		float t = (mass - this.visInfo.overlayMassScaleRange.x) / (this.visInfo.overlayMassScaleRange.y - this.visInfo.overlayMassScaleRange.x);
		return Mathf.Lerp(this.visInfo.overlayMassScaleValues.x, this.visInfo.overlayMassScaleValues.y, t);
	}

	// Token: 0x060092A0 RID: 37536 RVA: 0x00375D74 File Offset: 0x00373F74
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

	// Token: 0x060092A1 RID: 37537 RVA: 0x00389390 File Offset: 0x00387590
	private Color32 GetBackgroundColor(float insulation_lerp)
	{
		if (this.showContents)
		{
			return Color32.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayTintName), GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayInsulatedTintName), insulation_lerp);
		}
		return Color32.Lerp(this.visInfo.tint, this.visInfo.insulatedTint, insulation_lerp);
	}

	// Token: 0x060092A2 RID: 37538 RVA: 0x003893FC File Offset: 0x003875FC
	public void Render(float z, int render_layer, float lerp_percent, bool trigger_audio = false)
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I v = new Vector2I(Mathf.Max(0, visibleArea.Min.x - 1), Mathf.Max(0, visibleArea.Min.y - 1));
		Vector2I v2 = new Vector2I(Mathf.Min(Grid.WidthInCells - 1, visibleArea.Max.x + 1), Mathf.Min(Grid.HeightInCells - 1, visibleArea.Max.y + 1));
		this.animTime += (double)Time.deltaTime;
		if (trigger_audio)
		{
			if (this.audioInfo == null)
			{
				this.audioInfo = new List<SolidConduitFlowVisualizer.AudioInfo>();
			}
			for (int i = 0; i < this.audioInfo.Count; i++)
			{
				SolidConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
				audioInfo.distance = float.PositiveInfinity;
				audioInfo.position = Vector3.zero;
				audioInfo.blobCount = (audioInfo.blobCount + 1) % SolidConduitFlowVisualizer.BLOB_SOUND_COUNT;
				this.audioInfo[i] = audioInfo;
			}
		}
		Vector3 position = CameraController.Instance.transform.GetPosition();
		Element element = null;
		if (this.tuning.renderMesh)
		{
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
			for (int j = 0; j < this.flowManager.GetSOAInfo().NumEntries; j++)
			{
				Vector2I u = Grid.CellToXY(this.flowManager.GetSOAInfo().GetCell(j));
				if (!(u < v) && !(u > v2))
				{
					SolidConduitFlow.Conduit conduit = this.flowManager.GetSOAInfo().GetConduit(j);
					SolidConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(this.flowManager);
					SolidConduitFlow.ConduitContents initialContents = conduit.GetInitialContents(this.flowManager);
					bool flag = lastFlowInfo.direction > SolidConduitFlow.FlowDirection.None;
					if (flag)
					{
						int cell = conduit.GetCell(this.flowManager);
						int cellFromDirection = SolidConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
						Vector2I vector2I = Grid.CellToXY(cell);
						Vector2I vector2I2 = Grid.CellToXY(cellFromDirection);
						Vector2 vector = vector2I;
						if (cell != -1)
						{
							vector = Vector2.Lerp(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), lerp_percent);
						}
						float a = this.insulatedCells.Contains(cell) ? 1f : 0f;
						float b = this.insulatedCells.Contains(cellFromDirection) ? 1f : 0f;
						float insulation_lerp = Mathf.Lerp(a, b, lerp_percent);
						Color c = this.GetBackgroundColor(insulation_lerp);
						Vector2I uvbl = new Vector2I(0, 0);
						Vector2I uvtl = new Vector2I(0, 1);
						Vector2I uvbr = new Vector2I(1, 0);
						Vector2I uvtr = new Vector2I(1, 1);
						float highlight = 0f;
						if (this.showContents)
						{
							if (flag != initialContents.pickupableHandle.IsValid())
							{
								this.movingBallMesh.AddQuad(vector, c, this.tuning.size, 0f, 0f, uvbl, uvtl, uvbr, uvtr);
							}
						}
						else
						{
							element = null;
							if (Grid.PosToCell(new Vector3(vector.x + SolidConduitFlowVisualizer.GRID_OFFSET.x, vector.y + SolidConduitFlowVisualizer.GRID_OFFSET.y, 0f)) == this.highlightedCell)
							{
								highlight = 1f;
							}
						}
						Color32 contentsColor = this.GetContentsColor(element, c);
						float num = 1f;
						this.movingBallMesh.AddQuad(vector, contentsColor, this.tuning.size * num, 1f, highlight, uvbl, uvtl, uvbr, uvtr);
						if (trigger_audio)
						{
							this.AddAudioSource(conduit, position);
						}
					}
					if (initialContents.pickupableHandle.IsValid() && !flag)
					{
						int cell2 = conduit.GetCell(this.flowManager);
						Vector2 pos = Grid.CellToXY(cell2);
						float insulation_lerp2 = this.insulatedCells.Contains(cell2) ? 1f : 0f;
						Vector2I uvbl2 = new Vector2I(0, 0);
						Vector2I uvtl2 = new Vector2I(0, 1);
						Vector2I uvbr2 = new Vector2I(1, 0);
						Vector2I uvtr2 = new Vector2I(1, 1);
						float highlight2 = 0f;
						Color c2 = this.GetBackgroundColor(insulation_lerp2);
						float num2 = 1f;
						if (this.showContents)
						{
							this.staticBallMesh.AddQuad(pos, c2, this.tuning.size * num2, 0f, 0f, uvbl2, uvtl2, uvbr2, uvtr2);
						}
						else
						{
							element = null;
							if (cell2 == this.highlightedCell)
							{
								highlight2 = 1f;
							}
						}
						Color32 contentsColor2 = this.GetContentsColor(element, c2);
						this.staticBallMesh.AddQuad(pos, contentsColor2, this.tuning.size * num2, 1f, highlight2, uvbl2, uvtl2, uvbr2, uvtr2);
					}
				}
			}
			this.movingBallMesh.End(z, this.layer);
			this.staticBallMesh.End(z, this.layer);
		}
		if (trigger_audio)
		{
			this.TriggerAudio();
		}
	}

	// Token: 0x060092A3 RID: 37539 RVA: 0x000FFBF4 File Offset: 0x000FDDF4
	public void ColourizePipeContents(bool show_contents, bool move_to_overlay_layer)
	{
		this.showContents = show_contents;
		this.layer = ((show_contents && move_to_overlay_layer) ? LayerMask.NameToLayer("MaskedOverlay") : 0);
	}

	// Token: 0x060092A4 RID: 37540 RVA: 0x00389AC0 File Offset: 0x00387CC0
	private void AddAudioSource(SolidConduitFlow.Conduit conduit, Vector3 camera_pos)
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
					SolidConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
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
					SolidConduitFlowVisualizer.AudioInfo item = default(SolidConduitFlowVisualizer.AudioInfo);
					item.networkID = network.id;
					item.position = vector;
					item.distance = num;
					item.blobCount = 0;
					this.audioInfo.Add(item);
				}
			}
		}
	}

	// Token: 0x060092A5 RID: 37541 RVA: 0x00389BD8 File Offset: 0x00387DD8
	private void TriggerAudio()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		CameraController instance = CameraController.Instance;
		int num = 0;
		List<SolidConduitFlowVisualizer.AudioInfo> list = new List<SolidConduitFlowVisualizer.AudioInfo>();
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
			SolidConduitFlowVisualizer.AudioInfo audioInfo = list[j];
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

	// Token: 0x060092A6 RID: 37542 RVA: 0x000FFC15 File Offset: 0x000FDE15
	public void SetInsulated(int cell, bool insulated)
	{
		if (insulated)
		{
			this.insulatedCells.Add(cell);
			return;
		}
		this.insulatedCells.Remove(cell);
	}

	// Token: 0x060092A7 RID: 37543 RVA: 0x000FFC35 File Offset: 0x000FDE35
	public void SetHighlightedCell(int cell)
	{
		this.highlightedCell = cell;
	}

	// Token: 0x04006EDB RID: 28379
	private SolidConduitFlow flowManager;

	// Token: 0x04006EDC RID: 28380
	private EventReference overlaySound;

	// Token: 0x04006EDD RID: 28381
	private bool showContents;

	// Token: 0x04006EDE RID: 28382
	private double animTime;

	// Token: 0x04006EDF RID: 28383
	private int layer;

	// Token: 0x04006EE0 RID: 28384
	private static Vector2 GRID_OFFSET = new Vector2(0.5f, 0.5f);

	// Token: 0x04006EE1 RID: 28385
	private static int BLOB_SOUND_COUNT = 7;

	// Token: 0x04006EE2 RID: 28386
	private List<SolidConduitFlowVisualizer.AudioInfo> audioInfo;

	// Token: 0x04006EE3 RID: 28387
	private HashSet<int> insulatedCells = new HashSet<int>();

	// Token: 0x04006EE4 RID: 28388
	private Game.ConduitVisInfo visInfo;

	// Token: 0x04006EE5 RID: 28389
	private SolidConduitFlowVisualizer.ConduitFlowMesh movingBallMesh;

	// Token: 0x04006EE6 RID: 28390
	private SolidConduitFlowVisualizer.ConduitFlowMesh staticBallMesh;

	// Token: 0x04006EE7 RID: 28391
	private int highlightedCell = -1;

	// Token: 0x04006EE8 RID: 28392
	private Color32 highlightColour = new Color(0.2f, 0.2f, 0.2f, 0.2f);

	// Token: 0x04006EE9 RID: 28393
	private SolidConduitFlowVisualizer.Tuning tuning;

	// Token: 0x02001B46 RID: 6982
	[Serializable]
	public class Tuning
	{
		// Token: 0x04006EEA RID: 28394
		public bool renderMesh;

		// Token: 0x04006EEB RID: 28395
		public float size;

		// Token: 0x04006EEC RID: 28396
		public float spriteCount;

		// Token: 0x04006EED RID: 28397
		public float framesPerSecond;

		// Token: 0x04006EEE RID: 28398
		public Texture2D backgroundTexture;

		// Token: 0x04006EEF RID: 28399
		public Texture2D foregroundTexture;
	}

	// Token: 0x02001B47 RID: 6983
	private class ConduitFlowMesh
	{
		// Token: 0x060092AA RID: 37546 RVA: 0x00389CCC File Offset: 0x00387ECC
		public ConduitFlowMesh()
		{
			this.mesh = new Mesh();
			this.mesh.name = "ConduitMesh";
			this.material = new Material(Shader.Find("Klei/ConduitBall"));
		}

		// Token: 0x060092AB RID: 37547 RVA: 0x00389D3C File Offset: 0x00387F3C
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

		// Token: 0x060092AC RID: 37548 RVA: 0x000FFC5A File Offset: 0x000FDE5A
		public void SetTexture(string id, Texture2D texture)
		{
			this.material.SetTexture(id, texture);
		}

		// Token: 0x060092AD RID: 37549 RVA: 0x000FFC69 File Offset: 0x000FDE69
		public void SetVector(string id, Vector4 data)
		{
			this.material.SetVector(id, data);
		}

		// Token: 0x060092AE RID: 37550 RVA: 0x000FFC78 File Offset: 0x000FDE78
		public void Begin()
		{
			this.positions.Clear();
			this.uvs.Clear();
			this.triangles.Clear();
			this.colors.Clear();
			this.quadIndex = 0;
		}

		// Token: 0x060092AF RID: 37551 RVA: 0x00389F30 File Offset: 0x00388130
		public void End(float z, int layer)
		{
			this.mesh.Clear();
			this.mesh.SetVertices(this.positions);
			this.mesh.SetUVs(0, this.uvs);
			this.mesh.SetColors(this.colors);
			this.mesh.SetTriangles(this.triangles, 0, false);
			Graphics.DrawMesh(this.mesh, new Vector3(SolidConduitFlowVisualizer.GRID_OFFSET.x, SolidConduitFlowVisualizer.GRID_OFFSET.y, z - 0.1f), Quaternion.identity, this.material, layer);
		}

		// Token: 0x060092B0 RID: 37552 RVA: 0x000FFCAD File Offset: 0x000FDEAD
		public void Cleanup()
		{
			UnityEngine.Object.Destroy(this.mesh);
			this.mesh = null;
			UnityEngine.Object.Destroy(this.material);
			this.material = null;
		}

		// Token: 0x04006EF0 RID: 28400
		private Mesh mesh;

		// Token: 0x04006EF1 RID: 28401
		private Material material;

		// Token: 0x04006EF2 RID: 28402
		private List<Vector3> positions = new List<Vector3>();

		// Token: 0x04006EF3 RID: 28403
		private List<Vector4> uvs = new List<Vector4>();

		// Token: 0x04006EF4 RID: 28404
		private List<int> triangles = new List<int>();

		// Token: 0x04006EF5 RID: 28405
		private List<Color32> colors = new List<Color32>();

		// Token: 0x04006EF6 RID: 28406
		private int quadIndex;
	}

	// Token: 0x02001B48 RID: 6984
	private struct AudioInfo
	{
		// Token: 0x04006EF7 RID: 28407
		public int networkID;

		// Token: 0x04006EF8 RID: 28408
		public int blobCount;

		// Token: 0x04006EF9 RID: 28409
		public float distance;

		// Token: 0x04006EFA RID: 28410
		public Vector3 position;
	}
}
