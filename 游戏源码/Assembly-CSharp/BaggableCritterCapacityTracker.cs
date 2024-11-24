using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

public class BaggableCritterCapacityTracker : KMonoBehaviour, ISim1000ms, IUserControlledCapacity
{
	public int maximumCreatures = 40;

	public CellOffset cavityOffset;

	public bool filteredCount;

	public System.Action onCountChanged;

	private int cavityCell;

	[MyCmpReq]
	private TreeFilterable filter;

	private static StatusItem capacityStatusItem;

	[Serialize]
	public int creatureLimit { get; set; } = 20;


	public int storedCreatureCount { get; private set; }

	float IUserControlledCapacity.UserMaxCapacity
	{
		get
		{
			return creatureLimit;
		}
		set
		{
			creatureLimit = Mathf.RoundToInt(value);
			if (onCountChanged != null)
			{
				onCountChanged();
			}
		}
	}

	float IUserControlledCapacity.AmountStored => storedCreatureCount;

	float IUserControlledCapacity.MinCapacity => 0f;

	float IUserControlledCapacity.MaxCapacity => maximumCreatures;

	bool IUserControlledCapacity.WholeValues => true;

	LocString IUserControlledCapacity.CapacityUnits => UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.UNITS_SUFFIX;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(this);
		cavityCell = Grid.OffsetCell(cell, cavityOffset);
		filter = GetComponent<TreeFilterable>();
		TreeFilterable treeFilterable = filter;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(RefreshCreatureCount));
		Subscribe(-905833192, OnCopySettings);
		Subscribe(144050788, RefreshCreatureCount);
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (capacityStatusItem == null)
		{
			capacityStatusItem = new StatusItem("CritterCapacity", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID);
			capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				IUserControlledCapacity userControlledCapacity = (IUserControlledCapacity)data;
				string newValue = Util.FormatWholeNumber(Mathf.Floor(userControlledCapacity.AmountStored));
				string newValue2 = Util.FormatWholeNumber(userControlledCapacity.UserMaxCapacity);
				str = str.Replace("{Stored}", newValue).Replace("{StoredUnits}", ((int)userControlledCapacity.AmountStored == 1) ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS).Replace("{Capacity}", newValue2)
					.Replace("{CapacityUnits}", ((int)userControlledCapacity.UserMaxCapacity == 1) ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS);
				return str;
			};
		}
		GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, capacityStatusItem, this);
	}

	protected override void OnCleanUp()
	{
		TreeFilterable treeFilterable = filter;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(RefreshCreatureCount));
		Unsubscribe(144050788);
		base.OnCleanUp();
	}

	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (!(gameObject == null))
		{
			BaggableCritterCapacityTracker component = gameObject.GetComponent<BaggableCritterCapacityTracker>();
			if (!(component == null))
			{
				creatureLimit = component.creatureLimit;
			}
		}
	}

	public void RefreshCreatureCount(object data = null)
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cavityCell);
		int num = storedCreatureCount;
		storedCreatureCount = 0;
		if (cavityForCell != null)
		{
			foreach (KPrefabID creature in cavityForCell.creatures)
			{
				if (!creature.HasTag(GameTags.Creatures.Bagged) && !creature.HasTag(GameTags.Trapped) && (!filteredCount || filter.AcceptedTags.Contains(creature.PrefabTag)))
				{
					storedCreatureCount++;
				}
			}
		}
		if (onCountChanged != null && storedCreatureCount != num)
		{
			onCountChanged();
		}
	}

	public void Sim1000ms(float dt)
	{
		RefreshCreatureCount();
	}
}
