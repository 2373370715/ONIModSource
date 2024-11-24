using System;
using System.Collections.Generic;

// Token: 0x02001EEC RID: 7916
public class SandboxSettings
{
	// Token: 0x0600A6B7 RID: 42679 RVA: 0x0010C0A6 File Offset: 0x0010A2A6
	public void AddIntSetting(string prefsKey, Action<int> setAction, int defaultValue)
	{
		this.intSettings.Add(new SandboxSettings.Setting<int>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x0600A6B8 RID: 42680 RVA: 0x0010C0BB File Offset: 0x0010A2BB
	public int GetIntSetting(string prefsKey)
	{
		return KPlayerPrefs.GetInt(prefsKey);
	}

	// Token: 0x0600A6B9 RID: 42681 RVA: 0x003F54F8 File Offset: 0x003F36F8
	public void SetIntSetting(string prefsKey, int value)
	{
		SandboxSettings.Setting<int> setting = this.intSettings.Find((SandboxSettings.Setting<int> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No intSetting named: ",
				prefsKey,
				" could be found amongst ",
				this.intSettings.Count.ToString(),
				" int settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x0600A6BA RID: 42682 RVA: 0x0010C0C3 File Offset: 0x0010A2C3
	public void RestoreIntSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetIntSetting(prefsKey, this.GetIntSetting(prefsKey));
			return;
		}
		this.ForceDefaultIntSetting(prefsKey);
	}

	// Token: 0x0600A6BB RID: 42683 RVA: 0x003F557C File Offset: 0x003F377C
	public void ForceDefaultIntSetting(string prefsKey)
	{
		this.SetIntSetting(prefsKey, this.intSettings.Find((SandboxSettings.Setting<int> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x0600A6BC RID: 42684 RVA: 0x0010C0E3 File Offset: 0x0010A2E3
	public void AddFloatSetting(string prefsKey, Action<float> setAction, float defaultValue)
	{
		this.floatSettings.Add(new SandboxSettings.Setting<float>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x0600A6BD RID: 42685 RVA: 0x0010C0F8 File Offset: 0x0010A2F8
	public float GetFloatSetting(string prefsKey)
	{
		return KPlayerPrefs.GetFloat(prefsKey);
	}

	// Token: 0x0600A6BE RID: 42686 RVA: 0x003F55C0 File Offset: 0x003F37C0
	public void SetFloatSetting(string prefsKey, float value)
	{
		SandboxSettings.Setting<float> setting = this.floatSettings.Find((SandboxSettings.Setting<float> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No KPlayerPrefs float setting named: ",
				prefsKey,
				" could be found amongst ",
				this.floatSettings.Count.ToString(),
				" float settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x0600A6BF RID: 42687 RVA: 0x0010C100 File Offset: 0x0010A300
	public void RestoreFloatSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetFloatSetting(prefsKey, this.GetFloatSetting(prefsKey));
			return;
		}
		this.ForceDefaultFloatSetting(prefsKey);
	}

	// Token: 0x0600A6C0 RID: 42688 RVA: 0x003F5644 File Offset: 0x003F3844
	public void ForceDefaultFloatSetting(string prefsKey)
	{
		this.SetFloatSetting(prefsKey, this.floatSettings.Find((SandboxSettings.Setting<float> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x0600A6C1 RID: 42689 RVA: 0x0010C120 File Offset: 0x0010A320
	public void AddStringSetting(string prefsKey, Action<string> setAction, string defaultValue)
	{
		this.stringSettings.Add(new SandboxSettings.Setting<string>(prefsKey, setAction, defaultValue));
	}

	// Token: 0x0600A6C2 RID: 42690 RVA: 0x0010C135 File Offset: 0x0010A335
	public string GetStringSetting(string prefsKey)
	{
		return KPlayerPrefs.GetString(prefsKey);
	}

	// Token: 0x0600A6C3 RID: 42691 RVA: 0x003F5688 File Offset: 0x003F3888
	public void SetStringSetting(string prefsKey, string value)
	{
		SandboxSettings.Setting<string> setting = this.stringSettings.Find((SandboxSettings.Setting<string> match) => match.PrefsKey == prefsKey);
		if (setting == null)
		{
			Debug.LogError(string.Concat(new string[]
			{
				"No KPlayerPrefs string setting named: ",
				prefsKey,
				" could be found amongst ",
				this.stringSettings.Count.ToString(),
				" settings."
			}));
		}
		setting.Value = value;
	}

	// Token: 0x0600A6C4 RID: 42692 RVA: 0x0010C13D File Offset: 0x0010A33D
	public void RestoreStringSetting(string prefsKey)
	{
		if (KPlayerPrefs.HasKey(prefsKey))
		{
			this.SetStringSetting(prefsKey, this.GetStringSetting(prefsKey));
			return;
		}
		this.ForceDefaultStringSetting(prefsKey);
	}

	// Token: 0x0600A6C5 RID: 42693 RVA: 0x003F570C File Offset: 0x003F390C
	public void ForceDefaultStringSetting(string prefsKey)
	{
		this.SetStringSetting(prefsKey, this.stringSettings.Find((SandboxSettings.Setting<string> match) => match.PrefsKey == prefsKey).defaultValue);
	}

	// Token: 0x0600A6C6 RID: 42694 RVA: 0x003F5750 File Offset: 0x003F3950
	public SandboxSettings()
	{
		this.AddStringSetting("SandboxTools.SelectedEntity", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedEntity", data);
			this.OnChangeEntity();
		}, "MushBar");
		this.AddIntSetting("SandboxTools.SelectedElement", delegate(int data)
		{
			KPlayerPrefs.SetInt("SandboxTools.SelectedElement", data);
			this.OnChangeElement(this.hasRestoredElement);
			this.hasRestoredElement = true;
		}, (int)ElementLoader.GetElementIndex(SimHashes.Oxygen));
		this.AddStringSetting("SandboxTools.SelectedDisease", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedDisease", data);
			this.OnChangeDisease();
		}, Db.Get().Diseases.FoodGerms.Id);
		this.AddIntSetting("SandboxTools.DiseaseCount", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandboxTools.DiseaseCount", val);
			this.OnChangeDiseaseCount();
		}, 0);
		this.AddStringSetting("SandboxTools.SelectedStory", delegate(string data)
		{
			KPlayerPrefs.SetString("SandboxTools.SelectedStory", data);
			this.OnChangeStory();
		}, Db.Get().Stories.resources[Db.Get().Stories.resources.Count - 1].Id);
		this.AddIntSetting("SandboxTools.BrushSize", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandboxTools.BrushSize", val);
			this.OnChangeBrushSize();
		}, 1);
		this.AddFloatSetting("SandboxTools.NoiseScale", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.NoiseScale", val);
			this.OnChangeNoiseScale();
		}, 1f);
		this.AddFloatSetting("SandboxTools.NoiseDensity", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.NoiseDensity", val);
			this.OnChangeNoiseDensity();
		}, 1f);
		this.AddFloatSetting("SandboxTools.Mass", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandboxTools.Mass", val);
			this.OnChangeMass();
		}, 1f);
		this.AddFloatSetting("SandbosTools.Temperature", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.Temperature", val);
			this.OnChangeTemperature();
		}, 300f);
		this.AddFloatSetting("SandbosTools.TemperatureAdditive", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.TemperatureAdditive", val);
			this.OnChangeAdditiveTemperature();
		}, 5f);
		this.AddFloatSetting("SandbosTools.StressAdditive", delegate(float val)
		{
			KPlayerPrefs.SetFloat("SandbosTools.StressAdditive", val);
			this.OnChangeAdditiveStress();
		}, 50f);
		this.AddIntSetting("SandbosTools.MoraleAdjustment", delegate(int val)
		{
			KPlayerPrefs.SetInt("SandbosTools.MoraleAdjustment", val);
			this.OnChangeMoraleAdjustment();
		}, 50);
	}

	// Token: 0x0600A6C7 RID: 42695 RVA: 0x003F592C File Offset: 0x003F3B2C
	public void RestorePrefs()
	{
		foreach (SandboxSettings.Setting<int> setting in this.intSettings)
		{
			this.RestoreIntSetting(setting.PrefsKey);
		}
		foreach (SandboxSettings.Setting<float> setting2 in this.floatSettings)
		{
			this.RestoreFloatSetting(setting2.PrefsKey);
		}
		foreach (SandboxSettings.Setting<string> setting3 in this.stringSettings)
		{
			this.RestoreStringSetting(setting3.PrefsKey);
		}
	}

	// Token: 0x040082FF RID: 33535
	private List<SandboxSettings.Setting<int>> intSettings = new List<SandboxSettings.Setting<int>>();

	// Token: 0x04008300 RID: 33536
	private List<SandboxSettings.Setting<float>> floatSettings = new List<SandboxSettings.Setting<float>>();

	// Token: 0x04008301 RID: 33537
	private List<SandboxSettings.Setting<string>> stringSettings = new List<SandboxSettings.Setting<string>>();

	// Token: 0x04008302 RID: 33538
	public bool InstantBuild = true;

	// Token: 0x04008303 RID: 33539
	private bool hasRestoredElement;

	// Token: 0x04008304 RID: 33540
	public Action<bool> OnChangeElement;

	// Token: 0x04008305 RID: 33541
	public System.Action OnChangeMass;

	// Token: 0x04008306 RID: 33542
	public System.Action OnChangeDisease;

	// Token: 0x04008307 RID: 33543
	public System.Action OnChangeDiseaseCount;

	// Token: 0x04008308 RID: 33544
	public System.Action OnChangeStory;

	// Token: 0x04008309 RID: 33545
	public System.Action OnChangeEntity;

	// Token: 0x0400830A RID: 33546
	public System.Action OnChangeBrushSize;

	// Token: 0x0400830B RID: 33547
	public System.Action OnChangeNoiseScale;

	// Token: 0x0400830C RID: 33548
	public System.Action OnChangeNoiseDensity;

	// Token: 0x0400830D RID: 33549
	public System.Action OnChangeTemperature;

	// Token: 0x0400830E RID: 33550
	public System.Action OnChangeAdditiveTemperature;

	// Token: 0x0400830F RID: 33551
	public System.Action OnChangeAdditiveStress;

	// Token: 0x04008310 RID: 33552
	public System.Action OnChangeMoraleAdjustment;

	// Token: 0x04008311 RID: 33553
	public const string KEY_SELECTED_ENTITY = "SandboxTools.SelectedEntity";

	// Token: 0x04008312 RID: 33554
	public const string KEY_SELECTED_ELEMENT = "SandboxTools.SelectedElement";

	// Token: 0x04008313 RID: 33555
	public const string KEY_SELECTED_DISEASE = "SandboxTools.SelectedDisease";

	// Token: 0x04008314 RID: 33556
	public const string KEY_DISEASE_COUNT = "SandboxTools.DiseaseCount";

	// Token: 0x04008315 RID: 33557
	public const string KEY_SELECTED_STORY = "SandboxTools.SelectedStory";

	// Token: 0x04008316 RID: 33558
	public const string KEY_BRUSH_SIZE = "SandboxTools.BrushSize";

	// Token: 0x04008317 RID: 33559
	public const string KEY_NOISE_SCALE = "SandboxTools.NoiseScale";

	// Token: 0x04008318 RID: 33560
	public const string KEY_NOISE_DENSITY = "SandboxTools.NoiseDensity";

	// Token: 0x04008319 RID: 33561
	public const string KEY_MASS = "SandboxTools.Mass";

	// Token: 0x0400831A RID: 33562
	public const string KEY_TEMPERATURE = "SandbosTools.Temperature";

	// Token: 0x0400831B RID: 33563
	public const string KEY_TEMPERATURE_ADDITIVE = "SandbosTools.TemperatureAdditive";

	// Token: 0x0400831C RID: 33564
	public const string KEY_STRESS_ADDITIVE = "SandbosTools.StressAdditive";

	// Token: 0x0400831D RID: 33565
	public const string KEY_MORALE_ADJUSTMENT = "SandbosTools.MoraleAdjustment";

	// Token: 0x02001EED RID: 7917
	public class Setting<T>
	{
		// Token: 0x0600A6D5 RID: 42709 RVA: 0x0010C2A2 File Offset: 0x0010A4A2
		public Setting(string prefsKey, Action<T> setAction, T defaultValue)
		{
			this.prefsKey = prefsKey;
			this.SetAction = setAction;
			this.defaultValue = defaultValue;
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x0600A6D6 RID: 42710 RVA: 0x0010C2BF File Offset: 0x0010A4BF
		public string PrefsKey
		{
			get
			{
				return this.prefsKey;
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (set) Token: 0x0600A6D7 RID: 42711 RVA: 0x0010C2C7 File Offset: 0x0010A4C7
		public T Value
		{
			set
			{
				this.SetAction(value);
			}
		}

		// Token: 0x0400831E RID: 33566
		private string prefsKey;

		// Token: 0x0400831F RID: 33567
		private Action<T> SetAction;

		// Token: 0x04008320 RID: 33568
		public T defaultValue;
	}
}
