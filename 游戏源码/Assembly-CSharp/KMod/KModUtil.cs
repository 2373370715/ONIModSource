using System.IO;
using Klei;

namespace KMod;

public class KModUtil
{
	public static KModHeader GetHeader(IFileSource file_source, string defaultStaticID, string defaultTitle, string defaultDescription, bool devMod)
	{
		string text = "mod.yaml";
		string text2 = file_source.Read(text);
		YamlIO.ErrorHandler handle_error = delegate(YamlIO.Error e, bool force_warning)
		{
			YamlIO.LogError(e, !devMod);
		};
		KModHeader kModHeader = ((!string.IsNullOrEmpty(text2)) ? YamlIO.Parse<KModHeader>(text2, new FileHandle
		{
			full_path = Path.Combine(file_source.GetRoot(), text)
		}, handle_error) : null);
		if (kModHeader == null)
		{
			kModHeader = new KModHeader
			{
				title = defaultTitle,
				description = defaultDescription,
				staticID = defaultStaticID
			};
		}
		if (string.IsNullOrEmpty(kModHeader.staticID))
		{
			kModHeader.staticID = defaultStaticID;
		}
		if (kModHeader.title == null)
		{
			kModHeader.title = defaultTitle;
		}
		if (kModHeader.description == null)
		{
			kModHeader.description = defaultDescription;
		}
		return kModHeader;
	}
}
