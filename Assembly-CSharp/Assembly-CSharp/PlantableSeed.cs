﻿using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/PlantableSeed")]
public class PlantableSeed : KMonoBehaviour, IReceptacleDirection, IGameObjectEffectDescriptor
{
		public SingleEntityReceptacle.ReceptacleDirection Direction
	{
		get
		{
			return this.direction;
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.PlantableSeeds.Add(this);
	}

	protected override void OnCleanUp()
	{
		Components.PlantableSeeds.Remove(this);
		base.OnCleanUp();
	}

	public void TryPlant(bool allow_plant_from_storage = false)
	{
		this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
		if (!allow_plant_from_storage && base.gameObject.HasTag(GameTags.Stored))
		{
			return;
		}
		int cell = Grid.PosToCell(base.gameObject);
		if (this.TestSuitableGround(cell))
		{
			Vector3 position = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.PlantID), position, Grid.SceneLayer.BuildingFront, null, 0);
			MutantPlant component = gameObject.GetComponent<MutantPlant>();
			if (component != null)
			{
				base.GetComponent<MutantPlant>().CopyMutationsTo(component);
			}
			gameObject.SetActive(true);
			Pickupable pickupable = this.pickupable.Take(1f);
			if (pickupable != null)
			{
				gameObject.GetComponent<Crop>() != null;
				Util.KDestroyGameObject(pickupable.gameObject);
				return;
			}
			KCrashReporter.Assert(false, "Seed has fractional total amount < 1f", null);
		}
	}

	public bool TestSuitableGround(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num;
		if (this.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			num = Grid.CellAbove(cell);
		}
		else
		{
			num = Grid.CellBelow(cell);
		}
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (Grid.Foundation[num])
		{
			return false;
		}
		if (Grid.Element[num].hardness >= 150)
		{
			return false;
		}
		if (this.replantGroundTag.IsValid && !Grid.Element[num].HasTag(this.replantGroundTag))
		{
			return false;
		}
		GameObject prefab = Assets.GetPrefab(this.PlantID);
		EntombVulnerable component = prefab.GetComponent<EntombVulnerable>();
		if (component != null && !component.IsCellSafe(cell))
		{
			return false;
		}
		DrowningMonitor component2 = prefab.GetComponent<DrowningMonitor>();
		if (component2 != null && !component2.IsCellSafe(cell))
		{
			return false;
		}
		TemperatureVulnerable component3 = prefab.GetComponent<TemperatureVulnerable>();
		if (component3 != null && !component3.IsCellSafe(cell) && Grid.Element[cell].id != SimHashes.Vacuum)
		{
			return false;
		}
		UprootedMonitor component4 = prefab.GetComponent<UprootedMonitor>();
		if (component4 != null && !component4.IsSuitableFoundation(cell))
		{
			return false;
		}
		OccupyArea component5 = prefab.GetComponent<OccupyArea>();
		return !(component5 != null) || component5.CanOccupyArea(cell, ObjectLayer.Building);
	}

	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			Descriptor item = new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_CEILING, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_CEILING, Descriptor.DescriptorType.Requirement, false);
			list.Add(item);
		}
		else if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
		{
			Descriptor item2 = new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_WALL, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_WALL, Descriptor.DescriptorType.Requirement, false);
			list.Add(item2);
		}
		return list;
	}

	public Tag PlantID;

	public Tag PreviewID;

	[Serialize]
	public float timeUntilSelfPlant;

	public Tag replantGroundTag;

	public string domesticatedDescription;

	public SingleEntityReceptacle.ReceptacleDirection direction;

	[MyCmpGet]
	private Pickupable pickupable;
}
