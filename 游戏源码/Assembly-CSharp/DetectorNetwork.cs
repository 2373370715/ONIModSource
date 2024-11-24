using System;
using STRINGS;

// Token: 0x02000D1E RID: 3358
public class DetectorNetwork : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>
{
	// Token: 0x060041B6 RID: 16822 RVA: 0x0023EB18 File Offset: 0x0023CD18
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (DetectorNetwork.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.operational.InitializeStates(this).EventTransition(GameHashes.OperationalChanged, this.inoperational, (DetectorNetwork.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
	}

	// Token: 0x04002CCF RID: 11471
	public StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.FloatParameter networkQuality;

	// Token: 0x04002CD0 RID: 11472
	public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State inoperational;

	// Token: 0x04002CD1 RID: 11473
	public DetectorNetwork.NetworkStates operational;

	// Token: 0x02000D1F RID: 3359
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000D20 RID: 3360
	public class NetworkStates : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State
	{
		// Token: 0x060041B9 RID: 16825 RVA: 0x0023EBA0 File Offset: 0x0023CDA0
		public DetectorNetwork.NetworkStates InitializeStates(DetectorNetwork parent)
		{
			base.DefaultState(this.poor);
			GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State state = this.poor;
			string name = BUILDING.STATUSITEMS.NETWORKQUALITY.NAME;
			string tooltip = BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Exclamation;
			NotificationType notification_type = NotificationType.BadMinor;
			bool allow_multiples = false;
			Func<string, DetectorNetwork.Instance, string> resolve_string_callback = new Func<string, DetectorNetwork.Instance, string>(this.StringCallback);
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, resolve_string_callback, null, null).ParamTransition<float>(parent.networkQuality, this.good, (DetectorNetwork.Instance smi, float p) => (double)p >= 0.8);
			GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State state2 = this.good;
			string name2 = BUILDING.STATUSITEMS.NETWORKQUALITY.NAME;
			string tooltip2 = BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP;
			string icon2 = "";
			StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
			NotificationType notification_type2 = NotificationType.Neutral;
			bool allow_multiples2 = false;
			resolve_string_callback = new Func<string, DetectorNetwork.Instance, string>(this.StringCallback);
			state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, resolve_string_callback, null, null).ParamTransition<float>(parent.networkQuality, this.poor, (DetectorNetwork.Instance smi, float p) => (double)p < 0.8);
			return this;
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x0023ECA8 File Offset: 0x0023CEA8
		private string StringCallback(string str, DetectorNetwork.Instance smi)
		{
			MathUtil.MinMax detectTimeRangeForWorld = Game.Instance.spaceScannerNetworkManager.GetDetectTimeRangeForWorld(smi.GetMyWorldId());
			float num = Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(smi.GetMyWorldId());
			num = num.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(0f, 0.5f));
			return str.Replace("{TotalQuality}", GameUtil.GetFormattedPercent(smi.GetNetworkQuality01() * 100f, GameUtil.TimeSlice.None)).Replace("{WorstTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.min, "F0")).Replace("{BestTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.max, "F0")).Replace("{Coverage}", GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None));
		}

		// Token: 0x04002CD2 RID: 11474
		public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State poor;

		// Token: 0x04002CD3 RID: 11475
		public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State good;
	}

	// Token: 0x02000D22 RID: 3362
	public new class Instance : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.GameInstance
	{
		// Token: 0x060041C0 RID: 16832 RVA: 0x000CA8A7 File Offset: 0x000C8AA7
		public Instance(IStateMachineTarget master, DetectorNetwork.Def def) : base(master, def)
		{
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x000CA8B1 File Offset: 0x000C8AB1
		public override void StartSM()
		{
			this.worldId = base.master.gameObject.GetMyWorldId();
			Components.DetectorNetworks.Add(this.worldId, this);
			base.StartSM();
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x000CA8E0 File Offset: 0x000C8AE0
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Components.DetectorNetworks.Remove(this.worldId, this);
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x000CA8FA File Offset: 0x000C8AFA
		public void Internal_SetNetworkQuality(float quality01)
		{
			base.sm.networkQuality.Set(quality01, base.smi, false);
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x000CA915 File Offset: 0x000C8B15
		public float GetNetworkQuality01()
		{
			return base.sm.networkQuality.Get(base.smi);
		}

		// Token: 0x04002CD7 RID: 11479
		[NonSerialized]
		private int worldId;
	}
}
