using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001FCF RID: 8143
public class SingleItemSelectionRow : KMonoBehaviour
{
	// Token: 0x17000B06 RID: 2822
	// (get) Token: 0x0600AC5F RID: 44127 RVA: 0x001100B2 File Offset: 0x0010E2B2
	public virtual string InvalidTagTitle
	{
		get
		{
			return UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.NO_SELECTION;
		}
	}

	// Token: 0x17000B07 RID: 2823
	// (get) Token: 0x0600AC60 RID: 44128 RVA: 0x001100BE File Offset: 0x0010E2BE
	// (set) Token: 0x0600AC61 RID: 44129 RVA: 0x001100C6 File Offset: 0x0010E2C6
	public Tag InvalidTag { get; protected set; } = GameTags.Void;

	// Token: 0x17000B08 RID: 2824
	// (get) Token: 0x0600AC62 RID: 44130 RVA: 0x001100CF File Offset: 0x0010E2CF
	// (set) Token: 0x0600AC63 RID: 44131 RVA: 0x001100D7 File Offset: 0x0010E2D7
	public new Tag tag { get; protected set; }

	// Token: 0x17000B09 RID: 2825
	// (get) Token: 0x0600AC64 RID: 44132 RVA: 0x001100E0 File Offset: 0x0010E2E0
	public bool IsVisible
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	// Token: 0x17000B0A RID: 2826
	// (get) Token: 0x0600AC65 RID: 44133 RVA: 0x001100ED File Offset: 0x0010E2ED
	// (set) Token: 0x0600AC66 RID: 44134 RVA: 0x001100F5 File Offset: 0x0010E2F5
	public bool IsSelected { get; protected set; }

	// Token: 0x0600AC67 RID: 44135 RVA: 0x001100FE File Offset: 0x0010E2FE
	protected override void OnPrefabInit()
	{
		this.regularColor = this.outline.color;
		base.OnPrefabInit();
	}

	// Token: 0x0600AC68 RID: 44136 RVA: 0x0040E5DC File Offset: 0x0040C7DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.button != null)
		{
			this.button.onPointerEnter += delegate()
			{
				if (!this.IsSelected)
				{
					this.outline.color = this.outlineHighLightColor;
				}
			};
			this.button.onPointerExit += delegate()
			{
				if (!this.IsSelected)
				{
					this.outline.color = this.regularColor;
				}
			};
			this.button.onClick += this.OnItemClicked;
		}
	}

	// Token: 0x0600AC69 RID: 44137 RVA: 0x00108B3C File Offset: 0x00106D3C
	public virtual void SetVisibleState(bool isVisible)
	{
		base.gameObject.SetActive(isVisible);
	}

	// Token: 0x0600AC6A RID: 44138 RVA: 0x00110117 File Offset: 0x0010E317
	protected virtual void OnItemClicked()
	{
		Action<SingleItemSelectionRow> clicked = this.Clicked;
		if (clicked == null)
		{
			return;
		}
		clicked(this);
	}

	// Token: 0x0600AC6B RID: 44139 RVA: 0x0040E644 File Offset: 0x0040C844
	public virtual void SetTag(Tag tag)
	{
		this.tag = tag;
		this.SetText((tag == this.InvalidTag) ? this.InvalidTagTitle : tag.ProperName());
		if (tag != this.InvalidTag)
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(tag, "ui", false);
			this.SetIcon(uisprite.first, uisprite.second);
			return;
		}
		this.SetIcon(null, Color.white);
	}

	// Token: 0x0600AC6C RID: 44140 RVA: 0x0011012A File Offset: 0x0010E32A
	protected virtual void SetText(string assignmentStr)
	{
		this.labelText.text = ((!string.IsNullOrEmpty(assignmentStr)) ? assignmentStr : "-");
	}

	// Token: 0x0600AC6D RID: 44141 RVA: 0x00110147 File Offset: 0x0010E347
	public virtual void SetSelected(bool selected)
	{
		this.IsSelected = selected;
		this.outline.color = (selected ? this.outlineHighLightColor : this.outlineDefaultColor);
		this.BG.color = (selected ? this.BGHighLightColor : Color.white);
	}

	// Token: 0x0600AC6E RID: 44142 RVA: 0x00110187 File Offset: 0x0010E387
	protected virtual void SetIcon(Sprite sprite, Color color)
	{
		this.icon.sprite = sprite;
		this.icon.color = color;
		this.icon.gameObject.SetActive(sprite != null);
	}

	// Token: 0x0400876D RID: 34669
	[SerializeField]
	protected Image icon;

	// Token: 0x0400876E RID: 34670
	[SerializeField]
	protected LocText labelText;

	// Token: 0x0400876F RID: 34671
	[SerializeField]
	protected Image BG;

	// Token: 0x04008770 RID: 34672
	[SerializeField]
	protected Image outline;

	// Token: 0x04008771 RID: 34673
	[SerializeField]
	protected Color outlineHighLightColor = new Color32(168, 74, 121, byte.MaxValue);

	// Token: 0x04008772 RID: 34674
	[SerializeField]
	protected Color BGHighLightColor = new Color32(168, 74, 121, 80);

	// Token: 0x04008773 RID: 34675
	[SerializeField]
	protected Color outlineDefaultColor = new Color32(204, 204, 204, byte.MaxValue);

	// Token: 0x04008774 RID: 34676
	protected Color regularColor = Color.white;

	// Token: 0x04008775 RID: 34677
	[SerializeField]
	public KButton button;

	// Token: 0x04008779 RID: 34681
	public Action<SingleItemSelectionRow> Clicked;
}
