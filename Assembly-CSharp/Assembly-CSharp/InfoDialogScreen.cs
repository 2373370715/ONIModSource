using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class InfoDialogScreen : KModalScreen {
    [SerializeField]
    private GameObject contentContainer;

    private bool escapeCloses;

    [Space(10f), SerializeField]
    private LocText header;

    [SerializeField]
    private GameObject leftButtonPanel;

    [SerializeField]
    private GameObject leftButtonPrefab;

    [SerializeField]
    private InfoScreenLineItem lineItemTemplate;

    public System.Action onDeactivateFn;

    [SerializeField]
    private InfoScreenPlainText plainTextTemplate;

    [SerializeField]
    private GameObject rightButtonPanel;

    [SerializeField]
    private GameObject rightButtonPrefab;

    [SerializeField]
    private InfoScreenSpriteItem spriteItemTemplate;

    [SerializeField]
    private InfoScreenPlainText subHeaderTemplate;

    public InfoScreenPlainText GetSubHeaderPrefab()       { return subHeaderTemplate; }
    public InfoScreenPlainText GetPlainTextPrefab()       { return plainTextTemplate; }
    public InfoScreenLineItem  GetLineItemPrefab()        { return lineItemTemplate; }
    public GameObject          GetPrimaryButtonPrefab()   { return leftButtonPrefab; }
    public GameObject          GetSecondaryButtonPrefab() { return rightButtonPrefab; }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        gameObject.SetActive(false);
    }

    public override bool IsModal() { return true; }

    public override void OnKeyDown(KButtonEvent e) {
        if (!escapeCloses) {
            e.TryConsume(Action.Escape);
            return;
        }

        if (e.TryConsume(Action.Escape)) {
            Deactivate();
            return;
        }

        if (PlayerController.Instance != null && PlayerController.Instance.ConsumeIfNotDragging(e, Action.MouseRight)) {
            Deactivate();
            return;
        }

        base.OnKeyDown(e);
    }

    protected override void OnShow(bool show) {
        base.OnShow(show);
        if (!show && onDeactivateFn != null) onDeactivateFn();
    }

    public InfoDialogScreen AddDefaultOK(bool escapeCloses = false) {
        AddOption(UI.CONFIRMDIALOG.OK, delegate(InfoDialogScreen d) { d.Deactivate(); }, true);
        this.escapeCloses = escapeCloses;
        return this;
    }

    public InfoDialogScreen AddDefaultCancel() {
        AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d) { d.Deactivate(); });
        escapeCloses = true;
        return this;
    }

    public InfoDialogScreen AddOption(string text, Action<InfoDialogScreen> action, bool rightSide = false) {
        var gameObject = Util.KInstantiateUI(rightSide ? rightButtonPrefab : leftButtonPrefab,
                                             rightSide ? rightButtonPanel : leftButtonPanel,
                                             true);

        gameObject.gameObject.GetComponentInChildren<LocText>().text =  text;
        gameObject.gameObject.GetComponent<KButton>().onClick        += delegate { action(this); };
        return this;
    }

    public InfoDialogScreen AddOption(bool rightSide, out KButton button, out LocText buttonText) {
        var gameObject = Util.KInstantiateUI(rightSide ? rightButtonPrefab : leftButtonPrefab,
                                             rightSide ? rightButtonPanel : leftButtonPanel,
                                             true);

        button     = gameObject.GetComponent<KButton>();
        buttonText = gameObject.GetComponentInChildren<LocText>();
        return this;
    }

    public InfoDialogScreen SetHeader(string header) {
        this.header.text = header;
        return this;
    }

    public InfoDialogScreen AddSprite(Sprite sprite) {
        Util.KInstantiateUI<InfoScreenSpriteItem>(spriteItemTemplate.gameObject, contentContainer).SetSprite(sprite);
        return this;
    }

    public InfoDialogScreen AddPlainText(string text) {
        Util.KInstantiateUI<InfoScreenPlainText>(plainTextTemplate.gameObject, contentContainer).SetText(text);
        return this;
    }

    public InfoDialogScreen AddLineItem(string text, string tooltip) {
        var infoScreenLineItem = Util.KInstantiateUI<InfoScreenLineItem>(lineItemTemplate.gameObject, contentContainer);
        infoScreenLineItem.SetText(text);
        infoScreenLineItem.SetTooltip(tooltip);
        return this;
    }

    public InfoDialogScreen AddSubHeader(string text) {
        Util.KInstantiateUI<InfoScreenPlainText>(subHeaderTemplate.gameObject, contentContainer).SetText(text);
        return this;
    }

    public InfoDialogScreen AddSpacer(float height) {
        var gameObject = new GameObject("spacer");
        gameObject.SetActive(false);
        gameObject.transform.SetParent(contentContainer.transform, false);
        var layoutElement = gameObject.AddComponent<LayoutElement>();
        layoutElement.minHeight       = height;
        layoutElement.preferredHeight = height;
        layoutElement.flexibleHeight  = 0f;
        gameObject.SetActive(true);
        return this;
    }

    public InfoDialogScreen AddUI<T>(T prefab, out T spawn) where T : MonoBehaviour {
        spawn = Util.KInstantiateUI<T>(prefab.gameObject, contentContainer, true);
        return this;
    }

    public InfoDialogScreen AddDescriptors(List<Descriptor> descriptors) {
        for (var i = 0; i < descriptors.Count; i++)
            AddLineItem(descriptors[i].IndentedText(), descriptors[i].tooltipText);

        return this;
    }
}