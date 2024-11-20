using System;
using HarmonyLib;

namespace ShinseiSuperHeater
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch(typeof(Db), "Initialize")]
	public class WaterAddToTech
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000023EA File Offset: 0x000005EA
		[HarmonyPostfix]
		private static void Postfix()
		{
			Db.Get().Techs.Get("TemperatureModulation").unlockedItemIDs.Add("ShinseiSuperHeater");
		}
	}
}
