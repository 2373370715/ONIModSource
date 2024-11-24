using System;
using System.Linq;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class MinionBrowserScreen : KMonoBehaviour
{
	private enum MultiToggleState
	{
		Default,
		Selected,
		NonInteractable
	}

	[Serializable]
	public class CyclerUI
	{
		public delegate void OnSelectedFn();

		[SerializeField]
		public KButton cyclePrevButton;

		[SerializeField]
		public KButton cycleNextButton;

		[SerializeField]
		public LocText currentLabel;

		[NonSerialized]
		private int selectedIndex = -1;

		[NonSerialized]
		private OnSelectedFn[] cycleOptions;

		public void Initialize(OnSelectedFn[] cycleOptions)
		{
			cyclePrevButton.onClick += CyclePrev;
			cycleNextButton.onClick += CycleNext;
			SetCycleOptions(cycleOptions);
		}

		public void SetCycleOptions(OnSelectedFn[] cycleOptions)
		{
			DebugUtil.Assert(cycleOptions != null);
			DebugUtil.Assert(cycleOptions.Length != 0);
			this.cycleOptions = cycleOptions;
			GoTo(0);
		}

		public void GoTo(int wrappingIndex)
		{
			if (cycleOptions != null && cycleOptions.Length != 0)
			{
				while (wrappingIndex < 0)
				{
					wrappingIndex += cycleOptions.Length;
				}
				while (wrappingIndex >= cycleOptions.Length)
				{
					wrappingIndex -= cycleOptions.Length;
				}
				selectedIndex = wrappingIndex;
				cycleOptions[selectedIndex]();
			}
		}

		public void CyclePrev()
		{
			GoTo(selectedIndex - 1);
		}

		public void CycleNext()
		{
			GoTo(selectedIndex + 1);
		}

		public void SetLabel(string text)
		{
			currentLabel.text = text;
		}
	}

	public abstract class GridItem : IEquatable<GridItem>
	{
		public class MinionInstanceTarget : GridItem
		{
			public GameObject minionInstance;

			public MinionIdentity minionIdentity;

			public Personality personality;

			public override Sprite GetIcon()
			{
				return personality.GetMiniIcon();
			}

			public override string GetName()
			{
				return minionIdentity.GetProperName();
			}

			public override string GetUniqueId()
			{
				return "minion_instance_id::" + minionInstance.GetInstanceID();
			}

			public override Personality GetPersonality()
			{
				return personality;
			}

			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.FromMinion(outfitType, minionInstance);
			}

			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromMinion(minionInstance);
			}
		}

		public class PersonalityTarget : GridItem
		{
			public Personality personality;

			public override Sprite GetIcon()
			{
				return personality.GetMiniIcon();
			}

			public override string GetName()
			{
				return personality.Name;
			}

			public override string GetUniqueId()
			{
				return "personality::" + personality.nameStringKey;
			}

			public override Personality GetPersonality()
			{
				return personality;
			}

			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.TryFromTemplateId(personality.GetSelectedTemplateOutfitId(outfitType));
			}

			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromPersonality(personality);
			}
		}

		public abstract string GetName();

		public abstract Sprite GetIcon();

		public abstract string GetUniqueId();

		public abstract Personality GetPersonality();

		public abstract Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType);

		public abstract JoyResponseOutfitTarget GetJoyResponseOutfitTarget();

		public override bool Equals(object obj)
		{
			if (obj is GridItem other)
			{
				return Equals(other);
			}
			return false;
		}

		public bool Equals(GridItem other)
		{
			return GetHashCode() == other.GetHashCode();
		}

		public override int GetHashCode()
		{
			return Hash.SDBMLower(GetUniqueId());
		}

		public override string ToString()
		{
			return GetUniqueId();
		}

		public static MinionInstanceTarget Of(GameObject minionInstance)
		{
			MinionIdentity component = minionInstance.GetComponent<MinionIdentity>();
			return new MinionInstanceTarget
			{
				minionInstance = minionInstance,
				minionIdentity = component,
				personality = Db.Get().Personalities.Get(component.personalityResourceId)
			};
		}

		public static PersonalityTarget Of(Personality personality)
		{
			return new PersonalityTarget
			{
				personality = personality
			};
		}
	}

	[Header("ItemGalleryColumn")]
	[SerializeField]
	private RectTransform galleryGridContent;

	[SerializeField]
	private GameObject gridItemPrefab;

	private GridLayouter gridLayouter;

	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	[SerializeField]
	private UIMinion UIMinion;

	[SerializeField]
	private LocText detailsHeaderText;

	[SerializeField]
	private Image detailHeaderIcon;

	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	[SerializeField]
	private CyclerUI cycler;

	[SerializeField]
	private KButton editButton;

	[SerializeField]
	private LocText editButtonText;

	[SerializeField]
	private KButton changeOutfitButton;

	private Option<ClothingOutfitUtility.OutfitType> selectedOutfitType;

	private Option<ClothingOutfitTarget> selectedOutfit;

	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	private GridItem selectedGridItem;

	private System.Action OnEditClickedFn;

	private bool isFirstDisplay = true;

	private bool postponeConfiguration = true;

	private UIPrefabLocalPool galleryGridItemPool;

	private System.Action RefreshGalleryFn;

	private System.Action RefreshOutfitDescriptionFn;

	private ClothingOutfitUtility.OutfitType currentOutfitType;

	public MinionBrowserScreenConfig Config { get; private set; }

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		gridLayouter = new GridLayouter
		{
			minCellSize = 112f,
			maxCellSize = 144f,
			targetGridLayouts = galleryGridContent.GetComponents<GridLayoutGroup>().ToList()
		};
		galleryGridItemPool = new UIPrefabLocalPool(gridItemPrefab, galleryGridContent.gameObject);
	}

	protected override void OnCmpEnable()
	{
		if (isFirstDisplay)
		{
			isFirstDisplay = false;
			PopulateGallery();
			RefreshPreview();
			cycler.Initialize(CreateCycleOptions());
			editButton.onClick += delegate
			{
				if (OnEditClickedFn != null)
				{
					OnEditClickedFn();
				}
			};
			changeOutfitButton.onClick += OnClickChangeOutfit;
		}
		else
		{
			RefreshGallery();
			RefreshPreview();
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			RefreshGallery();
			RefreshPreview();
		});
	}

	private void Update()
	{
		gridLayouter.CheckIfShouldResizeGrid();
	}

	protected override void OnSpawn()
	{
		postponeConfiguration = false;
		if (Config.isValid)
		{
			Configure(Config);
		}
		else
		{
			Configure(MinionBrowserScreenConfig.Personalities());
		}
	}

	public void Configure(MinionBrowserScreenConfig config)
	{
		Config = config;
		if (!postponeConfiguration)
		{
			PopulateGallery();
			RefreshPreview();
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
		GridItem[] items = Config.items;
		foreach (GridItem item2 in items)
		{
			AddGridIcon(item2);
		}
		RefreshGallery();
		SelectMinion(Config.defaultSelectedItem.Unwrap());
		void AddGridIcon(GridItem item)
		{
			GameObject gameObject = galleryGridItemPool.Borrow();
			gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = item.GetIcon();
			gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(item.GetName());
			string requiredDlcId = item.GetPersonality().requiredDlcId;
			ToolTip component = gameObject.GetComponent<ToolTip>();
			Image component2 = gameObject.transform.Find("DlcBanner").GetComponent<Image>();
			if (DlcManager.IsDlcId(requiredDlcId))
			{
				component2.gameObject.SetActive(value: true);
				component2.color = DlcManager.GetDlcBannerColor(requiredDlcId);
				component.SetSimpleTooltip(string.Format(UI.MINION_BROWSER_SCREEN.TOOLTIP_FROM_DLC, DlcManager.GetDlcTitle(requiredDlcId)));
			}
			else
			{
				component2.gameObject.SetActive(value: false);
				component.ClearMultiStringTooltip();
			}
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle multiToggle = toggle;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(OnMouseOverToggle));
			MultiToggle multiToggle2 = toggle;
			multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, (System.Action)delegate
			{
				SelectMinion(item);
			});
			RefreshGalleryFn = (System.Action)Delegate.Combine(RefreshGalleryFn, (System.Action)delegate
			{
				toggle.ChangeState((item == selectedGridItem) ? 1 : 0);
			});
		}
	}

	private void SelectMinion(GridItem item)
	{
		selectedGridItem = item;
		RefreshGallery();
		RefreshPreview();
		UIMinion.GetMinionVoice().PlaySoundUI("voice_land");
	}

	public void RefreshPreview()
	{
		UIMinion.SetMinion(selectedGridItem.GetPersonality());
		UIMinion.ReactToPersonalityChange();
		detailsHeaderText.SetText(selectedGridItem.GetName());
		detailHeaderIcon.sprite = selectedGridItem.GetIcon();
		RefreshOutfitDescription();
		RefreshPreviewButtonsInteractable();
		SetDioramaBG();
	}

	private void RefreshOutfitDescription()
	{
		if (RefreshOutfitDescriptionFn != null)
		{
			RefreshOutfitDescriptionFn();
		}
	}

	private void OnClickChangeOutfit()
	{
		if (!selectedOutfitType.IsNone())
		{
			OutfitBrowserScreenConfig.Minion(selectedOutfitType.Unwrap(), selectedGridItem).WithOutfit(selectedOutfit).ApplyAndOpenScreen();
		}
	}

	private void RefreshPreviewButtonsInteractable()
	{
		editButton.isInteractable = true;
		if (currentOutfitType == ClothingOutfitUtility.OutfitType.JoyResponse)
		{
			Option<string> joyResponseEditError = GetJoyResponseEditError();
			if (joyResponseEditError.IsSome())
			{
				editButton.isInteractable = false;
				editButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(joyResponseEditError.Unwrap());
			}
			else
			{
				editButton.isInteractable = true;
				editButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
			}
		}
	}

	private void SetDioramaBG()
	{
		dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(currentOutfitType);
	}

	private Option<string> GetJoyResponseEditError()
	{
		string joyTrait = selectedGridItem.GetPersonality().joyTrait;
		if (!(joyTrait == "BalloonArtist"))
		{
			return Option.Some(UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_NO_FACADES_FOR_JOY_TRAIT.Replace("{JoyResponseType}", Db.Get().traits.Get(joyTrait).Name));
		}
		return Option.None;
	}

	public void SetEditingOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		currentOutfitType = outfitType;
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_OUTFIT_ITEMS;
			changeOutfitButton.gameObject.SetActive(value: true);
			break;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS;
			changeOutfitButton.gameObject.SetActive(value: true);
			break;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_JOY_RESPONSE;
			changeOutfitButton.gameObject.SetActive(value: false);
			break;
		default:
			throw new NotImplementedException();
		}
		RefreshPreviewButtonsInteractable();
		OnEditClickedFn = delegate
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				OutfitDesignerScreenConfig.Minion(selectedOutfit.IsSome() ? selectedOutfit.Unwrap() : ClothingOutfitTarget.ForNewTemplateOutfit(outfitType), selectedGridItem).ApplyAndOpenScreen();
				break;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
				JoyResponseScreenConfig.From(selectedGridItem).WithInitialSelection(selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().AndThen((string id) => Db.Get().Permits.BalloonArtistFacades.Get(id))).ApplyAndOpenScreen();
				break;
			default:
				throw new NotImplementedException();
			}
		};
		RefreshOutfitDescriptionFn = delegate
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				selectedOutfit = selectedGridItem.GetClothingOutfitTarget(outfitType);
				UIMinion.SetOutfit(outfitType, selectedOutfit);
				outfitDescriptionPanel.Refresh(selectedOutfit, outfitType, selectedGridItem.GetPersonality());
				break;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
			{
				selectedOutfit = selectedGridItem.GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType.Clothing);
				UIMinion.SetOutfit(ClothingOutfitUtility.OutfitType.Clothing, selectedOutfit);
				string text = selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().UnwrapOr(null);
				outfitDescriptionPanel.Refresh((text != null) ? Db.Get().Permits.Get(text) : null, outfitType, selectedGridItem.GetPersonality());
				break;
			}
			default:
				throw new NotImplementedException();
			}
		};
		RefreshOutfitDescription();
	}

	private CyclerUI.OnSelectedFn[] CreateCycleOptions()
	{
		CyclerUI.OnSelectedFn[] array = new CyclerUI.OnSelectedFn[3];
		for (int i = 0; i < 3; i++)
		{
			ClothingOutfitUtility.OutfitType outfitType = (ClothingOutfitUtility.OutfitType)i;
			array[i] = delegate
			{
				selectedOutfitType = Option.Some(outfitType);
				cycler.SetLabel(outfitType.GetName());
				SetEditingOutfitType(outfitType);
				RefreshPreview();
			};
		}
		return array;
	}

	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover"));
	}
}
