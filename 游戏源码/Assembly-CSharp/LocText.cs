using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using STRINGS;
using TMPro;
using UnityEngine;

public class LocText : TextMeshProUGUI
{
	public string key;

	public TextStyleSetting textStyleSetting;

	public bool allowOverride;

	public bool staticLayout;

	private TextLinkHandler textLinkHandler;

	private string originalString = string.Empty;

	[SerializeField]
	private bool allowLinksInternal;

	private static readonly Dictionary<string, Action> ActionLookup = Enum.GetNames(typeof(Action)).ToDictionary((string x) => x, (string x) => (Action)Enum.Parse(typeof(Action), x), StringComparer.OrdinalIgnoreCase);

	private static readonly Dictionary<string, Pair<LocString, LocString>> ClickLookup = new Dictionary<string, Pair<LocString, LocString>>
	{
		{
			UI.ClickType.Click.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESS, UI.CONTROLS.CLICK)
		},
		{
			UI.ClickType.Clickable.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLE, UI.CONTROLS.CLICKABLE)
		},
		{
			UI.ClickType.Clicked.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSED, UI.CONTROLS.CLICKED)
		},
		{
			UI.ClickType.Clicking.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSING, UI.CONTROLS.CLICKING)
		},
		{
			UI.ClickType.Clicks.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSES, UI.CONTROLS.CLICKS)
		},
		{
			UI.ClickType.click.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSLOWER, UI.CONTROLS.CLICKLOWER)
		},
		{
			UI.ClickType.clickable.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLELOWER, UI.CONTROLS.CLICKABLELOWER)
		},
		{
			UI.ClickType.clicked.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDLOWER, UI.CONTROLS.CLICKEDLOWER)
		},
		{
			UI.ClickType.clicking.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGLOWER, UI.CONTROLS.CLICKINGLOWER)
		},
		{
			UI.ClickType.clicks.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSESLOWER, UI.CONTROLS.CLICKSLOWER)
		},
		{
			UI.ClickType.CLICK.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSUPPER, UI.CONTROLS.CLICKUPPER)
		},
		{
			UI.ClickType.CLICKABLE.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLEUPPER, UI.CONTROLS.CLICKABLEUPPER)
		},
		{
			UI.ClickType.CLICKED.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDUPPER, UI.CONTROLS.CLICKEDUPPER)
		},
		{
			UI.ClickType.CLICKING.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGUPPER, UI.CONTROLS.CLICKINGUPPER)
		},
		{
			UI.ClickType.CLICKS.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSESUPPER, UI.CONTROLS.CLICKSUPPER)
		}
	};

	private const string linkPrefix_open = "<link=\"";

	private const string linkSuffix = "</link>";

	private const string linkColorPrefix = "<b><style=\"KLink\">";

	private const string linkColorSuffix = "</style></b>";

	private static readonly string combinedPrefix = "<b><style=\"KLink\"><link=\"";

	private static readonly string combinedSuffix = "</style></b></link>";

	public bool AllowLinks
	{
		get
		{
			return allowLinksInternal;
		}
		set
		{
			allowLinksInternal = value;
			RefreshLinkHandler();
			raycastTarget = raycastTarget || allowLinksInternal;
		}
	}

	public override string text
	{
		get
		{
			return base.text;
		}
		set
		{
			base.text = FilterInput(value);
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
	}

	[ContextMenu("Apply Settings")]
	public void ApplySettings()
	{
		if (this.key != "" && Application.isPlaying)
		{
			StringKey key = new StringKey(this.key);
			text = Strings.Get(key);
		}
		if (textStyleSetting != null)
		{
			SetTextStyleSetting.ApplyStyle(this, textStyleSetting);
		}
	}

	private new void Awake()
	{
		base.Awake();
		if (Application.isPlaying)
		{
			if (key != "")
			{
				StringEntry stringEntry = Strings.Get(new StringKey(key));
				text = stringEntry.String;
			}
			text = Localization.Fixup(text);
			base.isRightToLeftText = Localization.IsRightToLeft;
			KInputManager.InputChange.AddListener(RefreshText);
			SetTextStyleSetting setTextStyleSetting = base.gameObject.GetComponent<SetTextStyleSetting>();
			if (setTextStyleSetting == null)
			{
				setTextStyleSetting = base.gameObject.AddComponent<SetTextStyleSetting>();
			}
			if (!allowOverride)
			{
				setTextStyleSetting.SetStyle(textStyleSetting);
			}
			textLinkHandler = GetComponent<TextLinkHandler>();
		}
	}

	private new void Start()
	{
		base.Start();
		RefreshLinkHandler();
	}

	private new void OnDestroy()
	{
		KInputManager.InputChange.RemoveListener(RefreshText);
		base.OnDestroy();
	}

	public override void SetLayoutDirty()
	{
		if (!staticLayout)
		{
			base.SetLayoutDirty();
		}
	}

	public void SetLinkOverrideAction(Func<string, bool> action)
	{
		RefreshLinkHandler();
		if (textLinkHandler != null)
		{
			textLinkHandler.overrideLinkAction = action;
		}
	}

	public override void SetText(string text)
	{
		text = FilterInput(text);
		base.SetText(text);
	}

	private string FilterInput(string input)
	{
		if (input != null)
		{
			string obj = ParseText(input);
			if (obj != input)
			{
				originalString = input;
			}
			else
			{
				originalString = string.Empty;
			}
			input = obj;
		}
		if (AllowLinks)
		{
			return ModifyLinkStrings(input);
		}
		return input;
	}

	public static string ParseText(string input)
	{
		string pattern = "\\{Hotkey/(\\w+)\\}";
		string input2 = Regex.Replace(input, pattern, delegate(Match m)
		{
			string value3 = m.Groups[1].Value;
			Action value4;
			return ActionLookup.TryGetValue(value3, out value4) ? GameUtil.GetHotkeyString(value4) : m.Value;
		});
		pattern = "\\(ClickType/(\\w+)\\)";
		return Regex.Replace(input2, pattern, delegate(Match m)
		{
			string value = m.Groups[1].Value;
			Pair<LocString, LocString> value2;
			return ClickLookup.TryGetValue(value, out value2) ? (KInputManager.currentControllerIsGamepad ? value2.first.ToString() : value2.second.ToString()) : m.Value;
		});
	}

	private void RefreshText()
	{
		if (originalString != string.Empty)
		{
			SetText(originalString);
		}
	}

	protected override void GenerateTextMesh()
	{
		base.GenerateTextMesh();
	}

	internal void SwapFont(TMP_FontAsset font, bool isRightToLeft)
	{
		base.font = font;
		if (key != "")
		{
			StringEntry stringEntry = Strings.Get(new StringKey(key));
			text = stringEntry.String;
		}
		text = Localization.Fixup(text);
		base.isRightToLeftText = isRightToLeft;
	}

	private static string ModifyLinkStrings(string input)
	{
		if (input == null || input.IndexOf("<b><style=\"KLink\">") != -1)
		{
			return input;
		}
		StringBuilder stringBuilder = new StringBuilder(input);
		stringBuilder.Replace("<link=\"", combinedPrefix);
		stringBuilder.Replace("</link>", combinedSuffix);
		return stringBuilder.ToString();
	}

	private void RefreshLinkHandler()
	{
		if (textLinkHandler == null && allowLinksInternal)
		{
			textLinkHandler = GetComponent<TextLinkHandler>();
			if (textLinkHandler == null)
			{
				textLinkHandler = base.gameObject.AddComponent<TextLinkHandler>();
			}
		}
		else if (!allowLinksInternal && textLinkHandler != null)
		{
			UnityEngine.Object.Destroy(textLinkHandler);
			textLinkHandler = null;
		}
		if (textLinkHandler != null)
		{
			textLinkHandler.CheckMouseOver();
		}
	}
}
