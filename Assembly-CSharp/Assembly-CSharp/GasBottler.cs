﻿using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/GasBottler")]
public class GasBottler : Workable
{
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new GasBottler.Controller.Instance(this);
		this.smi.StartSM();
		this.UpdateStoredItemState();
	}

	protected override void OnCleanUp()
	{
		if (this.smi != null)
		{
			this.smi.StopSM("OnCleanUp");
		}
		base.OnCleanUp();
	}

	private void UpdateStoredItemState()
	{
		this.storage.allowItemRemoval = (this.smi != null && this.smi.GetCurrentState() == this.smi.sm.ready);
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject != null)
			{
				gameObject.Trigger(-778359855, this.storage);
			}
		}
	}

	public Storage storage;

	private GasBottler.Controller.Instance smi;

	private class Controller : GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler>
	{
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			this.empty.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.filling, (GasBottler.Controller.Instance smi) => smi.master.storage.IsFull());
			this.filling.PlayAnim("working").OnAnimQueueComplete(this.ready);
			this.ready.EventTransition(GameHashes.OnStorageChange, this.pickup, (GasBottler.Controller.Instance smi) => !smi.master.storage.IsFull()).Enter(delegate(GasBottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = true;
				foreach (GameObject gameObject in smi.master.storage.items)
				{
					gameObject.GetComponent<KPrefabID>().AddTag(GameTags.GasSource, false);
					gameObject.Trigger(-778359855, smi.master.storage);
				}
			}).Exit(delegate(GasBottler.Controller.Instance smi)
			{
				smi.master.storage.allowItemRemoval = false;
				foreach (GameObject go in smi.master.storage.items)
				{
					go.Trigger(-778359855, smi.master.storage);
				}
			});
			this.pickup.PlayAnim("pick_up").OnAnimQueueComplete(this.empty);
		}

		public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State empty;

		public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State filling;

		public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State ready;

		public GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.State pickup;

		public new class Instance : GameStateMachine<GasBottler.Controller, GasBottler.Controller.Instance, GasBottler, object>.GameInstance
		{
			public Instance(GasBottler master) : base(master)
			{
			}
		}
	}
}
