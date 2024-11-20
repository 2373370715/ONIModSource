using System;
using HarmonyLib;

namespace ShinseiSuperHeater
{
	// Token: 0x02000005 RID: 5
	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	public class WaterAddToBuild
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002220 File Offset: 0x00000420
		[HarmonyPostfix]
		private static void Postfix()
		{
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS.SHINSEISUPERHEATER.NAME",
				"液体冷却器"
			});
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS.SHINSEISUPERHEATER.DESC",
				"降低液体温度"
			});
			Strings.Add(new string[]
			{
				"STRINGS.BUILDINGS.PREFABS.SHINSEISUPERHEATER.EFFECT",
				"最低可以把温度降到-200摄氏度"
			});
			ModUtil.AddBuildingToPlanScreen("Utilities", "ShinseiSuperHeater");
		}
	}
}
