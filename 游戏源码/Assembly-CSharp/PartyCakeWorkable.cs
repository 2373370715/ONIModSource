using System;
using TUNING;

// Token: 0x02000F12 RID: 3858
public class PartyCakeWorkable : Workable
{
	// Token: 0x06004DBD RID: 19901 RVA: 0x00265B98 File Offset: 0x00263D98
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.alwaysShowProgressBar = true;
		this.resetProgressOnStop = false;
		this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_desalinator_kanim")
		};
		this.workAnims = PartyCakeWorkable.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			PartyCakeWorkable.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			PartyCakeWorkable.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x06004DBE RID: 19902 RVA: 0x000D279B File Offset: 0x000D099B
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		base.OnWorkTick(worker, dt);
		base.GetComponent<KBatchedAnimController>().SetPositionPercent(this.GetPercentComplete());
		return false;
	}

	// Token: 0x04003603 RID: 13827
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04003604 RID: 13828
	private static readonly HashedString PST_ANIM = new HashedString("salt_pst");
}
