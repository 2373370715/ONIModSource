using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/FrontEndManager")]
public class FrontEndManager : KMonoBehaviour
{
	public static FrontEndManager Instance;

	public static bool firstInit = true;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
		GameObject parent = base.gameObject;
		string highestActiveDlcId = DlcManager.GetHighestActiveDlcId();
		if (highestActiveDlcId == null || (highestActiveDlcId != null && highestActiveDlcId.Length == 0) || !(highestActiveDlcId == "EXPANSION1_ID"))
		{
			Util.KInstantiateUI(ScreenPrefabs.Instance.MainMenuForVanilla, parent, force_active: true);
		}
		else
		{
			Util.KInstantiateUI(ScreenPrefabs.Instance.MainMenuForSpacedOut, parent, force_active: true);
		}
		GameObject[] screensPrefabsToSpawn;
		List<GameObject> gameObjectsToDestroyOnNextCreate;
		CoroutineRunner coroutineRunner;
		if (firstInit)
		{
			firstInit = false;
			GameObject[] array = new GameObject[3]
			{
				ScreenPrefabs.Instance.MainMenuIntroShort,
				ScreenPrefabs.Instance.MainMenuHealthyGameMessage,
				ScreenPrefabs.Instance.DLCBetaWarningScreen
			};
			for (int i = 0; i < array.Length; i++)
			{
				Util.KInstantiateUI(array[i], parent, force_active: true);
			}
			screensPrefabsToSpawn = new GameObject[3]
			{
				ScreenPrefabs.Instance.KleiItemDropScreen,
				ScreenPrefabs.Instance.LockerMenuScreen,
				ScreenPrefabs.Instance.LockerNavigator
			};
			gameObjectsToDestroyOnNextCreate = new List<GameObject>();
			coroutineRunner = CoroutineRunner.Create();
			UnityEngine.Object.DontDestroyOnLoad(coroutineRunner);
			CreateCanvases();
			Singleton<KBatchedAnimUpdater>.Instance.OnClear += RecreateCanvases;
		}
		void CreateCanvases()
		{
			int num = 30;
			GameObject[] array2 = screensPrefabsToSpawn;
			foreach (GameObject gameObject in array2)
			{
				GameObject gameObject2 = MakeKleiCanvas(gameObject.name + " Canvas");
				gameObject2.GetComponent<Canvas>().sortingOrder = num++;
				Util.KInstantiateUI(gameObject, gameObject2, force_active: true);
				UnityEngine.Object.DontDestroyOnLoad(gameObject2);
				gameObjectsToDestroyOnNextCreate.Add(gameObject2);
			}
		}
		void RecreateCanvases()
		{
			if (!(coroutineRunner == null) && (bool)coroutineRunner)
			{
				foreach (GameObject item in gameObjectsToDestroyOnNextCreate)
				{
					UnityEngine.Object.Destroy(item);
				}
				gameObjectsToDestroyOnNextCreate.Clear();
				coroutineRunner.StopAllCoroutines();
				coroutineRunner.Run(Updater.Series(Updater.WaitOneFrame(), Updater.Do((System.Action)CreateCanvases)));
			}
		}
	}

	protected override void OnForcedCleanUp()
	{
		Instance = null;
		base.OnForcedCleanUp();
	}

	private void LateUpdate()
	{
		if (Debug.developerConsoleVisible)
		{
			Debug.developerConsoleVisible = false;
		}
		KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
		KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
		KAnimBatchManager.Instance().Render();
	}

	public GameObject MakeKleiCanvas(string gameObjectName = "Canvas")
	{
		GameObject result = new GameObject(gameObjectName, typeof(RectTransform));
		ConfigureAsKleiCanvas(result);
		return result;
	}

	public void ConfigureAsKleiCanvas(GameObject gameObject)
	{
		Canvas canvas = gameObject.AddOrGet<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 10;
		canvas.pixelPerfect = false;
		canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.Normal | AdditionalCanvasShaderChannels.Tangent;
		GraphicRaycaster graphicRaycaster = gameObject.AddOrGet<GraphicRaycaster>();
		graphicRaycaster.ignoreReversedGraphics = true;
		graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;
		graphicRaycaster.blockingMask = -1;
		CanvasScaler canvasScaler = gameObject.AddOrGet<CanvasScaler>();
		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
		canvasScaler.referencePixelsPerUnit = 100f;
		gameObject.AddOrGet<KCanvasScaler>();
	}
}
