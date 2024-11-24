﻿using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class Telepad : StateMachineComponent<Telepad.StatesInstance>
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<Deconstructable>().allowDeconstruction = false;
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		if (num == 0)
		{
			global::Debug.LogError(string.Concat(new string[]
			{
				"Headquarters spawned at: (",
				num.ToString(),
				",",
				num2.ToString(),
				")"
			}));
		}
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Telepads.Add(this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
		base.smi.StartSM();
	}

	protected override void OnCleanUp()
	{
		Components.Telepads.Remove(this);
		base.OnCleanUp();
	}

	public void Update()
	{
		if (base.smi.IsColonyLost())
		{
			return;
		}
		if (Immigration.Instance.ImmigrantsAvailable && base.GetComponent<Operational>().IsOperational)
		{
			base.smi.sm.openPortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NewDuplicantsAvailable, this);
		}
		else
		{
			base.smi.sm.closePortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Wattson, this);
		}
		if (this.GetTimeRemaining() < -120f)
		{
			Messenger.Instance.QueueMessage(new DuplicantsLeftMessage());
			Immigration.Instance.EndImmigration();
		}
	}

	public void RejectAll()
	{
		Immigration.Instance.EndImmigration();
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	public void OnAcceptDelivery(ITelepadDeliverable delivery)
	{
		int cell = Grid.PosToCell(this);
		Immigration.Instance.EndImmigration();
		GameObject gameObject = delivery.Deliver(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
		if (component != null)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, GameClock.Instance.GetTimeSinceStartOfReport(), string.Format(UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, DUPLICANTS.CHORES.NOT_EXISTING_TASK), gameObject.GetProperName());
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.gameObject.GetComponent<KSelectable>().GetMyWorldId(), false))
			{
				minionIdentity.GetComponent<Effects>().Add("NewCrewArrival", true);
			}
			MinionResume component2 = component.GetComponent<MinionResume>();
			int num = 0;
			while ((float)num < this.startingSkillPoints)
			{
				component2.ForceAddSkillPoint();
				num++;
			}
		}
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	public float GetTimeRemaining()
	{
		return Immigration.Instance.GetTimeRemaining();
	}

	[MyCmpReq]
	private KSelectable selectable;

	private MeterController meter;

	private const float MAX_IMMIGRATION_TIME = 120f;

	private const int NUM_METER_NOTCHES = 8;

	private List<MinionStartingStats> minionStats;

	public float startingSkillPoints;

	public static readonly HashedString[] PortalBirthAnim = new HashedString[]
	{
		"portalbirth"
	};

	public class StatesInstance : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.GameInstance
	{
		public StatesInstance(Telepad master) : base(master)
		{
		}

		public bool IsColonyLost()
		{
			return GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver();
		}

		public void UpdateMeter()
		{
			float timeRemaining = Immigration.Instance.GetTimeRemaining();
			float totalWaitTime = Immigration.Instance.GetTotalWaitTime();
			float positionPercent = Mathf.Clamp01(1f - timeRemaining / totalWaitTime);
			base.master.meter.SetPositionPercent(positionPercent);
		}
	}

	public class States : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad>
	{
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.OnSignal(this.idlePortal, this.resetToIdle);
			this.resetToIdle.GoTo(this.idle);
			this.idle.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.UpdateMeter();
			}).Update("TelepadMeter", delegate(Telepad.StatesInstance smi, float dt)
			{
				smi.UpdateMeter();
			}, UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).PlayAnim("idle").OnSignal(this.openPortal, this.opening);
			this.unoperational.PlayAnim("idle").Enter("StopImmigration", delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).EventTransition(GameHashes.OperationalChanged, this.idle, (Telepad.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.opening.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.open);
			this.open.OnSignal(this.closePortal, this.close).Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_loop", KAnim.PlayMode.Loop).Transition(this.close, (Telepad.StatesInstance smi) => smi.IsColonyLost(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OperationalChanged, this.close, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.close.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).PlayAnims((Telepad.StatesInstance smi) => Telepad.States.workingAnims, KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
		}

		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal openPortal;

		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal closePortal;

		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal idlePortal;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State idle;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State resetToIdle;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State opening;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State open;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State close;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State unoperational;

		private static readonly HashedString[] workingAnims = new HashedString[]
		{
			"working_loop",
			"working_pst"
		};
	}
}
