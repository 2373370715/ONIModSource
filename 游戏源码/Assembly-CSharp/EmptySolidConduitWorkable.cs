using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200128E RID: 4750
[AddComponentMenu("KMonoBehaviour/Workable/EmptySolidConduitWorkable")]
public class EmptySolidConduitWorkable : Workable, IEmptyConduitWorkable
{
	// Token: 0x0600618B RID: 24971 RVA: 0x002B3BA8 File Offset: 0x002B1DA8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		base.SetWorkTime(float.PositiveInfinity);
		this.faceTargetWhenWorking = true;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		base.Subscribe<EmptySolidConduitWorkable>(2127324410, EmptySolidConduitWorkable.OnEmptyConduitCancelledDelegate);
		if (EmptySolidConduitWorkable.emptySolidConduitStatusItem == null)
		{
			EmptySolidConduitWorkable.emptySolidConduitStatusItem = new StatusItem("EmptySolidConduit", BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.NAME, BUILDINGS.PREFABS.CONDUIT.STATUS_ITEM.TOOLTIP, "status_item_empty_pipe", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.SolidConveyor.ID, 32770, true, null);
		}
		this.requiredSkillPerk = Db.Get().SkillPerks.CanDoPlumbing.Id;
		this.shouldShowSkillPerkStatusItem = false;
	}

	// Token: 0x0600618C RID: 24972 RVA: 0x000DFA80 File Offset: 0x000DDC80
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.elapsedTime != -1f)
		{
			this.MarkForEmptying();
		}
	}

	// Token: 0x0600618D RID: 24973 RVA: 0x002B3C68 File Offset: 0x002B1E68
	public void MarkForEmptying()
	{
		if (this.chore == null && this.HasContents())
		{
			StatusItem statusItem = this.GetStatusItem();
			base.GetComponent<KSelectable>().ToggleStatusItem(statusItem, true, null);
			this.CreateWorkChore();
		}
	}

	// Token: 0x0600618E RID: 24974 RVA: 0x002B3CA4 File Offset: 0x002B1EA4
	private bool HasContents()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid();
	}

	// Token: 0x0600618F RID: 24975 RVA: 0x000DFA9B File Offset: 0x000DDC9B
	private void CancelEmptying()
	{
		this.CleanUpVisualization();
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel");
			this.chore = null;
			this.shouldShowSkillPerkStatusItem = false;
			this.UpdateStatusItem(null);
		}
	}

	// Token: 0x06006190 RID: 24976 RVA: 0x002B3CDC File Offset: 0x002B1EDC
	private void CleanUpVisualization()
	{
		StatusItem statusItem = this.GetStatusItem();
		KSelectable component = base.GetComponent<KSelectable>();
		if (component != null)
		{
			component.ToggleStatusItem(statusItem, false, null);
		}
		this.elapsedTime = -1f;
		if (this.chore != null)
		{
			base.GetComponent<Prioritizable>().RemoveRef();
		}
	}

	// Token: 0x06006191 RID: 24977 RVA: 0x000DFAD0 File Offset: 0x000DDCD0
	protected override void OnCleanUp()
	{
		this.CancelEmptying();
		base.OnCleanUp();
	}

	// Token: 0x06006192 RID: 24978 RVA: 0x000D48B5 File Offset: 0x000D2AB5
	private SolidConduitFlow GetFlowManager()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x06006193 RID: 24979 RVA: 0x000DFADE File Offset: 0x000DDCDE
	private void OnEmptyConduitCancelled(object data)
	{
		this.CancelEmptying();
	}

	// Token: 0x06006194 RID: 24980 RVA: 0x000DFAE6 File Offset: 0x000DDCE6
	private StatusItem GetStatusItem()
	{
		return EmptySolidConduitWorkable.emptySolidConduitStatusItem;
	}

	// Token: 0x06006195 RID: 24981 RVA: 0x002B3D28 File Offset: 0x002B1F28
	private void CreateWorkChore()
	{
		base.GetComponent<Prioritizable>().AddRef();
		this.chore = new WorkChore<EmptySolidConduitWorkable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, Db.Get().SkillPerks.CanDoPlumbing.Id);
		this.elapsedTime = 0f;
		this.emptiedPipe = false;
		this.shouldShowSkillPerkStatusItem = true;
		this.UpdateStatusItem(null);
	}

	// Token: 0x06006196 RID: 24982 RVA: 0x002B3DB8 File Offset: 0x002B1FB8
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.elapsedTime == -1f)
		{
			return true;
		}
		bool result = false;
		this.elapsedTime += dt;
		if (!this.emptiedPipe)
		{
			if (this.elapsedTime > 4f)
			{
				this.EmptyContents();
				this.emptiedPipe = true;
				this.elapsedTime = 0f;
			}
		}
		else if (this.elapsedTime > 2f)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			if (this.GetFlowManager().GetContents(cell).pickupableHandle.IsValid())
			{
				this.elapsedTime = 0f;
				this.emptiedPipe = false;
			}
			else
			{
				this.CleanUpVisualization();
				this.chore = null;
				result = true;
				this.shouldShowSkillPerkStatusItem = false;
				this.UpdateStatusItem(null);
			}
		}
		return result;
	}

	// Token: 0x06006197 RID: 24983 RVA: 0x000DFA13 File Offset: 0x000DDC13
	public override bool InstantlyFinish(WorkerBase worker)
	{
		worker.Work(4f);
		return true;
	}

	// Token: 0x06006198 RID: 24984 RVA: 0x002B3E84 File Offset: 0x002B2084
	public void EmptyContents()
	{
		int cell_idx = Grid.PosToCell(base.transform.GetPosition());
		this.GetFlowManager().RemovePickupable(cell_idx);
		this.elapsedTime = 0f;
	}

	// Token: 0x06006199 RID: 24985 RVA: 0x000DFAED File Offset: 0x000DDCED
	public override float GetPercentComplete()
	{
		return Mathf.Clamp01(this.elapsedTime / 4f);
	}

	// Token: 0x0400457A RID: 17786
	[MyCmpReq]
	private SolidConduit conduit;

	// Token: 0x0400457B RID: 17787
	private static StatusItem emptySolidConduitStatusItem;

	// Token: 0x0400457C RID: 17788
	private Chore chore;

	// Token: 0x0400457D RID: 17789
	private const float RECHECK_PIPE_INTERVAL = 2f;

	// Token: 0x0400457E RID: 17790
	private const float TIME_TO_EMPTY_PIPE = 4f;

	// Token: 0x0400457F RID: 17791
	private const float NO_EMPTY_SCHEDULED = -1f;

	// Token: 0x04004580 RID: 17792
	[Serialize]
	private float elapsedTime = -1f;

	// Token: 0x04004581 RID: 17793
	private bool emptiedPipe = true;

	// Token: 0x04004582 RID: 17794
	private static readonly EventSystem.IntraObjectHandler<EmptySolidConduitWorkable> OnEmptyConduitCancelledDelegate = new EventSystem.IntraObjectHandler<EmptySolidConduitWorkable>(delegate(EmptySolidConduitWorkable component, object data)
	{
		component.OnEmptyConduitCancelled(data);
	});
}
