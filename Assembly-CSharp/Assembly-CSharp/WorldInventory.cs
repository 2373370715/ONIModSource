﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/WorldInventory")]
public class WorldInventory : KMonoBehaviour, ISaveLoadable
{
		public WorldContainer WorldContainer
	{
		get
		{
			if (this.m_worldContainer == null)
			{
				this.m_worldContainer = base.GetComponent<WorldContainer>();
			}
			return this.m_worldContainer;
		}
	}

		public bool HasValidCount
	{
		get
		{
			return this.hasValidCount;
		}
	}

		private int worldId
	{
		get
		{
			WorldContainer worldContainer = this.WorldContainer;
			if (!(worldContainer != null))
			{
				return -1;
			}
			return worldContainer.id;
		}
	}

	protected override void OnPrefabInit()
	{
		base.Subscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
		base.Subscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
		base.Subscribe<WorldInventory>(631075836, WorldInventory.OnNewDayDelegate);
		this.m_worldContainer = base.GetComponent<WorldContainer>();
	}

	protected override void OnCleanUp()
	{
		base.Unsubscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
		base.Unsubscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
		base.OnCleanUp();
	}

	private void GenerateInventoryReport(object data)
	{
		int num = 0;
		int num2 = 0;
		foreach (Brain brain in Components.Brains.GetWorldItems(this.worldId, false))
		{
			CreatureBrain creatureBrain = brain as CreatureBrain;
			if (creatureBrain != null)
			{
				if (creatureBrain.HasTag(GameTags.Creatures.Wild))
				{
					num++;
					ReportManager.Instance.ReportValue(ReportManager.ReportType.WildCritters, 1f, creatureBrain.GetProperName(), creatureBrain.GetProperName());
				}
				else
				{
					num2++;
					ReportManager.Instance.ReportValue(ReportManager.ReportType.DomesticatedCritters, 1f, creatureBrain.GetProperName(), creatureBrain.GetProperName());
				}
			}
		}
		if (DlcManager.IsExpansion1Active())
		{
			WorldContainer component = base.GetComponent<WorldContainer>();
			if (component != null && component.IsModuleInterior)
			{
				Clustercraft clustercraft = component.GetComponent<ClusterGridEntity>() as Clustercraft;
				if (clustercraft != null && clustercraft.Status != Clustercraft.CraftStatus.Grounded)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, clustercraft.Name, null);
					return;
				}
			}
		}
		else
		{
			foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
			{
				if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, spacecraft.rocketName, null);
				}
			}
		}
	}

	protected override void OnSpawn()
	{
		this.Prober = MinionGroupProber.Get();
		base.StartCoroutine(this.InitialRefresh());
	}

	private IEnumerator InitialRefresh()
	{
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		for (int j = 0; j < Components.Pickupables.Count; j++)
		{
			Pickupable pickupable = Components.Pickupables[j];
			if (pickupable != null)
			{
				ReachabilityMonitor.Instance smi = pickupable.GetSMI<ReachabilityMonitor.Instance>();
				if (smi != null)
				{
					smi.UpdateReachability();
				}
			}
		}
		yield break;
	}

	public bool IsReachable(Pickupable pickupable)
	{
		return this.Prober.IsReachable(pickupable);
	}

	public float GetTotalAmount(Tag tag, bool includeRelatedWorlds)
	{
		float result = 0f;
		this.accessibleAmounts.TryGetValue(tag, out result);
		return result;
	}

	public ICollection<Pickupable> GetPickupables(Tag tag, bool includeRelatedWorlds = false)
	{
		if (!includeRelatedWorlds)
		{
			HashSet<Pickupable> result = null;
			this.Inventory.TryGetValue(tag, out result);
			return result;
		}
		return ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
	}

	public List<Pickupable> CreatePickupablesList(Tag tag)
	{
		HashSet<Pickupable> hashSet = null;
		this.Inventory.TryGetValue(tag, out hashSet);
		if (hashSet == null)
		{
			return null;
		}
		return hashSet.ToList<Pickupable>();
	}

	public float GetAmount(Tag tag, bool includeRelatedWorlds)
	{
		float num;
		if (!includeRelatedWorlds)
		{
			num = this.GetTotalAmount(tag, includeRelatedWorlds);
			num -= MaterialNeeds.GetAmount(tag, this.worldId, includeRelatedWorlds);
		}
		else
		{
			num = ClusterUtil.GetAmountFromRelatedWorlds(this, tag);
		}
		return Mathf.Max(num, 0f);
	}

	public int GetCountWithAdditionalTag(Tag tag, Tag additionalTag, bool includeRelatedWorlds = false)
	{
		ICollection<Pickupable> collection;
		if (!includeRelatedWorlds)
		{
			collection = this.GetPickupables(tag, false);
		}
		else
		{
			ICollection<Pickupable> pickupablesFromRelatedWorlds = ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
			collection = pickupablesFromRelatedWorlds;
		}
		ICollection<Pickupable> collection2 = collection;
		int num = 0;
		if (collection2 != null)
		{
			if (additionalTag.IsValid)
			{
				using (IEnumerator<Pickupable> enumerator = collection2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasTag(additionalTag))
						{
							num++;
						}
					}
					return num;
				}
			}
			num = collection2.Count;
		}
		return num;
	}

	public float GetAmountWithoutTag(Tag tag, bool includeRelatedWorlds = false, Tag[] forbiddenTags = null)
	{
		if (forbiddenTags == null)
		{
			return this.GetAmount(tag, includeRelatedWorlds);
		}
		float num = 0f;
		ICollection<Pickupable> collection;
		if (!includeRelatedWorlds)
		{
			collection = this.GetPickupables(tag, false);
		}
		else
		{
			ICollection<Pickupable> pickupablesFromRelatedWorlds = ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
			collection = pickupablesFromRelatedWorlds;
		}
		ICollection<Pickupable> collection2 = collection;
		if (collection2 != null)
		{
			foreach (Pickupable pickupable in collection2)
			{
				if (pickupable != null && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate) && !pickupable.KPrefabID.HasAnyTags(forbiddenTags))
				{
					num += pickupable.TotalAmount;
				}
			}
		}
		return num;
	}

	private void Update()
	{
		int num = 0;
		Dictionary<Tag, HashSet<Pickupable>>.Enumerator enumerator = this.Inventory.GetEnumerator();
		int worldId = this.worldId;
		while (enumerator.MoveNext())
		{
			KeyValuePair<Tag, HashSet<Pickupable>> keyValuePair = enumerator.Current;
			if (num == this.accessibleUpdateIndex || this.firstUpdate)
			{
				Tag key = keyValuePair.Key;
				IEnumerable<Pickupable> value = keyValuePair.Value;
				float num2 = 0f;
				foreach (Pickupable pickupable in value)
				{
					if (pickupable != null && pickupable.GetMyWorldId() == worldId && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
					{
						num2 += pickupable.TotalAmount;
					}
				}
				if (!this.hasValidCount && this.accessibleUpdateIndex + 1 >= this.Inventory.Count)
				{
					this.hasValidCount = true;
					if (this.worldId == ClusterManager.Instance.activeWorldId)
					{
						this.hasValidCount = true;
						PinnedResourcesPanel.Instance.ClearExcessiveNewItems();
						PinnedResourcesPanel.Instance.Refresh();
					}
				}
				this.accessibleAmounts[key] = num2;
				this.accessibleUpdateIndex = (this.accessibleUpdateIndex + 1) % this.Inventory.Count;
				break;
			}
			num++;
		}
		this.firstUpdate = false;
	}

	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	private void OnAddedFetchable(object data)
	{
		GameObject gameObject = (GameObject)data;
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		if (component.HasAnyTags(WorldInventory.NonCritterEntitiesTags))
		{
			return;
		}
		Pickupable component2 = gameObject.GetComponent<Pickupable>();
		if (component2.GetMyWorldId() != this.worldId)
		{
			return;
		}
		Tag tag = component.PrefabID();
		if (!this.Inventory.ContainsKey(tag))
		{
			Tag categoryForEntity = DiscoveredResources.GetCategoryForEntity(component);
			DebugUtil.DevAssertArgs(categoryForEntity.IsValid, new object[]
			{
				component2.name,
				"was found by worldinventory but doesn't have a category! Add it to the element definition."
			});
			DiscoveredResources.Instance.Discover(tag, categoryForEntity);
		}
		HashSet<Pickupable> hashSet;
		if (!this.Inventory.TryGetValue(tag, out hashSet))
		{
			hashSet = new HashSet<Pickupable>();
			this.Inventory[tag] = hashSet;
		}
		hashSet.Add(component2);
		foreach (Tag key in component.Tags)
		{
			if (!this.Inventory.TryGetValue(key, out hashSet))
			{
				hashSet = new HashSet<Pickupable>();
				this.Inventory[key] = hashSet;
			}
			hashSet.Add(component2);
		}
	}

	private void OnRemovedFetchable(object data)
	{
		Pickupable component = ((GameObject)data).GetComponent<Pickupable>();
		KPrefabID kprefabID = component.KPrefabID;
		HashSet<Pickupable> hashSet;
		if (this.Inventory.TryGetValue(kprefabID.PrefabTag, out hashSet))
		{
			hashSet.Remove(component);
		}
		foreach (Tag key in kprefabID.Tags)
		{
			if (this.Inventory.TryGetValue(key, out hashSet))
			{
				hashSet.Remove(component);
			}
		}
	}

	public Dictionary<Tag, float> GetAccessibleAmounts()
	{
		return this.accessibleAmounts;
	}

	private WorldContainer m_worldContainer;

	[Serialize]
	public List<Tag> pinnedResources = new List<Tag>();

	[Serialize]
	public List<Tag> notifyResources = new List<Tag>();

	private Dictionary<Tag, HashSet<Pickupable>> Inventory = new Dictionary<Tag, HashSet<Pickupable>>();

	private MinionGroupProber Prober;

	private Dictionary<Tag, float> accessibleAmounts = new Dictionary<Tag, float>();

	private bool hasValidCount;

	private static readonly EventSystem.IntraObjectHandler<WorldInventory> OnNewDayDelegate = new EventSystem.IntraObjectHandler<WorldInventory>(delegate(WorldInventory component, object data)
	{
		component.GenerateInventoryReport(data);
	});

	private int accessibleUpdateIndex;

	private bool firstUpdate = true;

	private static Tag[] NonCritterEntitiesTags = new Tag[]
	{
		GameTags.DupeBrain,
		GameTags.Robot
	};
}
