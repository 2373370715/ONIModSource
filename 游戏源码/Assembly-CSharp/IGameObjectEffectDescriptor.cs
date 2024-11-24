using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001402 RID: 5122
public interface IGameObjectEffectDescriptor
{
	// Token: 0x0600693D RID: 26941
	List<Descriptor> GetDescriptors(GameObject go);
}
