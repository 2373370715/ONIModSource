using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001ACE RID: 6862
public class HoverTextDrawer
{
	// Token: 0x06008FB4 RID: 36788 RVA: 0x00377C7C File Offset: 0x00375E7C
	public HoverTextDrawer(HoverTextDrawer.Skin skin, RectTransform parent)
	{
		this.shadowBars = new HoverTextDrawer.Pool<Image>(skin.shadowBarWidget.gameObject, parent);
		this.selectBorders = new HoverTextDrawer.Pool<Image>(skin.selectBorderWidget.gameObject, parent);
		this.textWidgets = new HoverTextDrawer.Pool<LocText>(skin.textWidget.gameObject, parent);
		this.iconWidgets = new HoverTextDrawer.Pool<Image>(skin.iconWidget.gameObject, parent);
		this.skin = skin;
	}

	// Token: 0x06008FB5 RID: 36789 RVA: 0x000FDEB7 File Offset: 0x000FC0B7
	public void SetEnabled(bool enabled)
	{
		this.shadowBars.SetEnabled(enabled);
		this.textWidgets.SetEnabled(enabled);
		this.iconWidgets.SetEnabled(enabled);
		this.selectBorders.SetEnabled(enabled);
	}

	// Token: 0x06008FB6 RID: 36790 RVA: 0x00377CF4 File Offset: 0x00375EF4
	public void BeginDrawing(Vector2 root_pos)
	{
		this.rootPos = root_pos + this.skin.baseOffset;
		if (this.skin.enableDebugOffset)
		{
			this.rootPos += this.skin.debugOffset;
		}
		this.currentPos = this.rootPos;
		this.textWidgets.BeginDrawing();
		this.iconWidgets.BeginDrawing();
		this.shadowBars.BeginDrawing();
		this.selectBorders.BeginDrawing();
		this.firstShadowBar = true;
		this.minLineHeight = 0;
	}

	// Token: 0x06008FB7 RID: 36791 RVA: 0x000FDEE9 File Offset: 0x000FC0E9
	public void EndDrawing()
	{
		this.shadowBars.EndDrawing();
		this.iconWidgets.EndDrawing();
		this.textWidgets.EndDrawing();
		this.selectBorders.EndDrawing();
	}

	// Token: 0x06008FB8 RID: 36792 RVA: 0x00377D88 File Offset: 0x00375F88
	public void DrawText(string text, TextStyleSetting style, Color color, bool override_color = true)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		LocText widget = this.textWidgets.Draw(this.currentPos).widget;
		Color color2 = Color.white;
		if (widget.textStyleSetting != style)
		{
			widget.textStyleSetting = style;
			widget.ApplySettings();
		}
		if (style != null)
		{
			color2 = style.textColor;
		}
		if (override_color)
		{
			color2 = color;
		}
		widget.color = color2;
		if (widget.text != text)
		{
			widget.text = text;
			widget.KForceUpdateDirty();
		}
		this.currentPos.x = this.currentPos.x + widget.renderedWidth;
		this.maxShadowX = Mathf.Max(this.currentPos.x, this.maxShadowX);
		this.minLineHeight = (int)Mathf.Max((float)this.minLineHeight, widget.renderedHeight);
	}

	// Token: 0x06008FB9 RID: 36793 RVA: 0x000FDF17 File Offset: 0x000FC117
	public void DrawText(string text, TextStyleSetting style)
	{
		this.DrawText(text, style, Color.white, false);
	}

	// Token: 0x06008FBA RID: 36794 RVA: 0x000FDF27 File Offset: 0x000FC127
	public void AddIndent(int width = 36)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.currentPos.x = this.currentPos.x + (float)width;
	}

	// Token: 0x06008FBB RID: 36795 RVA: 0x00377E60 File Offset: 0x00376060
	public void NewLine(int min_height = 26)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.currentPos.y = this.currentPos.y - (float)Math.Max(min_height, this.minLineHeight);
		this.currentPos.x = this.rootPos.x;
		this.minLineHeight = 0;
	}

	// Token: 0x06008FBC RID: 36796 RVA: 0x000FDF48 File Offset: 0x000FC148
	public void DrawIcon(Sprite icon, int min_width = 18)
	{
		this.DrawIcon(icon, Color.white, min_width, 2);
	}

	// Token: 0x06008FBD RID: 36797 RVA: 0x00377EB4 File Offset: 0x003760B4
	public void DrawIcon(Sprite icon, Color color, int image_size = 18, int horizontal_spacing = 2)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.AddIndent(horizontal_spacing);
		HoverTextDrawer.Pool<Image>.Entry entry = this.iconWidgets.Draw(this.currentPos + this.skin.shadowImageOffset);
		entry.widget.sprite = icon;
		entry.widget.color = this.skin.shadowImageColor;
		entry.rect.sizeDelta = new Vector2((float)image_size, (float)image_size);
		HoverTextDrawer.Pool<Image>.Entry entry2 = this.iconWidgets.Draw(this.currentPos);
		entry2.widget.sprite = icon;
		entry2.widget.color = color;
		entry2.rect.sizeDelta = new Vector2((float)image_size, (float)image_size);
		this.AddIndent(horizontal_spacing);
		this.currentPos.x = this.currentPos.x + (float)image_size;
		this.maxShadowX = Mathf.Max(this.currentPos.x, this.maxShadowX);
	}

	// Token: 0x06008FBE RID: 36798 RVA: 0x00377FA0 File Offset: 0x003761A0
	public void BeginShadowBar(bool selected = false)
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		if (this.firstShadowBar)
		{
			this.firstShadowBar = false;
		}
		else
		{
			this.NewLine(22);
		}
		this.isShadowBarSelected = selected;
		this.shadowStartPos = this.currentPos;
		this.maxShadowX = this.rootPos.x;
	}

	// Token: 0x06008FBF RID: 36799 RVA: 0x00377FF8 File Offset: 0x003761F8
	public void EndShadowBar()
	{
		if (!this.skin.drawWidgets)
		{
			return;
		}
		this.NewLine(22);
		HoverTextDrawer.Pool<Image>.Entry entry = this.shadowBars.Draw(this.currentPos);
		entry.rect.anchoredPosition = this.shadowStartPos + new Vector2(-this.skin.shadowBarBorder.x, this.skin.shadowBarBorder.y);
		entry.rect.sizeDelta = new Vector2(this.maxShadowX - this.rootPos.x + this.skin.shadowBarBorder.x * 2f, this.shadowStartPos.y - this.currentPos.y + this.skin.shadowBarBorder.y * 2f);
		if (this.isShadowBarSelected)
		{
			HoverTextDrawer.Pool<Image>.Entry entry2 = this.selectBorders.Draw(this.currentPos);
			entry2.rect.anchoredPosition = this.shadowStartPos + new Vector2(-this.skin.shadowBarBorder.x - this.skin.selectBorder.x, this.skin.shadowBarBorder.y + this.skin.selectBorder.y);
			entry2.rect.sizeDelta = new Vector2(this.maxShadowX - this.rootPos.x + this.skin.shadowBarBorder.x * 2f + this.skin.selectBorder.x * 2f, this.shadowStartPos.y - this.currentPos.y + this.skin.shadowBarBorder.y * 2f + this.skin.selectBorder.y * 2f);
		}
	}

	// Token: 0x06008FC0 RID: 36800 RVA: 0x000FDF58 File Offset: 0x000FC158
	public void Cleanup()
	{
		this.shadowBars.Cleanup();
		this.textWidgets.Cleanup();
		this.iconWidgets.Cleanup();
	}

	// Token: 0x04006C79 RID: 27769
	public HoverTextDrawer.Skin skin;

	// Token: 0x04006C7A RID: 27770
	private Vector2 currentPos;

	// Token: 0x04006C7B RID: 27771
	private Vector2 rootPos;

	// Token: 0x04006C7C RID: 27772
	private Vector2 shadowStartPos;

	// Token: 0x04006C7D RID: 27773
	private float maxShadowX;

	// Token: 0x04006C7E RID: 27774
	private bool firstShadowBar;

	// Token: 0x04006C7F RID: 27775
	private bool isShadowBarSelected;

	// Token: 0x04006C80 RID: 27776
	private int minLineHeight;

	// Token: 0x04006C81 RID: 27777
	private HoverTextDrawer.Pool<LocText> textWidgets;

	// Token: 0x04006C82 RID: 27778
	private HoverTextDrawer.Pool<Image> iconWidgets;

	// Token: 0x04006C83 RID: 27779
	private HoverTextDrawer.Pool<Image> shadowBars;

	// Token: 0x04006C84 RID: 27780
	private HoverTextDrawer.Pool<Image> selectBorders;

	// Token: 0x02001ACF RID: 6863
	[Serializable]
	public class Skin
	{
		// Token: 0x04006C85 RID: 27781
		public Vector2 baseOffset;

		// Token: 0x04006C86 RID: 27782
		public LocText textWidget;

		// Token: 0x04006C87 RID: 27783
		public Image iconWidget;

		// Token: 0x04006C88 RID: 27784
		public Vector2 shadowImageOffset;

		// Token: 0x04006C89 RID: 27785
		public Color shadowImageColor;

		// Token: 0x04006C8A RID: 27786
		public Image shadowBarWidget;

		// Token: 0x04006C8B RID: 27787
		public Image selectBorderWidget;

		// Token: 0x04006C8C RID: 27788
		public Vector2 shadowBarBorder;

		// Token: 0x04006C8D RID: 27789
		public Vector2 selectBorder;

		// Token: 0x04006C8E RID: 27790
		public bool drawWidgets;

		// Token: 0x04006C8F RID: 27791
		public bool enableProfiling;

		// Token: 0x04006C90 RID: 27792
		public bool enableDebugOffset;

		// Token: 0x04006C91 RID: 27793
		public bool drawInProgressHoverText;

		// Token: 0x04006C92 RID: 27794
		public Vector2 debugOffset;
	}

	// Token: 0x02001AD0 RID: 6864
	private class Pool<WidgetType> where WidgetType : MonoBehaviour
	{
		// Token: 0x06008FC2 RID: 36802 RVA: 0x003781DC File Offset: 0x003763DC
		public Pool(GameObject prefab, RectTransform master_root)
		{
			this.prefab = prefab;
			GameObject gameObject = new GameObject(typeof(WidgetType).Name);
			this.root = gameObject.AddComponent<RectTransform>();
			this.root.SetParent(master_root);
			this.root.anchoredPosition = Vector2.zero;
			this.root.anchorMin = Vector2.zero;
			this.root.anchorMax = Vector2.one;
			this.root.sizeDelta = Vector2.zero;
			gameObject.AddComponent<CanvasGroup>();
		}

		// Token: 0x06008FC3 RID: 36803 RVA: 0x00378278 File Offset: 0x00376478
		public HoverTextDrawer.Pool<WidgetType>.Entry Draw(Vector2 pos)
		{
			HoverTextDrawer.Pool<WidgetType>.Entry entry;
			if (this.drawnWidgets < this.entries.Count)
			{
				entry = this.entries[this.drawnWidgets];
				if (!entry.widget.gameObject.activeSelf)
				{
					entry.widget.gameObject.SetActive(true);
				}
			}
			else
			{
				GameObject gameObject = Util.KInstantiateUI(this.prefab, this.root.gameObject, false);
				gameObject.SetActive(true);
				entry.widget = gameObject.GetComponent<WidgetType>();
				entry.rect = gameObject.GetComponent<RectTransform>();
				this.entries.Add(entry);
			}
			entry.rect.anchoredPosition = new Vector2(pos.x, pos.y);
			this.drawnWidgets++;
			return entry;
		}

		// Token: 0x06008FC4 RID: 36804 RVA: 0x000FDF7B File Offset: 0x000FC17B
		public void BeginDrawing()
		{
			this.drawnWidgets = 0;
		}

		// Token: 0x06008FC5 RID: 36805 RVA: 0x0037834C File Offset: 0x0037654C
		public void EndDrawing()
		{
			for (int i = this.drawnWidgets; i < this.entries.Count; i++)
			{
				if (this.entries[i].widget.gameObject.activeSelf)
				{
					this.entries[i].widget.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06008FC6 RID: 36806 RVA: 0x000FDF84 File Offset: 0x000FC184
		public void SetEnabled(bool enabled)
		{
			if (enabled)
			{
				this.root.gameObject.GetComponent<CanvasGroup>().alpha = 1f;
				return;
			}
			this.root.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
		}

		// Token: 0x06008FC7 RID: 36807 RVA: 0x003783B8 File Offset: 0x003765B8
		public void Cleanup()
		{
			foreach (HoverTextDrawer.Pool<WidgetType>.Entry entry in this.entries)
			{
				UnityEngine.Object.Destroy(entry.widget.gameObject);
			}
			this.entries.Clear();
		}

		// Token: 0x04006C93 RID: 27795
		private GameObject prefab;

		// Token: 0x04006C94 RID: 27796
		private RectTransform root;

		// Token: 0x04006C95 RID: 27797
		private List<HoverTextDrawer.Pool<WidgetType>.Entry> entries = new List<HoverTextDrawer.Pool<WidgetType>.Entry>();

		// Token: 0x04006C96 RID: 27798
		private int drawnWidgets;

		// Token: 0x02001AD1 RID: 6865
		public struct Entry
		{
			// Token: 0x04006C97 RID: 27799
			public WidgetType widget;

			// Token: 0x04006C98 RID: 27800
			public RectTransform rect;
		}
	}
}
