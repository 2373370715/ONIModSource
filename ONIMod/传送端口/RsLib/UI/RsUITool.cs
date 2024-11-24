﻿using UnityEngine;
using UnityEngine.UI;

namespace RsLib;

public static class RsUITool {
    /// <summary>
    ///     布满到父类
    /// </summary>
    /// <param name="rectTransform"></param>
    public static void FullParent(this RectTransform rectTransform) {
        rectTransform.anchorMin        = Vector2.zero;
        rectTransform.anchorMax        = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta        = Vector2.zero;
    }

    public static void SetSizeWithPreferred(this RectTransform rectTransform) {
        var element = rectTransform.GetComponent<LayoutElement>();
        var w       = LayoutUtility.GetPreferredWidth(rectTransform);
        var h       = LayoutUtility.GetPreferredHeight(rectTransform);
        rectTransform.sizeDelta = new Vector2(w, h);
    }

    public static Vector2 LocalSizeWithScale(this RectTransform rectTransform) {
        Vector2 scale     = rectTransform.localScale;
        var     sizeDelta = rectTransform.sizeDelta;
        return scale * sizeDelta;
    }

    public static RectTransform LeftTopWithAnchored(this RectTransform rectTransform, float left, float top) {
        var positon = rectTransform.LocalSizeWithScale() * rectTransform.pivot;
        rectTransform.anchoredPosition = positon + new Vector2(left, -top);
        return rectTransform;
    }
}