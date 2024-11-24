using System;
using System.Collections.Generic;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

public class SpiceGrinder : GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>
{
	public class Option : IConfigurableConsumerOption
	{
		public readonly Tag Id;

		public readonly Spice Spice;

		private string name;

		private string fullDescription;

		private string spiceDescription;

		private string ingredientDescriptions;

		private Effect statBonus;

		public Effect StatBonus
		{
			get
			{
				if (statBonus == null)
				{
					return null;
				}
				if (string.IsNullOrEmpty(spiceDescription))
				{
					CreateDescription();
					GetName();
				}
				statBonus.Name = name;
				statBonus.description = spiceDescription;
				return statBonus;
			}
		}

		public Option(Spice spice)
		{
			Id = new Tag(spice.Id);
			Spice = spice;
			if (spice.StatBonus != null)
			{
				statBonus = new Effect(spice.Id, GetName(), spiceDescription, 600f, show_in_ui: true, trigger_floating_text: false, is_bad: false);
				statBonus.Add(spice.StatBonus);
				Db.Get().effects.Add(statBonus);
			}
		}

		public Tag GetID()
		{
			return Spice.Id;
		}

		public string GetName()
		{
			if (string.IsNullOrEmpty(name))
			{
				string text = "STRINGS.ITEMS.SPICES." + Spice.Id.ToUpper() + ".NAME";
				Strings.TryGet(text, out var result);
				name = "MISSING " + text;
				if (result != null)
				{
					name = result;
				}
			}
			return name;
		}

		public string GetDetailedDescription()
		{
			if (string.IsNullOrEmpty(fullDescription))
			{
				CreateDescription();
			}
			return fullDescription;
		}

		public string GetDescription()
		{
			if (!string.IsNullOrEmpty(spiceDescription))
			{
				return spiceDescription;
			}
			string text = "STRINGS.ITEMS.SPICES." + Spice.Id.ToUpper() + ".DESC";
			Strings.TryGet(text, out var result);
			spiceDescription = "MISSING " + text;
			if (result != null)
			{
				spiceDescription = result.String;
			}
			return spiceDescription;
		}

		private void CreateDescription()
		{
			string text = "STRINGS.ITEMS.SPICES." + Spice.Id.ToUpper() + ".DESC";
			Strings.TryGet(text, out var result);
			spiceDescription = "MISSING " + text;
			if (result != null)
			{
				spiceDescription = result.String;
			}
			ingredientDescriptions = $"\n\n<b>{BUILDINGS.PREFABS.SPICEGRINDER.INGREDIENTHEADER}</b>";
			for (int i = 0; i < Spice.Ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = Spice.Ingredients[i];
				GameObject prefab = Assets.GetPrefab((ingredient.IngredientSet != null && ingredient.IngredientSet.Length != 0) ? ingredient.IngredientSet[0] : ((Tag)null));
				ingredientDescriptions += string.Format("\n{0}{1} {2}{3}", "    • ", prefab.GetProperName(), ingredient.AmountKG, GameUtil.GetUnitTypeMassOrUnit(prefab));
			}
			fullDescription = spiceDescription + ingredientDescriptions;
		}

		public Sprite GetIcon()
		{
			return Assets.GetSprite(Spice.Image);
		}

		public IConfigurableConsumerIngredient[] GetIngredients()
		{
			return Spice.Ingredients;
		}
	}

	public class Def : BaseDef
	{
	}

	public class StatesInstance : GameInstance
	{
		private static string HASH_FOOD = "food";

		private KBatchedAnimController kbac;

		private KBatchedAnimController foodKBAC;

		[MyCmpReq]
		public RoomTracker roomTracker;

		[MyCmpReq]
		public SpiceGrinderWorkable workable;

		[Serialize]
		private int spiceHash;

		private SpiceInstance currentSpice;

		private Edible currentFood;

		private Storage seedStorage;

		private Storage foodStorage;

		private MeterController meter;

		private Tag[] foodFilter = new Tag[1];

		private FilteredStorage foodStorageFilter;

		private Operational operational;

		private Guid missingResourceStatusItem = Guid.Empty;

		private StatusItem mutantSeedStatusItem;

		private FetchChore[] SpiceFetches;

		[Serialize]
		private bool allowMutantSeeds = true;

		public bool IsOperational
		{
			get
			{
				if (operational != null)
				{
					return operational.IsOperational;
				}
				return false;
			}
		}

		public float AvailableFood
		{
			get
			{
				if (!(foodStorage == null))
				{
					return foodStorage.MassStored();
				}
				return 0f;
			}
		}

		public Option SelectedOption
		{
			get
			{
				if (!(currentSpice.Id == Tag.Invalid))
				{
					return SettingOptions[currentSpice.Id];
				}
				return null;
			}
		}

		public Edible CurrentFood
		{
			get
			{
				GameObject gameObject = foodStorage.FindFirst(GameTags.Edible);
				currentFood = ((gameObject != null) ? gameObject.GetComponent<Edible>() : null);
				return currentFood;
			}
		}

		public bool HasOpenFetches => Array.Exists(SpiceFetches, (FetchChore fetch) => fetch != null);

		public bool AllowMutantSeeds
		{
			get
			{
				return allowMutantSeeds;
			}
			set
			{
				allowMutantSeeds = value;
				ToggleMutantSeedFetches(allowMutantSeeds);
			}
		}

		public StatesInstance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			workable.Grinder = this;
			Storage[] components = base.gameObject.GetComponents<Storage>();
			foodStorage = components[0];
			seedStorage = components[1];
			operational = GetComponent<Operational>();
			kbac = GetComponent<KBatchedAnimController>();
			foodStorageFilter = new FilteredStorage(GetComponent<KPrefabID>(), foodFilter, null, use_logic_meter: false, Db.Get().ChoreTypes.CookFetch);
			foodStorageFilter.SetHasMeter(has_meter: false);
			meter = new MeterController(kbac, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, "meter_frame", "meter_level");
			SetupFoodSymbol();
			UpdateFoodSymbol();
			Subscribe(-905833192, OnCopySettings);
			base.sm.UpdateInKitchen(this);
			Prioritizable.AddRef(base.gameObject);
			Subscribe(493375141, OnRefreshUserMenu);
		}

		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Prioritizable.RemoveRef(base.gameObject);
		}

		public void Initialize()
		{
			if (DlcManager.IsExpansion1Active())
			{
				mutantSeedStatusItem = new StatusItem("SPICEGRINDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, OverlayModes.None.ID, showWorldIcon: false);
				if (AllowMutantSeeds)
				{
					KSelectable component = GetComponent<KSelectable>();
					if (component != null)
					{
						component.AddStatusItem(mutantSeedStatusItem);
					}
				}
			}
			SettingOptions.TryGetValue(new Tag(spiceHash), out var value);
			OnOptionSelected(value);
			base.sm.OnStorageChanged(this, null);
			UpdateMeter();
		}

		private void OnRefreshUserMenu(object data)
		{
			if (DlcManager.FeatureRadiationEnabled())
			{
				Game.Instance.userMenu.AddButton(base.smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", base.smi.AllowMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT, delegate
				{
					base.smi.AllowMutantSeeds = !base.smi.AllowMutantSeeds;
					OnRefreshUserMenu(base.smi);
				}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP));
			}
		}

		public void ToggleMutantSeedFetches(bool allow)
		{
			if (!DlcManager.IsExpansion1Active())
			{
				return;
			}
			UpdateMutantSeedFetches();
			if (allow)
			{
				seedStorage.storageFilters.Add(GameTags.MutatedSeed);
				KSelectable component = GetComponent<KSelectable>();
				if (component != null)
				{
					component.AddStatusItem(mutantSeedStatusItem);
				}
				return;
			}
			if (seedStorage.GetMassAvailable(GameTags.MutatedSeed) > 0f)
			{
				seedStorage.Drop(GameTags.MutatedSeed);
			}
			seedStorage.storageFilters.Remove(GameTags.MutatedSeed);
			KSelectable component2 = GetComponent<KSelectable>();
			if (component2 != null)
			{
				component2.RemoveStatusItem(mutantSeedStatusItem);
			}
		}

		private void UpdateMutantSeedFetches()
		{
			if (SpiceFetches == null)
			{
				return;
			}
			Tag[] tags = new Tag[2]
			{
				GameTags.Seed,
				GameTags.CropSeed
			};
			for (int num = SpiceFetches.Length - 1; num >= 0; num--)
			{
				FetchChore fetchChore = SpiceFetches[num];
				if (fetchChore != null)
				{
					foreach (Tag tag in SpiceFetches[num].tags)
					{
						if (Assets.GetPrefab(tag).HasAnyTags(tags))
						{
							fetchChore.Cancel("MutantSeedChanges");
							SpiceFetches[num] = CreateFetchChore(fetchChore.tags, fetchChore.amount);
						}
					}
				}
			}
		}

		private void OnCopySettings(object data)
		{
			SpiceGrinderWorkable component = ((GameObject)data).GetComponent<SpiceGrinderWorkable>();
			if (component != null)
			{
				currentSpice = component.Grinder.currentSpice;
				SettingOptions.TryGetValue(new Tag(component.Grinder.spiceHash), out var value);
				OnOptionSelected(value);
				allowMutantSeeds = component.Grinder.AllowMutantSeeds;
			}
		}

		public void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "foodSymbol");
			gameObject.SetActive(value: false);
			bool symbolVisible;
			Vector3 position = kbac.GetSymbolTransform(HASH_FOOD, out symbolVisible).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			foodKBAC.AnimFiles = new KAnimFile[1] { Assets.GetAnim("mushbar_kanim") };
			foodKBAC.initialAnim = "object";
			kbac.SetSymbolVisiblity(HASH_FOOD, is_visible: false);
		}

		public void UpdateFoodSymbol()
		{
			bool flag = AvailableFood > 0f && CurrentFood != null;
			foodKBAC.gameObject.SetActive(flag);
			if (flag)
			{
				foodKBAC.SwapAnims(CurrentFood.GetComponent<KBatchedAnimController>().AnimFiles);
				foodKBAC.Play("object", KAnim.PlayMode.Loop);
			}
		}

		public void UpdateMeter()
		{
			meter.SetPositionPercent(seedStorage.MassStored() / seedStorage.capacityKg);
		}

		public void SpiceFood()
		{
			float num = CurrentFood.Calories / 1000f;
			CurrentFood.SpiceEdible(currentSpice, SpiceGrinderConfig.SpicedStatus);
			foodStorage.Drop(CurrentFood.gameObject);
			currentFood = null;
			UpdateFoodSymbol();
			Spice.Ingredient[] ingredients = SettingOptions[currentSpice.Id].Spice.Ingredients;
			foreach (Spice.Ingredient ingredient in ingredients)
			{
				float num2 = num * ingredient.AmountKG / 1000f;
				int num3 = ingredient.IngredientSet.Length - 1;
				while (num2 > 0f && num3 >= 0)
				{
					Tag tag = ingredient.IngredientSet[num3];
					seedStorage.ConsumeAndGetDisease(tag, num2, out var amount_consumed, out var _, out var _);
					num2 -= amount_consumed;
					num3--;
				}
			}
			base.sm.isReady.Set(value: false, this);
		}

		public bool CanSpice(float kcalToSpice)
		{
			bool flag = true;
			float num = kcalToSpice / 1000f;
			Spice.Ingredient[] ingredients = SettingOptions[currentSpice.Id].Spice.Ingredients;
			Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
			for (int i = 0; i < ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = ingredients[i];
				float num2 = 0f;
				int num3 = 0;
				while (ingredient.IngredientSet != null && num3 < ingredient.IngredientSet.Length)
				{
					num2 += seedStorage.GetMassAvailable(ingredient.IngredientSet[num3]);
					num3++;
				}
				float num4 = num * ingredient.AmountKG / 1000f;
				flag = flag && num4 <= num2;
				if (num4 > num2)
				{
					dictionary.Add(ingredient.IngredientSet[0], num4 - num2);
					if (SpiceFetches != null && SpiceFetches[i] == null)
					{
						SpiceFetches[i] = CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
					}
				}
			}
			UpdateSpiceIngredientStatus(flag, dictionary);
			return flag;
		}

		private FetchChore CreateFetchChore(Tag[] ingredientIngredientSet, float amount)
		{
			return CreateFetchChore(new HashSet<Tag>(ingredientIngredientSet), amount);
		}

		private FetchChore CreateFetchChore(HashSet<Tag> ingredients, float amount)
		{
			float num = Mathf.Max(amount, 1f);
			return new FetchChore(Db.Get().ChoreTypes.CookFetch, seedStorage, num, ingredients, FetchChore.MatchCriteria.MatchID, Tag.Invalid, on_complete: ClearFetchChore, forbidden_tags: AllowMutantSeeds ? null : new Tag[1] { GameTags.MutatedSeed });
		}

		private void ClearFetchChore(Chore obj)
		{
			if (!(obj is FetchChore { isComplete: not false } fetchChore) || SpiceFetches == null)
			{
				return;
			}
			for (int num = SpiceFetches.Length - 1; num >= 0; num--)
			{
				if (SpiceFetches[num] == fetchChore)
				{
					float num2 = fetchChore.originalAmount - fetchChore.amount;
					if (num2 > 0f)
					{
						SpiceFetches[num] = CreateFetchChore(fetchChore.tags, num2);
					}
					else
					{
						SpiceFetches[num] = null;
					}
					break;
				}
			}
		}

		private void UpdateSpiceIngredientStatus(bool can_spice, Dictionary<Tag, float> missing_spices)
		{
			KSelectable component = GetComponent<KSelectable>();
			if (!can_spice)
			{
				if (missingResourceStatusItem != Guid.Empty)
				{
					missingResourceStatusItem = component.ReplaceStatusItem(missingResourceStatusItem, Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
				}
				else
				{
					missingResourceStatusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
				}
			}
			else
			{
				missingResourceStatusItem = component.RemoveStatusItem(missingResourceStatusItem);
			}
		}

		public void OnOptionSelected(Option spiceOption)
		{
			base.smi.GetComponent<Operational>().SetFlag(spiceSet, spiceOption != null);
			if (spiceOption == null)
			{
				kbac.Play("default");
				kbac.SetSymbolTint("stripe_anim2", Color.white);
			}
			else
			{
				kbac.Play(IsOperational ? "on" : "off");
			}
			CancelFetches("SpiceChanged");
			if (currentSpice.Id != Tag.Invalid)
			{
				seedStorage.DropAll();
				UpdateMeter();
				base.sm.isReady.Set(value: false, this);
			}
			if (missingResourceStatusItem != Guid.Empty)
			{
				missingResourceStatusItem = GetComponent<KSelectable>().RemoveStatusItem(missingResourceStatusItem);
			}
			if (spiceOption == null)
			{
				return;
			}
			currentSpice = new SpiceInstance
			{
				Id = spiceOption.Id,
				TotalKG = spiceOption.Spice.TotalKG
			};
			SetSpiceSymbolColours(spiceOption.Spice);
			spiceHash = currentSpice.Id.GetHash();
			seedStorage.capacityKg = currentSpice.TotalKG * 10f;
			Spice.Ingredient[] ingredients = spiceOption.Spice.Ingredients;
			SpiceFetches = new FetchChore[ingredients.Length];
			Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
			for (int i = 0; i < ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = ingredients[i];
				float num = ((CurrentFood != null) ? (CurrentFood.Calories * ingredient.AmountKG / 1000000f) : 0f);
				if (seedStorage.GetMassAvailable(ingredient.IngredientSet[0]) < num)
				{
					SpiceFetches[i] = CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
				}
				if (CurrentFood != null)
				{
					dictionary.Add(ingredient.IngredientSet[0], num);
				}
			}
			if (CurrentFood != null)
			{
				UpdateSpiceIngredientStatus(can_spice: false, dictionary);
			}
			foodFilter[0] = currentSpice.Id;
			foodStorageFilter.FilterChanged();
		}

		public void CancelFetches(string reason)
		{
			if (SpiceFetches == null)
			{
				return;
			}
			for (int i = 0; i < SpiceFetches.Length; i++)
			{
				if (SpiceFetches[i] != null)
				{
					SpiceFetches[i].Cancel(reason);
					SpiceFetches[i] = null;
				}
			}
		}

		private void SetSpiceSymbolColours(Spice spice)
		{
			kbac.SetSymbolTint("stripe_anim2", spice.PrimaryColor);
			kbac.SetSymbolTint("stripe_anim1", spice.SecondaryColor);
			kbac.SetSymbolTint("grinder", spice.PrimaryColor);
		}
	}

	public static Dictionary<Tag, Option> SettingOptions = null;

	public static readonly Operational.Flag spiceSet = new Operational.Flag("spiceSet", Operational.Flag.Type.Functional);

	public static Operational.Flag inKitchen = new Operational.Flag("inKitchen", Operational.Flag.Type.Functional);

	public State inoperational;

	public State operational;

	public State ready;

	public BoolParameter isReady;

	public static void InitializeSpices()
	{
		Spices spices = Db.Get().Spices;
		SettingOptions = new Dictionary<Tag, Option>();
		for (int i = 0; i < spices.Count; i++)
		{
			Spice spice = spices[i];
			if (DlcManager.IsDlcListValidForCurrentContent(spice.DlcIds))
			{
				SettingOptions.Add(spice.Id, new Option(spice));
			}
		}
	}

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = inoperational;
		root.Enter(OnEnterRoot).EventHandler(GameHashes.OnStorageChange, OnStorageChanged);
		inoperational.EventTransition(GameHashes.OperationalChanged, ready, IsOperational).EventHandler(GameHashes.UpdateRoom, UpdateInKitchen).Enter(delegate(StatesInstance smi)
		{
			smi.Play((smi.SelectedOption != null) ? "off" : "default");
			smi.CancelFetches("inoperational");
			if (smi.SelectedOption == null)
			{
				smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected);
			}
		})
			.Exit(delegate(StatesInstance smi)
			{
				smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected);
			});
		operational.EventTransition(GameHashes.OperationalChanged, inoperational, GameStateMachine<SpiceGrinder, StatesInstance, IStateMachineTarget, Def>.Not(IsOperational)).EventHandler(GameHashes.UpdateRoom, UpdateInKitchen).ParamTransition(isReady, ready, GameStateMachine<SpiceGrinder, StatesInstance, IStateMachineTarget, Def>.IsTrue)
			.Update(delegate(StatesInstance smi, float dt)
			{
				if (smi.CurrentFood != null && !smi.HasOpenFetches)
				{
					bool value = smi.CanSpice(smi.CurrentFood.Calories);
					isReady.Set(value, smi);
				}
			}, UpdateRate.SIM_1000ms)
			.PlayAnim("on");
		ready.EventTransition(GameHashes.OperationalChanged, inoperational, GameStateMachine<SpiceGrinder, StatesInstance, IStateMachineTarget, Def>.Not(IsOperational)).EventHandler(GameHashes.UpdateRoom, UpdateInKitchen).ParamTransition(isReady, operational, GameStateMachine<SpiceGrinder, StatesInstance, IStateMachineTarget, Def>.IsFalse)
			.ToggleRecurringChore(CreateChore);
	}

	private void UpdateInKitchen(StatesInstance smi)
	{
		smi.GetComponent<Operational>().SetFlag(inKitchen, smi.roomTracker.IsInCorrectRoom());
	}

	private void OnEnterRoot(StatesInstance smi)
	{
		smi.Initialize();
	}

	private bool IsOperational(StatesInstance smi)
	{
		return smi.IsOperational;
	}

	private void OnStorageChanged(StatesInstance smi, object data)
	{
		smi.UpdateMeter();
		smi.UpdateFoodSymbol();
		if (smi.SelectedOption != null)
		{
			bool value = smi.AvailableFood > 0f && smi.CanSpice(smi.CurrentFood.Calories);
			smi.sm.isReady.Set(value, smi);
		}
	}

	private Chore CreateChore(StatesInstance smi)
	{
		return new WorkChore<SpiceGrinderWorkable>(Db.Get().ChoreTypes.Cook, smi.workable);
	}
}
