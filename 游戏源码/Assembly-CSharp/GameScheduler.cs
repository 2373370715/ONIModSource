using System;
using UnityEngine;

// Token: 0x02000815 RID: 2069
[AddComponentMenu("KMonoBehaviour/scripts/GameScheduler")]
public class GameScheduler : KMonoBehaviour, IScheduler
{
	// Token: 0x0600250F RID: 9487 RVA: 0x000B8295 File Offset: 0x000B6495
	public static void DestroyInstance()
	{
		GameScheduler.Instance = null;
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x000B829D File Offset: 0x000B649D
	protected override void OnPrefabInit()
	{
		GameScheduler.Instance = this;
		Singleton<StateMachineManager>.Instance.RegisterScheduler(this.scheduler);
	}

	// Token: 0x06002511 RID: 9489 RVA: 0x000B82B5 File Offset: 0x000B64B5
	public SchedulerHandle Schedule(string name, float time, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, time, callback, callback_data, group);
	}

	// Token: 0x06002512 RID: 9490 RVA: 0x000B82C9 File Offset: 0x000B64C9
	public SchedulerHandle ScheduleNextFrame(string name, Action<object> callback, object callback_data = null, SchedulerGroup group = null)
	{
		return this.scheduler.Schedule(name, 0f, callback, callback_data, group);
	}

	// Token: 0x06002513 RID: 9491 RVA: 0x000B82E0 File Offset: 0x000B64E0
	private void Update()
	{
		this.scheduler.Update();
	}

	// Token: 0x06002514 RID: 9492 RVA: 0x000B82ED File Offset: 0x000B64ED
	protected override void OnLoadLevel()
	{
		this.scheduler.FreeResources();
		this.scheduler = null;
	}

	// Token: 0x06002515 RID: 9493 RVA: 0x000B8301 File Offset: 0x000B6501
	public SchedulerGroup CreateGroup()
	{
		return new SchedulerGroup(this.scheduler);
	}

	// Token: 0x06002516 RID: 9494 RVA: 0x000B830E File Offset: 0x000B650E
	public Scheduler GetScheduler()
	{
		return this.scheduler;
	}

	// Token: 0x04001915 RID: 6421
	private Scheduler scheduler = new Scheduler(new GameScheduler.GameSchedulerClock());

	// Token: 0x04001916 RID: 6422
	public static GameScheduler Instance;

	// Token: 0x02000816 RID: 2070
	public class GameSchedulerClock : SchedulerClock
	{
		// Token: 0x06002518 RID: 9496 RVA: 0x000B832E File Offset: 0x000B652E
		public override float GetTime()
		{
			return GameClock.Instance.GetTime();
		}
	}
}
