using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/FilterSideScreenRow")]
public class FilterSideScreenRow : SingleItemSelectionRow
{
	public override string InvalidTagTitle => UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION;

	protected override void SetIcon(Sprite sprite, Color color)
	{
		if (icon != null)
		{
			icon.gameObject.SetActive(value: false);
		}
	}
}
