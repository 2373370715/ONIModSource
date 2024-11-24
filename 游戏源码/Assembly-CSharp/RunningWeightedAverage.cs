using System.Collections.Generic;
using UnityEngine;

public class RunningWeightedAverage
{
	private List<Tuple<float, float>> samples = new List<Tuple<float, float>>();

	private float min;

	private float max;

	private bool ignoreZero;

	private int validSampleCount;

	private int maxSamples = 20;

	public float GetUnweightedAverage => GetAverageOfLastSeconds(4f);

	public bool HasEverHadValidValues => validSampleCount >= maxSamples;

	public RunningWeightedAverage(float minValue = float.MinValue, float maxValue = float.MaxValue, int sampleCount = 20, bool allowZero = true)
	{
		min = minValue;
		max = maxValue;
		ignoreZero = !allowZero;
		samples = new List<Tuple<float, float>>();
	}

	public void AddSample(float value, float timeOfRecord)
	{
		if (!ignoreZero || value != 0f)
		{
			if (value > max)
			{
				value = max;
			}
			if (value < min)
			{
				value = min;
			}
			if (validSampleCount <= maxSamples)
			{
				validSampleCount++;
			}
			samples.Add(new Tuple<float, float>(value, timeOfRecord));
			if (samples.Count > maxSamples)
			{
				samples.RemoveAt(0);
			}
		}
	}

	public int ValidRecordsInLastSeconds(float seconds)
	{
		int num = 0;
		int num2 = samples.Count - 1;
		while (num2 >= 0 && !(Time.time - samples[num2].second > seconds))
		{
			num++;
			num2--;
		}
		return num;
	}

	private float GetAverageOfLastSeconds(float seconds)
	{
		float num = 0f;
		int num2 = 0;
		int num3 = samples.Count - 1;
		while (num3 >= 0 && !(Time.time - samples[num3].second > seconds))
		{
			num += samples[num3].first;
			num2++;
			num3--;
		}
		if (num2 == 0)
		{
			return 0f;
		}
		return num / (float)num2;
	}
}
