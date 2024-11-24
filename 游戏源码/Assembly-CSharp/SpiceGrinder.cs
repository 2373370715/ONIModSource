using System;
using System.Collections.Generic;
using Database;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005A9 RID: 1449
public class SpiceGrinder : GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>
{
	// Token: 0x060019CE RID: 6606 RVA: 0x001A4628 File Offset: 0x001A2828
	public static void InitializeSpices()
	{
		Spices spices = Db.Get().Spices;
		SpiceGrinder.SettingOptions = new Dictionary<Tag, SpiceGrinder.Option>();
		for (int i = 0; i < spices.Count; i++)
		{
			Spice spice = spices[i];
			if (DlcManager.IsDlcListValidForCurrentContent(spice.DlcIds))
			{
				SpiceGrinder.SettingOptions.Add(spice.Id, new SpiceGrinder.Option(spice));
			}
		}
	}

	// Token: 0x060019CF RID: 6607 RVA: 0x001A468C File Offset: 0x001A288C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.root.Enter(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.OnEnterRoot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameEvent.Callback(this.OnStorageChanged));
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.ready, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational)).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).Enter(delegate(SpiceGrinder.StatesInstance smi)
		{
			smi.Play((smi.SelectedOption != null) ? "off" : "default", KAnim.PlayMode.Once);
			smi.CancelFetches("inoperational");
			if (smi.SelectedOption == null)
			{
				smi.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected, null);
			}
		}).Exit(delegate(SpiceGrinder.StatesInstance smi)
		{
			smi.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.NoSpiceSelected, false);
		});
		this.operational.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>(this.isReady, this.ready, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsTrue).Update(delegate(SpiceGrinder.StatesInstance smi, float dt)
		{
			if (smi.CurrentFood != null && !smi.HasOpenFetches)
			{
				bool value = smi.CanSpice(smi.CurrentFood.Calories);
				this.isReady.Set(value, smi, false);
			}
		}, UpdateRate.SIM_1000ms, false).PlayAnim("on");
		this.ready.EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Not(new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.Transition.ConditionCallback(this.IsOperational))).EventHandler(GameHashes.UpdateRoom, new StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State.Callback(this.UpdateInKitchen)).ParamTransition<bool>(this.isReady, this.operational, GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.IsFalse).ToggleRecurringChore(new Func<SpiceGrinder.StatesInstance, Chore>(this.CreateChore), null);
	}

	// Token: 0x060019D0 RID: 6608 RVA: 0x000B0FB5 File Offset: 0x000AF1B5
	private void UpdateInKitchen(SpiceGrinder.StatesInstance smi)
	{
		smi.GetComponent<Operational>().SetFlag(SpiceGrinder.inKitchen, smi.roomTracker.IsInCorrectRoom());
	}

	// Token: 0x060019D1 RID: 6609 RVA: 0x000B0FD2 File Offset: 0x000AF1D2
	private void OnEnterRoot(SpiceGrinder.StatesInstance smi)
	{
		smi.Initialize();
	}

	// Token: 0x060019D2 RID: 6610 RVA: 0x000B0FDA File Offset: 0x000AF1DA
	private bool IsOperational(SpiceGrinder.StatesInstance smi)
	{
		return smi.IsOperational;
	}

	// Token: 0x060019D3 RID: 6611 RVA: 0x001A4828 File Offset: 0x001A2A28
	private void OnStorageChanged(SpiceGrinder.StatesInstance smi, object data)
	{
		smi.UpdateMeter();
		smi.UpdateFoodSymbol();
		if (smi.SelectedOption == null)
		{
			return;
		}
		bool value = smi.AvailableFood > 0f && smi.CanSpice(smi.CurrentFood.Calories);
		smi.sm.isReady.Set(value, smi, false);
	}

	// Token: 0x060019D4 RID: 6612 RVA: 0x001A4880 File Offset: 0x001A2A80
	private Chore CreateChore(SpiceGrinder.StatesInstance smi)
	{
		return new WorkChore<SpiceGrinderWorkable>(Db.Get().ChoreTypes.Cook, smi.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x04001075 RID: 4213
	public static Dictionary<Tag, SpiceGrinder.Option> SettingOptions = null;

	// Token: 0x04001076 RID: 4214
	public static readonly Operational.Flag spiceSet = new Operational.Flag("spiceSet", Operational.Flag.Type.Functional);

	// Token: 0x04001077 RID: 4215
	public static Operational.Flag inKitchen = new Operational.Flag("inKitchen", Operational.Flag.Type.Functional);

	// Token: 0x04001078 RID: 4216
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State inoperational;

	// Token: 0x04001079 RID: 4217
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State operational;

	// Token: 0x0400107A RID: 4218
	public GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.State ready;

	// Token: 0x0400107B RID: 4219
	public StateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.BoolParameter isReady;

	// Token: 0x020005AA RID: 1450
	public class Option : IConfigurableConsumerOption
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060019D8 RID: 6616 RVA: 0x001A48FC File Offset: 0x001A2AFC
		public Effect StatBonus
		{
			get
			{
				if (this.statBonus == null)
				{
					return null;
				}
				if (string.IsNullOrEmpty(this.spiceDescription))
				{
					this.CreateDescription();
					this.GetName();
				}
				this.statBonus.Name = this.name;
				this.statBonus.description = this.spiceDescription;
				return this.statBonus;
			}
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x001A4958 File Offset: 0x001A2B58
		public Option(Spice spice)
		{
			this.Id = new Tag(spice.Id);
			this.Spice = spice;
			if (spice.StatBonus != null)
			{
				this.statBonus = new Effect(spice.Id, this.GetName(), this.spiceDescription, 600f, true, false, false, null, -1f, 0f, null, "");
				this.statBonus.Add(spice.StatBonus);
				Db.Get().effects.Add(this.statBonus);
			}
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x000B1012 File Offset: 0x000AF212
		public Tag GetID()
		{
			return this.Spice.Id;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x001A49E8 File Offset: 0x001A2BE8
		public string GetName()
		{
			if (string.IsNullOrEmpty(this.name))
			{
				string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".NAME";
				StringEntry stringEntry;
				Strings.TryGet(text, out stringEntry);
				this.name = "MISSING " + text;
				if (stringEntry != null)
				{
					this.name = stringEntry;
				}
			}
			return this.name;
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x000B1024 File Offset: 0x000AF224
		public string GetDetailedDescription()
		{
			if (string.IsNullOrEmpty(this.fullDescription))
			{
				this.CreateDescription();
			}
			return this.fullDescription;
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x001A4A54 File Offset: 0x001A2C54
		public string GetDescription()
		{
			if (!string.IsNullOrEmpty(this.spiceDescription))
			{
				return this.spiceDescription;
			}
			string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".DESC";
			StringEntry stringEntry;
			Strings.TryGet(text, out stringEntry);
			this.spiceDescription = "MISSING " + text;
			if (stringEntry != null)
			{
				this.spiceDescription = stringEntry.String;
			}
			return this.spiceDescription;
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x001A4AC4 File Offset: 0x001A2CC4
		private void CreateDescription()
		{
			string text = "STRINGS.ITEMS.SPICES." + this.Spice.Id.ToUpper() + ".DESC";
			StringEntry stringEntry;
			Strings.TryGet(text, out stringEntry);
			this.spiceDescription = "MISSING " + text;
			if (stringEntry != null)
			{
				this.spiceDescription = stringEntry.String;
			}
			this.ingredientDescriptions = string.Format("\n\n<b>{0}</b>", BUILDINGS.PREFABS.SPICEGRINDER.INGREDIENTHEADER);
			for (int i = 0; i < this.Spice.Ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = this.Spice.Ingredients[i];
				GameObject prefab = Assets.GetPrefab((ingredient.IngredientSet != null && ingredient.IngredientSet.Length != 0) ? ingredient.IngredientSet[0] : null);
				this.ingredientDescriptions += string.Format("\n{0}{1} {2}{3}", new object[]
				{
					"    • ",
					prefab.GetProperName(),
					ingredient.AmountKG,
					GameUtil.GetUnitTypeMassOrUnit(prefab)
				});
			}
			this.fullDescription = this.spiceDescription + this.ingredientDescriptions;
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x000B103F File Offset: 0x000AF23F
		public Sprite GetIcon()
		{
			return Assets.GetSprite(this.Spice.Image);
		}

		// Token: 0x060019E0 RID: 6624 RVA: 0x001A4BEC File Offset: 0x001A2DEC
		public IConfigurableConsumerIngredient[] GetIngredients()
		{
			return this.Spice.Ingredients;
		}

		// Token: 0x0400107C RID: 4220
		public readonly Tag Id;

		// Token: 0x0400107D RID: 4221
		public readonly Spice Spice;

		// Token: 0x0400107E RID: 4222
		private string name;

		// Token: 0x0400107F RID: 4223
		private string fullDescription;

		// Token: 0x04001080 RID: 4224
		private string spiceDescription;

		// Token: 0x04001081 RID: 4225
		private string ingredientDescriptions;

		// Token: 0x04001082 RID: 4226
		private Effect statBonus;
	}

	// Token: 0x020005AB RID: 1451
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020005AC RID: 1452
	public class StatesInstance : GameStateMachine<SpiceGrinder, SpiceGrinder.StatesInstance, IStateMachineTarget, SpiceGrinder.Def>.GameInstance
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060019E2 RID: 6626 RVA: 0x000B1056 File Offset: 0x000AF256
		public bool IsOperational
		{
			get
			{
				return this.operational != null && this.operational.IsOperational;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060019E3 RID: 6627 RVA: 0x000B1073 File Offset: 0x000AF273
		public float AvailableFood
		{
			get
			{
				if (!(this.foodStorage == null))
				{
					return this.foodStorage.MassStored();
				}
				return 0f;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060019E4 RID: 6628 RVA: 0x000B1094 File Offset: 0x000AF294
		public SpiceGrinder.Option SelectedOption
		{
			get
			{
				if (!(this.currentSpice.Id == Tag.Invalid))
				{
					return SpiceGrinder.SettingOptions[this.currentSpice.Id];
				}
				return null;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060019E5 RID: 6629 RVA: 0x001A4C08 File Offset: 0x001A2E08
		public Edible CurrentFood
		{
			get
			{
				GameObject gameObject = this.foodStorage.FindFirst(GameTags.Edible);
				this.currentFood = ((gameObject != null) ? gameObject.GetComponent<Edible>() : null);
				return this.currentFood;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060019E6 RID: 6630 RVA: 0x000B10C4 File Offset: 0x000AF2C4
		public bool HasOpenFetches
		{
			get
			{
				return Array.Exists<FetchChore>(this.SpiceFetches, (FetchChore fetch) => fetch != null);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x000B10F0 File Offset: 0x000AF2F0
		// (set) Token: 0x060019E8 RID: 6632 RVA: 0x000B10F8 File Offset: 0x000AF2F8
		public bool AllowMutantSeeds
		{
			get
			{
				return this.allowMutantSeeds;
			}
			set
			{
				this.allowMutantSeeds = value;
				this.ToggleMutantSeedFetches(this.allowMutantSeeds);
			}
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x001A4C44 File Offset: 0x001A2E44
		public StatesInstance(IStateMachineTarget master, SpiceGrinder.Def def) : base(master, def)
		{
			this.workable.Grinder = this;
			Storage[] components = base.gameObject.GetComponents<Storage>();
			this.foodStorage = components[0];
			this.seedStorage = components[1];
			this.operational = base.GetComponent<Operational>();
			this.kbac = base.GetComponent<KBatchedAnimController>();
			this.foodStorageFilter = new FilteredStorage(base.GetComponent<KPrefabID>(), this.foodFilter, null, false, Db.Get().ChoreTypes.CookFetch);
			this.foodStorageFilter.SetHasMeter(false);
			this.meter = new MeterController(this.kbac, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_frame",
				"meter_level"
			});
			this.SetupFoodSymbol();
			this.UpdateFoodSymbol();
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
			base.sm.UpdateInKitchen(this);
			Prioritizable.AddRef(base.gameObject);
			base.Subscribe(493375141, new Action<object>(this.OnRefreshUserMenu));
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x000B110D File Offset: 0x000AF30D
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			Prioritizable.RemoveRef(base.gameObject);
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x001A4D74 File Offset: 0x001A2F74
		public void Initialize()
		{
			if (DlcManager.IsExpansion1Active())
			{
				this.mutantSeedStatusItem = new StatusItem("SPICEGRINDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
				if (this.AllowMutantSeeds)
				{
					KSelectable component = base.GetComponent<KSelectable>();
					if (component != null)
					{
						component.AddStatusItem(this.mutantSeedStatusItem, null);
					}
				}
			}
			SpiceGrinder.Option spiceOption;
			SpiceGrinder.SettingOptions.TryGetValue(new Tag(this.spiceHash), out spiceOption);
			this.OnOptionSelected(spiceOption);
			base.sm.OnStorageChanged(this, null);
			this.UpdateMeter();
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x001A4E0C File Offset: 0x001A300C
		private void OnRefreshUserMenu(object data)
		{
			if (DlcManager.FeatureRadiationEnabled())
			{
				Game.Instance.userMenu.AddButton(base.smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", base.smi.AllowMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT, delegate()
				{
					base.smi.AllowMutantSeeds = !base.smi.AllowMutantSeeds;
					this.OnRefreshUserMenu(base.smi);
				}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP, true), 1f);
			}
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x001A4E88 File Offset: 0x001A3088
		public void ToggleMutantSeedFetches(bool allow)
		{
			if (DlcManager.IsExpansion1Active())
			{
				this.UpdateMutantSeedFetches();
				if (allow)
				{
					this.seedStorage.storageFilters.Add(GameTags.MutatedSeed);
					KSelectable component = base.GetComponent<KSelectable>();
					if (component != null)
					{
						component.AddStatusItem(this.mutantSeedStatusItem, null);
						return;
					}
				}
				else
				{
					if (this.seedStorage.GetMassAvailable(GameTags.MutatedSeed) > 0f)
					{
						this.seedStorage.Drop(GameTags.MutatedSeed);
					}
					this.seedStorage.storageFilters.Remove(GameTags.MutatedSeed);
					KSelectable component2 = base.GetComponent<KSelectable>();
					if (component2 != null)
					{
						component2.RemoveStatusItem(this.mutantSeedStatusItem, false);
					}
				}
			}
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x001A4F38 File Offset: 0x001A3138
		private void UpdateMutantSeedFetches()
		{
			if (this.SpiceFetches != null)
			{
				Tag[] tags = new Tag[]
				{
					GameTags.Seed,
					GameTags.CropSeed
				};
				for (int i = this.SpiceFetches.Length - 1; i >= 0; i--)
				{
					FetchChore fetchChore = this.SpiceFetches[i];
					if (fetchChore != null)
					{
						using (HashSet<Tag>.Enumerator enumerator = this.SpiceFetches[i].tags.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (Assets.GetPrefab(enumerator.Current).HasAnyTags(tags))
								{
									fetchChore.Cancel("MutantSeedChanges");
									this.SpiceFetches[i] = this.CreateFetchChore(fetchChore.tags, fetchChore.amount);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x001A5008 File Offset: 0x001A3208
		private void OnCopySettings(object data)
		{
			SpiceGrinderWorkable component = ((GameObject)data).GetComponent<SpiceGrinderWorkable>();
			if (component != null)
			{
				this.currentSpice = component.Grinder.currentSpice;
				SpiceGrinder.Option spiceOption;
				SpiceGrinder.SettingOptions.TryGetValue(new Tag(component.Grinder.spiceHash), out spiceOption);
				this.OnOptionSelected(spiceOption);
				this.allowMutantSeeds = component.Grinder.AllowMutantSeeds;
			}
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x001A5070 File Offset: 0x001A3270
		public void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "foodSymbol");
			gameObject.SetActive(false);
			bool flag;
			Vector3 position = this.kbac.GetSymbolTransform(SpiceGrinder.StatesInstance.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			this.kbac.SetSymbolVisiblity(SpiceGrinder.StatesInstance.HASH_FOOD, false);
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x001A512C File Offset: 0x001A332C
		public void UpdateFoodSymbol()
		{
			bool flag = this.AvailableFood > 0f && this.CurrentFood != null;
			this.foodKBAC.gameObject.SetActive(flag);
			if (flag)
			{
				this.foodKBAC.SwapAnims(this.CurrentFood.GetComponent<KBatchedAnimController>().AnimFiles);
				this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
			}
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x000B1120 File Offset: 0x000AF320
		public void UpdateMeter()
		{
			this.meter.SetPositionPercent(this.seedStorage.MassStored() / this.seedStorage.capacityKg);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x001A51A8 File Offset: 0x001A33A8
		public void SpiceFood()
		{
			float num = this.CurrentFood.Calories / 1000f;
			this.CurrentFood.SpiceEdible(this.currentSpice, SpiceGrinderConfig.SpicedStatus);
			this.foodStorage.Drop(this.CurrentFood.gameObject, true);
			this.currentFood = null;
			this.UpdateFoodSymbol();
			foreach (Spice.Ingredient ingredient in SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients)
			{
				float num2 = num * ingredient.AmountKG / 1000f;
				int num3 = ingredient.IngredientSet.Length - 1;
				while (num2 > 0f && num3 >= 0)
				{
					Tag tag = ingredient.IngredientSet[num3];
					float num4;
					SimUtil.DiseaseInfo diseaseInfo;
					float num5;
					this.seedStorage.ConsumeAndGetDisease(tag, num2, out num4, out diseaseInfo, out num5);
					num2 -= num4;
					num3--;
				}
			}
			base.sm.isReady.Set(false, this, false);
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x001A52A8 File Offset: 0x001A34A8
		public bool CanSpice(float kcalToSpice)
		{
			bool flag = true;
			float num = kcalToSpice / 1000f;
			Spice.Ingredient[] ingredients = SpiceGrinder.SettingOptions[this.currentSpice.Id].Spice.Ingredients;
			Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
			for (int i = 0; i < ingredients.Length; i++)
			{
				Spice.Ingredient ingredient = ingredients[i];
				float num2 = 0f;
				int num3 = 0;
				while (ingredient.IngredientSet != null && num3 < ingredient.IngredientSet.Length)
				{
					num2 += this.seedStorage.GetMassAvailable(ingredient.IngredientSet[num3]);
					num3++;
				}
				float num4 = num * ingredient.AmountKG / 1000f;
				flag &= (num4 <= num2);
				if (num4 > num2)
				{
					dictionary.Add(ingredient.IngredientSet[0], num4 - num2);
					if (this.SpiceFetches != null && this.SpiceFetches[i] == null)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
					}
				}
			}
			this.UpdateSpiceIngredientStatus(flag, dictionary);
			return flag;
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x000B1144 File Offset: 0x000AF344
		private FetchChore CreateFetchChore(Tag[] ingredientIngredientSet, float amount)
		{
			return this.CreateFetchChore(new HashSet<Tag>(ingredientIngredientSet), amount);
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x001A53C4 File Offset: 0x001A35C4
		private FetchChore CreateFetchChore(HashSet<Tag> ingredients, float amount)
		{
			float num = Mathf.Max(amount, 1f);
			ChoreType cookFetch = Db.Get().ChoreTypes.CookFetch;
			Storage destination = this.seedStorage;
			float amount2 = num;
			FetchChore.MatchCriteria criteria = FetchChore.MatchCriteria.MatchID;
			Tag invalid = Tag.Invalid;
			Action<Chore> on_complete = new Action<Chore>(this.ClearFetchChore);
			Tag[] forbidden_tags;
			if (!this.AllowMutantSeeds)
			{
				(forbidden_tags = new Tag[1])[0] = GameTags.MutatedSeed;
			}
			else
			{
				forbidden_tags = null;
			}
			return new FetchChore(cookFetch, destination, amount2, ingredients, criteria, invalid, forbidden_tags, null, true, on_complete, null, null, Operational.State.Operational, 0);
		}

		// Token: 0x060019F7 RID: 6647 RVA: 0x001A5430 File Offset: 0x001A3630
		private void ClearFetchChore(Chore obj)
		{
			FetchChore fetchChore = obj as FetchChore;
			if (fetchChore == null || !fetchChore.isComplete || this.SpiceFetches == null)
			{
				return;
			}
			int i = this.SpiceFetches.Length - 1;
			while (i >= 0)
			{
				if (this.SpiceFetches[i] == fetchChore)
				{
					float num = fetchChore.originalAmount - fetchChore.amount;
					if (num > 0f)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(fetchChore.tags, num);
						return;
					}
					this.SpiceFetches[i] = null;
					return;
				}
				else
				{
					i--;
				}
			}
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x001A54B0 File Offset: 0x001A36B0
		private void UpdateSpiceIngredientStatus(bool can_spice, Dictionary<Tag, float> missing_spices)
		{
			KSelectable component = base.GetComponent<KSelectable>();
			if (can_spice)
			{
				this.missingResourceStatusItem = component.RemoveStatusItem(this.missingResourceStatusItem, false);
				return;
			}
			if (this.missingResourceStatusItem != Guid.Empty)
			{
				this.missingResourceStatusItem = component.ReplaceStatusItem(this.missingResourceStatusItem, Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
				return;
			}
			this.missingResourceStatusItem = component.AddStatusItem(Db.Get().BuildingStatusItems.MaterialsUnavailable, missing_spices);
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x001A552C File Offset: 0x001A372C
		public void OnOptionSelected(SpiceGrinder.Option spiceOption)
		{
			base.smi.GetComponent<Operational>().SetFlag(SpiceGrinder.spiceSet, spiceOption != null);
			if (spiceOption == null)
			{
				this.kbac.Play("default", KAnim.PlayMode.Once, 1f, 0f);
				this.kbac.SetSymbolTint("stripe_anim2", Color.white);
			}
			else
			{
				this.kbac.Play(this.IsOperational ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
			}
			this.CancelFetches("SpiceChanged");
			if (this.currentSpice.Id != Tag.Invalid)
			{
				this.seedStorage.DropAll(false, false, default(Vector3), true, null);
				this.UpdateMeter();
				base.sm.isReady.Set(false, this, false);
			}
			if (this.missingResourceStatusItem != Guid.Empty)
			{
				this.missingResourceStatusItem = base.GetComponent<KSelectable>().RemoveStatusItem(this.missingResourceStatusItem, false);
			}
			if (spiceOption != null)
			{
				this.currentSpice = new SpiceInstance
				{
					Id = spiceOption.Id,
					TotalKG = spiceOption.Spice.TotalKG
				};
				this.SetSpiceSymbolColours(spiceOption.Spice);
				this.spiceHash = this.currentSpice.Id.GetHash();
				this.seedStorage.capacityKg = this.currentSpice.TotalKG * 10f;
				Spice.Ingredient[] ingredients = spiceOption.Spice.Ingredients;
				this.SpiceFetches = new FetchChore[ingredients.Length];
				Dictionary<Tag, float> dictionary = new Dictionary<Tag, float>();
				for (int i = 0; i < ingredients.Length; i++)
				{
					Spice.Ingredient ingredient = ingredients[i];
					float num = (this.CurrentFood != null) ? (this.CurrentFood.Calories * ingredient.AmountKG / 1000000f) : 0f;
					if (this.seedStorage.GetMassAvailable(ingredient.IngredientSet[0]) < num)
					{
						this.SpiceFetches[i] = this.CreateFetchChore(ingredient.IngredientSet, ingredient.AmountKG * 10f);
					}
					if (this.CurrentFood != null)
					{
						dictionary.Add(ingredient.IngredientSet[0], num);
					}
				}
				if (this.CurrentFood != null)
				{
					this.UpdateSpiceIngredientStatus(false, dictionary);
				}
				this.foodFilter[0] = this.currentSpice.Id;
				this.foodStorageFilter.FilterChanged();
			}
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x001A57B8 File Offset: 0x001A39B8
		public void CancelFetches(string reason)
		{
			if (this.SpiceFetches != null)
			{
				for (int i = 0; i < this.SpiceFetches.Length; i++)
				{
					if (this.SpiceFetches[i] != null)
					{
						this.SpiceFetches[i].Cancel(reason);
						this.SpiceFetches[i] = null;
					}
				}
			}
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x001A5804 File Offset: 0x001A3A04
		private void SetSpiceSymbolColours(Spice spice)
		{
			this.kbac.SetSymbolTint("stripe_anim2", spice.PrimaryColor);
			this.kbac.SetSymbolTint("stripe_anim1", spice.SecondaryColor);
			this.kbac.SetSymbolTint("grinder", spice.PrimaryColor);
		}

		// Token: 0x04001083 RID: 4227
		private static string HASH_FOOD = "food";

		// Token: 0x04001084 RID: 4228
		private KBatchedAnimController kbac;

		// Token: 0x04001085 RID: 4229
		private KBatchedAnimController foodKBAC;

		// Token: 0x04001086 RID: 4230
		[MyCmpReq]
		public RoomTracker roomTracker;

		// Token: 0x04001087 RID: 4231
		[MyCmpReq]
		public SpiceGrinderWorkable workable;

		// Token: 0x04001088 RID: 4232
		[Serialize]
		private int spiceHash;

		// Token: 0x04001089 RID: 4233
		private SpiceInstance currentSpice;

		// Token: 0x0400108A RID: 4234
		private Edible currentFood;

		// Token: 0x0400108B RID: 4235
		private Storage seedStorage;

		// Token: 0x0400108C RID: 4236
		private Storage foodStorage;

		// Token: 0x0400108D RID: 4237
		private MeterController meter;

		// Token: 0x0400108E RID: 4238
		private Tag[] foodFilter = new Tag[1];

		// Token: 0x0400108F RID: 4239
		private FilteredStorage foodStorageFilter;

		// Token: 0x04001090 RID: 4240
		private Operational operational;

		// Token: 0x04001091 RID: 4241
		private Guid missingResourceStatusItem = Guid.Empty;

		// Token: 0x04001092 RID: 4242
		private StatusItem mutantSeedStatusItem;

		// Token: 0x04001093 RID: 4243
		private FetchChore[] SpiceFetches;

		// Token: 0x04001094 RID: 4244
		[Serialize]
		private bool allowMutantSeeds = true;
	}
}
