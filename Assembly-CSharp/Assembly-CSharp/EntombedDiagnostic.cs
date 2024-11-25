﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class EntombedDiagnostic : ColonyDiagnostic
{
		public EntombedDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_action_dig";
		base.AddCriterion("CheckEntombed", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.CRITERIA.CHECKENTOMBED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEntombed)));
	}

		private ColonyDiagnostic.DiagnosticResult CheckEntombed()
	{
		List<BuildingComplete> worldItems = Components.EntombedBuildings.GetWorldItems(base.worldID, false);
		this.m_entombedCount = 0;
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Normal;
		result.Message = UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.NORMAL;
		foreach (BuildingComplete buildingComplete in worldItems)
		{
			if (!buildingComplete.IsNullOrDestroyed() && buildingComplete.prefabid.HasTag(GameTags.Entombed))
			{
				result.opinion = ColonyDiagnostic.DiagnosticResult.Opinion.Bad;
				result.Message = UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.BUILDING_ENTOMBED;
				result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(buildingComplete.gameObject.transform.position, buildingComplete.gameObject);
				this.m_entombedCount++;
			}
		}
		return result;
	}

		public override string GetAverageValueString()
	{
		return this.m_entombedCount.ToString();
	}

		private int m_entombedCount;
}
