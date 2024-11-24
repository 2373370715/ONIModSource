using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001A1F RID: 6687
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Vent")]
public class Vent : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x1700091F RID: 2335
	// (get) Token: 0x06008B76 RID: 35702 RVA: 0x000FB29E File Offset: 0x000F949E
	// (set) Token: 0x06008B77 RID: 35703 RVA: 0x000FB2A6 File Offset: 0x000F94A6
	public int SortKey
	{
		get
		{
			return this.sortKey;
		}
		set
		{
			this.sortKey = value;
		}
	}

	// Token: 0x06008B78 RID: 35704 RVA: 0x0035FD5C File Offset: 0x0035DF5C
	public void UpdateVentedMass(SimHashes element, float mass)
	{
		if (!this.lifeTimeVentMass.ContainsKey(element))
		{
			this.lifeTimeVentMass.Add(element, mass);
			return;
		}
		Dictionary<SimHashes, float> dictionary = this.lifeTimeVentMass;
		dictionary[element] += mass;
	}

	// Token: 0x06008B79 RID: 35705 RVA: 0x000FB2AF File Offset: 0x000F94AF
	public float GetVentedMass(SimHashes element)
	{
		if (this.lifeTimeVentMass.ContainsKey(element))
		{
			return this.lifeTimeVentMass[element];
		}
		return 0f;
	}

	// Token: 0x06008B7A RID: 35706 RVA: 0x0035FDA0 File Offset: 0x0035DFA0
	public bool Closed()
	{
		bool flag = false;
		return (this.operational.Flags.TryGetValue(LogicOperationalController.LogicOperationalFlag, out flag) && !flag) || (this.operational.Flags.TryGetValue(BuildingEnabledButton.EnabledFlag, out flag) && !flag);
	}

	// Token: 0x06008B7B RID: 35707 RVA: 0x0035FDEC File Offset: 0x0035DFEC
	protected override void OnSpawn()
	{
		Building component = base.GetComponent<Building>();
		this.cell = component.GetUtilityOutputCell();
		this.smi = new Vent.StatesInstance(this);
		this.smi.StartSM();
	}

	// Token: 0x06008B7C RID: 35708 RVA: 0x0035FE24 File Offset: 0x0035E024
	public Vent.State GetEndPointState()
	{
		Vent.State result = Vent.State.Invalid;
		Endpoint endpoint = this.endpointType;
		if (endpoint != Endpoint.Source)
		{
			if (endpoint == Endpoint.Sink)
			{
				result = Vent.State.Ready;
				int num = this.cell;
				if (!this.IsValidOutputCell(num))
				{
					result = (Grid.Solid[num] ? Vent.State.Blocked : Vent.State.OverPressure);
				}
			}
		}
		else
		{
			result = (this.IsConnected() ? Vent.State.Ready : Vent.State.Blocked);
		}
		return result;
	}

	// Token: 0x06008B7D RID: 35709 RVA: 0x0035FE78 File Offset: 0x0035E078
	public bool IsConnected()
	{
		UtilityNetwork networkForCell = Conduit.GetNetworkManager(this.conduitType).GetNetworkForCell(this.cell);
		return networkForCell != null && (networkForCell as FlowUtilityNetwork).HasSinks;
	}

	// Token: 0x17000920 RID: 2336
	// (get) Token: 0x06008B7E RID: 35710 RVA: 0x000FB2D1 File Offset: 0x000F94D1
	public bool IsBlocked
	{
		get
		{
			return this.GetEndPointState() != Vent.State.Ready;
		}
	}

	// Token: 0x06008B7F RID: 35711 RVA: 0x0035FEAC File Offset: 0x0035E0AC
	private bool IsValidOutputCell(int output_cell)
	{
		bool result = false;
		if ((this.structure == null || !this.structure.IsEntombed() || !this.Closed()) && !Grid.Solid[output_cell])
		{
			result = (Grid.Mass[output_cell] < this.overpressureMass);
		}
		return result;
	}

	// Token: 0x06008B80 RID: 35712 RVA: 0x0035FF00 File Offset: 0x0035E100
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		string formattedMass = GameUtil.GetFormattedMass(this.overpressureMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVER_PRESSURE_MASS, formattedMass), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.OVER_PRESSURE_MASS, formattedMass), Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x040068D9 RID: 26841
	private int cell = -1;

	// Token: 0x040068DA RID: 26842
	private int sortKey;

	// Token: 0x040068DB RID: 26843
	[Serialize]
	public Dictionary<SimHashes, float> lifeTimeVentMass = new Dictionary<SimHashes, float>();

	// Token: 0x040068DC RID: 26844
	private Vent.StatesInstance smi;

	// Token: 0x040068DD RID: 26845
	[SerializeField]
	public ConduitType conduitType = ConduitType.Gas;

	// Token: 0x040068DE RID: 26846
	[SerializeField]
	public Endpoint endpointType;

	// Token: 0x040068DF RID: 26847
	[SerializeField]
	public float overpressureMass = 1f;

	// Token: 0x040068E0 RID: 26848
	[NonSerialized]
	public bool showConnectivityIcons = true;

	// Token: 0x040068E1 RID: 26849
	[MyCmpGet]
	[NonSerialized]
	public Structure structure;

	// Token: 0x040068E2 RID: 26850
	[MyCmpGet]
	[NonSerialized]
	public Operational operational;

	// Token: 0x02001A20 RID: 6688
	public enum State
	{
		// Token: 0x040068E4 RID: 26852
		Invalid,
		// Token: 0x040068E5 RID: 26853
		Ready,
		// Token: 0x040068E6 RID: 26854
		Blocked,
		// Token: 0x040068E7 RID: 26855
		OverPressure,
		// Token: 0x040068E8 RID: 26856
		Closed
	}

	// Token: 0x02001A21 RID: 6689
	public class StatesInstance : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.GameInstance
	{
		// Token: 0x06008B82 RID: 35714 RVA: 0x000FB312 File Offset: 0x000F9512
		public StatesInstance(Vent master) : base(master)
		{
			this.exhaust = master.GetComponent<Exhaust>();
		}

		// Token: 0x06008B83 RID: 35715 RVA: 0x000FB327 File Offset: 0x000F9527
		public bool NeedsExhaust()
		{
			return this.exhaust != null && base.master.GetEndPointState() != Vent.State.Ready && base.master.endpointType == Endpoint.Source;
		}

		// Token: 0x06008B84 RID: 35716 RVA: 0x000FB355 File Offset: 0x000F9555
		public bool Blocked()
		{
			return base.master.GetEndPointState() == Vent.State.Blocked && base.master.endpointType > Endpoint.Source;
		}

		// Token: 0x06008B85 RID: 35717 RVA: 0x000FB375 File Offset: 0x000F9575
		public bool OverPressure()
		{
			return this.exhaust != null && base.master.GetEndPointState() == Vent.State.OverPressure && base.master.endpointType > Endpoint.Source;
		}

		// Token: 0x06008B86 RID: 35718 RVA: 0x0035FF54 File Offset: 0x0035E154
		public void CheckTransitions()
		{
			if (this.NeedsExhaust())
			{
				base.smi.GoTo(base.sm.needExhaust);
				return;
			}
			if (base.master.Closed())
			{
				base.smi.GoTo(base.sm.closed);
				return;
			}
			if (this.Blocked())
			{
				base.smi.GoTo(base.sm.open.blocked);
				return;
			}
			if (this.OverPressure())
			{
				base.smi.GoTo(base.sm.open.overPressure);
				return;
			}
			base.smi.GoTo(base.sm.open.idle);
		}

		// Token: 0x06008B87 RID: 35719 RVA: 0x000FB3A3 File Offset: 0x000F95A3
		public StatusItem SelectStatusItem(StatusItem gas_status_item, StatusItem liquid_status_item)
		{
			if (base.master.conduitType != ConduitType.Gas)
			{
				return liquid_status_item;
			}
			return gas_status_item;
		}

		// Token: 0x040068E9 RID: 26857
		private Exhaust exhaust;
	}

	// Token: 0x02001A22 RID: 6690
	public class States : GameStateMachine<Vent.States, Vent.StatesInstance, Vent>
	{
		// Token: 0x06008B88 RID: 35720 RVA: 0x00360008 File Offset: 0x0035E208
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.open.idle;
			this.root.Update("CheckTransitions", delegate(Vent.StatesInstance smi, float dt)
			{
				smi.CheckTransitions();
			}, UpdateRate.SIM_200ms, false);
			this.open.TriggerOnEnter(GameHashes.VentOpen, null);
			this.closed.TriggerOnEnter(GameHashes.VentClosed, null);
			this.open.blocked.ToggleStatusItem((Vent.StatesInstance smi) => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentObstructed, Db.Get().BuildingStatusItems.LiquidVentObstructed), null);
			this.open.overPressure.ToggleStatusItem((Vent.StatesInstance smi) => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, Db.Get().BuildingStatusItems.LiquidVentOverPressure), null);
		}

		// Token: 0x040068EA RID: 26858
		public Vent.States.OpenState open;

		// Token: 0x040068EB RID: 26859
		public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State closed;

		// Token: 0x040068EC RID: 26860
		public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State needExhaust;

		// Token: 0x02001A23 RID: 6691
		public class OpenState : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State
		{
			// Token: 0x040068ED RID: 26861
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State idle;

			// Token: 0x040068EE RID: 26862
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State blocked;

			// Token: 0x040068EF RID: 26863
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State overPressure;
		}
	}
}
