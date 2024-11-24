using System;
using UnityEngine;

// Token: 0x02001FFE RID: 8190
public class SimpleTransformAnimation : MonoBehaviour
{
	// Token: 0x0600ADF3 RID: 44531 RVA: 0x000A5E40 File Offset: 0x000A4040
	private void Start()
	{
	}

	// Token: 0x0600ADF4 RID: 44532 RVA: 0x0011135D File Offset: 0x0010F55D
	private void Update()
	{
		base.transform.Rotate(this.rotationSpeed * Time.unscaledDeltaTime);
		base.transform.Translate(this.translateSpeed * Time.unscaledDeltaTime);
	}

	// Token: 0x0400889D RID: 34973
	[SerializeField]
	private Vector3 rotationSpeed;

	// Token: 0x0400889E RID: 34974
	[SerializeField]
	private Vector3 translateSpeed;
}
