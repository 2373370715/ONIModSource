using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02002034 RID: 8244
public class Tween : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600AF7D RID: 44925 RVA: 0x001120BB File Offset: 0x001102BB
	private void Awake()
	{
		this.Selectable = base.GetComponent<Selectable>();
	}

	// Token: 0x0600AF7E RID: 44926 RVA: 0x001120C9 File Offset: 0x001102C9
	public void OnPointerEnter(PointerEventData data)
	{
		this.Direction = 1f;
	}

	// Token: 0x0600AF7F RID: 44927 RVA: 0x001120D6 File Offset: 0x001102D6
	public void OnPointerExit(PointerEventData data)
	{
		this.Direction = -1f;
	}

	// Token: 0x0600AF80 RID: 44928 RVA: 0x00420D94 File Offset: 0x0041EF94
	private void Update()
	{
		if (this.Selectable.interactable)
		{
			float x = base.transform.localScale.x;
			float num = x + this.Direction * Time.unscaledDeltaTime * Tween.ScaleSpeed;
			num = Mathf.Min(num, Tween.Scale);
			num = Mathf.Max(num, 1f);
			if (num != x)
			{
				base.transform.localScale = new Vector3(num, num, 1f);
			}
		}
	}

	// Token: 0x04008A50 RID: 35408
	private static float Scale = 1.025f;

	// Token: 0x04008A51 RID: 35409
	private static float ScaleSpeed = 0.5f;

	// Token: 0x04008A52 RID: 35410
	private Selectable Selectable;

	// Token: 0x04008A53 RID: 35411
	private float Direction = -1f;
}
