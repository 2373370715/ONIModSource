using System;
using HarmonyLib;

namespace ShinseiSuperHeater
{
	// Token: 0x02000002 RID: 2
	[HarmonyPatch(typeof(Db), "Initialize")]
	public class AirAddToTech
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		[HarmonyPostfix]
		private static void Postfix()
		{
			Db.Get().Techs.Get("TemperatureModulation").unlockedItemIDs.Add("ShinseiIndustrialAirHeater");
		}
	}
}
