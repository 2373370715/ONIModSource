using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200190D RID: 6413
public class OrbitalDeployCargoModule : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>
{
	// Token: 0x0600858B RID: 34187 RVA: 0x00348F84 File Offset: 0x00347184
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.ClusterDestinationReached, delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			if (smi.AutoDeploy && smi.IsValidDropLocation())
			{
				smi.DeployCargoPods();
			}
		});
		this.grounded.DefaultState(this.grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false);
		this.grounded.loading.PlayAnim((OrbitalDeployCargoModule.StatesInstance smi) => smi.GetLoadingAnimName(), KAnim.PlayMode.Once).ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).OnAnimQueueComplete(this.grounded.loaded);
		this.grounded.loaded.ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).EventTransition(GameHashes.OnStorageChange, this.grounded.loading, (OrbitalDeployCargoModule.StatesInstance smi) => smi.NeedsVisualUpdate());
		this.grounded.empty.Enter(delegate(OrbitalDeployCargoModule.StatesInstance smi)
		{
			this.numVisualCapsules.Set(0, smi, false);
		}).PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsTrue);
		this.not_grounded.DefaultState(this.not_grounded.loaded).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.loaded.PlayAnim("loaded").ParamTransition<bool>(this.hasCargo, this.not_grounded.empty, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsFalse).OnSignal(this.emptyCargo, this.not_grounded.emptying);
		this.not_grounded.emptying.PlayAnim("deploying").GoTo(this.not_grounded.empty);
		this.not_grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.not_grounded.loaded, GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IsTrue);
	}

	// Token: 0x040064E2 RID: 25826
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.BoolParameter hasCargo;

	// Token: 0x040064E3 RID: 25827
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.Signal emptyCargo;

	// Token: 0x040064E4 RID: 25828
	public OrbitalDeployCargoModule.GroundedStates grounded;

	// Token: 0x040064E5 RID: 25829
	public OrbitalDeployCargoModule.NotGroundedStates not_grounded;

	// Token: 0x040064E6 RID: 25830
	public StateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.IntParameter numVisualCapsules;

	// Token: 0x0200190E RID: 6414
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040064E7 RID: 25831
		public float numCapsules;
	}

	// Token: 0x0200190F RID: 6415
	public class GroundedStates : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State
	{
		// Token: 0x040064E8 RID: 25832
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loading;

		// Token: 0x040064E9 RID: 25833
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loaded;

		// Token: 0x040064EA RID: 25834
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State empty;
	}

	// Token: 0x02001910 RID: 6416
	public class NotGroundedStates : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State
	{
		// Token: 0x040064EB RID: 25835
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State loaded;

		// Token: 0x040064EC RID: 25836
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State emptying;

		// Token: 0x040064ED RID: 25837
		public GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.State empty;
	}

	// Token: 0x02001911 RID: 6417
	public class StatesInstance : GameStateMachine<OrbitalDeployCargoModule, OrbitalDeployCargoModule.StatesInstance, IStateMachineTarget, OrbitalDeployCargoModule.Def>.GameInstance, IEmptyableCargo
	{
		// Token: 0x06008591 RID: 34193 RVA: 0x00349200 File Offset: 0x00347400
		public StatesInstance(IStateMachineTarget master, OrbitalDeployCargoModule.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<Storage>();
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new LoadingCompleteCondition(this.storage));
			base.gameObject.Subscribe(-1683615038, new Action<object>(this.SetupMeter));
		}

		// Token: 0x06008592 RID: 34194 RVA: 0x000F7963 File Offset: 0x000F5B63
		private void SetupMeter(object obj)
		{
			KBatchedAnimTracker componentInChildren = base.gameObject.GetComponentInChildren<KBatchedAnimTracker>();
			componentInChildren.forceAlwaysAlive = true;
			componentInChildren.matchParentOffset = true;
		}

		// Token: 0x06008593 RID: 34195 RVA: 0x000F797D File Offset: 0x000F5B7D
		protected override void OnCleanUp()
		{
			base.gameObject.Unsubscribe(-1683615038, new Action<object>(this.SetupMeter));
			base.OnCleanUp();
		}

		// Token: 0x06008594 RID: 34196 RVA: 0x00349258 File Offset: 0x00347458
		public bool NeedsVisualUpdate()
		{
			int num = base.sm.numVisualCapsules.Get(this);
			int num2 = Mathf.FloorToInt(this.storage.MassStored() / 200f);
			if (num < num2)
			{
				base.sm.numVisualCapsules.Delta(1, this);
				return true;
			}
			return false;
		}

		// Token: 0x06008595 RID: 34197 RVA: 0x003492A8 File Offset: 0x003474A8
		public string GetLoadingAnimName()
		{
			int num = base.sm.numVisualCapsules.Get(this);
			int num2 = Mathf.RoundToInt(this.storage.capacityKg / 200f);
			if (num == num2)
			{
				return "loading6_full";
			}
			if (num == num2 - 1)
			{
				return "loading5";
			}
			if (num == num2 - 2)
			{
				return "loading4";
			}
			if (num == num2 - 3 || num > 2)
			{
				return "loading3_repeat";
			}
			if (num == 2)
			{
				return "loading2";
			}
			if (num == 1)
			{
				return "loading1";
			}
			return "deployed";
		}

		// Token: 0x06008596 RID: 34198 RVA: 0x0034932C File Offset: 0x0034752C
		public void DeployCargoPods()
		{
			Clustercraft component = base.master.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			ClusterGridEntity orbitAsteroid = component.GetOrbitAsteroid();
			if (orbitAsteroid != null)
			{
				WorldContainer component2 = orbitAsteroid.GetComponent<WorldContainer>();
				int id = component2.id;
				Vector3 position = new Vector3(component2.minimumBounds.x + 1f, component2.maximumBounds.y, Grid.GetLayerZ(Grid.SceneLayer.Front));
				while (this.storage.MassStored() > 0f)
				{
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("RailGunPayload"), position);
					gameObject.GetComponent<Pickupable>().deleteOffGrid = false;
					float num = 0f;
					while (num < 200f && this.storage.MassStored() > 0f)
					{
						num += this.storage.Transfer(gameObject.GetComponent<Storage>(), GameTags.Stored, 200f - num, false, true);
					}
					gameObject.SetActive(true);
					gameObject.GetSMI<RailGunPayload.StatesInstance>().Travel(component.Location, component2.GetMyWorldLocation());
				}
			}
			this.CheckIfLoaded();
		}

		// Token: 0x06008597 RID: 34199 RVA: 0x0034944C File Offset: 0x0034764C
		public bool CheckIfLoaded()
		{
			bool flag = this.storage.MassStored() > 0f;
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x06008598 RID: 34200 RVA: 0x000F79A1 File Offset: 0x000F5BA1
		public bool IsValidDropLocation()
		{
			return base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetOrbitAsteroid() != null;
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06008599 RID: 34201 RVA: 0x000F79BE File Offset: 0x000F5BBE
		// (set) Token: 0x0600859A RID: 34202 RVA: 0x000F79C6 File Offset: 0x000F5BC6
		public bool AutoDeploy
		{
			get
			{
				return this.autoDeploy;
			}
			set
			{
				this.autoDeploy = value;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x0600859B RID: 34203 RVA: 0x000A65EC File Offset: 0x000A47EC
		public bool CanAutoDeploy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600859C RID: 34204 RVA: 0x000F79CF File Offset: 0x000F5BCF
		public void EmptyCargo()
		{
			this.DeployCargoPods();
		}

		// Token: 0x0600859D RID: 34205 RVA: 0x000F79D7 File Offset: 0x000F5BD7
		public bool CanEmptyCargo()
		{
			return base.sm.hasCargo.Get(base.smi) && this.IsValidDropLocation();
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x0600859E RID: 34206 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool ChooseDuplicant
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x0600859F RID: 34207 RVA: 0x000AD332 File Offset: 0x000AB532
		// (set) Token: 0x060085A0 RID: 34208 RVA: 0x000A5E40 File Offset: 0x000A4040
		public MinionIdentity ChosenDuplicant
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x060085A1 RID: 34209 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool ModuleDeployed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040064EE RID: 25838
		private Storage storage;

		// Token: 0x040064EF RID: 25839
		[Serialize]
		private bool autoDeploy;
	}
}
