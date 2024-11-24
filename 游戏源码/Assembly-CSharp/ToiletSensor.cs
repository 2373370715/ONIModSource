using System;

// Token: 0x0200082E RID: 2094
public class ToiletSensor : Sensor
{
	// Token: 0x06002579 RID: 9593 RVA: 0x000B88D0 File Offset: 0x000B6AD0
	public ToiletSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
	}

	// Token: 0x0600257A RID: 9594 RVA: 0x001CC4E4 File Offset: 0x001CA6E4
	public override void Update()
	{
		IUsable usable = null;
		int num = int.MaxValue;
		bool flag = false;
		foreach (IUsable usable2 in Components.Toilets.Items)
		{
			if (usable2.IsUsable())
			{
				flag = true;
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(usable2.transform.GetPosition()));
				if (navigationCost != -1 && navigationCost < num)
				{
					usable = usable2;
					num = navigationCost;
				}
			}
		}
		bool flag2 = Components.Toilets.Count > 0;
		if (usable != this.toilet || flag2 != this.areThereAnyToilets || this.areThereAnyUsableToilets != flag)
		{
			this.toilet = usable;
			this.areThereAnyToilets = flag2;
			this.areThereAnyUsableToilets = flag;
			base.Trigger(-752545459, null);
		}
	}

	// Token: 0x0600257B RID: 9595 RVA: 0x000B88E5 File Offset: 0x000B6AE5
	public bool AreThereAnyToilets()
	{
		return this.areThereAnyToilets;
	}

	// Token: 0x0600257C RID: 9596 RVA: 0x000B88ED File Offset: 0x000B6AED
	public bool AreThereAnyUsableToilets()
	{
		return this.areThereAnyUsableToilets;
	}

	// Token: 0x0600257D RID: 9597 RVA: 0x000B88F5 File Offset: 0x000B6AF5
	public IUsable GetNearestUsableToilet()
	{
		return this.toilet;
	}

	// Token: 0x0400194C RID: 6476
	private Navigator navigator;

	// Token: 0x0400194D RID: 6477
	private IUsable toilet;

	// Token: 0x0400194E RID: 6478
	private bool areThereAnyToilets;

	// Token: 0x0400194F RID: 6479
	private bool areThereAnyUsableToilets;
}
