using System;
using System.Collections.Generic;

// Token: 0x02000822 RID: 2082
public class ClosestEdibleSensor : Sensor
{
	// Token: 0x06002546 RID: 9542 RVA: 0x000B855F File Offset: 0x000B675F
	public ClosestEdibleSensor(Sensors sensors) : base(sensors)
	{
	}

	// Token: 0x06002547 RID: 9543 RVA: 0x001CBEC8 File Offset: 0x001CA0C8
	public override void Update()
	{
		HashSet<Tag> forbiddenTagSet = base.GetComponent<ConsumableConsumer>().forbiddenTagSet;
		Pickupable pickupable = Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), forbiddenTagSet, ClosestEdibleSensor.requiredSearchTags);
		bool flag = this.edibleInReachButNotPermitted;
		Edible x = null;
		bool flag2 = false;
		if (pickupable != null)
		{
			x = pickupable.GetComponent<Edible>();
			flag2 = true;
			flag = false;
		}
		else
		{
			flag = (Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), new HashSet<Tag>(), ClosestEdibleSensor.requiredSearchTags) != null);
		}
		if (x != this.edible || this.hasEdible != flag2)
		{
			this.edible = x;
			this.hasEdible = flag2;
			this.edibleInReachButNotPermitted = flag;
			base.Trigger(86328522, this.edible);
		}
	}

	// Token: 0x06002548 RID: 9544 RVA: 0x000B857D File Offset: 0x000B677D
	public Edible GetEdible()
	{
		return this.edible;
	}

	// Token: 0x0400192D RID: 6445
	private Edible edible;

	// Token: 0x0400192E RID: 6446
	private bool hasEdible;

	// Token: 0x0400192F RID: 6447
	public bool edibleInReachButNotPermitted;

	// Token: 0x04001930 RID: 6448
	public static Tag[] requiredSearchTags = new Tag[]
	{
		GameTags.Edible
	};
}
