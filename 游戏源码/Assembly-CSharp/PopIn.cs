using System;
using UnityEngine;

// Token: 0x02001EB1 RID: 7857
public class PopIn : MonoBehaviour
{
	// Token: 0x0600A4FE RID: 42238 RVA: 0x0010B05B File Offset: 0x0010925B
	private void OnEnable()
	{
		this.StartPopIn(true);
	}

	// Token: 0x0600A4FF RID: 42239 RVA: 0x003EA6F8 File Offset: 0x003E88F8
	private void Update()
	{
		float num = Mathf.Lerp(base.transform.localScale.x, this.targetScale, Time.unscaledDeltaTime * this.speed);
		base.transform.localScale = new Vector3(num, num, 1f);
	}

	// Token: 0x0600A500 RID: 42240 RVA: 0x0010B064 File Offset: 0x00109264
	public void StartPopIn(bool force_reset = false)
	{
		if (force_reset)
		{
			base.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
		}
		this.targetScale = 1f;
	}

	// Token: 0x0600A501 RID: 42241 RVA: 0x0010B093 File Offset: 0x00109293
	public void StartPopOut()
	{
		this.targetScale = 0f;
	}

	// Token: 0x04008126 RID: 33062
	private float targetScale;

	// Token: 0x04008127 RID: 33063
	public float speed;
}
