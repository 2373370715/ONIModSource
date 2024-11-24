using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

public class PlantBranchGrower : GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>
{
	public class Def : BaseDef
	{
		public string BRANCH_PREFAB_NAME;

		public int MAX_BRANCH_COUNT = -1;

		public CellOffset[] BRANCH_OFFSETS;

		public bool harvestOnDrown;

		public bool propagateHarvestDesignation = true;

		public Func<int, bool> additionalBranchGrowRequirements;

		public Action<PlantBranch.Instance, Instance> onBranchHarvested;

		public Action<PlantBranch.Instance, Instance> onBranchSpawned;

		public StatusItem growingBranchesStatusItem = Db.Get().MiscStatusItems.GrowingBranches;

		public Action<Instance> onEarlySpawn;
	}

	public new class Instance : GameInstance
	{
		private IManageGrowingStates growing;

		[MyCmpGet]
		private UprootedMonitor uprootMonitor;

		[Serialize]
		private Ref<KPrefabID>[] branches;

		private static List<int> spawn_choices = new List<int>();

		public bool IsUprooted
		{
			get
			{
				if (uprootMonitor != null)
				{
					return uprootMonitor.IsUprooted;
				}
				return false;
			}
		}

		public bool IsGrown
		{
			get
			{
				if (growing != null)
				{
					return growing.PercentGrown() >= 1f;
				}
				return true;
			}
		}

		public int MaxBranchesAllowedAtOnce
		{
			get
			{
				if (base.def.MAX_BRANCH_COUNT >= 0)
				{
					return Mathf.Min(base.def.MAX_BRANCH_COUNT, base.def.BRANCH_OFFSETS.Length);
				}
				return base.def.BRANCH_OFFSETS.Length;
			}
		}

		public int CurrentBranchCount
		{
			get
			{
				int num = 0;
				if (branches != null)
				{
					int num2 = 0;
					while (num2 < branches.Length)
					{
						num += ((GetBranch(num2++) != null) ? 1 : 0);
					}
				}
				return num;
			}
		}

		public GameObject GetBranch(int idx)
		{
			if (branches != null && branches[idx] != null)
			{
				KPrefabID kPrefabID = branches[idx].Get();
				if (kPrefabID != null)
				{
					return kPrefabID.gameObject;
				}
			}
			return null;
		}

		protected override void OnCleanUp()
		{
			SetTrunkOccupyingCellsAsPlant(doSet: false);
			base.OnCleanUp();
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			growing = GetComponent<IManageGrowingStates>();
			growing = ((growing != null) ? growing : base.gameObject.GetSMI<IManageGrowingStates>());
			SetTrunkOccupyingCellsAsPlant(doSet: true);
			Subscribe(1119167081, OnNewGameSpawn);
			Subscribe(144050788, OnUpdateRoom);
		}

		public override void StartSM()
		{
			base.StartSM();
			base.def.onEarlySpawn?.Invoke(this);
			DefineBranchArray();
			Subscribe(-216549700, OnUprooted);
			Subscribe(-266953818, delegate
			{
				UpdateAutoHarvestValue();
			});
			if (base.def.harvestOnDrown)
			{
				Subscribe(-750750377, OnUprooted);
			}
		}

		private void OnUpdateRoom(object data)
		{
			if (branches != null)
			{
				ActionPerBranch(delegate(GameObject branch)
				{
					branch.Trigger(144050788, data);
				});
			}
		}

		private void SetTrunkOccupyingCellsAsPlant(bool doSet)
		{
			CellOffset[] occupiedCellsOffsets = GetComponent<OccupyArea>().OccupiedCellsOffsets;
			int cell = Grid.PosToCell(base.gameObject);
			for (int i = 0; i < occupiedCellsOffsets.Length; i++)
			{
				int cell2 = Grid.OffsetCell(cell, occupiedCellsOffsets[i]);
				if (doSet)
				{
					Grid.Objects[cell2, 5] = base.gameObject;
				}
				else if (Grid.Objects[cell2, 5] == base.gameObject)
				{
					Grid.Objects[cell2, 5] = null;
				}
			}
		}

		private void OnNewGameSpawn(object data)
		{
			DefineBranchArray();
			float percentage = 1f;
			if ((double)UnityEngine.Random.value < 0.1)
			{
				percentage = UnityEngine.Random.Range(0.75f, 0.99f);
			}
			else
			{
				GoTo(base.sm.worldgen);
			}
			growing.OverrideMaturityLevel(percentage);
		}

		public void ManuallyDefineBranchArray(KPrefabID[] _branches)
		{
			DefineBranchArray();
			for (int i = 0; i < Mathf.Min(branches.Length, _branches.Length); i++)
			{
				KPrefabID kPrefabID = _branches[i];
				if (kPrefabID != null)
				{
					if (branches[i] == null)
					{
						branches[i] = new Ref<KPrefabID>();
					}
					branches[i].Set(kPrefabID);
				}
				else
				{
					branches[i] = null;
				}
			}
		}

		private void DefineBranchArray()
		{
			if (branches == null)
			{
				branches = new Ref<KPrefabID>[base.def.BRANCH_OFFSETS.Length];
			}
		}

		public void ActionPerBranch(Action<GameObject> action)
		{
			for (int i = 0; i < branches.Length; i++)
			{
				GameObject branch = GetBranch(i);
				if (branch != null)
				{
					action?.Invoke(branch.gameObject);
				}
			}
		}

		public GameObject[] GetExistingBranches()
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < branches.Length; i++)
			{
				GameObject branch = GetBranch(i);
				if (branch != null)
				{
					list.Add(branch.gameObject);
				}
			}
			return list.ToArray();
		}

		public void OnBranchRemoved(GameObject _branch)
		{
			for (int i = 0; i < branches.Length; i++)
			{
				GameObject branch = GetBranch(i);
				if (branch != null && branch == _branch)
				{
					branches[i] = null;
				}
			}
			base.gameObject.Trigger(-1586842875);
		}

		public void OnBrancHarvested(PlantBranch.Instance branch)
		{
			base.def.onBranchHarvested?.Invoke(branch, this);
		}

		private void OnUprooted(object data = null)
		{
			for (int i = 0; i < branches.Length; i++)
			{
				GameObject branch = GetBranch(i);
				if (branch != null)
				{
					branch.Trigger(-216549700);
				}
			}
		}

		public List<int> GetAvailableSpawnPositions()
		{
			spawn_choices.Clear();
			int cell = Grid.PosToCell(this);
			for (int i = 0; i < base.def.BRANCH_OFFSETS.Length; i++)
			{
				int cell2 = Grid.OffsetCell(cell, base.def.BRANCH_OFFSETS[i]);
				if (GetBranch(i) == null && CanBranchGrowInCell(cell2))
				{
					spawn_choices.Add(i);
				}
			}
			return spawn_choices;
		}

		public void RefreshBranchZPositionOffset(GameObject _branch)
		{
			if (branches == null)
			{
				return;
			}
			for (int i = 0; i < branches.Length; i++)
			{
				GameObject branch = GetBranch(i);
				if (branch != null && branch == _branch)
				{
					Vector3 position = branch.transform.position;
					position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) - 0.8f / (float)branches.Length * (float)i;
					branch.transform.SetPosition(position);
				}
			}
		}

		public bool SpawnRandomBranch(float growth_percentage = 0f)
		{
			if (IsUprooted)
			{
				return false;
			}
			if (CurrentBranchCount >= MaxBranchesAllowedAtOnce)
			{
				return false;
			}
			List<int> availableSpawnPositions = GetAvailableSpawnPositions();
			availableSpawnPositions.Shuffle();
			if (availableSpawnPositions.Count > 0)
			{
				int idx = availableSpawnPositions[0];
				PlantBranch.Instance instance = SpawnBranchAtIndex(idx);
				IManageGrowingStates component = instance.GetComponent<IManageGrowingStates>();
				((component != null) ? component : instance.gameObject.GetSMI<IManageGrowingStates>())?.OverrideMaturityLevel(growth_percentage);
				instance.StartSM();
				base.gameObject.Trigger(-1586842875, instance);
				base.def.onBranchSpawned?.Invoke(instance, this);
				return true;
			}
			return false;
		}

		private PlantBranch.Instance SpawnBranchAtIndex(int idx)
		{
			if (idx < 0 || idx >= branches.Length)
			{
				return null;
			}
			GameObject branch = GetBranch(idx);
			if (branch != null)
			{
				return branch.GetSMI<PlantBranch.Instance>();
			}
			Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), base.def.BRANCH_OFFSETS[idx]), Grid.SceneLayer.BuildingFront);
			GameObject obj = Util.KInstantiate(Assets.GetPrefab(base.def.BRANCH_PREFAB_NAME), position);
			obj.SetActive(value: true);
			PlantBranch.Instance sMI = obj.GetSMI<PlantBranch.Instance>();
			MutantPlant component = GetComponent<MutantPlant>();
			if (component != null)
			{
				MutantPlant component2 = sMI.GetComponent<MutantPlant>();
				if (component2 != null)
				{
					component.CopyMutationsTo(component2);
					PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = component2.GetSubSpeciesInfo();
					PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(subSpeciesInfo, component2);
					PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(subSpeciesInfo.ID);
				}
			}
			UpdateAutoHarvestValue(sMI);
			sMI.SetTrunk(this);
			branches[idx] = new Ref<KPrefabID>();
			branches[idx].Set(sMI.GetComponent<KPrefabID>());
			return sMI;
		}

		private bool CanBranchGrowInCell(int cell)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			if (Grid.Solid[cell])
			{
				return false;
			}
			if (Grid.Objects[cell, 1] != null)
			{
				return false;
			}
			if (Grid.Objects[cell, 5] != null)
			{
				return false;
			}
			if (Grid.Foundation[cell])
			{
				return false;
			}
			int cell2 = Grid.CellAbove(cell);
			if (!Grid.IsValidCell(cell2))
			{
				return false;
			}
			if (Grid.IsSubstantialLiquid(cell2))
			{
				return false;
			}
			if (base.def.additionalBranchGrowRequirements != null && !base.def.additionalBranchGrowRequirements(cell))
			{
				return false;
			}
			return true;
		}

		public void UpdateAutoHarvestValue(PlantBranch.Instance specificBranch = null)
		{
			HarvestDesignatable component = GetComponent<HarvestDesignatable>();
			if (!(component != null) || branches == null)
			{
				return;
			}
			if (specificBranch != null)
			{
				HarvestDesignatable component2 = specificBranch.GetComponent<HarvestDesignatable>();
				if (component2 != null)
				{
					component2.SetHarvestWhenReady(component.HarvestWhenReady);
				}
			}
			else
			{
				if (!base.def.propagateHarvestDesignation)
				{
					return;
				}
				for (int i = 0; i < branches.Length; i++)
				{
					GameObject branch = GetBranch(i);
					if (branch != null)
					{
						HarvestDesignatable component3 = branch.GetComponent<HarvestDesignatable>();
						if (component3 != null)
						{
							component3.SetHarvestWhenReady(component.HarvestWhenReady);
						}
					}
				}
			}
		}
	}

	public State worldgen;

	public State wilt;

	public State maturing;

	public State growingBranches;

	public State fullyGrown;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = wilt;
		worldgen.Update(WorldGenUpdate, UpdateRate.RENDER_EVERY_TICK);
		wilt.TagTransition(GameTags.Wilting, maturing, on_remove: true);
		maturing.TagTransition(GameTags.Wilting, wilt).EnterTransition(growingBranches, IsMature).EventTransition(GameHashes.Grow, growingBranches);
		growingBranches.TagTransition(GameTags.Wilting, wilt).EventTransition(GameHashes.ConsumePlant, maturing, GameStateMachine<PlantBranchGrower, Instance, IStateMachineTarget, Def>.Not(IsMature)).EventTransition(GameHashes.TreeBranchCountChanged, fullyGrown, AllBranchesCreated)
			.ToggleStatusItem((Instance smi) => smi.def.growingBranchesStatusItem)
			.Update(GrowBranchUpdate, UpdateRate.SIM_4000ms);
		fullyGrown.TagTransition(GameTags.Wilting, wilt).EventTransition(GameHashes.ConsumePlant, maturing, GameStateMachine<PlantBranchGrower, Instance, IStateMachineTarget, Def>.Not(IsMature)).EventTransition(GameHashes.TreeBranchCountChanged, growingBranches, NotAllBranchesCreated);
	}

	public static bool NotAllBranchesCreated(Instance smi)
	{
		return smi.CurrentBranchCount < smi.MaxBranchesAllowedAtOnce;
	}

	public static bool AllBranchesCreated(Instance smi)
	{
		return smi.CurrentBranchCount >= smi.MaxBranchesAllowedAtOnce;
	}

	public static bool IsMature(Instance smi)
	{
		return smi.IsGrown;
	}

	public static void GrowBranchUpdate(Instance smi, float dt)
	{
		smi.SpawnRandomBranch();
	}

	public static void WorldGenUpdate(Instance smi, float dt)
	{
		float growth_percentage = UnityEngine.Random.Range(0f, 1f);
		if (!smi.SpawnRandomBranch(growth_percentage))
		{
			smi.GoTo(smi.sm.defaultState);
		}
	}
}
