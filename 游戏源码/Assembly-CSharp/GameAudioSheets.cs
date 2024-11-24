using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000962 RID: 2402
public class GameAudioSheets : AudioSheets
{
	// Token: 0x06002B48 RID: 11080 RVA: 0x000BC229 File Offset: 0x000BA429
	public static GameAudioSheets Get()
	{
		if (GameAudioSheets._Instance == null)
		{
			GameAudioSheets._Instance = Resources.Load<GameAudioSheets>("GameAudioSheets");
		}
		return GameAudioSheets._Instance;
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x001DEDD0 File Offset: 0x001DCFD0
	public override void Initialize()
	{
		this.validFileNames.Add("game_triggered");
		foreach (KAnimFile kanimFile in Assets.instance.AnimAssets)
		{
			if (!(kanimFile == null))
			{
				this.validFileNames.Add(kanimFile.name);
			}
		}
		base.Initialize();
		foreach (AudioSheet audioSheet in this.sheets)
		{
			foreach (AudioSheet.SoundInfo soundInfo in audioSheet.soundInfos)
			{
				if (soundInfo.Type == "MouthFlapSoundEvent" || soundInfo.Type == "VoiceSoundEvent")
				{
					HashSet<HashedString> hashSet = null;
					if (!this.animsNotAllowedToPlaySpeech.TryGetValue(soundInfo.File, out hashSet))
					{
						hashSet = new HashSet<HashedString>();
						this.animsNotAllowedToPlaySpeech[soundInfo.File] = hashSet;
					}
					hashSet.Add(soundInfo.Anim);
				}
			}
		}
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x001DEF38 File Offset: 0x001DD138
	protected override AnimEvent CreateSoundOfType(string type, string file_name, string sound_name, int frame, float min_interval, string dlcId)
	{
		SoundEvent soundEvent = null;
		bool shouldCameraScalePosition = true;
		if (sound_name.Contains(":disable_camera_position_scaling"))
		{
			sound_name = sound_name.Replace(":disable_camera_position_scaling", "");
			shouldCameraScalePosition = false;
		}
		if (type == "FloorSoundEvent")
		{
			soundEvent = new FloorSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "SoundEvent" || type == "LoopingSoundEvent")
		{
			bool is_looping = type == "LoopingSoundEvent";
			string[] array = sound_name.Split(':', StringSplitOptions.None);
			sound_name = array[0];
			soundEvent = new SoundEvent(file_name, sound_name, frame, true, is_looping, min_interval, false);
			for (int i = 1; i < array.Length; i++)
			{
				if (array[i] == "IGNORE_PAUSE")
				{
					soundEvent.ignorePause = true;
				}
				else
				{
					global::Debug.LogWarning(sound_name + " has unknown parameter " + array[i]);
				}
			}
		}
		else if (type == "LadderSoundEvent")
		{
			soundEvent = new LadderSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "LaserSoundEvent")
		{
			soundEvent = new LaserSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "HatchDrillSoundEvent")
		{
			soundEvent = new HatchDrillSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "CreatureChewSoundEvent")
		{
			soundEvent = new CreatureChewSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "BuildingDamageSoundEvent")
		{
			soundEvent = new BuildingDamageSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "WallDamageSoundEvent")
		{
			soundEvent = new WallDamageSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "RemoteSoundEvent")
		{
			soundEvent = new RemoteSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "VoiceSoundEvent" || type == "LoopingVoiceSoundEvent")
		{
			soundEvent = new VoiceSoundEvent(file_name, sound_name, frame, type == "LoopingVoiceSoundEvent");
		}
		else if (type == "MouthFlapSoundEvent")
		{
			soundEvent = new MouthFlapSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "MainMenuSoundEvent")
		{
			soundEvent = new MainMenuSoundEvent(file_name, sound_name, frame);
		}
		else if (type == "ClusterMapSoundEvent")
		{
			soundEvent = new ClusterMapSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "ClusterMapLoopingSoundEvent")
		{
			soundEvent = new ClusterMapSoundEvent(file_name, sound_name, frame, true);
		}
		else if (type == "UIAnimationSoundEvent")
		{
			soundEvent = new UIAnimationSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "UIAnimationVoiceSoundEvent")
		{
			soundEvent = new UIAnimationVoiceSoundEvent(file_name, sound_name, frame, false);
		}
		else if (type == "UIAnimationLoopingSoundEvent")
		{
			soundEvent = new UIAnimationSoundEvent(file_name, sound_name, frame, true);
		}
		else if (type == "CreatureVariationSoundEvent")
		{
			soundEvent = new CreatureVariationSoundEvent(file_name, sound_name, frame, true, type == "LoopingSoundEvent", min_interval, false);
		}
		else if (type == "CountedSoundEvent")
		{
			soundEvent = new CountedSoundEvent(file_name, sound_name, frame, true, false, min_interval, false);
		}
		else if (type == "SculptingSoundEvent")
		{
			soundEvent = new SculptingSoundEvent(file_name, sound_name, frame, true, false, min_interval, false);
		}
		else if (type == "PhonoboxSoundEvent")
		{
			soundEvent = new PhonoboxSoundEvent(file_name, sound_name, frame, min_interval);
		}
		else if (type == "PlantMutationSoundEvent")
		{
			soundEvent = new PlantMutationSoundEvent(file_name, sound_name, frame, min_interval);
		}
		if (soundEvent != null)
		{
			soundEvent.shouldCameraScalePosition = shouldCameraScalePosition;
		}
		return soundEvent;
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x001DF280 File Offset: 0x001DD480
	public bool IsAnimAllowedToPlaySpeech(KAnim.Anim anim)
	{
		HashSet<HashedString> hashSet = null;
		return !this.animsNotAllowedToPlaySpeech.TryGetValue(anim.animFile.name, out hashSet) || !hashSet.Contains(anim.hash);
	}

	// Token: 0x04001D1B RID: 7451
	private static GameAudioSheets _Instance;

	// Token: 0x04001D1C RID: 7452
	private HashSet<HashedString> validFileNames = new HashSet<HashedString>();

	// Token: 0x04001D1D RID: 7453
	private Dictionary<HashedString, HashSet<HashedString>> animsNotAllowedToPlaySpeech = new Dictionary<HashedString, HashSet<HashedString>>();

	// Token: 0x02000963 RID: 2403
	private class SingleAudioSheetLoader : AsyncLoader
	{
		// Token: 0x06002B4D RID: 11085 RVA: 0x000BC26A File Offset: 0x000BA46A
		public override void Run()
		{
			this.sheet.soundInfos = new ResourceLoader<AudioSheet.SoundInfo>(this.text, this.name).resources.ToArray();
		}

		// Token: 0x04001D1E RID: 7454
		public AudioSheet sheet;

		// Token: 0x04001D1F RID: 7455
		public string text;

		// Token: 0x04001D20 RID: 7456
		public string name;
	}

	// Token: 0x02000964 RID: 2404
	private class GameAudioSheetLoader : GlobalAsyncLoader<GameAudioSheets.GameAudioSheetLoader>
	{
		// Token: 0x06002B4F RID: 11087 RVA: 0x001DF2C0 File Offset: 0x001DD4C0
		public override void CollectLoaders(List<AsyncLoader> loaders)
		{
			foreach (AudioSheet audioSheet in GameAudioSheets.Get().sheets)
			{
				loaders.Add(new GameAudioSheets.SingleAudioSheetLoader
				{
					sheet = audioSheet,
					text = audioSheet.asset.text,
					name = audioSheet.asset.name
				});
			}
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x000A5E40 File Offset: 0x000A4040
		public override void Run()
		{
		}
	}
}
