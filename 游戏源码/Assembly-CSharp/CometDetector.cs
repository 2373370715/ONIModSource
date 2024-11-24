using System;
using System.Collections.Generic;
using KSerialization;

public class CometDetector : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>
{
	public class Def : BaseDef
	{
	}

	public class OnStates : State
	{
		public State pre;

		public State loop;

		public WorkingStates working;

		public State pst;
	}

	public class WorkingStates : State
	{
		public State pre;

		public State loop;

		public State pst;
	}

	public new class Instance : GameInstance
	{
		public bool ShowWorkingStatus;

		[Serialize]
		private Ref<LaunchConditionManager> targetCraft;

		[NonSerialized]
		public float remainingSecondsToFreezeLogicSignal;

		private DetectorNetwork.Def detectorNetworkDef;

		private DetectorNetwork.Instance detectorNetwork;

		private List<GameplayEventInstance> meteorShowers = new List<GameplayEventInstance>();

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			detectorNetworkDef = new DetectorNetwork.Def();
			targetCraft = new Ref<LaunchConditionManager>();
		}

		public override void StartSM()
		{
			if (detectorNetwork == null)
			{
				detectorNetwork = (DetectorNetwork.Instance)detectorNetworkDef.CreateSMI(base.master);
			}
			detectorNetwork.StartSM();
			base.StartSM();
		}

		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			detectorNetwork.StopSM(reason);
		}

		public void UpdateDetectionState(bool currentDetection, bool expectedDetectionForState)
		{
			KPrefabID component = GetComponent<KPrefabID>();
			if (currentDetection)
			{
				component.AddTag(GameTags.Detecting);
			}
			else
			{
				component.RemoveTag(GameTags.Detecting);
			}
			if (currentDetection == expectedDetectionForState)
			{
				SetLogicSignal(currentDetection);
			}
		}

		public void ScanSky(bool expectedDetectionForState)
		{
			LaunchConditionManager launchConditionManager = targetCraft.Get();
			Option<SpaceScannerTarget> option = ((launchConditionManager == null) ? ((Option<SpaceScannerTarget>)SpaceScannerTarget.MeteorShower()) : ((SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(targetCraft.Get()).state != Spacecraft.MissionState.Destroyed) ? ((Option<SpaceScannerTarget>)SpaceScannerTarget.RocketBaseGame(launchConditionManager)) : ((Option<SpaceScannerTarget>)Option.None)));
			bool flag = option.IsSome() && Game.Instance.spaceScannerNetworkManager.IsTargetDetectedOnWorld(this.GetMyWorldId(), option.Unwrap());
			base.smi.sm.lastIsTargetDetected.Set(flag, this);
			UpdateDetectionState(flag, expectedDetectionForState);
		}

		public void SetLogicSignal(bool on)
		{
			GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);
		}

		public void SetTargetCraft(LaunchConditionManager target)
		{
			targetCraft.Set(target);
		}

		public LaunchConditionManager GetTargetCraft()
		{
			return targetCraft.Get();
		}
	}

	public State off;

	public OnStates on;

	public BoolParameter lastIsTargetDetected;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = off;
		base.serializable = SerializeType.ParamsOnly;
		root.Enter(delegate(Instance smi)
		{
			smi.UpdateDetectionState(lastIsTargetDetected.Get(smi), expectedDetectionForState: true);
			smi.remainingSecondsToFreezeLogicSignal = 3f;
		}).Update(delegate(Instance smi, float deltaSeconds)
		{
			smi.remainingSecondsToFreezeLogicSignal -= deltaSeconds;
			if (smi.remainingSecondsToFreezeLogicSignal < 0f)
			{
				smi.remainingSecondsToFreezeLogicSignal = 0f;
			}
			else
			{
				smi.SetLogicSignal(lastIsTargetDetected.Get(smi));
			}
		});
		off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, on, (Instance smi) => smi.GetComponent<Operational>().IsOperational);
		on.DefaultState(on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning).Enter("ToggleActive", delegate(Instance smi)
		{
			smi.GetComponent<Operational>().SetActive(value: true);
		})
			.Exit("ToggleActive", delegate(Instance smi)
			{
				smi.GetComponent<Operational>().SetActive(value: false);
			});
		on.pre.PlayAnim("on_pre").OnAnimQueueComplete(on.loop);
		on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, on.pst, (Instance smi) => !smi.GetComponent<Operational>().IsOperational).TagTransition(GameTags.Detecting, on.working)
			.Enter("UpdateLogic", delegate(Instance smi)
			{
				smi.UpdateDetectionState(smi.HasTag(GameTags.Detecting), expectedDetectionForState: false);
			})
			.Update("Scan Sky", delegate(Instance smi, float dt)
			{
				smi.ScanSky(expectedDetectionForState: false);
			});
		on.pst.PlayAnim("on_pst").OnAnimQueueComplete(off);
		on.working.DefaultState(on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors).Enter("UpdateLogic", delegate(Instance smi)
		{
			smi.SetLogicSignal(on: true);
		})
			.Exit("UpdateLogic", delegate(Instance smi)
			{
				smi.SetLogicSignal(on: false);
			})
			.Update("Scan Sky", delegate(Instance smi, float dt)
			{
				smi.ScanSky(expectedDetectionForState: true);
			});
		on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(on.working.loop);
		on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, on.working.pst, (Instance smi) => !smi.GetComponent<Operational>().IsOperational).EventTransition(GameHashes.ActiveChanged, on.working.pst, (Instance smi) => !smi.GetComponent<Operational>().IsActive)
			.TagTransition(GameTags.Detecting, on.working.pst, on_remove: true);
		on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(on.loop);
	}
}
