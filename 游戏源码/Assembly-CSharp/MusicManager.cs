using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using ProcGen;
using UnityEngine;

// Token: 0x02001635 RID: 5685
[AddComponentMenu("KMonoBehaviour/scripts/MusicManager")]
public class MusicManager : KMonoBehaviour, ISerializationCallbackReceiver
{
	// Token: 0x1700076D RID: 1901
	// (get) Token: 0x0600759C RID: 30108 RVA: 0x000ED362 File Offset: 0x000EB562
	public Dictionary<string, MusicManager.SongInfo> SongMap
	{
		get
		{
			return this.songMap;
		}
	}

	// Token: 0x0600759D RID: 30109 RVA: 0x00306C50 File Offset: 0x00304E50
	public void PlaySong(string song_name, bool canWait = false)
	{
		this.Log("Play: " + song_name);
		if (!AudioDebug.Get().musicEnabled)
		{
			return;
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.songMap.TryGetValue(song_name, out songInfo))
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Unknown song:",
				song_name
			});
			return;
		}
		if (this.activeSongs.ContainsKey(song_name))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Trying to play duplicate song:",
				song_name
			});
			return;
		}
		if (this.activeSongs.Count == 0)
		{
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array = new object[1];
				int num = 0;
				string str = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array[num] = str + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array);
			}
			int num2 = (songInfo.numberOfVariations > 0) ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
			if (num2 != -1)
			{
				songInfo.ev.setParameterByName("variation", (float)num2, false);
			}
			if (songInfo.dynamic)
			{
				songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_DELAY, 16000f);
				songInfo.ev.setProperty(EVENT_PROPERTY.SCHEDULE_LOOKAHEAD, 48000f);
				this.activeDynamicSong = songInfo;
			}
			songInfo.ev.start();
			this.activeSongs[song_name] = songInfo;
			return;
		}
		List<string> list = new List<string>(this.activeSongs.Keys);
		if (songInfo.interruptsActiveMusic)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.activeSongs[list[i]].interruptsActiveMusic)
				{
					MusicManager.SongInfo songInfo2 = this.activeSongs[list[i]];
					songInfo2.ev.setParameterByName("interrupted_dimmed", 1f, false);
					this.Log("Dimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
					songInfo.songsOnHold.Add(list[i]);
				}
			}
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array2 = new object[1];
				int num3 = 0;
				string str2 = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array2[num3] = str2 + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array2);
			}
			songInfo.ev.start();
			songInfo.ev.release();
			this.activeSongs[song_name] = songInfo;
			return;
		}
		int num4 = 0;
		foreach (string key in this.activeSongs.Keys)
		{
			MusicManager.SongInfo songInfo3 = this.activeSongs[key];
			if (!songInfo3.interruptsActiveMusic && songInfo3.priority > num4)
			{
				num4 = songInfo3.priority;
			}
		}
		if (songInfo.priority >= num4)
		{
			for (int j = 0; j < list.Count; j++)
			{
				MusicManager.SongInfo songInfo4 = this.activeSongs[list[j]];
				FMOD.Studio.EventInstance ev = songInfo4.ev;
				if (!songInfo4.interruptsActiveMusic)
				{
					ev.setParameterByName("interrupted_dimmed", 1f, false);
					ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
					this.activeSongs.Remove(list[j]);
					list.Remove(list[j]);
				}
			}
			songInfo.ev = KFMOD.CreateInstance(songInfo.fmodEvent);
			if (!songInfo.ev.isValid())
			{
				object[] array3 = new object[1];
				int num5 = 0;
				string str3 = "Failed to find FMOD event [";
				EventReference fmodEvent = songInfo.fmodEvent;
				array3[num5] = str3 + fmodEvent.ToString() + "]";
				DebugUtil.LogWarningArgs(array3);
			}
			int num6 = (songInfo.numberOfVariations > 0) ? UnityEngine.Random.Range(1, songInfo.numberOfVariations + 1) : -1;
			if (num6 != -1)
			{
				songInfo.ev.setParameterByName("variation", (float)num6, false);
			}
			songInfo.ev.start();
			this.activeSongs[song_name] = songInfo;
		}
	}

	// Token: 0x0600759E RID: 30110 RVA: 0x00307060 File Offset: 0x00305260
	public void StopSong(string song_name, bool shouldLog = true, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		if (shouldLog)
		{
			this.Log("Stop: " + song_name);
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.songMap.TryGetValue(song_name, out songInfo))
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Unknown song:",
				song_name
			});
			return;
		}
		if (!this.activeSongs.ContainsKey(song_name))
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Trying to stop a song that isn't playing:",
				song_name
			});
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		ev.stop(stopMode);
		ev.release();
		if (songInfo.dynamic)
		{
			this.activeDynamicSong = null;
		}
		if (songInfo.songsOnHold.Count > 0)
		{
			for (int i = 0; i < songInfo.songsOnHold.Count; i++)
			{
				MusicManager.SongInfo songInfo2;
				if (this.activeSongs.TryGetValue(songInfo.songsOnHold[i], out songInfo2) && songInfo2.ev.isValid())
				{
					FMOD.Studio.EventInstance ev2 = songInfo2.ev;
					this.Log("Undimming: " + Assets.GetSimpleSoundEventName(songInfo2.fmodEvent));
					ev2.setParameterByName("interrupted_dimmed", 0f, false);
					songInfo.songsOnHold.Remove(songInfo.songsOnHold[i]);
				}
				else
				{
					songInfo.songsOnHold.Remove(songInfo.songsOnHold[i]);
				}
			}
		}
		this.activeSongs.Remove(song_name);
	}

	// Token: 0x0600759F RID: 30111 RVA: 0x003071C4 File Offset: 0x003053C4
	public void KillAllSongs(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		this.Log("Kill All Songs");
		if (this.DynamicMusicIsActive())
		{
			this.StopDynamicMusic(true);
		}
		List<string> list = new List<string>(this.activeSongs.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			this.StopSong(list[i], true, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060075A0 RID: 30112 RVA: 0x0030721C File Offset: 0x0030541C
	public void SetSongParameter(string song_name, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", song_name, parameter_name, parameter_value));
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		if (ev.isValid())
		{
			ev.setParameterByName(parameter_name, parameter_value, false);
		}
	}

	// Token: 0x060075A1 RID: 30113 RVA: 0x00307274 File Offset: 0x00305474
	public void SetSongParameter(string song_name, string parameter_name, string parameter_lable, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", song_name, parameter_name, parameter_lable));
		}
		MusicManager.SongInfo songInfo = null;
		if (!this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			return;
		}
		FMOD.Studio.EventInstance ev = songInfo.ev;
		if (ev.isValid())
		{
			ev.setParameterByNameWithLabel(parameter_name, parameter_lable, false);
		}
	}

	// Token: 0x060075A2 RID: 30114 RVA: 0x003072C8 File Offset: 0x003054C8
	public bool SongIsPlaying(string song_name)
	{
		MusicManager.SongInfo songInfo = null;
		return this.activeSongs.TryGetValue(song_name, out songInfo) && songInfo.musicPlaybackState != PLAYBACK_STATE.STOPPED;
	}

	// Token: 0x060075A3 RID: 30115 RVA: 0x003072F4 File Offset: 0x003054F4
	private void Update()
	{
		this.ClearFinishedSongs();
		if (this.DynamicMusicIsActive())
		{
			this.SetDynamicMusicZoomLevel();
			this.SetDynamicMusicTimeSinceLastJob();
			if (this.activeDynamicSong.useTimeOfDay)
			{
				this.SetDynamicMusicTimeOfDay();
			}
			if (GameClock.Instance != null && GameClock.Instance.GetCurrentCycleAsPercentage() >= this.duskTimePercentage / 100f)
			{
				this.StopDynamicMusic(false);
			}
		}
	}

	// Token: 0x060075A4 RID: 30116 RVA: 0x0030735C File Offset: 0x0030555C
	private void ClearFinishedSongs()
	{
		if (this.activeSongs.Count > 0)
		{
			ListPool<string, MusicManager>.PooledList pooledList = ListPool<string, MusicManager>.Allocate();
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
			{
				MusicManager.SongInfo value = keyValuePair.Value;
				FMOD.Studio.EventInstance ev = value.ev;
				ev.getPlaybackState(out value.musicPlaybackState);
				if (value.musicPlaybackState == PLAYBACK_STATE.STOPPED || value.musicPlaybackState == PLAYBACK_STATE.STOPPING)
				{
					pooledList.Add(keyValuePair.Key);
					foreach (string song_name in value.songsOnHold)
					{
						this.SetSongParameter(song_name, "interrupted_dimmed", 0f, true);
					}
					value.songsOnHold.Clear();
				}
			}
			foreach (string key in pooledList)
			{
				this.activeSongs.Remove(key);
			}
			pooledList.Recycle();
		}
	}

	// Token: 0x060075A5 RID: 30117 RVA: 0x003074AC File Offset: 0x003056AC
	public void OnEscapeMenu(bool paused)
	{
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
		{
			if (keyValuePair.Value != null)
			{
				this.StartFadeToPause(keyValuePair.Value.ev, paused, 0.25f);
			}
		}
	}

	// Token: 0x060075A6 RID: 30118 RVA: 0x0030751C File Offset: 0x0030571C
	public void OnSupplyClosetMenu(bool paused, float fadeTime)
	{
		bool flag = !paused;
		if (!PauseScreen.Instance.IsNullOrDestroyed() && PauseScreen.Instance.IsActive() && flag && MusicManager.instance.SongIsPlaying("Music_ESC_Menu"))
		{
			MusicManager.SongInfo songInfo = this.songMap["Music_ESC_Menu"];
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
			{
				if (keyValuePair.Value != null && keyValuePair.Value != songInfo)
				{
					this.StartFadeToPause(keyValuePair.Value.ev, paused, 0.25f);
				}
			}
			this.StartFadeToPause(songInfo.ev, false, 0.25f);
			return;
		}
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair2 in this.activeSongs)
		{
			if (keyValuePair2.Value != null)
			{
				this.StartFadeToPause(keyValuePair2.Value.ev, paused, fadeTime);
			}
		}
	}

	// Token: 0x060075A7 RID: 30119 RVA: 0x000ED36A File Offset: 0x000EB56A
	public void StartFadeToPause(FMOD.Studio.EventInstance inst, bool paused, float fadeTime = 0.25f)
	{
		if (paused)
		{
			base.StartCoroutine(this.FadeToPause(inst, fadeTime));
			return;
		}
		base.StartCoroutine(this.FadeToUnpause(inst, fadeTime));
	}

	// Token: 0x060075A8 RID: 30120 RVA: 0x000ED38E File Offset: 0x000EB58E
	private IEnumerator FadeToPause(FMOD.Studio.EventInstance inst, float fadeTime)
	{
		float startVolume;
		float targetVolume;
		inst.getVolume(out startVolume, out targetVolume);
		targetVolume = 0f;
		float lerpTime = 0f;
		while (lerpTime < 1f)
		{
			lerpTime += Time.unscaledDeltaTime / fadeTime;
			float volume = Mathf.Lerp(startVolume, targetVolume, lerpTime);
			inst.setVolume(volume);
			yield return null;
		}
		inst.setPaused(true);
		yield break;
	}

	// Token: 0x060075A9 RID: 30121 RVA: 0x000ED3A4 File Offset: 0x000EB5A4
	private IEnumerator FadeToUnpause(FMOD.Studio.EventInstance inst, float fadeTime)
	{
		float startVolume;
		float targetVolume;
		inst.getVolume(out startVolume, out targetVolume);
		targetVolume = 1f;
		float lerpTime = 0f;
		inst.setPaused(false);
		while (lerpTime < 1f)
		{
			lerpTime += Time.unscaledDeltaTime / fadeTime;
			float volume = Mathf.Lerp(startVolume, targetVolume, lerpTime);
			inst.setVolume(volume);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060075AA RID: 30122 RVA: 0x00307648 File Offset: 0x00305848
	public void WattsonStartDynamicMusic()
	{
		ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
		if (currentClusterLayout != null && currentClusterLayout.clusterAudio != null && !string.IsNullOrWhiteSpace(currentClusterLayout.clusterAudio.musicFirst))
		{
			DebugUtil.Assert(this.fullSongPlaylist.songMap.ContainsKey(currentClusterLayout.clusterAudio.musicFirst), "Attempting to play dlc music that isn't in the fullSongPlaylist");
			this.activePlaylist = this.fullSongPlaylist;
			this.PlayDynamicMusic(currentClusterLayout.clusterAudio.musicFirst);
			return;
		}
		this.PlayDynamicMusic();
	}

	// Token: 0x060075AB RID: 30123 RVA: 0x003076C8 File Offset: 0x003058C8
	public void PlayDynamicMusic()
	{
		if (this.DynamicMusicIsActive())
		{
			this.Log("Trying to play DynamicMusic when it is already playing.");
			return;
		}
		string nextDynamicSong = this.GetNextDynamicSong();
		this.PlayDynamicMusic(nextDynamicSong);
	}

	// Token: 0x060075AC RID: 30124 RVA: 0x003076F8 File Offset: 0x003058F8
	private void PlayDynamicMusic(string song_name)
	{
		if (song_name == "NONE")
		{
			return;
		}
		this.PlaySong(song_name, false);
		MusicManager.SongInfo songInfo;
		if (this.activeSongs.TryGetValue(song_name, out songInfo))
		{
			this.activeDynamicSong = songInfo;
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot);
			if (SpeedControlScreen.Instance != null && SpeedControlScreen.Instance.IsPaused)
			{
				this.SetDynamicMusicPaused();
			}
			if (OverlayScreen.Instance != null && OverlayScreen.Instance.mode != OverlayModes.None.ID)
			{
				this.SetDynamicMusicOverlayActive();
			}
			this.SetDynamicMusicPlayHook();
			this.SetDynamicMusicKeySigniture();
			string key = "Volume_Music";
			if (KPlayerPrefs.HasKey(key))
			{
				float @float = KPlayerPrefs.GetFloat(key);
				AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", @float, true);
			}
			AudioMixer.instance.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "intensity", songInfo.sfxAttenuationPercentage / 100f, true);
			return;
		}
		this.Log("DynamicMusic song " + song_name + " did not start.");
		string text = "";
		foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.activeSongs)
		{
			text = text + keyValuePair.Key + ", ";
			global::Debug.Log(text);
		}
		DebugUtil.DevAssert(false, "Song failed to play: " + song_name, null);
	}

	// Token: 0x060075AD RID: 30125 RVA: 0x0030787C File Offset: 0x00305A7C
	public void StopDynamicMusic(bool stopImmediate = false)
	{
		if (this.activeDynamicSong != null)
		{
			FMOD.Studio.STOP_MODE stopMode = stopImmediate ? FMOD.Studio.STOP_MODE.IMMEDIATE : FMOD.Studio.STOP_MODE.ALLOWFADEOUT;
			this.Log("Stop DynamicMusic: " + Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent));
			this.StopSong(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), true, stopMode);
			this.activeDynamicSong = null;
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	// Token: 0x060075AE RID: 30126 RVA: 0x003078F0 File Offset: 0x00305AF0
	public string GetNextDynamicSong()
	{
		string result = "";
		if (this.alwaysPlayMusic && this.nextMusicType == MusicManager.TypeOfMusic.None)
		{
			while (this.nextMusicType == MusicManager.TypeOfMusic.None)
			{
				this.CycleToNextMusicType();
			}
		}
		switch (this.nextMusicType)
		{
		case MusicManager.TypeOfMusic.DynamicSong:
			result = this.fullSongPlaylist.GetNextSong();
			this.activePlaylist = this.fullSongPlaylist;
			break;
		case MusicManager.TypeOfMusic.MiniSong:
			result = this.miniSongPlaylist.GetNextSong();
			this.activePlaylist = this.miniSongPlaylist;
			break;
		case MusicManager.TypeOfMusic.None:
			result = "NONE";
			this.activePlaylist = null;
			break;
		}
		this.CycleToNextMusicType();
		return result;
	}

	// Token: 0x060075AF RID: 30127 RVA: 0x00307988 File Offset: 0x00305B88
	private void CycleToNextMusicType()
	{
		int num = this.musicTypeIterator + 1;
		this.musicTypeIterator = num;
		this.musicTypeIterator = num % this.musicStyleOrder.Length;
		this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
	}

	// Token: 0x060075B0 RID: 30128 RVA: 0x000ED3BA File Offset: 0x000EB5BA
	public bool DynamicMusicIsActive()
	{
		return this.activeDynamicSong != null;
	}

	// Token: 0x060075B1 RID: 30129 RVA: 0x000ED3C7 File Offset: 0x000EB5C7
	public void SetDynamicMusicPaused()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 1f, true);
		}
	}

	// Token: 0x060075B2 RID: 30130 RVA: 0x000ED3F2 File Offset: 0x000EB5F2
	public void SetDynamicMusicUnpaused()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "Paused", 0f, true);
		}
	}

	// Token: 0x060075B3 RID: 30131 RVA: 0x003079C8 File Offset: 0x00305BC8
	public void SetDynamicMusicZoomLevel()
	{
		if (CameraController.Instance != null)
		{
			float parameter_value = 100f - Camera.main.orthographicSize / 20f * 100f;
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "zoomPercentage", parameter_value, false);
		}
	}

	// Token: 0x060075B4 RID: 30132 RVA: 0x000ED41D File Offset: 0x000EB61D
	public void SetDynamicMusicTimeSinceLastJob()
	{
		this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "secsSinceNewJob", Time.time - Game.Instance.LastTimeWorkStarted, false);
	}

	// Token: 0x060075B5 RID: 30133 RVA: 0x00307A1C File Offset: 0x00305C1C
	public void SetDynamicMusicTimeOfDay()
	{
		if (this.time >= this.timeOfDayUpdateRate)
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "timeOfDay", GameClock.Instance.GetCurrentCycleAsPercentage(), false);
			this.time = 0f;
		}
		this.time += Time.deltaTime;
	}

	// Token: 0x060075B6 RID: 30134 RVA: 0x000ED44B File Offset: 0x000EB64B
	public void SetDynamicMusicOverlayActive()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 1f, true);
		}
	}

	// Token: 0x060075B7 RID: 30135 RVA: 0x000ED476 File Offset: 0x000EB676
	public void SetDynamicMusicOverlayInactive()
	{
		if (this.DynamicMusicIsActive())
		{
			this.SetSongParameter(Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent), "overlayActive", 0f, true);
		}
	}

	// Token: 0x060075B8 RID: 30136 RVA: 0x00307A7C File Offset: 0x00305C7C
	public void SetDynamicMusicPlayHook()
	{
		if (this.DynamicMusicIsActive())
		{
			string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
			this.SetSongParameter(simpleSoundEventName, "playHook", this.activeDynamicSong.playHook ? 1f : 0f, true);
			this.activePlaylist.songMap[simpleSoundEventName].playHook = !this.activePlaylist.songMap[simpleSoundEventName].playHook;
		}
	}

	// Token: 0x060075B9 RID: 30137 RVA: 0x000ED4A1 File Offset: 0x000EB6A1
	public bool ShouldPlayDynamicMusicLoadedGame()
	{
		return GameClock.Instance.GetCurrentCycleAsPercentage() <= this.loadGameCutoffPercentage / 100f;
	}

	// Token: 0x060075BA RID: 30138 RVA: 0x00307AF8 File Offset: 0x00305CF8
	public void SetDynamicMusicKeySigniture()
	{
		if (this.DynamicMusicIsActive())
		{
			string simpleSoundEventName = Assets.GetSimpleSoundEventName(this.activeDynamicSong.fmodEvent);
			string musicKeySigniture = this.activePlaylist.songMap[simpleSoundEventName].musicKeySigniture;
			float value;
			if (!(musicKeySigniture == "Ab"))
			{
				if (!(musicKeySigniture == "Bb"))
				{
					if (!(musicKeySigniture == "C"))
					{
						if (!(musicKeySigniture == "D"))
						{
							value = 2f;
						}
						else
						{
							value = 3f;
						}
					}
					else
					{
						value = 2f;
					}
				}
				else
				{
					value = 1f;
				}
			}
			else
			{
				value = 0f;
			}
			RuntimeManager.StudioSystem.setParameterByName("MusicInKey", value, false);
		}
	}

	// Token: 0x1700076E RID: 1902
	// (get) Token: 0x060075BB RID: 30139 RVA: 0x000ED4BE File Offset: 0x000EB6BE
	public static MusicManager instance
	{
		get
		{
			return MusicManager._instance;
		}
	}

	// Token: 0x060075BC RID: 30140 RVA: 0x000ED4C5 File Offset: 0x000EB6C5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!RuntimeManager.IsInitialized)
		{
			base.enabled = false;
			return;
		}
		if (KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayMusicKey))
		{
			this.alwaysPlayMusic = (KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayMusicKey) == 1);
		}
	}

	// Token: 0x060075BD RID: 30141 RVA: 0x000ED4FF File Offset: 0x000EB6FF
	protected override void OnPrefabInit()
	{
		MusicManager._instance = this;
		this.ConfigureSongs();
		this.nextMusicType = this.musicStyleOrder[this.musicTypeIterator];
	}

	// Token: 0x060075BE RID: 30142 RVA: 0x000ED520 File Offset: 0x000EB720
	protected override void OnCleanUp()
	{
		MusicManager._instance = null;
	}

	// Token: 0x060075BF RID: 30143 RVA: 0x000ED528 File Offset: 0x000EB728
	private static bool IsValidForDLCContext(string dlcid)
	{
		if (SaveLoader.Instance != null)
		{
			return SaveLoader.Instance.IsDLCActiveForCurrentSave(dlcid);
		}
		return DlcManager.IsContentSubscribed(dlcid);
	}

	// Token: 0x060075C0 RID: 30144 RVA: 0x00307BAC File Offset: 0x00305DAC
	[ContextMenu("Reload")]
	public void ConfigureSongs()
	{
		this.songMap.Clear();
		this.fullSongPlaylist.Clear();
		this.miniSongPlaylist.Clear();
		foreach (MusicManager.DynamicSong dynamicSong in this.fullSongs)
		{
			if (MusicManager.IsValidForDLCContext(dynamicSong.requiredDlcId))
			{
				string simpleSoundEventName = Assets.GetSimpleSoundEventName(dynamicSong.fmodEvent);
				MusicManager.SongInfo songInfo = new MusicManager.SongInfo();
				songInfo.fmodEvent = dynamicSong.fmodEvent;
				songInfo.requiredDlcId = dynamicSong.requiredDlcId;
				songInfo.priority = 100;
				songInfo.interruptsActiveMusic = false;
				songInfo.dynamic = true;
				songInfo.useTimeOfDay = dynamicSong.useTimeOfDay;
				songInfo.numberOfVariations = dynamicSong.numberOfVariations;
				songInfo.musicKeySigniture = dynamicSong.musicKeySigniture;
				songInfo.sfxAttenuationPercentage = this.dynamicMusicSFXAttenuationPercentage;
				this.songMap[simpleSoundEventName] = songInfo;
				this.fullSongPlaylist.songMap[simpleSoundEventName] = songInfo;
			}
		}
		foreach (MusicManager.Minisong minisong in this.miniSongs)
		{
			if (MusicManager.IsValidForDLCContext(minisong.requiredDlcId))
			{
				string simpleSoundEventName2 = Assets.GetSimpleSoundEventName(minisong.fmodEvent);
				MusicManager.SongInfo songInfo2 = new MusicManager.SongInfo();
				songInfo2.fmodEvent = minisong.fmodEvent;
				songInfo2.requiredDlcId = minisong.requiredDlcId;
				songInfo2.priority = 100;
				songInfo2.interruptsActiveMusic = false;
				songInfo2.dynamic = true;
				songInfo2.useTimeOfDay = false;
				songInfo2.numberOfVariations = 5;
				songInfo2.musicKeySigniture = minisong.musicKeySigniture;
				songInfo2.sfxAttenuationPercentage = this.miniSongSFXAttenuationPercentage;
				this.songMap[simpleSoundEventName2] = songInfo2;
				this.miniSongPlaylist.songMap[simpleSoundEventName2] = songInfo2;
			}
		}
		foreach (MusicManager.Stinger stinger in this.stingers)
		{
			if (MusicManager.IsValidForDLCContext(stinger.requiredDlcId))
			{
				string simpleSoundEventName3 = Assets.GetSimpleSoundEventName(stinger.fmodEvent);
				MusicManager.SongInfo songInfo3 = new MusicManager.SongInfo();
				songInfo3.fmodEvent = stinger.fmodEvent;
				songInfo3.priority = 100;
				songInfo3.interruptsActiveMusic = true;
				songInfo3.dynamic = false;
				songInfo3.useTimeOfDay = false;
				songInfo3.numberOfVariations = 0;
				songInfo3.requiredDlcId = stinger.requiredDlcId;
				this.songMap[simpleSoundEventName3] = songInfo3;
			}
		}
		foreach (MusicManager.MenuSong menuSong in this.menuSongs)
		{
			if (MusicManager.IsValidForDLCContext(menuSong.requiredDlcId))
			{
				string simpleSoundEventName4 = Assets.GetSimpleSoundEventName(menuSong.fmodEvent);
				MusicManager.SongInfo songInfo4 = new MusicManager.SongInfo();
				songInfo4.fmodEvent = menuSong.fmodEvent;
				songInfo4.priority = 100;
				songInfo4.interruptsActiveMusic = true;
				songInfo4.dynamic = false;
				songInfo4.useTimeOfDay = false;
				songInfo4.numberOfVariations = 0;
				songInfo4.requiredDlcId = menuSong.requiredDlcId;
				this.songMap[simpleSoundEventName4] = songInfo4;
			}
		}
		this.fullSongPlaylist.ResetUnplayedSongs();
		this.miniSongPlaylist.ResetUnplayedSongs();
	}

	// Token: 0x060075C1 RID: 30145 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x060075C2 RID: 30146 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x060075C3 RID: 30147 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void Log(string s)
	{
	}

	// Token: 0x04005821 RID: 22561
	private const string VARIATION_ID = "variation";

	// Token: 0x04005822 RID: 22562
	private const string INTERRUPTED_DIMMED_ID = "interrupted_dimmed";

	// Token: 0x04005823 RID: 22563
	private const string MUSIC_KEY = "MusicInKey";

	// Token: 0x04005824 RID: 22564
	private const float DYNAMIC_MUSIC_SCHEDULE_DELAY = 16000f;

	// Token: 0x04005825 RID: 22565
	private const float DYNAMIC_MUSIC_SCHEDULE_LOOKAHEAD = 48000f;

	// Token: 0x04005826 RID: 22566
	[Header("Song Lists")]
	[Tooltip("Play during the daytime. The mix of the song is affected by the player's input, like pausing the sim, activating an overlay, or zooming in and out.")]
	[SerializeField]
	private MusicManager.DynamicSong[] fullSongs;

	// Token: 0x04005827 RID: 22567
	[Tooltip("Simple dynamic songs which are more ambient in nature, which play quietly during \"non-music\" days. These are affected by Pause and OverlayActive.")]
	[SerializeField]
	private MusicManager.Minisong[] miniSongs;

	// Token: 0x04005828 RID: 22568
	[Tooltip("Triggered by in-game events, such as completing research or night-time falling. They will temporarily interrupt a dynamicSong, fading the dynamicSong back in after the stinger is complete.")]
	[SerializeField]
	private MusicManager.Stinger[] stingers;

	// Token: 0x04005829 RID: 22569
	[Tooltip("Generally songs that don't play during gameplay, while a menu is open. For example, the ESC menu or the Starmap.")]
	[SerializeField]
	private MusicManager.MenuSong[] menuSongs;

	// Token: 0x0400582A RID: 22570
	private Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();

	// Token: 0x0400582B RID: 22571
	public Dictionary<string, MusicManager.SongInfo> activeSongs = new Dictionary<string, MusicManager.SongInfo>();

	// Token: 0x0400582C RID: 22572
	[Space]
	[Header("Tuning Values")]
	[Tooltip("Just before night-time (88%), dynamic music fades out. At which point of the day should the music fade?")]
	[SerializeField]
	private float duskTimePercentage = 85f;

	// Token: 0x0400582D RID: 22573
	[Tooltip("If we load into a save and the day is almost over, we shouldn't play music because it will stop soon anyway. At what point of the day should we not play music?")]
	[SerializeField]
	private float loadGameCutoffPercentage = 50f;

	// Token: 0x0400582E RID: 22574
	[Tooltip("When dynamic music is active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
	[SerializeField]
	private float dynamicMusicSFXAttenuationPercentage = 65f;

	// Token: 0x0400582F RID: 22575
	[Tooltip("When mini songs are active, we play a snapshot which attenuates the ambience and SFX. What intensity should that snapshot be applied?")]
	[SerializeField]
	private float miniSongSFXAttenuationPercentage;

	// Token: 0x04005830 RID: 22576
	[SerializeField]
	private MusicManager.TypeOfMusic[] musicStyleOrder;

	// Token: 0x04005831 RID: 22577
	[NonSerialized]
	public bool alwaysPlayMusic;

	// Token: 0x04005832 RID: 22578
	private MusicManager.DynamicSongPlaylist fullSongPlaylist = new MusicManager.DynamicSongPlaylist();

	// Token: 0x04005833 RID: 22579
	private MusicManager.DynamicSongPlaylist miniSongPlaylist = new MusicManager.DynamicSongPlaylist();

	// Token: 0x04005834 RID: 22580
	[NonSerialized]
	public MusicManager.SongInfo activeDynamicSong;

	// Token: 0x04005835 RID: 22581
	[NonSerialized]
	public MusicManager.DynamicSongPlaylist activePlaylist;

	// Token: 0x04005836 RID: 22582
	private MusicManager.TypeOfMusic nextMusicType;

	// Token: 0x04005837 RID: 22583
	private int musicTypeIterator;

	// Token: 0x04005838 RID: 22584
	private float time;

	// Token: 0x04005839 RID: 22585
	private float timeOfDayUpdateRate = 2f;

	// Token: 0x0400583A RID: 22586
	private static MusicManager _instance;

	// Token: 0x0400583B RID: 22587
	[NonSerialized]
	public List<string> MusicDebugLog = new List<string>();

	// Token: 0x02001636 RID: 5686
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class SongInfo
	{
		// Token: 0x0400583C RID: 22588
		public EventReference fmodEvent;

		// Token: 0x0400583D RID: 22589
		[NonSerialized]
		public int priority;

		// Token: 0x0400583E RID: 22590
		[NonSerialized]
		public bool interruptsActiveMusic;

		// Token: 0x0400583F RID: 22591
		[NonSerialized]
		public bool dynamic;

		// Token: 0x04005840 RID: 22592
		[NonSerialized]
		public string requiredDlcId = "";

		// Token: 0x04005841 RID: 22593
		[NonSerialized]
		public bool useTimeOfDay;

		// Token: 0x04005842 RID: 22594
		[NonSerialized]
		public int numberOfVariations;

		// Token: 0x04005843 RID: 22595
		[NonSerialized]
		public string musicKeySigniture = "C";

		// Token: 0x04005844 RID: 22596
		[NonSerialized]
		public FMOD.Studio.EventInstance ev;

		// Token: 0x04005845 RID: 22597
		[NonSerialized]
		public List<string> songsOnHold = new List<string>();

		// Token: 0x04005846 RID: 22598
		[NonSerialized]
		public PLAYBACK_STATE musicPlaybackState;

		// Token: 0x04005847 RID: 22599
		[NonSerialized]
		public bool playHook = true;

		// Token: 0x04005848 RID: 22600
		[NonSerialized]
		public float sfxAttenuationPercentage = 65f;
	}

	// Token: 0x02001637 RID: 5687
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class DynamicSong
	{
		// Token: 0x04005849 RID: 22601
		public EventReference fmodEvent;

		// Token: 0x0400584A RID: 22602
		[Tooltip("Some songs are set up to have Morning, Daytime, Hook, and Intro sections. Toggle this ON if this song has those sections.")]
		[SerializeField]
		public bool useTimeOfDay;

		// Token: 0x0400584B RID: 22603
		[Tooltip("Some songs have different possible start locations. Enter how many start locations this song is set up to support.")]
		[SerializeField]
		public int numberOfVariations;

		// Token: 0x0400584C RID: 22604
		[Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
		[SerializeField]
		public string musicKeySigniture = "";

		// Token: 0x0400584D RID: 22605
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x02001638 RID: 5688
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class Stinger
	{
		// Token: 0x0400584E RID: 22606
		public EventReference fmodEvent;

		// Token: 0x0400584F RID: 22607
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x02001639 RID: 5689
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class MenuSong
	{
		// Token: 0x04005850 RID: 22608
		public EventReference fmodEvent;

		// Token: 0x04005851 RID: 22609
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x0200163A RID: 5690
	[DebuggerDisplay("{fmodEvent}")]
	[Serializable]
	public class Minisong
	{
		// Token: 0x04005852 RID: 22610
		public EventReference fmodEvent;

		// Token: 0x04005853 RID: 22611
		[Tooltip("Some songs have different key signitures. Enter the key this music is in.")]
		[SerializeField]
		public string musicKeySigniture = "";

		// Token: 0x04005854 RID: 22612
		[Tooltip("Should playback of this song be limited to an active DLC?")]
		[SerializeField]
		public string requiredDlcId = "";
	}

	// Token: 0x0200163B RID: 5691
	public enum TypeOfMusic
	{
		// Token: 0x04005856 RID: 22614
		DynamicSong,
		// Token: 0x04005857 RID: 22615
		MiniSong,
		// Token: 0x04005858 RID: 22616
		None
	}

	// Token: 0x0200163C RID: 5692
	public class DynamicSongPlaylist
	{
		// Token: 0x060075CA RID: 30154 RVA: 0x000ED5E6 File Offset: 0x000EB7E6
		public void Clear()
		{
			this.songMap.Clear();
			this.unplayedSongs.Clear();
			this.lastSongPlayed = "";
		}

		// Token: 0x060075CB RID: 30155 RVA: 0x00307F34 File Offset: 0x00306134
		public string GetNextSong()
		{
			string text;
			if (this.unplayedSongs.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
				text = this.unplayedSongs[index];
				this.unplayedSongs.RemoveAt(index);
			}
			else
			{
				this.ResetUnplayedSongs();
				bool flag = this.unplayedSongs.Count > 1;
				if (flag)
				{
					for (int i = 0; i < this.unplayedSongs.Count; i++)
					{
						if (this.unplayedSongs[i] == this.lastSongPlayed)
						{
							this.unplayedSongs.Remove(this.unplayedSongs[i]);
							break;
						}
					}
				}
				int index2 = UnityEngine.Random.Range(0, this.unplayedSongs.Count);
				text = this.unplayedSongs[index2];
				this.unplayedSongs.RemoveAt(index2);
				if (flag)
				{
					this.unplayedSongs.Add(this.lastSongPlayed);
				}
			}
			this.lastSongPlayed = text;
			global::Debug.Assert(this.songMap.ContainsKey(text), "Missing song " + text);
			return Assets.GetSimpleSoundEventName(this.songMap[text].fmodEvent);
		}

		// Token: 0x060075CC RID: 30156 RVA: 0x00308060 File Offset: 0x00306260
		public void ResetUnplayedSongs()
		{
			this.unplayedSongs.Clear();
			foreach (KeyValuePair<string, MusicManager.SongInfo> keyValuePair in this.songMap)
			{
				if (MusicManager.IsValidForDLCContext(keyValuePair.Value.requiredDlcId))
				{
					this.unplayedSongs.Add(keyValuePair.Key);
				}
			}
		}

		// Token: 0x04005859 RID: 22617
		public Dictionary<string, MusicManager.SongInfo> songMap = new Dictionary<string, MusicManager.SongInfo>();

		// Token: 0x0400585A RID: 22618
		public List<string> unplayedSongs = new List<string>();

		// Token: 0x0400585B RID: 22619
		private string lastSongPlayed = "";
	}
}
