using System.Collections.Generic;

public class KAnimSynchronizer {
    private readonly KAnimControllerBase               masterController;
    private readonly List<KAnimSynchronizedController> SyncedControllers = new List<KAnimSynchronizedController>();
    private readonly List<KAnimControllerBase>         Targets           = new List<KAnimControllerBase>();
    public KAnimSynchronizer(KAnimControllerBase master_controller) { masterController = master_controller; }
    public  string IdleAnim                              { get; set; } = "idle_default";
    private void   Clear(KAnimControllerBase controller) { controller.Play(IdleAnim, KAnim.PlayMode.Loop); }
    public  void   Add(KAnimControllerBase   controller) { Targets.Add(controller); }

    public void Remove(KAnimControllerBase controller) {
        Clear(controller);
        Targets.Remove(controller);
    }

    public  void RemoveWithoutIdleAnim(KAnimControllerBase controller) { Targets.Remove(controller); }
    private void Clear(KAnimSynchronizedController controller) { controller.Play(IdleAnim, KAnim.PlayMode.Loop); }
    public  void Add(KAnimSynchronizedController controller) { SyncedControllers.Add(controller); }

    public void Remove(KAnimSynchronizedController controller) {
        Clear(controller);
        SyncedControllers.Remove(controller);
    }

    public void Clear() {
        foreach (var kanimControllerBase in Targets)
            if (!(kanimControllerBase == null) && kanimControllerBase.AnimFiles != null)
                Clear(kanimControllerBase);

        Targets.Clear();
        foreach (var kanimSynchronizedController in SyncedControllers)
            if (!(kanimSynchronizedController.synchronizedController == null) &&
                kanimSynchronizedController.synchronizedController.AnimFiles != null)
                Clear(kanimSynchronizedController);

        SyncedControllers.Clear();
    }

    public void Sync(KAnimControllerBase controller) {
        if (masterController == null) return;

        if (controller == null) return;

        var currentAnim = masterController.GetCurrentAnim();
        if (currentAnim != null                           &&
            !string.IsNullOrEmpty(controller.defaultAnim) &&
            !controller.HasAnimation(currentAnim.name)) {
            controller.Play(controller.defaultAnim, KAnim.PlayMode.Loop);
            return;
        }

        if (currentAnim == null) return;

        var mode        = masterController.GetMode();
        var playSpeed   = masterController.GetPlaySpeed();
        var elapsedTime = masterController.GetElapsedTime();
        controller.Play(currentAnim.name, mode, playSpeed, elapsedTime);
        var component = controller.GetComponent<Facing>();
        if (component != null) {
            var num = component.transform.GetPosition().x;
            num += masterController.FlipX ? -0.5f : 0.5f;
            component.Face(num);
            return;
        }

        controller.FlipX = masterController.FlipX;
        controller.FlipY = masterController.FlipY;
    }

    public void SyncController(KAnimSynchronizedController controller) {
        if (masterController == null) return;

        if (controller == null) return;

        var currentAnim = masterController.GetCurrentAnim();
        var s           = currentAnim != null ? currentAnim.name + controller.Postfix : string.Empty;
        if (!string.IsNullOrEmpty(controller.synchronizedController.defaultAnim) &&
            !controller.synchronizedController.HasAnimation(s)) {
            controller.Play(controller.synchronizedController.defaultAnim, KAnim.PlayMode.Loop);
            return;
        }

        if (currentAnim == null) return;

        var mode        = masterController.GetMode();
        var playSpeed   = masterController.GetPlaySpeed();
        var elapsedTime = masterController.GetElapsedTime();
        controller.Play(s, mode, playSpeed, elapsedTime);
        var component = controller.synchronizedController.GetComponent<Facing>();
        if (component != null) {
            var num = component.transform.GetPosition().x;
            num += masterController.FlipX ? -0.5f : 0.5f;
            component.Face(num);
            return;
        }

        controller.synchronizedController.FlipX = masterController.FlipX;
        controller.synchronizedController.FlipY = masterController.FlipY;
    }

    public void Sync() {
        for (var i = 0; i < Targets.Count; i++) {
            var controller = Targets[i];
            Sync(controller);
        }

        for (var j = 0; j < SyncedControllers.Count; j++) {
            var controller2 = SyncedControllers[j];
            SyncController(controller2);
        }
    }

    public void SyncTime() {
        var elapsedTime = masterController.GetElapsedTime();
        for (var i = 0; i < Targets.Count; i++) Targets[i].SetElapsedTime(elapsedTime);
        for (var j = 0; j < SyncedControllers.Count; j++)
            SyncedControllers[j].synchronizedController.SetElapsedTime(elapsedTime);
    }
}