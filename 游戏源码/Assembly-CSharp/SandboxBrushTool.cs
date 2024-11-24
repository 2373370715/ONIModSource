using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using UnityEngine;

// Token: 0x0200143A RID: 5178
public class SandboxBrushTool : BrushTool
{
	// Token: 0x06006B4D RID: 27469 RVA: 0x000E67BB File Offset: 0x000E49BB
	public static void DestroyInstance()
	{
		SandboxBrushTool.instance = null;
	}

	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06006B4E RID: 27470 RVA: 0x000E67C3 File Offset: 0x000E49C3
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006B4F RID: 27471 RVA: 0x000E67CF File Offset: 0x000E49CF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxBrushTool.instance = this;
	}

	// Token: 0x06006B50 RID: 27472 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B51 RID: 27473 RVA: 0x002E21FC File Offset: 0x002E03FC
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
		SandboxToolParameterMenu.SelectorValue elementSelector = SandboxToolParameterMenu.instance.elementSelector;
		elementSelector.onValueChanged = (Action<object>)Delegate.Combine(elementSelector.onValueChanged, new Action<object>(this.OnElementChanged));
	}

	// Token: 0x06006B52 RID: 27474 RVA: 0x000E67DD File Offset: 0x000E49DD
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.audioEvent.release();
	}

	// Token: 0x06006B53 RID: 27475 RVA: 0x002E22D4 File Offset: 0x002E04D4
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int num in this.recentlyAffectedCells)
		{
			Color color = new Color(this.recentAffectedCellColor[num].r, this.recentAffectedCellColor[num].g, this.recentAffectedCellColor[num].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 10f), -1f, 1f, 0.1f, 0.2f));
			colors.Add(new ToolMenu.CellColorData(num, color));
		}
		foreach (int cell in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06006B54 RID: 27476 RVA: 0x002E23EC File Offset: 0x002E05EC
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
					this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
				}
			}
		}
	}

	// Token: 0x06006B55 RID: 27477 RVA: 0x002E2484 File Offset: 0x002E0684
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
	}

	// Token: 0x06006B56 RID: 27478 RVA: 0x002E2620 File Offset: 0x002E0820
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

	// Token: 0x06006B57 RID: 27479 RVA: 0x000E6802 File Offset: 0x000E4A02
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06006B58 RID: 27480 RVA: 0x000E681B File Offset: 0x000E4A1B
	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
		this.StopSound();
	}

	// Token: 0x06006B59 RID: 27481 RVA: 0x000E682A File Offset: 0x000E4A2A
	private void OnElementChanged(object new_element)
	{
		this.clearVisitedCells();
	}

	// Token: 0x06006B5A RID: 27482 RVA: 0x002E2668 File Offset: 0x002E0868
	protected override string GetDragSound()
	{
		string str = (ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")].state & Element.State.Solid).ToString();
		return "SandboxTool_Brush_" + str + "_Add";
	}

	// Token: 0x06006B5B RID: 27483 RVA: 0x002E26B8 File Offset: 0x002E08B8
	protected override void PlaySound()
	{
		base.PlaySound();
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		string sound;
		switch (element.state & Element.State.Solid)
		{
		case Element.State.Vacuum:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Gas", false);
			break;
		case Element.State.Gas:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Gas", false);
			break;
		case Element.State.Liquid:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Liquid", false);
			break;
		case Element.State.Solid:
			sound = GlobalAssets.GetSound("Brush_" + element.substance.GetOreBumpSound(), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Brush_Rock", false);
			}
			break;
		default:
			sound = GlobalAssets.GetSound("Brush_Rock", false);
			break;
		}
		this.audioEvent = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		this.audioEvent.set3DAttributes(attributes);
		this.audioEvent.start();
	}

	// Token: 0x06006B5C RID: 27484 RVA: 0x000E6832 File Offset: 0x000E4A32
	private void StopSound()
	{
		this.audioEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.audioEvent.release();
	}

	// Token: 0x040050BD RID: 20669
	public static SandboxBrushTool instance;

	// Token: 0x040050BE RID: 20670
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x040050BF RID: 20671
	private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>();

	// Token: 0x040050C0 RID: 20672
	private EventInstance audioEvent;
}
