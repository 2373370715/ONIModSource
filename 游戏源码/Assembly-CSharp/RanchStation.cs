using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000F3C RID: 3900
public class RanchStation : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>
{
	// Token: 0x06004EC0 RID: 20160 RVA: 0x00268E2C File Offset: 0x0026702C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Operational;
		this.Unoperational.TagTransition(GameTags.Operational, this.Operational, false);
		this.Operational.TagTransition(GameTags.Operational, this.Unoperational, true).ToggleChore((RanchStation.Instance smi) => smi.CreateChore(), this.Unoperational, this.Unoperational).Update("FindRanachable", delegate(RanchStation.Instance smi, float dt)
		{
			smi.FindRanchable(null);
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x040036F1 RID: 14065
	public StateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.BoolParameter RancherIsReady;

	// Token: 0x040036F2 RID: 14066
	public GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State Unoperational;

	// Token: 0x040036F3 RID: 14067
	public RanchStation.OperationalState Operational;

	// Token: 0x02000F3D RID: 3901
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040036F4 RID: 14068
		public Func<GameObject, RanchStation.Instance, bool> IsCritterEligibleToBeRanchedCb;

		// Token: 0x040036F5 RID: 14069
		public Action<GameObject> OnRanchCompleteCb;

		// Token: 0x040036F6 RID: 14070
		public Action<GameObject, float, Workable> OnRanchWorkTick;

		// Token: 0x040036F7 RID: 14071
		public HashedString RanchedPreAnim = "idle_loop";

		// Token: 0x040036F8 RID: 14072
		public HashedString RanchedLoopAnim = "idle_loop";

		// Token: 0x040036F9 RID: 14073
		public HashedString RanchedPstAnim = "idle_loop";

		// Token: 0x040036FA RID: 14074
		public HashedString RanchedAbortAnim = "idle_loop";

		// Token: 0x040036FB RID: 14075
		public HashedString RancherInteractAnim = "anim_interacts_rancherstation_kanim";

		// Token: 0x040036FC RID: 14076
		public StatusItem RanchingStatusItem = Db.Get().DuplicantStatusItems.Ranching;

		// Token: 0x040036FD RID: 14077
		public StatusItem CreatureRanchingStatusItem = Db.Get().CreatureStatusItems.GettingRanched;

		// Token: 0x040036FE RID: 14078
		public float WorkTime = 12f;

		// Token: 0x040036FF RID: 14079
		public Func<RanchStation.Instance, int> GetTargetRanchCell = (RanchStation.Instance smi) => Grid.PosToCell(smi);
	}

	// Token: 0x02000F3F RID: 3903
	public class OperationalState : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.State
	{
	}

	// Token: 0x02000F40 RID: 3904
	public new class Instance : GameStateMachine<RanchStation, RanchStation.Instance, IStateMachineTarget, RanchStation.Def>.GameInstance
	{
		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06004EC7 RID: 20167 RVA: 0x000D34FB File Offset: 0x000D16FB
		public RanchedStates.Instance ActiveRanchable
		{
			get
			{
				return this.activeRanchable;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06004EC8 RID: 20168 RVA: 0x000D3503 File Offset: 0x000D1703
		private bool isCritterAvailableForRanching
		{
			get
			{
				return this.targetRanchables.Count > 0;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06004EC9 RID: 20169 RVA: 0x000D3513 File Offset: 0x000D1713
		public bool IsCritterAvailableForRanching
		{
			get
			{
				this.ValidateTargetRanchables();
				return this.isCritterAvailableForRanching;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06004ECA RID: 20170 RVA: 0x000D3521 File Offset: 0x000D1721
		public bool HasRancher
		{
			get
			{
				return this.rancher != null;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06004ECB RID: 20171 RVA: 0x000D352F File Offset: 0x000D172F
		public bool IsRancherReady
		{
			get
			{
				return base.sm.RancherIsReady.Get(this);
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06004ECC RID: 20172 RVA: 0x000D3542 File Offset: 0x000D1742
		public Extents StationExtents
		{
			get
			{
				return this.station.GetExtents();
			}
		}

		// Token: 0x06004ECD RID: 20173 RVA: 0x000D354F File Offset: 0x000D174F
		public int GetRanchNavTarget()
		{
			return base.def.GetTargetRanchCell(this);
		}

		// Token: 0x06004ECE RID: 20174 RVA: 0x000D3562 File Offset: 0x000D1762
		public Instance(IStateMachineTarget master, RanchStation.Def def) : base(master, def)
		{
			base.gameObject.AddOrGet<RancherChore.RancherWorkable>();
			this.station = base.GetComponent<BuildingComplete>();
		}

		// Token: 0x06004ECF RID: 20175 RVA: 0x00268F8C File Offset: 0x0026718C
		public Chore CreateChore()
		{
			RancherChore rancherChore = new RancherChore(base.GetComponent<KPrefabID>());
			StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.TargetParameter targetParameter = rancherChore.smi.sm.rancher;
			StateMachine<RancherChore.RancherChoreStates, RancherChore.RancherChoreStates.Instance, IStateMachineTarget, object>.Parameter<GameObject>.Context context = targetParameter.GetContext(rancherChore.smi);
			context.onDirty = (Action<RancherChore.RancherChoreStates.Instance>)Delegate.Combine(context.onDirty, new Action<RancherChore.RancherChoreStates.Instance>(this.OnRancherChanged));
			this.rancher = targetParameter.Get<WorkerBase>(rancherChore.smi);
			return rancherChore;
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x000D354F File Offset: 0x000D174F
		public int GetTargetRanchCell()
		{
			return base.def.GetTargetRanchCell(this);
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x00268FF8 File Offset: 0x002671F8
		public override void StartSM()
		{
			base.StartSM();
			base.Subscribe(144050788, new Action<object>(this.OnRoomUpdated));
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.GetTargetRanchCell());
			if (cavityForCell != null && cavityForCell.room != null)
			{
				this.OnRoomUpdated(cavityForCell.room);
			}
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x000D358F File Offset: 0x000D178F
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			base.Unsubscribe(144050788, new Action<object>(this.OnRoomUpdated));
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x000D35AF File Offset: 0x000D17AF
		private void OnRoomUpdated(object data)
		{
			if (data == null)
			{
				return;
			}
			this.ranch = (data as Room);
			if (this.ranch.roomType != Db.Get().RoomTypes.CreaturePen)
			{
				this.TriggerRanchStationNoLongerAvailable();
				this.ranch = null;
			}
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x000D35EA File Offset: 0x000D17EA
		private void OnRancherChanged(RancherChore.RancherChoreStates.Instance choreInstance)
		{
			this.rancher = choreInstance.sm.rancher.Get<WorkerBase>(choreInstance);
			this.TriggerRanchStationNoLongerAvailable();
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x000D3609 File Offset: 0x000D1809
		public bool TryGetRanched(RanchedStates.Instance ranchable)
		{
			return this.activeRanchable == null || this.activeRanchable == ranchable;
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x000D361E File Offset: 0x000D181E
		public void MessageCreatureArrived(RanchedStates.Instance critter)
		{
			this.activeRanchable = critter;
			base.sm.RancherIsReady.Set(false, this, false);
			base.Trigger(-1357116271, null);
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x000D3647 File Offset: 0x000D1847
		public void MessageRancherReady()
		{
			base.sm.RancherIsReady.Set(true, base.smi, false);
			this.MessageRanchables(GameHashes.RancherReadyAtRanchStation);
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x00269050 File Offset: 0x00267250
		private bool CanRanchableBeRanchedAtRanchStation(RanchableMonitor.Instance ranchable)
		{
			bool flag = !ranchable.IsNullOrStopped();
			if (flag && ranchable.TargetRanchStation != null && ranchable.TargetRanchStation != this)
			{
				flag = (!ranchable.TargetRanchStation.IsRunning() || !ranchable.TargetRanchStation.HasRancher);
			}
			flag = (flag && base.def.IsCritterEligibleToBeRanchedCb(ranchable.gameObject, this));
			flag = (flag && ranchable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<RanchedStates>());
			if (flag)
			{
				int cell = Grid.PosToCell(ranchable.transform.GetPosition());
				CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
				if (cavityForCell == null || this.ranch == null || cavityForCell != this.ranch.cavity)
				{
					flag = false;
				}
				else
				{
					int cell2 = this.GetRanchNavTarget();
					if (ranchable.HasTag(GameTags.Creatures.Flyer))
					{
						cell2 = Grid.CellAbove(cell2);
					}
					flag = (ranchable.NavComponent.GetNavigationCost(cell2) != -1);
				}
			}
			return flag;
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x0026913C File Offset: 0x0026733C
		public void ValidateTargetRanchables()
		{
			if (!this.HasRancher)
			{
				return;
			}
			foreach (RanchableMonitor.Instance instance in this.targetRanchables.ToArray())
			{
				if (instance.States == null || !this.CanRanchableBeRanchedAtRanchStation(instance))
				{
					this.Abandon(instance);
				}
			}
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x00269188 File Offset: 0x00267388
		public void FindRanchable(object _ = null)
		{
			if (this.ranch == null)
			{
				return;
			}
			this.ValidateTargetRanchables();
			if (this.targetRanchables.Count == 2)
			{
				return;
			}
			List<KPrefabID> creatures = this.ranch.cavity.creatures;
			if (this.HasRancher && !this.isCritterAvailableForRanching && creatures.Count == 0)
			{
				this.TryNotifyEmptyRanch();
			}
			for (int i = 0; i < creatures.Count; i++)
			{
				KPrefabID kprefabID = creatures[i];
				if (!(kprefabID == null))
				{
					RanchableMonitor.Instance smi = kprefabID.GetSMI<RanchableMonitor.Instance>();
					if (!this.targetRanchables.Contains(smi) && this.CanRanchableBeRanchedAtRanchStation(smi) && smi != null)
					{
						smi.States.SetRanchStation(this);
						this.targetRanchables.Add(smi);
						return;
					}
				}
			}
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x000D366D File Offset: 0x000D186D
		public Option<CavityInfo> GetCavityInfo()
		{
			if (this.ranch.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return this.ranch.cavity;
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x00269240 File Offset: 0x00267440
		public void RanchCreature()
		{
			if (this.activeRanchable.IsNullOrStopped())
			{
				return;
			}
			global::Debug.Assert(this.activeRanchable != null, "targetRanchable was null");
			global::Debug.Assert(this.activeRanchable.GetMaster() != null, "GetMaster was null");
			global::Debug.Assert(base.def != null, "def was null");
			global::Debug.Assert(base.def.OnRanchCompleteCb != null, "onRanchCompleteCb cb was null");
			base.def.OnRanchCompleteCb(this.activeRanchable.gameObject);
			this.targetRanchables.Remove(this.activeRanchable.Monitor);
			this.activeRanchable.Trigger(1827504087, null);
			this.activeRanchable = null;
			this.FindRanchable(null);
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x00269304 File Offset: 0x00267504
		public void TriggerRanchStationNoLongerAvailable()
		{
			for (int i = this.targetRanchables.Count - 1; i >= 0; i--)
			{
				RanchableMonitor.Instance instance = this.targetRanchables[i];
				if (instance.IsNullOrStopped() || instance.States.IsNullOrStopped())
				{
					this.targetRanchables.RemoveAt(i);
				}
				else
				{
					this.targetRanchables.Remove(instance);
					instance.Trigger(1689625967, null);
				}
			}
			global::Debug.Assert(this.targetRanchables.Count == 0, "targetRanchables is not empty");
			this.activeRanchable = null;
			base.sm.RancherIsReady.Set(false, this, false);
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x002693A8 File Offset: 0x002675A8
		public void MessageRanchables(GameHashes hash)
		{
			for (int i = 0; i < this.targetRanchables.Count; i++)
			{
				RanchableMonitor.Instance instance = this.targetRanchables[i];
				if (!instance.IsNullOrStopped())
				{
					Game.BrainScheduler.PrioritizeBrain(instance.GetComponent<CreatureBrain>());
					if (!instance.States.IsNullOrStopped())
					{
						instance.Trigger((int)hash, null);
					}
				}
			}
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x00269408 File Offset: 0x00267608
		public void Abandon(RanchableMonitor.Instance critter)
		{
			if (critter == null)
			{
				global::Debug.LogWarning("Null critter trying to abandon ranch station");
				this.targetRanchables.Remove(critter);
				return;
			}
			critter.TargetRanchStation = null;
			if (this.targetRanchables.Remove(critter))
			{
				if (critter.States == null)
				{
					return;
				}
				bool flag = !this.isCritterAvailableForRanching;
				if (critter.States == this.activeRanchable)
				{
					flag = true;
					this.activeRanchable = null;
				}
				if (flag)
				{
					this.TryNotifyEmptyRanch();
				}
			}
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x000D3697 File Offset: 0x000D1897
		private void TryNotifyEmptyRanch()
		{
			if (!this.HasRancher)
			{
				return;
			}
			this.rancher.Trigger(-364750427, null);
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x000D36B3 File Offset: 0x000D18B3
		public bool IsCritterInQueue(RanchableMonitor.Instance critter)
		{
			return this.targetRanchables.Contains(critter);
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x000D36C1 File Offset: 0x000D18C1
		public List<RanchableMonitor.Instance> DEBUG_GetTargetRanchables()
		{
			return this.targetRanchables;
		}

		// Token: 0x04003702 RID: 14082
		private const int QUEUE_SIZE = 2;

		// Token: 0x04003703 RID: 14083
		private List<RanchableMonitor.Instance> targetRanchables = new List<RanchableMonitor.Instance>();

		// Token: 0x04003704 RID: 14084
		private RanchedStates.Instance activeRanchable;

		// Token: 0x04003705 RID: 14085
		private Room ranch;

		// Token: 0x04003706 RID: 14086
		private WorkerBase rancher;

		// Token: 0x04003707 RID: 14087
		private BuildingComplete station;
	}
}
