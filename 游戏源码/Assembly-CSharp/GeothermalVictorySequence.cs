using System;
using System.Collections;
using UnityEngine;

public static class GeothermalVictorySequence
{
	public static GeothermalVent VictoryVent;

	public static void Start(KMonoBehaviour controller)
	{
		controller.StartCoroutine(Sequence());
	}

	private static IEnumerator Sequence()
	{
		if (VictoryVent == null)
		{
			DebugUtil.DevLogError("No vent set, something went wrong");
			yield break;
		}
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(playSound: false);
		}
		CameraController.Instance.SetWorldInteractive(state: false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
		AudioMixer.instance.Start(Db.Get().ColonyAchievements.ActivateGeothermalPlant.victoryNISSnapshot);
		MusicManager.instance.PlaySong("Music_Victory_02_NIS");
		_ = Vector3.up * 5f;
		CameraController.Instance.FadeOut();
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		Vector3 ventTargetPositon = VictoryVent.transform.position + Vector3.up * 3f;
		CameraController.Instance.SetTargetPos(ventTargetPositon, 10f, playSound: false);
		CameraController.Instance.SetOverrideZoomSpeed(10f);
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		CameraController.Instance.FadeIn();
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(playSound: false);
		}
		SpeedControlScreen.Instance.SetSpeed(0);
		CameraController.Instance.SetOverrideZoomSpeed(0.05f);
		CameraController.Instance.SetTargetPos(ventTargetPositon, 20f, playSound: false);
		VictoryVent.SpawnKeepsake();
		yield return SequenceUtil.WaitForSecondsRealtime(4f);
		foreach (GeothermalVent.ElementInfo item in GeothermalControllerConfig.GetClearingEntombedVentReward())
		{
			VictoryVent.addMaterial(item);
		}
		yield return SequenceUtil.WaitForSecondsRealtime(5f);
		CameraController.Instance.FadeOut();
		yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
		MusicManager.instance.StopSong("Music_Victory_02_NIS");
		AudioMixer.instance.Stop(Db.Get().ColonyAchievements.ActivateGeothermalPlant.victoryNISSnapshot);
		yield return SequenceUtil.WaitForSecondsRealtime(2f);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(playSound: false);
		}
		VideoScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
		component.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.ActivateGeothermalPlant.shortVideoName), unskippable: true, AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		component.QueueVictoryVideoLoop(queue: true, Db.Get().ColonyAchievements.ActivateGeothermalPlant.messageBody, Db.Get().ColonyAchievements.ActivateGeothermalPlant.Id, Db.Get().ColonyAchievements.ActivateGeothermalPlant.loopVideoName);
		component.OnStop = (System.Action)Delegate.Combine(component.OnStop, (System.Action)delegate
		{
			StoryMessageScreen.HideInterface(hide: false);
			CameraController.Instance.FadeIn();
			CameraController.Instance.SetWorldInteractive(state: true);
			CameraController.Instance.SetOverrideZoomSpeed(1f);
			HoverTextScreen.Instance.Show();
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
			RootMenu.Instance.canTogglePauseScreen = true;
			Game.Instance.unlocks.Unlock("notes_earthquake");
		});
		VictoryVent = null;
	}
}
