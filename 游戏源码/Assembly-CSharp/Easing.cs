using System;
using UnityEngine;

public class Easing
{
	public delegate float EasingFn(float f);

	public const EasingFn PARAM_DEFAULT = null;

	public static readonly EasingFn Linear = (float x) => x;

	public static readonly EasingFn SmoothStep = (float x) => Mathf.SmoothStep(0f, 1f, x);

	public static readonly EasingFn QuadIn = (float x) => x * x;

	public static readonly EasingFn QuadOut = (float x) => 1f - (1f - x) * (1f - x);

	public static readonly EasingFn QuadInOut = (float x) => (!((double)x < 0.5)) ? (1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f) : (2f * x * x);

	public static readonly EasingFn CubicIn = (float x) => x * x * x;

	public static readonly EasingFn CubicOut = (float x) => 1f - Mathf.Pow(1f - x, 3f);

	public static readonly EasingFn CubicInOut = (float x) => (!((double)x < 0.5)) ? (1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f) : (4f * x * x * x);

	public static readonly EasingFn QuartIn = (float x) => x * x * x * x;

	public static readonly EasingFn QuartOut = (float x) => 1f - Mathf.Pow(1f - x, 4f);

	public static readonly EasingFn QuartInOut = (float x) => (!((double)x < 0.5)) ? (1f - Mathf.Pow(-2f * x + 2f, 4f) / 2f) : (8f * x * x * x * x);

	public static readonly EasingFn QuintIn = (float x) => x * x * x * x * x;

	public static readonly EasingFn QuintOut = (float x) => 1f - Mathf.Pow(1f - x, 5f);

	public static readonly EasingFn QuintInOut = (float x) => (!((double)x < 0.5)) ? (1f - Mathf.Pow(-2f * x + 2f, 5f) / 2f) : (16f * x * x * x * x * x);

	public static readonly EasingFn ExpoIn = (float x) => (x != 0f) ? Mathf.Pow(2f, 10f * x - 10f) : 0f;

	public static readonly EasingFn ExpoOut = (float x) => (x != 1f) ? (1f - Mathf.Pow(2f, -10f * x)) : 1f;

	public static readonly EasingFn ExpoInOut = (float x) => (x != 0f) ? ((x != 1f) ? ((!((double)x < 0.5)) ? ((2f - Mathf.Pow(2f, -20f * x + 10f)) / 2f) : (Mathf.Pow(2f, 20f * x - 10f) / 2f)) : 1f) : 0f;

	public static readonly EasingFn SineIn = (float x) => 1f - Mathf.Cos(x * (float)Math.PI / 2f);

	public static readonly EasingFn SineOut = (float x) => Mathf.Sin(x * (float)Math.PI / 2f);

	public static readonly EasingFn SineInOut = (float x) => (0f - (Mathf.Cos((float)Math.PI * x) - 1f)) / 2f;

	public static readonly EasingFn CircIn = (float x) => 1f - Mathf.Sqrt(1f - Mathf.Pow(x, 2f));

	public static readonly EasingFn CircOut = (float x) => Mathf.Sqrt(1f - Mathf.Pow(x - 1f, 2f));

	public static readonly EasingFn CircInOut = (float x) => (!((double)x < 0.5)) ? ((Mathf.Sqrt(1f - Mathf.Pow(-2f * x + 2f, 2f)) + 1f) / 2f) : ((1f - Mathf.Sqrt(1f - Mathf.Pow(2f * x, 2f))) / 2f);

	public static readonly EasingFn EaseOutBack = (float x) => 1f + 2.70158f * Mathf.Pow(x - 1f, 3f) + 1.70158f * Mathf.Pow(x - 1f, 2f);

	public static readonly EasingFn ElasticIn = (float x) => (x != 0f) ? ((x != 1f) ? ((0f - Mathf.Pow(2f, 10f * x - 10f)) * Mathf.Sin((x * 10f - 10.75f) * ((float)Math.PI * 2f / 3f))) : 1f) : 0f;

	public static readonly EasingFn ElasticOut = (float x) => (x != 0f) ? ((x != 1f) ? (Mathf.Pow(2f, -10f * x) * Mathf.Sin((x * 10f - 0.75f) * ((float)Math.PI * 2f / 3f)) + 1f) : 1f) : 0f;

	public static readonly EasingFn ElasticInOut = (float x) => (x != 0f) ? ((x != 1f) ? ((!((double)x < 0.5)) ? (Mathf.Pow(2f, -20f * x + 10f) * Mathf.Sin((20f * x - 11.125f) * ((float)Math.PI * 4f / 9f)) / 2f + 1f) : ((0f - Mathf.Pow(2f, 20f * x - 10f) * Mathf.Sin((20f * x - 11.125f) * ((float)Math.PI * 4f / 9f))) / 2f)) : 1f) : 0f;

	public static readonly EasingFn BackIn = (float x) => 2.70158f * x * x * x - 1.70158f * x * x;

	public static readonly EasingFn BackOut = (float x) => 1f + 2.70158f * Mathf.Pow(x - 1f, 3f) + 1.70158f * Mathf.Pow(x - 1f, 2f);

	public static readonly EasingFn BackInOut = (float x) => (!((double)x < 0.5)) ? ((Mathf.Pow(2f * x - 2f, 2f) * (3.5949094f * (x * 2f - 2f) + 2.5949094f) + 2f) / 2f) : (Mathf.Pow(2f * x, 2f) * (7.189819f * x - 2.5949094f) / 2f);

	public static readonly EasingFn BounceIn = (float x) => 1f - BounceOut(1f - x);

	public static readonly EasingFn BounceOut = delegate(float x)
	{
		if (x < 0.36363637f)
		{
			return 7.5625f * x * x;
		}
		if (x < 0.72727275f)
		{
			return 7.5625f * (x -= 0.54545456f) * x + 0.75f;
		}
		return ((double)x < 0.9090909090909091) ? (7.5625f * (x -= 0.8181818f) * x + 0.9375f) : (7.5625f * (x -= 21f / 22f) * x + 63f / 64f);
	};

	public static readonly EasingFn BounceInOut = (float x) => (!((double)x < 0.5)) ? ((1f + BounceOut(2f * x - 1f)) / 2f) : ((1f - BounceOut(1f - 2f * x)) / 2f);
}
