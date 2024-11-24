using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02001903 RID: 6403
internal class UpdateRocketLandingParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06008551 RID: 34129 RVA: 0x000F7720 File Offset: 0x000F5920
	public UpdateRocketLandingParameter() : base("rocketLanding")
	{
	}

	// Token: 0x06008552 RID: 34130 RVA: 0x00347C20 File Offset: 0x00345E20
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateRocketLandingParameter.Entry item = new UpdateRocketLandingParameter.Entry
		{
			rocketModule = sound.transform.GetComponent<RocketModule>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06008553 RID: 34131 RVA: 0x00347C7C File Offset: 0x00345E7C
	public override void Update(float dt)
	{
		foreach (UpdateRocketLandingParameter.Entry entry in this.entries)
		{
			if (!(entry.rocketModule == null))
			{
				LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
				if (!(conditionManager == null))
				{
					ILaunchableRocket component = conditionManager.GetComponent<ILaunchableRocket>();
					if (component != null)
					{
						if (component.isLanding)
						{
							EventInstance ev = entry.ev;
							ev.setParameterByID(entry.parameterId, 1f, false);
						}
						else
						{
							EventInstance ev = entry.ev;
							ev.setParameterByID(entry.parameterId, 0f, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x06008554 RID: 34132 RVA: 0x00347D38 File Offset: 0x00345F38
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

	// Token: 0x040064B8 RID: 25784
	private List<UpdateRocketLandingParameter.Entry> entries = new List<UpdateRocketLandingParameter.Entry>();

	// Token: 0x02001904 RID: 6404
	private struct Entry
	{
		// Token: 0x040064B9 RID: 25785
		public RocketModule rocketModule;

		// Token: 0x040064BA RID: 25786
		public EventInstance ev;

		// Token: 0x040064BB RID: 25787
		public PARAMETER_ID parameterId;
	}
}
