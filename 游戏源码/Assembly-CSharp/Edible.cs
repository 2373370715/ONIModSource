using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A3D RID: 2621
[AddComponentMenu("KMonoBehaviour/Workable/Edible")]
public class Edible : Workable, IGameObjectEffectDescriptor, ISaveLoadable, IExtendSplitting
{
	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x0600300D RID: 12301 RVA: 0x000BF3C6 File Offset: 0x000BD5C6
	// (set) Token: 0x0600300E RID: 12302 RVA: 0x000BF3D3 File Offset: 0x000BD5D3
	public float Units
	{
		get
		{
			return this.primaryElement.Units;
		}
		set
		{
			this.primaryElement.Units = value;
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x0600300F RID: 12303 RVA: 0x000BF3E1 File Offset: 0x000BD5E1
	public float MassPerUnit
	{
		get
		{
			return this.primaryElement.MassPerUnit;
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06003010 RID: 12304 RVA: 0x000BF3EE File Offset: 0x000BD5EE
	// (set) Token: 0x06003011 RID: 12305 RVA: 0x000BF402 File Offset: 0x000BD602
	public float Calories
	{
		get
		{
			return this.Units * this.foodInfo.CaloriesPerUnit;
		}
		set
		{
			this.Units = value / this.foodInfo.CaloriesPerUnit;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06003012 RID: 12306 RVA: 0x000BF417 File Offset: 0x000BD617
	// (set) Token: 0x06003013 RID: 12307 RVA: 0x000BF41F File Offset: 0x000BD61F
	public EdiblesManager.FoodInfo FoodInfo
	{
		get
		{
			return this.foodInfo;
		}
		set
		{
			this.foodInfo = value;
			this.FoodID = this.foodInfo.Id;
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x06003014 RID: 12308 RVA: 0x000BF439 File Offset: 0x000BD639
	// (set) Token: 0x06003015 RID: 12309 RVA: 0x000BF441 File Offset: 0x000BD641
	public bool isBeingConsumed { get; private set; }

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06003016 RID: 12310 RVA: 0x000BF44A File Offset: 0x000BD64A
	public List<SpiceInstance> Spices
	{
		get
		{
			return this.spices;
		}
	}

	// Token: 0x06003017 RID: 12311 RVA: 0x001FA6FC File Offset: 0x001F88FC
	protected override void OnPrefabInit()
	{
		this.primaryElement = base.GetComponent<PrimaryElement>();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.shouldTransferDiseaseWithWorker = false;
		base.OnPrefabInit();
		if (this.foodInfo == null)
		{
			if (this.FoodID == null)
			{
				global::Debug.LogError("No food FoodID");
			}
			this.foodInfo = EdiblesManager.GetFoodInfo(this.FoodID);
		}
		base.Subscribe<Edible>(748399584, Edible.OnCraftDelegate);
		base.Subscribe<Edible>(1272413801, Edible.OnCraftDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Eating;
		this.synchronizeAnims = false;
		Components.Edibles.Add(this);
	}

	// Token: 0x06003018 RID: 12312 RVA: 0x001FA7B0 File Offset: 0x001F89B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ToggleGenericSpicedTag(base.gameObject.HasTag(GameTags.SpicedFood));
		if (this.spices != null)
		{
			for (int i = 0; i < this.spices.Count; i++)
			{
				this.ApplySpiceEffects(this.spices[i], SpiceGrinderConfig.SpicedStatus);
			}
		}
		if (base.GetComponent<KPrefabID>().HasTag(GameTags.Rehydrated))
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.RehydratedFood, null);
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.Edible, this);
	}

	// Token: 0x06003019 RID: 12313 RVA: 0x001FA868 File Offset: 0x001F8A68
	public override HashedString[] GetWorkAnims(WorkerBase worker)
	{
		EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
		bool flag = smi != null && smi.UseSalt();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null && component.CurrentHat != null)
		{
			if (!flag)
			{
				return Edible.hatWorkAnims;
			}
			return Edible.saltHatWorkAnims;
		}
		else
		{
			if (!flag)
			{
				return Edible.normalWorkAnims;
			}
			return Edible.saltWorkAnims;
		}
	}

	// Token: 0x0600301A RID: 12314 RVA: 0x001FA8C0 File Offset: 0x001F8AC0
	public override HashedString[] GetWorkPstAnims(WorkerBase worker, bool successfully_completed)
	{
		EatChore.StatesInstance smi = worker.GetSMI<EatChore.StatesInstance>();
		bool flag = smi != null && smi.UseSalt();
		MinionResume component = worker.GetComponent<MinionResume>();
		if (component != null && component.CurrentHat != null)
		{
			if (!flag)
			{
				return Edible.hatWorkPstAnim;
			}
			return Edible.saltHatWorkPstAnim;
		}
		else
		{
			if (!flag)
			{
				return Edible.normalWorkPstAnim;
			}
			return Edible.saltWorkPstAnim;
		}
	}

	// Token: 0x0600301B RID: 12315 RVA: 0x000BF452 File Offset: 0x000BD652
	private void OnCraft(object data)
	{
		WorldResourceAmountTracker<RationTracker>.Get().RegisterAmountProduced(this.Calories);
	}

	// Token: 0x0600301C RID: 12316 RVA: 0x001FA918 File Offset: 0x001F8B18
	public float GetFeedingTime(WorkerBase worker)
	{
		float num = this.Calories * 2E-05f;
		if (worker != null)
		{
			BingeEatChore.StatesInstance smi = worker.GetSMI<BingeEatChore.StatesInstance>();
			if (smi != null && smi.IsBingeEating())
			{
				num /= 2f;
			}
		}
		return num;
	}

	// Token: 0x0600301D RID: 12317 RVA: 0x001FA958 File Offset: 0x001F8B58
	protected override void OnStartWork(WorkerBase worker)
	{
		this.totalFeedingTime = this.GetFeedingTime(worker);
		base.SetWorkTime(this.totalFeedingTime);
		this.caloriesConsumed = 0f;
		this.unitsConsumed = 0f;
		this.totalUnits = this.Units;
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		this.totalConsumableCalories = this.Units * this.foodInfo.CaloriesPerUnit;
		this.StartConsuming();
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x001FA9D0 File Offset: 0x001F8BD0
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.currentlyLit)
		{
			if (this.currentModifier != this.caloriesLitSpaceModifier)
			{
				worker.GetAttributes().Remove(this.currentModifier);
				worker.GetAttributes().Add(this.caloriesLitSpaceModifier);
				this.currentModifier = this.caloriesLitSpaceModifier;
			}
		}
		else if (this.currentModifier != this.caloriesModifier)
		{
			worker.GetAttributes().Remove(this.currentModifier);
			worker.GetAttributes().Add(this.caloriesModifier);
			this.currentModifier = this.caloriesModifier;
		}
		return this.OnTickConsume(worker, dt);
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x000BF464 File Offset: 0x000BD664
	protected override void OnStopWork(WorkerBase worker)
	{
		if (this.currentModifier != null)
		{
			worker.GetAttributes().Remove(this.currentModifier);
			this.currentModifier = null;
		}
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		this.StopConsuming(worker);
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x001FAA68 File Offset: 0x001F8C68
	private bool OnTickConsume(WorkerBase worker, float dt)
	{
		if (!this.isBeingConsumed)
		{
			DebugUtil.DevLogError("OnTickConsume while we're not eating, this would set a NaN mass on this Edible");
			return true;
		}
		bool result = false;
		float num = dt / this.totalFeedingTime;
		float num2 = num * this.totalConsumableCalories;
		if (this.caloriesConsumed + num2 > this.totalConsumableCalories)
		{
			num2 = this.totalConsumableCalories - this.caloriesConsumed;
		}
		this.caloriesConsumed += num2;
		worker.GetAmounts().Get("Calories").value += num2;
		float num3 = this.totalUnits * num;
		if (this.Units - num3 < 0f)
		{
			num3 = this.Units;
		}
		this.Units -= num3;
		this.unitsConsumed += num3;
		if (this.Units <= 0f)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x000BF49D File Offset: 0x000BD69D
	public void SpiceEdible(SpiceInstance spice, StatusItem status)
	{
		this.spices.Add(spice);
		this.ApplySpiceEffects(spice, status);
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x001FAB34 File Offset: 0x001F8D34
	protected virtual void ApplySpiceEffects(SpiceInstance spice, StatusItem status)
	{
		base.GetComponent<KPrefabID>().AddTag(spice.Id, true);
		this.ToggleGenericSpicedTag(true);
		base.GetComponent<KSelectable>().AddStatusItem(status, this.spices);
		if (spice.FoodModifier != null)
		{
			base.gameObject.GetAttributes().Add(spice.FoodModifier);
		}
		if (spice.CalorieModifier != null)
		{
			this.Calories += spice.CalorieModifier.Value;
		}
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x001FABB0 File Offset: 0x001F8DB0
	private void ToggleGenericSpicedTag(bool isSpiced)
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (isSpiced)
		{
			component.RemoveTag(GameTags.UnspicedFood);
			component.AddTag(GameTags.SpicedFood, true);
			return;
		}
		component.RemoveTag(GameTags.SpicedFood);
		component.AddTag(GameTags.UnspicedFood, false);
	}

	// Token: 0x06003024 RID: 12324 RVA: 0x001FABF8 File Offset: 0x001F8DF8
	public bool CanAbsorb(Edible other)
	{
		bool flag = this.spices.Count == other.spices.Count;
		flag &= (base.gameObject.HasTag(GameTags.Rehydrated) == other.gameObject.HasTag(GameTags.Rehydrated));
		flag &= (!base.gameObject.HasTag(GameTags.Dehydrated) && !other.gameObject.HasTag(GameTags.Dehydrated));
		int num = 0;
		while (flag && num < this.spices.Count)
		{
			int num2 = 0;
			while (flag && num2 < other.spices.Count)
			{
				flag = (this.spices[num].Id == other.spices[num2].Id);
				num2++;
			}
			num++;
		}
		return flag;
	}

	// Token: 0x06003025 RID: 12325 RVA: 0x000BF4B3 File Offset: 0x000BD6B3
	private void StartConsuming()
	{
		DebugUtil.DevAssert(!this.isBeingConsumed, "Can't StartConsuming()...we've already started", null);
		this.isBeingConsumed = true;
		base.worker.Trigger(1406130139, this);
	}

	// Token: 0x06003026 RID: 12326 RVA: 0x001FACCC File Offset: 0x001F8ECC
	private void StopConsuming(WorkerBase worker)
	{
		DebugUtil.DevAssert(this.isBeingConsumed, "StopConsuming() called without StartConsuming()", null);
		this.isBeingConsumed = false;
		for (int i = 0; i < this.foodInfo.Effects.Count; i++)
		{
			worker.GetComponent<Effects>().Add(this.foodInfo.Effects[i], true);
		}
		ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -this.caloriesConsumed, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.EATEN, "{0}", this.GetProperName()), worker.GetProperName());
		this.AddOnConsumeEffects(worker);
		worker.Trigger(1121894420, this);
		base.Trigger(-10536414, worker.gameObject);
		this.unitsConsumed = float.NaN;
		this.caloriesConsumed = float.NaN;
		this.totalUnits = float.NaN;
		if (this.Units < 0.001f)
		{
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x06003027 RID: 12327 RVA: 0x000BF4E1 File Offset: 0x000BD6E1
	public static string GetEffectForFoodQuality(int qualityLevel)
	{
		qualityLevel = Mathf.Clamp(qualityLevel, -1, 5);
		return Edible.qualityEffects[qualityLevel];
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x001FADBC File Offset: 0x001F8FBC
	private void AddOnConsumeEffects(WorkerBase worker)
	{
		int num = Mathf.RoundToInt(worker.GetAttributes().Add(Db.Get().Attributes.FoodExpectation).GetTotalValue());
		int qualityLevel = this.FoodInfo.Quality + num;
		Effects component = worker.GetComponent<Effects>();
		component.Add(Edible.GetEffectForFoodQuality(qualityLevel), true);
		for (int i = 0; i < this.spices.Count; i++)
		{
			Effect statBonus = this.spices[i].StatBonus;
			if (statBonus != null)
			{
				float duration = statBonus.duration;
				statBonus.duration = this.caloriesConsumed * 0.001f / 1000f * 600f;
				component.Add(statBonus, true);
				statBonus.duration = duration;
			}
		}
		if (base.gameObject.HasTag(GameTags.Rehydrated))
		{
			component.Add(FoodRehydratorConfig.RehydrationEffect, true);
		}
	}

	// Token: 0x06003029 RID: 12329 RVA: 0x000BF4F8 File Offset: 0x000BD6F8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Edibles.Remove(this);
	}

	// Token: 0x0600302A RID: 12330 RVA: 0x000BF50B File Offset: 0x000BD70B
	public int GetQuality()
	{
		return this.foodInfo.Quality;
	}

	// Token: 0x0600302B RID: 12331 RVA: 0x001FAE9C File Offset: 0x001F909C
	public int GetMorale()
	{
		int num = 0;
		string effectForFoodQuality = Edible.GetEffectForFoodQuality(this.foodInfo.Quality);
		foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(effectForFoodQuality).SelfModifiers)
		{
			if (attributeModifier.AttributeId == Db.Get().Attributes.QualityOfLife.Id)
			{
				num += Mathf.RoundToInt(attributeModifier.Value);
			}
		}
		return num;
	}

	// Token: 0x0600302C RID: 12332 RVA: 0x001FAF3C File Offset: 0x001F913C
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.CALORIES, GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.CALORIES, GameUtil.GetFormattedCalories(this.foodInfo.CaloriesPerUnit, GameUtil.TimeSlice.None, true)), Descriptor.DescriptorType.Information, false));
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_QUALITY, GameUtil.GetFormattedFoodQuality(this.foodInfo.Quality)), Descriptor.DescriptorType.Effect, false));
		int morale = this.GetMorale();
		list.Add(new Descriptor(string.Format(UI.GAMEOBJECTEFFECTS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0)), string.Format(UI.GAMEOBJECTEFFECTS.TOOLTIPS.FOOD_MORALE, GameUtil.AddPositiveSign(morale.ToString(), morale > 0)), Descriptor.DescriptorType.Effect, false));
		foreach (string text in this.foodInfo.Effects)
		{
			string text2 = "";
			foreach (AttributeModifier attributeModifier in Db.Get().effects.Get(text).SelfModifiers)
			{
				text2 = string.Concat(new string[]
				{
					text2,
					"\n    • ",
					Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME"),
					": ",
					attributeModifier.GetFormattedString()
				});
			}
			list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".DESCRIPTION") + text2, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x0600302D RID: 12333 RVA: 0x001FB198 File Offset: 0x001F9398
	public void ApplySpicesToOtherEdible(Edible other)
	{
		if (this.spices != null && other != null)
		{
			for (int i = 0; i < this.spices.Count; i++)
			{
				other.SpiceEdible(this.spices[i], SpiceGrinderConfig.SpicedStatus);
			}
		}
	}

	// Token: 0x0600302E RID: 12334 RVA: 0x001FB1E4 File Offset: 0x001F93E4
	public void OnSplitTick(Pickupable thePieceTaken)
	{
		Edible component = thePieceTaken.GetComponent<Edible>();
		this.ApplySpicesToOtherEdible(component);
		if (base.GetComponent<KPrefabID>().HasTag(GameTags.Rehydrated))
		{
			component.AddTag(GameTags.Rehydrated);
		}
	}

	// Token: 0x04002064 RID: 8292
	private PrimaryElement primaryElement;

	// Token: 0x04002065 RID: 8293
	public string FoodID;

	// Token: 0x04002066 RID: 8294
	private EdiblesManager.FoodInfo foodInfo;

	// Token: 0x04002068 RID: 8296
	public float unitsConsumed = float.NaN;

	// Token: 0x04002069 RID: 8297
	public float caloriesConsumed = float.NaN;

	// Token: 0x0400206A RID: 8298
	private float totalFeedingTime = float.NaN;

	// Token: 0x0400206B RID: 8299
	private float totalUnits = float.NaN;

	// Token: 0x0400206C RID: 8300
	private float totalConsumableCalories = float.NaN;

	// Token: 0x0400206D RID: 8301
	[Serialize]
	private List<SpiceInstance> spices = new List<SpiceInstance>();

	// Token: 0x0400206E RID: 8302
	private AttributeModifier caloriesModifier = new AttributeModifier("CaloriesDelta", 50000f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, false, true, true);

	// Token: 0x0400206F RID: 8303
	private AttributeModifier caloriesLitSpaceModifier = new AttributeModifier("CaloriesDelta", (1f + DUPLICANTSTATS.STANDARD.Light.LIGHT_WORK_EFFICIENCY_BONUS) / 2E-05f, DUPLICANTS.MODIFIERS.EATINGCALORIES.NAME, false, true, true);

	// Token: 0x04002070 RID: 8304
	private AttributeModifier currentModifier;

	// Token: 0x04002071 RID: 8305
	private static readonly EventSystem.IntraObjectHandler<Edible> OnCraftDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate(Edible component, object data)
	{
		component.OnCraft(data);
	});

	// Token: 0x04002072 RID: 8306
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04002073 RID: 8307
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre",
		"working_loop"
	};

	// Token: 0x04002074 RID: 8308
	private static readonly HashedString[] saltWorkAnims = new HashedString[]
	{
		"salt_pre",
		"salt_loop"
	};

	// Token: 0x04002075 RID: 8309
	private static readonly HashedString[] saltHatWorkAnims = new HashedString[]
	{
		"salt_hat_pre",
		"salt_hat_loop"
	};

	// Token: 0x04002076 RID: 8310
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04002077 RID: 8311
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"hat_pst"
	};

	// Token: 0x04002078 RID: 8312
	private static readonly HashedString[] saltWorkPstAnim = new HashedString[]
	{
		"salt_pst"
	};

	// Token: 0x04002079 RID: 8313
	private static readonly HashedString[] saltHatWorkPstAnim = new HashedString[]
	{
		"salt_hat_pst"
	};

	// Token: 0x0400207A RID: 8314
	private static Dictionary<int, string> qualityEffects = new Dictionary<int, string>
	{
		{
			-1,
			"EdibleMinus3"
		},
		{
			0,
			"EdibleMinus2"
		},
		{
			1,
			"EdibleMinus1"
		},
		{
			2,
			"Edible0"
		},
		{
			3,
			"Edible1"
		},
		{
			4,
			"Edible2"
		},
		{
			5,
			"Edible3"
		}
	};

	// Token: 0x02000A3E RID: 2622
	public class EdibleStartWorkInfo : WorkerBase.StartWorkInfo
	{
		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06003031 RID: 12337 RVA: 0x000BF518 File Offset: 0x000BD718
		// (set) Token: 0x06003032 RID: 12338 RVA: 0x000BF520 File Offset: 0x000BD720
		public float amount { get; private set; }

		// Token: 0x06003033 RID: 12339 RVA: 0x000BF529 File Offset: 0x000BD729
		public EdibleStartWorkInfo(Workable workable, float amount) : base(workable)
		{
			this.amount = amount;
		}
	}
}
