using System;
using System.Collections;
using UnityEngine;

public static class FocusTargetSequence
{
	public struct Data
	{
		public int WorldId;

		public float OrthographicSize;

		public float TargetSize;

		public Vector3 Target;

		public EventInfoData PopupData;

		public System.Action CompleteCB;

		public Func<bool> CanCompleteCB;

		public void Clear()
		{
			PopupData = null;
			CompleteCB = null;
			CanCompleteCB = null;
		}
	}

	private static Coroutine sequenceCoroutine = null;

	private static KSelectable prevSelected = null;

	private static bool wasPaused = false;

	private static int prevSpeed = -1;

	public static void Start(MonoBehaviour coroutineRunner, Data sequenceData)
	{
		sequenceCoroutine = coroutineRunner.StartCoroutine(RunSequence(sequenceData));
	}

	public static void Cancel(MonoBehaviour coroutineRunner)
	{
		if (sequenceCoroutine != null)
		{
			coroutineRunner.StopCoroutine(sequenceCoroutine);
			sequenceCoroutine = null;
			if (prevSpeed >= 0)
			{
				SpeedControlScreen.Instance.SetSpeed(prevSpeed);
			}
			if (SpeedControlScreen.Instance.IsPaused && !wasPaused)
			{
				SpeedControlScreen.Instance.Unpause(playSound: false);
			}
			if (!SpeedControlScreen.Instance.IsPaused && wasPaused)
			{
				SpeedControlScreen.Instance.Pause(playSound: false);
			}
			SetUIVisible(visible: true);
			CameraController.Instance.SetWorldInteractive(state: true);
			SelectTool.Instance.Select(prevSelected, skipSound: true);
			prevSelected = null;
			wasPaused = false;
			prevSpeed = -1;
		}
	}

	public static IEnumerator RunSequence(Data sequenceData)
	{
		SaveGame.Instance.GetComponent<UserNavigation>();
		CameraController.Instance.FadeOut();
		prevSpeed = SpeedControlScreen.Instance.GetSpeed();
		SpeedControlScreen.Instance.SetSpeed(0);
		wasPaused = SpeedControlScreen.Instance.IsPaused;
		if (!wasPaused)
		{
			SpeedControlScreen.Instance.Pause(playSound: false);
		}
		PlayerController.Instance.CancelDragging();
		CameraController.Instance.SetWorldInteractive(state: false);
		yield return CameraController.Instance.activeFadeRoutine;
		prevSelected = SelectTool.Instance.selected;
		SelectTool.Instance.Select(null, skipSound: true);
		SetUIVisible(visible: false);
		ClusterManager.Instance.SetActiveWorld(sequenceData.WorldId);
		ManagementMenu.Instance.CloseAll();
		CameraController.Instance.SnapTo(sequenceData.Target, sequenceData.OrthographicSize);
		if (sequenceData.PopupData != null)
		{
			EventInfoScreen.ShowPopup(sequenceData.PopupData);
		}
		CameraController.Instance.FadeIn(0f, 2f);
		if (sequenceData.TargetSize - sequenceData.OrthographicSize > Mathf.Epsilon)
		{
			CameraController.Instance.StartCoroutine(CameraController.Instance.DoCinematicZoom(sequenceData.TargetSize));
		}
		if (sequenceData.CanCompleteCB != null)
		{
			SpeedControlScreen.Instance.Unpause(playSound: false);
			while (!sequenceData.CanCompleteCB())
			{
				yield return SequenceUtil.WaitForNextFrame;
			}
			SpeedControlScreen.Instance.Pause(playSound: false);
		}
		CameraController.Instance.SetWorldInteractive(state: true);
		SpeedControlScreen.Instance.SetSpeed(prevSpeed);
		if (SpeedControlScreen.Instance.IsPaused && !wasPaused)
		{
			SpeedControlScreen.Instance.Unpause(playSound: false);
		}
		if (sequenceData.CompleteCB != null)
		{
			sequenceData.CompleteCB();
		}
		SetUIVisible(visible: true);
		SelectTool.Instance.Select(prevSelected, skipSound: true);
		sequenceData.Clear();
		sequenceCoroutine = null;
		prevSpeed = -1;
		wasPaused = false;
		prevSelected = null;
	}

	private static void SetUIVisible(bool visible)
	{
		NotificationScreen.Instance.Show(visible);
		OverlayMenu.Instance.Show(visible);
		ManagementMenu.Instance.Show(visible);
		ToolMenu.Instance.Show(visible);
		ToolMenu.Instance.PriorityScreen.Show(visible);
		PinnedResourcesPanel.Instance.Show(visible);
		TopLeftControlScreen.Instance.Show(visible);
		DateTime.Instance.Show(visible);
		BuildWatermark.Instance.Show(visible);
		BuildWatermark.Instance.Show(visible);
		ColonyDiagnosticScreen.Instance.Show(visible);
		RootMenu.Instance.Show(visible);
		if (PlanScreen.Instance != null)
		{
			PlanScreen.Instance.Show(visible);
		}
		if (BuildMenu.Instance != null)
		{
			BuildMenu.Instance.Show(visible);
		}
		if (WorldSelector.Instance != null)
		{
			WorldSelector.Instance.Show(visible);
		}
	}
}
