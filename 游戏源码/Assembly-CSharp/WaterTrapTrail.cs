public class WaterTrapTrail : GameStateMachine<WaterTrapTrail, WaterTrapTrail.Instance, IStateMachineTarget, WaterTrapTrail.Def>
{
	public class Def : BaseDef
	{
	}

	public new class Instance : GameInstance
	{
		[MyCmpGet]
		private Operational operational;

		[MyCmpGet]
		private RangeVisualizer rangeVisualizer;

		private HandleVector<int>.Handle partitionerEntry_buildings;

		private HandleVector<int>.Handle partitionerEntry_solids;

		private Lure.Instance _lureSMI;

		public bool IsOperational => operational.IsOperational;

		public Lure.Instance lureSMI
		{
			get
			{
				if (_lureSMI == null)
				{
					_lureSMI = base.gameObject.GetSMI<Lure.Instance>();
				}
				return _lureSMI;
			}
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
		}

		public override void StartSM()
		{
			base.StartSM();
			RegisterListenersToCellChanges();
		}

		private void RegisterListenersToCellChanges()
		{
			int widthInCells = GetComponent<BuildingComplete>().Def.WidthInCells;
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
			partitionerEntry_solids = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, OnLowerCellChanged);
			partitionerEntry_buildings = GameScenePartitioner.Instance.Add("WaterTrapTrail", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], OnLowerCellChanged);
		}

		private void UnregisterListenersToCellChanges()
		{
			GameScenePartitioner.Instance.Free(ref partitionerEntry_solids);
			GameScenePartitioner.Instance.Free(ref partitionerEntry_buildings);
		}

		private void OnLowerCellChanged(object o)
		{
			RefreshDepthAvailable(base.smi, 0f);
		}

		protected override void OnCleanUp()
		{
			UnregisterListenersToCellChanges();
			base.OnCleanUp();
		}

		public void SetRangeVisualizerVisibility(bool visible)
		{
			rangeVisualizer.RangeMax.x = ((!visible) ? (-1) : 0);
		}

		public void SetRangeVisualizerOffset(Vector2I offset)
		{
			rangeVisualizer.OriginOffset = offset;
		}

		public void ChangeTrapCellPosition(int cell)
		{
			if (lureSMI != null)
			{
				lureSMI.ChangeLureCellPosition(cell);
			}
			base.gameObject.GetComponent<TrapTrigger>().SetTriggerCell(cell);
		}
	}

	private static string CAPTURING_SYMBOL_OVERRIDE_NAME = "creatureSymbol";

	public State retracted;

	public State loose;

	private IntParameter depthAvailable = new IntParameter(-1);

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = retracted;
		base.serializable = SerializeType.Never;
		retracted.EventHandler(GameHashes.TrapArmWorkPST, delegate(Instance smi)
		{
			RefreshDepthAvailable(smi, 0f);
		}).EventHandlerTransition(GameHashes.TagsChanged, loose, ShouldBeVisible).Enter(delegate(Instance smi)
		{
			RefreshDepthAvailable(smi, 0f);
		});
		loose.EventHandlerTransition(GameHashes.TagsChanged, retracted, OnTagsChangedWhenOnLooseState).EventHandler(GameHashes.TrapCaptureCompleted, delegate(Instance smi)
		{
			RefreshDepthAvailable(smi, 0f);
		}).Enter(delegate(Instance smi)
		{
			RefreshDepthAvailable(smi, 0f);
		});
	}

	public static bool OnTagsChangedWhenOnLooseState(Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance sMI = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		if (sMI != null)
		{
			sMI.CAPTURING_SYMBOL_NAME = CAPTURING_SYMBOL_OVERRIDE_NAME + smi.sm.depthAvailable.Get(smi);
		}
		return ShouldBeInvisible(smi, tagOBJ);
	}

	public static bool ShouldBeInvisible(Instance smi, object tagOBJ)
	{
		return !ShouldBeVisible(smi, tagOBJ);
	}

	public static bool ShouldBeVisible(Instance smi, object tagOBJ)
	{
		ReusableTrap.Instance sMI = smi.gameObject.GetSMI<ReusableTrap.Instance>();
		bool isOperational = smi.IsOperational;
		bool flag = smi.HasTag(GameTags.TrapArmed);
		bool flag2 = sMI != null && sMI.IsInsideState(sMI.sm.operational.capture) && !sMI.IsInsideState(sMI.sm.operational.capture.idle) && !sMI.IsInsideState(sMI.sm.operational.capture.release);
		bool flag3 = sMI != null && sMI.IsInsideState(sMI.sm.operational.unarmed) && sMI.GetWorkable().WorkInPstAnimation;
		if (isOperational)
		{
			return flag || flag2 || flag3;
		}
		return false;
	}

	public static void RefreshDepthAvailable(Instance smi, float dt)
	{
		bool flag = ShouldBeVisible(smi, null);
		int num = Grid.PosToCell(smi);
		int num2 = (flag ? WaterTrapGuide.GetDepthAvailable(num, smi.gameObject) : 0);
		int num3 = 4;
		if (num2 != smi.sm.depthAvailable.Get(smi))
		{
			KAnimControllerBase component = smi.GetComponent<KAnimControllerBase>();
			for (int i = 1; i <= num3; i++)
			{
				component.SetSymbolVisiblity("pipe" + i, i <= num2);
				component.SetSymbolVisiblity(CAPTURING_SYMBOL_OVERRIDE_NAME + i, i == num2);
			}
			int cell = Grid.OffsetCell(num, 0, -num2);
			smi.ChangeTrapCellPosition(cell);
			WaterTrapGuide.OccupyArea(smi.gameObject, num2);
			smi.sm.depthAvailable.Set(num2, smi);
		}
		smi.SetRangeVisualizerOffset(new Vector2I(0, -num2));
		smi.SetRangeVisualizerVisibility(flag);
	}
}
