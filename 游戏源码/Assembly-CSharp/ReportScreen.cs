using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001EC1 RID: 7873
public class ReportScreen : KScreen
{
	// Token: 0x17000A9E RID: 2718
	// (get) Token: 0x0600A56A RID: 42346 RVA: 0x0010B589 File Offset: 0x00109789
	// (set) Token: 0x0600A56B RID: 42347 RVA: 0x0010B590 File Offset: 0x00109790
	public static ReportScreen Instance { get; private set; }

	// Token: 0x0600A56C RID: 42348 RVA: 0x0010B598 File Offset: 0x00109798
	public static void DestroyInstance()
	{
		ReportScreen.Instance = null;
	}

	// Token: 0x0600A56D RID: 42349 RVA: 0x003EC624 File Offset: 0x003EA824
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ReportScreen.Instance = this;
		this.closeButton.onClick += delegate()
		{
			ManagementMenu.Instance.CloseAll();
		};
		this.prevButton.onClick += delegate()
		{
			this.ShowReport(this.currentReport.day - 1);
		};
		this.nextButton.onClick += delegate()
		{
			this.ShowReport(this.currentReport.day + 1);
		};
		this.summaryButton.onClick += delegate()
		{
			RetiredColonyData currentColonyRetiredColonyData = RetireColonyUtility.GetCurrentColonyRetiredColonyData();
			MainMenu.ActivateRetiredColoniesScreenFromData(PauseScreen.Instance.transform.parent.gameObject, currentColonyRetiredColonyData);
		};
		base.ConsumeMouseScroll = true;
	}

	// Token: 0x0600A56E RID: 42350 RVA: 0x0010197B File Offset: 0x000FFB7B
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600A56F RID: 42351 RVA: 0x0010B5A0 File Offset: 0x001097A0
	protected override void OnShow(bool bShow)
	{
		base.OnShow(bShow);
		if (ReportManager.Instance != null)
		{
			this.currentReport = ReportManager.Instance.TodaysReport;
		}
	}

	// Token: 0x0600A570 RID: 42352 RVA: 0x0010B5C6 File Offset: 0x001097C6
	public void SetTitle(string title)
	{
		this.title.text = title;
	}

	// Token: 0x0600A571 RID: 42353 RVA: 0x0010B5D4 File Offset: 0x001097D4
	public override void ScreenUpdate(bool b)
	{
		base.ScreenUpdate(b);
		this.Refresh();
	}

	// Token: 0x0600A572 RID: 42354 RVA: 0x003EC6C8 File Offset: 0x003EA8C8
	private void Refresh()
	{
		global::Debug.Assert(this.currentReport != null);
		if (this.currentReport.day == ReportManager.Instance.TodaysReport.day)
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE_TODAY, this.currentReport.day));
		}
		else if (this.currentReport.day == ReportManager.Instance.TodaysReport.day - 1)
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE_YESTERDAY, this.currentReport.day));
		}
		else
		{
			this.SetTitle(string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day));
		}
		bool flag = this.currentReport.day < ReportManager.Instance.TodaysReport.day;
		this.nextButton.isInteractable = flag;
		if (flag)
		{
			this.nextButton.GetComponent<ToolTip>().toolTip = string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day + 1);
			this.nextButton.GetComponent<ToolTip>().enabled = true;
		}
		else
		{
			this.nextButton.GetComponent<ToolTip>().enabled = false;
		}
		flag = (this.currentReport.day > 1);
		this.prevButton.isInteractable = flag;
		if (flag)
		{
			this.prevButton.GetComponent<ToolTip>().toolTip = string.Format(UI.ENDOFDAYREPORT.DAY_TITLE, this.currentReport.day - 1);
			this.prevButton.GetComponent<ToolTip>().enabled = true;
		}
		else
		{
			this.prevButton.GetComponent<ToolTip>().enabled = false;
		}
		this.AddSpacer(0);
		int num = 1;
		foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> keyValuePair in ReportManager.Instance.ReportGroups)
		{
			ReportManager.ReportEntry entry = this.currentReport.GetEntry(keyValuePair.Key);
			if (num != keyValuePair.Value.group)
			{
				num = keyValuePair.Value.group;
				this.AddSpacer(num);
			}
			bool flag2 = entry.accumulate != 0f || keyValuePair.Value.reportIfZero;
			if (keyValuePair.Value.isHeader)
			{
				this.CreateHeader(keyValuePair.Value);
			}
			else if (flag2)
			{
				this.CreateOrUpdateLine(entry, keyValuePair.Value, flag2);
			}
		}
	}

	// Token: 0x0600A573 RID: 42355 RVA: 0x0010B5E3 File Offset: 0x001097E3
	public void ShowReport(int day)
	{
		this.currentReport = ReportManager.Instance.FindReport(day);
		global::Debug.Assert(this.currentReport != null, "Can't find report for day: " + day.ToString());
		this.Refresh();
	}

	// Token: 0x0600A574 RID: 42356 RVA: 0x003EC964 File Offset: 0x003EAB64
	private GameObject AddSpacer(int group)
	{
		GameObject gameObject;
		if (this.lineItems.ContainsKey(group.ToString()))
		{
			gameObject = this.lineItems[group.ToString()];
		}
		else
		{
			gameObject = Util.KInstantiateUI(this.lineItemSpacer, this.contentFolder, false);
			gameObject.name = "Spacer" + group.ToString();
			this.lineItems[group.ToString()] = gameObject;
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600A575 RID: 42357 RVA: 0x003EC9E4 File Offset: 0x003EABE4
	private GameObject CreateHeader(ReportManager.ReportGroup reportGroup)
	{
		GameObject gameObject = null;
		this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
		if (gameObject == null)
		{
			gameObject = Util.KInstantiateUI(this.lineItemHeader, this.contentFolder, true);
			gameObject.name = "LineItemHeader" + this.lineItems.Count.ToString();
			this.lineItems[reportGroup.stringKey] = gameObject;
		}
		gameObject.SetActive(true);
		gameObject.GetComponent<ReportScreenHeader>().SetMainEntry(reportGroup);
		return gameObject;
	}

	// Token: 0x0600A576 RID: 42358 RVA: 0x003ECA6C File Offset: 0x003EAC6C
	private GameObject CreateOrUpdateLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, bool is_line_active)
	{
		GameObject gameObject = null;
		this.lineItems.TryGetValue(reportGroup.stringKey, out gameObject);
		if (!is_line_active)
		{
			if (gameObject != null && gameObject.activeSelf)
			{
				gameObject.SetActive(false);
			}
		}
		else
		{
			if (gameObject == null)
			{
				gameObject = Util.KInstantiateUI(this.lineItem, this.contentFolder, true);
				gameObject.name = "LineItem" + this.lineItems.Count.ToString();
				this.lineItems[reportGroup.stringKey] = gameObject;
			}
			gameObject.SetActive(true);
			gameObject.GetComponent<ReportScreenEntry>().SetMainEntry(entry, reportGroup);
		}
		return gameObject;
	}

	// Token: 0x0600A577 RID: 42359 RVA: 0x0010B61B File Offset: 0x0010981B
	private void OnClickClose()
	{
		base.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Close", false));
		this.Show(false);
	}

	// Token: 0x04008197 RID: 33175
	[SerializeField]
	private LocText title;

	// Token: 0x04008198 RID: 33176
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04008199 RID: 33177
	[SerializeField]
	private KButton prevButton;

	// Token: 0x0400819A RID: 33178
	[SerializeField]
	private KButton nextButton;

	// Token: 0x0400819B RID: 33179
	[SerializeField]
	private KButton summaryButton;

	// Token: 0x0400819C RID: 33180
	[SerializeField]
	private GameObject lineItem;

	// Token: 0x0400819D RID: 33181
	[SerializeField]
	private GameObject lineItemSpacer;

	// Token: 0x0400819E RID: 33182
	[SerializeField]
	private GameObject lineItemHeader;

	// Token: 0x0400819F RID: 33183
	[SerializeField]
	private GameObject contentFolder;

	// Token: 0x040081A0 RID: 33184
	private Dictionary<string, GameObject> lineItems = new Dictionary<string, GameObject>();

	// Token: 0x040081A1 RID: 33185
	private ReportManager.DailyReport currentReport;
}
