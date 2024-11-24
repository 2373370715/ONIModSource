using System.Diagnostics;
using Klei.AI;
using STRINGS;

public class OvercrowdingMonitor : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>
{
	public class Def : BaseDef
	{
		public int spaceRequiredPerCreature;
	}

	public new class Instance : GameInstance
	{
		public CavityInfo cavity;

		public bool isBaby;

		public bool isFish;

		public Effect futureOvercrowdedEffect;

		public Effect overcrowdedEffect;

		public AttributeModifier overcrowdedModifier;

		public Effect fishOvercrowdedEffect;

		public AttributeModifier fishOvercrowdedModifier;

		public Effect stuckEffect;

		[MyCmpReq]
		public KPrefabID kpid;

		[MyCmpReq]
		public Effects effects;

		[MySmiGet]
		public FishOvercrowdingMonitor.Instance fishOvercrowdingMonitor;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			BabyMonitor.Def def2 = master.gameObject.GetDef<BabyMonitor.Def>();
			isBaby = def2 != null;
			FishOvercrowdingMonitor.Def def3 = master.gameObject.GetDef<FishOvercrowdingMonitor.Def>();
			isFish = def3 != null;
			futureOvercrowdedEffect = new Effect("FutureOvercrowded", CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, CREATURES.MODIFIERS.FUTURE_OVERCROWDED.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: true);
			futureOvercrowdedEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, is_multiplier: true));
			overcrowdedEffect = new Effect("Overcrowded", CREATURES.MODIFIERS.OVERCROWDED.NAME, CREATURES.MODIFIERS.OVERCROWDED.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: true);
			overcrowdedModifier = new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 0f, CREATURES.MODIFIERS.OVERCROWDED.NAME, is_multiplier: false, uiOnly: false, is_readonly: false);
			overcrowdedEffect.Add(overcrowdedModifier);
			fishOvercrowdedEffect = new Effect("Overcrowded", CREATURES.MODIFIERS.OVERCROWDED.NAME, CREATURES.MODIFIERS.OVERCROWDED.FISHTOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: true);
			fishOvercrowdedModifier = new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -5f, CREATURES.MODIFIERS.OVERCROWDED.NAME, is_multiplier: false, uiOnly: false, is_readonly: false);
			fishOvercrowdedEffect.Add(fishOvercrowdedModifier);
			stuckEffect = new Effect("Confined", CREATURES.MODIFIERS.CONFINED.NAME, CREATURES.MODIFIERS.CONFINED.TOOLTIP, 0f, show_in_ui: true, trigger_floating_text: false, is_bad: true);
			stuckEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, CREATURES.MODIFIERS.CONFINED.NAME));
			stuckEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.CONFINED.NAME, is_multiplier: true));
			UpdateState(this, 0f);
		}

		protected override void OnCleanUp()
		{
			if (cavity != null)
			{
				if (kpid.HasTag(GameTags.Egg))
				{
					cavity.RemoveFromCavity(kpid, cavity.eggs);
				}
				else
				{
					cavity.RemoveFromCavity(kpid, cavity.creatures);
				}
			}
		}

		public void RoomRefreshUpdateCavity()
		{
			UpdateState(this, 0f);
		}
	}

	public const float OVERCROWDED_FERTILITY_DEBUFF = -1f;

	public static Tag[] confinementImmunity = new Tag[2]
	{
		GameTags.Creatures.Burrowed,
		GameTags.Creatures.Digger
	};

	[Conditional("DETAILED_OVERCROWDING_MONITOR_PROFILE")]
	private static void BeginDetailedSample(string regionName)
	{
	}

	[Conditional("DETAILED_OVERCROWDING_MONITOR_PROFILE")]
	private static void EndDetailedSample(string regionName)
	{
	}

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = root;
		root.Update(UpdateState, UpdateRate.SIM_1000ms, load_balance: true);
	}

	private static bool IsConfined(Instance smi)
	{
		if (smi.kpid.HasAnyTags(confinementImmunity))
		{
			return false;
		}
		if (smi.isFish)
		{
			int cell = Grid.PosToCell(smi);
			if (Grid.IsValidCell(cell) && !Grid.IsLiquid(cell))
			{
				return true;
			}
			if (smi.fishOvercrowdingMonitor.cellCount < smi.def.spaceRequiredPerCreature)
			{
				return true;
			}
		}
		else
		{
			if (smi.cavity == null)
			{
				return true;
			}
			if (smi.cavity.numCells < smi.def.spaceRequiredPerCreature)
			{
				return true;
			}
		}
		return false;
	}

	private static bool IsFutureOvercrowded(Instance smi)
	{
		if (smi.cavity != null)
		{
			int num = smi.cavity.creatures.Count + smi.cavity.eggs.Count;
			if (num == 0 || smi.cavity.eggs.Count == 0)
			{
				return false;
			}
			return smi.cavity.numCells / num < smi.def.spaceRequiredPerCreature;
		}
		return false;
	}

	private static int CalculateOvercrowdedModifer(Instance smi)
	{
		if (smi.fishOvercrowdingMonitor != null)
		{
			int fishCount = smi.fishOvercrowdingMonitor.fishCount;
			if (fishCount > 0)
			{
				int num = smi.fishOvercrowdingMonitor.cellCount / smi.def.spaceRequiredPerCreature;
				if (num < smi.fishOvercrowdingMonitor.fishCount)
				{
					return -(fishCount - num);
				}
				return 0;
			}
			return 0;
		}
		if (smi.cavity == null)
		{
			return 0;
		}
		if (smi.cavity.creatures.Count > 1)
		{
			int num2 = smi.cavity.numCells / smi.def.spaceRequiredPerCreature;
			if (num2 < smi.cavity.creatures.Count)
			{
				return -(smi.cavity.creatures.Count - num2);
			}
			return 0;
		}
		return 0;
	}

	private static bool IsOvercrowded(Instance smi)
	{
		if (smi.def.spaceRequiredPerCreature == 0)
		{
			return false;
		}
		if (smi.fishOvercrowdingMonitor != null)
		{
			int fishCount = smi.fishOvercrowdingMonitor.fishCount;
			if (fishCount > 0)
			{
				return smi.fishOvercrowdingMonitor.cellCount / fishCount < smi.def.spaceRequiredPerCreature;
			}
			int cell = Grid.PosToCell(smi);
			if (Grid.IsValidCell(cell))
			{
				return !Grid.IsLiquid(cell);
			}
			return false;
		}
		if (smi.cavity != null && smi.cavity.creatures.Count > 1)
		{
			return smi.cavity.numCells / smi.cavity.creatures.Count < smi.def.spaceRequiredPerCreature;
		}
		return false;
	}

	private static void UpdateState(Instance smi, float dt)
	{
		bool flag = smi.kpid.HasTag(GameTags.Creatures.Confined);
		bool flag2 = smi.kpid.HasTag(GameTags.Creatures.Expecting);
		bool flag3 = smi.kpid.HasTag(GameTags.Creatures.Overcrowded);
		UpdateCavity(smi, dt);
		if (smi.def.spaceRequiredPerCreature == 0)
		{
			return;
		}
		bool flag4 = IsConfined(smi);
		bool flag5 = IsOvercrowded(smi);
		if (flag5)
		{
			if (!smi.isFish)
			{
				smi.overcrowdedModifier.SetValue(CalculateOvercrowdedModifer(smi));
			}
			else
			{
				smi.fishOvercrowdedModifier.SetValue(CalculateOvercrowdedModifer(smi));
			}
		}
		bool flag6 = !smi.isBaby && IsFutureOvercrowded(smi);
		if (flag != flag4 || flag2 != flag6 || flag3 != flag5)
		{
			KPrefabID kpid = smi.kpid;
			Effect effect = (smi.isFish ? smi.fishOvercrowdedEffect : smi.overcrowdedEffect);
			kpid.SetTag(GameTags.Creatures.Confined, flag4);
			kpid.SetTag(GameTags.Creatures.Overcrowded, flag5);
			kpid.SetTag(GameTags.Creatures.Expecting, flag6);
			SetEffect(smi, smi.stuckEffect, flag4);
			SetEffect(smi, effect, !flag4 && flag5);
			SetEffect(smi, smi.futureOvercrowdedEffect, !flag4 && flag6);
		}
	}

	private static void SetEffect(Instance smi, Effect effect, bool set)
	{
		if (set)
		{
			smi.effects.Add(effect, should_save: false);
		}
		else
		{
			smi.effects.Remove(effect);
		}
	}

	private static void UpdateCavity(Instance smi, float dt)
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(smi));
		if (cavityForCell == smi.cavity)
		{
			return;
		}
		if (smi.cavity != null)
		{
			if (smi.kpid.HasTag(GameTags.Egg))
			{
				smi.cavity.RemoveFromCavity(smi.kpid, smi.cavity.eggs);
			}
			else
			{
				smi.cavity.RemoveFromCavity(smi.kpid, smi.cavity.creatures);
			}
			Game.Instance.roomProber.UpdateRoom(cavityForCell);
		}
		smi.cavity = cavityForCell;
		if (smi.cavity != null)
		{
			if (smi.kpid.HasTag(GameTags.Egg))
			{
				smi.cavity.eggs.Add(smi.kpid);
			}
			else
			{
				smi.cavity.creatures.Add(smi.kpid);
			}
			Game.Instance.roomProber.UpdateRoom(smi.cavity);
		}
	}
}
