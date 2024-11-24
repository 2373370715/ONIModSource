using System;
using Klei;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;

// Token: 0x020010D6 RID: 4310
public class ConduitDiseaseManager : KCompactedVector<ConduitDiseaseManager.Data>
{
	// Token: 0x06005883 RID: 22659 RVA: 0x0028C254 File Offset: 0x0028A454
	private static ElemGrowthInfo GetGrowthInfo(byte disease_idx, ushort elem_idx)
	{
		ElemGrowthInfo result;
		if (disease_idx != 255)
		{
			result = Db.Get().Diseases[(int)disease_idx].elemGrowthInfo[(int)elem_idx];
		}
		else
		{
			result = Disease.DEFAULT_GROWTH_INFO;
		}
		return result;
	}

	// Token: 0x06005884 RID: 22660 RVA: 0x000D9B83 File Offset: 0x000D7D83
	public ConduitDiseaseManager(ConduitTemperatureManager temperature_manager) : base(0)
	{
		this.temperatureManager = temperature_manager;
	}

	// Token: 0x06005885 RID: 22661 RVA: 0x0028C290 File Offset: 0x0028A490
	public HandleVector<int>.Handle Allocate(HandleVector<int>.Handle temperature_handle, ref ConduitFlow.ConduitContents contents)
	{
		ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
		ConduitDiseaseManager.Data initial_data = new ConduitDiseaseManager.Data(temperature_handle, elementIndex, contents.mass, contents.diseaseIdx, contents.diseaseCount);
		return base.Allocate(initial_data);
	}

	// Token: 0x06005886 RID: 22662 RVA: 0x0028C2CC File Offset: 0x0028A4CC
	public void SetData(HandleVector<int>.Handle handle, ref ConduitFlow.ConduitContents contents)
	{
		ConduitDiseaseManager.Data data = base.GetData(handle);
		data.diseaseCount = contents.diseaseCount;
		if (contents.diseaseIdx != data.diseaseIdx)
		{
			data.diseaseIdx = contents.diseaseIdx;
			ushort elementIndex = ElementLoader.GetElementIndex(contents.element);
			data.growthInfo = ConduitDiseaseManager.GetGrowthInfo(contents.diseaseIdx, elementIndex);
		}
		base.SetData(handle, data);
	}

	// Token: 0x06005887 RID: 22663 RVA: 0x0028C330 File Offset: 0x0028A530
	public void Sim200ms(float dt)
	{
		using (new KProfiler.Region("ConduitDiseaseManager.SimUpdate", null))
		{
			for (int i = 0; i < this.data.Count; i++)
			{
				ConduitDiseaseManager.Data data = this.data[i];
				if (data.diseaseIdx != 255)
				{
					float num = data.accumulatedError;
					num += data.growthInfo.CalculateDiseaseCountDelta(data.diseaseCount, data.mass, dt);
					Disease disease = Db.Get().Diseases[(int)data.diseaseIdx];
					float num2 = Disease.HalfLifeToGrowthRate(Disease.CalculateRangeHalfLife(this.temperatureManager.GetTemperature(data.temperatureHandle), ref disease.temperatureRange, ref disease.temperatureHalfLives), dt);
					num += (float)data.diseaseCount * num2 - (float)data.diseaseCount;
					int num3 = (int)num;
					data.accumulatedError = num - (float)num3;
					data.diseaseCount += num3;
					if (data.diseaseCount <= 0)
					{
						data.diseaseCount = 0;
						data.diseaseIdx = byte.MaxValue;
						data.accumulatedError = 0f;
					}
					this.data[i] = data;
				}
			}
		}
	}

	// Token: 0x06005888 RID: 22664 RVA: 0x0028C480 File Offset: 0x0028A680
	public void ModifyDiseaseCount(HandleVector<int>.Handle h, int disease_count_delta)
	{
		ConduitDiseaseManager.Data data = base.GetData(h);
		data.diseaseCount = Math.Max(0, data.diseaseCount + disease_count_delta);
		if (data.diseaseCount == 0)
		{
			data.diseaseIdx = byte.MaxValue;
		}
		base.SetData(h, data);
	}

	// Token: 0x06005889 RID: 22665 RVA: 0x0028C4C8 File Offset: 0x0028A6C8
	public void AddDisease(HandleVector<int>.Handle h, byte disease_idx, int disease_count)
	{
		ConduitDiseaseManager.Data data = base.GetData(h);
		SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(disease_idx, disease_count, data.diseaseIdx, data.diseaseCount);
		data.diseaseIdx = diseaseInfo.idx;
		data.diseaseCount = diseaseInfo.count;
		base.SetData(h, data);
	}

	// Token: 0x04003E67 RID: 15975
	private ConduitTemperatureManager temperatureManager;

	// Token: 0x020010D7 RID: 4311
	public struct Data
	{
		// Token: 0x0600588A RID: 22666 RVA: 0x000D9B93 File Offset: 0x000D7D93
		public Data(HandleVector<int>.Handle temperature_handle, ushort elem_idx, float mass, byte disease_idx, int disease_count)
		{
			this.diseaseIdx = disease_idx;
			this.elemIdx = elem_idx;
			this.mass = mass;
			this.diseaseCount = disease_count;
			this.accumulatedError = 0f;
			this.temperatureHandle = temperature_handle;
			this.growthInfo = ConduitDiseaseManager.GetGrowthInfo(disease_idx, elem_idx);
		}

		// Token: 0x04003E68 RID: 15976
		public byte diseaseIdx;

		// Token: 0x04003E69 RID: 15977
		public ushort elemIdx;

		// Token: 0x04003E6A RID: 15978
		public int diseaseCount;

		// Token: 0x04003E6B RID: 15979
		public float accumulatedError;

		// Token: 0x04003E6C RID: 15980
		public float mass;

		// Token: 0x04003E6D RID: 15981
		public HandleVector<int>.Handle temperatureHandle;

		// Token: 0x04003E6E RID: 15982
		public ElemGrowthInfo growthInfo;
	}
}
