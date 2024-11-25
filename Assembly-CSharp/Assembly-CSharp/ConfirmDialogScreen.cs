using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmDialogScreen : KModalScreen {
    private System.Action cancelAction;

    [SerializeField]
    private GameObject cancelButton;

    private System.Action configurableAction;

    [SerializeField]
    private GameObject configurableButton;

    private System.Action confirmAction;

    [SerializeField]
    private GameObject confirmButton;

    public bool deactivateOnCancelAction       = true;
    public bool deactivateOnConfigurableAction = true;
    public bool deactivateOnConfirmAction      = true;

    [SerializeField]
    private Image image;

    public System.Action onDeactivateCB;

    [SerializeField]
    private LocText popupMessage;

    [SerializeField]
    private LocText titleText;

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        gameObject.SetActive(false);
    }

    public override bool IsModal() { return true; }

    public override void OnKeyDown(KButtonEvent e) {
        if (e.TryConsume(Action.Escape)) {
            OnSelect_CANCEL();
            return;
        }

        base.OnKeyDown(e);
    }

    public void PopupConfirmDialog(string        text,
                                   System.Action on_confirm,
                                   System.Action on_cancel,
                                   string        configurable_text       = null,
                                   System.Action on_configurable_clicked = null,
                                   string        title_text              = null,
                                   string        confirm_text            = null,
                                   string        cancel_text             = null,
                                   Sprite        image_sprite            = null) {
        while (transform.parent.GetComponent<Canvas>() == null && transform.parent.parent != null)
            transform.SetParent(transform.parent.parent);

        transform.SetAsLastSibling();
        confirmAction      = on_confirm;
        cancelAction       = on_cancel;
        configurableAction = on_configurable_clicked;
        var num = 0;
        if (confirmAction      != null) num++;
        if (cancelAction       != null) num++;
        if (configurableAction != null) num++;
        confirmButton.GetComponentInChildren<LocText>().text
            = confirm_text == null ? UI.CONFIRMDIALOG.OK.text : confirm_text;

        cancelButton.GetComponentInChildren<LocText>().text
            = cancel_text == null ? UI.CONFIRMDIALOG.CANCEL.text : cancel_text;

        confirmButton.GetComponent<KButton>().onClick      += OnSelect_OK;
        cancelButton.GetComponent<KButton>().onClick       += OnSelect_CANCEL;
        configurableButton.GetComponent<KButton>().onClick += OnSelect_third;
        cancelButton.SetActive(on_cancel != null);
        if (configurableButton != null) {
            configurableButton.SetActive(configurableAction != null);
            if (configurable_text != null)
                configurableButton.GetComponentInChildren<LocText>().text = configurable_text;
        }

        if (image_sprite != null) {
            image.sprite = image_sprite;
            image.gameObject.SetActive(true);
        }

        if (title_text != null) {
            titleText.key  = "";
            titleText.text = title_text;
        }

        popupMessage.text = text;
    }

    public void OnSelect_OK() {
        if (deactivateOnConfirmAction) Deactivate();
        if (confirmAction != null) confirmAction();
    }

    public void OnSelect_CANCEL() {
        if (deactivateOnCancelAction) Deactivate();
        if (cancelAction != null) cancelAction();
    }

    public void OnSelect_third() {
        if (deactivateOnConfigurableAction) Deactivate();
        if (configurableAction != null) configurableAction();
    }

    protected override void OnDeactivate() {
        if (onDeactivateCB != null) onDeactivateCB();
        base.OnDeactivate();
    }
}