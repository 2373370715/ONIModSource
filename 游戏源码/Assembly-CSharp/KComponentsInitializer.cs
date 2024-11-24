using System;

// Token: 0x0200147D RID: 5245
public class KComponentsInitializer : KComponentSpawn
{
	// Token: 0x06006CBB RID: 27835 RVA: 0x000E765F File Offset: 0x000E585F
	private void Awake()
	{
		KComponentSpawn.instance = this;
		this.comps = new GameComps();
	}
}
