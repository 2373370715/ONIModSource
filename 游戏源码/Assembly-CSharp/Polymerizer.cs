using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F1E RID: 3870
[SerializationConfig(MemberSerialization.OptIn)]
public class Polymerizer : StateMachineComponent<Polymerizer.StatesInstance>
{
	// Token: 0x06004E00 RID: 19968 RVA: 0x00266AA0 File Offset: 0x00264CA0
	protected override void OnSpawn()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.plasticMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		this.oilMeter = new MeterController(component, "meter2_target", "meter2", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		component.SetSymbolVisiblity("meter_target", true);
		this.UpdateOilMeter();
		base.smi.StartSM();
		base.Subscribe<Polymerizer>(-1697596308, Polymerizer.OnStorageChangedDelegate);
	}

	// Token: 0x06004E01 RID: 19969 RVA: 0x00266B44 File Offset: 0x00264D44
	private void TryEmit()
	{
		GameObject gameObject = this.storage.FindFirst(this.emitTag);
		if (gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			this.UpdatePercentDone(component);
			this.TryEmit(component);
		}
	}

	// Token: 0x06004E02 RID: 19970 RVA: 0x00266B84 File Offset: 0x00264D84
	private void TryEmit(PrimaryElement primary_elem)
	{
		if (primary_elem.Mass >= this.emitMass)
		{
			this.plasticMeter.SetPositionPercent(0f);
			GameObject gameObject = this.storage.Drop(primary_elem.gameObject, true);
			Rotatable component = base.GetComponent<Rotatable>();
			Vector3 vector = component.transform.GetPosition() + component.GetRotatedOffset(this.emitOffset);
			int i = Grid.PosToCell(vector);
			if (Grid.Solid[i])
			{
				vector += component.GetRotatedOffset(Vector3.left);
			}
			gameObject.transform.SetPosition(vector);
			PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.exhaustElement);
			if (primaryElement != null)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(vector), primaryElement.ElementID, null, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, true, -1);
				primaryElement.Mass = 0f;
				primaryElement.ModifyDiseaseCount(int.MinValue, "Polymerizer.Exhaust");
			}
		}
	}

	// Token: 0x06004E03 RID: 19971 RVA: 0x00266C7C File Offset: 0x00264E7C
	private void UpdatePercentDone(PrimaryElement primary_elem)
	{
		float positionPercent = Mathf.Clamp01(primary_elem.Mass / this.emitMass);
		this.plasticMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06004E04 RID: 19972 RVA: 0x00266CA8 File Offset: 0x00264EA8
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		if (gameObject.HasTag(PolymerizerConfig.INPUT_ELEMENT_TAG))
		{
			this.UpdateOilMeter();
		}
	}

	// Token: 0x06004E05 RID: 19973 RVA: 0x00266CDC File Offset: 0x00264EDC
	private void UpdateOilMeter()
	{
		float num = 0f;
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject.HasTag(PolymerizerConfig.INPUT_ELEMENT_TAG))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				num += component.Mass;
			}
		}
		float positionPercent = Mathf.Clamp01(num / this.consumer.capacityKG);
		this.oilMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x0400362F RID: 13871
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x04003630 RID: 13872
	[SerializeField]
	public float emitMass = 1f;

	// Token: 0x04003631 RID: 13873
	[SerializeField]
	public Tag emitTag;

	// Token: 0x04003632 RID: 13874
	[SerializeField]
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04003633 RID: 13875
	[SerializeField]
	public SimHashes exhaustElement = SimHashes.Vacuum;

	// Token: 0x04003634 RID: 13876
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04003635 RID: 13877
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003636 RID: 13878
	[MyCmpGet]
	private ConduitConsumer consumer;

	// Token: 0x04003637 RID: 13879
	[MyCmpGet]
	private ElementConverter converter;

	// Token: 0x04003638 RID: 13880
	private MeterController plasticMeter;

	// Token: 0x04003639 RID: 13881
	private MeterController oilMeter;

	// Token: 0x0400363A RID: 13882
	private static readonly EventSystem.IntraObjectHandler<Polymerizer> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Polymerizer>(delegate(Polymerizer component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02000F1F RID: 3871
	public class StatesInstance : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.GameInstance
	{
		// Token: 0x06004E08 RID: 19976 RVA: 0x000D2BAF File Offset: 0x000D0DAF
		public StatesInstance(Polymerizer smi) : base(smi)
		{
		}
	}

	// Token: 0x02000F20 RID: 3872
	public class States : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer>
	{
		// Token: 0x06004E09 RID: 19977 RVA: 0x00266D74 File Offset: 0x00264F74
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.EventTransition(GameHashes.OperationalChanged, this.off, (Polymerizer.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.off.EventTransition(GameHashes.OperationalChanged, this.on, (Polymerizer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.EventTransition(GameHashes.OnStorageChange, this.converting, (Polymerizer.StatesInstance smi) => smi.master.converter.CanConvertAtAll());
			this.converting.Enter("Ready", delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).EventHandler(GameHashes.OnStorageChange, delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.TryEmit();
			}).EventTransition(GameHashes.OnStorageChange, this.on, (Polymerizer.StatesInstance smi) => !smi.master.converter.CanConvertAtAll()).Exit("Ready", delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
		}

		// Token: 0x0400363B RID: 13883
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State off;

		// Token: 0x0400363C RID: 13884
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State on;

		// Token: 0x0400363D RID: 13885
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State converting;
	}
}
