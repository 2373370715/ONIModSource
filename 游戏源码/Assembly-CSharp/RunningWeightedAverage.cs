using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001802 RID: 6146
public class RunningWeightedAverage
{
	// Token: 0x06007ECB RID: 32459 RVA: 0x000F394D File Offset: 0x000F1B4D
	public RunningWeightedAverage(float minValue = -3.4028235E+38f, float maxValue = 3.4028235E+38f, int sampleCount = 20, bool allowZero = true)
	{
		this.min = minValue;
		this.max = maxValue;
		this.ignoreZero = !allowZero;
		this.samples = new List<global::Tuple<float, float>>();
	}

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x06007ECC RID: 32460 RVA: 0x000F398C File Offset: 0x000F1B8C
	public float GetUnweightedAverage
	{
		get
		{
			return this.GetAverageOfLastSeconds(4f);
		}
	}

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x06007ECD RID: 32461 RVA: 0x000F3999 File Offset: 0x000F1B99
	public bool HasEverHadValidValues
	{
		get
		{
			return this.validSampleCount >= this.maxSamples;
		}
	}

	// Token: 0x06007ECE RID: 32462 RVA: 0x0032C81C File Offset: 0x0032AA1C
	public void AddSample(float value, float timeOfRecord)
	{
		if (this.ignoreZero && value == 0f)
		{
			return;
		}
		if (value > this.max)
		{
			value = this.max;
		}
		if (value < this.min)
		{
			value = this.min;
		}
		if (this.validSampleCount <= this.maxSamples)
		{
			this.validSampleCount++;
		}
		this.samples.Add(new global::Tuple<float, float>(value, timeOfRecord));
		if (this.samples.Count > this.maxSamples)
		{
			this.samples.RemoveAt(0);
		}
	}

	// Token: 0x06007ECF RID: 32463 RVA: 0x0032C8AC File Offset: 0x0032AAAC
	public int ValidRecordsInLastSeconds(float seconds)
	{
		int num = 0;
		int num2 = this.samples.Count - 1;
		while (num2 >= 0 && Time.time - this.samples[num2].second <= seconds)
		{
			num++;
			num2--;
		}
		return num;
	}

	// Token: 0x06007ED0 RID: 32464 RVA: 0x0032C8F4 File Offset: 0x0032AAF4
	private float GetAverageOfLastSeconds(float seconds)
	{
		float num = 0f;
		int num2 = 0;
		int num3 = this.samples.Count - 1;
		while (num3 >= 0 && Time.time - this.samples[num3].second <= seconds)
		{
			num += this.samples[num3].first;
			num2++;
			num3--;
		}
		if (num2 == 0)
		{
			return 0f;
		}
		return num / (float)num2;
	}

	// Token: 0x04006017 RID: 24599
	private List<global::Tuple<float, float>> samples = new List<global::Tuple<float, float>>();

	// Token: 0x04006018 RID: 24600
	private float min;

	// Token: 0x04006019 RID: 24601
	private float max;

	// Token: 0x0400601A RID: 24602
	private bool ignoreZero;

	// Token: 0x0400601B RID: 24603
	private int validSampleCount;

	// Token: 0x0400601C RID: 24604
	private int maxSamples = 20;
}
