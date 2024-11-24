using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000BD4 RID: 3028
public class StatusItemStackTraceWatcher : IDisposable
{
	// Token: 0x060039FA RID: 14842 RVA: 0x000C56B7 File Offset: 0x000C38B7
	public bool GetShouldWatch()
	{
		return this.shouldWatch;
	}

	// Token: 0x060039FB RID: 14843 RVA: 0x000C56BF File Offset: 0x000C38BF
	public void SetShouldWatch(bool shouldWatch)
	{
		if (this.shouldWatch == shouldWatch)
		{
			return;
		}
		this.shouldWatch = shouldWatch;
		this.Refresh();
	}

	// Token: 0x060039FC RID: 14844 RVA: 0x000C56D8 File Offset: 0x000C38D8
	public Option<StatusItemGroup> GetTarget()
	{
		return this.currentTarget;
	}

	// Token: 0x060039FD RID: 14845 RVA: 0x00225CD8 File Offset: 0x00223ED8
	public void SetTarget(Option<StatusItemGroup> nextTarget)
	{
		if (this.currentTarget.IsNone() && nextTarget.IsNone())
		{
			return;
		}
		if (this.currentTarget.IsSome() && nextTarget.IsSome() && this.currentTarget.Unwrap() == nextTarget.Unwrap())
		{
			return;
		}
		this.currentTarget = nextTarget;
		this.Refresh();
	}

	// Token: 0x060039FE RID: 14846 RVA: 0x00225D34 File Offset: 0x00223F34
	private void Refresh()
	{
		if (this.onCleanup != null)
		{
			System.Action action = this.onCleanup;
			if (action != null)
			{
				action();
			}
			this.onCleanup = null;
		}
		if (!this.shouldWatch)
		{
			return;
		}
		if (this.currentTarget.IsSome())
		{
			StatusItemGroup target = this.currentTarget.Unwrap();
			Action<StatusItemGroup.Entry, StatusItemCategory> onAddStatusItem = delegate(StatusItemGroup.Entry entry, StatusItemCategory category)
			{
				this.entryIdToStackTraceMap[entry.id] = new StackTrace(true);
			};
			StatusItemGroup target3 = target;
			target3.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Combine(target3.OnAddStatusItem, onAddStatusItem);
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				StatusItemGroup target2 = target;
				target2.OnAddStatusItem = (Action<StatusItemGroup.Entry, StatusItemCategory>)Delegate.Remove(target2.OnAddStatusItem, onAddStatusItem);
			}));
			StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB destroyListener = this.currentTarget.Unwrap().gameObject.AddOrGet<StatusItemStackTraceWatcher.StatusItemStackTraceWatcher_OnDestroyListenerMB>();
			destroyListener.owner = this;
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				if (destroyListener.IsNullOrDestroyed())
				{
					return;
				}
				UnityEngine.Object.Destroy(destroyListener);
			}));
			this.onCleanup = (System.Action)Delegate.Combine(this.onCleanup, new System.Action(delegate()
			{
				this.entryIdToStackTraceMap.Clear();
			}));
		}
	}

	// Token: 0x060039FF RID: 14847 RVA: 0x000C56E0 File Offset: 0x000C38E0
	public bool GetStackTraceForEntry(StatusItemGroup.Entry entry, out StackTrace stackTrace)
	{
		return this.entryIdToStackTraceMap.TryGetValue(entry.id, out stackTrace);
	}

	// Token: 0x06003A00 RID: 14848 RVA: 0x000C56F4 File Offset: 0x000C38F4
	public void Dispose()
	{
		if (this.onCleanup != null)
		{
			System.Action action = this.onCleanup;
			if (action != null)
			{
				action();
			}
			this.onCleanup = null;
		}
	}

	// Token: 0x04002795 RID: 10133
	private Dictionary<Guid, StackTrace> entryIdToStackTraceMap = new Dictionary<Guid, StackTrace>();

	// Token: 0x04002796 RID: 10134
	private Option<StatusItemGroup> currentTarget;

	// Token: 0x04002797 RID: 10135
	private bool shouldWatch;

	// Token: 0x04002798 RID: 10136
	private System.Action onCleanup;

	// Token: 0x02000BD5 RID: 3029
	public class StatusItemStackTraceWatcher_OnDestroyListenerMB : MonoBehaviour
	{
		// Token: 0x06003A04 RID: 14852 RVA: 0x00225E54 File Offset: 0x00224054
		private void OnDestroy()
		{
			bool flag = this.owner != null;
			bool flag2 = this.owner.currentTarget.IsSome() && this.owner.currentTarget.Unwrap().gameObject == base.gameObject;
			if (flag && flag2)
			{
				this.owner.SetTarget(Option.None);
			}
		}

		// Token: 0x04002799 RID: 10137
		public StatusItemStackTraceWatcher owner;
	}
}
