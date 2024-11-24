using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001E1E RID: 7710
public class MinionBrowserScreen : KMonoBehaviour
{
	// Token: 0x0600A15B RID: 41307 RVA: 0x003D7748 File Offset: 0x003D5948
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.gridLayouter = new GridLayouter
		{
			minCellSize = 112f,
			maxCellSize = 144f,
			targetGridLayouts = this.galleryGridContent.GetComponents<GridLayoutGroup>().ToList<GridLayoutGroup>()
		};
		this.galleryGridItemPool = new UIPrefabLocalPool(this.gridItemPrefab, this.galleryGridContent.gameObject);
	}

	// Token: 0x0600A15C RID: 41308 RVA: 0x003D77B0 File Offset: 0x003D59B0
	protected override void OnCmpEnable()
	{
		if (this.isFirstDisplay)
		{
			this.isFirstDisplay = false;
			this.PopulateGallery();
			this.RefreshPreview();
			this.cycler.Initialize(this.CreateCycleOptions());
			this.editButton.onClick += delegate()
			{
				if (this.OnEditClickedFn != null)
				{
					this.OnEditClickedFn();
				}
			};
			this.changeOutfitButton.onClick += this.OnClickChangeOutfit;
		}
		else
		{
			this.RefreshGallery();
			this.RefreshPreview();
		}
		KleiItemsStatusRefresher.AddOrGetListener(this).OnRefreshUI(delegate
		{
			this.RefreshGallery();
			this.RefreshPreview();
		});
	}

	// Token: 0x0600A15D RID: 41309 RVA: 0x00108DBB File Offset: 0x00106FBB
	private void Update()
	{
		this.gridLayouter.CheckIfShouldResizeGrid();
	}

	// Token: 0x0600A15E RID: 41310 RVA: 0x003D783C File Offset: 0x003D5A3C
	protected override void OnSpawn()
	{
		this.postponeConfiguration = false;
		if (this.Config.isValid)
		{
			this.Configure(this.Config);
			return;
		}
		this.Configure(MinionBrowserScreenConfig.Personalities(default(Option<Personality>)));
	}

	// Token: 0x17000A71 RID: 2673
	// (get) Token: 0x0600A15F RID: 41311 RVA: 0x00108DC8 File Offset: 0x00106FC8
	// (set) Token: 0x0600A160 RID: 41312 RVA: 0x00108DD0 File Offset: 0x00106FD0
	public MinionBrowserScreenConfig Config { get; private set; }

	// Token: 0x0600A161 RID: 41313 RVA: 0x00108DD9 File Offset: 0x00106FD9
	public void Configure(MinionBrowserScreenConfig config)
	{
		this.Config = config;
		if (this.postponeConfiguration)
		{
			return;
		}
		this.PopulateGallery();
		this.RefreshPreview();
	}

	// Token: 0x0600A162 RID: 41314 RVA: 0x00108DF7 File Offset: 0x00106FF7
	private void RefreshGallery()
	{
		if (this.RefreshGalleryFn != null)
		{
			this.RefreshGalleryFn();
		}
	}

	// Token: 0x0600A163 RID: 41315 RVA: 0x003D7880 File Offset: 0x003D5A80
	public void PopulateGallery()
	{
		this.RefreshGalleryFn = null;
		this.galleryGridItemPool.ReturnAll();
		foreach (MinionBrowserScreen.GridItem item in this.Config.items)
		{
			this.<PopulateGallery>g__AddGridIcon|32_0(item);
		}
		this.RefreshGallery();
		this.SelectMinion(this.Config.defaultSelectedItem.Unwrap());
	}

	// Token: 0x0600A164 RID: 41316 RVA: 0x003D78E4 File Offset: 0x003D5AE4
	private void SelectMinion(MinionBrowserScreen.GridItem item)
	{
		this.selectedGridItem = item;
		this.RefreshGallery();
		this.RefreshPreview();
		this.UIMinion.GetMinionVoice().PlaySoundUI("voice_land");
	}

	// Token: 0x0600A165 RID: 41317 RVA: 0x003D791C File Offset: 0x003D5B1C
	public void RefreshPreview()
	{
		this.UIMinion.SetMinion(this.selectedGridItem.GetPersonality());
		this.UIMinion.ReactToPersonalityChange();
		this.detailsHeaderText.SetText(this.selectedGridItem.GetName());
		this.detailHeaderIcon.sprite = this.selectedGridItem.GetIcon();
		this.RefreshOutfitDescription();
		this.RefreshPreviewButtonsInteractable();
		this.SetDioramaBG();
	}

	// Token: 0x0600A166 RID: 41318 RVA: 0x00108E0C File Offset: 0x0010700C
	private void RefreshOutfitDescription()
	{
		if (this.RefreshOutfitDescriptionFn != null)
		{
			this.RefreshOutfitDescriptionFn();
		}
	}

	// Token: 0x0600A167 RID: 41319 RVA: 0x003D7988 File Offset: 0x003D5B88
	private void OnClickChangeOutfit()
	{
		if (this.selectedOutfitType.IsNone())
		{
			return;
		}
		OutfitBrowserScreenConfig.Minion(this.selectedOutfitType.Unwrap(), this.selectedGridItem).WithOutfit(this.selectedOutfit).ApplyAndOpenScreen();
	}

	// Token: 0x0600A168 RID: 41320 RVA: 0x003D79D0 File Offset: 0x003D5BD0
	private void RefreshPreviewButtonsInteractable()
	{
		this.editButton.isInteractable = true;
		if (this.currentOutfitType == ClothingOutfitUtility.OutfitType.JoyResponse)
		{
			Option<string> joyResponseEditError = this.GetJoyResponseEditError();
			if (joyResponseEditError.IsSome())
			{
				this.editButton.isInteractable = false;
				this.editButton.gameObject.AddOrGet<ToolTip>().SetSimpleTooltip(joyResponseEditError.Unwrap());
				return;
			}
			this.editButton.isInteractable = true;
			this.editButton.gameObject.AddOrGet<ToolTip>().ClearMultiStringTooltip();
		}
	}

	// Token: 0x0600A169 RID: 41321 RVA: 0x00108E21 File Offset: 0x00107021
	private void SetDioramaBG()
	{
		this.dioramaBGImage.sprite = KleiPermitDioramaVis.GetDioramaBackground(this.currentOutfitType);
	}

	// Token: 0x0600A16A RID: 41322 RVA: 0x003D7A4C File Offset: 0x003D5C4C
	private Option<string> GetJoyResponseEditError()
	{
		string joyTrait = this.selectedGridItem.GetPersonality().joyTrait;
		if (!(joyTrait == "BalloonArtist"))
		{
			return Option.Some<string>(UI.JOY_RESPONSE_DESIGNER_SCREEN.TOOLTIP_NO_FACADES_FOR_JOY_TRAIT.Replace("{JoyResponseType}", Db.Get().traits.Get(joyTrait).Name));
		}
		return Option.None;
	}

	// Token: 0x0600A16B RID: 41323 RVA: 0x003D7AAC File Offset: 0x003D5CAC
	public void SetEditingOutfitType(ClothingOutfitUtility.OutfitType outfitType)
	{
		this.currentOutfitType = outfitType;
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_OUTFIT_ITEMS;
			this.changeOutfitButton.gameObject.SetActive(true);
			break;
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_JOY_RESPONSE;
			this.changeOutfitButton.gameObject.SetActive(false);
			break;
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			this.editButtonText.text = UI.MINION_BROWSER_SCREEN.BUTTON_EDIT_ATMO_SUIT_OUTFIT_ITEMS;
			this.changeOutfitButton.gameObject.SetActive(true);
			break;
		default:
			throw new NotImplementedException();
		}
		this.RefreshPreviewButtonsInteractable();
		this.OnEditClickedFn = delegate()
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				OutfitDesignerScreenConfig.Minion(this.selectedOutfit.IsSome() ? this.selectedOutfit.Unwrap() : ClothingOutfitTarget.ForNewTemplateOutfit(outfitType), this.selectedGridItem).ApplyAndOpenScreen();
				return;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
			{
				JoyResponseScreenConfig joyResponseScreenConfig = JoyResponseScreenConfig.From(this.selectedGridItem);
				joyResponseScreenConfig = joyResponseScreenConfig.WithInitialSelection(this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().AndThen<BalloonArtistFacadeResource>((string id) => Db.Get().Permits.BalloonArtistFacades.Get(id)));
				joyResponseScreenConfig.ApplyAndOpenScreen();
				return;
			}
			default:
				throw new NotImplementedException();
			}
		};
		this.RefreshOutfitDescriptionFn = delegate()
		{
			switch (outfitType)
			{
			case ClothingOutfitUtility.OutfitType.Clothing:
			case ClothingOutfitUtility.OutfitType.AtmoSuit:
				this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(outfitType);
				this.UIMinion.SetOutfit(outfitType, this.selectedOutfit);
				this.outfitDescriptionPanel.Refresh(this.selectedOutfit, outfitType, this.selectedGridItem.GetPersonality());
				return;
			case ClothingOutfitUtility.OutfitType.JoyResponse:
			{
				this.selectedOutfit = this.selectedGridItem.GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType.Clothing);
				this.UIMinion.SetOutfit(ClothingOutfitUtility.OutfitType.Clothing, this.selectedOutfit);
				string text = this.selectedGridItem.GetJoyResponseOutfitTarget().ReadFacadeId().UnwrapOr(null, null);
				this.outfitDescriptionPanel.Refresh((text != null) ? Db.Get().Permits.Get(text) : null, outfitType, this.selectedGridItem.GetPersonality());
				return;
			}
			default:
				throw new NotImplementedException();
			}
		};
		this.RefreshOutfitDescription();
	}

	// Token: 0x0600A16C RID: 41324 RVA: 0x003D7BA4 File Offset: 0x003D5DA4
	private MinionBrowserScreen.CyclerUI.OnSelectedFn[] CreateCycleOptions()
	{
		MinionBrowserScreen.CyclerUI.OnSelectedFn[] array = new MinionBrowserScreen.CyclerUI.OnSelectedFn[3];
		for (int i = 0; i < 3; i++)
		{
			ClothingOutfitUtility.OutfitType outfitType = (ClothingOutfitUtility.OutfitType)i;
			array[i] = delegate()
			{
				this.selectedOutfitType = Option.Some<ClothingOutfitUtility.OutfitType>(outfitType);
				this.cycler.SetLabel(outfitType.GetName());
				this.SetEditingOutfitType(outfitType);
				this.RefreshPreview();
			};
		}
		return array;
	}

	// Token: 0x0600A16D RID: 41325 RVA: 0x001051E4 File Offset: 0x001033E4
	private void OnMouseOverToggle()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("HUD_Mouseover", false));
	}

	// Token: 0x0600A171 RID: 41329 RVA: 0x003D7BE8 File Offset: 0x003D5DE8
	[CompilerGenerated]
	private void <PopulateGallery>g__AddGridIcon|32_0(MinionBrowserScreen.GridItem item)
	{
		GameObject gameObject = this.galleryGridItemPool.Borrow();
		gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = item.GetIcon();
		gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(item.GetName());
		string requiredDlcId = item.GetPersonality().requiredDlcId;
		ToolTip component = gameObject.GetComponent<ToolTip>();
		Image component2 = gameObject.transform.Find("DlcBanner").GetComponent<Image>();
		if (DlcManager.IsDlcId(requiredDlcId))
		{
			component2.gameObject.SetActive(true);
			component2.color = DlcManager.GetDlcBannerColor(requiredDlcId);
			component.SetSimpleTooltip(string.Format(UI.MINION_BROWSER_SCREEN.TOOLTIP_FROM_DLC, DlcManager.GetDlcTitle(requiredDlcId)));
		}
		else
		{
			component2.gameObject.SetActive(false);
			component.ClearMultiStringTooltip();
		}
		MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
		MultiToggle toggle3 = toggle;
		toggle3.onEnter = (System.Action)Delegate.Combine(toggle3.onEnter, new System.Action(this.OnMouseOverToggle));
		MultiToggle toggle2 = toggle;
		toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
		{
			this.SelectMinion(item);
		}));
		this.RefreshGalleryFn = (System.Action)Delegate.Combine(this.RefreshGalleryFn, new System.Action(delegate()
		{
			toggle.ChangeState((item == this.selectedGridItem) ? 1 : 0);
		}));
	}

	// Token: 0x04007DE2 RID: 32226
	[Header("ItemGalleryColumn")]
	[SerializeField]
	private RectTransform galleryGridContent;

	// Token: 0x04007DE3 RID: 32227
	[SerializeField]
	private GameObject gridItemPrefab;

	// Token: 0x04007DE4 RID: 32228
	private GridLayouter gridLayouter;

	// Token: 0x04007DE5 RID: 32229
	[Header("SelectionDetailsColumn")]
	[SerializeField]
	private KleiPermitDioramaVis permitVis;

	// Token: 0x04007DE6 RID: 32230
	[SerializeField]
	private UIMinion UIMinion;

	// Token: 0x04007DE7 RID: 32231
	[SerializeField]
	private LocText detailsHeaderText;

	// Token: 0x04007DE8 RID: 32232
	[SerializeField]
	private Image detailHeaderIcon;

	// Token: 0x04007DE9 RID: 32233
	[SerializeField]
	private OutfitDescriptionPanel outfitDescriptionPanel;

	// Token: 0x04007DEA RID: 32234
	[SerializeField]
	private MinionBrowserScreen.CyclerUI cycler;

	// Token: 0x04007DEB RID: 32235
	[SerializeField]
	private KButton editButton;

	// Token: 0x04007DEC RID: 32236
	[SerializeField]
	private LocText editButtonText;

	// Token: 0x04007DED RID: 32237
	[SerializeField]
	private KButton changeOutfitButton;

	// Token: 0x04007DEE RID: 32238
	private Option<ClothingOutfitUtility.OutfitType> selectedOutfitType;

	// Token: 0x04007DEF RID: 32239
	private Option<ClothingOutfitTarget> selectedOutfit;

	// Token: 0x04007DF0 RID: 32240
	[Header("Diorama Backgrounds")]
	[SerializeField]
	private Image dioramaBGImage;

	// Token: 0x04007DF1 RID: 32241
	private MinionBrowserScreen.GridItem selectedGridItem;

	// Token: 0x04007DF2 RID: 32242
	private System.Action OnEditClickedFn;

	// Token: 0x04007DF3 RID: 32243
	private bool isFirstDisplay = true;

	// Token: 0x04007DF5 RID: 32245
	private bool postponeConfiguration = true;

	// Token: 0x04007DF6 RID: 32246
	private UIPrefabLocalPool galleryGridItemPool;

	// Token: 0x04007DF7 RID: 32247
	private System.Action RefreshGalleryFn;

	// Token: 0x04007DF8 RID: 32248
	private System.Action RefreshOutfitDescriptionFn;

	// Token: 0x04007DF9 RID: 32249
	private ClothingOutfitUtility.OutfitType currentOutfitType;

	// Token: 0x02001E1F RID: 7711
	private enum MultiToggleState
	{
		// Token: 0x04007DFB RID: 32251
		Default,
		// Token: 0x04007DFC RID: 32252
		Selected,
		// Token: 0x04007DFD RID: 32253
		NonInteractable
	}

	// Token: 0x02001E20 RID: 7712
	[Serializable]
	public class CyclerUI
	{
		// Token: 0x0600A172 RID: 41330 RVA: 0x00108E72 File Offset: 0x00107072
		public void Initialize(MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
		{
			this.cyclePrevButton.onClick += this.CyclePrev;
			this.cycleNextButton.onClick += this.CycleNext;
			this.SetCycleOptions(cycleOptions);
		}

		// Token: 0x0600A173 RID: 41331 RVA: 0x00108EA9 File Offset: 0x001070A9
		public void SetCycleOptions(MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions)
		{
			DebugUtil.Assert(cycleOptions != null);
			DebugUtil.Assert(cycleOptions.Length != 0);
			this.cycleOptions = cycleOptions;
			this.GoTo(0);
		}

		// Token: 0x0600A174 RID: 41332 RVA: 0x003D7D54 File Offset: 0x003D5F54
		public void GoTo(int wrappingIndex)
		{
			if (this.cycleOptions == null || this.cycleOptions.Length == 0)
			{
				return;
			}
			while (wrappingIndex < 0)
			{
				wrappingIndex += this.cycleOptions.Length;
			}
			while (wrappingIndex >= this.cycleOptions.Length)
			{
				wrappingIndex -= this.cycleOptions.Length;
			}
			this.selectedIndex = wrappingIndex;
			this.cycleOptions[this.selectedIndex]();
		}

		// Token: 0x0600A175 RID: 41333 RVA: 0x00108ECC File Offset: 0x001070CC
		public void CyclePrev()
		{
			this.GoTo(this.selectedIndex - 1);
		}

		// Token: 0x0600A176 RID: 41334 RVA: 0x00108EDC File Offset: 0x001070DC
		public void CycleNext()
		{
			this.GoTo(this.selectedIndex + 1);
		}

		// Token: 0x0600A177 RID: 41335 RVA: 0x00108EEC File Offset: 0x001070EC
		public void SetLabel(string text)
		{
			this.currentLabel.text = text;
		}

		// Token: 0x04007DFE RID: 32254
		[SerializeField]
		public KButton cyclePrevButton;

		// Token: 0x04007DFF RID: 32255
		[SerializeField]
		public KButton cycleNextButton;

		// Token: 0x04007E00 RID: 32256
		[SerializeField]
		public LocText currentLabel;

		// Token: 0x04007E01 RID: 32257
		[NonSerialized]
		private int selectedIndex = -1;

		// Token: 0x04007E02 RID: 32258
		[NonSerialized]
		private MinionBrowserScreen.CyclerUI.OnSelectedFn[] cycleOptions;

		// Token: 0x02001E21 RID: 7713
		// (Invoke) Token: 0x0600A17A RID: 41338
		public delegate void OnSelectedFn();
	}

	// Token: 0x02001E22 RID: 7714
	public abstract class GridItem : IEquatable<MinionBrowserScreen.GridItem>
	{
		// Token: 0x0600A17D RID: 41341
		public abstract string GetName();

		// Token: 0x0600A17E RID: 41342
		public abstract Sprite GetIcon();

		// Token: 0x0600A17F RID: 41343
		public abstract string GetUniqueId();

		// Token: 0x0600A180 RID: 41344
		public abstract Personality GetPersonality();

		// Token: 0x0600A181 RID: 41345
		public abstract Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType);

		// Token: 0x0600A182 RID: 41346
		public abstract JoyResponseOutfitTarget GetJoyResponseOutfitTarget();

		// Token: 0x0600A183 RID: 41347 RVA: 0x003D7DB8 File Offset: 0x003D5FB8
		public override bool Equals(object obj)
		{
			MinionBrowserScreen.GridItem gridItem = obj as MinionBrowserScreen.GridItem;
			return gridItem != null && this.Equals(gridItem);
		}

		// Token: 0x0600A184 RID: 41348 RVA: 0x001052BB File Offset: 0x001034BB
		public bool Equals(MinionBrowserScreen.GridItem other)
		{
			return this.GetHashCode() == other.GetHashCode();
		}

		// Token: 0x0600A185 RID: 41349 RVA: 0x00108F09 File Offset: 0x00107109
		public override int GetHashCode()
		{
			return Hash.SDBMLower(this.GetUniqueId());
		}

		// Token: 0x0600A186 RID: 41350 RVA: 0x00108F16 File Offset: 0x00107116
		public override string ToString()
		{
			return this.GetUniqueId();
		}

		// Token: 0x0600A187 RID: 41351 RVA: 0x003D7DD8 File Offset: 0x003D5FD8
		public static MinionBrowserScreen.GridItem.MinionInstanceTarget Of(GameObject minionInstance)
		{
			MinionIdentity component = minionInstance.GetComponent<MinionIdentity>();
			return new MinionBrowserScreen.GridItem.MinionInstanceTarget
			{
				minionInstance = minionInstance,
				minionIdentity = component,
				personality = Db.Get().Personalities.Get(component.personalityResourceId)
			};
		}

		// Token: 0x0600A188 RID: 41352 RVA: 0x00108F1E File Offset: 0x0010711E
		public static MinionBrowserScreen.GridItem.PersonalityTarget Of(Personality personality)
		{
			return new MinionBrowserScreen.GridItem.PersonalityTarget
			{
				personality = personality
			};
		}

		// Token: 0x02001E23 RID: 7715
		public class MinionInstanceTarget : MinionBrowserScreen.GridItem
		{
			// Token: 0x0600A18A RID: 41354 RVA: 0x00108F2C File Offset: 0x0010712C
			public override Sprite GetIcon()
			{
				return this.personality.GetMiniIcon();
			}

			// Token: 0x0600A18B RID: 41355 RVA: 0x00108F39 File Offset: 0x00107139
			public override string GetName()
			{
				return this.minionIdentity.GetProperName();
			}

			// Token: 0x0600A18C RID: 41356 RVA: 0x003D7E1C File Offset: 0x003D601C
			public override string GetUniqueId()
			{
				return "minion_instance_id::" + this.minionInstance.GetInstanceID().ToString();
			}

			// Token: 0x0600A18D RID: 41357 RVA: 0x00108F46 File Offset: 0x00107146
			public override Personality GetPersonality()
			{
				return this.personality;
			}

			// Token: 0x0600A18E RID: 41358 RVA: 0x00108F4E File Offset: 0x0010714E
			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.FromMinion(outfitType, this.minionInstance);
			}

			// Token: 0x0600A18F RID: 41359 RVA: 0x00108F61 File Offset: 0x00107161
			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromMinion(this.minionInstance);
			}

			// Token: 0x04007E03 RID: 32259
			public GameObject minionInstance;

			// Token: 0x04007E04 RID: 32260
			public MinionIdentity minionIdentity;

			// Token: 0x04007E05 RID: 32261
			public Personality personality;
		}

		// Token: 0x02001E24 RID: 7716
		public class PersonalityTarget : MinionBrowserScreen.GridItem
		{
			// Token: 0x0600A191 RID: 41361 RVA: 0x00108F76 File Offset: 0x00107176
			public override Sprite GetIcon()
			{
				return this.personality.GetMiniIcon();
			}

			// Token: 0x0600A192 RID: 41362 RVA: 0x00108F83 File Offset: 0x00107183
			public override string GetName()
			{
				return this.personality.Name;
			}

			// Token: 0x0600A193 RID: 41363 RVA: 0x00108F90 File Offset: 0x00107190
			public override string GetUniqueId()
			{
				return "personality::" + this.personality.nameStringKey;
			}

			// Token: 0x0600A194 RID: 41364 RVA: 0x00108FA7 File Offset: 0x001071A7
			public override Personality GetPersonality()
			{
				return this.personality;
			}

			// Token: 0x0600A195 RID: 41365 RVA: 0x00108FAF File Offset: 0x001071AF
			public override Option<ClothingOutfitTarget> GetClothingOutfitTarget(ClothingOutfitUtility.OutfitType outfitType)
			{
				return ClothingOutfitTarget.TryFromTemplateId(this.personality.GetSelectedTemplateOutfitId(outfitType));
			}

			// Token: 0x0600A196 RID: 41366 RVA: 0x00108FC2 File Offset: 0x001071C2
			public override JoyResponseOutfitTarget GetJoyResponseOutfitTarget()
			{
				return JoyResponseOutfitTarget.FromPersonality(this.personality);
			}

			// Token: 0x04007E06 RID: 32262
			public Personality personality;
		}
	}
}
