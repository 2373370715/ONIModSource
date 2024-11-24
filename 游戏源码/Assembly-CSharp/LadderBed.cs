using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000E13 RID: 3603
public class LadderBed : GameStateMachine<LadderBed, LadderBed.Instance, IStateMachineTarget, LadderBed.Def>
{
	// Token: 0x060046EA RID: 18154 RVA: 0x000CDFC6 File Offset: 0x000CC1C6
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
	}

	// Token: 0x04003123 RID: 12579
	public static string lightBedShakeSoundPath = GlobalAssets.GetSound("LadderBed_LightShake", false);

	// Token: 0x04003124 RID: 12580
	public static string noDupeBedShakeSoundPath = GlobalAssets.GetSound("LadderBed_Shake", false);

	// Token: 0x04003125 RID: 12581
	public static string LADDER_BED_COUNT_BELOW_PARAMETER = "bed_count";

	// Token: 0x02000E14 RID: 3604
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003126 RID: 12582
		public CellOffset[] offsets;
	}

	// Token: 0x02000E15 RID: 3605
	public new class Instance : GameStateMachine<LadderBed, LadderBed.Instance, IStateMachineTarget, LadderBed.Def>.GameInstance
	{
		// Token: 0x060046EE RID: 18158 RVA: 0x0025080C File Offset: 0x0024EA0C
		public Instance(IStateMachineTarget master, LadderBed.Def def) : base(master, def)
		{
			ScenePartitionerLayer scenePartitionerLayer = GameScenePartitioner.Instance.objectLayers[40];
			this.m_cell = Grid.PosToCell(master.gameObject);
			foreach (CellOffset offset in def.offsets)
			{
				int cell = Grid.OffsetCell(this.m_cell, offset);
				if (Grid.IsValidCell(this.m_cell) && Grid.IsValidCell(cell))
				{
					this.m_partitionEntires.Add(GameScenePartitioner.Instance.Add("LadderBed.Constructor", base.gameObject, cell, GameScenePartitioner.Instance.pickupablesChangedLayer, new Action<object>(this.OnMoverChanged)));
					this.OnMoverChanged(null);
				}
			}
			AttachableBuilding attachable = this.m_attachable;
			attachable.onAttachmentNetworkChanged = (Action<object>)Delegate.Combine(attachable.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentChanged));
			this.OnAttachmentChanged(null);
			base.Subscribe(-717201811, new Action<object>(this.OnSleepDisturbedByMovement));
			master.GetComponent<KAnimControllerBase>().GetLayering().GetLink().syncTint = false;
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x00250924 File Offset: 0x0024EB24
		private void OnSleepDisturbedByMovement(object obj)
		{
			base.GetComponent<KAnimControllerBase>().Play("interrupt_light", KAnim.PlayMode.Once, 1f, 0f);
			EventInstance instance = SoundEvent.BeginOneShot(LadderBed.lightBedShakeSoundPath, base.smi.transform.GetPosition(), 1f, false);
			instance.setParameterByName(LadderBed.LADDER_BED_COUNT_BELOW_PARAMETER, (float)this.numBelow, false);
			SoundEvent.EndOneShot(instance);
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x000CE004 File Offset: 0x000CC204
		private void OnAttachmentChanged(object data)
		{
			this.numBelow = AttachableBuilding.CountAttachedBelow(this.m_attachable);
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x00250990 File Offset: 0x0024EB90
		private void OnMoverChanged(object obj)
		{
			Pickupable pickupable = obj as Pickupable;
			if (pickupable != null && pickupable.gameObject != null && pickupable.KPrefabID.HasTag(GameTags.BaseMinion) && pickupable.GetComponent<Navigator>().CurrentNavType == NavType.Ladder)
			{
				if (this.m_sleepable.worker == null)
				{
					base.GetComponent<KAnimControllerBase>().Play("interrupt_light_nodupe", KAnim.PlayMode.Once, 1f, 0f);
					EventInstance instance = SoundEvent.BeginOneShot(LadderBed.noDupeBedShakeSoundPath, base.smi.transform.GetPosition(), 1f, false);
					instance.setParameterByName(LadderBed.LADDER_BED_COUNT_BELOW_PARAMETER, (float)this.numBelow, false);
					SoundEvent.EndOneShot(instance);
					return;
				}
				if (pickupable.gameObject != this.m_sleepable.worker.gameObject)
				{
					this.m_sleepable.worker.Trigger(-717201811, null);
				}
			}
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x00250A8C File Offset: 0x0024EC8C
		protected override void OnCleanUp()
		{
			foreach (HandleVector<int>.Handle handle in this.m_partitionEntires)
			{
				GameScenePartitioner.Instance.Free(ref handle);
			}
			AttachableBuilding attachable = this.m_attachable;
			attachable.onAttachmentNetworkChanged = (Action<object>)Delegate.Remove(attachable.onAttachmentNetworkChanged, new Action<object>(this.OnAttachmentChanged));
			base.OnCleanUp();
		}

		// Token: 0x04003127 RID: 12583
		private List<HandleVector<int>.Handle> m_partitionEntires = new List<HandleVector<int>.Handle>();

		// Token: 0x04003128 RID: 12584
		private int m_cell;

		// Token: 0x04003129 RID: 12585
		[MyCmpGet]
		private Ownable m_ownable;

		// Token: 0x0400312A RID: 12586
		[MyCmpGet]
		private Sleepable m_sleepable;

		// Token: 0x0400312B RID: 12587
		[MyCmpGet]
		private AttachableBuilding m_attachable;

		// Token: 0x0400312C RID: 12588
		private int numBelow;
	}
}
