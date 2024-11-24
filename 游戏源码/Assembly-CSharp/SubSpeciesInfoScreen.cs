using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002023 RID: 8227
public class SubSpeciesInfoScreen : KModalScreen
{
	// Token: 0x0600AF1E RID: 44830 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x0600AF1F RID: 44831 RVA: 0x0010197B File Offset: 0x000FFB7B
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x0600AF20 RID: 44832 RVA: 0x0041EA44 File Offset: 0x0041CC44
	private void ClearMutations()
	{
		for (int i = this.mutationLineItems.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.mutationLineItems[i]);
		}
		this.mutationLineItems.Clear();
	}

	// Token: 0x0600AF21 RID: 44833 RVA: 0x00111DA5 File Offset: 0x0010FFA5
	public void DisplayDiscovery(Tag speciesID, Tag subSpeciesID, GeneticAnalysisStation station)
	{
		this.SetSubspecies(speciesID, subSpeciesID);
		this.targetStation = station;
	}

	// Token: 0x0600AF22 RID: 44834 RVA: 0x0041EA88 File Offset: 0x0041CC88
	private void SetSubspecies(Tag speciesID, Tag subSpeciesID)
	{
		this.ClearMutations();
		ref PlantSubSpeciesCatalog.SubSpeciesInfo subSpecies = PlantSubSpeciesCatalog.Instance.GetSubSpecies(speciesID, subSpeciesID);
		this.plantIcon.sprite = Def.GetUISprite(Assets.GetPrefab(speciesID), "ui", false).first;
		foreach (string id in subSpecies.mutationIDs)
		{
			PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
			GameObject gameObject = Util.KInstantiateUI(this.mutationsItemPrefab, this.mutationsList.gameObject, true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("nameLabel").text = plantMutation.Name;
			component.GetReference<LocText>("descriptionLabel").text = plantMutation.description;
			this.mutationLineItems.Add(gameObject);
		}
	}

	// Token: 0x040089E4 RID: 35300
	[SerializeField]
	private KButton renameButton;

	// Token: 0x040089E5 RID: 35301
	[SerializeField]
	private KButton saveButton;

	// Token: 0x040089E6 RID: 35302
	[SerializeField]
	private KButton discardButton;

	// Token: 0x040089E7 RID: 35303
	[SerializeField]
	private RectTransform mutationsList;

	// Token: 0x040089E8 RID: 35304
	[SerializeField]
	private Image plantIcon;

	// Token: 0x040089E9 RID: 35305
	[SerializeField]
	private GameObject mutationsItemPrefab;

	// Token: 0x040089EA RID: 35306
	private List<GameObject> mutationLineItems = new List<GameObject>();

	// Token: 0x040089EB RID: 35307
	private GeneticAnalysisStation targetStation;
}
