using System;
using UnityEngine;

// Token: 0x02000C48 RID: 3144
public class BionicUpgrade_ExplorerBooster : GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>
{
	// Token: 0x06003C43 RID: 15427 RVA: 0x0022D780 File Offset: 0x0022B980
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.not_ready;
		this.not_ready.ParamTransition<float>(this.Progress, this.ready, GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.IsGTEOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBooster, null);
		this.ready.ParamTransition<float>(this.Progress, this.not_ready, GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.IsLTOne).ToggleStatusItem(Db.Get().MiscStatusItems.BionicExplorerBoosterReady, null);
	}

	// Token: 0x04002931 RID: 10545
	public const float DataGatheringDuration = 600f;

	// Token: 0x04002932 RID: 10546
	private StateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.FloatParameter Progress;

	// Token: 0x04002933 RID: 10547
	public GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.State not_ready;

	// Token: 0x04002934 RID: 10548
	public GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.State ready;

	// Token: 0x02000C49 RID: 3145
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000C4A RID: 3146
	public new class Instance : GameStateMachine<BionicUpgrade_ExplorerBooster, BionicUpgrade_ExplorerBooster.Instance, IStateMachineTarget, BionicUpgrade_ExplorerBooster.Def>.GameInstance
	{
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06003C46 RID: 15430 RVA: 0x000C6F1D File Offset: 0x000C511D
		public bool IsBeingMonitored
		{
			get
			{
				return this.monitor != null;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06003C47 RID: 15431 RVA: 0x000C6F28 File Offset: 0x000C5128
		public bool IsReady
		{
			get
			{
				return this.Progress == 1f;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06003C48 RID: 15432 RVA: 0x000C6F37 File Offset: 0x000C5137
		public float Progress
		{
			get
			{
				return base.sm.Progress.Get(this);
			}
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x000C6F4A File Offset: 0x000C514A
		public Instance(IStateMachineTarget master, BionicUpgrade_ExplorerBooster.Def def) : base(master, def)
		{
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x000C6F54 File Offset: 0x000C5154
		public void SetMonitor(BionicUpgrade_ExplorerBoosterMonitor.Instance monitor)
		{
			this.monitor = monitor;
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x0022D800 File Offset: 0x0022BA00
		public void AddData(float dataProgressDelta)
		{
			float dataProgress = Mathf.Clamp(this.Progress + dataProgressDelta, 0f, 1f);
			this.SetDataProgress(dataProgress);
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x000C6F5D File Offset: 0x000C515D
		public void SetDataProgress(float dataProgress)
		{
			Mathf.Clamp(dataProgress, 0f, 1f);
			base.sm.Progress.Set(dataProgress, this, false);
		}

		// Token: 0x04002935 RID: 10549
		private BionicUpgrade_ExplorerBoosterMonitor.Instance monitor;
	}
}
