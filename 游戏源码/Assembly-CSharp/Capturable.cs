using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200113B RID: 4411
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Capturable")]
public class Capturable : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06005A31 RID: 23089 RVA: 0x000DACD9 File Offset: 0x000D8ED9
	public bool IsMarkedForCapture
	{
		get
		{
			return this.markedForCapture;
		}
	}

	// Token: 0x06005A32 RID: 23090 RVA: 0x00293CE0 File Offset: 0x00291EE0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Capturables.Add(this);
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.resetProgressOnStop = true;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.multitoolContext = "capture";
		this.multitoolHitEffectTag = "fx_capture_splash";
	}

	// Token: 0x06005A33 RID: 23091 RVA: 0x00293DA0 File Offset: 0x00291FA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Capturable>(1623392196, Capturable.OnDeathDelegate);
		base.Subscribe<Capturable>(493375141, Capturable.OnRefreshUserMenuDelegate);
		base.Subscribe<Capturable>(-1582839653, Capturable.OnTagsChangedDelegate);
		if (this.markedForCapture)
		{
			Prioritizable.AddRef(base.gameObject);
		}
		this.UpdateStatusItem();
		this.UpdateChore();
		base.SetWorkTime(10f);
	}

	// Token: 0x06005A34 RID: 23092 RVA: 0x000DACE1 File Offset: 0x000D8EE1
	protected override void OnCleanUp()
	{
		Components.Capturables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06005A35 RID: 23093 RVA: 0x00293E10 File Offset: 0x00292010
	public override Vector3 GetTargetPoint()
	{
		Vector3 result = base.transform.GetPosition();
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			result = component.bounds.center;
		}
		result.z = 0f;
		return result;
	}

	// Token: 0x06005A36 RID: 23094 RVA: 0x000DACF4 File Offset: 0x000D8EF4
	private void OnDeath(object data)
	{
		this.allowCapture = false;
		this.markedForCapture = false;
		this.UpdateChore();
	}

	// Token: 0x06005A37 RID: 23095 RVA: 0x000DAD0A File Offset: 0x000D8F0A
	private void OnTagsChanged(object data)
	{
		this.MarkForCapture(this.markedForCapture);
	}

	// Token: 0x06005A38 RID: 23096 RVA: 0x00293E58 File Offset: 0x00292058
	public void MarkForCapture(bool mark)
	{
		PrioritySetting priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
		this.MarkForCapture(mark, priority, false);
	}

	// Token: 0x06005A39 RID: 23097 RVA: 0x00293E78 File Offset: 0x00292078
	public void MarkForCapture(bool mark, PrioritySetting priority, bool updateMarkedPriority = false)
	{
		mark = (mark && this.IsCapturable());
		if (this.markedForCapture && !mark)
		{
			Prioritizable.RemoveRef(base.gameObject);
		}
		else if (!this.markedForCapture && mark)
		{
			Prioritizable.AddRef(base.gameObject);
			Prioritizable component = base.GetComponent<Prioritizable>();
			if (component)
			{
				component.SetMasterPriority(priority);
			}
		}
		else if (updateMarkedPriority && this.markedForCapture && mark)
		{
			Prioritizable component2 = base.GetComponent<Prioritizable>();
			if (component2)
			{
				component2.SetMasterPriority(priority);
			}
		}
		this.markedForCapture = mark;
		this.UpdateStatusItem();
		this.UpdateChore();
	}

	// Token: 0x06005A3A RID: 23098 RVA: 0x00293F14 File Offset: 0x00292114
	public bool IsCapturable()
	{
		return this.allowCapture && !base.gameObject.HasTag(GameTags.Trapped) && !base.gameObject.HasTag(GameTags.Stored) && !base.gameObject.HasTag(GameTags.Creatures.Bagged);
	}

	// Token: 0x06005A3B RID: 23099 RVA: 0x00293F68 File Offset: 0x00292168
	private void OnRefreshUserMenu(object data)
	{
		if (!this.IsCapturable())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.markedForCapture) ? new KIconButtonMenu.ButtonInfo("action_capture", UI.USERMENUACTIONS.CAPTURE.NAME, delegate()
		{
			this.MarkForCapture(true);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CAPTURE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_capture", UI.USERMENUACTIONS.CANCELCAPTURE.NAME, delegate()
		{
			this.MarkForCapture(false);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELCAPTURE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06005A3C RID: 23100 RVA: 0x0029400C File Offset: 0x0029220C
	private void UpdateStatusItem()
	{
		this.shouldShowSkillPerkStatusItem = this.markedForCapture;
		base.UpdateStatusItem(null);
		if (this.markedForCapture)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderCapture, this);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderCapture, false);
	}

	// Token: 0x06005A3D RID: 23101 RVA: 0x00294070 File Offset: 0x00292270
	private void UpdateChore()
	{
		if (this.markedForCapture && this.chore == null)
		{
			this.chore = new WorkChore<Capturable>(Db.Get().ChoreTypes.Capture, this, null, true, null, new Action<Chore>(this.OnChoreBegins), new Action<Chore>(this.OnChoreEnds), true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			return;
		}
		if (!this.markedForCapture && this.chore != null)
		{
			this.chore.Cancel("not marked for capture");
			this.chore = null;
		}
	}

	// Token: 0x06005A3E RID: 23102 RVA: 0x002940F8 File Offset: 0x002922F8
	private void OnChoreBegins(Chore chore)
	{
		IdleStates.Instance smi = base.gameObject.GetSMI<IdleStates.Instance>();
		if (smi != null)
		{
			smi.GoTo(smi.sm.root);
			smi.GetComponent<Navigator>().Stop(false, true);
		}
	}

	// Token: 0x06005A3F RID: 23103 RVA: 0x00294134 File Offset: 0x00292334
	private void OnChoreEnds(Chore chore)
	{
		IdleStates.Instance smi = base.gameObject.GetSMI<IdleStates.Instance>();
		if (smi != null)
		{
			smi.GoTo(smi.sm.GetDefaultState());
		}
	}

	// Token: 0x06005A40 RID: 23104 RVA: 0x000DAD18 File Offset: 0x000D8F18
	protected override void OnStartWork(WorkerBase worker)
	{
		base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Stunned, false);
	}

	// Token: 0x06005A41 RID: 23105 RVA: 0x000DAD2B File Offset: 0x000D8F2B
	protected override void OnStopWork(WorkerBase worker)
	{
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Stunned);
	}

	// Token: 0x06005A42 RID: 23106 RVA: 0x00294164 File Offset: 0x00292364
	protected override void OnCompleteWork(WorkerBase worker)
	{
		int num = this.NaturalBuildingCell();
		if (Grid.Solid[num])
		{
			int num2 = Grid.CellAbove(num);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				num = num2;
			}
		}
		this.MarkForCapture(false);
		this.baggable.SetWrangled();
		this.baggable.transform.SetPosition(Grid.CellToPosCCC(num, Grid.SceneLayer.Ore));
	}

	// Token: 0x06005A43 RID: 23107 RVA: 0x002941D0 File Offset: 0x002923D0
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.allowCapture)
		{
			descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_WRANGLE, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_WRANGLE, Descriptor.DescriptorType.Effect, false));
		}
		return descriptors;
	}

	// Token: 0x04003FA4 RID: 16292
	[MyCmpAdd]
	private Baggable baggable;

	// Token: 0x04003FA5 RID: 16293
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x04003FA6 RID: 16294
	public bool allowCapture = true;

	// Token: 0x04003FA7 RID: 16295
	[Serialize]
	private bool markedForCapture;

	// Token: 0x04003FA8 RID: 16296
	private Chore chore;

	// Token: 0x04003FA9 RID: 16297
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnDeathDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04003FAA RID: 16298
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04003FAB RID: 16299
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnTagsChanged(data);
	});
}
