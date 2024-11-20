using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RsLib;

public class RsGameObjectDebug {
    private static readonly StringBuilder stringBuilder = new();

    private static void RecursiveBuildFullName(Transform obj) {
        if (obj == null) return;

        RecursiveBuildFullName(obj.transform.parent);
        stringBuilder.Append("/").Append(obj.name);
    }

    private static StringBuilder BuildFullName(GameObject obj) {
        stringBuilder.Length = 0;
        RecursiveBuildFullName(obj.transform);
        return stringBuilder.Append(" (").Append(obj.GetInstanceID()).Append(")");
    }

    public static string FullName(GameObject obj) { return BuildFullName(obj).ToString(); }

    public static string FullName(Component cmp) {
        return BuildFullName(cmp.gameObject)
               .Append(" (")
               .Append(cmp.GetType())
               .Append(" ")
               .Append(cmp.GetInstanceID().ToString())
               .Append(")")
               .ToString();
    }

    public static string AllComponentTypeFullNames(GameObject obj) {
        stringBuilder.Length = 0;
        RecursiveBuildAllComponentTypeFullNames(obj);
        return stringBuilder.ToString();
    }

    private static void RecursiveBuildAllComponentTypeFullNames(GameObject obj) {
        var components = new List<Component>();
        obj.GetComponents(components);
        for (var i = 0; i < components.Count; i++) {
            var component = components[i];
            if (i != 0) stringBuilder.Append(" | ");
            stringBuilder.Append(component.GetType().FullName);
        }
    }

    public static string BuildTreeInChildren(GameObject obj) {
        stringBuilder.Length = 0;
        RecursiveBuildTreeInChildren(obj.transform);
        return stringBuilder.ToString();
    }

    private static void RecursiveBuildTreeInChildren(Transform transform, string prefix = "") {
        if (transform == null) return;

        stringBuilder.AppendLine()
                     .Append(prefix)
                     .Append("|- ")
                     .Append(transform.name)
                     .Append(" [")
                     .Append(transform.gameObject.GetInstanceID())
                     .Append(" ")
                     .Append(transform.gameObject.activeSelf ? "active" : "noActive")
                     .Append(" layer:")
                     .Append(LayerMask.LayerToName(transform.gameObject.layer));

        if (transform is RectTransform rect) {
            stringBuilder.Append(" ").Append(rect.sizeDelta.x).Append("x").Append(rect.sizeDelta.y);
            stringBuilder.Append(" anchor:")
                         .Append(rect.anchorMin.x)
                         .Append("x")
                         .Append(rect.anchorMin.y)
                         .Append("x")
                         .Append(rect.anchorMax.x)
                         .Append("x")
                         .Append(rect.anchorMax.y);

            stringBuilder.AppendFormat(" rect:{0}x{1}x{2}x{3}",
                                       rect.rect.xMin,
                                       rect.rect.yMax,
                                       rect.rect.xMax,
                                       rect.rect.yMin);

            stringBuilder.AppendFormat(" offset:{0}x{1}x{2}x{3}",
                                       rect.offsetMin.x,
                                       rect.offsetMin.y,
                                       rect.offsetMax.x,
                                       rect.offsetMax.y);
        }

        // LayoutElement layoutElement = transform.GetComponent<LayoutElement>();
        // if (null != layoutElement)
        // {
        //     stringBuilder.AppendFormat(" preferred:{0}x{1}", layoutElement.preferredWidth,
        //         layoutElement.preferredHeight);
        //     stringBuilder.AppendFormat(" flexible:{0}x{1}", layoutElement.flexibleWidth,
        //         layoutElement.flexibleHeight);
        // }
        //
        stringBuilder.Append("]");

        stringBuilder.Append("  [");
        RecursiveBuildAllComponentTypeFullNames(transform.gameObject);
        stringBuilder.Append("]");

        var text = transform.GetComponent<TMP_Text>();
        if (text != null)
            stringBuilder.AppendLine()
                         .Append(prefix + "    ")
                         .AppendFormat("[text:{0}]", text.text != null ? Regex.Unescape(text.text) : "<null>");

        var image = transform.GetComponent<Image>();
        if (image != null)
            stringBuilder.AppendLine()
                         .Append(prefix)
                         .AppendFormat("[sprite:{0}]", image.sprite != null ? image.sprite.name : "<null>");

        for (var i = 0; i < transform.childCount; i++)
            RecursiveBuildTreeInChildren(transform.GetChild(i), prefix + "    ");
    }

    private static void BuildComponents(GameObject go, string prefix = "") {
        var components = go.GetComponents<Component>();
        foreach (var component in components) {
            stringBuilder.AppendFormat("{0}[{1}]", prefix, component.GetType().FullName);

            if (component is Image image) stringBuilder.AppendFormat("{0}:{1}", "sprite", image.sprite.name);

            if (component is TMP_Text text) stringBuilder.AppendFormat("{0}:{1}", "text", text.text);
        }
    }

    private static void BuildComponentFields(Component component) { }
}