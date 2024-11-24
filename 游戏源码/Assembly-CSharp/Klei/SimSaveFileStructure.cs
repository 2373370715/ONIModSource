using System;

namespace Klei
{
	// Token: 0x02003AE7 RID: 15079
	public class SimSaveFileStructure
	{
		// Token: 0x0600E7C3 RID: 59331 RVA: 0x0013B096 File Offset: 0x00139296
		public SimSaveFileStructure()
		{
			this.worldDetail = new WorldDetailSave();
		}

		// Token: 0x0400E3A6 RID: 58278
		public int WidthInCells;

		// Token: 0x0400E3A7 RID: 58279
		public int HeightInCells;

		// Token: 0x0400E3A8 RID: 58280
		public int x;

		// Token: 0x0400E3A9 RID: 58281
		public int y;

		// Token: 0x0400E3AA RID: 58282
		public byte[] Sim;

		// Token: 0x0400E3AB RID: 58283
		public WorldDetailSave worldDetail;
	}
}
