using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020013D8 RID: 5080
public class HEPBattery : GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>
{
	// Token: 0x06006818 RID: 26648 RVA: 0x002D6504 File Offset: 0x002D4704
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.operational, false).Update(delegate(HEPBattery.Instance smi, float dt)
		{
			smi.DoConsumeParticlesWhileDisabled(dt);
			smi.UpdateDecayStatusItem(false);
		}, UpdateRate.SIM_200ms, false);
		this.operational.Enter("SetActive(true)", delegate(HEPBattery.Instance smi)
		{
			smi.operational.SetActive(true, false);
		}).Exit("SetActive(false)", delegate(HEPBattery.Instance smi)
		{
			smi.operational.SetActive(false, false);
		}).PlayAnim("on", KAnim.PlayMode.Loop).TagTransition(GameTags.Operational, this.inoperational, true).Update(new Action<HEPBattery.Instance, float>(this.LauncherUpdate), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06006819 RID: 26649 RVA: 0x002D65EC File Offset: 0x002D47EC
	public void LauncherUpdate(HEPBattery.Instance smi, float dt)
	{
		smi.UpdateDecayStatusItem(true);
		smi.UpdateMeter(null);
		smi.operational.SetActive(smi.particleStorage.Particles > 0f, false);
		smi.launcherTimer += dt;
		if (smi.launcherTimer < smi.def.minLaunchInterval || !smi.AllowSpawnParticles)
		{
			return;
		}
		if (smi.particleStorage.Particles >= smi.particleThreshold)
		{
			smi.launcherTimer = 0f;
			this.Fire(smi);
		}
	}

	// Token: 0x0600681A RID: 26650 RVA: 0x002D6674 File Offset: 0x002D4874
	public void Fire(HEPBattery.Instance smi)
	{
		int highEnergyParticleOutputCell = smi.GetComponent<Building>().GetHighEnergyParticleOutputCell();
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("HighEnergyParticle"), Grid.CellToPosCCC(highEnergyParticleOutputCell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, null, 0);
		gameObject.SetActive(true);
		if (gameObject != null)
		{
			HighEnergyParticle component = gameObject.GetComponent<HighEnergyParticle>();
			component.payload = smi.particleStorage.ConsumeAndGet(smi.particleThreshold);
			component.SetDirection(smi.def.direction);
		}
	}

	// Token: 0x04004E8A RID: 20106
	public static readonly HashedString FIRE_PORT_ID = "HEPBatteryFire";

	// Token: 0x04004E8B RID: 20107
	public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State inoperational;

	// Token: 0x04004E8C RID: 20108
	public GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.State operational;

	// Token: 0x020013D9 RID: 5081
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04004E8D RID: 20109
		public float particleDecayRate;

		// Token: 0x04004E8E RID: 20110
		public float minLaunchInterval;

		// Token: 0x04004E8F RID: 20111
		public float minSlider;

		// Token: 0x04004E90 RID: 20112
		public float maxSlider;

		// Token: 0x04004E91 RID: 20113
		public EightDirection direction;
	}

	// Token: 0x020013DA RID: 5082
	public new class Instance : GameStateMachine<HEPBattery, HEPBattery.Instance, IStateMachineTarget, HEPBattery.Def>.GameInstance, ISingleSliderControl, ISliderControl
	{
		// Token: 0x0600681E RID: 26654 RVA: 0x002D66EC File Offset: 0x002D48EC
		public Instance(IStateMachineTarget master, HEPBattery.Def def) : base(master, def)
		{
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			this.UpdateMeter(null);
		}

		// Token: 0x0600681F RID: 26655 RVA: 0x000E42AE File Offset: 0x000E24AE
		public void DoConsumeParticlesWhileDisabled(float dt)
		{
			if (this.m_skipFirstUpdate)
			{
				this.m_skipFirstUpdate = false;
				return;
			}
			this.particleStorage.ConsumeAndGet(dt * base.def.particleDecayRate);
			this.UpdateMeter(null);
		}

		// Token: 0x06006820 RID: 26656 RVA: 0x000E42E0 File Offset: 0x000E24E0
		public void UpdateMeter(object data = null)
		{
			this.meterController.SetPositionPercent(this.particleStorage.Particles / this.particleStorage.Capacity());
		}

		// Token: 0x06006821 RID: 26657 RVA: 0x002D6778 File Offset: 0x002D4978
		public void UpdateDecayStatusItem(bool hasPower)
		{
			if (!hasPower)
			{
				if (this.particleStorage.Particles > 0f)
				{
					if (this.statusHandle == Guid.Empty)
					{
						this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.LosingRadbolts, null);
						return;
					}
				}
				else if (this.statusHandle != Guid.Empty)
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
					this.statusHandle = Guid.Empty;
					return;
				}
			}
			else if (this.statusHandle != Guid.Empty)
			{
				base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
				this.statusHandle = Guid.Empty;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06006822 RID: 26658 RVA: 0x000E4304 File Offset: 0x000E2504
		public bool AllowSpawnParticles
		{
			get
			{
				return this.hasLogicWire && this.isLogicActive;
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x06006823 RID: 26659 RVA: 0x000E4316 File Offset: 0x000E2516
		public bool HasLogicWire
		{
			get
			{
				return this.hasLogicWire;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06006824 RID: 26660 RVA: 0x000E431E File Offset: 0x000E251E
		public bool IsLogicActive
		{
			get
			{
				return this.isLogicActive;
			}
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x002D6834 File Offset: 0x002D4A34
		private LogicCircuitNetwork GetNetwork()
		{
			int portCell = base.GetComponent<LogicPorts>().GetPortCell(HEPBattery.FIRE_PORT_ID);
			return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x002D6864 File Offset: 0x002D4A64
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID == HEPBattery.FIRE_PORT_ID)
			{
				this.isLogicActive = (logicValueChanged.newValue > 0);
				this.hasLogicWire = (this.GetNetwork() != null);
			}
		}

		// Token: 0x06006827 RID: 26663 RVA: 0x002D68A8 File Offset: 0x002D4AA8
		private void OnCopySettings(object data)
		{
			GameObject gameObject = data as GameObject;
			if (gameObject != null)
			{
				HEPBattery.Instance smi = gameObject.GetSMI<HEPBattery.Instance>();
				if (smi != null)
				{
					this.particleThreshold = smi.particleThreshold;
				}
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x06006828 RID: 26664 RVA: 0x000CD83A File Offset: 0x000CBA3A
		public string SliderTitleKey
		{
			get
			{
				return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TITLE";
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06006829 RID: 26665 RVA: 0x000CA9A4 File Offset: 0x000C8BA4
		public string SliderUnits
		{
			get
			{
				return UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES;
			}
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public int SliderDecimalPlaces(int index)
		{
			return 0;
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x000E4326 File Offset: 0x000E2526
		public float GetSliderMin(int index)
		{
			return base.def.minSlider;
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x000E4333 File Offset: 0x000E2533
		public float GetSliderMax(int index)
		{
			return base.def.maxSlider;
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x000E4340 File Offset: 0x000E2540
		public float GetSliderValue(int index)
		{
			return this.particleThreshold;
		}

		// Token: 0x0600682E RID: 26670 RVA: 0x000E4348 File Offset: 0x000E2548
		public void SetSliderValue(float value, int index)
		{
			this.particleThreshold = value;
		}

		// Token: 0x0600682F RID: 26671 RVA: 0x000CD85C File Offset: 0x000CBA5C
		public string GetSliderTooltipKey(int index)
		{
			return "STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP";
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x000E4351 File Offset: 0x000E2551
		string ISliderControl.GetSliderTooltip(int index)
		{
			return string.Format(Strings.Get("STRINGS.UI.UISIDESCREENS.RADBOLTTHRESHOLDSIDESCREEN.TOOLTIP"), this.particleThreshold);
		}

		// Token: 0x04004E92 RID: 20114
		[MyCmpReq]
		public HighEnergyParticleStorage particleStorage;

		// Token: 0x04004E93 RID: 20115
		[MyCmpGet]
		public Operational operational;

		// Token: 0x04004E94 RID: 20116
		[MyCmpAdd]
		public CopyBuildingSettings copyBuildingSettings;

		// Token: 0x04004E95 RID: 20117
		[Serialize]
		public float launcherTimer;

		// Token: 0x04004E96 RID: 20118
		[Serialize]
		public float particleThreshold = 50f;

		// Token: 0x04004E97 RID: 20119
		public bool ShowWorkingStatus;

		// Token: 0x04004E98 RID: 20120
		private bool m_skipFirstUpdate = true;

		// Token: 0x04004E99 RID: 20121
		private MeterController meterController;

		// Token: 0x04004E9A RID: 20122
		private Guid statusHandle = Guid.Empty;

		// Token: 0x04004E9B RID: 20123
		private bool hasLogicWire;

		// Token: 0x04004E9C RID: 20124
		private bool isLogicActive;
	}
}
