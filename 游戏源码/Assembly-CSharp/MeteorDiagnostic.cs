using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x0200122E RID: 4654
public class MeteorDiagnostic : ColonyDiagnostic
{
	// Token: 0x06005F51 RID: 24401 RVA: 0x002A8D78 File Offset: 0x002A6F78
	public MeteorDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "meteors";
		base.AddCriterion("BombardmentUnderway", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.CRITERIA.CHECKUNDERWAY, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckMeteorBombardment)));
	}

	// Token: 0x06005F52 RID: 24402 RVA: 0x000A6F3E File Offset: 0x000A513E
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06005F53 RID: 24403 RVA: 0x002A8DC8 File Offset: 0x002A6FC8
	public ColonyDiagnostic.DiagnosticResult CheckMeteorBombardment()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.NORMAL, null);
		List<GameplayEventInstance> list = new List<GameplayEventInstance>();
		GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(base.worldID, ref list);
		for (int i = 0; i < list.Count; i++)
		{
			MeteorShowerEvent.StatesInstance statesInstance = list[i].smi as MeteorShowerEvent.StatesInstance;
			if (statesInstance != null && statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Warning;
				result.Message = string.Format(UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.SHOWER_UNDERWAY, GameUtil.GetFormattedTime(statesInstance.BombardTimeRemaining(), "F0"));
			}
		}
		return result;
	}
}
