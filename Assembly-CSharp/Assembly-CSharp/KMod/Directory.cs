﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Klei;
using UnityEngine;

namespace KMod
{
		internal struct Directory : IFileSource
	{
				public Directory(string root)
		{
			this.root = root;
			this.file_system = new AliasDirectory(root, root, Application.streamingAssetsPath, true);
		}

				public string GetRoot()
		{
			return this.root;
		}

				public bool Exists()
		{
			return Directory.Exists(this.GetRoot());
		}

				public bool Exists(string relative_path)
		{
			return this.Exists() && new DirectoryInfo(FileSystem.Normalize(Path.Combine(this.root, relative_path))).Exists;
		}

				public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
		{
			relative_root = (relative_root ?? "");
			string text = FileSystem.Normalize(Path.Combine(this.root, relative_root));
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				global::Debug.LogError("Cannot iterate over $" + text + ", this directory does not exist");
				return;
			}
			foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
			{
				file_system_items.Add(new FileSystemItem
				{
					name = fileSystemInfo.Name,
					type = ((fileSystemInfo is DirectoryInfo) ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File)
				});
			}
		}

				public IFileDirectory GetFileSystem()
		{
			return this.file_system;
		}

				public void CopyTo(string path, List<string> extensions = null)
		{
			try
			{
				Directory.CopyDirectory(this.root, path, extensions);
			}
			catch (UnauthorizedAccessException)
			{
				FileUtil.ErrorDialog(FileUtil.ErrorType.UnauthorizedAccess, path, null, null);
			}
			catch (IOException)
			{
				FileUtil.ErrorDialog(FileUtil.ErrorType.IOError, path, null, null);
			}
			catch
			{
				throw;
			}
		}

				public string Read(string relative_path)
		{
			string result;
			try
			{
				using (FileStream fileStream = File.OpenRead(Path.Combine(this.root, relative_path)))
				{
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, (int)fileStream.Length);
					result = Encoding.UTF8.GetString(array);
				}
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

				private static int CopyDirectory(string sourceDirName, string destDirName, List<string> extensions)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			if (!directoryInfo.Exists)
			{
				return 0;
			}
			if (!FileUtil.CreateDirectory(destDirName, 0))
			{
				return 0;
			}
			FileInfo[] files = directoryInfo.GetFiles();
			int num = 0;
			foreach (FileInfo fileInfo in files)
			{
				bool flag = extensions == null || extensions.Count == 0;
				if (extensions != null)
				{
					using (List<string>.Enumerator enumerator = extensions.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == Path.GetExtension(fileInfo.Name).ToLower())
							{
								flag = true;
								break;
							}
						}
					}
				}
				if (flag)
				{
					string destFileName = Path.Combine(destDirName, fileInfo.Name);
					fileInfo.CopyTo(destFileName, false);
					num++;
				}
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
				num += Directory.CopyDirectory(directoryInfo2.FullName, destDirName2, extensions);
			}
			if (num == 0)
			{
				FileUtil.DeleteDirectory(destDirName, 0);
			}
			return num;
		}

				public void Dispose()
		{
		}

				private AliasDirectory file_system;

				private string root;
	}
}
