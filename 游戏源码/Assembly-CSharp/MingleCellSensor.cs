using System;
using UnityEngine;

// Token: 0x02000828 RID: 2088
public class MingleCellSensor : Sensor
{
	// Token: 0x06002556 RID: 9558 RVA: 0x000B8681 File Offset: 0x000B6881
	public MingleCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
	}

	// Token: 0x06002557 RID: 9559 RVA: 0x001CC1A8 File Offset: 0x001CA3A8
	public override void Update()
	{
		this.cell = Grid.InvalidCell;
		int num = int.MaxValue;
		ListPool<int, MingleCellSensor>.PooledList pooledList = ListPool<int, MingleCellSensor>.Allocate();
		int num2 = 50;
		foreach (int num3 in Game.Instance.mingleCellTracker.mingleCells)
		{
			if (this.brain.IsCellClear(num3))
			{
				int navigationCost = this.navigator.GetNavigationCost(num3);
				if (navigationCost != -1)
				{
					if (num3 == Grid.InvalidCell || navigationCost < num)
					{
						this.cell = num3;
						num = navigationCost;
					}
					if (navigationCost < num2)
					{
						pooledList.Add(num3);
					}
				}
			}
		}
		if (pooledList.Count > 0)
		{
			this.cell = pooledList[UnityEngine.Random.Range(0, pooledList.Count)];
		}
		pooledList.Recycle();
	}

	// Token: 0x06002558 RID: 9560 RVA: 0x000B86A2 File Offset: 0x000B68A2
	public int GetCell()
	{
		return this.cell;
	}

	// Token: 0x0400193D RID: 6461
	private MinionBrain brain;

	// Token: 0x0400193E RID: 6462
	private Navigator navigator;

	// Token: 0x0400193F RID: 6463
	private int cell;
}
