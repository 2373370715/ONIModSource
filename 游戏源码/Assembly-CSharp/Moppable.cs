using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A9A RID: 2714
[AddComponentMenu("KMonoBehaviour/Workable/Moppable")]
public class Moppable : Workable, ISim1000ms, ISim200ms
{
	// Token: 0x06003238 RID: 12856 RVA: 0x00202BE0 File Offset: 0x00200DE0
	private Moppable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x00202C3C File Offset: 0x00200E3C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Mopping;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.childRenderer = base.GetComponentInChildren<MeshRenderer>();
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x0600323A RID: 12858 RVA: 0x00202CC0 File Offset: 0x00200EC0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!this.IsThereLiquid())
		{
			base.gameObject.DeleteObject();
			return;
		}
		Grid.Objects[Grid.PosToCell(base.gameObject), 8] = base.gameObject;
		new WorkChore<Moppable>(Db.Get().ChoreTypes.Mop, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		base.SetWorkTime(float.PositiveInfinity);
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.WaitingForMop, null);
		base.Subscribe<Moppable>(493375141, Moppable.OnRefreshUserMenuDelegate);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_mop_dirtywater_kanim")
		};
		this.partitionerEntry = GameScenePartitioner.Instance.Add("Moppable.OnSpawn", base.gameObject, new Extents(Grid.PosToCell(this), new CellOffset[]
		{
			new CellOffset(0, 0)
		}), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		this.Refresh();
		base.Subscribe<Moppable>(-1432940121, Moppable.OnReachableChangedDelegate);
		new ReachabilityMonitor.Instance(this).StartSM();
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x00202E0C File Offset: 0x0020100C
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("icon_cancel", UI.USERMENUACTIONS.CANCELMOP.NAME, new System.Action(this.OnCancel), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELMOP.TOOLTIP, true), 1f);
	}

	// Token: 0x0600323C RID: 12860 RVA: 0x000C099F File Offset: 0x000BEB9F
	private void OnCancel()
	{
		DetailsScreen.Instance.Show(false);
		base.gameObject.Trigger(2127324410, null);
	}

	// Token: 0x0600323D RID: 12861 RVA: 0x000C09BD File Offset: 0x000BEBBD
	protected override void OnStartWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Add(this, false);
		this.Refresh();
		this.MopTick(this.amountMoppedPerTick);
	}

	// Token: 0x0600323E RID: 12862 RVA: 0x000C09DD File Offset: 0x000BEBDD
	protected override void OnStopWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x0600323F RID: 12863 RVA: 0x000C09DD File Offset: 0x000BEBDD
	protected override void OnCompleteWork(WorkerBase worker)
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06003240 RID: 12864 RVA: 0x000C09EA File Offset: 0x000BEBEA
	public override bool InstantlyFinish(WorkerBase worker)
	{
		this.MopTick(1000f);
		return true;
	}

	// Token: 0x06003241 RID: 12865 RVA: 0x00202E68 File Offset: 0x00201068
	public void Sim1000ms(float dt)
	{
		if (this.amountMopped > 0f)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, GameUtil.GetFormattedMass(-this.amountMopped, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), base.transform, 1.5f, false);
			this.amountMopped = 0f;
		}
	}

	// Token: 0x06003242 RID: 12866 RVA: 0x000C09F8 File Offset: 0x000BEBF8
	public void Sim200ms(float dt)
	{
		if (base.worker != null)
		{
			this.Refresh();
			this.MopTick(this.amountMoppedPerTick);
		}
	}

	// Token: 0x06003243 RID: 12867 RVA: 0x00202EC4 File Offset: 0x002010C4
	private void OnCellMopped(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		if (this == null)
		{
			return;
		}
		if (mass_cb_info.mass > 0f)
		{
			this.amountMopped += mass_cb_info.mass;
			int cell = Grid.PosToCell(this);
			SubstanceChunk substanceChunk = LiquidSourceManager.Instance.CreateChunk(ElementLoader.elements[(int)mass_cb_info.elemIdx], mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount, Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore));
			substanceChunk.transform.SetPosition(substanceChunk.transform.GetPosition() + new Vector3((UnityEngine.Random.value - 0.5f) * 0.5f, 0f, 0f));
		}
	}

	// Token: 0x06003244 RID: 12868 RVA: 0x00202F7C File Offset: 0x0020117C
	public static void MopCell(int cell, float amount, Action<Sim.MassConsumedCallback, object> cb)
	{
		if (Grid.Element[cell].IsLiquid)
		{
			int callbackIdx = -1;
			if (cb != null)
			{
				callbackIdx = Game.Instance.massConsumedCallbackManager.Add(cb, null, "Moppable").index;
			}
			SimMessages.ConsumeMass(cell, Grid.Element[cell].id, amount, 1, callbackIdx);
		}
	}

	// Token: 0x06003245 RID: 12869 RVA: 0x00202FD0 File Offset: 0x002011D0
	private void MopTick(float mopAmount)
	{
		int cell = Grid.PosToCell(this);
		for (int i = 0; i < this.offsets.Length; i++)
		{
			int num = Grid.OffsetCell(cell, this.offsets[i]);
			if (Grid.Element[num].IsLiquid)
			{
				Moppable.MopCell(num, mopAmount, new Action<Sim.MassConsumedCallback, object>(this.OnCellMopped));
			}
		}
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x0020302C File Offset: 0x0020122C
	private bool IsThereLiquid()
	{
		int cell = Grid.PosToCell(this);
		bool result = false;
		for (int i = 0; i < this.offsets.Length; i++)
		{
			int num = Grid.OffsetCell(cell, this.offsets[i]);
			if (Grid.Element[num].IsLiquid && Grid.Mass[num] <= MopTool.maxMopAmt)
			{
				result = true;
			}
		}
		return result;
	}

	// Token: 0x06003247 RID: 12871 RVA: 0x0020308C File Offset: 0x0020128C
	private void Refresh()
	{
		if (!this.IsThereLiquid())
		{
			if (!this.destroyHandle.IsValid)
			{
				this.destroyHandle = GameScheduler.Instance.Schedule("DestroyMoppable", 1f, delegate(object moppable)
				{
					this.TryDestroy();
				}, this, null);
				return;
			}
		}
		else if (this.destroyHandle.IsValid)
		{
			this.destroyHandle.ClearScheduler();
		}
	}

	// Token: 0x06003248 RID: 12872 RVA: 0x000C0A1A File Offset: 0x000BEC1A
	private void OnLiquidChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x06003249 RID: 12873 RVA: 0x000C0A22 File Offset: 0x000BEC22
	private void TryDestroy()
	{
		if (this != null)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x0600324A RID: 12874 RVA: 0x000C0A38 File Offset: 0x000BEC38
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x0600324B RID: 12875 RVA: 0x002030F0 File Offset: 0x002012F0
	private void OnReachableChanged(object data)
	{
		if (this.childRenderer != null)
		{
			Material material = this.childRenderer.material;
			bool flag = (bool)data;
			if (material.color == Game.Instance.uiColours.Dig.invalidLocation)
			{
				return;
			}
			KSelectable component = base.GetComponent<KSelectable>();
			if (flag)
			{
				material.color = Game.Instance.uiColours.Dig.validLocation;
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.MopUnreachable, false);
				return;
			}
			component.AddStatusItem(Db.Get().BuildingStatusItems.MopUnreachable, this);
			GameScheduler.Instance.Schedule("Locomotion Tutorial", 2f, delegate(object obj)
			{
				Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, true);
			}, null, null);
			material.color = Game.Instance.uiColours.Dig.unreachable;
		}
	}

	// Token: 0x040021C8 RID: 8648
	[MyCmpReq]
	private KSelectable Selectable;

	// Token: 0x040021C9 RID: 8649
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x040021CA RID: 8650
	public float amountMoppedPerTick = 1000f;

	// Token: 0x040021CB RID: 8651
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040021CC RID: 8652
	private SchedulerHandle destroyHandle;

	// Token: 0x040021CD RID: 8653
	private float amountMopped;

	// Token: 0x040021CE RID: 8654
	private MeshRenderer childRenderer;

	// Token: 0x040021CF RID: 8655
	private CellOffset[] offsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x040021D0 RID: 8656
	private static readonly EventSystem.IntraObjectHandler<Moppable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Moppable>(delegate(Moppable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040021D1 RID: 8657
	private static readonly EventSystem.IntraObjectHandler<Moppable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Moppable>(delegate(Moppable component, object data)
	{
		component.OnReachableChanged(data);
	});
}
