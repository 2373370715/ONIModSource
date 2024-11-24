using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using UnityEngine;

// Token: 0x02001E08 RID: 7688
public class MeterScreen : KScreen, IRender1000ms
{
	// Token: 0x17000A6F RID: 2671
	// (get) Token: 0x0600A0F5 RID: 41205 RVA: 0x00108A5D File Offset: 0x00106C5D
	// (set) Token: 0x0600A0F6 RID: 41206 RVA: 0x00108A64 File Offset: 0x00106C64
	public static MeterScreen Instance { get; private set; }

	// Token: 0x0600A0F7 RID: 41207 RVA: 0x00108A6C File Offset: 0x00106C6C
	public static void DestroyInstance()
	{
		MeterScreen.Instance = null;
	}

	// Token: 0x17000A70 RID: 2672
	// (get) Token: 0x0600A0F8 RID: 41208 RVA: 0x00108A74 File Offset: 0x00106C74
	public bool StartValuesSet
	{
		get
		{
			return this.startValuesSet;
		}
	}

	// Token: 0x0600A0F9 RID: 41209 RVA: 0x00108A7C File Offset: 0x00106C7C
	protected override void OnPrefabInit()
	{
		MeterScreen.Instance = this;
	}

	// Token: 0x0600A0FA RID: 41210 RVA: 0x003D6594 File Offset: 0x003D4794
	protected override void OnSpawn()
	{
		this.RedAlertTooltip.OnToolTip = new Func<string>(this.OnRedAlertTooltip);
		MultiToggle redAlertButton = this.RedAlertButton;
		redAlertButton.onClick = (System.Action)Delegate.Combine(redAlertButton.onClick, new System.Action(delegate()
		{
			this.OnRedAlertClick();
		}));
		Game.Instance.Subscribe(1983128072, delegate(object data)
		{
			this.Refresh();
		});
		Game.Instance.Subscribe(1585324898, delegate(object data)
		{
			this.RefreshRedAlertButtonState();
		});
		Game.Instance.Subscribe(-1393151672, delegate(object data)
		{
			this.RefreshRedAlertButtonState();
		});
	}

	// Token: 0x0600A0FB RID: 41211 RVA: 0x003D6634 File Offset: 0x003D4834
	private void OnRedAlertClick()
	{
		bool flag = !ClusterManager.Instance.activeWorld.AlertManager.IsRedAlertToggledOn();
		ClusterManager.Instance.activeWorld.AlertManager.ToggleRedAlert(flag);
		if (flag)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
			return;
		}
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
	}

	// Token: 0x0600A0FC RID: 41212 RVA: 0x00108A84 File Offset: 0x00106C84
	private void RefreshRedAlertButtonState()
	{
		this.RedAlertButton.ChangeState(ClusterManager.Instance.activeWorld.IsRedAlert() ? 1 : 0);
	}

	// Token: 0x0600A0FD RID: 41213 RVA: 0x00108AA6 File Offset: 0x00106CA6
	public void Render1000ms(float dt)
	{
		this.Refresh();
	}

	// Token: 0x0600A0FE RID: 41214 RVA: 0x00108AAE File Offset: 0x00106CAE
	public void InitializeValues()
	{
		if (this.startValuesSet)
		{
			return;
		}
		this.startValuesSet = true;
		this.Refresh();
	}

	// Token: 0x0600A0FF RID: 41215 RVA: 0x003D6694 File Offset: 0x003D4894
	private void Refresh()
	{
		this.RefreshWorldMinionIdentities();
		this.RefreshMinions();
		for (int i = 0; i < this.valueDisplayers.Length; i++)
		{
			this.valueDisplayers[i].Refresh();
		}
		this.RefreshRedAlertButtonState();
	}

	// Token: 0x0600A100 RID: 41216 RVA: 0x003D66D4 File Offset: 0x003D48D4
	private void RefreshWorldMinionIdentities()
	{
		this.worldLiveMinionIdentities = new List<MinionIdentity>(from x in Components.LiveMinionIdentities.GetWorldItems(ClusterManager.Instance.activeWorldId, false)
		where !x.IsNullOrDestroyed()
		select x);
	}

	// Token: 0x0600A101 RID: 41217 RVA: 0x00108AC6 File Offset: 0x00106CC6
	private List<MinionIdentity> GetWorldMinionIdentities()
	{
		if (this.worldLiveMinionIdentities == null)
		{
			this.RefreshWorldMinionIdentities();
		}
		return this.worldLiveMinionIdentities;
	}

	// Token: 0x0600A102 RID: 41218 RVA: 0x003D6728 File Offset: 0x003D4928
	private void RefreshMinions()
	{
		int count = Components.LiveMinionIdentities.Count;
		int count2 = this.GetWorldMinionIdentities().Count;
		if (count2 == this.cachedMinionCount)
		{
			return;
		}
		this.cachedMinionCount = count2;
		string newString;
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			ClusterGridEntity component = ClusterManager.Instance.activeWorld.GetComponent<ClusterGridEntity>();
			newString = string.Format(UI.TOOLTIPS.METERSCREEN_POPULATION_CLUSTER, component.Name, count2, count);
			this.currentMinions.text = string.Format("{0}/{1}", count2, count);
		}
		else
		{
			this.currentMinions.text = string.Format("{0}", count);
			newString = string.Format(UI.TOOLTIPS.METERSCREEN_POPULATION, count.ToString("0"));
		}
		this.MinionsTooltip.ClearMultiStringTooltip();
		this.MinionsTooltip.AddMultiStringTooltip(newString, this.ToolTipStyle_Header);
	}

	// Token: 0x0600A103 RID: 41219 RVA: 0x003D6814 File Offset: 0x003D4A14
	private string OnRedAlertTooltip()
	{
		this.RedAlertTooltip.ClearMultiStringTooltip();
		this.RedAlertTooltip.AddMultiStringTooltip(UI.TOOLTIPS.RED_ALERT_TITLE, this.ToolTipStyle_Header);
		this.RedAlertTooltip.AddMultiStringTooltip(UI.TOOLTIPS.RED_ALERT_CONTENT, this.ToolTipStyle_Property);
		return "";
	}

	// Token: 0x04007DA7 RID: 32167
	[SerializeField]
	private LocText currentMinions;

	// Token: 0x04007DA9 RID: 32169
	public ToolTip MinionsTooltip;

	// Token: 0x04007DAA RID: 32170
	public MeterScreen_ValueTrackerDisplayer[] valueDisplayers;

	// Token: 0x04007DAB RID: 32171
	public TextStyleSetting ToolTipStyle_Header;

	// Token: 0x04007DAC RID: 32172
	public TextStyleSetting ToolTipStyle_Property;

	// Token: 0x04007DAD RID: 32173
	private bool startValuesSet;

	// Token: 0x04007DAE RID: 32174
	public MultiToggle RedAlertButton;

	// Token: 0x04007DAF RID: 32175
	public ToolTip RedAlertTooltip;

	// Token: 0x04007DB0 RID: 32176
	private MeterScreen.DisplayInfo immunityDisplayInfo = new MeterScreen.DisplayInfo
	{
		selectedIndex = -1
	};

	// Token: 0x04007DB1 RID: 32177
	private List<MinionIdentity> worldLiveMinionIdentities;

	// Token: 0x04007DB2 RID: 32178
	private int cachedMinionCount = -1;

	// Token: 0x02001E09 RID: 7689
	private struct DisplayInfo
	{
		// Token: 0x04007DB3 RID: 32179
		public int selectedIndex;
	}
}
