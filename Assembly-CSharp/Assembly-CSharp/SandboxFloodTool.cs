using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using UnityEngine;

public class SandboxFloodTool : FloodTool
{
		public static void DestroyInstance()
	{
		SandboxFloodTool.instance = null;
	}

		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxFloodTool.instance = this;
		this.floodCriteria = ((int cell) => Grid.IsValidCell(cell) && Grid.Element[cell] == Grid.Element[this.mouseCell] && Grid.WorldIdx[cell] == Grid.WorldIdx[this.mouseCell]);
		this.paintArea = delegate(HashSet<int> cells)
		{
			foreach (int cell in cells)
			{
				this.PaintCell(cell);
			}
		};
	}

		private void PaintCell(int cell)
	{
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
		Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
		if (disease != null)
		{
			index = Db.Get().Diseases.GetIndex(disease.id);
		}
		int index2 = Game.Instance.callbackManager.Add(item).index;
		int cell2 = cell;
		SimHashes id = element.id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float floatSetting = this.settings.GetFloatSetting("SandboxTools.Mass");
		float floatSetting2 = this.settings.GetFloatSetting("SandbosTools.Temperature");
		int callbackIdx = index2;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, floatSetting, floatSetting2, index, this.settings.GetIntSetting("SandboxTools.DiseaseCount"), callbackIdx);
	}

			private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

		public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

		protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
	}

		protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.ev.release();
	}

		public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsToAffect)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.areaColour));
		}
	}

		public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.cellsToAffect = base.Flood(Grid.PosToCell(cursorPos));
	}

		public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		string sound;
		if (element.IsSolid)
		{
			sound = GlobalAssets.GetSound("Break_" + element.substance.GetMiningBreakSound(), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Break_Rock", false);
			}
		}
		else if (element.IsGas)
		{
			sound = GlobalAssets.GetSound("SandboxTool_Bucket_Gas", false);
		}
		else if (element.IsLiquid)
		{
			sound = GlobalAssets.GetSound("SandboxTool_Bucket_Liquid", false);
		}
		else
		{
			sound = GlobalAssets.GetSound("Break_Rock", false);
		}
		this.ev = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setParameterByName("SandboxToggle", 1f, false);
		this.ev.start();
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Bucket", false));
	}

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

		public static SandboxFloodTool instance;

		protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

		protected HashSet<int> cellsToAffect = new HashSet<int>();

		protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

		private EventInstance ev;
}
