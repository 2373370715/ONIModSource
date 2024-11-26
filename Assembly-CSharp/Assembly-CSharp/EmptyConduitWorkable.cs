using System;
using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/EmptyConduitWorkable")]
public class EmptyConduitWorkable : Workable, IEmptyConduitWorkable {
    private const  float      RECHECK_PIPE_INTERVAL = 2f;
    private const  float      TIME_TO_EMPTY_PIPE    = 4f;
    private const  float      NO_EMPTY_SCHEDULED    = -1f;
    private static StatusItem emptyLiquidConduitStatusItem;
    private static StatusItem emptyGasConduitStatusItem;

    private static readonly EventSystem.IntraObjectHandler<EmptyConduitWorkable> OnEmptyConduitCancelledDelegate
        = new EventSystem.IntraObjectHandler<EmptyConduitWorkable>(delegate(EmptyConduitWorkable component,
                                                                            object               data) {
                                                                       component.OnEmptyConduitCancelled(data);
                                                                   });

    private Chore chore;

    [MyCmpReq]
    private Conduit conduit;

    [Serialize]
    private float elapsedTime = -1f;

    private bool emptiedPipe = true;

    public void MarkForEmptying() {
        if (chore == null && HasContents()) {
            var statusItem = GetStatusItem();
            GetComponent<KSelectable>().ToggleStatusItem(statusItem, true);
            CreateWorkChore();
        }
    }

    public void EmptyContents() {
        var cell            = Grid.PosToCell(transform.GetPosition());
        var conduitContents = GetFlowManager().RemoveElement(cell, float.PositiveInfinity);
        elapsedTime = 0f;
        if (conduitContents.mass > 0f && conduitContents.element != SimHashes.Vacuum) {
            var           type = conduit.type;
            IChunkManager instance;
            if (type != ConduitType.Gas) {
                if (type != ConduitType.Liquid) throw new ArgumentException();

                instance = LiquidSourceManager.Instance;
            } else
                instance = GasSourceManager.Instance;

            instance.CreateChunk(conduitContents.element,
                                 conduitContents.mass,
                                 conduitContents.temperature,
                                 conduitContents.diseaseIdx,
                                 conduitContents.diseaseCount,
                                 Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore))
                    .Trigger(580035959, worker);
        }
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        SetOffsetTable(OffsetGroups.InvertedStandardTable);
        SetWorkTime(float.PositiveInfinity);
        faceTargetWhenWorking = true;
        multitoolContext      = "build";
        multitoolHitEffectTag = EffectConfigs.BuildSplashId;
        Subscribe(2127324410, OnEmptyConduitCancelledDelegate);
        if (emptyLiquidConduitStatusItem == null) {
            emptyLiquidConduitStatusItem = new StatusItem("EmptyLiquidConduit",
                                                          BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME,
                                                          BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP,
                                                          "status_item_empty_pipe",
                                                          StatusItem.IconType.Custom,
                                                          NotificationType.Neutral,
                                                          false,
                                                          OverlayModes.LiquidConduits.ID,
                                                          66);

            emptyGasConduitStatusItem = new StatusItem("EmptyGasConduit",
                                                       BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME,
                                                       BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP,
                                                       "status_item_empty_pipe",
                                                       StatusItem.IconType.Custom,
                                                       NotificationType.Neutral,
                                                       false,
                                                       OverlayModes.GasConduits.ID,
                                                       130);
        }

        requiredSkillPerk             = Db.Get().SkillPerks.CanDoPlumbing.Id;
        shouldShowSkillPerkStatusItem = false;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        if (elapsedTime != -1f) MarkForEmptying();
    }

    private bool HasContents() {
        var cell = Grid.PosToCell(transform.GetPosition());
        return GetFlowManager().GetContents(cell).mass > 0f;
    }

    private void CancelEmptying() {
        CleanUpVisualization();
        if (chore != null) {
            chore.Cancel("Cancel");
            chore                         = null;
            shouldShowSkillPerkStatusItem = false;
            UpdateStatusItem();
        }
    }

    private void CleanUpVisualization() {
        var statusItem = GetStatusItem();
        var component  = GetComponent<KSelectable>();
        if (component != null) component.ToggleStatusItem(statusItem, false);
        elapsedTime = -1f;
        if (chore != null) GetComponent<Prioritizable>().RemoveRef();
    }

    protected override void OnCleanUp() {
        CancelEmptying();
        base.OnCleanUp();
    }

    private ConduitFlow GetFlowManager() {
        if (conduit.type != ConduitType.Gas) return Game.Instance.liquidConduitFlow;

        return Game.Instance.gasConduitFlow;
    }

    private void OnEmptyConduitCancelled(object data) { CancelEmptying(); }

    private StatusItem GetStatusItem() {
        var        type = conduit.type;
        StatusItem result;
        if (type != ConduitType.Gas) {
            if (type != ConduitType.Liquid) throw new ArgumentException();

            result = emptyLiquidConduitStatusItem;
        } else
            result = emptyGasConduitStatusItem;

        return result;
    }

    private void CreateWorkChore() {
        GetComponent<Prioritizable>().AddRef();
        chore = new WorkChore<EmptyConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage,
                                                    this,
                                                    null,
                                                    true,
                                                    null,
                                                    null,
                                                    null,
                                                    true,
                                                    null,
                                                    false,
                                                    false);

        chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDoPlumbing.Id);
        elapsedTime                   = 0f;
        emptiedPipe                   = false;
        shouldShowSkillPerkStatusItem = true;
        UpdateStatusItem();
    }

    protected override bool OnWorkTick(WorkerBase worker, float dt) {
        if (elapsedTime == -1f) return true;

        var result = false;
        elapsedTime += dt;
        if (!emptiedPipe) {
            if (elapsedTime > 4f) {
                EmptyContents();
                emptiedPipe = true;
                elapsedTime = 0f;
            }
        } else if (elapsedTime > 2f) {
            var cell = Grid.PosToCell(transform.GetPosition());
            if (GetFlowManager().GetContents(cell).mass > 0f) {
                elapsedTime = 0f;
                emptiedPipe = false;
            } else {
                CleanUpVisualization();
                chore                         = null;
                result                        = true;
                shouldShowSkillPerkStatusItem = false;
                UpdateStatusItem();
            }
        }

        return result;
    }

    public override bool InstantlyFinish(WorkerBase worker) {
        worker.Work(4f);
        return true;
    }

    public override float GetPercentComplete() { return Mathf.Clamp01(elapsedTime / 4f); }
}