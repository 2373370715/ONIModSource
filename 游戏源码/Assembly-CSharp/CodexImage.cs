using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C50 RID: 7248
public class CodexImage : CodexWidget<CodexImage>
{
	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x0600971B RID: 38683 RVA: 0x0010239B File Offset: 0x0010059B
	// (set) Token: 0x0600971C RID: 38684 RVA: 0x001023A3 File Offset: 0x001005A3
	public Sprite sprite { get; set; }

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x0600971D RID: 38685 RVA: 0x001023AC File Offset: 0x001005AC
	// (set) Token: 0x0600971E RID: 38686 RVA: 0x001023B4 File Offset: 0x001005B4
	public Color color { get; set; }

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x06009720 RID: 38688 RVA: 0x001023D0 File Offset: 0x001005D0
	// (set) Token: 0x0600971F RID: 38687 RVA: 0x001023BD File Offset: 0x001005BD
	public string spriteName
	{
		get
		{
			return "--> " + ((this.sprite == null) ? "NULL" : this.sprite.ToString());
		}
		set
		{
			this.sprite = Assets.GetSprite(value);
		}
	}

	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x06009722 RID: 38690 RVA: 0x001023D0 File Offset: 0x001005D0
	// (set) Token: 0x06009721 RID: 38689 RVA: 0x003AA298 File Offset: 0x003A8498
	public string batchedAnimPrefabSourceID
	{
		get
		{
			return "--> " + ((this.sprite == null) ? "NULL" : this.sprite.ToString());
		}
		set
		{
			GameObject prefab = Assets.GetPrefab(value);
			KBatchedAnimController kbatchedAnimController = (prefab != null) ? prefab.GetComponent<KBatchedAnimController>() : null;
			KAnimFile kanimFile = (kbatchedAnimController != null) ? kbatchedAnimController.AnimFiles[0] : null;
			this.sprite = ((kanimFile != null) ? Def.GetUISpriteFromMultiObjectAnim(kanimFile, "ui", false, "") : null);
		}
	}

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x06009724 RID: 38692 RVA: 0x000CA99D File Offset: 0x000C8B9D
	// (set) Token: 0x06009723 RID: 38691 RVA: 0x003AA2FC File Offset: 0x003A84FC
	public string elementIcon
	{
		get
		{
			return "";
		}
		set
		{
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(value.ToTag(), "ui", false);
			this.sprite = uisprite.first;
			this.color = uisprite.second;
		}
	}

	// Token: 0x06009725 RID: 38693 RVA: 0x001023FC File Offset: 0x001005FC
	public CodexImage()
	{
		this.color = Color.white;
	}

	// Token: 0x06009726 RID: 38694 RVA: 0x0010240F File Offset: 0x0010060F
	public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite, Color color) : base(preferredWidth, preferredHeight)
	{
		this.sprite = sprite;
		this.color = color;
	}

	// Token: 0x06009727 RID: 38695 RVA: 0x00102428 File Offset: 0x00100628
	public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite) : this(preferredWidth, preferredHeight, sprite, Color.white)
	{
	}

	// Token: 0x06009728 RID: 38696 RVA: 0x00102438 File Offset: 0x00100638
	public CodexImage(int preferredWidth, int preferredHeight, global::Tuple<Sprite, Color> coloredSprite) : this(preferredWidth, preferredHeight, coloredSprite.first, coloredSprite.second)
	{
	}

	// Token: 0x06009729 RID: 38697 RVA: 0x0010244E File Offset: 0x0010064E
	public CodexImage(global::Tuple<Sprite, Color> coloredSprite) : this(-1, -1, coloredSprite)
	{
	}

	// Token: 0x0600972A RID: 38698 RVA: 0x00102459 File Offset: 0x00100659
	public void ConfigureImage(Image image)
	{
		image.sprite = this.sprite;
		image.color = this.color;
	}

	// Token: 0x0600972B RID: 38699 RVA: 0x00102473 File Offset: 0x00100673
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		this.ConfigureImage(contentGameObject.GetComponent<Image>());
		base.ConfigurePreferredLayout(contentGameObject);
	}
}
