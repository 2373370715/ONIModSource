using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001168 RID: 4456
[AddComponentMenu("KMonoBehaviour/scripts/DrowningMonitor")]
public class DrowningMonitor : KMonoBehaviour, IWiltCause, ISlicedSim1000ms
{
	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x06005AF4 RID: 23284 RVA: 0x000DB3E8 File Offset: 0x000D95E8
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

	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x06005AF5 RID: 23285 RVA: 0x000DB40A File Offset: 0x000D960A
	public bool Drowning
	{
		get
		{
			return this.drowning;
		}
	}

	// Token: 0x06005AF6 RID: 23286 RVA: 0x00295E60 File Offset: 0x00294060
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.timeToDrown = 75f;
		if (DrowningMonitor.drowningEffect == null)
		{
			DrowningMonitor.drowningEffect = new Effect("Drowning", CREATURES.STATUSITEMS.DROWNING.NAME, CREATURES.STATUSITEMS.DROWNING.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
			DrowningMonitor.drowningEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, CREATURES.STATUSITEMS.DROWNING.NAME, false, false, true));
		}
		if (DrowningMonitor.saturatedEffect == null)
		{
			DrowningMonitor.saturatedEffect = new Effect("Saturated", CREATURES.STATUSITEMS.SATURATED.NAME, CREATURES.STATUSITEMS.SATURATED.TOOLTIP, 0f, false, false, true, null, -1f, 0f, null, "");
			DrowningMonitor.saturatedEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -100f, CREATURES.STATUSITEMS.SATURATED.NAME, false, false, true));
		}
	}

	// Token: 0x06005AF7 RID: 23287 RVA: 0x00295F70 File Offset: 0x00294170
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SlicedUpdaterSim1000ms<DrowningMonitor>.instance.RegisterUpdate1000ms(this);
		this.OnMove();
		this.CheckDrowning(null);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnMove), "DrowningMonitor.OnSpawn");
	}

	// Token: 0x06005AF8 RID: 23288 RVA: 0x00295FC0 File Offset: 0x002941C0
	private void OnMove()
	{
		if (this.partitionerEntry.IsValid())
		{
			Extents ext = this.occupyArea.GetExtents();
			GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, ext);
		}
		else
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("DrowningMonitor.OnSpawn", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.liquidChangedLayer, new Action<object>(this.OnLiquidChanged));
		}
		this.CheckDrowning(null);
	}

	// Token: 0x06005AF9 RID: 23289 RVA: 0x000DB412 File Offset: 0x000D9612
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnMove));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		SlicedUpdaterSim1000ms<DrowningMonitor>.instance.UnregisterUpdate1000ms(this);
		base.OnCleanUp();
	}

	// Token: 0x06005AFA RID: 23290 RVA: 0x0029603C File Offset: 0x0029423C
	private void CheckDrowning(object data = null)
	{
		if (this.drowned)
		{
			return;
		}
		int cell = Grid.PosToCell(base.gameObject.transform.GetPosition());
		if (!this.IsCellSafe(cell))
		{
			if (!this.drowning)
			{
				this.drowning = true;
				base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Drowning, false);
				base.Trigger(1949704522, null);
			}
			if (this.timeToDrown <= 0f && this.canDrownToDeath)
			{
				DeathMonitor.Instance smi = this.GetSMI<DeathMonitor.Instance>();
				if (smi != null)
				{
					smi.Kill(Db.Get().Deaths.Drowned);
				}
				base.Trigger(-750750377, null);
				this.drowned = true;
			}
		}
		else if (this.drowning)
		{
			this.drowning = false;
			base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Drowning);
			base.Trigger(99949694, null);
		}
		if (this.livesUnderWater)
		{
			this.saturatedStatusGuid = this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Saturated, this.saturatedStatusGuid, this.drowning, this);
		}
		else
		{
			this.drowningStatusGuid = this.selectable.ToggleStatusItem(Db.Get().CreatureStatusItems.Drowning, this.drowningStatusGuid, this.drowning, this);
		}
		if (this.effects != null)
		{
			if (this.drowning)
			{
				if (this.livesUnderWater)
				{
					this.effects.Add(DrowningMonitor.saturatedEffect, false);
					return;
				}
				this.effects.Add(DrowningMonitor.drowningEffect, false);
				return;
			}
			else
			{
				if (this.livesUnderWater)
				{
					this.effects.Remove(DrowningMonitor.saturatedEffect);
					return;
				}
				this.effects.Remove(DrowningMonitor.drowningEffect);
			}
		}
	}

	// Token: 0x06005AFB RID: 23291 RVA: 0x000DB451 File Offset: 0x000D9651
	private static bool CellSafeTest(int testCell, object data)
	{
		return !Grid.IsNavigatableLiquid(testCell);
	}

	// Token: 0x06005AFC RID: 23292 RVA: 0x000DB45C File Offset: 0x000D965C
	public bool IsCellSafe(int cell)
	{
		return this.occupyArea.TestArea(cell, this, DrowningMonitor.CellSafeTestDelegate);
	}

	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x06005AFD RID: 23293 RVA: 0x000DB470 File Offset: 0x000D9670
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Drowning
			};
		}
	}

	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x06005AFE RID: 23294 RVA: 0x000DB47C File Offset: 0x000D967C
	public string WiltStateString
	{
		get
		{
			if (this.livesUnderWater)
			{
				return "    • " + CREATURES.STATUSITEMS.SATURATED.NAME;
			}
			return "    • " + CREATURES.STATUSITEMS.DROWNING.NAME;
		}
	}

	// Token: 0x06005AFF RID: 23295 RVA: 0x000DB4AF File Offset: 0x000D96AF
	private void OnLiquidChanged(object data)
	{
		this.CheckDrowning(null);
	}

	// Token: 0x06005B00 RID: 23296 RVA: 0x002961E4 File Offset: 0x002943E4
	public void SlicedSim1000ms(float dt)
	{
		this.CheckDrowning(null);
		if (this.drowning)
		{
			if (!this.drowned)
			{
				this.timeToDrown -= dt;
				if (this.timeToDrown <= 0f)
				{
					this.CheckDrowning(null);
					return;
				}
			}
		}
		else
		{
			this.timeToDrown += dt * 5f;
			this.timeToDrown = Mathf.Clamp(this.timeToDrown, 0f, 75f);
		}
	}

	// Token: 0x04004025 RID: 16421
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04004026 RID: 16422
	[MyCmpGet]
	private Effects effects;

	// Token: 0x04004027 RID: 16423
	private OccupyArea _occupyArea;

	// Token: 0x04004028 RID: 16424
	[Serialize]
	[SerializeField]
	private float timeToDrown;

	// Token: 0x04004029 RID: 16425
	[Serialize]
	private bool drowned;

	// Token: 0x0400402A RID: 16426
	private bool drowning;

	// Token: 0x0400402B RID: 16427
	protected const float MaxDrownTime = 75f;

	// Token: 0x0400402C RID: 16428
	protected const float RegenRate = 5f;

	// Token: 0x0400402D RID: 16429
	protected const float CellLiquidThreshold = 0.95f;

	// Token: 0x0400402E RID: 16430
	public bool canDrownToDeath = true;

	// Token: 0x0400402F RID: 16431
	public bool livesUnderWater;

	// Token: 0x04004030 RID: 16432
	private Guid drowningStatusGuid;

	// Token: 0x04004031 RID: 16433
	private Guid saturatedStatusGuid;

	// Token: 0x04004032 RID: 16434
	private Extents extents;

	// Token: 0x04004033 RID: 16435
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04004034 RID: 16436
	public static Effect drowningEffect;

	// Token: 0x04004035 RID: 16437
	public static Effect saturatedEffect;

	// Token: 0x04004036 RID: 16438
	private static readonly Func<int, object, bool> CellSafeTestDelegate = (int testCell, object data) => DrowningMonitor.CellSafeTest(testCell, data);
}
