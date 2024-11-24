using System;
using Database;
using UnityEngine;

// Token: 0x02000BE9 RID: 3049
public class BalloonFX : GameStateMachine<BalloonFX, BalloonFX.Instance>
{
	// Token: 0x06003A5C RID: 14940 RVA: 0x00227174 File Offset: 0x00225374
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.Exit("DestroyFX", delegate(BalloonFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x040027BC RID: 10172
	public StateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x040027BD RID: 10173
	public KAnimFile defaultAnim = Assets.GetAnim("balloon_anim_kanim");

	// Token: 0x040027BE RID: 10174
	private KAnimFile defaultBalloon = Assets.GetAnim("balloon_basic_red_kanim");

	// Token: 0x040027BF RID: 10175
	private const string defaultAnimName = "balloon_anim_kanim";

	// Token: 0x040027C0 RID: 10176
	private const string balloonAnimName = "balloon_basic_red_kanim";

	// Token: 0x040027C1 RID: 10177
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x040027C2 RID: 10178
	private const int TARGET_OVERRIDE_PRIORITY = 0;

	// Token: 0x02000BEA RID: 3050
	public new class Instance : GameStateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003A5E RID: 14942 RVA: 0x002271C8 File Offset: 0x002253C8
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.balloonAnimController = FXHelpers.CreateEffectOverride(new string[]
			{
				"balloon_anim_kanim",
				"balloon_basic_red_kanim"
			}, master.gameObject.transform.GetPosition() + new Vector3(0f, 0.3f, 1f), master.transform, true, Grid.SceneLayer.Creatures, false);
			base.sm.fx.Set(this.balloonAnimController.gameObject, base.smi, false);
			this.balloonAnimController.defaultAnim = "idle_default";
			master.GetComponent<KBatchedAnimController>().GetSynchronizer().Add(this.balloonAnimController.GetComponent<KBatchedAnimController>());
		}

		// Token: 0x06003A5F RID: 14943 RVA: 0x00227280 File Offset: 0x00225480
		public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverride)
		{
			KAnimFile kanimFile = balloonOverride.animFile.IsSome() ? balloonOverride.animFile.Unwrap() : base.smi.sm.defaultBalloon;
			this.balloonAnimController.SwapAnims(new KAnimFile[]
			{
				base.smi.sm.defaultAnim,
				kanimFile
			});
			SymbolOverrideController component = this.balloonAnimController.GetComponent<SymbolOverrideController>();
			if (this.currentBodyOverrideSymbol.IsSome())
			{
				component.RemoveSymbolOverride("body", 0);
			}
			if (balloonOverride.symbol.IsNone())
			{
				if (this.currentBodyOverrideSymbol.IsSome())
				{
					component.AddSymbolOverride("body", base.smi.sm.defaultAnim.GetData().build.GetSymbol("body"), 0);
				}
				this.balloonAnimController.SetBatchGroupOverride(HashedString.Invalid);
			}
			else
			{
				component.AddSymbolOverride("body", balloonOverride.symbol.Unwrap(), 0);
				this.balloonAnimController.SetBatchGroupOverride(kanimFile.batchTag);
			}
			this.currentBodyOverrideSymbol = balloonOverride;
		}

		// Token: 0x06003A60 RID: 14944 RVA: 0x000C5AC9 File Offset: 0x000C3CC9
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x040027C3 RID: 10179
		private KBatchedAnimController balloonAnimController;

		// Token: 0x040027C4 RID: 10180
		private Option<BalloonOverrideSymbol> currentBodyOverrideSymbol;
	}
}
