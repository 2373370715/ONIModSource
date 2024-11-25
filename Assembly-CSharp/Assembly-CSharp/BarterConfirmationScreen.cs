using Database;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class BarterConfirmationScreen : KModalScreen {
    [SerializeField]
    private KButton cancelButton;

    [SerializeField]
    private KButton closeButton;

    [SerializeField]
    private KButton confirmButton;

    [SerializeField]
    private LocText confirmButtonActionLabel;

    [SerializeField]
    private LocText confirmButtonFilamentLabel;

    [SerializeField]
    private GameObject contentContainer;

    [SerializeField]
    private GameObject filamentIcon;

    [SerializeField]
    private GameObject itemIcon;

    [SerializeField]
    private LocText itemLabel;

    [SerializeField]
    private LocText largeCostLabel;

    [SerializeField]
    private LocText largeQuantityLabel;

    [SerializeField]
    private KBatchedAnimController loadingAnimation;

    [SerializeField]
    private GameObject loadingContainer;

    [SerializeField]
    private LocText mainResultLabel;

    [SerializeField]
    private LocText panelHeaderLabel;

    [SerializeField]
    private GameObject resultContainer;

    [SerializeField]
    private LocText resultFilamentLabel;

    [SerializeField]
    private Image resultIcon;

    [SerializeField]
    private LocText resultLabel;

    private bool shouldCloseScreen;

    [SerializeField]
    private LocText transactionDescriptionLabel;

    protected override void OnActivate() {
        base.OnActivate();
        closeButton.onClick  += delegate { Show(false); };
        cancelButton.onClick += delegate { Show(false); };
    }

    public void Present(PermitResource permit, bool isPurchase) {
        Show();
        ShowContentContainer(true);
        ShowLoadingPanel(false);
        HideResultPanel();
        if (isPurchase) {
            itemIcon.transform.SetAsLastSibling();
            filamentIcon.transform.SetAsFirstSibling();
        } else {
            itemIcon.transform.SetAsFirstSibling();
            filamentIcon.transform.SetAsLastSibling();
        }

        KleiItems.ResponseCallback <>9__1;
        KleiItems.ResponseCallback <>9__2;
        confirmButton.onClick += delegate {
                                     var serverTypeFromPermit = PermitItems.GetServerTypeFromPermit(permit);
                                     if (serverTypeFromPermit == null) return;

                                     ShowContentContainer(false);
                                     HideResultPanel();
                                     ShowLoadingPanel(true);
                                     if (isPurchase) {
                                         var                        itemType = serverTypeFromPermit;
                                         KleiItems.ResponseCallback cb;
                                         if ((cb =  <>9__1) == null)
                                         {
                                             cb = (<>9__1 = delegate(KleiItems.Result result) {
                                                                if (this.IsNullOrDestroyed()) return;

                                                                ShowContentContainer(false);
                                                                ShowLoadingPanel(false);
                                                                if (!result.Success) {
                                                                    ShowResultPanel(permit, true, false);
                                                                    return;
                                                                }

                                                                ShowResultPanel(permit, true, true);
                                                            });
                                         }

                                         KleiItems.AddRequestBarterGainItem(itemType, cb);
                                         return;
                                     }

                                     var itemInstanceID = KleiItems.GetItemInstanceID(serverTypeFromPermit);
                                     KleiItems.ResponseCallback cb2;
                                     if ((cb2 =  <>9__2) == null)
                                     {
                                         cb2 = (<>9__2 = delegate(KleiItems.Result result) {
                                                             if (this.IsNullOrDestroyed()) return;

                                                             ShowContentContainer(false);
                                                             ShowLoadingPanel(false);
                                                             if (!result.Success) {
                                                                 ShowResultPanel(permit, false, false);
                                                                 return;
                                                             }

                                                             ShowResultPanel(permit, false, true);
                                                         });
                                     }

                                     KleiItems.AddRequestBarterLoseItem(itemInstanceID, cb2);
                                 };

        ulong num;
        ulong num2;
        PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
        var permitPresentationInfo = permit.GetPermitPresentationInfo();
        itemIcon.GetComponent<Image>().sprite = permitPresentationInfo.sprite;
        itemLabel.SetText(permit.Name);
        transactionDescriptionLabel.SetText(isPurchase
                                                ? UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_PRINT
                                                : UI.KLEI_INVENTORY_SCREEN.BARTERING.ACTION_DESCRIPTION_RECYCLE);

        panelHeaderLabel.SetText(isPurchase
                                     ? UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_PRINT_HEADER
                                     : UI.KLEI_INVENTORY_SCREEN.BARTERING.CONFIRM_RECYCLE_HEADER);

        confirmButtonActionLabel.SetText(isPurchase
                                             ? UI.KLEI_INVENTORY_SCREEN.BARTERING.BUY
                                             : UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL);

        confirmButtonFilamentLabel.SetText(isPurchase
                                               ? num.ToString()
                                               : UIConstants.ColorPrefixGreen + "+" + num2 + UIConstants.ColorSuffix);

        largeCostLabel.SetText(isPurchase ? "x" + num : "x" + num2);
    }

    private void Update() {
        if (shouldCloseScreen) {
            ShowContentContainer(false);
            ShowLoadingPanel(false);
            HideResultPanel();
            Show(false);
        }
    }

    private void ShowContentContainer(bool show) { contentContainer.SetActive(show); }

    private void ShowLoadingPanel(bool show) {
        loadingContainer.SetActive(show);
        resultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.LOADING);
        if (show)
            loadingAnimation.Play("loading_rocket", KAnim.PlayMode.Loop);
        else
            loadingAnimation.Stop();

        if (!show) shouldCloseScreen = false;
    }

    private void HideResultPanel() { resultContainer.SetActive(false); }

    private void ShowResultPanel(PermitResource permit, bool isPurchase, bool transationResult) {
        resultContainer.SetActive(true);
        if (!transationResult) {
            resultIcon.sprite = Assets.GetSprite("error_message");
            mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_ERROR);
            panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_INCOMPLETE_HEADER);
            resultFilamentLabel.SetText("");
            KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Failed"));
            return;
        }

        panelHeaderLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.TRANSACTION_COMPLETE_HEADER);
        if (isPurchase) {
            var permitPresentationInfo = permit.GetPermitPresentationInfo();
            resultIcon.sprite = permitPresentationInfo.sprite;
            resultFilamentLabel.SetText("");
            mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.PURCHASE_SUCCESS);
            KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Print_Succeed"));
            return;
        }

        ulong num;
        ulong num2;
        PermitItems.TryGetBarterPrice(permit.Id, out num, out num2);
        resultIcon.sprite = Assets.GetSprite("filament");
        resultFilamentLabel.GetComponent<LocText>().SetText("x" + num2);
        mainResultLabel.SetText(UI.KLEI_INVENTORY_SCREEN.BARTERING.SELL_SUCCESS);
        KFMOD.PlayUISound(GlobalAssets.GetSound("SupplyCloset_Bartering_Succeed"));
    }
}