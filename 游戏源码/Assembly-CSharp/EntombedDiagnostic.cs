using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001226 RID: 4646
public class EntombedDiagnostic : ColonyDiagnostic
{
	// Token: 0x06005F33 RID: 24371 RVA: 0x002A8164 File Offset: 0x002A6364
	public EntombedDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "icon_action_dig";
		base.AddCriterion("CheckEntombed", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.ENTOMBEDDIAGNOSTIC.CRITERIA.CHECKENTOMBED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckEntombed)));
	}

	// Token: 0x06005F34 RID: 24372 RVA: 0x002A81B4 File Offset: 0x002A63B4
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

	// Token: 0x06005F35 RID: 24373 RVA: 0x000DE27C File Offset: 0x000DC47C
	public override string GetAverageValueString()
	{
		return this.m_entombedCount.ToString();
	}

	// Token: 0x040043B3 RID: 17331
	private int m_entombedCount;
}
