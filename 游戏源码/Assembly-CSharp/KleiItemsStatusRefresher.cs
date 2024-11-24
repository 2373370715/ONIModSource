using System;
using System.Collections.Generic;
using UnityEngine;

public static class KleiItemsStatusRefresher
{
	public class UIListener : MonoBehaviour
	{
		private System.Action refreshUIFn;

		public void Internal_RefreshUI()
		{
			if (refreshUIFn != null)
			{
				refreshUIFn();
			}
		}

		public void OnRefreshUI(System.Action fn)
		{
			refreshUIFn = fn;
		}

		private void OnEnable()
		{
			listeners.Add(this);
		}

		private void OnDisable()
		{
			listeners.Remove(this);
		}
	}

	public static HashSet<UIListener> listeners = new HashSet<UIListener>();

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Initialize()
	{
		KleiItems.AddInventoryRefreshCallback(OnRefreshResponseFromServer);
	}

	private static void OnRefreshResponseFromServer()
	{
		foreach (UIListener listener in listeners)
		{
			listener.Internal_RefreshUI();
		}
	}

	public static void Refresh()
	{
		foreach (UIListener listener in listeners)
		{
			listener.Internal_RefreshUI();
		}
	}

	public static UIListener AddOrGetListener(Component component)
	{
		return AddOrGetListener(component.gameObject);
	}

	public static UIListener AddOrGetListener(GameObject onGameObject)
	{
		return onGameObject.AddOrGet<UIListener>();
	}
}
