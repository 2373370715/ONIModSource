using Database;
using UnityEngine;

public class BalloonFX : GameStateMachine<BalloonFX, BalloonFX.Instance>
{
	public new class Instance : GameInstance
	{
		private KBatchedAnimController balloonAnimController;

		private Option<BalloonOverrideSymbol> currentBodyOverrideSymbol;

		public Instance(IStateMachineTarget master)
			: base(master)
		{
			balloonAnimController = FXHelpers.CreateEffectOverride(new string[2] { "balloon_anim_kanim", "balloon_basic_red_kanim" }, master.gameObject.transform.GetPosition() + new Vector3(0f, 0.3f, 1f), master.transform, update_looping_sounds_position: true, Grid.SceneLayer.Creatures);
			base.sm.fx.Set(balloonAnimController.gameObject, base.smi);
			balloonAnimController.defaultAnim = "idle_default";
			master.GetComponent<KBatchedAnimController>().GetSynchronizer().Add(balloonAnimController.GetComponent<KBatchedAnimController>());
		}

		public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverride)
		{
			KAnimFile kAnimFile = (balloonOverride.animFile.IsSome() ? balloonOverride.animFile.Unwrap() : base.smi.sm.defaultBalloon);
			balloonAnimController.SwapAnims(new KAnimFile[2]
			{
				base.smi.sm.defaultAnim,
				kAnimFile
			});
			SymbolOverrideController component = balloonAnimController.GetComponent<SymbolOverrideController>();
			if (currentBodyOverrideSymbol.IsSome())
			{
				component.RemoveSymbolOverride("body");
			}
			if (balloonOverride.symbol.IsNone())
			{
				if (currentBodyOverrideSymbol.IsSome())
				{
					component.AddSymbolOverride("body", base.smi.sm.defaultAnim.GetData().build.GetSymbol("body"));
				}
				balloonAnimController.SetBatchGroupOverride(HashedString.Invalid);
			}
			else
			{
				component.AddSymbolOverride("body", balloonOverride.symbol.Unwrap());
				balloonAnimController.SetBatchGroupOverride(kAnimFile.batchTag);
			}
			currentBodyOverrideSymbol = balloonOverride;
		}

		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}
	}

	public TargetParameter fx;

	public KAnimFile defaultAnim = Assets.GetAnim("balloon_anim_kanim");

	private KAnimFile defaultBalloon = Assets.GetAnim("balloon_basic_red_kanim");

	private const string defaultAnimName = "balloon_anim_kanim";

	private const string balloonAnimName = "balloon_basic_red_kanim";

	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	private const int TARGET_OVERRIDE_PRIORITY = 0;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = root;
		Target(fx);
		root.Exit("DestroyFX", delegate(Instance smi)
		{
			smi.DestroyFX();
		});
	}
}
