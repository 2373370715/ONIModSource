using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FAE RID: 8110
public class ReceptacleSideScreen : SideScreenContent, IRender1000ms
{
	// Token: 0x0600AB6B RID: 43883 RVA: 0x00409FF0 File Offset: 0x004081F0
	public override string GetTitle()
	{
		if (this.targetReceptacle == null)
		{
			return Strings.Get(this.titleKey).ToString().Replace("{0}", "");
		}
		return string.Format(Strings.Get(this.titleKey), this.targetReceptacle.GetProperName());
	}

	// Token: 0x0600AB6C RID: 43884 RVA: 0x0040A04C File Offset: 0x0040824C
	public void Initialize(SingleEntityReceptacle target)
	{
		if (target == null)
		{
			global::Debug.LogError("SingleObjectReceptacle provided was null.");
			return;
		}
		this.targetReceptacle = target;
		base.gameObject.SetActive(true);
		this.depositObjectMap = new Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity>();
		this.entityToggles.ForEach(delegate(ReceptacleToggle rbi)
		{
			UnityEngine.Object.Destroy(rbi.gameObject);
		});
		this.entityToggles.Clear();
		foreach (Tag tag in this.targetReceptacle.possibleDepositObjectTags)
		{
			List<GameObject> prefabsWithTag = Assets.GetPrefabsWithTag(tag);
			int num = prefabsWithTag.Count;
			List<IHasSortOrder> list = new List<IHasSortOrder>();
			foreach (GameObject gameObject in prefabsWithTag)
			{
				if (!this.targetReceptacle.IsValidEntity(gameObject))
				{
					num--;
				}
				else
				{
					IHasSortOrder component = gameObject.GetComponent<IHasSortOrder>();
					if (component != null)
					{
						list.Add(component);
					}
				}
			}
			global::Debug.Assert(list.Count == num, "Not all entities in this receptacle implement IHasSortOrder!");
			list.Sort((IHasSortOrder a, IHasSortOrder b) => a.sortOrder - b.sortOrder);
			foreach (IHasSortOrder hasSortOrder in list)
			{
				GameObject gameObject2 = (hasSortOrder as MonoBehaviour).gameObject;
				GameObject gameObject3 = Util.KInstantiateUI(this.entityToggle, this.requestObjectList, false);
				gameObject3.SetActive(true);
				ReceptacleToggle newToggle = gameObject3.GetComponent<ReceptacleToggle>();
				IReceptacleDirection component2 = gameObject2.GetComponent<IReceptacleDirection>();
				string entityName = this.GetEntityName(gameObject2.PrefabID());
				newToggle.title.text = entityName;
				Sprite entityIcon = this.GetEntityIcon(gameObject2.PrefabID());
				if (entityIcon == null)
				{
					entityIcon = this.elementPlaceholderSpr;
				}
				newToggle.image.sprite = entityIcon;
				newToggle.toggle.onClick += delegate()
				{
					this.ToggleClicked(newToggle);
				};
				newToggle.toggle.onPointerEnter += delegate()
				{
					this.CheckAmountsAndUpdate(null);
				};
				ToolTip component3 = newToggle.GetComponent<ToolTip>();
				if (component3 != null)
				{
					component3.SetSimpleTooltip(this.GetEntityTooltip(gameObject2.PrefabID()));
				}
				this.depositObjectMap.Add(newToggle, new ReceptacleSideScreen.SelectableEntity
				{
					tag = gameObject2.PrefabID(),
					direction = ((component2 != null) ? component2.Direction : SingleEntityReceptacle.ReceptacleDirection.Top),
					asset = gameObject2
				});
				this.entityToggles.Add(newToggle);
			}
		}
		this.RestoreSelectionFromOccupant();
		this.selectedEntityToggle = null;
		if (this.entityToggles.Count > 0)
		{
			if (this.entityPreviousSelectionMap.ContainsKey(this.targetReceptacle))
			{
				int index = this.entityPreviousSelectionMap[this.targetReceptacle];
				this.ToggleClicked(this.entityToggles[index]);
			}
			else
			{
				this.subtitleLabel.SetText(Strings.Get(this.subtitleStringSelect).ToString());
				this.requestSelectedEntityBtn.isInteractable = false;
				this.descriptionLabel.SetText(Strings.Get(this.subtitleStringSelectDescription).ToString());
				this.HideAllDescriptorPanels();
			}
		}
		this.onStorageChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1697596308, new Action<object>(this.CheckAmountsAndUpdate));
		this.onOccupantValidChangedHandle = this.targetReceptacle.gameObject.Subscribe(-1820564715, new Action<object>(this.OnOccupantValidChanged));
		this.UpdateState(null);
		SimAndRenderScheduler.instance.Add(this, false);
	}

	// Token: 0x0600AB6D RID: 43885 RVA: 0x0040A46C File Offset: 0x0040866C
	protected virtual void UpdateState(object data)
	{
		this.requestSelectedEntityBtn.ClearOnClick();
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (this.CheckReceptacleOccupied())
		{
			Uprootable uprootable = this.targetReceptacle.Occupant.GetComponent<Uprootable>();
			if (uprootable != null && uprootable.IsMarkedForUproot)
			{
				this.requestSelectedEntityBtn.onClick += delegate()
				{
					uprootable.ForceCancelUproot(null);
					this.UpdateState(null);
				};
				this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelRemove).ToString();
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingRemoval).ToString(), this.targetReceptacle.Occupant.GetProperName()));
			}
			else
			{
				this.requestSelectedEntityBtn.onClick += delegate()
				{
					this.targetReceptacle.OrderRemoveOccupant();
					this.UpdateState(null);
				};
				this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringRemove).ToString();
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringEntityDeposited).ToString(), this.targetReceptacle.Occupant.GetProperName()));
			}
			this.requestSelectedEntityBtn.isInteractable = true;
			this.ToggleObjectPicker(false);
			Tag tag = this.targetReceptacle.Occupant.GetComponent<KSelectable>().PrefabID();
			this.ConfigureActiveEntity(tag);
			this.SetResultDescriptions(this.targetReceptacle.Occupant);
		}
		else if (this.targetReceptacle.GetActiveRequest != null)
		{
			this.requestSelectedEntityBtn.onClick += delegate()
			{
				this.targetReceptacle.CancelActiveRequest();
				this.ClearSelection();
				this.UpdateAvailableAmounts(null);
				this.UpdateState(null);
			};
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringCancelDeposit).ToString();
			this.requestSelectedEntityBtn.isInteractable = true;
			this.ToggleObjectPicker(false);
			this.ConfigureActiveEntity(this.targetReceptacle.GetActiveRequest.tagsFirst);
			GameObject prefab = Assets.GetPrefab(this.targetReceptacle.GetActiveRequest.tagsFirst);
			if (prefab != null)
			{
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingDelivery).ToString(), prefab.GetProperName()));
				this.SetResultDescriptions(prefab);
			}
		}
		else if (this.selectedEntityToggle != null)
		{
			this.requestSelectedEntityBtn.onClick += delegate()
			{
				this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
				this.UpdateAvailableAmounts(null);
				this.UpdateState(null);
			};
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
			this.targetReceptacle.SetPreview(this.depositObjectMap[this.selectedEntityToggle].tag, false);
			bool flag = this.CanDepositEntity(this.depositObjectMap[this.selectedEntityToggle]);
			this.requestSelectedEntityBtn.isInteractable = flag;
			this.SetImageToggleState(this.selectedEntityToggle.toggle, flag ? ImageToggleState.State.Active : ImageToggleState.State.DisabledActive);
			this.ToggleObjectPicker(true);
			GameObject prefab2 = Assets.GetPrefab(this.selectedDepositObjectTag);
			if (prefab2 != null)
			{
				this.subtitleLabel.SetText(string.Format(Strings.Get(this.subtitleStringAwaitingSelection).ToString(), prefab2.GetProperName()));
				this.SetResultDescriptions(prefab2);
			}
		}
		else
		{
			this.requestSelectedEntityBtn.GetComponentInChildren<LocText>().text = Strings.Get(this.requestStringDeposit).ToString();
			this.requestSelectedEntityBtn.isInteractable = false;
			this.ToggleObjectPicker(true);
		}
		this.UpdateAvailableAmounts(null);
		this.UpdateListeners();
	}

	// Token: 0x0600AB6E RID: 43886 RVA: 0x0040A7EC File Offset: 0x004089EC
	private void UpdateListeners()
	{
		if (this.CheckReceptacleOccupied())
		{
			if (this.onObjectDestroyedHandle == -1)
			{
				this.onObjectDestroyedHandle = this.targetReceptacle.Occupant.gameObject.Subscribe(1969584890, delegate(object d)
				{
					this.UpdateState(null);
				});
				return;
			}
		}
		else if (this.onObjectDestroyedHandle != -1)
		{
			this.onObjectDestroyedHandle = -1;
		}
	}

	// Token: 0x0600AB6F RID: 43887 RVA: 0x0040A848 File Offset: 0x00408A48
	private void OnOccupantValidChanged(object obj)
	{
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (!this.CheckReceptacleOccupied() && this.targetReceptacle.GetActiveRequest != null)
		{
			bool flag = false;
			ReceptacleSideScreen.SelectableEntity entity;
			if (this.depositObjectMap.TryGetValue(this.selectedEntityToggle, out entity))
			{
				flag = this.CanDepositEntity(entity);
			}
			if (!flag)
			{
				this.targetReceptacle.CancelActiveRequest();
				this.ClearSelection();
				this.UpdateState(null);
				this.UpdateAvailableAmounts(null);
			}
		}
	}

	// Token: 0x0600AB70 RID: 43888 RVA: 0x0010F53F File Offset: 0x0010D73F
	private bool CanDepositEntity(ReceptacleSideScreen.SelectableEntity entity)
	{
		return this.ValidRotationForDeposit(entity.direction) && (!this.RequiresAvailableAmountToDeposit() || this.GetAvailableAmount(entity.tag) > 0f) && this.AdditionalCanDepositTest();
	}

	// Token: 0x0600AB71 RID: 43889 RVA: 0x000A65EC File Offset: 0x000A47EC
	protected virtual bool AdditionalCanDepositTest()
	{
		return true;
	}

	// Token: 0x0600AB72 RID: 43890 RVA: 0x000A65EC File Offset: 0x000A47EC
	protected virtual bool RequiresAvailableAmountToDeposit()
	{
		return true;
	}

	// Token: 0x0600AB73 RID: 43891 RVA: 0x0040A8BC File Offset: 0x00408ABC
	private void ClearSelection()
	{
		foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
		{
			keyValuePair.Key.toggle.Deselect();
		}
	}

	// Token: 0x0600AB74 RID: 43892 RVA: 0x0040A91C File Offset: 0x00408B1C
	private void ToggleObjectPicker(bool Show)
	{
		this.requestObjectListContainer.SetActive(Show);
		if (this.scrollBarContainer != null)
		{
			this.scrollBarContainer.SetActive(Show);
		}
		this.requestObjectList.SetActive(Show);
		this.activeEntityContainer.SetActive(!Show);
	}

	// Token: 0x0600AB75 RID: 43893 RVA: 0x0040A96C File Offset: 0x00408B6C
	private void ConfigureActiveEntity(Tag tag)
	{
		string properName = Assets.GetPrefab(tag).GetProperName();
		this.activeEntityContainer.GetComponentInChildrenOnly<LocText>().text = properName;
		this.activeEntityContainer.transform.GetChild(0).gameObject.GetComponentInChildrenOnly<Image>().sprite = this.GetEntityIcon(tag);
	}

	// Token: 0x0600AB76 RID: 43894 RVA: 0x0010F572 File Offset: 0x0010D772
	protected virtual string GetEntityName(Tag prefabTag)
	{
		return Assets.GetPrefab(prefabTag).GetProperName();
	}

	// Token: 0x0600AB77 RID: 43895 RVA: 0x0040A9C0 File Offset: 0x00408BC0
	protected virtual string GetEntityTooltip(Tag prefabTag)
	{
		InfoDescription component = Assets.GetPrefab(prefabTag).GetComponent<InfoDescription>();
		string text = this.GetEntityName(prefabTag);
		if (component != null)
		{
			text = text + "\n\n" + component.description;
		}
		return text;
	}

	// Token: 0x0600AB78 RID: 43896 RVA: 0x0010E7A7 File Offset: 0x0010C9A7
	protected virtual Sprite GetEntityIcon(Tag prefabTag)
	{
		return Def.GetUISprite(Assets.GetPrefab(prefabTag), "ui", false).first;
	}

	// Token: 0x0600AB79 RID: 43897 RVA: 0x0040AA00 File Offset: 0x00408C00
	public override bool IsValidForTarget(GameObject target)
	{
		SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
		return component != null && component.enabled && target.GetComponent<PlantablePlot>() == null && target.GetComponent<EggIncubator>() == null && target.GetComponent<SpecialCargoBayClusterReceptacle>() == null;
	}

	// Token: 0x0600AB7A RID: 43898 RVA: 0x0040AA50 File Offset: 0x00408C50
	public override void SetTarget(GameObject target)
	{
		SingleEntityReceptacle component = target.GetComponent<SingleEntityReceptacle>();
		if (component == null)
		{
			global::Debug.LogError("The object selected doesn't have a SingleObjectReceptacle!");
			return;
		}
		this.Initialize(component);
		this.UpdateState(null);
	}

	// Token: 0x0600AB7B RID: 43899 RVA: 0x000A5E40 File Offset: 0x000A4040
	protected virtual void RestoreSelectionFromOccupant()
	{
	}

	// Token: 0x0600AB7C RID: 43900 RVA: 0x0040AA88 File Offset: 0x00408C88
	public override void ClearTarget()
	{
		if (this.targetReceptacle != null)
		{
			if (this.CheckReceptacleOccupied())
			{
				this.targetReceptacle.Occupant.gameObject.Unsubscribe(this.onObjectDestroyedHandle);
				this.onObjectDestroyedHandle = -1;
			}
			this.targetReceptacle.Unsubscribe(this.onStorageChangedHandle);
			this.onStorageChangedHandle = -1;
			this.targetReceptacle.Unsubscribe(this.onOccupantValidChangedHandle);
			this.onOccupantValidChangedHandle = -1;
			if (this.targetReceptacle.GetActiveRequest == null)
			{
				this.targetReceptacle.SetPreview(Tag.Invalid, false);
			}
			SimAndRenderScheduler.instance.Remove(this);
			this.targetReceptacle = null;
		}
	}

	// Token: 0x0600AB7D RID: 43901 RVA: 0x0040AB30 File Offset: 0x00408D30
	protected void SetImageToggleState(KToggle toggle, ImageToggleState.State state)
	{
		switch (state)
		{
		case ImageToggleState.State.Disabled:
			toggle.GetComponent<ImageToggleState>().SetDisabled();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.desaturatedMaterial;
			return;
		case ImageToggleState.State.Inactive:
			toggle.GetComponent<ImageToggleState>().SetInactive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.defaultMaterial;
			return;
		case ImageToggleState.State.Active:
			toggle.GetComponent<ImageToggleState>().SetActive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.defaultMaterial;
			return;
		case ImageToggleState.State.DisabledActive:
			toggle.GetComponent<ImageToggleState>().SetDisabledActive();
			toggle.gameObject.GetComponentInChildrenOnly<Image>().material = this.desaturatedMaterial;
			return;
		default:
			return;
		}
	}

	// Token: 0x0600AB7E RID: 43902 RVA: 0x0010F57F File Offset: 0x0010D77F
	public void Render1000ms(float dt)
	{
		this.CheckAmountsAndUpdate(null);
	}

	// Token: 0x0600AB7F RID: 43903 RVA: 0x0010F588 File Offset: 0x0010D788
	private void CheckAmountsAndUpdate(object data)
	{
		if (this.targetReceptacle == null)
		{
			return;
		}
		if (this.UpdateAvailableAmounts(null))
		{
			this.UpdateState(null);
		}
	}

	// Token: 0x0600AB80 RID: 43904 RVA: 0x0040ABDC File Offset: 0x00408DDC
	private bool UpdateAvailableAmounts(object data)
	{
		bool result = false;
		foreach (KeyValuePair<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> keyValuePair in this.depositObjectMap)
		{
			if (!DebugHandler.InstantBuildMode && this.hideUndiscoveredEntities && !DiscoveredResources.Instance.IsDiscovered(keyValuePair.Value.tag))
			{
				keyValuePair.Key.gameObject.SetActive(false);
			}
			else if (!keyValuePair.Key.gameObject.activeSelf)
			{
				keyValuePair.Key.gameObject.SetActive(true);
			}
			float availableAmount = this.GetAvailableAmount(keyValuePair.Value.tag);
			if (keyValuePair.Value.lastAmount != availableAmount)
			{
				result = true;
				keyValuePair.Value.lastAmount = availableAmount;
				keyValuePair.Key.amount.text = availableAmount.ToString();
			}
			if (!this.ValidRotationForDeposit(keyValuePair.Value.direction) || availableAmount <= 0f)
			{
				if (this.selectedEntityToggle != keyValuePair.Key)
				{
					this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Disabled);
				}
				else
				{
					this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.DisabledActive);
				}
			}
			else if (this.selectedEntityToggle != keyValuePair.Key)
			{
				this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Inactive);
			}
			else
			{
				this.SetImageToggleState(keyValuePair.Key.toggle, ImageToggleState.State.Active);
			}
		}
		return result;
	}

	// Token: 0x0600AB81 RID: 43905 RVA: 0x0040AD7C File Offset: 0x00408F7C
	protected float GetAvailableAmount(Tag tag)
	{
		if (this.ALLOW_ORDER_IGNORING_WOLRD_NEED)
		{
			IEnumerable<Pickupable> pickupables = this.targetReceptacle.GetMyWorld().worldInventory.GetPickupables(tag, true);
			float num = 0f;
			foreach (Pickupable pickupable in pickupables)
			{
				num += (float)Mathf.CeilToInt(pickupable.TotalAmount);
			}
			return num;
		}
		return this.targetReceptacle.GetMyWorld().worldInventory.GetAmount(tag, true);
	}

	// Token: 0x0600AB82 RID: 43906 RVA: 0x0010F5A9 File Offset: 0x0010D7A9
	private bool ValidRotationForDeposit(SingleEntityReceptacle.ReceptacleDirection depositDir)
	{
		return this.targetReceptacle.rotatable == null || depositDir == this.targetReceptacle.Direction;
	}

	// Token: 0x0600AB83 RID: 43907 RVA: 0x0040AE0C File Offset: 0x0040900C
	protected virtual void ToggleClicked(ReceptacleToggle toggle)
	{
		if (!this.depositObjectMap.ContainsKey(toggle))
		{
			global::Debug.LogError("Recipe not found on recipe list.");
			return;
		}
		if (this.selectedEntityToggle != null)
		{
			bool flag = this.CanDepositEntity(this.depositObjectMap[this.selectedEntityToggle]);
			this.requestSelectedEntityBtn.isInteractable = flag;
			this.SetImageToggleState(this.selectedEntityToggle.toggle, flag ? ImageToggleState.State.Inactive : ImageToggleState.State.Disabled);
		}
		this.selectedEntityToggle = toggle;
		this.entityPreviousSelectionMap[this.targetReceptacle] = this.entityToggles.IndexOf(toggle);
		this.selectedDepositObjectTag = this.depositObjectMap[toggle].tag;
		MutantPlant component = this.depositObjectMap[toggle].asset.GetComponent<MutantPlant>();
		this.selectedDepositObjectAdditionalTag = (component ? component.SubSpeciesID : Tag.Invalid);
		this.UpdateAvailableAmounts(null);
		this.UpdateState(null);
	}

	// Token: 0x0600AB84 RID: 43908 RVA: 0x0010F5CE File Offset: 0x0010D7CE
	private void CreateOrder(bool isInfinite)
	{
		this.targetReceptacle.CreateOrder(this.selectedDepositObjectTag, this.selectedDepositObjectAdditionalTag);
	}

	// Token: 0x0600AB85 RID: 43909 RVA: 0x0010F5E7 File Offset: 0x0010D7E7
	protected bool CheckReceptacleOccupied()
	{
		return this.targetReceptacle != null && this.targetReceptacle.Occupant != null;
	}

	// Token: 0x0600AB86 RID: 43910 RVA: 0x0040AEF8 File Offset: 0x004090F8
	protected virtual void SetResultDescriptions(GameObject go)
	{
		string text = "";
		InfoDescription component = go.GetComponent<InfoDescription>();
		if (component)
		{
			text = component.description;
		}
		else
		{
			KPrefabID component2 = go.GetComponent<KPrefabID>();
			if (component2 != null)
			{
				Element element = ElementLoader.GetElement(component2.PrefabID());
				if (element != null)
				{
					text = element.Description();
				}
			}
			else
			{
				text = go.GetProperName();
			}
		}
		this.descriptionLabel.SetText(text);
	}

	// Token: 0x0600AB87 RID: 43911 RVA: 0x0040AF60 File Offset: 0x00409160
	protected virtual void HideAllDescriptorPanels()
	{
		for (int i = 0; i < this.descriptorPanels.Count; i++)
		{
			this.descriptorPanels[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x040086B1 RID: 34481
	protected bool ALLOW_ORDER_IGNORING_WOLRD_NEED = true;

	// Token: 0x040086B2 RID: 34482
	[SerializeField]
	protected KButton requestSelectedEntityBtn;

	// Token: 0x040086B3 RID: 34483
	[SerializeField]
	private string requestStringDeposit;

	// Token: 0x040086B4 RID: 34484
	[SerializeField]
	private string requestStringCancelDeposit;

	// Token: 0x040086B5 RID: 34485
	[SerializeField]
	private string requestStringRemove;

	// Token: 0x040086B6 RID: 34486
	[SerializeField]
	private string requestStringCancelRemove;

	// Token: 0x040086B7 RID: 34487
	public GameObject activeEntityContainer;

	// Token: 0x040086B8 RID: 34488
	public GameObject nothingDiscoveredContainer;

	// Token: 0x040086B9 RID: 34489
	[SerializeField]
	protected LocText descriptionLabel;

	// Token: 0x040086BA RID: 34490
	protected Dictionary<SingleEntityReceptacle, int> entityPreviousSelectionMap = new Dictionary<SingleEntityReceptacle, int>();

	// Token: 0x040086BB RID: 34491
	[SerializeField]
	private string subtitleStringSelect;

	// Token: 0x040086BC RID: 34492
	[SerializeField]
	private string subtitleStringSelectDescription;

	// Token: 0x040086BD RID: 34493
	[SerializeField]
	private string subtitleStringAwaitingSelection;

	// Token: 0x040086BE RID: 34494
	[SerializeField]
	private string subtitleStringAwaitingDelivery;

	// Token: 0x040086BF RID: 34495
	[SerializeField]
	private string subtitleStringEntityDeposited;

	// Token: 0x040086C0 RID: 34496
	[SerializeField]
	private string subtitleStringAwaitingRemoval;

	// Token: 0x040086C1 RID: 34497
	[SerializeField]
	private LocText subtitleLabel;

	// Token: 0x040086C2 RID: 34498
	[SerializeField]
	private List<DescriptorPanel> descriptorPanels;

	// Token: 0x040086C3 RID: 34499
	public Material defaultMaterial;

	// Token: 0x040086C4 RID: 34500
	public Material desaturatedMaterial;

	// Token: 0x040086C5 RID: 34501
	[SerializeField]
	private GameObject requestObjectList;

	// Token: 0x040086C6 RID: 34502
	[SerializeField]
	private GameObject requestObjectListContainer;

	// Token: 0x040086C7 RID: 34503
	[SerializeField]
	private GameObject scrollBarContainer;

	// Token: 0x040086C8 RID: 34504
	[SerializeField]
	private GameObject entityToggle;

	// Token: 0x040086C9 RID: 34505
	[SerializeField]
	private Sprite buttonSelectedBG;

	// Token: 0x040086CA RID: 34506
	[SerializeField]
	private Sprite buttonNormalBG;

	// Token: 0x040086CB RID: 34507
	[SerializeField]
	private Sprite elementPlaceholderSpr;

	// Token: 0x040086CC RID: 34508
	[SerializeField]
	private bool hideUndiscoveredEntities;

	// Token: 0x040086CD RID: 34509
	protected ReceptacleToggle selectedEntityToggle;

	// Token: 0x040086CE RID: 34510
	protected SingleEntityReceptacle targetReceptacle;

	// Token: 0x040086CF RID: 34511
	protected Tag selectedDepositObjectTag;

	// Token: 0x040086D0 RID: 34512
	protected Tag selectedDepositObjectAdditionalTag;

	// Token: 0x040086D1 RID: 34513
	protected Dictionary<ReceptacleToggle, ReceptacleSideScreen.SelectableEntity> depositObjectMap;

	// Token: 0x040086D2 RID: 34514
	protected List<ReceptacleToggle> entityToggles = new List<ReceptacleToggle>();

	// Token: 0x040086D3 RID: 34515
	private int onObjectDestroyedHandle = -1;

	// Token: 0x040086D4 RID: 34516
	private int onOccupantValidChangedHandle = -1;

	// Token: 0x040086D5 RID: 34517
	private int onStorageChangedHandle = -1;

	// Token: 0x02001FAF RID: 8111
	protected class SelectableEntity
	{
		// Token: 0x040086D6 RID: 34518
		public Tag tag;

		// Token: 0x040086D7 RID: 34519
		public SingleEntityReceptacle.ReceptacleDirection direction;

		// Token: 0x040086D8 RID: 34520
		public GameObject asset;

		// Token: 0x040086D9 RID: 34521
		public float lastAmount = -1f;
	}
}
