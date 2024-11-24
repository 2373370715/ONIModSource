using System;

// Token: 0x02000960 RID: 2400
public class ElementsAudio
{
	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06002B43 RID: 11075 RVA: 0x000BC208 File Offset: 0x000BA408
	public static ElementsAudio Instance
	{
		get
		{
			if (ElementsAudio._instance == null)
			{
				ElementsAudio._instance = new ElementsAudio();
			}
			return ElementsAudio._instance;
		}
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x000BC220 File Offset: 0x000BA420
	public void LoadData(ElementsAudio.ElementAudioConfig[] elements_audio_configs)
	{
		this.elementAudioConfigs = elements_audio_configs;
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x001DED34 File Offset: 0x001DCF34
	public ElementsAudio.ElementAudioConfig GetConfigForElement(SimHashes id)
	{
		if (this.elementAudioConfigs != null)
		{
			for (int i = 0; i < this.elementAudioConfigs.Length; i++)
			{
				if (this.elementAudioConfigs[i].elementID == id)
				{
					return this.elementAudioConfigs[i];
				}
			}
		}
		return null;
	}

	// Token: 0x04001D11 RID: 7441
	private static ElementsAudio _instance;

	// Token: 0x04001D12 RID: 7442
	private ElementsAudio.ElementAudioConfig[] elementAudioConfigs;

	// Token: 0x02000961 RID: 2401
	public class ElementAudioConfig : Resource
	{
		// Token: 0x04001D13 RID: 7443
		public SimHashes elementID;

		// Token: 0x04001D14 RID: 7444
		public AmbienceType ambienceType = AmbienceType.None;

		// Token: 0x04001D15 RID: 7445
		public SolidAmbienceType solidAmbienceType = SolidAmbienceType.None;

		// Token: 0x04001D16 RID: 7446
		public string miningSound = "";

		// Token: 0x04001D17 RID: 7447
		public string miningBreakSound = "";

		// Token: 0x04001D18 RID: 7448
		public string oreBumpSound = "";

		// Token: 0x04001D19 RID: 7449
		public string floorEventAudioCategory = "";

		// Token: 0x04001D1A RID: 7450
		public string creatureChewSound = "";
	}
}
