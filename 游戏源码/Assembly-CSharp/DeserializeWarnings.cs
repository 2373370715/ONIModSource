using System;
using UnityEngine;

// Token: 0x02001218 RID: 4632
[AddComponentMenu("KMonoBehaviour/scripts/DeserializeWarnings")]
public class DeserializeWarnings : KMonoBehaviour
{
	// Token: 0x06005EFA RID: 24314 RVA: 0x000DE018 File Offset: 0x000DC218
	public static void DestroyInstance()
	{
		DeserializeWarnings.Instance = null;
	}

	// Token: 0x06005EFB RID: 24315 RVA: 0x000DE020 File Offset: 0x000DC220
	protected override void OnPrefabInit()
	{
		DeserializeWarnings.Instance = this;
	}

	// Token: 0x04004388 RID: 17288
	public DeserializeWarnings.Warning BuildingTemeperatureIsZeroKelvin;

	// Token: 0x04004389 RID: 17289
	public DeserializeWarnings.Warning PipeContentsTemperatureIsNan;

	// Token: 0x0400438A RID: 17290
	public DeserializeWarnings.Warning PrimaryElementTemperatureIsNan;

	// Token: 0x0400438B RID: 17291
	public DeserializeWarnings.Warning PrimaryElementHasNoElement;

	// Token: 0x0400438C RID: 17292
	public static DeserializeWarnings Instance;

	// Token: 0x02001219 RID: 4633
	public struct Warning
	{
		// Token: 0x06005EFD RID: 24317 RVA: 0x000DE028 File Offset: 0x000DC228
		public void Warn(string message, GameObject obj = null)
		{
			if (!this.isSet)
			{
				global::Debug.LogWarning(message, obj);
				this.isSet = true;
			}
		}

		// Token: 0x0400438D RID: 17293
		private bool isSet;
	}
}
