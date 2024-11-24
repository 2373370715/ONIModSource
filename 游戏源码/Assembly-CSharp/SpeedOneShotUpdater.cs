using System;

// Token: 0x02002011 RID: 8209
public class SpeedOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x0600AE9B RID: 44699 RVA: 0x001119A3 File Offset: 0x0010FBA3
	public SpeedOneShotUpdater() : base("Speed")
	{
	}

	// Token: 0x0600AE9C RID: 44700 RVA: 0x001119B5 File Offset: 0x0010FBB5
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		sound.ev.setParameterByID(sound.description.GetParameterId(base.parameter), SpeedLoopingSoundUpdater.GetSpeedParameterValue(), false);
	}
}
