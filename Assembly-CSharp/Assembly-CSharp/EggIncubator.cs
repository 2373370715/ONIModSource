﻿using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class EggIncubator : SingleEntityReceptacle, ISaveLoadable, ISim1000ms
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoReplaceEntity = true;
		this.choreType = Db.Get().ChoreTypes.RanchingFetch;
		this.statusItemNeed = Db.Get().BuildingStatusItems.NeedEgg;
		this.statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableEgg;
		this.statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingEggDelivery;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.occupyingObjectRelativePosition = new Vector3(0.5f, 1f, -1f);
		this.synchronizeAnims = false;
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("egg_target", false);
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.Subscribe<EggIncubator>(-905833192, EggIncubator.OnCopySettingsDelegate);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.occupyingObject)
		{
			if (base.occupyingObject.HasTag(GameTags.Creature))
			{
				this.storage.allowItemRemoval = true;
			}
			this.storage.RenotifyAll();
			this.PositionOccupyingObject();
		}
		base.Subscribe<EggIncubator>(-592767678, EggIncubator.OnOperationalChangedDelegate);
		base.Subscribe<EggIncubator>(-731304873, EggIncubator.OnOccupantChangedDelegate);
		base.Subscribe<EggIncubator>(-1697596308, EggIncubator.OnStorageChangeDelegate);
		this.smi = new EggIncubatorStates.Instance(this);
		this.smi.StartSM();
	}

	private void OnCopySettings(object data)
	{
		EggIncubator component = ((GameObject)data).GetComponent<EggIncubator>();
		if (component != null)
		{
			this.autoReplaceEntity = component.autoReplaceEntity;
			if (base.occupyingObject == null)
			{
				if (!(this.requestedEntityTag == component.requestedEntityTag) || !(this.requestedEntityAdditionalFilterTag == component.requestedEntityAdditionalFilterTag))
				{
					base.CancelActiveRequest();
				}
				if (this.fetchChore == null)
				{
					Tag requestedEntityTag = component.requestedEntityTag;
					this.CreateOrder(requestedEntityTag, component.requestedEntityAdditionalFilterTag);
				}
			}
			if (base.occupyingObject != null)
			{
				Prioritizable component2 = base.GetComponent<Prioritizable>();
				if (component2 != null)
				{
					Prioritizable component3 = base.occupyingObject.GetComponent<Prioritizable>();
					if (component3 != null)
					{
						component3.SetMasterPriority(component2.GetMasterPriority());
					}
				}
			}
		}
	}

	protected override void OnCleanUp()
	{
		this.smi.StopSM("cleanup");
		base.OnCleanUp();
	}

	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		if (base.occupyingObject != null)
		{
			this.tracker = base.occupyingObject.AddComponent<KBatchedAnimTracker>();
			this.tracker.symbol = "egg_target";
			this.tracker.forceAlwaysVisible = true;
		}
		this.UpdateProgress();
	}

	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		UnityEngine.Object.Destroy(this.tracker);
		this.tracker = null;
		this.UpdateProgress();
	}

	private void OnOperationalChanged(object data = null)
	{
		if (!base.occupyingObject)
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	private void OnOccupantChanged(object data = null)
	{
		if (!base.occupyingObject)
		{
			this.storage.allowItemRemoval = false;
		}
	}

	private void OnStorageChange(object data = null)
	{
		if (base.occupyingObject && !this.storage.items.Contains(base.occupyingObject))
		{
			this.UnsubscribeFromOccupant();
			this.ClearOccupant();
		}
	}

	protected override void ClearOccupant()
	{
		bool flag = false;
		if (base.occupyingObject != null)
		{
			flag = !base.occupyingObject.HasTag(GameTags.Egg);
		}
		base.ClearOccupant();
		if (this.autoReplaceEntity && flag && this.requestedEntityTag.IsValid)
		{
			this.CreateOrder(this.requestedEntityTag, Tag.Invalid);
		}
	}

	protected override void PositionOccupyingObject()
	{
		base.PositionOccupyingObject();
		base.occupyingObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
		KSelectable component = base.occupyingObject.GetComponent<KSelectable>();
		if (component != null)
		{
			component.IsSelectable = true;
		}
	}

	public override void OrderRemoveOccupant()
	{
		UnityEngine.Object.Destroy(this.tracker);
		this.tracker = null;
		this.storage.DropAll(false, false, default(Vector3), true, null);
		base.occupyingObject = null;
		this.ClearOccupant();
	}

	public float GetProgress()
	{
		float result = 0f;
		if (base.occupyingObject)
		{
			AmountInstance amountInstance = base.occupyingObject.GetAmounts().Get(Db.Get().Amounts.Incubation);
			if (amountInstance != null)
			{
				result = amountInstance.value / amountInstance.GetMax();
			}
			else
			{
				result = 1f;
			}
		}
		return result;
	}

	private void UpdateProgress()
	{
		this.meter.SetPositionPercent(this.GetProgress());
	}

	public void Sim1000ms(float dt)
	{
		this.UpdateProgress();
		this.UpdateChore();
	}

	public void StoreBaby(GameObject baby)
	{
		this.UnsubscribeFromOccupant();
		this.storage.DropAll(false, false, default(Vector3), true, null);
		this.storage.allowItemRemoval = true;
		this.storage.Store(baby, false, false, true, false);
		base.occupyingObject = baby;
		this.SubscribeToOccupant();
		base.Trigger(-731304873, base.occupyingObject);
	}

	private void UpdateChore()
	{
		if (this.operational.IsOperational && this.EggNeedsAttention())
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<EggIncubatorWorkable>(Db.Get().ChoreTypes.EggSing, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("now is not the time for song");
			this.chore = null;
		}
	}

	private bool EggNeedsAttention()
	{
		if (!base.Occupant)
		{
			return false;
		}
		IncubationMonitor.Instance instance = base.Occupant.GetSMI<IncubationMonitor.Instance>();
		return instance != null && !instance.HasSongBuff();
	}

	[MyCmpAdd]
	private EggIncubatorWorkable workable;

	[MyCmpAdd]
	private CopyBuildingSettings copySettings;

	private Chore chore;

	private EggIncubatorStates.Instance smi;

	private KBatchedAnimTracker tracker;

	private MeterController meter;

	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnOccupantChanged(data);
	});

	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnStorageChange(data);
	});

	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnCopySettings(data);
	});
}
