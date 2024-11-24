using System;

namespace Klei
{
	// Token: 0x02003AE0 RID: 15072
	public struct SolidInfo
	{
		// Token: 0x0600E7BA RID: 59322 RVA: 0x0013B018 File Offset: 0x00139218
		public SolidInfo(int cellIdx, bool isSolid)
		{
			this.cellIdx = cellIdx;
			this.isSolid = isSolid;
		}

		// Token: 0x0400E383 RID: 58243
		public int cellIdx;

		// Token: 0x0400E384 RID: 58244
		public bool isSolid;
	}
}
