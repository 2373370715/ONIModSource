﻿using System;
using UnityEngine;

public class FXAnim : GameStateMachine<FXAnim, FXAnim.Instance>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		base.Target(this.fx);
		this.loop.Enter(delegate(FXAnim.Instance smi)
		{
			smi.Enter();
		}).EventTransition(GameHashes.AnimQueueComplete, this.restart, null).Exit("Post", delegate(FXAnim.Instance smi)
		{
			smi.Exit();
		});
		this.restart.GoTo(this.loop);
	}

		public StateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.TargetParameter fx;

		public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State loop;

		public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State restart;

		public new class Instance : GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.GameInstance
	{
				public Instance(IStateMachineTarget master, string kanim_file, string anim, KAnim.PlayMode mode, Vector3 offset, Color32 tint_colour) : base(master)
		{
			this.animController = FXHelpers.CreateEffect(kanim_file, base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			this.animController.gameObject.Subscribe(-1061186183, new Action<object>(this.OnAnimQueueComplete));
			this.animController.TintColour = tint_colour;
			base.sm.fx.Set(this.animController.gameObject, base.smi, false);
			this.anim = anim;
			this.mode = mode;
		}

				public void Enter()
		{
			this.animController.Play(this.anim, this.mode, 1f, 0f);
		}

				public void Exit()
		{
			this.DestroyFX();
		}

				private void OnAnimQueueComplete(object data)
		{
			this.DestroyFX();
		}

				private void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

				private string anim;

				private KAnim.PlayMode mode;

				private KBatchedAnimController animController;
	}
}
