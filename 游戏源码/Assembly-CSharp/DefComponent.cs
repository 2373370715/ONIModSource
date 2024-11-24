using System;
using UnityEngine;

// Token: 0x0200082F RID: 2095
[Serializable]
public class DefComponent<T> where T : Component
{
	// Token: 0x0600257E RID: 9598 RVA: 0x000B88FD File Offset: 0x000B6AFD
	public DefComponent(T cmp)
	{
		this.cmp = cmp;
	}

	// Token: 0x0600257F RID: 9599 RVA: 0x001CC5C4 File Offset: 0x001CA7C4
	public T Get(StateMachine.Instance smi)
	{
		T[] components = this.cmp.GetComponents<T>();
		int num = 0;
		while (num < components.Length && !(components[num] == this.cmp))
		{
			num++;
		}
		return smi.gameObject.GetComponents<T>()[num];
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x000B890C File Offset: 0x000B6B0C
	public static implicit operator DefComponent<T>(T cmp)
	{
		return new DefComponent<T>(cmp);
	}

	// Token: 0x04001950 RID: 6480
	[SerializeField]
	private T cmp;
}
