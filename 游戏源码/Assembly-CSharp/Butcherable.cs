using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001052 RID: 4178
[AddComponentMenu("KMonoBehaviour/Workable/Butcherable")]
public class Butcherable : Workable, ISaveLoadable
{
	// Token: 0x0600553D RID: 21821 RVA: 0x000D79B0 File Offset: 0x000D5BB0
	public void SetDrops(string[] drops)
	{
		this.drops = drops;
	}

	// Token: 0x0600553E RID: 21822 RVA: 0x0027D968 File Offset: 0x0027BB68
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Butcherable>(1272413801, Butcherable.SetReadyToButcherDelegate);
		base.Subscribe<Butcherable>(493375141, Butcherable.OnRefreshUserMenuDelegate);
		this.workTime = 3f;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
	}

	// Token: 0x0600553F RID: 21823 RVA: 0x000D79B9 File Offset: 0x000D5BB9
	public void SetReadyToButcher(object param)
	{
		this.readyToButcher = true;
	}

	// Token: 0x06005540 RID: 21824 RVA: 0x000D79C2 File Offset: 0x000D5BC2
	public void SetReadyToButcher(bool ready)
	{
		this.readyToButcher = ready;
	}

	// Token: 0x06005541 RID: 21825 RVA: 0x0027D9C8 File Offset: 0x0027BBC8
	public void ActivateChore(object param)
	{
		if (this.chore != null)
		{
			return;
		}
		this.chore = new WorkChore<Butcherable>(Db.Get().ChoreTypes.Harvest, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x06005542 RID: 21826 RVA: 0x000D79CB File Offset: 0x000D5BCB
	public void CancelChore(object param)
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x06005543 RID: 21827 RVA: 0x000D79ED File Offset: 0x000D5BED
	private void OnClickCancel()
	{
		this.CancelChore(null);
	}

	// Token: 0x06005544 RID: 21828 RVA: 0x000D79F6 File Offset: 0x000D5BF6
	private void OnClickButcher()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnButcherComplete();
			return;
		}
		this.ActivateChore(null);
	}

	// Token: 0x06005545 RID: 21829 RVA: 0x0027DA14 File Offset: 0x0027BC14
	private void OnRefreshUserMenu(object data)
	{
		if (!this.readyToButcher)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_harvest", "Cancel Meatify", new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, "", true) : new KIconButtonMenu.ButtonInfo("action_harvest", "Meatify", new System.Action(this.OnClickButcher), global::Action.NumActions, null, null, null, "", true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06005546 RID: 21830 RVA: 0x000D7A0D File Offset: 0x000D5C0D
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.OnButcherComplete();
	}

	// Token: 0x06005547 RID: 21831 RVA: 0x0027DAA4 File Offset: 0x0027BCA4
	public GameObject[] CreateDrops()
	{
		GameObject[] array = new GameObject[this.drops.Length];
		for (int i = 0; i < this.drops.Length; i++)
		{
			GameObject gameObject = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, this.drops[i], Grid.SceneLayer.Ore);
			gameObject.SetActive(true);
			Edible component = gameObject.GetComponent<Edible>();
			if (component)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
			}
			array[i] = gameObject;
		}
		return array;
	}

	// Token: 0x06005548 RID: 21832 RVA: 0x0027DB3C File Offset: 0x0027BD3C
	public void OnButcherComplete()
	{
		if (this.butchered)
		{
			return;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component && component.IsSelected)
		{
			SelectTool.Instance.Select(null, false);
		}
		Pickupable component2 = base.GetComponent<Pickupable>();
		Storage storage = (component2 != null) ? component2.storage : null;
		GameObject[] array = this.CreateDrops();
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (storage != null && storage.storeDropsFromButcherables)
				{
					storage.Store(array[i], false, false, true, false);
				}
			}
		}
		this.chore = null;
		this.butchered = true;
		this.readyToButcher = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
		base.Trigger(395373363, array);
	}

	// Token: 0x06005549 RID: 21833 RVA: 0x0027DC04 File Offset: 0x0027BE04
	private int GetDropSpawnLocation()
	{
		int num = Grid.PosToCell(base.gameObject);
		int num2 = Grid.CellAbove(num);
		if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
		{
			return num2;
		}
		return num;
	}

	// Token: 0x04003BC5 RID: 15301
	[MyCmpGet]
	private KAnimControllerBase controller;

	// Token: 0x04003BC6 RID: 15302
	[MyCmpGet]
	private Harvestable harvestable;

	// Token: 0x04003BC7 RID: 15303
	private bool readyToButcher;

	// Token: 0x04003BC8 RID: 15304
	private bool butchered;

	// Token: 0x04003BC9 RID: 15305
	public string[] drops;

	// Token: 0x04003BCA RID: 15306
	private Chore chore;

	// Token: 0x04003BCB RID: 15307
	private static readonly EventSystem.IntraObjectHandler<Butcherable> SetReadyToButcherDelegate = new EventSystem.IntraObjectHandler<Butcherable>(delegate(Butcherable component, object data)
	{
		component.SetReadyToButcher(data);
	});

	// Token: 0x04003BCC RID: 15308
	private static readonly EventSystem.IntraObjectHandler<Butcherable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Butcherable>(delegate(Butcherable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
