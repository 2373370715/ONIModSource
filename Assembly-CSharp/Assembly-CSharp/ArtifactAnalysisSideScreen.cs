﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactAnalysisSideScreen : SideScreenContent
{
	public override string GetTitle()
	{
		if (this.targetArtifactStation != null)
		{
			return string.Format(base.GetTitle(), this.targetArtifactStation.GetProperName());
		}
		return base.GetTitle();
	}

	public override void ClearTarget()
	{
		this.targetArtifactStation = null;
		base.ClearTarget();
	}

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<ArtifactAnalysisStation.StatesInstance>() != null;
	}

	private void RefreshRows()
	{
		if (this.undiscoveredRow == null)
		{
			this.undiscoveredRow = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
			HierarchyReferences component = this.undiscoveredRow.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("label").SetText(UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED);
			component.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.ARTIFACTANALYSISSIDESCREEN.NO_ARTIFACTS_DISCOVERED_TOOLTIP);
			component.GetReference<Image>("icon").sprite = Assets.GetSprite("unknown");
			component.GetReference<Image>("icon").color = Color.grey;
		}
		List<string> analyzedArtifactIDs = ArtifactSelector.Instance.GetAnalyzedArtifactIDs();
		this.undiscoveredRow.SetActive(analyzedArtifactIDs.Count == 0);
		foreach (string text in analyzedArtifactIDs)
		{
			if (!this.rows.ContainsKey(text))
			{
				GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer, true);
				this.rows.Add(text, gameObject);
				GameObject artifactPrefab = Assets.GetPrefab(text);
				HierarchyReferences component2 = gameObject.GetComponent<HierarchyReferences>();
				component2.GetReference<LocText>("label").SetText(artifactPrefab.GetProperName());
				component2.GetReference<Image>("icon").sprite = Def.GetUISprite(artifactPrefab, text, false).first;
				component2.GetComponent<KButton>().onClick += delegate()
				{
					this.OpenEvent(artifactPrefab);
				};
			}
		}
	}

	private void OpenEvent(GameObject artifactPrefab)
	{
		SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.ArtifactReveal, -1, null).smi as SimpleEvent.StatesInstance;
		statesInstance.artifact = artifactPrefab;
		artifactPrefab.GetComponent<KPrefabID>();
		artifactPrefab.GetComponent<InfoDescription>();
		string text = artifactPrefab.PrefabID().Name.ToUpper();
		text = text.Replace("ARTIFACT_", "");
		string key = "STRINGS.UI.SPACEARTIFACTS." + text + ".ARTIFACT";
		string text2 = string.Format("<b>{0}</b>", artifactPrefab.GetProperName());
		StringEntry stringEntry;
		Strings.TryGet(key, out stringEntry);
		if (stringEntry != null && !stringEntry.String.IsNullOrWhiteSpace())
		{
			text2 = text2 + "\n\n" + stringEntry.String;
		}
		if (text2 != null && !text2.IsNullOrWhiteSpace())
		{
			statesInstance.SetTextParameter("desc", text2);
		}
		statesInstance.ShowEventPopup();
	}

	public override void SetTarget(GameObject target)
	{
		this.targetArtifactStation = target;
		base.SetTarget(target);
		this.RefreshRows();
	}

	[SerializeField]
	private GameObject rowPrefab;

	private GameObject targetArtifactStation;

	[SerializeField]
	private GameObject rowContainer;

	private Dictionary<string, GameObject> rows = new Dictionary<string, GameObject>();

	private GameObject undiscoveredRow;
}
