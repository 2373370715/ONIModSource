using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001BFF RID: 7167
public class AsteroidClock : MonoBehaviour
{
	// Token: 0x060094E1 RID: 38113 RVA: 0x00100F05 File Offset: 0x000FF105
	private void Awake()
	{
		this.UpdateOverlay();
	}

	// Token: 0x060094E2 RID: 38114 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void Start()
	{
	}

	// Token: 0x060094E3 RID: 38115 RVA: 0x00100F0D File Offset: 0x000FF10D
	private void Update()
	{
		if (GameClock.Instance != null)
		{
			this.rotationTransform.rotation = Quaternion.Euler(0f, 0f, 360f * -GameClock.Instance.GetCurrentCycleAsPercentage());
		}
	}

	// Token: 0x060094E4 RID: 38116 RVA: 0x00397298 File Offset: 0x00395498
	private void UpdateOverlay()
	{
		float fillAmount = 0.125f;
		this.NightOverlay.fillAmount = fillAmount;
	}

	// Token: 0x04007362 RID: 29538
	public Transform rotationTransform;

	// Token: 0x04007363 RID: 29539
	public Image NightOverlay;
}
