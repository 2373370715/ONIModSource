using System;
using UnityEngine;

// Token: 0x020005FF RID: 1535
public class GameObjectPool : ObjectPool<GameObject>
{
	// Token: 0x06001BE4 RID: 7140 RVA: 0x000B2355 File Offset: 0x000B0555
	public GameObjectPool(Func<GameObject> instantiator, int initial_count = 0) : base(instantiator, initial_count)
	{
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x000B235F File Offset: 0x000B055F
	public override GameObject GetInstance()
	{
		return base.GetInstance();
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x001AC914 File Offset: 0x001AAB14
	public void Destroy()
	{
		for (int i = this.unused.Count - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.unused.Pop());
		}
	}
}
