using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020016E3 RID: 5859
public class PlantBranchGrower : GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>
{
	// Token: 0x060078A0 RID: 30880 RVA: 0x00311A1C File Offset: 0x0030FC1C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.wilt;
		this.worldgen.Update(new Action<PlantBranchGrower.Instance, float>(PlantBranchGrower.WorldGenUpdate), UpdateRate.RENDER_EVERY_TICK, false);
		this.wilt.TagTransition(GameTags.Wilting, this.maturing, true);
		this.maturing.TagTransition(GameTags.Wilting, this.wilt, false).EnterTransition(this.growingBranches, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature)).EventTransition(GameHashes.Grow, this.growingBranches, null);
		this.growingBranches.TagTransition(GameTags.Wilting, this.wilt, false).EventTransition(GameHashes.ConsumePlant, this.maturing, GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Not(new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature))).EventTransition(GameHashes.TreeBranchCountChanged, this.fullyGrown, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.AllBranchesCreated)).ToggleStatusItem((PlantBranchGrower.Instance smi) => smi.def.growingBranchesStatusItem, null).Update(new Action<PlantBranchGrower.Instance, float>(PlantBranchGrower.GrowBranchUpdate), UpdateRate.SIM_4000ms, false);
		this.fullyGrown.TagTransition(GameTags.Wilting, this.wilt, false).EventTransition(GameHashes.ConsumePlant, this.maturing, GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Not(new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.IsMature))).EventTransition(GameHashes.TreeBranchCountChanged, this.growingBranches, new StateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.Transition.ConditionCallback(PlantBranchGrower.NotAllBranchesCreated));
	}

	// Token: 0x060078A1 RID: 30881 RVA: 0x000EF568 File Offset: 0x000ED768
	public static bool NotAllBranchesCreated(PlantBranchGrower.Instance smi)
	{
		return smi.CurrentBranchCount < smi.MaxBranchesAllowedAtOnce;
	}

	// Token: 0x060078A2 RID: 30882 RVA: 0x000EF578 File Offset: 0x000ED778
	public static bool AllBranchesCreated(PlantBranchGrower.Instance smi)
	{
		return smi.CurrentBranchCount >= smi.MaxBranchesAllowedAtOnce;
	}

	// Token: 0x060078A3 RID: 30883 RVA: 0x000EF58B File Offset: 0x000ED78B
	public static bool IsMature(PlantBranchGrower.Instance smi)
	{
		return smi.IsGrown;
	}

	// Token: 0x060078A4 RID: 30884 RVA: 0x000EF593 File Offset: 0x000ED793
	public static void GrowBranchUpdate(PlantBranchGrower.Instance smi, float dt)
	{
		smi.SpawnRandomBranch(0f);
	}

	// Token: 0x060078A5 RID: 30885 RVA: 0x00311B8C File Offset: 0x0030FD8C
	public static void WorldGenUpdate(PlantBranchGrower.Instance smi, float dt)
	{
		float growth_percentage = UnityEngine.Random.Range(0f, 1f);
		if (!smi.SpawnRandomBranch(growth_percentage))
		{
			smi.GoTo(smi.sm.defaultState);
		}
	}

	// Token: 0x04005A5F RID: 23135
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State worldgen;

	// Token: 0x04005A60 RID: 23136
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State wilt;

	// Token: 0x04005A61 RID: 23137
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State maturing;

	// Token: 0x04005A62 RID: 23138
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State growingBranches;

	// Token: 0x04005A63 RID: 23139
	public GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.State fullyGrown;

	// Token: 0x020016E4 RID: 5860
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005A64 RID: 23140
		public string BRANCH_PREFAB_NAME;

		// Token: 0x04005A65 RID: 23141
		public int MAX_BRANCH_COUNT = -1;

		// Token: 0x04005A66 RID: 23142
		public CellOffset[] BRANCH_OFFSETS;

		// Token: 0x04005A67 RID: 23143
		public bool harvestOnDrown;

		// Token: 0x04005A68 RID: 23144
		public bool propagateHarvestDesignation = true;

		// Token: 0x04005A69 RID: 23145
		public Func<int, bool> additionalBranchGrowRequirements;

		// Token: 0x04005A6A RID: 23146
		public Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchHarvested;

		// Token: 0x04005A6B RID: 23147
		public Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchSpawned;

		// Token: 0x04005A6C RID: 23148
		public StatusItem growingBranchesStatusItem = Db.Get().MiscStatusItems.GrowingBranches;

		// Token: 0x04005A6D RID: 23149
		public Action<PlantBranchGrower.Instance> onEarlySpawn;
	}

	// Token: 0x020016E5 RID: 5861
	public new class Instance : GameStateMachine<PlantBranchGrower, PlantBranchGrower.Instance, IStateMachineTarget, PlantBranchGrower.Def>.GameInstance
	{
		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060078A8 RID: 30888 RVA: 0x000EF5D4 File Offset: 0x000ED7D4
		public bool IsUprooted
		{
			get
			{
				return this.uprootMonitor != null && this.uprootMonitor.IsUprooted;
			}
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x060078A9 RID: 30889 RVA: 0x000EF5F1 File Offset: 0x000ED7F1
		public bool IsGrown
		{
			get
			{
				return this.growing == null || this.growing.PercentGrown() >= 1f;
			}
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x060078AA RID: 30890 RVA: 0x000EF612 File Offset: 0x000ED812
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

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x060078AB RID: 30891 RVA: 0x00311BC4 File Offset: 0x0030FDC4
		public int CurrentBranchCount
		{
			get
			{
				int num = 0;
				if (this.branches != null)
				{
					int i = 0;
					while (i < this.branches.Length)
					{
						num += ((this.GetBranch(i++) != null) ? 1 : 0);
					}
				}
				return num;
			}
		}

		// Token: 0x060078AC RID: 30892 RVA: 0x00311C08 File Offset: 0x0030FE08
		public GameObject GetBranch(int idx)
		{
			if (this.branches != null && this.branches[idx] != null)
			{
				KPrefabID kprefabID = this.branches[idx].Get();
				if (kprefabID != null)
				{
					return kprefabID.gameObject;
				}
			}
			return null;
		}

		// Token: 0x060078AD RID: 30893 RVA: 0x000EF64D File Offset: 0x000ED84D
		protected override void OnCleanUp()
		{
			this.SetTrunkOccupyingCellsAsPlant(false);
			base.OnCleanUp();
		}

		// Token: 0x060078AE RID: 30894 RVA: 0x00311C48 File Offset: 0x0030FE48
		public Instance(IStateMachineTarget master, PlantBranchGrower.Def def) : base(master, def)
		{
			this.growing = base.GetComponent<IManageGrowingStates>();
			this.growing = ((this.growing != null) ? this.growing : base.gameObject.GetSMI<IManageGrowingStates>());
			this.SetTrunkOccupyingCellsAsPlant(true);
			base.Subscribe(1119167081, new Action<object>(this.OnNewGameSpawn));
			base.Subscribe(144050788, new Action<object>(this.OnUpdateRoom));
		}

		// Token: 0x060078AF RID: 30895 RVA: 0x00311CC0 File Offset: 0x0030FEC0
		public override void StartSM()
		{
			base.StartSM();
			Action<PlantBranchGrower.Instance> onEarlySpawn = base.def.onEarlySpawn;
			if (onEarlySpawn != null)
			{
				onEarlySpawn(this);
			}
			this.DefineBranchArray();
			base.Subscribe(-216549700, new Action<object>(this.OnUprooted));
			base.Subscribe(-266953818, delegate(object obj)
			{
				this.UpdateAutoHarvestValue(null);
			});
			if (base.def.harvestOnDrown)
			{
				base.Subscribe(-750750377, new Action<object>(this.OnUprooted));
			}
		}

		// Token: 0x060078B0 RID: 30896 RVA: 0x00311D44 File Offset: 0x0030FF44
		private void OnUpdateRoom(object data)
		{
			if (this.branches == null)
			{
				return;
			}
			this.ActionPerBranch(delegate(GameObject branch)
			{
				branch.Trigger(144050788, data);
			});
		}

		// Token: 0x060078B1 RID: 30897 RVA: 0x00311D7C File Offset: 0x0030FF7C
		private void SetTrunkOccupyingCellsAsPlant(bool doSet)
		{
			CellOffset[] occupiedCellsOffsets = base.GetComponent<OccupyArea>().OccupiedCellsOffsets;
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

		// Token: 0x060078B2 RID: 30898 RVA: 0x00311DFC File Offset: 0x0030FFFC
		private void OnNewGameSpawn(object data)
		{
			this.DefineBranchArray();
			float percentage = 1f;
			if ((double)UnityEngine.Random.value < 0.1)
			{
				percentage = UnityEngine.Random.Range(0.75f, 0.99f);
			}
			else
			{
				this.GoTo(base.sm.worldgen);
			}
			this.growing.OverrideMaturityLevel(percentage);
		}

		// Token: 0x060078B3 RID: 30899 RVA: 0x00311E58 File Offset: 0x00310058
		public void ManuallyDefineBranchArray(KPrefabID[] _branches)
		{
			this.DefineBranchArray();
			for (int i = 0; i < Mathf.Min(this.branches.Length, _branches.Length); i++)
			{
				KPrefabID kprefabID = _branches[i];
				if (kprefabID != null)
				{
					if (this.branches[i] == null)
					{
						this.branches[i] = new Ref<KPrefabID>();
					}
					this.branches[i].Set(kprefabID);
				}
				else
				{
					this.branches[i] = null;
				}
			}
		}

		// Token: 0x060078B4 RID: 30900 RVA: 0x000EF65C File Offset: 0x000ED85C
		private void DefineBranchArray()
		{
			if (this.branches == null)
			{
				this.branches = new Ref<KPrefabID>[base.def.BRANCH_OFFSETS.Length];
			}
		}

		// Token: 0x060078B5 RID: 30901 RVA: 0x00311EC4 File Offset: 0x003100C4
		public void ActionPerBranch(Action<GameObject> action)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null && action != null)
				{
					action(branch.gameObject);
				}
			}
		}

		// Token: 0x060078B6 RID: 30902 RVA: 0x00311F04 File Offset: 0x00310104
		public GameObject[] GetExistingBranches()
		{
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null)
				{
					list.Add(branch.gameObject);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060078B7 RID: 30903 RVA: 0x00311F50 File Offset: 0x00310150
		public void OnBranchRemoved(GameObject _branch)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null && branch == _branch)
				{
					this.branches[i] = null;
				}
			}
			base.gameObject.Trigger(-1586842875, null);
		}

		// Token: 0x060078B8 RID: 30904 RVA: 0x000EF67E File Offset: 0x000ED87E
		public void OnBrancHarvested(PlantBranch.Instance branch)
		{
			Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchHarvested = base.def.onBranchHarvested;
			if (onBranchHarvested == null)
			{
				return;
			}
			onBranchHarvested(branch, this);
		}

		// Token: 0x060078B9 RID: 30905 RVA: 0x00311FA4 File Offset: 0x003101A4
		private void OnUprooted(object data = null)
		{
			for (int i = 0; i < this.branches.Length; i++)
			{
				GameObject branch = this.GetBranch(i);
				if (branch != null)
				{
					branch.Trigger(-216549700, null);
				}
			}
		}

		// Token: 0x060078BA RID: 30906 RVA: 0x00311FE4 File Offset: 0x003101E4
		public List<int> GetAvailableSpawnPositions()
		{
			PlantBranchGrower.Instance.spawn_choices.Clear();
			int cell = Grid.PosToCell(this);
			for (int i = 0; i < base.def.BRANCH_OFFSETS.Length; i++)
			{
				int cell2 = Grid.OffsetCell(cell, base.def.BRANCH_OFFSETS[i]);
				if (this.GetBranch(i) == null && this.CanBranchGrowInCell(cell2))
				{
					PlantBranchGrower.Instance.spawn_choices.Add(i);
				}
			}
			return PlantBranchGrower.Instance.spawn_choices;
		}

		// Token: 0x060078BB RID: 30907 RVA: 0x0031205C File Offset: 0x0031025C
		public void RefreshBranchZPositionOffset(GameObject _branch)
		{
			if (this.branches != null)
			{
				for (int i = 0; i < this.branches.Length; i++)
				{
					GameObject branch = this.GetBranch(i);
					if (branch != null && branch == _branch)
					{
						Vector3 position = branch.transform.position;
						position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) - 0.8f / (float)this.branches.Length * (float)i;
						branch.transform.SetPosition(position);
					}
				}
			}
		}

		// Token: 0x060078BC RID: 30908 RVA: 0x003120D8 File Offset: 0x003102D8
		public bool SpawnRandomBranch(float growth_percentage = 0f)
		{
			if (this.IsUprooted)
			{
				return false;
			}
			if (this.CurrentBranchCount >= this.MaxBranchesAllowedAtOnce)
			{
				return false;
			}
			List<int> availableSpawnPositions = this.GetAvailableSpawnPositions();
			availableSpawnPositions.Shuffle<int>();
			if (availableSpawnPositions.Count > 0)
			{
				int idx = availableSpawnPositions[0];
				PlantBranch.Instance instance = this.SpawnBranchAtIndex(idx);
				IManageGrowingStates manageGrowingStates = instance.GetComponent<IManageGrowingStates>();
				manageGrowingStates = ((manageGrowingStates != null) ? manageGrowingStates : instance.gameObject.GetSMI<IManageGrowingStates>());
				if (manageGrowingStates != null)
				{
					manageGrowingStates.OverrideMaturityLevel(growth_percentage);
				}
				instance.StartSM();
				base.gameObject.Trigger(-1586842875, instance);
				Action<PlantBranch.Instance, PlantBranchGrower.Instance> onBranchSpawned = base.def.onBranchSpawned;
				if (onBranchSpawned != null)
				{
					onBranchSpawned(instance, this);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060078BD RID: 30909 RVA: 0x0031217C File Offset: 0x0031037C
		private PlantBranch.Instance SpawnBranchAtIndex(int idx)
		{
			if (idx < 0 || idx >= this.branches.Length)
			{
				return null;
			}
			GameObject branch = this.GetBranch(idx);
			if (branch != null)
			{
				return branch.GetSMI<PlantBranch.Instance>();
			}
			Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), base.def.BRANCH_OFFSETS[idx]), Grid.SceneLayer.BuildingFront);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(base.def.BRANCH_PREFAB_NAME), position);
			gameObject.SetActive(true);
			PlantBranch.Instance smi = gameObject.GetSMI<PlantBranch.Instance>();
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null)
			{
				MutantPlant component2 = smi.GetComponent<MutantPlant>();
				if (component2 != null)
				{
					component.CopyMutationsTo(component2);
					PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = component2.GetSubSpeciesInfo();
					PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(subSpeciesInfo, component2);
					PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(subSpeciesInfo.ID);
				}
			}
			this.UpdateAutoHarvestValue(smi);
			smi.SetTrunk(this);
			this.branches[idx] = new Ref<KPrefabID>();
			this.branches[idx].Set(smi.GetComponent<KPrefabID>());
			return smi;
		}

		// Token: 0x060078BE RID: 30910 RVA: 0x00312280 File Offset: 0x00310480
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
			return Grid.IsValidCell(cell2) && !Grid.IsSubstantialLiquid(cell2, 0.35f) && (base.def.additionalBranchGrowRequirements == null || base.def.additionalBranchGrowRequirements(cell));
		}

		// Token: 0x060078BF RID: 30911 RVA: 0x00312324 File Offset: 0x00310524
		public void UpdateAutoHarvestValue(PlantBranch.Instance specificBranch = null)
		{
			HarvestDesignatable component = base.GetComponent<HarvestDesignatable>();
			if (component != null && this.branches != null)
			{
				if (specificBranch != null)
				{
					HarvestDesignatable component2 = specificBranch.GetComponent<HarvestDesignatable>();
					if (component2 != null)
					{
						component2.SetHarvestWhenReady(component.HarvestWhenReady);
					}
					return;
				}
				if (base.def.propagateHarvestDesignation)
				{
					for (int i = 0; i < this.branches.Length; i++)
					{
						GameObject branch = this.GetBranch(i);
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

		// Token: 0x04005A6E RID: 23150
		private IManageGrowingStates growing;

		// Token: 0x04005A6F RID: 23151
		[MyCmpGet]
		private UprootedMonitor uprootMonitor;

		// Token: 0x04005A70 RID: 23152
		[Serialize]
		private Ref<KPrefabID>[] branches;

		// Token: 0x04005A71 RID: 23153
		private static List<int> spawn_choices = new List<int>();
	}
}
