using System;

// Token: 0x02001801 RID: 6145
public class RunningAverage
{
	// Token: 0x06007EC7 RID: 32455 RVA: 0x000F3918 File Offset: 0x000F1B18
	public RunningAverage(float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f, int sampleCount = 15, bool allowZero = true)
	{
		this.min = minValue;
		this.max = maxValue;
		this.ignoreZero = !allowZero;
		this.samples = new float[sampleCount];
	}

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x06007EC8 RID: 32456 RVA: 0x000F3945 File Offset: 0x000F1B45
	public float AverageValue
	{
		get
		{
			return this.GetAverage();
		}
	}

	// Token: 0x06007EC9 RID: 32457 RVA: 0x0032C748 File Offset: 0x0032A948
	public void AddSample(float value)
	{
		if (value < this.min || value > this.max || (this.ignoreZero && value == 0f))
		{
			return;
		}
		if (this.validValues < this.samples.Length)
		{
			this.validValues++;
		}
		for (int i = 0; i < this.samples.Length - 1; i++)
		{
			this.samples[i] = this.samples[i + 1];
		}
		this.samples[this.samples.Length - 1] = value;
	}

	// Token: 0x06007ECA RID: 32458 RVA: 0x0032C7D0 File Offset: 0x0032A9D0
	private float GetAverage()
	{
		float num = 0f;
		for (int i = this.samples.Length - 1; i > this.samples.Length - 1 - this.validValues; i--)
		{
			num += this.samples[i];
		}
		return num / (float)this.validValues;
	}

	// Token: 0x04006012 RID: 24594
	private float[] samples;

	// Token: 0x04006013 RID: 24595
	private float min;

	// Token: 0x04006014 RID: 24596
	private float max;

	// Token: 0x04006015 RID: 24597
	private bool ignoreZero;

	// Token: 0x04006016 RID: 24598
	private int validValues;
}
