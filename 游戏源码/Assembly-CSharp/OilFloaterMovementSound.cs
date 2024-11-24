using System;

// Token: 0x020000F5 RID: 245
internal class OilFloaterMovementSound : KMonoBehaviour
{
	// Token: 0x060003D8 RID: 984 RVA: 0x00152AF0 File Offset: 0x00150CF0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.sound = GlobalAssets.GetSound(this.sound, false);
		base.Subscribe<OilFloaterMovementSound>(1027377649, OilFloaterMovementSound.OnObjectMovementStateChangedDelegate);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "OilFloaterMovementSound");
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x00152B48 File Offset: 0x00150D48
	private void OnObjectMovementStateChanged(object data)
	{
		GameHashes gameHashes = (GameHashes)data;
		this.isMoving = (gameHashes == GameHashes.ObjectMovementWakeUp);
		this.UpdateSound();
	}

	// Token: 0x060003DA RID: 986 RVA: 0x000A72FE File Offset: 0x000A54FE
	private void OnCellChanged()
	{
		this.UpdateSound();
	}

	// Token: 0x060003DB RID: 987 RVA: 0x00152B70 File Offset: 0x00150D70
	private void UpdateSound()
	{
		bool flag = this.isMoving && base.GetComponent<Navigator>().CurrentNavType != NavType.Swim;
		if (flag == this.isPlayingSound)
		{
			return;
		}
		LoopingSounds component = base.GetComponent<LoopingSounds>();
		if (flag)
		{
			component.StartSound(this.sound);
		}
		else
		{
			component.StopSound(this.sound);
		}
		this.isPlayingSound = flag;
	}

	// Token: 0x060003DC RID: 988 RVA: 0x000A7306 File Offset: 0x000A5506
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
	}

	// Token: 0x040002AB RID: 683
	public string sound;

	// Token: 0x040002AC RID: 684
	public bool isPlayingSound;

	// Token: 0x040002AD RID: 685
	public bool isMoving;

	// Token: 0x040002AE RID: 686
	private static readonly EventSystem.IntraObjectHandler<OilFloaterMovementSound> OnObjectMovementStateChangedDelegate = new EventSystem.IntraObjectHandler<OilFloaterMovementSound>(delegate(OilFloaterMovementSound component, object data)
	{
		component.OnObjectMovementStateChanged(data);
	});
}
