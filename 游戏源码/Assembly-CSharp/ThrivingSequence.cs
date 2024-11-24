using System;
using System.Collections;
using UnityEngine;

public static class ThrivingSequence
{
	public static void Start(KMonoBehaviour controller)
	{
		controller.StartCoroutine(Sequence());
	}

	private static IEnumerator Sequence()
	{
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(playSound: false);
		}
		CameraController.Instance.SetWorldInteractive(state: false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().VictoryMessageSnapshot);
		AudioMixer.instance.Start(Db.Get().ColonyAchievements.Thriving.victoryNISSnapshot);
		MusicManager.instance.PlaySong("Music_Victory_02_NIS");
		Vector3 cameraBiasUp = Vector3.up * 5f;
		GameObject cameraTaget = null;
		foreach (Telepad telepad in Components.Telepads)
		{
			if (telepad != null)
			{
				cameraTaget = telepad.gameObject;
			}
		}
		if (cameraTaget != null)
		{
			CameraController.Instance.FadeOut(1f, 2f);
			yield return SequenceUtil.WaitForSecondsRealtime(1f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 10f, playSound: false);
			CameraController.Instance.SetOverrideZoomSpeed(10f);
			yield return SequenceUtil.WaitForSecondsRealtime(0.4f);
			if (SpeedControlScreen.Instance.IsPaused)
			{
				SpeedControlScreen.Instance.Unpause(playSound: false);
			}
			SpeedControlScreen.Instance.SetSpeed(1);
			CameraController.Instance.SetOverrideZoomSpeed(0.05f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 20f, playSound: false);
			CameraController.Instance.FadeIn(0f, 2f);
			foreach (MinionIdentity liveMinionIdentity in Components.LiveMinionIdentities)
			{
				if (liveMinionIdentity != null)
				{
					liveMinionIdentity.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
					Db db = Db.Get();
					new EmoteChore(liveMinionIdentity.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 2);
				}
			}
			yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
			yield return SequenceUtil.WaitForSecondsRealtime(3f);
		}
		cameraTaget = null;
		foreach (ComplexFabricator complexFabricator in Components.ComplexFabricators)
		{
			if (complexFabricator != null)
			{
				cameraTaget = complexFabricator.gameObject;
			}
		}
		if (cameraTaget == null)
		{
			foreach (Generator generator in Components.Generators)
			{
				if (generator != null)
				{
					cameraTaget = generator.gameObject;
				}
			}
		}
		if (cameraTaget == null)
		{
			foreach (Fabricator fabricator in Components.Fabricators)
			{
				if (fabricator != null)
				{
					cameraTaget = fabricator.gameObject;
				}
			}
		}
		if (cameraTaget != null)
		{
			CameraController.Instance.FadeOut(1f, 2f);
			yield return SequenceUtil.WaitForSecondsRealtime(1f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position + cameraBiasUp, 10f, playSound: false);
			CameraController.Instance.SetOverrideZoomSpeed(10f);
			yield return SequenceUtil.WaitForSecondsRealtime(0.4f);
			CameraController.Instance.SetOverrideZoomSpeed(0.1f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position + cameraBiasUp, 20f, playSound: false);
			CameraController.Instance.FadeIn(0f, 2f);
			foreach (MinionIdentity liveMinionIdentity2 in Components.LiveMinionIdentities)
			{
				if (liveMinionIdentity2 != null)
				{
					liveMinionIdentity2.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
					Db db2 = Db.Get();
					new EmoteChore(liveMinionIdentity2.GetComponent<ChoreProvider>(), db2.ChoreTypes.EmoteHighPriority, db2.Emotes.Minion.Cheer, 2);
				}
			}
			yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
			yield return SequenceUtil.WaitForSecondsRealtime(3f);
		}
		cameraTaget = null;
		foreach (MonumentPart monumentPart in Components.MonumentParts)
		{
			if (monumentPart.IsMonumentCompleted())
			{
				cameraTaget = monumentPart.gameObject;
			}
		}
		if (cameraTaget != null)
		{
			CameraController.Instance.FadeOut(1f, 2f);
			yield return SequenceUtil.WaitForSecondsRealtime(1f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 15f, playSound: false);
			CameraController.Instance.SetOverrideZoomSpeed(10f);
			yield return SequenceUtil.WaitForSecondsRealtime(0.4f);
			CameraController.Instance.FadeIn(0f, 2f);
			foreach (MinionIdentity liveMinionIdentity3 in Components.LiveMinionIdentities)
			{
				if (liveMinionIdentity3 != null)
				{
					liveMinionIdentity3.GetComponent<Facing>().Face(cameraTaget.transform.position.x);
					Db db3 = Db.Get();
					new EmoteChore(liveMinionIdentity3.GetComponent<ChoreProvider>(), db3.ChoreTypes.EmoteHighPriority, db3.Emotes.Minion.Cheer, 2);
				}
			}
			yield return SequenceUtil.WaitForSecondsRealtime(0.5f);
			CameraController.Instance.SetOverrideZoomSpeed(0.075f);
			CameraController.Instance.SetTargetPos(cameraTaget.transform.position, 25f, playSound: false);
			yield return SequenceUtil.WaitForSecondsRealtime(5f);
		}
		CameraController.Instance.FadeOut();
		MusicManager.instance.StopSong("Music_Victory_02_NIS");
		AudioMixer.instance.Stop(Db.Get().ColonyAchievements.Thriving.victoryNISSnapshot);
		yield return SequenceUtil.WaitForSecondsRealtime(2f);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			SpeedControlScreen.Instance.Pause(playSound: false);
		}
		VideoScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
		component.PlayVideo(Assets.GetVideo(Db.Get().ColonyAchievements.Thriving.shortVideoName), unskippable: true, AudioMixerSnapshots.Get().VictoryCinematicSnapshot);
		component.QueueVictoryVideoLoop(queue: true, Db.Get().ColonyAchievements.Thriving.messageBody, Db.Get().ColonyAchievements.Thriving.Id, Db.Get().ColonyAchievements.Thriving.loopVideoName);
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
		});
	}
}
