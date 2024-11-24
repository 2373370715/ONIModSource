using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000CC1 RID: 3265
public class Checkpoint : StateMachineComponent<Checkpoint.SMInstance>
{
	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06003F2D RID: 16173 RVA: 0x000C90BD File Offset: 0x000C72BD
	private bool RedLightDesiredState
	{
		get
		{
			return this.hasLogicWire && !this.hasInputHigh && this.operational.IsOperational;
		}
	}

	// Token: 0x06003F2E RID: 16174 RVA: 0x00236AC0 File Offset: 0x00234CC0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Checkpoint>(-801688580, Checkpoint.OnLogicValueChangedDelegate);
		base.Subscribe<Checkpoint>(-592767678, Checkpoint.OnOperationalChangedDelegate);
		base.smi.StartSM();
		if (Checkpoint.infoStatusItem_Logic == null)
		{
			Checkpoint.infoStatusItem_Logic = new StatusItem("CheckpointLogic", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			Checkpoint.infoStatusItem_Logic.resolveStringCallback = new Func<string, object, string>(Checkpoint.ResolveInfoStatusItem_Logic);
		}
		this.Refresh(this.redLight);
	}

	// Token: 0x06003F2F RID: 16175 RVA: 0x000C90DC File Offset: 0x000C72DC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.ClearReactable();
	}

	// Token: 0x06003F30 RID: 16176 RVA: 0x000C90EA File Offset: 0x000C72EA
	public void RefreshLight()
	{
		if (this.redLight != this.RedLightDesiredState)
		{
			this.Refresh(this.RedLightDesiredState);
			this.statusDirty = true;
		}
		if (this.statusDirty)
		{
			this.RefreshStatusItem();
		}
	}

	// Token: 0x06003F31 RID: 16177 RVA: 0x00236B54 File Offset: 0x00234D54
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(Checkpoint.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x06003F32 RID: 16178 RVA: 0x000C911B File Offset: 0x000C731B
	private static string ResolveInfoStatusItem_Logic(string format_str, object data)
	{
		return ((Checkpoint)data).RedLight ? BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_CLOSED : BUILDING.STATUSITEMS.CHECKPOINT.LOGIC_CONTROLLED_OPEN;
	}

	// Token: 0x06003F33 RID: 16179 RVA: 0x000C913B File Offset: 0x000C733B
	private void CreateNewReactable()
	{
		if (this.reactable == null)
		{
			this.reactable = new Checkpoint.CheckpointReactable(this);
		}
	}

	// Token: 0x06003F34 RID: 16180 RVA: 0x000C9151 File Offset: 0x000C7351
	private void OrphanReactable()
	{
		this.reactable = null;
	}

	// Token: 0x06003F35 RID: 16181 RVA: 0x000C915A File Offset: 0x000C735A
	private void ClearReactable()
	{
		if (this.reactable != null)
		{
			this.reactable.Cleanup();
			this.reactable = null;
		}
	}

	// Token: 0x17000303 RID: 771
	// (get) Token: 0x06003F36 RID: 16182 RVA: 0x000C9176 File Offset: 0x000C7376
	public bool RedLight
	{
		get
		{
			return this.redLight;
		}
	}

	// Token: 0x06003F37 RID: 16183 RVA: 0x00236B84 File Offset: 0x00234D84
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == Checkpoint.PORT_ID)
		{
			this.hasInputHigh = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.hasLogicWire = (this.GetNetwork() != null);
			this.statusDirty = true;
		}
	}

	// Token: 0x06003F38 RID: 16184 RVA: 0x000C917E File Offset: 0x000C737E
	private void OnOperationalChanged(object data)
	{
		this.statusDirty = true;
	}

	// Token: 0x06003F39 RID: 16185 RVA: 0x00236BD4 File Offset: 0x00234DD4
	private void RefreshStatusItem()
	{
		bool on = this.operational.IsOperational && this.hasLogicWire;
		this.selectable.ToggleStatusItem(Checkpoint.infoStatusItem_Logic, on, this);
		this.statusDirty = false;
	}

	// Token: 0x06003F3A RID: 16186 RVA: 0x00236C14 File Offset: 0x00234E14
	private void Refresh(bool redLightState)
	{
		this.redLight = redLightState;
		this.operational.SetActive(this.operational.IsOperational && this.redLight, false);
		base.smi.sm.redLight.Set(this.redLight, base.smi, false);
		if (this.redLight)
		{
			this.CreateNewReactable();
			return;
		}
		this.ClearReactable();
	}

	// Token: 0x04002B19 RID: 11033
	[MyCmpReq]
	public Operational operational;

	// Token: 0x04002B1A RID: 11034
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002B1B RID: 11035
	private static StatusItem infoStatusItem_Logic;

	// Token: 0x04002B1C RID: 11036
	private Checkpoint.CheckpointReactable reactable;

	// Token: 0x04002B1D RID: 11037
	public static readonly HashedString PORT_ID = "Checkpoint";

	// Token: 0x04002B1E RID: 11038
	private bool hasLogicWire;

	// Token: 0x04002B1F RID: 11039
	private bool hasInputHigh;

	// Token: 0x04002B20 RID: 11040
	private bool redLight;

	// Token: 0x04002B21 RID: 11041
	private bool statusDirty = true;

	// Token: 0x04002B22 RID: 11042
	private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>(delegate(Checkpoint component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04002B23 RID: 11043
	private static readonly EventSystem.IntraObjectHandler<Checkpoint> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Checkpoint>(delegate(Checkpoint component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x02000CC2 RID: 3266
	private class CheckpointReactable : Reactable
	{
		// Token: 0x06003F3D RID: 16189 RVA: 0x00236CD4 File Offset: 0x00234ED4
		public CheckpointReactable(Checkpoint checkpoint) : base(checkpoint.gameObject, "CheckpointReactable", Db.Get().ChoreTypes.Checkpoint, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
		{
			this.checkpoint = checkpoint;
			this.rotated = this.gameObject.GetComponent<Rotatable>().IsRotated;
			this.preventChoreInterruption = false;
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x00236D44 File Offset: 0x00234F44
		public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
		{
			if (this.reactor != null)
			{
				return false;
			}
			if (this.checkpoint == null)
			{
				base.Cleanup();
				return false;
			}
			if (!this.checkpoint.RedLight)
			{
				return false;
			}
			if (this.rotated)
			{
				return transition.x < 0;
			}
			return transition.x > 0;
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x00236DA4 File Offset: 0x00234FA4
		protected override void InternalBegin()
		{
			this.reactor_navigator = this.reactor.GetComponent<Navigator>();
			KBatchedAnimController component = this.reactor.GetComponent<KBatchedAnimController>();
			component.AddAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"), 1f);
			component.Play("idle_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
			this.checkpoint.OrphanReactable();
			this.checkpoint.CreateNewReactable();
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x00236E34 File Offset: 0x00235034
		public override void Update(float dt)
		{
			if (this.checkpoint == null || !this.checkpoint.RedLight || this.reactor_navigator == null)
			{
				base.Cleanup();
				return;
			}
			this.reactor_navigator.AdvancePath(false);
			if (!this.reactor_navigator.path.IsValid())
			{
				base.Cleanup();
				return;
			}
			NavGrid.Transition nextTransition = this.reactor_navigator.GetNextTransition();
			if (!(this.rotated ? (nextTransition.x < 0) : (nextTransition.x > 0)))
			{
				base.Cleanup();
			}
		}

		// Token: 0x06003F41 RID: 16193 RVA: 0x000C9196 File Offset: 0x000C7396
		protected override void InternalEnd()
		{
			if (this.reactor != null)
			{
				this.reactor.GetComponent<KBatchedAnimController>().RemoveAnimOverrides(Assets.GetAnim("anim_idle_distracted_kanim"));
			}
		}

		// Token: 0x06003F42 RID: 16194 RVA: 0x000A5E40 File Offset: 0x000A4040
		protected override void InternalCleanup()
		{
		}

		// Token: 0x04002B24 RID: 11044
		private Checkpoint checkpoint;

		// Token: 0x04002B25 RID: 11045
		private Navigator reactor_navigator;

		// Token: 0x04002B26 RID: 11046
		private bool rotated;
	}

	// Token: 0x02000CC3 RID: 3267
	public class SMInstance : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.GameInstance
	{
		// Token: 0x06003F43 RID: 16195 RVA: 0x000C91C5 File Offset: 0x000C73C5
		public SMInstance(Checkpoint master) : base(master)
		{
		}
	}

	// Token: 0x02000CC4 RID: 3268
	public class States : GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint>
	{
		// Token: 0x06003F44 RID: 16196 RVA: 0x00236EC8 File Offset: 0x002350C8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.go;
			this.root.Update("RefreshLight", delegate(Checkpoint.SMInstance smi, float dt)
			{
				smi.master.RefreshLight();
			}, UpdateRate.SIM_200ms, false);
			this.stop.ParamTransition<bool>(this.redLight, this.go, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsFalse).PlayAnim("red_light");
			this.go.ParamTransition<bool>(this.redLight, this.stop, GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.IsTrue).PlayAnim("green_light");
		}

		// Token: 0x04002B27 RID: 11047
		public StateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.BoolParameter redLight;

		// Token: 0x04002B28 RID: 11048
		public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State stop;

		// Token: 0x04002B29 RID: 11049
		public GameStateMachine<Checkpoint.States, Checkpoint.SMInstance, Checkpoint, object>.State go;
	}
}
