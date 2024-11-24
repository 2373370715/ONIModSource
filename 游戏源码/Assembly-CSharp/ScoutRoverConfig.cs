using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200026B RID: 619
public class ScoutRoverConfig : IEntityConfig
{
	// Token: 0x0600090A RID: 2314 RVA: 0x0016493C File Offset: 0x00162B3C
	public GameObject CreatePrefab()
	{
		return BaseRoverConfig.BaseRover("ScoutRover", STRINGS.ROBOTS.MODELS.SCOUT.NAME, GameTags.Robots.Models.ScoutRover, STRINGS.ROBOTS.MODELS.SCOUT.DESC, "scout_bot_kanim", 100f, 1f, 2f, TUNING.ROBOTS.SCOUTBOT.CARRY_CAPACITY, TUNING.ROBOTS.SCOUTBOT.DIGGING, TUNING.ROBOTS.SCOUTBOT.CONSTRUCTION, TUNING.ROBOTS.SCOUTBOT.ATHLETICS, TUNING.ROBOTS.SCOUTBOT.HIT_POINTS, TUNING.ROBOTS.SCOUTBOT.BATTERY_CAPACITY, TUNING.ROBOTS.SCOUTBOT.BATTERY_DEPLETION_RATE, Db.Get().Amounts.InternalChemicalBattery, false);
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x000AA581 File Offset: 0x000A8781
	public void OnPrefabInit(GameObject inst)
	{
		BaseRoverConfig.OnPrefabInit(inst, Db.Get().Amounts.InternalChemicalBattery);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x001649B4 File Offset: 0x00162BB4
	public void OnSpawn(GameObject inst)
	{
		BaseRoverConfig.OnSpawn(inst);
		Effects effects = inst.GetComponent<Effects>();
		if (inst.transform.parent == null)
		{
			if (effects.HasEffect("ScoutBotCharging"))
			{
				effects.Remove("ScoutBotCharging");
			}
		}
		else if (!effects.HasEffect("ScoutBotCharging"))
		{
			effects.Add("ScoutBotCharging", false);
		}
		inst.Subscribe(856640610, delegate(object data)
		{
			if (inst.transform.parent == null)
			{
				if (effects.HasEffect("ScoutBotCharging"))
				{
					effects.Remove("ScoutBotCharging");
					return;
				}
			}
			else if (!effects.HasEffect("ScoutBotCharging"))
			{
				effects.Add("ScoutBotCharging", false);
			}
		});
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x000A6566 File Offset: 0x000A4766
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x040006C5 RID: 1733
	public const string ID = "ScoutRover";

	// Token: 0x040006C6 RID: 1734
	public const float MASS = 100f;

	// Token: 0x040006C7 RID: 1735
	private const float WIDTH = 1f;

	// Token: 0x040006C8 RID: 1736
	private const float HEIGHT = 2f;

	// Token: 0x040006C9 RID: 1737
	public const int MAXIMUM_TECH_CONSTRUCTION_TIER = 2;
}
