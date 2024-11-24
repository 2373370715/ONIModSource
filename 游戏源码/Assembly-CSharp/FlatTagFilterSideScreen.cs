using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F6C RID: 8044
public class FlatTagFilterSideScreen : SideScreenContent
{
	// Token: 0x0600A9BF RID: 43455 RVA: 0x0010E44A File Offset: 0x0010C64A
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<FlatTagFilterable>() != null;
	}

	// Token: 0x0600A9C0 RID: 43456 RVA: 0x0010E458 File Offset: 0x0010C658
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.tagFilterable = target.GetComponent<FlatTagFilterable>();
		this.Build();
	}

	// Token: 0x0600A9C1 RID: 43457 RVA: 0x0040271C File Offset: 0x0040091C
	private void Build()
	{
		this.headerLabel.SetText(this.tagFilterable.GetHeaderText());
		foreach (KeyValuePair<Tag, GameObject> keyValuePair in this.rows)
		{
			Util.KDestroyGameObject(keyValuePair.Value);
		}
		this.rows.Clear();
		foreach (Tag tag in this.tagFilterable.tagOptions)
		{
			GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.listContainer, false);
			gameObject.gameObject.name = tag.ProperName();
			this.rows.Add(tag, gameObject);
		}
		this.Refresh();
	}

	// Token: 0x0600A9C2 RID: 43458 RVA: 0x00402810 File Offset: 0x00400A10
	private void Refresh()
	{
		using (Dictionary<Tag, GameObject>.Enumerator enumerator = this.rows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<Tag, GameObject> kvp = enumerator.Current;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(kvp.Key.ProperNameStripLink());
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").sprite = Def.GetUISprite(kvp.Key, "ui", false).first;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<Image>("Icon").color = Def.GetUISprite(kvp.Key, "ui", false).second;
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.tagFilterable.ToggleTag(kvp.Key);
					this.Refresh();
				};
				kvp.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.tagFilterable.selectedTags.Contains(kvp.Key) ? 1 : 0);
				kvp.Value.SetActive(!this.tagFilterable.displayOnlyDiscoveredTags || DiscoveredResources.Instance.IsDiscovered(kvp.Key));
			}
		}
	}

	// Token: 0x0600A9C3 RID: 43459 RVA: 0x0010E473 File Offset: 0x0010C673
	public override string GetTitle()
	{
		return this.tagFilterable.gameObject.GetProperName();
	}

	// Token: 0x0400857D RID: 34173
	private FlatTagFilterable tagFilterable;

	// Token: 0x0400857E RID: 34174
	[SerializeField]
	private GameObject rowPrefab;

	// Token: 0x0400857F RID: 34175
	[SerializeField]
	private GameObject listContainer;

	// Token: 0x04008580 RID: 34176
	[SerializeField]
	private LocText headerLabel;

	// Token: 0x04008581 RID: 34177
	private Dictionary<Tag, GameObject> rows = new Dictionary<Tag, GameObject>();
}
