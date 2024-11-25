using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[ExecuteAlways]
public class KleiPermitDioramaVisScaler : UIBehaviour {
    public const float REFERENCE_WIDTH  = 1700f;
    public const float REFERENCE_HEIGHT = 800f;

    [SerializeField]
    private RectTransform root;

    [SerializeField]
    private RectTransform scaleTarget;

    [SerializeField]
    private RectTransform slot;

    protected override void OnRectTransformDimensionsChange() { Layout(); }
    public             void Layout()                          { Layout(root, scaleTarget, slot); }

    public static void Layout(RectTransform root, RectTransform scaleTarget, RectTransform slot) {
        var aspectRatio       = 2.125f;
        var aspectRatioFitter = slot.FindOrAddComponent<AspectRatioFitter>();
        aspectRatioFitter.aspectRatio = aspectRatio;
        aspectRatioFitter.aspectMode  = AspectRatioFitter.AspectMode.WidthControlsHeight;
        var num  = 1700f;
        var a    = Mathf.Max(0.1f, root.rect.width) / num;
        var num2 = 800f;
        var b    = Mathf.Max(0.1f, root.rect.height) / num2;
        var d    = Mathf.Max(a, b);
        scaleTarget.localScale       = Vector3.one * d;
        scaleTarget.sizeDelta        = new Vector2(1700f, 800f);
        scaleTarget.anchorMin        = Vector2.one * 0.5f;
        scaleTarget.anchorMax        = Vector2.one * 0.5f;
        scaleTarget.pivot            = Vector2.one * 0.5f;
        scaleTarget.anchoredPosition = Vector2.zero;
    }
}