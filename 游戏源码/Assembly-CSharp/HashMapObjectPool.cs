using System;
using System.Collections.Generic;

public class HashMapObjectPool<PoolKey, PoolValue>
{
	public interface IPoolDescriptor
	{
		PoolKey PoolId { get; }

		PoolValue GetInstance();
	}

	private Dictionary<PoolKey, ObjectPool<PoolValue>> objectPoolMap = new Dictionary<PoolKey, ObjectPool<PoolValue>>();

	private int initialCount;

	private PoolKey currentPoolId;

	private Func<PoolKey, PoolValue> instantiator;

	public HashMapObjectPool(Func<PoolKey, PoolValue> instantiator, int initialCount = 0)
	{
		this.initialCount = initialCount;
		this.instantiator = instantiator;
	}

	public HashMapObjectPool(IPoolDescriptor[] descriptors, int initialCount = 0)
	{
		this.initialCount = initialCount;
		for (int i = 0; i < descriptors.Length; i++)
		{
			if (objectPoolMap.ContainsKey(descriptors[i].PoolId))
			{
				Debug.LogWarning($"HshMapObjectPool alaready contains key of {descriptors[i].PoolId}! Skipping!");
			}
			else
			{
				objectPoolMap[descriptors[i].PoolId] = new ObjectPool<PoolValue>(descriptors[i].GetInstance, initialCount);
			}
		}
	}

	public PoolValue GetInstance(PoolKey poolId)
	{
		if (!objectPoolMap.TryGetValue(poolId, out var value))
		{
			ObjectPool<PoolValue> objectPool2 = (objectPoolMap[poolId] = new ObjectPool<PoolValue>(PoolInstantiator, initialCount));
			value = objectPool2;
		}
		currentPoolId = poolId;
		return value.GetInstance();
	}

	public void ReleaseInstance(PoolKey poolId, PoolValue inst)
	{
		if (inst != null && objectPoolMap.TryGetValue(poolId, out var value))
		{
			value.ReleaseInstance(inst);
		}
	}

	private PoolValue PoolInstantiator()
	{
		if (instantiator == null)
		{
			return default(PoolValue);
		}
		return instantiator(currentPoolId);
	}
}
