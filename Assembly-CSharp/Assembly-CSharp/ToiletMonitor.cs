﻿using System;

public class ToiletMonitor : GameStateMachine<ToiletMonitor, ToiletMonitor.Instance>
{
		public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventHandler(GameHashes.ToiletSensorChanged, delegate(ToiletMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		}).Exit("ClearStatusItem", delegate(ToiletMonitor.Instance smi)
		{
			smi.ClearStatusItem();
		});
	}

		public GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.State satisfied;

		public GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.State unsatisfied;

		public new class Instance : GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
				public Instance(IStateMachineTarget master) : base(master)
		{
			this.toiletSensor = base.GetComponent<Sensors>().GetSensor<ToiletSensor>();
		}

				public void RefreshStatusItem()
		{
			StatusItem status_item = null;
			if (!this.toiletSensor.AreThereAnyToilets())
			{
				status_item = Db.Get().DuplicantStatusItems.NoToilets;
			}
			else if (!this.toiletSensor.AreThereAnyUsableToilets())
			{
				status_item = Db.Get().DuplicantStatusItems.NoUsableToilets;
			}
			else if (this.toiletSensor.GetNearestUsableToilet() == null)
			{
				status_item = Db.Get().DuplicantStatusItems.ToiletUnreachable;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Toilet, status_item, null);
		}

				public void ClearStatusItem()
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Toilet, null, null);
		}

				private ToiletSensor toiletSensor;
	}
}
