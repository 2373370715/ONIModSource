using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200160A RID: 5642
public class ThreatMonitor : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>
{
	// Token: 0x060074CA RID: 29898 RVA: 0x003046CC File Offset: 0x003028CC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.safe;
		this.root.EventHandler(GameHashes.SafeFromThreats, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.OnSafe(d);
		}).EventHandler(GameHashes.Attacked, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.OnAttacked(d);
		}).EventHandler(GameHashes.ObjectDestroyed, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.Cleanup(d);
		});
		this.safe.Enter(delegate(ThreatMonitor.Instance smi)
		{
			smi.revengeThreat.Clear();
		}).Enter(new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback(ThreatMonitor.SeekThreats)).EventHandler(GameHashes.FactionChanged, new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback(ThreatMonitor.SeekThreats));
		this.safe.passive.DoNothing();
		this.safe.seeking.PreBrainUpdate(delegate(ThreatMonitor.Instance smi)
		{
			smi.RefreshThreat(null);
		});
		this.threatened.duplicant.Transition(this.safe, GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Not(new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Transition.ConditionCallback(ThreatMonitor.DupeHasValidTarget)), UpdateRate.SIM_200ms);
		this.threatened.duplicant.ShouldFight.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateAttackChore), this.safe).Update("DupeUpdateTarget", new Action<ThreatMonitor.Instance, float>(ThreatMonitor.DupeUpdateTarget), UpdateRate.SIM_200ms, false);
		this.threatened.duplicant.ShoudFlee.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateFleeChore), this.safe);
		this.threatened.creature.ToggleBehaviour(GameTags.Creatures.Flee, (ThreatMonitor.Instance smi) => !smi.WillFight(), delegate(ThreatMonitor.Instance smi)
		{
			smi.GoTo(this.safe);
		}).ToggleBehaviour(GameTags.Creatures.Attack, (ThreatMonitor.Instance smi) => smi.WillFight(), delegate(ThreatMonitor.Instance smi)
		{
			smi.GoTo(this.safe);
		}).Update("CritterCalmUpdate", new Action<ThreatMonitor.Instance, float>(ThreatMonitor.CritterCalmUpdate), UpdateRate.SIM_200ms, false).PreBrainUpdate(new Action<ThreatMonitor.Instance>(ThreatMonitor.CritterUpdateThreats));
	}

	// Token: 0x060074CB RID: 29899 RVA: 0x00304928 File Offset: 0x00302B28
	private static void SeekThreats(ThreatMonitor.Instance smi)
	{
		Faction faction = FactionManager.Instance.GetFaction(smi.alignment.Alignment);
		if (smi.IAmADuplicant || faction.CanAttack)
		{
			smi.GoTo(smi.sm.safe.seeking);
			return;
		}
		smi.GoTo(smi.sm.safe.passive);
	}

	// Token: 0x060074CC RID: 29900 RVA: 0x00304988 File Offset: 0x00302B88
	private static bool DupeHasValidTarget(ThreatMonitor.Instance smi)
	{
		bool result = false;
		if (smi.MainThreat != null && smi.MainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
		{
			IApproachable component = smi.MainThreat.GetComponent<RangedAttackable>();
			if (component != null)
			{
				result = (smi.navigator.GetNavigationCost(component) != -1);
			}
		}
		return result;
	}

	// Token: 0x060074CD RID: 29901 RVA: 0x000ECB09 File Offset: 0x000EAD09
	private static void DupeUpdateTarget(ThreatMonitor.Instance smi, float dt)
	{
		if (!ThreatMonitor.DupeHasValidTarget(smi))
		{
			smi.Trigger(2144432245, null);
		}
	}

	// Token: 0x060074CE RID: 29902 RVA: 0x000ECB1F File Offset: 0x000EAD1F
	private static void CritterCalmUpdate(ThreatMonitor.Instance smi, float dt)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (smi.revengeThreat.target != null && smi.revengeThreat.Calm(dt, smi.alignment))
		{
			smi.Trigger(-21431934, null);
		}
	}

	// Token: 0x060074CF RID: 29903 RVA: 0x000ECB5D File Offset: 0x000EAD5D
	private static void CritterUpdateThreats(ThreatMonitor.Instance smi)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (!smi.CheckForThreats() && !ThreatMonitor.IsInSafeState(smi))
		{
			smi.GoTo(smi.sm.safe);
		}
	}

	// Token: 0x060074D0 RID: 29904 RVA: 0x000ECB89 File Offset: 0x000EAD89
	private static bool IsInSafeState(ThreatMonitor.Instance smi)
	{
		return smi.GetCurrentState() == smi.sm.safe.passive || smi.GetCurrentState() == smi.sm.safe.seeking;
	}

	// Token: 0x060074D1 RID: 29905 RVA: 0x000ECBBD File Offset: 0x000EADBD
	private Chore CreateAttackChore(ThreatMonitor.Instance smi)
	{
		return new AttackChore(smi.master, smi.MainThreat);
	}

	// Token: 0x060074D2 RID: 29906 RVA: 0x000ECBD0 File Offset: 0x000EADD0
	private Chore CreateFleeChore(ThreatMonitor.Instance smi)
	{
		return new FleeChore(smi.master, smi.MainThreat);
	}

	// Token: 0x04005775 RID: 22389
	public ThreatMonitor.SafeStates safe;

	// Token: 0x04005776 RID: 22390
	public ThreatMonitor.ThreatenedStates threatened;

	// Token: 0x0200160B RID: 5643
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005777 RID: 22391
		public Health.HealthState fleethresholdState = Health.HealthState.Injured;

		// Token: 0x04005778 RID: 22392
		public Tag[] friendlyCreatureTags;

		// Token: 0x04005779 RID: 22393
		public int maxSearchEntities = 50;

		// Token: 0x0400577A RID: 22394
		public int maxSearchDistance = 20;

		// Token: 0x0400577B RID: 22395
		public CellOffset[] offsets = OffsetGroups.Use;
	}

	// Token: 0x0200160C RID: 5644
	public class SafeStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x0400577C RID: 22396
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State passive;

		// Token: 0x0400577D RID: 22397
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State seeking;
	}

	// Token: 0x0200160D RID: 5645
	public class ThreatenedStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x0400577E RID: 22398
		public ThreatMonitor.ThreatenedDuplicantStates duplicant;

		// Token: 0x0400577F RID: 22399
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State creature;
	}

	// Token: 0x0200160E RID: 5646
	public class ThreatenedDuplicantStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x04005780 RID: 22400
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShoudFlee;

		// Token: 0x04005781 RID: 22401
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShouldFight;
	}

	// Token: 0x0200160F RID: 5647
	public struct Grudge
	{
		// Token: 0x060074DA RID: 29914 RVA: 0x003049DC File Offset: 0x00302BDC
		public void Reset(FactionAlignment revengeTarget)
		{
			this.target = revengeTarget;
			float num = 10f;
			this.grudgeTime = num;
		}

		// Token: 0x060074DB RID: 29915 RVA: 0x00304A00 File Offset: 0x00302C00
		public bool Calm(float dt, FactionAlignment self)
		{
			if (this.grudgeTime <= 0f)
			{
				return true;
			}
			this.grudgeTime = Mathf.Max(0f, this.grudgeTime - dt);
			if (this.grudgeTime == 0f)
			{
				if (FactionManager.Instance.GetDisposition(self.Alignment, this.target.Alignment) != FactionManager.Disposition.Attack)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.GAMEOBJECTEFFECTS.FORGAVEATTACKER, self.transform, 2f, true);
				}
				this.Clear();
				return true;
			}
			return false;
		}

		// Token: 0x060074DC RID: 29916 RVA: 0x000ECC2B File Offset: 0x000EAE2B
		public void Clear()
		{
			this.grudgeTime = 0f;
			this.target = null;
		}

		// Token: 0x060074DD RID: 29917 RVA: 0x00304A94 File Offset: 0x00302C94
		public bool IsValidRevengeTarget(bool isDuplicant)
		{
			return this.target != null && this.target.IsAlignmentActive() && (this.target.health == null || !this.target.health.IsDefeated()) && (!isDuplicant || !this.target.IsPlayerTargeted());
		}

		// Token: 0x04005782 RID: 22402
		public FactionAlignment target;

		// Token: 0x04005783 RID: 22403
		public float grudgeTime;
	}

	// Token: 0x02001610 RID: 5648
	public new class Instance : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameInstance
	{
		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060074DE RID: 29918 RVA: 0x000ECC3F File Offset: 0x000EAE3F
		public GameObject MainThreat
		{
			get
			{
				return this.mainThreat;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060074DF RID: 29919 RVA: 0x000ECC47 File Offset: 0x000EAE47
		public bool IAmADuplicant
		{
			get
			{
				return this.alignment.Alignment == FactionManager.FactionID.Duplicant;
			}
		}

		// Token: 0x060074E0 RID: 29920 RVA: 0x00304AF8 File Offset: 0x00302CF8
		public Instance(IStateMachineTarget master, ThreatMonitor.Def def) : base(master, def)
		{
			this.alignment = master.GetComponent<FactionAlignment>();
			this.navigator = master.GetComponent<Navigator>();
			this.choreDriver = master.GetComponent<ChoreDriver>();
			this.health = master.GetComponent<Health>();
			this.choreConsumer = master.GetComponent<ChoreConsumer>();
			this.refreshThreatDelegate = new Action<object>(this.RefreshThreat);
		}

		// Token: 0x060074E1 RID: 29921 RVA: 0x000ECC57 File Offset: 0x000EAE57
		public void ClearMainThreat()
		{
			this.SetMainThreat(null);
		}

		// Token: 0x060074E2 RID: 29922 RVA: 0x00304B68 File Offset: 0x00302D68
		public void SetMainThreat(GameObject threat)
		{
			if (threat == this.mainThreat)
			{
				return;
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
				if (threat == null)
				{
					base.Trigger(2144432245, null);
				}
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
			this.mainThreat = threat;
			if (this.mainThreat != null)
			{
				this.mainThreat.Subscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Subscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x060074E3 RID: 29923 RVA: 0x000ECC60 File Offset: 0x000EAE60
		public bool HasThreat()
		{
			return this.MainThreat != null;
		}

		// Token: 0x060074E4 RID: 29924 RVA: 0x000ECC6E File Offset: 0x000EAE6E
		public void OnSafe(object data)
		{
			if (this.revengeThreat.target != null)
			{
				if (!this.revengeThreat.target.GetComponent<FactionAlignment>().IsAlignmentActive())
				{
					this.revengeThreat.Clear();
				}
				this.ClearMainThreat();
			}
		}

		// Token: 0x060074E5 RID: 29925 RVA: 0x00304C50 File Offset: 0x00302E50
		public void OnAttacked(object data)
		{
			FactionAlignment factionAlignment = (FactionAlignment)data;
			this.revengeThreat.Reset(factionAlignment);
			if (this.mainThreat == null)
			{
				this.SetMainThreat(factionAlignment.gameObject);
				this.GoToThreatened();
			}
			else if (!this.WillFight())
			{
				this.GoToThreatened();
			}
			if (factionAlignment.GetComponent<Bee>())
			{
				Chore chore = (this.choreDriver != null) ? this.choreDriver.GetCurrentChore() : null;
				if (chore != null && chore.gameObject.GetComponent<HiveWorkableEmpty>() != null)
				{
					chore.gameObject.GetComponent<HiveWorkableEmpty>().wasStung = true;
				}
			}
		}

		// Token: 0x060074E6 RID: 29926 RVA: 0x00304CF4 File Offset: 0x00302EF4
		public bool WillFight()
		{
			if (this.choreConsumer != null)
			{
				if (!this.choreConsumer.IsPermittedByUser(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
				if (!this.choreConsumer.IsPermittedByTraits(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
			}
			return this.health.State < base.smi.def.fleethresholdState;
		}

		// Token: 0x060074E7 RID: 29927 RVA: 0x00304D70 File Offset: 0x00302F70
		private void GotoThreatResponse()
		{
			Chore currentChore = base.smi.master.GetComponent<ChoreDriver>().GetCurrentChore();
			if (this.WillFight() && this.mainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
			{
				base.smi.GoTo(base.smi.sm.threatened.duplicant.ShouldFight);
				return;
			}
			if (currentChore != null && currentChore.target != null && currentChore.target != base.master && currentChore.target.GetComponent<Pickupable>() != null)
			{
				return;
			}
			base.smi.GoTo(base.smi.sm.threatened.duplicant.ShoudFlee);
		}

		// Token: 0x060074E8 RID: 29928 RVA: 0x000ECCAB File Offset: 0x000EAEAB
		public void GoToThreatened()
		{
			if (this.IAmADuplicant)
			{
				this.GotoThreatResponse();
				return;
			}
			base.smi.GoTo(base.sm.threatened.creature);
		}

		// Token: 0x060074E9 RID: 29929 RVA: 0x000ECCD7 File Offset: 0x000EAED7
		public void Cleanup(object data)
		{
			if (this.mainThreat)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x060074EA RID: 29930 RVA: 0x00304E28 File Offset: 0x00303028
		public void RefreshThreat(object data)
		{
			if (!base.IsRunning())
			{
				return;
			}
			if (base.smi.CheckForThreats())
			{
				this.GoToThreatened();
				return;
			}
			if (!ThreatMonitor.IsInSafeState(base.smi))
			{
				base.Trigger(-21431934, null);
				base.smi.GoTo(base.sm.safe);
			}
		}

		// Token: 0x060074EB RID: 29931 RVA: 0x00304E84 File Offset: 0x00303084
		public bool CheckForThreats()
		{
			if (base.isMasterNull)
			{
				return false;
			}
			GameObject x;
			if (this.revengeThreat.IsValidRevengeTarget(this.IAmADuplicant))
			{
				x = this.revengeThreat.target.gameObject;
			}
			else if (this.IAmADuplicant)
			{
				x = this.FindThreatDuplicant();
			}
			else
			{
				x = this.FindThreatOther();
			}
			this.SetMainThreat(x);
			return x != null;
		}

		// Token: 0x060074EC RID: 29932 RVA: 0x00304EE8 File Offset: 0x003030E8
		private GameObject FindThreatDuplicant()
		{
			this.threats.Clear();
			if (this.WillFight())
			{
				foreach (object obj in Components.PlayerTargeted)
				{
					FactionAlignment factionAlignment = (FactionAlignment)obj;
					if (!factionAlignment.IsNullOrDestroyed() && factionAlignment.IsPlayerTargeted() && !factionAlignment.health.IsDefeated() && this.navigator.CanReach(factionAlignment.attackable.GetCell(), base.smi.def.offsets))
					{
						this.threats.Add(factionAlignment);
					}
				}
			}
			return this.PickBestTarget(this.threats);
		}

		// Token: 0x060074ED RID: 29933 RVA: 0x000ECD12 File Offset: 0x000EAF12
		private GameObject FindThreatOther()
		{
			this.threats.Clear();
			this.GatherThreats();
			return this.PickBestTarget(this.threats);
		}

		// Token: 0x060074EE RID: 29934 RVA: 0x00304FAC File Offset: 0x003031AC
		private void GatherThreats()
		{
			ListPool<ScenePartitionerEntry, ThreatMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ThreatMonitor>.Allocate();
			Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.def.maxSearchDistance);
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.attackableEntitiesLayer, pooledList);
			int count = pooledList.Count;
			int num = Mathf.Min(count, base.def.maxSearchEntities);
			for (int i = 0; i < num; i++)
			{
				if (this.currentUpdateIndex >= count)
				{
					this.currentUpdateIndex = 0;
				}
				ScenePartitionerEntry scenePartitionerEntry = pooledList[this.currentUpdateIndex];
				this.currentUpdateIndex++;
				FactionAlignment factionAlignment = scenePartitionerEntry.obj as FactionAlignment;
				if (!(factionAlignment.transform == null) && !(factionAlignment == this.alignment) && (base.def.friendlyCreatureTags == null || !factionAlignment.kprefabID.HasAnyTags(base.def.friendlyCreatureTags)) && factionAlignment.IsAlignmentActive() && FactionManager.Instance.GetDisposition(this.alignment.Alignment, factionAlignment.Alignment) == FactionManager.Disposition.Attack && this.navigator.CanReach(factionAlignment.attackable.GetCell(), base.smi.def.offsets))
				{
					this.threats.Add(factionAlignment);
				}
			}
			pooledList.Recycle();
		}

		// Token: 0x060074EF RID: 29935 RVA: 0x00305108 File Offset: 0x00303308
		public GameObject PickBestTarget(List<FactionAlignment> threats)
		{
			float num = 1f;
			Vector2 a = base.gameObject.transform.GetPosition();
			GameObject result = null;
			float num2 = float.PositiveInfinity;
			for (int i = threats.Count - 1; i >= 0; i--)
			{
				FactionAlignment factionAlignment = threats[i];
				float num3 = Vector2.Distance(a, factionAlignment.transform.GetPosition()) / num;
				if (num3 < num2)
				{
					num2 = num3;
					result = factionAlignment.gameObject;
				}
			}
			return result;
		}

		// Token: 0x04005784 RID: 22404
		public FactionAlignment alignment;

		// Token: 0x04005785 RID: 22405
		public Navigator navigator;

		// Token: 0x04005786 RID: 22406
		public ChoreDriver choreDriver;

		// Token: 0x04005787 RID: 22407
		private Health health;

		// Token: 0x04005788 RID: 22408
		private ChoreConsumer choreConsumer;

		// Token: 0x04005789 RID: 22409
		public ThreatMonitor.Grudge revengeThreat;

		// Token: 0x0400578A RID: 22410
		public int currentUpdateIndex;

		// Token: 0x0400578B RID: 22411
		private GameObject mainThreat;

		// Token: 0x0400578C RID: 22412
		private List<FactionAlignment> threats = new List<FactionAlignment>();

		// Token: 0x0400578D RID: 22413
		private Action<object> refreshThreatDelegate;
	}
}
