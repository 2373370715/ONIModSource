using System;
using KSerialization;
using STRINGS;
using UnityEngine;

public class FixedCapturePoint : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>
{
	public class Def : BaseDef
	{
		public Func<Instance, FixedCapturableMonitor.Instance, bool> isAmountStoredOverCapacity;

		public Func<Instance, int> getTargetCapturePoint = delegate(Instance smi)
		{
			int num = Grid.PosToCell(smi);
			Navigator navigator = smi.targetCapturable.Navigator;
			if (Grid.IsValidCell(num - 1) && navigator.CanReach(num - 1))
			{
				return num - 1;
			}
			return (Grid.IsValidCell(num + 1) && navigator.CanReach(num + 1)) ? (num + 1) : num;
		};

		public bool allowBabies;
	}

	public class OperationalState : State
	{
		public State manual;

		public State automated;
	}

	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameInstance
	{
		public BaggableCritterCapacityTracker critterCapactiy;

		private int captureCell;

		private Operational operationComp;

		private LogicPorts logicPorts;

		public FixedCapturableMonitor.Instance targetCapturable { get; private set; }

		public bool shouldCreatureGoGetCaptured { get; private set; }

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			Subscribe(-905833192, OnCopySettings);
			captureCell = Grid.PosToCell(base.transform.GetPosition());
			critterCapactiy = GetComponent<BaggableCritterCapacityTracker>();
			operationComp = GetComponent<Operational>();
			logicPorts = GetComponent<LogicPorts>();
			if (logicPorts != null)
			{
				Subscribe(-801688580, OnLogicEvent);
				operationComp.SetFlag(enabledFlag, !logicPorts.IsPortConnected("CritterPickUpInput") || logicPorts.GetInputValue("CritterPickUpInput") > 0);
			}
			else
			{
				operationComp.SetFlag(enabledFlag, value: true);
			}
		}

		private void OnLogicEvent(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == "CritterPickUpInput" && logicPorts.IsPortConnected("CritterPickUpInput"))
			{
				operationComp.SetFlag(enabledFlag, logicValueChanged.newValue > 0);
			}
		}

		public override void StartSM()
		{
			base.StartSM();
			if (GetComponent<AutoWrangleCapture>() == null)
			{
				base.sm.automated.Set(value: true, this);
			}
		}

		private void OnCopySettings(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (!(gameObject == null))
			{
				Instance sMI = gameObject.GetSMI<Instance>();
				if (sMI != null)
				{
					base.sm.automated.Set(base.sm.automated.Get(sMI), this);
				}
			}
		}

		public bool GetAutomated()
		{
			return base.sm.automated.Get(this);
		}

		public void SetAutomated(bool automate)
		{
			base.sm.automated.Set(automate, this);
		}

		public Chore CreateChore()
		{
			FindFixedCapturable();
			return new FixedCaptureChore(GetComponent<KPrefabID>());
		}

		public bool IsCreatureAvailableForFixedCapture()
		{
			if (!targetCapturable.IsNullOrStopped())
			{
				CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(captureCell);
				return CanCapturableBeCapturedAtCapturePoint(targetCapturable, this, cavityForCell, captureCell);
			}
			return false;
		}

		public void SetRancherIsAvailableForCapturing()
		{
			shouldCreatureGoGetCaptured = true;
		}

		public void ClearRancherIsAvailableForCapturing()
		{
			shouldCreatureGoGetCaptured = false;
		}

		private static bool CanCapturableBeCapturedAtCapturePoint(FixedCapturableMonitor.Instance capturable, Instance capture_point, CavityInfo capture_cavity_info, int capture_cell)
		{
			if (!capturable.IsRunning())
			{
				return false;
			}
			if (capturable.targetCapturePoint != capture_point && !capturable.targetCapturePoint.IsNullOrStopped())
			{
				return false;
			}
			int cell = Grid.PosToCell(capturable.transform.GetPosition());
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			if (cavityForCell == null || cavityForCell != capture_cavity_info)
			{
				return false;
			}
			if (capturable.HasTag(GameTags.Creatures.Bagged))
			{
				return false;
			}
			if (capturable.isBaby && !capture_point.def.allowBabies)
			{
				return false;
			}
			if (!capturable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>())
			{
				return false;
			}
			if (capturable.Navigator.GetNavigationCost(capture_cell) == -1)
			{
				return false;
			}
			return capture_point.def.isAmountStoredOverCapacity(capture_point, capturable);
		}

		public void FindFixedCapturable()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(num);
			if (cavityForCell == null)
			{
				ResetCapturePoint();
				return;
			}
			if (!targetCapturable.IsNullOrStopped() && !CanCapturableBeCapturedAtCapturePoint(targetCapturable, this, cavityForCell, num))
			{
				ResetCapturePoint();
			}
			if (!targetCapturable.IsNullOrStopped())
			{
				return;
			}
			foreach (FixedCapturableMonitor.Instance fixedCapturableMonitor in Components.FixedCapturableMonitors)
			{
				if (CanCapturableBeCapturedAtCapturePoint(fixedCapturableMonitor, this, cavityForCell, num))
				{
					targetCapturable = fixedCapturableMonitor;
					if (!targetCapturable.IsNullOrStopped())
					{
						targetCapturable.targetCapturePoint = this;
					}
					break;
				}
			}
		}

		public void ResetCapturePoint()
		{
			Trigger(643180843);
			if (!targetCapturable.IsNullOrStopped())
			{
				targetCapturable.targetCapturePoint = null;
				targetCapturable.Trigger(1034952693);
				targetCapturable = null;
			}
		}
	}

	public class AutoWrangleCapture : KMonoBehaviour, ICheckboxControl
	{
		private Instance fcp;

		string ICheckboxControl.CheckboxTitleKey => UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.TITLE.key.String;

		string ICheckboxControl.CheckboxLabel => UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE;

		string ICheckboxControl.CheckboxTooltip => UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE_TOOLTIP;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			fcp = this.GetSMI<Instance>();
		}

		bool ICheckboxControl.GetCheckboxValue()
		{
			return fcp.GetAutomated();
		}

		void ICheckboxControl.SetCheckboxValue(bool value)
		{
			fcp.SetAutomated(value);
		}
	}

	public static readonly Operational.Flag enabledFlag = new Operational.Flag("enabled", Operational.Flag.Type.Requirement);

	private BoolParameter automated;

	public State unoperational;

	public OperationalState operational;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = operational;
		base.serializable = SerializeType.Both_DEPRECATED;
		unoperational.TagTransition(GameTags.Operational, operational);
		operational.DefaultState(operational.manual).TagTransition(GameTags.Operational, unoperational, on_remove: true);
		operational.manual.ParamTransition(automated, operational.automated, GameStateMachine<FixedCapturePoint, Instance, IStateMachineTarget, Def>.IsTrue);
		operational.automated.ParamTransition(automated, operational.manual, GameStateMachine<FixedCapturePoint, Instance, IStateMachineTarget, Def>.IsFalse).ToggleChore((Instance smi) => smi.CreateChore(), unoperational, unoperational).Update("FindFixedCapturable", delegate(Instance smi, float dt)
		{
			smi.FindFixedCapturable();
		}, UpdateRate.SIM_1000ms);
	}
}
