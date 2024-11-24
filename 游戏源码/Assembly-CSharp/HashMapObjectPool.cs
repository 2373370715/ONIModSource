using System;
using System.Collections.Generic;

// Token: 0x02000600 RID: 1536
public class HashMapObjectPool<PoolKey, PoolValue>
{
	// Token: 0x06001BE7 RID: 7143 RVA: 0x000B2367 File Offset: 0x000B0567
	public HashMapObjectPool(Func<PoolKey, PoolValue> instantiator, int initialCount = 0)
	{
		this.initialCount = initialCount;
		this.instantiator = instantiator;
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x001AC94C File Offset: 0x001AAB4C
	public HashMapObjectPool(HashMapObjectPool<PoolKey, PoolValue>.IPoolDescriptor[] descriptors, int initialCount = 0)
	{
		this.initialCount = initialCount;
		for (int i = 0; i < descriptors.Length; i++)
		{
			if (this.objectPoolMap.ContainsKey(descriptors[i].PoolId))
			{
				Debug.LogWarning(string.Format("HshMapObjectPool alaready contains key of {0}! Skipping!", descriptors[i].PoolId));
			}
			else
			{
				this.objectPoolMap[descriptors[i].PoolId] = new ObjectPool<PoolValue>(new Func<PoolValue>(descriptors[i].GetInstance), initialCount);
			}
		}
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x001AC9DC File Offset: 0x001AABDC
	public PoolValue GetInstance(PoolKey poolId)
	{
		ObjectPool<PoolValue> objectPool;
		if (!this.objectPoolMap.TryGetValue(poolId, out objectPool))
		{
			objectPool = (this.objectPoolMap[poolId] = new ObjectPool<PoolValue>(new Func<PoolValue>(this.PoolInstantiator), this.initialCount));
		}
		this.currentPoolId = poolId;
		return objectPool.GetInstance();
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x001ACA30 File Offset: 0x001AAC30
	public void ReleaseInstance(PoolKey poolId, PoolValue inst)
	{
		ObjectPool<PoolValue> objectPool;
		if (inst == null || !this.objectPoolMap.TryGetValue(poolId, out objectPool))
		{
			return;
		}
		objectPool.ReleaseInstance(inst);
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x001ACA60 File Offset: 0x001AAC60
	private PoolValue PoolInstantiator()
	{
		if (this.instantiator == null)
		{
			return default(PoolValue);
		}
		return this.instantiator(this.currentPoolId);
	}

	// Token: 0x04001192 RID: 4498
	private Dictionary<PoolKey, ObjectPool<PoolValue>> objectPoolMap = new Dictionary<PoolKey, ObjectPool<PoolValue>>();

	// Token: 0x04001193 RID: 4499
	private int initialCount;

	// Token: 0x04001194 RID: 4500
	private PoolKey currentPoolId;

	// Token: 0x04001195 RID: 4501
	private Func<PoolKey, PoolValue> instantiator;

	// Token: 0x02000601 RID: 1537
	public interface IPoolDescriptor
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06001BEC RID: 7148
		PoolKey PoolId { get; }

		// Token: 0x06001BED RID: 7149
		PoolValue GetInstance();
	}
}
