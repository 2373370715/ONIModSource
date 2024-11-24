using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CEC RID: 7404
[AddComponentMenu("KMonoBehaviour/scripts/GraphedBar")]
[Serializable]
public class GraphedBar : KMonoBehaviour
{
	// Token: 0x06009AA2 RID: 39586 RVA: 0x001048F0 File Offset: 0x00102AF0
	public void SetFormat(GraphedBarFormatting format)
	{
		this.format = format;
	}

	// Token: 0x06009AA3 RID: 39587 RVA: 0x003BA908 File Offset: 0x003B8B08
	public void SetValues(int[] values, float x_position)
	{
		this.ClearValues();
		base.gameObject.rectTransform().anchorMin = new Vector2(x_position, 0f);
		base.gameObject.rectTransform().anchorMax = new Vector2(x_position, 1f);
		base.gameObject.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)this.format.width);
		for (int i = 0; i < values.Length; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.prefab_segment, this.segments_container, true);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.preferredHeight = (float)values[i];
			component.minWidth = (float)this.format.width;
			gameObject.GetComponent<Image>().color = this.format.colors[i % this.format.colors.Length];
			this.segments.Add(gameObject);
		}
	}

	// Token: 0x06009AA4 RID: 39588 RVA: 0x003BA9E8 File Offset: 0x003B8BE8
	public void ClearValues()
	{
		foreach (GameObject obj in this.segments)
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		this.segments.Clear();
	}

	// Token: 0x040078CE RID: 30926
	public GameObject segments_container;

	// Token: 0x040078CF RID: 30927
	public GameObject prefab_segment;

	// Token: 0x040078D0 RID: 30928
	private List<GameObject> segments = new List<GameObject>();

	// Token: 0x040078D1 RID: 30929
	private GraphedBarFormatting format;
}
