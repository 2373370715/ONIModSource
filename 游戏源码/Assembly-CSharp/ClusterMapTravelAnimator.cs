using System;
using UnityEngine;

// Token: 0x0200196C RID: 6508
public class ClusterMapTravelAnimator : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x060087B7 RID: 34743 RVA: 0x00351564 File Offset: 0x0034F764
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.idle;
		this.root.OnTargetLost(this.entityTarget, null);
		this.idle.Target(this.entityTarget).Transition(this.grounded, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsGrounded), UpdateRate.SIM_200ms).Transition(this.surfaceTransitioning, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning), UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)).Target(this.masterTarget);
		this.grounded.Transition(this.surfaceTransitioning, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning), UpdateRate.SIM_200ms);
		this.surfaceTransitioning.Update(delegate(ClusterMapTravelAnimator.StatesInstance smi, float dt)
		{
			this.DoOrientToPath(smi);
		}, UpdateRate.SIM_200ms, false).Transition(this.repositioning, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms);
		this.repositioning.Transition(this.traveling.orientToIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoReposition), UpdateRate.RENDER_EVERY_TICK);
		this.traveling.DefaultState(this.traveling.orientToPath);
		this.traveling.travelIdle.Target(this.entityTarget).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling.orientToIdle, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling))).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling.orientToPath, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToPath))).EventTransition(GameHashes.ClusterLocationChanged, this.traveling.move, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoMove))).Target(this.masterTarget);
		this.traveling.orientToPath.Transition(this.traveling.travelIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToPath), UpdateRate.RENDER_EVERY_TICK).Target(this.entityTarget).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).Target(this.masterTarget);
		this.traveling.move.Transition(this.traveling.travelIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoMove), UpdateRate.RENDER_EVERY_TICK);
		this.traveling.orientToIdle.Transition(this.idle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToIdle), UpdateRate.RENDER_EVERY_TICK);
	}

	// Token: 0x060087B8 RID: 34744 RVA: 0x000F8C65 File Offset: 0x000F6E65
	private bool IsTraveling(ClusterMapTravelAnimator.StatesInstance smi)
	{
		return smi.entity.GetComponent<ClusterTraveler>().IsTraveling();
	}

	// Token: 0x060087B9 RID: 34745 RVA: 0x00351858 File Offset: 0x0034FA58
	private bool IsSurfaceTransitioning(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && (clustercraft.Status == Clustercraft.CraftStatus.Landing || clustercraft.Status == Clustercraft.CraftStatus.Launching);
	}

	// Token: 0x060087BA RID: 34746 RVA: 0x00351890 File Offset: 0x0034FA90
	private bool IsGrounded(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && clustercraft.Status == Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x060087BB RID: 34747 RVA: 0x003518C0 File Offset: 0x0034FAC0
	private bool DoReposition(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Vector3 position = ClusterGrid.Instance.GetPosition(smi.entity);
		return smi.MoveTowards(position, Time.unscaledDeltaTime);
	}

	// Token: 0x060087BC RID: 34748 RVA: 0x003518C0 File Offset: 0x0034FAC0
	private bool DoMove(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Vector3 position = ClusterGrid.Instance.GetPosition(smi.entity);
		return smi.MoveTowards(position, Time.unscaledDeltaTime);
	}

	// Token: 0x060087BD RID: 34749 RVA: 0x003518EC File Offset: 0x0034FAEC
	private bool DoOrientToPath(ClusterMapTravelAnimator.StatesInstance smi)
	{
		float pathAngle = smi.GetComponent<ClusterMapVisualizer>().GetPathAngle();
		return smi.RotateTowards(pathAngle, Time.unscaledDeltaTime);
	}

	// Token: 0x060087BE RID: 34750 RVA: 0x000F8C77 File Offset: 0x000F6E77
	private bool DoOrientToIdle(ClusterMapTravelAnimator.StatesInstance smi)
	{
		return smi.keepRotationOnIdle || smi.RotateTowards(0f, Time.unscaledDeltaTime);
	}

	// Token: 0x060087BF RID: 34751 RVA: 0x00351914 File Offset: 0x0034FB14
	private bool ClusterChangedAtMyLocation(ClusterMapTravelAnimator.StatesInstance smi, object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		return clusterLocationChangedEvent.oldLocation == smi.entity.Location || clusterLocationChangedEvent.newLocation == smi.entity.Location;
	}

	// Token: 0x04006656 RID: 26198
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;

	// Token: 0x04006657 RID: 26199
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State grounded;

	// Token: 0x04006658 RID: 26200
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State repositioning;

	// Token: 0x04006659 RID: 26201
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State surfaceTransitioning;

	// Token: 0x0400665A RID: 26202
	public ClusterMapTravelAnimator.TravelingStates traveling;

	// Token: 0x0400665B RID: 26203
	public StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x0200196D RID: 6509
	private class Tuning : TuningData<ClusterMapTravelAnimator.Tuning>
	{
		// Token: 0x0400665C RID: 26204
		public float visualizerTransitionSpeed = 1f;

		// Token: 0x0400665D RID: 26205
		public float visualizerRotationSpeed = 1f;
	}

	// Token: 0x0200196E RID: 6510
	public class TravelingStates : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x0400665E RID: 26206
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State travelIdle;

		// Token: 0x0400665F RID: 26207
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State orientToPath;

		// Token: 0x04006660 RID: 26208
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State move;

		// Token: 0x04006661 RID: 26209
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State orientToIdle;
	}

	// Token: 0x0200196F RID: 6511
	public class StatesInstance : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x060087C4 RID: 34756 RVA: 0x000F8CCB File Offset: 0x000F6ECB
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x060087C5 RID: 34757 RVA: 0x00351958 File Offset: 0x0034FB58
		public bool MoveTowards(Vector3 targetPosition, float dt)
		{
			RectTransform component = base.GetComponent<RectTransform>();
			ClusterMapVisualizer component2 = base.GetComponent<ClusterMapVisualizer>();
			Vector3 localPosition = component.GetLocalPosition();
			Vector3 vector = targetPosition - localPosition;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			float num = TuningData<ClusterMapTravelAnimator.Tuning>.Get().visualizerTransitionSpeed * dt;
			if (num < magnitude)
			{
				Vector3 b = normalized * num;
				component.SetLocalPosition(localPosition + b);
				component2.RefreshPathDrawing();
				return false;
			}
			component.SetLocalPosition(targetPosition);
			component2.RefreshPathDrawing();
			return true;
		}

		// Token: 0x060087C6 RID: 34758 RVA: 0x003519DC File Offset: 0x0034FBDC
		public bool RotateTowards(float targetAngle, float dt)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			float num = targetAngle - this.simpleAngle;
			if (num > 180f)
			{
				num -= 360f;
			}
			else if (num < -180f)
			{
				num += 360f;
			}
			float num2 = TuningData<ClusterMapTravelAnimator.Tuning>.Get().visualizerRotationSpeed * dt;
			if (num > 0f && num2 < num)
			{
				this.simpleAngle += num2;
				component.SetAnimRotation(this.simpleAngle);
				return false;
			}
			if (num < 0f && -num2 > num)
			{
				this.simpleAngle -= num2;
				component.SetAnimRotation(this.simpleAngle);
				return false;
			}
			this.simpleAngle = targetAngle;
			component.SetAnimRotation(this.simpleAngle);
			return true;
		}

		// Token: 0x04006662 RID: 26210
		public ClusterGridEntity entity;

		// Token: 0x04006663 RID: 26211
		private float simpleAngle;

		// Token: 0x04006664 RID: 26212
		public bool keepRotationOnIdle;
	}
}
