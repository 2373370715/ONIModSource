using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000136 RID: 310
public class BuzzStates : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>
{
	// Token: 0x06000490 RID: 1168 RVA: 0x00156DA8 File Offset: 0x00154FA8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State state = this.root.Exit("StopNavigator", delegate(BuzzStates.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		});
		string name = CREATURES.STATUSITEMS.IDLE.NAME;
		string tooltip = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).ToggleTag(GameTags.Idle);
		this.idle.Enter(new StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State.Callback(this.PlayIdle)).ToggleScheduleCallback("DoBuzz", (BuzzStates.Instance smi) => (float)UnityEngine.Random.Range(3, 10), delegate(BuzzStates.Instance smi)
		{
			this.numMoves.Set(UnityEngine.Random.Range(4, 6), smi, false);
			smi.GoTo(this.buzz.move);
		});
		this.buzz.ParamTransition<int>(this.numMoves, this.idle, (BuzzStates.Instance smi, int p) => p <= 0);
		this.buzz.move.Enter(new StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State.Callback(this.MoveToNewCell)).EventTransition(GameHashes.DestinationReached, this.buzz.pause, null).EventTransition(GameHashes.NavigationFailed, this.buzz.pause, null);
		this.buzz.pause.Enter(delegate(BuzzStates.Instance smi)
		{
			this.numMoves.Set(this.numMoves.Get(smi) - 1, smi, false);
			smi.GoTo(this.buzz.move);
		});
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x00156F24 File Offset: 0x00155124
	public void MoveToNewCell(BuzzStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		BuzzStates.MoveCellQuery moveCellQuery = new BuzzStates.MoveCellQuery(component.CurrentNavType);
		moveCellQuery.allowLiquid = smi.gameObject.HasTag(GameTags.Amphibious);
		component.RunQuery(moveCellQuery);
		component.GoTo(moveCellQuery.GetResultCell(), null);
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00156F70 File Offset: 0x00155170
	public void PlayIdle(BuzzStates.Instance smi)
	{
		KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
		Navigator component2 = smi.GetComponent<Navigator>();
		NavType nav_type = component2.CurrentNavType;
		if (smi.GetComponent<Facing>().GetFacing())
		{
			nav_type = NavGrid.MirrorNavType(nav_type);
		}
		if (smi.def.customIdleAnim != null)
		{
			HashedString invalid = HashedString.Invalid;
			HashedString hashedString = smi.def.customIdleAnim(smi, ref invalid);
			if (hashedString != HashedString.Invalid)
			{
				if (invalid != HashedString.Invalid)
				{
					component.Play(invalid, KAnim.PlayMode.Once, 1f, 0f);
				}
				component.Queue(hashedString, KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		HashedString idleAnim = component2.NavGrid.GetIdleAnim(nav_type);
		component.Play(idleAnim, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0400034B RID: 843
	private StateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.IntParameter numMoves;

	// Token: 0x0400034C RID: 844
	private BuzzStates.BuzzingStates buzz;

	// Token: 0x0400034D RID: 845
	public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State idle;

	// Token: 0x0400034E RID: 846
	public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State move;

	// Token: 0x02000137 RID: 311
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400034F RID: 847
		public BuzzStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x02000138 RID: 312
		// (Invoke) Token: 0x06000498 RID: 1176
		public delegate HashedString IdleAnimCallback(BuzzStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x02000139 RID: 313
	public new class Instance : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.GameInstance
	{
		// Token: 0x0600049B RID: 1179 RVA: 0x000A7A4F File Offset: 0x000A5C4F
		public Instance(Chore<BuzzStates.Instance> chore, BuzzStates.Def def) : base(chore, def)
		{
		}
	}

	// Token: 0x0200013A RID: 314
	public class BuzzingStates : GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State
	{
		// Token: 0x04000350 RID: 848
		public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State move;

		// Token: 0x04000351 RID: 849
		public GameStateMachine<BuzzStates, BuzzStates.Instance, IStateMachineTarget, BuzzStates.Def>.State pause;
	}

	// Token: 0x0200013B RID: 315
	public class MoveCellQuery : PathFinderQuery
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x000A7A61 File Offset: 0x000A5C61
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x000A7A69 File Offset: 0x000A5C69
		public bool allowLiquid { get; set; }

		// Token: 0x0600049F RID: 1183 RVA: 0x000A7A72 File Offset: 0x000A5C72
		public MoveCellQuery(NavType navType)
		{
			this.navType = navType;
			this.maxIterations = UnityEngine.Random.Range(5, 25);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00157034 File Offset: 0x00155234
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			bool flag = this.navType != NavType.Swim;
			bool flag2 = this.navType == NavType.Swim || this.allowLiquid;
			bool flag3 = Grid.IsSubstantialLiquid(cell, 0.35f);
			if (flag3 && !flag2)
			{
				return false;
			}
			if (!flag3 && !flag)
			{
				return false;
			}
			this.targetCell = cell;
			int num = this.maxIterations - 1;
			this.maxIterations = num;
			return num <= 0;
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x000A7A9A File Offset: 0x000A5C9A
		public override int GetResultCell()
		{
			return this.targetCell;
		}

		// Token: 0x04000352 RID: 850
		private NavType navType;

		// Token: 0x04000353 RID: 851
		private int targetCell = Grid.InvalidCell;

		// Token: 0x04000354 RID: 852
		private int maxIterations;
	}
}
