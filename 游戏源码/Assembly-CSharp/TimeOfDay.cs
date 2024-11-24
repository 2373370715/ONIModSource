using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FMOD.Studio;
using KSerialization;
using ProcGen;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/TimeOfDay")]
public class TimeOfDay : KMonoBehaviour, ISaveLoadable
{
	public enum TimeRegion
	{
		Invalid,
		Day,
		Night
	}

	private const string MILESTONE_CYCLE_REACHED_AUDIO_NAME = "Stinger_Day_Celebrate";

	public static List<int> MILESTONE_CYCLES = new List<int>(2) { 99, 999 };

	[Serialize]
	private float scale;

	private EventInstance nightLPEvent;

	public static TimeOfDay Instance;

	public string stingerDay;

	public string stingerNight;

	private bool isEclipse;

	public static bool IsMilestoneApproaching
	{
		get
		{
			if (Instance != null && GameClock.Instance != null)
			{
				TimeRegion currentTimeRegion = Instance.GetCurrentTimeRegion();
				int cycle = GameClock.Instance.GetCycle();
				if (currentTimeRegion == TimeRegion.Night && MILESTONE_CYCLES != null)
				{
					return MILESTONE_CYCLES.Contains(cycle + 1);
				}
				return false;
			}
			return false;
		}
	}

	public static bool IsMilestoneDay
	{
		get
		{
			if (Instance != null && GameClock.Instance != null)
			{
				TimeRegion currentTimeRegion = Instance.GetCurrentTimeRegion();
				int cycle = GameClock.Instance.GetCycle();
				if (currentTimeRegion == TimeRegion.Day && MILESTONE_CYCLES != null)
				{
					return MILESTONE_CYCLES.Contains(cycle);
				}
				return false;
			}
			return false;
		}
	}

	public TimeRegion timeRegion { get; private set; }

	public static void DestroyInstance()
	{
		Instance = null;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Instance = null;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		timeRegion = GetCurrentTimeRegion();
		string clusterId = SaveLoader.Instance.GameInfo.clusterId;
		ClusterLayout clusterData = SettingsCache.clusterLayouts.GetClusterData(clusterId);
		if (clusterData != null && !string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerDay))
		{
			stingerDay = clusterData.clusterAudio.stingerDay;
		}
		else
		{
			stingerDay = "Stinger_Day";
		}
		if (clusterData != null && !string.IsNullOrWhiteSpace(clusterData.clusterAudio.stingerNight))
		{
			stingerNight = clusterData.clusterAudio.stingerNight;
		}
		else
		{
			stingerNight = "Stinger_Loop_Night";
		}
		if (!MusicManager.instance.SongIsPlaying(stingerNight) && GetCurrentTimeRegion() == TimeRegion.Night)
		{
			MusicManager.instance.PlaySong(stingerNight);
			MusicManager.instance.SetSongParameter(stingerNight, "Music_PlayStinger", 0f);
		}
		UpdateSunlightIntensity();
	}

	[OnDeserialized]
	private void OnDeserialized()
	{
		UpdateVisuals();
	}

	public TimeRegion GetCurrentTimeRegion()
	{
		if (GameClock.Instance.IsNighttime())
		{
			return TimeRegion.Night;
		}
		return TimeRegion.Day;
	}

	private void Update()
	{
		UpdateVisuals();
		TimeRegion currentTimeRegion = GetCurrentTimeRegion();
		int cycle = GameClock.Instance.GetCycle();
		if (currentTimeRegion != timeRegion)
		{
			if (IsMilestoneApproaching)
			{
				Game.Instance.Trigger(-720092972, cycle);
			}
			if (IsMilestoneDay)
			{
				Game.Instance.Trigger(2070437606, cycle);
			}
			TriggerSoundChange(currentTimeRegion, IsMilestoneDay);
			timeRegion = currentTimeRegion;
			Trigger(1791086652);
		}
	}

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
		scale = Mathf.Lerp(scale, b, Time.deltaTime * num2);
		float y = UpdateSunlightIntensity();
		Shader.SetGlobalVector("_TimeOfDay", new Vector4(scale, y, 0f, 0f));
	}

	public void Sim4000ms(float dt)
	{
		UpdateSunlightIntensity();
	}

	public void SetEclipse(bool eclipse)
	{
		isEclipse = eclipse;
	}

	private float UpdateSunlightIntensity()
	{
		float daytimeDurationInPercentage = GameClock.Instance.GetDaytimeDurationInPercentage();
		float num = GameClock.Instance.GetCurrentCycleAsPercentage() / daytimeDurationInPercentage;
		if (num >= 1f || isEclipse)
		{
			num = 0f;
		}
		float num2 = Mathf.Sin(num * (float)Math.PI);
		Game.Instance.currentFallbackSunlightIntensity = num2 * 80000f;
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			worldContainer.currentSunlightIntensity = num2 * (float)worldContainer.sunlight;
			worldContainer.currentCosmicIntensity = worldContainer.cosmicRadiation;
		}
		return num2;
	}

	private void TriggerSoundChange(TimeRegion new_region, bool milestoneReached)
	{
		switch (new_region)
		{
		case TimeRegion.Day:
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().NightStartedMigrated);
			if (MusicManager.instance.SongIsPlaying(stingerNight))
			{
				MusicManager.instance.StopSong(stingerNight);
			}
			if (milestoneReached)
			{
				MusicManager.instance.PlaySong("Stinger_Day_Celebrate");
			}
			else
			{
				MusicManager.instance.PlaySong(stingerDay);
			}
			MusicManager.instance.PlayDynamicMusic();
			break;
		case TimeRegion.Night:
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().NightStartedMigrated);
			MusicManager.instance.PlaySong(stingerNight);
			break;
		}
	}

	public void SetScale(float new_scale)
	{
		scale = new_scale;
	}
}
