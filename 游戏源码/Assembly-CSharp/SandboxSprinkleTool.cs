using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02001448 RID: 5192
public class SandboxSprinkleTool : BrushTool
{
	// Token: 0x06006BC3 RID: 27587 RVA: 0x000E6DA5 File Offset: 0x000E4FA5
	public static void DestroyInstance()
	{
		SandboxSprinkleTool.instance = null;
	}

	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06006BC4 RID: 27588 RVA: 0x000E67C3 File Offset: 0x000E49C3
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006BC5 RID: 27589 RVA: 0x000E6DAD File Offset: 0x000E4FAD
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxSprinkleTool.instance = this;
	}

	// Token: 0x06006BC6 RID: 27590 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006BC7 RID: 27591 RVA: 0x002E3AB0 File Offset: 0x002E1CB0
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.noiseScaleSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.noiseDensitySlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.BrushSize"), true);
	}

	// Token: 0x06006BC8 RID: 27592 RVA: 0x000E68B1 File Offset: 0x000E4AB1
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06006BC9 RID: 27593 RVA: 0x002E3BA8 File Offset: 0x002E1DA8
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int num in this.recentlyAffectedCells)
		{
			Color color = new Color(this.recentAffectedCellColor[num].r, this.recentAffectedCellColor[num].g, this.recentAffectedCellColor[num].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 5f), -1f, 1f, 0.1f, 0.2f));
			colors.Add(new ToolMenu.CellColorData(num, color));
		}
		foreach (int num2 in this.cellsInRadius)
		{
			if (this.recentlyAffectedCells.Contains(num2))
			{
				Color radiusIndicatorColor = this.radiusIndicatorColor;
				Color color2 = this.recentAffectedCellColor[num2];
				color2.a = 0.2f;
				Color color3 = new Color((radiusIndicatorColor.r + color2.r) / 2f, (radiusIndicatorColor.g + color2.g) / 2f, (radiusIndicatorColor.b + color2.b) / 2f, radiusIndicatorColor.a + (1f - radiusIndicatorColor.a) * color2.a);
				colors.Add(new ToolMenu.CellColorData(num2, color3));
			}
			else
			{
				colors.Add(new ToolMenu.CellColorData(num2, this.radiusIndicatorColor));
			}
		}
	}

	// Token: 0x06006BCA RID: 27594 RVA: 0x002E3D70 File Offset: 0x002E1F70
	public override void SetBrushSize(int radius)
	{
		this.brushRadius = radius;
		this.brushOffsets.Clear();
		for (int i = 0; i < this.brushRadius * 2; i++)
		{
			for (int j = 0; j < this.brushRadius * 2; j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), new Vector2((float)this.brushRadius, (float)this.brushRadius)) < (float)this.brushRadius - 0.8f)
				{
					Vector2 vector = Grid.CellToXY(Grid.OffsetCell(this.currentCell, i, j));
					float num = PerlinSimplexNoise.noise(vector.x / this.settings.GetFloatSetting("SandboxTools.NoiseDensity"), vector.y / this.settings.GetFloatSetting("SandboxTools.NoiseDensity"), Time.realtimeSinceStartup);
					if (this.settings.GetFloatSetting("SandboxTools.NoiseScale") <= num)
					{
						this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
					}
				}
			}
		}
	}

	// Token: 0x06006BCB RID: 27595 RVA: 0x000E6DBB File Offset: 0x000E4FBB
	private void Update()
	{
		this.OnMouseMove(Grid.CellToPos(this.currentCell));
	}

	// Token: 0x06006BCC RID: 27596 RVA: 0x000E6DCE File Offset: 0x000E4FCE
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
	}

	// Token: 0x06006BCD RID: 27597 RVA: 0x002E3E7C File Offset: 0x002E207C
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		this.recentlyAffectedCells.Add(cell);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		if (!this.recentAffectedCellColor.ContainsKey(cell))
		{
			this.recentAffectedCellColor.Add(cell, element.substance.uiColour);
		}
		else
		{
			this.recentAffectedCellColor[cell] = element.substance.uiColour;
		}
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
			this.recentAffectedCellColor.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		byte index2 = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
		Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
		if (disease != null)
		{
			index2 = Db.Get().Diseases.GetIndex(disease.id);
		}
		int cell2 = cell;
		SimHashes id = element.id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float floatSetting = this.settings.GetFloatSetting("SandboxTools.Mass");
		float floatSetting2 = this.settings.GetFloatSetting("SandbosTools.Temperature");
		int callbackIdx = index;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, floatSetting, floatSetting2, index2, this.settings.GetIntSetting("SandboxTools.DiseaseCount"), callbackIdx);
		this.SetBrushSize(this.brushRadius);
	}

	// Token: 0x06006BCE RID: 27598 RVA: 0x002E2620 File Offset: 0x002E0820
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.SandboxCopyElement))
		{
			int cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
			if (Grid.IsValidCell(cell))
			{
				SandboxSampleTool.Sample(cell);
			}
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x040050E4 RID: 20708
	public static SandboxSprinkleTool instance;

	// Token: 0x040050E5 RID: 20709
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x040050E6 RID: 20710
	private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>();
}
