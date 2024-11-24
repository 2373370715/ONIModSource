using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000D74 RID: 3444
public class FlatTagFilterable : KMonoBehaviour
{
	// Token: 0x06004378 RID: 17272 RVA: 0x000CB98A File Offset: 0x000C9B8A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		TreeFilterable component = base.GetComponent<TreeFilterable>();
		component.filterByStorageCategoriesOnSpawn = false;
		component.UpdateFilters(new HashSet<Tag>(this.selectedTags));
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
	}

	// Token: 0x06004379 RID: 17273 RVA: 0x002450BC File Offset: 0x002432BC
	public void SelectTag(Tag tag, bool state)
	{
		global::Debug.Assert(this.tagOptions.Contains(tag), "The tag " + tag.Name + " is not valid for this filterable - it must be added to tagOptions");
		if (state)
		{
			if (!this.selectedTags.Contains(tag))
			{
				this.selectedTags.Add(tag);
			}
		}
		else if (this.selectedTags.Contains(tag))
		{
			this.selectedTags.Remove(tag);
		}
		base.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(this.selectedTags));
	}

	// Token: 0x0600437A RID: 17274 RVA: 0x000CB9C7 File Offset: 0x000C9BC7
	public void ToggleTag(Tag tag)
	{
		this.SelectTag(tag, !this.selectedTags.Contains(tag));
	}

	// Token: 0x0600437B RID: 17275 RVA: 0x000CB9DF File Offset: 0x000C9BDF
	public string GetHeaderText()
	{
		return this.headerText;
	}

	// Token: 0x0600437C RID: 17276 RVA: 0x00245140 File Offset: 0x00243340
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (base.GetComponent<KPrefabID>().PrefabID() != gameObject.GetComponent<KPrefabID>().PrefabID())
		{
			return;
		}
		this.selectedTags.Clear();
		foreach (Tag tag in gameObject.GetComponent<FlatTagFilterable>().selectedTags)
		{
			this.SelectTag(tag, true);
		}
		base.GetComponent<TreeFilterable>().UpdateFilters(new HashSet<Tag>(this.selectedTags));
	}

	// Token: 0x04002E35 RID: 11829
	[Serialize]
	public List<Tag> selectedTags = new List<Tag>();

	// Token: 0x04002E36 RID: 11830
	public List<Tag> tagOptions = new List<Tag>();

	// Token: 0x04002E37 RID: 11831
	public string headerText;

	// Token: 0x04002E38 RID: 11832
	public bool displayOnlyDiscoveredTags = true;
}
