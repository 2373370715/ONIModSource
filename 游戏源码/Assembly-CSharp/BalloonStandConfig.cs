using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000438 RID: 1080
public class BalloonStandConfig : IEntityConfig
{
	// Token: 0x06001270 RID: 4720 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x00188CCC File Offset: 0x00186ECC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(BalloonStandConfig.ID, BalloonStandConfig.ID, false);
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_balloon_receiver_kanim")
		};
		GetBalloonWorkable getBalloonWorkable = gameObject.AddOrGet<GetBalloonWorkable>();
		getBalloonWorkable.workTime = 2f;
		getBalloonWorkable.workLayer = Grid.SceneLayer.BuildingFront;
		getBalloonWorkable.overrideAnims = overrideAnims;
		getBalloonWorkable.synchronizeAnims = false;
		return gameObject;
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x00188D2C File Offset: 0x00186F2C
	public void OnSpawn(GameObject inst)
	{
		GetBalloonWorkable component = inst.GetComponent<GetBalloonWorkable>();
		WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, component, null, true, new Action<Chore>(this.MakeNewBalloonChore), null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, true, true);
		workChore.AddPrecondition(this.HasNoBalloon, workChore);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		component.GetBalloonArtist().NextBalloonOverride();
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x00188DAC File Offset: 0x00186FAC
	private void MakeNewBalloonChore(Chore chore)
	{
		GetBalloonWorkable component = chore.target.GetComponent<GetBalloonWorkable>();
		WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, component, null, true, new Action<Chore>(this.MakeNewBalloonChore), null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, true, true);
		workChore.AddPrecondition(this.HasNoBalloon, workChore);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		component.GetBalloonArtist().NextBalloonOverride();
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x00188E30 File Offset: 0x00187030
	public BalloonStandConfig()
	{
		Chore.Precondition hasNoBalloon = default(Chore.Precondition);
		hasNoBalloon.id = "HasNoBalloon";
		hasNoBalloon.description = "__ Duplicant doesn't have a balloon already";
		hasNoBalloon.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !context.consumerState.gameObject.GetComponent<Effects>().HasEffect("HasBalloon");
		};
		this.HasNoBalloon = hasNoBalloon;
		base..ctor();
	}

	// Token: 0x04000C96 RID: 3222
	public static readonly string ID = "BalloonStand";

	// Token: 0x04000C97 RID: 3223
	private Chore.Precondition HasNoBalloon;
}
