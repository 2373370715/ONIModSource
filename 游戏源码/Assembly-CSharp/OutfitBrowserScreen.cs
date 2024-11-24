using System;
using System.Collections.Generic;
using System.Linq;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutfitBrowserScreen : KMonoBehaviour
{
	public class State
	{
		private Option<ClothingOutfitTarget> m_selectedOutfitOpt;

		private ClothingOutfitUtility.OutfitType m_currentOutfitType;

		private string m_filter;

		public Option<ClothingOutfitTarget> SelectedOutfitOpt
		{
			get
			{
				return m_selectedOutfitOpt;
			}
			set
			{
				m_selectedOutfitOpt = value;
				if (this.OnSelectedOutfitOptChanged != null)
				{
					this.OnSelectedOutfitOptChanged();
				}
			}
		}

		public ClothingOutfitUtility.OutfitType CurrentOutfitType
		{
			get
			{
				return m_currentOutfitType;
			}
			set
			{
				m_currentOutfitType = value;
				if (this.OnCurrentOutfitTypeChanged != null)
				{
					this.OnCurrentOutfitTypeChanged();
				}
			}
		}

		public string Filter
		{
			get
			{
				return m_filter;
			}
			set
			{
				m_filter = value;
				if (this.OnFilterChanged != null)
				{
					this.OnFilterChanged();
				}
			}
		}

		public event System.Action OnSelectedOutfitOptChanged;

		public event System.Action OnCurrentOutfitTypeChanged;

		public event System.Action OnFilterChanged;
	}

	private enum MultiToggleState
	{
		Default,
		Selected,
		NonInteractable
	}

	[Header("ItemGalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	[SerializeField]
	private OutfitBrowserScreen_CategoriesAndSearchBar categoriesAndSearchBar;

	[SerializeField]
	private RectTransform galleryGridContent;

	[SerializeField]
	private GameObject gridItemPrefab;

	[SerializeField]
	private GameObject addButtonGridItem;

	private UIPrefabLocalPool galleryGridItemPool;

	private GridLayouter gridLayouter;

	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	[SerializeField]
	private UIMinionOrMannequin dioramaMinionOrMannequin;

	[SerializeField]
	private Image dioramaBG;

	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	[SerializeField]
	private KButton pickOutfitButton;

	[SerializeField]
	private KButton editOutfitButton;

	[SerializeField]
	private KButton renameOutfitButton;

	[SerializeField]
	private KButton deleteOutfitButton;

	[Header("Misc")]
	[SerializeField]
	private KInputTextField inputFieldPrefab;

	[SerializeField]
	public ColorStyleSetting selectedCategoryStyle;

	[SerializeField]
	public ColorStyleSetting notSelectedCategoryStyle;

	public State state = new State();

	public Option<ClothingOutfitUtility.OutfitType> lastShownOutfitType = Option.None;

	private Dictionary<string, MultiToggle> outfits = new Dictionary<string, MultiToggle>();

	private bool postponeConfiguration = true;

	private bool isFirstDisplay = true;

	private System.Action RefreshGalleryFn;

	public OutfitBrowserScreenConfig Config { get; private set; }

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		galleryGridItemPool = new UIPrefabLocalPool(gridItemPrefab, galleryGridContent.gameObject);
		gridLayouter = new GridLayouter
		{
			minCellSize = 112f,
			maxCellSize = 144f,
			targetGridLayouts = galleryGridContent.GetComponents<GridLayoutGroup>().ToList()
		};
		categoriesAndSearchBar.InitializeWith(this);
		pickOutfitButton.onClick += OnClickPickOutfit;
		editOutfitButton.onClick += delegate
		{
			if (!state.SelectedOutfitOpt.IsNone())
			{
				new OutfitDesignerScreenConfig(state.SelectedOutfitOpt.Unwrap(), Config.minionPersonality, Config.targetMinionInstance, OnOutfitDesignerWritesToOutfitTarget).ApplyAndOpenScreen();
			}
		};
		renameOutfitButton.onClick += delegate
		{
			ClothingOutfitTarget selectedOutfit2 = state.SelectedOutfitOpt.Unwrap();
			MakeRenamePopup(inputFieldPrefab, selectedOutfit2, () => selectedOutfit2.ReadName(), delegate(string new_name)
			{
				selectedOutfit2.WriteName(new_name);
				Configure(Config.WithOutfit(selectedOutfit2));
			});
		};
		deleteOutfitButton.onClick += delegate
		{
			ClothingOutfitTarget selectedOutfit = state.SelectedOutfitOpt.Unwrap();
			MakeDeletePopup(selectedOutfit, delegate
			{
				selectedOutfit.Delete();
				Configure(Config.WithOutfit(Option.None));
			});
		};
	}

	protected override void OnCmpEnable()
	{
		if (isFirstDisplay)
		{
			isFirstDisplay = false;
			dioramaMinionOrMannequin.TrySpawn();
			FirstTimeSetup();
			postponeConfiguration = false;
			Configure(Config);
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			RefreshGallery();
			outfitDescriptionPanel.Refresh(state.SelectedOutfitOpt, ClothingOutfitUtility.OutfitType.Clothing, Config.minionPersonality);
		});
	}

	private void FirstTimeSetup()
	{
		state.OnCurrentOutfitTypeChanged += delegate
		{
			PopulateGallery();
			Option<ClothingOutfitTarget> selectedOutfitOpt = ((!Config.minionPersonality.HasValue && !Config.selectedTarget.HasValue) ? ClothingOutfitTarget.GetRandom(state.CurrentOutfitType) : Config.selectedTarget);
			if (selectedOutfitOpt.IsSome() && selectedOutfitOpt.Unwrap().DoesExist())
			{
				state.SelectedOutfitOpt = selectedOutfitOpt;
			}
			else
			{
				state.SelectedOutfitOpt = Option.None;
			}
		};
		state.OnSelectedOutfitOptChanged += delegate
		{
			if (state.SelectedOutfitOpt.IsSome())
			{
				selectionHeaderLabel.text = state.SelectedOutfitOpt.Unwrap().ReadName();
			}
			else
			{
				selectionHeaderLabel.text = UI.OUTFIT_NAME.NONE;
			}
			dioramaMinionOrMannequin.current.SetOutfit(state.CurrentOutfitType, state.SelectedOutfitOpt);
			dioramaMinionOrMannequin.current.ReactToFullOutfitChange();
			outfitDescriptionPanel.Refresh(state.SelectedOutfitOpt, state.CurrentOutfitType, Config.minionPersonality);
			dioramaBG.sprite = KleiPermitDioramaVis.GetDioramaBackground(state.CurrentOutfitType);
			pickOutfitButton.gameObject.SetActive(Config.isPickingOutfitForDupe);
			if (Config.minionPersonality.IsSome())
			{
				pickOutfitButton.isInteractable = !state.SelectedOutfitOpt.IsSome() || !state.SelectedOutfitOpt.Unwrap().DoesContainLockedItems();
				KleiItemsUI.ConfigureTooltipOn(pickOutfitButton.gameObject, pickOutfitButton.isInteractable ? ((Option<string>)Option.None) : Option.Some(UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_PICK_OUTFIT_ERROR_LOCKED.Replace("{MinionName}", Config.GetMinionName())));
			}
			editOutfitButton.isInteractable = state.SelectedOutfitOpt.IsSome();
			renameOutfitButton.isInteractable = state.SelectedOutfitOpt.IsSome() && state.SelectedOutfitOpt.Unwrap().CanWriteName;
			KleiItemsUI.ConfigureTooltipOn(renameOutfitButton.gameObject, renameOutfitButton.isInteractable ? UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT : UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_RENAME_OUTFIT_ERROR_READONLY);
			deleteOutfitButton.isInteractable = state.SelectedOutfitOpt.IsSome() && state.SelectedOutfitOpt.Unwrap().CanDelete;
			KleiItemsUI.ConfigureTooltipOn(deleteOutfitButton.gameObject, deleteOutfitButton.isInteractable ? UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT : UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_DELETE_OUTFIT_ERROR_READONLY);
			state.OnSelectedOutfitOptChanged += RefreshGallery;
			state.OnFilterChanged += RefreshGallery;
			state.OnCurrentOutfitTypeChanged += RefreshGallery;
			RefreshGallery();
		};
	}

	public void Configure(OutfitBrowserScreenConfig config)
	{
		Config = config;
		if (!postponeConfiguration)
		{
			dioramaMinionOrMannequin.SetFrom(config.minionPersonality);
			if (config.targetMinionInstance.HasValue)
			{
				galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.targetMinionInstance.Value.GetProperName());
			}
			else if (config.minionPersonality.HasValue)
			{
				galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.MINION_GALLERY_HEADER.Replace("{MinionName}", config.minionPersonality.Value.Name);
			}
			else
			{
				galleryHeaderLabel.text = UI.OUTFIT_BROWSER_SCREEN.COLUMN_HEADERS.GALLERY_HEADER;
			}
			state.CurrentOutfitType = config.onlyShowOutfitType.UnwrapOr(lastShownOutfitType.UnwrapOr(ClothingOutfitUtility.OutfitType.Clothing));
			if (base.gameObject.activeInHierarchy)
			{
				base.gameObject.SetActive(value: false);
				base.gameObject.SetActive(value: true);
			}
		}
	}

	private void RefreshGallery()
	{
		if (RefreshGalleryFn != null)
		{
			RefreshGalleryFn();
		}
	}

	private void PopulateGallery()
	{
		outfits.Clear();
		galleryGridItemPool.ReturnAll();
		RefreshGalleryFn = null;
		if (Config.isPickingOutfitForDupe)
		{
			AddGridIconForTarget(Option.None);
		}
		if (Config.targetMinionInstance.HasValue)
		{
			AddGridIconForTarget(ClothingOutfitTarget.FromMinion(state.CurrentOutfitType, Config.targetMinionInstance.Value));
		}
		foreach (ClothingOutfitTarget item in from outfit in ClothingOutfitTarget.GetAllTemplates()
			where outfit.OutfitType == state.CurrentOutfitType
			select outfit)
		{
			AddGridIconForTarget(item);
		}
		addButtonGridItem.transform.SetAsLastSibling();
		addButtonGridItem.SetActive(value: true);
		addButtonGridItem.GetComponent<MultiToggle>().onClick = delegate
		{
			new OutfitDesignerScreenConfig(ClothingOutfitTarget.ForNewTemplateOutfit(state.CurrentOutfitType), Config.minionPersonality, Config.targetMinionInstance, OnOutfitDesignerWritesToOutfitTarget).ApplyAndOpenScreen();
		};
		RefreshGallery();
		void AddGridIconForTarget(Option<ClothingOutfitTarget> target)
		{
			GameObject spawn = galleryGridItemPool.Borrow();
			GameObject obj = spawn.transform.GetChild(1).gameObject;
			GameObject isUnownedOverlayGO = spawn.transform.GetChild(2).gameObject;
			GameObject dlcBannerGO = spawn.transform.GetChild(3).gameObject;
			obj.SetActive(value: true);
			bool shouldShowOutfitWithDefaultItems = target.IsNone() || state.CurrentOutfitType == ClothingOutfitUtility.OutfitType.AtmoSuit;
			UIMannequin componentInChildren = obj.GetComponentInChildren<UIMannequin>();
			dioramaMinionOrMannequin.mannequin.shouldShowOutfitWithDefaultItems = shouldShowOutfitWithDefaultItems;
			componentInChildren.shouldShowOutfitWithDefaultItems = shouldShowOutfitWithDefaultItems;
			componentInChildren.personalityToUseForDefaultClothing = Config.minionPersonality;
			componentInChildren.SetOutfit(state.CurrentOutfitType, target);
			RectTransform component = obj.GetComponent<RectTransform>();
			float x;
			float num;
			float num2;
			float y;
			switch (state.CurrentOutfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
				x = 8f;
				num = 8f;
				num2 = 8f;
				y = 8f;
				break;
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				x = 24f;
				num = 16f;
				num2 = 32f;
				y = 8f;
				break;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
				throw new NotSupportedException();
			default:
				throw new NotImplementedException();
			}
			component.offsetMin = new Vector2(x, y);
			component.offsetMax = new Vector2(0f - num, 0f - num2);
			MultiToggle button = spawn.GetComponent<MultiToggle>();
			MultiToggle multiToggle = button;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(OnMouseOverToggle));
			button.onClick = delegate
			{
				state.SelectedOutfitOpt = target;
			};
			RefreshGalleryFn = (System.Action)Delegate.Combine(RefreshGalleryFn, (System.Action)delegate
			{
				button.ChangeState((target == state.SelectedOutfitOpt) ? 1 : 0);
				if (string.IsNullOrWhiteSpace(state.Filter) || target.IsNone())
				{
					spawn.SetActive(value: true);
				}
				else
				{
					spawn.SetActive(target.Unwrap().ReadName().ToLower()
						.Contains(state.Filter.ToLower()));
				}
				if (!target.HasValue)
				{
					KleiItemsUI.ConfigureTooltipOn(spawn, KleiItemsUI.WrapAsToolTipTitle(KleiItemsUI.GetNoneOutfitName(state.CurrentOutfitType)));
					isUnownedOverlayGO.SetActive(value: false);
				}
				else
				{
					KleiItemsUI.ConfigureTooltipOn(spawn, KleiItemsUI.WrapAsToolTipTitle(target.Value.ReadName()));
					isUnownedOverlayGO.SetActive(target.Value.DoesContainLockedItems());
				}
				if (target.IsSome() && target.Unwrap().impl is ClothingOutfitTarget.DatabaseAuthoredTemplate databaseAuthoredTemplate)
				{
					string dlcIdFrom = databaseAuthoredTemplate.resource.GetDlcIdFrom();
					if (DlcManager.IsDlcId(dlcIdFrom))
					{
						dlcBannerGO.GetComponent<Image>().color = DlcManager.GetDlcBannerColor(dlcIdFrom);
						dlcBannerGO.SetActive(value: true);
					}
					else
					{
						dlcBannerGO.SetActive(value: false);
					}
				}
				else
				{
					dlcBannerGO.SetActive(value: false);
				}
			});
			SetButtonClickUISound(target, button);
		}
	}

	private void OnOutfitDesignerWritesToOutfitTarget(ClothingOutfitTarget outfit)
	{
		Configure(Config.WithOutfit(outfit));
	}

	private void Update()
	{
		gridLayouter.CheckIfShouldResizeGrid();
	}

	private void OnClickPickOutfit()
	{
		if (Config.targetMinionInstance.IsSome())
		{
			Config.targetMinionInstance.Unwrap().GetComponent<WearableAccessorizer>().ApplyClothingItems(state.CurrentOutfitType, state.SelectedOutfitOpt.AndThen((ClothingOutfitTarget outfit) => outfit.ReadItemValues()).UnwrapOr(ClothingOutfitTarget.NO_ITEM_VALUES));
		}
		else if (Config.minionPersonality.IsSome())
		{
			Config.minionPersonality.Value.SetSelectedTemplateOutfitId(state.CurrentOutfitType, state.SelectedOutfitOpt.AndThen((ClothingOutfitTarget o) => o.OutfitId));
		}
		LockerNavigator.Instance.PopScreen();
	}

	public static void MakeDeletePopup(ClothingOutfitTarget sourceTarget, System.Action deleteFn)
	{
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.HEADER.Replace("{OutfitName}", sourceTarget.ReadName())).AddPlainText(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BODY.Replace("{OutfitName}", sourceTarget.ReadName())).AddOption(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_YES_DELETE, delegate(InfoDialogScreen d)
			{
				deleteFn();
				d.Deactivate();
			}, rightSide: true)
				.AddOption(UI.OUTFIT_BROWSER_SCREEN.DELETE_WARNING_POPUP.BUTTON_DONT_DELETE, delegate(InfoDialogScreen d)
				{
					d.Deactivate();
				});
		});
	}

	public static void MakeRenamePopup(KInputTextField inputFieldPrefab, ClothingOutfitTarget sourceTarget, Func<string> readName, Action<string> writeName)
	{
		KInputTextField inputField;
		InfoScreenPlainText errorText;
		KButton okButton;
		LocText okButtonText;
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.OUTFIT_BROWSER_SCREEN.RENAME_POPUP.HEADER).AddUI(inputFieldPrefab, out inputField).AddSpacer(8f)
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
				writeName(inputField.text);
				dialog.Deactivate();
			};
			Refresh(readName());
		});
		void Refresh(string candidateName)
		{
			ClothingOutfitNameProposal clothingOutfitNameProposal = ClothingOutfitNameProposal.FromExistingOutfit(candidateName, sourceTarget, isSameNameAllowed: true);
			inputField.text = candidateName;
			switch (clothingOutfitNameProposal.result)
			{
			case ClothingOutfitNameProposal.Result.NewOutfit:
			case ClothingOutfitNameProposal.Result.SameOutfit:
				errorText.gameObject.SetActive(value: false);
				okButton.isInteractable = true;
				break;
			case ClothingOutfitNameProposal.Result.Error_NoInputName:
				errorText.gameObject.SetActive(value: false);
				okButton.isInteractable = false;
				break;
			case ClothingOutfitNameProposal.Result.Error_NameAlreadyExists:
			case ClothingOutfitNameProposal.Result.Error_SameOutfitReadonly:
				errorText.gameObject.SetActive(value: true);
				errorText.SetText(UI.OUTFIT_NAME.ERROR_NAME_EXISTS.Replace("{OutfitName}", candidateName));
				okButton.isInteractable = false;
				break;
			default:
				DebugUtil.DevAssert(test: false, $"Unhandled name proposal case: {clothingOutfitNameProposal.result}");
				break;
			}
		}
	}

	private void SetButtonClickUISound(Option<ClothingOutfitTarget> target, MultiToggle toggle)
	{
		if (!target.HasValue)
		{
			toggle.states[1].on_click_override_sound_path = "HUD_Click";
			toggle.states[0].on_click_override_sound_path = "HUD_Click";
			return;
		}
		bool flag = !target.Value.DoesContainLockedItems();
		toggle.states[1].on_click_override_sound_path = "ClothingItem_Click";
		toggle.states[1].sound_parameter_name = "Unlocked";
		toggle.states[1].sound_parameter_value = (flag ? 1f : 0f);
		toggle.states[1].has_sound_parameter = true;
		toggle.states[0].on_click_override_sound_path = "ClothingItem_Click";
		toggle.states[0].sound_parameter_name = "Unlocked";
		toggle.states[0].sound_parameter_value = (flag ? 1f : 0f);
		toggle.states[0].has_sound_parameter = true;
	}

	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover"));
	}
}
