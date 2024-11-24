using System;

// Token: 0x02000C29 RID: 3113
internal class ElementAudioFileLoader : AsyncCsvLoader<ElementAudioFileLoader, ElementsAudio.ElementAudioConfig>
{
	// Token: 0x06003B7E RID: 15230 RVA: 0x000C6613 File Offset: 0x000C4813
	public ElementAudioFileLoader() : base(Assets.instance.elementAudio)
	{
	}

	// Token: 0x06003B7F RID: 15231 RVA: 0x000C6625 File Offset: 0x000C4825
	public override void Run()
	{
		base.Run();
	}
}
