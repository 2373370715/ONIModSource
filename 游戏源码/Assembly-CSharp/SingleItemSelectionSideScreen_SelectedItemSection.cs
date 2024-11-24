using STRINGS;
using UnityEngine;

public class SingleItemSelectionSideScreen_SelectedItemSection : KMonoBehaviour
{
	[Header("References")]
	[SerializeField]
	private LocText title;

	[SerializeField]
	private LocText contentText;

	[SerializeField]
	private KImage image;

	public Tag Item { get; private set; }

	public void Clear()
	{
		SetItem(null);
	}

	public void SetItem(Tag item)
	{
		Item = item;
		if (Item != GameTags.Void)
		{
			SetTitleText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.TITLE);
			SetContentText(Item.ProperName());
			Tuple<Sprite, Color> uISprite = Def.GetUISprite(Item);
			SetImage(uISprite.first, uISprite.second);
		}
		else
		{
			SetTitleText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_TITLE);
			SetContentText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_MESSAGE);
			SetImage(null, Color.white);
		}
	}

	private void SetTitleText(string text)
	{
		title.text = text;
	}

	private void SetContentText(string text)
	{
		contentText.text = text;
	}

	private void SetImage(Sprite sprite, Color color)
	{
		image.sprite = sprite;
		image.color = color;
		image.gameObject.SetActive(sprite != null);
	}
}
