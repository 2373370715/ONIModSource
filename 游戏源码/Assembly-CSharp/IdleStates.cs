using STRINGS;
using UnityEngine;

public class IdleStates : GameStateMachine<IdleStates, IdleStates.Instance, IStateMachineTarget, IdleStates.Def>
{
	public class Def : BaseDef
	{
		public delegate HashedString IdleAnimCallback(Instance smi, ref HashedString pre_anim);

		public IdleAnimCallback customIdleAnim;
	}

	public new class Instance : GameInstance
	{
		public Instance(Chore<Instance> chore, Def def)
			: base((IStateMachineTarget)chore, def)
		{
		}
	}

	public class MoveCellQuery : PathFinderQuery
	{
		private NavType navType;

		private int targetCell = Grid.InvalidCell;

		private int maxIterations;

		public bool allowLiquid { get; set; }

		public bool submerged { get; set; }

		public bool lowerCellBias { get; set; }

		public MoveCellQuery(NavType navType)
		{
			this.navType = navType;
			maxIterations = Random.Range(5, 25);
		}

		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			if (!Grid.IsValidCell(cell))
			{
				return false;
			}
			Grid.ObjectLayers[1].TryGetValue(cell, out var value);
			if (value != null)
			{
				BuildingUnderConstruction component = value.GetComponent<BuildingUnderConstruction>();
				if (component != null && (component.Def.IsFoundation || component.HasTag(GameTags.NoCreatureIdling)))
				{
					return false;
				}
			}
			bool flag = submerged || Grid.IsNavigatableLiquid(cell);
			bool flag2 = navType != NavType.Swim;
			bool flag3 = navType == NavType.Swim || allowLiquid;
			if (flag && !flag3)
			{
				return false;
			}
			if (!flag && !flag2)
			{
				return false;
			}
			if (targetCell == Grid.InvalidCell || !lowerCellBias)
			{
				targetCell = cell;
			}
			else
			{
				int num = Grid.CellRow(targetCell);
				if (Grid.CellRow(cell) < num)
				{
					targetCell = cell;
				}
			}
			return --maxIterations <= 0;
		}

		public override int GetResultCell()
		{
			return targetCell;
		}
	}

	private State loop;

	private State move;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = loop;
		root.Exit("StopNavigator", delegate(Instance smi)
		{
			smi.GetComponent<Navigator>().Stop();
		}).ToggleStatusItem(CREATURES.STATUSITEMS.IDLE.NAME, CREATURES.STATUSITEMS.IDLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).ToggleTag(GameTags.Idle);
		loop.Enter(PlayIdle).ToggleScheduleCallback("IdleMove", (Instance smi) => Random.Range(3, 10), delegate(Instance smi)
		{
			smi.GoTo(move);
		});
		move.Enter(MoveToNewCell).EventTransition(GameHashes.DestinationReached, loop).EventTransition(GameHashes.NavigationFailed, loop);
	}

	public void MoveToNewCell(Instance smi)
	{
		if (smi.HasTag(GameTags.StationaryIdling))
		{
			smi.GoTo(smi.sm.loop);
			return;
		}
		Navigator component = smi.GetComponent<Navigator>();
		MoveCellQuery moveCellQuery = new MoveCellQuery(component.CurrentNavType);
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
			moveCellQuery.lowerCellBias = num2 == 10;
		}
		component.RunQuery(moveCellQuery);
		component.GoTo(moveCellQuery.GetResultCell());
	}

	public void PlayIdle(Instance smi)
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
			HashedString pre_anim = HashedString.Invalid;
			HashedString hashedString = smi.def.customIdleAnim(smi, ref pre_anim);
			if (hashedString != HashedString.Invalid)
			{
				if (pre_anim != HashedString.Invalid)
				{
					component.Play(pre_anim);
				}
				component.Queue(hashedString, KAnim.PlayMode.Loop);
				return;
			}
		}
		HashedString idleAnim = component2.NavGrid.GetIdleAnim(nav_type);
		component.Play(idleAnim, KAnim.PlayMode.Loop);
	}
}
