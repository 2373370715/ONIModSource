using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000CCF RID: 3279
public class ClusterCometDetector : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>
{
	// Token: 0x06003F76 RID: 16246 RVA: 0x00237878 File Offset: 0x00235A78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Enter(delegate(ClusterCometDetector.Instance smi)
		{
			smi.UpdateDetectionState(this.lastIsTargetDetected.Get(smi), true);
			smi.remainingSecondsToFreezeLogicSignal = 3f;
		}).Update(delegate(ClusterCometDetector.Instance smi, float deltaSeconds)
		{
			smi.remainingSecondsToFreezeLogicSignal -= deltaSeconds;
			if (smi.remainingSecondsToFreezeLogicSignal < 0f)
			{
				smi.remainingSecondsToFreezeLogicSignal = 0f;
				return;
			}
			smi.SetLogicSignal(this.lastIsTargetDetected.Get(smi));
		}, UpdateRate.SIM_200ms, false);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (ClusterCometDetector.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning, null).Enter("ToggleActive", delegate(ClusterCometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(true, false);
		}).Exit("ToggleActive", delegate(ClusterCometDetector.Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(false, false);
		});
		this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
		this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).TagTransition(GameTags.Detecting, this.on.working, false).Enter("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.UpdateDetectionState(smi.HasTag(GameTags.Detecting), false);
		}).Update("Scan Sky", delegate(ClusterCometDetector.Instance smi, float dt)
		{
			smi.ScanSky(false);
		}, UpdateRate.SIM_200ms, false);
		this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
		this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors, null).Enter("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.SetLogicSignal(true);
		}).Exit("UpdateLogic", delegate(ClusterCometDetector.Instance smi)
		{
			smi.SetLogicSignal(false);
		}).Update("Scan Sky", delegate(ClusterCometDetector.Instance smi, float dt)
		{
			smi.ScanSky(true);
		}, UpdateRate.SIM_200ms, false);
		this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
		this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (ClusterCometDetector.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Detecting, this.on.working.pst, true);
		this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop);
	}

	// Token: 0x04002B4E RID: 11086
	public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State off;

	// Token: 0x04002B4F RID: 11087
	public ClusterCometDetector.OnStates on;

	// Token: 0x04002B50 RID: 11088
	public StateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.BoolParameter lastIsTargetDetected;

	// Token: 0x02000CD0 RID: 3280
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000CD1 RID: 3281
	public class OnStates : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State
	{
		// Token: 0x04002B51 RID: 11089
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pre;

		// Token: 0x04002B52 RID: 11090
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State loop;

		// Token: 0x04002B53 RID: 11091
		public ClusterCometDetector.WorkingStates working;

		// Token: 0x04002B54 RID: 11092
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pst;
	}

	// Token: 0x02000CD2 RID: 3282
	public class WorkingStates : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State
	{
		// Token: 0x04002B55 RID: 11093
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pre;

		// Token: 0x04002B56 RID: 11094
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State loop;

		// Token: 0x04002B57 RID: 11095
		public GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.State pst;
	}

	// Token: 0x02000CD3 RID: 3283
	public new class Instance : GameStateMachine<ClusterCometDetector, ClusterCometDetector.Instance, IStateMachineTarget, ClusterCometDetector.Def>.GameInstance
	{
		// Token: 0x06003F7D RID: 16253 RVA: 0x000C93CB File Offset: 0x000C75CB
		public Instance(IStateMachineTarget master, ClusterCometDetector.Def def) : base(master, def)
		{
			this.detectorNetworkDef = new DetectorNetwork.Def();
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x000C93EB File Offset: 0x000C75EB
		public override void StartSM()
		{
			if (this.detectorNetwork == null)
			{
				this.detectorNetwork = (DetectorNetwork.Instance)this.detectorNetworkDef.CreateSMI(base.master);
			}
			this.detectorNetwork.StartSM();
			base.StartSM();
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x000C9422 File Offset: 0x000C7622
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			this.detectorNetwork.StopSM(reason);
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x00237C30 File Offset: 0x00235E30
		public void UpdateDetectionState(bool currentDetection, bool expectedDetectionForState)
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			if (currentDetection)
			{
				component.AddTag(GameTags.Detecting, false);
			}
			else
			{
				component.RemoveTag(GameTags.Detecting);
			}
			if (currentDetection == expectedDetectionForState)
			{
				this.SetLogicSignal(currentDetection);
			}
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x00237C6C File Offset: 0x00235E6C
		public void ScanSky(bool expectedDetectionForState)
		{
			Option<SpaceScannerTarget> option;
			switch (this.GetDetectorState())
			{
			case ClusterCometDetector.Instance.ClusterCometDetectorState.MeteorShower:
				option = SpaceScannerTarget.MeteorShower();
				break;
			case ClusterCometDetector.Instance.ClusterCometDetectorState.BallisticObject:
				option = SpaceScannerTarget.BallisticObject();
				break;
			case ClusterCometDetector.Instance.ClusterCometDetectorState.Rocket:
				if (this.targetCraft != null && this.targetCraft.Get() != null)
				{
					option = SpaceScannerTarget.RocketDlc1(this.targetCraft.Get());
				}
				else
				{
					option = Option.None;
				}
				break;
			default:
				throw new NotImplementedException();
			}
			bool flag = option.IsSome() && Game.Instance.spaceScannerNetworkManager.IsTargetDetectedOnWorld(this.GetMyWorldId(), option.Unwrap());
			base.smi.sm.lastIsTargetDetected.Set(flag, this, false);
			this.UpdateDetectionState(flag, expectedDetectionForState);
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x000C9437 File Offset: 0x000C7637
		public void SetLogicSignal(bool on)
		{
			base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
		}

		// Token: 0x06003F83 RID: 16259 RVA: 0x000C9450 File Offset: 0x000C7650
		public void SetDetectorState(ClusterCometDetector.Instance.ClusterCometDetectorState newState)
		{
			this.detectorState = newState;
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x000C9459 File Offset: 0x000C7659
		public ClusterCometDetector.Instance.ClusterCometDetectorState GetDetectorState()
		{
			return this.detectorState;
		}

		// Token: 0x06003F85 RID: 16261 RVA: 0x000C9461 File Offset: 0x000C7661
		public void SetClustercraftTarget(Clustercraft target)
		{
			if (target)
			{
				this.targetCraft = new Ref<Clustercraft>(target);
				return;
			}
			this.targetCraft = null;
		}

		// Token: 0x06003F86 RID: 16262 RVA: 0x000C947F File Offset: 0x000C767F
		public Clustercraft GetClustercraftTarget()
		{
			if (this.targetCraft == null)
			{
				return null;
			}
			return this.targetCraft.Get();
		}

		// Token: 0x04002B58 RID: 11096
		public bool ShowWorkingStatus;

		// Token: 0x04002B59 RID: 11097
		[Serialize]
		private ClusterCometDetector.Instance.ClusterCometDetectorState detectorState;

		// Token: 0x04002B5A RID: 11098
		[Serialize]
		private Ref<Clustercraft> targetCraft;

		// Token: 0x04002B5B RID: 11099
		[NonSerialized]
		public float remainingSecondsToFreezeLogicSignal;

		// Token: 0x04002B5C RID: 11100
		private DetectorNetwork.Def detectorNetworkDef;

		// Token: 0x04002B5D RID: 11101
		private DetectorNetwork.Instance detectorNetwork;

		// Token: 0x04002B5E RID: 11102
		private List<GameplayEventInstance> meteorShowers = new List<GameplayEventInstance>();

		// Token: 0x02000CD4 RID: 3284
		public enum ClusterCometDetectorState
		{
			// Token: 0x04002B60 RID: 11104
			MeteorShower,
			// Token: 0x04002B61 RID: 11105
			BallisticObject,
			// Token: 0x04002B62 RID: 11106
			Rocket
		}
	}
}
