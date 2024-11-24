using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F7C RID: 8060
public class IntSliderSideScreen : SideScreenContent
{
	// Token: 0x0600AA0F RID: 43535 RVA: 0x00403D1C File Offset: 0x00401F1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
			this.sliderSets[i].valueSlider.wholeNumbers = true;
		}
	}

	// Token: 0x0600AA10 RID: 43536 RVA: 0x0010E803 File Offset: 0x0010CA03
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IIntSliderControl>() != null || target.GetSMI<IIntSliderControl>() != null;
	}

	// Token: 0x0600AA11 RID: 43537 RVA: 0x00403D70 File Offset: 0x00401F70
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IIntSliderControl>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<IIntSliderControl>();
		}
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain a Manual Generator component");
			return;
		}
		this.titleKey = this.target.SliderTitleKey;
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetTarget(this.target, i);
		}
	}

	// Token: 0x040085B7 RID: 34231
	private IIntSliderControl target;

	// Token: 0x040085B8 RID: 34232
	public List<SliderSet> sliderSets;
}
