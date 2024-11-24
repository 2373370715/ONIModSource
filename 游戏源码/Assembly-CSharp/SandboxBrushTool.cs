using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using UnityEngine;

public class SandboxBrushTool : BrushTool
{
	public static SandboxBrushTool instance;

	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>();

	private EventInstance audioEvent;

	private SandboxSettings settings => SandboxToolParameterMenu.instance.settings;

	public static void DestroyInstance()
	{
		instance = null;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		instance = this;
	}

	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(value: true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(value: true);
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(value: true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(value: true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(value: true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(value: true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(value: true);
		SandboxToolParameterMenu.SelectorValue elementSelector = SandboxToolParameterMenu.instance.elementSelector;
		elementSelector.onValueChanged = (Action<object>)Delegate.Combine(elementSelector.onValueChanged, new Action<object>(OnElementChanged));
	}

	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(value: false);
		audioEvent.release();
	}

	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int recentlyAffectedCell in recentlyAffectedCells)
		{
			Color color = new Color(recentAffectedCellColor[recentlyAffectedCell].r, recentAffectedCellColor[recentlyAffectedCell].g, recentAffectedCellColor[recentlyAffectedCell].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 10f), -1f, 1f, 0.1f, 0.2f));
			colors.Add(new ToolMenu.CellColorData(recentlyAffectedCell, color));
		}
		foreach (int item in cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(item, radiusIndicatorColor));
		}
	}

	public override void SetBrushSize(int radius)
	{
		brushRadius = radius;
		brushOffsets.Clear();
		for (int i = 0; i < brushRadius * 2; i++)
		{
			for (int j = 0; j < brushRadius * 2; j++)
			{
				if (Vector2.Distance(new Vector2(i, j), new Vector2(brushRadius, brushRadius)) < (float)brushRadius - 0.8f)
				{
					brushOffsets.Add(new Vector2(i - brushRadius, j - brushRadius));
				}
			}
		}
	}

	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		recentlyAffectedCells.Add(cell);
		Element element = ElementLoader.elements[settings.GetIntSetting("SandboxTools.SelectedElement")];
		if (!recentAffectedCellColor.ContainsKey(cell))
		{
			recentAffectedCellColor.Add(cell, element.substance.uiColour);
		}
		else
		{
			recentAffectedCellColor[cell] = element.substance.uiColour;
		}
		Game.CallbackInfo item = new Game.CallbackInfo(delegate
		{
			recentlyAffectedCells.Remove(cell);
			recentAffectedCellColor.Remove(cell);
		});
		int index = Game.Instance.callbackManager.Add(item).index;
		byte index2 = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
		Disease disease = Db.Get().Diseases.TryGet(settings.GetStringSetting("SandboxTools.SelectedDisease"));
		if (disease != null)
		{
			index2 = Db.Get().Diseases.GetIndex(disease.id);
		}
		int gameCell = cell;
		SimHashes id = element.id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float floatSetting = settings.GetFloatSetting("SandboxTools.Mass");
		float floatSetting2 = settings.GetFloatSetting("SandbosTools.Temperature");
		int callbackIdx = index;
		SimMessages.ReplaceElement(gameCell, id, sandBoxTool, floatSetting, floatSetting2, index2, settings.GetIntSetting("SandboxTools.DiseaseCount"), callbackIdx);
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(Action.SandboxCopyElement))
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

	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click"));
	}

	public override void OnLeftClickUp(Vector3 cursor_pos)
	{
		base.OnLeftClickUp(cursor_pos);
		StopSound();
	}

	private void OnElementChanged(object new_element)
	{
		clearVisitedCells();
	}

	protected override string GetDragSound()
	{
		string text = (ElementLoader.elements[settings.GetIntSetting("SandboxTools.SelectedElement")].state & Element.State.Solid).ToString();
		return "SandboxTool_Brush_" + text + "_Add";
	}

	protected override void PlaySound()
	{
		base.PlaySound();
		Element element = ElementLoader.elements[settings.GetIntSetting("SandboxTools.SelectedElement")];
		string sound;
		switch (element.state & Element.State.Solid)
		{
		case Element.State.Solid:
			sound = GlobalAssets.GetSound("Brush_" + element.substance.GetOreBumpSound());
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Brush_Rock");
			}
			break;
		case Element.State.Gas:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Gas");
			break;
		case Element.State.Vacuum:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Gas");
			break;
		case Element.State.Liquid:
			sound = GlobalAssets.GetSound("SandboxTool_Brush_Liquid");
			break;
		default:
			sound = GlobalAssets.GetSound("Brush_Rock");
			break;
		}
		audioEvent = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		audioEvent.set3DAttributes(attributes);
		audioEvent.start();
	}

	private void StopSound()
	{
		audioEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		audioEvent.release();
	}
}
