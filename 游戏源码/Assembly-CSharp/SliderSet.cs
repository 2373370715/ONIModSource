using System;
using UnityEngine;

// Token: 0x02001FD9 RID: 8153
[Serializable]
public class SliderSet
{
	// Token: 0x0600ACC0 RID: 44224 RVA: 0x0040F200 File Offset: 0x0040D400
	public void SetupSlider(int index)
	{
		this.index = index;
		this.valueSlider.onReleaseHandle += delegate()
		{
			this.valueSlider.value = Mathf.Round(this.valueSlider.value * 10f) / 10f;
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onDrag += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onMove += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.valueSlider.onPointerDown += delegate()
		{
			this.ReceiveValueFromSlider();
		};
		this.numberInput.onEndEdit += delegate()
		{
			this.ReceiveValueFromInput();
		};
	}

	// Token: 0x0600ACC1 RID: 44225 RVA: 0x0040F288 File Offset: 0x0040D488
	public void SetTarget(ISliderControl target, int index)
	{
		this.index = index;
		this.target = target;
		ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
		if (component != null)
		{
			component.SetSimpleTooltip(target.GetSliderTooltip(index));
		}
		if (this.targetLabel != null)
		{
			this.targetLabel.text = ((target.SliderTitleKey != null) ? Strings.Get(target.SliderTitleKey) : "");
		}
		this.unitsLabel.text = target.SliderUnits;
		this.minLabel.text = target.GetSliderMin(index).ToString() + target.SliderUnits;
		this.maxLabel.text = target.GetSliderMax(index).ToString() + target.SliderUnits;
		this.numberInput.minValue = target.GetSliderMin(index);
		this.numberInput.maxValue = target.GetSliderMax(index);
		this.numberInput.decimalPlaces = target.SliderDecimalPlaces(index);
		this.numberInput.field.characterLimit = Mathf.FloorToInt(1f + Mathf.Log10(this.numberInput.maxValue + (float)this.numberInput.decimalPlaces));
		Vector2 sizeDelta = this.numberInput.GetComponent<RectTransform>().sizeDelta;
		sizeDelta.x = (float)((this.numberInput.field.characterLimit + 1) * 10);
		this.numberInput.GetComponent<RectTransform>().sizeDelta = sizeDelta;
		this.valueSlider.minValue = target.GetSliderMin(index);
		this.valueSlider.maxValue = target.GetSliderMax(index);
		this.valueSlider.value = target.GetSliderValue(index);
		this.SetValue(target.GetSliderValue(index));
		if (index == 0)
		{
			this.numberInput.Activate();
		}
	}

	// Token: 0x0600ACC2 RID: 44226 RVA: 0x0040F45C File Offset: 0x0040D65C
	private void ReceiveValueFromSlider()
	{
		float num = this.valueSlider.value;
		if (this.numberInput.decimalPlaces != -1)
		{
			float num2 = Mathf.Pow(10f, (float)this.numberInput.decimalPlaces);
			num = Mathf.Round(num * num2) / num2;
		}
		this.SetValue(num);
	}

	// Token: 0x0600ACC3 RID: 44227 RVA: 0x0040F4AC File Offset: 0x0040D6AC
	private void ReceiveValueFromInput()
	{
		float num = this.numberInput.currentValue;
		if (this.numberInput.decimalPlaces != -1)
		{
			float num2 = Mathf.Pow(10f, (float)this.numberInput.decimalPlaces);
			num = Mathf.Round(num * num2) / num2;
		}
		this.valueSlider.value = num;
		this.SetValue(num);
	}

	// Token: 0x0600ACC4 RID: 44228 RVA: 0x0040F508 File Offset: 0x0040D708
	private void SetValue(float value)
	{
		float num = value;
		if (num > this.target.GetSliderMax(this.index))
		{
			num = this.target.GetSliderMax(this.index);
		}
		else if (num < this.target.GetSliderMin(this.index))
		{
			num = this.target.GetSliderMin(this.index);
		}
		this.UpdateLabel(num);
		this.target.SetSliderValue(num, this.index);
		ToolTip component = this.valueSlider.handleRect.GetComponent<ToolTip>();
		if (component != null)
		{
			component.SetSimpleTooltip(this.target.GetSliderTooltip(this.index));
		}
	}

	// Token: 0x0600ACC5 RID: 44229 RVA: 0x0040F5B0 File Offset: 0x0040D7B0
	private void UpdateLabel(float value)
	{
		float num = Mathf.Round(value * 10f) / 10f;
		this.numberInput.SetDisplayValue(num.ToString());
	}

	// Token: 0x04008796 RID: 34710
	public KSlider valueSlider;

	// Token: 0x04008797 RID: 34711
	public KNumberInputField numberInput;

	// Token: 0x04008798 RID: 34712
	public LocText targetLabel;

	// Token: 0x04008799 RID: 34713
	public LocText unitsLabel;

	// Token: 0x0400879A RID: 34714
	public LocText minLabel;

	// Token: 0x0400879B RID: 34715
	public LocText maxLabel;

	// Token: 0x0400879C RID: 34716
	[NonSerialized]
	public int index;

	// Token: 0x0400879D RID: 34717
	private ISliderControl target;
}
