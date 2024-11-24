using System;
using System.Collections;

// Token: 0x02000611 RID: 1553
public class Promise<T> : IEnumerator
{
	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x06001C45 RID: 7237 RVA: 0x000B28DD File Offset: 0x000B0ADD
	public bool IsResolved
	{
		get
		{
			return this.promise.IsResolved;
		}
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x000B28EA File Offset: 0x000B0AEA
	public Promise(Action<Action<T>> fn)
	{
		fn(delegate(T value)
		{
			this.Resolve(value);
		});
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x000B290F File Offset: 0x000B0B0F
	public Promise()
	{
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x000B2922 File Offset: 0x000B0B22
	public void EnsureResolved(T value)
	{
		this.result = value;
		this.promise.EnsureResolved();
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x000B2936 File Offset: 0x000B0B36
	public void Resolve(T value)
	{
		this.result = value;
		this.promise.Resolve();
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x001ACEE8 File Offset: 0x001AB0E8
	public Promise<T> Then(Action<T> fn)
	{
		this.promise.Then(delegate
		{
			fn(this.result);
		});
		return this;
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x000B294A File Offset: 0x000B0B4A
	public Promise ThenWait(Func<Promise> fn)
	{
		return this.promise.ThenWait(fn);
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x000B2958 File Offset: 0x000B0B58
	public Promise<T> ThenWait(Func<Promise<T>> fn)
	{
		return this.promise.ThenWait<T>(fn);
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x06001C4D RID: 7245 RVA: 0x000AD332 File Offset: 0x000AB532
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x000B2966 File Offset: 0x000B0B66
	bool IEnumerator.MoveNext()
	{
		return !this.promise.IsResolved;
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x000A5E40 File Offset: 0x000A4040
	void IEnumerator.Reset()
	{
	}

	// Token: 0x040011AF RID: 4527
	private Promise promise = new Promise();

	// Token: 0x040011B0 RID: 4528
	private T result;
}
