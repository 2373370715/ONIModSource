using System;
using System.Collections;

// Token: 0x0200060A RID: 1546
public class Promise : IEnumerator
{
	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06001C27 RID: 7207 RVA: 0x000B2784 File Offset: 0x000B0984
	public bool IsResolved
	{
		get
		{
			return this.m_is_resolved;
		}
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x000B278C File Offset: 0x000B098C
	public Promise(Action<System.Action> fn)
	{
		fn(delegate
		{
			this.Resolve();
		});
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x000A5E2C File Offset: 0x000A402C
	public Promise()
	{
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x000B27A6 File Offset: 0x000B09A6
	public void EnsureResolved()
	{
		if (this.IsResolved)
		{
			return;
		}
		this.Resolve();
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x000B27B7 File Offset: 0x000B09B7
	public void Resolve()
	{
		DebugUtil.Assert(!this.m_is_resolved, "Can only resolve a promise once");
		this.m_is_resolved = true;
		if (this.on_complete != null)
		{
			this.on_complete();
			this.on_complete = null;
		}
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x000B27ED File Offset: 0x000B09ED
	public Promise Then(System.Action callback)
	{
		if (this.m_is_resolved)
		{
			callback();
		}
		else
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, callback);
		}
		return this;
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x001ACCAC File Offset: 0x001AAEAC
	public Promise ThenWait(Func<Promise> callback)
	{
		if (this.m_is_resolved)
		{
			return callback();
		}
		return new Promise(delegate(System.Action resolve)
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, new System.Action(delegate()
			{
				callback().Then(resolve);
			}));
		});
	}

	// Token: 0x06001C2E RID: 7214 RVA: 0x001ACCF4 File Offset: 0x001AAEF4
	public Promise<T> ThenWait<T>(Func<Promise<T>> callback)
	{
		if (this.m_is_resolved)
		{
			return callback();
		}
		return new Promise<T>(delegate(Action<T> resolve)
		{
			this.on_complete = (System.Action)Delegate.Combine(this.on_complete, new System.Action(delegate()
			{
				callback().Then(resolve);
			}));
		});
	}

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06001C2F RID: 7215 RVA: 0x000AD332 File Offset: 0x000AB532
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001C30 RID: 7216 RVA: 0x000B2817 File Offset: 0x000B0A17
	bool IEnumerator.MoveNext()
	{
		return !this.IsResolved;
	}

	// Token: 0x06001C31 RID: 7217 RVA: 0x000A5E40 File Offset: 0x000A4040
	void IEnumerator.Reset()
	{
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x000B2822 File Offset: 0x000B0A22
	static Promise()
	{
		Promise.m_instant.Resolve();
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06001C33 RID: 7219 RVA: 0x000B2838 File Offset: 0x000B0A38
	public static Promise Instant
	{
		get
		{
			return Promise.m_instant;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06001C34 RID: 7220 RVA: 0x000B283F File Offset: 0x000B0A3F
	public static Promise Fail
	{
		get
		{
			return new Promise();
		}
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x001ACD3C File Offset: 0x001AAF3C
	public static Promise All(params Promise[] promises)
	{
		Promise.<>c__DisplayClass21_0 CS$<>8__locals1 = new Promise.<>c__DisplayClass21_0();
		CS$<>8__locals1.promises = promises;
		if (CS$<>8__locals1.promises == null || CS$<>8__locals1.promises.Length == 0)
		{
			return Promise.Instant;
		}
		CS$<>8__locals1.all_resolved_promise = new Promise();
		Promise[] promises2 = CS$<>8__locals1.promises;
		for (int i = 0; i < promises2.Length; i++)
		{
			promises2[i].Then(new System.Action(CS$<>8__locals1.<All>g__TryResolve|0));
		}
		return CS$<>8__locals1.all_resolved_promise;
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x000B2846 File Offset: 0x000B0A46
	public static Promise Chain(params Func<Promise>[] make_promise_fns)
	{
		Promise.<>c__DisplayClass22_0 CS$<>8__locals1 = new Promise.<>c__DisplayClass22_0();
		CS$<>8__locals1.make_promise_fns = make_promise_fns;
		CS$<>8__locals1.all_resolve_promise = new Promise();
		CS$<>8__locals1.current_promise_fn_index = 0;
		CS$<>8__locals1.<Chain>g__TryNext|0();
		return CS$<>8__locals1.all_resolve_promise;
	}

	// Token: 0x0400119F RID: 4511
	private System.Action on_complete;

	// Token: 0x040011A0 RID: 4512
	private bool m_is_resolved;

	// Token: 0x040011A1 RID: 4513
	private static Promise m_instant = new Promise();
}
