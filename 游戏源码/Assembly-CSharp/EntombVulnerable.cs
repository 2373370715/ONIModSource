using System;
using KSerialization;
using STRINGS;

public class EntombVulnerable : KMonoBehaviour, IWiltCause
{
	[MyCmpReq]
	private KSelectable selectable;

	[MyCmpGet]
	private Operational operational;

	private OccupyArea _occupyArea;

	[Serialize]
	private bool isEntombed;

	private StatusItem DefaultEntombedStatusItem = Db.Get().CreatureStatusItems.Entombed;

	[NonSerialized]
	private StatusItem EntombedStatusItem;

	private bool showStatusItemOnEntombed = true;

	public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);

	private HandleVector<int>.Handle partitionerEntry;

	private static readonly Func<int, object, bool> IsCellSafeCBDelegate = (int cell, object data) => IsCellSafeCB(cell, data);

	private OccupyArea occupyArea
	{
		get
		{
			if (_occupyArea == null)
			{
				_occupyArea = GetComponent<OccupyArea>();
			}
			return _occupyArea;
		}
	}

	public bool GetEntombed => isEntombed;

	public string WiltStateString => Db.Get().CreatureStatusItems.Entombed.resolveStringCallback(CREATURES.STATUSITEMS.ENTOMBED.LINE_ITEM, base.gameObject);

	public WiltCondition.Condition[] Conditions => new WiltCondition.Condition[1] { WiltCondition.Condition.Entombed };

	public void SetStatusItem(StatusItem si)
	{
		bool flag = showStatusItemOnEntombed;
		SetShowStatusItemOnEntombed(val: false);
		EntombedStatusItem = si;
		SetShowStatusItemOnEntombed(flag);
	}

	public void SetShowStatusItemOnEntombed(bool val)
	{
		showStatusItemOnEntombed = val;
		if (isEntombed && EntombedStatusItem != null)
		{
			if (showStatusItemOnEntombed)
			{
				selectable.AddStatusItem(EntombedStatusItem);
			}
			else
			{
				selectable.RemoveStatusItem(EntombedStatusItem);
			}
		}
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (EntombedStatusItem == null)
		{
			EntombedStatusItem = DefaultEntombedStatusItem;
		}
		partitionerEntry = GameScenePartitioner.Instance.Add("EntombVulnerable", base.gameObject, occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, OnSolidChanged);
		CheckEntombed();
		if (isEntombed)
		{
			GetComponent<KPrefabID>().AddTag(GameTags.Entombed);
			Trigger(-1089732772, true);
		}
	}

	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref partitionerEntry);
		base.OnCleanUp();
	}

	private void OnSolidChanged(object data)
	{
		CheckEntombed();
	}

	private void CheckEntombed()
	{
		int cell = Grid.PosToCell(base.gameObject.transform.GetPosition());
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		if (!IsCellSafe(cell))
		{
			if (!isEntombed)
			{
				isEntombed = true;
				if (showStatusItemOnEntombed)
				{
					selectable.AddStatusItem(EntombedStatusItem, base.gameObject);
				}
				GetComponent<KPrefabID>().AddTag(GameTags.Entombed);
				Trigger(-1089732772, true);
			}
		}
		else if (isEntombed)
		{
			isEntombed = false;
			selectable.RemoveStatusItem(EntombedStatusItem);
			GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
			Trigger(-1089732772, false);
		}
		if (operational != null)
		{
			operational.SetFlag(notEntombedFlag, !isEntombed);
		}
	}

	public bool IsCellSafe(int cell)
	{
		return occupyArea.TestArea(cell, null, IsCellSafeCBDelegate);
	}

	private static bool IsCellSafeCB(int cell, object data)
	{
		if (Grid.IsValidCell(cell))
		{
			return !Grid.Solid[cell];
		}
		return false;
	}
}
