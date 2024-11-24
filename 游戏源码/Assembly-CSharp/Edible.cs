using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Edible")]
public class Edible : Workable, IGameObjectEffectDescriptor, ISaveLoadable, IExtendSplitting
{
	public class EdibleStartWorkInfo : Worker.StartWorkInfo
	{
		public float amount { get; private set; }

		public EdibleStartWorkInfo(Workable workable, float amount)
			: base(workable)
		{
			this.amount = amount;
		}
	}

	private PrimaryElement primaryElement;

	public string FoodID;

	private EdiblesManager.FoodInfo foodInfo;

	public float unitsConsumed = float.NaN;

	public float caloriesConsumed = float.NaN;

	private float totalFeedingTime = float.NaN;

	private float totalUnits = float.NaN;

	private float totalConsumableCalories = float.NaN;

	[Serialize]
	private List<SpiceInstance> spices = new List<SpiceInstance>();

	private AttributeModifier caloriesModifier = new AttributeModifier("CaloriesDelta", 50000f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, is_multiplier: false, uiOnly: true);

	private AttributeModifier caloriesLitSpaceModifier = new AttributeModifier("CaloriesDelta", 57500f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, is_multiplier: false, uiOnly: true);

	private AttributeModifier currentModifier;

	private static readonly EventSystem.IntraObjectHandler<Edible> OnCraftDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		component.OnCraft(data);
	});

	private static readonly HashedString[] normalWorkAnims = new HashedString[2] { "working_pre", "working_loop" };

	private static readonly HashedString[] hatWorkAnims = new HashedString[2] { "hat_pre", "working_loop" };

	private static readonly HashedString[] saltWorkAnims = new HashedString[2] { "salt_pre", "salt_loop" };

	private static readonly HashedString[] saltHatWorkAnims = new HashedString[2] { "salt_hat_pre", "salt_hat_loop" };

	private static readonly HashedString[] normalWorkPstAnim = new HashedString[1] { "working_pst" };

	private static readonly HashedString[] hatWorkPstAnim = new HashedString[1] { "hat_pst" };

	private static readonly HashedString[] saltWorkPstAnim = new HashedString[1] { "salt_pst" };

	private static readonly HashedString[] saltHatWorkPstAnim = new HashedString[1] { "salt_hat_pst" };

	private static Dictionary<int, string> qualityEffects = new Dictionary<int, string>
	{
		{ -1, "EdibleMinus3" },
		{ 0, "EdibleMinus2" },
		{ 1, "EdibleMinus1" },
		{ 2, "Edible0" },
		{ 3, "Edible1" },
		{ 4, "Edible2" },
		{ 5, "Edible3" }
	};

	public float Units
	{
		get
		{
			return primaryElement.Units;
		}
		set
		{
			primaryElement.Units = value;
		}
	}

	public float MassPerUnit => primaryElement.MassPerUnit;

	public float Calories
	{
		get
		{
			return Units * foodInfo.CaloriesPerUnit;
		}
		set
		{
			Units = value / foodInfo.CaloriesPerUnit;
		}
	}

	public EdiblesManager.FoodInfo FoodInfo
	{
		get
		{
			return foodInfo;
		}
		set
		{
			foodInfo = value;
			FoodID = foodInfo.Id;
		}
	}

	public bool isBeingConsumed { get; private set; }

	public List<SpiceInstance> Spices => spices;

	protected override void OnPrefabInit()
	{
		primaryElement = GetComponent<PrimaryElement>();
		SetReportType(ReportManager.ReportType.PersonalTime);
		showProgressBar = false;
		SetOffsetTable(OffsetGroups.InvertedStandardTable);
		shouldTransferDiseaseWithWorker = false;
		base.OnPrefabInit();
		if (foodInfo == null)
		{
			if (FoodID == null)
			{
				Debug.LogError("No food FoodID");
			}
			foodInfo = EdiblesManager.GetFoodInfo(FoodID);
		}
		Subscribe(748399584, OnCraftDelegate);
		Subscribe(1272413801, OnCraftDelegate);
		workerStatusItem = Db.Get().DuplicantStatusItems.Eating;
		synchronizeAnims = false;
		Components.Edibles.Add(this);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		ToggleGenericSpicedTag(base.gameObject.HasTag(GameTags.SpicedFood));
		if (spices != null)
		{
			for (int i = 0; i < spices.Count; i++)
			{
				ApplySpiceEffects(spices[i], SpiceGrinderConfig.SpicedStatus);
			}
		}
		if (GetComponent<KPrefabID>().HasTag(GameTags.Rehydrated))
		{
			GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.RehydratedFood);
		}
		GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.Edible, this);
	}

	public override HashedString[] GetWorkAnims(Worker worker)
	{
		bool flag = worker.GetSMI<EatChore.StatesInstance>()?.UseSalt() ?? false;
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null && component.CurrentHat != null)
		{
			if (!flag)
			{
				return hatWorkAnims;
			}
			return saltHatWorkAnims;
		}
		if (!flag)
		{
			return normalWorkAnims;
		}
		return saltWorkAnims;
	}

	public override HashedString[] GetWorkPstAnims(Worker worker, bool successfully_completed)
	{
		bool flag = worker.GetSMI<EatChore.StatesInstance>()?.UseSalt() ?? false;
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null && component.CurrentHat != null)
		{
			if (!flag)
			{
				return hatWorkPstAnim;
			}
			return saltHatWorkPstAnim;
		}
		if (!flag)
		{
			return normalWorkPstAnim;
		}
		return saltWorkPstAnim;
	}

	private void OnCraft(object data)
	{
		RationTracker.Get().RegisterCaloriesProduced(Calories);
	}

	public float GetFeedingTime(Worker worker)
	{
		float num = Calories * 2E-05f;
		if (worker != null)
		{
			BingeEatChore.StatesInstance sMI = worker.GetSMI<BingeEatChore.StatesInstance>();
			if (sMI != null && sMI.IsBingeEating())
			{
				num /= 2f;
			}
		}
		return num;
	}

	protected override void OnStartWork(Worker worker)
	{
		totalFeedingTime = GetFeedingTime(worker);
		SetWorkTime(totalFeedingTime);
		caloriesConsumed = 0f;
		unitsConsumed = 0f;
		totalUnits = Units;
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse);
		totalConsumableCalories = Units * foodInfo.CaloriesPerUnit;
		StartConsuming();
	}

	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (currentlyLit)
		{
			if (currentModifier != caloriesLitSpaceModifier)
			{
				worker.GetAttributes().Remove(currentModifier);
				worker.GetAttributes().Add(caloriesLitSpaceModifier);
				currentModifier = caloriesLitSpaceModifier;
			}
		}
		else if (currentModifier != caloriesModifier)
		{
			worker.GetAttributes().Remove(currentModifier);
			worker.GetAttributes().Add(caloriesModifier);
			currentModifier = caloriesModifier;
		}
		return OnTickConsume(worker, dt);
	}

	protected override void OnStopWork(Worker worker)
	{
		if (currentModifier != null)
		{
			worker.GetAttributes().Remove(currentModifier);
			currentModifier = null;
		}
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		StopConsuming(worker);
	}

	private bool OnTickConsume(Worker worker, float dt)
	{
		if (!isBeingConsumed)
		{
			DebugUtil.DevLogError("OnTickConsume while we're not eating, this would set a NaN mass on this Edible");
			return true;
		}
		bool result = false;
		float num = dt / totalFeedingTime;
		float num2 = num * totalConsumableCalories;
		if (caloriesConsumed + num2 > totalConsumableCalories)
		{
			num2 = totalConsumableCalories - caloriesConsumed;
		}
		caloriesConsumed += num2;
		worker.GetAmounts().Get("Calories").value += num2;
		float num3 = totalUnits * num;
		if (Units - num3 < 0f)
		{
			num3 = Units;
		}
		Units -= num3;
		unitsConsumed += num3;
		if (Units <= 0f)
		{
			result = true;
		}
		return result;
	}

	public void SpiceEdible(SpiceInstance spice, StatusItem status)
	{
		spices.Add(spice);
		ApplySpiceEffects(spice, status);
	}

	protected virtual void ApplySpiceEffects(SpiceInstance spice, StatusItem status)
	{
		GetComponent<KPrefabID>().AddTag(spice.Id, serialize: true);
		ToggleGenericSpicedTag(isSpiced: true);
		GetComponent<KSelectable>().AddStatusItem(status, spices);
		if (spice.FoodModifier != null)
		{
			base.gameObject.GetAttributes().Add(spice.FoodModifier);
		}
		if (spice.CalorieModifier != null)
		{
			Calories += spice.CalorieModifier.Value;
		}
	}

	private void ToggleGenericSpicedTag(bool isSpiced)
	{
		KPrefabID component = GetComponent<KPrefabID>();
		if (isSpiced)
		{
			component.RemoveTag(GameTags.UnspicedFood);
			component.AddTag(GameTags.SpicedFood, serialize: true);
		}
		else
		{
			component.RemoveTag(GameTags.SpicedFood);
			component.AddTag(GameTags.UnspicedFood);
		}
	}

	public bool CanAbsorb(Edible other)
	{
		bool flag = spices.Count == other.spices.Count;
		flag &= base.gameObject.HasTag(GameTags.Rehydrated) == other.gameObject.HasTag(GameTags.Rehydrated);
		flag &= !base.gameObject.HasTag(GameTags.Dehydrated) && !other.gameObject.HasTag(GameTags.Dehydrated);
		int num = 0;
		while (flag && num < spices.Count)
		{
			int num2 = 0;
			while (flag && num2 < other.spices.Count)
			{
				flag = spices[num].Id == other.spices[num2].Id;
				num2++;
			}
			num++;
		}
		return flag;
	}

	private void StartConsuming()
	{
		DebugUtil.DevAssert(!isBeingConsumed, "Can't StartConsuming()...we've already started");
		isBeingConsumed = true;
		base.worker.Trigger(1406130139, this);
	}

	private void StopConsuming(Worker worker)
	{
		DebugUtil.DevAssert(isBeingConsumed, "StopConsuming() called without StartConsuming()");
		isBeingConsumed = false;
		for (int i = 0; i < foodInfo.Effects.Count; i++)
		{
			worker.GetComponent<Effects>().Add(foodInfo.Effects[i], should_save: true);
		}
		ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, 0f - caloriesConsumed, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.EATEN, "{0}", this.GetProperName()), worker.GetProperName());
		AddOnConsumeEffects(worker);
		worker.Trigger(1121894420, this);
		Trigger(-10536414, worker.gameObject);
		unitsConsumed = float.NaN;
		caloriesConsumed = float.NaN;
		totalUnits = float.NaN;
		if (Units < 0.001f)
		{
			base.gameObject.DeleteObject();
		}
	}

	public static string GetEffectForFoodQuality(int qualityLevel)
	{
		qualityLevel = Mathf.Clamp(qualityLevel, -1, 5);
		return qualityEffects[qualityLevel];
	}

	private void AddOnConsumeEffects(Worker worker)
	{
		int num = Mathf.RoundToInt(worker.GetAttributes().Add(Db.Get().Attributes.FoodExpectation).GetTotalValue());
		int qualityLevel = FoodInfo.Quality + num;
		Effects component = worker.GetComponent<Effects>();
		component.Add(GetEffectForFoodQuality(qualityLevel), should_save: true);
		for (int i = 0; i < spices.Count; i++)
		{
			Effect statBonus = spices[i].StatBonus;
			if (statBonus != null)
			{
				float duration = statBonus.duration;
				statBonus.duration = caloriesConsumed * 0.001f / 1000f * 600f;
				component.Add(statBonus, should_save: true);
				statBonus.duration = duration;
			}
		}
		if (base.gameObject.HasTag(GameTags.Rehydrated))
		{
			component.Add(FoodRehydratorConfig.RehydrationEffect, should_save: true);
		}
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Edibles.Remove(this);
	}

	public int GetQuality()
	{
		return foodInfo.Quality;
	}

	public int GetMorale()
	{
		int num = 0;
		string effectForFoodQuality = GetEffectForFoodQuality(foodInfo.Quality);
		foreach (AttributeModifier selfModifier in Db.Get().effects.Get(effectForFoodQuality).SelfModifiers)
		{
			if (selfModifier.AttributeId == Db.Get().Attributes.QualityOfLife.Id)
			{
				num += Mathf.RoundToInt(selfModifier.Value);
			}
		}
		return num;
	}

	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.CALORIES, GameUtil.GetFormattedCalories(foodInfo.CaloriesPerUnit)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.CALORIES, GameUtil.GetFormattedCalories(foodInfo.CaloriesPerUnit)), Descriptor.DescriptorType.Information));
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(foodInfo.Quality)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(foodInfo.Quality))));
		int morale = GetMorale();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0))));
		foreach (string effect in foodInfo.Effects)
		{
			string text = "";
			foreach (AttributeModifier selfModifier in Db.Get().effects.Get(effect).SelfModifiers)
			{
				text = string.Concat(text, "\n    • ", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + selfModifier.AttributeId.ToUpper() + ".NAME"), ": ", selfModifier.GetFormattedString());
			}
			list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + effect.ToUpper() + ".NAME"), string.Concat(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + effect.ToUpper() + ".DESCRIPTION"), text)));
		}
		return list;
	}

	public void ApplySpicesToOtherEdible(Edible other)
	{
		if (spices != null && other != null)
		{
			for (int i = 0; i < spices.Count; i++)
			{
				other.SpiceEdible(spices[i], SpiceGrinderConfig.SpicedStatus);
			}
		}
	}

	public void OnSplitTick(Pickupable thePieceTaken)
	{
		Edible component = thePieceTaken.GetComponent<Edible>();
		ApplySpicesToOtherEdible(component);
		if (GetComponent<KPrefabID>().HasTag(GameTags.Rehydrated))
		{
			component.AddTag(GameTags.Rehydrated);
		}
	}
}
