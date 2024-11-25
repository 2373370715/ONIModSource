using STRINGS;

public class ConditionPilotOnBoard : ProcessCondition {
    private readonly PassengerRocketModule module;
    private readonly RocketModuleCluster   rocketModule;

    public ConditionPilotOnBoard(PassengerRocketModule module) {
        this.module  = module;
        rocketModule = module.GetComponent<RocketModuleCluster>();
    }

    public override Status EvaluateCondition() {
        if (module.CheckPilotBoarded()) return Status.Ready;

        if (rocketModule.CraftInterface.GetRobotPilotModule() != null) return Status.Warning;

        return Status.Failure;
    }

    public override string GetStatusMessage(Status status) {
        if (status == Status.Ready) return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.READY;

        if (status == Status.Warning && rocketModule.CraftInterface.GetRobotPilotModule() != null)
            return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.ROBO_PILOT_WARNING;

        return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.FAILURE;
    }

    public override string GetStatusTooltip(Status status) {
        if (status == Status.Ready) return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.READY;

        if (status == Status.Warning && rocketModule.CraftInterface.GetRobotPilotModule() != null)
            return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.ROBO_PILOT_WARNING;

        return UI.STARMAP.LAUNCHCHECKLIST.PILOT_BOARDED.TOOLTIP.FAILURE;
    }

    public override bool ShowInUI() { return true; }
}