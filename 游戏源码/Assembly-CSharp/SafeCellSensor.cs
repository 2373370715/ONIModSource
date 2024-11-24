using System;
using Klei.AI;

// Token: 0x0200082B RID: 2091
public class SafeCellSensor : Sensor
{
	// Token: 0x0600255D RID: 9565 RVA: 0x001CC288 File Offset: 0x001CA488
	public SafeCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.prefabid = base.GetComponent<KPrefabID>();
		this.traits = base.GetComponent<Traits>();
	}

	// Token: 0x0600255E RID: 9566 RVA: 0x001CC2D8 File Offset: 0x001CA4D8
	public override void Update()
	{
		if (!this.prefabid.HasTag(GameTags.Idle))
		{
			this.cell = Grid.InvalidCell;
			return;
		}
		bool flag = this.HasSafeCell();
		this.RunSafeCellQuery(false);
		bool flag2 = this.HasSafeCell();
		if (flag2 != flag)
		{
			if (flag2)
			{
				this.sensors.Trigger(982561777, null);
				return;
			}
			this.sensors.Trigger(506919987, null);
		}
	}

	// Token: 0x0600255F RID: 9567 RVA: 0x000B871B File Offset: 0x000B691B
	public void RunSafeCellQuery(bool avoid_light)
	{
		this.cell = this.RunAndGetSafeCellQueryResult(avoid_light);
		if (this.cell == Grid.PosToCell(this.navigator))
		{
			this.cell = Grid.InvalidCell;
		}
	}

	// Token: 0x06002560 RID: 9568 RVA: 0x001CC344 File Offset: 0x001CA544
	public int RunAndGetSafeCellQueryResult(bool avoid_light)
	{
		MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)this.navigator.GetCurrentAbilities();
		minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
		SafeCellQuery safeCellQuery = PathFinderQueries.safeCellQuery.Reset(this.brain, avoid_light);
		this.navigator.RunQuery(safeCellQuery);
		minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
		this.cell = safeCellQuery.GetResultCell();
		return this.cell;
	}

	// Token: 0x06002561 RID: 9569 RVA: 0x000B8748 File Offset: 0x000B6948
	public int GetSensorCell()
	{
		return this.cell;
	}

	// Token: 0x06002562 RID: 9570 RVA: 0x000B8750 File Offset: 0x000B6950
	public int GetCellQuery()
	{
		if (this.cell == Grid.InvalidCell)
		{
			this.RunSafeCellQuery(false);
		}
		return this.cell;
	}

	// Token: 0x06002563 RID: 9571 RVA: 0x000B876C File Offset: 0x000B696C
	public int GetSleepCellQuery()
	{
		if (this.cell == Grid.InvalidCell)
		{
			this.RunSafeCellQuery(!this.traits.HasTrait("NightLight"));
		}
		return this.cell;
	}

	// Token: 0x06002564 RID: 9572 RVA: 0x000B879D File Offset: 0x000B699D
	public bool HasSafeCell()
	{
		return this.cell != Grid.InvalidCell && this.cell != Grid.PosToCell(this.sensors);
	}

	// Token: 0x04001943 RID: 6467
	private MinionBrain brain;

	// Token: 0x04001944 RID: 6468
	private Navigator navigator;

	// Token: 0x04001945 RID: 6469
	private KPrefabID prefabid;

	// Token: 0x04001946 RID: 6470
	private Traits traits;

	// Token: 0x04001947 RID: 6471
	private int cell = Grid.InvalidCell;
}
