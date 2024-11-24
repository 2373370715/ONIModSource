using System;
using STRINGS;
using UnityEngine;

public class PlantBranch : GameStateMachine<PlantBranch, PlantBranch.Instance, IStateMachineTarget, PlantBranch.Def>
{
	public class Def : BaseDef
	{
		public Action<PlantBranchGrower.Instance, Instance> animationSetupCallback;

		public Action<Instance> onEarlySpawn;
	}

	public new class Instance : GameInstance, IWiltCause
	{
		public PlantBranchGrower.Instance trunk;

		private int trunkWiltHandle = -1;

		private int trunkWiltRecoverHandle = -1;

		public bool HasTrunk
		{
			get
			{
				if (trunk != null && !trunk.IsNullOrDestroyed())
				{
					return !trunk.isMasterNull;
				}
				return false;
			}
		}

		public string WiltStateString => "    • " + DUPLICANTS.STATS.TRUNKHEALTH.NAME;

		public WiltCondition.Condition[] Conditions => new WiltCondition.Condition[1] { WiltCondition.Condition.UnhealthyRoot };

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			SetOccupyGridSpace(active: true);
			Subscribe(1272413801, OnHarvest);
		}

		public override void StartSM()
		{
			base.StartSM();
			base.def.onEarlySpawn?.Invoke(this);
			trunk = GetTrunk();
			if (!HasTrunk)
			{
				Debug.LogWarning("Tree Branch loaded with missing trunk reference. Destroying...");
				Util.KDestroyGameObject(base.gameObject);
			}
			else
			{
				SubscribeToTrunk();
				base.def.animationSetupCallback?.Invoke(trunk, this);
			}
		}

		private void OnHarvest(object data)
		{
			if (HasTrunk)
			{
				trunk.OnBrancHarvested(this);
			}
		}

		protected override void OnCleanUp()
		{
			UnsubscribeToTrunk();
			SetOccupyGridSpace(active: false);
			base.OnCleanUp();
		}

		private void SetOccupyGridSpace(bool active)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (active)
			{
				GameObject gameObject = Grid.Objects[cell, 5];
				if (gameObject != null && gameObject != base.gameObject)
				{
					Debug.LogWarningFormat(base.gameObject, "PlantBranch.SetOccupyGridSpace already occupied by {0}", gameObject);
					Util.KDestroyGameObject(base.gameObject);
				}
				else
				{
					Grid.Objects[cell, 5] = base.gameObject;
				}
			}
			else if (Grid.Objects[cell, 5] == base.gameObject)
			{
				Grid.Objects[cell, 5] = null;
			}
		}

		public void SetTrunk(PlantBranchGrower.Instance trunk)
		{
			this.trunk = trunk;
			base.smi.sm.Trunk.Set(trunk.gameObject, this);
			SubscribeToTrunk();
			base.def.animationSetupCallback?.Invoke(trunk, this);
		}

		public PlantBranchGrower.Instance GetTrunk()
		{
			if (base.smi.sm.Trunk.IsNull(this))
			{
				return null;
			}
			return base.sm.Trunk.Get(this).GetSMI<PlantBranchGrower.Instance>();
		}

		private void SubscribeToTrunk()
		{
			if (HasTrunk)
			{
				if (trunkWiltHandle == -1)
				{
					trunkWiltHandle = trunk.gameObject.Subscribe(-724860998, OnTrunkWilt);
				}
				if (trunkWiltRecoverHandle == -1)
				{
					trunkWiltRecoverHandle = trunk.gameObject.Subscribe(712767498, OnTrunkRecover);
				}
				Trigger(912965142, !trunk.GetComponent<WiltCondition>().IsWilting());
				ReceptacleMonitor component = GetComponent<ReceptacleMonitor>();
				PlantablePlot receptacle = trunk.GetComponent<ReceptacleMonitor>().GetReceptacle();
				component.SetReceptacle(receptacle);
				trunk.RefreshBranchZPositionOffset(base.gameObject);
				GetComponent<BudUprootedMonitor>().SetParentObject(trunk.GetComponent<KPrefabID>());
			}
		}

		private void UnsubscribeToTrunk()
		{
			if (HasTrunk)
			{
				trunk.gameObject.Unsubscribe(trunkWiltHandle);
				trunk.gameObject.Unsubscribe(trunkWiltRecoverHandle);
				trunkWiltHandle = -1;
				trunkWiltRecoverHandle = -1;
				trunk.OnBranchRemoved(base.gameObject);
			}
		}

		private void OnTrunkWilt(object data = null)
		{
			Trigger(912965142, false);
		}

		private void OnTrunkRecover(object data = null)
		{
			Trigger(912965142, true);
		}
	}

	private TargetParameter Trunk;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = root;
	}
}
