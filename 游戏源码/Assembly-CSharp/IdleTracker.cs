using System.Collections.Generic;

public class IdleTracker : WorldTracker
{
	public IdleTracker(int worldID)
		: base(worldID)
	{
	}

	public override void UpdateData()
	{
		objectsOfInterest.Clear();
		int num = 0;
		List<MinionIdentity> worldItems = Components.LiveMinionIdentities.GetWorldItems(base.WorldID);
		for (int i = 0; i < worldItems.Count; i++)
		{
			if (worldItems[i].HasTag(GameTags.Idle))
			{
				num++;
				objectsOfInterest.Add(worldItems[i].gameObject);
			}
		}
		AddPoint(num);
	}

	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
