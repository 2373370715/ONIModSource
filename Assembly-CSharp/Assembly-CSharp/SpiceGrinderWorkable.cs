using System.Linq;
using TUNING;
using UnityEngine;

public class SpiceGrinderWorkable : Workable, IConfigurableConsumer {
    [SerializeField]
    public Vector3 finishedSeedDropOffset;

    public SpiceGrinder.StatesInstance Grinder;

    [MyCmpAdd]
    public Notifier notifier;

    public IConfigurableConsumerOption[] GetSettingOptions() { return SpiceGrinder.SettingOptions.Values.ToArray(); }
    public IConfigurableConsumerOption   GetSelectedOption() { return Grinder.SelectedOption; }

    public void SetSelectedOption(IConfigurableConsumerOption option) {
        Grinder.OnOptionSelected(option as SpiceGrinder.Option);
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        requiredSkillPerk             = Db.Get().SkillPerks.CanSpiceGrinder.Id;
        workerStatusItem              = Db.Get().DuplicantStatusItems.Spicing;
        attributeConverter            = Db.Get().AttributeConverters.CookingSpeed;
        attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
        skillExperienceSkillGroup     = Db.Get().SkillGroups.Cooking.Id;
        skillExperienceMultiplier     = SKILLS.PART_DAY_EXPERIENCE;
        overrideAnims                 = new[] { Assets.GetAnim("anim_interacts_spice_grinder_kanim") };
        SetWorkTime(5f);
        showProgressBar      = true;
        lightEfficiencyBonus = true;
    }

    protected override void OnStartWork(WorkerBase worker) {
        if (Grinder.CurrentFood != null) {
            var num = Grinder.CurrentFood.Calories * 0.001f / 1000f;
            SetWorkTime(num                                 * 5f);
        } else {
            Debug.LogWarning("SpiceGrider attempted to start spicing with no food");
            StopWork(worker, true);
        }

        Grinder.UpdateFoodSymbol();
    }

    protected override void OnAbortWork(WorkerBase worker) {
        if (Grinder.CurrentFood == null) return;

        Grinder.UpdateFoodSymbol();
    }

    protected override void OnCompleteWork(WorkerBase worker) {
        if (Grinder.CurrentFood == null) return;

        Grinder.SpiceFood();
    }
}