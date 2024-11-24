using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000A59 RID: 2649
public class FixedCapturePoint : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>
{
	// Token: 0x060030C3 RID: 12483 RVA: 0x001FD86C File Offset: 0x001FBA6C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.operational;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.unoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.DefaultState(this.operational.manual).TagTransition(GameTags.Operational, this.unoperational, true);
		this.operational.manual.ParamTransition<bool>(this.automated, this.operational.automated, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsTrue);
		this.operational.automated.ParamTransition<bool>(this.automated, this.operational.manual, GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.IsFalse).ToggleChore((FixedCapturePoint.Instance smi) => smi.CreateChore(), this.unoperational, this.unoperational).Update("FindFixedCapturable", delegate(FixedCapturePoint.Instance smi, float dt)
		{
			smi.FindFixedCapturable();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x040020F6 RID: 8438
	public static readonly Operational.Flag enabledFlag = new Operational.Flag("enabled", Operational.Flag.Type.Requirement);

	// Token: 0x040020F7 RID: 8439
	private StateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.BoolParameter automated;

	// Token: 0x040020F8 RID: 8440
	public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State unoperational;

	// Token: 0x040020F9 RID: 8441
	public FixedCapturePoint.OperationalState operational;

	// Token: 0x02000A5A RID: 2650
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040020FA RID: 8442
		public Func<FixedCapturePoint.Instance, FixedCapturableMonitor.Instance, bool> isAmountStoredOverCapacity;

		// Token: 0x040020FB RID: 8443
		public Func<FixedCapturePoint.Instance, int> getTargetCapturePoint = delegate(FixedCapturePoint.Instance smi)
		{
			int num = Grid.PosToCell(smi);
			Navigator navigator = smi.targetCapturable.Navigator;
			if (Grid.IsValidCell(num - 1) && navigator.CanReach(num - 1))
			{
				return num - 1;
			}
			if (Grid.IsValidCell(num + 1) && navigator.CanReach(num + 1))
			{
				return num + 1;
			}
			return num;
		};

		// Token: 0x040020FC RID: 8444
		public bool allowBabies;
	}

	// Token: 0x02000A5C RID: 2652
	public class OperationalState : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State
	{
		// Token: 0x040020FF RID: 8447
		public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State manual;

		// Token: 0x04002100 RID: 8448
		public GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.State automated;
	}

	// Token: 0x02000A5D RID: 2653
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<FixedCapturePoint, FixedCapturePoint.Instance, IStateMachineTarget, FixedCapturePoint.Def>.GameInstance
	{
		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060030CB RID: 12491 RVA: 0x000BFB50 File Offset: 0x000BDD50
		// (set) Token: 0x060030CC RID: 12492 RVA: 0x000BFB58 File Offset: 0x000BDD58
		public FixedCapturableMonitor.Instance targetCapturable { get; private set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060030CD RID: 12493 RVA: 0x000BFB61 File Offset: 0x000BDD61
		// (set) Token: 0x060030CE RID: 12494 RVA: 0x000BFB69 File Offset: 0x000BDD69
		public bool shouldCreatureGoGetCaptured { get; private set; }

		// Token: 0x060030CF RID: 12495 RVA: 0x001FD9C8 File Offset: 0x001FBBC8
		public Instance(IStateMachineTarget master, FixedCapturePoint.Def def) : base(master, def)
		{
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			this.captureCell = Grid.PosToCell(base.transform.GetPosition());
			this.critterCapactiy = base.GetComponent<BaggableCritterCapacityTracker>();
			this.operationComp = base.GetComponent<Operational>();
			this.logicPorts = base.GetComponent<LogicPorts>();
			if (this.logicPorts != null)
			{
				base.Subscribe(-801688580, new Action<object>(this.OnLogicEvent));
				this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, !this.logicPorts.IsPortConnected("CritterPickUpInput") || this.logicPorts.GetInputValue("CritterPickUpInput") > 0);
				return;
			}
			this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, true);
		}

		// Token: 0x060030D0 RID: 12496 RVA: 0x001FDAA8 File Offset: 0x001FBCA8
		private void OnLogicEvent(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == "CritterPickUpInput" && this.logicPorts.IsPortConnected("CritterPickUpInput"))
			{
				this.operationComp.SetFlag(FixedCapturePoint.enabledFlag, logicValueChanged.newValue > 0);
			}
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x000BFB72 File Offset: 0x000BDD72
		public override void StartSM()
		{
			base.StartSM();
			if (base.GetComponent<FixedCapturePoint.AutoWrangleCapture>() == null)
			{
				base.sm.automated.Set(true, this, false);
			}
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x001FDB04 File Offset: 0x001FBD04
		private void OnCopySettings(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject == null)
			{
				return;
			}
			FixedCapturePoint.Instance smi = gameObject.GetSMI<FixedCapturePoint.Instance>();
			if (smi == null)
			{
				return;
			}
			base.sm.automated.Set(base.sm.automated.Get(smi), this, false);
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000BFB9C File Offset: 0x000BDD9C
		public bool GetAutomated()
		{
			return base.sm.automated.Get(this);
		}

		// Token: 0x060030D4 RID: 12500 RVA: 0x000BFBAF File Offset: 0x000BDDAF
		public void SetAutomated(bool automate)
		{
			base.sm.automated.Set(automate, this, false);
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x000BFBC5 File Offset: 0x000BDDC5
		public Chore CreateChore()
		{
			this.FindFixedCapturable();
			return new FixedCaptureChore(base.GetComponent<KPrefabID>());
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x001FDB54 File Offset: 0x001FBD54
		public bool IsCreatureAvailableForFixedCapture()
		{
			if (!this.targetCapturable.IsNullOrStopped())
			{
				CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.captureCell);
				return FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, this.captureCell);
			}
			return false;
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000BFBD8 File Offset: 0x000BDDD8
		public void SetRancherIsAvailableForCapturing()
		{
			this.shouldCreatureGoGetCaptured = true;
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000BFBE1 File Offset: 0x000BDDE1
		public void ClearRancherIsAvailableForCapturing()
		{
			this.shouldCreatureGoGetCaptured = false;
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x001FDB9C File Offset: 0x001FBD9C
		private static bool CanCapturableBeCapturedAtCapturePoint(FixedCapturableMonitor.Instance capturable, FixedCapturePoint.Instance capture_point, CavityInfo capture_cavity_info, int capture_cell)
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
			return cavityForCell != null && cavityForCell == capture_cavity_info && !capturable.HasTag(GameTags.Creatures.Bagged) && (!capturable.isBaby || capture_point.def.allowBabies) && capturable.ChoreConsumer.IsChoreEqualOrAboveCurrentChorePriority<FixedCaptureStates>() && capturable.Navigator.GetNavigationCost(capture_cell) != -1 && capture_point.def.isAmountStoredOverCapacity(capture_point, capturable);
		}

		// Token: 0x060030DA RID: 12506 RVA: 0x001FDC50 File Offset: 0x001FBE50
		public void FindFixedCapturable()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(num);
			if (cavityForCell == null)
			{
				this.ResetCapturePoint();
				return;
			}
			if (!this.targetCapturable.IsNullOrStopped() && !FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(this.targetCapturable, this, cavityForCell, num))
			{
				this.ResetCapturePoint();
			}
			if (this.targetCapturable.IsNullOrStopped())
			{
				foreach (object obj in Components.FixedCapturableMonitors)
				{
					FixedCapturableMonitor.Instance instance = (FixedCapturableMonitor.Instance)obj;
					if (FixedCapturePoint.Instance.CanCapturableBeCapturedAtCapturePoint(instance, this, cavityForCell, num))
					{
						this.targetCapturable = instance;
						if (!this.targetCapturable.IsNullOrStopped())
						{
							this.targetCapturable.targetCapturePoint = this;
							break;
						}
						break;
					}
				}
			}
		}

		// Token: 0x060030DB RID: 12507 RVA: 0x000BFBEA File Offset: 0x000BDDEA
		public void ResetCapturePoint()
		{
			base.Trigger(643180843, null);
			if (!this.targetCapturable.IsNullOrStopped())
			{
				this.targetCapturable.targetCapturePoint = null;
				this.targetCapturable.Trigger(1034952693, null);
				this.targetCapturable = null;
			}
		}

		// Token: 0x04002103 RID: 8451
		public BaggableCritterCapacityTracker critterCapactiy;

		// Token: 0x04002104 RID: 8452
		private int captureCell;

		// Token: 0x04002105 RID: 8453
		private Operational operationComp;

		// Token: 0x04002106 RID: 8454
		private LogicPorts logicPorts;
	}

	// Token: 0x02000A5E RID: 2654
	public class AutoWrangleCapture : KMonoBehaviour, ICheckboxControl
	{
		// Token: 0x060030DC RID: 12508 RVA: 0x000BFC29 File Offset: 0x000BDE29
		protected override void OnSpawn()
		{
			base.OnSpawn();
			this.fcp = this.GetSMI<FixedCapturePoint.Instance>();
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060030DD RID: 12509 RVA: 0x000BFC3D File Offset: 0x000BDE3D
		string ICheckboxControl.CheckboxTitleKey
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.TITLE.key.String;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060030DE RID: 12510 RVA: 0x000BFC4E File Offset: 0x000BDE4E
		string ICheckboxControl.CheckboxLabel
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060030DF RID: 12511 RVA: 0x000BFC5A File Offset: 0x000BDE5A
		string ICheckboxControl.CheckboxTooltip
		{
			get
			{
				return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.AUTOWRANGLE_TOOLTIP;
			}
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x000BFC66 File Offset: 0x000BDE66
		bool ICheckboxControl.GetCheckboxValue()
		{
			return this.fcp.GetAutomated();
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x000BFC73 File Offset: 0x000BDE73
		void ICheckboxControl.SetCheckboxValue(bool value)
		{
			this.fcp.SetAutomated(value);
		}

		// Token: 0x04002107 RID: 8455
		private FixedCapturePoint.Instance fcp;
	}
}
