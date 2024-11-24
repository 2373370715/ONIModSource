using System;
using Database;
using UnityEngine;

// Token: 0x02000DC6 RID: 3526
[AddComponentMenu("KMonoBehaviour/Workable/GetBalloonWorkable")]
public class GetBalloonWorkable : Workable
{
	// Token: 0x06004557 RID: 17751 RVA: 0x0024B714 File Offset: 0x00249914
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = null;
		this.workingStatusItem = null;
		this.workAnims = GetBalloonWorkable.GET_BALLOON_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			GetBalloonWorkable.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			GetBalloonWorkable.PST_ANIM
		};
	}

	// Token: 0x06004558 RID: 17752 RVA: 0x0024B778 File Offset: 0x00249978
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
		if (balloonOverride.animFile.IsNone())
		{
			worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", Assets.GetAnim("balloon_anim_kanim").GetData().build.GetSymbol("body"), 0);
			return;
		}
		worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", balloonOverride.symbol.Unwrap(), 0);
	}

	// Token: 0x06004559 RID: 17753 RVA: 0x0024B814 File Offset: 0x00249A14
	protected override void OnCompleteWork(WorkerBase worker)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EquippableBalloon"), worker.transform.GetPosition());
		gameObject.GetComponent<Equippable>().Assign(worker.GetComponent<MinionIdentity>());
		gameObject.GetComponent<Equippable>().isEquipped = true;
		gameObject.SetActive(true);
		base.OnCompleteWork(worker);
		BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
		this.balloonArtist.GiveBalloon(balloonOverride);
		gameObject.GetComponent<EquippableBalloon>().SetBalloonOverride(balloonOverride);
	}

	// Token: 0x0600455A RID: 17754 RVA: 0x000CCDFB File Offset: 0x000CAFFB
	public override Vector3 GetFacingTarget()
	{
		return this.balloonArtist.master.transform.GetPosition();
	}

	// Token: 0x0600455B RID: 17755 RVA: 0x000CCE12 File Offset: 0x000CB012
	public void SetBalloonArtist(BalloonArtistChore.StatesInstance chore)
	{
		this.balloonArtist = chore;
	}

	// Token: 0x0600455C RID: 17756 RVA: 0x000CCE1B File Offset: 0x000CB01B
	public BalloonArtistChore.StatesInstance GetBalloonArtist()
	{
		return this.balloonArtist;
	}

	// Token: 0x04002FD6 RID: 12246
	private static readonly HashedString[] GET_BALLOON_ANIMS = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04002FD7 RID: 12247
	private static readonly HashedString PST_ANIM = new HashedString("working_pst");

	// Token: 0x04002FD8 RID: 12248
	private BalloonArtistChore.StatesInstance balloonArtist;

	// Token: 0x04002FD9 RID: 12249
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x04002FDA RID: 12250
	private const int TARGET_OVERRIDE_PRIORITY = 0;
}
