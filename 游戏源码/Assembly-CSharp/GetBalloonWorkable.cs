using Database;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/GetBalloonWorkable")]
public class GetBalloonWorkable : Workable
{
	private static readonly HashedString[] GET_BALLOON_ANIMS = new HashedString[2] { "working_pre", "working_loop" };

	private static readonly HashedString PST_ANIM = new HashedString("working_pst");

	private BalloonArtistChore.StatesInstance balloonArtist;

	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	private const int TARGET_OVERRIDE_PRIORITY = 0;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		faceTargetWhenWorking = true;
		workerStatusItem = null;
		workingStatusItem = null;
		workAnims = GET_BALLOON_ANIMS;
		workingPstComplete = new HashedString[1] { PST_ANIM };
		workingPstFailed = new HashedString[1] { PST_ANIM };
	}

	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		BalloonOverrideSymbol balloonOverride = balloonArtist.GetBalloonOverride();
		if (balloonOverride.animFile.IsNone())
		{
			worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", Assets.GetAnim("balloon_anim_kanim").GetData().build.GetSymbol("body"));
		}
		else
		{
			worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", balloonOverride.symbol.Unwrap());
		}
	}

	protected override void OnCompleteWork(Worker worker)
	{
		GameObject obj = Util.KInstantiate(Assets.GetPrefab("EquippableBalloon"), worker.transform.GetPosition());
		obj.GetComponent<Equippable>().Assign(worker.GetComponent<MinionIdentity>());
		obj.GetComponent<Equippable>().isEquipped = true;
		obj.SetActive(value: true);
		base.OnCompleteWork(worker);
		BalloonOverrideSymbol balloonOverride = balloonArtist.GetBalloonOverride();
		balloonArtist.GiveBalloon(balloonOverride);
		obj.GetComponent<EquippableBalloon>().SetBalloonOverride(balloonOverride);
	}

	public override Vector3 GetFacingTarget()
	{
		return balloonArtist.master.transform.GetPosition();
	}

	public void SetBalloonArtist(BalloonArtistChore.StatesInstance chore)
	{
		balloonArtist = chore;
	}

	public BalloonArtistChore.StatesInstance GetBalloonArtist()
	{
		return balloonArtist;
	}
}
