using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001450 RID: 5200
public class SandboxStressTool : BrushTool
{
	// Token: 0x06006BE7 RID: 27623 RVA: 0x000E6E90 File Offset: 0x000E5090
	public static void DestroyInstance()
	{
		SandboxStressTool.instance = null;
	}

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06006BE8 RID: 27624 RVA: 0x000A6F3E File Offset: 0x000A513E
	public override string[] DlcIDs
	{
		get
		{
			return DlcManager.AVAILABLE_ALL_VERSIONS;
		}
	}

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06006BE9 RID: 27625 RVA: 0x000E67C3 File Offset: 0x000E49C3
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006BEA RID: 27626 RVA: 0x000E6E98 File Offset: 0x000E5098
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxStressTool.instance = this;
	}

	// Token: 0x06006BEB RID: 27627 RVA: 0x000CA99D File Offset: 0x000C8B9D
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06006BEC RID: 27628 RVA: 0x000E5D27 File Offset: 0x000E3F27
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006BED RID: 27629 RVA: 0x002E4848 File Offset: 0x002E2A48
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.stressAdditiveSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.stressAdditiveSlider.SetValue(5f, true);
		SandboxToolParameterMenu.instance.moraleSlider.SetValue(0f, true);
		if (DebugHandler.InstantBuildMode)
		{
			SandboxToolParameterMenu.instance.moraleSlider.row.SetActive(true);
		}
	}

	// Token: 0x06006BEE RID: 27630 RVA: 0x000E6EA6 File Offset: 0x000E50A6
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.StopSound();
	}

	// Token: 0x06006BEF RID: 27631 RVA: 0x002E48E8 File Offset: 0x002E2AE8
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06006BF0 RID: 27632 RVA: 0x000E68CA File Offset: 0x000E4ACA
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006BF1 RID: 27633 RVA: 0x000E6802 File Offset: 0x000E4A02
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06006BF2 RID: 27634 RVA: 0x002E49A0 File Offset: 0x002E2BA0
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			GameObject gameObject = Components.LiveMinionIdentities[i].gameObject;
			if (Grid.PosToCell(gameObject) == cell)
			{
				float num = -1f * SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.StressAdditive");
				Db.Get().Amounts.Stress.Lookup(Components.LiveMinionIdentities[i].gameObject).ApplyDelta(num);
				if (num >= 0f)
				{
					PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_angry"), GameUtil.GetFormattedPercent(num, GameUtil.TimeSlice.None), gameObject.transform, 1.5f, false);
				}
				else
				{
					PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_happy"), GameUtil.GetFormattedPercent(num, GameUtil.TimeSlice.None), gameObject.transform, 1.5f, false);
				}
				this.PlaySound(num, gameObject.transform.GetPosition());
				int intSetting = SandboxToolParameterMenu.instance.settings.GetIntSetting("SandbosTools.MoraleAdjustment");
				AttributeInstance attributeInstance = gameObject.GetAttributes().Get(Db.Get().Attributes.QualityOfLife);
				MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
				if (this.moraleAdjustments.ContainsKey(component))
				{
					attributeInstance.Remove(this.moraleAdjustments[component]);
					this.moraleAdjustments.Remove(component);
				}
				if (intSetting != 0)
				{
					AttributeModifier attributeModifier = new AttributeModifier(attributeInstance.Id, (float)intSetting, () => DUPLICANTS.MODIFIERS.SANDBOXMORALEADJUSTMENT.NAME, false, false);
					attributeModifier.SetValue((float)intSetting);
					attributeInstance.Add(attributeModifier);
					this.moraleAdjustments.Add(component, attributeModifier);
				}
			}
		}
	}

	// Token: 0x06006BF3 RID: 27635 RVA: 0x002E4B68 File Offset: 0x002E2D68
	private void PlaySound(float sliderValue, Vector3 position)
	{
		this.ev = KFMOD.CreateInstance(this.UISoundPath);
		ATTRIBUTES_3D attributes = position.To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setParameterByNameWithLabel("SanboxTool_Effect", (sliderValue >= 0f) ? "Decrease" : "Increase", false);
		this.ev.start();
	}

	// Token: 0x06006BF4 RID: 27636 RVA: 0x000E6EC5 File Offset: 0x000E50C5
	private void StopSound()
	{
		this.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

	// Token: 0x040050F9 RID: 20729
	public static SandboxStressTool instance;

	// Token: 0x040050FA RID: 20730
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x040050FB RID: 20731
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x040050FC RID: 20732
	private string UISoundPath = GlobalAssets.GetSound("SandboxTool_Happy", false);

	// Token: 0x040050FD RID: 20733
	private EventInstance ev;

	// Token: 0x040050FE RID: 20734
	private Dictionary<MinionIdentity, AttributeModifier> moraleAdjustments = new Dictionary<MinionIdentity, AttributeModifier>();
}
