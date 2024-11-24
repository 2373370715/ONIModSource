using System.Collections.Generic;
using UnityEngine;

public class SingleSliderSideScreen : SideScreenContent
{
	private ISingleSliderControl target;

	public List<SliderSet> sliderSets;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < sliderSets.Count; i++)
		{
			sliderSets[i].SetupSlider(i);
		}
	}

	public override bool IsValidForTarget(GameObject target)
	{
		KPrefabID component = target.GetComponent<KPrefabID>();
		ISingleSliderControl component2 = target.GetComponent<ISingleSliderControl>();
		component2 = ((component2 != null) ? component2 : target.GetSMI<ISingleSliderControl>());
		if (component2 != null && !component.IsPrefabID("HydrogenGenerator".ToTag()) && !component.IsPrefabID("MethaneGenerator".ToTag()) && !component.IsPrefabID("PetroleumGenerator".ToTag()) && !component.IsPrefabID("DevGenerator".ToTag()) && !component.HasTag(GameTags.DeadReactor))
		{
			return component2.GetSliderMin(0) != component2.GetSliderMax(0);
		}
		return false;
	}

	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			Debug.LogError("Invalid gameObject received");
			return;
		}
		target = new_target.GetComponent<ISingleSliderControl>();
		if (target == null)
		{
			target = new_target.GetSMI<ISingleSliderControl>();
			if (target == null)
			{
				Debug.LogError("The gameObject received does not contain a ISingleSliderControl implementation");
				return;
			}
		}
		titleKey = target.SliderTitleKey;
		for (int i = 0; i < sliderSets.Count; i++)
		{
			sliderSets[i].SetTarget(target, i);
		}
	}
}
