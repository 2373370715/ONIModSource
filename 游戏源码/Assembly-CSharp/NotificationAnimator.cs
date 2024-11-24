using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DFD RID: 7677
public class NotificationAnimator : MonoBehaviour
{
	// Token: 0x0600A0B0 RID: 41136 RVA: 0x001086E8 File Offset: 0x001068E8
	public void Begin(bool startOffset = true)
	{
		this.Reset();
		this.animating = true;
		if (startOffset)
		{
			this.layoutElement.minWidth = 100f;
			return;
		}
		this.layoutElement.minWidth = 1f;
		this.speed = -10f;
	}

	// Token: 0x0600A0B1 RID: 41137 RVA: 0x00108726 File Offset: 0x00106926
	private void Reset()
	{
		this.bounceCount = 2;
		this.layoutElement = base.GetComponent<LayoutElement>();
		this.layoutElement.minWidth = 0f;
		this.speed = 1f;
	}

	// Token: 0x0600A0B2 RID: 41138 RVA: 0x00108756 File Offset: 0x00106956
	public void Stop()
	{
		this.Reset();
		this.animating = false;
	}

	// Token: 0x0600A0B3 RID: 41139 RVA: 0x003D5F7C File Offset: 0x003D417C
	private void LateUpdate()
	{
		if (!this.animating)
		{
			return;
		}
		this.layoutElement.minWidth -= this.speed;
		this.speed += 0.5f;
		if (this.layoutElement.minWidth <= 0f)
		{
			if (this.bounceCount > 0)
			{
				this.bounceCount--;
				this.speed = -this.speed / Mathf.Pow(2f, (float)(2 - this.bounceCount));
				this.layoutElement.minWidth = -this.speed;
				return;
			}
			this.layoutElement.minWidth = 0f;
			this.Stop();
		}
	}

	// Token: 0x04007D87 RID: 32135
	private const float START_SPEED = 1f;

	// Token: 0x04007D88 RID: 32136
	private const float ACCELERATION = 0.5f;

	// Token: 0x04007D89 RID: 32137
	private const float BOUNCE_DAMPEN = 2f;

	// Token: 0x04007D8A RID: 32138
	private const int BOUNCE_COUNT = 2;

	// Token: 0x04007D8B RID: 32139
	private const float OFFSETX = 100f;

	// Token: 0x04007D8C RID: 32140
	private float speed = 1f;

	// Token: 0x04007D8D RID: 32141
	private int bounceCount = 2;

	// Token: 0x04007D8E RID: 32142
	private LayoutElement layoutElement;

	// Token: 0x04007D8F RID: 32143
	[SerializeField]
	private bool animating = true;
}
