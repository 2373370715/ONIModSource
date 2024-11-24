using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutfitDesignerScreen : KMonoBehaviour
{
	private enum MultiToggleState
	{
		Default,
		Selected,
		NonInteractable
	}

	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	[SerializeField]
	private GameObject categoryRowPrefab;

	private UIPrefabLocalPool categoryRowPool;

	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	[SerializeField]
	private RectTransform galleryGridContent;

	[SerializeField]
	private GameObject subcategoryUiPrefab;

	[SerializeField]
	private GameObject gridItemPrefab;

	private UIPrefabLocalPool subcategoryUiPool;

	private UIPrefabLocalPool galleryGridItemPool;

	private GridLayouter galleryGridLayouter;

	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	[SerializeField]
	private UIMinionOrMannequin minionOrMannequin;

	[SerializeField]
	private Image dioramaBG;

	[SerializeField]
	private KButton primaryButton;

	[SerializeField]
	private KButton secondaryButton;

	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	[SerializeField]
	private KInputTextField inputFieldPrefab;

	public static Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]> outfitTypeToCategoriesDict;

	private bool postponeConfiguration = true;

	private System.Action updateSaveButtonsFn;

	private System.Action RefreshCategoriesFn;

	private System.Action RefreshGalleryFn;

	private Func<bool> preventScreenPopFn;

	public OutfitDesignerScreenConfig Config { get; private set; }

	public PermitResource SelectedPermit { get; private set; }

	public PermitCategory SelectedCategory { get; private set; }

	public OutfitDesignerScreen_OutfitState outfitState { get; private set; }

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Debug.Assert(categoryRowPrefab.transform.parent == categoryListContent.transform);
		Debug.Assert(gridItemPrefab.transform.parent == galleryGridContent.transform);
		Debug.Assert(subcategoryUiPrefab.transform.parent == galleryGridContent.transform);
		categoryRowPrefab.SetActive(value: false);
		gridItemPrefab.SetActive(value: false);
		galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = galleryGridContent.GetComponents<GridLayoutGroup>().ToList()
		};
		galleryGridLayouter.overrideParentForSizeReference = galleryGridContent;
		categoryRowPool = new UIPrefabLocalPool(categoryRowPrefab, categoryListContent.gameObject);
		galleryGridItemPool = new UIPrefabLocalPool(gridItemPrefab, galleryGridContent.gameObject);
		subcategoryUiPool = new UIPrefabLocalPool(subcategoryUiPrefab, galleryGridContent.gameObject);
		if (outfitTypeToCategoriesDict == null)
		{
			outfitTypeToCategoriesDict = new Dictionary<ClothingOutfitUtility.OutfitType, PermitCategory[]>
			{
				[ClothingOutfitUtility.OutfitType.Clothing] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_CLOTHING,
				[ClothingOutfitUtility.OutfitType.AtmoSuit] = ClothingOutfitUtility.PERMIT_CATEGORIES_FOR_ATMO_SUITS
			};
		}
		InventoryOrganization.Initialize();
	}

	private void Update()
	{
		galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	protected override void OnSpawn()
	{
		postponeConfiguration = false;
		minionOrMannequin.TrySpawn();
		if (!Config.isValid)
		{
			throw new NotSupportedException("Cannot open OutfitDesignerScreen without a config. Make sure to call Configure() before enabling the screen");
		}
		Configure(Config);
	}

	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			RefreshCategories();
			RefreshGallery();
			RefreshOutfitState();
		});
	}

	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		UnregisterPreventScreenPop();
	}

	private void UpdateSaveButtons()
	{
		if (updateSaveButtonsFn != null)
		{
			updateSaveButtonsFn();
		}
	}

	public void Configure(OutfitDesignerScreenConfig config)
	{
		Config = config;
		if (config.targetMinionInstance.HasValue)
		{
			outfitState = OutfitDesignerScreen_OutfitState.ForMinionInstance(Config.sourceTarget, config.targetMinionInstance.Value);
		}
		else
		{
			outfitState = OutfitDesignerScreen_OutfitState.ForTemplateOutfit(Config.sourceTarget);
		}
		if (postponeConfiguration)
		{
			return;
		}
		RegisterPreventScreenPop();
		minionOrMannequin.SetFrom(config.minionPersonality).SpawnedAvatar.GetComponent<WearableAccessorizer>();
		using (ListPool<ClothingItemResource, OutfitDesignerScreen>.PooledList clothingItems = PoolsFor<OutfitDesignerScreen>.AllocateList<ClothingItemResource>())
		{
			outfitState.AddItemValuesTo(clothingItems);
			minionOrMannequin.SetFrom(config.minionPersonality).SetOutfit(config.sourceTarget.OutfitType, clothingItems);
		}
		PopulateCategories();
		SelectCategory(outfitTypeToCategoriesDict[outfitState.outfitType][0]);
		galleryGridLayouter.RequestGridResize();
		RefreshOutfitState();
		if (Config.targetMinionInstance.HasValue)
		{
			updateSaveButtonsFn = null;
			primaryButton.ClearOnClick();
			primaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_MINION.Replace("{MinionName}", Config.targetMinionInstance.Value.GetProperName()));
			primaryButton.onClick += delegate
			{
				ClothingOutfitTarget clothingOutfitTarget = ClothingOutfitTarget.FromMinion(Config.sourceTarget.OutfitType, Config.targetMinionInstance.Value);
				clothingOutfitTarget.WriteItems(Config.sourceTarget.OutfitType, outfitState.GetItems());
				if (Config.onWriteToOutfitTargetFn != null)
				{
					Config.onWriteToOutfitTargetFn(clothingOutfitTarget);
				}
				LockerNavigator.Instance.PopScreen();
			};
			secondaryButton.ClearOnClick();
			secondaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.BUTTON_APPLY_TO_TEMPLATE);
			secondaryButton.onClick += delegate
			{
				MakeApplyToTemplatePopup(inputFieldPrefab, outfitState, Config.targetMinionInstance.Value, Config.outfitTemplate, Config.onWriteToOutfitTargetFn);
			};
			updateSaveButtonsFn = (System.Action)Delegate.Combine(updateSaveButtonsFn, (System.Action)delegate
			{
				if (outfitState.DoesContainLockedItems())
				{
					primaryButton.isInteractable = false;
					primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
					secondaryButton.isInteractable = false;
					secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
				}
				else
				{
					primaryButton.isInteractable = true;
					primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					secondaryButton.isInteractable = true;
					secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
				}
			});
		}
		else
		{
			if (!Config.outfitTemplate.HasValue)
			{
				throw new NotSupportedException();
			}
			updateSaveButtonsFn = null;
			primaryButton.ClearOnClick();
			primaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_SAVE);
			primaryButton.onClick += delegate
			{
				outfitState.destinationTarget.WriteName(outfitState.name);
				outfitState.destinationTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
				if (Config.minionPersonality.HasValue)
				{
					Config.minionPersonality.Value.SetSelectedTemplateOutfitId(outfitState.destinationTarget.OutfitType, outfitState.destinationTarget.OutfitId);
				}
				if (Config.onWriteToOutfitTargetFn != null)
				{
					Config.onWriteToOutfitTargetFn(outfitState.destinationTarget);
				}
				LockerNavigator.Instance.PopScreen();
			};
			secondaryButton.ClearOnClick();
			secondaryButton.GetComponentInChildren<LocText>().SetText(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.BUTTON_COPY);
			secondaryButton.onClick += delegate
			{
				MakeCopyPopup(this, inputFieldPrefab, outfitState, Config.outfitTemplate.Value, Config.minionPersonality, Config.onWriteToOutfitTargetFn);
			};
			updateSaveButtonsFn = (System.Action)Delegate.Combine(updateSaveButtonsFn, (System.Action)delegate
			{
				if (!outfitState.destinationTarget.CanWriteItems)
				{
					primaryButton.isInteractable = false;
					primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_READONLY);
					if (outfitState.DoesContainLockedItems())
					{
						secondaryButton.isInteractable = false;
						secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
					}
					else
					{
						secondaryButton.isInteractable = true;
						secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					}
				}
				else if (outfitState.DoesContainLockedItems())
				{
					primaryButton.isInteractable = false;
					primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
					secondaryButton.isInteractable = false;
					secondaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(UI.OUTFIT_DESIGNER_SCREEN.OUTFIT_TEMPLATE.TOOLTIP_SAVE_ERROR_LOCKED);
				}
				else
				{
					primaryButton.isInteractable = true;
					primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
					secondaryButton.isInteractable = true;
					secondaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
				}
			});
		}
		UpdateSaveButtons();
	}

	private void RefreshOutfitState()
	{
		selectionHeaderLabel.text = outfitState.name;
		outfitDescriptionPanel.Refresh(outfitState, Config.minionPersonality);
		UpdateSaveButtons();
	}

	private void RefreshCategories()
	{
		if (RefreshCategoriesFn != null)
		{
			RefreshCategoriesFn();
		}
	}

	public void PopulateCategories()
	{
		RefreshCategoriesFn = null;
		categoryRowPool.ReturnAll();
		PermitCategory[] array = outfitTypeToCategoriesDict[outfitState.outfitType];
		foreach (PermitCategory permitCategory in array)
		{
			GameObject gameObject = categoryRowPool.Borrow();
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(PermitCategories.GetUppercaseDisplayName(permitCategory));
			component.GetReference<Image>("Icon").sprite = Assets.GetSprite(PermitCategories.GetIconName(permitCategory));
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle multiToggle = toggle;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(OnMouseOverToggle));
			toggle.onClick = delegate
			{
				SelectCategory(permitCategory);
			};
			RefreshCategoriesFn = (System.Action)Delegate.Combine(RefreshCategoriesFn, (System.Action)delegate
			{
				toggle.ChangeState((permitCategory == SelectedCategory) ? 1 : 0);
			});
			SetCatogoryClickUISound(permitCategory, toggle);
		}
	}

	public void SelectCategory(PermitCategory permitCategory)
	{
		SelectedCategory = permitCategory;
		galleryHeaderLabel.text = PermitCategories.GetDisplayName(permitCategory);
		RefreshCategories();
		PopulateGallery();
		Option<ClothingItemResource> itemForCategory = outfitState.GetItemForCategory(permitCategory);
		if (itemForCategory.HasValue)
		{
			SelectPermit(itemForCategory.Value);
		}
		else
		{
			SelectPermit(null);
		}
	}

	private void RefreshGallery()
	{
		if (RefreshGalleryFn != null)
		{
			RefreshGalleryFn();
		}
	}

	public void PopulateGallery()
	{
		RefreshGalleryFn = null;
		galleryGridItemPool.ReturnAll();
		subcategoryUiPool.ReturnAll();
		galleryGridLayouter.targetGridLayouts.Clear();
		galleryGridLayouter.OnSizeGridComplete = null;
		Promise<KleiInventoryUISubcategory> onFirstDisplayCategoryDecided = new Promise<KleiInventoryUISubcategory>();
		AddGridIconForPermit(null);
		foreach (ClothingItemResource resource in Db.Get().Permits.ClothingItems.resources)
		{
			if (resource.Category == SelectedCategory && resource.outfitType == Config.sourceTarget.OutfitType && !resource.Id.StartsWith("visonly_"))
			{
				AddGridIconForPermit(resource);
			}
		}
		foreach (GameObject item in subcategoryUiPool.GetBorrowedObjects().StableSort(Comparer<GameObject>.Create(delegate(GameObject a, GameObject b)
		{
			KleiInventoryUISubcategory component4 = a.GetComponent<KleiInventoryUISubcategory>();
			KleiInventoryUISubcategory component5 = b.GetComponent<KleiInventoryUISubcategory>();
			int sortKey = InventoryOrganization.subcategoryIdToPresentationDataMap[component4.subcategoryID].sortKey;
			int sortKey2 = InventoryOrganization.subcategoryIdToPresentationDataMap[component5.subcategoryID].sortKey;
			return sortKey.CompareTo(sortKey2);
		})))
		{
			item.transform.SetAsLastSibling();
		}
		GameObject gameObject2 = subcategoryUiPool.GetBorrowedObjects().FirstOrDefault((GameObject gameObject) => gameObject.GetComponent<KleiInventoryUISubcategory>().IsOpen);
		if (gameObject2 != null)
		{
			onFirstDisplayCategoryDecided.Resolve(gameObject2.GetComponent<KleiInventoryUISubcategory>());
		}
		galleryGridLayouter.RequestGridResize();
		RefreshGallery();
		void AddGridIconForPermit(PermitResource permit)
		{
			GameObject gridItemGameObject = galleryGridItemPool.Borrow();
			HierarchyReferences component3 = gridItemGameObject.GetComponent<HierarchyReferences>();
			Image reference = component3.GetReference<Image>("Icon");
			MultiToggle toggle = gridItemGameObject.GetComponent<MultiToggle>();
			Image isUnownedOverlay = component3.GetReference<Image>("IsUnownedOverlay");
			Image reference2 = component3.GetReference<Image>("DlcBanner");
			if (permit == null)
			{
				onFirstDisplayCategoryDecided.Then(delegate(KleiInventoryUISubcategory subcategoryUi)
				{
					gridItemGameObject.transform.SetParent(subcategoryUi.gridLayout.transform);
					gridItemGameObject.transform.SetAsFirstSibling();
				});
				reference.sprite = KleiItemsUI.GetNoneClothingItemIcon(SelectedCategory, Config.minionPersonality);
				KleiItemsUI.ConfigureTooltipOn(gridItemGameObject, KleiItemsUI.GetNoneTooltipStringFor(SelectedCategory));
				isUnownedOverlay.gameObject.SetActive(value: false);
			}
			else
			{
				gridItemGameObject.transform.SetParent(GetOrSpawnSubcategoryUiForPermit(InventoryOrganization.GetPermitSubcategory(permit)).gridLayout.transform);
				reference.sprite = permit.GetPermitPresentationInfo().sprite;
				KleiItemsUI.ConfigureTooltipOn(gridItemGameObject, KleiItemsUI.GetTooltipStringFor(permit));
				RefreshGalleryFn = (System.Action)Delegate.Combine(RefreshGalleryFn, (System.Action)delegate
				{
					isUnownedOverlay.gameObject.SetActive(!permit.IsUnlocked());
				});
			}
			string dlcId = ((permit == null) ? null : permit.GetDlcIdFrom());
			if (DlcManager.IsDlcId(dlcId))
			{
				reference2.gameObject.SetActive(value: true);
				reference2.color = DlcManager.GetDlcBannerColor(dlcId);
			}
			else
			{
				reference2.gameObject.SetActive(value: false);
			}
			MultiToggle multiToggle = toggle;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(OnMouseOverToggle));
			toggle.onClick = delegate
			{
				SelectPermit(permit);
			};
			RefreshGalleryFn = (System.Action)Delegate.Combine(RefreshGalleryFn, (System.Action)delegate
			{
				toggle.ChangeState((permit == SelectedPermit) ? 1 : 0);
			});
			SetItemClickUISound(permit, toggle);
		}
		KleiInventoryUISubcategory GetOrSpawn()
		{
			foreach (GameObject borrowedObject in subcategoryUiPool.GetBorrowedObjects())
			{
				KleiInventoryUISubcategory component = borrowedObject.GetComponent<KleiInventoryUISubcategory>();
				if (P_0.subcategoryId == component.subcategoryID)
				{
					return component;
				}
			}
			KleiInventoryUISubcategory component2 = subcategoryUiPool.Borrow().GetComponent<KleiInventoryUISubcategory>();
			galleryGridLayouter.targetGridLayouts.Add(component2.gridLayout);
			GridLayouter gridLayouter = galleryGridLayouter;
			gridLayouter.OnSizeGridComplete = (System.Action)Delegate.Combine(gridLayouter.OnSizeGridComplete, new System.Action(component2.RefreshDisplay));
			return component2;
		}
		KleiInventoryUISubcategory GetOrSpawnSubcategoryUiForPermit(string subcategoryId)
		{
			bool flag = ((subcategoryId == null || !(subcategoryId == "UNCATEGORIZED")) ? true : false);
			bool open = flag;
			KleiInventoryUISubcategory orSpawn = GetOrSpawn();
			orSpawn.subcategoryID = subcategoryId;
			orSpawn.SetIdentity(InventoryOrganization.GetSubcategoryName(subcategoryId), InventoryOrganization.subcategoryIdToPresentationDataMap[subcategoryId].icon);
			orSpawn.ToggleOpen(open);
			return orSpawn;
		}
	}

	public void SelectPermit(PermitResource permit)
	{
		SelectedPermit = permit;
		RefreshGallery();
		UpdateSelectedItemDetails();
		UpdateSaveButtons();
	}

	public void UpdateSelectedItemDetails()
	{
		Option<ClothingItemResource> item = Option.None;
		if (SelectedPermit != null && SelectedPermit is ClothingItemResource clothingItemResource)
		{
			item = clothingItemResource;
		}
		outfitState.SetItemForCategory(SelectedCategory, item);
		minionOrMannequin.current.SetOutfit(outfitState);
		minionOrMannequin.current.ReactToClothingItemChange(SelectedCategory);
		outfitDescriptionPanel.Refresh(outfitState, Config.minionPersonality);
		dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(SelectedCategory);
	}

	private void RegisterPreventScreenPop()
	{
		UnregisterPreventScreenPop();
		preventScreenPopFn = delegate
		{
			if (outfitState.IsDirty())
			{
				RegisterPreventScreenPop();
				MakeSaveWarningPopup(outfitState, delegate
				{
					UnregisterPreventScreenPop();
					LockerNavigator.Instance.PopScreen();
				});
				return true;
			}
			return false;
		};
		LockerNavigator.Instance.preventScreenPop.Add(preventScreenPopFn);
	}

	private void UnregisterPreventScreenPop()
	{
		if (preventScreenPopFn != null)
		{
			LockerNavigator.Instance.preventScreenPop.Remove(preventScreenPopFn);
			preventScreenPopFn = null;
		}
	}

	public static void MakeSaveWarningPopup(OutfitDesignerScreen_OutfitState outfitState, System.Action discardChangesFn)
	{
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddPlainText(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY).AddOption(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_DISCARD, delegate(InfoDialogScreen d)
			{
				d.Deactivate();
				discardChangesFn();
			}, rightSide: true)
				.AddOption(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_RETURN, delegate(InfoDialogScreen d)
				{
					d.Deactivate();
				});
		});
	}

	public static void MakeApplyToTemplatePopup(KInputTextField inputFieldPrefab, OutfitDesignerScreen_OutfitState outfitState, GameObject targetMinionInstance, Option<ClothingOutfitTarget> existingOutfitTemplate, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		ClothingOutfitNameProposal proposal = default(ClothingOutfitNameProposal);
		Color errorTextColor = Util.ColorFromHex("F44A47");
		Color defaultTextColor = Util.ColorFromHex("FFFFFF");
		KInputTextField inputField;
		InfoScreenPlainText descLabel;
		KButton saveButton;
		LocText saveButtonText;
		LocText descLocText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.HEADER.Replace("{OutfitName}", outfitState.name)).AddUI(inputFieldPrefab, out inputField).AddSpacer(8f)
				.AddUI(dialog.GetPlainTextPrefab(), out descLabel)
				.AddOption(rightSide: true, out saveButton, out saveButtonText)
				.AddDefaultCancel();
			descLocText = descLabel.gameObject.GetComponent<LocText>();
			descLocText.allowOverride = true;
			descLocText.alignment = TextAlignmentOptions.BottomLeft;
			descLocText.color = errorTextColor;
			descLocText.fontSize = 14f;
			descLabel.SetText("");
			inputField.onValueChanged.AddListener(Refresh);
			saveButton.onClick += delegate
			{
				ClothingOutfitTarget clothingOutfitTarget = ClothingOutfitTarget.FromMinion(outfitState.outfitType, targetMinionInstance);
				ClothingOutfitTarget clothingOutfitTarget2 = proposal.result switch
				{
					ClothingOutfitNameProposal.Result.NewOutfit => ClothingOutfitTarget.ForNewTemplateOutfit(outfitState.outfitType, proposal.candidateName), 
					ClothingOutfitNameProposal.Result.SameOutfit => existingOutfitTemplate.Value, 
					_ => throw new NotSupportedException($"Can't save outfit with name \"{proposal.candidateName}\", failed with result: {proposal.result}"), 
				};
				clothingOutfitTarget2.WriteItems(outfitState.outfitType, outfitState.GetItems());
				clothingOutfitTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
				if (onWriteToOutfitTargetFn != null)
				{
					onWriteToOutfitTargetFn(clothingOutfitTarget2);
				}
				dialog.Deactivate();
				LockerNavigator.Instance.PopScreen();
			};
			if (existingOutfitTemplate.HasValue)
			{
				if (existingOutfitTemplate.Value.CanWriteName && existingOutfitTemplate.Value.CanWriteItems)
				{
					Refresh(existingOutfitTemplate.Value.OutfitId);
				}
				else
				{
					Refresh(ClothingOutfitTarget.ForTemplateCopyOf(existingOutfitTemplate.Value).OutfitId);
				}
			}
			else
			{
				Refresh(outfitState.name);
			}
		});
		void Refresh(string candidateName)
		{
			if (existingOutfitTemplate.IsSome())
			{
				proposal = ClothingOutfitNameProposal.FromExistingOutfit(candidateName, existingOutfitTemplate.Unwrap(), isSameNameAllowed: true);
			}
			else
			{
				proposal = ClothingOutfitNameProposal.ForNewOutfit(candidateName);
			}
			inputField.text = candidateName;
			switch (proposal.result)
			{
			case ClothingOutfitNameProposal.Result.NewOutfit:
				descLabel.gameObject.SetActive(value: true);
				descLabel.SetText(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.DESC_SAVE_NEW.Replace("{OutfitName}", candidateName).Replace("{MinionName}", targetMinionInstance.GetProperName()));
				descLocText.color = defaultTextColor;
				saveButtonText.text = UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.BUTTON_SAVE_NEW;
				saveButton.isInteractable = true;
				break;
			case ClothingOutfitNameProposal.Result.SameOutfit:
				descLabel.gameObject.SetActive(value: true);
				descLabel.SetText(UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.DESC_SAVE_EXISTING.Replace("{OutfitName}", candidateName).Replace("{MinionName}", targetMinionInstance.GetProperName()));
				descLocText.color = defaultTextColor;
				saveButtonText.text = UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.BUTTON_SAVE_EXISTING;
				saveButton.isInteractable = true;
				break;
			case ClothingOutfitNameProposal.Result.Error_NoInputName:
				descLabel.gameObject.SetActive(value: false);
				saveButtonText.text = UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.BUTTON_SAVE_NEW;
				saveButton.isInteractable = false;
				break;
			case ClothingOutfitNameProposal.Result.Error_NameAlreadyExists:
			case ClothingOutfitNameProposal.Result.Error_SameOutfitReadonly:
				descLabel.gameObject.SetActive(value: true);
				descLabel.SetText(UI.OUTFIT_NAME.ERROR_NAME_EXISTS.Replace("{OutfitName}", candidateName));
				descLocText.color = errorTextColor;
				saveButtonText.text = UI.OUTFIT_DESIGNER_SCREEN.MINION_INSTANCE.APPLY_TEMPLATE_POPUP.BUTTON_SAVE_NEW;
				saveButton.isInteractable = false;
				break;
			default:
				DebugUtil.DevAssert(test: false, $"Unhandled name proposal case: {proposal.result}");
				break;
			}
		}
	}

	public static void MakeCopyPopup(OutfitDesignerScreen screen, KInputTextField inputFieldPrefab, OutfitDesignerScreen_OutfitState outfitState, ClothingOutfitTarget outfitTemplate, Option<Personality> minionPersonality, Action<ClothingOutfitTarget> onWriteToOutfitTargetFn)
	{
		ClothingOutfitNameProposal proposal = default(ClothingOutfitNameProposal);
		KInputTextField inputField;
		InfoScreenPlainText errorText;
		KButton okButton;
		LocText okButtonText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_DESIGNER_SCREEN.COPY_POPUP.HEADER).AddUI(inputFieldPrefab, out inputField).AddSpacer(8f)
				.AddUI(dialog.GetPlainTextPrefab(), out errorText)
				.AddOption(rightSide: true, out okButton, out okButtonText)
				.AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
				{
					d.Deactivate();
				});
			inputField.onValueChanged.AddListener(Refresh);
			errorText.gameObject.SetActive(value: false);
			LocText component = errorText.gameObject.GetComponent<LocText>();
			component.allowOverride = true;
			component.alignment = TextAlignmentOptions.BottomLeft;
			component.color = Util.ColorFromHex("F44A47");
			component.fontSize = 14f;
			errorText.SetText("");
			okButtonText.text = UI.CONFIRMDIALOG.OK;
			okButton.onClick += delegate
			{
				if (proposal.result != ClothingOutfitNameProposal.Result.NewOutfit)
				{
					throw new NotSupportedException($"Can't save outfit with name \"{proposal.candidateName}\", failed with result: {proposal.result}");
				}
				ClothingOutfitTarget sourceTarget = ClothingOutfitTarget.ForNewTemplateOutfit(outfitTemplate.OutfitType, proposal.candidateName);
				sourceTarget.WriteItems(outfitState.outfitType, outfitState.GetItems());
				if (minionPersonality.HasValue)
				{
					minionPersonality.Value.SetSelectedTemplateOutfitId(sourceTarget.OutfitType, sourceTarget.OutfitId);
				}
				if (onWriteToOutfitTargetFn != null)
				{
					onWriteToOutfitTargetFn(sourceTarget);
				}
				dialog.Deactivate();
				screen.Configure(screen.Config.WithOutfit(sourceTarget));
			};
			Refresh(ClothingOutfitTarget.ForTemplateCopyOf(outfitTemplate).OutfitId);
		});
		void Refresh(string candidateName)
		{
			proposal = ClothingOutfitNameProposal.FromExistingOutfit(candidateName, outfitTemplate, isSameNameAllowed: false);
			inputField.text = candidateName;
			switch (proposal.result)
			{
			case ClothingOutfitNameProposal.Result.NewOutfit:
				errorText.gameObject.SetActive(value: false);
				okButton.isInteractable = true;
				break;
			case ClothingOutfitNameProposal.Result.Error_NoInputName:
				errorText.gameObject.SetActive(value: false);
				okButton.isInteractable = false;
				break;
			case ClothingOutfitNameProposal.Result.SameOutfit:
			case ClothingOutfitNameProposal.Result.Error_NameAlreadyExists:
			case ClothingOutfitNameProposal.Result.Error_SameOutfitReadonly:
				errorText.gameObject.SetActive(value: true);
				errorText.SetText(UI.OUTFIT_NAME.ERROR_NAME_EXISTS.Replace("{OutfitName}", candidateName));
				okButton.isInteractable = false;
				break;
			default:
				DebugUtil.DevAssert(test: false, $"Unhandled name proposal case: {proposal.result}");
				break;
			}
		}
	}

	private void SetCatogoryClickUISound(PermitCategory category, MultiToggle toggle)
	{
		toggle.states[1].on_click_override_sound_path = category.ToString() + "_Click";
		toggle.states[0].on_click_override_sound_path = category.ToString() + "_Click";
	}

	private void SetItemClickUISound(PermitResource permit, MultiToggle toggle)
	{
		if (permit == null)
		{
			toggle.states[1].on_click_override_sound_path = "HUD_Click";
			toggle.states[0].on_click_override_sound_path = "HUD_Click";
			return;
		}
		string clothingItemSoundName = GetClothingItemSoundName(permit);
		toggle.states[1].on_click_override_sound_path = clothingItemSoundName + "_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = clothingItemSoundName + "_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (permit.IsUnlocked() ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	public static string GetClothingItemSoundName(PermitResource permit)
	{
		if (permit == null)
		{
			return "HUD";
		}
		return permit.Category switch
		{
			PermitCategory.DupeTops => "tops", 
			PermitCategory.DupeBottoms => "bottoms", 
			PermitCategory.DupeGloves => "gloves", 
			PermitCategory.DupeShoes => "shoes", 
			PermitCategory.DupeHats => "hats", 
			_ => "HUD", 
		};
	}

	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover"));
	}
}
