using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020007BF RID: 1983
public class SparkleStreaker : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance>
{
	// Token: 0x0600238A RID: 9098 RVA: 0x001C58E4 File Offset: 0x001C3AE4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsSparkleStreaker").ToggleLoopingSound(this.soundPath, null, true, true, true).Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.sparkleStreakFX = Util.KInstantiate(EffectPrefabs.Instance.SparkleStreakFX, smi.master.transform.GetPosition() + this.offset);
			smi.sparkleStreakFX.transform.SetParent(smi.master.transform);
			smi.sparkleStreakFX.SetActive(true);
			smi.CreatePasserbyReactable();
		}).Exit(delegate(SparkleStreaker.Instance smi)
		{
			Util.KDestroyGameObject(smi.sparkleStreakFX);
			smi.ClearPasserbyReactable();
		});
		this.overjoyed.idle.Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.SetSparkleSoundParam(0f);
		}).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.moving, (SparkleStreaker.Instance smi) => smi.IsMoving());
		this.overjoyed.moving.Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.SetSparkleSoundParam(1f);
		}).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.idle, (SparkleStreaker.Instance smi) => !smi.IsMoving());
	}

	// Token: 0x0400177D RID: 6013
	private Vector3 offset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x0400177E RID: 6014
	public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x0400177F RID: 6015
	public SparkleStreaker.OverjoyedStates overjoyed;

	// Token: 0x04001780 RID: 6016
	public string soundPath = GlobalAssets.GetSound("SparkleStreaker_lp", false);

	// Token: 0x04001781 RID: 6017
	public HashedString SPARKLE_STREAKER_MOVING_PARAMETER = "sparkleStreaker_moving";

	// Token: 0x020007C0 RID: 1984
	public class OverjoyedStates : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04001782 RID: 6018
		public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04001783 RID: 6019
		public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State moving;
	}

	// Token: 0x020007C1 RID: 1985
	public new class Instance : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600238E RID: 9102 RVA: 0x000B7244 File Offset: 0x000B5444
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x001C5B20 File Offset: 0x001C3D20
		public void CreatePasserbyReactable()
		{
			if (this.passerbyReactable == null)
			{
				EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 0f, 600f, float.PositiveInfinity, 0f);
				Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
				emoteReactable.SetEmote(clapCheer).SetThought(Db.Get().Thoughts.Happy).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
				emoteReactable.RegisterEmoteStepCallbacks("clapcheer_pre", new Action<GameObject>(this.AddReactionEffect), null);
				this.passerbyReactable = emoteReactable;
			}
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x000B724D File Offset: 0x000B544D
		private void AddReactionEffect(GameObject reactor)
		{
			reactor.GetComponent<Effects>().Add("SawSparkleStreaker", true);
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x000B7124 File Offset: 0x000B5324
		private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
		{
			return transition.end == NavType.Floor;
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x000B7261 File Offset: 0x000B5461
		public void ClearPasserbyReactable()
		{
			if (this.passerbyReactable != null)
			{
				this.passerbyReactable.Cleanup();
				this.passerbyReactable = null;
			}
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x000B727D File Offset: 0x000B547D
		public bool IsMoving()
		{
			return base.smi.master.GetComponent<Navigator>().IsMoving();
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x000B7294 File Offset: 0x000B5494
		public void SetSparkleSoundParam(float val)
		{
			base.GetComponent<LoopingSounds>().SetParameter(GlobalAssets.GetSound("SparkleStreaker_lp", false), "sparkleStreaker_moving", val);
		}

		// Token: 0x04001784 RID: 6020
		private Reactable passerbyReactable;

		// Token: 0x04001785 RID: 6021
		public GameObject sparkleStreakFX;
	}
}
