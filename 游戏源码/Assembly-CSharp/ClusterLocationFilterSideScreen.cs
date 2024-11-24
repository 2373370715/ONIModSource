using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F45 RID: 8005
public class ClusterLocationFilterSideScreen : SideScreenContent
{
	// Token: 0x0600A909 RID: 43273 RVA: 0x0010DCF3 File Offset: 0x0010BEF3
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicClusterLocationSensor>() != null;
	}

	// Token: 0x0600A90A RID: 43274 RVA: 0x0010DD01 File Offset: 0x0010BF01
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.sensor = target.GetComponent<LogicClusterLocationSensor>();
		this.Build();
	}

	// Token: 0x0600A90B RID: 43275 RVA: 0x003FF274 File Offset: 0x003FD474
	private void ClearRows()
	{
		if (this.emptySpaceRow != null)
		{
			Util.KDestroyGameObject(this.emptySpaceRow);
		}
		foreach (KeyValuePair<AxialI, GameObject> keyValuePair in this.worldRows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.worldRows.Clear();
	}

	// Token: 0x0600A90C RID: 43276 RVA: 0x003FF2F0 File Offset: 0x003FD4F0
	private void Build()
	{
		this.headerLabel.SetText(UI.UISIDESCREENS.CLUSTERLOCATIONFILTERSIDESCREEN.HEADER);
		this.ClearRows();
		this.emptySpaceRow = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
		this.emptySpaceRow.SetActive(true);
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (!worldContainer.IsModuleInterior)
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
				gameObject.gameObject.name = worldContainer.GetProperName();
				AxialI myWorldLocation = worldContainer.GetMyWorldLocation();
				global::Debug.Assert(!this.worldRows.ContainsKey(myWorldLocation), "Adding two worlds/POI with the same cluster location to ClusterLocationFilterSideScreen UI: " + worldContainer.GetProperName());
				this.worldRows.Add(myWorldLocation, gameObject);
			}
		}
		this.Refresh();
	}

	// Token: 0x0600A90D RID: 43277 RVA: 0x003FF3E4 File Offset: 0x003FD5E4
	private void Refresh()
	{
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(UI.UISIDESCREENS.CLUSTERLOCATIONFILTERSIDESCREEN.EMPTY_SPACE_ROW);
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite("hex_soft", "ui", false).first;
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Color.black;
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
		{
			this.sensor.SetSpaceEnabled(!this.sensor.ActiveInSpace);
			this.Refresh();
		};
		this.emptySpaceRow.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.sensor.ActiveInSpace ? 1 : 0);
		using (Dictionary<AxialI, GameObject>.Enumerator enumerator = this.worldRows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<AxialI, GameObject> kvp = enumerator.Current;
				ClusterGridEntity clusterGridEntity = ClusterGrid.Instance.cellContents[kvp.Key][0];
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(clusterGridEntity.GetProperName());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(clusterGridEntity, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(clusterGridEntity, "ui", false).second;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.sensor.SetLocationEnabled(kvp.Key, !this.sensor.CheckLocationSelected(kvp.Key));
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.sensor.CheckLocationSelected(kvp.Key) ? 1 : 0);
				kvp.Value.SetActive(ClusterGrid.Instance.GetCellRevealLevel(kvp.Key) == ClusterRevealLevel.Visible);
			}
		}
	}

	// Token: 0x040084DD RID: 34013
	private LogicClusterLocationSensor sensor;

	// Token: 0x040084DE RID: 34014
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x040084DF RID: 34015
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x040084E0 RID: 34016
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x040084E1 RID: 34017
	private Dictionary<AxialI, GameObject> worldRows = new Dictionary<AxialI, GameObject>();

	// Token: 0x040084E2 RID: 34018
	private GameObject emptySpaceRow;
}
