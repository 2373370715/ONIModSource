﻿using System;

public class KCalTracker : WorldTracker
{
	public KCalTracker(int worldID) : base(worldID)
	{
	}

	public override void UpdateData()
	{
		base.AddPoint(RationTracker.Get().CountRations(null, ClusterManager.Instance.GetWorld(base.WorldID).worldInventory, true));
	}

	public override string FormatValueString(float value)
	{
		return GameUtil.GetFormattedCalories(value, GameUtil.TimeSlice.None, true);
	}
}
