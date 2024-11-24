using System;
using STRINGS;
using UnityEngine;

// Token: 0x020016E0 RID: 5856
public class PlantBranch : GameStateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>
{
	// Token: 0x0600788F RID: 30863 RVA: 0x000EF44C File Offset: 0x000ED64C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x04005A59 RID: 23129
	private StateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>.TargetParameter Trunk;

	// Token: 0x020016E1 RID: 5857
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005A5A RID: 23130
		public Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback;

		// Token: 0x04005A5B RID: 23131
		public Action<PlantBranch.Instance> onEarlySpawn;
	}

	// Token: 0x020016E2 RID: 5858
	public new class Instance : GameStateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>.GameInstance, IWiltCause
	{
		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06007892 RID: 30866 RVA: 0x000EF465 File Offset: 0x000ED665
		public bool HasTrunk
		{
			get
			{
				return this.trunk != null && !this.trunk.IsNullOrDestroyed() && !this.trunk.isMasterNull;
			}
		}

		// Token: 0x06007893 RID: 30867 RVA: 0x000EF48C File Offset: 0x000ED68C
		public Instance(IStateMachineTarget master, PlantBranch.Def def) : base(master, def)
		{
			this.SetOccupyGridSpace(true);
			base.Subscribe(1272413801, new Action<object>(this.OnHarvest));
		}

		// Token: 0x06007894 RID: 30868 RVA: 0x00311770 File Offset: 0x0030F970
		public override void StartSM()
		{
			base.StartSM();
			Action<PlantBranch.Instance> onEarlySpawn = base.def.onEarlySpawn;
			if (onEarlySpawn != null)
			{
				onEarlySpawn(this);
			}
			this.trunk = this.GetTrunk();
			if (!this.HasTrunk)
			{
				global::Debug.LogWarning("Tree Branch loaded with missing trunk reference. Destroying...");
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			this.SubscribeToTrunk();
			Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback = base.def.animationSetupCallback;
			if (animationSetupCallback == null)
			{
				return;
			}
			animationSetupCallback(this.trunk, this);
		}

		// Token: 0x06007895 RID: 30869 RVA: 0x000EF4C2 File Offset: 0x000ED6C2
		private void OnHarvest(object data)
		{
			if (this.HasTrunk)
			{
				this.trunk.OnBrancHarvested(this);
			}
		}

		// Token: 0x06007896 RID: 30870 RVA: 0x000EF4D8 File Offset: 0x000ED6D8
		protected override void OnCleanUp()
		{
			this.UnsubscribeToTrunk();
			this.SetOccupyGridSpace(false);
			base.OnCleanUp();
		}

		// Token: 0x06007897 RID: 30871 RVA: 0x003117E8 File Offset: 0x0030F9E8
		private void SetOccupyGridSpace(bool active)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!active)
			{
				if (Grid.Objects[cell, 5] == base.gameObject)
				{
					Grid.Objects[cell, 5] = null;
				}
				return;
			}
			GameObject gameObject = Grid.Objects[cell, 5];
			if (gameObject != null && gameObject != base.gameObject)
			{
				global::Debug.LogWarningFormat(base.gameObject, "PlantBranch.SetOccupyGridSpace already occupied by {0}", new object[]
				{
					gameObject
				});
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			Grid.Objects[cell, 5] = base.gameObject;
		}

		// Token: 0x06007898 RID: 30872 RVA: 0x00311888 File Offset: 0x0030FA88
		public void SetTrunk(PlantBranchGrower.Instance trunk)
		{
			this.trunk = trunk;
			base.smi.sm.Trunk.Set(trunk.gameObject, this, false);
			this.SubscribeToTrunk();
			Action<PlantBranchGrower.Instance, PlantBranch.Instance> animationSetupCallback = base.def.animationSetupCallback;
			if (animationSetupCallback == null)
			{
				return;
			}
			animationSetupCallback(trunk, this);
		}

		// Token: 0x06007899 RID: 30873 RVA: 0x000EF4ED File Offset: 0x000ED6ED
		public PlantBranchGrower.Instance GetTrunk()
		{
			if (base.smi.sm.Trunk.IsNull(this))
			{
				return null;
			}
			return base.sm.Trunk.Get(this).GetSMI<PlantBranchGrower.Instance>();
		}

		// Token: 0x0600789A RID: 30874 RVA: 0x003118D8 File Offset: 0x0030FAD8
		private void SubscribeToTrunk()
		{
			if (!this.HasTrunk)
			{
				return;
			}
			if (this.trunkWiltHandle == -1)
			{
				this.trunkWiltHandle = this.trunk.gameObject.Subscribe(-724860998, new Action<object>(this.OnTrunkWilt));
			}
			if (this.trunkWiltRecoverHandle == -1)
			{
				this.trunkWiltRecoverHandle = this.trunk.gameObject.Subscribe(712767498, new Action<object>(this.OnTrunkRecover));
			}
			base.Trigger(912965142, !this.trunk.GetComponent<WiltCondition>().IsWilting());
			ReceptacleMonitor component = base.GetComponent<ReceptacleMonitor>();
			PlantablePlot receptacle = this.trunk.GetComponent<ReceptacleMonitor>().GetReceptacle();
			component.SetReceptacle(receptacle);
			this.trunk.RefreshBranchZPositionOffset(base.gameObject);
			base.GetComponent<BudUprootedMonitor>().SetParentObject(this.trunk.GetComponent<KPrefabID>());
		}

		// Token: 0x0600789B RID: 30875 RVA: 0x003119B8 File Offset: 0x0030FBB8
		private void UnsubscribeToTrunk()
		{
			if (!this.HasTrunk)
			{
				return;
			}
			this.trunk.gameObject.Unsubscribe(this.trunkWiltHandle);
			this.trunk.gameObject.Unsubscribe(this.trunkWiltRecoverHandle);
			this.trunkWiltHandle = -1;
			this.trunkWiltRecoverHandle = -1;
			this.trunk.OnBranchRemoved(base.gameObject);
		}

		// Token: 0x0600789C RID: 30876 RVA: 0x000EF51F File Offset: 0x000ED71F
		private void OnTrunkWilt(object data = null)
		{
			base.Trigger(912965142, false);
		}

		// Token: 0x0600789D RID: 30877 RVA: 0x000EF532 File Offset: 0x000ED732
		private void OnTrunkRecover(object data = null)
		{
			base.Trigger(912965142, true);
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x0600789E RID: 30878 RVA: 0x000EF545 File Offset: 0x000ED745
		public string WiltStateString
		{
			get
			{
				return "    • " + DUPLICANTS.STATS.TRUNKHEALTH.NAME;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x0600789F RID: 30879 RVA: 0x000EF55B File Offset: 0x000ED75B
		public WiltCondition.Condition[] Conditions
		{
			get
			{
				return new WiltCondition.Condition[]
				{
					WiltCondition.Condition.UnhealthyRoot
				};
			}
		}

		// Token: 0x04005A5C RID: 23132
		public PlantBranchGrower.Instance trunk;

		// Token: 0x04005A5D RID: 23133
		private int trunkWiltHandle = -1;

		// Token: 0x04005A5E RID: 23134
		private int trunkWiltRecoverHandle = -1;
	}
}
