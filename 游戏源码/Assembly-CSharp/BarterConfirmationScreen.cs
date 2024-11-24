using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class BarterConfirmationScreen : KModalScreen
{
	[SerializeField]
	private GameObject itemIcon;

	[SerializeField]
	private GameObject filamentIcon;

	[SerializeField]
	private LocText largeCostLabel;

	[SerializeField]
	private LocText largeQuantityLabel;

	[SerializeField]
	private LocText itemLabel;

	[SerializeField]
	private LocText transactionDescriptionLabel;

	[SerializeField]
	private KButton confirmButton;

	[SerializeField]
	private KButton cancelButton;

	[SerializeField]
	private KButton closeButton;

	[SerializeField]
	private LocText panelHeaderLabel;

	[SerializeField]
	private LocText confirmButtonActionLabel;

	[SerializeField]
	private LocText confirmButtonFilamentLabel;

	[SerializeField]
	private LocText resultLabel;

	[SerializeField]
	private KBatchedAnimController loadingAnimation;

	[SerializeField]
	private GameObject contentContainer;

	[SerializeField]
	private GameObject loadingContainer;

	[SerializeField]
	private GameObject resultContainer;

	[SerializeField]
	private Image resultIcon;

	[SerializeField]
	private LocText mainResultLabel;

	[SerializeField]
	private LocText resultFilamentLabel;

	private bool shouldCloseScreen;

	protected override void OnActivate()
	{
		base.OnActivate();
		closeButton.onClick += delegate
		{
			Show(show: false);
		};
		cancelButton.onClick += delegate
		{
			Show(show: false);
		};
	}

	public void Present(PermitResource permit, bool isPurchase)
	{
		Show();
		ShowContentContainer(show: true);
		ShowLoadingPanel(show: false);
		HideResultPanel();
		if (isPurchase)
		{
			itemIcon.transform.SetAsLastSibling();
			filamentIcon.transform.SetAsFirstSibling();
		}
		else
		{
			itemIcon.transform.SetAsFirstSibling();
			filamentIcon.transform.SetAsLastSibling();
		}
		confirmButton.onClick += delegate
		{
			string serverTypeFromPermit = PermitItems.GetServerTypeFromPermit(permit);
			if (serverTypeFromPermit != null)
			{
				ShowContentContainer(show: false);
				HideResultPanel();
				ShowLoadingPanel(show: true);
				if (isPurchase)
				{
					KleiItems.AddRequestBarterGainItem(serverTypeFromPermit, delegate(KleiItems.Result result)
					{
						if (!this.IsNullOrDestroyed())
						{
							ShowContentContainer(show: false);
							ShowLoadingPanel(show: false);
							if (!result.Success)
							{
								ShowResultPanel(permit, isPurchase: true, transationResult: false);
							}
							else
							{
								ShowResultPanel(permit, isPurchase: true, transationResult: true);
							}
						}
					});
				}
				else
				{
					KleiItems.AddRequestBarterLoseItem(KleiItems.GetItemInstanceID(serverTypeFromPermit), delegate(KleiItems.Result result)
					{
						if (!this.IsNullOrDestroyed())
						{
							ShowContentContainer(show: false);
							ShowLoadingPanel(show: false);
							if (!result.Success)
							{
								ShowResultPanel(permit, isPurchase: false, transationResult: false);
							}
							else
							{
								ShowResultPanel(permit, isPurchase: false, transationResult: true);
							}
						}
					});
				}
			}
		};
		PermitItems.TryGetBarterPrice(permit.Id, out var buy_price, out var sell_price);
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		itemIcon.GetComponent<Image>().sprite = permitPresentationInfo.sprite;
		itemLabel.SetText(permit.Name);
		transactionDescriptionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_PRINT : UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_RECYCLE);
		panelHeaderLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_PRINT_HEADER : UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_RECYCLE_HEADER);
		confirmButtonActionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.BUY : UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL);
		confirmButtonFilamentLabel.SetText(isPurchase ? buy_price.ToString() : (UIConstants.ColorPrefixGreen + "+" + sell_price + UIConstants.ColorSuffix));
		largeCostLabel.SetText(isPurchase ? ("x" + buy_price) : ("x" + sell_price));
	}

	private void Update()
	{
		if (shouldCloseScreen)
		{
			ShowContentContainer(show: false);
			ShowLoadingPanel(show: false);
			HideResultPanel();
			Show(show: false);
		}
	}

	private void ShowContentContainer(bool show)
	{
		contentContainer.SetActive(show);
	}

	private void ShowLoadingPanel(bool show)
	{
		loadingContainer.SetActive(show);
		resultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.LOADING);
		if (show)
		{
			loadingAnimation.Play("loading_rocket", KAnim.PlayMode.Loop);
		}
		else
		{
			loadingAnimation.Stop();
		}
		if (!show)
		{
			shouldCloseScreen = false;
		}
	}

	private void HideResultPanel()
	{
		resultContainer.SetActive(value: false);
	}

	private void ShowResultPanel(PermitResource permit, bool isPurchase, bool transationResult)
	{
		resultContainer.SetActive(value: true);
		if (!transationResult)
		{
			resultIcon.sprite = Assets.GetSprite("error_message");
			mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_ERROR);
			panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_INCOMPLETE_HEADER);
			resultFilamentLabel.SetText("");
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Failed"));
			return;
		}
		panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_COMPLETE_HEADER);
		if (isPurchase)
		{
			PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
			resultIcon.sprite = permitPresentationInfo.sprite;
			resultFilamentLabel.SetText("");
			mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.PURCHASE_SUCCESS);
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Print_Succeed"));
		}
		else
		{
			PermitItems.TryGetBarterPrice(permit.Id, out var _, out var sell_price);
			resultIcon.sprite = Assets.GetSprite("filament");
			resultFilamentLabel.GetComponent<LocText>().SetText("x" + sell_price);
			mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL_SUCCESS);
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Succeed"));
		}
	}
}
