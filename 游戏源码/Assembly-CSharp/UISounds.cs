using System;
using UnityEngine;

// Token: 0x020019F8 RID: 6648
[AddComponentMenu("KMonoBehaviour/scripts/UISounds")]
public class UISounds : KMonoBehaviour
{
	// Token: 0x1700090E RID: 2318
	// (get) Token: 0x06008A89 RID: 35465 RVA: 0x000FAB18 File Offset: 0x000F8D18
	// (set) Token: 0x06008A8A RID: 35466 RVA: 0x000FAB1F File Offset: 0x000F8D1F
	public static UISounds Instance { get; private set; }

	// Token: 0x06008A8B RID: 35467 RVA: 0x000FAB27 File Offset: 0x000F8D27
	public static void DestroyInstance()
	{
		UISounds.Instance = null;
	}

	// Token: 0x06008A8C RID: 35468 RVA: 0x000FAB2F File Offset: 0x000F8D2F
	protected override void OnPrefabInit()
	{
		UISounds.Instance = this;
	}

	// Token: 0x06008A8D RID: 35469 RVA: 0x000FAB37 File Offset: 0x000F8D37
	public static void PlaySound(UISounds.Sound sound)
	{
		UISounds.Instance.PlaySoundInternal(sound);
	}

	// Token: 0x06008A8E RID: 35470 RVA: 0x0035B848 File Offset: 0x00359A48
	private void PlaySoundInternal(UISounds.Sound sound)
	{
		for (int i = 0; i < this.soundData.Length; i++)
		{
			if (this.soundData[i].sound == sound)
			{
				if (this.logSounds)
				{
					DebugUtil.LogArgs(new object[]
					{
						"Play sound",
						this.soundData[i].name
					});
				}
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound(this.soundData[i].name, false));
			}
		}
	}

	// Token: 0x04006844 RID: 26692
	[SerializeField]
	private bool logSounds;

	// Token: 0x04006845 RID: 26693
	[SerializeField]
	private UISounds.SoundData[] soundData;

	// Token: 0x020019F9 RID: 6649
	public enum Sound
	{
		// Token: 0x04006848 RID: 26696
		NegativeNotification,
		// Token: 0x04006849 RID: 26697
		PositiveNotification,
		// Token: 0x0400684A RID: 26698
		Select,
		// Token: 0x0400684B RID: 26699
		Negative,
		// Token: 0x0400684C RID: 26700
		Back,
		// Token: 0x0400684D RID: 26701
		ClickObject,
		// Token: 0x0400684E RID: 26702
		HUD_Mouseover,
		// Token: 0x0400684F RID: 26703
		Object_Mouseover,
		// Token: 0x04006850 RID: 26704
		ClickHUD,
		// Token: 0x04006851 RID: 26705
		Object_AutoSelected
	}

	// Token: 0x020019FA RID: 6650
	[Serializable]
	private struct SoundData
	{
		// Token: 0x04006852 RID: 26706
		public string name;

		// Token: 0x04006853 RID: 26707
		public UISounds.Sound sound;
	}
}
