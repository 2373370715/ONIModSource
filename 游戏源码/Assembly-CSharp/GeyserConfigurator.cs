using System;
using System.Collections.Generic;
using Klei;
using UnityEngine;

// Token: 0x02001381 RID: 4993
[AddComponentMenu("KMonoBehaviour/scripts/GeyserConfigurator")]
public class GeyserConfigurator : KMonoBehaviour
{
	// Token: 0x0600669D RID: 26269 RVA: 0x002D0000 File Offset: 0x002CE200
	public static GeyserConfigurator.GeyserType FindType(HashedString typeId)
	{
		GeyserConfigurator.GeyserType geyserType = null;
		if (typeId != HashedString.Invalid)
		{
			geyserType = GeyserConfigurator.geyserTypes.Find((GeyserConfigurator.GeyserType t) => t.id == typeId);
		}
		if (geyserType == null)
		{
			global::Debug.LogError(string.Format("Tried finding a geyser with id {0} but it doesn't exist!", typeId.ToString()));
		}
		return geyserType;
	}

	// Token: 0x0600669E RID: 26270 RVA: 0x000E308F File Offset: 0x000E128F
	public GeyserConfigurator.GeyserInstanceConfiguration MakeConfiguration()
	{
		return this.CreateRandomInstance(this.presetType, this.presetMin, this.presetMax);
	}

	// Token: 0x0600669F RID: 26271 RVA: 0x002D006C File Offset: 0x002CE26C
	private GeyserConfigurator.GeyserInstanceConfiguration CreateRandomInstance(HashedString typeId, float min, float max)
	{
		KRandom randomSource = new KRandom(SaveLoader.Instance.clusterDetailSave.globalWorldSeed + (int)base.transform.GetPosition().x + (int)base.transform.GetPosition().y);
		return new GeyserConfigurator.GeyserInstanceConfiguration
		{
			typeId = typeId,
			rateRoll = this.Roll(randomSource, min, max),
			iterationLengthRoll = this.Roll(randomSource, 0f, 1f),
			iterationPercentRoll = this.Roll(randomSource, min, max),
			yearLengthRoll = this.Roll(randomSource, 0f, 1f),
			yearPercentRoll = this.Roll(randomSource, min, max)
		};
	}

	// Token: 0x060066A0 RID: 26272 RVA: 0x000E30A9 File Offset: 0x000E12A9
	private float Roll(KRandom randomSource, float min, float max)
	{
		return (float)(randomSource.NextDouble() * (double)(max - min)) + min;
	}

	// Token: 0x04004D0C RID: 19724
	private static List<GeyserConfigurator.GeyserType> geyserTypes;

	// Token: 0x04004D0D RID: 19725
	public HashedString presetType;

	// Token: 0x04004D0E RID: 19726
	public float presetMin;

	// Token: 0x04004D0F RID: 19727
	public float presetMax = 1f;

	// Token: 0x02001382 RID: 4994
	public enum GeyserShape
	{
		// Token: 0x04004D11 RID: 19729
		Gas,
		// Token: 0x04004D12 RID: 19730
		Liquid,
		// Token: 0x04004D13 RID: 19731
		Molten
	}

	// Token: 0x02001383 RID: 4995
	public class GeyserType
	{
		// Token: 0x060066A2 RID: 26274 RVA: 0x002D011C File Offset: 0x002CE31C
		public GeyserType(string id, SimHashes element, GeyserConfigurator.GeyserShape shape, float temperature, float minRatePerCycle, float maxRatePerCycle, float maxPressure, float minIterationLength = 60f, float maxIterationLength = 1140f, float minIterationPercent = 0.1f, float maxIterationPercent = 0.9f, float minYearLength = 15000f, float maxYearLength = 135000f, float minYearPercent = 0.4f, float maxYearPercent = 0.8f, float geyserTemperature = 372.15f, string DlcID = "")
		{
			this.id = id;
			this.idHash = id;
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
			if (GeyserConfigurator.geyserTypes == null)
			{
				GeyserConfigurator.geyserTypes = new List<GeyserConfigurator.GeyserType>();
			}
			GeyserConfigurator.geyserTypes.Add(this);
		}

		// Token: 0x060066A3 RID: 26275 RVA: 0x000E30CC File Offset: 0x000E12CC
		public GeyserConfigurator.GeyserType AddDisease(SimUtil.DiseaseInfo diseaseInfo)
		{
			this.diseaseInfo = diseaseInfo;
			return this;
		}

		// Token: 0x060066A4 RID: 26276 RVA: 0x002D01E8 File Offset: 0x002CE3E8
		public GeyserType()
		{
			this.id = "Blank";
			this.element = SimHashes.Void;
			this.temperature = 0f;
			this.minRatePerCycle = 0f;
			this.maxRatePerCycle = 0f;
			this.maxPressure = 0f;
			this.minIterationLength = 0f;
			this.maxIterationLength = 0f;
			this.minIterationPercent = 0f;
			this.maxIterationPercent = 0f;
			this.minYearLength = 0f;
			this.maxYearLength = 0f;
			this.minYearPercent = 0f;
			this.maxYearPercent = 0f;
			this.geyserTemperature = 0f;
			this.DlcID = "";
		}

		// Token: 0x04004D14 RID: 19732
		public string id;

		// Token: 0x04004D15 RID: 19733
		public HashedString idHash;

		// Token: 0x04004D16 RID: 19734
		public SimHashes element;

		// Token: 0x04004D17 RID: 19735
		public GeyserConfigurator.GeyserShape shape;

		// Token: 0x04004D18 RID: 19736
		public float temperature;

		// Token: 0x04004D19 RID: 19737
		public float minRatePerCycle;

		// Token: 0x04004D1A RID: 19738
		public float maxRatePerCycle;

		// Token: 0x04004D1B RID: 19739
		public float maxPressure;

		// Token: 0x04004D1C RID: 19740
		public SimUtil.DiseaseInfo diseaseInfo = SimUtil.DiseaseInfo.Invalid;

		// Token: 0x04004D1D RID: 19741
		public float minIterationLength;

		// Token: 0x04004D1E RID: 19742
		public float maxIterationLength;

		// Token: 0x04004D1F RID: 19743
		public float minIterationPercent;

		// Token: 0x04004D20 RID: 19744
		public float maxIterationPercent;

		// Token: 0x04004D21 RID: 19745
		public float minYearLength;

		// Token: 0x04004D22 RID: 19746
		public float maxYearLength;

		// Token: 0x04004D23 RID: 19747
		public float minYearPercent;

		// Token: 0x04004D24 RID: 19748
		public float maxYearPercent;

		// Token: 0x04004D25 RID: 19749
		public float geyserTemperature;

		// Token: 0x04004D26 RID: 19750
		public string DlcID;

		// Token: 0x04004D27 RID: 19751
		public const string BLANK_ID = "Blank";

		// Token: 0x04004D28 RID: 19752
		public const SimHashes BLANK_ELEMENT = SimHashes.Void;

		// Token: 0x04004D29 RID: 19753
		public const string BLANK_DLCID = "";
	}

	// Token: 0x02001384 RID: 4996
	[Serializable]
	public class GeyserInstanceConfiguration
	{
		// Token: 0x060066A5 RID: 26277 RVA: 0x000E30D6 File Offset: 0x000E12D6
		public Geyser.GeyserModification GetModifier()
		{
			return this.modifier;
		}

		// Token: 0x060066A6 RID: 26278 RVA: 0x002D02B8 File Offset: 0x002CE4B8
		public void Init(bool reinit = false)
		{
			if (this.didInit && !reinit)
			{
				return;
			}
			this.didInit = true;
			this.scaledRate = this.Resample(this.rateRoll, this.geyserType.minRatePerCycle, this.geyserType.maxRatePerCycle);
			this.scaledIterationLength = this.Resample(this.iterationLengthRoll, this.geyserType.minIterationLength, this.geyserType.maxIterationLength);
			this.scaledIterationPercent = this.Resample(this.iterationPercentRoll, this.geyserType.minIterationPercent, this.geyserType.maxIterationPercent);
			this.scaledYearLength = this.Resample(this.yearLengthRoll, this.geyserType.minYearLength, this.geyserType.maxYearLength);
			this.scaledYearPercent = this.Resample(this.yearPercentRoll, this.geyserType.minYearPercent, this.geyserType.maxYearPercent);
		}

		// Token: 0x060066A7 RID: 26279 RVA: 0x000E30DE File Offset: 0x000E12DE
		public void SetModifier(Geyser.GeyserModification modifier)
		{
			this.modifier = modifier;
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x060066A8 RID: 26280 RVA: 0x000E30E7 File Offset: 0x000E12E7
		public GeyserConfigurator.GeyserType geyserType
		{
			get
			{
				return GeyserConfigurator.FindType(this.typeId);
			}
		}

		// Token: 0x060066A9 RID: 26281 RVA: 0x002D03A0 File Offset: 0x002CE5A0
		private float GetModifiedValue(float geyserVariable, float modifier, Geyser.ModificationMethod method)
		{
			float num = geyserVariable;
			if (method != Geyser.ModificationMethod.Values)
			{
				if (method == Geyser.ModificationMethod.Percentages)
				{
					num += geyserVariable * modifier;
				}
			}
			else
			{
				num += modifier;
			}
			return num;
		}

		// Token: 0x060066AA RID: 26282 RVA: 0x000E30F4 File Offset: 0x000E12F4
		public float GetMaxPressure()
		{
			return this.GetModifiedValue(this.geyserType.maxPressure, this.modifier.maxPressureModifier, Geyser.maxPressureModificationMethod);
		}

		// Token: 0x060066AB RID: 26283 RVA: 0x000E3117 File Offset: 0x000E1317
		public float GetIterationLength()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledIterationLength, this.modifier.iterationDurationModifier, Geyser.IterationDurationModificationMethod);
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x000E313C File Offset: 0x000E133C
		public float GetIterationPercent()
		{
			this.Init(false);
			return Mathf.Clamp(this.GetModifiedValue(this.scaledIterationPercent, this.modifier.iterationPercentageModifier, Geyser.IterationPercentageModificationMethod), 0f, 1f);
		}

		// Token: 0x060066AD RID: 26285 RVA: 0x000E3170 File Offset: 0x000E1370
		public float GetOnDuration()
		{
			return this.GetIterationLength() * this.GetIterationPercent();
		}

		// Token: 0x060066AE RID: 26286 RVA: 0x000E317F File Offset: 0x000E137F
		public float GetOffDuration()
		{
			return this.GetIterationLength() * (1f - this.GetIterationPercent());
		}

		// Token: 0x060066AF RID: 26287 RVA: 0x000E3194 File Offset: 0x000E1394
		public float GetMassPerCycle()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledRate, this.modifier.massPerCycleModifier, Geyser.massModificationMethod);
		}

		// Token: 0x060066B0 RID: 26288 RVA: 0x002D03C4 File Offset: 0x002CE5C4
		public float GetEmitRate()
		{
			float num = 600f / this.GetIterationLength();
			return this.GetMassPerCycle() / num / this.GetOnDuration();
		}

		// Token: 0x060066B1 RID: 26289 RVA: 0x000E31B9 File Offset: 0x000E13B9
		public float GetYearLength()
		{
			this.Init(false);
			return this.GetModifiedValue(this.scaledYearLength, this.modifier.yearDurationModifier, Geyser.yearDurationModificationMethod);
		}

		// Token: 0x060066B2 RID: 26290 RVA: 0x000E31DE File Offset: 0x000E13DE
		public float GetYearPercent()
		{
			this.Init(false);
			return Mathf.Clamp(this.GetModifiedValue(this.scaledYearPercent, this.modifier.yearPercentageModifier, Geyser.yearPercentageModificationMethod), 0f, 1f);
		}

		// Token: 0x060066B3 RID: 26291 RVA: 0x000E3212 File Offset: 0x000E1412
		public float GetYearOnDuration()
		{
			return this.GetYearLength() * this.GetYearPercent();
		}

		// Token: 0x060066B4 RID: 26292 RVA: 0x000E3221 File Offset: 0x000E1421
		public float GetYearOffDuration()
		{
			return this.GetYearLength() * (1f - this.GetYearPercent());
		}

		// Token: 0x060066B5 RID: 26293 RVA: 0x000E3236 File Offset: 0x000E1436
		public SimHashes GetElement()
		{
			if (!this.modifier.modifyElement || this.modifier.newElement == (SimHashes)0)
			{
				return this.geyserType.element;
			}
			return this.modifier.newElement;
		}

		// Token: 0x060066B6 RID: 26294 RVA: 0x000E3269 File Offset: 0x000E1469
		public float GetTemperature()
		{
			return this.GetModifiedValue(this.geyserType.temperature, this.modifier.temperatureModifier, Geyser.temperatureModificationMethod);
		}

		// Token: 0x060066B7 RID: 26295 RVA: 0x000E328C File Offset: 0x000E148C
		public byte GetDiseaseIdx()
		{
			return this.geyserType.diseaseInfo.idx;
		}

		// Token: 0x060066B8 RID: 26296 RVA: 0x000E329E File Offset: 0x000E149E
		public int GetDiseaseCount()
		{
			return this.geyserType.diseaseInfo.count;
		}

		// Token: 0x060066B9 RID: 26297 RVA: 0x002D03F0 File Offset: 0x002CE5F0
		public float GetAverageEmission()
		{
			float num = this.GetEmitRate() * this.GetOnDuration();
			return this.GetYearOnDuration() / this.GetIterationLength() * num / this.GetYearLength();
		}

		// Token: 0x060066BA RID: 26298 RVA: 0x002D0424 File Offset: 0x002CE624
		private float Resample(float t, float min, float max)
		{
			float num = 6f;
			float num2 = 0.002472623f;
			float num3 = t * (1f - num2 * 2f) + num2;
			return (-Mathf.Log(1f / num3 - 1f) + num) / (num * 2f) * (max - min) + min;
		}

		// Token: 0x04004D2A RID: 19754
		public HashedString typeId;

		// Token: 0x04004D2B RID: 19755
		public float rateRoll;

		// Token: 0x04004D2C RID: 19756
		public float iterationLengthRoll;

		// Token: 0x04004D2D RID: 19757
		public float iterationPercentRoll;

		// Token: 0x04004D2E RID: 19758
		public float yearLengthRoll;

		// Token: 0x04004D2F RID: 19759
		public float yearPercentRoll;

		// Token: 0x04004D30 RID: 19760
		public float scaledRate;

		// Token: 0x04004D31 RID: 19761
		public float scaledIterationLength;

		// Token: 0x04004D32 RID: 19762
		public float scaledIterationPercent;

		// Token: 0x04004D33 RID: 19763
		public float scaledYearLength;

		// Token: 0x04004D34 RID: 19764
		public float scaledYearPercent;

		// Token: 0x04004D35 RID: 19765
		private bool didInit;

		// Token: 0x04004D36 RID: 19766
		private Geyser.GeyserModification modifier;
	}
}
