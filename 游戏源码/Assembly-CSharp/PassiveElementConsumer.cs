using System;

// Token: 0x0200169B RID: 5787
public class PassiveElementConsumer : ElementConsumer, IGameObjectEffectDescriptor
{
	// Token: 0x06007787 RID: 30599 RVA: 0x000A65EC File Offset: 0x000A47EC
	protected override bool IsActive()
	{
		return true;
	}
}
