using TUNING;

public class SpaceTreeSyrupHarvestWorkable : Workable {
    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        SetWorkerStatusItem(Db.Get().DuplicantStatusItems.Harvesting);
        workAnims                     = new HashedString[] { "syrup_harvest_trunk_pre", "syrup_harvest_trunk_loop" };
        workingPstComplete            = new HashedString[] { "syrup_harvest_trunk_pst" };
        workingPstFailed              = new HashedString[] { "syrup_harvest_trunk_loop" };
        attributeConverter            = Db.Get().AttributeConverters.HarvestSpeed;
        attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
        skillExperienceSkillGroup     = Db.Get().SkillGroups.Farming.Id;
        skillExperienceMultiplier     = SKILLS.PART_DAY_EXPERIENCE;
        requiredSkillPerk             = Db.Get().SkillPerks.CanFarmTinker.Id;
        overrideAnims                 = new[] { Assets.GetAnim("anim_interacts_syrup_tree_kanim") };
        synchronizeAnims              = true;
        shouldShowSkillPerkStatusItem = false;
        SetWorkTime(10f);
        resetProgressOnStop = true;
    }

    protected override void OnSpawn()                      { base.OnSpawn(); }
    protected override void OnStartWork(WorkerBase worker) { base.OnStartWork(worker); }
}