using System;
using KSerialization;
using STRINGS;

// Token: 0x0200117D RID: 4477
public class EntombVulnerable : KMonoBehaviour, IWiltCause
{
	// Token: 0x1700056B RID: 1387
	// (get) Token: 0x06005B55 RID: 23381 RVA: 0x000DB8ED File Offset: 0x000D9AED
	private OccupyArea occupyArea
	{
		get
		{
			if (this._occupyArea == null)
			{
				this._occupyArea = base.GetComponent<OccupyArea>();
			}
			return this._occupyArea;
		}
	}

	// Token: 0x1700056C RID: 1388
	// (get) Token: 0x06005B56 RID: 23382 RVA: 0x000DB90F File Offset: 0x000D9B0F
	public bool GetEntombed
	{
		get
		{
			return this.isEntombed;
		}
	}

	// Token: 0x06005B57 RID: 23383 RVA: 0x00297360 File Offset: 0x00295560
	public void SetStatusItem(StatusItem si)
	{
		bool flag = this.showStatusItemOnEntombed;
		this.SetShowStatusItemOnEntombed(false);
		this.EntombedStatusItem = si;
		this.SetShowStatusItemOnEntombed(flag);
	}

	// Token: 0x06005B58 RID: 23384 RVA: 0x0029738C File Offset: 0x0029558C
	public void SetShowStatusItemOnEntombed(bool val)
	{
		this.showStatusItemOnEntombed = val;
		if (this.isEntombed && this.EntombedStatusItem != null)
		{
			if (this.showStatusItemOnEntombed)
			{
				this.selectable.AddStatusItem(this.EntombedStatusItem, null);
				return;
			}
			this.selectable.RemoveStatusItem(this.EntombedStatusItem, false);
		}
	}

	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x06005B59 RID: 23385 RVA: 0x000DB917 File Offset: 0x000D9B17
	public string WiltStateString
	{
		get
		{
			return Db.Get().CreatureStatusItems.Entombed.resolveStringCallback(CREATURES.STATUSITEMS.ENTOMBED.LINE_ITEM, base.gameObject);
		}
	}

	// Token: 0x1700056E RID: 1390
	// (get) Token: 0x06005B5A RID: 23386 RVA: 0x000DB942 File Offset: 0x000D9B42
	public WiltCondition.Condition[] Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Entombed
			};
		}
	}

	// Token: 0x06005B5B RID: 23387 RVA: 0x002973E0 File Offset: 0x002955E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.EntombedStatusItem == null)
		{
			this.EntombedStatusItem = this.DefaultEntombedStatusItem;
		}
		this.partitionerEntry = GameScenePartitioner.Instance.Add("EntombVulnerable", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnSolidChanged));
		this.CheckEntombed();
		if (this.isEntombed)
		{
			base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
			base.Trigger(-1089732772, true);
		}
	}

	// Token: 0x06005B5C RID: 23388 RVA: 0x000DB94F File Offset: 0x000D9B4F
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06005B5D RID: 23389 RVA: 0x000DB967 File Offset: 0x000D9B67
	private void OnSolidChanged(object data)
	{
		this.CheckEntombed();
	}

	// Token: 0x06005B5E RID: 23390 RVA: 0x00297474 File Offset: 0x00295674
	private void CheckEntombed()
	{
		int cell = Grid.PosToCell(base.gameObject.transform.GetPosition());
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		if (!this.IsCellSafe(cell))
		{
			if (!this.isEntombed)
			{
				this.isEntombed = true;
				if (this.showStatusItemOnEntombed)
				{
					this.selectable.AddStatusItem(this.EntombedStatusItem, base.gameObject);
				}
				base.GetComponent<KPrefabID>().AddTag(GameTags.Entombed, false);
				base.Trigger(-1089732772, true);
			}
		}
		else if (this.isEntombed)
		{
			this.isEntombed = false;
			this.selectable.RemoveStatusItem(this.EntombedStatusItem, false);
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Entombed);
			base.Trigger(-1089732772, false);
		}
		if (this.operational != null)
		{
			this.operational.SetFlag(EntombVulnerable.notEntombedFlag, !this.isEntombed);
		}
	}

	// Token: 0x06005B5F RID: 23391 RVA: 0x000DB96F File Offset: 0x000D9B6F
	public bool IsCellSafe(int cell)
	{
		return this.occupyArea.TestArea(cell, null, EntombVulnerable.IsCellSafeCBDelegate);
	}

	// Token: 0x06005B60 RID: 23392 RVA: 0x000DB983 File Offset: 0x000D9B83
	private static bool IsCellSafeCB(int cell, object data)
	{
		return Grid.IsValidCell(cell) && !Grid.Solid[cell];
	}

	// Token: 0x0400407F RID: 16511
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04004080 RID: 16512
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04004081 RID: 16513
	private OccupyArea _occupyArea;

	// Token: 0x04004082 RID: 16514
	[Serialize]
	private bool isEntombed;

	// Token: 0x04004083 RID: 16515
	private StatusItem DefaultEntombedStatusItem = Db.Get().CreatureStatusItems.Entombed;

	// Token: 0x04004084 RID: 16516
	[NonSerialized]
	private StatusItem EntombedStatusItem;

	// Token: 0x04004085 RID: 16517
	private bool showStatusItemOnEntombed = true;

	// Token: 0x04004086 RID: 16518
	public static readonly Operational.Flag notEntombedFlag = new Operational.Flag("not_entombed", Operational.Flag.Type.Functional);

	// Token: 0x04004087 RID: 16519
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04004088 RID: 16520
	private static readonly Func<int, object, bool> IsCellSafeCBDelegate = (int cell, object data) => EntombVulnerable.IsCellSafeCB(cell, data);
}
