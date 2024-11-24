using System;
using TUNING;

// Token: 0x02000EE1 RID: 3809
public class NavTeleporter : KMonoBehaviour
{
	// Token: 0x06004CC3 RID: 19651 RVA: 0x002637A8 File Offset: 0x002619A8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.NavTeleporters, false);
		this.Register();
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "NavTeleporterCellChanged");
	}

	// Token: 0x06004CC4 RID: 19652 RVA: 0x002637F4 File Offset: 0x002619F4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		int cell = this.GetCell();
		if (cell != Grid.InvalidCell)
		{
			Grid.HasNavTeleporter[cell] = false;
		}
		this.Deregister();
		Components.NavTeleporters.Remove(this);
	}

	// Token: 0x06004CC5 RID: 19653 RVA: 0x000D1C76 File Offset: 0x000CFE76
	public void SetOverrideCell(int cell)
	{
		this.overrideCell = cell;
	}

	// Token: 0x06004CC6 RID: 19654 RVA: 0x000D1C7F File Offset: 0x000CFE7F
	public int GetCell()
	{
		if (this.overrideCell >= 0)
		{
			return this.overrideCell;
		}
		return Grid.OffsetCell(Grid.PosToCell(this), this.offset);
	}

	// Token: 0x06004CC7 RID: 19655 RVA: 0x00263834 File Offset: 0x00261A34
	public void TwoWayTarget(NavTeleporter nt)
	{
		if (this.target != null)
		{
			if (nt != null)
			{
				nt.SetTarget(null);
			}
			this.BreakLink();
		}
		this.target = nt;
		if (this.target != null)
		{
			this.SetLink();
			if (nt != null)
			{
				nt.SetTarget(this);
			}
		}
	}

	// Token: 0x06004CC8 RID: 19656 RVA: 0x000D1CA2 File Offset: 0x000CFEA2
	public void EnableTwoWayTarget(bool enable)
	{
		if (enable)
		{
			this.target.SetLink();
			this.SetLink();
			return;
		}
		this.target.BreakLink();
		this.BreakLink();
	}

	// Token: 0x06004CC9 RID: 19657 RVA: 0x000D1CCA File Offset: 0x000CFECA
	public void SetTarget(NavTeleporter nt)
	{
		if (this.target != null)
		{
			this.BreakLink();
		}
		this.target = nt;
		if (this.target != null)
		{
			this.SetLink();
		}
	}

	// Token: 0x06004CCA RID: 19658 RVA: 0x00263890 File Offset: 0x00261A90
	private void Register()
	{
		int cell = this.GetCell();
		if (!Grid.IsValidCell(cell))
		{
			this.lastRegisteredCell = Grid.InvalidCell;
			return;
		}
		Grid.HasNavTeleporter[cell] = true;
		Pathfinding.Instance.AddDirtyNavGridCell(cell);
		this.lastRegisteredCell = cell;
		if (this.target != null)
		{
			this.SetLink();
		}
	}

	// Token: 0x06004CCB RID: 19659 RVA: 0x002638EC File Offset: 0x00261AEC
	private void SetLink()
	{
		int cell = this.target.GetCell();
		Pathfinding.Instance.GetNavGrid(DUPLICANTSTATS.STANDARD.BaseStats.NAV_GRID_NAME).teleportTransitions[this.lastRegisteredCell] = cell;
		Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
	}

	// Token: 0x06004CCC RID: 19660 RVA: 0x00263940 File Offset: 0x00261B40
	public void Deregister()
	{
		if (this.lastRegisteredCell != Grid.InvalidCell)
		{
			this.BreakLink();
			Grid.HasNavTeleporter[this.lastRegisteredCell] = false;
			Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
			this.lastRegisteredCell = Grid.InvalidCell;
		}
	}

	// Token: 0x06004CCD RID: 19661 RVA: 0x000D1CFB File Offset: 0x000CFEFB
	private void BreakLink()
	{
		Pathfinding.Instance.GetNavGrid(DUPLICANTSTATS.STANDARD.BaseStats.NAV_GRID_NAME).teleportTransitions.Remove(this.lastRegisteredCell);
		Pathfinding.Instance.AddDirtyNavGridCell(this.lastRegisteredCell);
	}

	// Token: 0x06004CCE RID: 19662 RVA: 0x0026398C File Offset: 0x00261B8C
	private void OnCellChanged()
	{
		this.Deregister();
		this.Register();
		if (this.target != null)
		{
			NavTeleporter component = this.target.GetComponent<NavTeleporter>();
			if (component != null)
			{
				component.SetTarget(this);
			}
		}
	}

	// Token: 0x0400355E RID: 13662
	private NavTeleporter target;

	// Token: 0x0400355F RID: 13663
	private int lastRegisteredCell = Grid.InvalidCell;

	// Token: 0x04003560 RID: 13664
	public CellOffset offset;

	// Token: 0x04003561 RID: 13665
	private int overrideCell = -1;
}
