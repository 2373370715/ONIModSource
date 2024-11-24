using System;
using UnityEngine;

// Token: 0x020000EC RID: 236
[AddComponentMenu("KMonoBehaviour/scripts/LightSymbolTracker")]
public class LightSymbolTracker : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x060003C3 RID: 963 RVA: 0x00151C54 File Offset: 0x0014FE54
	public void RenderEveryTick(float dt)
	{
		Vector3 v = Vector3.zero;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		bool flag;
		v = (component.GetTransformMatrix() * component.GetSymbolLocalTransform(this.targetSymbol, out flag)).MultiplyPoint(Vector3.zero) - base.transform.GetPosition();
		base.GetComponent<Light2D>().Offset = v;
	}

	// Token: 0x04000283 RID: 643
	public HashedString targetSymbol;
}
