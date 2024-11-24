using System;
using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C06 RID: 7174
public class BarterConfirmationScreen : KModalScreen
{
	// Token: 0x0600950E RID: 38158 RVA: 0x00101159 File Offset: 0x000FF359
	protected override void OnActivate()
	{
		base.OnActivate();
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.cancelButton.onClick += delegate()
		{
			this.Show(false);
		};
	}

	// Token: 0x0600950F RID: 38159 RVA: 0x00398684 File Offset: 0x00396884
	public void Present(PermitResource permit, bool isPurchase)
	{
		this.Show(true);
		this.ShowContentContainer(true);
		this.ShowLoadingPanel(false);
		this.HideResultPanel();
		if (isPurchase)
		{
			this.itemIcon.transform.SetAsLastSibling();
			this.filamentIcon.transform.SetAsFirstSibling();
		}
		else
		{
			this.itemIcon.transform.SetAsFirstSibling();
			this.filamentIcon.transform.SetAsLastSibling();
		}
		KleiItems.ResponseCallback <>9__1;
		KleiItems.ResponseCallback <>9__2;
		this.confirmButton.onClick += delegate()
		{
			string serverTypeFromPermit = PermitItems.GetServerTypeFromPermit(permit);
			if (serverTypeFromPermit == null)
			{
				return;
			}
			this.ShowContentContainer(false);
			this.HideResultPanel();
			this.ShowLoadingPanel(true);
			if (isPurchase)
			{
				string itemType = serverTypeFromPermit;
				KleiItems.ResponseCallback cb;
				if ((cb = <>9__1) == null)
				{
					cb = (<>9__1 = delegate(KleiItems.Result result)
					{
						if (this.IsNullOrDestroyed())
						{
							return;
						}
						this.ShowContentContainer(false);
						this.ShowLoadingPanel(false);
						if (!result.Success)
						{
							this.ShowResultPanel(permit, true, false);
							return;
						}
						this.ShowResultPanel(permit, true, true);
					});
				}
				KleiItems.AddRequestBarterGainItem(itemType, cb);
				return;
			}
			ulong itemInstanceID = KleiItems.GetItemInstanceID(serverTypeFromPermit);
			KleiItems.ResponseCallback cb2;
			if ((cb2 = <>9__2) == null)
			{
				cb2 = (<>9__2 = delegate(KleiItems.Result result)
				{
					if (this.IsNullOrDestroyed())
					{
						return;
					}
					this.ShowContentContainer(false);
					this.ShowLoadingPanel(false);
					if (!result.Success)
					{
						this.ShowResultPanel(permit, false, false);
						return;
					}
					this.ShowResultPanel(permit, false, true);
				});
			}
			KleiItems.AddRequestBarterLoseItem(itemInstanceID, cb2);
		};
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
		PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
		this.itemIcon.GetComponent<Image>().sprite = permitPresentationInfo.sprite;
		this.itemLabel.SetText(permit.Name);
		this.transactionDescriptionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_PRINT : UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_RECYCLE);
		this.panelHeaderLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_PRINT_HEADER : UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_RECYCLE_HEADER);
		this.confirmButtonActionLabel.SetText(isPurchase ? UI.KLEI_INVENTORY_SCREEN.BARTERING.BUY : UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL);
		this.confirmButtonFilamentLabel.SetText(isPurchase ? num.ToString() : (UIConstants.ColorPrefixGreen + "+" + num2.ToString() + UIConstants.ColorSuffix));
		this.largeCostLabel.SetText(isPurchase ? ("x" + num.ToString()) : ("x" + num2.ToString()));
	}

	// Token: 0x06009510 RID: 38160 RVA: 0x0010118F File Offset: 0x000FF38F
	private void Update()
	{
		if (this.shouldCloseScreen)
		{
			this.ShowContentContainer(false);
			this.ShowLoadingPanel(false);
			this.HideResultPanel();
			this.Show(false);
		}
	}

	// Token: 0x06009511 RID: 38161 RVA: 0x001011B4 File Offset: 0x000FF3B4
	private void ShowContentContainer(bool show)
	{
		this.contentContainer.SetActive(show);
	}

	// Token: 0x06009512 RID: 38162 RVA: 0x00398850 File Offset: 0x00396A50
	private void ShowLoadingPanel(bool show)
	{
		this.loadingContainer.SetActive(show);
		this.resultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.LOADING);
		if (show)
		{
			this.loadingAnimation.Play("loading_rocket", KAnim.PlayMode.Loop, 1f, 0f);
		}
		else
		{
			this.loadingAnimation.Stop();
		}
		if (!show)
		{
			this.shouldCloseScreen = false;
		}
	}

	// Token: 0x06009513 RID: 38163 RVA: 0x001011C2 File Offset: 0x000FF3C2
	private void HideResultPanel()
	{
		this.resultContainer.SetActive(false);
	}

	// Token: 0x06009514 RID: 38164 RVA: 0x003988B8 File Offset: 0x00396AB8
	private void ShowResultPanel(PermitResource permit, bool isPurchase, bool transationResult)
	{
		this.resultContainer.SetActive(true);
		if (!transationResult)
		{
			this.resultIcon.sprite = Assets.GetSprite("error_message");
			this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_ERROR);
			this.panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_INCOMPLETE_HEADER);
			this.resultFilamentLabel.SetText("");
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Failed", false));
			return;
		}
		this.panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_COMPLETE_HEADER);
		if (isPurchase)
		{
			PermitPresentationInfo permitPresentationInfo = permit.GetPermitPresentationInfo();
			this.resultIcon.sprite = permitPresentationInfo.sprite;
			this.resultFilamentLabel.SetText("");
			this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.PURCHASE_SUCCESS);
			KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Print_Succeed", false));
			return;
		}
		ulong num;
		ulong num2;
		PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
		this.resultIcon.sprite = Assets.GetSprite("filament");
		this.resultFilamentLabel.GetComponent<LocText>().SetText("x" + num2.ToString());
		this.mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL_SUCCESS);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Succeed", false));
	}

	// Token: 0x04007397 RID: 29591
	[SerializeField]
	private GameObject itemIcon;

	// Token: 0x04007398 RID: 29592
	[SerializeField]
	private GameObject filamentIcon;

	// Token: 0x04007399 RID: 29593
	[SerializeField]
	private LocText largeCostLabel;

	// Token: 0x0400739A RID: 29594
	[SerializeField]
	private LocText largeQuantityLabel;

	// Token: 0x0400739B RID: 29595
	[SerializeField]
	private LocText itemLabel;

	// Token: 0x0400739C RID: 29596
	[SerializeField]
	private LocText transactionDescriptionLabel;

	// Token: 0x0400739D RID: 29597
	[SerializeField]
	private KButton confirmButton;

	// Token: 0x0400739E RID: 29598
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x0400739F RID: 29599
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040073A0 RID: 29600
	[SerializeField]
	private LocText panelHeaderLabel;

	// Token: 0x040073A1 RID: 29601
	[SerializeField]
	private LocText confirmButtonActionLabel;

	// Token: 0x040073A2 RID: 29602
	[SerializeField]
	private LocText confirmButtonFilamentLabel;

	// Token: 0x040073A3 RID: 29603
	[SerializeField]
	private LocText resultLabel;

	// Token: 0x040073A4 RID: 29604
	[SerializeField]
	private KBatchedAnimController loadingAnimation;

	// Token: 0x040073A5 RID: 29605
	[SerializeField]
	private GameObject contentContainer;

	// Token: 0x040073A6 RID: 29606
	[SerializeField]
	private GameObject loadingContainer;

	// Token: 0x040073A7 RID: 29607
	[SerializeField]
	private GameObject resultContainer;

	// Token: 0x040073A8 RID: 29608
	[SerializeField]
	private Image resultIcon;

	// Token: 0x040073A9 RID: 29609
	[SerializeField]
	private LocText mainResultLabel;

	// Token: 0x040073AA RID: 29610
	[SerializeField]
	private LocText resultFilamentLabel;

	// Token: 0x040073AB RID: 29611
	private bool shouldCloseScreen;
}
