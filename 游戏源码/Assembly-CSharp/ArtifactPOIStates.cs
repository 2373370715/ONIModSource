using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020018B1 RID: 6321
[AddComponentMenu("KMonoBehaviour/scripts/ArtifactPOIStates")]
public class ArtifactPOIStates : GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>
{
	// Token: 0x060082E4 RID: 33508 RVA: 0x0033E3A4 File Offset: 0x0033C5A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.root.Enter(delegate(ArtifactPOIStates.Instance smi)
		{
			if (smi.configuration == null || smi.configuration.typeId == HashedString.Invalid)
			{
				smi.configuration = smi.GetComponent<ArtifactPOIConfigurator>().MakeConfiguration();
				smi.PickNewArtifactToHarvest();
				smi.poiCharge = 1f;
			}
		});
		this.idle.ParamTransition<float>(this.poiCharge, this.recharging, (ArtifactPOIStates.Instance smi, float f) => f < 1f);
		this.recharging.ParamTransition<float>(this.poiCharge, this.idle, (ArtifactPOIStates.Instance smi, float f) => f >= 1f).EventHandler(GameHashes.NewDay, (ArtifactPOIStates.Instance smi) => GameClock.Instance, delegate(ArtifactPOIStates.Instance smi)
		{
			smi.RechargePOI(600f);
		});
	}

	// Token: 0x04006356 RID: 25430
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State idle;

	// Token: 0x04006357 RID: 25431
	public GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.State recharging;

	// Token: 0x04006358 RID: 25432
	public StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter poiCharge = new StateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.FloatParameter(1f);

	// Token: 0x020018B2 RID: 6322
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020018B3 RID: 6323
	public new class Instance : GameStateMachine<ArtifactPOIStates, ArtifactPOIStates.Instance, IStateMachineTarget, ArtifactPOIStates.Def>.GameInstance, IGameObjectEffectDescriptor
	{
		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x060082E7 RID: 33511 RVA: 0x000F604B File Offset: 0x000F424B
		// (set) Token: 0x060082E8 RID: 33512 RVA: 0x000F6053 File Offset: 0x000F4253
		public float poiCharge
		{
			get
			{
				return this._poiCharge;
			}
			set
			{
				this._poiCharge = value;
				base.smi.sm.poiCharge.Set(value, base.smi, false);
			}
		}

		// Token: 0x060082E9 RID: 33513 RVA: 0x000F607A File Offset: 0x000F427A
		public Instance(IStateMachineTarget target, ArtifactPOIStates.Def def) : base(target, def)
		{
		}

		// Token: 0x060082EA RID: 33514 RVA: 0x0033E4A4 File Offset: 0x0033C6A4
		public void PickNewArtifactToHarvest()
		{
			if (this.numHarvests <= 0 && !string.IsNullOrEmpty(this.configuration.GetArtifactID()))
			{
				this.artifactToHarvest = this.configuration.GetArtifactID();
				ArtifactSelector.Instance.ReserveArtifactID(this.artifactToHarvest, ArtifactType.Any);
				return;
			}
			this.artifactToHarvest = ArtifactSelector.Instance.GetUniqueArtifactID(ArtifactType.Space);
		}

		// Token: 0x060082EB RID: 33515 RVA: 0x000F6084 File Offset: 0x000F4284
		public string GetArtifactToHarvest()
		{
			if (this.CanHarvestArtifact())
			{
				if (string.IsNullOrEmpty(this.artifactToHarvest))
				{
					this.PickNewArtifactToHarvest();
				}
				return this.artifactToHarvest;
			}
			return null;
		}

		// Token: 0x060082EC RID: 33516 RVA: 0x000F60A9 File Offset: 0x000F42A9
		public void HarvestArtifact()
		{
			if (this.CanHarvestArtifact())
			{
				this.numHarvests++;
				this.poiCharge = 0f;
				this.artifactToHarvest = null;
				this.PickNewArtifactToHarvest();
			}
		}

		// Token: 0x060082ED RID: 33517 RVA: 0x0033E500 File Offset: 0x0033C700
		public void RechargePOI(float dt)
		{
			float delta = dt / this.configuration.GetRechargeTime();
			this.DeltaPOICharge(delta);
		}

		// Token: 0x060082EE RID: 33518 RVA: 0x000F60D9 File Offset: 0x000F42D9
		public float RechargeTimeRemaining()
		{
			return (float)Mathf.CeilToInt((this.configuration.GetRechargeTime() - this.configuration.GetRechargeTime() * this.poiCharge) / 600f) * 600f;
		}

		// Token: 0x060082EF RID: 33519 RVA: 0x000F610B File Offset: 0x000F430B
		public void DeltaPOICharge(float delta)
		{
			this.poiCharge += delta;
			this.poiCharge = Mathf.Min(1f, this.poiCharge);
		}

		// Token: 0x060082F0 RID: 33520 RVA: 0x000F6131 File Offset: 0x000F4331
		public bool CanHarvestArtifact()
		{
			return this.poiCharge >= 1f;
		}

		// Token: 0x060082F1 RID: 33521 RVA: 0x000C9B47 File Offset: 0x000C7D47
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>();
		}

		// Token: 0x04006359 RID: 25433
		[Serialize]
		public ArtifactPOIConfigurator.ArtifactPOIInstanceConfiguration configuration;

		// Token: 0x0400635A RID: 25434
		[Serialize]
		private float _poiCharge;

		// Token: 0x0400635B RID: 25435
		[Serialize]
		public string artifactToHarvest;

		// Token: 0x0400635C RID: 25436
		[Serialize]
		private int numHarvests;
	}
}
