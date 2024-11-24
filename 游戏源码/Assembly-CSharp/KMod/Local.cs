using System;
using System.IO;
using Klei;
using STRINGS;

namespace KMod
{
	// Token: 0x020021D2 RID: 8658
	public class Local : IDistributionPlatform
	{
		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x0600B781 RID: 46977 RVA: 0x00116303 File Offset: 0x00114503
		// (set) Token: 0x0600B782 RID: 46978 RVA: 0x0011630B File Offset: 0x0011450B
		public string folder { get; private set; }

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x0600B783 RID: 46979 RVA: 0x00116314 File Offset: 0x00114514
		// (set) Token: 0x0600B784 RID: 46980 RVA: 0x0011631C File Offset: 0x0011451C
		public Label.DistributionPlatform distribution_platform { get; private set; }

		// Token: 0x0600B785 RID: 46981 RVA: 0x00116325 File Offset: 0x00114525
		public string GetDirectory()
		{
			return FileSystem.Normalize(Path.Combine(Manager.GetDirectory(), this.folder));
		}

		// Token: 0x0600B786 RID: 46982 RVA: 0x00461560 File Offset: 0x0045F760
		private void Subscribe(string directoryName, long timestamp, IFileSource file_source, bool isDevMod)
		{
			Label label = new Label
			{
				id = directoryName,
				distribution_platform = this.distribution_platform,
				version = (long)directoryName.GetHashCode(),
				title = directoryName
			};
			KModHeader header = KModUtil.GetHeader(file_source, label.defaultStaticID, directoryName, directoryName, isDevMod);
			label.title = header.title;
			Mod mod = new Mod(label, header.staticID, header.description, file_source, UI.FRONTEND.MODS.TOOLTIPS.MANAGE_LOCAL_MOD, delegate()
			{
				App.OpenWebURL("file://" + file_source.GetRoot());
			});
			if (file_source.GetType() == typeof(Directory))
			{
				mod.status = Mod.Status.Installed;
			}
			Global.Instance.modManager.Subscribe(mod, this);
		}

		// Token: 0x0600B787 RID: 46983 RVA: 0x00461634 File Offset: 0x0045F834
		public Local(string folder, Label.DistributionPlatform distribution_platform, bool isDevFolder)
		{
			this.folder = folder;
			this.distribution_platform = distribution_platform;
			DirectoryInfo directoryInfo = new DirectoryInfo(this.GetDirectory());
			if (!directoryInfo.Exists)
			{
				return;
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				string name = directoryInfo2.Name;
				this.Subscribe(name, directoryInfo2.LastWriteTime.ToFileTime(), new Directory(directoryInfo2.FullName), isDevFolder);
			}
		}
	}
}
