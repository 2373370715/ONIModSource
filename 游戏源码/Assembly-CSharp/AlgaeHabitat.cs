using System;
using UnityEngine;

// Token: 0x02000C8E RID: 3214
public class AlgaeHabitat : StateMachineComponent<AlgaeHabitat.SMInstance>
{
	// Token: 0x06003DD8 RID: 15832 RVA: 0x0023297C File Offset: 0x00230B7C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true);
		}, null, null);
		this.ConfigurePollutedWaterOutput();
		Tutorial.Instance.oxygenGenerators.Add(base.gameObject);
	}

	// Token: 0x06003DD9 RID: 15833 RVA: 0x000C822F File Offset: 0x000C642F
	protected override void OnCleanUp()
	{
		Tutorial.Instance.oxygenGenerators.Remove(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06003DDA RID: 15834 RVA: 0x002329EC File Offset: 0x00230BEC
	private void ConfigurePollutedWaterOutput()
	{
		Storage storage = null;
		Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
		foreach (Storage storage2 in base.GetComponents<Storage>())
		{
			if (storage2.storageFilters.Contains(tag))
			{
				storage = storage2;
				break;
			}
		}
		foreach (ElementConverter elementConverter in base.GetComponents<ElementConverter>())
		{
			ElementConverter.OutputElement[] outputElements = elementConverter.outputElements;
			for (int j = 0; j < outputElements.Length; j++)
			{
				if (outputElements[j].elementHash == SimHashes.DirtyWater)
				{
					elementConverter.SetStorage(storage);
					break;
				}
			}
		}
		this.pollutedWaterStorage = storage;
	}

	// Token: 0x04002A2D RID: 10797
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002A2E RID: 10798
	private Storage pollutedWaterStorage;

	// Token: 0x04002A2F RID: 10799
	[SerializeField]
	public float lightBonusMultiplier = 1.1f;

	// Token: 0x04002A30 RID: 10800
	public CellOffset pressureSampleOffset = CellOffset.none;

	// Token: 0x02000C8F RID: 3215
	public class SMInstance : GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.GameInstance
	{
		// Token: 0x06003DDC RID: 15836 RVA: 0x000C826B File Offset: 0x000C646B
		public SMInstance(AlgaeHabitat master) : base(master)
		{
			this.converter = master.GetComponent<ElementConverter>();
		}

		// Token: 0x06003DDD RID: 15837 RVA: 0x000C8280 File Offset: 0x000C6480
		public bool HasEnoughMass(Tag tag)
		{
			return this.converter.HasEnoughMass(tag, false);
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x000C828F File Offset: 0x000C648F
		public bool NeedsEmptying()
		{
			return base.smi.master.pollutedWaterStorage.RemainingCapacity() <= 0f;
		}

		// Token: 0x06003DDF RID: 15839 RVA: 0x00232A98 File Offset: 0x00230C98
		public void CreateEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("dupe");
			}
			AlgaeHabitatEmpty component = base.master.GetComponent<AlgaeHabitatEmpty>();
			this.emptyChore = new WorkChore<AlgaeHabitatEmpty>(Db.Get().ChoreTypes.EmptyStorage, component, null, true, new Action<Chore>(this.OnEmptyComplete), null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
		}

		// Token: 0x06003DE0 RID: 15840 RVA: 0x000C82B0 File Offset: 0x000C64B0
		public void CancelEmptyChore()
		{
			if (this.emptyChore != null)
			{
				this.emptyChore.Cancel("Cancelled");
				this.emptyChore = null;
			}
		}

		// Token: 0x06003DE1 RID: 15841 RVA: 0x00232B00 File Offset: 0x00230D00
		private void OnEmptyComplete(Chore chore)
		{
			this.emptyChore = null;
			base.master.pollutedWaterStorage.DropAll(true, false, default(Vector3), true, null);
		}

		// Token: 0x04002A31 RID: 10801
		public ElementConverter converter;

		// Token: 0x04002A32 RID: 10802
		public Chore emptyChore;
	}

	// Token: 0x02000C90 RID: 3216
	public class States : GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat>
	{
		// Token: 0x06003DE2 RID: 15842 RVA: 0x00232B34 File Offset: 0x00230D34
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.noAlgae;
			this.root.EventTransition(GameHashes.OperationalChanged, this.notoperational, (AlgaeHabitat.SMInstance smi) => !smi.master.operational.IsOperational).EventTransition(GameHashes.OperationalChanged, this.noAlgae, (AlgaeHabitat.SMInstance smi) => smi.master.operational.IsOperational);
			this.notoperational.QueueAnim("off", false, null);
			this.gotAlgae.PlayAnim("on_pre").OnAnimQueueComplete(this.noWater);
			this.gotEmptied.PlayAnim("on_pre").OnAnimQueueComplete(this.generatingOxygen);
			this.lostAlgae.PlayAnim("on_pst").OnAnimQueueComplete(this.noAlgae);
			this.noAlgae.QueueAnim("off", false, null).EventTransition(GameHashes.OnStorageChange, this.gotAlgae, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Algae)).Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
			this.noWater.QueueAnim("on", false, null).Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.GetComponent<PassiveElementConsumer>().EnableConsumption(true);
			}).EventTransition(GameHashes.OnStorageChange, this.lostAlgae, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Algae)).EventTransition(GameHashes.OnStorageChange, this.gotWater, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Algae) && smi.HasEnoughMass(GameTags.Water));
			this.needsEmptying.QueueAnim("off", false, null).Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.CreateEmptyChore();
			}).Exit(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.CancelEmptyChore();
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.HabitatNeedsEmptying, null).EventTransition(GameHashes.OnStorageChange, this.noAlgae, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Algae) || !smi.HasEnoughMass(GameTags.Water)).EventTransition(GameHashes.OnStorageChange, this.gotEmptied, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Algae) && smi.HasEnoughMass(GameTags.Water) && !smi.NeedsEmptying());
			this.gotWater.PlayAnim("working_pre").OnAnimQueueComplete(this.needsEmptying);
			this.generatingOxygen.Enter(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).Exit(delegate(AlgaeHabitat.SMInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			}).Update("GeneratingOxygen", delegate(AlgaeHabitat.SMInstance smi, float dt)
			{
				int num = Grid.PosToCell(smi.master.transform.GetPosition());
				smi.converter.OutputMultiplier = ((Grid.LightCount[num] > 0) ? smi.master.lightBonusMultiplier : 1f);
			}, UpdateRate.SIM_200ms, false).QueueAnim("working_loop", true, null).EventTransition(GameHashes.OnStorageChange, this.stoppedGeneratingOxygen, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Water) || !smi.HasEnoughMass(GameTags.Algae) || smi.NeedsEmptying());
			this.stoppedGeneratingOxygen.PlayAnim("working_pst").OnAnimQueueComplete(this.stoppedGeneratingOxygenTransition);
			this.stoppedGeneratingOxygenTransition.EventTransition(GameHashes.OnStorageChange, this.needsEmptying, (AlgaeHabitat.SMInstance smi) => smi.NeedsEmptying()).EventTransition(GameHashes.OnStorageChange, this.noWater, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Water)).EventTransition(GameHashes.OnStorageChange, this.lostAlgae, (AlgaeHabitat.SMInstance smi) => !smi.HasEnoughMass(GameTags.Algae)).EventTransition(GameHashes.OnStorageChange, this.gotWater, (AlgaeHabitat.SMInstance smi) => smi.HasEnoughMass(GameTags.Water) && smi.HasEnoughMass(GameTags.Algae));
		}

		// Token: 0x04002A33 RID: 10803
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State generatingOxygen;

		// Token: 0x04002A34 RID: 10804
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State stoppedGeneratingOxygen;

		// Token: 0x04002A35 RID: 10805
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State stoppedGeneratingOxygenTransition;

		// Token: 0x04002A36 RID: 10806
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State noWater;

		// Token: 0x04002A37 RID: 10807
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State noAlgae;

		// Token: 0x04002A38 RID: 10808
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State needsEmptying;

		// Token: 0x04002A39 RID: 10809
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotAlgae;

		// Token: 0x04002A3A RID: 10810
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotWater;

		// Token: 0x04002A3B RID: 10811
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotEmptied;

		// Token: 0x04002A3C RID: 10812
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State lostAlgae;

		// Token: 0x04002A3D RID: 10813
		public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State notoperational;
	}
}
