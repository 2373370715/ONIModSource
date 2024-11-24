using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200218F RID: 8591
	public class EstablishColonies : VictoryColonyAchievementRequirement
	{
		// Token: 0x0600B698 RID: 46744 RVA: 0x00459FF4 File Offset: 0x004581F4
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.ESTABLISH_COLONIES.Replace("{goalBaseCount}", EstablishColonies.BASE_COUNT.ToString()).Replace("{baseCount}", this.GetColonyCount().ToString()).Replace("{neededCount}", EstablishColonies.BASE_COUNT.ToString());
		}

		// Token: 0x0600B699 RID: 46745 RVA: 0x00115A00 File Offset: 0x00113C00
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x0600B69A RID: 46746 RVA: 0x00115AFE File Offset: 0x00113CFE
		public override bool Success()
		{
			return this.GetColonyCount() >= EstablishColonies.BASE_COUNT;
		}

		// Token: 0x0600B69B RID: 46747 RVA: 0x00115B10 File Offset: 0x00113D10
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.SEVERAL_COLONIES;
		}

		// Token: 0x0600B69C RID: 46748 RVA: 0x00277378 File Offset: 0x00275578
		private int GetColonyCount()
		{
			int num = 0;
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				Activatable component = Components.Telepads[i].GetComponent<Activatable>();
				if (component == null || component.IsActivated)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x040094FB RID: 38139
		public static int BASE_COUNT = 5;
	}
}
