using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000D3E RID: 3390
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Door")]
public class Door : Workable, ISaveLoadable, ISim200ms, INavDoor
{
	// Token: 0x0600424C RID: 16972 RVA: 0x002405BC File Offset: 0x0023E7BC
	private void OnCopySettings(object data)
	{
		Door component = ((GameObject)data).GetComponent<Door>();
		if (component != null)
		{
			this.QueueStateChange(component.requestedState);
		}
	}

	// Token: 0x0600424D RID: 16973 RVA: 0x000CADBB File Offset: 0x000C8FBB
	public Door()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x0600424E RID: 16974 RVA: 0x000CADEB File Offset: 0x000C8FEB
	public Door.ControlState CurrentState
	{
		get
		{
			return this.controlState;
		}
	}

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x0600424F RID: 16975 RVA: 0x000CADF3 File Offset: 0x000C8FF3
	public Door.ControlState RequestedState
	{
		get
		{
			return this.requestedState;
		}
	}

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06004250 RID: 16976 RVA: 0x000CADFB File Offset: 0x000C8FFB
	public bool ShouldBlockFallingSand
	{
		get
		{
			return this.rotatable.GetOrientation() != this.verticalOrientation;
		}
	}

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06004251 RID: 16977 RVA: 0x000CAE13 File Offset: 0x000C9013
	public bool isSealed
	{
		get
		{
			return this.controller != null && this.controller.sm.isSealed.Get(this.controller);
		}
	}

	// Token: 0x06004252 RID: 16978 RVA: 0x002405EC File Offset: 0x0023E7EC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = Door.OVERRIDE_ANIMS;
		this.synchronizeAnims = false;
		base.SetWorkTime(3f);
		if (!string.IsNullOrEmpty(this.doorClosingSoundEventName))
		{
			this.doorClosingSound = GlobalAssets.GetSound(this.doorClosingSoundEventName, false);
		}
		if (!string.IsNullOrEmpty(this.doorOpeningSoundEventName))
		{
			this.doorOpeningSound = GlobalAssets.GetSound(this.doorOpeningSoundEventName, false);
		}
		base.Subscribe<Door>(-905833192, Door.OnCopySettingsDelegate);
	}

	// Token: 0x06004253 RID: 16979 RVA: 0x000CAE3A File Offset: 0x000C903A
	private Door.ControlState GetNextState(Door.ControlState wantedState)
	{
		return (wantedState + 1) % Door.ControlState.NumStates;
	}

	// Token: 0x06004254 RID: 16980 RVA: 0x000CAE41 File Offset: 0x000C9041
	private static bool DisplacesGas(Door.DoorType type)
	{
		return type != Door.DoorType.Internal;
	}

	// Token: 0x06004255 RID: 16981 RVA: 0x0024066C File Offset: 0x0023E86C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.GetComponent<KPrefabID>() != null)
		{
			this.log = new LoggerFSS("Door", 35);
		}
		if (!this.allowAutoControl && this.controlState == Door.ControlState.Auto)
		{
			this.controlState = Door.ControlState.Locked;
		}
		StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
		HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
		if (Door.DisplacesGas(this.doorType))
		{
			structureTemperatures.Bypass(handle);
		}
		this.controller = new Door.Controller.Instance(this);
		this.controller.StartSM();
		if (this.doorType == Door.DoorType.Sealed && !this.hasBeenUnsealed)
		{
			this.Seal();
		}
		this.UpdateDoorSpeed(this.operational.IsOperational);
		base.Subscribe<Door>(-592767678, Door.OnOperationalChangedDelegate);
		base.Subscribe<Door>(824508782, Door.OnOperationalChangedDelegate);
		base.Subscribe<Door>(-801688580, Door.OnLogicValueChangedDelegate);
		this.requestedState = this.CurrentState;
		this.ApplyRequestedControlState(true);
		int num = (this.rotatable.GetOrientation() == Orientation.Neutral) ? (this.building.Def.WidthInCells * (this.building.Def.HeightInCells - 1)) : 0;
		int num2 = (this.rotatable.GetOrientation() == Orientation.Neutral) ? this.building.Def.WidthInCells : this.building.Def.HeightInCells;
		for (int num3 = 0; num3 != num2; num3++)
		{
			int num4 = this.building.PlacementCells[num + num3];
			Grid.FakeFloor.Add(num4);
			Pathfinding.Instance.AddDirtyNavGridCell(num4);
		}
		List<int> list = new List<int>();
		foreach (int num5 in this.building.PlacementCells)
		{
			Grid.HasDoor[num5] = true;
			if (this.rotatable.IsRotated)
			{
				list.Add(Grid.CellAbove(num5));
				list.Add(Grid.CellBelow(num5));
			}
			else
			{
				list.Add(Grid.CellLeft(num5));
				list.Add(Grid.CellRight(num5));
			}
			SimMessages.SetCellProperties(num5, 8);
			if (Door.DisplacesGas(this.doorType))
			{
				Grid.RenderedByWorld[num5] = false;
			}
		}
	}

	// Token: 0x06004256 RID: 16982 RVA: 0x002408AC File Offset: 0x0023EAAC
	protected override void OnCleanUp()
	{
		this.UpdateDoorState(true);
		List<int> list = new List<int>();
		foreach (int num in this.building.PlacementCells)
		{
			SimMessages.ClearCellProperties(num, 12);
			Grid.RenderedByWorld[num] = Grid.Element[num].substance.renderedByWorld;
			Grid.FakeFloor.Remove(num);
			if (Grid.Element[num].IsSolid)
			{
				SimMessages.ReplaceAndDisplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.DoorOpen, 0f, -1f, byte.MaxValue, 0, -1);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(num);
			if (this.rotatable.IsRotated)
			{
				list.Add(Grid.CellAbove(num));
				list.Add(Grid.CellBelow(num));
			}
			else
			{
				list.Add(Grid.CellLeft(num));
				list.Add(Grid.CellRight(num));
			}
		}
		foreach (int num2 in this.building.PlacementCells)
		{
			Grid.HasDoor[num2] = false;
			Game.Instance.SetDupePassableSolid(num2, false, Grid.Solid[num2]);
			Grid.CritterImpassable[num2] = false;
			Grid.DupeImpassable[num2] = false;
			Pathfinding.Instance.AddDirtyNavGridCell(num2);
		}
		base.OnCleanUp();
	}

	// Token: 0x06004257 RID: 16983 RVA: 0x000CAE4A File Offset: 0x000C904A
	public void Seal()
	{
		this.controller.sm.isSealed.Set(true, this.controller, false);
	}

	// Token: 0x06004258 RID: 16984 RVA: 0x000CAE6A File Offset: 0x000C906A
	public void OrderUnseal()
	{
		this.controller.GoTo(this.controller.sm.Sealed.awaiting_unlock);
	}

	// Token: 0x06004259 RID: 16985 RVA: 0x00240A08 File Offset: 0x0023EC08
	private void RefreshControlState()
	{
		switch (this.controlState)
		{
		case Door.ControlState.Auto:
			this.controller.sm.isLocked.Set(false, this.controller, false);
			break;
		case Door.ControlState.Opened:
			this.controller.sm.isLocked.Set(false, this.controller, false);
			break;
		case Door.ControlState.Locked:
			this.controller.sm.isLocked.Set(true, this.controller, false);
			break;
		}
		base.Trigger(279163026, this.controlState);
		this.SetWorldState();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.CurrentDoorControlState, this);
	}

	// Token: 0x0600425A RID: 16986 RVA: 0x00240AD8 File Offset: 0x0023ECD8
	private void OnOperationalChanged(object data)
	{
		bool isOperational = this.operational.IsOperational;
		if (isOperational != this.on)
		{
			this.UpdateDoorSpeed(isOperational);
			if (this.on && base.GetComponent<KPrefabID>().HasTag(GameTags.Transition))
			{
				this.SetActive(true);
				return;
			}
			this.SetActive(false);
		}
	}

	// Token: 0x0600425B RID: 16987 RVA: 0x00240B2C File Offset: 0x0023ED2C
	private void UpdateDoorSpeed(bool powered)
	{
		this.on = powered;
		this.UpdateAnimAndSoundParams(powered);
		float positionPercent = this.animController.GetPositionPercent();
		this.animController.Play(this.animController.CurrentAnim.hash, this.animController.PlayMode, 1f, 0f);
		this.animController.SetPositionPercent(positionPercent);
	}

	// Token: 0x0600425C RID: 16988 RVA: 0x00240B90 File Offset: 0x0023ED90
	private void UpdateAnimAndSoundParams(bool powered)
	{
		if (powered)
		{
			this.animController.PlaySpeedMultiplier = this.poweredAnimSpeed;
			if (this.doorClosingSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorClosingSound, Door.SOUND_POWERED_PARAMETER, 1f);
			}
			if (this.doorOpeningSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorOpeningSound, Door.SOUND_POWERED_PARAMETER, 1f);
				return;
			}
		}
		else
		{
			this.animController.PlaySpeedMultiplier = this.unpoweredAnimSpeed;
			if (this.doorClosingSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorClosingSound, Door.SOUND_POWERED_PARAMETER, 0f);
			}
			if (this.doorOpeningSound != null)
			{
				this.loopingSounds.UpdateFirstParameter(this.doorOpeningSound, Door.SOUND_POWERED_PARAMETER, 0f);
			}
		}
	}

	// Token: 0x0600425D RID: 16989 RVA: 0x000CAE8C File Offset: 0x000C908C
	private void SetActive(bool active)
	{
		if (this.operational.IsOperational)
		{
			this.operational.SetActive(active, false);
		}
	}

	// Token: 0x0600425E RID: 16990 RVA: 0x00240C50 File Offset: 0x0023EE50
	private void SetWorldState()
	{
		int[] placementCells = this.building.PlacementCells;
		bool is_door_open = this.IsOpen();
		this.SetPassableState(is_door_open, placementCells);
		this.SetSimState(is_door_open, placementCells);
	}

	// Token: 0x0600425F RID: 16991 RVA: 0x00240C80 File Offset: 0x0023EE80
	private void SetPassableState(bool is_door_open, IList<int> cells)
	{
		for (int i = 0; i < cells.Count; i++)
		{
			int num = cells[i];
			switch (this.doorType)
			{
			case Door.DoorType.Pressure:
			case Door.DoorType.ManualPressure:
			case Door.DoorType.Sealed:
			{
				Grid.CritterImpassable[num] = (this.controlState != Door.ControlState.Opened);
				bool solid = !is_door_open;
				bool passable = this.controlState != Door.ControlState.Locked;
				Game.Instance.SetDupePassableSolid(num, passable, solid);
				if (this.controlState == Door.ControlState.Opened)
				{
					this.doorOpenLiquidRefreshHack = true;
					this.doorOpenLiquidRefreshTime = 1f;
				}
				break;
			}
			case Door.DoorType.Internal:
				Grid.CritterImpassable[num] = (this.controlState != Door.ControlState.Opened);
				Grid.DupeImpassable[num] = (this.controlState == Door.ControlState.Locked);
				break;
			}
			Pathfinding.Instance.AddDirtyNavGridCell(num);
		}
	}

	// Token: 0x06004260 RID: 16992 RVA: 0x00240D58 File Offset: 0x0023EF58
	private void SetSimState(bool is_door_open, IList<int> cells)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		float mass = component.Mass / (float)cells.Count;
		for (int i = 0; i < cells.Count; i++)
		{
			int num = cells[i];
			Door.DoorType doorType = this.doorType;
			if (doorType <= Door.DoorType.ManualPressure || doorType == Door.DoorType.Sealed)
			{
				World.Instance.groundRenderer.MarkDirty(num);
				if (is_door_open)
				{
					SimMessages.Dig(num, Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimDoorOpened), false)).index, true);
					if (this.ShouldBlockFallingSand)
					{
						SimMessages.ClearCellProperties(num, 4);
					}
					else
					{
						SimMessages.SetCellProperties(num, 4);
					}
				}
				else
				{
					HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimDoorClosed), false));
					float temperature = component.Temperature;
					if (temperature <= 0f)
					{
						temperature = component.Temperature;
					}
					SimMessages.ReplaceAndDisplaceElement(num, component.ElementID, CellEventLogger.Instance.DoorClose, mass, temperature, byte.MaxValue, 0, handle.index);
					SimMessages.SetCellProperties(num, 4);
				}
			}
		}
	}

	// Token: 0x06004261 RID: 16993 RVA: 0x00240E78 File Offset: 0x0023F078
	private void UpdateDoorState(bool cleaningUp)
	{
		foreach (int num in this.building.PlacementCells)
		{
			if (Grid.IsValidCell(num))
			{
				Grid.Foundation[num] = !cleaningUp;
			}
		}
	}

	// Token: 0x06004262 RID: 16994 RVA: 0x00240EBC File Offset: 0x0023F0BC
	public void QueueStateChange(Door.ControlState nextState)
	{
		if (this.requestedState != nextState)
		{
			this.requestedState = nextState;
		}
		else
		{
			this.requestedState = this.controlState;
		}
		if (this.requestedState == this.controlState)
		{
			if (this.changeStateChore != null)
			{
				this.changeStateChore.Cancel("Change state");
				this.changeStateChore = null;
				base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
			}
			return;
		}
		if (DebugHandler.InstantBuildMode)
		{
			this.controlState = this.requestedState;
			this.RefreshControlState();
			this.OnOperationalChanged(null);
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
			this.Open();
			this.Close();
			return;
		}
		if (this.changeStateChore != null)
		{
			this.changeStateChore.Cancel("Change state");
		}
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, this);
		this.changeStateChore = new WorkChore<Door>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x06004263 RID: 16995 RVA: 0x00240FDC File Offset: 0x0023F1DC
	private void OnSimDoorOpened()
	{
		if (this == null || !Door.DisplacesGas(this.doorType))
		{
			return;
		}
		StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
		HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
		structureTemperatures.UnBypass(handle);
		this.do_melt_check = false;
	}

	// Token: 0x06004264 RID: 16996 RVA: 0x00241020 File Offset: 0x0023F220
	private void OnSimDoorClosed()
	{
		if (this == null || !Door.DisplacesGas(this.doorType))
		{
			return;
		}
		StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
		HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
		structureTemperatures.Bypass(handle);
		this.do_melt_check = true;
	}

	// Token: 0x06004265 RID: 16997 RVA: 0x000CAEA8 File Offset: 0x000C90A8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.changeStateChore = null;
		this.ApplyRequestedControlState(false);
	}

	// Token: 0x06004266 RID: 16998 RVA: 0x00241064 File Offset: 0x0023F264
	public void Open()
	{
		if (this.openCount == 0 && Door.DisplacesGas(this.doorType))
		{
			StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
			HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
			if (handle.IsValid() && structureTemperatures.IsBypassed(handle))
			{
				int[] placementCells = this.building.PlacementCells;
				float num = 0f;
				int num2 = 0;
				foreach (int i2 in placementCells)
				{
					if (Grid.Mass[i2] > 0f)
					{
						num2++;
						num += Grid.Temperature[i2];
					}
				}
				if (num2 > 0)
				{
					num /= (float)placementCells.Length;
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					KCrashReporter.Assert(num > 0f, "Door has calculated an invalid temperature", null);
					component.Temperature = num;
				}
			}
		}
		this.openCount++;
		Door.ControlState controlState = this.controlState;
		if (controlState > Door.ControlState.Opened)
		{
			return;
		}
		this.controller.sm.isOpen.Set(true, this.controller, false);
	}

	// Token: 0x06004267 RID: 16999 RVA: 0x00241178 File Offset: 0x0023F378
	public void Close()
	{
		this.openCount = Mathf.Max(0, this.openCount - 1);
		if (this.openCount == 0 && Door.DisplacesGas(this.doorType))
		{
			StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
			HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			if (handle.IsValid() && !structureTemperatures.IsBypassed(handle))
			{
				float temperature = structureTemperatures.GetPayload(handle).Temperature;
				component.Temperature = temperature;
			}
		}
		switch (this.controlState)
		{
		case Door.ControlState.Auto:
			if (this.openCount == 0)
			{
				this.controller.sm.isOpen.Set(false, this.controller, false);
				Game.Instance.userMenu.Refresh(base.gameObject);
			}
			break;
		case Door.ControlState.Opened:
			break;
		case Door.ControlState.Locked:
			this.controller.sm.isOpen.Set(false, this.controller, false);
			return;
		default:
			return;
		}
	}

	// Token: 0x06004268 RID: 17000 RVA: 0x000CAEBF File Offset: 0x000C90BF
	public bool IsPendingClose()
	{
		return this.controller.IsInsideState(this.controller.sm.closedelay);
	}

	// Token: 0x06004269 RID: 17001 RVA: 0x0024126C File Offset: 0x0023F46C
	public bool IsOpen()
	{
		return this.controller.IsInsideState(this.controller.sm.open) || this.controller.IsInsideState(this.controller.sm.closedelay) || this.controller.IsInsideState(this.controller.sm.closeblocked);
	}

	// Token: 0x0600426A RID: 17002 RVA: 0x002412D0 File Offset: 0x0023F4D0
	private void ApplyRequestedControlState(bool force = false)
	{
		if (this.requestedState == this.controlState && !force)
		{
			return;
		}
		this.controlState = this.requestedState;
		this.RefreshControlState();
		this.OnOperationalChanged(null);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ChangeDoorControlState, false);
		base.Trigger(1734268753, this);
		if (!force)
		{
			this.Open();
			this.Close();
		}
	}

	// Token: 0x0600426B RID: 17003 RVA: 0x00241340 File Offset: 0x0023F540
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != Door.OPEN_CLOSE_PORT_ID)
		{
			return;
		}
		int newValue = logicValueChanged.newValue;
		if (this.changeStateChore != null)
		{
			this.changeStateChore.Cancel("Change state");
			this.changeStateChore = null;
		}
		this.requestedState = (LogicCircuitNetwork.IsBitActive(0, newValue) ? Door.ControlState.Opened : Door.ControlState.Locked);
		this.applyLogicChange = true;
	}

	// Token: 0x0600426C RID: 17004 RVA: 0x002413AC File Offset: 0x0023F5AC
	public void Sim200ms(float dt)
	{
		if (this == null)
		{
			return;
		}
		if (this.doorOpenLiquidRefreshHack)
		{
			this.doorOpenLiquidRefreshTime -= dt;
			if (this.doorOpenLiquidRefreshTime <= 0f)
			{
				this.doorOpenLiquidRefreshHack = false;
				foreach (int cell in this.building.PlacementCells)
				{
					Pathfinding.Instance.AddDirtyNavGridCell(cell);
				}
			}
		}
		if (this.applyLogicChange)
		{
			this.applyLogicChange = false;
			this.ApplyRequestedControlState(false);
		}
		if (this.do_melt_check)
		{
			StructureTemperatureComponents structureTemperatures = GameComps.StructureTemperatures;
			HandleVector<int>.Handle handle = structureTemperatures.GetHandle(base.gameObject);
			if (handle.IsValid() && structureTemperatures.IsBypassed(handle))
			{
				foreach (int i2 in this.building.PlacementCells)
				{
					if (!Grid.Solid[i2])
					{
						Util.KDestroyGameObject(this);
						return;
					}
				}
			}
		}
	}

	// Token: 0x0600426E RID: 17006 RVA: 0x000CAEDC File Offset: 0x000C90DC
	bool INavDoor.get_isSpawned()
	{
		return base.isSpawned;
	}

	// Token: 0x04002D2A RID: 11562
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002D2B RID: 11563
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x04002D2C RID: 11564
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x04002D2D RID: 11565
	[MyCmpReq]
	public Building building;

	// Token: 0x04002D2E RID: 11566
	[MyCmpGet]
	private EnergyConsumer consumer;

	// Token: 0x04002D2F RID: 11567
	[MyCmpAdd]
	private LoopingSounds loopingSounds;

	// Token: 0x04002D30 RID: 11568
	public Orientation verticalOrientation;

	// Token: 0x04002D31 RID: 11569
	[SerializeField]
	public bool hasComplexUserControls;

	// Token: 0x04002D32 RID: 11570
	[SerializeField]
	public float unpoweredAnimSpeed = 0.25f;

	// Token: 0x04002D33 RID: 11571
	[SerializeField]
	public float poweredAnimSpeed = 1f;

	// Token: 0x04002D34 RID: 11572
	[SerializeField]
	public Door.DoorType doorType;

	// Token: 0x04002D35 RID: 11573
	[SerializeField]
	public bool allowAutoControl = true;

	// Token: 0x04002D36 RID: 11574
	[SerializeField]
	public string doorClosingSoundEventName;

	// Token: 0x04002D37 RID: 11575
	[SerializeField]
	public string doorOpeningSoundEventName;

	// Token: 0x04002D38 RID: 11576
	private string doorClosingSound;

	// Token: 0x04002D39 RID: 11577
	private string doorOpeningSound;

	// Token: 0x04002D3A RID: 11578
	private static readonly HashedString SOUND_POWERED_PARAMETER = "doorPowered";

	// Token: 0x04002D3B RID: 11579
	private static readonly HashedString SOUND_PROGRESS_PARAMETER = "doorProgress";

	// Token: 0x04002D3C RID: 11580
	[Serialize]
	private bool hasBeenUnsealed;

	// Token: 0x04002D3D RID: 11581
	[Serialize]
	private Door.ControlState controlState;

	// Token: 0x04002D3E RID: 11582
	private bool on;

	// Token: 0x04002D3F RID: 11583
	private bool do_melt_check;

	// Token: 0x04002D40 RID: 11584
	private int openCount;

	// Token: 0x04002D41 RID: 11585
	private Door.ControlState requestedState;

	// Token: 0x04002D42 RID: 11586
	private Chore changeStateChore;

	// Token: 0x04002D43 RID: 11587
	private Door.Controller.Instance controller;

	// Token: 0x04002D44 RID: 11588
	private LoggerFSS log;

	// Token: 0x04002D45 RID: 11589
	private const float REFRESH_HACK_DELAY = 1f;

	// Token: 0x04002D46 RID: 11590
	private bool doorOpenLiquidRefreshHack;

	// Token: 0x04002D47 RID: 11591
	private float doorOpenLiquidRefreshTime;

	// Token: 0x04002D48 RID: 11592
	private static readonly EventSystem.IntraObjectHandler<Door> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002D49 RID: 11593
	public static readonly HashedString OPEN_CLOSE_PORT_ID = new HashedString("DoorOpenClose");

	// Token: 0x04002D4A RID: 11594
	private static readonly KAnimFile[] OVERRIDE_ANIMS = new KAnimFile[]
	{
		Assets.GetAnim("anim_use_remote_kanim")
	};

	// Token: 0x04002D4B RID: 11595
	private static readonly EventSystem.IntraObjectHandler<Door> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04002D4C RID: 11596
	private static readonly EventSystem.IntraObjectHandler<Door> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04002D4D RID: 11597
	private bool applyLogicChange;

	// Token: 0x02000D3F RID: 3391
	public enum DoorType
	{
		// Token: 0x04002D4F RID: 11599
		Pressure,
		// Token: 0x04002D50 RID: 11600
		ManualPressure,
		// Token: 0x04002D51 RID: 11601
		Internal,
		// Token: 0x04002D52 RID: 11602
		Sealed
	}

	// Token: 0x02000D40 RID: 3392
	public enum ControlState
	{
		// Token: 0x04002D54 RID: 11604
		Auto,
		// Token: 0x04002D55 RID: 11605
		Opened,
		// Token: 0x04002D56 RID: 11606
		Locked,
		// Token: 0x04002D57 RID: 11607
		NumStates
	}

	// Token: 0x02000D41 RID: 3393
	public class Controller : GameStateMachine<Door.Controller, Door.Controller.Instance, Door>
	{
		// Token: 0x0600426F RID: 17007 RVA: 0x00241538 File Offset: 0x0023F738
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.closed;
			this.root.Update("RefreshIsBlocked", delegate(Door.Controller.Instance smi, float dt)
			{
				smi.RefreshIsBlocked();
			}, UpdateRate.SIM_200ms, false).ParamTransition<bool>(this.isSealed, this.Sealed.closed, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue);
			this.closeblocked.PlayAnim("open").ParamTransition<bool>(this.isOpen, this.open, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>(this.isBlocked, this.closedelay, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse);
			this.closedelay.PlayAnim("open").ScheduleGoTo(0.5f, this.closing).ParamTransition<bool>(this.isOpen, this.open, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>(this.isBlocked, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue);
			this.closing.ParamTransition<bool>(this.isBlocked, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ToggleTag(GameTags.Transition).ToggleLoopingSound("Closing loop", (Door.Controller.Instance smi) => smi.master.doorClosingSound, (Door.Controller.Instance smi) => !string.IsNullOrEmpty(smi.master.doorClosingSound)).Enter("SetParams", delegate(Door.Controller.Instance smi)
			{
				smi.master.UpdateAnimAndSoundParams(smi.master.on);
			}).Update(delegate(Door.Controller.Instance smi, float dt)
			{
				if (smi.master.doorClosingSound != null)
				{
					smi.master.loopingSounds.UpdateSecondParameter(smi.master.doorClosingSound, Door.SOUND_PROGRESS_PARAMETER, smi.Get<KBatchedAnimController>().GetPositionPercent());
				}
			}, UpdateRate.SIM_33ms, false).Enter("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(true);
			}).Exit("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(false);
			}).PlayAnim("closing").OnAnimQueueComplete(this.closed);
			this.open.PlayAnim("open").ParamTransition<bool>(this.isOpen, this.closeblocked, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse).Enter("SetWorldStateOpen", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			});
			this.closed.PlayAnim("closed").ParamTransition<bool>(this.isOpen, this.opening, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).ParamTransition<bool>(this.isLocked, this.locking, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsTrue).Enter("SetWorldStateClosed", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			});
			this.locking.PlayAnim("locked_pre").OnAnimQueueComplete(this.locked).Enter("SetWorldStateClosed", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			});
			this.locked.PlayAnim("locked").ParamTransition<bool>(this.isLocked, this.unlocking, GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.IsFalse);
			this.unlocking.PlayAnim("locked_pst").OnAnimQueueComplete(this.closed);
			this.opening.ToggleTag(GameTags.Transition).ToggleLoopingSound("Opening loop", (Door.Controller.Instance smi) => smi.master.doorOpeningSound, (Door.Controller.Instance smi) => !string.IsNullOrEmpty(smi.master.doorOpeningSound)).Enter("SetParams", delegate(Door.Controller.Instance smi)
			{
				smi.master.UpdateAnimAndSoundParams(smi.master.on);
			}).Update(delegate(Door.Controller.Instance smi, float dt)
			{
				if (smi.master.doorOpeningSound != null)
				{
					smi.master.loopingSounds.UpdateSecondParameter(smi.master.doorOpeningSound, Door.SOUND_PROGRESS_PARAMETER, smi.Get<KBatchedAnimController>().GetPositionPercent());
				}
			}, UpdateRate.SIM_33ms, false).Enter("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(true);
			}).Exit("SetActive", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetActive(false);
			}).PlayAnim("opening").OnAnimQueueComplete(this.open);
			this.Sealed.Enter(delegate(Door.Controller.Instance smi)
			{
				OccupyArea component = smi.master.GetComponent<OccupyArea>();
				for (int i = 0; i < component.OccupiedCellsOffsets.Length; i++)
				{
					Grid.PreventFogOfWarReveal[Grid.OffsetCell(Grid.PosToCell(smi.master.gameObject), component.OccupiedCellsOffsets[i])] = false;
				}
				smi.sm.isLocked.Set(true, smi, false);
				smi.master.controlState = Door.ControlState.Locked;
				smi.master.RefreshControlState();
				if (smi.master.GetComponent<Unsealable>().facingRight)
				{
					smi.master.GetComponent<KBatchedAnimController>().FlipX = true;
				}
			}).Enter("SetWorldStateClosed", delegate(Door.Controller.Instance smi)
			{
				smi.master.SetWorldState();
			}).Exit(delegate(Door.Controller.Instance smi)
			{
				smi.sm.isLocked.Set(false, smi, false);
				smi.master.GetComponent<AccessControl>().controlEnabled = true;
				smi.master.controlState = Door.ControlState.Opened;
				smi.master.RefreshControlState();
				smi.sm.isOpen.Set(true, smi, false);
				smi.sm.isLocked.Set(false, smi, false);
				smi.sm.isSealed.Set(false, smi, false);
			});
			this.Sealed.closed.PlayAnim("sealed", KAnim.PlayMode.Once);
			this.Sealed.awaiting_unlock.ToggleChore((Door.Controller.Instance smi) => this.CreateUnsealChore(smi, true), this.Sealed.chore_pst);
			this.Sealed.chore_pst.Enter(delegate(Door.Controller.Instance smi)
			{
				smi.master.hasBeenUnsealed = true;
				if (smi.master.GetComponent<Unsealable>().unsealed)
				{
					smi.GoTo(this.opening);
					FogOfWarMask.ClearMask(Grid.CellRight(Grid.PosToCell(smi.master.gameObject)));
					FogOfWarMask.ClearMask(Grid.CellLeft(Grid.PosToCell(smi.master.gameObject)));
					return;
				}
				smi.GoTo(this.Sealed.closed);
			});
		}

		// Token: 0x06004270 RID: 17008 RVA: 0x00241A74 File Offset: 0x0023FC74
		private Chore CreateUnsealChore(Door.Controller.Instance smi, bool approach_right)
		{
			return new WorkChore<Unsealable>(Db.Get().ChoreTypes.Toggle, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x04002D58 RID: 11608
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State open;

		// Token: 0x04002D59 RID: 11609
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State opening;

		// Token: 0x04002D5A RID: 11610
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closed;

		// Token: 0x04002D5B RID: 11611
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closing;

		// Token: 0x04002D5C RID: 11612
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closedelay;

		// Token: 0x04002D5D RID: 11613
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closeblocked;

		// Token: 0x04002D5E RID: 11614
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State locking;

		// Token: 0x04002D5F RID: 11615
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State locked;

		// Token: 0x04002D60 RID: 11616
		public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State unlocking;

		// Token: 0x04002D61 RID: 11617
		public Door.Controller.SealedStates Sealed;

		// Token: 0x04002D62 RID: 11618
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isOpen;

		// Token: 0x04002D63 RID: 11619
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isLocked;

		// Token: 0x04002D64 RID: 11620
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isBlocked;

		// Token: 0x04002D65 RID: 11621
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isSealed;

		// Token: 0x04002D66 RID: 11622
		public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter sealDirectionRight;

		// Token: 0x02000D42 RID: 3394
		public class SealedStates : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State
		{
			// Token: 0x04002D67 RID: 11623
			public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closed;

			// Token: 0x04002D68 RID: 11624
			public Door.Controller.SealedStates.AwaitingUnlock awaiting_unlock;

			// Token: 0x04002D69 RID: 11625
			public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State chore_pst;

			// Token: 0x02000D43 RID: 3395
			public class AwaitingUnlock : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State
			{
				// Token: 0x04002D6A RID: 11626
				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State awaiting_arrival;

				// Token: 0x04002D6B RID: 11627
				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State unlocking;
			}
		}

		// Token: 0x02000D44 RID: 3396
		public new class Instance : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.GameInstance
		{
			// Token: 0x06004276 RID: 17014 RVA: 0x000CAEFE File Offset: 0x000C90FE
			public Instance(Door door) : base(door)
			{
			}

			// Token: 0x06004277 RID: 17015 RVA: 0x00241B2C File Offset: 0x0023FD2C
			public void RefreshIsBlocked()
			{
				bool value = false;
				foreach (int cell in this.building.PlacementCells)
				{
					if (Grid.Objects[cell, 40] != null)
					{
						value = true;
						break;
					}
				}
				base.sm.isBlocked.Set(value, base.smi, false);
			}

			// Token: 0x04002D6C RID: 11628
			[MyCmpReq]
			public Building building;
		}
	}
}
