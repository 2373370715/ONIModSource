using System.Collections;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class KleiItemDropScreen : KModalScreen
{
	private struct ServerRequestState
	{
		public bool revealRequested;

		public bool revealConfirmedByServer;

		public bool revealRejectedByServer;

		public void Reset()
		{
			revealRequested = false;
			revealConfirmedByServer = false;
			revealRejectedByServer = false;
		}
	}

	[SerializeField]
	private RectTransform shieldMaskRect;

	[SerializeField]
	private KButton closeButton;

	[Header("Animated Item")]
	[SerializeField]
	private KleiItemDropScreen_PermitVis permitVisualizer;

	[SerializeField]
	private KBatchedAnimController animatedPod;

	[SerializeField]
	private LocText userMessageLabel;

	[Header("Item Info")]
	[SerializeField]
	private RectTransform itemTextContainer;

	[SerializeField]
	private LocText itemNameLabel;

	[SerializeField]
	private LocText itemDescriptionLabel;

	[SerializeField]
	private LocText itemRarityLabel;

	[SerializeField]
	private LocText itemCategoryLabel;

	[Header("Accept Button")]
	[SerializeField]
	private RectTransform acceptButtonRect;

	[SerializeField]
	private KButton acceptButton;

	[SerializeField]
	private KBatchedAnimController animatedLoadingIcon;

	[SerializeField]
	private KButton acknowledgeButton;

	[SerializeField]
	private LocText errorMessage;

	private Coroutine activePresentationRoutine;

	private ServerRequestState serverRequestState;

	private bool giftAcknowledged;

	private bool noItemAvailableAcknowledged;

	public static KleiItemDropScreen Instance;

	private bool shouldDoCloseRoutine;

	private const float TEXT_AND_BUTTON_ANIMATE_OFFSET_Y = -30f;

	private PrefabDefinedUIPosition acceptButtonPosition = new PrefabDefinedUIPosition();

	private PrefabDefinedUIPosition itemTextContainerPosition = new PrefabDefinedUIPosition();

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Instance = this;
		closeButton.onClick += delegate
		{
			Show(show: false);
		};
		if (string.IsNullOrEmpty(KleiAccount.KleiToken))
		{
			base.Show(show: false);
		}
	}

	protected override void OnActivate()
	{
		Instance = this;
		Show(show: false);
	}

	public override void Show(bool show = true)
	{
		serverRequestState.Reset();
		if (!show)
		{
			animatedLoadingIcon.gameObject.SetActive(value: false);
			if (activePresentationRoutine != null)
			{
				StopCoroutine(activePresentationRoutine);
			}
			if (shouldDoCloseRoutine)
			{
				closeButton.gameObject.SetActive(value: false);
				Updater.RunRoutine(this, AnimateScreenOutRoutine()).Then(delegate
				{
					base.Show(show: false);
				});
				shouldDoCloseRoutine = false;
			}
			else
			{
				base.Show(show: false);
			}
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot);
		}
		else
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndItemDropScreenSnapshot);
			base.Show();
		}
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
		{
			Show(show: false);
		}
		base.OnKeyDown(e);
	}

	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			if (PermitItems.HasUnopenedItem())
			{
				PresentNextUnopenedItem();
				shouldDoCloseRoutine = true;
			}
			else
			{
				userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
				PresentNoItemAvailablePrompt(firstItemPresentation: true);
				shouldDoCloseRoutine = true;
			}
		}
	}

	public void PresentNextUnopenedItem(bool firstItemPresentation = true)
	{
		foreach (KleiItems.ItemData item in PermitItems.IterateInventory())
		{
			if (!item.IsOpened)
			{
				PresentItem(item, firstItemPresentation);
				return;
			}
		}
		PresentNoItemAvailablePrompt(firstItemPresentation: false);
	}

	public void PresentItem(KleiItems.ItemData item, bool firstItemPresentation)
	{
		userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.THANKS_FOR_PLAYING);
		giftAcknowledged = false;
		serverRequestState.revealConfirmedByServer = false;
		serverRequestState.revealRejectedByServer = false;
		if (activePresentationRoutine != null)
		{
			StopCoroutine(activePresentationRoutine);
		}
		activePresentationRoutine = StartCoroutine(PresentItemRoutine(item, firstItemPresentation));
		acceptButton.ClearOnClick();
		acknowledgeButton.ClearOnClick();
		acceptButton.GetComponentInChildren<LocText>().SetText(UI.ITEM_DROP_SCREEN.PRINT_ITEM_BUTTON);
		acceptButton.onClick += delegate
		{
			serverRequestState.revealRequested = true;
			PermitItems.QueueRequestOpenOrUnboxItem(item, OnOpenItemRequestResponse);
		};
		acknowledgeButton.onClick += delegate
		{
			if (serverRequestState.revealConfirmedByServer)
			{
				giftAcknowledged = true;
			}
		};
	}

	public void OnOpenItemRequestResponse(KleiItems.Result result)
	{
		if (serverRequestState.revealRequested)
		{
			serverRequestState.revealRequested = false;
			if (result.Success)
			{
				serverRequestState.revealRejectedByServer = false;
				serverRequestState.revealConfirmedByServer = true;
			}
			else
			{
				serverRequestState.revealRejectedByServer = true;
				serverRequestState.revealConfirmedByServer = false;
			}
		}
	}

	public void PresentNoItemAvailablePrompt(bool firstItemPresentation)
	{
		userMessageLabel.SetText(UI.ITEM_DROP_SCREEN.NOTHING_AVAILABLE);
		noItemAvailableAcknowledged = false;
		acknowledgeButton.ClearOnClick();
		acceptButton.ClearOnClick();
		acceptButton.GetComponentInChildren<LocText>().SetText(UI.ITEM_DROP_SCREEN.DISMISS_BUTTON);
		acceptButton.onClick += delegate
		{
			noItemAvailableAcknowledged = true;
		};
		if (activePresentationRoutine != null)
		{
			StopCoroutine(activePresentationRoutine);
		}
		activePresentationRoutine = StartCoroutine(PresentNoItemAvailableRoutine(firstItemPresentation));
	}

	private IEnumerator AnimateScreenInRoutine()
	{
		float scaleFactor = base.transform.parent.GetComponent<CanvasScaler>().scaleFactor;
		float OPEN_WIDTH = (float)Screen.width / scaleFactor;
		float y = Mathf.Clamp((float)Screen.height / scaleFactor, 720f, 900f);
		KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Open"));
		userMessageLabel.gameObject.SetActive(value: false);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			shieldMaskRect.sizeDelta = v2;
		}, shieldMaskRect.sizeDelta, new Vector2(shieldMaskRect.sizeDelta.x, y), 0.5f, Easing.CircInOut);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			shieldMaskRect.sizeDelta = v2;
		}, shieldMaskRect.sizeDelta, new Vector2(OPEN_WIDTH, shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut);
		userMessageLabel.gameObject.SetActive(value: true);
	}

	private IEnumerator AnimateScreenOutRoutine()
	{
		KFMOD.PlayUISound(GlobalAssets.GetSound("GiftItemDrop_Screen_Close"));
		userMessageLabel.gameObject.SetActive(value: false);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			shieldMaskRect.sizeDelta = v2;
		}, shieldMaskRect.sizeDelta, new Vector2(8f, shieldMaskRect.sizeDelta.y), 0.25f, Easing.CircInOut);
		yield return Updater.Ease(delegate(Vector2 v2)
		{
			shieldMaskRect.sizeDelta = v2;
		}, shieldMaskRect.sizeDelta, new Vector2(shieldMaskRect.sizeDelta.x, 0f), 0.25f, Easing.CircInOut);
	}

	private IEnumerator PresentNoItemAvailableRoutine(bool firstItem)
	{
		yield return null;
		itemNameLabel.SetText("");
		itemDescriptionLabel.SetText("");
		itemRarityLabel.SetText("");
		itemCategoryLabel.SetText("");
		if (firstItem)
		{
			animatedPod.Play("idle", KAnim.PlayMode.Loop);
			acceptButtonRect.gameObject.SetActive(value: false);
			shieldMaskRect.sizeDelta = new Vector2(8f, 0f);
			shieldMaskRect.gameObject.SetActive(value: true);
		}
		if (firstItem)
		{
			closeButton.gameObject.SetActive(value: false);
			yield return Updater.WaitForSeconds(0.5f);
			yield return AnimateScreenInRoutine();
			yield return Updater.WaitForSeconds(0.125f);
			closeButton.gameObject.SetActive(value: true);
		}
		else
		{
			yield return Updater.WaitForSeconds(0.25f);
		}
		Vector2 animate_offset = new Vector2(0f, -30f);
		acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0f;
		acceptButtonRect.gameObject.SetActive(value: true);
		acceptButtonPosition.SetOn(acceptButtonRect);
		yield return Updater.WaitForSeconds(0.75f);
		yield return PresUtil.OffsetToAndFade(acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
		yield return Updater.Until(() => noItemAvailableAcknowledged);
		yield return PresUtil.OffsetFromAndFade(acceptButton.rectTransform(), animate_offset, 0f, 0.125f, Easing.SmoothStep);
		Show(show: false);
	}

	private IEnumerator PresentItemRoutine(KleiItems.ItemData item, bool firstItem)
	{
		yield return null;
		if (item.ItemId == 0L)
		{
			Debug.LogError("Could not find dropped item inventory.");
			yield break;
		}
		itemNameLabel.SetText("");
		itemDescriptionLabel.SetText("");
		itemRarityLabel.SetText("");
		itemCategoryLabel.SetText("");
		permitVisualizer.ResetState();
		if (firstItem)
		{
			animatedPod.Play("idle", KAnim.PlayMode.Loop);
			acceptButtonRect.gameObject.SetActive(value: false);
			shieldMaskRect.sizeDelta = new Vector2(8f, 0f);
			shieldMaskRect.gameObject.SetActive(value: true);
		}
		if (firstItem)
		{
			closeButton.gameObject.SetActive(value: false);
			yield return Updater.WaitForSeconds(0.5f);
			yield return AnimateScreenInRoutine();
			yield return Updater.WaitForSeconds(0.125f);
			closeButton.gameObject.SetActive(value: true);
		}
		else
		{
			yield return Updater.WaitForSeconds(0.25f);
		}
		Vector2 animate_offset = new Vector2(0f, -30f);
		acceptButtonRect.FindOrAddComponent<CanvasGroup>().alpha = 0f;
		acceptButtonRect.gameObject.SetActive(value: true);
		acceptButtonPosition.SetOn(acceptButtonRect);
		animatedPod.Play("powerup");
		animatedPod.Queue("working_loop", KAnim.PlayMode.Loop);
		yield return Updater.WaitForSeconds(1.25f);
		yield return PresUtil.OffsetToAndFade(acceptButton.rectTransform(), animate_offset, 1f, 0.125f, Easing.ExpoOut);
		yield return Updater.Until(() => serverRequestState.revealRequested);
		yield return PresUtil.OffsetFromAndFade(acceptButton.rectTransform(), animate_offset, 0f, 0.125f, Easing.SmoothStep);
		animatedLoadingIcon.gameObject.rectTransform().anchoredPosition = new Vector2(0f, -352f);
		if (animatedLoadingIcon.GetComponent<CanvasGroup>() != null)
		{
			animatedLoadingIcon.GetComponent<CanvasGroup>().alpha = 1f;
		}
		yield return new WaitForSecondsRealtime(0.3f);
		if (!serverRequestState.revealConfirmedByServer && !serverRequestState.revealRejectedByServer)
		{
			animatedLoadingIcon.gameObject.SetActive(value: true);
			animatedLoadingIcon.Play("loading_rocket", KAnim.PlayMode.Loop);
			yield return Updater.Until(() => serverRequestState.revealConfirmedByServer || serverRequestState.revealRejectedByServer);
			yield return new WaitForSecondsRealtime(2f);
			yield return PresUtil.OffsetFromAndFade(animatedLoadingIcon.gameObject.rectTransform(), new Vector2(0f, -512f), 0f, 0.25f, Easing.SmoothStep);
			animatedLoadingIcon.gameObject.SetActive(value: false);
		}
		if (serverRequestState.revealRejectedByServer)
		{
			animatedPod.Play("idle", KAnim.PlayMode.Loop);
			errorMessage.gameObject.SetActive(value: true);
			yield return Updater.WaitForSeconds(3f);
			errorMessage.gameObject.SetActive(value: false);
		}
		else if (serverRequestState.revealConfirmedByServer)
		{
			animatedPod.Play("additional_pre");
			animatedPod.Queue("working_loop", KAnim.PlayMode.Loop);
			yield return Updater.WaitForSeconds(1f);
			DropScreenPresentationInfo info = default(DropScreenPresentationInfo);
			info.UseEquipmentVis = false;
			info.BuildOverride = null;
			info.Sprite = null;
			string name = "";
			string desc = "";
			string categoryString = "";
			PermitRarity rarity;
			if (PermitItems.TryGetBoxInfo(item, out name, out desc, out var icon_name))
			{
				info.UseEquipmentVis = false;
				info.BuildOverride = null;
				info.Sprite = Assets.GetSprite(icon_name);
				rarity = PermitRarity.Loyalty;
			}
			else
			{
				PermitResource permitResource = Db.Get().Permits.Get(item.Id);
				info.Sprite = permitResource.GetPermitPresentationInfo().sprite;
				info.UseEquipmentVis = permitResource.Category == PermitCategory.Equipment;
				if (permitResource is EquippableFacadeResource)
				{
					info.BuildOverride = (permitResource as EquippableFacadeResource).BuildOverride;
				}
				name = permitResource.Name;
				desc = permitResource.Description;
				rarity = permitResource.Rarity;
				switch (permitResource.Category)
				{
				case PermitCategory.Building:
					categoryString = Assets.GetPrefab((permitResource as BuildingFacadeResource).PrefabID).GetProperName();
					break;
				case PermitCategory.JoyResponse:
					categoryString = PermitCategories.GetDisplayName(permitResource.Category);
					if (permitResource is BalloonArtistFacadeResource)
					{
						categoryString = PermitCategories.GetDisplayName(permitResource.Category) + ": " + UI.KLEI_INVENTORY_SCREEN.CATEGORIES.JOY_RESPONSES.BALLOON_ARTIST;
					}
					break;
				case PermitCategory.Artwork:
					categoryString = Assets.GetPrefab((permitResource as ArtableStage).prefabId).GetProperName();
					break;
				default:
					categoryString = PermitCategories.GetDisplayName(permitResource.Category);
					break;
				}
			}
			permitVisualizer.ConfigureWith(info);
			yield return permitVisualizer.AnimateIn();
			KFMOD.PlayUISoundWithLabeledParameter(GlobalAssets.GetSound("GiftItemDrop_Rarity"), "GiftItemRarity", $"{rarity}");
			itemNameLabel.SetText(name);
			itemDescriptionLabel.SetText(desc);
			itemRarityLabel.SetText(rarity.GetLocStringName());
			itemCategoryLabel.SetText(categoryString);
			itemTextContainerPosition.SetOn(itemTextContainer);
			yield return Updater.Parallel(PresUtil.OffsetToAndFade(itemTextContainer.rectTransform(), animate_offset, 1f, 0.125f, Easing.CircInOut));
			yield return Updater.Until(() => giftAcknowledged);
			animatedPod.Play("working_pst");
			animatedPod.Queue("idle", KAnim.PlayMode.Loop);
			yield return Updater.Parallel(PresUtil.OffsetFromAndFade(itemTextContainer.rectTransform(), animate_offset, 0f, 0.125f, Easing.CircInOut));
			itemNameLabel.SetText("");
			itemDescriptionLabel.SetText("");
			itemRarityLabel.SetText("");
			itemCategoryLabel.SetText("");
			yield return permitVisualizer.AnimateOut();
		}
		PresentNextUnopenedItem(firstItemPresentation: false);
	}

	public static bool HasItemsToShow()
	{
		if (PermitItems.HasUnopenedItem())
		{
			return true;
		}
		return false;
	}
}
