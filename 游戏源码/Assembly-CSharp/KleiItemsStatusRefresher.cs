using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001D4F RID: 7503
public static class KleiItemsStatusRefresher
{
	// Token: 0x06009CB4 RID: 40116 RVA: 0x00105F76 File Offset: 0x00104176
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Initialize()
	{
		KleiItems.AddInventoryRefreshCallback(new KleiItems.InventoryRefreshCallback(KleiItemsStatusRefresher.OnRefreshResponseFromServer));
	}

	// Token: 0x06009CB5 RID: 40117 RVA: 0x003C525C File Offset: 0x003C345C
	private static void OnRefreshResponseFromServer()
	{
		foreach (KleiItemsStatusRefresher.UIListener uilistener in KleiItemsStatusRefresher.listeners)
		{
			uilistener.Internal_RefreshUI();
		}
	}

	// Token: 0x06009CB6 RID: 40118 RVA: 0x003C525C File Offset: 0x003C345C
	public static void Refresh()
	{
		foreach (KleiItemsStatusRefresher.UIListener uilistener in KleiItemsStatusRefresher.listeners)
		{
			uilistener.Internal_RefreshUI();
		}
	}

	// Token: 0x06009CB7 RID: 40119 RVA: 0x00105F89 File Offset: 0x00104189
	public static KleiItemsStatusRefresher.UIListener AddOrGetListener(Component component)
	{
		return KleiItemsStatusRefresher.AddOrGetListener(component.gameObject);
	}

	// Token: 0x06009CB8 RID: 40120 RVA: 0x00105F96 File Offset: 0x00104196
	public static KleiItemsStatusRefresher.UIListener AddOrGetListener(GameObject onGameObject)
	{
		return onGameObject.AddOrGet<KleiItemsStatusRefresher.UIListener>();
	}

	// Token: 0x04007ADE RID: 31454
	public static HashSet<KleiItemsStatusRefresher.UIListener> listeners = new HashSet<KleiItemsStatusRefresher.UIListener>();

	// Token: 0x02001D50 RID: 7504
	public class UIListener : MonoBehaviour
	{
		// Token: 0x06009CBA RID: 40122 RVA: 0x00105FAA File Offset: 0x001041AA
		public void Internal_RefreshUI()
		{
			if (this.refreshUIFn != null)
			{
				this.refreshUIFn();
			}
		}

		// Token: 0x06009CBB RID: 40123 RVA: 0x00105FBF File Offset: 0x001041BF
		public void OnRefreshUI(System.Action fn)
		{
			this.refreshUIFn = fn;
		}

		// Token: 0x06009CBC RID: 40124 RVA: 0x00105FC8 File Offset: 0x001041C8
		private void OnEnable()
		{
			KleiItemsStatusRefresher.listeners.Add(this);
		}

		// Token: 0x06009CBD RID: 40125 RVA: 0x00105FD6 File Offset: 0x001041D6
		private void OnDisable()
		{
			KleiItemsStatusRefresher.listeners.Remove(this);
		}

		// Token: 0x04007ADF RID: 31455
		private System.Action refreshUIFn;
	}
}
