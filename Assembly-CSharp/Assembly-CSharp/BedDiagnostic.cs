﻿using System;
using System.Collections.Generic;
using STRINGS;

public class BedDiagnostic : ColonyDiagnostic
{
	public BedDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_action_region_bedroom";
		base.AddCriterion("CheckEnoughBeds", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.CRITERIA.CHECKENOUGHBEDS, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEnoughBeds)));
	}

	private ColonyDiagnostic.DiagnosticResult CheckEnoughBeds()
	{
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.worldID, false);
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		if (worldItems.Count == 0)
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = base.NO_MINIONS;
		}
		else
		{
			result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
			result.Message = UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NORMAL;
			int num = 0;
			List<Sleepable> worldItems2 = Components.Sleepables.GetWorldItems(base.worldID, false);
			for (int i = 0; i < worldItems2.Count; i++)
			{
				if (worldItems2[i].GetComponent<Assignable>() != null && worldItems2[i].GetComponent<Clinic>() == null)
				{
					num++;
				}
			}
			if (num < worldItems.Count)
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Concern;
				result.Message = UI.COLONY_DIAGNOSTICS.BEDDIAGNOSTIC.NOT_ENOUGH_BEDS;
			}
		}
		return result;
	}

	public override ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, base.NO_MINIONS, null);
		if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(base.worldID, out result))
		{
			return result;
		}
		return base.Evaluate();
	}

	public override string GetAverageValueString()
	{
		return Components.Sleepables.GetWorldItems(base.worldID, false).FindAll((Sleepable match) => match.GetComponent<Assignable>() != null && match.GetComponent<Clinic>() == null).Count.ToString() + "/" + Components.LiveMinionIdentities.GetWorldItems(base.worldID, false).Count.ToString();
	}
}
