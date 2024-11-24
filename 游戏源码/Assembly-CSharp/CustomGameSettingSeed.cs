using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;

public class CustomGameSettingSeed : CustomGameSettingWidget
{
	[SerializeField]
	private LocText Label;

	[SerializeField]
	private ToolTip ToolTip;

	[SerializeField]
	private KInputTextField Input;

	[SerializeField]
	private KButton RandomizeButton;

	[SerializeField]
	private ToolTip InputToolTip;

	[SerializeField]
	private ToolTip RandomizeButtonToolTip;

	private const int MAX_VALID_SEED = int.MaxValue;

	private SeedSettingConfig config;

	private bool allowChange = true;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Input.onEndEdit.AddListener(OnEndEdit);
		Input.onValueChanged.AddListener(OnValueChanged);
		RandomizeButton.onClick += GetNewRandomSeed;
	}

	public void Initialize(SeedSettingConfig config)
	{
		this.config = config;
		Label.text = config.label;
		ToolTip.toolTip = config.tooltip;
		GetNewRandomSeed();
	}

	public override void Refresh()
	{
		base.Refresh();
		string currentQualitySettingLevelId = CustomGameSettings.Instance.GetCurrentQualitySettingLevelId(config);
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		allowChange = currentClusterLayout.fixedCoordinate == -1;
		Input.interactable = allowChange;
		RandomizeButton.isInteractable = allowChange;
		if (allowChange)
		{
			InputToolTip.enabled = false;
			RandomizeButtonToolTip.enabled = false;
		}
		else
		{
			InputToolTip.enabled = true;
			RandomizeButtonToolTip.enabled = true;
			InputToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
			RandomizeButtonToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
		}
		Input.text = currentQualitySettingLevelId;
	}

	private char ValidateInput(string text, int charIndex, char addedChar)
	{
		if ('0' > addedChar || addedChar > '9')
		{
			return '\0';
		}
		return addedChar;
	}

	private void OnEndEdit(string text)
	{
		int seed;
		try
		{
			seed = Convert.ToInt32(text);
		}
		catch
		{
			seed = 0;
		}
		SetSeed(seed);
	}

	public void SetSeed(int seed)
	{
		seed = Mathf.Min(seed, int.MaxValue);
		CustomGameSettings.Instance.SetQualitySetting(config, seed.ToString());
		Refresh();
	}

	private void OnValueChanged(string text)
	{
		int num = 0;
		try
		{
			num = Convert.ToInt32(text);
		}
		catch
		{
			if (text.Length > 0)
			{
				Input.text = text.Substring(0, text.Length - 1);
			}
			else
			{
				Input.text = "";
			}
		}
		if (num > int.MaxValue)
		{
			Input.text = text.Substring(0, text.Length - 1);
		}
	}

	private void GetNewRandomSeed()
	{
		int seed = UnityEngine.Random.Range(0, int.MaxValue);
		SetSeed(seed);
	}
}
