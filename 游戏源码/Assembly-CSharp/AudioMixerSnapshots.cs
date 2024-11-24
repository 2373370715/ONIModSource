using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

// Token: 0x02001A8E RID: 6798
public class AudioMixerSnapshots : ScriptableObject
{
	// Token: 0x06008E12 RID: 36370 RVA: 0x0036FBD4 File Offset: 0x0036DDD4
	[ContextMenu("Reload")]
	public void ReloadSnapshots()
	{
		this.snapshotMap.Clear();
		EventReference[] array = this.snapshots;
		for (int i = 0; i < array.Length; i++)
		{
			string eventReferencePath = KFMOD.GetEventReferencePath(array[i]);
			if (!eventReferencePath.IsNullOrWhiteSpace())
			{
				this.snapshotMap.Add(eventReferencePath);
			}
		}
	}

	// Token: 0x06008E13 RID: 36371 RVA: 0x000FCC8A File Offset: 0x000FAE8A
	public static AudioMixerSnapshots Get()
	{
		if (AudioMixerSnapshots.instance == null)
		{
			AudioMixerSnapshots.instance = Resources.Load<AudioMixerSnapshots>("AudioMixerSnapshots");
		}
		return AudioMixerSnapshots.instance;
	}

	// Token: 0x04006AC8 RID: 27336
	public EventReference TechFilterOnMigrated;

	// Token: 0x04006AC9 RID: 27337
	public EventReference TechFilterLogicOn;

	// Token: 0x04006ACA RID: 27338
	public EventReference NightStartedMigrated;

	// Token: 0x04006ACB RID: 27339
	public EventReference MenuOpenMigrated;

	// Token: 0x04006ACC RID: 27340
	public EventReference MenuOpenHalfEffect;

	// Token: 0x04006ACD RID: 27341
	public EventReference SpeedPausedMigrated;

	// Token: 0x04006ACE RID: 27342
	public EventReference DuplicantCountAttenuatorMigrated;

	// Token: 0x04006ACF RID: 27343
	public EventReference NewBaseSetupSnapshot;

	// Token: 0x04006AD0 RID: 27344
	public EventReference FrontEndSnapshot;

	// Token: 0x04006AD1 RID: 27345
	public EventReference FrontEndWelcomeScreenSnapshot;

	// Token: 0x04006AD2 RID: 27346
	public EventReference FrontEndWorldGenerationSnapshot;

	// Token: 0x04006AD3 RID: 27347
	public EventReference IntroNIS;

	// Token: 0x04006AD4 RID: 27348
	public EventReference PulseSnapshot;

	// Token: 0x04006AD5 RID: 27349
	public EventReference ESCPauseSnapshot;

	// Token: 0x04006AD6 RID: 27350
	public EventReference MENUNewDuplicantSnapshot;

	// Token: 0x04006AD7 RID: 27351
	public EventReference UserVolumeSettingsSnapshot;

	// Token: 0x04006AD8 RID: 27352
	public EventReference DuplicantCountMovingSnapshot;

	// Token: 0x04006AD9 RID: 27353
	public EventReference DuplicantCountSleepingSnapshot;

	// Token: 0x04006ADA RID: 27354
	public EventReference PortalLPDimmedSnapshot;

	// Token: 0x04006ADB RID: 27355
	public EventReference DynamicMusicPlayingSnapshot;

	// Token: 0x04006ADC RID: 27356
	public EventReference FabricatorSideScreenOpenSnapshot;

	// Token: 0x04006ADD RID: 27357
	public EventReference SpaceVisibleSnapshot;

	// Token: 0x04006ADE RID: 27358
	public EventReference MENUStarmapSnapshot;

	// Token: 0x04006ADF RID: 27359
	public EventReference MENUStarmapNotPausedSnapshot;

	// Token: 0x04006AE0 RID: 27360
	public EventReference GameNotFocusedSnapshot;

	// Token: 0x04006AE1 RID: 27361
	public EventReference FacilityVisibleSnapshot;

	// Token: 0x04006AE2 RID: 27362
	public EventReference TutorialVideoPlayingSnapshot;

	// Token: 0x04006AE3 RID: 27363
	public EventReference VictoryMessageSnapshot;

	// Token: 0x04006AE4 RID: 27364
	public EventReference VictoryNISGenericSnapshot;

	// Token: 0x04006AE5 RID: 27365
	public EventReference VictoryNISRocketSnapshot;

	// Token: 0x04006AE6 RID: 27366
	public EventReference VictoryCinematicSnapshot;

	// Token: 0x04006AE7 RID: 27367
	public EventReference VictoryFadeToBlackSnapshot;

	// Token: 0x04006AE8 RID: 27368
	public EventReference MuteDynamicMusicSnapshot;

	// Token: 0x04006AE9 RID: 27369
	public EventReference ActiveBaseChangeSnapshot;

	// Token: 0x04006AEA RID: 27370
	public EventReference EventPopupSnapshot;

	// Token: 0x04006AEB RID: 27371
	public EventReference SmallRocketInteriorReverbSnapshot;

	// Token: 0x04006AEC RID: 27372
	public EventReference MediumRocketInteriorReverbSnapshot;

	// Token: 0x04006AED RID: 27373
	public EventReference MainMenuVideoPlayingSnapshot;

	// Token: 0x04006AEE RID: 27374
	public EventReference TechFilterRadiationOn;

	// Token: 0x04006AEF RID: 27375
	public EventReference FrontEndSupplyClosetSnapshot;

	// Token: 0x04006AF0 RID: 27376
	public EventReference FrontEndItemDropScreenSnapshot;

	// Token: 0x04006AF1 RID: 27377
	[SerializeField]
	private EventReference[] snapshots;

	// Token: 0x04006AF2 RID: 27378
	[NonSerialized]
	public List<string> snapshotMap = new List<string>();

	// Token: 0x04006AF3 RID: 27379
	private static AudioMixerSnapshots instance;
}
