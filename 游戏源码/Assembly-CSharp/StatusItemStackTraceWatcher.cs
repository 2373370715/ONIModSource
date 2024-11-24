using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StatusItemStackTraceWatcher : IDisposable
{
	public class StatusItemStackTraceWatcher_OnDestroyListenerMB : MonoBehaviour
	{
		public StatusItemStackTraceWatcher owner;

		private void OnDestroy()
		{
			bool num = owner != null;
			bool flag = owner.currentTarget.IsSome() && owner.currentTarget.Unwrap().gameObject == base.gameObject;
			if (num && flag)
			{
				owner.SetTarget(Option.None);
			}
		}
	}

	private Dictionary<Guid, StackTrace> entryIdToStackTraceMap = new Dictionary<Guid, StackTrace>();

	private Option<StatusItemGroup> currentTarget;

	private bool shouldWatch;

	private System.Action onCleanup;

	public bool GetShouldWatch()
	{
		return shouldWatch;
	}

	public void SetShouldWatch(bool shouldWatch)
	{
		if (this.shouldWatch != shouldWatch)
		{
			this.shouldWatch = shouldWatch;
			Refresh();
		}
	}

	public Option<StatusItemGroup> GetTarget()
	{
		return currentTarget;
	}

	public void SetTarget(Option<StatusItemGroup> nextTarget)
	{
		if ((!currentTarget.IsNone() || !nextTarget.IsNone()) && (!currentTarget.IsSome() || !nextTarget.IsSome() || currentTarget.Unwrap() != nextTarget.Unwrap()))
		{
			currentTarget = nextTarget;
			Refresh();
		}
	}

	private void Refresh()
	{
		if (onCleanup != null)
		{
			onCleanup?.Invoke();
			onCleanup = null;
		}
		if (!shouldWatch || !currentTarget.IsSome())
		{
			return;
		}
		StatusItemGroup target = currentTarget.Unwrap();
		Action<StatusItemGroup.Entry, StatusItemCategory> onAddStatusItem = delegate(StatusItemGroup.Entry entry, StatusItemCategory category)
		{
			entryIdToStackTraceMap[entry.id] = new StackTrace(fNeedFileInfo: true);
		};
		StatusItemGroup statusItemGroup = target;
		statusItemGroup.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Combine(statusItemGroup.OnAddStatusItem, onAddStatusItem);
		onCleanup = (System.Action)Delegate.Combine(onCleanup, (System.Action)delegate
		{
			StatusItemGroup statusItemGroup2 = target;
			statusItemGroup2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Remove(statusItemGroup2.OnAddStatusItem, onAddStatusItem);
		});
		StatusItemStackTraceWatcher_OnDestroyListenerMB destroyListener = currentTarget.Unwrap().gameObject.AddOrGet<StatusItemStackTraceWatcher_OnDestroyListenerMB>();
		destroyListener.owner = this;
		onCleanup = (System.Action)Delegate.Combine(onCleanup, (System.Action)delegate
		{
			if (!destroyListener.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(destroyListener);
			}
		});
		onCleanup = (System.Action)Delegate.Combine(onCleanup, (System.Action)delegate
		{
			entryIdToStackTraceMap.Clear();
		});
	}

	public bool GetStackTraceForEntry(StatusItemGroup.Entry entry, out StackTrace stackTrace)
	{
		return entryIdToStackTraceMap.TryGetValue(entry.id, out stackTrace);
	}

	public void Dispose()
	{
		if (onCleanup != null)
		{
			onCleanup?.Invoke();
			onCleanup = null;
		}
	}
}
