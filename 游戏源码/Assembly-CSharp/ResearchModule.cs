using System;
using TUNING;
using UnityEngine;

// Token: 0x02000F58 RID: 3928
[AddComponentMenu("KMonoBehaviour/scripts/ResearchModule")]
public class ResearchModule : KMonoBehaviour
{
	// Token: 0x06004F8D RID: 20365 RVA: 0x0026C4CC File Offset: 0x0026A6CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KBatchedAnimController>().Play("grounded", KAnim.PlayMode.Loop, 1f, 0f);
		base.Subscribe<ResearchModule>(-1277991738, ResearchModule.OnLaunchDelegate);
		base.Subscribe<ResearchModule>(-887025858, ResearchModule.OnLandDelegate);
	}

	// Token: 0x06004F8E RID: 20366 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnLaunch(object data)
	{
	}

	// Token: 0x06004F8F RID: 20367 RVA: 0x0026C524 File Offset: 0x0026A724
	public void OnLand(object data)
	{
		if (!DlcManager.FeatureClusterSpaceEnabled())
		{
			SpaceDestination.ResearchOpportunity researchOpportunity = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(base.GetComponent<RocketModule>().conditionManager.GetComponent<ILaunchableRocket>())).TryCompleteResearchOpportunity();
			if (researchOpportunity != null)
			{
				GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("ResearchDatabank"), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
				gameObject.SetActive(true);
				gameObject.GetComponent<PrimaryElement>().Mass = (float)researchOpportunity.dataValue;
				if (!string.IsNullOrEmpty(researchOpportunity.discoveredRareItem))
				{
					GameObject prefab = Assets.GetPrefab(researchOpportunity.discoveredRareItem);
					if (prefab == null)
					{
						KCrashReporter.Assert(false, "Missing prefab: " + researchOpportunity.discoveredRareItem, null);
					}
					else
					{
						GameUtil.KInstantiate(prefab, base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0).SetActive(true);
					}
				}
			}
		}
		GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab("ResearchDatabank"), base.gameObject.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
		gameObject2.SetActive(true);
		gameObject2.GetComponent<PrimaryElement>().Mass = (float)ROCKETRY.DESTINATION_RESEARCH.EVERGREEN;
	}

	// Token: 0x04003780 RID: 14208
	private static readonly EventSystem.IntraObjectHandler<ResearchModule> OnLaunchDelegate = new EventSystem.IntraObjectHandler<ResearchModule>(delegate(ResearchModule component, object data)
	{
		component.OnLaunch(data);
	});

	// Token: 0x04003781 RID: 14209
	private static readonly EventSystem.IntraObjectHandler<ResearchModule> OnLandDelegate = new EventSystem.IntraObjectHandler<ResearchModule>(delegate(ResearchModule component, object data)
	{
		component.OnLand(data);
	});
}
