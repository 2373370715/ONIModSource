using System.Collections.Generic;
using KSerialization;
using UnityEngine;

public class FlatTagFilterable : KMonoBehaviour
{
	[Serialize]
	public List<Tag> selectedTags = new List<Tag>();

	public List<Tag> tagOptions = new List<Tag>();

	public string headerText;

	public bool displayOnlyDiscoveredTags = true;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		TreeFilterable component = GetComponent<TreeFilterable>();
		component.filterByStorageCategoriesOnSpawn = false;
		component.UpdateFilters(new HashSet<Tag>(selectedTags));
		Subscribe(-905833192, OnCopySettings);
	}

	public void SelectTag(Tag tag, bool state)
	{
		Debug.Assert(tagOptions.Contains(tag), "The tag " + tag.Name + " is not valid for this filterable - it must be added to tagOptions");
		if (state)
		{
			if (!selectedTags.Contains(tag))
			{
				selectedTags.Add(tag);
			}
		}
		else if (selectedTags.Contains(tag))
		{
			selectedTags.Remove(tag);
		}
		GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(selectedTags));
	}

	public void ToggleTag(Tag tag)
	{
		SelectTag(tag, !selectedTags.Contains(tag));
	}

	public string GetHeaderText()
	{
		return headerText;
	}

	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (GetComponent<KPrefabID>().PrefabID() != gameObject.GetComponent<KPrefabID>().PrefabID())
		{
			return;
		}
		selectedTags.Clear();
		foreach (Tag selectedTag in gameObject.GetComponent<FlatTagFilterable>().selectedTags)
		{
			SelectTag(selectedTag, state: true);
		}
		GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(selectedTags));
	}
}
