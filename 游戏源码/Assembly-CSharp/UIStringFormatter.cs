using System.Collections.Generic;

public class UIStringFormatter
{
	private struct Entry
	{
		public string format;

		public string key;

		public string value;

		public string result;
	}

	private List<Entry> entries = new List<Entry>();
}
