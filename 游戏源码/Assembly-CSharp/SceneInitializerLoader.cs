using System;
using UnityEngine;

// Token: 0x02001832 RID: 6194
public class SceneInitializerLoader : MonoBehaviour
{
	// Token: 0x06007FF7 RID: 32759 RVA: 0x003328F8 File Offset: 0x00330AF8
	private void Awake()
	{
		Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		KMonoBehaviour.isLoadingScene = false;
		Singleton<StateMachineManager>.Instance.Clear();
		Util.KInstantiate(this.sceneInitializer, null, null);
		if (SceneInitializerLoader.ReportDeferredError != null && SceneInitializerLoader.deferred_error.IsValid)
		{
			SceneInitializerLoader.ReportDeferredError(SceneInitializerLoader.deferred_error);
			SceneInitializerLoader.deferred_error = default(SceneInitializerLoader.DeferredError);
		}
	}

	// Token: 0x040060F8 RID: 24824
	public SceneInitializer sceneInitializer;

	// Token: 0x040060F9 RID: 24825
	public static SceneInitializerLoader.DeferredError deferred_error;

	// Token: 0x040060FA RID: 24826
	public static SceneInitializerLoader.DeferredErrorDelegate ReportDeferredError;

	// Token: 0x02001833 RID: 6195
	public struct DeferredError
	{
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06007FF9 RID: 32761 RVA: 0x000F43E2 File Offset: 0x000F25E2
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.msg);
			}
		}

		// Token: 0x040060FB RID: 24827
		public string msg;

		// Token: 0x040060FC RID: 24828
		public string stack_trace;
	}

	// Token: 0x02001834 RID: 6196
	// (Invoke) Token: 0x06007FFB RID: 32763
	public delegate void DeferredErrorDelegate(SceneInitializerLoader.DeferredError deferred_error);
}
