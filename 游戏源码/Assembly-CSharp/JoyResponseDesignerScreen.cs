using System;
using System.Linq;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class JoyResponseDesignerScreen : KMonoBehaviour
{
	public class JoyResponseCategory
	{
		public string displayName;

		public Sprite icon;

		public GalleryItem[] items;
	}

	private enum MultiToggleState
	{
		Default,
		Selected
	}

	public abstract class GalleryItem : IEquatable<GalleryItem>
	{
		public class BalloonArtistFacadeTarget : GalleryItem
		{
			public Option<BalloonArtistFacadeResource> permit;

			public override Sprite GetIcon()
			{
				return permit.AndThen((BalloonArtistFacadeResource p) => p.GetPermitPresentationInfo().sprite).UnwrapOrElse(() => KleiItemsUI.GetNoneBalloonArtistIcon());
			}

			public override string GetName()
			{
				return permit.AndThen((BalloonArtistFacadeResource p) => p.Name).UnwrapOrElse(() => KleiItemsUI.GetNoneClothingItemStrings(PermitCategory.JoyResponse).name);
			}

			public override string GetUniqueId()
			{
				return "balloon_artist_facade::" + permit.AndThen((BalloonArtistFacadeResource p) => p.Id).UnwrapOr("<none>");
			}

			public override Option<PermitResource> GetPermitResource()
			{
				return permit.AndThen((Func<BalloonArtistFacadeResource, PermitResource>)((BalloonArtistFacadeResource p) => p));
			}

			public override bool IsUnlocked()
			{
				return GetPermitResource().AndThen((PermitResource p) => p.IsUnlocked()).UnwrapOr(fallback_value: true);
			}
		}

		public abstract string GetName();

		public abstract Sprite GetIcon();

		public abstract string GetUniqueId();

		public abstract bool IsUnlocked();

		public abstract Option<PermitResource> GetPermitResource();

		public override bool Equals(object obj)
		{
			if (obj is GalleryItem other)
			{
				return Equals(other);
			}
			return false;
		}

		public bool Equals(GalleryItem other)
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

		public static BalloonArtistFacadeTarget Of(Option<BalloonArtistFacadeResource> permit)
		{
			return new BalloonArtistFacadeTarget
			{
				permit = permit
			};
		}
	}

	[Header("CategoryColumn")]
	[SerializeField]
	private RectTransform categoryListContent;

	[SerializeField]
	private GameObject categoryRowPrefab;

	[Header("GalleryColumn")]
	[SerializeField]
	private LocText galleryHeaderLabel;

	[SerializeField]
	private RectTransform galleryGridContent;

	[SerializeField]
	private GameObject galleryItemPrefab;

	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private LocText selectionHeaderLabel;

	[SerializeField]
	private KleiPermitDioramaVis_JoyResponseBalloon dioramaVis;

	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	[SerializeField]
	private KButton primaryButton;

	public JoyResponseCategory[] joyResponseCategories;

	private bool postponeConfiguration = true;

	private Option<JoyResponseCategory> selectedCategoryOpt;

	private UIPrefabLocalPool categoryRowPool;

	private System.Action RefreshCategoriesFn;

	private GalleryItem selectedGalleryItem;

	private UIPrefabLocalPool galleryGridItemPool;

	private GridLayouter galleryGridLayouter;

	private System.Action RefreshGalleryFn;

	public System.Action RefreshPreviewFn;

	private Func<bool> preventScreenPopFn;

	public JoyResponseScreenConfig Config { get; private set; }

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Debug.Assert(categoryRowPrefab.transform.parent == categoryListContent.transform);
		Debug.Assert(galleryItemPrefab.transform.parent == galleryGridContent.transform);
		categoryRowPrefab.SetActive(value: false);
		galleryItemPrefab.SetActive(value: false);
		galleryGridLayouter = new GridLayouter
		{
			minCellSize = 64f,
			maxCellSize = 96f,
			targetGridLayouts = galleryGridContent.GetComponents<GridLayoutGroup>().ToList()
		};
		categoryRowPool = new UIPrefabLocalPool(categoryRowPrefab, categoryListContent.gameObject);
		galleryGridItemPool = new UIPrefabLocalPool(galleryItemPrefab, galleryGridContent.gameObject);
		JoyResponseCategory[] array = new JoyResponseCategory[1];
		JoyResponseCategory obj = new JoyResponseCategory
		{
			displayName = UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST,
			icon = Assets.GetSprite("icon_inventory_balloonartist")
		};
		GalleryItem[] items = Db.Get().Permits.BalloonArtistFacades.resources.Select((BalloonArtistFacadeResource r) => GalleryItem.Of(r)).Prepend(GalleryItem.Of(Option.None)).ToArray();
		obj.items = items;
		array[0] = obj;
		joyResponseCategories = array;
		dioramaVis.ConfigureSetup();
	}

	private void Update()
	{
		galleryGridLayouter.CheckIfShouldResizeGrid();
	}

	protected override void OnSpawn()
	{
		postponeConfiguration = false;
		if (Config.isValid)
		{
			Configure(Config);
			return;
		}
		throw new InvalidOperationException("Cannot open up JoyResponseDesignerScreen without a target personality or minion instance");
	}

	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			Configure(Config);
		});
	}

	public void Configure(JoyResponseScreenConfig config)
	{
		Config = config;
		if (postponeConfiguration)
		{
			return;
		}
		RegisterPreventScreenPop();
		primaryButton.ClearOnClick();
		primaryButton.GetComponentInChildren<LocText>().SetText(UI.JOY_RESPONSE_DESIGNER_SCREEN.BUTTON_APPLY_TO_MINION.Replace("{MinionName}", Config.target.GetMinionName()));
		primaryButton.onClick += delegate
		{
			Option<PermitResource> permitResource = selectedGalleryItem.GetPermitResource();
			if (permitResource.IsSome())
			{
				Debug.Log("Save selected balloon " + selectedGalleryItem.GetName() + " for " + Config.target.GetMinionName());
				if (CanSaveSelection())
				{
					Config.target.WriteFacadeId(permitResource.Unwrap().Id);
				}
			}
			else
			{
				Debug.Log("Save selected balloon " + selectedGalleryItem.GetName() + " for " + Config.target.GetMinionName());
				Config.target.WriteFacadeId(Option.None);
			}
			LockerNavigator.Instance.PopScreen();
		};
		PopulateCategories();
		PopulateGallery();
		PopulatePreview();
		if (Config.initalSelectedItem.IsSome())
		{
			SelectGalleryItem(Config.initalSelectedItem.Unwrap());
		}
	}

	private bool CanSaveSelection()
	{
		return GetSaveSelectionError().IsNone();
	}

	private Option<string> GetSaveSelectionError()
	{
		if (!selectedGalleryItem.IsUnlocked())
		{
			return Option.Some(UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_PICK_JOY_RESPONSE_ERROR_LOCKED.Replace("{MinionName}", Config.target.GetMinionName()));
		}
		return Option.None;
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
		JoyResponseCategory[] array = joyResponseCategories;
		foreach (JoyResponseCategory category in array)
		{
			GameObject gameObject = categoryRowPool.Borrow();
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(category.displayName);
			component.GetReference<Image>("Icon").sprite = category.icon;
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle multiToggle = toggle;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(OnMouseOverToggle));
			toggle.onClick = delegate
			{
				SelectCategory(category);
			};
			RefreshCategoriesFn = (System.Action)Delegate.Combine(RefreshCategoriesFn, (System.Action)delegate
			{
				toggle.ChangeState((category == selectedCategoryOpt) ? 1 : 0);
			});
			SetCatogoryClickUISound(category, toggle);
		}
		SelectCategory(joyResponseCategories[0]);
	}

	public void SelectCategory(JoyResponseCategory category)
	{
		selectedCategoryOpt = category;
		galleryHeaderLabel.text = category.displayName;
		RefreshCategories();
		PopulateGallery();
		RefreshPreview();
	}

	private void SetCatogoryClickUISound(JoyResponseCategory category, MultiToggle toggle)
	{
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
		if (!selectedCategoryOpt.IsNone())
		{
			JoyResponseCategory joyResponseCategory = selectedCategoryOpt.Unwrap();
			GalleryItem[] items = joyResponseCategory.items;
			foreach (GalleryItem item2 in items)
			{
				AddGridIcon(item2);
			}
			SelectGalleryItem(joyResponseCategory.items[0]);
		}
		void AddGridIcon(GalleryItem item)
		{
			GameObject gameObject = galleryGridItemPool.Borrow();
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<Image>("Icon").sprite = item.GetIcon();
			component.GetReference<Image>("IsUnownedOverlay").gameObject.SetActive(!item.IsUnlocked());
			Option<PermitResource> permitResource = item.GetPermitResource();
			if (permitResource.IsSome())
			{
				KleiItemsUI.ConfigureTooltipOn(gameObject, KleiItemsUI.GetTooltipStringFor(permitResource.Unwrap()));
			}
			else
			{
				KleiItemsUI.ConfigureTooltipOn(gameObject, KleiItemsUI.GetNoneTooltipStringFor(PermitCategory.JoyResponse));
			}
			MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
			MultiToggle multiToggle = toggle;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(OnMouseOverToggle));
			MultiToggle multiToggle2 = toggle;
			multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, (System.Action)delegate
			{
				SelectGalleryItem(item);
			});
			RefreshGalleryFn = (System.Action)Delegate.Combine(RefreshGalleryFn, (System.Action)delegate
			{
				toggle.ChangeState((item == selectedGalleryItem) ? 1 : 0);
			});
		}
	}

	public void SelectGalleryItem(GalleryItem item)
	{
		selectedGalleryItem = item;
		RefreshGallery();
		RefreshPreview();
	}

	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover"));
	}

	public void RefreshPreview()
	{
		if (RefreshPreviewFn != null)
		{
			RefreshPreviewFn();
		}
	}

	public void PopulatePreview()
	{
		RefreshPreviewFn = (System.Action)Delegate.Combine(RefreshPreviewFn, (System.Action)delegate
		{
			if (selectedGalleryItem is GalleryItem.BalloonArtistFacadeTarget balloonArtistFacadeTarget)
			{
				Option<PermitResource> permitResource = balloonArtistFacadeTarget.GetPermitResource();
				selectionHeaderLabel.SetText(balloonArtistFacadeTarget.GetName());
				dioramaVis.SetMinion(Config.target.GetPersonality());
				dioramaVis.ConfigureWith(balloonArtistFacadeTarget.permit);
				outfitDescriptionPanel.Refresh(permitResource.UnwrapOr(null), ClothingOutfitUtility.OutfitType.JoyResponse, Config.target.GetPersonality());
				Option<string> saveSelectionError = GetSaveSelectionError();
				if (saveSelectionError.IsSome())
				{
					primaryButton.isInteractable = false;
					primaryButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(saveSelectionError.Unwrap());
				}
				else
				{
					primaryButton.isInteractable = true;
					primaryButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
				}
				return;
			}
			throw new NotImplementedException();
		});
		RefreshPreview();
	}

	private void RegisterPreventScreenPop()
	{
		UnregisterPreventScreenPop();
		preventScreenPopFn = delegate
		{
			if (Config.target.ReadFacadeId() != selectedGalleryItem.GetPermitResource().AndThen((PermitResource r) => r.Id))
			{
				RegisterPreventScreenPop();
				MakeSaveWarningPopup(Config.target, delegate
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

	public static void MakeSaveWarningPopup(JoyResponseOutfitTarget target, System.Action discardChangesFn)
	{
		LockerNavigator.Instance.ShowDialogPopup(delegate(InfoDialogScreen dialog)
		{
			dialog.SetHeader(UI.JOY_RESPONSE_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.HEADER.Replace("{MinionName}", target.GetMinionName())).AddPlainText(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BODY).AddOption(UI.OUTFIT_DESIGNER_SCREEN.CHANGES_NOT_SAVED_WARNING_POPUP.BUTTON_DISCARD, delegate(InfoDialogScreen d)
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
}
