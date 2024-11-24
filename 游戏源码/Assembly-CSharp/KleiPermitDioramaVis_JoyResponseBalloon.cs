using System;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02001D62 RID: 7522
public class KleiPermitDioramaVis_JoyResponseBalloon : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009D15 RID: 40213 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009D16 RID: 40214 RVA: 0x003C6664 File Offset: 0x003C4864
	public void ConfigureSetup()
	{
		this.minionUI.transform.localScale = Vector3.one * 0.7f;
		this.minionUI.transform.localPosition = new Vector3(this.minionUI.transform.localPosition.x - 73f, this.minionUI.transform.localPosition.y - 152f + 8f, this.minionUI.transform.localPosition.z);
	}

	// Token: 0x06009D17 RID: 40215 RVA: 0x0010621E File Offset: 0x0010441E
	public void ConfigureWith(PermitResource permit)
	{
		this.ConfigureWith(Option.Some<BalloonArtistFacadeResource>((BalloonArtistFacadeResource)permit));
	}

	// Token: 0x06009D18 RID: 40216 RVA: 0x003C66F8 File Offset: 0x003C48F8
	public void ConfigureWith(Option<BalloonArtistFacadeResource> permit)
	{
		KleiPermitDioramaVis_JoyResponseBalloon.<>c__DisplayClass10_0 CS$<>8__locals1 = new KleiPermitDioramaVis_JoyResponseBalloon.<>c__DisplayClass10_0();
		CS$<>8__locals1.permit = permit;
		KBatchedAnimController component = this.minionUI.SpawnedAvatar.GetComponent<KBatchedAnimController>();
		CS$<>8__locals1.minionSymbolOverrider = this.minionUI.SpawnedAvatar.GetComponent<SymbolOverrideController>();
		this.minionUI.SetMinion(this.specificPersonality.UnwrapOrElse(() => (from p in Db.Get().Personalities.GetAll(true, true)
		where p.joyTrait == "BalloonArtist"
		select p).GetRandom<Personality>(), null));
		if (!this.didAddAnims)
		{
			this.didAddAnims = true;
			component.AddAnimOverrides(Assets.GetAnim("anim_interacts_balloon_artist_kanim"), 0f);
		}
		component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
		CS$<>8__locals1.<ConfigureWith>g__DisplayNextBalloon|3();
		Updater[] array = new Updater[2];
		array[0] = Updater.WaitForSeconds(1.3f);
		int num = 1;
		Func<Updater>[] array2 = new Func<Updater>[2];
		array2[0] = (() => Updater.WaitForSeconds(1.618f));
		array2[1] = (() => Updater.Do(new System.Action(base.<ConfigureWith>g__DisplayNextBalloon|3)));
		array[num] = Updater.Loop(array2);
		this.QueueUpdater(Updater.Series(array));
	}

	// Token: 0x06009D19 RID: 40217 RVA: 0x00106231 File Offset: 0x00104431
	public void SetMinion(Personality personality)
	{
		this.specificPersonality = personality;
		if (base.gameObject.activeInHierarchy)
		{
			this.minionUI.SetMinion(personality);
		}
	}

	// Token: 0x06009D1A RID: 40218 RVA: 0x00106258 File Offset: 0x00104458
	private void QueueUpdater(Updater updater)
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.RunUpdater(updater);
			return;
		}
		this.updaterToRunOnStart = updater;
	}

	// Token: 0x06009D1B RID: 40219 RVA: 0x0010627B File Offset: 0x0010447B
	private void RunUpdater(Updater updater)
	{
		if (this.updaterRoutine != null)
		{
			base.StopCoroutine(this.updaterRoutine);
			this.updaterRoutine = null;
		}
		this.updaterRoutine = base.StartCoroutine(updater);
	}

	// Token: 0x06009D1C RID: 40220 RVA: 0x001062AA File Offset: 0x001044AA
	private void OnEnable()
	{
		if (this.updaterToRunOnStart.IsSome())
		{
			this.RunUpdater(this.updaterToRunOnStart.Unwrap());
			this.updaterToRunOnStart = Option.None;
		}
	}

	// Token: 0x04007B18 RID: 31512
	private const int FRAMES_TO_MAKE_BALLOON_IN_ANIM = 39;

	// Token: 0x04007B19 RID: 31513
	private const float SECONDS_TO_MAKE_BALLOON_IN_ANIM = 1.3f;

	// Token: 0x04007B1A RID: 31514
	private const float SECONDS_BETWEEN_BALLOONS = 1.618f;

	// Token: 0x04007B1B RID: 31515
	[SerializeField]
	private UIMinion minionUI;

	// Token: 0x04007B1C RID: 31516
	private bool didAddAnims;

	// Token: 0x04007B1D RID: 31517
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x04007B1E RID: 31518
	private const int TARGET_OVERRIDE_PRIORITY = 0;

	// Token: 0x04007B1F RID: 31519
	private Option<Personality> specificPersonality;

	// Token: 0x04007B20 RID: 31520
	private Option<PermitResource> lastConfiguredPermit;

	// Token: 0x04007B21 RID: 31521
	private Option<Updater> updaterToRunOnStart;

	// Token: 0x04007B22 RID: 31522
	private Coroutine updaterRoutine;
}
