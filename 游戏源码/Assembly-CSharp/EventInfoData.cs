using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200078B RID: 1931
public class EventInfoData
{
	// Token: 0x060022BA RID: 8890 RVA: 0x001C3F00 File Offset: 0x001C2100
	public EventInfoData(string title, string description, HashedString animFileName)
	{
		this.title = title;
		this.description = description;
		this.animFileName = animFileName;
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000B692A File Offset: 0x000B4B2A
	public List<EventInfoData.Option> GetOptions()
	{
		this.FinalizeText();
		return this.options;
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x001C3F50 File Offset: 0x001C2150
	public EventInfoData.Option AddOption(string mainText, string description = null)
	{
		EventInfoData.Option option = new EventInfoData.Option
		{
			mainText = mainText,
			description = description
		};
		this.options.Add(option);
		this.dirty = true;
		return option;
	}

	// Token: 0x060022BD RID: 8893 RVA: 0x001C3F88 File Offset: 0x001C2188
	public EventInfoData.Option SimpleOption(string mainText, System.Action callback)
	{
		EventInfoData.Option option = new EventInfoData.Option
		{
			mainText = mainText,
			callback = callback
		};
		this.options.Add(option);
		this.dirty = true;
		return option;
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x000B6938 File Offset: 0x000B4B38
	public EventInfoData.Option AddDefaultOption(System.Action callback = null)
	{
		return this.SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_NAME, callback);
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x000B694B File Offset: 0x000B4B4B
	public EventInfoData.Option AddDefaultConsiderLaterOption(System.Action callback = null)
	{
		return this.SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_CONSIDER_NAME, callback);
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x000B695E File Offset: 0x000B4B5E
	public void SetTextParameter(string key, string value)
	{
		this.textParameters[key] = value;
		this.dirty = true;
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x001C3FC0 File Offset: 0x001C21C0
	public void FinalizeText()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		foreach (KeyValuePair<string, string> keyValuePair in this.textParameters)
		{
			string oldValue = "{" + keyValuePair.Key + "}";
			if (this.title != null)
			{
				this.title = this.title.Replace(oldValue, keyValuePair.Value);
			}
			if (this.description != null)
			{
				this.description = this.description.Replace(oldValue, keyValuePair.Value);
			}
			if (this.location != null)
			{
				this.location = this.location.Replace(oldValue, keyValuePair.Value);
			}
			if (this.whenDescription != null)
			{
				this.whenDescription = this.whenDescription.Replace(oldValue, keyValuePair.Value);
			}
			foreach (EventInfoData.Option option in this.options)
			{
				if (option.mainText != null)
				{
					option.mainText = option.mainText.Replace(oldValue, keyValuePair.Value);
				}
				if (option.description != null)
				{
					option.description = option.description.Replace(oldValue, keyValuePair.Value);
				}
				if (option.tooltip != null)
				{
					option.tooltip = option.tooltip.Replace(oldValue, keyValuePair.Value);
				}
				foreach (EventInfoData.OptionIcon optionIcon in option.informationIcons)
				{
					if (optionIcon.tooltip != null)
					{
						optionIcon.tooltip = optionIcon.tooltip.Replace(oldValue, keyValuePair.Value);
					}
				}
				foreach (EventInfoData.OptionIcon optionIcon2 in option.consequenceIcons)
				{
					if (optionIcon2.tooltip != null)
					{
						optionIcon2.tooltip = optionIcon2.tooltip.Replace(oldValue, keyValuePair.Value);
					}
				}
			}
		}
	}

	// Token: 0x040016E1 RID: 5857
	public string title;

	// Token: 0x040016E2 RID: 5858
	public string description;

	// Token: 0x040016E3 RID: 5859
	public string location;

	// Token: 0x040016E4 RID: 5860
	public string whenDescription;

	// Token: 0x040016E5 RID: 5861
	public Transform clickFocus;

	// Token: 0x040016E6 RID: 5862
	public GameObject[] minions;

	// Token: 0x040016E7 RID: 5863
	public GameObject artifact;

	// Token: 0x040016E8 RID: 5864
	public HashedString animFileName;

	// Token: 0x040016E9 RID: 5865
	public HashedString mainAnim = "event";

	// Token: 0x040016EA RID: 5866
	public Dictionary<string, string> textParameters = new Dictionary<string, string>();

	// Token: 0x040016EB RID: 5867
	public List<EventInfoData.Option> options = new List<EventInfoData.Option>();

	// Token: 0x040016EC RID: 5868
	public System.Action showCallback;

	// Token: 0x040016ED RID: 5869
	private bool dirty;

	// Token: 0x0200078C RID: 1932
	public class OptionIcon
	{
		// Token: 0x060022C2 RID: 8898 RVA: 0x000B6974 File Offset: 0x000B4B74
		public OptionIcon(Sprite sprite, EventInfoData.OptionIcon.ContainerType containerType, string tooltip, float scale = 1f)
		{
			this.sprite = sprite;
			this.containerType = containerType;
			this.tooltip = tooltip;
			this.scale = scale;
		}

		// Token: 0x040016EE RID: 5870
		public EventInfoData.OptionIcon.ContainerType containerType;

		// Token: 0x040016EF RID: 5871
		public Sprite sprite;

		// Token: 0x040016F0 RID: 5872
		public string tooltip;

		// Token: 0x040016F1 RID: 5873
		public float scale;

		// Token: 0x0200078D RID: 1933
		public enum ContainerType
		{
			// Token: 0x040016F3 RID: 5875
			Neutral,
			// Token: 0x040016F4 RID: 5876
			Positive,
			// Token: 0x040016F5 RID: 5877
			Negative,
			// Token: 0x040016F6 RID: 5878
			Information
		}
	}

	// Token: 0x0200078E RID: 1934
	public class Option
	{
		// Token: 0x060022C3 RID: 8899 RVA: 0x000B6999 File Offset: 0x000B4B99
		public void AddInformationIcon(string tooltip, float scale = 1f)
		{
			this.informationIcons.Add(new EventInfoData.OptionIcon(null, EventInfoData.OptionIcon.ContainerType.Information, tooltip, scale));
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x000B69AF File Offset: 0x000B4BAF
		public void AddPositiveIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Positive, tooltip, scale));
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x000B69C5 File Offset: 0x000B4BC5
		public void AddNeutralIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Neutral, tooltip, scale));
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x000B69DB File Offset: 0x000B4BDB
		public void AddNegativeIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Negative, tooltip, scale));
		}

		// Token: 0x040016F7 RID: 5879
		public string mainText;

		// Token: 0x040016F8 RID: 5880
		public string description;

		// Token: 0x040016F9 RID: 5881
		public string tooltip;

		// Token: 0x040016FA RID: 5882
		public System.Action callback;

		// Token: 0x040016FB RID: 5883
		public List<EventInfoData.OptionIcon> informationIcons = new List<EventInfoData.OptionIcon>();

		// Token: 0x040016FC RID: 5884
		public List<EventInfoData.OptionIcon> consequenceIcons = new List<EventInfoData.OptionIcon>();

		// Token: 0x040016FD RID: 5885
		public bool allowed = true;
	}
}
