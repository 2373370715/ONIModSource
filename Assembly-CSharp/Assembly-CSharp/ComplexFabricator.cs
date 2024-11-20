using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/scripts/ComplexFabricator")]
public class ComplexFabricator : KMonoBehaviour, ISim200ms, ISim1000ms {
    private const int MaxPrefetchCount = 2;
    public static int MAX_QUEUE_SIZE   = 99;
    public static int QUEUE_INFINITE   = -1;

    private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnStorageChangeDelegate
        = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data) {
                                                                    component.OnStorageChange(data);
                                                                });

    private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnParticleStorageChangedDelegate
        = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data) {
                                                                    component.OnStorageChange(data);
                                                                });

    private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnDroppedAllDelegate
        = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data) {
                                                                    component.OnDroppedAll(data);
                                                                });

    private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnOperationalChangedDelegate
        = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data) {
                                                                    component.OnOperationalChanged(data);
                                                                });

    private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnCopySettingsDelegate
        = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data) {
                                                                    component.OnCopySettings(data);
                                                                });

    private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnRefreshUserMenuDelegate
        = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data) {
                                                                    component.OnRefreshUserMenu(data);
                                                                });

    [SerializeField]
    public bool allowManualFluidDelivery = true;

    [SerializeField]
    public Storage buildStorage;

    private bool      cancelling;
    private Chore     chore;
    public  ChoreType choreType;
    public  bool      duplicantOperated = true;

    [MyCmpAdd]
    protected ComplexFabricatorSM fabricatorSM;

    [SerializeField]
    public HashedString fetchChoreTypeIdHash = Db.Get().ChoreTypes.FabricateFetch.IdHash;

    private readonly Tag[] forbiddenMutantTags = { GameTags.MutatedSeed };

    [Serialize]
    private bool forbidMutantSeeds;

    private bool hasOpenOrders;

    [SerializeField]
    public float heatedTemperature;

    [SerializeField]
    public Storage inStorage;

    public Tag  keepAdditionalTag = Tag.Invalid;
    public bool keepExcessLiquids;
    public bool labelByResult = true;

    [Serialize]
    private string lastWorkingRecipe;

    [MyCmpAdd]
    private LoopingSounds loopingSounds;

    private readonly Dictionary<Tag, float> materialNeedCache = new Dictionary<Tag, float>();
    private          bool                   nextOrderIsWorkable;
    private readonly List<int>              openOrderCounts = new List<int>();

    [MyCmpReq]
    protected Operational operational;

    public Vector3 outputOffset = Vector3.zero;

    [SerializeField]
    public Storage outStorage;

    private ProgressBar     progressBar;
    private bool            queueDirty = true;
    private ComplexRecipe[] recipe_list;

    [Serialize]
    private readonly Dictionary<string, int> recipeQueueCounts = new Dictionary<string, int>();

    public bool   showProgressBar;
    public string SideScreenRecipeScreenTitle = UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_DETAILS;

    public ComplexFabricatorSideScreen.StyleSetting sideScreenStyle
        = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

    public string SideScreenSubtitleLabel = UI.UISIDESCREENS.FABRICATORSIDESCREEN.SUBTITLE;

    [SerializeField]
    public bool storeProduced;

    protected ComplexFabricatorWorkable workable;
    private   int                       workingOrderIdx   = -1;
    public    StatusItem                workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorProducing;
    public    ComplexFabricatorWorkable Workable => workable;

    public bool ForbidMutantSeeds {
        get => forbidMutantSeeds;
        set {
            forbidMutantSeeds = value;
            ToggleMutantSeedFetches();
            UpdateMutantSeedStatusItem();
        }
    }

    public Tag[] ForbiddenTags {
        get {
            if (!forbidMutantSeeds) return null;

            return forbiddenMutantTags;
        }
    }

    public int CurrentOrderIdx { get; private set; }

    public ComplexRecipe CurrentWorkingOrder {
        get {
            if (!HasWorkingOrder) return null;

            return recipe_list[workingOrderIdx];
        }
    }

    public ComplexRecipe NextOrder {
        get {
            if (!nextOrderIsWorkable) return null;

            return recipe_list[CurrentOrderIdx];
        }
    }

    [field: Serialize]
    public float OrderProgress { get; set; }

    public  bool             HasAnyOrder      => HasWorkingOrder    || hasOpenOrders;
    public  bool             HasWorker        => !duplicantOperated || workable.worker != null;
    public  bool             WaitingForWorker => HasWorkingOrder && !HasWorker;
    private bool             HasWorkingOrder  => workingOrderIdx > -1;
    public  List<FetchList2> DebugFetchLists  { get; } = new List<FetchList2>();

    public void Sim1000ms(float dt) {
        RefreshAndStartNextOrder();
        if (materialNeedCache.Count > 0 && DebugFetchLists.Count == 0) {
            Debug.LogWarningFormat(gameObject,
                                   "{0} has material needs cached, but no open fetches. materialNeedCache={1}, fetchListList={2}",
                                   gameObject,
                                   materialNeedCache.Count,
                                   DebugFetchLists.Count);

            queueDirty = true;
        }
    }

    public void Sim200ms(float dt) {
        if (!operational.IsOperational) return;

        operational.SetActive(HasWorkingOrder && HasWorker);
        if (!duplicantOperated && HasWorkingOrder) {
            var complexRecipe = recipe_list[workingOrderIdx];
            OrderProgress += dt / complexRecipe.time;
            if (OrderProgress >= 1f) {
                ShowProgressBar(false);
                CompleteWorkingOrder();
            }
        }
    }

    [OnDeserialized]
    protected virtual void OnDeserializedMethod() {
        var list = new List<string>();
        foreach (var text in recipeQueueCounts.Keys)
            if (ComplexRecipeManager.Get().GetRecipe(text) == null)
                list.Add(text);

        foreach (var text2 in list) {
            Debug.LogWarningFormat("{1} removing missing recipe from queue: {0}", text2, name);
            recipeQueueCounts.Remove(text2);
        }
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        GetRecipes();
        simRenderLoadBalance = true;
        choreType            = Db.Get().ChoreTypes.Fabricate;
        Subscribe(-1957399615, OnDroppedAllDelegate);
        Subscribe(-592767678,  OnOperationalChangedDelegate);
        Subscribe(-905833192,  OnCopySettingsDelegate);
        Subscribe(-1697596308, OnStorageChangeDelegate);
        Subscribe(-1837862626, OnParticleStorageChangedDelegate);
        workable = GetComponent<ComplexFabricatorWorkable>();
        Components.ComplexFabricators.Add(this);
        Subscribe(493375141, OnRefreshUserMenuDelegate);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        InitRecipeQueueCount();
        foreach (var key in recipeQueueCounts.Keys)
            if (recipeQueueCounts[key] == 100)
                recipeQueueCounts[key] = QUEUE_INFINITE;

        buildStorage.Transfer(inStorage, true, true);
        DropExcessIngredients(inStorage);
        var num                       = FindRecipeIndex(lastWorkingRecipe);
        if (num > -1) CurrentOrderIdx = num;
        UpdateMutantSeedStatusItem();
    }

    protected override void OnCleanUp() {
        CancelAllOpenOrders();
        CancelChore();
        Components.ComplexFabricators.Remove(this);
        base.OnCleanUp();
    }

    private void OnRefreshUserMenu(object data) {
        if (SaveLoader.Instance.IsDLCActiveForCurrentSave("EXPANSION1_ID") && HasRecipiesWithSeeds())
            Game.Instance.userMenu.AddButton(gameObject,
                                             new KIconButtonMenu.ButtonInfo("action_switch_toggle",
                                                                            ForbidMutantSeeds
                                                                                ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS
                                                                                    .ACCEPT
                                                                                : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS
                                                                                    .REJECT,
                                                                            delegate {
                                                                                ForbidMutantSeeds = !ForbidMutantSeeds;
                                                                                OnRefreshUserMenu(null);
                                                                            },
                                                                            Action.NumActions,
                                                                            null,
                                                                            null,
                                                                            null,
                                                                            UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS
                                                                              .TOOLTIP));
    }

    private bool HasRecipiesWithSeeds() {
        var result = false;
        var array  = recipe_list;
        for (var i = 0; i < array.Length; i++) {
            var ingredients = array[i].ingredients;
            for (var j = 0; j < ingredients.Length; j++) {
                var prefab = Assets.GetPrefab(ingredients[j].material);
                if (prefab != null && prefab.GetComponent<PlantableSeed>() != null) {
                    result = true;
                    break;
                }
            }
        }

        return result;
    }

    private void UpdateMutantSeedStatusItem() {
        gameObject.GetComponent<KSelectable>()
                  .ToggleStatusItem(Db.Get().BuildingStatusItems.FabricatorAcceptsMutantSeeds,
                                    SaveLoader.Instance.IsDLCActiveForCurrentSave("EXPANSION1_ID") &&
                                    HasRecipiesWithSeeds()                                         &&
                                    !forbidMutantSeeds);
    }

    private void OnOperationalChanged(object data) {
        if ((bool)data)
            queueDirty = true;
        else
            CancelAllOpenOrders();

        UpdateChore();
    }

    private void RefreshAndStartNextOrder() {
        if (!operational.IsOperational) return;

        if (queueDirty) RefreshQueue();
        if (!HasWorkingOrder && nextOrderIsWorkable) {
            ShowProgressBar(true);
            StartWorkingOrder(CurrentOrderIdx);
        }
    }

    public virtual float GetPercentComplete() { return OrderProgress; }

    private void ShowProgressBar(bool show) {
        if (show && showProgressBar && !duplicantOperated) {
            if (progressBar == null) progressBar = ProgressBar.CreateProgressBar(gameObject, GetPercentComplete);
            progressBar.enabled = true;
            progressBar.SetVisibility(true);
            return;
        }

        if (progressBar != null) {
            progressBar.gameObject.DeleteObject();
            progressBar = null;
        }
    }

    public void SetQueueDirty() { queueDirty = true; }

    private void RefreshQueue() {
        queueDirty = false;
        ValidateWorkingOrder();
        ValidateNextOrder();
        UpdateOpenOrders();
        DropExcessIngredients(inStorage);
        Trigger(1721324763, this);
    }

    private void StartWorkingOrder(int index) {
        Debug.Assert(!HasWorkingOrder, "machineOrderIdx already set");
        workingOrderIdx = index;
        if (recipe_list[workingOrderIdx].id != lastWorkingRecipe) {
            OrderProgress     = 0f;
            lastWorkingRecipe = recipe_list[workingOrderIdx].id;
        }

        TransferCurrentRecipeIngredientsForBuild();
        Debug.Assert(openOrderCounts[workingOrderIdx] > 0, "openOrderCount invalid");
        var list   = openOrderCounts;
        var index2 = workingOrderIdx;
        var num    = list[index2];
        list[index2] = num - 1;
        UpdateChore();
        Trigger(2023536846, recipe_list[workingOrderIdx]);
        AdvanceNextOrder();
    }

    private void CancelWorkingOrder() {
        Debug.Assert(HasWorkingOrder, "machineOrderIdx not set");
        buildStorage.Transfer(inStorage, true, true);
        workingOrderIdx = -1;
        OrderProgress   = 0f;
        UpdateChore();
    }

    public void CompleteWorkingOrder() {
        if (!HasWorkingOrder) {
            Debug.LogWarning("CompleteWorkingOrder called with no working order.", gameObject);
            return;
        }

        var complexRecipe = recipe_list[workingOrderIdx];
        SpawnOrderProduct(complexRecipe);
        var num = buildStorage.MassStored();
        if (num != 0f) {
            Debug.LogWarningFormat(gameObject,
                                   "{0} build storage contains mass {1} after order completion.",
                                   gameObject,
                                   num);

            buildStorage.Transfer(inStorage, true, true);
        }

        DecrementRecipeQueueCountInternal(complexRecipe);
        workingOrderIdx = -1;
        OrderProgress   = 0f;
        CancelChore();
        Trigger(1355439576, complexRecipe);
        if (!cancelling) RefreshAndStartNextOrder();
    }

    private void ValidateWorkingOrder() {
        if (!HasWorkingOrder) return;

        var recipe = recipe_list[workingOrderIdx];
        if (!IsRecipeQueued(recipe)) CancelWorkingOrder();
    }

    private void UpdateChore() {
        if (!duplicantOperated) return;

        var flag = operational.IsOperational && HasWorkingOrder;
        if (flag && chore == null) {
            CreateChore();
            return;
        }

        if (!flag && chore != null) CancelChore();
    }

    private void AdvanceNextOrder() {
        for (var i = 0; i < recipe_list.Length; i++) {
            CurrentOrderIdx = (CurrentOrderIdx + 1) % recipe_list.Length;
            var recipe = recipe_list[CurrentOrderIdx];
            nextOrderIsWorkable = GetRemainingQueueCount(recipe) > 0 && HasIngredients(recipe, inStorage);
            if (nextOrderIsWorkable) break;
        }
    }

    private void ValidateNextOrder() {
        var recipe = recipe_list[CurrentOrderIdx];
        nextOrderIsWorkable = GetRemainingQueueCount(recipe) > 0 && HasIngredients(recipe, inStorage);
        if (!nextOrderIsWorkable) AdvanceNextOrder();
    }

    private void CancelAllOpenOrders() {
        for (var i = 0; i < openOrderCounts.Count; i++) openOrderCounts[i] = 0;
        ClearMaterialNeeds();
        CancelFetches();
    }

    private void UpdateOpenOrders() {
        var recipes = GetRecipes();
        if (recipes.Length != openOrderCounts.Count)
            Debug.LogErrorFormat(gameObject,
                                 "Recipe count {0} doesn't match open order count {1}",
                                 recipes.Length,
                                 openOrderCounts.Count);

        var flag = false;
        hasOpenOrders = false;
        for (var i = 0; i < recipes.Length; i++) {
            var recipe                                 = recipes[i];
            var recipePrefetchCount                    = GetRecipePrefetchCount(recipe);
            if (recipePrefetchCount > 0) hasOpenOrders = true;
            var num                                    = openOrderCounts[i];
            if (num != recipePrefetchCount) {
                if (recipePrefetchCount < num) flag = true;
                openOrderCounts[i] = recipePrefetchCount;
            }
        }

        var pooledDictionary  = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
        var pooledDictionary2 = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
        for (var j = 0; j < openOrderCounts.Count; j++)
            if (openOrderCounts[j] > 0)
                foreach (var recipeElement in recipe_list[j].ingredients)
                    pooledDictionary[recipeElement.material] = inStorage.GetAmountAvailable(recipeElement.material);

        for (var l = 0; l < recipe_list.Length; l++) {
            var num2 = openOrderCounts[l];
            if (num2 > 0)
                foreach (var recipeElement2 in recipe_list[l].ingredients) {
                    var num3 = recipeElement2.amount * num2;
                    var num4 = num3 - pooledDictionary[recipeElement2.material];
                    if (num4 > 0f) {
                        float num5;
                        pooledDictionary2.TryGetValue(recipeElement2.material, out num5);
                        pooledDictionary2[recipeElement2.material] = num5 + num4;
                        pooledDictionary[recipeElement2.material]  = 0f;
                    } else {
                        var pooledDictionary3 = pooledDictionary;
                        var material          = recipeElement2.material;
                        pooledDictionary3[material] -= num3;
                    }
                }
        }

        if (flag) CancelFetches();
        if (pooledDictionary2.Count > 0) UpdateFetches(pooledDictionary2);
        UpdateMaterialNeeds(pooledDictionary2);
        pooledDictionary2.Recycle();
        pooledDictionary.Recycle();
    }

    private void UpdateMaterialNeeds(Dictionary<Tag, float> missingAmounts) {
        ClearMaterialNeeds();
        foreach (var keyValuePair in missingAmounts) {
            MaterialNeeds.UpdateNeed(keyValuePair.Key, keyValuePair.Value, gameObject.GetMyWorldId());
            materialNeedCache.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }

    private void ClearMaterialNeeds() {
        foreach (var keyValuePair in materialNeedCache)
            MaterialNeeds.UpdateNeed(keyValuePair.Key, -keyValuePair.Value, gameObject.GetMyWorldId());

        materialNeedCache.Clear();
    }

    public int HighestHEPQueued() {
        var num = 0;
        foreach (var keyValuePair in recipeQueueCounts)
            if (keyValuePair.Value > 0)
                num = Math.Max(recipe_list[FindRecipeIndex(keyValuePair.Key)].consumedHEP, num);

        return num;
    }

    private void OnFetchComplete() {
        for (var i = DebugFetchLists.Count - 1; i >= 0; i--)
            if (DebugFetchLists[i].IsComplete) {
                DebugFetchLists.RemoveAt(i);
                queueDirty = true;
            }
    }

    private void OnStorageChange(object data) { queueDirty = true; }

    private void OnDroppedAll(object data) {
        if (HasWorkingOrder) CancelWorkingOrder();
        CancelAllOpenOrders();
        RefreshQueue();
    }

    private void DropExcessIngredients(Storage storage) {
        var hashSet = new HashSet<Tag>();
        if (keepAdditionalTag != Tag.Invalid) hashSet.Add(keepAdditionalTag);
        for (var i = 0; i < recipe_list.Length; i++) {
            var complexRecipe = recipe_list[i];
            if (IsRecipeQueued(complexRecipe))
                foreach (var recipeElement in complexRecipe.ingredients)
                    hashSet.Add(recipeElement.material);
        }

        for (var k = storage.items.Count - 1; k >= 0; k--) {
            var gameObject = storage.items[k];
            if (!(gameObject == null)) {
                var component = gameObject.GetComponent<PrimaryElement>();
                if (!(component == null) && (!keepExcessLiquids || !component.Element.IsLiquid)) {
                    var component2 = gameObject.GetComponent<KPrefabID>();
                    if (component2 && !hashSet.Contains(component2.PrefabID())) storage.Drop(gameObject);
                }
            }
        }
    }

    private void OnCopySettings(object data) {
        var gameObject = (GameObject)data;
        if (gameObject == null) return;

        var component = gameObject.GetComponent<ComplexFabricator>();
        if (component == null) return;

        ForbidMutantSeeds = component.ForbidMutantSeeds;
        foreach (var complexRecipe in recipe_list) {
            int count;
            if (!component.recipeQueueCounts.TryGetValue(complexRecipe.id, out count)) count = 0;
            SetRecipeQueueCountInternal(complexRecipe, count);
        }

        RefreshQueue();
    }

    private int CompareRecipe(ComplexRecipe a, ComplexRecipe b) {
        if (a.sortOrder != b.sortOrder) return a.sortOrder - b.sortOrder;

        return StringComparer.InvariantCulture.Compare(a.id, b.id);
    }

    public ComplexRecipe[] GetRecipes() {
        if (recipe_list == null) {
            var prefabTag = GetComponent<KPrefabID>().PrefabTag;
            var recipes   = ComplexRecipeManager.Get().recipes;
            var list      = new List<ComplexRecipe>();
            foreach (var complexRecipe in recipes)
                using (var enumerator2 = complexRecipe.fabricators.GetEnumerator()) {
                    while (enumerator2.MoveNext())
                        if (enumerator2.Current == prefabTag &&
                            SaveLoader.Instance.IsDlcListActiveForCurrentSave(complexRecipe.GetDlcIds()))
                            list.Add(complexRecipe);
                }

            recipe_list = list.ToArray();
            Array.Sort(recipe_list, CompareRecipe);
        }

        return recipe_list;
    }

    private void InitRecipeQueueCount() {
        foreach (var complexRecipe in GetRecipes()) {
            var flag = false;
            using (var enumerator = recipeQueueCounts.Keys.GetEnumerator()) {
                while (enumerator.MoveNext())
                    if (enumerator.Current == complexRecipe.id) {
                        flag = true;
                        break;
                    }
            }

            if (!flag) recipeQueueCounts.Add(complexRecipe.id, 0);
            openOrderCounts.Add(0);
        }
    }

    private int FindRecipeIndex(string id) {
        for (var i = 0; i < recipe_list.Length; i++)
            if (recipe_list[i].id == id)
                return i;

        return -1;
    }

    public int GetRecipeQueueCount(ComplexRecipe recipe) { return recipeQueueCounts[recipe.id]; }

    public bool IsRecipeQueued(ComplexRecipe recipe) {
        var num = recipeQueueCounts[recipe.id];
        Debug.Assert(num >= 0 || num == QUEUE_INFINITE);
        return num != 0;
    }

    public int GetRecipePrefetchCount(ComplexRecipe recipe) {
        var remainingQueueCount = GetRemainingQueueCount(recipe);
        Debug.Assert(remainingQueueCount >= 0);
        return Mathf.Min(2, remainingQueueCount);
    }

    private int GetRemainingQueueCount(ComplexRecipe recipe) {
        var num = recipeQueueCounts[recipe.id];
        Debug.Assert(num >= 0 || num == QUEUE_INFINITE);
        if (num == QUEUE_INFINITE) return MAX_QUEUE_SIZE;

        if (num > 0) {
            if (IsCurrentRecipe(recipe)) num--;
            return num;
        }

        return 0;
    }

    private bool IsCurrentRecipe(ComplexRecipe recipe) {
        return workingOrderIdx >= 0 && recipe_list[workingOrderIdx].id == recipe.id;
    }

    public void SetRecipeQueueCount(ComplexRecipe recipe, int count) {
        SetRecipeQueueCountInternal(recipe, count);
        RefreshQueue();
    }

    private void SetRecipeQueueCountInternal(ComplexRecipe recipe, int count) { recipeQueueCounts[recipe.id] = count; }

    public void IncrementRecipeQueueCount(ComplexRecipe recipe) {
        if (recipeQueueCounts[recipe.id] == QUEUE_INFINITE)
            recipeQueueCounts[recipe.id] = 0;
        else if (recipeQueueCounts[recipe.id] >= MAX_QUEUE_SIZE)
            recipeQueueCounts[recipe.id] = QUEUE_INFINITE;
        else {
            var dictionary = recipeQueueCounts;
            var id         = recipe.id;
            var num        = dictionary[id];
            dictionary[id] = num + 1;
        }

        RefreshQueue();
    }

    public void DecrementRecipeQueueCount(ComplexRecipe recipe, bool respectInfinite = true) {
        DecrementRecipeQueueCountInternal(recipe, respectInfinite);
        RefreshQueue();
    }

    private void DecrementRecipeQueueCountInternal(ComplexRecipe recipe, bool respectInfinite = true) {
        if (!respectInfinite || recipeQueueCounts[recipe.id] != QUEUE_INFINITE) {
            if (recipeQueueCounts[recipe.id] == QUEUE_INFINITE) {
                recipeQueueCounts[recipe.id] = MAX_QUEUE_SIZE;
                return;
            }

            if (recipeQueueCounts[recipe.id] == 0) {
                recipeQueueCounts[recipe.id] = QUEUE_INFINITE;
                return;
            }

            var dictionary = recipeQueueCounts;
            var id         = recipe.id;
            var num        = dictionary[id];
            dictionary[id] = num - 1;
        }
    }

    private void CreateChore() {
        Debug.Assert(chore == null, "chore should be null");
        chore = workable.CreateWorkChore(choreType, OrderProgress);
    }

    private void CancelChore() {
        if (cancelling) return;

        cancelling = true;
        if (chore != null) {
            chore.Cancel("order cancelled");
            chore = null;
        }

        cancelling = false;
    }

    private void UpdateFetches(DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary missingAmounts) {
        var byHash = Db.Get().ChoreTypes.GetByHash(fetchChoreTypeIdHash);
        foreach (var keyValuePair in missingAmounts) {
            if (!allowManualFluidDelivery) {
                var element = ElementLoader.GetElement(keyValuePair.Key);
                if (element != null && (element.IsLiquid || element.IsGas)) continue;
            }

            if (keyValuePair.Value >= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !HasPendingFetch(keyValuePair.Key)) {
                var fetchList  = new FetchList2(inStorage, byHash);
                var fetchList2 = fetchList;
                var key        = keyValuePair.Key;
                var value      = keyValuePair.Value;
                fetchList2.Add(key, ForbiddenTags, value);
                fetchList.ShowStatusItem = false;
                fetchList.Submit(OnFetchComplete, false);
                DebugFetchLists.Add(fetchList);
            }
        }
    }

    private bool HasPendingFetch(Tag tag) {
        foreach (var fetchList in DebugFetchLists) {
            float num;
            fetchList.MinimumAmount.TryGetValue(tag, out num);
            if (num > 0f) return true;
        }

        return false;
    }

    private void CancelFetches() {
        foreach (var fetchList in DebugFetchLists) fetchList.Cancel("cancel all orders");
        DebugFetchLists.Clear();
    }

    protected virtual void TransferCurrentRecipeIngredientsForBuild() {
        var ingredients = recipe_list[workingOrderIdx].ingredients;
        var i           = 0;
        while (i < ingredients.Length) {
            var   recipeElement = ingredients[i];
            float num;
            for (;;) {
                num = recipeElement.amount - buildStorage.GetAmountAvailable(recipeElement.material);
                if (num <= 0f) break;

                if (inStorage.GetAmountAvailable(recipeElement.material) <= 0f) goto Block_2;

                inStorage.Transfer(buildStorage, recipeElement.material, num, false, true);
            }

            IL_9D:
            i++;
            continue;

            Block_2:
            Debug.LogWarningFormat("TransferCurrentRecipeIngredientsForBuild ran out of {0} but still needed {1} more.",
                                   recipeElement.material,
                                   num);

            goto IL_9D;
        }
    }

    protected virtual bool HasIngredients(ComplexRecipe recipe, Storage storage) {
        var ingredients = recipe.ingredients;
        if (recipe.consumedHEP > 0) {
            var component = GetComponent<HighEnergyParticleStorage>();
            if (component == null || component.Particles < recipe.consumedHEP) return false;
        }

        foreach (var recipeElement in ingredients) {
            var amountAvailable = storage.GetAmountAvailable(recipeElement.material);
            if (recipeElement.amount - amountAvailable >= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT) return false;
        }

        return true;
    }

    private void ToggleMutantSeedFetches() {
        if (HasAnyOrder) {
            var byHash = Db.Get().ChoreTypes.GetByHash(fetchChoreTypeIdHash);
            var list   = new List<FetchList2>();
            foreach (var fetchList in DebugFetchLists) {
                foreach (var fetchOrder in fetchList.FetchOrders) {
                    foreach (var tag in fetchOrder.Tags) {
                        var prefab = Assets.GetPrefab(tag);
                        if (prefab != null && prefab.GetComponent<PlantableSeed>() != null) {
                            fetchList.Cancel("MutantSeedTagChanged");
                            list.Add(fetchList);
                        }
                    }
                }
            }

            foreach (var fetchList2 in list) {
                DebugFetchLists.Remove(fetchList2);
                foreach (var fetchOrder2 in fetchList2.FetchOrders) {
                    foreach (var tag2 in fetchOrder2.Tags) {
                        var fetchList3  = new FetchList2(inStorage, byHash);
                        var fetchList4  = fetchList3;
                        var tag3        = tag2;
                        var totalAmount = fetchOrder2.TotalAmount;
                        fetchList4.Add(tag3, ForbiddenTags, totalAmount);
                        fetchList3.ShowStatusItem = false;
                        fetchList3.Submit(OnFetchComplete, false);
                        DebugFetchLists.Add(fetchList3);
                    }
                }
            }
        }
    }

    protected virtual List<GameObject> SpawnOrderProduct(ComplexRecipe recipe) {
        var                 list = new List<GameObject>();
        SimUtil.DiseaseInfo diseaseInfo;
        diseaseInfo.count = 0;
        diseaseInfo.idx   = 0;
        var    num                                             = 0f;
        var    num2                                            = 0f;
        string text                                            = null;
        foreach (var recipeElement in recipe.ingredients) num2 += recipeElement.amount;
        ComplexRecipe.RecipeElement recipeElement2             = null;
        foreach (var recipeElement3 in recipe.ingredients) {
            var num3 = recipeElement3.amount / num2;
            if (recipe.ProductHasFacade && text.IsNullOrWhiteSpace()) {
                var component = buildStorage.FindFirst(recipeElement3.material).GetComponent<RepairableEquipment>();
                if (component != null) text = component.facadeID;
            }

            if (recipeElement3.inheritElement || recipeElement3.Edible) recipeElement2 = recipeElement3;
            if (recipeElement3.Edible)
                buildStorage.TransferMass(outStorage, recipeElement3.material, recipeElement3.amount, true, true, true);
            else {
                float               num4;
                SimUtil.DiseaseInfo diseaseInfo2;
                float               num5;
                buildStorage.ConsumeAndGetDisease(recipeElement3.material,
                                                  recipeElement3.amount,
                                                  out num4,
                                                  out diseaseInfo2,
                                                  out num5);

                if (diseaseInfo2.count > diseaseInfo.count) diseaseInfo = diseaseInfo2;
                num += num5 * num3;
            }
        }

        if (recipe.consumedHEP > 0) GetComponent<HighEnergyParticleStorage>().ConsumeAndGet(recipe.consumedHEP);
        foreach (var recipeElement4 in recipe.results) {
            var gameObject = buildStorage.FindFirst(recipeElement4.material);
            if (gameObject != null) {
                var component2 = gameObject.GetComponent<Edible>();
                if (component2)
                    ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated,
                                                       -component2.Calories,
                                                       StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED,
                                                                               "{0}",
                                                                               component2.GetProperName()),
                                                       UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
            }

            switch (recipeElement4.temperatureOperation) {
                case ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature:
                case ComplexRecipe.RecipeElement.TemperatureOperation.Heated: {
                    var gameObject2
                        = GameUtil.KInstantiate(Assets.GetPrefab(recipeElement4.material), Grid.SceneLayer.Ore);

                    var cell = Grid.PosToCell(this);
                    gameObject2.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore) + outputOffset);
                    var component3 = gameObject2.GetComponent<PrimaryElement>();
                    component3.Units = recipeElement4.amount;
                    component3.Temperature
                        = recipeElement4.temperatureOperation ==
                          ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature
                              ? num
                              : heatedTemperature;

                    if (recipeElement2 != null) {
                        var element = ElementLoader.GetElement(recipeElement2.material);
                        if (element != null) component3.SetElement(element.id, false);
                    }

                    if (recipe.ProductHasFacade && !text.IsNullOrWhiteSpace()) {
                        var component4 = gameObject2.GetComponent<Equippable>();
                        if (component4 != null) EquippableFacade.AddFacadeToEquippable(component4, text);
                    }

                    gameObject2.SetActive(true);
                    var num6 = recipeElement4.amount / recipe.TotalResultUnits();
                    component3.AddDisease(diseaseInfo.idx,
                                          Mathf.RoundToInt(diseaseInfo.count * num6),
                                          "ComplexFabricator.CompleteOrder");

                    if (!recipeElement4.facadeID.IsNullOrWhiteSpace()) {
                        var component5 = gameObject2.GetComponent<Equippable>();
                        if (component5 != null)
                            EquippableFacade.AddFacadeToEquippable(component5, recipeElement4.facadeID);
                    }

                    gameObject2.GetComponent<KMonoBehaviour>().Trigger(748399584);
                    list.Add(gameObject2);
                    if (storeProduced || recipeElement4.storeElement) outStorage.Store(gameObject2);
                    break;
                }
                case ComplexRecipe.RecipeElement.TemperatureOperation.Melted:
                    if (storeProduced || recipeElement4.storeElement) {
                        var temperature = ElementLoader.GetElement(recipeElement4.material).defaultValues.temperature;
                        outStorage.AddLiquid(ElementLoader.GetElementID(recipeElement4.material),
                                             recipeElement4.amount,
                                             temperature,
                                             0,
                                             0);
                    }

                    break;
                case ComplexRecipe.RecipeElement.TemperatureOperation.Dehydrated:
                    for (var j = 0; j < (int)recipeElement4.amount; j++) {
                        var gameObject3
                            = GameUtil.KInstantiate(Assets.GetPrefab(recipeElement4.material), Grid.SceneLayer.Ore);

                        var cell2 = Grid.PosToCell(this);
                        gameObject3.transform.SetPosition(Grid.CellToPosCCC(cell2, Grid.SceneLayer.Ore) + outputOffset);
                        var amount = recipeElement2.amount / recipeElement4.amount;
                        gameObject3.GetComponent<PrimaryElement>().Temperature
                            = recipeElement4.temperatureOperation ==
                              ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature
                                  ? num
                                  : heatedTemperature;

                        var component6 = gameObject3.GetComponent<DehydratedFoodPackage>();
                        if (component6 != null) {
                            var component7 = component6.GetComponent<Storage>();
                            outStorage.TransferMass(component7, recipeElement2.material, amount, true);
                        }

                        gameObject3.SetActive(true);
                        gameObject3.GetComponent<KMonoBehaviour>().Trigger(748399584);
                        list.Add(gameObject3);
                        if (storeProduced || recipeElement4.storeElement) outStorage.Store(gameObject3);
                    }

                    break;
            }

            if (list.Count > 0) {
                var component8 = GetComponent<SymbolOverrideController>();
                if (component8 != null) {
                    var build  = list[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
                    var symbol = build.GetSymbol(build.name);
                    if (symbol != null) {
                        component8.TryRemoveSymbolOverride("output_tracker");
                        component8.AddSymbolOverride("output_tracker", symbol);
                    } else
                        Debug.LogWarning(component8.name + " is missing symbol " + build.name);
                }
            }
        }

        if (recipe.producedHEP > 0) GetComponent<HighEnergyParticleStorage>().Store(recipe.producedHEP);
        return list;
    }

    public virtual List<Descriptor> GetDescriptors(GameObject go) {
        var list    = new List<Descriptor>();
        var recipes = GetRecipes();
        if (recipes.Length != 0) {
            var item = default(Descriptor);
            item.SetupDescriptor(UI.BUILDINGEFFECTS.PROCESSES, UI.BUILDINGEFFECTS.TOOLTIPS.PROCESSES);
            list.Add(item);
        }

        foreach (var complexRecipe in recipes) {
            var text   = "";
            var uiname = complexRecipe.GetUIName(false);
            foreach (var recipeElement in complexRecipe.ingredients)
                text = text +
                       "• " +
                       string.Format(UI.BUILDINGEFFECTS.PROCESSEDITEM,
                                     recipeElement.material.ProperName(),
                                     recipeElement.amount) +
                       "\n";

            var item2 = new Descriptor(uiname, string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.FABRICATOR_INGREDIENTS, text));
            item2.IncreaseIndent();
            list.Add(item2);
        }

        return list;
    }

    public virtual List<Descriptor> AdditionalEffectsForRecipe(ComplexRecipe recipe) { return new List<Descriptor>(); }

    public string GetConversationTopic() {
        if (HasWorkingOrder) {
            var complexRecipe = recipe_list[workingOrderIdx];
            if (complexRecipe != null) return complexRecipe.results[0].material.Name;
        }

        return null;
    }

    public bool NeedsMoreHEPForQueuedRecipe() {
        if (hasOpenOrders) {
            var component = GetComponent<HighEnergyParticleStorage>();
            foreach (var keyValuePair in recipeQueueCounts)
                if (keyValuePair.Value > 0)
                    foreach (var complexRecipe in GetRecipes())
                        if (complexRecipe.id == keyValuePair.Key && complexRecipe.consumedHEP > component.Particles)
                            return true;

            return false;
        }

        return false;
    }
}