using System;
using UnityEngine;

public class MessageDialogFrame : KScreen {
    [SerializeField]
    private RectTransform body;

    [SerializeField]
    private KButton closeButton;

    [SerializeField]
    private MultiToggle dontShowAgainButton;

    private System.Action dontShowAgainDelegate;

    [SerializeField]
    private GameObject dontShowAgainElement;

    [SerializeField]
    private KToggle nextMessageButton;

    [SerializeField]
    private LocText title;

    public override float GetSortKey() { return 15f; }

    protected override void OnActivate() {
        closeButton.onClick       += OnClickClose;
        nextMessageButton.onClick += OnClickNextMessage;
        var multiToggle = dontShowAgainButton;
        multiToggle.onClick
            = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(OnClickDontShowAgain));

        var flag = KPlayerPrefs.GetInt("HideTutorial_CheckState", 0) == 1;
        dontShowAgainButton.ChangeState(flag ? 0 : 1);
        Subscribe(Messenger.Instance.gameObject, -599791736, OnMessagesChanged);
        OnMessagesChanged(null);
    }

    protected override void OnDeactivate() {
        Unsubscribe(Messenger.Instance.gameObject, -599791736, OnMessagesChanged);
    }

    private void OnClickClose() {
        TryDontShowAgain();
        Destroy(gameObject);
    }

    private void OnClickNextMessage() {
        TryDontShowAgain();
        Destroy(gameObject);
        NotificationScreen.Instance.OnClickNextMessage();
    }

    private void OnClickDontShowAgain() {
        dontShowAgainButton.NextState();
        var flag = dontShowAgainButton.CurrentState == 0;
        KPlayerPrefs.SetInt("HideTutorial_CheckState", flag ? 1 : 0);
    }

    private void OnMessagesChanged(object data) {
        nextMessageButton.gameObject.SetActive(Messenger.Instance.Count != 0);
    }

    public void SetMessage(MessageDialog dialog, Message message) {
        title.text = message.GetTitle().ToUpper();
        dialog.GetComponent<RectTransform>().SetParent(body.GetComponent<RectTransform>());
        var component = dialog.GetComponent<RectTransform>();
        component.offsetMin = Vector2.zero;
        component.offsetMax = Vector2.zero;
        dialog.transform.SetLocalPosition(Vector3.zero);
        dialog.SetMessage(message);
        dialog.OnClickAction();
        if (dialog.CanDontShowAgain) {
            dontShowAgainElement.SetActive(true);
            dontShowAgainDelegate = dialog.OnDontShowAgain;
            return;
        }

        dontShowAgainElement.SetActive(false);
        dontShowAgainDelegate = null;
    }

    private void TryDontShowAgain() {
        if (dontShowAgainDelegate != null && dontShowAgainButton.CurrentState == 0) dontShowAgainDelegate();
    }
}