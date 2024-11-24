using System;
using UnityEngine;

// Token: 0x0200124B RID: 4683
public class DreamBubble : KMonoBehaviour
{
	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x06005FED RID: 24557 RVA: 0x000DE80E File Offset: 0x000DCA0E
	// (set) Token: 0x06005FEC RID: 24556 RVA: 0x000DE805 File Offset: 0x000DCA05
	public bool IsVisible { get; private set; }

	// Token: 0x06005FEE RID: 24558 RVA: 0x000DE816 File Offset: 0x000DCA16
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.dreamBackgroundComponent.SetSymbolVisiblity(this.snapToPivotSymbol, false);
		this.SetVisibility(false);
	}

	// Token: 0x06005FEF RID: 24559 RVA: 0x002AC0C4 File Offset: 0x002AA2C4
	public void Tick(float dt)
	{
		if (this._currentDream != null && this._currentDream.Icons.Length != 0)
		{
			float num = this._timePassedSinceDreamStarted / this._currentDream.secondPerImage;
			int num2 = Mathf.FloorToInt(num);
			float num3 = num - (float)num2;
			int num4 = (int)Mathf.Repeat((float)Mathf.FloorToInt(num), (float)this._currentDream.Icons.Length);
			if (this.dreamContentComponent.sprite != this._currentDream.Icons[num4])
			{
				this.dreamContentComponent.sprite = this._currentDream.Icons[num4];
			}
			this.dreamContentComponent.rectTransform.localScale = Vector3.one * num3;
			this._color.a = (Mathf.Sin(num3 * 6.2831855f - 1.5707964f) + 1f) * 0.5f;
			this.dreamContentComponent.color = this._color;
			this._timePassedSinceDreamStarted += dt;
		}
	}

	// Token: 0x06005FF0 RID: 24560 RVA: 0x002AC1C0 File Offset: 0x002AA3C0
	public void SetDream(Dream dream)
	{
		this._currentDream = dream;
		this.dreamBackgroundComponent.Stop();
		this.dreamBackgroundComponent.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim(dream.BackgroundAnim)
		};
		this.dreamContentComponent.color = this._color;
		this.dreamContentComponent.enabled = (dream != null && dream.Icons != null && dream.Icons.Length != 0);
		this._timePassedSinceDreamStarted = 0f;
		this._color.a = 0f;
	}

	// Token: 0x06005FF1 RID: 24561 RVA: 0x002AC254 File Offset: 0x002AA454
	public void SetVisibility(bool visible)
	{
		this.IsVisible = visible;
		this.dreamBackgroundComponent.SetVisiblity(visible);
		this.dreamContentComponent.gameObject.SetActive(visible);
		if (visible)
		{
			if (this._currentDream != null)
			{
				this.dreamBackgroundComponent.Play("dream_loop", KAnim.PlayMode.Loop, 1f, 0f);
			}
			this.dreamBubbleBorderKanim.Play("dream_bubble_loop", KAnim.PlayMode.Loop, 1f, 0f);
			this.maskKanim.Play("dream_bubble_mask", KAnim.PlayMode.Loop, 1f, 0f);
			return;
		}
		this.dreamBackgroundComponent.Stop();
		this.maskKanim.Stop();
		this.dreamBubbleBorderKanim.Stop();
	}

	// Token: 0x06005FF2 RID: 24562 RVA: 0x000DE83C File Offset: 0x000DCA3C
	public void StopDreaming()
	{
		this._currentDream = null;
		this.SetVisibility(false);
	}

	// Token: 0x04004405 RID: 17413
	public KBatchedAnimController dreamBackgroundComponent;

	// Token: 0x04004406 RID: 17414
	public KBatchedAnimController maskKanim;

	// Token: 0x04004407 RID: 17415
	public KBatchedAnimController dreamBubbleBorderKanim;

	// Token: 0x04004408 RID: 17416
	public KImage dreamContentComponent;

	// Token: 0x04004409 RID: 17417
	private const string dreamBackgroundAnimationName = "dream_loop";

	// Token: 0x0400440A RID: 17418
	private const string dreamMaskAnimationName = "dream_bubble_mask";

	// Token: 0x0400440B RID: 17419
	private const string dreamBubbleBorderAnimationName = "dream_bubble_loop";

	// Token: 0x0400440C RID: 17420
	private HashedString snapToPivotSymbol = new HashedString("snapto_pivot");

	// Token: 0x0400440E RID: 17422
	private Dream _currentDream;

	// Token: 0x0400440F RID: 17423
	private float _timePassedSinceDreamStarted;

	// Token: 0x04004410 RID: 17424
	private Color _color = Color.white;

	// Token: 0x04004411 RID: 17425
	private const float PI_2 = 6.2831855f;

	// Token: 0x04004412 RID: 17426
	private const float HALF_PI = 1.5707964f;
}
