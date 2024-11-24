using System;
using Klei.AI;
using UnityEngine;

public class EffectImmunityProviderStation<StateMachineInstanceType> : GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def> where StateMachineInstanceType : EffectImmunityProviderStation<StateMachineInstanceType>.BaseInstance
{
	public class Def : BaseDef
	{
		public Action<GameObject, StateMachineInstanceType> onEffectApplied;

		public Func<GameObject, bool> specialRequirements;

		public string overrideFileName;

		public string[] overrideAnims;

		public CellOffset[][] range;

		public virtual string[] DefaultAnims()
		{
			return new string[3] { "", "", "" };
		}

		public virtual string DefaultAnimFileName()
		{
			return "anim_warmup_kanim";
		}

		public string[] GetAnimNames()
		{
			if (overrideAnims != null)
			{
				return overrideAnims;
			}
			return DefaultAnims();
		}

		public string GetAnimFileName()
		{
			if (overrideFileName != null)
			{
				return overrideFileName;
			}
			return DefaultAnimFileName();
		}
	}

	public abstract class BaseInstance : GameInstance
	{
		public string AnimFileName => base.def.GetAnimFileName();

		public string PreAnimName => base.def.GetAnimNames()[0];

		public string LoopAnimName => base.def.GetAnimNames()[1];

		public string PstAnimName => base.def.GetAnimNames()[2];

		public bool CanBeUsed
		{
			get
			{
				if (IsActive)
				{
					if (base.def.specialRequirements != null)
					{
						return base.def.specialRequirements(base.gameObject);
					}
					return true;
				}
				return false;
			}
		}

		protected bool IsActive => IsInsideState(base.sm.active);

		public BaseInstance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public int GetBestAvailableCell(Navigator dupeLooking, out int _cost)
		{
			_cost = int.MaxValue;
			if (!CanBeUsed)
			{
				return Grid.InvalidCell;
			}
			int num = Grid.PosToCell(this);
			int num2 = Grid.InvalidCell;
			if (base.def.range == null)
			{
				if (dupeLooking.CanReach(num))
				{
					_cost = dupeLooking.GetNavigationCost(num);
					return num;
				}
				return Grid.InvalidCell;
			}
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

		public void ApplyImmunityEffect(GameObject target, bool triggerEvents = true)
		{
			Effects component = target.GetComponent<Effects>();
			if (!(component == null))
			{
				ApplyImmunityEffect(component);
				if (triggerEvents)
				{
					base.def.onEffectApplied?.Invoke(component.gameObject, (StateMachineInstanceType)this);
				}
			}
		}

		protected abstract void ApplyImmunityEffect(Effects target);

		public override void StartSM()
		{
			Components.EffectImmunityProviderStations.Add(this);
			base.StartSM();
		}

		protected override void OnCleanUp()
		{
			Components.EffectImmunityProviderStations.Remove(this);
			base.OnCleanUp();
		}
	}

	public State inactive;

	public State active;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = inactive;
		inactive.EventTransition(GameHashes.ActiveChanged, active, (StateMachineInstanceType smi) => smi.GetComponent<Operational>().IsActive);
		active.EventTransition(GameHashes.ActiveChanged, inactive, (StateMachineInstanceType smi) => !smi.GetComponent<Operational>().IsActive);
	}
}
