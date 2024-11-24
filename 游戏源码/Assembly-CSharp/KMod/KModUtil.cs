using System;
using System.IO;
using Klei;

namespace KMod
{
	// Token: 0x020021D0 RID: 8656
	public class KModUtil
	{
		// Token: 0x0600B77D RID: 46973 RVA: 0x004614A8 File Offset: 0x0045F6A8
		public static KModHeader GetHeader(IFileSource file_source, string defaultStaticID, string defaultTitle, string defaultDescription, bool devMod)
		{
			string text = "mod.yaml";
			string text2 = file_source.Read(text);
			YamlIO.ErrorHandler handle_error = delegate(YamlIO.Error e, bool force_warning)
			{
				YamlIO.LogError(e, !devMod);
			};
			KModHeader kmodHeader = (!string.IsNullOrEmpty(text2)) ? YamlIO.Parse<KModHeader>(text2, new FileHandle
			{
				full_path = Path.Combine(file_source.GetRoot(), text)
			}, handle_error, null) : null;
			if (kmodHeader == null)
			{
				kmodHeader = new KModHeader
				{
					title = defaultTitle,
					description = defaultDescription,
					staticID = defaultStaticID
				};
			}
			if (string.IsNullOrEmpty(kmodHeader.staticID))
			{
				kmodHeader.staticID = defaultStaticID;
			}
			if (kmodHeader.title == null)
			{
				kmodHeader.title = defaultTitle;
			}
			if (kmodHeader.description == null)
			{
				kmodHeader.description = defaultDescription;
			}
			return kmodHeader;
		}
	}
}
