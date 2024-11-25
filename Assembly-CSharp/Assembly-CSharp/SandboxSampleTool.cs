using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using UnityEngine;

public class SandboxSampleTool : InterfaceTool
{
		public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		colors.Add(new ToolMenu.CellColorData(this.currentCell, this.radiusIndicatorColor));
	}

		public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.currentCell = Grid.PosToCell(cursorPos);
	}

		public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		int cell = Grid.PosToCell(cursor_pos);
		if (!Grid.IsValidCell(cell))
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.DEBUG_TOOLS.INVALID_LOCATION, null, cursor_pos, 1.5f, false, true);
			return;
		}
		SandboxSampleTool.Sample(cell);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
		this.PlaySound();
	}

		public static void Sample(int cell)
	{
		SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.SelectedElement", (int)Grid.Element[cell].idx);
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.Mass", Mathf.Round(Grid.Mass[cell] * 100f) / 100f);
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.Temperature", Mathf.Round(Grid.Temperature[cell] * 10f) / 10f);
		SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.DiseaseCount", Grid.DiseaseCount[cell]);
		SandboxToolParameterMenu.instance.RefreshDisplay();
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
		this.StopSound();
	}

		private void PlaySound()
	{
		Element element = ElementLoader.elements[SandboxToolParameterMenu.instance.settings.GetIntSetting("SandboxTools.SelectedElement")];
		float volume = 1f;
		float pitch = 1f;
		string sound = GlobalAssets.GetSound("Ore_bump_Rock", false);
		switch (element.state & Element.State.Solid)
		{
		case Element.State.Vacuum:
			sound = GlobalAssets.GetSound("ConduitBlob_Gas", false);
			break;
		case Element.State.Gas:
			sound = GlobalAssets.GetSound("ConduitBlob_Gas", false);
			break;
		case Element.State.Liquid:
			sound = GlobalAssets.GetSound("ConduitBlob_Liquid", false);
			break;
		case Element.State.Solid:
			sound = GlobalAssets.GetSound("Ore_bump_" + element.substance.GetMiningSound(), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Ore_bump_Rock", false);
			}
			volume = 0.7f;
			pitch = 2f;
			break;
		}
		this.ev = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setVolume(volume);
		this.ev.setPitch(pitch);
		this.ev.setParameterByName("blobCount", (float)UnityEngine.Random.Range(0, 6), false);
		this.ev.setParameterByName("SandboxToggle", 1f, false);
		this.ev.start();
	}

		private void StopSound()
	{
		this.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

		protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);

		private int currentCell;

		private EventInstance ev;
}
