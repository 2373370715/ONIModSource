using System;
using UnityEngine;

public class GameObjectPool : ObjectPool<GameObject>
{
	public GameObjectPool(Func<GameObject> instantiator, int initial_count = 0)
		: base(instantiator, initial_count)
	{
	}

	public override GameObject GetInstance()
	{
		return base.GetInstance();
	}

	public void Destroy()
	{
		for (int num = unused.Count - 1; num >= 0; num--)
		{
			UnityEngine.Object.Destroy(unused.Pop());
		}
	}
}
