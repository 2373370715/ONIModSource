using System;

// Token: 0x02000D88 RID: 3464
public class FossilMineSM : ComplexFabricatorSM
{
	// Token: 0x060043E9 RID: 17385 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected override void OnSpawn()
	{
	}

	// Token: 0x060043EA RID: 17386 RVA: 0x000CBE59 File Offset: 0x000CA059
	public void Activate()
	{
		base.smi.StartSM();
	}

	// Token: 0x060043EB RID: 17387 RVA: 0x000CBE66 File Offset: 0x000CA066
	public void Deactivate()
	{
		base.smi.StopSM("FossilMine.Deactivated");
	}
}
