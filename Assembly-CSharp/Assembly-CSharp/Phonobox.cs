﻿using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Phonobox : StateMachineComponent<Phonobox.StatesInstance>, IGameObjectEffectDescriptor
{
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new PhonoboxWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("PhonoboxWorkable", pos);
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			PhonoboxWorkable phonoboxWorkable = go.AddOrGet<PhonoboxWorkable>();
			phonoboxWorkable.owner = this;
			this.workables[i] = phonoboxWorkable;
		}
	}

	protected override void OnCleanUp()
	{
		this.UpdateChores(false);
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<PhonoboxWorkable> workChore = new WorkChore<PhonoboxWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		return workChore;
	}

	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	public void UpdateChores(bool update = true)
	{
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (update)
			{
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = this.CreateChore(i);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
	}

	public void AddWorker(Worker player)
	{
		this.players.Add(player);
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	public void RemoveWorker(Worker player)
	{
		this.players.Remove(player);
		base.smi.sm.playerCount.Set(this.players.Count, base.smi, false);
	}

	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Danced", true);
		return list;
	}

	public const string SPECIFIC_EFFECT = "Danced";

	public const string TRACKING_EFFECT = "RecentlyDanced";

	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(-1, 0),
		new CellOffset(1, 0),
		new CellOffset(-2, 0),
		new CellOffset(2, 0)
	};

	private PhonoboxWorkable[] workables;

	private Chore[] chores;

	private HashSet<Worker> players = new HashSet<Worker>();

	private static string[] building_anims = new string[]
	{
		"working_loop",
		"working_loop2",
		"working_loop3"
	};

	public class States : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox>
	{
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(false);
			}).TagTransition(GameTags.Operational, this.operational, false).PlayAnim("off");
			this.operational.TagTransition(GameTags.Operational, this.unoperational, true).Enter("CreateChore", delegate(Phonobox.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(Phonobox.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).DefaultState(this.operational.stopped);
			this.operational.stopped.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(false);
			}).ParamTransition<int>(this.playerCount, this.operational.pre, (Phonobox.StatesInstance smi, int p) => p > 0).PlayAnim("on");
			this.operational.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.operational.playing);
			this.operational.playing.Enter(delegate(Phonobox.StatesInstance smi)
			{
				smi.SetActive(true);
			}).ScheduleGoTo(25f, this.operational.song_end).ParamTransition<int>(this.playerCount, this.operational.post, (Phonobox.StatesInstance smi, int p) => p == 0).PlayAnim(new Func<Phonobox.StatesInstance, string>(Phonobox.States.GetPlayAnim), KAnim.PlayMode.Loop);
			this.operational.song_end.ParamTransition<int>(this.playerCount, this.operational.bridge, (Phonobox.StatesInstance smi, int p) => p > 0).ParamTransition<int>(this.playerCount, this.operational.post, (Phonobox.StatesInstance smi, int p) => p == 0);
			this.operational.bridge.PlayAnim("working_trans").OnAnimQueueComplete(this.operational.playing);
			this.operational.post.PlayAnim("working_pst").OnAnimQueueComplete(this.operational.stopped);
		}

		public static string GetPlayAnim(Phonobox.StatesInstance smi)
		{
			int num = UnityEngine.Random.Range(0, Phonobox.building_anims.Length);
			return Phonobox.building_anims[num];
		}

		public StateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.IntParameter playerCount;

		public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State unoperational;

		public Phonobox.States.OperationalStates operational;

		public class OperationalStates : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State
		{
			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State stopped;

			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State pre;

			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State bridge;

			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State playing;

			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State song_end;

			public GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.State post;
		}
	}

	public class StatesInstance : GameStateMachine<Phonobox.States, Phonobox.StatesInstance, Phonobox, object>.GameInstance
	{
		public StatesInstance(Phonobox smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		private FetchChore chore;

		private Operational operational;
	}
}
