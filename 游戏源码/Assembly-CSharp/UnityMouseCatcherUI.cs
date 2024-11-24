using UnityEngine;
using UnityEngine.UI;

public class UnityMouseCatcherUI
{
	private static Canvas m_instance_canvas;

	public static Canvas ManifestCanvas()
	{
		if (m_instance_canvas != null && (bool)m_instance_canvas)
		{
			return m_instance_canvas;
		}
		GameObject gameObject = new GameObject("UnityMouseCatcherUI Canvas");
		Object.DontDestroyOnLoad(gameObject);
		Canvas canvas = gameObject.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 32767;
		canvas.pixelPerfect = false;
		m_instance_canvas = canvas;
		gameObject.AddComponent<GraphicRaycaster>();
		GameObject gameObject2 = new GameObject("ImGui Consume Input", typeof(RectTransform));
		gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
		RectTransform component = gameObject2.GetComponent<RectTransform>();
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.sizeDelta = Vector2.zero;
		component.anchoredPosition = Vector2.zero;
		Image image = gameObject2.AddComponent<Image>();
		image.sprite = Resources.Load<Sprite>("1x1_white");
		image.color = new Color(1f, 1f, 1f, 0f);
		image.raycastTarget = true;
		return m_instance_canvas;
	}

	public static void SetEnabled(bool is_enabled)
	{
		Canvas canvas = ManifestCanvas();
		if (canvas.gameObject.activeSelf != is_enabled)
		{
			canvas.gameObject.SetActive(is_enabled);
		}
		if (canvas.enabled != is_enabled)
		{
			canvas.enabled = is_enabled;
		}
	}
}
