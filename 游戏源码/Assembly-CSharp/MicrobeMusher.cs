using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000EAC RID: 3756
public class MicrobeMusher : ComplexFabricator
{
	// Token: 0x06004BB4 RID: 19380 RVA: 0x000CA35A File Offset: 0x000C855A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.Cook;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
	}

	// Token: 0x06004BB5 RID: 19381 RVA: 0x0025F788 File Offset: 0x0025D988
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true);
		}, null, null);
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Mushing;
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.workable.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_ration"
		});
		this.workable.meter.meterController.SetSymbolVisiblity(MicrobeMusher.canHash, false);
		this.workable.meter.meterController.SetSymbolVisiblity(MicrobeMusher.meterRationHash, false);
	}

	// Token: 0x06004BB6 RID: 19382 RVA: 0x0025F8B4 File Offset: 0x0025DAB4
	protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = base.SpawnOrderProduct(recipe);
		foreach (GameObject gameObject in list)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component != null)
			{
				if (gameObject.PrefabID() == "MushBar")
				{
					byte index = Db.Get().Diseases.GetIndex("FoodPoisoning");
					component.AddDisease(index, 1000, "Made of mud");
				}
				if (gameObject.GetComponent<PrimaryElement>().DiseaseCount > 0)
				{
					Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_DiseaseCooking, true);
				}
			}
		}
		return list;
	}

	// Token: 0x04003478 RID: 13432
	[SerializeField]
	public Vector3 mushbarSpawnOffset = Vector3.right;

	// Token: 0x04003479 RID: 13433
	private static readonly KAnimHashedString meterRationHash = new KAnimHashedString("meter_ration");

	// Token: 0x0400347A RID: 13434
	private static readonly KAnimHashedString canHash = new KAnimHashedString("can");
}
