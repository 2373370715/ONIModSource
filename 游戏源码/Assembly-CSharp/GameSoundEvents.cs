using System;

// Token: 0x02001334 RID: 4916
public static class GameSoundEvents
{
	// Token: 0x04004A55 RID: 19029
	public static GameSoundEvents.Event BatteryFull = new GameSoundEvents.Event("game_triggered.battery_full");

	// Token: 0x04004A56 RID: 19030
	public static GameSoundEvents.Event BatteryWarning = new GameSoundEvents.Event("game_triggered.battery_warning");

	// Token: 0x04004A57 RID: 19031
	public static GameSoundEvents.Event BatteryDischarged = new GameSoundEvents.Event("game_triggered.battery_drained");

	// Token: 0x02001335 RID: 4917
	public class Event
	{
		// Token: 0x060064F2 RID: 25842 RVA: 0x000E1E0D File Offset: 0x000E000D
		public Event(string name)
		{
			this.Name = name;
		}

		// Token: 0x04004A58 RID: 19032
		public HashedString Name;
	}
}
