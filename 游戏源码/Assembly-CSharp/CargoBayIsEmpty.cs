using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001978 RID: 6520
public class CargoBayIsEmpty : ProcessCondition
{
	// Token: 0x060087E8 RID: 34792 RVA: 0x000F8E4A File Offset: 0x000F704A
	public CargoBayIsEmpty(CommandModule module)
	{
		this.commandModule = module;
	}

	// Token: 0x060087E9 RID: 34793 RVA: 0x00352368 File Offset: 0x00350568
	public override ProcessCondition.Status EvaluateCondition()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			CargoBay component = gameObject.GetComponent<CargoBay>();
			if (component != null && component.storage.MassStored() != 0f)
			{
				return ProcessCondition.Status.Failure;
			}
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x060087EA RID: 34794 RVA: 0x000F8E59 File Offset: 0x000F7059
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return UI.STARMAP.CARGOEMPTY.NAME;
	}

	// Token: 0x060087EB RID: 34795 RVA: 0x000F8E65 File Offset: 0x000F7065
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return UI.STARMAP.CARGOEMPTY.TOOLTIP;
	}

	// Token: 0x060087EC RID: 34796 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006681 RID: 26241
	private CommandModule commandModule;
}
