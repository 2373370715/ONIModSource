using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class DispenserSideScreen : SideScreenContent
{
	[SerializeField]
	private KButton dispenseButton;

	[SerializeField]
	private RectTransform itemRowContainer;

	[SerializeField]
	private GameObject itemRowPrefab;

	private IDispenser targetDispenser;

	private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IDispenser>() != null;
	}

	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		targetDispenser = target.GetComponent<IDispenser>();
		Refresh();
	}

	private void Refresh()
	{
		dispenseButton.ClearOnClick();
		foreach (KeyValuePair<Tag, GameObject> row in rows)
		{
			Object.Destroy(row.Value);
		}
		rows.Clear();
		foreach (Tag item in targetDispenser.DispensedItems())
		{
			GameObject gameObject = Util.KInstantiateUI(itemRowPrefab, itemRowContainer.gameObject, force_active: true);
			rows.Add(item, gameObject);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<Image>("Icon").sprite = Def.GetUISprite(item).first;
			component.GetReference<LocText>("Label").text = Assets.GetPrefab(item).GetProperName();
			gameObject.GetComponent<MultiToggle>().ChangeState((!(item == targetDispenser.SelectedItem())) ? 1 : 0);
		}
		if (targetDispenser.HasOpenChore())
		{
			dispenseButton.onClick += delegate
			{
				targetDispenser.OnCancelDispense();
				Refresh();
			};
			dispenseButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_CANCEL;
		}
		else
		{
			dispenseButton.onClick += delegate
			{
				targetDispenser.OnOrderDispense();
				Refresh();
			};
			dispenseButton.GetComponentInChildren<LocText>().text = UI.UISIDESCREENS.DISPENSERSIDESCREEN.BUTTON_DISPENSE;
		}
		targetDispenser.OnStopWorkEvent -= Refresh;
		targetDispenser.OnStopWorkEvent += Refresh;
	}

	private void SelectTag(Tag tag)
	{
		targetDispenser.SelectItem(tag);
		Refresh();
	}
}
