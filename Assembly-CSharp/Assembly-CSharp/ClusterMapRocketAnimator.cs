﻿using System;
using UnityEngine;

public class ClusterMapRocketAnimator : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer>
{
		public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.idle;
		this.root.Transition(null, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.entityTarget.IsNull), UpdateRate.SIM_200ms).Target(this.entityTarget).EventHandlerTransition(GameHashes.RocketSelfDestructRequested, this.exploding, (ClusterMapRocketAnimator.StatesInstance smi, object data) => true).EventHandlerTransition(GameHashes.StartMining, this.utility.mining, (ClusterMapRocketAnimator.StatesInstance smi, object data) => true).EventHandlerTransition(GameHashes.RocketLaunched, this.moving.takeoff, (ClusterMapRocketAnimator.StatesInstance smi, object data) => true);
		this.idle.Target(this.masterTarget).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("idle_loop", KAnim.PlayMode.Loop);
		}).Target(this.entityTarget).Transition(this.moving.traveling, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling), UpdateRate.SIM_200ms).Transition(this.grounded, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsGrounded), UpdateRate.SIM_200ms).Transition(this.moving.landing, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsLanding), UpdateRate.SIM_200ms).Transition(this.utility.mining, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsMining), UpdateRate.SIM_200ms);
		this.grounded.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(false, smi);
			smi.ToggleVisAnim(false);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
			smi.ToggleVisAnim(true);
		}).Target(this.entityTarget).EventTransition(GameHashes.RocketLaunched, this.moving.takeoff, null);
		this.moving.takeoff.Transition(this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("launching", KAnim.PlayMode.Loop);
			this.ToggleSelectable(false, smi);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
		});
		this.moving.landing.Transition(this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("landing", KAnim.PlayMode.Loop);
			this.ToggleSelectable(false, smi);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
		});
		this.moving.traveling.DefaultState(this.moving.traveling.regular).Target(this.entityTarget).EventTransition(GameHashes.ClusterLocationChanged, this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling))).EventTransition(GameHashes.ClusterDestinationChanged, this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)));
		this.moving.traveling.regular.Target(this.entityTarget).Transition(this.moving.traveling.boosted, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsBoosted), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop);
		});
		this.moving.traveling.boosted.Target(this.entityTarget).Transition(this.moving.traveling.regular, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsBoosted)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("boosted", KAnim.PlayMode.Loop);
		});
		this.utility.Target(this.masterTarget).EventTransition(GameHashes.ClusterDestinationChanged, this.idle, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling));
		this.utility.mining.DefaultState(this.utility.mining.pre).Target(this.entityTarget).EventTransition(GameHashes.StopMining, this.utility.mining.pst, null);
		this.utility.mining.pre.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_pre", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.utility.mining.loop);
			});
		});
		this.utility.mining.loop.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_loop", KAnim.PlayMode.Loop);
		});
		this.utility.mining.pst.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_pst", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.idle);
			});
		});
		this.exploding.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("rocket_self_destruct_kanim")
			});
			smi.PlayVisAnim("explode", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.exploding_pst);
			});
		});
		this.exploding_pst.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().Stop();
			smi.entity.gameObject.Trigger(-1311384361, null);
		});
	}

		private bool ClusterChangedAtMyLocation(ClusterMapRocketAnimator.StatesInstance smi, object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		return clusterLocationChangedEvent.oldLocation == smi.entity.Location || clusterLocationChangedEvent.newLocation == smi.entity.Location;
	}

		private bool IsTraveling(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return smi.entity.GetComponent<ClusterTraveler>().IsTraveling() && ((Clustercraft)smi.entity).HasResourcesToMove(1, Clustercraft.CombustionResource.All);
	}

		private bool IsBoosted(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).controlStationBuffTimeRemaining > 0f;
	}

		private bool IsGrounded(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).Status == Clustercraft.CraftStatus.Grounded;
	}

		private bool IsLanding(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).Status == Clustercraft.CraftStatus.Landing;
	}

		private bool IsMining(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).HasTag(GameTags.POIHarvesting);
	}

		private bool IsSurfaceTransitioning(ClusterMapRocketAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && (clustercraft.Status == Clustercraft.CraftStatus.Landing || clustercraft.Status == Clustercraft.CraftStatus.Launching);
	}

		private void ToggleSelectable(bool isSelectable, ClusterMapRocketAnimator.StatesInstance smi)
	{
		if (smi.entity.IsNullOrDestroyed())
		{
			return;
		}
		KSelectable component = smi.entity.GetComponent<KSelectable>();
		component.IsSelectable = isSelectable;
		if (!isSelectable && component.IsSelected && ClusterMapScreen.Instance.GetMode() != ClusterMapScreen.Mode.SelectDestination)
		{
			ClusterMapSelectTool.Instance.Select(null, true);
			SelectTool.Instance.Select(null, true);
		}
	}

		public StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;

		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State grounded;

		public ClusterMapRocketAnimator.MovingStates moving;

		public ClusterMapRocketAnimator.UtilityStates utility;

		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State exploding;

		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State exploding_pst;

		public class TravelingStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
				public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State regular;

				public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State boosted;
	}

		public class MovingStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
				public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State takeoff;

				public ClusterMapRocketAnimator.TravelingStates traveling;

				public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State landing;
	}

		public class UtilityStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
				public ClusterMapRocketAnimator.UtilityStates.MiningStates mining;

				public class MiningStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
		{
						public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State pre;

						public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State loop;

						public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State pst;
		}
	}

		public class StatesInstance : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
				public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

				public void PlayVisAnim(string animName, KAnim.PlayMode playMode)
		{
			base.GetComponent<ClusterMapVisualizer>().PlayAnim(animName, playMode);
		}

				public void ToggleVisAnim(bool on)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			if (!on)
			{
				component.GetFirstAnimController().Play("grounded", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

				public void SubscribeOnVisAnimComplete(Action<object> action)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			this.UnsubscribeOnVisAnimComplete();
			this.animCompleteSubscriber = component.GetFirstAnimController().gameObject;
			this.animCompleteHandle = this.animCompleteSubscriber.Subscribe(-1061186183, action);
		}

				public void UnsubscribeOnVisAnimComplete()
		{
			if (this.animCompleteHandle != -1)
			{
				DebugUtil.DevAssert(this.animCompleteSubscriber != null, "ClusterMapRocketAnimator animCompleteSubscriber GameObject is null. Whatever the previous gameObject in this variable was, it may not have unsubscribed from an event properly", null);
				this.animCompleteSubscriber.Unsubscribe(this.animCompleteHandle);
				this.animCompleteHandle = -1;
			}
		}

				protected override void OnCleanUp()
		{
			base.OnCleanUp();
			this.UnsubscribeOnVisAnimComplete();
		}

				public ClusterGridEntity entity;

				private int animCompleteHandle = -1;

				private GameObject animCompleteSubscriber;
	}
}
