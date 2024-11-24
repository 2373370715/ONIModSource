using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/GeyserConfigurator")]
public class GeyserConfigurator : KMonoBehaviour
{
	public enum GeyserShape
	{
		Gas,
		Liquid,
		Molten
	}

	public class GeyserType
	{
		public string id;

		public HashedString idHash;

		public SimHashes element;

		public GeyserShape shape;

		public float temperature;

		public float minRatePerCycle;

		public float maxRatePerCycle;

		public float maxPressure;

		public SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;

		public float minIterationLength;

		public float maxIterationLength;

		public float minIterationPercent;

		public float maxIterationPercent;

		public float minYearLength;

		public float maxYearLength;

		public float minYearPercent;

		public float maxYearPercent;

		public float geyserTemperature;

		public string DlcID;

		public const string BLANK_ID = "Blank";

		public const SimHashes BLANK_ELEMENT = SimHashes.Void;

		public const string BLANK_DLCID = "";

		public GeyserType(string id, SimHashes element, GeyserShape shape, float temperature, float minRatePerCycle, float maxRatePerCycle, float maxPressure, float minIterationLength = 60f, float maxIterationLength = 1140f, float minIterationPercent = 0.1f, float maxIterationPercent = 0.9f, float minYearLength = 15000f, float maxYearLength = 135000f, float minYearPercent = 0.4f, float maxYearPercent = 0.8f, float geyserTemperature = 372.15f, string DlcID = "")
		{
			this.id = id;
			idHash = id;
			this.element = element;
			this.shape = shape;
			this.temperature = temperature;
			this.minRatePerCycle = minRatePerCycle;
			this.maxRatePerCycle = maxRatePerCycle;
			this.maxPressure = maxPressure;
			this.minIterationLength = minIterationLength;
			this.maxIterationLength = maxIterationLength;
			this.minIterationPercent = minIterationPercent;
			this.maxIterationPercent = maxIterationPercent;
			this.minYearLength = minYearLength;
			this.maxYearLength = maxYearLength;
			this.minYearPercent = minYearPercent;
			this.maxYearPercent = maxYearPercent;
			this.DlcID = DlcID;
			this.geyserTemperature = geyserTemperature;
			if (geyserTypes == null)
			{
				geyserTypes = new List<GeyserType>();
			}
			geyserTypes.Add(this);
		}

		public GeyserType AddDisease(SimUtil.DiseaseInfo diseaseInfo)
		{
			this.diseaseInfo = diseaseInfo;
			return this;
		}

		public GeyserType()
		{
			id = "Blank";
			element = SimHashes.Void;
			temperature = 0f;
			minRatePerCycle = 0f;
			maxRatePerCycle = 0f;
			maxPressure = 0f;
			minIterationLength = 0f;
			maxIterationLength = 0f;
			minIterationPercent = 0f;
			maxIterationPercent = 0f;
			minYearLength = 0f;
			maxYearLength = 0f;
			minYearPercent = 0f;
			maxYearPercent = 0f;
			geyserTemperature = 0f;
			DlcID = "";
		}
	}

	[Serializable]
	public class GeyserInstanceConfiguration
	{
		public HashedString typeId;

		public float rateRoll;

		public float iterationLengthRoll;

		public float iterationPercentRoll;

		public float yearLengthRoll;

		public float yearPercentRoll;

		public float scaledRate;

		public float scaledIterationLength;

		public float scaledIterationPercent;

		public float scaledYearLength;

		public float scaledYearPercent;

		private bool didInit;

		private Geyser.GeyserModification modifier;

		public GeyserType geyserType => FindType(typeId);

		public Geyser.GeyserModification GetModifier()
		{
			return modifier;
		}

		public void Init(bool reinit = false)
		{
			if (!didInit || reinit)
			{
				didInit = true;
				scaledRate = Resample(rateRoll, geyserType.minRatePerCycle, geyserType.maxRatePerCycle);
				scaledIterationLength = Resample(iterationLengthRoll, geyserType.minIterationLength, geyserType.maxIterationLength);
				scaledIterationPercent = Resample(iterationPercentRoll, geyserType.minIterationPercent, geyserType.maxIterationPercent);
				scaledYearLength = Resample(yearLengthRoll, geyserType.minYearLength, geyserType.maxYearLength);
				scaledYearPercent = Resample(yearPercentRoll, geyserType.minYearPercent, geyserType.maxYearPercent);
			}
		}

		public void SetModifier(Geyser.GeyserModification modifier)
		{
			this.modifier = modifier;
		}

		private float GetModifiedValue(float geyserVariable, float modifier, Geyser.ModificationMethod method)
		{
			float num = geyserVariable;
			switch (method)
			{
			case Geyser.ModificationMethod.Percentages:
				num += geyserVariable * modifier;
				break;
			case Geyser.ModificationMethod.Values:
				num += modifier;
				break;
			}
			return num;
		}

		public float GetMaxPressure()
		{
			return GetModifiedValue(geyserType.maxPressure, modifier.maxPressureModifier, Geyser.maxPressureModificationMethod);
		}

		public float GetIterationLength()
		{
			Init();
			return GetModifiedValue(scaledIterationLength, modifier.iterationDurationModifier, Geyser.IterationDurationModificationMethod);
		}

		public float GetIterationPercent()
		{
			Init();
			return Mathf.Clamp(GetModifiedValue(scaledIterationPercent, modifier.iterationPercentageModifier, Geyser.IterationPercentageModificationMethod), 0f, 1f);
		}

		public float GetOnDuration()
		{
			return GetIterationLength() * GetIterationPercent();
		}

		public float GetOffDuration()
		{
			return GetIterationLength() * (1f - GetIterationPercent());
		}

		public float GetMassPerCycle()
		{
			Init();
			return GetModifiedValue(scaledRate, modifier.massPerCycleModifier, Geyser.massModificationMethod);
		}

		public float GetEmitRate()
		{
			float num = 600f / GetIterationLength();
			return GetMassPerCycle() / num / GetOnDuration();
		}

		public float GetYearLength()
		{
			Init();
			return GetModifiedValue(scaledYearLength, modifier.yearDurationModifier, Geyser.yearDurationModificationMethod);
		}

		public float GetYearPercent()
		{
			Init();
			return Mathf.Clamp(GetModifiedValue(scaledYearPercent, modifier.yearPercentageModifier, Geyser.yearPercentageModificationMethod), 0f, 1f);
		}

		public float GetYearOnDuration()
		{
			return GetYearLength() * GetYearPercent();
		}

		public float GetYearOffDuration()
		{
			return GetYearLength() * (1f - GetYearPercent());
		}

		public SimHashes GetElement()
		{
			if (!modifier.modifyElement || modifier.newElement == (SimHashes)0)
			{
				return geyserType.element;
			}
			return modifier.newElement;
		}

		public float GetTemperature()
		{
			return GetModifiedValue(geyserType.temperature, modifier.temperatureModifier, Geyser.temperatureModificationMethod);
		}

		public byte GetDiseaseIdx()
		{
			return geyserType.diseaseInfo.idx;
		}

		public int GetDiseaseCount()
		{
			return geyserType.diseaseInfo.count;
		}

		public float GetAverageEmission()
		{
			float num = GetEmitRate() * GetOnDuration();
			return GetYearOnDuration() / GetIterationLength() * num / GetYearLength();
		}

		private float Resample(float t, float min, float max)
		{
			float num = 6f;
			float num2 = 0.002472623f;
			float num3 = t * (1f - num2 * 2f) + num2;
			return (0f - Mathf.Log(1f / num3 - 1f) + num) / (num * 2f) * (max - min) + min;
		}
	}

	private static List<GeyserType> geyserTypes;

	public HashedString presetType;

	public float presetMin;

	public float presetMax = 1f;

	public static GeyserType FindType(HashedString typeId)
	{
		GeyserType geyserType = null;
		if (typeId != HashedString.Invalid)
		{
			geyserType = geyserTypes.Find((GeyserType t) => t.id == typeId);
		}
		if (geyserType == null)
		{
			Debug.LogError($"Tried finding a geyser with id {typeId.ToString()} but it doesn't exist!");
		}
		return geyserType;
	}

	public GeyserInstanceConfiguration MakeConfiguration()
	{
		return CreateRandomInstance(presetType, presetMin, presetMax);
	}

	private GeyserInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		KRandom randomSource = new KRandom(SaveLoader.Instance.clusterDetailSave.globalWorldSeed + (int)base.transform.GetPosition().x + (int)base.transform.GetPosition().y);
		return new GeyserInstanceConfiguration
		{
			typeId = typeId,
			rateRoll = Roll(randomSource, min, max),
			iterationLengthRoll = Roll(randomSource, 0f, 1f),
			iterationPercentRoll = Roll(randomSource, min, max),
			yearLengthRoll = Roll(randomSource, 0f, 1f),
			yearPercentRoll = Roll(randomSource, min, max)
		};
	}

	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}
}
