using System;
using TUNING;
using UnityEngine;

// Token: 0x02000C93 RID: 3219
[AddComponentMenu("KMonoBehaviour/Workable/AlgaeHabitatEmpty")]
public class AlgaeHabitatEmpty : Workable
{
	// Token: 0x06003DFC RID: 15868 RVA: 0x00232FDC File Offset: 0x002311DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.workAnims = AlgaeHabitatEmpty.CLEAN_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			AlgaeHabitatEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			AlgaeHabitatEmpty.PST_ANIM
		};
		this.synchronizeAnims = false;
	}

	// Token: 0x04002A54 RID: 10836
	private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]
	{
		"sponge_pre",
		"sponge_loop"
	};

	// Token: 0x04002A55 RID: 10837
	private static readonly HashedString PST_ANIM = new HashedString("sponge_pst");
}
