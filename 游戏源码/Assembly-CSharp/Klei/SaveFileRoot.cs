using System;
using System.Collections.Generic;
using KMod;

namespace Klei
{
	// Token: 0x02003AE2 RID: 15074
	internal class SaveFileRoot
	{
		// Token: 0x0600E7BD RID: 59325 RVA: 0x0013B031 File Offset: 0x00139231
		public SaveFileRoot()
		{
			this.streamed = new Dictionary<string, byte[]>();
		}

		// Token: 0x0400E386 RID: 58246
		public int WidthInCells;

		// Token: 0x0400E387 RID: 58247
		public int HeightInCells;

		// Token: 0x0400E388 RID: 58248
		public Dictionary<string, byte[]> streamed;

		// Token: 0x0400E389 RID: 58249
		public string clusterID;

		// Token: 0x0400E38A RID: 58250
		public List<ModInfo> requiredMods;

		// Token: 0x0400E38B RID: 58251
		public List<Label> active_mods;
	}
}
