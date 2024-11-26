namespace Geyser_Control {
    internal class DormancyButton : KMonoBehaviour, ISidescreenButtonControl {
        [MyCmpReq]
        private readonly Geyser geyser;

        [MyCmpReq]
        private readonly KSelectable kSelectable;

        [MyCmpReq]
        private readonly Studyable studyable;

        public string SidescreenButtonText =>
            IsDormant(geyser) ? STRINGS.BUTTONS.DORMANCYBUTTON.EXIT.NAME : STRINGS.BUTTONS.DORMANCYBUTTON.ENTER.NAME;

        public string SidescreenButtonTooltip {
            get {
                if (!studyable.Studied) return STRINGS.BUTTONS.DISABLED.TOOLTIP;

                return IsDormant(geyser)
                           ? STRINGS.BUTTONS.DORMANCYBUTTON.EXIT.TOOLTIP
                           : STRINGS.BUTTONS.DORMANCYBUTTON.ENTER.TOOLTIP;
            }
        }

        public void                   SetButtonTextOverride(ButtonMenuTextOverride textOverride) { }
        bool ISidescreenButtonControl.SidescreenEnabled() { return true; }
        public bool                   SidescreenButtonInteractable() { return studyable.Studied; }

        public void OnSidescreenButtonPressed() {
            if (!IsDormant(geyser)) {
                geyser.ShiftTimeTo(Geyser.TimeShiftStep.DormantState);
                geyser.smi.GoTo(geyser.smi.sm.dormant);
            } else {
                geyser.ShiftTimeTo(Geyser.TimeShiftStep.ActiveState);
                geyser.smi.GoTo(geyser.smi.sm.idle);
            }

            if (kSelectable.IsSelected) {
                SelectTool.Instance.Select(null);
                SelectTool.Instance.Select(kSelectable);
            }
        }

        public int  HorizontalGroupID()         { return -1; }
        public int  ButtonSideScreenSortOrder() { return 2; }
        public bool IsDormant(Geyser geyser)    { return geyser.smi.GetCurrentState() == geyser.smi.sm.dormant; }
    }
}