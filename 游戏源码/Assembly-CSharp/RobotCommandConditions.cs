using System;

// Token: 0x02001923 RID: 6435
public class RobotCommandConditions : CommandConditions
{
	// Token: 0x06008615 RID: 34325 RVA: 0x0034AFF8 File Offset: 0x003491F8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		RocketModule component = base.GetComponent<RocketModule>();
		this.reachable = (ConditionDestinationReachable)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionDestinationReachable(base.GetComponent<RocketModule>()));
		this.allModulesComplete = (ConditionAllModulesComplete)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionAllModulesComplete(base.GetComponent<ILaunchableRocket>()));
		if (base.GetComponent<ILaunchableRocket>().registerType == LaunchableRocketRegisterType.Spacecraft)
		{
			this.destHasResources = (ConditionHasMinimumMass)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionHasMinimumMass(base.GetComponent<CommandModule>()));
			this.cargoEmpty = (CargoBayIsEmpty)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new CargoBayIsEmpty(base.GetComponent<CommandModule>()));
		}
		else if (base.GetComponent<ILaunchableRocket>().registerType == LaunchableRocketRegisterType.Clustercraft)
		{
			this.hasEngine = (ConditionHasEngine)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionHasEngine(base.GetComponent<ILaunchableRocket>()));
			this.hasNosecone = (ConditionHasNosecone)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionHasNosecone(base.GetComponent<LaunchableRocketCluster>()));
			this.onLaunchPad = (ConditionOnLaunchPad)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionOnLaunchPad(base.GetComponent<RocketModuleCluster>().CraftInterface));
			this.HasCargoBayForNoseconeHarvest = (ConditionHasCargoBayForNoseconeHarvest)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionHasCargoBayForNoseconeHarvest(base.GetComponent<LaunchableRocketCluster>()));
		}
		int bufferWidth = 1;
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			bufferWidth = 0;
		}
		this.flightPathIsClear = (ConditionFlightPathIsClear)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketFlight, new ConditionFlightPathIsClear(base.gameObject, bufferWidth));
		this.robotPilotReady = (ConditionRobotPilotReady)component.AddModuleCondition((base.GetComponent<ILaunchableRocket>().registerType == LaunchableRocketRegisterType.Spacecraft) ? ProcessCondition.ProcessConditionType.RocketPrep : ProcessCondition.ProcessConditionType.RocketFlight, new ConditionRobotPilotReady(base.GetComponent<RoboPilotModule>()));
	}

	// Token: 0x04006537 RID: 25911
	public ConditionRobotPilotReady robotPilotReady;
}
