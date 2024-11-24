using System;

// Token: 0x0200174B RID: 5963
public abstract class RemoteDockWorkTargetComponent : KMonoBehaviour, IRemoteDockWorkTarget
{
	// Token: 0x06007AC9 RID: 31433 RVA: 0x000F0ADA File Offset: 0x000EECDA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.RemoteDockWorkTargets.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06007ACA RID: 31434 RVA: 0x000F0AF8 File Offset: 0x000EECF8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.RemoteDockWorkTargets.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x170007B6 RID: 1974
	// (get) Token: 0x06007ACB RID: 31435
	public abstract Chore RemoteDockChore { get; }

	// Token: 0x170007B7 RID: 1975
	// (get) Token: 0x06007ACC RID: 31436 RVA: 0x000F0B16 File Offset: 0x000EED16
	public virtual IApproachable Approachable
	{
		get
		{
			return base.gameObject.GetComponent<IApproachable>();
		}
	}
}
