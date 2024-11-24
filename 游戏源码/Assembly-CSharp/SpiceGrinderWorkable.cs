using System.Linq;
using TUNING;
using UnityEngine;

public class SpiceGrinderWorkable : Workable, IConfigurableConsumer
{
	[MyCmpAdd]
	public Notifier notifier;

	[SerializeField]
	public Vector3 finishedSeedDropOffset;

	public SpiceGrinder.StatesInstance Grinder;

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		requiredSkillPerk = Db.Get().SkillPerks.CanSpiceGrinder.Id;
		workerStatusItem = Db.Get().DuplicantStatusItems.Spicing;
		attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		skillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		overrideAnims = new KAnimFile[1] { Assets.GetAnim("anim_interacts_spice_grinder_kanim") };
		SetWorkTime(5f);
		showProgressBar = true;
		lightEfficiencyBonus = true;
	}

	protected override void OnStartWork(Worker worker)
	{
		if (Grinder.CurrentFood != null)
		{
			float num = Grinder.CurrentFood.Calories * 0.001f / 1000f;
			SetWorkTime(num * 5f);
		}
		else
		{
			Debug.LogWarning("SpiceGrider attempted to start spicing with no food");
			StopWork(worker, aborted: true);
		}
		Grinder.UpdateFoodSymbol();
	}

	protected override void OnAbortWork(Worker worker)
	{
		if (!(Grinder.CurrentFood == null))
		{
			Grinder.UpdateFoodSymbol();
		}
	}

	protected override void OnCompleteWork(Worker worker)
	{
		if (!(Grinder.CurrentFood == null))
		{
			Grinder.SpiceFood();
		}
	}

	public IConfigurableConsumerOption[] GetSettingOptions()
	{
		return SpiceGrinder.SettingOptions.Values.ToArray();
	}

	public IConfigurableConsumerOption GetSelectedOption()
	{
		return Grinder.SelectedOption;
	}

	public void SetSelectedOption(IConfigurableConsumerOption option)
	{
		Grinder.OnOptionSelected(option as SpiceGrinder.Option);
	}
}
