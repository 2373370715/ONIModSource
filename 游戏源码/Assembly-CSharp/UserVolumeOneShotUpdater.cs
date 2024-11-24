using System;

// Token: 0x02000957 RID: 2391
internal abstract class UserVolumeOneShotUpdater : OneShotSoundParameterUpdater
{
	// Token: 0x06002B34 RID: 11060 RVA: 0x000BC18D File Offset: 0x000BA38D
	public UserVolumeOneShotUpdater(string parameter, string player_pref) : base(parameter)
	{
		this.playerPref = player_pref;
	}

	// Token: 0x06002B35 RID: 11061 RVA: 0x001DE340 File Offset: 0x001DC540
	public override void Play(OneShotSoundParameterUpdater.Sound sound)
	{
		if (!string.IsNullOrEmpty(this.playerPref))
		{
			float @float = KPlayerPrefs.GetFloat(this.playerPref);
			sound.ev.setParameterByID(sound.description.GetParameterId(base.parameter), @float, false);
		}
	}

	// Token: 0x04001CEE RID: 7406
	private string playerPref;
}
