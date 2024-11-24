using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/Brain")]
public class Brain : KMonoBehaviour
{
	private bool running;

	private bool suspend;

	protected KPrefabID prefabId;

	protected ChoreConsumer choreConsumer;

	public event System.Action onPreUpdate;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	protected override void OnSpawn()
	{
		prefabId = GetComponent<KPrefabID>();
		choreConsumer = GetComponent<ChoreConsumer>();
		running = true;
		Components.Brains.Add(this);
	}

	public virtual void UpdateBrain()
	{
		if (this.onPreUpdate != null)
		{
			this.onPreUpdate();
		}
		if (IsRunning())
		{
			UpdateChores();
		}
	}

	private bool FindBetterChore(ref Chore.Precondition.Context context)
	{
		return choreConsumer.FindNextChore(ref context);
	}

	private void UpdateChores()
	{
		if (prefabId.HasTag(GameTags.PreventChoreInterruption))
		{
			return;
		}
		Chore.Precondition.Context context = default(Chore.Precondition.Context);
		if (FindBetterChore(ref context))
		{
			if (prefabId.HasTag(GameTags.PerformingWorkRequest))
			{
				Trigger(1485595942);
			}
			else
			{
				choreConsumer.choreDriver.SetChore(context);
			}
		}
	}

	public bool IsRunning()
	{
		if (running)
		{
			return !suspend;
		}
		return false;
	}

	public void Reset(string reason)
	{
		Stop("Reset");
		running = true;
	}

	public void Stop(string reason)
	{
		GetComponent<ChoreDriver>().StopChore();
		running = false;
	}

	public void Resume(string caller)
	{
		suspend = false;
	}

	public void Suspend(string caller)
	{
		suspend = true;
	}

	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		Stop("OnCmpDisable");
	}

	protected override void OnCleanUp()
	{
		Stop("OnCleanUp");
		Components.Brains.Remove(this);
	}
}
