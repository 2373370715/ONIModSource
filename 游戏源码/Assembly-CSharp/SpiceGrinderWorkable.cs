using System;
using System.Linq;
using TUNING;
using UnityEngine;

// Token: 0x02000FAF RID: 4015
public class SpiceGrinderWorkable : Workable, IConfigurableConsumer
{
	// Token: 0x06005137 RID: 20791 RVA: 0x00270DAC File Offset: 0x0026EFAC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanSpiceGrinder.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Spicing;
		this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_spice_grinder_kanim")
		};
		base.SetWorkTime(5f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06005138 RID: 20792 RVA: 0x00270E6C File Offset: 0x0026F06C
	protected override void OnStartWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood != null)
		{
			float num = this.Grinder.CurrentFood.Calories * 0.001f / 1000f;
			base.SetWorkTime(num * 5f);
		}
		else
		{
			global::Debug.LogWarning("SpiceGrider attempted to start spicing with no food");
			base.StopWork(worker, true);
		}
		this.Grinder.UpdateFoodSymbol();
	}

	// Token: 0x06005139 RID: 20793 RVA: 0x000D4F1F File Offset: 0x000D311F
	protected override void OnAbortWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood == null)
		{
			return;
		}
		this.Grinder.UpdateFoodSymbol();
	}

	// Token: 0x0600513A RID: 20794 RVA: 0x000D4F40 File Offset: 0x000D3140
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood == null)
		{
			return;
		}
		this.Grinder.SpiceFood();
	}

	// Token: 0x0600513B RID: 20795 RVA: 0x00270ED8 File Offset: 0x0026F0D8
	public IConfigurableConsumerOption[] GetSettingOptions()
	{
		return SpiceGrinder.SettingOptions.Values.ToArray<SpiceGrinder.Option>();
	}

	// Token: 0x0600513C RID: 20796 RVA: 0x000D4F61 File Offset: 0x000D3161
	public IConfigurableConsumerOption GetSelectedOption()
	{
		return this.Grinder.SelectedOption;
	}

	// Token: 0x0600513D RID: 20797 RVA: 0x000D4F6E File Offset: 0x000D316E
	public void SetSelectedOption(IConfigurableConsumerOption option)
	{
		this.Grinder.OnOptionSelected(option as SpiceGrinder.Option);
	}

	// Token: 0x040038BB RID: 14523
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x040038BC RID: 14524
	[SerializeField]
	public Vector3 finishedSeedDropOffset;

	// Token: 0x040038BD RID: 14525
	public SpiceGrinder.StatesInstance Grinder;
}
