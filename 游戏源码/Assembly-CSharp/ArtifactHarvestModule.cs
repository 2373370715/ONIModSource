using System;
using UnityEngine;

// Token: 0x020018A7 RID: 6311
public class ArtifactHarvestModule : GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>
{
	// Token: 0x060082C0 RID: 33472 RVA: 0x0033DEB8 File Offset: 0x0033C0B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.root.Enter(delegate(ArtifactHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanHarvest();
		});
		this.grounded.TagTransition(GameTags.RocketNotOnGround, this.not_grounded, false);
		this.not_grounded.DefaultState(this.not_grounded.not_harvesting).EventHandler(GameHashes.ClusterLocationChanged, (ArtifactHarvestModule.StatesInstance smi) => Game.Instance, delegate(ArtifactHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanHarvest();
		}).EventHandler(GameHashes.OnStorageChange, delegate(ArtifactHarvestModule.StatesInstance smi)
		{
			smi.CheckIfCanHarvest();
		}).TagTransition(GameTags.RocketNotOnGround, this.grounded, true);
		this.not_grounded.not_harvesting.PlayAnim("loaded").ParamTransition<bool>(this.canHarvest, this.not_grounded.harvesting, GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.IsTrue);
		this.not_grounded.harvesting.PlayAnim("deploying").Update(delegate(ArtifactHarvestModule.StatesInstance smi, float dt)
		{
			smi.HarvestFromPOI(dt);
		}, UpdateRate.SIM_4000ms, false).ParamTransition<bool>(this.canHarvest, this.not_grounded.not_harvesting, GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.IsFalse);
	}

	// Token: 0x04006335 RID: 25397
	public StateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.BoolParameter canHarvest;

	// Token: 0x04006336 RID: 25398
	public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State grounded;

	// Token: 0x04006337 RID: 25399
	public ArtifactHarvestModule.NotGroundedStates not_grounded;

	// Token: 0x020018A8 RID: 6312
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018A9 RID: 6313
	public class NotGroundedStates : GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State
	{
		// Token: 0x04006338 RID: 25400
		public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State not_harvesting;

		// Token: 0x04006339 RID: 25401
		public GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.State harvesting;
	}

	// Token: 0x020018AA RID: 6314
	public class StatesInstance : GameStateMachine<ArtifactHarvestModule, ArtifactHarvestModule.StatesInstance, IStateMachineTarget, ArtifactHarvestModule.Def>.GameInstance
	{
		// Token: 0x060082C4 RID: 33476 RVA: 0x000F5F52 File Offset: 0x000F4152
		public StatesInstance(IStateMachineTarget master, ArtifactHarvestModule.Def def) : base(master, def)
		{
		}

		// Token: 0x060082C5 RID: 33477 RVA: 0x0033E034 File Offset: 0x0033C234
		public void HarvestFromPOI(float dt)
		{
			ClusterGridEntity poiatCurrentLocation = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>().GetPOIAtCurrentLocation();
			if (poiatCurrentLocation.IsNullOrDestroyed())
			{
				return;
			}
			ArtifactPOIStates.Instance smi = poiatCurrentLocation.GetSMI<ArtifactPOIStates.Instance>();
			if ((poiatCurrentLocation.GetComponent<ArtifactPOIClusterGridEntity>() || poiatCurrentLocation.GetComponent<HarvestablePOIClusterGridEntity>()) && !smi.IsNullOrDestroyed())
			{
				bool flag = false;
				string artifactToHarvest = smi.GetArtifactToHarvest();
				if (artifactToHarvest != null)
				{
					GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(artifactToHarvest), base.transform.position);
					gameObject.SetActive(true);
					this.receptacle.ForceDeposit(gameObject);
					this.storage.Store(gameObject, false, false, true, false);
					smi.HarvestArtifact();
					if (smi.configuration.DestroyOnHarvest())
					{
						flag = true;
					}
					if (flag)
					{
						poiatCurrentLocation.gameObject.DeleteObject();
					}
				}
			}
		}

		// Token: 0x060082C6 RID: 33478 RVA: 0x0033E0FC File Offset: 0x0033C2FC
		public bool CheckIfCanHarvest()
		{
			Clustercraft component = base.GetComponent<RocketModuleCluster>().CraftInterface.GetComponent<Clustercraft>();
			if (component == null)
			{
				return false;
			}
			ClusterGridEntity poiatCurrentLocation = component.GetPOIAtCurrentLocation();
			if (poiatCurrentLocation != null && (poiatCurrentLocation.GetComponent<ArtifactPOIClusterGridEntity>() || poiatCurrentLocation.GetComponent<HarvestablePOIClusterGridEntity>()))
			{
				ArtifactPOIStates.Instance smi = poiatCurrentLocation.GetSMI<ArtifactPOIStates.Instance>();
				if (smi != null && smi.CanHarvestArtifact() && this.receptacle.Occupant == null)
				{
					base.sm.canHarvest.Set(true, this, false);
					return true;
				}
			}
			base.sm.canHarvest.Set(false, this, false);
			return false;
		}

		// Token: 0x0400633A RID: 25402
		[MyCmpReq]
		private Storage storage;

		// Token: 0x0400633B RID: 25403
		[MyCmpReq]
		private SingleEntityReceptacle receptacle;
	}
}
