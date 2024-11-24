using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F47 RID: 8007
public class CometDetectorSideScreen : SideScreenContent
{
	// Token: 0x0600A912 RID: 43282 RVA: 0x0010DD50 File Offset: 0x0010BF50
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshOptions();
		}
	}

	// Token: 0x0600A913 RID: 43283 RVA: 0x003FF6A0 File Offset: 0x003FD8A0
	private void RefreshOptions()
	{
		if (this.clusterDetector != null)
		{
			int num = 0;
			this.SetClusterRow(num++, UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.COMETS, Assets.GetSprite("meteors"), ClusterCometDetector.Instance.ClusterCometDetectorState.MeteorShower, null);
			this.SetClusterRow(num++, UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.DUPEMADE, Assets.GetSprite("dupe_made_ballistics"), ClusterCometDetector.Instance.ClusterCometDetectorState.BallisticObject, null);
			foreach (object obj in Components.Clustercrafts)
			{
				Clustercraft clustercraft = (Clustercraft)obj;
				this.SetClusterRow(num++, clustercraft.Name, Assets.GetSprite("rocket_landing"), ClusterCometDetector.Instance.ClusterCometDetectorState.Rocket, clustercraft);
			}
			for (int i = num; i < this.rowContainer.childCount; i++)
			{
				this.rowContainer.GetChild(i).gameObject.SetActive(false);
			}
			return;
		}
		int num2 = 0;
		this.SetRow(num2++, UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.COMETS, Assets.GetSprite("meteors"), null);
		foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
		{
			this.SetRow(num2++, spacecraft.GetRocketName(), Assets.GetSprite("rocket_landing"), spacecraft.launchConditions);
		}
		for (int j = num2; j < this.rowContainer.childCount; j++)
		{
			this.rowContainer.GetChild(j).gameObject.SetActive(false);
		}
	}

	// Token: 0x0600A914 RID: 43284 RVA: 0x003FF868 File Offset: 0x003FDA68
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x0600A915 RID: 43285 RVA: 0x0010DD62 File Offset: 0x0010BF62
	public override void SetTarget(GameObject target)
	{
		if (DlcManager.IsExpansion1Active())
		{
			this.clusterDetector = target.GetSMI<ClusterCometDetector.Instance>();
		}
		else
		{
			this.detector = target.GetSMI<CometDetector.Instance>();
		}
		this.RefreshOptions();
	}

	// Token: 0x0600A916 RID: 43286 RVA: 0x003FF8AC File Offset: 0x003FDAAC
	private void SetClusterRow(int idx, string name, Sprite icon, ClusterCometDetector.Instance.ClusterCometDetectorState state, Clustercraft rocketTarget = null)
	{
		GameObject gameObject;
		if (idx < this.rowContainer.childCount)
		{
			gameObject = this.rowContainer.GetChild(idx).gameObject;
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		}
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("label").text = name;
		component.GetReference<Image>("icon").sprite = icon;
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		component2.ChangeState((this.clusterDetector.GetDetectorState() == state && this.clusterDetector.GetClustercraftTarget() == rocketTarget) ? 1 : 0);
		ClusterCometDetector.Instance.ClusterCometDetectorState _state = state;
		Clustercraft _rocketTarget = rocketTarget;
		component2.onClick = delegate()
		{
			this.clusterDetector.SetDetectorState(_state);
			this.clusterDetector.SetClustercraftTarget(_rocketTarget);
			this.RefreshOptions();
		};
	}

	// Token: 0x0600A917 RID: 43287 RVA: 0x003FF97C File Offset: 0x003FDB7C
	private void SetRow(int idx, string name, Sprite icon, LaunchConditionManager target)
	{
		GameObject gameObject;
		if (idx < this.rowContainer.childCount)
		{
			gameObject = this.rowContainer.GetChild(idx).gameObject;
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		}
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("label").text = name;
		component.GetReference<Image>("icon").sprite = icon;
		MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
		component2.ChangeState((this.detector.GetTargetCraft() == target) ? 1 : 0);
		LaunchConditionManager _target = target;
		component2.onClick = delegate()
		{
			this.detector.SetTargetCraft(_target);
			this.RefreshOptions();
		};
	}

	// Token: 0x0600A918 RID: 43288 RVA: 0x0010DD8B File Offset: 0x0010BF8B
	public override bool IsValidForTarget(GameObject target)
	{
		if (DlcManager.IsExpansion1Active())
		{
			return target.GetSMI<ClusterCometDetector.Instance>() != null;
		}
		return target.GetSMI<CometDetector.Instance>() != null;
	}

	// Token: 0x040084E5 RID: 34021
	private CometDetector.Instance detector;

	// Token: 0x040084E6 RID: 34022
	private ClusterCometDetector.Instance clusterDetector;

	// Token: 0x040084E7 RID: 34023
	public GameObject rowPrefab;

	// Token: 0x040084E8 RID: 34024
	public RectTransform rowContainer;

	// Token: 0x040084E9 RID: 34025
	public Dictionary<object, GameObject> rows = new Dictionary<object, GameObject>();
}
