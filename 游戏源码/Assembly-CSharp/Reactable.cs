using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200080C RID: 2060
public abstract class Reactable
{
	// Token: 0x1700010B RID: 267
	// (get) Token: 0x060024D0 RID: 9424 RVA: 0x000B7F9D File Offset: 0x000B619D
	public bool IsValid
	{
		get
		{
			return this.partitionerEntry.IsValid();
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x060024D1 RID: 9425 RVA: 0x000B7FAA File Offset: 0x000B61AA
	// (set) Token: 0x060024D2 RID: 9426 RVA: 0x000B7FB2 File Offset: 0x000B61B2
	public float creationTime { get; private set; }

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x060024D3 RID: 9427 RVA: 0x000B7FBB File Offset: 0x000B61BB
	public bool IsReacting
	{
		get
		{
			return this.reactor != null;
		}
	}

	// Token: 0x060024D4 RID: 9428 RVA: 0x001CAD5C File Offset: 0x001C8F5C
	public Reactable(GameObject gameObject, HashedString id, ChoreType chore_type, int range_width = 15, int range_height = 8, bool follow_transform = false, float globalCooldown = 0f, float localCooldown = 0f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f, ObjectLayer overrideLayer = ObjectLayer.NumLayers)
	{
		this.rangeHeight = range_height;
		this.rangeWidth = range_width;
		this.id = id;
		this.gameObject = gameObject;
		this.choreType = chore_type;
		this.globalCooldown = globalCooldown;
		this.localCooldown = localCooldown;
		this.lifeSpan = lifeSpan;
		this.initialDelay = ((max_initial_delay > 0f) ? UnityEngine.Random.Range(0f, max_initial_delay) : 0f);
		this.creationTime = GameClock.Instance.GetTime();
		ObjectLayer objectLayer = (overrideLayer == ObjectLayer.NumLayers) ? this.reactionLayer : overrideLayer;
		ReactionMonitor.Def def = gameObject.GetDef<ReactionMonitor.Def>();
		if (overrideLayer != objectLayer && def != null)
		{
			objectLayer = def.ReactionLayer;
		}
		this.reactionLayer = objectLayer;
		this.Initialize(follow_transform);
	}

	// Token: 0x060024D5 RID: 9429 RVA: 0x000B7FC9 File Offset: 0x000B61C9
	public void Initialize(bool followTransform)
	{
		this.UpdateLocation();
		if (followTransform)
		{
			this.transformId = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.gameObject.transform, new System.Action(this.UpdateLocation), "Reactable follow transform");
		}
	}

	// Token: 0x060024D6 RID: 9430 RVA: 0x000B8000 File Offset: 0x000B6200
	public void Begin(GameObject reactor)
	{
		this.reactor = reactor;
		this.lastTriggerTime = GameClock.Instance.GetTime();
		this.InternalBegin();
	}

	// Token: 0x060024D7 RID: 9431 RVA: 0x001CAE38 File Offset: 0x001C9038
	public void End()
	{
		this.InternalEnd();
		if (this.reactor != null)
		{
			GameObject gameObject = this.reactor;
			this.InternalEnd();
			this.reactor = null;
			if (gameObject != null)
			{
				ReactionMonitor.Instance smi = gameObject.GetSMI<ReactionMonitor.Instance>();
				if (smi != null)
				{
					smi.StopReaction();
				}
			}
		}
	}

	// Token: 0x060024D8 RID: 9432 RVA: 0x001CAE88 File Offset: 0x001C9088
	public bool CanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		float time = GameClock.Instance.GetTime();
		float num = time - this.creationTime;
		float num2 = time - this.lastTriggerTime;
		if (num < this.initialDelay || num2 < this.globalCooldown)
		{
			return false;
		}
		ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
		Chore chore = (component != null) ? component.choreDriver.GetCurrentChore() : null;
		if (chore == null || this.choreType.priority <= chore.choreType.priority)
		{
			return false;
		}
		int num3 = 0;
		while (this.additionalPreconditions != null && num3 < this.additionalPreconditions.Count)
		{
			if (!this.additionalPreconditions[num3](reactor, transition))
			{
				return false;
			}
			num3++;
		}
		return this.InternalCanBegin(reactor, transition);
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x000B801F File Offset: 0x000B621F
	public bool IsExpired()
	{
		return GameClock.Instance.GetTime() - this.creationTime > this.lifeSpan;
	}

	// Token: 0x060024DA RID: 9434
	public abstract bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition);

	// Token: 0x060024DB RID: 9435
	public abstract void Update(float dt);

	// Token: 0x060024DC RID: 9436
	protected abstract void InternalBegin();

	// Token: 0x060024DD RID: 9437
	protected abstract void InternalEnd();

	// Token: 0x060024DE RID: 9438
	protected abstract void InternalCleanup();

	// Token: 0x060024DF RID: 9439 RVA: 0x001CAF44 File Offset: 0x001C9144
	public void Cleanup()
	{
		this.End();
		this.InternalCleanup();
		if (this.transformId != -1)
		{
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transformId, new System.Action(this.UpdateLocation));
			this.transformId = -1;
		}
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x060024E0 RID: 9440 RVA: 0x001CAF9C File Offset: 0x001C919C
	private void UpdateLocation()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.gameObject != null)
		{
			this.sourceCell = Grid.PosToCell(this.gameObject);
			Extents extents = new Extents(Grid.PosToXY(this.gameObject.transform.GetPosition()).x - this.rangeWidth / 2, Grid.PosToXY(this.gameObject.transform.GetPosition()).y - this.rangeHeight / 2, this.rangeWidth, this.rangeHeight);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("Reactable", this, extents, GameScenePartitioner.Instance.objectLayers[(int)this.reactionLayer], null);
		}
	}

	// Token: 0x060024E1 RID: 9441 RVA: 0x000B803A File Offset: 0x000B623A
	public Reactable AddPrecondition(Reactable.ReactablePrecondition precondition)
	{
		if (this.additionalPreconditions == null)
		{
			this.additionalPreconditions = new List<Reactable.ReactablePrecondition>();
		}
		this.additionalPreconditions.Add(precondition);
		return this;
	}

	// Token: 0x060024E2 RID: 9442 RVA: 0x000B805C File Offset: 0x000B625C
	public void InsertPrecondition(int index, Reactable.ReactablePrecondition precondition)
	{
		if (this.additionalPreconditions == null)
		{
			this.additionalPreconditions = new List<Reactable.ReactablePrecondition>();
		}
		index = Math.Min(index, this.additionalPreconditions.Count);
		this.additionalPreconditions.Insert(index, precondition);
	}

	// Token: 0x040018EB RID: 6379
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040018EC RID: 6380
	protected GameObject gameObject;

	// Token: 0x040018ED RID: 6381
	public HashedString id;

	// Token: 0x040018EE RID: 6382
	public bool preventChoreInterruption = true;

	// Token: 0x040018EF RID: 6383
	public int sourceCell;

	// Token: 0x040018F0 RID: 6384
	private int rangeWidth;

	// Token: 0x040018F1 RID: 6385
	private int rangeHeight;

	// Token: 0x040018F2 RID: 6386
	private int transformId = -1;

	// Token: 0x040018F3 RID: 6387
	public float globalCooldown;

	// Token: 0x040018F4 RID: 6388
	public float localCooldown;

	// Token: 0x040018F5 RID: 6389
	public float lifeSpan = float.PositiveInfinity;

	// Token: 0x040018F6 RID: 6390
	private float lastTriggerTime = -2.1474836E+09f;

	// Token: 0x040018F7 RID: 6391
	private float initialDelay;

	// Token: 0x040018F9 RID: 6393
	protected GameObject reactor;

	// Token: 0x040018FA RID: 6394
	private ChoreType choreType;

	// Token: 0x040018FB RID: 6395
	protected LoggerFSS log;

	// Token: 0x040018FC RID: 6396
	private List<Reactable.ReactablePrecondition> additionalPreconditions;

	// Token: 0x040018FD RID: 6397
	private ObjectLayer reactionLayer;

	// Token: 0x0200080D RID: 2061
	// (Invoke) Token: 0x060024E4 RID: 9444
	public delegate bool ReactablePrecondition(GameObject go, Navigator.ActiveTransition transition);
}
