using System.Collections.Generic;
using RsLib;
using STRINGS;
using UnityEngine;

namespace RsTransferPort;

public class MyHighEnergyParticleDirectionSideScreen : RsSideScreenContent {
    [RsSideScreen.CopyField]
    private KButton activeButton;

    [RsSideScreen.CopyField]
    public List<KButton> Buttons;

    [RsSideScreen.CopyField]
    public LocText directionLabel;

    private readonly string[] directionStrings = new string[8] {
        UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_N,
        UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_NW,
        UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_W,
        UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_SW,
        UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_S,
        UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_SE,
        UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_E,
        UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_NE
    };

    private         IMyHighEnergyParticleDirection target;
    public override string GetTitle() { return UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.TITLE; }

    protected override void OnSpawn() {
        base.OnSpawn();
        for (var index = 0; index < Buttons.Count; ++index) {
            var button = Buttons[index];
            button.onClick += (System.Action)(() => {
                                                  var num = Buttons.IndexOf(button);
                                                  if (activeButton != null) activeButton.isInteractable = true;
                                                  button.isInteractable = false;
                                                  activeButton          = button;
                                                  if (target == null) return;

                                                  target.Direction = EightDirectionUtil.AngleToDirection(num * 45);
                                                  Game.Instance.ForceOverlayUpdate(true);
                                                  Refresh();
                                              });
        }
    }

    public override int GetSideScreenSortOrder() { return 10; }

    public override bool IsValidForTarget(GameObject target) {
        return target.GetComponent<IMyHighEnergyParticleDirection>() != null;
    }

    public override void SetTarget(GameObject new_target) {
        if (RsUtil.IsNullOrDestroyed(new_target)) return;

        target = new_target.GetComponent<IMyHighEnergyParticleDirection>();
        Refresh();
    }

    private void Refresh() {
        var directionIndex = EightDirectionUtil.GetDirectionIndex(target.Direction);
        if (directionIndex >= 0 && directionIndex < Buttons.Count)
            Buttons[directionIndex].SignalClick(KKeyCode.Mouse0);
        else {
            if ((bool)(Object)activeButton) activeButton.isInteractable = true;
            activeButton = null;
        }

        directionLabel.SetText(string.Format(UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.SELECTED_DIRECTION,
                                             directionStrings[directionIndex]));
    }
}