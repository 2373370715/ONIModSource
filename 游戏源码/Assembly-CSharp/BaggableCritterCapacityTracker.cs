using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200099B RID: 2459
public class BaggableCritterCapacityTracker : KMonoBehaviour, ISim1000ms, IUserControlledCapacity
{
	// Token: 0x17000199 RID: 409
	// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x000BCE27 File Offset: 0x000BB027
	// (set) Token: 0x06002CA3 RID: 11427 RVA: 0x000BCE2F File Offset: 0x000BB02F
	[Serialize]
	public int creatureLimit { get; set; } = 20;

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x06002CA4 RID: 11428 RVA: 0x000BCE38 File Offset: 0x000BB038
	// (set) Token: 0x06002CA5 RID: 11429 RVA: 0x000BCE40 File Offset: 0x000BB040
	public int storedCreatureCount { get; private set; }

	// Token: 0x06002CA6 RID: 11430 RVA: 0x001EC654 File Offset: 0x001EA854
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(this);
		this.cavityCell = Grid.OffsetCell(cell, this.cavityOffset);
		this.filter = base.GetComponent<TreeFilterable>();
		TreeFilterable treeFilterable = this.filter;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Combine(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.RefreshCreatureCount));
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
		base.Subscribe(144050788, new Action<object>(this.RefreshCreatureCount));
	}

	// Token: 0x06002CA7 RID: 11431 RVA: 0x001EC6E4 File Offset: 0x001EA8E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (BaggableCritterCapacityTracker.capacityStatusItem == null)
		{
			BaggableCritterCapacityTracker.capacityStatusItem = new StatusItem("CritterCapacity", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			BaggableCritterCapacityTracker.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				IUserControlledCapacity userControlledCapacity = (IUserControlledCapacity)data;
				string newValue = Util.FormatWholeNumber(Mathf.Floor(userControlledCapacity.AmountStored));
				string newValue2 = Util.FormatWholeNumber(userControlledCapacity.UserMaxCapacity);
				str = str.Replace("{Stored}", newValue).Replace("{StoredUnits}", ((int)userControlledCapacity.AmountStored == 1) ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS).Replace("{Capacity}", newValue2).Replace("{CapacityUnits}", ((int)userControlledCapacity.UserMaxCapacity == 1) ? BUILDING.STATUSITEMS.CRITTERCAPACITY.UNIT : BUILDING.STATUSITEMS.CRITTERCAPACITY.UNITS);
				return str;
			};
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, BaggableCritterCapacityTracker.capacityStatusItem, this);
	}

	// Token: 0x06002CA8 RID: 11432 RVA: 0x000BCE49 File Offset: 0x000BB049
	protected override void OnCleanUp()
	{
		TreeFilterable treeFilterable = this.filter;
		treeFilterable.OnFilterChanged = (Action<HashSet<Tag>>)Delegate.Remove(treeFilterable.OnFilterChanged, new Action<HashSet<Tag>>(this.RefreshCreatureCount));
		base.Unsubscribe(144050788);
		base.OnCleanUp();
	}

	// Token: 0x06002CA9 RID: 11433 RVA: 0x001EC770 File Offset: 0x001EA970
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		BaggableCritterCapacityTracker component = gameObject.GetComponent<BaggableCritterCapacityTracker>();
		if (component == null)
		{
			return;
		}
		this.creatureLimit = component.creatureLimit;
	}

	// Token: 0x06002CAA RID: 11434 RVA: 0x001EC7AC File Offset: 0x001EA9AC
	public void RefreshCreatureCount(object data = null)
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(this.cavityCell);
		int storedCreatureCount = this.storedCreatureCount;
		this.storedCreatureCount = 0;
		if (cavityForCell != null)
		{
			foreach (KPrefabID kprefabID in cavityForCell.creatures)
			{
				if (!kprefabID.HasTag(GameTags.Creatures.Bagged) && !kprefabID.HasTag(GameTags.Trapped) && (!this.filteredCount || this.filter.AcceptedTags.Contains(kprefabID.PrefabTag)))
				{
					int storedCreatureCount2 = this.storedCreatureCount;
					this.storedCreatureCount = storedCreatureCount2 + 1;
				}
			}
		}
		if (this.onCountChanged != null && this.storedCreatureCount != storedCreatureCount)
		{
			this.onCountChanged();
		}
	}

	// Token: 0x06002CAB RID: 11435 RVA: 0x000BCE83 File Offset: 0x000BB083
	public void Sim1000ms(float dt)
	{
		this.RefreshCreatureCount(null);
	}

	// Token: 0x1700019B RID: 411
	// (get) Token: 0x06002CAC RID: 11436 RVA: 0x000BCE8C File Offset: 0x000BB08C
	// (set) Token: 0x06002CAD RID: 11437 RVA: 0x000BCE95 File Offset: 0x000BB095
	float IUserControlledCapacity.UserMaxCapacity
	{
		get
		{
			return (float)this.creatureLimit;
		}
		set
		{
			this.creatureLimit = Mathf.RoundToInt(value);
			if (this.onCountChanged != null)
			{
				this.onCountChanged();
			}
		}
	}

	// Token: 0x1700019C RID: 412
	// (get) Token: 0x06002CAE RID: 11438 RVA: 0x000BCEB6 File Offset: 0x000BB0B6
	float IUserControlledCapacity.AmountStored
	{
		get
		{
			return (float)this.storedCreatureCount;
		}
	}

	// Token: 0x1700019D RID: 413
	// (get) Token: 0x06002CAF RID: 11439 RVA: 0x000BCEBF File Offset: 0x000BB0BF
	float IUserControlledCapacity.MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700019E RID: 414
	// (get) Token: 0x06002CB0 RID: 11440 RVA: 0x000BCEC6 File Offset: 0x000BB0C6
	float IUserControlledCapacity.MaxCapacity
	{
		get
		{
			return (float)this.maximumCreatures;
		}
	}

	// Token: 0x1700019F RID: 415
	// (get) Token: 0x06002CB1 RID: 11441 RVA: 0x000A65EC File Offset: 0x000A47EC
	bool IUserControlledCapacity.WholeValues
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001A0 RID: 416
	// (get) Token: 0x06002CB2 RID: 11442 RVA: 0x000BCECF File Offset: 0x000BB0CF
	LocString IUserControlledCapacity.CapacityUnits
	{
		get
		{
			return UI.UISIDESCREENS.CAPTURE_POINT_SIDE_SCREEN.UNITS_SUFFIX;
		}
	}

	// Token: 0x04001E00 RID: 7680
	public int maximumCreatures = 40;

	// Token: 0x04001E01 RID: 7681
	public CellOffset cavityOffset;

	// Token: 0x04001E02 RID: 7682
	public bool filteredCount;

	// Token: 0x04001E03 RID: 7683
	public System.Action onCountChanged;

	// Token: 0x04001E04 RID: 7684
	private int cavityCell;

	// Token: 0x04001E05 RID: 7685
	[MyCmpReq]
	private TreeFilterable filter;

	// Token: 0x04001E06 RID: 7686
	private static StatusItem capacityStatusItem;
}
