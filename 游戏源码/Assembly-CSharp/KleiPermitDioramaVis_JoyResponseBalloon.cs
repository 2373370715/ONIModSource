using System;
using System.Linq;
using Database;
using UnityEngine;

public class KleiPermitDioramaVis_JoyResponseBalloon : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	private const int FRAMES_TO_MAKE_BALLOON_IN_ANIM = 39;

	private const float SECONDS_TO_MAKE_BALLOON_IN_ANIM = 1.3f;

	private const float SECONDS_BETWEEN_BALLOONS = 1.618f;

	[SerializeField]
	private UIMinion minionUI;

	private bool didAddAnims;

	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	private const int TARGET_OVERRIDE_PRIORITY = 0;

	private Option<Personality> specificPersonality;

	private Option<PermitResource> lastConfiguredPermit;

	private Option<Updater> updaterToRunOnStart;

	private Coroutine updaterRoutine;

	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public void ConfigureSetup()
	{
		minionUI.transform.localScale = Vector3.one * 0.7f;
		minionUI.transform.localPosition = new Vector3(minionUI.transform.localPosition.x - 73f, minionUI.transform.localPosition.y - 152f + 8f, minionUI.transform.localPosition.z);
	}

	public void ConfigureWith(PermitResource permit)
	{
		ConfigureWith(Option.Some((BalloonArtistFacadeResource)permit));
	}

	public void ConfigureWith(Option<BalloonArtistFacadeResource> permit)
	{
		KBatchedAnimController component = minionUI.SpawnedAvatar.GetComponent<KBatchedAnimController>();
		SymbolOverrideController minionSymbolOverrider = minionUI.SpawnedAvatar.GetComponent<SymbolOverrideController>();
		minionUI.SetMinion(specificPersonality.UnwrapOrElse(() => (from p in Db.Get().Personalities.GetAll(onlyEnabledMinions: true, onlyStartingMinions: true)
			where p.joyTrait == "BalloonArtist"
			select p).GetRandom()));
		if (!didAddAnims)
		{
			didAddAnims = true;
			component.AddAnimOverrides(Assets.GetAnim("anim_interacts_balloon_artist_kanim"));
		}
		component.Play("working_pre");
		component.Queue("working_loop", KAnim.PlayMode.Loop);
		DisplayNextBalloon();
		QueueUpdater(Updater.Series(Updater.WaitForSeconds(1.3f), Updater.Loop(() => Updater.WaitForSeconds(1.618f), () => Updater.Do((System.Action)DisplayNextBalloon))));
		void DisplayNextBalloon()
		{
			if (permit.IsSome())
			{
				minionSymbolOverrider.AddSymbolOverride("body", permit.Unwrap().GetNextOverride().symbol.Unwrap());
			}
			else
			{
				minionSymbolOverrider.AddSymbolOverride("body", Assets.GetAnim("balloon_anim_kanim").GetData().build.GetSymbol("body"));
			}
		}
	}

	public void SetMinion(Personality personality)
	{
		specificPersonality = personality;
		if (base.gameObject.activeInHierarchy)
		{
			minionUI.SetMinion(personality);
		}
	}

	private void QueueUpdater(Updater updater)
	{
		if (base.gameObject.activeInHierarchy)
		{
			RunUpdater(updater);
		}
		else
		{
			updaterToRunOnStart = updater;
		}
	}

	private void RunUpdater(Updater updater)
	{
		if (updaterRoutine != null)
		{
			StopCoroutine(updaterRoutine);
			updaterRoutine = null;
		}
		updaterRoutine = StartCoroutine(updater);
	}

	private void OnEnable()
	{
		if (updaterToRunOnStart.IsSome())
		{
			RunUpdater(updaterToRunOnStart.Unwrap());
			updaterToRunOnStart = Option.None;
		}
	}
}
