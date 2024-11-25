using System.Collections.Generic;
using UnityEngine;

public class TransitionDriver {
    private static readonly Navigator.ActiveTransition emptyTransition = new Navigator.ActiveTransition();

    public static ObjectPool<Navigator.ActiveTransition> TransitionPool
        = new ObjectPool<Navigator.ActiveTransition>(() => new Navigator.ActiveTransition(), 128);

    private          Brain                         brain;
    private readonly Stack<InterruptOverrideLayer> interruptOverrideStack = new Stack<InterruptOverrideLayer>(8);
    private          bool                          isComplete;
    private          LoggerFS                      log;
    private          Navigator                     navigator;
    public           List<OverrideLayer>           overrideLayers = new List<OverrideLayer>();
    private          Vector3                       targetPos;
    public TransitionDriver(Navigator navigator) { log = new LoggerFS("TransitionDriver"); }
    public Navigator.ActiveTransition GetTransition { get; private set; }

    public void BeginTransition(Navigator navigator, NavGrid.Transition transition, float defaultSpeed) {
        var instance = TransitionPool.GetInstance();
        instance.Init(transition, defaultSpeed);
        BeginTransition(navigator, instance);
    }

    private void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition) {
        var flag = interruptOverrideStack.Count != 0;
        foreach (var overrideLayer in overrideLayers)
            if (!flag || !(overrideLayer is InterruptOverrideLayer))
                overrideLayer.BeginTransition(navigator, transition);

        this.navigator = navigator;
        GetTransition  = transition;
        isComplete     = false;
        var sceneLayer = navigator.sceneLayer;
        if (transition.navGridTransition.start == NavType.Tube || transition.navGridTransition.end == NavType.Tube)
            sceneLayer = Grid.SceneLayer.BuildingUse;
        else if (transition.navGridTransition.start == NavType.Solid &&
                 transition.navGridTransition.end   == NavType.Solid) {
            sceneLayer = Grid.SceneLayer.FXFront;
            navigator.animController.SetSceneLayer(sceneLayer);
        } else if (transition.navGridTransition.start == NavType.Solid ||
                   transition.navGridTransition.end   == NavType.Solid)
            navigator.animController.SetSceneLayer(sceneLayer);

        var cell = Grid.OffsetCell(Grid.PosToCell(navigator), transition.x, transition.y);
        targetPos = Grid.CellToPosCBC(cell, sceneLayer);
        if (transition.isLooping) {
            KAnimControllerBase animController = navigator.animController;
            animController.PlaySpeedMultiplier = transition.animSpeed;
            var flag2 = transition.preAnim != "";
            var flag3 = animController.CurrentAnim != null && animController.CurrentAnim.name == transition.anim;
            if (flag2 && animController.CurrentAnim != null && animController.CurrentAnim.name == transition.preAnim) {
                animController.ClearQueue();
                animController.Queue(transition.anim, KAnim.PlayMode.Loop);
            } else if (flag3) {
                if (animController.PlayMode != KAnim.PlayMode.Loop) {
                    animController.ClearQueue();
                    animController.Queue(transition.anim, KAnim.PlayMode.Loop);
                }
            } else if (flag2) {
                animController.Play(transition.preAnim);
                animController.Queue(transition.anim, KAnim.PlayMode.Loop);
            } else
                animController.Play(transition.anim, KAnim.PlayMode.Loop);
        } else if (transition.anim != null) {
            var animController2 = navigator.animController;
            animController2.PlaySpeedMultiplier = transition.animSpeed;
            animController2.Play(transition.anim);
            navigator.Subscribe(-1061186183, OnAnimComplete);
        }

        if (transition.navGridTransition.y != 0) {
            if (transition.navGridTransition.start == NavType.RightWall)
                navigator.facing.SetFacing(transition.navGridTransition.y < 0);
            else if (transition.navGridTransition.start == NavType.LeftWall)
                navigator.facing.SetFacing(transition.navGridTransition.y > 0);
        }

        if (transition.navGridTransition.x != 0) {
            if (transition.navGridTransition.start == NavType.Ceiling)
                navigator.facing.SetFacing(transition.navGridTransition.x > 0);
            else if (transition.navGridTransition.start != NavType.LeftWall &&
                     transition.navGridTransition.start != NavType.RightWall)
                navigator.facing.SetFacing(transition.navGridTransition.x < 0);
        }

        brain = navigator.GetComponent<Brain>();
    }

    public void UpdateTransition(float dt) {
        if (this.navigator == null) return;

        foreach (var overrideLayer in overrideLayers) {
            var flag  = interruptOverrideStack.Count != 0;
            var flag2 = overrideLayer is InterruptOverrideLayer;
            if (!flag || !flag2 || interruptOverrideStack.Peek() == overrideLayer)
                overrideLayer.UpdateTransition(navigator, GetTransition);
        }

        if (!isComplete && GetTransition.isCompleteCB != null) isComplete = GetTransition.isCompleteCB();
        if (brain != null) {
            var flag3 = isComplete;
        }

        if (GetTransition.isLooping) {
            var speed    = GetTransition.speed;
            var position = navigator.transform.GetPosition();
            var num      = Grid.PosToCell(position);
            if (GetTransition.x > 0) {
                position.x += dt * speed;
                if (position.x > targetPos.x) isComplete = true;
            } else if (GetTransition.x < 0) {
                position.x -= dt * speed;
                if (position.x < targetPos.x) isComplete = true;
            } else
                position.x = targetPos.x;

            if (GetTransition.y > 0) {
                position.y += dt * speed;
                if (position.y > targetPos.y) isComplete = true;
            } else if (GetTransition.y < 0) {
                position.y -= dt * speed;
                if (position.y < targetPos.y) isComplete = true;
            } else
                position.y = targetPos.y;

            navigator.transform.SetPosition(position);
            var num2 = Grid.PosToCell(position);
            if (num2 != num) navigator.Trigger(915392638, num2);
        }

        if (isComplete) {
            isComplete = false;
            var navigator = this.navigator;
            navigator.SetCurrentNavType(GetTransition.end);
            navigator.transform.SetPosition(targetPos);
            EndTransition();
            navigator.AdvancePath();
        }
    }

    public void EndTransition() {
        if (navigator != null) {
            interruptOverrideStack.Clear();
            foreach (var overrideLayer in overrideLayers) overrideLayer.EndTransition(navigator, GetTransition);
            navigator.animController.PlaySpeedMultiplier = 1f;
            navigator.Unsubscribe(-1061186183, OnAnimComplete);
            if (brain != null) brain.Resume("move_handler");
            TransitionPool.ReleaseInstance(GetTransition);
            GetTransition = null;
            navigator     = null;
            brain         = null;
        }
    }

    private void OnAnimComplete(object data) {
        if (navigator != null) navigator.Unsubscribe(-1061186183, OnAnimComplete);
        isComplete = true;
    }

    public static Navigator.ActiveTransition SwapTransitionWithEmpty(Navigator.ActiveTransition src) {
        var instance = TransitionPool.GetInstance();
        instance.Copy(src);
        src.Copy(emptyTransition);
        return instance;
    }

    public class OverrideLayer {
        public OverrideLayer(Navigator navigator) { }
        public virtual void Destroy()                                                                    { }
        public virtual void BeginTransition(Navigator  navigator, Navigator.ActiveTransition transition) { }
        public virtual void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition) { }
        public virtual void EndTransition(Navigator    navigator, Navigator.ActiveTransition transition) { }
    }

    public class InterruptOverrideLayer : OverrideLayer {
        protected TransitionDriver           driver;
        protected Navigator.ActiveTransition originalTransition;
        public InterruptOverrideLayer(Navigator navigator) : base(navigator) { driver = navigator.transitionDriver; }
        protected bool InterruptInProgress => originalTransition != null;

        public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition) {
            driver.interruptOverrideStack.Push(this);
            originalTransition = SwapTransitionWithEmpty(transition);
        }

        public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition) {
            if (!IsOverrideComplete()) return;

            driver.interruptOverrideStack.Pop();
            transition.Copy(originalTransition);
            TransitionPool.ReleaseInstance(originalTransition);
            originalTransition = null;
            EndTransition(navigator, transition);
            driver.BeginTransition(navigator, transition);
        }

        public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition) {
            base.EndTransition(navigator, transition);
            if (originalTransition == null) return;

            TransitionPool.ReleaseInstance(originalTransition);
            originalTransition = null;
        }

        protected virtual bool IsOverrideComplete() {
            return originalTransition                   != null &&
                   driver.interruptOverrideStack.Count  != 0    &&
                   driver.interruptOverrideStack.Peek() == this;
        }
    }
}