﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/GraphedBar")]
[Serializable]
public class GraphedBar : KMonoBehaviour
{
		public void SetFormat(GraphedBarFormatting format)
	{
		this.format = format;
	}

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

		public void ClearValues()
	{
		foreach (GameObject obj in this.segments)
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		this.segments.Clear();
	}

		public GameObject segments_container;

		public GameObject prefab_segment;

		private List<GameObject> segments = new List<GameObject>();

		private GraphedBarFormatting format;
}
