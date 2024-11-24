using System;
using UnityEngine;

public class DreamBubble : KMonoBehaviour
{
	public KBatchedAnimController dreamBackgroundComponent;

	public KBatchedAnimController maskKanim;

	public KBatchedAnimController dreamBubbleBorderKanim;

	public KImage dreamContentComponent;

	private const string dreamBackgroundAnimationName = "dream_loop";

	private const string dreamMaskAnimationName = "dream_bubble_mask";

	private const string dreamBubbleBorderAnimationName = "dream_bubble_loop";

	private HashedString snapToPivotSymbol = new HashedString("snapto_pivot");

	private Dream _currentDream;

	private float _timePassedSinceDreamStarted;

	private Color _color = Color.white;

	private const float PI_2 = (float)Math.PI * 2f;

	private const float HALF_PI = (float)Math.PI / 2f;

	public bool IsVisible { get; private set; }

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		dreamBackgroundComponent.SetSymbolVisiblity(snapToPivotSymbol, is_visible: false);
		SetVisibility(visible: false);
	}

	public void Tick(float dt)
	{
		if (_currentDream != null && _currentDream.Icons.Length != 0)
		{
			float num = _timePassedSinceDreamStarted / _currentDream.secondPerImage;
			int num2 = Mathf.FloorToInt(num);
			float num3 = num - (float)num2;
			int num4 = (int)Mathf.Repeat(Mathf.FloorToInt(num), _currentDream.Icons.Length);
			if (dreamContentComponent.sprite != _currentDream.Icons[num4])
			{
				dreamContentComponent.sprite = _currentDream.Icons[num4];
			}
			dreamContentComponent.rectTransform.localScale = Vector3.one * num3;
			_color.a = (Mathf.Sin(num3 * ((float)Math.PI * 2f) - (float)Math.PI / 2f) + 1f) * 0.5f;
			dreamContentComponent.color = _color;
			_timePassedSinceDreamStarted += dt;
		}
	}

	public void SetDream(Dream dream)
	{
		_currentDream = dream;
		dreamBackgroundComponent.Stop();
		dreamBackgroundComponent.AnimFiles = new KAnimFile[1] { Assets.GetAnim(dream.BackgroundAnim) };
		dreamContentComponent.color = _color;
		dreamContentComponent.enabled = dream != null && dream.Icons != null && dream.Icons.Length != 0;
		_timePassedSinceDreamStarted = 0f;
		_color.a = 0f;
	}

	public void SetVisibility(bool visible)
	{
		IsVisible = visible;
		dreamBackgroundComponent.SetVisiblity(visible);
		dreamContentComponent.gameObject.SetActive(visible);
		if (visible)
		{
			if (_currentDream != null)
			{
				dreamBackgroundComponent.Play("dream_loop", KAnim.PlayMode.Loop);
			}
			dreamBubbleBorderKanim.Play("dream_bubble_loop", KAnim.PlayMode.Loop);
			maskKanim.Play("dream_bubble_mask", KAnim.PlayMode.Loop);
		}
		else
		{
			dreamBackgroundComponent.Stop();
			maskKanim.Stop();
			dreamBubbleBorderKanim.Stop();
		}
	}

	public void StopDreaming()
	{
		_currentDream = null;
		SetVisibility(visible: false);
	}
}
