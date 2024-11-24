using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200184D RID: 6221
public class ResearchCompleted : SelectModuleCondition
{
	// Token: 0x060080AD RID: 32941 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IgnoreInSanboxMode()
	{
		return true;
	}

	// Token: 0x060080AE RID: 32942 RVA: 0x0033569C File Offset: 0x0033389C
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null)
		{
			return true;
		}
		TechItem techItem = Db.Get().TechItems.TryGet(selectedPart.PrefabID);
		return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || techItem == null || techItem.IsComplete();
	}

	// Token: 0x060080AF RID: 32943 RVA: 0x000F4ADE File Offset: 0x000F2CDE
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.RESEARCHED.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.RESEARCHED.FAILED;
	}
}
