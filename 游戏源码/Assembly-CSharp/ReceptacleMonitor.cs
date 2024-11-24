using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020011C8 RID: 4552
[SkipSaveFileSerialization]
public class ReceptacleMonitor : StateMachineComponent<ReceptacleMonitor.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x06005CD2 RID: 23762 RVA: 0x000DC980 File Offset: 0x000DAB80
	public bool Replanted
	{
		get
		{
			return this.replanted;
		}
	}

	// Token: 0x06005CD3 RID: 23763 RVA: 0x000DC988 File Offset: 0x000DAB88
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06005CD4 RID: 23764 RVA: 0x000DC99B File Offset: 0x000DAB9B
	public PlantablePlot GetReceptacle()
	{
		return (PlantablePlot)base.smi.sm.receptacle.Get(base.smi);
	}

	// Token: 0x06005CD5 RID: 23765 RVA: 0x0029C924 File Offset: 0x0029AB24
	public void SetReceptacle(PlantablePlot plot = null)
	{
		if (plot == null)
		{
			base.smi.sm.receptacle.Set(null, base.smi, false);
			this.replanted = false;
		}
		else
		{
			base.smi.sm.receptacle.Set(plot, base.smi, false);
			this.replanted = true;
		}
		base.Trigger(-1636776682, null);
	}

	// Token: 0x06005CD6 RID: 23766 RVA: 0x0029C994 File Offset: 0x0029AB94
	public void Sim1000ms(float dt)
	{
		if (base.smi.sm.receptacle.Get(base.smi) == null)
		{
			base.smi.GoTo(base.smi.sm.wild);
			return;
		}
		Operational component = base.smi.sm.receptacle.Get(base.smi).GetComponent<Operational>();
		if (component == null)
		{
			base.smi.GoTo(base.smi.sm.operational);
			return;
		}
		if (component.IsOperational)
		{
			base.smi.GoTo(base.smi.sm.operational);
			return;
		}
		base.smi.GoTo(base.smi.sm.inoperational);
	}

	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06005CD7 RID: 23767 RVA: 0x000DC9BD File Offset: 0x000DABBD
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Receptacle
			};
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x06005CD8 RID: 23768 RVA: 0x0029CA68 File Offset: 0x0029AC68
	public string WiltStateString
	{
		get
		{
			string text = "";
			if (base.smi.IsInsideState(base.smi.sm.inoperational))
			{
				text += CREATURES.STATUSITEMS.RECEPTACLEINOPERATIONAL.NAME;
			}
			return text;
		}
	}

	// Token: 0x06005CD9 RID: 23769 RVA: 0x000DC9CA File Offset: 0x000DABCA
	public bool HasReceptacle()
	{
		return !base.smi.IsInsideState(base.smi.sm.wild);
	}

	// Token: 0x06005CDA RID: 23770 RVA: 0x000DC9EA File Offset: 0x000DABEA
	public bool HasOperationalReceptacle()
	{
		return base.smi.IsInsideState(base.smi.sm.operational);
	}

	// Token: 0x06005CDB RID: 23771 RVA: 0x000DCA07 File Offset: 0x000DAC07
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_RECEPTACLE, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_RECEPTACLE, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x040041B1 RID: 16817
	private bool replanted;

	// Token: 0x020011C9 RID: 4553
	public class StatesInstance : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.GameInstance
	{
		// Token: 0x06005CDD RID: 23773 RVA: 0x000DCA37 File Offset: 0x000DAC37
		public StatesInstance(ReceptacleMonitor master) : base(master)
		{
		}
	}

	// Token: 0x020011CA RID: 4554
	public class States : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor>
	{
		// Token: 0x06005CDE RID: 23774 RVA: 0x0029CAAC File Offset: 0x0029ACAC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.wild;
			base.serializable = StateMachine.SerializeType.Never;
			this.wild.TriggerOnEnter(GameHashes.ReceptacleOperational, null);
			this.inoperational.TriggerOnEnter(GameHashes.ReceptacleInoperational, null);
			this.operational.TriggerOnEnter(GameHashes.ReceptacleOperational, null);
		}

		// Token: 0x040041B2 RID: 16818
		public StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.ObjectParameter<SingleEntityReceptacle> receptacle;

		// Token: 0x040041B3 RID: 16819
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State wild;

		// Token: 0x040041B4 RID: 16820
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State inoperational;

		// Token: 0x040041B5 RID: 16821
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State operational;
	}
}
