using System;
using System.Collections.Generic;

// Token: 0x02000B6C RID: 2924
public class WarmthProvider : GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>
{
	// Token: 0x06003788 RID: 14216 RVA: 0x000C4075 File Offset: 0x000C2275
	public static bool IsWarmCell(int cell)
	{
		return WarmthProvider.WarmCells.ContainsKey(cell) && WarmthProvider.WarmCells[cell] > 0;
	}

	// Token: 0x06003789 RID: 14217 RVA: 0x000C4094 File Offset: 0x000C2294
	public static int GetWarmthValue(int cell)
	{
		if (!WarmthProvider.WarmCells.ContainsKey(cell))
		{
			return -1;
		}
		return (int)WarmthProvider.WarmCells[cell];
	}

	// Token: 0x0600378A RID: 14218 RVA: 0x00218024 File Offset: 0x00216224
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.off;
		this.off.EventTransition(GameHashes.ActiveChanged, this.on, (WarmthProvider.Instance smi) => smi.GetComponent<Operational>().IsActive).Enter(new StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State.Callback(WarmthProvider.RemoveWarmCells));
		this.on.EventTransition(GameHashes.ActiveChanged, this.off, (WarmthProvider.Instance smi) => !smi.GetComponent<Operational>().IsActive).TagTransition(GameTags.Operational, this.off, true).Enter(new StateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State.Callback(WarmthProvider.AddWarmCells));
	}

	// Token: 0x0600378B RID: 14219 RVA: 0x000C40B0 File Offset: 0x000C22B0
	private static void AddWarmCells(WarmthProvider.Instance smi)
	{
		smi.AddWarmCells();
	}

	// Token: 0x0600378C RID: 14220 RVA: 0x000C40B8 File Offset: 0x000C22B8
	private static void RemoveWarmCells(WarmthProvider.Instance smi)
	{
		smi.RemoveWarmCells();
	}

	// Token: 0x040025BE RID: 9662
	public static Dictionary<int, byte> WarmCells = new Dictionary<int, byte>();

	// Token: 0x040025BF RID: 9663
	public GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State off;

	// Token: 0x040025C0 RID: 9664
	public GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.State on;

	// Token: 0x02000B6D RID: 2925
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040025C1 RID: 9665
		public Vector2I OriginOffset;

		// Token: 0x040025C2 RID: 9666
		public Vector2I RangeMin;

		// Token: 0x040025C3 RID: 9667
		public Vector2I RangeMax;

		// Token: 0x040025C4 RID: 9668
		public Func<int, bool> blockingCellCallback = new Func<int, bool>(Grid.IsSolidCell);
	}

	// Token: 0x02000B6E RID: 2926
	public new class Instance : GameStateMachine<WarmthProvider, WarmthProvider.Instance, IStateMachineTarget, WarmthProvider.Def>.GameInstance
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06003790 RID: 14224 RVA: 0x000C40EE File Offset: 0x000C22EE
		public bool IsWarming
		{
			get
			{
				return base.IsInsideState(base.sm.on);
			}
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x000C4101 File Offset: 0x000C2301
		public Instance(IStateMachineTarget master, WarmthProvider.Def def) : base(master, def)
		{
		}

		// Token: 0x06003792 RID: 14226 RVA: 0x002180E0 File Offset: 0x002162E0
		public override void StartSM()
		{
			EntityCellVisualizer component = base.GetComponent<EntityCellVisualizer>();
			if (component != null)
			{
				component.AddPort(EntityCellVisualizer.Ports.HeatSource, default(CellOffset));
			}
			this.WorldID = base.gameObject.GetMyWorldId();
			this.SetupRange();
			this.CreateCellListeners();
			base.StartSM();
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x00218134 File Offset: 0x00216334
		private void SetupRange()
		{
			Vector2I u = Grid.PosToXY(base.transform.GetPosition());
			Vector2I vector2I = base.def.OriginOffset;
			this.range_min = base.def.RangeMin;
			this.range_max = base.def.RangeMax;
			Rotatable rotatable;
			if (base.gameObject.TryGetComponent<Rotatable>(out rotatable))
			{
				vector2I = rotatable.GetRotatedOffset(vector2I);
				Vector2I rotatedOffset = rotatable.GetRotatedOffset(this.range_min);
				Vector2I rotatedOffset2 = rotatable.GetRotatedOffset(this.range_max);
				this.range_min.x = ((rotatedOffset.x < rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				this.range_min.y = ((rotatedOffset.y < rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
				this.range_max.x = ((rotatedOffset.x > rotatedOffset2.x) ? rotatedOffset.x : rotatedOffset2.x);
				this.range_max.y = ((rotatedOffset.y > rotatedOffset2.y) ? rotatedOffset.y : rotatedOffset2.y);
			}
			this.origin = u + vector2I;
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x00218268 File Offset: 0x00216468
		public bool ContainsCell(int cell)
		{
			if (this.cellsInRange == null)
			{
				return false;
			}
			for (int i = 0; i < this.cellsInRange.Length; i++)
			{
				if (this.cellsInRange[i] == cell)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x002182A0 File Offset: 0x002164A0
		private void UnmarkAllCellsInRange()
		{
			if (this.cellsInRange != null)
			{
				for (int i = 0; i < this.cellsInRange.Length; i++)
				{
					int num = this.cellsInRange[i];
					if (WarmthProvider.WarmCells.ContainsKey(num))
					{
						Dictionary<int, byte> warmCells = WarmthProvider.WarmCells;
						int key = num;
						byte b = warmCells[key];
						warmCells[key] = b - 1;
					}
				}
			}
			this.cellsInRange = null;
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x00218300 File Offset: 0x00216500
		private void UpdateCellsInRange()
		{
			this.UnmarkAllCellsInRange();
			Grid.PosToCell(this);
			List<int> list = new List<int>();
			for (int i = 0; i <= this.range_max.y - this.range_min.y; i++)
			{
				int y = this.origin.y + this.range_min.y + i;
				for (int j = 0; j <= this.range_max.x - this.range_min.x; j++)
				{
					int num = Grid.XYToCell(this.origin.x + this.range_min.x + j, y);
					if (Grid.IsValidCellInWorld(num, this.WorldID) && this.IsCellVisible(num))
					{
						list.Add(num);
						if (!WarmthProvider.WarmCells.ContainsKey(num))
						{
							WarmthProvider.WarmCells.Add(num, 0);
						}
						Dictionary<int, byte> warmCells = WarmthProvider.WarmCells;
						int key = num;
						byte b = warmCells[key];
						warmCells[key] = b + 1;
					}
				}
			}
			this.cellsInRange = list.ToArray();
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x000C410B File Offset: 0x000C230B
		public void AddWarmCells()
		{
			this.UpdateCellsInRange();
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x000C4113 File Offset: 0x000C2313
		public void RemoveWarmCells()
		{
			this.UnmarkAllCellsInRange();
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x000C411B File Offset: 0x000C231B
		protected override void OnCleanUp()
		{
			this.RemoveWarmCells();
			this.ClearCellListeners();
			base.OnCleanUp();
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x00218414 File Offset: 0x00216614
		public bool IsCellVisible(int cell)
		{
			Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(this));
			Vector2I vector2I2 = Grid.CellToXY(cell);
			return Grid.TestLineOfSight(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y, base.def.blockingCellCallback, false, false);
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x000C412F File Offset: 0x000C232F
		public void OnSolidCellChanged(object obj)
		{
			if (this.IsWarming)
			{
				this.UpdateCellsInRange();
			}
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x00218460 File Offset: 0x00216660
		private void CreateCellListeners()
		{
			Grid.PosToCell(this);
			List<HandleVector<int>.Handle> list = new List<HandleVector<int>.Handle>();
			for (int i = 0; i <= this.range_max.y - this.range_min.y; i++)
			{
				int y = this.origin.y + this.range_min.y + i;
				for (int j = 0; j <= this.range_max.x - this.range_min.x; j++)
				{
					int cell = Grid.XYToCell(this.origin.x + this.range_min.x + j, y);
					if (Grid.IsValidCellInWorld(cell, this.WorldID))
					{
						list.Add(GameScenePartitioner.Instance.Add("WarmthProvider Visibility", base.gameObject, cell, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidCellChanged)));
					}
				}
			}
			this.partitionEntries = list.ToArray();
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x00218550 File Offset: 0x00216750
		private void ClearCellListeners()
		{
			if (this.partitionEntries != null)
			{
				for (int i = 0; i < this.partitionEntries.Length; i++)
				{
					HandleVector<int>.Handle handle = this.partitionEntries[i];
					GameScenePartitioner.Instance.Free(ref handle);
				}
			}
		}

		// Token: 0x040025C5 RID: 9669
		public int WorldID;

		// Token: 0x040025C6 RID: 9670
		private int[] cellsInRange;

		// Token: 0x040025C7 RID: 9671
		private HandleVector<int>.Handle[] partitionEntries;

		// Token: 0x040025C8 RID: 9672
		public Vector2I range_min;

		// Token: 0x040025C9 RID: 9673
		public Vector2I range_max;

		// Token: 0x040025CA RID: 9674
		public Vector2I origin;
	}
}
