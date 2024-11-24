using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020019EC RID: 6636
[AddComponentMenu("KMonoBehaviour/Workable/Tinkerable")]
public class Tinkerable : Workable
{
	// Token: 0x06008A3D RID: 35389 RVA: 0x0035A898 File Offset: 0x00358A98
	public static Tinkerable MakePowerTinkerable(GameObject prefab)
	{
		RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
		roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
		Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
		tinkerable.tinkerMaterialTag = PowerControlStationConfig.TINKER_TOOLS;
		tinkerable.tinkerMaterialAmount = 1f;
		tinkerable.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
		tinkerable.onCompleteSFX = "Generator_Microchip_installed";
		tinkerable.boostSymbolNames = new string[]
		{
			"booster",
			"blue_light_bloom"
		};
		tinkerable.SetWorkTime(30f);
		tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		tinkerable.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		tinkerable.choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;
		tinkerable.choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;
		tinkerable.addedEffect = "PowerTinker";
		tinkerable.effectAttributeId = Db.Get().Attributes.Machinery.Id;
		tinkerable.effectMultiplier = 0.025f;
		tinkerable.multitoolContext = "powertinker";
		tinkerable.multitoolHitEffectTag = "fx_powertinker_splash";
		tinkerable.shouldShowSkillPerkStatusItem = false;
		prefab.AddOrGet<Storage>();
		prefab.AddOrGet<Effects>();
		prefab.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable);
		};
		return tinkerable;
	}

	// Token: 0x06008A3E RID: 35390 RVA: 0x0035AA20 File Offset: 0x00358C20
	public static Tinkerable MakeFarmTinkerable(GameObject prefab)
	{
		RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Farm.Id;
		roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
		Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
		tinkerable.tinkerMaterialTag = FarmStationConfig.TINKER_TOOLS;
		tinkerable.tinkerMaterialAmount = 1f;
		tinkerable.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
		tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		tinkerable.addedEffect = "FarmTinker";
		tinkerable.effectAttributeId = Db.Get().Attributes.Botanist.Id;
		tinkerable.effectMultiplier = 0.1f;
		tinkerable.SetWorkTime(15f);
		tinkerable.attributeConverter = Db.Get().AttributeConverters.PlantTendSpeed;
		tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		tinkerable.choreTypeTinker = Db.Get().ChoreTypes.CropTend.IdHash;
		tinkerable.choreTypeFetch = Db.Get().ChoreTypes.FarmFetch.IdHash;
		tinkerable.multitoolContext = "tend";
		tinkerable.multitoolHitEffectTag = "fx_tend_splash";
		tinkerable.shouldShowSkillPerkStatusItem = false;
		prefab.AddOrGet<Storage>();
		prefab.AddOrGet<Effects>();
		prefab.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable);
		};
		return tinkerable;
	}

	// Token: 0x06008A3F RID: 35391 RVA: 0x0035AB8C File Offset: 0x00358D8C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		base.Subscribe<Tinkerable>(-1157678353, Tinkerable.OnEffectRemovedDelegate);
		base.Subscribe<Tinkerable>(-1697596308, Tinkerable.OnStorageChangeDelegate);
		base.Subscribe<Tinkerable>(144050788, Tinkerable.OnUpdateRoomDelegate);
		base.Subscribe<Tinkerable>(-592767678, Tinkerable.OnOperationalChangedDelegate);
	}

	// Token: 0x06008A40 RID: 35392 RVA: 0x000FA815 File Offset: 0x000F8A15
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.prioritizableAdded = true;
		base.Subscribe<Tinkerable>(493375141, Tinkerable.OnRefreshUserMenuDelegate);
		this.UpdateVisual();
	}

	// Token: 0x06008A41 RID: 35393 RVA: 0x000FA846 File Offset: 0x000F8A46
	protected override void OnCleanUp()
	{
		this.UpdateMaterialReservation(false);
		if (this.updateHandle.IsValid)
		{
			this.updateHandle.ClearScheduler();
		}
		if (this.prioritizableAdded)
		{
			Prioritizable.RemoveRef(base.gameObject);
		}
		base.OnCleanUp();
	}

	// Token: 0x06008A42 RID: 35394 RVA: 0x000FA880 File Offset: 0x000F8A80
	private void OnOperationalChanged(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06008A43 RID: 35395 RVA: 0x000FA880 File Offset: 0x000F8A80
	private void OnEffectRemoved(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06008A44 RID: 35396 RVA: 0x000FA880 File Offset: 0x000F8A80
	private void OnUpdateRoom(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06008A45 RID: 35397 RVA: 0x000FA888 File Offset: 0x000F8A88
	private void OnStorageChange(object data)
	{
		if (((GameObject)data).IsPrefabID(this.tinkerMaterialTag))
		{
			this.QueueUpdateChore();
		}
	}

	// Token: 0x06008A46 RID: 35398 RVA: 0x0035AC3C File Offset: 0x00358E3C
	private void QueueUpdateChore()
	{
		if (this.updateHandle.IsValid)
		{
			this.updateHandle.ClearScheduler();
		}
		this.updateHandle = GameScheduler.Instance.Schedule("UpdateTinkerChore", 1.2f, new Action<object>(this.UpdateChoreCallback), null, null);
	}

	// Token: 0x06008A47 RID: 35399 RVA: 0x000FA8A3 File Offset: 0x000F8AA3
	private void UpdateChoreCallback(object obj)
	{
		this.UpdateChore();
	}

	// Token: 0x06008A48 RID: 35400 RVA: 0x0035AC8C File Offset: 0x00358E8C
	private void UpdateChore()
	{
		Operational component = base.GetComponent<Operational>();
		bool flag = component == null || component.IsFunctional;
		bool flag2 = this.HasEffect();
		bool flag3 = this.HasCorrectRoom();
		bool flag4 = !flag2 && flag && flag3 && this.userMenuAllowed;
		bool flag5 = flag2 || !flag3 || !this.userMenuAllowed;
		if (this.chore == null && flag4)
		{
			this.UpdateMaterialReservation(true);
			if (this.HasMaterial())
			{
				this.chore = new WorkChore<Tinkerable>(Db.Get().ChoreTypes.GetByHash(this.choreTypeTinker), this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				if (component != null)
				{
					this.chore.AddPrecondition(ChorePreconditions.instance.IsFunctional, component);
				}
			}
			else
			{
				this.chore = new FetchChore(Db.Get().ChoreTypes.GetByHash(this.choreTypeFetch), this.storage, this.tinkerMaterialAmount, new HashSet<Tag>
				{
					this.tinkerMaterialTag
				}, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, new Action<Chore>(this.OnFetchComplete), null, null, Operational.State.Functional, 0);
			}
			this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
			if (!string.IsNullOrEmpty(base.GetComponent<RoomTracker>().requiredRoomType))
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.IsInMyRoom, Grid.PosToCell(base.transform.GetPosition()));
				return;
			}
		}
		else if (this.chore != null && flag5)
		{
			this.UpdateMaterialReservation(false);
			this.chore.Cancel("No longer needed");
			this.chore = null;
		}
	}

	// Token: 0x06008A49 RID: 35401 RVA: 0x000FA8AB File Offset: 0x000F8AAB
	private bool HasCorrectRoom()
	{
		return this.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x06008A4A RID: 35402 RVA: 0x0035AE34 File Offset: 0x00359034
	private bool RoomHasTinkerstation()
	{
		if (!this.roomTracker.IsInCorrectRoom())
		{
			return false;
		}
		if (this.roomTracker.room == null)
		{
			return false;
		}
		foreach (KPrefabID kprefabID in this.roomTracker.room.buildings)
		{
			if (!(kprefabID == null))
			{
				TinkerStation component = kprefabID.GetComponent<TinkerStation>();
				if (component != null && component.outputPrefab == this.tinkerMaterialTag)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06008A4B RID: 35403 RVA: 0x0035AEDC File Offset: 0x003590DC
	private void UpdateMaterialReservation(bool shouldReserve)
	{
		if (shouldReserve && !this.hasReservedMaterial)
		{
			MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, this.tinkerMaterialAmount, base.gameObject.GetMyWorldId());
			this.hasReservedMaterial = shouldReserve;
			return;
		}
		if (!shouldReserve && this.hasReservedMaterial)
		{
			MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, -this.tinkerMaterialAmount, base.gameObject.GetMyWorldId());
			this.hasReservedMaterial = shouldReserve;
		}
	}

	// Token: 0x06008A4C RID: 35404 RVA: 0x000FA8B8 File Offset: 0x000F8AB8
	private void OnFetchComplete(Chore data)
	{
		this.UpdateMaterialReservation(false);
		this.chore = null;
		this.UpdateChore();
	}

	// Token: 0x06008A4D RID: 35405 RVA: 0x0035AF48 File Offset: 0x00359148
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.storage.ConsumeIgnoringDisease(this.tinkerMaterialTag, this.tinkerMaterialAmount);
		float totalValue = worker.GetAttributes().Get(Db.Get().Attributes.Get(this.effectAttributeId)).GetTotalValue();
		this.effects.Add(this.addedEffect, true).timeRemaining *= 1f + totalValue * this.effectMultiplier;
		this.UpdateVisual();
		this.UpdateMaterialReservation(false);
		this.chore = null;
		this.UpdateChore();
		string sound = GlobalAssets.GetSound(this.onCompleteSFX, false);
		if (sound != null)
		{
			SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, base.transform.position, 1f, false));
		}
	}

	// Token: 0x06008A4E RID: 35406 RVA: 0x0035B00C File Offset: 0x0035920C
	private void UpdateVisual()
	{
		if (this.boostSymbolNames == null)
		{
			return;
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		bool is_visible = this.effects.HasEffect(this.addedEffect);
		foreach (string str in this.boostSymbolNames)
		{
			component.SetSymbolVisiblity(str, is_visible);
		}
	}

	// Token: 0x06008A4F RID: 35407 RVA: 0x000FA8CE File Offset: 0x000F8ACE
	private bool HasMaterial()
	{
		return this.storage.GetAmountAvailable(this.tinkerMaterialTag) >= this.tinkerMaterialAmount;
	}

	// Token: 0x06008A50 RID: 35408 RVA: 0x000FA8EC File Offset: 0x000F8AEC
	private bool HasEffect()
	{
		return this.effects.HasEffect(this.addedEffect);
	}

	// Token: 0x06008A51 RID: 35409 RVA: 0x0035B064 File Offset: 0x00359264
	private void OnRefreshUserMenu(object data)
	{
		if (this.roomTracker.IsInCorrectRoom())
		{
			KIconButtonMenu.ButtonInfo button = this.userMenuAllowed ? new KIconButtonMenu.ButtonInfo("action_switch_toggle", UI.USERMENUACTIONS.TINKER.DISALLOW, new System.Action(this.OnClickToggleTinker), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.TINKER.TOOLTIP_DISALLOW, true) : new KIconButtonMenu.ButtonInfo("action_switch_toggle", UI.USERMENUACTIONS.TINKER.ALLOW, new System.Action(this.OnClickToggleTinker), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.TINKER.TOOLTIP_ALLOW, true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x06008A52 RID: 35410 RVA: 0x000FA8FF File Offset: 0x000F8AFF
	private void OnClickToggleTinker()
	{
		this.userMenuAllowed = !this.userMenuAllowed;
		this.UpdateChore();
	}

	// Token: 0x04006811 RID: 26641
	private Chore chore;

	// Token: 0x04006812 RID: 26642
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04006813 RID: 26643
	[MyCmpGet]
	private Effects effects;

	// Token: 0x04006814 RID: 26644
	[MyCmpGet]
	private RoomTracker roomTracker;

	// Token: 0x04006815 RID: 26645
	public Tag tinkerMaterialTag;

	// Token: 0x04006816 RID: 26646
	public float tinkerMaterialAmount;

	// Token: 0x04006817 RID: 26647
	public string addedEffect;

	// Token: 0x04006818 RID: 26648
	public string effectAttributeId;

	// Token: 0x04006819 RID: 26649
	public float effectMultiplier;

	// Token: 0x0400681A RID: 26650
	public string[] boostSymbolNames;

	// Token: 0x0400681B RID: 26651
	public string onCompleteSFX;

	// Token: 0x0400681C RID: 26652
	public HashedString choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;

	// Token: 0x0400681D RID: 26653
	public HashedString choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;

	// Token: 0x0400681E RID: 26654
	[Serialize]
	private bool userMenuAllowed = true;

	// Token: 0x0400681F RID: 26655
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnEffectRemovedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnEffectRemoved(data);
	});

	// Token: 0x04006820 RID: 26656
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04006821 RID: 26657
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x04006822 RID: 26658
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04006823 RID: 26659
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x04006824 RID: 26660
	private bool prioritizableAdded;

	// Token: 0x04006825 RID: 26661
	private SchedulerHandle updateHandle;

	// Token: 0x04006826 RID: 26662
	private bool hasReservedMaterial;
}
