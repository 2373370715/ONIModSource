using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000D1D RID: 3357
[AddComponentMenu("KMonoBehaviour/Workable/DesalinatorWorkableEmpty")]
public class DesalinatorWorkableEmpty : Workable
{
	// Token: 0x060041B2 RID: 16818 RVA: 0x0023EA58 File Offset: 0x0023CC58
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_desalinator_kanim")
		};
		this.workAnims = DesalinatorWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			DesalinatorWorkableEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			DesalinatorWorkableEmpty.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x060041B3 RID: 16819 RVA: 0x000CA815 File Offset: 0x000C8A15
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.timesCleaned++;
		base.OnCompleteWork(worker);
	}

	// Token: 0x04002CCC RID: 11468
	[Serialize]
	public int timesCleaned;

	// Token: 0x04002CCD RID: 11469
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04002CCE RID: 11470
	private static readonly HashedString PST_ANIM = new HashedString("salt_pst");
}
