using System;
using UnityEngine;

// Token: 0x02001BFB RID: 7163
public class SizePulse : MonoBehaviour
{
	// Token: 0x060094D5 RID: 38101 RVA: 0x00396FC4 File Offset: 0x003951C4
	private void Start()
	{
		if (base.GetComponents<SizePulse>().Length > 1)
		{
			UnityEngine.Object.Destroy(this);
		}
		RectTransform rectTransform = (RectTransform)base.transform;
		this.from = rectTransform.localScale;
		this.cur = this.from;
		this.to = this.from * this.multiplier;
	}

	// Token: 0x060094D6 RID: 38102 RVA: 0x00397024 File Offset: 0x00395224
	private void Update()
	{
		float num = this.updateWhenPaused ? Time.unscaledDeltaTime : Time.deltaTime;
		num *= this.speed;
		SizePulse.State state = this.state;
		if (state != SizePulse.State.Up)
		{
			if (state == SizePulse.State.Down)
			{
				this.cur = Vector2.Lerp(this.cur, this.from, num);
				if ((this.from - this.cur).sqrMagnitude < 0.0001f)
				{
					this.cur = this.from;
					this.state = SizePulse.State.Finished;
					if (this.onComplete != null)
					{
						this.onComplete();
					}
				}
			}
		}
		else
		{
			this.cur = Vector2.Lerp(this.cur, this.to, num);
			if ((this.to - this.cur).sqrMagnitude < 0.0001f)
			{
				this.cur = this.to;
				this.state = SizePulse.State.Down;
			}
		}
		((RectTransform)base.transform).localScale = new Vector3(this.cur.x, this.cur.y, 1f);
	}

	// Token: 0x0400734E RID: 29518
	public System.Action onComplete;

	// Token: 0x0400734F RID: 29519
	public Vector2 from = Vector2.one;

	// Token: 0x04007350 RID: 29520
	public Vector2 to = Vector2.one;

	// Token: 0x04007351 RID: 29521
	public float multiplier = 1.25f;

	// Token: 0x04007352 RID: 29522
	public float speed = 1f;

	// Token: 0x04007353 RID: 29523
	public bool updateWhenPaused;

	// Token: 0x04007354 RID: 29524
	private Vector2 cur;

	// Token: 0x04007355 RID: 29525
	private SizePulse.State state;

	// Token: 0x02001BFC RID: 7164
	private enum State
	{
		// Token: 0x04007357 RID: 29527
		Up,
		// Token: 0x04007358 RID: 29528
		Down,
		// Token: 0x04007359 RID: 29529
		Finished
	}
}
