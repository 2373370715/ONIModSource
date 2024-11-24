using System;

// Token: 0x02000347 RID: 839
public class FossilMineWorkable : ComplexFabricatorWorkable
{
	// Token: 0x06000D8B RID: 3467 RVA: 0x000ABF22 File Offset: 0x000AA122
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.shouldShowSkillPerkStatusItem = false;
	}
}
