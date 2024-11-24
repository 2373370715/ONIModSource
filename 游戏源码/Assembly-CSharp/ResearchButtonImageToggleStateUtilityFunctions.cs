using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B3A RID: 6970
public static class ResearchButtonImageToggleStateUtilityFunctions
{
	// Token: 0x06009245 RID: 37445 RVA: 0x003864D8 File Offset: 0x003846D8
	public static void Opacity(this Graphic graphic, float opacity)
	{
		Color color = graphic.color;
		color.a = opacity;
		graphic.color = color;
	}

	// Token: 0x06009246 RID: 37446 RVA: 0x003864FC File Offset: 0x003846FC
	public static WaitUntil FadeAway(this Graphic graphic, float duration, Func<bool> assertCondition = null)
	{
		float timer = 0f;
		float startingOpacity = graphic.color.a;
		return new WaitUntil(delegate()
		{
			if (timer >= duration || (assertCondition != null && !assertCondition()))
			{
				graphic.Opacity(0f);
				return true;
			}
			float num = timer / duration;
			num = 1f - num;
			graphic.Opacity(startingOpacity * num);
			timer += Time.unscaledDeltaTime;
			return false;
		});
	}

	// Token: 0x06009247 RID: 37447 RVA: 0x00386554 File Offset: 0x00384754
	public static WaitUntil FadeToVisible(this Graphic graphic, float duration, Func<bool> assertCondition = null)
	{
		float timer = 0f;
		float startingOpacity = graphic.color.a;
		float remainingOpacity = 1f - graphic.color.a;
		return new WaitUntil(delegate()
		{
			if (timer >= duration || (assertCondition != null && !assertCondition()))
			{
				graphic.Opacity(1f);
				return true;
			}
			float num = timer / duration;
			graphic.Opacity(startingOpacity + remainingOpacity * num);
			timer += Time.unscaledDeltaTime;
			return false;
		});
	}
}
