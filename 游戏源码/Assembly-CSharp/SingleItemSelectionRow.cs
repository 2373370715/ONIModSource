using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class SingleItemSelectionRow : KMonoBehaviour
{
	[SerializeField]
	protected Image icon;

	[SerializeField]
	protected LocText labelText;

	[SerializeField]
	protected Image BG;

	[SerializeField]
	protected Image outline;

	[SerializeField]
	protected Color outlineHighLightColor = new Color32(168, 74, 121, byte.MaxValue);

	[SerializeField]
	protected Color BGHighLightColor = new Color32(168, 74, 121, 80);

	[SerializeField]
	protected Color outlineDefaultColor = new Color32(204, 204, 204, byte.MaxValue);

	protected Color regularColor = Color.white;

	[SerializeField]
	public KButton button;

	public Action<SingleItemSelectionRow> Clicked;

	public virtual string InvalidTagTitle => UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.NO_SELECTION;

	public Tag InvalidTag { get; protected set; } = GameTags.Void;


	public new Tag tag { get; protected set; }

	public bool IsVisible => base.gameObject.activeSelf;

	public bool IsSelected { get; protected set; }

	protected override void OnPrefabInit()
	{
		regularColor = outline.color;
		base.OnPrefabInit();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!(button != null))
		{
			return;
		}
		button.onPointerEnter += delegate
		{
			if (!IsSelected)
			{
				outline.color = outlineHighLightColor;
			}
		};
		button.onPointerExit += delegate
		{
			if (!IsSelected)
			{
				outline.color = regularColor;
			}
		};
		button.onClick += OnItemClicked;
	}

	public virtual void SetVisibleState(bool isVisible)
	{
		base.gameObject.SetActive(isVisible);
	}

	protected virtual void OnItemClicked()
	{
		Clicked?.Invoke(this);
	}

	public virtual void SetTag(Tag tag)
	{
		this.tag = tag;
		SetText((tag == InvalidTag) ? InvalidTagTitle : tag.ProperName());
		if (tag != InvalidTag)
		{
			Tuple<Sprite, Color> uISprite = Def.GetUISprite(tag);
			SetIcon(uISprite.first, uISprite.second);
		}
		else
		{
			SetIcon(null, Color.white);
		}
	}

	protected virtual void SetText(string assignmentStr)
	{
		labelText.text = ((!string.IsNullOrEmpty(assignmentStr)) ? assignmentStr : "-");
	}

	public virtual void SetSelected(bool selected)
	{
		IsSelected = selected;
		outline.color = (selected ? outlineHighLightColor : outlineDefaultColor);
		BG.color = (selected ? BGHighLightColor : Color.white);
	}

	protected virtual void SetIcon(Sprite sprite, Color color)
	{
		icon.sprite = sprite;
		icon.color = color;
		icon.gameObject.SetActive(sprite != null);
	}
}
