using System;
using TUNING;

// Token: 0x02000C83 RID: 3203
public class BuildingInternalConstructorWorkable : Workable
{
	// Token: 0x06003D9E RID: 15774 RVA: 0x00231D88 File Offset: 0x0022FF88
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.resetProgressOnStop = false;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06003D9F RID: 15775 RVA: 0x000C7F4B File Offset: 0x000C614B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.constructorInstance = this.GetSMI<BuildingInternalConstructor.Instance>();
	}

	// Token: 0x06003DA0 RID: 15776 RVA: 0x000C7F5F File Offset: 0x000C615F
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.constructorInstance.ConstructionComplete(false);
	}

	// Token: 0x040029FA RID: 10746
	private BuildingInternalConstructor.Instance constructorInstance;
}
