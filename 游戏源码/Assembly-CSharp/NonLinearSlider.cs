using System;
using UnityEngine;

// Token: 0x02001E52 RID: 7762
public class NonLinearSlider : KSlider
{
	// Token: 0x0600A2A9 RID: 41641 RVA: 0x00109828 File Offset: 0x00107A28
	public static NonLinearSlider.Range[] GetDefaultRange(float maxValue)
	{
		return new NonLinearSlider.Range[]
		{
			new NonLinearSlider.Range(100f, maxValue)
		};
	}

	// Token: 0x0600A2AA RID: 41642 RVA: 0x00109842 File Offset: 0x00107A42
	protected override void Start()
	{
		base.Start();
		base.minValue = 0f;
		base.maxValue = 100f;
	}

	// Token: 0x0600A2AB RID: 41643 RVA: 0x00109860 File Offset: 0x00107A60
	public void SetRanges(NonLinearSlider.Range[] ranges)
	{
		this.ranges = ranges;
	}

	// Token: 0x0600A2AC RID: 41644 RVA: 0x003DE6C0 File Offset: 0x003DC8C0
	public float GetPercentageFromValue(float value)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.ranges.Length; i++)
		{
			if (value >= num2 && value <= this.ranges[i].peakValue)
			{
				float t = (value - num2) / (this.ranges[i].peakValue - num2);
				return Mathf.Lerp(num, num + this.ranges[i].width, t);
			}
			num += this.ranges[i].width;
			num2 = this.ranges[i].peakValue;
		}
		return 100f;
	}

	// Token: 0x0600A2AD RID: 41645 RVA: 0x003DE764 File Offset: 0x003DC964
	public float GetValueForPercentage(float percentage)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < this.ranges.Length; i++)
		{
			if (percentage >= num && num + this.ranges[i].width >= percentage)
			{
				float t = (percentage - num) / this.ranges[i].width;
				return Mathf.Lerp(num2, this.ranges[i].peakValue, t);
			}
			num += this.ranges[i].width;
			num2 = this.ranges[i].peakValue;
		}
		return num2;
	}

	// Token: 0x0600A2AE RID: 41646 RVA: 0x00109869 File Offset: 0x00107A69
	protected override void Set(float input, bool sendCallback)
	{
		base.Set(input, sendCallback);
	}

	// Token: 0x04007EEE RID: 32494
	public NonLinearSlider.Range[] ranges;

	// Token: 0x02001E53 RID: 7763
	[Serializable]
	public struct Range
	{
		// Token: 0x0600A2B0 RID: 41648 RVA: 0x0010987B File Offset: 0x00107A7B
		public Range(float width, float peakValue)
		{
			this.width = width;
			this.peakValue = peakValue;
		}

		// Token: 0x04007EEF RID: 32495
		public float width;

		// Token: 0x04007EF0 RID: 32496
		public float peakValue;
	}
}
