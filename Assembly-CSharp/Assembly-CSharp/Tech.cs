using System.Collections.Generic;
using Database;
using UnityEngine;

public class Tech : Resource {
    public  string                    category;
    public  Dictionary<string, float> costsByResearchTypeID = new Dictionary<string, float>();
    public  string                    desc;
    private ResourceTreeNode          node;
    public  List<Tech>                requiredTech = new List<Tech>();
    public  Tag[]                     tags;
    public  int                       tier;
    public  List<string>              unlockedItemIDs = new List<string>();
    public  List<TechItem>            unlockedItems   = new List<TechItem>();
    public  List<Tech>                unlockedTech    = new List<Tech>();

    public Tech(string                    id,
                List<string>              unlockedItemIDs,
                Techs                     techs,
                Dictionary<string, float> overrideDefaultCosts = null) : base(id,
                                                                              techs,
                                                                              Strings.Get("STRINGS.RESEARCH.TECHS." +
                                                                                  id.ToUpper()                      +
                                                                                  ".NAME")) {
        desc                 = Strings.Get("STRINGS.RESEARCH.TECHS." + id.ToUpper() + ".DESC");
        this.unlockedItemIDs = unlockedItemIDs;
        if (overrideDefaultCosts != null && DlcManager.IsExpansion1Active())
            foreach (var keyValuePair in overrideDefaultCosts)
                costsByResearchTypeID.Add(keyValuePair.Key, keyValuePair.Value);
    }

    public bool                        FoundNode => node != null;
    public Vector2                     center    => node.center;
    public float                       width     => node.width;
    public float                       height    => node.height;
    public List<ResourceTreeNode.Edge> edges     => node.edges;

    public void AddUnlockedItemIDs(params string[] ids) {
        foreach (var item in ids) unlockedItemIDs.Add(item);
    }

    public void RemoveUnlockedItemIDs(params string[] ids) {
        foreach (var text in ids)
            if (!unlockedItemIDs.Remove(text))
                DebugUtil.DevLogError("Tech item '" + text + "' does not exist to remove");
    }

    public bool RequiresResearchType(string type) {
        return costsByResearchTypeID.ContainsKey(type) && costsByResearchTypeID[type] > 0f;
    }

    public void SetNode(ResourceTreeNode node, string categoryID) {
        this.node = node;
        category  = categoryID;
    }

    public bool CanAfford(ResearchPointInventory pointInventory) {
        foreach (var keyValuePair in costsByResearchTypeID)
            if (pointInventory.PointsByTypeID[keyValuePair.Key] < keyValuePair.Value)
                return false;

        return true;
    }

    public string CostString(ResearchTypes types) {
        var text = "";
        foreach (var keyValuePair in costsByResearchTypeID) {
            text += string.Format("{0}:{1}",
                                  types.GetResearchType(keyValuePair.Key).name,
                                  keyValuePair.Value.ToString());

            text += "\n";
        }

        return text;
    }

    public bool IsComplete() {
        if (Research.Instance != null) {
            var techInstance = Research.Instance.Get(this);
            return techInstance != null && techInstance.IsComplete();
        }

        return false;
    }

    public bool ArePrerequisitesComplete() {
        using (var enumerator = requiredTech.GetEnumerator()) {
            while (enumerator.MoveNext())
                if (!enumerator.Current.IsComplete())
                    return false;
        }

        return true;
    }
}