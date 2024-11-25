using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class FarmDiagnostic : ColonyDiagnostic {
    private List<PlantablePlot> plots;

    public FarmDiagnostic(int worldID) : base(worldID, UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.ALL_NAME) {
        icon = "icon_errand_farm";
        AddCriterion("CheckHasFarms",
                     new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKHASFARMS,
                                             CheckHasFarms));

        AddCriterion("CheckPlanted",
                     new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKPLANTED, CheckPlanted));

        AddCriterion("CheckWilting",
                     new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKWILTING, CheckWilting));

        AddCriterion("CheckOperational",
                     new DiagnosticCriterion(UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.CRITERIA.CHECKOPERATIONAL,
                                             CheckOperational));
    }

    private void RefreshPlots() {
        plots = Components.PlantablePlots.GetItems(worldID).FindAll(match => match.HasDepositTag(GameTags.CropSeed));
    }

    private DiagnosticResult CheckHasFarms() {
        var result = new DiagnosticResult(DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
        if (plots.Count == 0) {
            result.opinion = DiagnosticResult.Opinion.Concern;
            result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NONE;
        }

        return result;
    }

    private DiagnosticResult CheckPlanted() {
        var result = new DiagnosticResult(DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
        var flag   = false;
        using (var enumerator = plots.GetEnumerator()) {
            while (enumerator.MoveNext())
                if (enumerator.Current.plant != null) {
                    flag = true;
                    break;
                }
        }

        if (!flag) {
            result.opinion = DiagnosticResult.Opinion.Concern;
            result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NONE_PLANTED;
        }

        return result;
    }

    private DiagnosticResult CheckWilting() {
        var result = new DiagnosticResult(DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
        foreach (var plantablePlot in plots)
            if (plantablePlot.plant != null && plantablePlot.plant.HasTag(GameTags.Wilting)) {
                var component = plantablePlot.plant.GetComponent<StandardCropPlant>();
                if (component != null                                           &&
                    component.smi.IsInsideState(component.smi.sm.alive.wilting) &&
                    component.smi.timeinstate > 15f) {
                    result.opinion = DiagnosticResult.Opinion.Concern;
                    result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.WILTING;
                    result.clickThroughTarget
                        = new Tuple<Vector3, GameObject>(plantablePlot.transform.position, plantablePlot.gameObject);

                    break;
                }
            }

        return result;
    }

    private DiagnosticResult CheckOperational() {
        var result = new DiagnosticResult(DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.GENERIC_CRITERIA_PASS);
        foreach (var plantablePlot in plots)
            if (plantablePlot.plant != null && !plantablePlot.HasTag(GameTags.Operational)) {
                result.opinion = DiagnosticResult.Opinion.Concern;
                result.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.INOPERATIONAL;
                result.clickThroughTarget
                    = new Tuple<Vector3, GameObject>(plantablePlot.transform.position, plantablePlot.gameObject);

                break;
            }

        return result;
    }

    public override string GetAverageValueString() {
        if (plots == null) RefreshPlots();
        return TrackerTool.Instance.GetWorldTracker<CropTracker>(worldID).GetCurrentValue() + "/" + plots.Count;
    }

    public override DiagnosticResult Evaluate() {
        DiagnosticResult diagnosticResult;
        if (ColonyDiagnosticUtility.IgnoreRocketsWithNoCrewRequested(worldID, out diagnosticResult))
            return diagnosticResult;

        RefreshPlots();
        diagnosticResult = base.Evaluate();
        if (diagnosticResult.opinion == DiagnosticResult.Opinion.Normal)
            diagnosticResult.Message = UI.COLONY_DIAGNOSTICS.FARMDIAGNOSTIC.NORMAL;

        return diagnosticResult;
    }
}