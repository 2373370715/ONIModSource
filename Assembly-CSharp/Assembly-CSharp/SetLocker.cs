using System;
using KSerialization;
using STRINGS;
using Random = UnityEngine.Random;

public class SetLocker : StateMachineComponent<SetLocker.StatesInstance>, ISidescreenButtonControl {
    private Chore chore;

    [Serialize]
    private string[] contents;

    public Vector2I dropOffset = Vector2I.zero;
    public bool     dropOnDeconstruct;
    public string   machineSound;
    public int[]    numDataBanks;
    public string   overrideAnim;

    [Serialize]
    private bool pendingRummage;

    public string[][] possible_contents_ids;

    [MyCmpAdd]
    private Prioritizable prioritizable;

    [Serialize]
    private bool used;

    public string SidescreenButtonText =>
        chore == null ? UI.USERMENUACTIONS.OPENPOI.NAME : UI.USERMENUACTIONS.OPENPOI.NAME_OFF;

    public string SidescreenButtonTooltip =>
        chore == null ? UI.USERMENUACTIONS.OPENPOI.TOOLTIP : UI.USERMENUACTIONS.OPENPOI.TOOLTIP_OFF;

    public bool SidescreenEnabled() { return true; }
    public int  HorizontalGroupID() { return -1; }

    public void OnSidescreenButtonPressed() {
        if (chore == null) {
            OnClickOpen();
            return;
        }

        OnClickCancel();
    }

    public             bool SidescreenButtonInteractable()                     { return !used; }
    public             int  ButtonSideScreenSortOrder()                        { return 20; }
    public             void SetButtonTextOverride(ButtonMenuTextOverride text) { throw new NotImplementedException(); }
    protected override void OnPrefabInit()                                     { base.OnPrefabInit(); }

    public void ChooseContents() {
        contents = possible_contents_ids[Random.Range(0, possible_contents_ids.GetLength(0))];
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        smi.StartSM();
        if (contents == null)
            ChooseContents();
        else {
            var array = contents;
            for (var i = 0; i < array.Length; i++)
                if (Assets.GetPrefab(array[i]) == null) {
                    ChooseContents();
                    break;
                }
        }

        if (pendingRummage) ActivateChore();
    }

    public void DropContents() {
        if (contents == null) return;

        if (DlcManager.IsExpansion1Active() && numDataBanks.Length >= 2) {
            var num = Random.Range(numDataBanks[0], numDataBanks[1]);
            for (var i = 0; i <= num; i++) {
                Scenario.SpawnPrefab(Grid.PosToCell(gameObject),
                                     dropOffset.x,
                                     dropOffset.y,
                                     "OrbitalResearchDatabank",
                                     Grid.SceneLayer.Front)
                        .SetActive(true);

                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus,
                                              Assets.GetPrefab("OrbitalResearchDatabank".ToTag()).GetProperName(),
                                              smi.master.transform);
            }
        }

        for (var j = 0; j < contents.Length; j++) {
            var gameObject = Scenario.SpawnPrefab(Grid.PosToCell(this.gameObject),
                                                  dropOffset.x,
                                                  dropOffset.y,
                                                  contents[j],
                                                  Grid.SceneLayer.Front);

            if (gameObject != null) {
                gameObject.SetActive(true);
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus,
                                              Assets.GetPrefab(contents[j].ToTag()).GetProperName(),
                                              smi.master.transform);
            }
        }

        this.gameObject.Trigger(-372600542, this);
    }

    private void OnClickOpen()   { ActivateChore(); }
    private void OnClickCancel() { CancelChore(); }

    public void ActivateChore(object param = null) {
        if (chore != null) return;

        Prioritizable.AddRef(gameObject);
        Trigger(1980521255);
        pendingRummage = true;
        GetComponent<Workable>().SetWorkTime(1.5f);
        chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage,
                                        this,
                                        null,
                                        true,
                                        delegate { CompleteChore(); },
                                        null,
                                        null,
                                        true,
                                        null,
                                        false,
                                        true,
                                        Assets.GetAnim(overrideAnim),
                                        false,
                                        true,
                                        true,
                                        PriorityScreen.PriorityClass.high);
    }

    public void CancelChore(object param = null) {
        if (chore == null) return;

        pendingRummage = false;
        Prioritizable.RemoveRef(gameObject);
        Trigger(1980521255);
        chore.Cancel("User cancelled");
        chore = null;
    }

    private void CompleteChore() {
        used = true;
        smi.GoTo(smi.sm.open);
        chore          = null;
        pendingRummage = false;
        Game.Instance.userMenu.Refresh(gameObject);
        Prioritizable.RemoveRef(gameObject);
    }

    public class StatesInstance : GameStateMachine<States, StatesInstance, SetLocker, object>.GameInstance {
        public StatesInstance(SetLocker master) : base(master) { }

        public override void StartSM() {
            base.StartSM();
            smi.Subscribe(-702296337,
                          delegate {
                              if (smi.master.dropOnDeconstruct && smi.IsInsideState(smi.sm.closed))
                                  smi.master.DropContents();
                          });
        }
    }

    public class States : GameStateMachine<States, StatesInstance, SetLocker> {
        public State closed;
        public State off;
        public State open;

        public override void InitializeStates(out BaseState default_state) {
            default_state = closed;
            serializable  = SerializeType.Both_DEPRECATED;
            closed.PlayAnim("on")
                  .Enter(delegate(StatesInstance smi) {
                             if (smi.master.machineSound != null) {
                                 var component = smi.master.GetComponent<LoopingSounds>();
                                 if (component != null)
                                     component.StartSound(GlobalAssets.GetSound(smi.master.machineSound));
                             }
                         });

            open.PlayAnim("working_pre")
                .QueueAnim("working_loop")
                .QueueAnim("working_pst")
                .OnAnimQueueComplete(off)
                .Exit(delegate(StatesInstance smi) { smi.master.DropContents(); });

            off.PlayAnim("off")
               .Enter(delegate(StatesInstance smi) {
                          if (smi.master.machineSound != null) {
                              var component = smi.master.GetComponent<LoopingSounds>();
                              if (component != null)
                                  component.StopSound(GlobalAssets.GetSound(smi.master.machineSound));
                          }
                      });
        }
    }
}