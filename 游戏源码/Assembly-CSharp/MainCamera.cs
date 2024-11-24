using System;
using UnityEngine;

// Token: 0x02000A8D RID: 2701
public class MainCamera : MonoBehaviour
{
	// Token: 0x060031F7 RID: 12791 RVA: 0x000C0697 File Offset: 0x000BE897
	private void Awake()
	{
		if (Camera.main != null)
		{
			UnityEngine.Object.Destroy(Camera.main.gameObject);
		}
		base.gameObject.tag = "MainCamera";
	}
}
