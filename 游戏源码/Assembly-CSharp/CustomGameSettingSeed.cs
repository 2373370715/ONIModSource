using System;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001C89 RID: 7305
public class CustomGameSettingSeed : CustomGameSettingWidget
{
	// Token: 0x06009848 RID: 38984 RVA: 0x003AF6C0 File Offset: 0x003AD8C0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.Input.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.Input.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		this.RandomizeButton.onClick += this.GetNewRandomSeed;
	}

	// Token: 0x06009849 RID: 38985 RVA: 0x0010309E File Offset: 0x0010129E
	public void Initialize(SeedSettingConfig config)
	{
		this.config = config;
		this.Label.text = config.label;
		this.ToolTip.toolTip = config.tooltip;
		this.GetNewRandomSeed();
	}

	// Token: 0x0600984A RID: 38986 RVA: 0x003AF724 File Offset: 0x003AD924
	public override void Refresh()
	{
		base.Refresh();
		string currentQualitySettingLevelId = CustomGameSettings.Instance.GetCurrentQualitySettingLevelId(this.config);
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		this.allowChange = (currentClusterLayout.fixedCoordinate == -1);
		this.Input.interactable = this.allowChange;
		this.RandomizeButton.isInteractable = this.allowChange;
		if (this.allowChange)
		{
			this.InputToolTip.enabled = false;
			this.RandomizeButtonToolTip.enabled = false;
		}
		else
		{
			this.InputToolTip.enabled = true;
			this.RandomizeButtonToolTip.enabled = true;
			this.InputToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
			this.RandomizeButtonToolTip.SetSimpleTooltip(UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLDGEN_SEED.FIXEDSEED);
		}
		this.Input.text = currentQualitySettingLevelId;
	}

	// Token: 0x0600984B RID: 38987 RVA: 0x001030CF File Offset: 0x001012CF
	private char ValidateInput(string text, int charIndex, char addedChar)
	{
		if ('0' > addedChar || addedChar > '9')
		{
			return '\0';
		}
		return addedChar;
	}

	// Token: 0x0600984C RID: 38988 RVA: 0x003AF7F4 File Offset: 0x003AD9F4
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
		this.SetSeed(seed);
	}

	// Token: 0x0600984D RID: 38989 RVA: 0x001030DE File Offset: 0x001012DE
	public void SetSeed(int seed)
	{
		seed = Mathf.Min(seed, int.MaxValue);
		CustomGameSettings.Instance.SetQualitySetting(this.config, seed.ToString());
		this.Refresh();
	}

	// Token: 0x0600984E RID: 38990 RVA: 0x003AF828 File Offset: 0x003ADA28
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
				this.Input.text = text.Substring(0, text.Length - 1);
			}
			else
			{
				this.Input.text = "";
			}
		}
		if (num > 2147483647)
		{
			this.Input.text = text.Substring(0, text.Length - 1);
		}
	}

	// Token: 0x0600984F RID: 38991 RVA: 0x003AF8AC File Offset: 0x003ADAAC
	private void GetNewRandomSeed()
	{
		int seed = UnityEngine.Random.Range(0, int.MaxValue);
		this.SetSeed(seed);
	}

	// Token: 0x04007694 RID: 30356
	[SerializeField]
	private LocText Label;

	// Token: 0x04007695 RID: 30357
	[SerializeField]
	private ToolTip ToolTip;

	// Token: 0x04007696 RID: 30358
	[SerializeField]
	private KInputTextField Input;

	// Token: 0x04007697 RID: 30359
	[SerializeField]
	private KButton RandomizeButton;

	// Token: 0x04007698 RID: 30360
	[SerializeField]
	private ToolTip InputToolTip;

	// Token: 0x04007699 RID: 30361
	[SerializeField]
	private ToolTip RandomizeButtonToolTip;

	// Token: 0x0400769A RID: 30362
	private const int MAX_VALID_SEED = 2147483647;

	// Token: 0x0400769B RID: 30363
	private SeedSettingConfig config;

	// Token: 0x0400769C RID: 30364
	private bool allowChange = true;
}
