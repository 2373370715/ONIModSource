using System;
using System.Collections.Generic;

namespace Klei
{
	// Token: 0x02003AE4 RID: 15076
	public class WorldGenSave
	{
		// Token: 0x0600E7BF RID: 59327 RVA: 0x0013B044 File Offset: 0x00139244
		public WorldGenSave()
		{
			this.data = new Data();
		}

		// Token: 0x0400E399 RID: 58265
		public Vector2I version;

		// Token: 0x0400E39A RID: 58266
		public Data data;

		// Token: 0x0400E39B RID: 58267
		public string worldID;

		// Token: 0x0400E39C RID: 58268
		public List<string> traitIDs;

		// Token: 0x0400E39D RID: 58269
		public List<string> storyTraitIDs;
	}
}
