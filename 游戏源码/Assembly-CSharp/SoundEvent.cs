using System;
using System.Diagnostics;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000935 RID: 2357
[DebuggerDisplay("{Name}")]
public class SoundEvent : AnimEvent
{
	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06002A88 RID: 10888 RVA: 0x000BBA5B File Offset: 0x000B9C5B
	// (set) Token: 0x06002A89 RID: 10889 RVA: 0x000BBA63 File Offset: 0x000B9C63
	public string sound { get; private set; }

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06002A8A RID: 10890 RVA: 0x000BBA6C File Offset: 0x000B9C6C
	// (set) Token: 0x06002A8B RID: 10891 RVA: 0x000BBA74 File Offset: 0x000B9C74
	public HashedString soundHash { get; private set; }

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06002A8C RID: 10892 RVA: 0x000BBA7D File Offset: 0x000B9C7D
	// (set) Token: 0x06002A8D RID: 10893 RVA: 0x000BBA85 File Offset: 0x000B9C85
	public bool looping { get; private set; }

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06002A8E RID: 10894 RVA: 0x000BBA8E File Offset: 0x000B9C8E
	// (set) Token: 0x06002A8F RID: 10895 RVA: 0x000BBA96 File Offset: 0x000B9C96
	public bool ignorePause { get; set; }

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06002A90 RID: 10896 RVA: 0x000BBA9F File Offset: 0x000B9C9F
	// (set) Token: 0x06002A91 RID: 10897 RVA: 0x000BBAA7 File Offset: 0x000B9CA7
	public bool shouldCameraScalePosition { get; set; }

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06002A92 RID: 10898 RVA: 0x000BBAB0 File Offset: 0x000B9CB0
	// (set) Token: 0x06002A93 RID: 10899 RVA: 0x000BBAB8 File Offset: 0x000B9CB8
	public float minInterval { get; private set; }

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06002A94 RID: 10900 RVA: 0x000BBAC1 File Offset: 0x000B9CC1
	// (set) Token: 0x06002A95 RID: 10901 RVA: 0x000BBAC9 File Offset: 0x000B9CC9
	public bool objectIsSelectedAndVisible { get; set; }

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06002A96 RID: 10902 RVA: 0x000BBAD2 File Offset: 0x000B9CD2
	// (set) Token: 0x06002A97 RID: 10903 RVA: 0x000BBADA File Offset: 0x000B9CDA
	public EffectorValues noiseValues { get; set; }

	// Token: 0x06002A98 RID: 10904 RVA: 0x000BBAE3 File Offset: 0x000B9CE3
	public SoundEvent()
	{
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x001DB1DC File Offset: 0x001D93DC
	public SoundEvent(string file_name, string sound_name, int frame, bool do_load, bool is_looping, float min_interval, bool is_dynamic) : base(file_name, sound_name, frame)
	{
		this.shouldCameraScalePosition = true;
		if (do_load)
		{
			this.sound = GlobalAssets.GetSound(sound_name, false);
			this.soundHash = new HashedString(this.sound);
			string.IsNullOrEmpty(this.sound);
		}
		this.minInterval = min_interval;
		this.looping = is_looping;
		this.isDynamic = is_dynamic;
		this.noiseValues = SoundEventVolumeCache.instance.GetVolume(file_name, sound_name);
	}

	// Token: 0x06002A9A RID: 10906 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public static bool ObjectIsSelectedAndVisible(GameObject go)
	{
		return false;
	}

	// Token: 0x06002A9B RID: 10907 RVA: 0x001DB254 File Offset: 0x001D9454
	public static Vector3 AudioHighlightListenerPosition(Vector3 sound_pos)
	{
		Vector3 position = SoundListenerController.Instance.transform.position;
		float x = 1f * sound_pos.x + 0f * position.x;
		float y = 1f * sound_pos.y + 0f * position.y;
		float z = 0f * position.z;
		return new Vector3(x, y, z);
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x001DB2BC File Offset: 0x001D94BC
	public static float GetVolume(bool objectIsSelectedAndVisible)
	{
		float result = 1f;
		if (objectIsSelectedAndVisible)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x000BBAEB File Offset: 0x000B9CEB
	public static bool ShouldPlaySound(KBatchedAnimController controller, string sound, bool is_looping, bool is_dynamic)
	{
		return SoundEvent.ShouldPlaySound(controller, sound, sound, is_looping, is_dynamic);
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x001DB2DC File Offset: 0x001D94DC
	public static bool ShouldPlaySound(KBatchedAnimController controller, string sound, HashedString soundHash, bool is_looping, bool is_dynamic)
	{
		CameraController instance = CameraController.Instance;
		if (instance == null)
		{
			return true;
		}
		Vector3 position = controller.transform.GetPosition();
		Vector3 offset = controller.Offset;
		position.x += offset.x;
		position.y += offset.y;
		if (!SoundCuller.IsAudibleWorld(position))
		{
			return false;
		}
		SpeedControlScreen instance2 = SpeedControlScreen.Instance;
		if (is_dynamic)
		{
			return (!(instance2 != null) || !instance2.IsPaused) && instance.IsAudibleSound(position);
		}
		if (sound == null || SoundEvent.IsLowPrioritySound(sound))
		{
			return false;
		}
		if (!instance.IsAudibleSound(position, soundHash))
		{
			if (!is_looping && !GlobalAssets.IsHighPriority(sound))
			{
				return false;
			}
		}
		else if (instance2 != null && instance2.IsPaused)
		{
			return false;
		}
		return true;
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x001DB3A8 File Offset: 0x001D95A8
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		GameObject gameObject = behaviour.controller.gameObject;
		this.objectIsSelectedAndVisible = SoundEvent.ObjectIsSelectedAndVisible(gameObject);
		if (this.objectIsSelectedAndVisible || SoundEvent.ShouldPlaySound(behaviour.controller, this.sound, this.soundHash, this.looping, this.isDynamic))
		{
			this.PlaySound(behaviour);
		}
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x001DB404 File Offset: 0x001D9604
	protected void PlaySound(AnimEventManager.EventPlayerData behaviour, string sound)
	{
		Vector3 vector = behaviour.controller.transform.GetPosition();
		vector.z = 0f;
		if (SoundEvent.ObjectIsSelectedAndVisible(behaviour.controller.gameObject))
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
		}
		KBatchedAnimController controller = behaviour.controller;
		if (controller != null)
		{
			Vector3 offset = controller.Offset;
			vector.x += offset.x;
			vector.y += offset.y;
		}
		AudioDebug audioDebug = AudioDebug.Get();
		if (audioDebug != null && audioDebug.debugSoundEvents)
		{
			string[] array = new string[7];
			array[0] = behaviour.name;
			array[1] = ", ";
			array[2] = sound;
			array[3] = ", ";
			array[4] = base.frame.ToString();
			array[5] = ", ";
			int num = 6;
			Vector3 vector2 = vector;
			array[num] = vector2.ToString();
			global::Debug.Log(string.Concat(array));
		}
		try
		{
			if (this.looping)
			{
				LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
				if (component == null)
				{
					global::Debug.Log(behaviour.name + " is missing LoopingSounds component. ");
				}
				else if (!component.StartSound(sound, behaviour, this.noiseValues, this.ignorePause, this.shouldCameraScalePosition))
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, behaviour.name)
					});
				}
			}
			else if (!SoundEvent.PlayOneShot(sound, behaviour, this.noiseValues, SoundEvent.GetVolume(this.objectIsSelectedAndVisible), this.objectIsSelectedAndVisible))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", sound, behaviour.name)
				});
			}
		}
		catch (Exception ex)
		{
			string text = string.Format(("Error trying to trigger sound [{0}] in behaviour [{1}] [{2}]\n{3}" + sound != null) ? sound.ToString() : "null", behaviour.GetType().ToString(), ex.Message, ex.StackTrace);
			global::Debug.LogError(text);
			throw new ArgumentException(text, ex);
		}
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x000BBAFC File Offset: 0x000B9CFC
	public virtual void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour, this.sound);
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x001DB604 File Offset: 0x001D9804
	public static Vector3 GetCameraScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		Vector3 result = Vector3.zero;
		if (CameraController.Instance != null)
		{
			result = CameraController.Instance.GetVerticallyScaledPosition(pos, objectIsSelectedAndVisible);
		}
		return result;
	}

	// Token: 0x06002AA3 RID: 10915 RVA: 0x000BBB0B File Offset: 0x000B9D0B
	public static FMOD.Studio.EventInstance BeginOneShot(EventReference event_ref, Vector3 pos, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		return KFMOD.BeginOneShot(event_ref, SoundEvent.GetCameraScaledPosition(pos, objectIsSelectedAndVisible), volume);
	}

	// Token: 0x06002AA4 RID: 10916 RVA: 0x000BBB1B File Offset: 0x000B9D1B
	public static FMOD.Studio.EventInstance BeginOneShot(string ev, Vector3 pos, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		return SoundEvent.BeginOneShot(RuntimeManager.PathToEventReference(ev), pos, volume, false);
	}

	// Token: 0x06002AA5 RID: 10917 RVA: 0x000BBB2B File Offset: 0x000B9D2B
	public static bool EndOneShot(FMOD.Studio.EventInstance instance)
	{
		return KFMOD.EndOneShot(instance);
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x001DB634 File Offset: 0x001D9834
	public static bool PlayOneShot(EventReference event_ref, Vector3 sound_pos, float volume = 1f)
	{
		bool result = false;
		if (!event_ref.IsNull)
		{
			FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(event_ref, sound_pos, volume, false);
			if (instance.isValid())
			{
				result = SoundEvent.EndOneShot(instance);
			}
		}
		return result;
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x000BBB33 File Offset: 0x000B9D33
	public static bool PlayOneShot(string sound, Vector3 sound_pos, float volume = 1f)
	{
		return SoundEvent.PlayOneShot(RuntimeManager.PathToEventReference(sound), sound_pos, volume);
	}

	// Token: 0x06002AA8 RID: 10920 RVA: 0x001DB668 File Offset: 0x001D9868
	public static bool PlayOneShot(string sound, AnimEventManager.EventPlayerData behaviour, EffectorValues noiseValues, float volume = 1f, bool objectIsSelectedAndVisible = false)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(sound))
		{
			Vector3 vector = behaviour.controller.transform.GetPosition();
			vector.z = 0f;
			if (objectIsSelectedAndVisible)
			{
				vector = SoundEvent.AudioHighlightListenerPosition(vector);
			}
			FMOD.Studio.EventInstance instance = SoundEvent.BeginOneShot(sound, vector, volume, false);
			if (instance.isValid())
			{
				result = SoundEvent.EndOneShot(instance);
			}
		}
		return result;
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x001D50C4 File Offset: 0x001D32C4
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (this.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(this.sound);
			}
		}
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x000BBB42 File Offset: 0x000B9D42
	protected static bool IsLowPrioritySound(string sound)
	{
		return sound != null && Camera.main != null && Camera.main.orthographicSize > AudioMixer.LOW_PRIORITY_CUTOFF_DISTANCE && !AudioMixer.instance.activeNIS && GlobalAssets.IsLowPriority(sound);
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x001DB6C4 File Offset: 0x001D98C4
	protected void PrintSoundDebug(string anim_name, string sound, string sound_name, Vector3 sound_pos)
	{
		if (sound != null)
		{
			string[] array = new string[7];
			array[0] = anim_name;
			array[1] = ", ";
			array[2] = sound_name;
			array[3] = ", ";
			array[4] = base.frame.ToString();
			array[5] = ", ";
			int num = 6;
			Vector3 vector = sound_pos;
			array[num] = vector.ToString();
			global::Debug.Log(string.Concat(array));
			return;
		}
		global::Debug.Log("Missing sound: " + anim_name + ", " + sound_name);
	}

	// Token: 0x04001C53 RID: 7251
	public static int IGNORE_INTERVAL = -1;

	// Token: 0x04001C5C RID: 7260
	protected bool isDynamic;
}
