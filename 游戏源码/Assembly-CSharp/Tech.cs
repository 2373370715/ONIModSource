using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x020017AA RID: 6058
public class Tech : Resource
{
	// Token: 0x170007E7 RID: 2023
	// (get) Token: 0x06007CB7 RID: 31927 RVA: 0x000F2164 File Offset: 0x000F0364
	public bool FoundNode
	{
		get
		{
			return this.node != null;
		}
	}

	// Token: 0x170007E8 RID: 2024
	// (get) Token: 0x06007CB8 RID: 31928 RVA: 0x000F216F File Offset: 0x000F036F
	public Vector2 center
	{
		get
		{
			return this.node.center;
		}
	}

	// Token: 0x170007E9 RID: 2025
	// (get) Token: 0x06007CB9 RID: 31929 RVA: 0x000F217C File Offset: 0x000F037C
	public float width
	{
		get
		{
			return this.node.width;
		}
	}

	// Token: 0x170007EA RID: 2026
	// (get) Token: 0x06007CBA RID: 31930 RVA: 0x000F2189 File Offset: 0x000F0389
	public float height
	{
		get
		{
			return this.node.height;
		}
	}

	// Token: 0x170007EB RID: 2027
	// (get) Token: 0x06007CBB RID: 31931 RVA: 0x000F2196 File Offset: 0x000F0396
	public List<ResourceTreeNode.Edge> edges
	{
		get
		{
			return this.node.edges;
		}
	}

	// Token: 0x06007CBC RID: 31932 RVA: 0x00322AC0 File Offset: 0x00320CC0
	public Tech(string id, List<string> unlockedItemIDs, Techs techs, Dictionary<string, float> overrideDefaultCosts = null) : base(id, techs, Strings.Get("STRINGS.RESEARCH.TECHS." + id.ToUpper() + ".NAME"))
	{
		this.desc = Strings.Get("STRINGS.RESEARCH.TECHS." + id.ToUpper() + ".DESC");
		this.unlockedItemIDs = unlockedItemIDs;
		if (overrideDefaultCosts != null && DlcManager.IsExpansion1Active())
		{
			foreach (KeyValuePair<string, float> keyValuePair in overrideDefaultCosts)
			{
				this.costsByResearchTypeID.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}

	// Token: 0x06007CBD RID: 31933 RVA: 0x00322BB8 File Offset: 0x00320DB8
	public void AddUnlockedItemIDs(params string[] ids)
	{
		foreach (string item in ids)
		{
			this.unlockedItemIDs.Add(item);
		}
	}

	// Token: 0x06007CBE RID: 31934 RVA: 0x00322BE8 File Offset: 0x00320DE8
	public void RemoveUnlockedItemIDs(params string[] ids)
	{
		foreach (string text in ids)
		{
			if (!this.unlockedItemIDs.Remove(text))
			{
				DebugUtil.DevLogError("Tech item '" + text + "' does not exist to remove");
			}
		}
	}

	// Token: 0x06007CBF RID: 31935 RVA: 0x000F21A3 File Offset: 0x000F03A3
	public bool RequiresResearchType(string type)
	{
		return this.costsByResearchTypeID.ContainsKey(type) && this.costsByResearchTypeID[type] > 0f;
	}

	// Token: 0x06007CC0 RID: 31936 RVA: 0x000F21C8 File Offset: 0x000F03C8
	public void SetNode(ResourceTreeNode node, string categoryID)
	{
		this.node = node;
		this.category = categoryID;
	}

	// Token: 0x06007CC1 RID: 31937 RVA: 0x00322C2C File Offset: 0x00320E2C
	public bool CanAfford(ResearchPointInventory pointInventory)
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
		{
			if (pointInventory.PointsByTypeID[keyValuePair.Key] < keyValuePair.Value)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06007CC2 RID: 31938 RVA: 0x00322C9C File Offset: 0x00320E9C
	public string CostString(ResearchTypes types)
	{
		string text = "";
		foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
		{
			text += string.Format("{0}:{1}", types.GetResearchType(keyValuePair.Key).name.ToString(), keyValuePair.Value.ToString());
			text += "\n";
		}
		return text;
	}

	// Token: 0x06007CC3 RID: 31939 RVA: 0x00322D34 File Offset: 0x00320F34
	public bool IsComplete()
	{
		if (Research.Instance != null)
		{
			TechInstance techInstance = Research.Instance.Get(this);
			return techInstance != null && techInstance.IsComplete();
		}
		return false;
	}

	// Token: 0x06007CC4 RID: 31940 RVA: 0x00322D68 File Offset: 0x00320F68
	public bool ArePrerequisitesComplete()
	{
		using (List<Tech>.Enumerator enumerator = this.requiredTech.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsComplete())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x04005E63 RID: 24163
	public List<Tech> requiredTech = new List<Tech>();

	// Token: 0x04005E64 RID: 24164
	public List<Tech> unlockedTech = new List<Tech>();

	// Token: 0x04005E65 RID: 24165
	public List<TechItem> unlockedItems = new List<TechItem>();

	// Token: 0x04005E66 RID: 24166
	public List<string> unlockedItemIDs = new List<string>();

	// Token: 0x04005E67 RID: 24167
	public int tier;

	// Token: 0x04005E68 RID: 24168
	public Dictionary<string, float> costsByResearchTypeID = new Dictionary<string, float>();

	// Token: 0x04005E69 RID: 24169
	public string desc;

	// Token: 0x04005E6A RID: 24170
	public string category;

	// Token: 0x04005E6B RID: 24171
	public Tag[] tags;

	// Token: 0x04005E6C RID: 24172
	private ResourceTreeNode node;
}
