using System;

// Token: 0x02001A43 RID: 6723
public class WaterTrapTrail : GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>
{
	// Token: 0x06008C35 RID: 35893 RVA: 0x0036268C File Offset: 0x0036088C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.retracted;
		base.serializable = StateMachine.SerializeType.Never;
		this.retracted.EventHandler(GameHashes.TrapArmWorkPST, delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		}).EventHandlerTransition(GameHashes.TagsChanged, this.loose, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.ShouldBeVisible)).Enter(delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		});
		this.loose.EventHandlerTransition(GameHashes.TagsChanged, this.retracted, new Func<WaterTrapTrail.Instance, object, bool>(WaterTrapTrail.OnTagsChangedWhenOnLooseState)).EventHandler(GameHashes.TrapCaptureCompleted, delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		}).Enter(delegate(WaterTrapTrail.Instance smi)
		{
			WaterTrapTrail.RefreshDepthAvailable(smi, 0f);
		});
	}

	// Token: 0x06008C36 RID: 35894 RVA: 0x00362788 File Offset: 0x00360988
	public static bool OnTagsChangedWhenOnLooseState(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance smi2 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		if (smi2 != null)
		{
			smi2.CAPTURING_SYMBOL_NAME = WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + smi.sm.depthAvailable.Get(smi).ToString();
		}
		return WaterTrapTrail.ShouldBeInvisible(smi, tagOBJ);
	}

	// Token: 0x06008C37 RID: 35895 RVA: 0x000FBA22 File Offset: 0x000F9C22
	public static bool ShouldBeInvisible(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		return !WaterTrapTrail.ShouldBeVisible(smi, tagOBJ);
	}

	// Token: 0x06008C38 RID: 35896 RVA: 0x003627D4 File Offset: 0x003609D4
	public static bool ShouldBeVisible(WaterTrapTrail.Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance smi2 = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		bool isOperational = smi.IsOperational;
		bool flag = smi.HasTag(GameTags.TrapArmed);
		bool flag2 = smi2 != null && smi2.IsInsideState(smi2.sm.operational.capture) && !smi2.IsInsideState(smi2.sm.operational.capture.idle) && !smi2.IsInsideState(smi2.sm.operational.capture.release);
		bool flag3 = smi2 != null && smi2.IsInsideState(smi2.sm.operational.unarmed) && smi2.GetWorkable().WorkInPstAnimation;
		return isOperational && (flag || flag2 || flag3);
	}

	// Token: 0x06008C39 RID: 35897 RVA: 0x0036288C File Offset: 0x00360A8C
	public static void RefreshDepthAvailable(WaterTrapTrail.Instance smi, float dt)
	{
		bool flag = WaterTrapTrail.ShouldBeVisible(smi, null);
		int num = Grid.PosToCell(smi);
		int num2 = flag ? WaterTrapGuide.GetDepthAvailable(num, smi.gameObject) : 0;
		int num3 = 4;
		if (num2 != smi.sm.depthAvailable.Get(smi))
		{
			KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
			for (int i = 1; i <= num3; i++)
			{
				component.SetSymbolVisiblity("pipe" + i.ToString(), i <= num2);
				component.SetSymbolVisiblity(WaterTrapTrail.CAPTURING_SYMBOL_OVERRIDE_NAME + i.ToString(), i == num2);
			}
			int cell = Grid.OffsetCell(num, 0, -num2);
			smi.ChangeTrapCellPosition(cell);
			WaterTrapGuide.OccupyArea(smi.gameObject, num2);
			smi.sm.depthAvailable.Set(num2, smi, false);
		}
		smi.SetRangeVisualizerOffset(new Vector2I(0, -num2));
		smi.SetRangeVisualizerVisibility(flag);
	}

	// Token: 0x04006990 RID: 27024
	private static string CAPTURING_SYMBOL_OVERRIDE_NAME = "creatureSymbol";

	// Token: 0x04006991 RID: 27025
	public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State retracted;

	// Token: 0x04006992 RID: 27026
	public GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.State loose;

	// Token: 0x04006993 RID: 27027
	private StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter depthAvailable = new StateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.IntParameter(-1);

	// Token: 0x02001A44 RID: 6724
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001A45 RID: 6725
	public new class Instance : GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>.GameInstance
	{
		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06008C3D RID: 35901 RVA: 0x000FBA4E File Offset: 0x000F9C4E
		public bool IsOperational
		{
			get
			{
				return this.operational.IsOperational;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06008C3E RID: 35902 RVA: 0x000FBA5B File Offset: 0x000F9C5B
		public Lure.Instance lureSMI
		{
			get
			{
				if (this._lureSMI == null)
				{
					this._lureSMI = base.gameObject.GetSMI<Lure.Instance>();
				}
				return this._lureSMI;
			}
		}

		// Token: 0x06008C3F RID: 35903 RVA: 0x000FBA7C File Offset: 0x000F9C7C
		public Instance(IStateMachineTarget master, WaterTrapTrail.Def def) : base(master, def)
		{
		}

		// Token: 0x06008C40 RID: 35904 RVA: 0x000FBA86 File Offset: 0x000F9C86
		public override void StartSM()
		{
			base.StartSM();
			this.RegisterListenersToCellChanges();
		}

		// Token: 0x06008C41 RID: 35905 RVA: 0x0036297C File Offset: 0x00360B7C
		private void RegisterListenersToCellChanges()
		{
			int widthInCells = base.GetComponent<BuildingComplete>().Def.WidthInCells;
			CellOffset[] array = new CellOffset[widthInCells * 4];
			for (int i = 0; i < 4; i++)
			{
				int y = -(i + 1);
				for (int j = 0; j < widthInCells; j++)
				{
					array[i * widthInCells + j] = new CellOffset(j, y);
				}
			}
			Extents extents = new Extents(Grid.PosToCell(base.transform.GetPosition()), array);
			this.partitionerEntry_solids = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnLowerCellChanged));
			this.partitionerEntry_buildings = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnLowerCellChanged));
		}

		// Token: 0x06008C42 RID: 35906 RVA: 0x000FBA94 File Offset: 0x000F9C94
		private void UnregisterListenersToCellChanges()
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
		}

		// Token: 0x06008C43 RID: 35907 RVA: 0x000FBAB6 File Offset: 0x000F9CB6
		private void OnLowerCellChanged(object o)
		{
			WaterTrapTrail.RefreshDepthAvailable(base.smi, 0f);
		}

		// Token: 0x06008C44 RID: 35908 RVA: 0x000FBAC8 File Offset: 0x000F9CC8
		protected override void OnCleanUp()
		{
			this.UnregisterListenersToCellChanges();
			base.OnCleanUp();
		}

		// Token: 0x06008C45 RID: 35909 RVA: 0x000FBAD6 File Offset: 0x000F9CD6
		public void SetRangeVisualizerVisibility(bool visible)
		{
			this.rangeVisualizer.RangeMax.x = (visible ? 0 : -1);
		}

		// Token: 0x06008C46 RID: 35910 RVA: 0x000FBAEF File Offset: 0x000F9CEF
		public void SetRangeVisualizerOffset(Vector2I offset)
		{
			this.rangeVisualizer.OriginOffset = offset;
		}

		// Token: 0x06008C47 RID: 35911 RVA: 0x000FBAFD File Offset: 0x000F9CFD
		public void ChangeTrapCellPosition(int cell)
		{
			if (this.lureSMI != null)
			{
				this.lureSMI.ChangeLureCellPosition(cell);
			}
			base.gameObject.GetComponent<TrapTrigger>().SetTriggerCell(cell);
		}

		// Token: 0x04006994 RID: 27028
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04006995 RID: 27029
		[MyCmpGet]
		private RangeVisualizer rangeVisualizer;

		// Token: 0x04006996 RID: 27030
		private HandleVector<int>.Handle partitionerEntry_buildings;

		// Token: 0x04006997 RID: 27031
		private HandleVector<int>.Handle partitionerEntry_solids;

		// Token: 0x04006998 RID: 27032
		private Lure.Instance _lureSMI;
	}
}
