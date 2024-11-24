using System;
using UnityEngine;

// Token: 0x02001270 RID: 4720
public class EffectPrefabs : MonoBehaviour
{
	// Token: 0x170005E0 RID: 1504
	// (get) Token: 0x060060C2 RID: 24770 RVA: 0x000DF1FE File Offset: 0x000DD3FE
	// (set) Token: 0x060060C3 RID: 24771 RVA: 0x000DF205 File Offset: 0x000DD405
	public static EffectPrefabs Instance { get; private set; }

	// Token: 0x060060C4 RID: 24772 RVA: 0x000DF20D File Offset: 0x000DD40D
	private void Awake()
	{
		EffectPrefabs.Instance = this;
	}

	// Token: 0x0400449A RID: 17562
	public GameObject DreamBubble;

	// Token: 0x0400449B RID: 17563
	public GameObject ThoughtBubble;

	// Token: 0x0400449C RID: 17564
	public GameObject ThoughtBubbleConvo;

	// Token: 0x0400449D RID: 17565
	public GameObject MeteorBackground;

	// Token: 0x0400449E RID: 17566
	public GameObject SparkleStreakFX;

	// Token: 0x0400449F RID: 17567
	public GameObject HappySingerFX;

	// Token: 0x040044A0 RID: 17568
	public GameObject HugFrenzyFX;

	// Token: 0x040044A1 RID: 17569
	public GameObject GameplayEventDisplay;

	// Token: 0x040044A2 RID: 17570
	public GameObject OpenTemporalTearBeam;

	// Token: 0x040044A3 RID: 17571
	public GameObject MissileSmokeTrailFX;
}
