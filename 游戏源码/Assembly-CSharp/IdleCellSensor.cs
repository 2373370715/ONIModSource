using System;
using UnityEngine;

// Token: 0x02000827 RID: 2087
public class IdleCellSensor : Sensor
{
	// Token: 0x06002553 RID: 9555 RVA: 0x000B864C File Offset: 0x000B684C
	public IdleCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
		this.prefabid = base.GetComponent<KPrefabID>();
	}

	// Token: 0x06002554 RID: 9556 RVA: 0x001CC12C File Offset: 0x001CA32C
	public override void Update()
	{
		if (!this.prefabid.HasTag(GameTags.Idle))
		{
			this.cell = Grid.InvalidCell;
			return;
		}
		MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)this.navigator.GetCurrentAbilities();
		minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
		IdleCellQuery idleCellQuery = PathFinderQueries.idleCellQuery.Reset(this.brain, UnityEngine.Random.Range(30, 60));
		this.navigator.RunQuery(idleCellQuery);
		minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
		this.cell = idleCellQuery.GetResultCell();
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x000B8679 File Offset: 0x000B6879
	public int GetCell()
	{
		return this.cell;
	}

	// Token: 0x04001939 RID: 6457
	private MinionBrain brain;

	// Token: 0x0400193A RID: 6458
	private Navigator navigator;

	// Token: 0x0400193B RID: 6459
	private KPrefabID prefabid;

	// Token: 0x0400193C RID: 6460
	private int cell;
}
