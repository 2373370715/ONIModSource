using STRINGS;
using UnityEngine;

public class YellowAlertManager : GameStateMachine<YellowAlertManager, YellowAlertManager.Instance> {
    public BoolParameter isOn = new BoolParameter();
    public State         off;
    public State         on;
    public State         on_pst;

    public override void InitializeStates(out BaseState default_state) {
        default_state = off;
        serializable  = SerializeType.Both_DEPRECATED;
        off.ParamTransition(isOn, on, IsTrue);
        on.Enter("EnterEvent", delegate { Game.Instance.Trigger(-741654735); })
          .Exit("ExitEvent", delegate { Game.Instance.Trigger(-2062778933); })
          .Enter("EnableVignette", delegate { Vignette.Instance.SetColor(new Color(1f, 1f, 0f, 0.1f)); })
          .Exit("DisableVignette", delegate { Vignette.Instance.Reset(); })
          .Enter("Sounds", delegate { KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON")); })
          .ToggleLoopingSound(GlobalAssets.GetSound("RedAlert_LP"))
          .ToggleNotification(smi => smi.notification)
          .ParamTransition(isOn, off, IsFalse);

        on_pst.Enter("Sounds", delegate { KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF")); });
    }

    public new class Instance : GameInstance {
        private static Instance instance;
        private        bool     hasTopPriorityChore;

        public Notification notification = new Notification(MISC.NOTIFICATIONS.YELLOWALERT.NAME,
                                                            NotificationType.Bad,
                                                            (notificationList, data) =>
                                                                MISC.NOTIFICATIONS.YELLOWALERT.TOOLTIP,
                                                            null,
                                                            false);

        public Instance(IStateMachineTarget master) : base(master) { instance = this; }
        public static void     DestroyInstance() { instance                   = null; }
        public static Instance Get()             { return instance; }
        public        bool     IsOn()            { return sm.isOn.Get(smi); }

        public void HasTopPriorityChore(bool on) {
            hasTopPriorityChore = on;
            Refresh();
        }

        private void Refresh() { sm.isOn.Set(hasTopPriorityChore, smi); }
    }
}