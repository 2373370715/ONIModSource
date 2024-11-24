using System;
using UnityEngine;

// Token: 0x020009A8 RID: 2472
[AddComponentMenu("KMonoBehaviour/scripts/CameraFollowHelper")]
public class CameraFollowHelper : KMonoBehaviour
{
	// Token: 0x06002D45 RID: 11589 RVA: 0x000BD549 File Offset: 0x000BB749
	private void LateUpdate()
	{
		if (CameraController.Instance != null)
		{
			CameraController.Instance.UpdateFollowTarget();
		}
	}
}
