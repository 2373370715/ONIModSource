using System;
using UnityEngine;

// Token: 0x02000F26 RID: 3878
[AddComponentMenu("KMonoBehaviour/scripts/Pump")]
public class Pump : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06004E39 RID: 20025 RVA: 0x000D2E23 File Offset: 0x000D1023
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.consumer.EnableConsumption(false);
	}

	// Token: 0x06004E3A RID: 20026 RVA: 0x000D2E37 File Offset: 0x000D1037
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.elapsedTime = 0f;
		this.pumpable = this.UpdateOperational();
		this.dispenser.GetConduitManager().AddConduitUpdater(new Action<float>(this.OnConduitUpdate), ConduitFlowPriority.LastPostUpdate);
	}

	// Token: 0x06004E3B RID: 20027 RVA: 0x000D2E74 File Offset: 0x000D1074
	protected override void OnCleanUp()
	{
		this.dispenser.GetConduitManager().RemoveConduitUpdater(new Action<float>(this.OnConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06004E3C RID: 20028 RVA: 0x002670A4 File Offset: 0x002652A4
	public void Sim1000ms(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime >= 1f)
		{
			this.pumpable = this.UpdateOperational();
			this.elapsedTime = 0f;
		}
		if (this.operational.IsOperational && this.pumpable)
		{
			this.operational.SetActive(true, false);
			return;
		}
		this.operational.SetActive(false, false);
	}

	// Token: 0x06004E3D RID: 20029 RVA: 0x00267114 File Offset: 0x00265314
	private bool UpdateOperational()
	{
		Element.State state = Element.State.Vacuum;
		ConduitType conduitType = this.dispenser.conduitType;
		if (conduitType != ConduitType.Gas)
		{
			if (conduitType == ConduitType.Liquid)
			{
				state = Element.State.Liquid;
			}
		}
		else
		{
			state = Element.State.Gas;
		}
		bool flag = this.IsPumpable(state, (int)this.consumer.consumptionRadius);
		StatusItem status_item = (state == Element.State.Gas) ? Db.Get().BuildingStatusItems.NoGasElementToPump : Db.Get().BuildingStatusItems.NoLiquidElementToPump;
		this.noElementStatusGuid = this.selectable.ToggleStatusItem(status_item, this.noElementStatusGuid, !flag, null);
		this.operational.SetFlag(Pump.PumpableFlag, !this.storage.IsFull() && flag);
		return flag;
	}

	// Token: 0x06004E3E RID: 20030 RVA: 0x002671B8 File Offset: 0x002653B8
	private bool IsPumpable(Element.State expected_state, int radius)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		for (int i = 0; i < (int)this.consumer.consumptionRadius; i++)
		{
			for (int j = 0; j < (int)this.consumer.consumptionRadius; j++)
			{
				int num2 = num + j + Grid.WidthInCells * i;
				if (Grid.Element[num2].IsState(expected_state))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004E3F RID: 20031 RVA: 0x000D2E98 File Offset: 0x000D1098
	private void OnConduitUpdate(float dt)
	{
		this.conduitBlockedStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.ConduitBlocked, this.conduitBlockedStatusGuid, this.dispenser.blocked, null);
	}

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06004E40 RID: 20032 RVA: 0x000D2ECC File Offset: 0x000D10CC
	public ConduitType conduitType
	{
		get
		{
			return this.dispenser.conduitType;
		}
	}

	// Token: 0x04003653 RID: 13907
	public static readonly Operational.Flag PumpableFlag = new Operational.Flag("vent", Operational.Flag.Type.Requirement);

	// Token: 0x04003654 RID: 13908
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003655 RID: 13909
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04003656 RID: 13910
	[MyCmpGet]
	private ElementConsumer consumer;

	// Token: 0x04003657 RID: 13911
	[MyCmpGet]
	private ConduitDispenser dispenser;

	// Token: 0x04003658 RID: 13912
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003659 RID: 13913
	private const float OperationalUpdateInterval = 1f;

	// Token: 0x0400365A RID: 13914
	private float elapsedTime;

	// Token: 0x0400365B RID: 13915
	private bool pumpable;

	// Token: 0x0400365C RID: 13916
	private Guid conduitBlockedStatusGuid;

	// Token: 0x0400365D RID: 13917
	private Guid noElementStatusGuid;
}
