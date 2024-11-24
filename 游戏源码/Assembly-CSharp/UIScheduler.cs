using System;
using UnityEngine;

// Token: 0x02002041 RID: 8257
[AddComponentMenu("KMonoBehaviour/scripts/UIScheduler")]
public class UIScheduler : KMonoBehaviour, IScheduler
{
	// Token: 0x0600AFC6 RID: 44998 RVA: 0x0011239A File Offset: 0x0011059A
	public static void DestroyInstance()
	{
		UIScheduler.Instance = null;
	}

	// Token: 0x0600AFC7 RID: 44999 RVA: 0x001123A2 File Offset: 0x001105A2
	protected override void OnPrefabInit()
	{
		UIScheduler.Instance = this;
	}

	// Token: 0x0600AFC8 RID: 45000 RVA: 0x001123AA File Offset: 0x001105AA
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x0600AFC9 RID: 45001 RVA: 0x001123BE File Offset: 0x001105BE
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x0600AFCA RID: 45002 RVA: 0x001123D5 File Offset: 0x001105D5
	private void Update()
	{
		this.scheduler.Update();
	}

	// Token: 0x0600AFCB RID: 45003 RVA: 0x001123E2 File Offset: 0x001105E2
	protected override void OnLoadLevel()
	{
		this.scheduler.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x0600AFCC RID: 45004 RVA: 0x001123F6 File Offset: 0x001105F6
	public SchedulerGroup CreateGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x0600AFCD RID: 45005 RVA: 0x00112403 File Offset: 0x00110603
	public Scheduler GetScheduler()
	{
		return this.scheduler;
	}

	// Token: 0x04008A89 RID: 35465
	private Scheduler scheduler = new Scheduler(new UIScheduler.UISchedulerClock());

	// Token: 0x04008A8A RID: 35466
	public static UIScheduler Instance;

	// Token: 0x02002042 RID: 8258
	public class UISchedulerClock : SchedulerClock
	{
		// Token: 0x0600AFCF RID: 45007 RVA: 0x00112423 File Offset: 0x00110623
		public override float GetTime()
		{
			return Time.unscaledTime;
		}
	}
}
