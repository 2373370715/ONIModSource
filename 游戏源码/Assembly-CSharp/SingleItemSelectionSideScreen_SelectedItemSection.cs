using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001FD7 RID: 8151
public class SingleItemSelectionSideScreen_SelectedItemSection : KMonoBehaviour
{
	// Token: 0x17000B12 RID: 2834
	// (get) Token: 0x0600ACB0 RID: 44208 RVA: 0x00110555 File Offset: 0x0010E755
	// (set) Token: 0x0600ACAF RID: 44207 RVA: 0x0011054C File Offset: 0x0010E74C
	public Tag Item { get; private set; }

	// Token: 0x0600ACB1 RID: 44209 RVA: 0x0011055D File Offset: 0x0010E75D
	public void Clear()
	{
		this.SetItem(null);
	}

	// Token: 0x0600ACB2 RID: 44210 RVA: 0x0040F160 File Offset: 0x0040D360
	public void SetItem(Tag item)
	{
		this.Item = item;
		if (this.Item != GameTags.Void)
		{
			this.SetTitleText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.TITLE);
			this.SetContentText(this.Item.ProperName());
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(this.Item, "ui", false);
			this.SetImage(uisprite.first, uisprite.second);
			return;
		}
		this.SetTitleText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_TITLE);
		this.SetContentText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_MESSAGE);
		this.SetImage(null, Color.white);
	}

	// Token: 0x0600ACB3 RID: 44211 RVA: 0x0011056B File Offset: 0x0010E76B
	private void SetTitleText(string text)
	{
		this.title.text = text;
	}

	// Token: 0x0600ACB4 RID: 44212 RVA: 0x00110579 File Offset: 0x0010E779
	private void SetContentText(string text)
	{
		this.contentText.text = text;
	}

	// Token: 0x0600ACB5 RID: 44213 RVA: 0x00110587 File Offset: 0x0010E787
	private void SetImage(Sprite sprite, Color color)
	{
		this.image.sprite = sprite;
		this.image.color = color;
		this.image.gameObject.SetActive(sprite != null);
	}

	// Token: 0x04008792 RID: 34706
	[Header("References")]
	[SerializeField]
	private LocText title;

	// Token: 0x04008793 RID: 34707
	[SerializeField]
	private LocText contentText;

	// Token: 0x04008794 RID: 34708
	[SerializeField]
	private KImage image;
}
