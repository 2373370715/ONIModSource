using UnityEngine;

public static class PresUtil
{
	public static Promise MoveAndFade(RectTransform rect, Vector2 targetAnchoredPosition, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		CanvasGroup canvasGroup = rect.FindOrAddComponent<CanvasGroup>();
		return rect.FindOrAddComponent<CoroutineRunner>().Run(Updater.Parallel(Updater.Ease(delegate(float f)
		{
			canvasGroup.alpha = f;
		}, canvasGroup.alpha, targetAlpha, duration, easing), Updater.Ease(delegate(Vector2 v2)
		{
			rect.anchoredPosition = v2;
		}, rect.anchoredPosition, targetAnchoredPosition, duration, easing)));
	}

	public static Promise OffsetFromAndFade(RectTransform rect, Vector2 offset, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		Vector2 anchoredPosition = rect.anchoredPosition;
		return MoveAndFade(rect, offset + anchoredPosition, targetAlpha, duration, easing);
	}

	public static Promise OffsetToAndFade(RectTransform rect, Vector2 offset, float targetAlpha, float duration, Easing.EasingFn easing = null)
	{
		Vector2 anchoredPosition = rect.anchoredPosition;
		rect.anchoredPosition = offset + anchoredPosition;
		return MoveAndFade(rect, anchoredPosition, targetAlpha, duration, easing);
	}
}
