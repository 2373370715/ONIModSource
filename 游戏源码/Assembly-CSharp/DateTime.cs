using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class DateTime : KScreen
{
	public static DateTime Instance;

	private const string MILESTONE_ANTICIPATION_ANIMATION_NAME = "100fx_pre";

	private const string MILESTONE_ANIMATION_NAME = "100fx";

	public LocText day;

	private int displayedDayCount = -1;

	[SerializeField]
	private KBatchedAnimController milestoneEffect;

	[SerializeField]
	private LocText text;

	[SerializeField]
	private ToolTip tooltip;

	[SerializeField]
	private TextStyleSetting tooltipstyle_Days;

	[SerializeField]
	private TextStyleSetting tooltipstyle_Playtime;

	[SerializeField]
	public KToggle scheduleToggle;

	public static void DestroyInstance()
	{
		Instance = null;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
		milestoneEffect.gameObject.SetActive(value: false);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		tooltip.OnComplexToolTip = BuildTooltip;
		Game.Instance.Subscribe(2070437606, OnMilestoneDayReached);
		Game.Instance.Subscribe(-720092972, OnMilestoneDayApproaching);
	}

	private List<Tuple<string, TextStyleSetting>> BuildTooltip()
	{
		List<Tuple<string, TextStyleSetting>> colonyToolTip = SaveGame.Instance.GetColonyToolTip();
		if (TimeOfDay.IsMilestoneApproaching)
		{
			colonyToolTip.Add(new Tuple<string, TextStyleSetting>(" ", null));
			colonyToolTip.Add(new Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_TITLE.text, ToolTipScreen.Instance.defaultTooltipHeaderStyle));
			colonyToolTip.Add(new Tuple<string, TextStyleSetting>(UI.ASTEROIDCLOCK.MILESTONE_DESCRIPTION.text.Replace("{0}", (GameClock.Instance.GetCycle() + 2).ToString()), ToolTipScreen.Instance.defaultTooltipBodyStyle));
		}
		return colonyToolTip;
	}

	private void Update()
	{
		if (GameClock.Instance != null && displayedDayCount != GameUtil.GetCurrentCycle())
		{
			text.text = Days();
			displayedDayCount = GameUtil.GetCurrentCycle();
		}
	}

	private void OnMilestoneDayApproaching(object data)
	{
		_ = (int)data;
		milestoneEffect.gameObject.SetActive(value: true);
		milestoneEffect.Play("100fx_pre", KAnim.PlayMode.Loop);
	}

	private void OnMilestoneDayReached(object data)
	{
		_ = (int)data;
		milestoneEffect.gameObject.SetActive(value: true);
		milestoneEffect.Play("100fx");
	}

	private string Days()
	{
		return GameUtil.GetCurrentCycle().ToString();
	}
}
