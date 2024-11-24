using System.Collections.Generic;
using STRINGS;

public class IdleDiagnostic : ColonyDiagnostic
{
	public IdleDiagnostic(int worldID)
		: base(worldID, UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.ALL_NAME)
	{
		tracker = TrackerTool.Instance.GetWorldTracker<IdleTracker>(worldID);
		icon = "icon_errand_operate";
		AddCriterion("CheckIdle", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.CRITERIA.CHECKIDLE, CheckIdle));
	}

	private DiagnosticResult CheckIdle()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID);
		DiagnosticResult result = new DiagnosticResult(DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
		if (worldItems.Count == 0)
		{
			result.opinion = DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
		}
		else
		{
			result.opinion = DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.NORMAL;
			if (tracker.GetMinValue(5f) > 0f && tracker.GetCurrentValue() > 0f)
			{
				result.opinion = DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.IDLEDIAGNOSTIC.IDLE;
				result.clickThroughObjects = tracker.objectsOfInterest;
			}
		}
		return result;
	}
}
