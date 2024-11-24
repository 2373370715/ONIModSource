using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001F64 RID: 8036
public class DualSliderSideScreen : SideScreenContent
{
	// Token: 0x0600A9A3 RID: 43427 RVA: 0x0040212C File Offset: 0x0040032C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
		}
	}

	// Token: 0x0600A9A4 RID: 43428 RVA: 0x0010E335 File Offset: 0x0010C535
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IDualSliderControl>() != null;
	}

	// Token: 0x0600A9A5 RID: 43429 RVA: 0x00402168 File Offset: 0x00400368
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IDualSliderControl>();
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

	// Token: 0x04008566 RID: 34150
	private IDualSliderControl target;

	// Token: 0x04008567 RID: 34151
	public List<SliderSet> sliderSets;
}
