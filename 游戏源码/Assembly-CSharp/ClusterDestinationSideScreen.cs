using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F43 RID: 8003
public class ClusterDestinationSideScreen : SideScreenContent
{
	// Token: 0x17000ACE RID: 2766
	// (get) Token: 0x0600A8F0 RID: 43248 RVA: 0x0010DBF7 File Offset: 0x0010BDF7
	// (set) Token: 0x0600A8F1 RID: 43249 RVA: 0x0010DBFF File Offset: 0x0010BDFF
	private ClusterDestinationSelector targetSelector { get; set; }

	// Token: 0x17000ACF RID: 2767
	// (get) Token: 0x0600A8F2 RID: 43250 RVA: 0x0010DC08 File Offset: 0x0010BE08
	// (set) Token: 0x0600A8F3 RID: 43251 RVA: 0x0010DC10 File Offset: 0x0010BE10
	private RocketClusterDestinationSelector targetRocketSelector { get; set; }

	// Token: 0x0600A8F4 RID: 43252 RVA: 0x003FEC7C File Offset: 0x003FCE7C
	protected override void OnSpawn()
	{
		this.changeDestinationButton.onClick += this.OnClickChangeDestination;
		this.clearDestinationButton.onClick += this.OnClickClearDestination;
		this.launchPadDropDown.targetDropDownContainer = GameScreenManager.Instance.ssOverlayCanvas;
		this.launchPadDropDown.CustomizeEmptyRow(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE, null);
		this.repeatButton.onClick += this.OnRepeatClicked;
	}

	// Token: 0x0600A8F5 RID: 43253 RVA: 0x0010D82C File Offset: 0x0010BA2C
	public override int GetSideScreenSortOrder()
	{
		return 300;
	}

	// Token: 0x0600A8F6 RID: 43254 RVA: 0x003FECFC File Offset: 0x003FCEFC
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.Refresh(null);
			this.m_refreshHandle = this.targetSelector.Subscribe(543433792, delegate(object data)
			{
				this.Refresh(null);
			});
			return;
		}
		if (this.m_refreshHandle != -1)
		{
			this.targetSelector.Unsubscribe(this.m_refreshHandle);
			this.m_refreshHandle = -1;
			this.launchPadDropDown.Close();
		}
	}

	// Token: 0x0600A8F7 RID: 43255 RVA: 0x003FED6C File Offset: 0x003FCF6C
	public override bool IsValidForTarget(GameObject target)
	{
		ClusterDestinationSelector component = target.GetComponent<ClusterDestinationSelector>();
		return (component != null && component.assignable) || (target.GetComponent<RocketModule>() != null && target.HasTag(GameTags.LaunchButtonRocketModule)) || (target.GetComponent<RocketControlStation>() != null && target.GetComponent<RocketControlStation>().GetMyWorld().GetComponent<Clustercraft>().Status != Clustercraft.CraftStatus.Launching);
	}

	// Token: 0x0600A8F8 RID: 43256 RVA: 0x003FEDDC File Offset: 0x003FCFDC
	public override void SetTarget(GameObject target)
	{
		this.targetSelector = target.GetComponent<ClusterDestinationSelector>();
		if (this.targetSelector == null)
		{
			if (target.GetComponent<RocketModuleCluster>() != null)
			{
				this.targetSelector = target.GetComponent<RocketModuleCluster>().CraftInterface.GetClusterDestinationSelector();
			}
			else if (target.GetComponent<RocketControlStation>() != null)
			{
				this.targetSelector = target.GetMyWorld().GetComponent<Clustercraft>().ModuleInterface.GetClusterDestinationSelector();
			}
		}
		this.targetRocketSelector = (this.targetSelector as RocketClusterDestinationSelector);
	}

	// Token: 0x0600A8F9 RID: 43257 RVA: 0x003FEE64 File Offset: 0x003FD064
	private void Refresh(object data = null)
	{
		if (!this.targetSelector.IsAtDestination())
		{
			Sprite sprite;
			string str;
			string text;
			ClusterGrid.Instance.GetLocationDescription(this.targetSelector.GetDestination(), out sprite, out str, out text);
			this.destinationImage.sprite = sprite;
			this.destinationLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.TITLE + ": " + str;
			this.clearDestinationButton.isInteractable = true;
		}
		else
		{
			this.destinationImage.sprite = Assets.GetSprite("hex_unknown");
			this.destinationLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.TITLE + ": " + UI.SPACEDESTINATIONS.NONE.NAME;
			this.clearDestinationButton.isInteractable = false;
		}
		if (this.targetRocketSelector != null)
		{
			List<LaunchPad> launchPadsForDestination = LaunchPad.GetLaunchPadsForDestination(this.targetRocketSelector.GetDestination());
			this.launchPadDropDown.gameObject.SetActive(true);
			this.repeatButton.gameObject.SetActive(true);
			this.launchPadDropDown.Initialize(launchPadsForDestination, new Action<IListableOption, object>(this.OnLaunchPadEntryClick), new Func<IListableOption, IListableOption, object, int>(this.PadDropDownSort), new Action<DropDownEntry, object>(this.PadDropDownEntryRefreshAction), true, this.targetRocketSelector);
			if (!this.targetRocketSelector.IsAtDestination() && launchPadsForDestination.Count > 0)
			{
				this.launchPadDropDown.openButton.isInteractable = true;
				LaunchPad destinationPad = this.targetRocketSelector.GetDestinationPad();
				if (destinationPad != null)
				{
					this.launchPadDropDown.selectedLabel.text = destinationPad.GetProperName();
				}
				else
				{
					this.launchPadDropDown.selectedLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
				}
			}
			else
			{
				this.launchPadDropDown.selectedLabel.text = UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.FIRSTAVAILABLE;
				this.launchPadDropDown.openButton.isInteractable = false;
			}
			this.StyleRepeatButton();
		}
		else
		{
			this.launchPadDropDown.gameObject.SetActive(false);
			this.repeatButton.gameObject.SetActive(false);
		}
		this.StyleChangeDestinationButton();
	}

	// Token: 0x0600A8FA RID: 43258 RVA: 0x0010DC19 File Offset: 0x0010BE19
	private void OnClickChangeDestination()
	{
		if (this.targetSelector.assignable)
		{
			ClusterMapScreen.Instance.ShowInSelectDestinationMode(this.targetSelector);
		}
		this.StyleChangeDestinationButton();
	}

	// Token: 0x0600A8FB RID: 43259 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void StyleChangeDestinationButton()
	{
	}

	// Token: 0x0600A8FC RID: 43260 RVA: 0x0010DC3E File Offset: 0x0010BE3E
	private void OnClickClearDestination()
	{
		this.targetSelector.SetDestination(this.targetSelector.GetMyWorldLocation());
	}

	// Token: 0x0600A8FD RID: 43261 RVA: 0x003FF06C File Offset: 0x003FD26C
	private void OnLaunchPadEntryClick(IListableOption option, object data)
	{
		LaunchPad destinationPad = (LaunchPad)option;
		this.targetRocketSelector.SetDestinationPad(destinationPad);
	}

	// Token: 0x0600A8FE RID: 43262 RVA: 0x003FF08C File Offset: 0x003FD28C
	private void PadDropDownEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		LaunchPad launchPad = (LaunchPad)entry.entryData;
		Clustercraft component = this.targetRocketSelector.GetComponent<Clustercraft>();
		if (!(launchPad != null))
		{
			entry.button.isInteractable = true;
			entry.image.sprite = Assets.GetBuildingDef("LaunchPad").GetUISprite("ui", false);
			entry.tooltip.SetSimpleTooltip(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_FIRST_AVAILABLE);
			return;
		}
		string simpleTooltip;
		if (component.CanLandAtPad(launchPad, out simpleTooltip) == Clustercraft.PadLandingStatus.CanNeverLand)
		{
			entry.button.isInteractable = false;
			entry.image.sprite = Assets.GetSprite("iconWarning");
			entry.tooltip.SetSimpleTooltip(simpleTooltip);
			return;
		}
		entry.button.isInteractable = true;
		entry.image.sprite = launchPad.GetComponent<Building>().Def.GetUISprite("ui", false);
		entry.tooltip.SetSimpleTooltip(string.Format(UI.UISIDESCREENS.CLUSTERDESTINATIONSIDESCREEN.DROPDOWN_TOOLTIP_VALID_SITE, launchPad.GetProperName()));
	}

	// Token: 0x0600A8FF RID: 43263 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	private int PadDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return 0;
	}

	// Token: 0x0600A900 RID: 43264 RVA: 0x0010DC56 File Offset: 0x0010BE56
	private void OnRepeatClicked()
	{
		this.targetRocketSelector.Repeat = !this.targetRocketSelector.Repeat;
		this.StyleRepeatButton();
	}

	// Token: 0x0600A901 RID: 43265 RVA: 0x0010DC77 File Offset: 0x0010BE77
	private void StyleRepeatButton()
	{
		this.repeatButton.bgImage.colorStyleSetting = (this.targetRocketSelector.Repeat ? this.repeatOn : this.repeatOff);
		this.repeatButton.bgImage.ApplyColorStyleSetting();
	}

	// Token: 0x040084CD RID: 33997
	public Image destinationImage;

	// Token: 0x040084CE RID: 33998
	public LocText destinationLabel;

	// Token: 0x040084CF RID: 33999
	public KButton changeDestinationButton;

	// Token: 0x040084D0 RID: 34000
	public KButton clearDestinationButton;

	// Token: 0x040084D1 RID: 34001
	public DropDown launchPadDropDown;

	// Token: 0x040084D2 RID: 34002
	public KButton repeatButton;

	// Token: 0x040084D3 RID: 34003
	public ColorStyleSetting repeatOff;

	// Token: 0x040084D4 RID: 34004
	public ColorStyleSetting repeatOn;

	// Token: 0x040084D5 RID: 34005
	public ColorStyleSetting defaultButton;

	// Token: 0x040084D6 RID: 34006
	public ColorStyleSetting highlightButton;

	// Token: 0x040084D9 RID: 34009
	private int m_refreshHandle = -1;
}
