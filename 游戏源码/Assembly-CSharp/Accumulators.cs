using System;
using System.Collections.Generic;

// Token: 0x02000C07 RID: 3079
public class Accumulators
{
	// Token: 0x06003ABF RID: 15039 RVA: 0x000C5E3E File Offset: 0x000C403E
	public Accumulators()
	{
		this.elapsedTime = 0f;
		this.accumulated = new KCompactedVector<float>(0);
		this.average = new KCompactedVector<float>(0);
	}

	// Token: 0x06003AC0 RID: 15040 RVA: 0x000C5E69 File Offset: 0x000C4069
	public HandleVector<int>.Handle Add(string name, KMonoBehaviour owner)
	{
		HandleVector<int>.Handle result = this.accumulated.Allocate(0f);
		this.average.Allocate(0f);
		return result;
	}

	// Token: 0x06003AC1 RID: 15041 RVA: 0x000C5E8C File Offset: 0x000C408C
	public HandleVector<int>.Handle Remove(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return HandleVector<int>.InvalidHandle;
		}
		this.accumulated.Free(handle);
		this.average.Free(handle);
		return HandleVector<int>.InvalidHandle;
	}

	// Token: 0x06003AC2 RID: 15042 RVA: 0x00228888 File Offset: 0x00226A88
	public void Sim200ms(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime < 3f)
		{
			return;
		}
		this.elapsedTime -= 3f;
		List<float> dataList = this.accumulated.GetDataList();
		List<float> dataList2 = this.average.GetDataList();
		int count = dataList.Count;
		float num = 0.33333334f;
		for (int i = 0; i < count; i++)
		{
			dataList2[i] = dataList[i] * num;
			dataList[i] = 0f;
		}
	}

	// Token: 0x06003AC3 RID: 15043 RVA: 0x000C5EBC File Offset: 0x000C40BC
	public float GetAverageRate(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return 0f;
		}
		return this.average.GetData(handle);
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x00228918 File Offset: 0x00226B18
	public void Accumulate(HandleVector<int>.Handle handle, float amount)
	{
		float data = this.accumulated.GetData(handle);
		this.accumulated.SetData(handle, data + amount);
	}

	// Token: 0x04002813 RID: 10259
	private const float TIME_WINDOW = 3f;

	// Token: 0x04002814 RID: 10260
	private float elapsedTime;

	// Token: 0x04002815 RID: 10261
	private KCompactedVector<float> accumulated;

	// Token: 0x04002816 RID: 10262
	private KCompactedVector<float> average;
}
