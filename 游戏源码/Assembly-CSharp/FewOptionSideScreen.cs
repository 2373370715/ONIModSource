using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FewOptionSideScreen : SideScreenContent
{
	public interface IFewOptionSideScreen
	{
		public struct Option
		{
			public Tag tag;

			public string labelText;

			public string tooltipText;

			public Tuple<Sprite, Color> iconSpriteColorTuple;

			public Option(Tag tag, string labelText, Tuple<Sprite, Color> iconSpriteColorTuple, string tooltipText = "")
			{
				this.tag = tag;
				this.labelText = labelText;
				this.iconSpriteColorTuple = iconSpriteColorTuple;
				this.tooltipText = tooltipText;
			}
		}

		Option[] GetOptions();

		void OnOptionSelected(Option option);

		Tag GetSelectedOption();
	}

	public GameObject rowPrefab;

	public RectTransform rowContainer;

	public Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();

	private IFewOptionSideScreen targetFewOptions;

	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (show)
		{
			RefreshOptions();
		}
	}

	private void RefreshOptions()
	{
		foreach (KeyValuePair<Tag, GameObject> row in rows)
		{
			row.Value.GetComponent<MultiToggle>().ChangeState((row.Key == targetFewOptions.GetSelectedOption()) ? 1 : 0);
		}
	}

	private void ClearRows()
	{
		for (int num = rowContainer.childCount - 1; num >= 0; num--)
		{
			Util.KDestroyGameObject(rowContainer.GetChild(num));
		}
		rows.Clear();
	}

	private void SpawnRows()
	{
		IFewOptionSideScreen.Option[] options = targetFewOptions.GetOptions();
		for (int i = 0; i < options.Length; i++)
		{
			IFewOptionSideScreen.Option option = options[i];
			GameObject gameObject = Util.KInstantiateUI(rowPrefab, rowContainer.gameObject, force_active: true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("label").SetText(option.labelText);
			component.GetReference<Image>("icon").sprite = option.iconSpriteColorTuple.first;
			component.GetReference<Image>("icon").color = option.iconSpriteColorTuple.second;
			gameObject.GetComponent<ToolTip>().toolTip = option.tooltipText;
			gameObject.GetComponent<MultiToggle>().onClick = delegate
			{
				targetFewOptions.OnOptionSelected(option);
				RefreshOptions();
			};
			rows.Add(option.tag, gameObject);
		}
		RefreshOptions();
	}

	public override void SetTarget(GameObject target)
	{
		ClearRows();
		targetFewOptions = target.GetComponent<IFewOptionSideScreen>();
		SpawnRows();
	}

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IFewOptionSideScreen>() != null;
	}
}
