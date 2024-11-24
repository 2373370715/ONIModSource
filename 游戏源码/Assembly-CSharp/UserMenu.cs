using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B68 RID: 2920
public class UserMenu
{
	// Token: 0x0600377D RID: 14205 RVA: 0x000C3FA0 File Offset: 0x000C21A0
	public void Refresh(GameObject go)
	{
		Game.Instance.Trigger(1980521255, go);
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x00217F00 File Offset: 0x00216100
	public void AddButton(GameObject go, KIconButtonMenu.ButtonInfo button, float sort_order = 1f)
	{
		if (button.onClick != null)
		{
			System.Action callback = button.onClick;
			button.onClick = delegate()
			{
				callback();
				Game.Instance.Trigger(1980521255, go);
			};
		}
		this.buttons.Add(new KeyValuePair<KIconButtonMenu.ButtonInfo, float>(button, sort_order));
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x000C3FB2 File Offset: 0x000C21B2
	public void AddSlider(GameObject go, UserMenu.SliderInfo slider)
	{
		this.sliders.Add(slider);
	}

	// Token: 0x06003780 RID: 14208 RVA: 0x00217F54 File Offset: 0x00216154
	public void AppendToScreen(GameObject go, UserMenuScreen screen)
	{
		this.buttons.Clear();
		this.sliders.Clear();
		go.Trigger(493375141, null);
		if (this.buttons.Count > 0)
		{
			this.buttons.Sort(delegate(KeyValuePair<KIconButtonMenu.ButtonInfo, float> x, KeyValuePair<KIconButtonMenu.ButtonInfo, float> y)
			{
				if (x.Value == y.Value)
				{
					return 0;
				}
				if (x.Value > y.Value)
				{
					return 1;
				}
				return -1;
			});
			for (int i = 0; i < this.buttons.Count; i++)
			{
				this.sortedButtons.Add(this.buttons[i].Key);
			}
			screen.AddButtons(this.sortedButtons);
			this.sortedButtons.Clear();
		}
		if (this.sliders.Count > 0)
		{
			screen.AddSliders(this.sliders);
		}
	}

	// Token: 0x0400259E RID: 9630
	public const float DECONSTRUCT_PRIORITY = 0f;

	// Token: 0x0400259F RID: 9631
	public const float DRAWPATHS_PRIORITY = 0.1f;

	// Token: 0x040025A0 RID: 9632
	public const float FOLLOWCAM_PRIORITY = 0.3f;

	// Token: 0x040025A1 RID: 9633
	public const float SETDIRECTION_PRIORITY = 0.4f;

	// Token: 0x040025A2 RID: 9634
	public const float AUTOBOTTLE_PRIORITY = 0.4f;

	// Token: 0x040025A3 RID: 9635
	public const float AUTOREPAIR_PRIORITY = 0.5f;

	// Token: 0x040025A4 RID: 9636
	public const float DEFAULT_PRIORITY = 1f;

	// Token: 0x040025A5 RID: 9637
	public const float SUITEQUIP_PRIORITY = 2f;

	// Token: 0x040025A6 RID: 9638
	public const float AUTODISINFECT_PRIORITY = 10f;

	// Token: 0x040025A7 RID: 9639
	public const float ROCKETUSAGERESTRICTION_PRIORITY = 11f;

	// Token: 0x040025A8 RID: 9640
	private List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>> buttons = new List<KeyValuePair<KIconButtonMenu.ButtonInfo, float>>();

	// Token: 0x040025A9 RID: 9641
	private List<UserMenu.SliderInfo> sliders = new List<UserMenu.SliderInfo>();

	// Token: 0x040025AA RID: 9642
	private List<KIconButtonMenu.ButtonInfo> sortedButtons = new List<KIconButtonMenu.ButtonInfo>();

	// Token: 0x02000B69 RID: 2921
	public class SliderInfo
	{
		// Token: 0x040025AB RID: 9643
		public MinMaxSlider.LockingType lockType = MinMaxSlider.LockingType.Drag;

		// Token: 0x040025AC RID: 9644
		public MinMaxSlider.Mode mode;

		// Token: 0x040025AD RID: 9645
		public Slider.Direction direction;

		// Token: 0x040025AE RID: 9646
		public bool interactable = true;

		// Token: 0x040025AF RID: 9647
		public bool lockRange;

		// Token: 0x040025B0 RID: 9648
		public string toolTip;

		// Token: 0x040025B1 RID: 9649
		public string toolTipMin;

		// Token: 0x040025B2 RID: 9650
		public string toolTipMax;

		// Token: 0x040025B3 RID: 9651
		public float minLimit;

		// Token: 0x040025B4 RID: 9652
		public float maxLimit = 100f;

		// Token: 0x040025B5 RID: 9653
		public float currentMinValue = 10f;

		// Token: 0x040025B6 RID: 9654
		public float currentMaxValue = 90f;

		// Token: 0x040025B7 RID: 9655
		public GameObject sliderGO;

		// Token: 0x040025B8 RID: 9656
		public Action<MinMaxSlider> onMinChange;

		// Token: 0x040025B9 RID: 9657
		public Action<MinMaxSlider> onMaxChange;
	}
}
