using System;
using UnityEngine;

// Token: 0x02001A85 RID: 6789
public abstract class VisualizerEffect : MonoBehaviour
{
	// Token: 0x06008DF6 RID: 36342
	protected abstract void SetupMaterial();

	// Token: 0x06008DF7 RID: 36343
	protected abstract void SetupOcclusionTex();

	// Token: 0x06008DF8 RID: 36344
	protected abstract void OnPostRender();

	// Token: 0x06008DF9 RID: 36345 RVA: 0x000FCB66 File Offset: 0x000FAD66
	protected virtual void Start()
	{
		this.SetupMaterial();
		this.SetupOcclusionTex();
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x04006AA8 RID: 27304
	protected Material material;

	// Token: 0x04006AA9 RID: 27305
	protected Camera myCamera;

	// Token: 0x04006AAA RID: 27306
	protected Texture2D OcclusionTex;
}
