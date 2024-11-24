using System;
using System.IO;
using ProcGenGame;
using UnityEngine;

// Token: 0x0200206A RID: 8298
public class WorldGenScreen : NewGameFlowScreen
{
	// Token: 0x0600B08E RID: 45198 RVA: 0x00112CCB File Offset: 0x00110ECB
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		WorldGenScreen.Instance = this;
	}

	// Token: 0x0600B08F RID: 45199 RVA: 0x00112CD9 File Offset: 0x00110ED9
	protected override void OnForcedCleanUp()
	{
		WorldGenScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600B090 RID: 45200 RVA: 0x0042559C File Offset: 0x0042379C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (MainMenu.Instance != null)
		{
			MainMenu.Instance.StopAmbience();
		}
		this.TriggerLoadingMusic();
		UnityEngine.Object.FindObjectOfType<FrontEndBackground>().gameObject.SetActive(false);
		SaveLoader.SetActiveSaveFilePath(null);
		try
		{
			if (File.Exists(WorldGen.WORLDGEN_SAVE_FILENAME))
			{
				File.Delete(WorldGen.WORLDGEN_SAVE_FILENAME);
			}
		}
		catch (Exception ex)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				ex.ToString()
			});
		}
		this.offlineWorldGen.Generate();
	}

	// Token: 0x0600B091 RID: 45201 RVA: 0x0042562C File Offset: 0x0042382C
	private void TriggerLoadingMusic()
	{
		if (AudioDebug.Get().musicEnabled && !MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MainMenu.Instance.StopMainMenuMusic();
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot);
			MusicManager.instance.PlaySong("Music_FrontEnd", false);
			MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 1f, true);
		}
	}

	// Token: 0x0600B092 RID: 45202 RVA: 0x00112CE7 File Offset: 0x00110EE7
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!e.Consumed)
		{
			e.TryConsume(global::Action.Escape);
		}
		if (!e.Consumed)
		{
			e.TryConsume(global::Action.MouseRight);
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04008B86 RID: 35718
	[MyCmpReq]
	private OfflineWorldGen offlineWorldGen;

	// Token: 0x04008B87 RID: 35719
	public static WorldGenScreen Instance;
}
