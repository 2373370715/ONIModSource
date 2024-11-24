using System;
using TUNING;

// Token: 0x020017C3 RID: 6083
public class ArmTrapWorkable : Workable
{
	// Token: 0x06007D5C RID: 32092 RVA: 0x00326418 File Offset: 0x00324618
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.CanBeArmedAtLongDistance)
		{
			base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			this.faceTargetWhenWorking = true;
			this.multitoolContext = "build";
			this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		}
		if (this.initialOffsets != null && this.initialOffsets.Length != 0)
		{
			base.SetOffsets(this.initialOffsets);
		}
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.ArmingTrap);
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		base.SetWorkTime(5f);
		this.synchronizeAnims = true;
		this.resetProgressOnStop = true;
	}

	// Token: 0x06007D5D RID: 32093 RVA: 0x000F28B6 File Offset: 0x000F0AB6
	public override void OnPendingCompleteWork(WorkerBase worker)
	{
		base.OnPendingCompleteWork(worker);
		this.WorkInPstAnimation = true;
		base.gameObject.Trigger(-2025798095, null);
	}

	// Token: 0x06007D5E RID: 32094 RVA: 0x000F28D7 File Offset: 0x000F0AD7
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.WorkInPstAnimation = false;
	}

	// Token: 0x04005EE2 RID: 24290
	public bool WorkInPstAnimation;

	// Token: 0x04005EE3 RID: 24291
	public bool CanBeArmedAtLongDistance;

	// Token: 0x04005EE4 RID: 24292
	public CellOffset[] initialOffsets;
}
