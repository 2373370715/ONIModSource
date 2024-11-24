using System;

// Token: 0x02001926 RID: 6438
public class CommandConditions : KMonoBehaviour
{
	// Token: 0x04006540 RID: 25920
	public ConditionDestinationReachable reachable;

	// Token: 0x04006541 RID: 25921
	public CargoBayIsEmpty cargoEmpty;

	// Token: 0x04006542 RID: 25922
	public ConditionHasMinimumMass destHasResources;

	// Token: 0x04006543 RID: 25923
	public ConditionAllModulesComplete allModulesComplete;

	// Token: 0x04006544 RID: 25924
	public ConditionHasCargoBayForNoseconeHarvest HasCargoBayForNoseconeHarvest;

	// Token: 0x04006545 RID: 25925
	public ConditionHasEngine hasEngine;

	// Token: 0x04006546 RID: 25926
	public ConditionHasNosecone hasNosecone;

	// Token: 0x04006547 RID: 25927
	public ConditionOnLaunchPad onLaunchPad;

	// Token: 0x04006548 RID: 25928
	public ConditionFlightPathIsClear flightPathIsClear;
}
