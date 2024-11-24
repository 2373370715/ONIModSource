using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000DCD RID: 3533
public class Grave : StateMachineComponent<Grave.StatesInstance>
{
	// Token: 0x06004579 RID: 17785 RVA: 0x000CCF7C File Offset: 0x000CB17C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Grave>(-1697596308, Grave.OnStorageChangedDelegate);
		this.epitaphIdx = UnityEngine.Random.Range(0, int.MaxValue);
	}

	// Token: 0x0600457A RID: 17786 RVA: 0x0024BCEC File Offset: 0x00249EEC
	protected override void OnSpawn()
	{
		base.GetComponent<Storage>().SetOffsets(Grave.DELIVERY_OFFSETS);
		Storage component = base.GetComponent<Storage>();
		Storage storage = component;
		storage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(storage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkEvent));
		KAnimFile anim = Assets.GetAnim("anim_bury_dupe_kanim");
		int num = 0;
		KAnim.Anim anim2;
		for (;;)
		{
			anim2 = anim.GetData().GetAnim(num);
			if (anim2 == null)
			{
				goto IL_8F;
			}
			if (anim2.name == "working_pre")
			{
				break;
			}
			num++;
		}
		float workTime = (float)(anim2.numFrames - 3) / anim2.frameRate;
		component.SetWorkTime(workTime);
		IL_8F:
		base.OnSpawn();
		base.smi.StartSM();
		Components.Graves.Add(this);
	}

	// Token: 0x0600457B RID: 17787 RVA: 0x000CCFA6 File Offset: 0x000CB1A6
	protected override void OnCleanUp()
	{
		Components.Graves.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x0600457C RID: 17788 RVA: 0x0024BDA4 File Offset: 0x00249FA4
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.graveName = gameObject.name;
			MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
			if (component != null)
			{
				Personality personality = Db.Get().Personalities.TryGet(component.personalityResourceId);
				KAnimFile anim = Assets.GetAnim("gravestone_kanim");
				if (personality != null && anim.GetData().GetAnim(personality.graveStone) != null)
				{
					this.graveAnim = personality.graveStone;
				}
			}
			Util.KDestroyGameObject(gameObject);
		}
	}

	// Token: 0x0600457D RID: 17789 RVA: 0x000CCFB9 File Offset: 0x000CB1B9
	private void OnWorkEvent(Workable workable, Workable.WorkableEvent evt)
	{
	}

	// Token: 0x04002FEF RID: 12271
	[Serialize]
	public string graveName;

	// Token: 0x04002FF0 RID: 12272
	[Serialize]
	public string graveAnim = "closed";

	// Token: 0x04002FF1 RID: 12273
	[Serialize]
	public int epitaphIdx;

	// Token: 0x04002FF2 RID: 12274
	[Serialize]
	public float burialTime = -1f;

	// Token: 0x04002FF3 RID: 12275
	private static readonly CellOffset[] DELIVERY_OFFSETS = new CellOffset[1];

	// Token: 0x04002FF4 RID: 12276
	private static readonly EventSystem.IntraObjectHandler<Grave> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Grave>(delegate(Grave component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02000DCE RID: 3534
	public class StatesInstance : GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.GameInstance
	{
		// Token: 0x06004580 RID: 17792 RVA: 0x000CD002 File Offset: 0x000CB202
		public StatesInstance(Grave master) : base(master)
		{
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x0024BE2C File Offset: 0x0024A02C
		public void CreateFetchTask()
		{
			this.chore = new FetchChore(Db.Get().ChoreTypes.FetchCritical, base.GetComponent<Storage>(), 1f, new HashSet<Tag>
			{
				GameTags.BaseMinion
			}, FetchChore.MatchCriteria.MatchTags, GameTags.Corpse, null, null, true, null, null, null, Operational.State.Operational, 0);
			this.chore.allowMultifetch = false;
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x000CD00B File Offset: 0x000CB20B
		public void CancelFetchTask()
		{
			this.chore.Cancel("Exit State");
			this.chore = null;
		}

		// Token: 0x04002FF5 RID: 12277
		private FetchChore chore;
	}

	// Token: 0x02000DCF RID: 3535
	public class States : GameStateMachine<Grave.States, Grave.StatesInstance, Grave>
	{
		// Token: 0x06004583 RID: 17795 RVA: 0x0024BE8C File Offset: 0x0024A08C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.empty;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.empty.PlayAnim("open").Enter("CreateFetchTask", delegate(Grave.StatesInstance smi)
			{
				smi.CreateFetchTask();
			}).Exit("CancelFetchTask", delegate(Grave.StatesInstance smi)
			{
				smi.CancelFetchTask();
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GraveEmpty, null).EventTransition(GameHashes.OnStorageChange, this.full, null);
			this.full.PlayAnim((Grave.StatesInstance smi) => smi.master.graveAnim, KAnim.PlayMode.Once).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Grave, null).Enter(delegate(Grave.StatesInstance smi)
			{
				if (smi.master.burialTime < 0f)
				{
					smi.master.burialTime = GameClock.Instance.GetTime();
				}
			});
		}

		// Token: 0x04002FF6 RID: 12278
		public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State empty;

		// Token: 0x04002FF7 RID: 12279
		public GameStateMachine<Grave.States, Grave.StatesInstance, Grave, object>.State full;
	}
}
