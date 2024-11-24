using System;
using UnityEngine;

// Token: 0x0200081A RID: 2074
public struct SchedulerEntry
{
	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06002526 RID: 9510 RVA: 0x000B83AA File Offset: 0x000B65AA
	// (set) Token: 0x06002527 RID: 9511 RVA: 0x000B83B2 File Offset: 0x000B65B2
	public SchedulerEntry.Details details { readonly get; private set; }

	// Token: 0x06002528 RID: 9512 RVA: 0x000B83BB File Offset: 0x000B65BB
	public SchedulerEntry(string name, float time, float time_interval, Action<object> callback, object callback_data, GameObject profiler_obj)
	{
		this.time = time;
		this.details = new SchedulerEntry.Details(name, callback, callback_data, time_interval, profiler_obj);
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x000B83D7 File Offset: 0x000B65D7
	public void FreeResources()
	{
		this.details = null;
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x0600252A RID: 9514 RVA: 0x000B83E0 File Offset: 0x000B65E0
	public Action<object> callback
	{
		get
		{
			return this.details.callback;
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x0600252B RID: 9515 RVA: 0x000B83ED File Offset: 0x000B65ED
	public object callbackData
	{
		get
		{
			return this.details.callbackData;
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x0600252C RID: 9516 RVA: 0x000B83FA File Offset: 0x000B65FA
	public float timeInterval
	{
		get
		{
			return this.details.timeInterval;
		}
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x000B8407 File Offset: 0x000B6607
	public override string ToString()
	{
		return this.time.ToString();
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x000B8414 File Offset: 0x000B6614
	public void Clear()
	{
		this.details.callback = null;
	}

	// Token: 0x0400191A RID: 6426
	public float time;

	// Token: 0x0200081B RID: 2075
	public class Details
	{
		// Token: 0x0600252F RID: 9519 RVA: 0x000B8422 File Offset: 0x000B6622
		public Details(string name, Action<object> callback, object callback_data, float time_interval, GameObject profiler_obj)
		{
			this.timeInterval = time_interval;
			this.callback = callback;
			this.callbackData = callback_data;
		}

		// Token: 0x0400191C RID: 6428
		public Action<object> callback;

		// Token: 0x0400191D RID: 6429
		public object callbackData;

		// Token: 0x0400191E RID: 6430
		public float timeInterval;
	}
}
