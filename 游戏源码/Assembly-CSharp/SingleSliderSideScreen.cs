using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001FDB RID: 8155
public class SingleSliderSideScreen : SideScreenContent
{
	// Token: 0x0600ACCC RID: 44236 RVA: 0x0040F5E4 File Offset: 0x0040D7E4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetupSlider(i);
		}
	}

	// Token: 0x0600ACCD RID: 44237 RVA: 0x0040F620 File Offset: 0x0040D820
	public override bool IsValidForTarget(GameObject target)
	{
		KPrefabID component = target.GetComponent<KPrefabID>();
		ISingleSliderControl singleSliderControl = target.GetComponent<ISingleSliderControl>();
		singleSliderControl = ((singleSliderControl != null) ? singleSliderControl : target.GetSMI<ISingleSliderControl>());
		return singleSliderControl != null && !component.IsPrefabID("HydrogenGenerator".ToTag()) && !component.IsPrefabID("MethaneGenerator".ToTag()) && !component.IsPrefabID("PetroleumGenerator".ToTag()) && !component.IsPrefabID("DevGenerator".ToTag()) && !component.HasTag(GameTags.DeadReactor) && singleSliderControl.GetSliderMin(0) != singleSliderControl.GetSliderMax(0);
	}

	// Token: 0x0600ACCE RID: 44238 RVA: 0x0040F6B8 File Offset: 0x0040D8B8
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<ISingleSliderControl>();
		if (this.target == null)
		{
			this.target = new_target.GetSMI<ISingleSliderControl>();
			if (this.target == null)
			{
				global::Debug.LogError("The gameObject received does not contain a ISingleSliderControl implementation");
				return;
			}
		}
		this.titleKey = this.target.SliderTitleKey;
		for (int i = 0; i < this.sliderSets.Count; i++)
		{
			this.sliderSets[i].SetTarget(this.target, i);
		}
	}

	// Token: 0x0400879E RID: 34718
	private ISingleSliderControl target;

	// Token: 0x0400879F RID: 34719
	public List<SliderSet> sliderSets;
}
