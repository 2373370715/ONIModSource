using System;
using UnityEngine;

// Token: 0x02001056 RID: 4182
public static class CameraSaveData
{
	// Token: 0x0600555D RID: 21853 RVA: 0x000D7B50 File Offset: 0x000D5D50
	public static void Load(FastReader reader)
	{
		CameraSaveData.position = reader.ReadVector3();
		CameraSaveData.localScale = reader.ReadVector3();
		CameraSaveData.rotation = reader.ReadQuaternion();
		CameraSaveData.orthographicsSize = reader.ReadSingle();
		CameraSaveData.valid = true;
	}

	// Token: 0x04003BDB RID: 15323
	public static bool valid;

	// Token: 0x04003BDC RID: 15324
	public static Vector3 position;

	// Token: 0x04003BDD RID: 15325
	public static Vector3 localScale;

	// Token: 0x04003BDE RID: 15326
	public static Quaternion rotation;

	// Token: 0x04003BDF RID: 15327
	public static float orthographicsSize;
}
