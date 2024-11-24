using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000CE5 RID: 3301
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ComplexFabricator")]
public class ComplexFabricator : RemoteDockWorkTargetComponent, ISim200ms, ISim1000ms
{
	// Token: 0x1700030C RID: 780
	// (get) Token: 0x06004007 RID: 16391 RVA: 0x000C9870 File Offset: 0x000C7A70
	public ComplexFabricatorWorkable Workable
	{
		get
		{
			return this.workable;
		}
	}

	// Token: 0x1700030D RID: 781
	// (get) Token: 0x06004008 RID: 16392 RVA: 0x000C9878 File Offset: 0x000C7A78
	// (set) Token: 0x06004009 RID: 16393 RVA: 0x000C9880 File Offset: 0x000C7A80
	public bool ForbidMutantSeeds
	{
		get
		{
			return this.forbidMutantSeeds;
		}
		set
		{
			this.forbidMutantSeeds = value;
			this.ToggleMutantSeedFetches();
			this.UpdateMutantSeedStatusItem();
		}
	}

	// Token: 0x1700030E RID: 782
	// (get) Token: 0x0600400A RID: 16394 RVA: 0x000C9895 File Offset: 0x000C7A95
	public Tag[] ForbiddenTags
	{
		get
		{
			if (!this.forbidMutantSeeds)
			{
				return null;
			}
			return this.forbiddenMutantTags;
		}
	}

	// Token: 0x1700030F RID: 783
	// (get) Token: 0x0600400B RID: 16395 RVA: 0x000C98A7 File Offset: 0x000C7AA7
	public int CurrentOrderIdx
	{
		get
		{
			return this.nextOrderIdx;
		}
	}

	// Token: 0x17000310 RID: 784
	// (get) Token: 0x0600400C RID: 16396 RVA: 0x000C98AF File Offset: 0x000C7AAF
	public ComplexRecipe CurrentWorkingOrder
	{
		get
		{
			if (!this.HasWorkingOrder)
			{
				return null;
			}
			return this.recipe_list[this.workingOrderIdx];
		}
	}

	// Token: 0x17000311 RID: 785
	// (get) Token: 0x0600400D RID: 16397 RVA: 0x000C98C8 File Offset: 0x000C7AC8
	public ComplexRecipe NextOrder
	{
		get
		{
			if (!this.nextOrderIsWorkable)
			{
				return null;
			}
			return this.recipe_list[this.nextOrderIdx];
		}
	}

	// Token: 0x17000312 RID: 786
	// (get) Token: 0x0600400E RID: 16398 RVA: 0x000C98E1 File Offset: 0x000C7AE1
	// (set) Token: 0x0600400F RID: 16399 RVA: 0x000C98E9 File Offset: 0x000C7AE9
	public float OrderProgress
	{
		get
		{
			return this.orderProgress;
		}
		set
		{
			this.orderProgress = value;
		}
	}

	// Token: 0x17000313 RID: 787
	// (get) Token: 0x06004010 RID: 16400 RVA: 0x000C98F2 File Offset: 0x000C7AF2
	public bool HasAnyOrder
	{
		get
		{
			return this.HasWorkingOrder || this.hasOpenOrders;
		}
	}

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x06004011 RID: 16401 RVA: 0x000C9904 File Offset: 0x000C7B04
	public bool HasWorker
	{
		get
		{
			return !this.duplicantOperated || this.workable.worker != null;
		}
	}

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x06004012 RID: 16402 RVA: 0x000C9921 File Offset: 0x000C7B21
	public bool WaitingForWorker
	{
		get
		{
			return this.HasWorkingOrder && !this.HasWorker;
		}
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x06004013 RID: 16403 RVA: 0x000C9936 File Offset: 0x000C7B36
	private bool HasWorkingOrder
	{
		get
		{
			return this.workingOrderIdx > -1;
		}
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06004014 RID: 16404 RVA: 0x000C9941 File Offset: 0x000C7B41
	public List<FetchList2> DebugFetchLists
	{
		get
		{
			return this.fetchListList;
		}
	}

	// Token: 0x06004015 RID: 16405 RVA: 0x0023947C File Offset: 0x0023767C
	[OnDeserialized]
	protected virtual void OnDeserializedMethod()
	{
		List<string> list = new List<string>();
		foreach (string text in this.recipeQueueCounts.Keys)
		{
			if (ComplexRecipeManager.Get().GetRecipe(text) == null)
			{
				list.Add(text);
			}
		}
		foreach (string text2 in list)
		{
			global::Debug.LogWarningFormat("{1} removing missing recipe from queue: {0}", new object[]
			{
				text2,
				base.name
			});
			this.recipeQueueCounts.Remove(text2);
		}
	}

	// Token: 0x06004016 RID: 16406 RVA: 0x0023954C File Offset: 0x0023774C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.GetRecipes();
		this.simRenderLoadBalance = true;
		this.choreType = Db.Get().ChoreTypes.Fabricate;
		base.Subscribe<ComplexFabricator>(-1957399615, ComplexFabricator.OnDroppedAllDelegate);
		base.Subscribe<ComplexFabricator>(-592767678, ComplexFabricator.OnOperationalChangedDelegate);
		base.Subscribe<ComplexFabricator>(-905833192, ComplexFabricator.OnCopySettingsDelegate);
		base.Subscribe<ComplexFabricator>(-1697596308, ComplexFabricator.OnStorageChangeDelegate);
		base.Subscribe<ComplexFabricator>(-1837862626, ComplexFabricator.OnParticleStorageChangedDelegate);
		this.workable = base.GetComponent<ComplexFabricatorWorkable>();
		Components.ComplexFabricators.Add(this);
		base.Subscribe<ComplexFabricator>(493375141, ComplexFabricator.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06004017 RID: 16407 RVA: 0x00239600 File Offset: 0x00237800
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.InitRecipeQueueCount();
		foreach (string key in this.recipeQueueCounts.Keys)
		{
			if (this.recipeQueueCounts[key] == 100)
			{
				this.recipeQueueCounts[key] = ComplexFabricator.QUEUE_INFINITE;
			}
		}
		this.buildStorage.Transfer(this.inStorage, true, true);
		this.DropExcessIngredients(this.inStorage);
		int num = this.FindRecipeIndex(this.lastWorkingRecipe);
		if (num > -1)
		{
			this.nextOrderIdx = num;
		}
		this.UpdateMutantSeedStatusItem();
	}

	// Token: 0x06004018 RID: 16408 RVA: 0x000C9949 File Offset: 0x000C7B49
	protected override void OnCleanUp()
	{
		this.CancelAllOpenOrders();
		this.CancelChore();
		Components.ComplexFabricators.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06004019 RID: 16409 RVA: 0x002396BC File Offset: 0x002378BC
	private void OnRefreshUserMenu(object data)
	{
		if (SaveLoader.Instance.IsDLCActiveForCurrentSave("EXPANSION1_ID") && this.HasRecipiesWithSeeds())
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", this.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT, delegate()
			{
				this.ForbidMutantSeeds = !this.ForbidMutantSeeds;
				this.OnRefreshUserMenu(null);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP, true), 1f);
		}
	}

	// Token: 0x0600401A RID: 16410 RVA: 0x00239740 File Offset: 0x00237940
	private bool HasRecipiesWithSeeds()
	{
		bool result = false;
		ComplexRecipe[] array = this.recipe_list;
		for (int i = 0; i < array.Length; i++)
		{
			ComplexRecipe.RecipeElement[] ingredients = array[i].ingredients;
			for (int j = 0; j < ingredients.Length; j++)
			{
				GameObject prefab = Assets.GetPrefab(ingredients[j].material);
				if (prefab != null && prefab.GetComponent<PlantableSeed>() != null)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x0600401B RID: 16411 RVA: 0x002397B0 File Offset: 0x002379B0
	private void UpdateMutantSeedStatusItem()
	{
		base.gameObject.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.FabricatorAcceptsMutantSeeds, SaveLoader.Instance.IsDLCActiveForCurrentSave("EXPANSION1_ID") && this.HasRecipiesWithSeeds() && !this.forbidMutantSeeds, null);
	}

	// Token: 0x0600401C RID: 16412 RVA: 0x000C9968 File Offset: 0x000C7B68
	private void OnOperationalChanged(object data)
	{
		if ((bool)data)
		{
			this.queueDirty = true;
		}
		else
		{
			this.CancelAllOpenOrders();
		}
		this.UpdateChore();
	}

	// Token: 0x0600401D RID: 16413 RVA: 0x00239808 File Offset: 0x00237A08
	public virtual void Sim1000ms(float dt)
	{
		this.RefreshAndStartNextOrder();
		if (this.materialNeedCache.Count > 0 && this.fetchListList.Count == 0)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} has material needs cached, but no open fetches. materialNeedCache={1}, fetchListList={2}", new object[]
			{
				base.gameObject,
				this.materialNeedCache.Count,
				this.fetchListList.Count
			});
			this.queueDirty = true;
		}
	}

	// Token: 0x0600401E RID: 16414 RVA: 0x000C9987 File Offset: 0x000C7B87
	protected virtual float ComputeWorkProgress(float dt, ComplexRecipe recipe)
	{
		return dt / recipe.time;
	}

	// Token: 0x0600401F RID: 16415 RVA: 0x00239884 File Offset: 0x00237A84
	public void Sim200ms(float dt)
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		this.operational.SetActive(this.HasWorkingOrder && this.HasWorker, false);
		if (!this.duplicantOperated && this.HasWorkingOrder)
		{
			this.orderProgress += this.ComputeWorkProgress(dt, this.recipe_list[this.workingOrderIdx]);
			if (this.orderProgress >= 1f)
			{
				this.ShowProgressBar(false);
				this.CompleteWorkingOrder();
			}
		}
	}

	// Token: 0x06004020 RID: 16416 RVA: 0x00239908 File Offset: 0x00237B08
	private void RefreshAndStartNextOrder()
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		if (this.queueDirty)
		{
			this.RefreshQueue();
		}
		if (!this.HasWorkingOrder && this.nextOrderIsWorkable)
		{
			this.ShowProgressBar(true);
			this.StartWorkingOrder(this.nextOrderIdx);
		}
	}

	// Token: 0x06004021 RID: 16417 RVA: 0x000C98E1 File Offset: 0x000C7AE1
	public virtual float GetPercentComplete()
	{
		return this.orderProgress;
	}

	// Token: 0x06004022 RID: 16418 RVA: 0x00239954 File Offset: 0x00237B54
	private void ShowProgressBar(bool show)
	{
		if (show && this.showProgressBar && !this.duplicantOperated)
		{
			if (this.progressBar == null)
			{
				this.progressBar = ProgressBar.CreateProgressBar(base.gameObject, new Func<float>(this.GetPercentComplete));
			}
			this.progressBar.enabled = true;
			this.progressBar.SetVisibility(true);
			return;
		}
		if (this.progressBar != null)
		{
			this.progressBar.gameObject.DeleteObject();
			this.progressBar = null;
		}
	}

	// Token: 0x06004023 RID: 16419 RVA: 0x000C9991 File Offset: 0x000C7B91
	public void SetQueueDirty()
	{
		this.queueDirty = true;
	}

	// Token: 0x06004024 RID: 16420 RVA: 0x000C999A File Offset: 0x000C7B9A
	private void RefreshQueue()
	{
		this.queueDirty = false;
		this.ValidateWorkingOrder();
		this.ValidateNextOrder();
		this.UpdateOpenOrders();
		this.DropExcessIngredients(this.inStorage);
		base.Trigger(1721324763, this);
	}

	// Token: 0x06004025 RID: 16421 RVA: 0x002399E0 File Offset: 0x00237BE0
	private void StartWorkingOrder(int index)
	{
		global::Debug.Assert(!this.HasWorkingOrder, "machineOrderIdx already set");
		this.workingOrderIdx = index;
		if (this.recipe_list[this.workingOrderIdx].id != this.lastWorkingRecipe)
		{
			this.orderProgress = 0f;
			this.lastWorkingRecipe = this.recipe_list[this.workingOrderIdx].id;
		}
		this.TransferCurrentRecipeIngredientsForBuild();
		global::Debug.Assert(this.openOrderCounts[this.workingOrderIdx] > 0, "openOrderCount invalid");
		List<int> list = this.openOrderCounts;
		int index2 = this.workingOrderIdx;
		int num = list[index2];
		list[index2] = num - 1;
		this.UpdateChore();
		base.Trigger(2023536846, this.recipe_list[this.workingOrderIdx]);
		this.AdvanceNextOrder();
	}

	// Token: 0x06004026 RID: 16422 RVA: 0x000C99CD File Offset: 0x000C7BCD
	private void CancelWorkingOrder()
	{
		global::Debug.Assert(this.HasWorkingOrder, "machineOrderIdx not set");
		this.buildStorage.Transfer(this.inStorage, true, true);
		this.workingOrderIdx = -1;
		this.orderProgress = 0f;
		this.UpdateChore();
	}

	// Token: 0x06004027 RID: 16423 RVA: 0x00239AB0 File Offset: 0x00237CB0
	public void CompleteWorkingOrder()
	{
		if (!this.HasWorkingOrder)
		{
			global::Debug.LogWarning("CompleteWorkingOrder called with no working order.", base.gameObject);
			return;
		}
		ComplexRecipe complexRecipe = this.recipe_list[this.workingOrderIdx];
		this.SpawnOrderProduct(complexRecipe);
		float num = this.buildStorage.MassStored();
		if (num != 0f)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} build storage contains mass {1} after order completion.", new object[]
			{
				base.gameObject,
				num
			});
			this.buildStorage.Transfer(this.inStorage, true, true);
		}
		this.DecrementRecipeQueueCountInternal(complexRecipe, true);
		this.workingOrderIdx = -1;
		this.orderProgress = 0f;
		this.CancelChore();
		base.Trigger(1355439576, complexRecipe);
		if (!this.cancelling)
		{
			this.RefreshAndStartNextOrder();
		}
	}

	// Token: 0x06004028 RID: 16424 RVA: 0x00239B78 File Offset: 0x00237D78
	private void ValidateWorkingOrder()
	{
		if (!this.HasWorkingOrder)
		{
			return;
		}
		ComplexRecipe recipe = this.recipe_list[this.workingOrderIdx];
		if (!this.IsRecipeQueued(recipe))
		{
			this.CancelWorkingOrder();
		}
	}

	// Token: 0x06004029 RID: 16425 RVA: 0x00239BAC File Offset: 0x00237DAC
	private void UpdateChore()
	{
		if (!this.duplicantOperated)
		{
			return;
		}
		bool flag = this.operational.IsOperational && this.HasWorkingOrder;
		if (flag && this.chore == null)
		{
			this.CreateChore();
			return;
		}
		if (!flag && this.chore != null)
		{
			this.CancelChore();
		}
	}

	// Token: 0x0600402A RID: 16426 RVA: 0x00239BFC File Offset: 0x00237DFC
	private void AdvanceNextOrder()
	{
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			this.nextOrderIdx = (this.nextOrderIdx + 1) % this.recipe_list.Length;
			ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
			this.nextOrderIsWorkable = (this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage));
			if (this.nextOrderIsWorkable)
			{
				break;
			}
		}
	}

	// Token: 0x0600402B RID: 16427 RVA: 0x00239C6C File Offset: 0x00237E6C
	private void ValidateNextOrder()
	{
		ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
		this.nextOrderIsWorkable = (this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage));
		if (!this.nextOrderIsWorkable)
		{
			this.AdvanceNextOrder();
		}
	}

	// Token: 0x0600402C RID: 16428 RVA: 0x00239CB8 File Offset: 0x00237EB8
	private void CancelAllOpenOrders()
	{
		for (int i = 0; i < this.openOrderCounts.Count; i++)
		{
			this.openOrderCounts[i] = 0;
		}
		this.ClearMaterialNeeds();
		this.CancelFetches();
	}

	// Token: 0x0600402D RID: 16429 RVA: 0x00239CF4 File Offset: 0x00237EF4
	private void UpdateOpenOrders()
	{
		ComplexRecipe[] recipes = this.GetRecipes();
		if (recipes.Length != this.openOrderCounts.Count)
		{
			global::Debug.LogErrorFormat(base.gameObject, "Recipe count {0} doesn't match open order count {1}", new object[]
			{
				recipes.Length,
				this.openOrderCounts.Count
			});
		}
		bool flag = false;
		this.hasOpenOrders = false;
		for (int i = 0; i < recipes.Length; i++)
		{
			ComplexRecipe recipe = recipes[i];
			int recipePrefetchCount = this.GetRecipePrefetchCount(recipe);
			if (recipePrefetchCount > 0)
			{
				this.hasOpenOrders = true;
			}
			int num = this.openOrderCounts[i];
			if (num != recipePrefetchCount)
			{
				if (recipePrefetchCount < num)
				{
					flag = true;
				}
				this.openOrderCounts[i] = recipePrefetchCount;
			}
		}
		DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
		DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary2 = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
		for (int j = 0; j < this.openOrderCounts.Count; j++)
		{
			if (this.openOrderCounts[j] > 0)
			{
				foreach (ComplexRecipe.RecipeElement recipeElement in this.recipe_list[j].ingredients)
				{
					pooledDictionary[recipeElement.material] = this.inStorage.GetAmountAvailable(recipeElement.material);
				}
			}
		}
		for (int l = 0; l < this.recipe_list.Length; l++)
		{
			int num2 = this.openOrderCounts[l];
			if (num2 > 0)
			{
				foreach (ComplexRecipe.RecipeElement recipeElement2 in this.recipe_list[l].ingredients)
				{
					float num3 = recipeElement2.amount * (float)num2;
					float num4 = num3 - pooledDictionary[recipeElement2.material];
					if (num4 > 0f)
					{
						float num5;
						pooledDictionary2.TryGetValue(recipeElement2.material, out num5);
						pooledDictionary2[recipeElement2.material] = num5 + num4;
						pooledDictionary[recipeElement2.material] = 0f;
					}
					else
					{
						DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary3 = pooledDictionary;
						Tag material = recipeElement2.material;
						pooledDictionary3[material] -= num3;
					}
				}
			}
		}
		if (flag)
		{
			this.CancelFetches();
		}
		if (pooledDictionary2.Count > 0)
		{
			this.UpdateFetches(pooledDictionary2);
		}
		this.UpdateMaterialNeeds(pooledDictionary2);
		pooledDictionary2.Recycle();
		pooledDictionary.Recycle();
	}

	// Token: 0x0600402E RID: 16430 RVA: 0x00239F40 File Offset: 0x00238140
	private void UpdateMaterialNeeds(Dictionary<Tag, float> missingAmounts)
	{
		this.ClearMaterialNeeds();
		foreach (KeyValuePair<Tag, float> keyValuePair in missingAmounts)
		{
			MaterialNeeds.UpdateNeed(keyValuePair.Key, keyValuePair.Value, base.gameObject.GetMyWorldId());
			this.materialNeedCache.Add(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x0600402F RID: 16431 RVA: 0x00239FC4 File Offset: 0x002381C4
	private void ClearMaterialNeeds()
	{
		foreach (KeyValuePair<Tag, float> keyValuePair in this.materialNeedCache)
		{
			MaterialNeeds.UpdateNeed(keyValuePair.Key, -keyValuePair.Value, base.gameObject.GetMyWorldId());
		}
		this.materialNeedCache.Clear();
	}

	// Token: 0x06004030 RID: 16432 RVA: 0x0023A03C File Offset: 0x0023823C
	public int HighestHEPQueued()
	{
		int num = 0;
		foreach (KeyValuePair<string, int> keyValuePair in this.recipeQueueCounts)
		{
			if (keyValuePair.Value > 0)
			{
				num = Math.Max(this.recipe_list[this.FindRecipeIndex(keyValuePair.Key)].consumedHEP, num);
			}
		}
		return num;
	}

	// Token: 0x06004031 RID: 16433 RVA: 0x0023A0B8 File Offset: 0x002382B8
	private void OnFetchComplete()
	{
		for (int i = this.fetchListList.Count - 1; i >= 0; i--)
		{
			if (this.fetchListList[i].IsComplete)
			{
				this.fetchListList.RemoveAt(i);
				this.queueDirty = true;
			}
		}
	}

	// Token: 0x06004032 RID: 16434 RVA: 0x000C9991 File Offset: 0x000C7B91
	private void OnStorageChange(object data)
	{
		this.queueDirty = true;
	}

	// Token: 0x06004033 RID: 16435 RVA: 0x000C9A0A File Offset: 0x000C7C0A
	private void OnDroppedAll(object data)
	{
		if (this.HasWorkingOrder)
		{
			this.CancelWorkingOrder();
		}
		this.CancelAllOpenOrders();
		this.RefreshQueue();
	}

	// Token: 0x06004034 RID: 16436 RVA: 0x0023A104 File Offset: 0x00238304
	private void DropExcessIngredients(Storage storage)
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		if (this.keepAdditionalTag != Tag.Invalid)
		{
			hashSet.Add(this.keepAdditionalTag);
		}
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			ComplexRecipe complexRecipe = this.recipe_list[i];
			if (this.IsRecipeQueued(complexRecipe))
			{
				foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
				{
					hashSet.Add(recipeElement.material);
				}
			}
		}
		for (int k = storage.items.Count - 1; k >= 0; k--)
		{
			GameObject gameObject = storage.items[k];
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && (!this.keepExcessLiquids || !component.Element.IsLiquid))
				{
					KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
					if (component2 && !hashSet.Contains(component2.PrefabID()))
					{
						storage.Drop(gameObject, true);
					}
				}
			}
		}
	}

	// Token: 0x06004035 RID: 16437 RVA: 0x0023A214 File Offset: 0x00238414
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		ComplexFabricator component = gameObject.GetComponent<ComplexFabricator>();
		if (component == null)
		{
			return;
		}
		this.ForbidMutantSeeds = component.ForbidMutantSeeds;
		foreach (ComplexRecipe complexRecipe in this.recipe_list)
		{
			int count;
			if (!component.recipeQueueCounts.TryGetValue(complexRecipe.id, out count))
			{
				count = 0;
			}
			this.SetRecipeQueueCountInternal(complexRecipe, count);
		}
		this.RefreshQueue();
	}

	// Token: 0x06004036 RID: 16438 RVA: 0x000C9A26 File Offset: 0x000C7C26
	private int CompareRecipe(ComplexRecipe a, ComplexRecipe b)
	{
		if (a.sortOrder != b.sortOrder)
		{
			return a.sortOrder - b.sortOrder;
		}
		return StringComparer.InvariantCulture.Compare(a.id, b.id);
	}

	// Token: 0x06004037 RID: 16439 RVA: 0x0023A294 File Offset: 0x00238494
	public ComplexRecipe[] GetRecipes()
	{
		if (this.recipe_list == null)
		{
			Tag prefabTag = base.GetComponent<KPrefabID>().PrefabTag;
			List<ComplexRecipe> recipes = ComplexRecipeManager.Get().recipes;
			List<ComplexRecipe> list = new List<ComplexRecipe>();
			foreach (ComplexRecipe complexRecipe in recipes)
			{
				using (List<Tag>.Enumerator enumerator2 = complexRecipe.fabricators.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current == prefabTag && SaveLoader.Instance.IsDlcListActiveForCurrentSave(complexRecipe.GetDlcIds()))
						{
							list.Add(complexRecipe);
						}
					}
				}
			}
			this.recipe_list = list.ToArray();
			Array.Sort<ComplexRecipe>(this.recipe_list, new Comparison<ComplexRecipe>(this.CompareRecipe));
		}
		return this.recipe_list;
	}

	// Token: 0x06004038 RID: 16440 RVA: 0x0023A388 File Offset: 0x00238588
	private void InitRecipeQueueCount()
	{
		foreach (ComplexRecipe complexRecipe in this.GetRecipes())
		{
			bool flag = false;
			using (Dictionary<string, int>.KeyCollection.Enumerator enumerator = this.recipeQueueCounts.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == complexRecipe.id)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.recipeQueueCounts.Add(complexRecipe.id, 0);
			}
			this.openOrderCounts.Add(0);
		}
	}

	// Token: 0x06004039 RID: 16441 RVA: 0x0023A428 File Offset: 0x00238628
	private int FindRecipeIndex(string id)
	{
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			if (this.recipe_list[i].id == id)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x0600403A RID: 16442 RVA: 0x000C9A5A File Offset: 0x000C7C5A
	public int GetRecipeQueueCount(ComplexRecipe recipe)
	{
		return this.recipeQueueCounts[recipe.id];
	}

	// Token: 0x0600403B RID: 16443 RVA: 0x0023A460 File Offset: 0x00238660
	public bool IsRecipeQueued(ComplexRecipe recipe)
	{
		int num = this.recipeQueueCounts[recipe.id];
		global::Debug.Assert(num >= 0 || num == ComplexFabricator.QUEUE_INFINITE);
		return num != 0;
	}

	// Token: 0x0600403C RID: 16444 RVA: 0x0023A498 File Offset: 0x00238698
	public int GetRecipePrefetchCount(ComplexRecipe recipe)
	{
		int remainingQueueCount = this.GetRemainingQueueCount(recipe);
		global::Debug.Assert(remainingQueueCount >= 0);
		return Mathf.Min(2, remainingQueueCount);
	}

	// Token: 0x0600403D RID: 16445 RVA: 0x0023A4C0 File Offset: 0x002386C0
	private int GetRemainingQueueCount(ComplexRecipe recipe)
	{
		int num = this.recipeQueueCounts[recipe.id];
		global::Debug.Assert(num >= 0 || num == ComplexFabricator.QUEUE_INFINITE);
		if (num == ComplexFabricator.QUEUE_INFINITE)
		{
			return ComplexFabricator.MAX_QUEUE_SIZE;
		}
		if (num > 0)
		{
			if (this.IsCurrentRecipe(recipe))
			{
				num--;
			}
			return num;
		}
		return 0;
	}

	// Token: 0x0600403E RID: 16446 RVA: 0x000C9A6D File Offset: 0x000C7C6D
	private bool IsCurrentRecipe(ComplexRecipe recipe)
	{
		return this.workingOrderIdx >= 0 && this.recipe_list[this.workingOrderIdx].id == recipe.id;
	}

	// Token: 0x0600403F RID: 16447 RVA: 0x000C9A97 File Offset: 0x000C7C97
	public void SetRecipeQueueCount(ComplexRecipe recipe, int count)
	{
		this.SetRecipeQueueCountInternal(recipe, count);
		this.RefreshQueue();
	}

	// Token: 0x06004040 RID: 16448 RVA: 0x000C9AA7 File Offset: 0x000C7CA7
	private void SetRecipeQueueCountInternal(ComplexRecipe recipe, int count)
	{
		this.recipeQueueCounts[recipe.id] = count;
	}

	// Token: 0x06004041 RID: 16449 RVA: 0x0023A518 File Offset: 0x00238718
	public void IncrementRecipeQueueCount(ComplexRecipe recipe)
	{
		if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
		{
			this.recipeQueueCounts[recipe.id] = 0;
		}
		else if (this.recipeQueueCounts[recipe.id] >= ComplexFabricator.MAX_QUEUE_SIZE)
		{
			this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
		}
		else
		{
			Dictionary<string, int> dictionary = this.recipeQueueCounts;
			string id = recipe.id;
			int num = dictionary[id];
			dictionary[id] = num + 1;
		}
		this.RefreshQueue();
	}

	// Token: 0x06004042 RID: 16450 RVA: 0x000C9ABB File Offset: 0x000C7CBB
	public void DecrementRecipeQueueCount(ComplexRecipe recipe, bool respectInfinite = true)
	{
		this.DecrementRecipeQueueCountInternal(recipe, respectInfinite);
		this.RefreshQueue();
	}

	// Token: 0x06004043 RID: 16451 RVA: 0x0023A5A8 File Offset: 0x002387A8
	private void DecrementRecipeQueueCountInternal(ComplexRecipe recipe, bool respectInfinite = true)
	{
		if (!respectInfinite || this.recipeQueueCounts[recipe.id] != ComplexFabricator.QUEUE_INFINITE)
		{
			if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
			{
				this.recipeQueueCounts[recipe.id] = ComplexFabricator.MAX_QUEUE_SIZE;
				return;
			}
			if (this.recipeQueueCounts[recipe.id] == 0)
			{
				this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
				return;
			}
			Dictionary<string, int> dictionary = this.recipeQueueCounts;
			string id = recipe.id;
			int num = dictionary[id];
			dictionary[id] = num - 1;
		}
	}

	// Token: 0x06004044 RID: 16452 RVA: 0x000C9ACB File Offset: 0x000C7CCB
	private void CreateChore()
	{
		global::Debug.Assert(this.chore == null, "chore should be null");
		this.chore = this.workable.CreateWorkChore(this.choreType, this.orderProgress);
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06004045 RID: 16453 RVA: 0x000C9AFD File Offset: 0x000C7CFD
	public override Chore RemoteDockChore
	{
		get
		{
			if (!this.duplicantOperated)
			{
				return null;
			}
			return this.chore;
		}
	}

	// Token: 0x06004046 RID: 16454 RVA: 0x000C9B0F File Offset: 0x000C7D0F
	private void CancelChore()
	{
		if (this.cancelling)
		{
			return;
		}
		this.cancelling = true;
		if (this.chore != null)
		{
			this.chore.Cancel("order cancelled");
			this.chore = null;
		}
		this.cancelling = false;
	}

	// Token: 0x06004047 RID: 16455 RVA: 0x0023A648 File Offset: 0x00238848
	private void UpdateFetches(DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary missingAmounts)
	{
		ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreTypeIdHash);
		foreach (KeyValuePair<Tag, float> keyValuePair in missingAmounts)
		{
			if (!this.allowManualFluidDelivery)
			{
				Element element = ElementLoader.GetElement(keyValuePair.Key);
				if (element != null && (element.IsLiquid || element.IsGas))
				{
					continue;
				}
			}
			if (keyValuePair.Value >= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.HasPendingFetch(keyValuePair.Key))
			{
				FetchList2 fetchList = new FetchList2(this.inStorage, byHash);
				FetchList2 fetchList2 = fetchList;
				Tag key = keyValuePair.Key;
				float value = keyValuePair.Value;
				fetchList2.Add(key, this.ForbiddenTags, value, Operational.State.None);
				fetchList.ShowStatusItem = false;
				fetchList.Submit(new System.Action(this.OnFetchComplete), false);
				this.fetchListList.Add(fetchList);
			}
		}
	}

	// Token: 0x06004048 RID: 16456 RVA: 0x0023A748 File Offset: 0x00238948
	private bool HasPendingFetch(Tag tag)
	{
		foreach (FetchList2 fetchList in this.fetchListList)
		{
			float num;
			fetchList.MinimumAmount.TryGetValue(tag, out num);
			if (num > 0f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004049 RID: 16457 RVA: 0x0023A7B0 File Offset: 0x002389B0
	private void CancelFetches()
	{
		foreach (FetchList2 fetchList in this.fetchListList)
		{
			fetchList.Cancel("cancel all orders");
		}
		this.fetchListList.Clear();
	}

	// Token: 0x0600404A RID: 16458 RVA: 0x0023A810 File Offset: 0x00238A10
	protected virtual void TransferCurrentRecipeIngredientsForBuild()
	{
		ComplexRecipe.RecipeElement[] ingredients = this.recipe_list[this.workingOrderIdx].ingredients;
		int i = 0;
		while (i < ingredients.Length)
		{
			ComplexRecipe.RecipeElement recipeElement = ingredients[i];
			float num;
			for (;;)
			{
				num = recipeElement.amount - this.buildStorage.GetAmountAvailable(recipeElement.material);
				if (num <= 0f)
				{
					break;
				}
				if (this.inStorage.GetAmountAvailable(recipeElement.material) <= 0f)
				{
					goto Block_2;
				}
				this.inStorage.Transfer(this.buildStorage, recipeElement.material, num, false, true);
			}
			IL_9D:
			i++;
			continue;
			Block_2:
			global::Debug.LogWarningFormat("TransferCurrentRecipeIngredientsForBuild ran out of {0} but still needed {1} more.", new object[]
			{
				recipeElement.material,
				num
			});
			goto IL_9D;
		}
	}

	// Token: 0x0600404B RID: 16459 RVA: 0x0023A8C8 File Offset: 0x00238AC8
	protected virtual bool HasIngredients(ComplexRecipe recipe, Storage storage)
	{
		ComplexRecipe.RecipeElement[] ingredients = recipe.ingredients;
		if (recipe.consumedHEP > 0)
		{
			HighEnergyParticleStorage component = base.GetComponent<HighEnergyParticleStorage>();
			if (component == null || component.Particles < (float)recipe.consumedHEP)
			{
				return false;
			}
		}
		foreach (ComplexRecipe.RecipeElement recipeElement in ingredients)
		{
			float amountAvailable = storage.GetAmountAvailable(recipeElement.material);
			if (recipeElement.amount - amountAvailable >= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600404C RID: 16460 RVA: 0x0023A940 File Offset: 0x00238B40
	private void ToggleMutantSeedFetches()
	{
		if (this.HasAnyOrder)
		{
			ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreTypeIdHash);
			List<FetchList2> list = new List<FetchList2>();
			foreach (FetchList2 fetchList in this.fetchListList)
			{
				foreach (FetchOrder2 fetchOrder in fetchList.FetchOrders)
				{
					foreach (Tag tag in fetchOrder.Tags)
					{
						GameObject prefab = Assets.GetPrefab(tag);
						if (prefab != null && prefab.GetComponent<PlantableSeed>() != null)
						{
							fetchList.Cancel("MutantSeedTagChanged");
							list.Add(fetchList);
						}
					}
				}
			}
			foreach (FetchList2 fetchList2 in list)
			{
				this.fetchListList.Remove(fetchList2);
				foreach (FetchOrder2 fetchOrder2 in fetchList2.FetchOrders)
				{
					foreach (Tag tag2 in fetchOrder2.Tags)
					{
						FetchList2 fetchList3 = new FetchList2(this.inStorage, byHash);
						FetchList2 fetchList4 = fetchList3;
						Tag tag3 = tag2;
						float totalAmount = fetchOrder2.TotalAmount;
						fetchList4.Add(tag3, this.ForbiddenTags, totalAmount, Operational.State.None);
						fetchList3.ShowStatusItem = false;
						fetchList3.Submit(new System.Action(this.OnFetchComplete), false);
						this.fetchListList.Add(fetchList3);
					}
				}
			}
		}
	}

	// Token: 0x0600404D RID: 16461 RVA: 0x0023AB80 File Offset: 0x00238D80
	protected virtual List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = new List<GameObject>();
		SimUtil.DiseaseInfo diseaseInfo;
		diseaseInfo.count = 0;
		diseaseInfo.idx = 0;
		float num = 0f;
		float num2 = 0f;
		string text = null;
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			num2 += recipeElement.amount;
		}
		ComplexRecipe.RecipeElement recipeElement2 = null;
		foreach (ComplexRecipe.RecipeElement recipeElement3 in recipe.ingredients)
		{
			float num3 = recipeElement3.amount / num2;
			if (recipe.ProductHasFacade && text.IsNullOrWhiteSpace())
			{
				RepairableEquipment component = this.buildStorage.FindFirst(recipeElement3.material).GetComponent<RepairableEquipment>();
				if (component != null)
				{
					text = component.facadeID;
				}
			}
			if (recipeElement3.inheritElement || recipeElement3.Edible)
			{
				recipeElement2 = recipeElement3;
			}
			if (recipeElement3.Edible)
			{
				this.buildStorage.TransferMass(this.outStorage, recipeElement3.material, recipeElement3.amount, true, true, true);
			}
			else
			{
				float num4;
				SimUtil.DiseaseInfo diseaseInfo2;
				float num5;
				this.buildStorage.ConsumeAndGetDisease(recipeElement3.material, recipeElement3.amount, out num4, out diseaseInfo2, out num5);
				if (diseaseInfo2.count > diseaseInfo.count)
				{
					diseaseInfo = diseaseInfo2;
				}
				num += num5 * num3;
			}
		}
		if (recipe.consumedHEP > 0)
		{
			base.GetComponent<HighEnergyParticleStorage>().ConsumeAndGet((float)recipe.consumedHEP);
		}
		foreach (ComplexRecipe.RecipeElement recipeElement4 in recipe.results)
		{
			GameObject gameObject = this.buildStorage.FindFirst(recipeElement4.material);
			if (gameObject != null)
			{
				Edible component2 = gameObject.GetComponent<Edible>();
				if (component2)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component2.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", component2.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
				}
			}
			switch (recipeElement4.temperatureOperation)
			{
			case ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature:
			case ComplexRecipe.RecipeElement.TemperatureOperation.Heated:
			{
				GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(recipeElement4.material), Grid.SceneLayer.Ore, null, 0);
				int cell = Grid.PosToCell(this);
				gameObject2.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore) + this.outputOffset);
				PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
				component3.Units = recipeElement4.amount;
				component3.Temperature = ((recipeElement4.temperatureOperation == ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature) ? num : this.heatedTemperature);
				if (recipeElement2 != null)
				{
					Element element = ElementLoader.GetElement(recipeElement2.material);
					if (element != null)
					{
						component3.SetElement(element.id, false);
					}
				}
				if (recipe.ProductHasFacade && !text.IsNullOrWhiteSpace())
				{
					Equippable component4 = gameObject2.GetComponent<Equippable>();
					if (component4 != null)
					{
						EquippableFacade.AddFacadeToEquippable(component4, text);
					}
				}
				gameObject2.SetActive(true);
				float num6 = recipeElement4.amount / recipe.TotalResultUnits();
				component3.AddDisease(diseaseInfo.idx, Mathf.RoundToInt((float)diseaseInfo.count * num6), "ComplexFabricator.CompleteOrder");
				if (!recipeElement4.facadeID.IsNullOrWhiteSpace())
				{
					Equippable component5 = gameObject2.GetComponent<Equippable>();
					if (component5 != null)
					{
						EquippableFacade.AddFacadeToEquippable(component5, recipeElement4.facadeID);
					}
				}
				gameObject2.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
				list.Add(gameObject2);
				if (this.storeProduced || recipeElement4.storeElement)
				{
					this.outStorage.Store(gameObject2, false, false, true, false);
				}
				break;
			}
			case ComplexRecipe.RecipeElement.TemperatureOperation.Melted:
				if (this.storeProduced || recipeElement4.storeElement)
				{
					float temperature = ElementLoader.GetElement(recipeElement4.material).defaultValues.temperature;
					this.outStorage.AddLiquid(ElementLoader.GetElementID(recipeElement4.material), recipeElement4.amount, temperature, 0, 0, false, true);
				}
				break;
			case ComplexRecipe.RecipeElement.TemperatureOperation.Dehydrated:
				for (int j = 0; j < (int)recipeElement4.amount; j++)
				{
					GameObject gameObject3 = GameUtil.KInstantiate(Assets.GetPrefab(recipeElement4.material), Grid.SceneLayer.Ore, null, 0);
					int cell2 = Grid.PosToCell(this);
					gameObject3.transform.SetPosition(Grid.CellToPosCCC(cell2, Grid.SceneLayer.Ore) + this.outputOffset);
					float amount = recipeElement2.amount / recipeElement4.amount;
					gameObject3.GetComponent<PrimaryElement>().Temperature = ((recipeElement4.temperatureOperation == ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature) ? num : this.heatedTemperature);
					DehydratedFoodPackage component6 = gameObject3.GetComponent<DehydratedFoodPackage>();
					if (component6 != null)
					{
						Storage component7 = component6.GetComponent<Storage>();
						this.outStorage.TransferMass(component7, recipeElement2.material, amount, true, false, false);
					}
					gameObject3.SetActive(true);
					gameObject3.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
					list.Add(gameObject3);
					if (this.storeProduced || recipeElement4.storeElement)
					{
						this.outStorage.Store(gameObject3, false, false, true, false);
					}
				}
				break;
			}
			if (list.Count > 0)
			{
				SymbolOverrideController component8 = base.GetComponent<SymbolOverrideController>();
				if (component8 != null)
				{
					KAnim.Build build = list[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
					KAnim.Build.Symbol symbol = build.GetSymbol(build.name);
					if (symbol != null)
					{
						component8.TryRemoveSymbolOverride("output_tracker", 0);
						component8.AddSymbolOverride("output_tracker", symbol, 0);
					}
					else
					{
						global::Debug.LogWarning(component8.name + " is missing symbol " + build.name);
					}
				}
			}
		}
		if (recipe.producedHEP > 0)
		{
			base.GetComponent<HighEnergyParticleStorage>().Store((float)recipe.producedHEP);
		}
		return list;
	}

	// Token: 0x0600404E RID: 16462 RVA: 0x0023B120 File Offset: 0x00239320
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		ComplexRecipe[] recipes = this.GetRecipes();
		if (recipes.Length != 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.PROCESSES, UI.BUILDINGEFFECTS.TOOLTIPS.PROCESSES, Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		foreach (ComplexRecipe complexRecipe in recipes)
		{
			string text = "";
			string uiname = complexRecipe.GetUIName(false);
			foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
			{
				text = text + "• " + string.Format(UI.BUILDINGEFFECTS.PROCESSEDITEM, recipeElement.material.ProperName(), recipeElement.amount) + "\n";
			}
			Descriptor item2 = new Descriptor(uiname, string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.FABRICATOR_INGREDIENTS, text), Descriptor.DescriptorType.Effect, false);
			item2.IncreaseIndent();
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x0600404F RID: 16463 RVA: 0x000C9B47 File Offset: 0x000C7D47
	public virtual List<Descriptor> AdditionalEffectsForRecipe(ComplexRecipe recipe)
	{
		return new List<Descriptor>();
	}

	// Token: 0x06004050 RID: 16464 RVA: 0x0023B218 File Offset: 0x00239418
	public string GetConversationTopic()
	{
		if (this.HasWorkingOrder)
		{
			ComplexRecipe complexRecipe = this.recipe_list[this.workingOrderIdx];
			if (complexRecipe != null)
			{
				return complexRecipe.results[0].material.Name;
			}
		}
		return null;
	}

	// Token: 0x06004051 RID: 16465 RVA: 0x0023B254 File Offset: 0x00239454
	public bool NeedsMoreHEPForQueuedRecipe()
	{
		if (this.hasOpenOrders)
		{
			HighEnergyParticleStorage component = base.GetComponent<HighEnergyParticleStorage>();
			foreach (KeyValuePair<string, int> keyValuePair in this.recipeQueueCounts)
			{
				if (keyValuePair.Value > 0)
				{
					foreach (ComplexRecipe complexRecipe in this.GetRecipes())
					{
						if (complexRecipe.id == keyValuePair.Key && (float)complexRecipe.consumedHEP > component.Particles)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04002BCC RID: 11212
	private const int MaxPrefetchCount = 2;

	// Token: 0x04002BCD RID: 11213
	public bool duplicantOperated = true;

	// Token: 0x04002BCE RID: 11214
	protected ComplexFabricatorWorkable workable;

	// Token: 0x04002BCF RID: 11215
	public string SideScreenSubtitleLabel = UI.UISIDESCREENS.FABRICATORSIDESCREEN.SUBTITLE;

	// Token: 0x04002BD0 RID: 11216
	public string SideScreenRecipeScreenTitle = UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_DETAILS;

	// Token: 0x04002BD1 RID: 11217
	[SerializeField]
	public HashedString fetchChoreTypeIdHash = Db.Get().ChoreTypes.FabricateFetch.IdHash;

	// Token: 0x04002BD2 RID: 11218
	[SerializeField]
	public float heatedTemperature;

	// Token: 0x04002BD3 RID: 11219
	[SerializeField]
	public bool storeProduced;

	// Token: 0x04002BD4 RID: 11220
	[SerializeField]
	public bool allowManualFluidDelivery = true;

	// Token: 0x04002BD5 RID: 11221
	public ComplexFabricatorSideScreen.StyleSetting sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

	// Token: 0x04002BD6 RID: 11222
	public bool labelByResult = true;

	// Token: 0x04002BD7 RID: 11223
	public Vector3 outputOffset = Vector3.zero;

	// Token: 0x04002BD8 RID: 11224
	public ChoreType choreType;

	// Token: 0x04002BD9 RID: 11225
	public bool keepExcessLiquids;

	// Token: 0x04002BDA RID: 11226
	public Tag keepAdditionalTag = Tag.Invalid;

	// Token: 0x04002BDB RID: 11227
	public StatusItem workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorProducing;

	// Token: 0x04002BDC RID: 11228
	public static int MAX_QUEUE_SIZE = 99;

	// Token: 0x04002BDD RID: 11229
	public static int QUEUE_INFINITE = -1;

	// Token: 0x04002BDE RID: 11230
	[Serialize]
	private Dictionary<string, int> recipeQueueCounts = new Dictionary<string, int>();

	// Token: 0x04002BDF RID: 11231
	private int nextOrderIdx;

	// Token: 0x04002BE0 RID: 11232
	private bool nextOrderIsWorkable;

	// Token: 0x04002BE1 RID: 11233
	private int workingOrderIdx = -1;

	// Token: 0x04002BE2 RID: 11234
	[Serialize]
	private string lastWorkingRecipe;

	// Token: 0x04002BE3 RID: 11235
	[Serialize]
	private float orderProgress;

	// Token: 0x04002BE4 RID: 11236
	private List<int> openOrderCounts = new List<int>();

	// Token: 0x04002BE5 RID: 11237
	[Serialize]
	private bool forbidMutantSeeds;

	// Token: 0x04002BE6 RID: 11238
	private Tag[] forbiddenMutantTags = new Tag[]
	{
		GameTags.MutatedSeed
	};

	// Token: 0x04002BE7 RID: 11239
	private bool queueDirty = true;

	// Token: 0x04002BE8 RID: 11240
	private bool hasOpenOrders;

	// Token: 0x04002BE9 RID: 11241
	private List<FetchList2> fetchListList = new List<FetchList2>();

	// Token: 0x04002BEA RID: 11242
	private Chore chore;

	// Token: 0x04002BEB RID: 11243
	private bool cancelling;

	// Token: 0x04002BEC RID: 11244
	private ComplexRecipe[] recipe_list;

	// Token: 0x04002BED RID: 11245
	private Dictionary<Tag, float> materialNeedCache = new Dictionary<Tag, float>();

	// Token: 0x04002BEE RID: 11246
	[SerializeField]
	public Storage inStorage;

	// Token: 0x04002BEF RID: 11247
	[SerializeField]
	public Storage buildStorage;

	// Token: 0x04002BF0 RID: 11248
	[SerializeField]
	public Storage outStorage;

	// Token: 0x04002BF1 RID: 11249
	[MyCmpAdd]
	private LoopingSounds loopingSounds;

	// Token: 0x04002BF2 RID: 11250
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04002BF3 RID: 11251
	[MyCmpAdd]
	protected ComplexFabricatorSM fabricatorSM;

	// Token: 0x04002BF4 RID: 11252
	private ProgressBar progressBar;

	// Token: 0x04002BF5 RID: 11253
	public bool showProgressBar;

	// Token: 0x04002BF6 RID: 11254
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04002BF7 RID: 11255
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnParticleStorageChangedDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04002BF8 RID: 11256
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnDroppedAllDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnDroppedAll(data);
	});

	// Token: 0x04002BF9 RID: 11257
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04002BFA RID: 11258
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002BFB RID: 11259
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
