using System;
using UnityEngine;

// Token: 0x02001A8B RID: 6795
public class FixGraphicsCorruption : MonoBehaviour
{
	// Token: 0x06008E07 RID: 36359 RVA: 0x000FCBFF File Offset: 0x000FADFF
	private void Start()
	{
		Camera component = base.GetComponent<Camera>();
		component.transparencySortMode = TransparencySortMode.Orthographic;
		component.tag = "Untagged";
	}

	// Token: 0x06008E08 RID: 36360 RVA: 0x000FCC18 File Offset: 0x000FAE18
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, dest);
	}
}
