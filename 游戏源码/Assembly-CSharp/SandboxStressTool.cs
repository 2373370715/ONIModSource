using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class SandboxStressTool : BrushTool
{
	public static SandboxStressTool instance;

	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	private string UISoundPath = GlobalAssets.GetSound("SandboxTool_Happy");

	private EventInstance ev;

	private Dictionary<MinionIdentity, AttributeModifier> moraleAdjustments = new Dictionary<MinionIdentity, AttributeModifier>();

	public override string[] DlcIDs => DlcManager.AVAILABLE_ALL_VERSIONS;

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

	protected override string GetDragSound()
	{
		return "";
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
		SandboxToolParameterMenu.instance.stressAdditiveSlider.row.SetActive(value: true);
		SandboxToolParameterMenu.instance.stressAdditiveSlider.SetValue(5f);
		SandboxToolParameterMenu.instance.moraleSlider.SetValue(0f);
		if (DebugHandler.InstantBuildMode)
		{
			SandboxToolParameterMenu.instance.moraleSlider.row.SetActive(value: true);
		}
	}

	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(value: false);
		StopSound();
	}

	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int recentlyAffectedCell in recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(recentlyAffectedCell, recentlyAffectedCellColor));
		}
		foreach (int item in cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(item, radiusIndicatorColor));
		}
	}

	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click"));
	}

	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			GameObject gameObject = Components.LiveMinionIdentities[i].gameObject;
			if (Grid.PosToCell(gameObject) != cell)
			{
				continue;
			}
			float num = -1f * SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.StressAdditive");
			Db.Get().Amounts.Stress.Lookup(Components.LiveMinionIdentities[i].gameObject).ApplyDelta(num);
			if (num >= 0f)
			{
				PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_angry"), GameUtil.GetFormattedPercent(num), gameObject.transform);
			}
			else
			{
				PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_happy"), GameUtil.GetFormattedPercent(num), gameObject.transform);
			}
			PlaySound(num, gameObject.transform.GetPosition());
			int intSetting = SandboxToolParameterMenu.instance.settings.GetIntSetting("SandbosTools.MoraleAdjustment");
			AttributeInstance attributeInstance = gameObject.GetAttributes().Get(Db.Get().Attributes.QualityOfLife);
			MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
			if (moraleAdjustments.ContainsKey(component))
			{
				attributeInstance.Remove(moraleAdjustments[component]);
				moraleAdjustments.Remove(component);
			}
			if (intSetting != 0)
			{
				AttributeModifier attributeModifier = new AttributeModifier(attributeInstance.Id, intSetting, () => DUPLICANTS.MODIFIERS.SANDBOXMORALEADJUSTMENT.NAME);
				attributeModifier.SetValue(intSetting);
				attributeInstance.Add(attributeModifier);
				moraleAdjustments.Add(component, attributeModifier);
			}
		}
	}

	private void PlaySound(float sliderValue, Vector3 position)
	{
		ev = KFMOD.CreateInstance(UISoundPath);
		ATTRIBUTES_3D attributes = position.To3DAttributes();
		ev.set3DAttributes(attributes);
		ev.setParameterByNameWithLabel("SanboxTool_Effect", (sliderValue >= 0f) ? "Decrease" : "Increase");
		ev.start();
	}

	private void StopSound()
	{
		ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		ev.release();
	}
}
