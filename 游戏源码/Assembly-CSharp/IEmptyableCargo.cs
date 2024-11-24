using System;

// Token: 0x02001F8A RID: 8074
public interface IEmptyableCargo
{
	// Token: 0x0600AA6C RID: 43628
	bool CanEmptyCargo();

	// Token: 0x0600AA6D RID: 43629
	void EmptyCargo();

	// Token: 0x17000AD6 RID: 2774
	// (get) Token: 0x0600AA6E RID: 43630
	IStateMachineTarget master { get; }

	// Token: 0x17000AD7 RID: 2775
	// (get) Token: 0x0600AA6F RID: 43631
	bool CanAutoDeploy { get; }

	// Token: 0x17000AD8 RID: 2776
	// (get) Token: 0x0600AA70 RID: 43632
	// (set) Token: 0x0600AA71 RID: 43633
	bool AutoDeploy { get; set; }

	// Token: 0x17000AD9 RID: 2777
	// (get) Token: 0x0600AA72 RID: 43634
	bool ChooseDuplicant { get; }

	// Token: 0x17000ADA RID: 2778
	// (get) Token: 0x0600AA73 RID: 43635
	bool ModuleDeployed { get; }

	// Token: 0x17000ADB RID: 2779
	// (get) Token: 0x0600AA74 RID: 43636
	// (set) Token: 0x0600AA75 RID: 43637
	MinionIdentity ChosenDuplicant { get; set; }
}
