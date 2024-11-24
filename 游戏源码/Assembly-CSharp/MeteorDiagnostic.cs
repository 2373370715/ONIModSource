using System.Collections.Generic;
using Klei.AI;
using STRINGS;

public class MeteorDiagnostic : ColonyDiagnostic
{
	public MeteorDiagnostic(int worldID)
		: base(worldID, UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.ALL_NAME)
	{
		icon = "meteors";
		AddCriterion("BombardmentUnderway", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.CRITERIA.CHECKUNDERWAY, CheckMeteorBombardment));
	}

	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	public DiagnosticResult CheckMeteorBombardment()
	{
		DiagnosticResult result = new DiagnosticResult(DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.NORMAL);
		List<GameplayEventInstance> results = new List<GameplayEventInstance>();
		GameplayEventManager.Instance.GetActiveEventsOfType<MeteorShowerEvent>(base.worldID, ref results);
		for (int i = 0; i < results.Count; i++)
		{
			if (results[i].smi is MeteorShowerEvent.StatesInstance statesInstance && statesInstance.IsInsideState(statesInstance.sm.running.bombarding))
			{
				result.opinion = DiagnosticResult.Opinion.Warning;
				result.Message = string.Format(UI.COLONY_DIAGNOSTICS.METEORDIAGNOSTIC.SHOWER_UNDERWAY, GameUtil.GetFormattedTime(statesInstance.BombardTimeRemaining()));
			}
		}
		return result;
	}
}
