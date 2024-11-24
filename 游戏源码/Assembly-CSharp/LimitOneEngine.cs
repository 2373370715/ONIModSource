using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001850 RID: 6224
public class LimitOneEngine : SelectModuleCondition
{
	// Token: 0x060080B8 RID: 32952 RVA: 0x00335848 File Offset: 0x00333A48
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		if (existingModule == null)
		{
			return true;
		}
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(existingModule.GetComponent<AttachableBuilding>()))
		{
			if (selectionContext != SelectModuleCondition.SelectionContext.ReplaceModule || !(gameObject == existingModule.gameObject))
			{
				if (gameObject.GetComponent<RocketEngineCluster>() != null)
				{
					return false;
				}
				if (gameObject.GetComponent<BuildingUnderConstruction>() != null && gameObject.GetComponent<BuildingUnderConstruction>().Def.BuildingComplete.GetComponent<RocketEngineCluster>() != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x060080B9 RID: 32953 RVA: 0x000F4B32 File Offset: 0x000F2D32
	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_ENGINE_PER_ROCKET.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.ONE_ENGINE_PER_ROCKET.FAILED;
	}
}
