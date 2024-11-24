using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Klei;
using UnityEngine;

namespace KMod
{
	// Token: 0x020021DA RID: 8666
	internal struct ZipFile : IFileSource
	{
		// Token: 0x0600B7A1 RID: 47009 RVA: 0x001163CF File Offset: 0x001145CF
		public ZipFile(string filename)
		{
			this.filename = filename;
			this.zipfile = ZipFile.Read(filename);
			this.file_system = new ZipFileDirectory(this.zipfile.Name, this.zipfile, Application.streamingAssetsPath, true);
		}

		// Token: 0x0600B7A2 RID: 47010 RVA: 0x00116406 File Offset: 0x00114606
		public string GetRoot()
		{
			return this.filename;
		}

		// Token: 0x0600B7A3 RID: 47011 RVA: 0x0011640E File Offset: 0x0011460E
		public bool Exists()
		{
			return File.Exists(this.GetRoot());
		}

		// Token: 0x0600B7A4 RID: 47012 RVA: 0x00461CB0 File Offset: 0x0045FEB0
		public bool Exists(string relative_path)
		{
			if (!this.Exists())
			{
				return false;
			}
			using (IEnumerator<ZipEntry> enumerator = this.zipfile.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (FileSystem.Normalize(enumerator.Current.FileName).StartsWith(relative_path))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B7A5 RID: 47013 RVA: 0x00461D18 File Offset: 0x0045FF18
		public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
		{
			HashSetPool<string, ZipFile>.PooledHashSet pooledHashSet = HashSetPool<string, ZipFile>.Allocate();
			string[] array;
			if (!string.IsNullOrEmpty(relative_root))
			{
				relative_root = (relative_root ?? "");
				relative_root = FileSystem.Normalize(relative_root);
				array = relative_root.Split('/', StringSplitOptions.None);
			}
			else
			{
				array = new string[0];
			}
			foreach (ZipEntry zipEntry in this.zipfile)
			{
				List<string> list = (from part in FileSystem.Normalize(zipEntry.FileName).Split('/', StringSplitOptions.None)
				where !string.IsNullOrEmpty(part)
				select part).ToList<string>();
				if (this.IsSharedRoot(array, list))
				{
					list = list.GetRange(array.Length, list.Count - array.Length);
					if (list.Count != 0)
					{
						string text = list[0];
						if (pooledHashSet.Add(text))
						{
							file_system_items.Add(new FileSystemItem
							{
								name = text,
								type = ((1 < list.Count) ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File)
							});
						}
					}
				}
			}
			pooledHashSet.Recycle();
		}

		// Token: 0x0600B7A6 RID: 47014 RVA: 0x00461E40 File Offset: 0x00460040
		private bool IsSharedRoot(string[] root_path, List<string> check_path)
		{
			for (int i = 0; i < root_path.Length; i++)
			{
				if (i >= check_path.Count || root_path[i] != check_path[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B7A7 RID: 47015 RVA: 0x0011641B File Offset: 0x0011461B
		public IFileDirectory GetFileSystem()
		{
			return this.file_system;
		}

		// Token: 0x0600B7A8 RID: 47016 RVA: 0x00461E78 File Offset: 0x00460078
		public void CopyTo(string path, List<string> extensions = null)
		{
			foreach (ZipEntry zipEntry in this.zipfile.Entries)
			{
				bool flag = extensions == null || extensions.Count == 0;
				if (extensions != null)
				{
					foreach (string value in extensions)
					{
						if (zipEntry.FileName.ToLower().EndsWith(value))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					string path2 = FileSystem.Normalize(Path.Combine(path, zipEntry.FileName));
					string directoryName = Path.GetDirectoryName(path2);
					if (string.IsNullOrEmpty(directoryName) || FileUtil.CreateDirectory(directoryName, 0))
					{
						using (MemoryStream memoryStream = new MemoryStream((int)zipEntry.UncompressedSize))
						{
							zipEntry.Extract(memoryStream);
							using (FileStream fileStream = FileUtil.Create(path2, 0))
							{
								fileStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B7A9 RID: 47017 RVA: 0x00461FC8 File Offset: 0x004601C8
		public string Read(string relative_path)
		{
			ICollection<ZipEntry> collection = this.zipfile.SelectEntries(relative_path);
			if (collection.Count == 0)
			{
				return string.Empty;
			}
			foreach (ZipEntry zipEntry in collection)
			{
				using (MemoryStream memoryStream = new MemoryStream((int)zipEntry.UncompressedSize))
				{
					zipEntry.Extract(memoryStream);
					return Encoding.UTF8.GetString(memoryStream.GetBuffer());
				}
			}
			return string.Empty;
		}

		// Token: 0x0600B7AA RID: 47018 RVA: 0x00116423 File Offset: 0x00114623
		public void Dispose()
		{
			this.zipfile.Dispose();
		}

		// Token: 0x0400962F RID: 38447
		private string filename;

		// Token: 0x04009630 RID: 38448
		private ZipFile zipfile;

		// Token: 0x04009631 RID: 38449
		private ZipFileDirectory file_system;
	}
}
