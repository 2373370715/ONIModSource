using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001852 RID: 6226
public class TopOnly : SelectModuleCondition
{
	// Token: 0x060080BE RID: 32958 RVA: 0x0033595C File Offset: 0x00333B5C
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		global::Debug.Assert(existingModule != null, "Existing module is null in top only condition");
		if (selectionContext == SelectModuleCondition.SelectionContext.ReplaceModule)
		{
			global::Debug.Assert(existingModule.GetComponent<LaunchPad>() == null, "Trying to replace launch pad with rocket module");
			return existingModule.GetComponent<BuildingAttachPoint>() == null || existingModule.GetComponent<BuildingAttachPoint>().points[0].attachedBuilding == null;
		}
		return existingModule.GetComponent<LaunchPad>() != null || (existingModule.GetComponent<BuildingAttachPoint>() != null && existingModule.GetComponent<BuildingAttachPoint>().points[0].attachedBuilding == null);
	}

	// Token: 0x060080BF RID: 32959 RVA: 0x000F4B66 File Offset: 0x000F2D66
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.TOP_ONLY.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.TOP_ONLY.FAILED;
	}
}
