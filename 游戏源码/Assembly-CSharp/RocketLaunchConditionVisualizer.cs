using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AEB RID: 2795
public class RocketLaunchConditionVisualizer : KMonoBehaviour
{
	// Token: 0x06003468 RID: 13416 RVA: 0x00209DAC File Offset: 0x00207FAC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			this.clusterModule = base.GetComponent<RocketModuleCluster>();
		}
		else
		{
			this.launchConditionManager = base.GetComponent<LaunchConditionManager>();
		}
		this.UpdateAllModuleData();
		base.Subscribe(1512695988, new Action<object>(this.OnAnyRocketModuleChanged));
	}

	// Token: 0x06003469 RID: 13417 RVA: 0x000C22DD File Offset: 0x000C04DD
	protected override void OnCleanUp()
	{
		base.Unsubscribe(1512695988, new Action<object>(this.OnAnyRocketModuleChanged));
	}

	// Token: 0x0600346A RID: 13418 RVA: 0x000C22F6 File Offset: 0x000C04F6
	private void OnAnyRocketModuleChanged(object obj)
	{
		this.UpdateAllModuleData();
	}

	// Token: 0x0600346B RID: 13419 RVA: 0x00209E00 File Offset: 0x00208000
	private void UpdateAllModuleData()
	{
		if (this.moduleVisualizeData != null)
		{
			this.moduleVisualizeData = null;
		}
		bool flag = this.clusterModule != null;
		List<Ref<RocketModuleCluster>> list = null;
		List<RocketModule> list2 = null;
		if (flag)
		{
			list = new List<Ref<RocketModuleCluster>>(this.clusterModule.CraftInterface.ClusterModules);
			this.moduleVisualizeData = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData[list.Count];
			list.Sort(delegate(Ref<RocketModuleCluster> a, Ref<RocketModuleCluster> b)
			{
				int y = Grid.PosToXY(a.Get().transform.GetPosition()).y;
				int y2 = Grid.PosToXY(b.Get().transform.GetPosition()).y;
				return y.CompareTo(y2);
			});
		}
		else
		{
			list2 = new List<RocketModule>(this.launchConditionManager.rocketModules);
			list2.Sort(delegate(RocketModule a, RocketModule b)
			{
				int y = Grid.PosToXY(a.transform.GetPosition()).y;
				int y2 = Grid.PosToXY(b.transform.GetPosition()).y;
				return y.CompareTo(y2);
			});
			this.moduleVisualizeData = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData[list2.Count];
		}
		for (int i = 0; i < this.moduleVisualizeData.Length; i++)
		{
			RocketModule rocketModule = flag ? list[i].Get() : list2[i];
			Building component = rocketModule.GetComponent<Building>();
			this.moduleVisualizeData[i] = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData
			{
				Module = rocketModule,
				RangeMax = Mathf.FloorToInt((float)component.Def.WidthInCells / 2f),
				RangeMin = -Mathf.FloorToInt((float)(component.Def.WidthInCells - 1) / 2f)
			};
		}
	}

	// Token: 0x04002368 RID: 9064
	public RocketLaunchConditionVisualizer.RocketModuleVisualizeData[] moduleVisualizeData;

	// Token: 0x04002369 RID: 9065
	private LaunchConditionManager launchConditionManager;

	// Token: 0x0400236A RID: 9066
	private RocketModuleCluster clusterModule;

	// Token: 0x02000AEC RID: 2796
	public struct RocketModuleVisualizeData
	{
		// Token: 0x0400236B RID: 9067
		public RocketModule Module;

		// Token: 0x0400236C RID: 9068
		public Vector2I OriginOffset;

		// Token: 0x0400236D RID: 9069
		public int RangeMin;

		// Token: 0x0400236E RID: 9070
		public int RangeMax;
	}
}
