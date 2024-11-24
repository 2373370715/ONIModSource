using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace KMod
{
	// Token: 0x020021CB RID: 8651
	public class LoadedModData
	{
		// Token: 0x0400961A RID: 38426
		public Harmony harmony;

		// Token: 0x0400961B RID: 38427
		public Dictionary<Assembly, UserMod2> userMod2Instances;

		// Token: 0x0400961C RID: 38428
		public ICollection<Assembly> dlls;

		// Token: 0x0400961D RID: 38429
		public ICollection<MethodBase> patched_methods;
	}
}
