using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiSliderSideScreen : SideScreenContent
{
	public LayoutElement sliderPrefab;

	public RectTransform sliderContainer;

	private IMultiSliderControl target;

	private List<GameObject> liveSliders = new List<GameObject>();

	private List<SliderSet> sliderSets = new List<SliderSet>();

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IMultiSliderControl>()?.SidescreenEnabled() ?? false;
	}

	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			Debug.LogError("Invalid gameObject received");
			return;
		}
		target = new_target.GetComponent<IMultiSliderControl>();
		titleKey = target.SidescreenTitleKey;
		Refresh();
	}

	private void Refresh()
	{
		while (liveSliders.Count < target.sliderControls.Length)
		{
			GameObject gameObject = Util.KInstantiateUI(sliderPrefab.gameObject, sliderContainer.gameObject, force_active: true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			SliderSet sliderSet = new SliderSet();
			sliderSet.valueSlider = component.GetReference<KSlider>("Slider");
			sliderSet.numberInput = component.GetReference<KNumberInputField>("NumberInputField");
			if (sliderSet.numberInput != null)
			{
				sliderSet.numberInput.Activate();
			}
			sliderSet.targetLabel = component.GetReference<LocText>("TargetLabel");
			sliderSet.unitsLabel = component.GetReference<LocText>("UnitsLabel");
			sliderSet.minLabel = component.GetReference<LocText>("MinLabel");
			sliderSet.maxLabel = component.GetReference<LocText>("MaxLabel");
			sliderSet.SetupSlider(liveSliders.Count);
			liveSliders.Add(gameObject);
			sliderSets.Add(sliderSet);
		}
		for (int i = 0; i < liveSliders.Count; i++)
		{
			if (i >= target.sliderControls.Length)
			{
				liveSliders[i].SetActive(value: false);
				continue;
			}
			if (!liveSliders[i].activeSelf)
			{
				liveSliders[i].SetActive(value: true);
				liveSliders[i].gameObject.SetActive(value: true);
			}
			sliderSets[i].SetTarget(target.sliderControls[i], i);
		}
	}
}
