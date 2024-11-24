using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

public class Grave : StateMachineComponent<Grave.StatesInstance>
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, Grave, object>.GameInstance
	{
		private FetchChore chore;

		public StatesInstance(Grave master)
			: base(master)
		{
		}

		public void CreateFetchTask()
		{
			chore = new FetchChore(Db.Get().ChoreTypes.FetchCritical, GetComponent<Storage>(), 1f, new HashSet<Tag> { GameTags.Minion }, FetchChore.MatchCriteria.MatchID, GameTags.Corpse);
			chore.allowMultifetch = false;
		}

		public void CancelFetchTask()
		{
			chore.Cancel("Exit State");
			chore = null;
		}
	}

	public class States : GameStateMachine<States, StatesInstance, Grave>
	{
		public State empty;

		public State full;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = empty;
			base.serializable = SerializeType.Both_DEPRECATED;
			empty.PlayAnim("open").Enter("CreateFetchTask", delegate(StatesInstance smi)
			{
				smi.CreateFetchTask();
			}).Exit("CancelFetchTask", delegate(StatesInstance smi)
			{
				smi.CancelFetchTask();
			})
				.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GraveEmpty)
				.EventTransition(GameHashes.OnStorageChange, full);
			full.PlayAnim((StatesInstance smi) => smi.master.graveAnim).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Grave).Enter(delegate(StatesInstance smi)
			{
				if (smi.master.burialTime < 0f)
				{
					smi.master.burialTime = GameClock.Instance.GetTime();
				}
			});
		}
	}

	[Serialize]
	public string graveName;

	[Serialize]
	public string graveAnim = "closed";

	[Serialize]
	public int epitaphIdx;

	[Serialize]
	public float burialTime = -1f;

	private static readonly CellOffset[] DELIVERY_OFFSETS = new CellOffset[1];

	private static readonly EventSystem.IntraObjectHandler<Grave> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Grave>(delegate(Grave component, object data)
	{
		component.OnStorageChanged(data);
	});

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Subscribe(-1697596308, OnStorageChangedDelegate);
		epitaphIdx = UnityEngine.Random.Range(0, int.MaxValue);
	}

	protected override void OnSpawn()
	{
		GetComponent<Storage>().SetOffsets(DELIVERY_OFFSETS);
		Storage component = GetComponent<Storage>();
		component.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(component.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(OnWorkEvent));
		KAnimFile anim = Assets.GetAnim("anim_bury_dupe_kanim");
		int num = 0;
		while (true)
		{
			KAnim.Anim anim2 = anim.GetData().GetAnim(num);
			if (anim2 == null)
			{
				break;
			}
			if (anim2.name == "working_pre")
			{
				float workTime = (float)(anim2.numFrames - 3) / anim2.frameRate;
				component.SetWorkTime(workTime);
				break;
			}
			num++;
		}
		base.OnSpawn();
		base.smi.StartSM();
		Components.Graves.Add(this);
	}

	protected override void OnCleanUp()
	{
		Components.Graves.Remove(this);
		base.OnCleanUp();
	}

	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (!(gameObject != null))
		{
			return;
		}
		graveName = gameObject.name;
		MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
		if (component != null)
		{
			Personality personality = Db.Get().Personalities.TryGet(component.personalityResourceId);
			KAnimFile anim = Assets.GetAnim("gravestone_kanim");
			if (personality != null && anim.GetData().GetAnim(personality.graveStone) != null)
			{
				graveAnim = personality.graveStone;
			}
		}
		Util.KDestroyGameObject(gameObject);
	}

	private void OnWorkEvent(Workable workable, Workable.WorkableEvent evt)
	{
	}
}
