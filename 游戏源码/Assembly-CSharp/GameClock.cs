using System;
using System.IO;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using UnityEngine;

// Token: 0x0200131E RID: 4894
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/GameClock")]
public class GameClock : KMonoBehaviour, ISaveLoadable, ISim33ms, IRender1000ms
{
	// Token: 0x06006481 RID: 25729 RVA: 0x000E196B File Offset: 0x000DFB6B
	public static void DestroyInstance()
	{
		GameClock.Instance = null;
	}

	// Token: 0x06006482 RID: 25730 RVA: 0x000E1973 File Offset: 0x000DFB73
	protected override void OnPrefabInit()
	{
		GameClock.Instance = this;
		this.timeSinceStartOfCycle = 50f;
	}

	// Token: 0x06006483 RID: 25731 RVA: 0x002C0A80 File Offset: 0x002BEC80
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.time != 0f)
		{
			this.cycle = (int)(this.time / 600f);
			this.timeSinceStartOfCycle = Mathf.Max(this.time - (float)this.cycle * 600f, 0f);
			this.time = 0f;
		}
	}

	// Token: 0x06006484 RID: 25732 RVA: 0x000E1986 File Offset: 0x000DFB86
	public void Sim33ms(float dt)
	{
		this.AddTime(dt);
	}

	// Token: 0x06006485 RID: 25733 RVA: 0x000E198F File Offset: 0x000DFB8F
	public void Render1000ms(float dt)
	{
		this.timePlayed += dt;
	}

	// Token: 0x06006486 RID: 25734 RVA: 0x000E199F File Offset: 0x000DFB9F
	private void LateUpdate()
	{
		this.frame++;
	}

	// Token: 0x06006487 RID: 25735 RVA: 0x002C0ADC File Offset: 0x002BECDC
	private void AddTime(float dt)
	{
		this.timeSinceStartOfCycle += dt;
		bool flag = false;
		while (this.timeSinceStartOfCycle >= 600f)
		{
			this.cycle++;
			this.timeSinceStartOfCycle -= 600f;
			base.Trigger(631075836, null);
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				worldContainer.Trigger(631075836, null);
			}
			flag = true;
		}
		if (!this.isNight && this.IsNighttime())
		{
			this.isNight = true;
			base.Trigger(-722330267, null);
		}
		if (this.isNight && !this.IsNighttime())
		{
			this.isNight = false;
		}
		if (flag && SaveGame.Instance.AutoSaveCycleInterval > 0 && this.cycle % SaveGame.Instance.AutoSaveCycleInterval == 0)
		{
			this.DoAutoSave(this.cycle);
		}
		int num = Mathf.FloorToInt(this.timeSinceStartOfCycle - dt / 25f);
		int num2 = Mathf.FloorToInt(this.timeSinceStartOfCycle / 25f);
		if (num != num2)
		{
			base.Trigger(-1215042067, num2);
		}
	}

	// Token: 0x06006488 RID: 25736 RVA: 0x000E19AF File Offset: 0x000DFBAF
	public float GetTimeSinceStartOfReport()
	{
		if (this.IsNighttime())
		{
			return 525f - this.GetTimeSinceStartOfCycle();
		}
		return this.GetTimeSinceStartOfCycle() + 75f;
	}

	// Token: 0x06006489 RID: 25737 RVA: 0x000E19D2 File Offset: 0x000DFBD2
	public float GetTimeSinceStartOfCycle()
	{
		return this.timeSinceStartOfCycle;
	}

	// Token: 0x0600648A RID: 25738 RVA: 0x000E19DA File Offset: 0x000DFBDA
	public float GetCurrentCycleAsPercentage()
	{
		return this.timeSinceStartOfCycle / 600f;
	}

	// Token: 0x0600648B RID: 25739 RVA: 0x000E19E8 File Offset: 0x000DFBE8
	public float GetTime()
	{
		return this.timeSinceStartOfCycle + (float)this.cycle * 600f;
	}

	// Token: 0x0600648C RID: 25740 RVA: 0x000E19FE File Offset: 0x000DFBFE
	public float GetTimeInCycles()
	{
		return (float)this.cycle + this.GetCurrentCycleAsPercentage();
	}

	// Token: 0x0600648D RID: 25741 RVA: 0x000E1A0E File Offset: 0x000DFC0E
	public int GetFrame()
	{
		return this.frame;
	}

	// Token: 0x0600648E RID: 25742 RVA: 0x000E1A16 File Offset: 0x000DFC16
	public int GetCycle()
	{
		return this.cycle;
	}

	// Token: 0x0600648F RID: 25743 RVA: 0x000E1A1E File Offset: 0x000DFC1E
	public bool IsNighttime()
	{
		return GameClock.Instance.GetCurrentCycleAsPercentage() >= 0.875f;
	}

	// Token: 0x06006490 RID: 25744 RVA: 0x000E1A34 File Offset: 0x000DFC34
	public float GetDaytimeDurationInPercentage()
	{
		return 0.875f;
	}

	// Token: 0x06006491 RID: 25745 RVA: 0x002C0C20 File Offset: 0x002BEE20
	public void SetTime(float new_time)
	{
		float dt = Mathf.Max(new_time - this.GetTime(), 0f);
		this.AddTime(dt);
	}

	// Token: 0x06006492 RID: 25746 RVA: 0x000E1A3B File Offset: 0x000DFC3B
	public float GetTimePlayedInSeconds()
	{
		return this.timePlayed;
	}

	// Token: 0x06006493 RID: 25747 RVA: 0x002C0C48 File Offset: 0x002BEE48
	private void DoAutoSave(int day)
	{
		if (GenericGameSettings.instance.disableAutosave)
		{
			return;
		}
		day++;
		OniMetrics.LogEvent(OniMetrics.Event.EndOfCycle, GameClock.NewCycleKey, day);
		OniMetrics.SendEvent(OniMetrics.Event.EndOfCycle, "DoAutoSave");
		string text = SaveLoader.GetActiveSaveFilePath();
		if (text == null)
		{
			text = SaveLoader.GetAutosaveFilePath();
		}
		int num = text.LastIndexOf("\\");
		if (num > 0)
		{
			int num2 = text.IndexOf(" Cycle ", num);
			if (num2 > 0)
			{
				text = text.Substring(0, num2);
			}
		}
		text = Path.ChangeExtension(text, null);
		text = text + " Cycle " + day.ToString();
		text = SaveScreen.GetValidSaveFilename(text);
		text = Path.Combine(SaveLoader.GetActiveAutoSavePath(), Path.GetFileName(text));
		string text2 = text;
		int num3 = 1;
		while (File.Exists(text))
		{
			text = text2.Replace(".sav", "");
			text = SaveScreen.GetValidSaveFilename(text2 + " (" + num3.ToString() + ")");
			num3++;
		}
		Game.Instance.StartDelayedSave(text, true, false);
	}

	// Token: 0x04004846 RID: 18502
	public static GameClock Instance;

	// Token: 0x04004847 RID: 18503
	[Serialize]
	private int frame;

	// Token: 0x04004848 RID: 18504
	[Serialize]
	private float time;

	// Token: 0x04004849 RID: 18505
	[Serialize]
	private float timeSinceStartOfCycle;

	// Token: 0x0400484A RID: 18506
	[Serialize]
	private int cycle;

	// Token: 0x0400484B RID: 18507
	[Serialize]
	private float timePlayed;

	// Token: 0x0400484C RID: 18508
	[Serialize]
	private bool isNight;

	// Token: 0x0400484D RID: 18509
	public static readonly string NewCycleKey = "NewCycle";
}
