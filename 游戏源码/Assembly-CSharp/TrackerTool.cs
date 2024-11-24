using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000B33 RID: 2867
public class TrackerTool : KMonoBehaviour
{
	// Token: 0x06003689 RID: 13961 RVA: 0x00213C20 File Offset: 0x00211E20
	protected override void OnSpawn()
	{
		TrackerTool.Instance = this;
		base.OnSpawn();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			this.AddNewWorldTrackers(worldContainer.id);
		}
		foreach (object obj in Components.LiveMinionIdentities)
		{
			this.AddMinionTrackers((MinionIdentity)obj);
		}
		Components.LiveMinionIdentities.OnAdd += this.AddMinionTrackers;
		ClusterManager.Instance.Subscribe(-1280433810, new Action<object>(this.Refresh));
		ClusterManager.Instance.Subscribe(-1078710002, new Action<object>(this.RemoveWorld));
	}

	// Token: 0x0600368A RID: 13962 RVA: 0x000C3785 File Offset: 0x000C1985
	protected override void OnForcedCleanUp()
	{
		TrackerTool.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600368B RID: 13963 RVA: 0x00213D1C File Offset: 0x00211F1C
	private void AddMinionTrackers(MinionIdentity identity)
	{
		this.minionTrackers.Add(identity, new List<MinionTracker>());
		identity.Subscribe(1969584890, delegate(object data)
		{
			this.minionTrackers.Remove(identity);
		});
	}

	// Token: 0x0600368C RID: 13964 RVA: 0x00213D70 File Offset: 0x00211F70
	private void Refresh(object data)
	{
		int worldID = (int)data;
		this.AddNewWorldTrackers(worldID);
	}

	// Token: 0x0600368D RID: 13965 RVA: 0x00213D8C File Offset: 0x00211F8C
	private void RemoveWorld(object data)
	{
		int world_id = (int)data;
		this.worldTrackers.RemoveAll((WorldTracker match) => match.WorldID == world_id);
	}

	// Token: 0x0600368E RID: 13966 RVA: 0x000C3793 File Offset: 0x000C1993
	public bool IsRocketInterior(int worldID)
	{
		return ClusterManager.Instance.GetWorld(worldID).IsModuleInterior;
	}

	// Token: 0x0600368F RID: 13967 RVA: 0x00213DC4 File Offset: 0x00211FC4
	private void AddNewWorldTrackers(int worldID)
	{
		this.worldTrackers.Add(new StressTracker(worldID));
		this.worldTrackers.Add(new KCalTracker(worldID));
		this.worldTrackers.Add(new IdleTracker(worldID));
		this.worldTrackers.Add(new BreathabilityTracker(worldID));
		this.worldTrackers.Add(new PowerUseTracker(worldID));
		this.worldTrackers.Add(new BatteryTracker(worldID));
		this.worldTrackers.Add(new CropTracker(worldID));
		this.worldTrackers.Add(new WorkingToiletTracker(worldID));
		this.worldTrackers.Add(new RadiationTracker(worldID));
		if (SaveLoader.Instance.IsDLCActiveForCurrentSave("DLC3_ID"))
		{
			this.worldTrackers.Add(new ElectrobankJoulesTracker(worldID));
		}
		if (ClusterManager.Instance.GetWorld(worldID).IsModuleInterior)
		{
			this.worldTrackers.Add(new RocketFuelTracker(worldID));
			this.worldTrackers.Add(new RocketOxidizerTracker(worldID));
		}
		for (int i = 0; i < Db.Get().ChoreGroups.Count; i++)
		{
			this.worldTrackers.Add(new WorkTimeTracker(worldID, Db.Get().ChoreGroups[i]));
			this.worldTrackers.Add(new ChoreCountTracker(worldID, Db.Get().ChoreGroups[i]));
		}
		this.worldTrackers.Add(new AllChoresCountTracker(worldID));
		this.worldTrackers.Add(new AllWorkTimeTracker(worldID));
		foreach (Tag tag in GameTags.CalorieCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag));
			foreach (GameObject gameObject in Assets.GetPrefabsWithTag(tag))
			{
				this.AddResourceTracker(worldID, gameObject.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag2 in GameTags.UnitCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag2));
			foreach (GameObject gameObject2 in Assets.GetPrefabsWithTag(tag2))
			{
				this.AddResourceTracker(worldID, gameObject2.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag3 in GameTags.MaterialCategories)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag3));
			foreach (GameObject gameObject3 in Assets.GetPrefabsWithTag(tag3))
			{
				this.AddResourceTracker(worldID, gameObject3.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (Tag tag4 in GameTags.OtherEntityTags)
		{
			this.worldTrackers.Add(new ResourceTracker(worldID, tag4));
			foreach (GameObject gameObject4 in Assets.GetPrefabsWithTag(tag4))
			{
				this.AddResourceTracker(worldID, gameObject4.GetComponent<KPrefabID>().PrefabTag);
			}
		}
		foreach (GameObject gameObject5 in Assets.GetPrefabsWithTag(GameTags.CookingIngredient))
		{
			this.AddResourceTracker(worldID, gameObject5.GetComponent<KPrefabID>().PrefabTag);
		}
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			this.AddResourceTracker(worldID, foodInfo.Id);
		}
		foreach (Element element in ElementLoader.elements)
		{
			this.AddResourceTracker(worldID, element.tag);
		}
	}

	// Token: 0x06003690 RID: 13968 RVA: 0x0021428C File Offset: 0x0021248C
	private void AddResourceTracker(int worldID, Tag tag)
	{
		if (this.worldTrackers.Find((WorldTracker match) => match is ResourceTracker && ((ResourceTracker)match).WorldID == worldID && ((ResourceTracker)match).tag == tag) != null)
		{
			return;
		}
		this.worldTrackers.Add(new ResourceTracker(worldID, tag));
	}

	// Token: 0x06003691 RID: 13969 RVA: 0x002142E4 File Offset: 0x002124E4
	public ResourceTracker GetResourceStatistic(int worldID, Tag tag)
	{
		return (ResourceTracker)this.worldTrackers.Find((WorldTracker match) => match is ResourceTracker && ((ResourceTracker)match).WorldID == worldID && ((ResourceTracker)match).tag == tag);
	}

	// Token: 0x06003692 RID: 13970 RVA: 0x00214324 File Offset: 0x00212524
	public WorldTracker GetWorldTracker<T>(int worldID) where T : WorldTracker
	{
		return (T)((object)this.worldTrackers.Find((WorldTracker match) => match is T && ((T)((object)match)).WorldID == worldID));
	}

	// Token: 0x06003693 RID: 13971 RVA: 0x00214360 File Offset: 0x00212560
	public ChoreCountTracker GetChoreGroupTracker(int worldID, ChoreGroup choreGroup)
	{
		return (ChoreCountTracker)this.worldTrackers.Find((WorldTracker match) => match is ChoreCountTracker && ((ChoreCountTracker)match).WorldID == worldID && ((ChoreCountTracker)match).choreGroup == choreGroup);
	}

	// Token: 0x06003694 RID: 13972 RVA: 0x002143A0 File Offset: 0x002125A0
	public WorkTimeTracker GetWorkTimeTracker(int worldID, ChoreGroup choreGroup)
	{
		return (WorkTimeTracker)this.worldTrackers.Find((WorldTracker match) => match is WorkTimeTracker && ((WorkTimeTracker)match).WorldID == worldID && ((WorkTimeTracker)match).choreGroup == choreGroup);
	}

	// Token: 0x06003695 RID: 13973 RVA: 0x000C37A5 File Offset: 0x000C19A5
	public MinionTracker GetMinionTracker<T>(MinionIdentity identity) where T : MinionTracker
	{
		return (T)((object)this.minionTrackers[identity].Find((MinionTracker match) => match is T));
	}

	// Token: 0x06003696 RID: 13974 RVA: 0x002143E0 File Offset: 0x002125E0
	public void Update()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		if (!this.trackerActive)
		{
			return;
		}
		if (this.minionTrackers.Count > 0)
		{
			this.updatingMinionTracker++;
			if (this.updatingMinionTracker >= this.minionTrackers.Count)
			{
				this.updatingMinionTracker = 0;
			}
			KeyValuePair<MinionIdentity, List<MinionTracker>> keyValuePair = this.minionTrackers.ElementAt(this.updatingMinionTracker);
			for (int i = 0; i < keyValuePair.Value.Count; i++)
			{
				keyValuePair.Value[i].UpdateData();
			}
		}
		if (this.worldTrackers.Count > 0)
		{
			for (int j = 0; j < this.numUpdatesPerFrame; j++)
			{
				this.updatingWorldTracker++;
				if (this.updatingWorldTracker >= this.worldTrackers.Count)
				{
					this.updatingWorldTracker = 0;
				}
				this.worldTrackers[this.updatingWorldTracker].UpdateData();
			}
		}
	}

	// Token: 0x04002517 RID: 9495
	public static TrackerTool Instance;

	// Token: 0x04002518 RID: 9496
	private List<WorldTracker> worldTrackers = new List<WorldTracker>();

	// Token: 0x04002519 RID: 9497
	private Dictionary<MinionIdentity, List<MinionTracker>> minionTrackers = new Dictionary<MinionIdentity, List<MinionTracker>>();

	// Token: 0x0400251A RID: 9498
	private int updatingWorldTracker;

	// Token: 0x0400251B RID: 9499
	private int updatingMinionTracker;

	// Token: 0x0400251C RID: 9500
	public bool trackerActive = true;

	// Token: 0x0400251D RID: 9501
	private int numUpdatesPerFrame = 50;
}
