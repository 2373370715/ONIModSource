using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x0200094F RID: 2383
public class AudioMixer
{
	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06002B11 RID: 11025 RVA: 0x000BC03A File Offset: 0x000BA23A
	public static AudioMixer instance
	{
		get
		{
			return AudioMixer._instance;
		}
	}

	// Token: 0x06002B12 RID: 11026 RVA: 0x001DD8D8 File Offset: 0x001DBAD8
	public static AudioMixer Create()
	{
		AudioMixer._instance = new AudioMixer();
		AudioMixerSnapshots audioMixerSnapshots = AudioMixerSnapshots.Get();
		if (audioMixerSnapshots != null)
		{
			audioMixerSnapshots.ReloadSnapshots();
		}
		return AudioMixer._instance;
	}

	// Token: 0x06002B13 RID: 11027 RVA: 0x000BC041 File Offset: 0x000BA241
	public static void Destroy()
	{
		AudioMixer._instance.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
		AudioMixer._instance = null;
	}

	// Token: 0x06002B14 RID: 11028 RVA: 0x001DD90C File Offset: 0x001DBB0C
	public EventInstance Start(EventReference event_ref)
	{
		string snapshot;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out snapshot);
		return this.Start(snapshot);
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x001DD938 File Offset: 0x001DBB38
	public EventInstance Start(string snapshot)
	{
		EventInstance eventInstance;
		if (!this.activeSnapshots.TryGetValue(snapshot, out eventInstance))
		{
			if (RuntimeManager.IsInitialized)
			{
				eventInstance = KFMOD.CreateInstance(snapshot);
				this.activeSnapshots[snapshot] = eventInstance;
				eventInstance.start();
				eventInstance.setParameterByName("snapshotActive", 1f, false);
			}
			else
			{
				eventInstance = default(EventInstance);
			}
		}
		AudioMixer.instance.Log("Start Snapshot: " + snapshot);
		return eventInstance;
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x001DD9B8 File Offset: 0x001DBBB8
	public bool Stop(EventReference event_ref, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		string s;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out s);
		return this.Stop(s, stop_mode);
	}

	// Token: 0x06002B17 RID: 11031 RVA: 0x001DD9E8 File Offset: 0x001DBBE8
	public bool Stop(HashedString snapshot, FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
	{
		bool result = false;
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshot, out eventInstance))
		{
			eventInstance.setParameterByName("snapshotActive", 0f, false);
			eventInstance.stop(stop_mode);
			eventInstance.release();
			this.activeSnapshots.Remove(snapshot);
			result = true;
			AudioMixer instance = AudioMixer.instance;
			string[] array = new string[5];
			array[0] = "Stop Snapshot: [";
			int num = 1;
			HashedString hashedString = snapshot;
			array[num] = hashedString.ToString();
			array[2] = "] with fadeout mode: [";
			array[3] = stop_mode.ToString();
			array[4] = "]";
			instance.Log(string.Concat(array));
		}
		else
		{
			AudioMixer instance2 = AudioMixer.instance;
			string str = "Tried to stop snapshot: [";
			HashedString hashedString = snapshot;
			instance2.Log(str + hashedString.ToString() + "] but it wasn't active.");
		}
		return result;
	}

	// Token: 0x06002B18 RID: 11032 RVA: 0x000BC054 File Offset: 0x000BA254
	public void Reset()
	{
		this.StopAll(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x001DDAB8 File Offset: 0x001DBCB8
	public void StopAll(FMOD.Studio.STOP_MODE stop_mode = FMOD.Studio.STOP_MODE.IMMEDIATE)
	{
		List<HashedString> list = new List<HashedString>();
		foreach (KeyValuePair<HashedString, EventInstance> keyValuePair in this.activeSnapshots)
		{
			if (keyValuePair.Key != AudioMixer.UserVolumeSettingsHash)
			{
				list.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.Stop(list[i], stop_mode);
		}
	}

	// Token: 0x06002B1A RID: 11034 RVA: 0x001DDB4C File Offset: 0x001DBD4C
	public bool SnapshotIsActive(EventReference event_ref)
	{
		string s;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out s);
		return this.SnapshotIsActive(s);
	}

	// Token: 0x06002B1B RID: 11035 RVA: 0x000BC05D File Offset: 0x000BA25D
	public bool SnapshotIsActive(HashedString snapshot_name)
	{
		return this.activeSnapshots.ContainsKey(snapshot_name);
	}

	// Token: 0x06002B1C RID: 11036 RVA: 0x001DDB7C File Offset: 0x001DBD7C
	public void SetSnapshotParameter(EventReference event_ref, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		string snapshot_name;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out snapshot_name);
		this.SetSnapshotParameter(snapshot_name, parameter_name, parameter_value, shouldLog);
	}

	// Token: 0x06002B1D RID: 11037 RVA: 0x001DDBAC File Offset: 0x001DBDAC
	public void SetSnapshotParameter(string snapshot_name, string parameter_name, float parameter_value, bool shouldLog = true)
	{
		if (shouldLog)
		{
			this.Log(string.Format("Set Param {0}: {1}, {2}", snapshot_name, parameter_name, parameter_value));
		}
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshot_name, out eventInstance))
		{
			eventInstance.setParameterByName(parameter_name, parameter_value, false);
			return;
		}
		this.Log(string.Concat(new string[]
		{
			"Tried to set [",
			parameter_name,
			"] to [",
			parameter_value.ToString(),
			"] but [",
			snapshot_name,
			"] is not active."
		}));
	}

	// Token: 0x06002B1E RID: 11038 RVA: 0x001DDC3C File Offset: 0x001DBE3C
	public void StartPersistentSnapshots()
	{
		this.persistentSnapshotsActive = true;
		this.Start(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
		this.Start(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
		this.Start(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
		this.spaceVisibleInst = this.Start(AudioMixerSnapshots.Get().SpaceVisibleSnapshot);
		this.facilityVisibleInst = this.Start(AudioMixerSnapshots.Get().FacilityVisibleSnapshot);
		this.Start(AudioMixerSnapshots.Get().PulseSnapshot);
	}

	// Token: 0x06002B1F RID: 11039 RVA: 0x001DDCC0 File Offset: 0x001DBEC0
	public void StopPersistentSnapshots()
	{
		this.persistentSnapshotsActive = false;
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().SpaceVisibleSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().FacilityVisibleSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.Stop(AudioMixerSnapshots.Get().PulseSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x06002B20 RID: 11040 RVA: 0x001DDD40 File Offset: 0x001DBF40
	private string GetSnapshotName(EventReference event_ref)
	{
		string result;
		RuntimeManager.GetEventDescription(event_ref.Guid).getPath(out result);
		return result;
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x001DDD64 File Offset: 0x001DBF64
	public void UpdatePersistentSnapshotParameters()
	{
		this.SetVisibleDuplicants();
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountMovingSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName, out this.duplicantCountMovingInst))
		{
			this.duplicantCountMovingInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["moving"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName2 = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountSleepingSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName2, out this.duplicantCountSleepingInst))
		{
			this.duplicantCountSleepingInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["sleeping"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName3 = this.GetSnapshotName(AudioMixerSnapshots.Get().DuplicantCountAttenuatorMigrated);
		if (this.activeSnapshots.TryGetValue(snapshotName3, out this.duplicantCountInst))
		{
			this.duplicantCountInst.setParameterByName("duplicantCount", (float)Mathf.Max(0, this.visibleDupes["visible"] - AudioMixer.VISIBLE_DUPLICANTS_BEFORE_ATTENUATION), false);
		}
		string snapshotName4 = this.GetSnapshotName(AudioMixerSnapshots.Get().PulseSnapshot);
		if (this.activeSnapshots.TryGetValue(snapshotName4, out this.pulseInst))
		{
			float num = AudioMixer.PULSE_SNAPSHOT_BPM / 60f;
			int speed = SpeedControlScreen.Instance.GetSpeed();
			if (speed == 1)
			{
				num /= 2f;
			}
			else if (speed == 2)
			{
				num /= 3f;
			}
			float value = Mathf.Abs(Mathf.Sin(Time.time * 3.1415927f * num));
			this.pulseInst.setParameterByName("Pulse", value, false);
		}
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x000BC070 File Offset: 0x000BA270
	public void UpdateSpaceVisibleSnapshot(float percent)
	{
		this.spaceVisibleInst.setParameterByName("spaceVisible", percent, false);
	}

	// Token: 0x06002B23 RID: 11043 RVA: 0x000BC085 File Offset: 0x000BA285
	public void PauseSpaceVisibleSnapshot(bool pause)
	{
		this.spaceVisibleInst.setParameterByName("spaceVisible", 0f, true);
		this.spaceVisibleInst.setPaused(pause);
	}

	// Token: 0x06002B24 RID: 11044 RVA: 0x000BC0AB File Offset: 0x000BA2AB
	public void UpdateFacilityVisibleSnapshot(float percent)
	{
		this.facilityVisibleInst.setParameterByName("facilityVisible", percent, false);
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x001DDF14 File Offset: 0x001DC114
	private void SetVisibleDuplicants()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			Vector3 position = Components.LiveMinionIdentities[i].transform.GetPosition();
			if (CameraController.Instance.IsVisiblePos(position))
			{
				num++;
				Navigator component = Components.LiveMinionIdentities[i].GetComponent<Navigator>();
				if (component != null && component.IsMoving())
				{
					num2++;
				}
				else
				{
					StaminaMonitor.Instance smi = Components.LiveMinionIdentities[i].GetComponent<WorkerBase>().GetSMI<StaminaMonitor.Instance>();
					if (smi != null && smi.IsSleeping())
					{
						num3++;
					}
				}
			}
		}
		this.visibleDupes["visible"] = num;
		this.visibleDupes["moving"] = num2;
		this.visibleDupes["sleeping"] = num3;
	}

	// Token: 0x06002B26 RID: 11046 RVA: 0x001DDFF4 File Offset: 0x001DC1F4
	public void StartUserVolumesSnapshot()
	{
		this.Start(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshotName, out eventInstance))
		{
			EventDescription eventDescription;
			eventInstance.getDescription(out eventDescription);
			USER_PROPERTY user_PROPERTY;
			eventDescription.getUserProperty("buses", out user_PROPERTY);
			string text = user_PROPERTY.stringValue();
			char separator = '-';
			string[] array = text.Split(separator, StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				float busLevel = 1f;
				string key = "Volume_" + array[i];
				if (KPlayerPrefs.HasKey(key))
				{
					busLevel = KPlayerPrefs.GetFloat(key);
				}
				AudioMixer.UserVolumeBus userVolumeBus = new AudioMixer.UserVolumeBus();
				userVolumeBus.busLevel = busLevel;
				userVolumeBus.labelString = Strings.Get("STRINGS.UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUDIO_BUS_" + array[i].ToUpper());
				this.userVolumeSettings.Add(array[i], userVolumeBus);
				this.SetUserVolume(array[i], userVolumeBus.busLevel);
			}
		}
	}

	// Token: 0x06002B27 RID: 11047 RVA: 0x001DE108 File Offset: 0x001DC308
	public void SetUserVolume(string bus, float value)
	{
		if (!this.userVolumeSettings.ContainsKey(bus))
		{
			global::Debug.LogError("The provided bus doesn't exist. Check yo'self fool!");
			return;
		}
		if (value > 1f)
		{
			value = 1f;
		}
		else if (value < 0f)
		{
			value = 0f;
		}
		this.userVolumeSettings[bus].busLevel = value;
		KPlayerPrefs.SetFloat("Volume_" + bus, value);
		string snapshotName = this.GetSnapshotName(AudioMixerSnapshots.Get().UserVolumeSettingsSnapshot);
		EventInstance eventInstance;
		if (this.activeSnapshots.TryGetValue(snapshotName, out eventInstance))
		{
			eventInstance.setParameterByName("userVolume_" + bus, this.userVolumeSettings[bus].busLevel, false);
		}
		else
		{
			this.Log(string.Concat(new string[]
			{
				"Tried to set [",
				bus,
				"] to [",
				value.ToString(),
				"] but UserVolumeSettingsSnapshot is not active."
			}));
		}
		if (bus == "Music")
		{
			this.SetSnapshotParameter(AudioMixerSnapshots.Get().DynamicMusicPlayingSnapshot, "userVolume_Music", value, true);
		}
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void Log(string s)
	{
	}

	// Token: 0x04001CD1 RID: 7377
	private static AudioMixer _instance = null;

	// Token: 0x04001CD2 RID: 7378
	private const string DUPLICANT_COUNT_ID = "duplicantCount";

	// Token: 0x04001CD3 RID: 7379
	private const string PULSE_ID = "Pulse";

	// Token: 0x04001CD4 RID: 7380
	private const string SNAPSHOT_ACTIVE_ID = "snapshotActive";

	// Token: 0x04001CD5 RID: 7381
	private const string SPACE_VISIBLE_ID = "spaceVisible";

	// Token: 0x04001CD6 RID: 7382
	private const string FACILITY_VISIBLE_ID = "facilityVisible";

	// Token: 0x04001CD7 RID: 7383
	private const string FOCUS_BUS_PATH = "bus:/SFX/Focus";

	// Token: 0x04001CD8 RID: 7384
	public Dictionary<HashedString, EventInstance> activeSnapshots = new Dictionary<HashedString, EventInstance>();

	// Token: 0x04001CD9 RID: 7385
	public List<HashedString> SnapshotDebugLog = new List<HashedString>();

	// Token: 0x04001CDA RID: 7386
	public bool activeNIS;

	// Token: 0x04001CDB RID: 7387
	public static float LOW_PRIORITY_CUTOFF_DISTANCE = 10f;

	// Token: 0x04001CDC RID: 7388
	public static float PULSE_SNAPSHOT_BPM = 120f;

	// Token: 0x04001CDD RID: 7389
	public static int VISIBLE_DUPLICANTS_BEFORE_ATTENUATION = 2;

	// Token: 0x04001CDE RID: 7390
	private EventInstance duplicantCountInst;

	// Token: 0x04001CDF RID: 7391
	private EventInstance pulseInst;

	// Token: 0x04001CE0 RID: 7392
	private EventInstance duplicantCountMovingInst;

	// Token: 0x04001CE1 RID: 7393
	private EventInstance duplicantCountSleepingInst;

	// Token: 0x04001CE2 RID: 7394
	private EventInstance spaceVisibleInst;

	// Token: 0x04001CE3 RID: 7395
	private EventInstance facilityVisibleInst;

	// Token: 0x04001CE4 RID: 7396
	private static readonly HashedString UserVolumeSettingsHash = new HashedString("event:/Snapshots/Mixing/Snapshot_UserVolumeSettings");

	// Token: 0x04001CE5 RID: 7397
	public bool persistentSnapshotsActive;

	// Token: 0x04001CE6 RID: 7398
	private Dictionary<string, int> visibleDupes = new Dictionary<string, int>();

	// Token: 0x04001CE7 RID: 7399
	public Dictionary<string, AudioMixer.UserVolumeBus> userVolumeSettings = new Dictionary<string, AudioMixer.UserVolumeBus>();

	// Token: 0x02000950 RID: 2384
	public class UserVolumeBus
	{
		// Token: 0x04001CE8 RID: 7400
		public string labelString;

		// Token: 0x04001CE9 RID: 7401
		public float busLevel;
	}
}
