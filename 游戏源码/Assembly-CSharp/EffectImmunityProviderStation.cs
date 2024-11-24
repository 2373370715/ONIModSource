using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A41 RID: 2625
public class EffectImmunityProviderStation<StateMachineInstanceType> : GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def> where StateMachineInstanceType : EffectImmunityProviderStation<StateMachineInstanceType>.BaseInstance
{
	// Token: 0x0600303A RID: 12346 RVA: 0x001FB530 File Offset: 0x001F9730
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.inactive;
		this.inactive.EventTransition(GameHashes.ActiveChanged, this.active, (StateMachineInstanceType smi) => smi.GetComponent<Operational>().IsActive);
		this.active.EventTransition(GameHashes.ActiveChanged, this.inactive, (StateMachineInstanceType smi) => !smi.GetComponent<Operational>().IsActive);
	}

	// Token: 0x04002080 RID: 8320
	public GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.State inactive;

	// Token: 0x04002081 RID: 8321
	public GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.State active;

	// Token: 0x02000A42 RID: 2626
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0600303C RID: 12348 RVA: 0x000BF573 File Offset: 0x000BD773
		public virtual string[] DefaultAnims()
		{
			return new string[]
			{
				"",
				"",
				""
			};
		}

		// Token: 0x0600303D RID: 12349 RVA: 0x000BD7B6 File Offset: 0x000BB9B6
		public virtual string DefaultAnimFileName()
		{
			return "anim_warmup_kanim";
		}

		// Token: 0x0600303E RID: 12350 RVA: 0x000BF593 File Offset: 0x000BD793
		public string[] GetAnimNames()
		{
			if (this.overrideAnims != null)
			{
				return this.overrideAnims;
			}
			return this.DefaultAnims();
		}

		// Token: 0x0600303F RID: 12351 RVA: 0x000BF5AA File Offset: 0x000BD7AA
		public string GetAnimFileName(GameObject entity)
		{
			if (this.overrideFileName != null)
			{
				return this.overrideFileName(entity);
			}
			return this.DefaultAnimFileName();
		}

		// Token: 0x04002082 RID: 8322
		public Action<GameObject, StateMachineInstanceType> onEffectApplied;

		// Token: 0x04002083 RID: 8323
		public Func<GameObject, bool> specialRequirements;

		// Token: 0x04002084 RID: 8324
		public Func<GameObject, string> overrideFileName;

		// Token: 0x04002085 RID: 8325
		public string[] overrideAnims;

		// Token: 0x04002086 RID: 8326
		public CellOffset[][] range;
	}

	// Token: 0x02000A43 RID: 2627
	public abstract class BaseInstance : GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.GameInstance
	{
		// Token: 0x06003041 RID: 12353 RVA: 0x000BF5C7 File Offset: 0x000BD7C7
		public string GetAnimFileName(GameObject entity)
		{
			return base.def.GetAnimFileName(entity);
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06003042 RID: 12354 RVA: 0x000BF5D5 File Offset: 0x000BD7D5
		public string PreAnimName
		{
			get
			{
				return base.def.GetAnimNames()[0];
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06003043 RID: 12355 RVA: 0x000BF5E4 File Offset: 0x000BD7E4
		public string LoopAnimName
		{
			get
			{
				return base.def.GetAnimNames()[1];
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06003044 RID: 12356 RVA: 0x000BF5F3 File Offset: 0x000BD7F3
		public string PstAnimName
		{
			get
			{
				return base.def.GetAnimNames()[2];
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06003045 RID: 12357 RVA: 0x000BF602 File Offset: 0x000BD802
		public bool CanBeUsed
		{
			get
			{
				return this.IsActive && (base.def.specialRequirements == null || base.def.specialRequirements(base.gameObject));
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06003046 RID: 12358 RVA: 0x000BF633 File Offset: 0x000BD833
		protected bool IsActive
		{
			get
			{
				return base.IsInsideState(base.sm.active);
			}
		}

		// Token: 0x06003047 RID: 12359 RVA: 0x000BF646 File Offset: 0x000BD846
		public BaseInstance(IStateMachineTarget master, EffectImmunityProviderStation<StateMachineInstanceType>.Def def) : base(master, def)
		{
		}

		// Token: 0x06003048 RID: 12360 RVA: 0x001FB5B8 File Offset: 0x001F97B8
		public int GetBestAvailableCell(Navigator dupeLooking, out int _cost)
		{
			_cost = int.MaxValue;
			if (!this.CanBeUsed)
			{
				return Grid.InvalidCell;
			}
			int num = Grid.PosToCell(this);
			int num2 = Grid.InvalidCell;
			if (base.def.range != null)
			{
				for (int i = 0; i < base.def.range.GetLength(0); i++)
				{
					int num3 = int.MaxValue;
					for (int j = 0; j < base.def.range[i].Length; j++)
					{
						int num4 = Grid.OffsetCell(num, base.def.range[i][j]);
						if (dupeLooking.CanReach(num4))
						{
							int navigationCost = dupeLooking.GetNavigationCost(num4);
							if (navigationCost < num3)
							{
								num3 = navigationCost;
								num2 = num4;
							}
						}
					}
					if (num2 != Grid.InvalidCell)
					{
						_cost = num3;
						break;
					}
				}
				return num2;
			}
			if (dupeLooking.CanReach(num))
			{
				_cost = dupeLooking.GetNavigationCost(num);
				return num;
			}
			return Grid.InvalidCell;
		}

		// Token: 0x06003049 RID: 12361 RVA: 0x001FB69C File Offset: 0x001F989C
		public void ApplyImmunityEffect(GameObject target, bool triggerEvents = true)
		{
			Effects component = target.GetComponent<Effects>();
			if (component == null)
			{
				return;
			}
			this.ApplyImmunityEffect(component);
			if (triggerEvents)
			{
				Action<GameObject, StateMachineInstanceType> onEffectApplied = base.def.onEffectApplied;
				if (onEffectApplied == null)
				{
					return;
				}
				onEffectApplied(component.gameObject, (StateMachineInstanceType)((object)this));
			}
		}

		// Token: 0x0600304A RID: 12362
		protected abstract void ApplyImmunityEffect(Effects target);

		// Token: 0x0600304B RID: 12363 RVA: 0x000BF650 File Offset: 0x000BD850
		public override void StartSM()
		{
			Components.EffectImmunityProviderStations.Add(this);
			base.StartSM();
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x000BF663 File Offset: 0x000BD863
		protected override void OnCleanUp()
		{
			Components.EffectImmunityProviderStations.Remove(this);
			base.OnCleanUp();
		}
	}
}
