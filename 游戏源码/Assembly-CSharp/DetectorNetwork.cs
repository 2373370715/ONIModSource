using System;
using STRINGS;

public class DetectorNetwork : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>
{
	public class Def : BaseDef
	{
	}

	public class NetworkStates : State
	{
		public State poor;

		public State good;

		public NetworkStates InitializeStates(DetectorNetwork parent)
		{
			DefaultState(poor);
			poor.ToggleStatusItem(BUILDING.STATUSITEMS.NETWORKQUALITY.NAME, BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP, "", StatusItem.IconType.Exclamation, NotificationType.BadMinor, allow_multiples: false, default(HashedString), 129022, StringCallback).ParamTransition(parent.networkQuality, good, (Instance smi, float p) => (double)p >= 0.8);
			good.ToggleStatusItem(BUILDING.STATUSITEMS.NETWORKQUALITY.NAME, BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, StringCallback).ParamTransition(parent.networkQuality, poor, (Instance smi, float p) => (double)p < 0.8);
			return this;
		}

		private string StringCallback(string str, Instance smi)
		{
			MathUtil.MinMax detectTimeRangeForWorld = Game.Instance.spaceScannerNetworkManager.GetDetectTimeRangeForWorld(smi.GetMyWorldId());
			float qualityForWorld = Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(smi.GetMyWorldId());
			qualityForWorld = qualityForWorld.Remap((min: 0f, max: 1f), (min: 0f, max: 0.5f));
			return str.Replace("{TotalQuality}", GameUtil.GetFormattedPercent(smi.GetNetworkQuality01() * 100f)).Replace("{WorstTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.min)).Replace("{BestTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.max))
				.Replace("{Coverage}", GameUtil.GetFormattedPercent(qualityForWorld * 100f));
		}
	}

	public new class Instance : GameInstance
	{
		[NonSerialized]
		private int worldId;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			worldId = base.master.gameObject.GetMyWorldId();
			Components.DetectorNetworks.Add(worldId, this);
			base.StartSM();
		}

		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Components.DetectorNetworks.Remove(worldId, this);
		}

		public void Internal_SetNetworkQuality(float quality01)
		{
			base.sm.networkQuality.Set(quality01, base.smi);
		}

		public float GetNetworkQuality01()
		{
			return base.sm.networkQuality.Get(base.smi);
		}
	}

	public FloatParameter networkQuality;

	public State inoperational;

	public NetworkStates operational;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = inoperational;
		inoperational.EventTransition(GameHashes.OperationalChanged, operational, (Instance smi) => smi.GetComponent<Operational>().IsOperational);
		operational.InitializeStates(this).EventTransition(GameHashes.OperationalChanged, inoperational, (Instance smi) => !smi.GetComponent<Operational>().IsOperational);
	}
}
