using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02001905 RID: 6405
internal class UpdateRocketSpeedParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06008555 RID: 34133 RVA: 0x000F773D File Offset: 0x000F593D
	public UpdateRocketSpeedParameter() : base("rocketSpeed")
	{
	}

	// Token: 0x06008556 RID: 34134 RVA: 0x00347D90 File Offset: 0x00345F90
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateRocketSpeedParameter.Entry item = new UpdateRocketSpeedParameter.Entry
		{
			rocketModule = sound.transform.GetComponent<RocketModule>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06008557 RID: 34135 RVA: 0x00347DEC File Offset: 0x00345FEC
	public override void Update(float dt)
	{
		foreach (UpdateRocketSpeedParameter.Entry entry in this.entries)
		{
			if (!(entry.rocketModule == null))
			{
				LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
				if (!(conditionManager == null))
				{
					ILaunchableRocket component = conditionManager.GetComponent<ILaunchableRocket>();
					if (component != null)
					{
						EventInstance ev = entry.ev;
						ev.setParameterByID(entry.parameterId, component.rocketSpeed, false);
					}
				}
			}
		}
	}

	// Token: 0x06008558 RID: 34136 RVA: 0x00347E84 File Offset: 0x00346084
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

	// Token: 0x040064BC RID: 25788
	private List<UpdateRocketSpeedParameter.Entry> entries = new List<UpdateRocketSpeedParameter.Entry>();

	// Token: 0x02001906 RID: 6406
	private struct Entry
	{
		// Token: 0x040064BD RID: 25789
		public RocketModule rocketModule;

		// Token: 0x040064BE RID: 25790
		public EventInstance ev;

		// Token: 0x040064BF RID: 25791
		public PARAMETER_ID parameterId;
	}
}
