using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Klei;
using Newtonsoft.Json;
using UnityEngine;

namespace KMod
{
	// Token: 0x020021E0 RID: 8672
	[JsonObject(MemberSerialization.OptIn)]
	[DebuggerDisplay("{title}")]
	public class Mod
	{
		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x0600B7B3 RID: 47027 RVA: 0x001164BC File Offset: 0x001146BC
		// (set) Token: 0x0600B7B4 RID: 47028 RVA: 0x001164C4 File Offset: 0x001146C4
		public Content available_content { get; private set; }

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x0600B7B5 RID: 47029 RVA: 0x001164CD File Offset: 0x001146CD
		// (set) Token: 0x0600B7B6 RID: 47030 RVA: 0x001164D5 File Offset: 0x001146D5
		[JsonProperty]
		public string staticID { get; private set; }

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x0600B7B7 RID: 47031 RVA: 0x001164DE File Offset: 0x001146DE
		// (set) Token: 0x0600B7B8 RID: 47032 RVA: 0x001164E6 File Offset: 0x001146E6
		public LocString manage_tooltip { get; private set; }

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x0600B7B9 RID: 47033 RVA: 0x001164EF File Offset: 0x001146EF
		// (set) Token: 0x0600B7BA RID: 47034 RVA: 0x001164F7 File Offset: 0x001146F7
		public System.Action on_managed { get; private set; }

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x0600B7BB RID: 47035 RVA: 0x00116500 File Offset: 0x00114700
		public bool is_managed
		{
			get
			{
				return this.manage_tooltip != null;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x0600B7BC RID: 47036 RVA: 0x0011650B File Offset: 0x0011470B
		public string title
		{
			get
			{
				return this.label.title;
			}
		}

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x0600B7BD RID: 47037 RVA: 0x00116518 File Offset: 0x00114718
		// (set) Token: 0x0600B7BE RID: 47038 RVA: 0x00116520 File Offset: 0x00114720
		public string description { get; private set; }

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x0600B7BF RID: 47039 RVA: 0x00116529 File Offset: 0x00114729
		// (set) Token: 0x0600B7C0 RID: 47040 RVA: 0x00116531 File Offset: 0x00114731
		public Content loaded_content { get; private set; }

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x0600B7C1 RID: 47041 RVA: 0x0011653A File Offset: 0x0011473A
		// (set) Token: 0x0600B7C2 RID: 47042 RVA: 0x00116542 File Offset: 0x00114742
		public IFileSource file_source
		{
			get
			{
				return this._fileSource;
			}
			set
			{
				if (this._fileSource != null)
				{
					this._fileSource.Dispose();
				}
				this._fileSource = value;
			}
		}

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x0600B7C3 RID: 47043 RVA: 0x0011655E File Offset: 0x0011475E
		// (set) Token: 0x0600B7C4 RID: 47044 RVA: 0x00116566 File Offset: 0x00114766
		public bool DevModCrashTriggered { get; private set; }

		// Token: 0x0600B7C5 RID: 47045 RVA: 0x0011656F File Offset: 0x0011476F
		[JsonConstructor]
		public Mod()
		{
		}

		// Token: 0x0600B7C6 RID: 47046 RVA: 0x0046206C File Offset: 0x0046026C
		public void CopyPersistentDataTo(Mod other_mod)
		{
			other_mod.status = this.status;
			other_mod.enabledForDlc = ((this.enabledForDlc != null) ? new List<string>(this.enabledForDlc) : new List<string>());
			other_mod.crash_count = this.crash_count;
			other_mod.loaded_content = this.loaded_content;
			other_mod.loaded_mod_data = this.loaded_mod_data;
			other_mod.reinstall_path = this.reinstall_path;
		}

		// Token: 0x0600B7C7 RID: 47047 RVA: 0x004620D8 File Offset: 0x004602D8
		public Mod(Label label, string staticID, string description, IFileSource file_source, LocString manage_tooltip, System.Action on_managed)
		{
			this.label = label;
			this.status = Mod.Status.NotInstalled;
			this.staticID = staticID;
			this.description = description;
			this.file_source = file_source;
			this.manage_tooltip = manage_tooltip;
			this.on_managed = on_managed;
			this.loaded_content = (Content)0;
			this.available_content = (Content)0;
			this.ScanContent();
		}

		// Token: 0x0600B7C8 RID: 47048 RVA: 0x00116582 File Offset: 0x00114782
		public bool IsEnabledForActiveDlc()
		{
			return this.IsEnabledForDlc(DlcManager.GetHighestActiveDlcId());
		}

		// Token: 0x0600B7C9 RID: 47049 RVA: 0x0011658F File Offset: 0x0011478F
		public bool IsEnabledForDlc(string dlcId)
		{
			return this.enabledForDlc != null && this.enabledForDlc.Contains(dlcId);
		}

		// Token: 0x0600B7CA RID: 47050 RVA: 0x001165A7 File Offset: 0x001147A7
		public void SetEnabledForActiveDlc(bool enabled)
		{
			this.SetEnabledForDlc(DlcManager.GetHighestActiveDlcId(), enabled);
		}

		// Token: 0x0600B7CB RID: 47051 RVA: 0x00462140 File Offset: 0x00460340
		public void SetEnabledForDlc(string dlcId, bool set_enabled)
		{
			if (this.enabledForDlc == null)
			{
				this.enabledForDlc = new List<string>();
			}
			bool flag = this.enabledForDlc.Contains(dlcId);
			if (set_enabled && !flag)
			{
				this.enabledForDlc.Add(dlcId);
				return;
			}
			if (!set_enabled && flag)
			{
				this.enabledForDlc.Remove(dlcId);
			}
		}

		// Token: 0x0600B7CC RID: 47052 RVA: 0x00462198 File Offset: 0x00460398
		public void ScanContent()
		{
			this.ModDevLog(string.Format("{0} ({1}): Setting up mod.", this.label, this.label.id));
			this.available_content = (Content)0;
			if (this.file_source == null)
			{
				if (this.label.id.EndsWith(".zip"))
				{
					DebugUtil.DevAssert(false, "Does this actually get used ever?", null);
					this.file_source = new ZipFile(this.label.install_path);
				}
				else
				{
					this.file_source = new Directory(this.label.install_path);
				}
			}
			if (!this.file_source.Exists())
			{
				global::Debug.LogWarning(string.Format("{0}: File source does not appear to be valid, skipping. ({1})", this.label, this.label.install_path));
				return;
			}
			KModHeader header = KModUtil.GetHeader(this.file_source, this.label.defaultStaticID, this.label.title, this.description, this.IsDev);
			if (this.label.title != header.title)
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"\t",
					this.label.title,
					" has a mod.yaml with the title `",
					header.title,
					"`, using that from now on."
				}));
			}
			if (this.label.defaultStaticID != header.staticID)
			{
				global::Debug.Log(string.Concat(new string[]
				{
					"\t",
					this.label.title,
					" has a mod.yaml with a staticID `",
					header.staticID,
					"`, using that from now on."
				}));
			}
			this.label.title = header.title;
			this.staticID = header.staticID;
			this.description = header.description;
			Mod.ArchivedVersion mostSuitableArchive = this.GetMostSuitableArchive();
			if (mostSuitableArchive == null)
			{
				global::Debug.LogWarning(string.Format("{0}: No archive supports this game version, skipping content.", this.label));
				this.contentCompatability = ModContentCompatability.DoesntSupportDLCConfig;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			this.packagedModInfo = mostSuitableArchive.info;
			Content content;
			this.ScanContentFromSource(mostSuitableArchive.relativePath, out content);
			if (content == (Content)0)
			{
				global::Debug.LogWarning(string.Format("{0}: No supported content for mod, skipping content.", this.label));
				this.contentCompatability = ModContentCompatability.NoContent;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			bool flag = mostSuitableArchive.info.APIVersion == 2;
			if ((content & Content.DLL) != (Content)0 && !flag)
			{
				global::Debug.LogWarning(string.Format("{0}: DLLs found but not using the correct API version.", this.label));
				this.contentCompatability = ModContentCompatability.OldAPI;
				this.available_content = (Content)0;
				this.SetEnabledForActiveDlc(false);
				return;
			}
			this.contentCompatability = ModContentCompatability.OK;
			this.available_content = content;
			this.relative_root = mostSuitableArchive.relativePath;
			global::Debug.Assert(this.content_source == null);
			this.content_source = new Directory(this.ContentPath);
			string arg = string.IsNullOrEmpty(this.relative_root) ? "root" : this.relative_root;
			global::Debug.Log(string.Format("{0}: Successfully loaded from path '{1}' with content '{2}'.", this.label, arg, this.available_content.ToString()));
		}

		// Token: 0x0600B7CD RID: 47053 RVA: 0x004624C8 File Offset: 0x004606C8
		private Mod.ArchivedVersion GetMostSuitableArchive()
		{
			Mod.PackagedModInfo packagedModInfo = this.GetModInfoForFolder("");
			if (packagedModInfo == null)
			{
				packagedModInfo = new Mod.PackagedModInfo
				{
					supportedContent = "vanilla_id",
					minimumSupportedBuild = 0
				};
				if (this.ScanContentFromSourceForTranslationsOnly(""))
				{
					this.ModDevLogWarning(string.Format("{0}: No mod_info.yaml found, but since it contains a translation, default its supported content to 'ALL'", this.label));
					packagedModInfo.supportedContent = "all";
				}
				else
				{
					this.ModDevLogWarning(string.Format("{0}: No mod_info.yaml found, default its supported content to 'VANILLA_ID'", this.label));
				}
			}
			Mod.ArchivedVersion archivedVersion = new Mod.ArchivedVersion
			{
				relativePath = "",
				info = packagedModInfo
			};
			if (!this.file_source.Exists("archived_versions"))
			{
				this.ModDevLog(string.Format("\t{0}: No archived_versions for this mod, using root version directly.", this.label));
				if (!this.DoesModSupportCurrentContent(packagedModInfo))
				{
					return null;
				}
				return archivedVersion;
			}
			else
			{
				List<FileSystemItem> list = new List<FileSystemItem>();
				this.file_source.GetTopLevelItems(list, "archived_versions");
				if (list.Count == 0)
				{
					this.ModDevLog(string.Format("\t{0}: No archived_versions for this mod, using root version directly.", this.label));
					if (!this.DoesModSupportCurrentContent(packagedModInfo))
					{
						return null;
					}
					return archivedVersion;
				}
				else
				{
					List<Mod.ArchivedVersion> list2 = new List<Mod.ArchivedVersion>();
					list2.Add(archivedVersion);
					foreach (FileSystemItem fileSystemItem in list)
					{
						if (fileSystemItem.type != FileSystemItem.ItemType.File)
						{
							string relativePath = Path.Combine("archived_versions", fileSystemItem.name);
							Mod.PackagedModInfo modInfoForFolder = this.GetModInfoForFolder(relativePath);
							if (modInfoForFolder != null)
							{
								list2.Add(new Mod.ArchivedVersion
								{
									relativePath = relativePath,
									info = modInfoForFolder
								});
							}
						}
					}
					list2 = (from v in list2
					where this.DoesModSupportCurrentContent(v.info)
					select v).ToList<Mod.ArchivedVersion>();
					list2 = (from v in list2
					where v.info.APIVersion == 2 || v.info.APIVersion == 0
					select v).ToList<Mod.ArchivedVersion>();
					Mod.ArchivedVersion archivedVersion2 = (from v in list2
					where (long)v.info.minimumSupportedBuild <= 642695L
					orderby v.info.minimumSupportedBuild descending
					select v).FirstOrDefault<Mod.ArchivedVersion>();
					if (archivedVersion2 == null)
					{
						return null;
					}
					return archivedVersion2;
				}
			}
		}

		// Token: 0x0600B7CE RID: 47054 RVA: 0x00462710 File Offset: 0x00460910
		private Mod.PackagedModInfo GetModInfoForFolder(string relative_root)
		{
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relative_root);
			bool flag = false;
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.File && fileSystemItem.name.ToLower() == "mod_info.yaml")
				{
					flag = true;
					break;
				}
			}
			string text = string.IsNullOrEmpty(relative_root) ? "root" : relative_root;
			if (!flag)
			{
				this.ModDevLogWarning(string.Concat(new string[]
				{
					"\t",
					this.title,
					": has no mod_info.yaml in folder '",
					text,
					"'"
				}));
				return null;
			}
			string text2 = this.file_source.Read(Path.Combine(relative_root, "mod_info.yaml"));
			if (string.IsNullOrEmpty(text2))
			{
				this.ModDevLogError(string.Format("\t{0}: Failed to read {1} in folder '{2}', skipping", this.label, "mod_info.yaml", text));
				return null;
			}
			YamlIO.ErrorHandler handle_error = delegate(YamlIO.Error e, bool force_warning)
			{
				YamlIO.LogError(e, !this.IsDev);
			};
			Mod.PackagedModInfo packagedModInfo = YamlIO.Parse<Mod.PackagedModInfo>(text2, default(FileHandle), handle_error, null);
			if (packagedModInfo == null)
			{
				this.ModDevLogError(string.Format("\t{0}: Failed to parse {1} in folder '{2}', text is {3}", new object[]
				{
					this.label,
					"mod_info.yaml",
					text,
					text2
				}));
				return null;
			}
			if (packagedModInfo.supportedContent == null)
			{
				this.ModDevLogError(string.Format("\t{0}: {1} in folder '{2}' does not specify supportedContent. Make sure you spelled it correctly in your mod_info!", this.label, "mod_info.yaml", text));
				return null;
			}
			if (packagedModInfo.lastWorkingBuild != 0)
			{
				this.ModDevLogError(string.Format("\t{0}: {1} in folder '{2}' is using `{3}`, please upgrade this to `{4}`", new object[]
				{
					this.label,
					"mod_info.yaml",
					text,
					"lastWorkingBuild",
					"minimumSupportedBuild"
				}));
				if (packagedModInfo.minimumSupportedBuild == 0)
				{
					packagedModInfo.minimumSupportedBuild = packagedModInfo.lastWorkingBuild;
				}
			}
			this.ModDevLog(string.Format("\t{0}: Found valid mod_info.yaml in folder '{1}': {2} at {3}", new object[]
			{
				this.label,
				text,
				packagedModInfo.supportedContent,
				packagedModInfo.minimumSupportedBuild
			}));
			return packagedModInfo;
		}

		// Token: 0x0600B7CF RID: 47055 RVA: 0x00462950 File Offset: 0x00460B50
		private bool DoesModSupportCurrentContent(Mod.PackagedModInfo mod_info)
		{
			string text = DlcManager.GetHighestActiveDlcId();
			if (text == "")
			{
				text = "vanilla_id";
			}
			text = text.ToLower();
			string text2 = mod_info.supportedContent.ToLower();
			return text2.Contains(text) || text2.Contains("all");
		}

		// Token: 0x0600B7D0 RID: 47056 RVA: 0x004629A0 File Offset: 0x00460BA0
		private bool ScanContentFromSourceForTranslationsOnly(string relativeRoot)
		{
			this.available_content = (Content)0;
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relativeRoot);
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.File && fileSystemItem.name.ToLower().EndsWith(".po"))
				{
					this.available_content |= Content.Translation;
				}
			}
			return this.available_content > (Content)0;
		}

		// Token: 0x0600B7D1 RID: 47057 RVA: 0x00462A38 File Offset: 0x00460C38
		private bool ScanContentFromSource(string relativeRoot, out Content available)
		{
			available = (Content)0;
			List<FileSystemItem> list = new List<FileSystemItem>();
			this.file_source.GetTopLevelItems(list, relativeRoot);
			foreach (FileSystemItem fileSystemItem in list)
			{
				if (fileSystemItem.type == FileSystemItem.ItemType.Directory)
				{
					string directory = fileSystemItem.name.ToLower();
					available |= this.AddDirectory(directory);
				}
				else
				{
					string file = fileSystemItem.name.ToLower();
					available |= this.AddFile(file);
				}
			}
			return available > (Content)0;
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x0600B7D2 RID: 47058 RVA: 0x001165B5 File Offset: 0x001147B5
		public string ContentPath
		{
			get
			{
				return Path.Combine(this.label.install_path, this.relative_root);
			}
		}

		// Token: 0x0600B7D3 RID: 47059 RVA: 0x001165CD File Offset: 0x001147CD
		public bool IsEmpty()
		{
			return this.available_content == (Content)0;
		}

		// Token: 0x0600B7D4 RID: 47060 RVA: 0x00462AD8 File Offset: 0x00460CD8
		private Content AddDirectory(string directory)
		{
			Content content = (Content)0;
			string text = directory.TrimEnd('/');
			uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
			if (num <= 1519694028U)
			{
				if (num != 948591336U)
				{
					if (num != 1318520008U)
					{
						if (num == 1519694028U)
						{
							if (text == "elements")
							{
								content |= Content.LayerableFiles;
							}
						}
					}
					else if (text == "buildingfacades")
					{
						content |= Content.Animation;
					}
				}
				else if (text == "templates")
				{
					content |= Content.LayerableFiles;
				}
			}
			else if (num <= 3037049615U)
			{
				if (num != 2960291089U)
				{
					if (num == 3037049615U)
					{
						if (text == "worldgen")
						{
							content |= Content.LayerableFiles;
						}
					}
				}
				else if (text == "strings")
				{
					content |= Content.Strings;
				}
			}
			else if (num != 3319670096U)
			{
				if (num == 3570262116U)
				{
					if (text == "codex")
					{
						content |= Content.LayerableFiles;
					}
				}
			}
			else if (text == "anim")
			{
				content |= Content.Animation;
			}
			return content;
		}

		// Token: 0x0600B7D5 RID: 47061 RVA: 0x00462BE8 File Offset: 0x00460DE8
		private Content AddFile(string file)
		{
			Content content = (Content)0;
			if (file.EndsWith(".dll"))
			{
				content |= Content.DLL;
			}
			if (file.EndsWith(".po"))
			{
				content |= Content.Translation;
			}
			return content;
		}

		// Token: 0x0600B7D6 RID: 47062 RVA: 0x001165D8 File Offset: 0x001147D8
		private static void AccumulateExtensions(Content content, List<string> extensions)
		{
			if ((content & Content.DLL) != (Content)0)
			{
				extensions.Add(".dll");
			}
			if ((content & (Content.Strings | Content.Translation)) != (Content)0)
			{
				extensions.Add(".po");
			}
		}

		// Token: 0x0600B7D7 RID: 47063 RVA: 0x00462C1C File Offset: 0x00460E1C
		[Conditional("DEBUG")]
		private void Assert(bool condition, string failure_message)
		{
			if (string.IsNullOrEmpty(this.title))
			{
				DebugUtil.Assert(condition, string.Format("{2}\n\t{0}\n\t{1}", this.title, this.label.ToString(), failure_message));
				return;
			}
			DebugUtil.Assert(condition, string.Format("{1}\n\t{0}", this.label.ToString(), failure_message));
		}

		// Token: 0x0600B7D8 RID: 47064 RVA: 0x00462C84 File Offset: 0x00460E84
		public void Install()
		{
			if (this.IsLocal)
			{
				this.status = Mod.Status.Installed;
				return;
			}
			this.status = Mod.Status.ReinstallPending;
			if (this.file_source == null)
			{
				return;
			}
			if (!FileUtil.DeleteDirectory(this.label.install_path, 0))
			{
				return;
			}
			if (!FileUtil.CreateDirectory(this.label.install_path, 0))
			{
				return;
			}
			this.file_source.CopyTo(this.label.install_path, null);
			this.file_source = new Directory(this.label.install_path);
			this.status = Mod.Status.Installed;
		}

		// Token: 0x0600B7D9 RID: 47065 RVA: 0x00462D14 File Offset: 0x00460F14
		public bool Uninstall()
		{
			this.SetEnabledForActiveDlc(false);
			if (this.loaded_content != (Content)0)
			{
				global::Debug.Log(string.Format("Can't uninstall {0}: still has loaded content: {1}", this.label.ToString(), this.loaded_content.ToString()));
				this.status = Mod.Status.UninstallPending;
				return false;
			}
			if (!this.IsLocal && !FileUtil.DeleteDirectory(this.label.install_path, 0))
			{
				global::Debug.Log(string.Format("Can't uninstall {0}: directory deletion failed", this.label.ToString()));
				this.status = Mod.Status.UninstallPending;
				return false;
			}
			this.status = Mod.Status.NotInstalled;
			return true;
		}

		// Token: 0x0600B7DA RID: 47066 RVA: 0x00462DBC File Offset: 0x00460FBC
		private bool LoadStrings()
		{
			string path = FileSystem.Normalize(Path.Combine(this.ContentPath, "strings"));
			if (!Directory.Exists(path))
			{
				return false;
			}
			int num = 0;
			foreach (FileInfo fileInfo in new DirectoryInfo(path).GetFiles())
			{
				if (!(fileInfo.Extension.ToLower() != ".po"))
				{
					num++;
					Localization.OverloadStrings(Localization.LoadStringsFile(fileInfo.FullName, false));
				}
			}
			return true;
		}

		// Token: 0x0600B7DB RID: 47067 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		private bool LoadTranslations()
		{
			return false;
		}

		// Token: 0x0600B7DC RID: 47068 RVA: 0x00462E3C File Offset: 0x0046103C
		private bool LoadAnimation()
		{
			string path = FileSystem.Normalize(Path.Combine(this.ContentPath, "anim"));
			if (!Directory.Exists(path))
			{
				return false;
			}
			int num = 0;
			DirectoryInfo[] directories = new DirectoryInfo(path).GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				foreach (DirectoryInfo directoryInfo in directories[i].GetDirectories())
				{
					KAnimFile.Mod mod = new KAnimFile.Mod();
					foreach (FileInfo fileInfo in directoryInfo.GetFiles())
					{
						if (fileInfo.Extension == ".png")
						{
							byte[] data = File.ReadAllBytes(fileInfo.FullName);
							Texture2D texture2D = new Texture2D(2, 2);
							texture2D.LoadImage(data);
							mod.textures.Add(texture2D);
						}
						else if (fileInfo.Extension == ".bytes")
						{
							string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
							byte[] array = File.ReadAllBytes(fileInfo.FullName);
							if (fileNameWithoutExtension.EndsWith("_anim"))
							{
								mod.anim = array;
							}
							else if (fileNameWithoutExtension.EndsWith("_build"))
							{
								mod.build = array;
							}
							else
							{
								DebugUtil.LogWarningArgs(new object[]
								{
									string.Format("Unhandled TextAsset ({0})...ignoring", fileInfo.FullName)
								});
							}
						}
						else
						{
							DebugUtil.LogWarningArgs(new object[]
							{
								string.Format("Unhandled asset ({0})...ignoring", fileInfo.FullName)
							});
						}
					}
					string name = directoryInfo.Name + "_kanim";
					if (mod.IsValid() && ModUtil.AddKAnimMod(name, mod))
					{
						num++;
					}
				}
			}
			return true;
		}

		// Token: 0x0600B7DD RID: 47069 RVA: 0x00463000 File Offset: 0x00461200
		public void Load(Content content)
		{
			content &= (this.available_content & ~this.loaded_content);
			if (content > (Content)0)
			{
				global::Debug.Log(string.Format("Loading mod content {2} [{0}:{1}] (provides {3})", new object[]
				{
					this.title,
					this.label.id,
					content.ToString(),
					this.available_content.ToString()
				}));
			}
			if ((content & Content.Strings) != (Content)0 && this.LoadStrings())
			{
				this.loaded_content |= Content.Strings;
			}
			if ((content & Content.Translation) != (Content)0 && this.LoadTranslations())
			{
				this.loaded_content |= Content.Translation;
			}
			if ((content & Content.DLL) != (Content)0)
			{
				this.loaded_mod_data = DLLLoader.LoadDLLs(this, this.staticID, this.ContentPath, this.IsDev);
				if (this.loaded_mod_data != null)
				{
					this.loaded_content |= Content.DLL;
				}
			}
			if ((content & Content.LayerableFiles) != (Content)0)
			{
				global::Debug.Assert(this.content_source != null, "Attempting to Load layerable files with content_source not initialized");
				FileSystem.file_sources.Insert(0, this.content_source.GetFileSystem());
				this.loaded_content |= Content.LayerableFiles;
			}
			if ((content & Content.Animation) != (Content)0 && this.LoadAnimation())
			{
				this.loaded_content |= Content.Animation;
			}
		}

		// Token: 0x0600B7DE RID: 47070 RVA: 0x001165FB File Offset: 0x001147FB
		public void PostLoad(IReadOnlyList<Mod> mods)
		{
			if ((this.loaded_content & Content.DLL) != (Content)0 && this.loaded_mod_data != null)
			{
				DLLLoader.PostLoadDLLs(this.staticID, this.loaded_mod_data, mods);
			}
		}

		// Token: 0x0600B7DF RID: 47071 RVA: 0x00116621 File Offset: 0x00114821
		public void Unload(Content content)
		{
			content &= this.loaded_content;
			if ((content & Content.LayerableFiles) != (Content)0)
			{
				FileSystem.file_sources.Remove(this.content_source.GetFileSystem());
				this.loaded_content &= ~Content.LayerableFiles;
			}
		}

		// Token: 0x0600B7E0 RID: 47072 RVA: 0x0011665A File Offset: 0x0011485A
		private void SetCrashCount(int new_crash_count)
		{
			this.crash_count = MathUtil.Clamp(0, 3, new_crash_count);
		}

		// Token: 0x17000BDC RID: 3036
		// (get) Token: 0x0600B7E1 RID: 47073 RVA: 0x0011666A File Offset: 0x0011486A
		public bool IsDev
		{
			get
			{
				return this.label.distribution_platform == Label.DistributionPlatform.Dev;
			}
		}

		// Token: 0x17000BDD RID: 3037
		// (get) Token: 0x0600B7E2 RID: 47074 RVA: 0x0011667A File Offset: 0x0011487A
		public bool IsLocal
		{
			get
			{
				return this.label.distribution_platform == Label.DistributionPlatform.Dev || this.label.distribution_platform == Label.DistributionPlatform.Local;
			}
		}

		// Token: 0x0600B7E3 RID: 47075 RVA: 0x0011669A File Offset: 0x0011489A
		public void SetCrashed()
		{
			this.SetCrashCount(this.crash_count + 1);
			if (!this.IsDev)
			{
				this.SetEnabledForActiveDlc(false);
			}
		}

		// Token: 0x0600B7E4 RID: 47076 RVA: 0x001166B9 File Offset: 0x001148B9
		public void Uncrash()
		{
			this.SetCrashCount(this.IsDev ? (this.crash_count - 1) : 0);
		}

		// Token: 0x0600B7E5 RID: 47077 RVA: 0x001166D4 File Offset: 0x001148D4
		public bool IsActive()
		{
			return this.loaded_content > (Content)0;
		}

		// Token: 0x0600B7E6 RID: 47078 RVA: 0x001166DF File Offset: 0x001148DF
		public bool AllActive(Content content)
		{
			return (this.loaded_content & content) == content;
		}

		// Token: 0x0600B7E7 RID: 47079 RVA: 0x001166EC File Offset: 0x001148EC
		public bool AllActive()
		{
			return (this.loaded_content & this.available_content) == this.available_content;
		}

		// Token: 0x0600B7E8 RID: 47080 RVA: 0x00116703 File Offset: 0x00114903
		public bool AnyActive(Content content)
		{
			return (this.loaded_content & content) > (Content)0;
		}

		// Token: 0x0600B7E9 RID: 47081 RVA: 0x00116710 File Offset: 0x00114910
		public bool HasContent()
		{
			return this.available_content > (Content)0;
		}

		// Token: 0x0600B7EA RID: 47082 RVA: 0x0011671B File Offset: 0x0011491B
		public bool HasAnyContent(Content content)
		{
			return (this.available_content & content) > (Content)0;
		}

		// Token: 0x0600B7EB RID: 47083 RVA: 0x00116728 File Offset: 0x00114928
		public bool HasOnlyTranslationContent()
		{
			return this.available_content == Content.Translation;
		}

		// Token: 0x0600B7EC RID: 47084 RVA: 0x00463140 File Offset: 0x00461340
		public Texture2D GetPreviewImage()
		{
			string text = null;
			foreach (string text2 in Mod.PREVIEW_FILENAMES)
			{
				if (Directory.Exists(this.ContentPath) && File.Exists(Path.Combine(this.ContentPath, text2)))
				{
					text = text2;
					break;
				}
			}
			if (text == null)
			{
				return null;
			}
			Texture2D result;
			try
			{
				byte[] data = File.ReadAllBytes(Path.Combine(this.ContentPath, text));
				Texture2D texture2D = new Texture2D(2, 2);
				texture2D.LoadImage(data);
				result = texture2D;
			}
			catch
			{
				global::Debug.LogWarning(string.Format("Mod {0} seems to have a preview.png but it didn't load correctly.", this.label));
				result = null;
			}
			return result;
		}

		// Token: 0x0600B7ED RID: 47085 RVA: 0x00116733 File Offset: 0x00114933
		public void ModDevLog(string msg)
		{
			if (this.IsDev)
			{
				global::Debug.Log(msg);
			}
		}

		// Token: 0x0600B7EE RID: 47086 RVA: 0x00116743 File Offset: 0x00114943
		public void ModDevLogWarning(string msg)
		{
			if (this.IsDev)
			{
				global::Debug.LogWarning(msg);
			}
		}

		// Token: 0x0600B7EF RID: 47087 RVA: 0x00116753 File Offset: 0x00114953
		public void ModDevLogError(string msg)
		{
			if (this.IsDev)
			{
				this.DevModCrashTriggered = true;
				global::Debug.LogError(msg);
			}
		}

		// Token: 0x04009649 RID: 38473
		public const int MOD_API_VERSION_NONE = 0;

		// Token: 0x0400964A RID: 38474
		public const int MOD_API_VERSION_HARMONY1 = 1;

		// Token: 0x0400964B RID: 38475
		public const int MOD_API_VERSION_HARMONY2 = 2;

		// Token: 0x0400964C RID: 38476
		public const int MOD_API_VERSION = 2;

		// Token: 0x0400964D RID: 38477
		[JsonProperty]
		public Label label;

		// Token: 0x0400964E RID: 38478
		[JsonProperty]
		public Mod.Status status;

		// Token: 0x0400964F RID: 38479
		[JsonProperty]
		public bool enabled;

		// Token: 0x04009650 RID: 38480
		[JsonProperty]
		public List<string> enabledForDlc;

		// Token: 0x04009652 RID: 38482
		[JsonProperty]
		public int crash_count;

		// Token: 0x04009653 RID: 38483
		[JsonProperty]
		public string reinstall_path;

		// Token: 0x04009655 RID: 38485
		public bool foundInStackTrace;

		// Token: 0x04009656 RID: 38486
		public string relative_root = "";

		// Token: 0x04009657 RID: 38487
		public Mod.PackagedModInfo packagedModInfo;

		// Token: 0x0400965C RID: 38492
		public LoadedModData loaded_mod_data;

		// Token: 0x0400965D RID: 38493
		private IFileSource _fileSource;

		// Token: 0x0400965E RID: 38494
		public IFileSource content_source;

		// Token: 0x0400965F RID: 38495
		public bool is_subscribed;

		// Token: 0x04009661 RID: 38497
		private const string VANILLA_ID = "vanilla_id";

		// Token: 0x04009662 RID: 38498
		private const string ALL_ID = "all";

		// Token: 0x04009663 RID: 38499
		private const string ARCHIVED_VERSIONS_FOLDER = "archived_versions";

		// Token: 0x04009664 RID: 38500
		private const string MOD_INFO_FILENAME = "mod_info.yaml";

		// Token: 0x04009665 RID: 38501
		public ModContentCompatability contentCompatability;

		// Token: 0x04009666 RID: 38502
		public const int MAX_CRASH_COUNT = 3;

		// Token: 0x04009667 RID: 38503
		private static readonly List<string> PREVIEW_FILENAMES = new List<string>
		{
			"preview.png",
			"Preview.png",
			"PREVIEW.PNG"
		};

		// Token: 0x020021E1 RID: 8673
		public enum Status
		{
			// Token: 0x04009669 RID: 38505
			NotInstalled,
			// Token: 0x0400966A RID: 38506
			Installed,
			// Token: 0x0400966B RID: 38507
			UninstallPending,
			// Token: 0x0400966C RID: 38508
			ReinstallPending
		}

		// Token: 0x020021E2 RID: 8674
		public class ArchivedVersion
		{
			// Token: 0x0400966D RID: 38509
			public string relativePath;

			// Token: 0x0400966E RID: 38510
			public Mod.PackagedModInfo info;
		}

		// Token: 0x020021E3 RID: 8675
		public class PackagedModInfo
		{
			// Token: 0x17000BDE RID: 3038
			// (get) Token: 0x0600B7F4 RID: 47092 RVA: 0x001167B6 File Offset: 0x001149B6
			// (set) Token: 0x0600B7F5 RID: 47093 RVA: 0x001167BE File Offset: 0x001149BE
			public string supportedContent { get; set; }

			// Token: 0x17000BDF RID: 3039
			// (get) Token: 0x0600B7F6 RID: 47094 RVA: 0x001167C7 File Offset: 0x001149C7
			// (set) Token: 0x0600B7F7 RID: 47095 RVA: 0x001167CF File Offset: 0x001149CF
			[Obsolete("Use minimumSupportedBuild instead!")]
			public int lastWorkingBuild { get; set; }

			// Token: 0x17000BE0 RID: 3040
			// (get) Token: 0x0600B7F8 RID: 47096 RVA: 0x001167D8 File Offset: 0x001149D8
			// (set) Token: 0x0600B7F9 RID: 47097 RVA: 0x001167E0 File Offset: 0x001149E0
			public int minimumSupportedBuild { get; set; }

			// Token: 0x17000BE1 RID: 3041
			// (get) Token: 0x0600B7FA RID: 47098 RVA: 0x001167E9 File Offset: 0x001149E9
			// (set) Token: 0x0600B7FB RID: 47099 RVA: 0x001167F1 File Offset: 0x001149F1
			public int APIVersion { get; set; }

			// Token: 0x17000BE2 RID: 3042
			// (get) Token: 0x0600B7FC RID: 47100 RVA: 0x001167FA File Offset: 0x001149FA
			// (set) Token: 0x0600B7FD RID: 47101 RVA: 0x00116802 File Offset: 0x00114A02
			public string version { get; set; }
		}
	}
}
