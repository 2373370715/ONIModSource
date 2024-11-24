using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class OutfitBrowserScreen_CategoriesAndSearchBar
{
	public enum SelectOutfitTypeButtonState
	{
		Disabled,
		Unselected,
		Selected
	}

	public readonly struct SelectOutfitTypeButton
	{
		public readonly OutfitBrowserScreen outfitBrowserScreen;

		public readonly RectTransform root;

		public readonly KButton button;

		public readonly KImage buttonImage;

		public readonly Image icon;

		public SelectOutfitTypeButton(OutfitBrowserScreen outfitBrowserScreen, GameObject rootGameObject)
		{
			this.outfitBrowserScreen = outfitBrowserScreen;
			root = rootGameObject.GetComponent<RectTransform>();
			button = rootGameObject.GetComponent<KButton>();
			buttonImage = rootGameObject.GetComponent<KImage>();
			icon = root.GetChild(0).GetComponent<Image>();
			Debug.Assert(root != null);
			Debug.Assert(button != null);
			Debug.Assert(buttonImage != null);
			Debug.Assert(icon != null);
		}

		public void SetState(SelectOutfitTypeButtonState state)
		{
			switch (state)
			{
			case SelectOutfitTypeButtonState.Disabled:
				button.isInteractable = false;
				buttonImage.colorStyleSetting = outfitBrowserScreen.notSelectedCategoryStyle;
				buttonImage.ApplyColorStyleSetting();
				break;
			case SelectOutfitTypeButtonState.Unselected:
				button.isInteractable = true;
				buttonImage.colorStyleSetting = outfitBrowserScreen.notSelectedCategoryStyle;
				buttonImage.ApplyColorStyleSetting();
				break;
			case SelectOutfitTypeButtonState.Selected:
				button.isInteractable = true;
				buttonImage.colorStyleSetting = outfitBrowserScreen.selectedCategoryStyle;
				buttonImage.ApplyColorStyleSetting();
				break;
			default:
				throw new NotImplementedException();
			}
		}
	}

	[NonSerialized]
	public SelectOutfitTypeButton clothingOutfitTypeButton;

	[NonSerialized]
	public SelectOutfitTypeButton atmosuitOutfitTypeButton;

	[NonSerialized]
	public OutfitBrowserScreen outfitBrowserScreen;

	public KButton selectOutfitType_Prefab;

	public KInputTextField searchTextField;

	public void InitializeWith(OutfitBrowserScreen outfitBrowserScreen)
	{
		this.outfitBrowserScreen = outfitBrowserScreen;
		clothingOutfitTypeButton = new SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(selectOutfitType_Prefab.gameObject, selectOutfitType_Prefab.transform.parent.gameObject, force_active: true));
		clothingOutfitTypeButton.button.onClick += delegate
		{
			SetOutfitType(ClothingOutfitUtility.OutfitType.Clothing);
		};
		clothingOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_equipment");
		KleiItemsUI.ConfigureTooltipOn(clothingOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_CLOTHING);
		atmosuitOutfitTypeButton = new SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(selectOutfitType_Prefab.gameObject, selectOutfitType_Prefab.transform.parent.gameObject, force_active: true));
		atmosuitOutfitTypeButton.button.onClick += delegate
		{
			SetOutfitType(ClothingOutfitUtility.OutfitType.AtmoSuit);
		};
		atmosuitOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_atmosuits");
		KleiItemsUI.ConfigureTooltipOn(atmosuitOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_ATMO_SUITS);
		searchTextField.onValueChanged.AddListener(delegate(string newFilter)
		{
			outfitBrowserScreen.state.Filter = newFilter;
		});
		searchTextField.transform.SetAsLastSibling();
		outfitBrowserScreen.state.OnCurrentOutfitTypeChanged += delegate
		{
			if (outfitBrowserScreen.Config.onlyShowOutfitType.IsSome())
			{
				clothingOutfitTypeButton.root.gameObject.SetActive(value: false);
				atmosuitOutfitTypeButton.root.gameObject.SetActive(value: false);
			}
			else
			{
				clothingOutfitTypeButton.root.gameObject.SetActive(value: true);
				atmosuitOutfitTypeButton.root.gameObject.SetActive(value: true);
				clothingOutfitTypeButton.SetState(SelectOutfitTypeButtonState.Unselected);
				atmosuitOutfitTypeButton.SetState(SelectOutfitTypeButtonState.Unselected);
				switch (outfitBrowserScreen.state.CurrentOutfitType)
				{
				case ClothingOutfitUtility.OutfitType.Clothing:
					clothingOutfitTypeButton.SetState(SelectOutfitTypeButtonState.Selected);
					break;
				case ClothingOutfitUtility.OutfitType.AtmoSuit:
					atmosuitOutfitTypeButton.SetState(SelectOutfitTypeButtonState.Selected);
					break;
				default:
					throw new NotImplementedException();
				}
			}
		};
	}

	public void SetOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		outfitBrowserScreen.state.CurrentOutfitType = outfitType;
	}
}
