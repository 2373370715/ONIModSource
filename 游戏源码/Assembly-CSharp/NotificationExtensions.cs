using System;
using System.Collections.Generic;

// Token: 0x02000AAF RID: 2735
public static class NotificationExtensions
{
	// Token: 0x060032FA RID: 13050 RVA: 0x00204B5C File Offset: 0x00202D5C
	public static string ReduceMessages(this List<Notification> notifications, bool countNames = true)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (Notification notification in notifications)
		{
			int num = 0;
			if (!dictionary.TryGetValue(notification.NotifierName, out num))
			{
				dictionary[notification.NotifierName] = 0;
			}
			dictionary[notification.NotifierName] = num + 1;
		}
		string text = "";
		foreach (KeyValuePair<string, int> keyValuePair in dictionary)
		{
			if (countNames)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					keyValuePair.Key,
					"(",
					keyValuePair.Value.ToString(),
					")"
				});
			}
			else
			{
				text = text + "\n" + keyValuePair.Key;
			}
		}
		return text;
	}
}
