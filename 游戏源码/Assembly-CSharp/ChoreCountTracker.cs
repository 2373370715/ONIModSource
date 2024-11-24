public class ChoreCountTracker : WorldTracker
{
	public ChoreGroup choreGroup;

	public ChoreCountTracker(int worldID, ChoreGroup group)
		: base(worldID)
	{
		choreGroup = group;
	}

	public override void UpdateData()
	{
		float num = 0f;
		GlobalChoreProvider.Instance.choreWorldMap.TryGetValue(base.WorldID, out var value);
		int num2 = 0;
		while (value != null && num2 < value.Count)
		{
			Chore chore = value[num2];
			if (chore != null && !chore.target.Equals(null) && !(chore.gameObject == null))
			{
				ChoreGroup[] groups = chore.choreType.groups;
				for (int i = 0; i < groups.Length; i++)
				{
					if (groups[i] == choreGroup)
					{
						num += 1f;
						break;
					}
				}
			}
			num2++;
		}
		GlobalChoreProvider.Instance.fetchMap.TryGetValue(base.WorldID, out var value2);
		int num3 = 0;
		while (value2 != null && num3 < value2.Count)
		{
			Chore chore2 = value2[num3];
			if (chore2 != null && !chore2.target.Equals(null) && !(chore2.gameObject == null))
			{
				ChoreGroup[] groups2 = chore2.choreType.groups;
				for (int j = 0; j < groups2.Length; j++)
				{
					if (groups2[j] == choreGroup)
					{
						num += 1f;
						break;
					}
				}
			}
			num3++;
		}
		AddPoint(num);
	}

	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
