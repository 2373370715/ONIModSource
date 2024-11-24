using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F35 RID: 7989
public class BionicSideScreen : SideScreenContent
{
	// Token: 0x0600A88E RID: 43150 RVA: 0x003FD778 File Offset: 0x003FB978
	private void OnBionicUpgradeSlotClicked(BionicSideScreenUpgradeSlot slotClicked)
	{
		bool flag = slotClicked == null || this.lastSlotSelected == slotClicked.upgradeSlot.GetAssignableSlotInstance();
		this.lastSlotSelected = (flag ? null : slotClicked.upgradeSlot.GetAssignableSlotInstance());
		this.RefreshSelectedStateInSlots();
		AssignableSlot bionicUpgrade = Db.Get().AssignableSlots.BionicUpgrade;
		AssignableSlotInstance assignableSlotInstance = flag ? null : slotClicked.upgradeSlot.GetAssignableSlotInstance();
		if (this.ownableSidescreen != null)
		{
			this.ownableSidescreen.SetSelectedSlot(assignableSlotInstance);
			return;
		}
		if (flag)
		{
			DetailsScreen.Instance.ClearSecondarySideScreen();
			return;
		}
		((OwnablesSecondSideScreen)DetailsScreen.Instance.SetSecondarySideScreen(this.ownableSecondSideScreenPrefab, bionicUpgrade.Name)).SetSlot(assignableSlotInstance);
	}

	// Token: 0x0600A88F RID: 43151 RVA: 0x003FD830 File Offset: 0x003FBA30
	private void RefreshSelectedStateInSlots()
	{
		for (int i = 0; i < this.bionicSlots.Count; i++)
		{
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			bionicSideScreenUpgradeSlot.SetSelected(bionicSideScreenUpgradeSlot.upgradeSlot.GetAssignableSlotInstance() == this.lastSlotSelected);
		}
	}

	// Token: 0x0600A890 RID: 43152 RVA: 0x003FD878 File Offset: 0x003FBA78
	public void RecreateBionicSlots()
	{
		int num = (this.upgradeMonitor != null) ? this.upgradeMonitor.def.SlotCount : 0;
		for (int i = 0; i < Mathf.Max(num, this.bionicSlots.Count); i++)
		{
			if (i >= this.bionicSlots.Count)
			{
				BionicSideScreenUpgradeSlot item = this.CreateBionicSlot();
				this.bionicSlots.Add(item);
			}
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			if (i < num)
			{
				BionicUpgradesMonitor.UpgradeComponentSlot upgradeSlot = this.upgradeMonitor.upgradeComponentSlots[i];
				bionicSideScreenUpgradeSlot.gameObject.SetActive(true);
				bionicSideScreenUpgradeSlot.Setup(upgradeSlot);
				bionicSideScreenUpgradeSlot.SetSelected(bionicSideScreenUpgradeSlot.upgradeSlot.GetAssignableSlotInstance() == this.lastSlotSelected);
			}
			else
			{
				bionicSideScreenUpgradeSlot.Setup(null);
				bionicSideScreenUpgradeSlot.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600A891 RID: 43153 RVA: 0x003FD948 File Offset: 0x003FBB48
	private BionicSideScreenUpgradeSlot CreateBionicSlot()
	{
		BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = Util.KInstantiateUI<BionicSideScreenUpgradeSlot>(this.originalBionicSlot.gameObject, this.originalBionicSlot.transform.parent.gameObject, false);
		bionicSideScreenUpgradeSlot.OnClick = (Action<BionicSideScreenUpgradeSlot>)Delegate.Combine(bionicSideScreenUpgradeSlot.OnClick, new Action<BionicSideScreenUpgradeSlot>(this.OnBionicUpgradeSlotClicked));
		return bionicSideScreenUpgradeSlot;
	}

	// Token: 0x0600A892 RID: 43154 RVA: 0x0010D7B6 File Offset: 0x0010B9B6
	private void OnBionicUpgradeChanged(object o)
	{
		this.RecreateBionicSlots();
	}

	// Token: 0x0600A893 RID: 43155 RVA: 0x0010D7BE File Offset: 0x0010B9BE
	private void OnBionicBecameOnline(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600A894 RID: 43156 RVA: 0x0010D7BE File Offset: 0x0010B9BE
	private void OnBionicBecameOffline(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600A895 RID: 43157 RVA: 0x0010D7BE File Offset: 0x0010B9BE
	private void OnBionicWattageChanged(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600A896 RID: 43158 RVA: 0x0010D7BE File Offset: 0x0010B9BE
	private void OnBionicBatterySaveModeChanged(object o)
	{
		this.RefreshSlots();
	}

	// Token: 0x0600A897 RID: 43159 RVA: 0x003FD9A0 File Offset: 0x003FBBA0
	private void RefreshSlots()
	{
		for (int i = 0; i < this.bionicSlots.Count; i++)
		{
			BionicSideScreenUpgradeSlot bionicSideScreenUpgradeSlot = this.bionicSlots[i];
			if (bionicSideScreenUpgradeSlot != null)
			{
				bionicSideScreenUpgradeSlot.Refresh();
			}
		}
	}

	// Token: 0x0600A898 RID: 43160 RVA: 0x003FD9E0 File Offset: 0x003FBBE0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.originalBionicSlot.gameObject.SetActive(false);
		this.ownableSidescreen = base.transform.parent.GetComponentInChildren<OwnablesSidescreen>();
		if (this.ownableSidescreen != null)
		{
			OwnablesSidescreen ownablesSidescreen = this.ownableSidescreen;
			ownablesSidescreen.OnSlotInstanceSelected = (Action<AssignableSlotInstance>)Delegate.Combine(ownablesSidescreen.OnSlotInstanceSelected, new Action<AssignableSlotInstance>(this.OnOwnableSidescreenRowSelected));
		}
	}

	// Token: 0x0600A899 RID: 43161 RVA: 0x0010D7C6 File Offset: 0x0010B9C6
	private void OnOwnableSidescreenRowSelected(AssignableSlotInstance slot)
	{
		this.lastSlotSelected = slot;
		this.RefreshSelectedStateInSlots();
	}

	// Token: 0x0600A89A RID: 43162 RVA: 0x003FDA50 File Offset: 0x003FBC50
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.lastSlotSelected = null;
		if (this.upgradeMonitor != null)
		{
			this.upgradeMonitor.Unsubscribe(160824499, new Action<object>(this.OnBionicBecameOnline));
			this.upgradeMonitor.Unsubscribe(-1730800797, new Action<object>(this.OnBionicBecameOffline));
			this.upgradeMonitor.Unsubscribe(2000325176, new Action<object>(this.OnBionicUpgradeChanged));
		}
		if (this.batteryMonitor != null)
		{
			this.batteryMonitor.Unsubscribe(1361471071, new Action<object>(this.OnBionicWattageChanged));
			this.batteryMonitor.Unsubscribe(-426516281, new Action<object>(this.OnBionicBatterySaveModeChanged));
		}
		this.batteryMonitor = target.GetSMI<BionicBatteryMonitor.Instance>();
		this.upgradeMonitor = target.GetSMI<BionicUpgradesMonitor.Instance>();
		this.upgradeMonitor.Subscribe(160824499, new Action<object>(this.OnBionicBecameOnline));
		this.upgradeMonitor.Subscribe(-1730800797, new Action<object>(this.OnBionicBecameOffline));
		this.upgradeMonitor.Subscribe(2000325176, new Action<object>(this.OnBionicUpgradeChanged));
		this.batteryMonitor.Subscribe(1361471071, new Action<object>(this.OnBionicWattageChanged));
		this.batteryMonitor.Subscribe(-426516281, new Action<object>(this.OnBionicBatterySaveModeChanged));
		this.RecreateBionicSlots();
	}

	// Token: 0x0600A89B RID: 43163 RVA: 0x0010D7D5 File Offset: 0x0010B9D5
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			this.RefreshSlots();
		}
	}

	// Token: 0x0600A89C RID: 43164 RVA: 0x0010D7E7 File Offset: 0x0010B9E7
	public override void ClearTarget()
	{
		base.ClearTarget();
		if (this.upgradeMonitor != null)
		{
			this.upgradeMonitor.Unsubscribe(2000325176, new Action<object>(this.OnBionicUpgradeChanged));
		}
		this.upgradeMonitor = null;
		this.lastSlotSelected = null;
	}

	// Token: 0x0600A89D RID: 43165 RVA: 0x0010D821 File Offset: 0x0010BA21
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetSMI<BionicBatteryMonitor.Instance>() != null;
	}

	// Token: 0x0600A89E RID: 43166 RVA: 0x0010D82C File Offset: 0x0010BA2C
	public override int GetSideScreenSortOrder()
	{
		return 300;
	}

	// Token: 0x04008487 RID: 33927
	public OwnablesSecondSideScreen ownableSecondSideScreenPrefab;

	// Token: 0x04008488 RID: 33928
	public BionicSideScreenUpgradeSlot originalBionicSlot;

	// Token: 0x04008489 RID: 33929
	private BionicUpgradesMonitor.Instance upgradeMonitor;

	// Token: 0x0400848A RID: 33930
	private BionicBatteryMonitor.Instance batteryMonitor;

	// Token: 0x0400848B RID: 33931
	private List<BionicSideScreenUpgradeSlot> bionicSlots = new List<BionicSideScreenUpgradeSlot>();

	// Token: 0x0400848C RID: 33932
	private OwnablesSidescreen ownableSidescreen;

	// Token: 0x0400848D RID: 33933
	private AssignableSlotInstance lastSlotSelected;
}
