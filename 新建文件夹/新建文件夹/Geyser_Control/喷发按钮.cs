using KSerialization;

namespace Geyser_Control {
   internal class EruptionButton : KMonoBehaviour, ISidescreenButtonControl, ISim1000ms {
        [MyCmpReq]
        private readonly Geyser geyser;

        [MyCmpReq]
        private readonly KSelectable kSelectable;

        [MyCmpReq]
        private readonly Studyable studyable;

        [Serialize]
        private bool cooldown = true;

        [Serialize]
        private int runSimIterations;
        
        public string SidescreenButtonText => STRINGS.BUTTONS.ERUPTIONBUTTON.NAME;
        
        public string SidescreenButtonTooltip =>
            studyable.Studied ? STRINGS.BUTTONS.ERUPTIONBUTTON.TOOLTIP : STRINGS.BUTTONS.DISABLED.TOOLTIP;

        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }

        bool ISidescreenButtonControl.SidescreenEnabled() { return true; }

        public bool SidescreenButtonInteractable() { return studyable.Studied && cooldown; }

        public void OnSidescreenButtonPressed() {
            cooldown = false;
            if (IsDormant(geyser)) {
                geyser.ShiftTimeTo(Geyser.TimeShiftStep.ActiveState);
                geyser.smi.GoTo(geyser.smi.sm.idle);
                return;
            }

            geyser.ShiftTimeTo(Geyser.TimeShiftStep.NextIteration);
            geyser.smi.GoTo(geyser.smi.sm.pre_erupt);
        }

        public int HorizontalGroupID() { return -1; }

        public int ButtonSideScreenSortOrder() { return 2; }

        public void Sim1000ms(float dt) {
            runSimIterations++;
            if (runSimIterations < 4) return;

            runSimIterations = 0;
            cooldown         = true;
            if (kSelectable.IsSelected) {
                SelectTool.Instance.Select(null);
                SelectTool.Instance.Select(kSelectable);
            }
        }

        public bool IsDormant(Geyser geyser) { return geyser.smi.GetCurrentState() == geyser.smi.sm.dormant; }
    }
}