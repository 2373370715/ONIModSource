﻿using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class LaunchPadSideScreen : SideScreenContent
{
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.startNewRocketbutton.onClick += this.ClickStartNewRocket;
		this.devAutoRocketButton.onClick += this.ClickAutoRocket;
	}

	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
		}
	}

	public override int GetSideScreenSortOrder()
	{
		return 100;
	}

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LaunchPad>() != null;
	}

	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		if (this.refreshEventHandle != -1)
		{
			this.selectedPad.Unsubscribe(this.refreshEventHandle);
		}
		this.selectedPad = new_target.GetComponent<LaunchPad>();
		if (this.selectedPad == null)
		{
			global::Debug.LogError("The gameObject received does not contain a LaunchPad component");
			return;
		}
		this.refreshEventHandle = this.selectedPad.Subscribe(-887025858, new Action<object>(this.RefreshWaitingToLandList));
		this.RefreshRocketButton();
		this.RefreshWaitingToLandList(null);
	}

	private void RefreshWaitingToLandList(object data = null)
	{
		for (int i = this.waitingToLandRows.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.waitingToLandRows[i]);
		}
		this.waitingToLandRows.Clear();
		this.nothingWaitingRow.SetActive(true);
		AxialI myWorldLocation = this.selectedPad.GetMyWorldLocation();
		foreach (ClusterGridEntity clusterGridEntity in ClusterGrid.Instance.GetEntitiesInRange(myWorldLocation, 1))
		{
			Clustercraft craft = clusterGridEntity as Clustercraft;
			if (!(craft == null) && craft.Status == Clustercraft.CraftStatus.InFlight && (!craft.IsFlightInProgress() || !(craft.Destination != myWorldLocation)))
			{
				GameObject gameObject = Util.KInstantiateUI(this.landableRocketRowPrefab, this.landableRowContainer, true);
				gameObject.GetComponentInChildren<LocText>().text = craft.Name;
				this.waitingToLandRows.Add(gameObject);
				KButton componentInChildren = gameObject.GetComponentInChildren<KButton>();
				componentInChildren.GetComponentInChildren<LocText>().SetText((craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == this.selectedPad) ? UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.CANCEL_LAND_BUTTON : UI.UISIDESCREENS.LAUNCHPADSIDESCREEN.LAND_BUTTON);
				string simpleTooltip;
				componentInChildren.isInteractable = (craft.CanLandAtPad(this.selectedPad, out simpleTooltip) != Clustercraft.PadLandingStatus.CanNeverLand);
				if (!componentInChildren.isInteractable)
				{
					componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
				}
				else
				{
					componentInChildren.GetComponent<ToolTip>().ClearMultiStringTooltip();
				}
				componentInChildren.onClick += delegate()
				{
					if (craft.ModuleInterface.GetClusterDestinationSelector().GetDestinationPad() == this.selectedPad)
					{
						craft.GetComponent<ClusterDestinationSelector>().SetDestination(craft.Location);
					}
					else
					{
						craft.LandAtPad(this.selectedPad);
					}
					this.RefreshWaitingToLandList(null);
				};
				this.nothingWaitingRow.SetActive(false);
			}
		}
	}

	private void ClickStartNewRocket()
	{
		((SelectModuleSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.changeModuleSideScreen, UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.CHANGEMODULEPANEL)).SetLaunchPad(this.selectedPad);
	}

	private void RefreshRocketButton()
	{
		bool isOperational = this.selectedPad.GetComponent<Operational>().IsOperational;
		this.startNewRocketbutton.isInteractable = (this.selectedPad.LandedRocket == null && isOperational);
		if (!isOperational)
		{
			this.startNewRocketbutton.GetComponent<ToolTip>().SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_PAD_DISABLED);
		}
		else
		{
			this.startNewRocketbutton.GetComponent<ToolTip>().ClearMultiStringTooltip();
		}
		this.devAutoRocketButton.isInteractable = (this.selectedPad.LandedRocket == null);
		this.devAutoRocketButton.gameObject.SetActive(DebugHandler.InstantBuildMode);
	}

	private void ClickAutoRocket()
	{
		AutoRocketUtility.StartAutoRocket(this.selectedPad);
	}

	public GameObject content;

	private LaunchPad selectedPad;

	public LocText DescriptionText;

	public GameObject landableRocketRowPrefab;

	public GameObject newRocketPanel;

	public KButton startNewRocketbutton;

	public KButton devAutoRocketButton;

	public GameObject landableRowContainer;

	public GameObject nothingWaitingRow;

	public KScreen changeModuleSideScreen;

	private int refreshEventHandle = -1;

	public List<GameObject> waitingToLandRows = new List<GameObject>();
}
