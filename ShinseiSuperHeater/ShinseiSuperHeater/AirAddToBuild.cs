using System;
using HarmonyLib;

namespace ShinseiSuperHeater
{
	// Token: 0x02000003 RID: 3
	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	public class AirAddToBuild
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002080 File Offset: 0x00000280
		[HarmonyPostfix]
		private static void Postfix()
		{
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS.SHINSEIINDUSTRIALAIRHEATER.NAME",
				"空间冷却器"
			});
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS.SHINSEIINDUSTRIALAIRHEATER.DESC",
				"可以降低空间温度"
			});
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS.SHINSEIINDUSTRIALAIRHEATER.EFFECT",
				"最低把空间温度降低到-200摄氏度"
			});
			ModUtil.AddBuildingToPlanScreen("Utilities", "ShinseiIndustrialAirHeater");
		}
	}
}
