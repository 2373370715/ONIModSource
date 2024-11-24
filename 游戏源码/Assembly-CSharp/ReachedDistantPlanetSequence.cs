using System;
using System.Collections;
using UnityEngine;

public static class ReachedDistantPlanetSequence
{
	public static void Start(KMonoBehaviour controller)
	{
		controller.StartCoroutine(Sequence());
	}

	private static IEnumerator Sequence()
	{
		Vector3 cameraTagetMid = Vector3.zero;
		Vector3 cameraTargetTop = Vector3.zero;
		Spacecraft spacecraft = null;
		foreach (Spacecraft item in SpacecraftManager.instance.GetSpacecraft())
		{
			if (item.state == Spacecraft.MissionState.Grounded || !(SpacecraftManager.instance.GetSpacecraftDestination(item.id).GetDestinationType().Id == Db.Get().SpaceDestinationTypes.Wormhole.Id))
			{
				continue;
			}
			spacecraft = item;
			foreach (RocketModule rocketModule in item.launchConditions.rocketModules)
			{
				if (rocketModule.GetComponent<RocketEngine>() != null)
				{
					cameraTagetMid = rocketModule.gameObject.transform.position + Vector3.up * 7f;
					break;
				}
			}
			cameraTargetTop = cameraTagetMid + Vector3.up * 20f;
		}
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(playSound: false);
		}
		CameraController.Instance.SetWorldInteractive(state: false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
		CameraController.Instance.FadeOut();
		yield return SequenceUtil.WaitForSecondsRealtime(3f);
		CameraController.Instance.SetTargetPos(cameraTagetMid, 15f, playSound: false);
		CameraController.Instance.SetOverrideZoomSpeed(5f);
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		AudioMixer.instance.Start(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
		CameraController.Instance.FadeIn();
		MusicManager.instance.PlaySong("Music_Victory_02_NIS");
		foreach (MinionIdentity liveMinionIdentity in Components.LiveMinionIdentities)
		{
			if (liveMinionIdentity != null)
			{
				liveMinionIdentity.GetComponent<Facing>().Face(cameraTagetMid.x);
				Db db = Db.Get();
				ChoreProvider component = liveMinionIdentity.GetComponent<ChoreProvider>();
				new EmoteChore(component, db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 2);
				new EmoteChore(component, db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 2);
				new EmoteChore(component, db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 2);
			}
		}
		yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
		if (SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Unpause(playSound: false);
		}
		SpeedControlScreen.Instance.SetSpeed(1);
		CameraController.Instance.SetOverrideZoomSpeed(0.01f);
		CameraController.Instance.SetTargetPos(cameraTargetTop, 35f, playSound: false);
		float baseZoomSpeed = 0.03f;
		for (int i = 0; i < 10; i++)
		{
			yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
			CameraController.Instance.SetOverrideZoomSpeed(baseZoomSpeed + (float)i * 0.006f);
		}
		yield return SequenceUtil.WaitForSecondsRealtime(6f);
		CameraController.Instance.FadeOut();
		MusicManager.instance.StopSong("Music_Victory_02_NIS");
		AudioMixer.instance.Stop(Db.Get().ColonyAchievements.ReachedDistantPlanet.victoryNISSnapshot);
		yield return SequenceUtil.WaitForSecondsRealtime(2f);
		spacecraft.TemporallyTear();
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(playSound: false);
		}
		VideoScreen component2 = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
		component2.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.ReachedDistantPlanet.shortVideoName), unskippable: true, AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		component2.QueueVictoryVideoLoop(queue: true, Db.Get().ColonyAchievements.ReachedDistantPlanet.messageBody, Db.Get().ColonyAchievements.ReachedDistantPlanet.Id, Db.Get().ColonyAchievements.ReachedDistantPlanet.loopVideoName);
		component2.OnStop = (System.Action)Delegate.Combine(component2.OnStop, (System.Action)delegate
		{
			StoryMessageScreen.HideInterface(hide: false);
			CameraController.Instance.FadeIn();
			CameraController.Instance.SetWorldInteractive(state: true);
			HoverTextScreen.Instance.Show();
			CameraController.Instance.SetOverrideZoomSpeed(1f);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MuteDynamicMusicSnapshot);
			RootMenu.Instance.canTogglePauseScreen = true;
		});
	}
}
