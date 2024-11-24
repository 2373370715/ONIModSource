using System;

// Token: 0x020010DB RID: 4315
internal abstract class DivisibleTask<SharedData> : IWorkItem<SharedData>
{
	// Token: 0x060058A1 RID: 22689 RVA: 0x000D9CC6 File Offset: 0x000D7EC6
	public void Run(SharedData sharedData)
	{
		this.RunDivision(sharedData);
	}

	// Token: 0x060058A2 RID: 22690 RVA: 0x000D9CCF File Offset: 0x000D7ECF
	protected DivisibleTask(string name)
	{
		this.name = name;
	}

	// Token: 0x060058A3 RID: 22691
	protected abstract void RunDivision(SharedData sharedData);

	// Token: 0x04003E81 RID: 16001
	public string name;

	// Token: 0x04003E82 RID: 16002
	public int start;

	// Token: 0x04003E83 RID: 16003
	public int end;
}
