using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020015B2 RID: 5554
[SerializationConfig(MemberSerialization.OptIn)]
public class MutantPlant : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x17000762 RID: 1890
	// (get) Token: 0x0600732E RID: 29486 RVA: 0x000EB86A File Offset: 0x000E9A6A
	public List<string> MutationIDs
	{
		get
		{
			return this.mutationIDs;
		}
	}

	// Token: 0x17000763 RID: 1891
	// (get) Token: 0x0600732F RID: 29487 RVA: 0x000EB872 File Offset: 0x000E9A72
	public bool IsOriginal
	{
		get
		{
			return this.mutationIDs == null || this.mutationIDs.Count == 0;
		}
	}

	// Token: 0x17000764 RID: 1892
	// (get) Token: 0x06007330 RID: 29488 RVA: 0x000EB88C File Offset: 0x000E9A8C
	public bool IsIdentified
	{
		get
		{
			return this.analyzed && PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.SubSpeciesID);
		}
	}

	// Token: 0x17000765 RID: 1893
	// (get) Token: 0x06007331 RID: 29489 RVA: 0x000EB8A8 File Offset: 0x000E9AA8
	// (set) Token: 0x06007332 RID: 29490 RVA: 0x000EB8CB File Offset: 0x000E9ACB
	public Tag SpeciesID
	{
		get
		{
			global::Debug.Assert(this.speciesID != null, "Ack, forgot to configure the species ID for this mutantPlant!");
			return this.speciesID;
		}
		set
		{
			this.speciesID = value;
		}
	}

	// Token: 0x17000766 RID: 1894
	// (get) Token: 0x06007333 RID: 29491 RVA: 0x000EB8D4 File Offset: 0x000E9AD4
	public Tag SubSpeciesID
	{
		get
		{
			if (this.cachedSubspeciesID == null)
			{
				this.cachedSubspeciesID = this.GetSubSpeciesInfo().ID;
			}
			return this.cachedSubspeciesID;
		}
	}

	// Token: 0x06007334 RID: 29492 RVA: 0x000EB900 File Offset: 0x000E9B00
	protected override void OnPrefabInit()
	{
		base.Subscribe<MutantPlant>(-2064133523, MutantPlant.OnAbsorbDelegate);
		base.Subscribe<MutantPlant>(1335436905, MutantPlant.OnSplitFromChunkDelegate);
	}

	// Token: 0x06007335 RID: 29493 RVA: 0x002FF83C File Offset: 0x002FDA3C
	protected override void OnSpawn()
	{
		if (this.IsOriginal || this.HasTag(GameTags.Plant))
		{
			this.analyzed = true;
		}
		if (!this.IsOriginal)
		{
			this.AddTag(GameTags.MutatedSeed);
		}
		this.AddTag(this.SubSpeciesID);
		Components.MutantPlants.Add(this);
		base.OnSpawn();
		this.ApplyMutations();
		this.UpdateNameAndTags();
		PlantSubSpeciesCatalog.Instance.DiscoverSubSpecies(this.GetSubSpeciesInfo(), this);
	}

	// Token: 0x06007336 RID: 29494 RVA: 0x000EB924 File Offset: 0x000E9B24
	protected override void OnCleanUp()
	{
		Components.MutantPlants.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06007337 RID: 29495 RVA: 0x002FF8B4 File Offset: 0x002FDAB4
	private void OnAbsorb(object data)
	{
		MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
		global::Debug.Assert(component != null && this.SubSpeciesID == component.SubSpeciesID, "Two seeds of different subspecies just absorbed!");
	}

	// Token: 0x06007338 RID: 29496 RVA: 0x002FF8F4 File Offset: 0x002FDAF4
	private void OnSplitFromChunk(object data)
	{
		MutantPlant component = (data as Pickupable).GetComponent<MutantPlant>();
		if (component != null)
		{
			component.CopyMutationsTo(this);
		}
	}

	// Token: 0x06007339 RID: 29497 RVA: 0x002FF920 File Offset: 0x002FDB20
	public void Mutate()
	{
		List<string> list = (this.mutationIDs != null) ? new List<string>(this.mutationIDs) : new List<string>();
		while (list.Count >= 1 && list.Count > 0)
		{
			list.RemoveAt(UnityEngine.Random.Range(0, list.Count));
		}
		list.Add(Db.Get().PlantMutations.GetRandomMutation(this.PrefabID().Name).Id);
		this.SetSubSpecies(list);
	}

	// Token: 0x0600733A RID: 29498 RVA: 0x000EB937 File Offset: 0x000E9B37
	public void Analyze()
	{
		this.analyzed = true;
		this.UpdateNameAndTags();
	}

	// Token: 0x0600733B RID: 29499 RVA: 0x002FF9A0 File Offset: 0x002FDBA0
	public void ApplyMutations()
	{
		if (this.IsOriginal)
		{
			return;
		}
		foreach (string id in this.mutationIDs)
		{
			Db.Get().PlantMutations.Get(id).ApplyTo(this);
		}
	}

	// Token: 0x0600733C RID: 29500 RVA: 0x000EB946 File Offset: 0x000E9B46
	public void DummySetSubspecies(List<string> mutations)
	{
		this.mutationIDs = mutations;
	}

	// Token: 0x0600733D RID: 29501 RVA: 0x000EB94F File Offset: 0x000E9B4F
	public void SetSubSpecies(List<string> mutations)
	{
		if (base.gameObject.HasTag(this.SubSpeciesID))
		{
			base.gameObject.RemoveTag(this.SubSpeciesID);
		}
		this.cachedSubspeciesID = Tag.Invalid;
		this.mutationIDs = mutations;
		this.UpdateNameAndTags();
	}

	// Token: 0x0600733E RID: 29502 RVA: 0x000EB98D File Offset: 0x000E9B8D
	public PlantSubSpeciesCatalog.SubSpeciesInfo GetSubSpeciesInfo()
	{
		return new PlantSubSpeciesCatalog.SubSpeciesInfo(this.SpeciesID, this.mutationIDs);
	}

	// Token: 0x0600733F RID: 29503 RVA: 0x000EB9A0 File Offset: 0x000E9BA0
	public void CopyMutationsTo(MutantPlant target)
	{
		target.SetSubSpecies(this.mutationIDs);
		target.analyzed = this.analyzed;
	}

	// Token: 0x06007340 RID: 29504 RVA: 0x002FFA0C File Offset: 0x002FDC0C
	public void UpdateNameAndTags()
	{
		bool flag = !base.IsInitialized() || this.IsIdentified;
		bool flag2 = PlantSubSpeciesCatalog.Instance == null || PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(this.SpeciesID).Count == 1;
		KPrefabID component = base.GetComponent<KPrefabID>();
		component.AddTag(this.SubSpeciesID, false);
		component.SetTag(GameTags.UnidentifiedSeed, !flag);
		base.gameObject.name = component.PrefabTag.ToString() + " (" + this.SubSpeciesID.ToString() + ")";
		base.GetComponent<KSelectable>().SetName(this.GetSubSpeciesInfo().GetNameWithMutations(component.PrefabTag.ProperName(), flag, flag2));
		KSelectable component2 = base.GetComponent<KSelectable>();
		foreach (Guid guid in this.statusItemHandles)
		{
			component2.RemoveStatusItem(guid, false);
		}
		this.statusItemHandles.Clear();
		if (!flag2)
		{
			if (this.IsOriginal)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.OriginalPlantMutation, null));
				return;
			}
			if (!flag)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.UnknownMutation, null));
				return;
			}
			foreach (string id in this.mutationIDs)
			{
				this.statusItemHandles.Add(component2.AddStatusItem(Db.Get().CreatureStatusItems.SpecificPlantMutation, Db.Get().PlantMutations.Get(id)));
			}
		}
	}

	// Token: 0x06007341 RID: 29505 RVA: 0x002FFC00 File Offset: 0x002FDE00
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		if (this.IsOriginal)
		{
			return null;
		}
		List<Descriptor> result = new List<Descriptor>();
		foreach (string id in this.mutationIDs)
		{
			Db.Get().PlantMutations.Get(id).GetDescriptors(ref result, go);
		}
		return result;
	}

	// Token: 0x06007342 RID: 29506 RVA: 0x002FFC78 File Offset: 0x002FDE78
	public List<string> GetSoundEvents()
	{
		List<string> list = new List<string>();
		if (this.mutationIDs != null)
		{
			foreach (string id in this.mutationIDs)
			{
				PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
				list.AddRange(plantMutation.AdditionalSoundEvents);
			}
		}
		return list;
	}

	// Token: 0x04005627 RID: 22055
	[Serialize]
	private bool analyzed;

	// Token: 0x04005628 RID: 22056
	[Serialize]
	private List<string> mutationIDs;

	// Token: 0x04005629 RID: 22057
	private List<Guid> statusItemHandles = new List<Guid>();

	// Token: 0x0400562A RID: 22058
	private const int MAX_MUTATIONS = 1;

	// Token: 0x0400562B RID: 22059
	[SerializeField]
	private Tag speciesID;

	// Token: 0x0400562C RID: 22060
	private Tag cachedSubspeciesID;

	// Token: 0x0400562D RID: 22061
	private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate(MutantPlant component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x0400562E RID: 22062
	private static readonly EventSystem.IntraObjectHandler<MutantPlant> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<MutantPlant>(delegate(MutantPlant component, object data)
	{
		component.OnSplitFromChunk(data);
	});
}
