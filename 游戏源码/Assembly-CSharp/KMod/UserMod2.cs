using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace KMod
{
	// Token: 0x020021CA RID: 8650
	public class UserMod2
	{
		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x0600B767 RID: 46951 RVA: 0x00116266 File Offset: 0x00114466
		// (set) Token: 0x0600B768 RID: 46952 RVA: 0x0011626E File Offset: 0x0011446E
		public Assembly assembly { get; set; }

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x0600B769 RID: 46953 RVA: 0x00116277 File Offset: 0x00114477
		// (set) Token: 0x0600B76A RID: 46954 RVA: 0x0011627F File Offset: 0x0011447F
		public string path { get; set; }

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x0600B76B RID: 46955 RVA: 0x00116288 File Offset: 0x00114488
		// (set) Token: 0x0600B76C RID: 46956 RVA: 0x00116290 File Offset: 0x00114490
		public Mod mod { get; set; }

		// Token: 0x0600B76D RID: 46957 RVA: 0x00116299 File Offset: 0x00114499
		public virtual void OnLoad(Harmony harmony)
		{
			harmony.PatchAll(this.assembly);
		}

		// Token: 0x0600B76E RID: 46958 RVA: 0x000A5E40 File Offset: 0x000A4040
		public virtual void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
		{
		}
	}
}
