using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001238 RID: 4664
public class TrappedDuplicantDiagnostic : ColonyDiagnostic
{
	// Token: 0x06005F78 RID: 24440 RVA: 0x002A9EA0 File Offset: 0x002A80A0
	public TrappedDuplicantDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.ALL_NAME)
	{
		this.icon = "overlay_power";
		base.AddCriterion("CheckTrapped", new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.CRITERIA.CHECKTRAPPED, new Func<ColonyDiagnostic.DiagnosticResult>(this.CheckTrapped)));
	}

	// Token: 0x06005F79 RID: 24441 RVA: 0x002A9EF0 File Offset: 0x002A80F0
	public ColonyDiagnostic.DiagnosticResult CheckTrapped()
	{
		ColonyDiagnostic.DiagnosticResult result = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS, null);
		bool flag = false;
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.worldID, false))
		{
			if (flag)
			{
				break;
			}
			if (!ClusterManager.Instance.GetWorld(base.worldID).IsModuleInterior && this.CheckMinionBasicallyIdle(minionIdentity))
			{
				Navigator component = minionIdentity.GetComponent<Navigator>();
				bool flag2 = true;
				foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.GetWorldItems(base.worldID, false))
				{
					if (!(minionIdentity == minionIdentity2) && !this.CheckMinionBasicallyIdle(minionIdentity2) && component.CanReach(minionIdentity2.GetComponent<IApproachable>()))
					{
						flag2 = false;
						break;
					}
				}
				List<Telepad> worldItems = Components.Telepads.GetWorldItems(component.GetMyWorld().id, false);
				if (worldItems != null && worldItems.Count > 0)
				{
					flag2 = (flag2 && !component.CanReach(worldItems[0].GetComponent<IApproachable>()));
				}
				List<WarpReceiver> worldItems2 = Components.WarpReceivers.GetWorldItems(component.GetMyWorld().id, false);
				if (worldItems2 != null && worldItems2.Count > 0)
				{
					foreach (WarpReceiver warpReceiver in worldItems2)
					{
						flag2 = (flag2 && !component.CanReach(worldItems2[0].GetComponent<IApproachable>()));
					}
				}
				foreach (Sleepable sleepable in Components.NormalBeds.WorldItemsEnumerate(component.GetMyWorldId(), true))
				{
					Assignable assignable = sleepable.assignable;
					if (assignable != null && assignable.IsAssignedTo(minionIdentity))
					{
						flag2 = (flag2 && !component.CanReach(sleepable.approachable));
					}
				}
				if (flag2)
				{
					result.clickThroughTarget = new global::Tuple<Vector3, GameObject>(minionIdentity.transform.position, minionIdentity.gameObject);
				}
				flag = (flag || flag2);
			}
		}
		result.opinion = (flag ? ColonyDiagnostic.DiagnosticResult.Opinion.Bad : ColonyDiagnostic.DiagnosticResult.Opinion.Normal);
		result.Message = (flag ? UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.STUCK : UI.COLONY_DIAGNOSTICS.TRAPPEDDUPLICANTDIAGNOSTIC.NORMAL);
		return result;
	}

	// Token: 0x06005F7A RID: 24442 RVA: 0x002AA1D0 File Offset: 0x002A83D0
	private bool CheckMinionBasicallyIdle(MinionIdentity minion)
	{
		KPrefabID component = minion.GetComponent<KPrefabID>();
		return component.HasTag(GameTags.Idle) || component.HasTag(GameTags.RecoveringBreath) || component.HasTag(GameTags.MakingMess);
	}
}
