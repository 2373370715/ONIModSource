using System;
using STRINGS;
using UnityEngine;

// Token: 0x020001C8 RID: 456
public class IdleStates : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>
{
	// Token: 0x06000639 RID: 1593 RVA: 0x0015B450 File Offset: 0x00159650
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State state = this.root.Exit("StopNavigator", delegate(IdleStates.Instance smi)
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
		this.loop.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(this.PlayIdle)).ToggleScheduleCallback("IdleMove", (IdleStates.Instance smi) => (float)UnityEngine.Random.Range(3, 10), delegate(IdleStates.Instance smi)
		{
			smi.GoTo(this.move);
		});
		this.move.Enter(new StateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State.Callback(this.MoveToNewCell)).EventTransition(GameHashes.DestinationReached, this.loop, null).EventTransition(GameHashes.NavigationFailed, this.loop, null);
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0015B568 File Offset: 0x00159768
	public void MoveToNewCell(IdleStates.Instance smi)
	{
		if (smi.HasTag(GameTags.StationaryIdling))
		{
			smi.GoTo(smi.sm.loop);
			return;
		}
		Navigator component = smi.GetComponent<Navigator>();
		IdleStates.MoveCellQuery moveCellQuery = new IdleStates.MoveCellQuery(component.CurrentNavType);
		moveCellQuery.allowLiquid = smi.gameObject.HasTag(GameTags.Amphibious);
		moveCellQuery.submerged = smi.gameObject.HasTag(GameTags.Creatures.Submerged);
		int num = Grid.PosToCell(component);
		if (component.CurrentNavType == NavType.Hover && CellSelectionObject.IsExposedToSpace(num))
		{
			int num2 = 0;
			int cell = num;
			for (int i = 0; i < 10; i++)
			{
				cell = Grid.CellBelow(cell);
				if (!Grid.IsValidCell(cell) || Grid.IsSolidCell(cell) || !CellSelectionObject.IsExposedToSpace(cell))
				{
					break;
				}
				num2++;
			}
			moveCellQuery.lowerCellBias = (num2 == 10);
		}
		component.RunQuery(moveCellQuery);
		component.GoTo(moveCellQuery.GetResultCell(), null);
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0015B64C File Offset: 0x0015984C
	public void PlayIdle(IdleStates.Instance smi)
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

	// Token: 0x04000483 RID: 1155
	private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State loop;

	// Token: 0x04000484 RID: 1156
	private GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.State move;

	// Token: 0x020001C9 RID: 457
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04000485 RID: 1157
		public IdleStates.Def.IdleAnimCallback customIdleAnim;

		// Token: 0x020001CA RID: 458
		// (Invoke) Token: 0x06000640 RID: 1600
		public delegate HashedString IdleAnimCallback(IdleStates.Instance smi, ref HashedString pre_anim);
	}

	// Token: 0x020001CB RID: 459
	public new class Instance : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>.GameInstance
	{
		// Token: 0x06000643 RID: 1603 RVA: 0x000A8D26 File Offset: 0x000A6F26
		public Instance(Chore<IdleStates.Instance> chore, IdleStates.Def def) : base(chore, def)
		{
		}
	}

	// Token: 0x020001CC RID: 460
	public class MoveCellQuery : PathFinderQuery
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x000A8D30 File Offset: 0x000A6F30
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x000A8D38 File Offset: 0x000A6F38
		public bool allowLiquid { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x000A8D41 File Offset: 0x000A6F41
		// (set) Token: 0x06000647 RID: 1607 RVA: 0x000A8D49 File Offset: 0x000A6F49
		public bool submerged { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x000A8D52 File Offset: 0x000A6F52
		// (set) Token: 0x06000649 RID: 1609 RVA: 0x000A8D5A File Offset: 0x000A6F5A
		public bool lowerCellBias { get; set; }

		// Token: 0x0600064A RID: 1610 RVA: 0x000A8D63 File Offset: 0x000A6F63
		public MoveCellQuery(NavType navType)
		{
			this.navType = navType;
			this.maxIterations = UnityEngine.Random.Range(5, 25);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0015B710 File Offset: 0x00159910
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			GameObject gameObject;
			Grid.ObjectLayers[1].TryGetValue(cell, out gameObject);
			if (gameObject != null)
			{
				BuildingUnderConstruction component = gameObject.GetComponent<BuildingUnderConstruction>();
				if (component != null && (component.Def.IsFoundation || component.HasTag(GameTags.NoCreatureIdling)))
				{
					return false;
				}
			}
			bool flag = this.submerged || Grid.IsNavigatableLiquid(cell);
			bool flag2 = this.navType != NavType.Swim;
			bool flag3 = this.navType == NavType.Swim || this.allowLiquid;
			if (flag && !flag3)
			{
				return false;
			}
			if (!flag && !flag2)
			{
				return false;
			}
			if (this.targetCell == Grid.InvalidCell || !this.lowerCellBias)
			{
				this.targetCell = cell;
			}
			else
			{
				int num = Grid.CellRow(this.targetCell);
				if (Grid.CellRow(cell) < num)
				{
					this.targetCell = cell;
				}
			}
			int num2 = this.maxIterations - 1;
			this.maxIterations = num2;
			return num2 <= 0;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x000A8D8B File Offset: 0x000A6F8B
		public override int GetResultCell()
		{
			return this.targetCell;
		}

		// Token: 0x04000486 RID: 1158
		private NavType navType;

		// Token: 0x04000487 RID: 1159
		private int targetCell = Grid.InvalidCell;

		// Token: 0x04000488 RID: 1160
		private int maxIterations;
	}
}
