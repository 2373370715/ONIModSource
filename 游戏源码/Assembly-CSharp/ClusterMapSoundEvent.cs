using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x0200090E RID: 2318
public class ClusterMapSoundEvent : SoundEvent
{
	// Token: 0x06002931 RID: 10545 RVA: 0x000BAC64 File Offset: 0x000B8E64
	public ClusterMapSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, true, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06002932 RID: 10546 RVA: 0x000BAC79 File Offset: 0x000B8E79
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		if (ClusterMapScreen.Instance.IsActive())
		{
			this.PlaySound(behaviour);
		}
	}

	// Token: 0x06002933 RID: 10547 RVA: 0x001D4FC4 File Offset: 0x001D31C4
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				global::Debug.Log(behaviour.name + " (Cluster Map Object) is missing LoopingSounds component.");
				return;
			}
			if (!component.StartSound(base.sound, true, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", base.sound, behaviour.name)
				});
				return;
			}
		}
		else
		{
			EventInstance instance = KFMOD.BeginOneShot(base.sound, Vector3.zero, 1f);
			instance.setParameterByName(ClusterMapSoundEvent.X_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().x / (float)Screen.width, false);
			instance.setParameterByName(ClusterMapSoundEvent.Y_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().y / (float)Screen.height, false);
			instance.setParameterByName(ClusterMapSoundEvent.ZOOM_PARAMETER, ClusterMapScreen.Instance.CurrentZoomPercentage(), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x06002934 RID: 10548 RVA: 0x001D50C4 File Offset: 0x001D32C4
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.sound);
			}
		}
	}

	// Token: 0x04001B80 RID: 7040
	private static string X_POSITION_PARAMETER = "Starmap_Position_X";

	// Token: 0x04001B81 RID: 7041
	private static string Y_POSITION_PARAMETER = "Starmap_Position_Y";

	// Token: 0x04001B82 RID: 7042
	private static string ZOOM_PARAMETER = "Starmap_Zoom_Percentage";
}
