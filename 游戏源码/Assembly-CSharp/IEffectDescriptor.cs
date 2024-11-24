using System;
using System.Collections.Generic;

// Token: 0x02001403 RID: 5123
[Obsolete("No longer used. Use IGameObjectEffectDescriptor instead", false)]
public interface IEffectDescriptor
{
	// Token: 0x0600693E RID: 26942
	List<Descriptor> GetDescriptors(BuildingDef def);
}
