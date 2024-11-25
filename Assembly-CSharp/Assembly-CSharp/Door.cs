﻿using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Door")]
public class Door : Workable, ISaveLoadable, ISim200ms, INavDoor
{
		private void OnCopySettings(object data)
	{
		Door component = ((GameObject)data).GetComponent<Door>();
		if (component != null)
		{
			this.QueueStateChange(component.requestedState);
		}
	}

		public Door()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

			public Door.ControlState CurrentState
	{
		get
		{
			return this.controlState;
		}
	}

			public Door.ControlState RequestedState
	{
		get
		{
			return this.requestedState;
		}
	}

			public bool ShouldBlockFallingSand
	{
		get
		{
			return this.rotatable.GetOrientation() != this.verticalOrientation;
		}
	}

			public bool isSealed
	{
		get
		{
			return this.controller != null && this.controller.sm.isSealed.Get(this.controller);
		}
	}

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

		private Door.ControlState GetNextState(Door.ControlState wantedState)
	{
		return (wantedState + 1) % Door.ControlState.NumStates;
	}

		private static bool DisplacesGas(Door.DoorType type)
	{
		return type != Door.DoorType.Internal;
	}

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

		public void Seal()
	{
		this.controller.sm.isSealed.Set(true, this.controller, false);
	}

		public void OrderUnseal()
	{
		this.controller.GoTo(this.controller.sm.Sealed.awaiting_unlock);
	}

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

		private void UpdateDoorSpeed(bool powered)
	{
		this.on = powered;
		this.UpdateAnimAndSoundParams(powered);
		float positionPercent = this.animController.GetPositionPercent();
		this.animController.Play(this.animController.CurrentAnim.hash, this.animController.PlayMode, 1f, 0f);
		this.animController.SetPositionPercent(positionPercent);
	}

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

		private void SetActive(bool active)
	{
		if (this.operational.IsOperational)
		{
			this.operational.SetActive(active, false);
		}
	}

		private void SetWorldState()
	{
		int[] placementCells = this.building.PlacementCells;
		bool is_door_open = this.IsOpen();
		this.SetPassableState(is_door_open, placementCells);
		this.SetSimState(is_door_open, placementCells);
	}

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

		protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.changeStateChore = null;
		this.ApplyRequestedControlState(false);
	}

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

		public bool IsPendingClose()
	{
		return this.controller.IsInsideState(this.controller.sm.closedelay);
	}

		public bool IsOpen()
	{
		return this.controller.IsInsideState(this.controller.sm.open) || this.controller.IsInsideState(this.controller.sm.closedelay) || this.controller.IsInsideState(this.controller.sm.closeblocked);
	}

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

		bool INavDoor.get_isSpawned()
	{
		return base.isSpawned;
	}

		[MyCmpReq]
	private Operational operational;

		[MyCmpGet]
	private Rotatable rotatable;

		[MyCmpReq]
	private KBatchedAnimController animController;

		[MyCmpReq]
	public Building building;

		[MyCmpGet]
	private EnergyConsumer consumer;

		[MyCmpAdd]
	private LoopingSounds loopingSounds;

		public Orientation verticalOrientation;

		[SerializeField]
	public bool hasComplexUserControls;

		[SerializeField]
	public float unpoweredAnimSpeed = 0.25f;

		[SerializeField]
	public float poweredAnimSpeed = 1f;

		[SerializeField]
	public Door.DoorType doorType;

		[SerializeField]
	public bool allowAutoControl = true;

		[SerializeField]
	public string doorClosingSoundEventName;

		[SerializeField]
	public string doorOpeningSoundEventName;

		private string doorClosingSound;

		private string doorOpeningSound;

		private static readonly HashedString SOUND_POWERED_PARAMETER = "doorPowered";

		private static readonly HashedString SOUND_PROGRESS_PARAMETER = "doorProgress";

		[Serialize]
	private bool hasBeenUnsealed;

		[Serialize]
	private Door.ControlState controlState;

		private bool on;

		private bool do_melt_check;

		private int openCount;

		private Door.ControlState requestedState;

		private Chore changeStateChore;

		private Door.Controller.Instance controller;

		private LoggerFSS log;

		private const float REFRESH_HACK_DELAY = 1f;

		private bool doorOpenLiquidRefreshHack;

		private float doorOpenLiquidRefreshTime;

		private static readonly EventSystem.IntraObjectHandler<Door> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnCopySettings(data);
	});

		public static readonly HashedString OPEN_CLOSE_PORT_ID = new HashedString("DoorOpenClose");

		private static readonly KAnimFile[] OVERRIDE_ANIMS = new KAnimFile[]
	{
		Assets.GetAnim("anim_use_remote_kanim")
	};

		private static readonly EventSystem.IntraObjectHandler<Door> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnOperationalChanged(data);
	});

		private static readonly EventSystem.IntraObjectHandler<Door> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Door>(delegate(Door component, object data)
	{
		component.OnLogicValueChanged(data);
	});

		private bool applyLogicChange;

		public enum DoorType
	{
				Pressure,
				ManualPressure,
				Internal,
				Sealed
	}

		public enum ControlState
	{
				Auto,
				Opened,
				Locked,
				NumStates
	}

		public class Controller : GameStateMachine<Door.Controller, Door.Controller.Instance, Door>
	{
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

				private Chore CreateUnsealChore(Door.Controller.Instance smi, bool approach_right)
		{
			return new WorkChore<Unsealable>(Db.Get().ChoreTypes.Toggle, smi.master, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State open;

				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State opening;

				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closed;

				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closing;

				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closedelay;

				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closeblocked;

				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State locking;

				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State locked;

				public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State unlocking;

				public Door.Controller.SealedStates Sealed;

				public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isOpen;

				public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isLocked;

				public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isBlocked;

				public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter isSealed;

				public StateMachine<Door.Controller, Door.Controller.Instance, Door, object>.BoolParameter sealDirectionRight;

				public class SealedStates : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State
		{
						public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State closed;

						public Door.Controller.SealedStates.AwaitingUnlock awaiting_unlock;

						public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State chore_pst;

						public class AwaitingUnlock : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State
			{
								public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State awaiting_arrival;

								public GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.State unlocking;
			}
		}

				public new class Instance : GameStateMachine<Door.Controller, Door.Controller.Instance, Door, object>.GameInstance
		{
						public Instance(Door door) : base(door)
			{
			}

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

						[MyCmpReq]
			public Building building;
		}
	}
}
