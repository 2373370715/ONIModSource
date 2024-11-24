using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reactable
{
	public delegate bool ReactablePrecondition(GameObject go, Navigator.ActiveTransition transition);

	private HandleVector<int>.Handle partitionerEntry;

	protected GameObject gameObject;

	public HashedString id;

	public bool preventChoreInterruption = true;

	public int sourceCell;

	private int rangeWidth;

	private int rangeHeight;

	private int transformId = -1;

	public float globalCooldown;

	public float localCooldown;

	public float lifeSpan = float.PositiveInfinity;

	private float lastTriggerTime = -2.1474836E+09f;

	private float initialDelay;

	protected GameObject reactor;

	private ChoreType choreType;

	protected LoggerFSS log;

	private List<ReactablePrecondition> additionalPreconditions;

	private ObjectLayer reactionLayer;

	public bool IsValid => partitionerEntry.IsValid();

	public float creationTime { get; private set; }

	public bool IsReacting => reactor != null;

	public Reactable(GameObject gameObject, HashedString id, ChoreType chore_type, int range_width = 15, int range_height = 8, bool follow_transform = false, float globalCooldown = 0f, float localCooldown = 0f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f, ObjectLayer overrideLayer = ObjectLayer.NumLayers)
	{
		rangeHeight = range_height;
		rangeWidth = range_width;
		this.id = id;
		this.gameObject = gameObject;
		choreType = chore_type;
		this.globalCooldown = globalCooldown;
		this.localCooldown = localCooldown;
		this.lifeSpan = lifeSpan;
		initialDelay = ((max_initial_delay > 0f) ? UnityEngine.Random.Range(0f, max_initial_delay) : 0f);
		creationTime = GameClock.Instance.GetTime();
		ObjectLayer objectLayer = ((overrideLayer == ObjectLayer.NumLayers) ? reactionLayer : overrideLayer);
		ReactionMonitor.Def def = gameObject.GetDef<ReactionMonitor.Def>();
		if (overrideLayer != objectLayer && def != null)
		{
			objectLayer = def.ReactionLayer;
		}
		reactionLayer = objectLayer;
		Initialize(follow_transform);
	}

	public void Initialize(bool followTransform)
	{
		UpdateLocation();
		if (followTransform)
		{
			transformId = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(gameObject.transform, UpdateLocation, "Reactable follow transform");
		}
	}

	public void Begin(GameObject reactor)
	{
		this.reactor = reactor;
		lastTriggerTime = GameClock.Instance.GetTime();
		InternalBegin();
	}

	public void End()
	{
		InternalEnd();
		if (reactor != null)
		{
			GameObject gameObject = reactor;
			InternalEnd();
			reactor = null;
			if (gameObject != null)
			{
				gameObject.GetSMI<ReactionMonitor.Instance>()?.StopReaction();
			}
		}
	}

	public bool CanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		float time = GameClock.Instance.GetTime();
		float num = time - creationTime;
		float num2 = time - lastTriggerTime;
		if (num < initialDelay || num2 < globalCooldown)
		{
			return false;
		}
		ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
		Chore chore = ((component != null) ? component.choreDriver.GetCurrentChore() : null);
		if (chore == null || choreType.priority <= chore.choreType.priority)
		{
			return false;
		}
		int num3 = 0;
		while (additionalPreconditions != null && num3 < additionalPreconditions.Count)
		{
			if (!additionalPreconditions[num3](reactor, transition))
			{
				return false;
			}
			num3++;
		}
		return InternalCanBegin(reactor, transition);
	}

	public bool IsExpired()
	{
		return GameClock.Instance.GetTime() - creationTime > lifeSpan;
	}

	public abstract bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition);

	public abstract void Update(float dt);

	protected abstract void InternalBegin();

	protected abstract void InternalEnd();

	protected abstract void InternalCleanup();

	public void Cleanup()
	{
		End();
		InternalCleanup();
		if (transformId != -1)
		{
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(transformId, UpdateLocation);
			transformId = -1;
		}
		GameScenePartitioner.Instance.Free(ref partitionerEntry);
	}

	private void UpdateLocation()
	{
		GameScenePartitioner.Instance.Free(ref partitionerEntry);
		if (gameObject != null)
		{
			sourceCell = Grid.PosToCell(gameObject);
			Extents extents = new Extents(Grid.PosToXY(gameObject.transform.GetPosition()).x - rangeWidth / 2, Grid.PosToXY(gameObject.transform.GetPosition()).y - rangeHeight / 2, rangeWidth, rangeHeight);
			partitionerEntry = GameScenePartitioner.Instance.Add("Reactable", this, extents, GameScenePartitioner.Instance.objectLayers[(int)reactionLayer], null);
		}
	}

	public Reactable AddPrecondition(ReactablePrecondition precondition)
	{
		if (additionalPreconditions == null)
		{
			additionalPreconditions = new List<ReactablePrecondition>();
		}
		additionalPreconditions.Add(precondition);
		return this;
	}

	public void InsertPrecondition(int index, ReactablePrecondition precondition)
	{
		if (additionalPreconditions == null)
		{
			additionalPreconditions = new List<ReactablePrecondition>();
		}
		index = Math.Min(index, additionalPreconditions.Count);
		additionalPreconditions.Insert(index, precondition);
	}
}
