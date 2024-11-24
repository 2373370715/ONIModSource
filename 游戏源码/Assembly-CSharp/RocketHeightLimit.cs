using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001854 RID: 6228
public class RocketHeightLimit : SelectModuleCondition
{
	// Token: 0x060080C4 RID: 32964 RVA: 0x00335C00 File Offset: 0x00333E00
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		int num = selectedPart.HeightInCells;
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule)
		{
			num -= existingModule.GetComponent<Building>().Def.HeightInCells;
		}
		if (existingModule == null)
		{
			return true;
		}
		RocketModuleCluster component = existingModule.GetComponent<RocketModuleCluster>();
		if (component == null)
		{
			return true;
		}
		int num2 = component.CraftInterface.MaxHeight;
		if (num2 <= 0)
		{
			num2 = ROCKETRY.ROCKET_HEIGHT.MAX_MODULE_STACK_HEIGHT;
		}
		RocketEngineCluster component2 = existingModule.GetComponent<RocketEngineCluster>();
		RocketEngineCluster component3 = selectedPart.BuildingComplete.GetComponent<RocketEngineCluster>();
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule && component2 != null)
		{
			if (component3 != null)
			{
				num2 = component3.maxHeight;
			}
			else
			{
				num2 = ROCKETRY.ROCKET_HEIGHT.MAX_MODULE_STACK_HEIGHT;
			}
		}
		if (component3 != null && selectionContext == SelectModuleCondition.SelectionContext.AddModuleBelow)
		{
			num2 = component3.maxHeight;
		}
		return num2 == -1 || component.CraftInterface.RocketHeight + num <= num2;
	}

	// Token: 0x060080C5 RID: 32965 RVA: 0x00335CC8 File Offset: 0x00333EC8
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		UnityEngine.Object engine = moduleBase.GetComponent<RocketModuleCluster>().CraftInterface.GetEngine();
		RocketEngineCluster component = selectedPart.BuildingComplete.GetComponent<RocketEngineCluster>();
		bool flag = engine != null || component != null;
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.COMPLETE;
		}
		if (flag)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.FAILED;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.MAX_HEIGHT.FAILED_NO_ENGINE;
	}
}
