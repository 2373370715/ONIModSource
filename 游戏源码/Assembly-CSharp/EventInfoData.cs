using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class EventInfoData
{
	public class OptionIcon
	{
		public enum ContainerType
		{
			Neutral,
			Positive,
			Negative,
			Information
		}

		public ContainerType containerType;

		public Sprite sprite;

		public string tooltip;

		public float scale;

		public OptionIcon(Sprite sprite, ContainerType containerType, string tooltip, float scale = 1f)
		{
			this.sprite = sprite;
			this.containerType = containerType;
			this.tooltip = tooltip;
			this.scale = scale;
		}
	}

	public class Option
	{
		public string mainText;

		public string description;

		public string tooltip;

		public System.Action callback;

		public List<OptionIcon> informationIcons = new List<OptionIcon>();

		public List<OptionIcon> consequenceIcons = new List<OptionIcon>();

		public bool allowed = true;

		public void AddInformationIcon(string tooltip, float scale = 1f)
		{
			informationIcons.Add(new OptionIcon(null, OptionIcon.ContainerType.Information, tooltip, scale));
		}

		public void AddPositiveIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			consequenceIcons.Add(new OptionIcon(sprite, OptionIcon.ContainerType.Positive, tooltip, scale));
		}

		public void AddNeutralIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			consequenceIcons.Add(new OptionIcon(sprite, OptionIcon.ContainerType.Neutral, tooltip, scale));
		}

		public void AddNegativeIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			consequenceIcons.Add(new OptionIcon(sprite, OptionIcon.ContainerType.Negative, tooltip, scale));
		}
	}

	public string title;

	public string description;

	public string location;

	public string whenDescription;

	public Transform clickFocus;

	public GameObject[] minions;

	public GameObject artifact;

	public HashedString animFileName;

	public HashedString mainAnim = "event";

	public Dictionary<string, string> textParameters = new Dictionary<string, string>();

	public List<Option> options = new List<Option>();

	public System.Action showCallback;

	private bool dirty;

	public EventInfoData(string title, string description, HashedString animFileName)
	{
		this.title = title;
		this.description = description;
		this.animFileName = animFileName;
	}

	public List<Option> GetOptions()
	{
		FinalizeText();
		return options;
	}

	public Option AddOption(string mainText, string description = null)
	{
		Option option = new Option
		{
			mainText = mainText,
			description = description
		};
		options.Add(option);
		dirty = true;
		return option;
	}

	public Option SimpleOption(string mainText, System.Action callback)
	{
		Option option = new Option
		{
			mainText = mainText,
			callback = callback
		};
		options.Add(option);
		dirty = true;
		return option;
	}

	public Option AddDefaultOption(System.Action callback = null)
	{
		return SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_NAME, callback);
	}

	public Option AddDefaultConsiderLaterOption(System.Action callback = null)
	{
		return SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_CONSIDER_NAME, callback);
	}

	public void SetTextParameter(string key, string value)
	{
		textParameters[key] = value;
		dirty = true;
	}

	public void FinalizeText()
	{
		if (!dirty)
		{
			return;
		}
		dirty = false;
		foreach (KeyValuePair<string, string> textParameter in textParameters)
		{
			string oldValue = "{" + textParameter.Key + "}";
			if (title != null)
			{
				title = title.Replace(oldValue, textParameter.Value);
			}
			if (description != null)
			{
				description = description.Replace(oldValue, textParameter.Value);
			}
			if (location != null)
			{
				location = location.Replace(oldValue, textParameter.Value);
			}
			if (whenDescription != null)
			{
				whenDescription = whenDescription.Replace(oldValue, textParameter.Value);
			}
			foreach (Option option in options)
			{
				if (option.mainText != null)
				{
					option.mainText = option.mainText.Replace(oldValue, textParameter.Value);
				}
				if (option.description != null)
				{
					option.description = option.description.Replace(oldValue, textParameter.Value);
				}
				if (option.tooltip != null)
				{
					option.tooltip = option.tooltip.Replace(oldValue, textParameter.Value);
				}
				foreach (OptionIcon informationIcon in option.informationIcons)
				{
					if (informationIcon.tooltip != null)
					{
						informationIcon.tooltip = informationIcon.tooltip.Replace(oldValue, textParameter.Value);
					}
				}
				foreach (OptionIcon consequenceIcon in option.consequenceIcons)
				{
					if (consequenceIcon.tooltip != null)
					{
						consequenceIcon.tooltip = consequenceIcon.tooltip.Replace(oldValue, textParameter.Value);
					}
				}
			}
		}
	}
}
