using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001C93 RID: 7315
public class DateTime : KScreen
{
	// Token: 0x0600987C RID: 39036 RVA: 0x001032EF File Offset: 0x001014EF
	public static void DestroyInstance()
	{
		global::DateTime.Instance = null;
	}

	// Token: 0x0600987D RID: 39037 RVA: 0x001032F7 File Offset: 0x001014F7
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		global::DateTime.Instance = this;
		this.milestoneEffect.gameObject.SetActive(false);
	}

	// Token: 0x0600987E RID: 39038 RVA: 0x003AFD40 File Offset: 0x003ADF40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.tooltip.OnComplexToolTip = new ToolTip.ComplexTooltipDelegate(this.BuildTooltip);
		Game.Instance.Subscribe(2070437606, new Action<object>(this.OnMilestoneDayReached));
		Game.Instance.Subscribe(-720092972, new Action<object>(this.OnMilestoneDayApproaching));
	}

	// Token: 0x0600987F RID: 39039 RVA: 0x003AFDA4 File Offset: 0x003ADFA4
	private List<global::Tuple<string, TextStyleSetting>> BuildTooltip()
	{
		List<global::Tuple<string, TextStyleSetting>> colonyToolTip = SaveGame.Instance.GetColonyToolTip();
		if (TimeOfDay.IsMilestoneApproaching)
		{
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(" ", null));
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_TITLE.text, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			colonyToolTip.Add(new global::Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_DESCRIPTION.text.Replace("{0}", (GameClock.Instance.GetCycle() + 2).ToString()), ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		return colonyToolTip;
	}

	// Token: 0x06009880 RID: 39040 RVA: 0x00103316 File Offset: 0x00101516
	private void Update()
	{
		if (GameClock.Instance != null && this.displayedDayCount != GameUtil.GetCurrentCycle())
		{
			this.text.text = this.Days();
			this.displayedDayCount = GameUtil.GetCurrentCycle();
		}
	}

	// Token: 0x06009881 RID: 39041 RVA: 0x0010334E File Offset: 0x0010154E
	private void OnMilestoneDayApproaching(object data)
	{
		int num = (int)data;
		this.milestoneEffect.gameObject.SetActive(true);
		this.milestoneEffect.Play("100fx_pre", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06009882 RID: 39042 RVA: 0x00103388 File Offset: 0x00101588
	private void OnMilestoneDayReached(object data)
	{
		int num = (int)data;
		this.milestoneEffect.gameObject.SetActive(true);
		this.milestoneEffect.Play("100fx", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06009883 RID: 39043 RVA: 0x003AFE34 File Offset: 0x003AE034
	private string Days()
	{
		return GameUtil.GetCurrentCycle().ToString();
	}

	// Token: 0x040076BF RID: 30399
	public static global::DateTime Instance;

	// Token: 0x040076C0 RID: 30400
	private const string MILESTONE_ANTICIPATION_ANIMATION_NAME = "100fx_pre";

	// Token: 0x040076C1 RID: 30401
	private const string MILESTONE_ANIMATION_NAME = "100fx";

	// Token: 0x040076C2 RID: 30402
	public LocText day;

	// Token: 0x040076C3 RID: 30403
	private int displayedDayCount = -1;

	// Token: 0x040076C4 RID: 30404
	[SerializeField]
	private KBatchedAnimController milestoneEffect;

	// Token: 0x040076C5 RID: 30405
	[SerializeField]
	private LocText text;

	// Token: 0x040076C6 RID: 30406
	[SerializeField]
	private ToolTip tooltip;

	// Token: 0x040076C7 RID: 30407
	[SerializeField]
	private TextStyleSetting tooltipstyle_Days;

	// Token: 0x040076C8 RID: 30408
	[SerializeField]
	private TextStyleSetting tooltipstyle_Playtime;

	// Token: 0x040076C9 RID: 30409
	[SerializeField]
	public KToggle scheduleToggle;
}
