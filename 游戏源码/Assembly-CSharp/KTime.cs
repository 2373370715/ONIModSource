using System;
using UnityEngine;

// Token: 0x02001487 RID: 5255
[AddComponentMenu("KMonoBehaviour/scripts/KTime")]
public class KTime : KMonoBehaviour
{
	// Token: 0x170006D2 RID: 1746
	// (get) Token: 0x06006CFA RID: 27898 RVA: 0x000E7841 File Offset: 0x000E5A41
	// (set) Token: 0x06006CFB RID: 27899 RVA: 0x000E7849 File Offset: 0x000E5A49
	public float UnscaledGameTime { get; set; }

	// Token: 0x170006D3 RID: 1747
	// (get) Token: 0x06006CFC RID: 27900 RVA: 0x000E7852 File Offset: 0x000E5A52
	// (set) Token: 0x06006CFD RID: 27901 RVA: 0x000E7859 File Offset: 0x000E5A59
	public static KTime Instance { get; private set; }

	// Token: 0x06006CFE RID: 27902 RVA: 0x000E7861 File Offset: 0x000E5A61
	public static void DestroyInstance()
	{
		KTime.Instance = null;
	}

	// Token: 0x06006CFF RID: 27903 RVA: 0x000E7869 File Offset: 0x000E5A69
	protected override void OnPrefabInit()
	{
		KTime.Instance = this;
		this.UnscaledGameTime = Time.unscaledTime;
	}

	// Token: 0x06006D00 RID: 27904 RVA: 0x000E7861 File Offset: 0x000E5A61
	protected override void OnCleanUp()
	{
		KTime.Instance = null;
	}

	// Token: 0x06006D01 RID: 27905 RVA: 0x000E787C File Offset: 0x000E5A7C
	public void Update()
	{
		if (!SpeedControlScreen.Instance.IsPaused)
		{
			this.UnscaledGameTime += Time.unscaledDeltaTime;
		}
	}
}
