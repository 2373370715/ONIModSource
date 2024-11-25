using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class CropTendingStates : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findCrop;
		this.root.Exit(delegate(CropTendingStates.Instance smi)
		{
			this.UnreserveCrop(smi);
			if (!smi.tendedSucceeded)
			{
				this.RestoreSymbolsVisibility(smi);
			}
		});
		this.findCrop.Enter(delegate(CropTendingStates.Instance smi)
		{
			this.FindCrop(smi);
			if (smi.sm.targetCrop.Get(smi) == null)
			{
				smi.GoTo(this.behaviourcomplete);
				return;
			}
			this.ReserverCrop(smi);
			smi.GoTo(this.moveToCrop);
		});
		GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State state = this.moveToCrop;
		string name = CREATURES.STATUSITEMS.DIVERGENT_WILL_TEND.NAME;
		string tooltip = CREATURES.STATUSITEMS.DIVERGENT_WILL_TEND.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).MoveTo((CropTendingStates.Instance smi) => smi.moveCell, this.tendCrop, this.behaviourcomplete, false).ParamTransition<GameObject>(this.targetCrop, this.behaviourcomplete, (CropTendingStates.Instance smi, GameObject p) => this.targetCrop.Get(smi) == null);
		GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State state2 = this.tendCrop.DefaultState(this.tendCrop.pre);
		string name2 = CREATURES.STATUSITEMS.DIVERGENT_TENDING.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.DIVERGENT_TENDING.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main).ParamTransition<GameObject>(this.targetCrop, this.behaviourcomplete, (CropTendingStates.Instance smi, GameObject p) => this.targetCrop.Get(smi) == null).Enter(delegate(CropTendingStates.Instance smi)
		{
			smi.animSet = this.GetCropTendingAnimSet(smi);
			this.StoreSymbolsVisibility(smi);
		});
		this.tendCrop.pre.Face(this.targetCrop, 0f).PlayAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending_pre, KAnim.PlayMode.Once).OnAnimQueueComplete(this.tendCrop.tend);
		this.tendCrop.tend.Enter(delegate(CropTendingStates.Instance smi)
		{
			this.SetSymbolsVisibility(smi, false);
		}).QueueAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending, false, null).OnAnimQueueComplete(this.tendCrop.pst);
		this.tendCrop.pst.QueueAnim((CropTendingStates.Instance smi) => smi.animSet.crop_tending_pst, false, null).OnAnimQueueComplete(this.behaviourcomplete).Exit(delegate(CropTendingStates.Instance smi)
		{
			GameObject gameObject = smi.sm.targetCrop.Get(smi);
			if (gameObject != null)
			{
				if (smi.effect != null)
				{
					gameObject.GetComponent<Effects>().Add(smi.effect, true);
				}
				smi.tendedSucceeded = true;
				CropTendingStates.CropTendingEventData data = new CropTendingStates.CropTendingEventData
				{
					source = smi.gameObject,
					cropId = smi.sm.targetCrop.Get(smi).PrefabID()
				};
				smi.sm.targetCrop.Get(smi).Trigger(90606262, data);
				smi.Trigger(90606262, data);
			}
		});
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToTendCrops, false);
	}

		private CropTendingStates.AnimSet GetCropTendingAnimSet(CropTendingStates.Instance smi)
	{
		CropTendingStates.AnimSet result;
		if (smi.def.animSetOverrides.TryGetValue(this.targetCrop.Get(smi).PrefabID(), out result))
		{
			return result;
		}
		return CropTendingStates.defaultAnimSet;
	}

		private void FindCrop(CropTendingStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		Crop crop = null;
		int moveCell = Grid.InvalidCell;
		int num = 100;
		int num2 = -1;
		foreach (Crop crop2 in Components.Crops.GetWorldItems(smi.gameObject.GetMyWorldId(), false))
		{
			if (Vector2.SqrMagnitude(crop2.transform.position - smi.transform.position) <= 625f)
			{
				if (smi.effect != null)
				{
					Effects component2 = crop2.GetComponent<Effects>();
					if (component2 != null)
					{
						bool flag = false;
						foreach (string effect_id in smi.def.ignoreEffectGroup)
						{
							if (component2.HasEffect(effect_id))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							continue;
						}
					}
				}
				Growing component3 = crop2.GetComponent<Growing>();
				if (!(component3 != null) || !component3.IsGrown())
				{
					KPrefabID component4 = crop2.GetComponent<KPrefabID>();
					if (!component4.HasTag(GameTags.Creatures.ReservedByCreature))
					{
						int num3;
						smi.def.interests.TryGetValue(crop2.PrefabID(), out num3);
						if (num3 >= num2)
						{
							bool flag2 = num3 > num2;
							int cell = Grid.PosToCell(crop2);
							int[] array = new int[]
							{
								Grid.CellLeft(cell),
								Grid.CellRight(cell)
							};
							if (component4.HasTag(GameTags.PlantedOnFloorVessel))
							{
								array = new int[]
								{
									Grid.CellLeft(cell),
									Grid.CellRight(cell),
									Grid.CellDownLeft(cell),
									Grid.CellDownRight(cell)
								};
							}
							int num4 = 100;
							int num5 = Grid.InvalidCell;
							for (int j = 0; j < array.Length; j++)
							{
								if (Grid.IsValidCell(array[j]))
								{
									int navigationCost = component.GetNavigationCost(array[j]);
									if (navigationCost != -1 && navigationCost < num4)
									{
										num4 = navigationCost;
										num5 = array[j];
									}
								}
							}
							if (num4 != -1 && num5 != Grid.InvalidCell && (flag2 || num4 < num))
							{
								moveCell = num5;
								num = num4;
								num2 = num3;
								crop = crop2;
							}
						}
					}
				}
			}
		}
		GameObject value = (crop != null) ? crop.gameObject : null;
		smi.sm.targetCrop.Set(value, smi, false);
		smi.moveCell = moveCell;
	}

		private void ReserverCrop(CropTendingStates.Instance smi)
	{
		GameObject gameObject = smi.sm.targetCrop.Get(smi);
		if (gameObject != null)
		{
			DebugUtil.Assert(!gameObject.HasTag(GameTags.Creatures.ReservedByCreature));
			gameObject.AddTag(GameTags.Creatures.ReservedByCreature);
		}
	}

		private void UnreserveCrop(CropTendingStates.Instance smi)
	{
		GameObject gameObject = smi.sm.targetCrop.Get(smi);
		if (gameObject != null)
		{
			gameObject.RemoveTag(GameTags.Creatures.ReservedByCreature);
		}
	}

		private void SetSymbolsVisibility(CropTendingStates.Instance smi, bool isVisible)
	{
		if (this.targetCrop.Get(smi) != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					foreach (string str in hide_symbols_after_pre)
					{
						component.SetSymbolVisiblity(str, isVisible);
					}
				}
			}
		}
	}

		private void StoreSymbolsVisibility(CropTendingStates.Instance smi)
	{
		if (this.targetCrop.Get(smi) != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					smi.symbolStates = new bool[hide_symbols_after_pre.Length];
					for (int i = 0; i < hide_symbols_after_pre.Length; i++)
					{
						smi.symbolStates[i] = component.GetSymbolVisiblity(hide_symbols_after_pre[i]);
					}
				}
			}
		}
	}

		private void RestoreSymbolsVisibility(CropTendingStates.Instance smi)
	{
		if (this.targetCrop.Get(smi) != null && smi.symbolStates != null)
		{
			string[] hide_symbols_after_pre = smi.animSet.hide_symbols_after_pre;
			if (hide_symbols_after_pre != null)
			{
				KAnimControllerBase component = this.targetCrop.Get(smi).GetComponent<KAnimControllerBase>();
				if (component != null)
				{
					for (int i = 0; i < hide_symbols_after_pre.Length; i++)
					{
						component.SetSymbolVisiblity(hide_symbols_after_pre[i], smi.symbolStates[i]);
					}
				}
			}
		}
	}

		private const int MAX_NAVIGATE_DISTANCE = 100;

		private const int MAX_SQR_EUCLIDEAN_DISTANCE = 625;

		private static CropTendingStates.AnimSet defaultAnimSet = new CropTendingStates.AnimSet
	{
		crop_tending_pre = "crop_tending_pre",
		crop_tending = "crop_tending_loop",
		crop_tending_pst = "crop_tending_pst"
	};

		public StateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.TargetParameter targetCrop;

		private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State findCrop;

		private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State moveToCrop;

		private CropTendingStates.TendingStates tendCrop;

		private GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State behaviourcomplete;

		public class AnimSet
	{
				public string crop_tending_pre;

				public string crop_tending;

				public string crop_tending_pst;

				public string[] hide_symbols_after_pre;
	}

		public class CropTendingEventData
	{
				public GameObject source;

				public Tag cropId;
	}

		public class Def : StateMachine.BaseDef
	{
				public string effectId;

				public string[] ignoreEffectGroup;

				public Dictionary<Tag, int> interests = new Dictionary<Tag, int>();

				public Dictionary<Tag, CropTendingStates.AnimSet> animSetOverrides = new Dictionary<Tag, CropTendingStates.AnimSet>();
	}

		public new class Instance : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.GameInstance
	{
				public Instance(Chore<CropTendingStates.Instance> chore, CropTendingStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToTendCrops);
			this.effect = Db.Get().effects.TryGet(base.smi.def.effectId);
		}

				public Effect effect;

				public int moveCell;

				public CropTendingStates.AnimSet animSet;

				public bool tendedSucceeded;

				public bool[] symbolStates;
	}

		public class TendingStates : GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State
	{
				public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State pre;

				public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State tend;

				public GameStateMachine<CropTendingStates, CropTendingStates.Instance, IStateMachineTarget, CropTendingStates.Def>.State pst;
	}
}
