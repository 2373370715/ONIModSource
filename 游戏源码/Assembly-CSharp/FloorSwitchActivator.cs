using System;
using UnityEngine;

// Token: 0x020012FF RID: 4863
[AddComponentMenu("KMonoBehaviour/scripts/FloorSwitchActivator")]
public class FloorSwitchActivator : KMonoBehaviour
{
	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x060063CF RID: 25551 RVA: 0x000E11E3 File Offset: 0x000DF3E3
	public PrimaryElement PrimaryElement
	{
		get
		{
			return this.primaryElement;
		}
	}

	// Token: 0x060063D0 RID: 25552 RVA: 0x000E11EB File Offset: 0x000DF3EB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
		this.OnCellChange();
	}

	// Token: 0x060063D1 RID: 25553 RVA: 0x000E11FF File Offset: 0x000DF3FF
	protected override void OnCleanUp()
	{
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x060063D2 RID: 25554 RVA: 0x002BCEE0 File Offset: 0x002BB0E0
	private void OnCellChange()
	{
		int num = Grid.PosToCell(this);
		GameScenePartitioner.Instance.UpdatePosition(this.partitionerEntry, num);
		if (Grid.IsValidCell(this.last_cell_occupied) && num != this.last_cell_occupied)
		{
			this.NotifyChanged(this.last_cell_occupied);
		}
		this.NotifyChanged(num);
		this.last_cell_occupied = num;
	}

	// Token: 0x060063D3 RID: 25555 RVA: 0x000E120D File Offset: 0x000DF40D
	private void NotifyChanged(int cell)
	{
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.floorSwitchActivatorChangedLayer, this);
	}

	// Token: 0x060063D4 RID: 25556 RVA: 0x000E1225 File Offset: 0x000DF425
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.Register();
	}

	// Token: 0x060063D5 RID: 25557 RVA: 0x000E1233 File Offset: 0x000DF433
	protected override void OnCmpDisable()
	{
		this.Unregister();
		base.OnCmpDisable();
	}

	// Token: 0x060063D6 RID: 25558 RVA: 0x002BCF38 File Offset: 0x002BB138
	private void Register()
	{
		if (this.registered)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("FloorSwitchActivator.Register", this, cell, GameScenePartitioner.Instance.floorSwitchActivatorLayer, null);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "FloorSwitchActivator.Register");
		this.registered = true;
	}

	// Token: 0x060063D7 RID: 25559 RVA: 0x002BCFA0 File Offset: 0x002BB1A0
	private void Unregister()
	{
		if (!this.registered)
		{
			return;
		}
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		if (this.last_cell_occupied > -1)
		{
			this.NotifyChanged(this.last_cell_occupied);
		}
		this.registered = false;
	}

	// Token: 0x04004755 RID: 18261
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04004756 RID: 18262
	private bool registered;

	// Token: 0x04004757 RID: 18263
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04004758 RID: 18264
	private int last_cell_occupied = -1;
}
