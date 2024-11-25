﻿using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class SpeedLoopingSoundUpdater : LoopingSoundParameterUpdater
{
		public SpeedLoopingSoundUpdater() : base("Speed")
	{
	}

		public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		SpeedLoopingSoundUpdater.Entry item = new SpeedLoopingSoundUpdater.Entry
		{
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

		public override void Update(float dt)
	{
		float speedParameterValue = SpeedLoopingSoundUpdater.GetSpeedParameterValue();
		foreach (SpeedLoopingSoundUpdater.Entry entry in this.entries)
		{
			EventInstance ev = entry.ev;
			ev.setParameterByID(entry.parameterId, speedParameterValue, false);
		}
	}

		public override void Remove(LoopingSoundParameterUpdater.Sound sound)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].ev.handle == sound.ev.handle)
			{
				this.entries.RemoveAt(i);
				return;
			}
		}
	}

		public static float GetSpeedParameterValue()
	{
		return Time.timeScale * 1f;
	}

		private List<SpeedLoopingSoundUpdater.Entry> entries = new List<SpeedLoopingSoundUpdater.Entry>();

		private struct Entry
	{
				public EventInstance ev;

				public PARAMETER_ID parameterId;
	}
}
