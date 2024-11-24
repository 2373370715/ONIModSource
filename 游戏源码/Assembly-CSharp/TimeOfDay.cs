using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02001793 RID: 6035
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TimeOfDay")]
public class TimeOfDay : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x170007D5 RID: 2005
	// (get) Token: 0x06007C29 RID: 31785 RVA: 0x0031FCE4 File Offset: 0x0031DEE4
	public static bool IsMilestoneApproaching
	{
		get
		{
			if (TimeOfDay.Instance != null && GameClock.Instance != null)
			{
				int currentTimeRegion = (int)TimeOfDay.Instance.GetCurrentTimeRegion();
				int cycle = GameClock.Instance.GetCycle();
				return currentTimeRegion == 2 && TimeOfDay.MILESTONE_CYCLES != null && TimeOfDay.MILESTONE_CYCLES.Contains(cycle + 1);
			}
			return false;
		}
	}

	// Token: 0x170007D6 RID: 2006
	// (get) Token: 0x06007C2A RID: 31786 RVA: 0x0031FD3C File Offset: 0x0031DF3C
	public static bool IsMilestoneDay
	{
		get
		{
			if (TimeOfDay.Instance != null && GameClock.Instance != null)
			{
				int currentTimeRegion = (int)TimeOfDay.Instance.GetCurrentTimeRegion();
				int cycle = GameClock.Instance.GetCycle();
				return currentTimeRegion == 1 && TimeOfDay.MILESTONE_CYCLES != null && TimeOfDay.MILESTONE_CYCLES.Contains(cycle);
			}
			return false;
		}
	}

	// Token: 0x170007D7 RID: 2007
	// (get) Token: 0x06007C2C RID: 31788 RVA: 0x000F1CE2 File Offset: 0x000EFEE2
	// (set) Token: 0x06007C2B RID: 31787 RVA: 0x000F1CD9 File Offset: 0x000EFED9
	public TimeOfDay.TimeRegion timeRegion { get; private set; }

	// Token: 0x06007C2D RID: 31789 RVA: 0x000F1CEA File Offset: 0x000EFEEA
	public static void DestroyInstance()
	{
		TimeOfDay.Instance = null;
	}

	// Token: 0x06007C2E RID: 31790 RVA: 0x000F1CF2 File Offset: 0x000EFEF2
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		TimeOfDay.Instance = this;
	}

	// Token: 0x06007C2F RID: 31791 RVA: 0x000F1D00 File Offset: 0x000EFF00
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		TimeOfDay.Instance = null;
	}

	// Token: 0x06007C30 RID: 31792 RVA: 0x0031FD94 File Offset: 0x0031DF94
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.timeRegion = this.GetCurrentTimeRegion();
		string clusterId = SaveLoader.Instance.GameInfo.clusterId;
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(clusterId);
		if (clusterData != null && !string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerDay))
		{
			this.stingerDay = clusterData.clusterAudio.stingerDay;
		}
		else
		{
			this.stingerDay = "Stinger_Day";
		}
		if (clusterData != null && !string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerNight))
		{
			this.stingerNight = clusterData.clusterAudio.stingerNight;
		}
		else
		{
			this.stingerNight = "Stinger_Loop_Night";
		}
		if (!MusicManager.instance.SongIsPlaying(this.stingerNight) && this.GetCurrentTimeRegion() == TimeOfDay.TimeRegion.Night)
		{
			MusicManager.instance.PlaySong(this.stingerNight, false);
			MusicManager.instance.SetSongParameter(this.stingerNight, "Music_PlayStinger", 0f, true);
		}
		this.UpdateSunlightIntensity();
	}

	// Token: 0x06007C31 RID: 31793 RVA: 0x000F1D0E File Offset: 0x000EFF0E
	[OnDeserialized]
	private void OnDeserialized()
	{
		this.UpdateVisuals();
	}

	// Token: 0x06007C32 RID: 31794 RVA: 0x000F1D16 File Offset: 0x000EFF16
	public TimeOfDay.TimeRegion GetCurrentTimeRegion()
	{
		if (GameClock.Instance.IsNighttime())
		{
			return TimeOfDay.TimeRegion.Night;
		}
		return TimeOfDay.TimeRegion.Day;
	}

	// Token: 0x06007C33 RID: 31795 RVA: 0x0031FE84 File Offset: 0x0031E084
	private void Update()
	{
		this.UpdateVisuals();
		TimeOfDay.TimeRegion currentTimeRegion = this.GetCurrentTimeRegion();
		int cycle = GameClock.Instance.GetCycle();
		if (currentTimeRegion != this.timeRegion)
		{
			if (TimeOfDay.IsMilestoneApproaching)
			{
				Game.Instance.Trigger(-720092972, cycle);
			}
			if (TimeOfDay.IsMilestoneDay)
			{
				Game.Instance.Trigger(2070437606, cycle);
			}
			this.TriggerSoundChange(currentTimeRegion, TimeOfDay.IsMilestoneDay);
			this.timeRegion = currentTimeRegion;
			base.Trigger(1791086652, null);
		}
	}

	// Token: 0x06007C34 RID: 31796 RVA: 0x0031FF0C File Offset: 0x0031E10C
	private void UpdateVisuals()
	{
		float num = 0.875f;
		float num2 = 0.2f;
		float num3 = 1f;
		float b = 0f;
		if (GameClock.Instance.GetCurrentCycleAsPercentage() >= num)
		{
			b = num3;
		}
		this.scale = Mathf.Lerp(this.scale, b, Time.deltaTime * num2);
		float y = this.UpdateSunlightIntensity();
		Shader.SetGlobalVector("_TimeOfDay", new Vector4(this.scale, y, 0f, 0f));
	}

	// Token: 0x06007C35 RID: 31797 RVA: 0x000F1D27 File Offset: 0x000EFF27
	public void Sim4000ms(float dt)
	{
		this.UpdateSunlightIntensity();
	}

	// Token: 0x06007C36 RID: 31798 RVA: 0x000F1D30 File Offset: 0x000EFF30
	public void SetEclipse(bool eclipse)
	{
		this.isEclipse = eclipse;
	}

	// Token: 0x06007C37 RID: 31799 RVA: 0x0031FF84 File Offset: 0x0031E184
	private float UpdateSunlightIntensity()
	{
		float daytimeDurationInPercentage = GameClock.Instance.GetDaytimeDurationInPercentage();
		float num = GameClock.Instance.GetCurrentCycleAsPercentage() / daytimeDurationInPercentage;
		if (num >= 1f || this.isEclipse)
		{
			num = 0f;
		}
		float num2 = Mathf.Sin(num * 3.1415927f);
		Game.Instance.currentFallbackSunlightIntensity = num2 * 80000f;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			worldContainer.currentSunlightIntensity = num2 * (float)worldContainer.sunlight;
			worldContainer.currentCosmicIntensity = (float)worldContainer.cosmicRadiation;
		}
		return num2;
	}

	// Token: 0x06007C38 RID: 31800 RVA: 0x0032003C File Offset: 0x0031E23C
	private void TriggerSoundChange(TimeOfDay.TimeRegion new_region, bool milestoneReached)
	{
		if (new_region == TimeOfDay.TimeRegion.Day)
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().NightStartedMigrated, STOP_MODE.ALLOWFADEOUT);
			if (MusicManager.instance.SongIsPlaying(this.stingerNight))
			{
				MusicManager.instance.StopSong(this.stingerNight, true, STOP_MODE.ALLOWFADEOUT);
			}
			if (milestoneReached)
			{
				MusicManager.instance.PlaySong("Stinger_Day_Celebrate", false);
			}
			else
			{
				MusicManager.instance.PlaySong(this.stingerDay, false);
			}
			MusicManager.instance.PlayDynamicMusic();
			return;
		}
		if (new_region != TimeOfDay.TimeRegion.Night)
		{
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().NightStartedMigrated);
		MusicManager.instance.PlaySong(this.stingerNight, false);
	}

	// Token: 0x06007C39 RID: 31801 RVA: 0x000F1D39 File Offset: 0x000EFF39
	public void SetScale(float new_scale)
	{
		this.scale = new_scale;
	}

	// Token: 0x04005DE8 RID: 24040
	private const string MILESTONE_CYCLE_REACHED_AUDIO_NAME = "Stinger_Day_Celebrate";

	// Token: 0x04005DE9 RID: 24041
	public static List<int> MILESTONE_CYCLES = new List<int>(2)
	{
		99,
		999
	};

	// Token: 0x04005DEA RID: 24042
	[Serialize]
	private float scale;

	// Token: 0x04005DEC RID: 24044
	private EventInstance nightLPEvent;

	// Token: 0x04005DED RID: 24045
	public static TimeOfDay Instance;

	// Token: 0x04005DEE RID: 24046
	public string stingerDay;

	// Token: 0x04005DEF RID: 24047
	public string stingerNight;

	// Token: 0x04005DF0 RID: 24048
	private bool isEclipse;

	// Token: 0x02001794 RID: 6036
	public enum TimeRegion
	{
		// Token: 0x04005DF2 RID: 24050
		Invalid,
		// Token: 0x04005DF3 RID: 24051
		Day,
		// Token: 0x04005DF4 RID: 24052
		Night
	}
}
