using UnityEngine;

public class PrefabDefinedUIPosition
{
	private Option<Vector2> position;

	public void SetOn(GameObject gameObject)
	{
		if (position.HasValue)
		{
			gameObject.rectTransform().anchoredPosition = position.Value;
		}
		else
		{
			position = gameObject.rectTransform().anchoredPosition;
		}
	}

	public void SetOn(Component component)
	{
		if (position.HasValue)
		{
			component.rectTransform().anchoredPosition = position.Value;
		}
		else
		{
			position = component.rectTransform().anchoredPosition;
		}
	}
}
