using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02001D00 RID: 7424
public class IncrementorToggle : MultiToggle
{
	// Token: 0x06009AED RID: 39661 RVA: 0x003BC854 File Offset: 0x003BAA54
	protected override void Update()
	{
		if (this.clickHeldDown)
		{
			this.totalHeldTime += Time.unscaledDeltaTime;
			if (this.timeToNextIncrement <= 0f)
			{
				this.PlayClickSound();
				this.onClick();
				this.timeToNextIncrement = Mathf.Lerp(this.timeBetweenIncrementsMax, this.timeBetweenIncrementsMin, this.totalHeldTime / 2.5f);
				return;
			}
			this.timeToNextIncrement -= Time.unscaledDeltaTime;
		}
	}

	// Token: 0x06009AEE RID: 39662 RVA: 0x003BC8D0 File Offset: 0x003BAAD0
	private void PlayClickSound()
	{
		if (this.play_sound_on_click)
		{
			if (this.states[this.state].on_click_override_sound_path == "")
			{
				KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Click", false));
				return;
			}
			KFMOD.PlayUISound(GlobalAssets.GetSound(this.states[this.state].on_click_override_sound_path, false));
		}
	}

	// Token: 0x06009AEF RID: 39663 RVA: 0x00104C5B File Offset: 0x00102E5B
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		this.timeToNextIncrement = this.timeBetweenIncrementsMax;
	}

	// Token: 0x06009AF0 RID: 39664 RVA: 0x003BC93C File Offset: 0x003BAB3C
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!this.clickHeldDown)
		{
			this.clickHeldDown = true;
			this.PlayClickSound();
			if (this.onClick != null)
			{
				this.onClick();
			}
		}
		if (this.states.Length - 1 < this.state)
		{
			global::Debug.LogWarning("Multi toggle has too few / no states");
		}
		base.RefreshHoverColor();
	}

	// Token: 0x06009AF1 RID: 39665 RVA: 0x00104C70 File Offset: 0x00102E70
	public override void OnPointerClick(PointerEventData eventData)
	{
		base.RefreshHoverColor();
	}

	// Token: 0x0400791B RID: 31003
	private float timeBetweenIncrementsMin = 0.033f;

	// Token: 0x0400791C RID: 31004
	private float timeBetweenIncrementsMax = 0.25f;

	// Token: 0x0400791D RID: 31005
	private const float incrementAccelerationScale = 2.5f;

	// Token: 0x0400791E RID: 31006
	private float timeToNextIncrement;
}
