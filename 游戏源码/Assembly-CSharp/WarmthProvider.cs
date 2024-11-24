using System;
using System.Collections.Generic;

public class WarmthProvider : GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>
{
	public class Def : BaseDef
	{
		public Vector2I OriginOffset;

		public Vector2I RangeMin;

		public Vector2I RangeMax;

		public Func<int, bool> blockingCellCallback = Grid.IsSolidCell;
	}

	public new class Instance : GameInstance
	{
		public int WorldID;

		private int[] cellsInRange;

		private HandleVector<int>.Handle[] partitionEntries;

		public Vector2I range_min;

		public Vector2I range_max;

		public Vector2I origin;

		public bool IsWarming => IsInsideState(base.sm.on);

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			EntityCellVisualizer component = GetComponent<EntityCellVisualizer>();
			if (component != null)
			{
				component.AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
			}
			WorldID = base.gameObject.GetMyWorldId();
			SetupRange();
			CreateCellListeners();
			base.StartSM();
		}

		private void SetupRange()
		{
			Vector2I vector2I = Grid.PosToXY(base.transform.GetPosition());
			Vector2I vector2I2 = base.def.OriginOffset;
			range_min = base.def.RangeMin;
			range_max = base.def.RangeMax;
			if (base.gameObject.TryGetComponent<Rotatable>(out var component))
			{
				vector2I2 = component.GetRotatedOffset(vector2I2);
				Vector2I rotatedOffset = component.GetRotatedOffset(range_min);
				Vector2I rotatedOffset2 = component.GetRotatedOffset(range_max);
				range_min.x = ((rotatedOffset.x < rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				range_min.y = ((rotatedOffset.y < rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
				range_max.x = ((rotatedOffset.x > rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				range_max.y = ((rotatedOffset.y > rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
			}
			origin = vector2I + vector2I2;
		}

		public bool ContainsCell(int cell)
		{
			if (cellsInRange == null)
			{
				return false;
			}
			for (int i = 0; i < cellsInRange.Length; i++)
			{
				if (cellsInRange[i] == cell)
				{
					return true;
				}
			}
			return false;
		}

		private void UnmarkAllCellsInRange()
		{
			if (cellsInRange != null)
			{
				for (int i = 0; i < cellsInRange.Length; i++)
				{
					int key = cellsInRange[i];
					if (WarmCells.ContainsKey(key))
					{
						WarmCells[key]--;
					}
				}
			}
			cellsInRange = null;
		}

		private void UpdateCellsInRange()
		{
			UnmarkAllCellsInRange();
			Grid.PosToCell(this);
			List<int> list = new List<int>();
			for (int i = 0; i <= range_max.y - range_min.y; i++)
			{
				int y = origin.y + range_min.y + i;
				for (int j = 0; j <= range_max.x - range_min.x; j++)
				{
					int num = Grid.XYToCell(origin.x + range_min.x + j, y);
					if (Grid.IsValidCellInWorld(num, WorldID) && IsCellVisible(num))
					{
						list.Add(num);
						if (!WarmCells.ContainsKey(num))
						{
							WarmCells.Add(num, 0);
						}
						WarmCells[num]++;
					}
				}
			}
			cellsInRange = list.ToArray();
		}

		public void AddWarmCells()
		{
			UpdateCellsInRange();
		}

		public void RemoveWarmCells()
		{
			UnmarkAllCellsInRange();
		}

		protected override void OnCleanUp()
		{
			RemoveWarmCells();
			ClearCellListeners();
			base.OnCleanUp();
		}

		public bool IsCellVisible(int cell)
		{
			Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(this));
			Vector2I vector2I2 = Grid.CellToXY(cell);
			return Grid.TestLineOfSight(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y, base.def.blockingCellCallback);
		}

		public void OnSolidCellChanged(object obj)
		{
			if (IsWarming)
			{
				UpdateCellsInRange();
			}
		}

		private void CreateCellListeners()
		{
			Grid.PosToCell(this);
			List<HandleVector<int>.Handle> list = new List<HandleVector<int>.Handle>();
			for (int i = 0; i <= range_max.y - range_min.y; i++)
			{
				int y = origin.y + range_min.y + i;
				for (int j = 0; j <= range_max.x - range_min.x; j++)
				{
					int cell = Grid.XYToCell(origin.x + range_min.x + j, y);
					if (Grid.IsValidCellInWorld(cell, WorldID))
					{
						list.Add(GameScenePartitioner.Instance.Add("WarmthProvider Visibility", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, OnSolidCellChanged));
					}
				}
			}
			partitionEntries = list.ToArray();
		}

		private void ClearCellListeners()
		{
			if (partitionEntries != null)
			{
				for (int i = 0; i < partitionEntries.Length; i++)
				{
					HandleVector<int>.Handle handle = partitionEntries[i];
					GameScenePartitioner.Instance.Free(ref handle);
				}
			}
		}
	}

	public static Dictionary<int, byte> WarmCells = new Dictionary<int, byte>();

	public State off;

	public State on;

	public static bool IsWarmCell(int cell)
	{
		if (WarmCells.ContainsKey(cell))
		{
			return WarmCells[cell] > 0;
		}
		return false;
	}

	public static int GetWarmthValue(int cell)
	{
		if (!WarmCells.ContainsKey(cell))
		{
			return -1;
		}
		return WarmCells[cell];
	}

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = off;
		off.EventTransition(GameHashes.ActiveChanged, on, (Instance smi) => smi.GetComponent<Operational>().IsActive).Enter(RemoveWarmCells);
		on.EventTransition(GameHashes.ActiveChanged, off, (Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Operational, off, on_remove: true).Enter(AddWarmCells);
	}

	private static void AddWarmCells(Instance smi)
	{
		smi.AddWarmCells();
	}

	private static void RemoveWarmCells(Instance smi)
	{
		smi.RemoveWarmCells();
	}
}
