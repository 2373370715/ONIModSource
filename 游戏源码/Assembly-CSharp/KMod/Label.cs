using System;
using System.Diagnostics;
using System.IO;
using Klei;
using Newtonsoft.Json;

namespace KMod
{
	// Token: 0x020021DC RID: 8668
	[JsonObject(MemberSerialization.Fields)]
	[DebuggerDisplay("{title}")]
	public struct Label
	{
		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x0600B7AE RID: 47022 RVA: 0x0011643C File Offset: 0x0011463C
		[JsonIgnore]
		private string distribution_platform_name
		{
			get
			{
				return this.distribution_platform.ToString();
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x0600B7AF RID: 47023 RVA: 0x0011644F File Offset: 0x0011464F
		[JsonIgnore]
		public string install_path
		{
			get
			{
				return FileSystem.Normalize(Path.Combine(Manager.GetDirectory(), this.distribution_platform_name, this.id));
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x0600B7B0 RID: 47024 RVA: 0x0011646C File Offset: 0x0011466C
		[JsonIgnore]
		public string defaultStaticID
		{
			get
			{
				return this.id + "." + this.distribution_platform.ToString();
			}
		}

		// Token: 0x0600B7B1 RID: 47025 RVA: 0x0011648F File Offset: 0x0011468F
		public override string ToString()
		{
			return this.title;
		}

		// Token: 0x0600B7B2 RID: 47026 RVA: 0x00116497 File Offset: 0x00114697
		public bool Match(Label rhs)
		{
			return this.id == rhs.id && this.distribution_platform == rhs.distribution_platform;
		}

		// Token: 0x04009634 RID: 38452
		public Label.DistributionPlatform distribution_platform;

		// Token: 0x04009635 RID: 38453
		public string id;

		// Token: 0x04009636 RID: 38454
		public string title;

		// Token: 0x04009637 RID: 38455
		public long version;

		// Token: 0x020021DD RID: 8669
		public enum DistributionPlatform
		{
			// Token: 0x04009639 RID: 38457
			Local,
			// Token: 0x0400963A RID: 38458
			Steam,
			// Token: 0x0400963B RID: 38459
			Epic,
			// Token: 0x0400963C RID: 38460
			Rail,
			// Token: 0x0400963D RID: 38461
			Dev
		}
	}
}
