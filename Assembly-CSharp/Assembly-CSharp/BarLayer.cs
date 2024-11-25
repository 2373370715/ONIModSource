using System.Collections.Generic;
using UnityEngine;

public class BarLayer : GraphLayer {
    public           GameObject             bar_container;
    public           GraphedBarFormatting[] bar_formats;
    private readonly List<GraphedBar>       bars = new List<GraphedBar>();
    public           GameObject             prefab_bar;
    public           int                    bar_count => bars.Count;

    public void NewBar(int[] values, float x_position, string ID = "") {
        var gameObject   = Util.KInstantiateUI(prefab_bar, bar_container, true);
        if (ID == "") ID = bars.Count.ToString();
        gameObject.name = string.Format("bar_{0}", ID);
        var component = gameObject.GetComponent<GraphedBar>();
        component.SetFormat(bar_formats[bars.Count % bar_formats.Length]);
        var array = new int[values.Length];
        for (var i = 0; i < values.Length; i++)
            array[i] = (int)(graph.rectTransform().rect.height * graph.GetRelativeSize(new Vector2(0f, values[i])).y);

        component.SetValues(array, graph.GetRelativePosition(new Vector2(x_position, 0f)).x);
        bars.Add(component);
    }

    public void ClearBars() {
        foreach (var graphedBar in bars)
            if (graphedBar != null && graphedBar.gameObject != null)
                DestroyImmediate(graphedBar.gameObject);

        bars.Clear();
    }
}