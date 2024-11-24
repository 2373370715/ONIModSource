using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F91 RID: 8081
public class MultiSliderSideScreen : SideScreenContent
{
	// Token: 0x0600AA97 RID: 43671 RVA: 0x004067E8 File Offset: 0x004049E8
	public override bool IsValidForTarget(GameObject target)
	{
		IMultiSliderControl component = target.GetComponent<IMultiSliderControl>();
		return component != null && component.SidescreenEnabled();
	}

	// Token: 0x0600AA98 RID: 43672 RVA: 0x0010ECB0 File Offset: 0x0010CEB0
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IMultiSliderControl>();
		this.titleKey = this.target.SidescreenTitleKey;
		this.Refresh();
	}

	// Token: 0x0600AA99 RID: 43673 RVA: 0x00406808 File Offset: 0x00404A08
	private void Refresh()
	{
		while (this.liveSliders.Count < this.target.sliderControls.Length)
		{
			GameObject gameObject = Util.KInstantiateUI(this.sliderPrefab.gameObject, this.sliderContainer.gameObject, true);
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
			sliderSet.SetupSlider(this.liveSliders.Count);
			this.liveSliders.Add(gameObject);
			this.sliderSets.Add(sliderSet);
		}
		for (int i = 0; i < this.liveSliders.Count; i++)
		{
			if (i >= this.target.sliderControls.Length)
			{
				this.liveSliders[i].SetActive(false);
			}
			else
			{
				if (!this.liveSliders[i].activeSelf)
				{
					this.liveSliders[i].SetActive(true);
					this.liveSliders[i].gameObject.SetActive(true);
				}
				this.sliderSets[i].SetTarget(this.target.sliderControls[i], i);
			}
		}
	}

	// Token: 0x0400861A RID: 34330
	public LayoutElement sliderPrefab;

	// Token: 0x0400861B RID: 34331
	public RectTransform sliderContainer;

	// Token: 0x0400861C RID: 34332
	private IMultiSliderControl target;

	// Token: 0x0400861D RID: 34333
	private List<GameObject> liveSliders = new List<GameObject>();

	// Token: 0x0400861E RID: 34334
	private List<SliderSet> sliderSets = new List<SliderSet>();
}
