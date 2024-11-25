namespace RsTransferPort;

public class ShowOverlaySelf : KMonoBehaviour {
    [MyCmpReq]
    private TransferPortChannel channelItem;

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Subscribe(493375141, OnRefreshUserMenu);
    }

    protected override void OnCleanUp() {
        base.OnCleanUp();
        Unsubscribe(493375141, OnRefreshUserMenu);
    }

    private void OnRefreshUserMenu(object data) {
        Game.Instance.userMenu.AddButton(gameObject,
                                         new KIconButtonMenu.ButtonInfo("show_overlay_self_icon",
                                                                        MYSTRINGS.UI.USERMENU.SHOWOVERLAYSELF_BUTTON.NAME,
                                                                        ShowOverlay,
                                                                        Action.BuildingUtility1,
                                                                        tooltipText: MYSTRINGS.UI.USERMENU
                                                                            .SHOWOVERLAYSELF_BUTTON.TOOLTIP));
    }

    public void ShowOverlay() {
        if (OverlayScreen.Instance == null) return;

        MyOverlayModes.PortChannel.ActiveChannel(channelItem.ChannelKey);
        if (OverlayScreen.Instance.mode != MyOverlayModes.PortChannel.ID)
            OverlayScreen.Instance.ToggleOverlay(MyOverlayModes.PortChannel.ID);
    }
}