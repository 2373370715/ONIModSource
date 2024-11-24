using System.Linq;

public static class StringSearchableListUtil
{
	public static bool DoAnyTagsMatchFilter(string[] lowercaseTags, in string filter)
	{
		string filter2 = filter.Trim().ToLowerInvariant();
		string[] source = filter2.Split(' ');
		foreach (string tag in lowercaseTags)
		{
			if (DoesTagMatchFilter(tag, in filter2))
			{
				return true;
			}
			if (source.Select((string f) => DoesTagMatchFilter(tag, in f)).All((bool result) => result))
			{
				return true;
			}
		}
		return false;
	}

	public static bool DoesTagMatchFilter(string lowercaseTag, in string filter)
	{
		if (string.IsNullOrWhiteSpace(filter))
		{
			return true;
		}
		if (lowercaseTag.Contains(filter))
		{
			return true;
		}
		return false;
	}

	public static bool ShouldUseFilter(string filter)
	{
		return !string.IsNullOrWhiteSpace(filter);
	}
}
