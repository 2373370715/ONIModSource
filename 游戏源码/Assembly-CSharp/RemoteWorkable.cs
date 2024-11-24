using System;

// Token: 0x0200174A RID: 5962
public abstract class RemoteWorkable : Workable, IRemoteDockWorkTarget
{
	// Token: 0x06007AC4 RID: 31428 RVA: 0x000F0A9E File Offset: 0x000EEC9E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RemoteDockWorkTargets.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06007AC5 RID: 31429 RVA: 0x000F0ABC File Offset: 0x000EECBC
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteDockWorkTargets.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x170007B4 RID: 1972
	// (get) Token: 0x06007AC6 RID: 31430
	public abstract Chore RemoteDockChore { get; }

	// Token: 0x170007B5 RID: 1973
	// (get) Token: 0x06007AC7 RID: 31431 RVA: 0x000B7C34 File Offset: 0x000B5E34
	public virtual IApproachable Approachable
	{
		get
		{
			return this;
		}
	}
}
