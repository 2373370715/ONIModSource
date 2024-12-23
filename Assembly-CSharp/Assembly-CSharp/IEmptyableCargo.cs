﻿public interface IEmptyableCargo {
    IStateMachineTarget master          { get; }
    bool                CanAutoDeploy   { get; }
    bool                AutoDeploy      { get; set; }
    bool                ChooseDuplicant { get; }
    bool                ModuleDeployed  { get; }
    MinionIdentity      ChosenDuplicant { get; set; }
    bool                CanEmptyCargo();
    void                EmptyCargo();
}