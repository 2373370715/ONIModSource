using System;
using System.Linq;

// Token: 0x0200061C RID: 1564
public static class StringSearchableListUtil
{
	// Token: 0x06001C6C RID: 7276 RVA: 0x001AD100 File Offset: 0x001AB300
	public static bool DoAnyTagsMatchFilter(string[] lowercaseTags, in string filter)
	{
		string text = filter.Trim().ToLowerInvariant();
		string[] source = text.Split(' ', StringSplitOptions.None);
		for (int i = 0; i < lowercaseTags.Length; i++)
		{
			string tag = lowercaseTags[i];
			if (StringSearchableListUtil.DoesTagMatchFilter(tag, text))
			{
				return true;
			}
			if ((from f in source
			select StringSearchableListUtil.DoesTagMatchFilter(tag, f)).All((bool result) => result))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001C6D RID: 7277 RVA: 0x000B2AD0 File Offset: 0x000B0CD0
	public static bool DoesTagMatchFilter(string lowercaseTag, in string filter)
	{
		return string.IsNullOrWhiteSpace(filter) || lowercaseTag.Contains(filter);
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x000B2AEA File Offset: 0x000B0CEA
	public static bool ShouldUseFilter(string filter)
	{
		return !string.IsNullOrWhiteSpace(filter);
	}
}
