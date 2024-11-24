using System;

namespace KMod
{
	// Token: 0x020021D6 RID: 8662
	public struct FileSystemItem
	{
		// Token: 0x04009628 RID: 38440
		public string name;

		// Token: 0x04009629 RID: 38441
		public FileSystemItem.ItemType type;

		// Token: 0x020021D7 RID: 8663
		public enum ItemType
		{
			// Token: 0x0400962B RID: 38443
			Directory,
			// Token: 0x0400962C RID: 38444
			File
		}
	}
}
