using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

public class ThreatMonitor : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>
{
	public class Def : BaseDef
	{
		public Health.HealthState fleethresholdState = Health.HealthState.Injured;

		public Tag[] friendlyCreatureTags;

		public int maxSearchEntities = 50;

		public int maxSearchDistance = 20;

		public CellOffset[] offsets = OffsetGroups.Use;
	}

	public class SafeStates : State
	{
		public State passive;

		public State seeking;
	}

	public class ThreatenedStates : State
	{
		public ThreatenedDuplicantStates duplicant;

		public State creature;
	}

	public class ThreatenedDuplicantStates : State
	{
		public State ShoudFlee;

		public State ShouldFight;
	}

	public struct Grudge
	{
		public FactionAlignment target;

		public float grudgeTime;

		public void Reset(FactionAlignment revengeTarget)
		{
			target = revengeTarget;
			float num = 10f;
			grudgeTime = num;
		}

		public bool Calm(float dt, FactionAlignment self)
		{
			if (grudgeTime <= 0f)
			{
				return true;
			}
			grudgeTime = Mathf.Max(0f, grudgeTime - dt);
			if (grudgeTime == 0f)
			{
				if (FactionManager.Instance.GetDisposition(self.Alignment, target.Alignment) != FactionManager.Disposition.Attack)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.GAMEOBJECTEFFECTS.FORGAVEATTACKER, self.transform, 2f, track_target: true);
				}
				Clear();
				return true;
			}
			return false;
		}

		public void Clear()
		{
			grudgeTime = 0f;
			target = null;
		}

		public bool IsValidRevengeTarget(bool isDuplicant)
		{
			if (target != null && target.IsAlignmentActive() && (target.health == null || !target.health.IsDefeated()))
			{
				if (isDuplicant)
				{
					return !target.IsPlayerTargeted();
				}
				return true;
			}
			return false;
		}
	}

	public new class Instance : GameInstance
	{
		public FactionAlignment alignment;

		public Navigator navigator;

		public ChoreDriver choreDriver;

		private Health health;

		private ChoreConsumer choreConsumer;

		public Grudge revengeThreat;

		public int currentUpdateIndex;

		private GameObject mainThreat;

		private List<FactionAlignment> threats = new List<FactionAlignment>();

		private Action<object> refreshThreatDelegate;

		public GameObject MainThreat => mainThreat;

		public bool IAmADuplicant => alignment.Alignment == FactionManager.FactionID.Duplicant;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			alignment = master.GetComponent<FactionAlignment>();
			navigator = master.GetComponent<Navigator>();
			choreDriver = master.GetComponent<ChoreDriver>();
			health = master.GetComponent<Health>();
			choreConsumer = master.GetComponent<ChoreConsumer>();
			refreshThreatDelegate = RefreshThreat;
		}

		public void ClearMainThreat()
		{
			SetMainThreat(null);
		}

		public void SetMainThreat(GameObject threat)
		{
			if (threat == mainThreat)
			{
				return;
			}
			if (mainThreat != null)
			{
				mainThreat.Unsubscribe(1623392196, refreshThreatDelegate);
				mainThreat.Unsubscribe(1969584890, refreshThreatDelegate);
				if (threat == null)
				{
					Trigger(2144432245);
				}
			}
			if (mainThreat != null)
			{
				mainThreat.Unsubscribe(1623392196, refreshThreatDelegate);
				mainThreat.Unsubscribe(1969584890, refreshThreatDelegate);
			}
			mainThreat = threat;
			if (mainThreat != null)
			{
				mainThreat.Subscribe(1623392196, refreshThreatDelegate);
				mainThreat.Subscribe(1969584890, refreshThreatDelegate);
			}
		}

		public bool HasThreat()
		{
			return MainThreat != null;
		}

		public void OnSafe(object data)
		{
			if (revengeThreat.target != null)
			{
				if (!revengeThreat.target.GetComponent<FactionAlignment>().IsAlignmentActive())
				{
					revengeThreat.Clear();
				}
				ClearMainThreat();
			}
		}

		public void OnAttacked(object data)
		{
			FactionAlignment factionAlignment = (FactionAlignment)data;
			revengeThreat.Reset(factionAlignment);
			if (mainThreat == null)
			{
				SetMainThreat(factionAlignment.gameObject);
				GoToThreatened();
			}
			else if (!WillFight())
			{
				GoToThreatened();
			}
			if ((bool)factionAlignment.GetComponent<Bee>())
			{
				Chore chore = ((choreDriver != null) ? choreDriver.GetCurrentChore() : null);
				if (chore != null && chore.gameObject.GetComponent<HiveWorkableEmpty>() != null)
				{
					chore.gameObject.GetComponent<HiveWorkableEmpty>().wasStung = true;
				}
			}
		}

		public bool WillFight()
		{
			if (choreConsumer != null)
			{
				if (!choreConsumer.IsPermittedByUser(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
				if (!choreConsumer.IsPermittedByTraits(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
			}
			return health.State < base.smi.def.fleethresholdState;
		}

		private void GotoThreatResponse()
		{
			Chore currentChore = base.smi.master.GetComponent<ChoreDriver>().GetCurrentChore();
			if (WillFight() && mainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
			{
				base.smi.GoTo(base.smi.sm.threatened.duplicant.ShouldFight);
			}
			else if (currentChore == null || currentChore.target == null || currentChore.target == base.master || !(currentChore.target.GetComponent<Pickupable>() != null))
			{
				base.smi.GoTo(base.smi.sm.threatened.duplicant.ShoudFlee);
			}
		}

		public void GoToThreatened()
		{
			if (IAmADuplicant)
			{
				GotoThreatResponse();
			}
			else
			{
				base.smi.GoTo(base.sm.threatened.creature);
			}
		}

		public void Cleanup(object data)
		{
			if ((bool)mainThreat)
			{
				mainThreat.Unsubscribe(1623392196, refreshThreatDelegate);
				mainThreat.Unsubscribe(1969584890, refreshThreatDelegate);
			}
		}

		public void RefreshThreat(object data)
		{
			if (IsRunning())
			{
				if (base.smi.CheckForThreats())
				{
					GoToThreatened();
				}
				else if (!IsInSafeState(base.smi))
				{
					Trigger(-21431934);
					base.smi.GoTo(base.sm.safe);
				}
			}
		}

		public bool CheckForThreats()
		{
			if (base.isMasterNull)
			{
				return false;
			}
			GameObject gameObject = (revengeThreat.IsValidRevengeTarget(IAmADuplicant) ? revengeThreat.target.gameObject : ((!IAmADuplicant) ? FindThreatOther() : FindThreatDuplicant()));
			SetMainThreat(gameObject);
			return gameObject != null;
		}

		private GameObject FindThreatDuplicant()
		{
			threats.Clear();
			if (WillFight())
			{
				foreach (FactionAlignment item in Components.PlayerTargeted)
				{
					if (!item.IsNullOrDestroyed() && item.IsPlayerTargeted() && !item.health.IsDefeated() && navigator.CanReach(item.attackable.GetCell(), base.smi.def.offsets))
					{
						threats.Add(item);
					}
				}
			}
			return PickBestTarget(threats);
		}

		private GameObject FindThreatOther()
		{
			threats.Clear();
			GatherThreats();
			return PickBestTarget(threats);
		}

		private void GatherThreats()
		{
			ListPool<ScenePartitionerEntry, ThreatMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ThreatMonitor>.Allocate();
			Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.def.maxSearchDistance);
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.attackableEntitiesLayer, pooledList);
			int count = pooledList.Count;
			int num = Mathf.Min(count, base.def.maxSearchEntities);
			for (int i = 0; i < num; i++)
			{
				if (currentUpdateIndex >= count)
				{
					currentUpdateIndex = 0;
				}
				ScenePartitionerEntry scenePartitionerEntry = pooledList[currentUpdateIndex];
				currentUpdateIndex++;
				FactionAlignment factionAlignment = scenePartitionerEntry.obj as FactionAlignment;
				if (!(factionAlignment.transform == null) && !(factionAlignment == alignment) && (base.def.friendlyCreatureTags == null || !factionAlignment.kprefabID.HasAnyTags(base.def.friendlyCreatureTags)) && factionAlignment.IsAlignmentActive() && FactionManager.Instance.GetDisposition(alignment.Alignment, factionAlignment.Alignment) == FactionManager.Disposition.Attack && navigator.CanReach(factionAlignment.attackable.GetCell(), base.smi.def.offsets))
				{
					threats.Add(factionAlignment);
				}
			}
			pooledList.Recycle();
		}

		public GameObject PickBestTarget(List<FactionAlignment> threats)
		{
			float num = 1f;
			Vector2 a = base.gameObject.transform.GetPosition();
			GameObject result = null;
			float num2 = float.PositiveInfinity;
			for (int num3 = threats.Count - 1; num3 >= 0; num3--)
			{
				FactionAlignment factionAlignment = threats[num3];
				float num4 = Vector2.Distance(a, factionAlignment.transform.GetPosition()) / num;
				if (num4 < num2)
				{
					num2 = num4;
					result = factionAlignment.gameObject;
				}
			}
			return result;
		}
	}

	public SafeStates safe;

	public ThreatenedStates threatened;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = safe;
		root.EventHandler(GameHashes.SafeFromThreats, delegate(Instance smi, object d)
		{
			smi.OnSafe(d);
		}).EventHandler(GameHashes.Attacked, delegate(Instance smi, object d)
		{
			smi.OnAttacked(d);
		}).EventHandler(GameHashes.ObjectDestroyed, delegate(Instance smi, object d)
		{
			smi.Cleanup(d);
		});
		safe.Enter(delegate(Instance smi)
		{
			smi.revengeThreat.Clear();
		}).Enter(SeekThreats).EventHandler(GameHashes.FactionChanged, SeekThreats);
		safe.passive.DoNothing();
		safe.seeking.PreBrainUpdate(delegate(Instance smi)
		{
			smi.RefreshThreat(null);
		});
		threatened.duplicant.Transition(safe, GameStateMachine<ThreatMonitor, Instance, IStateMachineTarget, Def>.Not(DupeHasValidTarget));
		threatened.duplicant.ShouldFight.ToggleChore(CreateAttackChore, safe).Update("DupeUpdateTarget", DupeUpdateTarget);
		threatened.duplicant.ShoudFlee.ToggleChore(CreateFleeChore, safe);
		threatened.creature.ToggleBehaviour(GameTags.Creatures.Flee, (Instance smi) => !smi.WillFight(), delegate(Instance smi)
		{
			smi.GoTo(safe);
		}).ToggleBehaviour(GameTags.Creatures.Attack, (Instance smi) => smi.WillFight(), delegate(Instance smi)
		{
			smi.GoTo(safe);
		}).Update("CritterCalmUpdate", CritterCalmUpdate)
			.PreBrainUpdate(CritterUpdateThreats);
	}

	private static void SeekThreats(Instance smi)
	{
		Faction faction = FactionManager.Instance.GetFaction(smi.alignment.Alignment);
		if (smi.IAmADuplicant || faction.CanAttack)
		{
			smi.GoTo(smi.sm.safe.seeking);
		}
		else
		{
			smi.GoTo(smi.sm.safe.passive);
		}
	}

	private static bool DupeHasValidTarget(Instance smi)
	{
		bool result = false;
		if (smi.MainThreat != null && smi.MainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
		{
			IApproachable component = smi.MainThreat.GetComponent<RangedAttackable>();
			if (component != null)
			{
				result = smi.navigator.GetNavigationCost(component) != -1;
			}
		}
		return result;
	}

	private static void DupeUpdateTarget(Instance smi, float dt)
	{
		if (!DupeHasValidTarget(smi))
		{
			smi.Trigger(2144432245);
		}
	}

	private static void CritterCalmUpdate(Instance smi, float dt)
	{
		if (!smi.isMasterNull && smi.revengeThreat.target != null && smi.revengeThreat.Calm(dt, smi.alignment))
		{
			smi.Trigger(-21431934);
		}
	}

	private static void CritterUpdateThreats(Instance smi)
	{
		if (!smi.isMasterNull && !smi.CheckForThreats() && !IsInSafeState(smi))
		{
			smi.GoTo(smi.sm.safe);
		}
	}

	private static bool IsInSafeState(Instance smi)
	{
		if (smi.GetCurrentState() != smi.sm.safe.passive)
		{
			return smi.GetCurrentState() == smi.sm.safe.seeking;
		}
		return true;
	}

	private Chore CreateAttackChore(Instance smi)
	{
		return new AttackChore(smi.master, smi.MainThreat);
	}

	private Chore CreateFleeChore(Instance smi)
	{
		return new FleeChore(smi.master, smi.MainThreat);
	}
}
