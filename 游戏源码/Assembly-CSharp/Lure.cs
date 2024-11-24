public class Lure : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>
{
	public class Def : BaseDef
	{
		public CellOffset[] defaultLurePoints = new CellOffset[1];

		public int radius = 50;

		public Tag[] initialLures;
	}

	public new class Instance : GameInstance
	{
		private int _cell = -1;

		private Tag[] lures;

		public HandleVector<int>.Handle partitionerEntry;

		private CellOffset[] _lurePoints;

		public int cell
		{
			get
			{
				if (_cell == -1)
				{
					_cell = Grid.PosToCell(base.transform.GetPosition());
				}
				return _cell;
			}
		}

		public CellOffset[] LurePoints
		{
			get
			{
				if (_lurePoints == null)
				{
					return base.def.defaultLurePoints;
				}
				return _lurePoints;
			}
			set
			{
				_lurePoints = value;
			}
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			base.StartSM();
			if (base.def.initialLures != null)
			{
				SetActiveLures(base.def.initialLures);
			}
		}

		public void ChangeLureCellPosition(int newCell)
		{
			bool flag = IsInsideState(base.sm.on);
			if (flag)
			{
				GoTo(base.sm.off);
			}
			LurePoints = new CellOffset[1] { Grid.GetOffset(Grid.PosToCell(base.smi.transform.GetPosition()), newCell) };
			_cell = newCell;
			if (flag)
			{
				GoTo(base.sm.on);
			}
		}

		public void SetActiveLures(Tag[] lures)
		{
			this.lures = lures;
			if (lures == null || lures.Length == 0)
			{
				GoTo(base.sm.off);
			}
			else
			{
				GoTo(base.sm.on);
			}
		}

		public bool IsActive()
		{
			return GetCurrentState() == base.sm.on;
		}

		public bool HasAnyLure(Tag[] creature_lures)
		{
			if (lures == null || creature_lures == null)
			{
				return false;
			}
			foreach (Tag tag in creature_lures)
			{
				Tag[] array = lures;
				foreach (Tag tag2 in array)
				{
					if (tag == tag2)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public State off;

	public State on;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = off;
		off.DoNothing();
		on.Enter(AddToScenePartitioner).Exit(RemoveFromScenePartitioner);
	}

	private void AddToScenePartitioner(Instance smi)
	{
		Extents extents = new Extents(smi.cell, smi.def.radius);
		smi.partitionerEntry = GameScenePartitioner.Instance.Add(name, smi, extents, GameScenePartitioner.Instance.lure, null);
	}

	private void RemoveFromScenePartitioner(Instance smi)
	{
		GameScenePartitioner.Instance.Free(ref smi.partitionerEntry);
	}
}
