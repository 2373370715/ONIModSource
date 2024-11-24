using System;
using UnityEngine;

// Token: 0x0200184B RID: 6219
public abstract class SelectModuleCondition
{
	// Token: 0x060080A9 RID: 32937
	public abstract bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext);

	// Token: 0x060080AA RID: 32938
	public abstract string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart);

	// Token: 0x060080AB RID: 32939 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public virtual bool IgnoreInSanboxMode()
	{
		return false;
	}

	// Token: 0x0200184C RID: 6220
	public enum SelectionContext
	{
		// Token: 0x040061AA RID: 25002
		AddModuleAbove,
		// Token: 0x040061AB RID: 25003
		AddModuleBelow,
		// Token: 0x040061AC RID: 25004
		ReplaceModule
	}
}
