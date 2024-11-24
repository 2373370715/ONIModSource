using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E74 RID: 7796
[Serializable]
public class OutfitBrowserScreen_CategoriesAndSearchBar
{
	// Token: 0x0600A379 RID: 41849 RVA: 0x003E182C File Offset: 0x003DFA2C
	public void InitializeWith(OutfitBrowserScreen outfitBrowserScreen)
	{
		this.outfitBrowserScreen = outfitBrowserScreen;
		this.clothingOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.selectOutfitType_Prefab.transform.parent.gameObject, true));
		this.clothingOutfitTypeButton.button.onClick += delegate()
		{
			this.SetOutfitType(ClothingOutfitUtility.OutfitType.Clothing);
		};
		this.clothingOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_equipment");
		KleiItemsUI.ConfigureTooltipOn(this.clothingOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_CLOTHING);
		this.atmosuitOutfitTypeButton = new OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton(outfitBrowserScreen, Util.KInstantiateUI(this.selectOutfitType_Prefab.gameObject, this.selectOutfitType_Prefab.transform.parent.gameObject, true));
		this.atmosuitOutfitTypeButton.button.onClick += delegate()
		{
			this.SetOutfitType(ClothingOutfitUtility.OutfitType.AtmoSuit);
		};
		this.atmosuitOutfitTypeButton.icon.sprite = Assets.GetSprite("icon_inventory_atmosuits");
		KleiItemsUI.ConfigureTooltipOn(this.atmosuitOutfitTypeButton.button.gameObject, UI.OUTFIT_BROWSER_SCREEN.TOOLTIP_FILTER_BY_ATMO_SUITS);
		this.searchTextField.onValueChanged.AddListener(delegate(string newFilter)
		{
			outfitBrowserScreen.state.Filter = newFilter;
		});
		this.searchTextField.transform.SetAsLastSibling();
		outfitBrowserScreen.state.OnCurrentOutfitTypeChanged += delegate()
		{
			if (outfitBrowserScreen.Config.onlyShowOutfitType.IsSome())
			{
				this.clothingOutfitTypeButton.root.gameObject.SetActive(false);
				this.atmosuitOutfitTypeButton.root.gameObject.SetActive(false);
				return;
			}
			this.clothingOutfitTypeButton.root.gameObject.SetActive(true);
			this.atmosuitOutfitTypeButton.root.gameObject.SetActive(true);
			this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
			this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected);
			ClothingOutfitUtility.OutfitType currentOutfitType = outfitBrowserScreen.state.CurrentOutfitType;
			if (currentOutfitType == ClothingOutfitUtility.OutfitType.Clothing)
			{
				this.clothingOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
				return;
			}
			if (currentOutfitType != ClothingOutfitUtility.OutfitType.AtmoSuit)
			{
				throw new NotImplementedException();
			}
			this.atmosuitOutfitTypeButton.SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected);
		};
	}

	// Token: 0x0600A37A RID: 41850 RVA: 0x0010A07B File Offset: 0x0010827B
	public void SetOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		this.outfitBrowserScreen.state.CurrentOutfitType = outfitType;
	}

	// Token: 0x04007FA5 RID: 32677
	[NonSerialized]
	public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton clothingOutfitTypeButton;

	// Token: 0x04007FA6 RID: 32678
	[NonSerialized]
	public OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButton atmosuitOutfitTypeButton;

	// Token: 0x04007FA7 RID: 32679
	[NonSerialized]
	public OutfitBrowserScreen outfitBrowserScreen;

	// Token: 0x04007FA8 RID: 32680
	public KButton selectOutfitType_Prefab;

	// Token: 0x04007FA9 RID: 32681
	public KInputTextField searchTextField;

	// Token: 0x02001E75 RID: 7797
	public enum SelectOutfitTypeButtonState
	{
		// Token: 0x04007FAB RID: 32683
		Disabled,
		// Token: 0x04007FAC RID: 32684
		Unselected,
		// Token: 0x04007FAD RID: 32685
		Selected
	}

	// Token: 0x02001E76 RID: 7798
	public readonly struct SelectOutfitTypeButton
	{
		// Token: 0x0600A37C RID: 41852 RVA: 0x003E19C4 File Offset: 0x003DFBC4
		public SelectOutfitTypeButton(OutfitBrowserScreen outfitBrowserScreen, GameObject rootGameObject)
		{
			this.outfitBrowserScreen = outfitBrowserScreen;
			this.root = rootGameObject.GetComponent<RectTransform>();
			this.button = rootGameObject.GetComponent<KButton>();
			this.buttonImage = rootGameObject.GetComponent<KImage>();
			this.icon = this.root.GetChild(0).GetComponent<Image>();
			global::Debug.Assert(this.root != null);
			global::Debug.Assert(this.button != null);
			global::Debug.Assert(this.buttonImage != null);
			global::Debug.Assert(this.icon != null);
		}

		// Token: 0x0600A37D RID: 41853 RVA: 0x003E1A58 File Offset: 0x003DFC58
		public void SetState(OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState state)
		{
			switch (state)
			{
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Disabled:
				this.button.isInteractable = false;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Unselected:
				this.button.isInteractable = true;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.notSelectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			case OutfitBrowserScreen_CategoriesAndSearchBar.SelectOutfitTypeButtonState.Selected:
				this.button.isInteractable = true;
				this.buttonImage.colorStyleSetting = this.outfitBrowserScreen.selectedCategoryStyle;
				this.buttonImage.ApplyColorStyleSetting();
				return;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x04007FAE RID: 32686
		public readonly OutfitBrowserScreen outfitBrowserScreen;

		// Token: 0x04007FAF RID: 32687
		public readonly RectTransform root;

		// Token: 0x04007FB0 RID: 32688
		public readonly KButton button;

		// Token: 0x04007FB1 RID: 32689
		public readonly KImage buttonImage;

		// Token: 0x04007FB2 RID: 32690
		public readonly Image icon;
	}
}
