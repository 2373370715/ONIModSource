using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002017 RID: 8215
[AddComponentMenu("KMonoBehaviour/scripts/StarmapPlanetVisualizer")]
public class StarmapPlanetVisualizer : KMonoBehaviour
{
	// Token: 0x0400895F RID: 35167
	public Image image;

	// Token: 0x04008960 RID: 35168
	public LocText label;

	// Token: 0x04008961 RID: 35169
	public MultiToggle button;

	// Token: 0x04008962 RID: 35170
	public RectTransform selection;

	// Token: 0x04008963 RID: 35171
	public GameObject analysisSelection;

	// Token: 0x04008964 RID: 35172
	public Image unknownBG;

	// Token: 0x04008965 RID: 35173
	public GameObject rocketIconContainer;
}
