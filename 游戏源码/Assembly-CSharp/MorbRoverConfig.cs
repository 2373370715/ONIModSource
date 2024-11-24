using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000254 RID: 596
public class MorbRoverConfig : IEntityConfig
{
	// Token: 0x06000883 RID: 2179 RVA: 0x00163284 File Offset: 0x00161484
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseRoverConfig.BaseRover("MorbRover", STRINGS.ROBOTS.MODELS.MORB.NAME, GameTags.Robots.Models.MorbRover, STRINGS.ROBOTS.MODELS.MORB.DESC, "morbRover_kanim", 300f, 1f, 2f, TUNING.ROBOTS.MORBBOT.CARRY_CAPACITY, 1f, 1f, 3f, TUNING.ROBOTS.MORBBOT.HIT_POINTS, 180000f, 30f, Db.Get().Amounts.InternalBioBattery, false);
		gameObject.GetComponent<PrimaryElement>().SetElement(SimHashes.Steel, false);
		gameObject.GetComponent<Deconstructable>().customWorkTime = 10f;
		return gameObject;
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x000AA0E5 File Offset: 0x000A82E5
	public void OnPrefabInit(GameObject inst)
	{
		BaseRoverConfig.OnPrefabInit(inst, Db.Get().Amounts.InternalBioBattery);
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x000AA0FC File Offset: 0x000A82FC
	public void OnSpawn(GameObject inst)
	{
		BaseRoverConfig.OnSpawn(inst);
		inst.Subscribe(1623392196, new Action<object>(this.TriggerDeconstructChoreOnDeath));
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x0016331C File Offset: 0x0016151C
	public void TriggerDeconstructChoreOnDeath(object obj)
	{
		if (obj != null)
		{
			Deconstructable component = ((GameObject)obj).GetComponent<Deconstructable>();
			if (!component.IsMarkedForDeconstruction())
			{
				component.QueueDeconstruction(false);
			}
		}
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0400065E RID: 1630
	public const string ID = "MorbRover";

	// Token: 0x0400065F RID: 1631
	public const SimHashes MATERIAL = SimHashes.Steel;

	// Token: 0x04000660 RID: 1632
	public const float MASS = 300f;

	// Token: 0x04000661 RID: 1633
	private const float WIDTH = 1f;

	// Token: 0x04000662 RID: 1634
	private const float HEIGHT = 2f;
}
