using System;
using System.Collections.Generic;
using Klei;

namespace KMod
{
	// Token: 0x020021D8 RID: 8664
	public interface IFileSource
	{
		// Token: 0x0600B78F RID: 46991
		string GetRoot();

		// Token: 0x0600B790 RID: 46992
		bool Exists();

		// Token: 0x0600B791 RID: 46993
		bool Exists(string relative_path);

		// Token: 0x0600B792 RID: 46994
		void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root = "");

		// Token: 0x0600B793 RID: 46995
		IFileDirectory GetFileSystem();

		// Token: 0x0600B794 RID: 46996
		void CopyTo(string path, List<string> extensions = null);

		// Token: 0x0600B795 RID: 46997
		string Read(string relative_path);

		// Token: 0x0600B796 RID: 46998
		void Dispose();
	}
}
