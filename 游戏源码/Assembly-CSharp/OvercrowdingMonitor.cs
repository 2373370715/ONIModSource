using System;
using System.Diagnostics;
using Klei.AI;
using STRINGS;

// Token: 0x020011B9 RID: 4537
public class OvercrowdingMonitor : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>
{
	// Token: 0x06005C83 RID: 23683 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("DETAILED_OVERCROWDING_MONITOR_PROFILE")]
	private static void BeginDetailedSample(string regionName)
	{
	}

	// Token: 0x06005C84 RID: 23684 RVA: 0x000A5E40 File Offset: 0x000A4040
	[Conditional("DETAILED_OVERCROWDING_MONITOR_PROFILE")]
	private static void EndDetailedSample(string regionName)
	{
	}

	// Token: 0x06005C85 RID: 23685 RVA: 0x000DC62A File Offset: 0x000DA82A
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<OvercrowdingMonitor.Instance, float>(OvercrowdingMonitor.UpdateState), UpdateRate.SIM_1000ms, true);
	}

	// Token: 0x06005C86 RID: 23686 RVA: 0x0029B2A0 File Offset: 0x002994A0
	private static bool IsConfined(OvercrowdingMonitor.Instance smi)
	{
		if (smi.kpid.HasAnyTags(OvercrowdingMonitor.confinementImmunity))
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

	// Token: 0x06005C87 RID: 23687 RVA: 0x0029B324 File Offset: 0x00299524
	private static bool IsFutureOvercrowded(OvercrowdingMonitor.Instance smi)
	{
		if (smi.cavity != null)
		{
			int num = smi.cavity.creatures.Count + smi.cavity.eggs.Count;
			return num != 0 && smi.cavity.eggs.Count != 0 && smi.cavity.numCells / num < smi.def.spaceRequiredPerCreature;
		}
		return false;
	}

	// Token: 0x06005C88 RID: 23688 RVA: 0x0029B390 File Offset: 0x00299590
	private static int CalculateOvercrowdedModifer(OvercrowdingMonitor.Instance smi)
	{
		if (smi.fishOvercrowdingMonitor != null)
		{
			int fishCount = smi.fishOvercrowdingMonitor.fishCount;
			if (fishCount <= 0)
			{
				return 0;
			}
			int num = smi.fishOvercrowdingMonitor.cellCount / smi.def.spaceRequiredPerCreature;
			if (num < smi.fishOvercrowdingMonitor.fishCount)
			{
				return -(fishCount - num);
			}
			return 0;
		}
		else
		{
			if (smi.cavity == null)
			{
				return 0;
			}
			if (smi.cavity.creatures.Count <= 1)
			{
				return 0;
			}
			int num2 = smi.cavity.numCells / smi.def.spaceRequiredPerCreature;
			if (num2 < smi.cavity.creatures.Count)
			{
				return -(smi.cavity.creatures.Count - num2);
			}
			return 0;
		}
	}

	// Token: 0x06005C89 RID: 23689 RVA: 0x0029B444 File Offset: 0x00299644
	private static bool IsOvercrowded(OvercrowdingMonitor.Instance smi)
	{
		if (smi.def.spaceRequiredPerCreature == 0)
		{
			return false;
		}
		if (smi.fishOvercrowdingMonitor == null)
		{
			return smi.cavity != null && smi.cavity.creatures.Count > 1 && smi.cavity.numCells / smi.cavity.creatures.Count < smi.def.spaceRequiredPerCreature;
		}
		int fishCount = smi.fishOvercrowdingMonitor.fishCount;
		if (fishCount > 0)
		{
			return smi.fishOvercrowdingMonitor.cellCount / fishCount < smi.def.spaceRequiredPerCreature;
		}
		int cell = Grid.PosToCell(smi);
		return Grid.IsValidCell(cell) && !Grid.IsLiquid(cell);
	}

	// Token: 0x06005C8A RID: 23690 RVA: 0x0029B4F4 File Offset: 0x002996F4
	private static void UpdateState(OvercrowdingMonitor.Instance smi, float dt)
	{
		bool flag = smi.kpid.HasTag(GameTags.Creatures.Confined);
		bool flag2 = smi.kpid.HasTag(GameTags.Creatures.Expecting);
		bool flag3 = smi.kpid.HasTag(GameTags.Creatures.Overcrowded);
		OvercrowdingMonitor.UpdateCavity(smi, dt);
		if (smi.def.spaceRequiredPerCreature == 0)
		{
			return;
		}
		bool flag4 = OvercrowdingMonitor.IsConfined(smi);
		bool flag5 = OvercrowdingMonitor.IsOvercrowded(smi);
		if (flag5)
		{
			if (!smi.isFish)
			{
				smi.overcrowdedModifier.SetValue((float)OvercrowdingMonitor.CalculateOvercrowdedModifer(smi));
			}
			else
			{
				smi.fishOvercrowdedModifier.SetValue((float)OvercrowdingMonitor.CalculateOvercrowdedModifer(smi));
			}
		}
		bool flag6 = !smi.isBaby && OvercrowdingMonitor.IsFutureOvercrowded(smi);
		if (flag != flag4 || flag2 != flag6 || flag3 != flag5)
		{
			KPrefabID kpid = smi.kpid;
			Effect effect = smi.isFish ? smi.fishOvercrowdedEffect : smi.overcrowdedEffect;
			kpid.SetTag(GameTags.Creatures.Confined, flag4);
			kpid.SetTag(GameTags.Creatures.Overcrowded, flag5);
			kpid.SetTag(GameTags.Creatures.Expecting, flag6);
			OvercrowdingMonitor.SetEffect(smi, smi.stuckEffect, flag4);
			OvercrowdingMonitor.SetEffect(smi, effect, !flag4 && flag5);
			OvercrowdingMonitor.SetEffect(smi, smi.futureOvercrowdedEffect, !flag4 && flag6);
		}
	}

	// Token: 0x06005C8B RID: 23691 RVA: 0x000DC64E File Offset: 0x000DA84E
	private static void SetEffect(OvercrowdingMonitor.Instance smi, Effect effect, bool set)
	{
		if (set)
		{
			smi.effects.Add(effect, false);
			return;
		}
		smi.effects.Remove(effect);
	}

	// Token: 0x06005C8C RID: 23692 RVA: 0x0029B620 File Offset: 0x00299820
	private static void UpdateCavity(OvercrowdingMonitor.Instance smi, float dt)
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(smi));
		if (cavityForCell != smi.cavity)
		{
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

	// Token: 0x04004160 RID: 16736
	public const float OVERCROWDED_FERTILITY_DEBUFF = -1f;

	// Token: 0x04004161 RID: 16737
	public static Tag[] confinementImmunity = new Tag[]
	{
		GameTags.Creatures.Burrowed,
		GameTags.Creatures.Digger
	};

	// Token: 0x020011BA RID: 4538
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04004162 RID: 16738
		public int spaceRequiredPerCreature;
	}

	// Token: 0x020011BB RID: 4539
	public new class Instance : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>.GameInstance
	{
		// Token: 0x06005C90 RID: 23696 RVA: 0x0029B718 File Offset: 0x00299918
		public Instance(IStateMachineTarget master, OvercrowdingMonitor.Def def) : base(master, def)
		{
			BabyMonitor.Def def2 = master.gameObject.GetDef<BabyMonitor.Def>();
			this.isBaby = (def2 != null);
			FishOvercrowdingMonitor.Def def3 = master.gameObject.GetDef<FishOvercrowdingMonitor.Def>();
			this.isFish = (def3 != null);
			this.futureOvercrowdedEffect = new Effect("FutureOvercrowded", CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, CREATURES.MODIFIERS.FUTURE_OVERCROWDED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.futureOvercrowdedEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, true, false, true));
			this.overcrowdedEffect = new Effect("Overcrowded", CREATURES.MODIFIERS.OVERCROWDED.NAME, CREATURES.MODIFIERS.OVERCROWDED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.overcrowdedModifier = new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 0f, CREATURES.MODIFIERS.OVERCROWDED.NAME, false, false, false);
			this.overcrowdedEffect.Add(this.overcrowdedModifier);
			this.fishOvercrowdedEffect = new Effect("Overcrowded", CREATURES.MODIFIERS.OVERCROWDED.NAME, CREATURES.MODIFIERS.OVERCROWDED.FISHTOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.fishOvercrowdedModifier = new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -5f, CREATURES.MODIFIERS.OVERCROWDED.NAME, false, false, false);
			this.fishOvercrowdedEffect.Add(this.fishOvercrowdedModifier);
			this.stuckEffect = new Effect("Confined", CREATURES.MODIFIERS.CONFINED.NAME, CREATURES.MODIFIERS.CONFINED.TOOLTIP, 0f, true, false, true, null, -1f, 0f, null, "");
			this.stuckEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, CREATURES.MODIFIERS.CONFINED.NAME, false, false, true));
			this.stuckEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, CREATURES.MODIFIERS.CONFINED.NAME, true, false, true));
			OvercrowdingMonitor.UpdateState(this, 0f);
		}

		// Token: 0x06005C91 RID: 23697 RVA: 0x0029B988 File Offset: 0x00299B88
		protected override void OnCleanUp()
		{
			if (this.cavity == null)
			{
				return;
			}
			if (this.kpid.HasTag(GameTags.Egg))
			{
				this.cavity.RemoveFromCavity(this.kpid, this.cavity.eggs);
				return;
			}
			this.cavity.RemoveFromCavity(this.kpid, this.cavity.creatures);
		}

		// Token: 0x06005C92 RID: 23698 RVA: 0x000DC69B File Offset: 0x000DA89B
		public void RoomRefreshUpdateCavity()
		{
			OvercrowdingMonitor.UpdateState(this, 0f);
		}

		// Token: 0x04004163 RID: 16739
		public CavityInfo cavity;

		// Token: 0x04004164 RID: 16740
		public bool isBaby;

		// Token: 0x04004165 RID: 16741
		public bool isFish;

		// Token: 0x04004166 RID: 16742
		public Effect futureOvercrowdedEffect;

		// Token: 0x04004167 RID: 16743
		public Effect overcrowdedEffect;

		// Token: 0x04004168 RID: 16744
		public AttributeModifier overcrowdedModifier;

		// Token: 0x04004169 RID: 16745
		public Effect fishOvercrowdedEffect;

		// Token: 0x0400416A RID: 16746
		public AttributeModifier fishOvercrowdedModifier;

		// Token: 0x0400416B RID: 16747
		public Effect stuckEffect;

		// Token: 0x0400416C RID: 16748
		[MyCmpReq]
		public KPrefabID kpid;

		// Token: 0x0400416D RID: 16749
		[MyCmpReq]
		public Effects effects;

		// Token: 0x0400416E RID: 16750
		[MySmiGet]
		public FishOvercrowdingMonitor.Instance fishOvercrowdingMonitor;
	}
}
