using System;

// Token: 0x02001D81 RID: 7553
public class Lure : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>
{
	// Token: 0x06009DD2 RID: 40402 RVA: 0x003C8968 File Offset: 0x003C6B68
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.off;
		this.off.DoNothing();
		this.on.Enter(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.AddToScenePartitioner)).Exit(new StateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State.Callback(this.RemoveFromScenePartitioner));
	}

	// Token: 0x06009DD3 RID: 40403 RVA: 0x003C89BC File Offset: 0x003C6BBC
	private void AddToScenePartitioner(Lure.Instance smi)
	{
		Extents extents = new Extents(smi.cell, smi.def.radius);
		smi.partitionerEntry = GameScenePartitioner.Instance.Add(this.name, smi, extents, GameScenePartitioner.Instance.lure, null);
	}

	// Token: 0x06009DD4 RID: 40404 RVA: 0x00106B00 File Offset: 0x00104D00
	private void RemoveFromScenePartitioner(Lure.Instance smi)
	{
		GameScenePartitioner.Instance.Free(ref smi.partitionerEntry);
	}

	// Token: 0x04007BAC RID: 31660
	public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State off;

	// Token: 0x04007BAD RID: 31661
	public GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.State on;

	// Token: 0x02001D82 RID: 7554
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04007BAE RID: 31662
		public CellOffset[] defaultLurePoints = new CellOffset[1];

		// Token: 0x04007BAF RID: 31663
		public int radius = 50;

		// Token: 0x04007BB0 RID: 31664
		public Tag[] initialLures;
	}

	// Token: 0x02001D83 RID: 7555
	public new class Instance : GameStateMachine<Lure, Lure.Instance, IStateMachineTarget, Lure.Def>.GameInstance
	{
		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06009DD7 RID: 40407 RVA: 0x00106B36 File Offset: 0x00104D36
		public int cell
		{
			get
			{
				if (this._cell == -1)
				{
					this._cell = Grid.PosToCell(base.transform.GetPosition());
				}
				return this._cell;
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06009DD8 RID: 40408 RVA: 0x00106B5D File Offset: 0x00104D5D
		// (set) Token: 0x06009DD9 RID: 40409 RVA: 0x00106B79 File Offset: 0x00104D79
		public CellOffset[] LurePoints
		{
			get
			{
				if (this._lurePoints == null)
				{
					return base.def.defaultLurePoints;
				}
				return this._lurePoints;
			}
			set
			{
				this._lurePoints = value;
			}
		}

		// Token: 0x06009DDA RID: 40410 RVA: 0x00106B82 File Offset: 0x00104D82
		public Instance(IStateMachineTarget master, Lure.Def def) : base(master, def)
		{
		}

		// Token: 0x06009DDB RID: 40411 RVA: 0x00106B93 File Offset: 0x00104D93
		public override void StartSM()
		{
			base.StartSM();
			if (base.def.initialLures != null)
			{
				this.SetActiveLures(base.def.initialLures);
			}
		}

		// Token: 0x06009DDC RID: 40412 RVA: 0x003C8A04 File Offset: 0x003C6C04
		public void ChangeLureCellPosition(int newCell)
		{
			bool flag = base.IsInsideState(base.sm.on);
			if (flag)
			{
				this.GoTo(base.sm.off);
			}
			this.LurePoints = new CellOffset[]
			{
				Grid.GetOffset(Grid.PosToCell(base.smi.transform.GetPosition()), newCell)
			};
			this._cell = newCell;
			if (flag)
			{
				this.GoTo(base.sm.on);
			}
		}

		// Token: 0x06009DDD RID: 40413 RVA: 0x00106BB9 File Offset: 0x00104DB9
		public void SetActiveLures(Tag[] lures)
		{
			this.lures = lures;
			if (lures == null || lures.Length == 0)
			{
				this.GoTo(base.sm.off);
				return;
			}
			this.GoTo(base.sm.on);
		}

		// Token: 0x06009DDE RID: 40414 RVA: 0x00106BEC File Offset: 0x00104DEC
		public bool IsActive()
		{
			return this.GetCurrentState() == base.sm.on;
		}

		// Token: 0x06009DDF RID: 40415 RVA: 0x003C8A80 File Offset: 0x003C6C80
		public bool HasAnyLure(Tag[] creature_lures)
		{
			if (this.lures == null || creature_lures == null)
			{
				return false;
			}
			foreach (Tag a in creature_lures)
			{
				foreach (Tag b in this.lures)
				{
					if (a == b)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04007BB1 RID: 31665
		private int _cell = -1;

		// Token: 0x04007BB2 RID: 31666
		private Tag[] lures;

		// Token: 0x04007BB3 RID: 31667
		public HandleVector<int>.Handle partitionerEntry;

		// Token: 0x04007BB4 RID: 31668
		private CellOffset[] _lurePoints;
	}
}
