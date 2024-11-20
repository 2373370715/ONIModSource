using System;
using System.Collections.Generic;

public class ClosestEdibleSensor : Sensor
{
	public ClosestEdibleSensor(Sensors sensors) : base(sensors)
	{
	}

	public override void Update()
	{
		HashSet<Tag> forbiddenTagSet = base.GetComponent<ConsumableConsumer>().forbiddenTagSet;
		Pickupable pickupable = Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), forbiddenTagSet, GameTags.Edible);
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
			flag = (Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), new HashSet<Tag>(), GameTags.Edible) != null);
		}
		if (x != this.edible || this.hasEdible != flag2)
		{
			this.edible = x;
			this.hasEdible = flag2;
			this.edibleInReachButNotPermitted = flag;
			base.Trigger(86328522, this.edible);
		}
	}

	public Edible GetEdible()
	{
		return this.edible;
	}

	private Edible edible;

	private bool hasEdible;

	public bool edibleInReachButNotPermitted;
}
