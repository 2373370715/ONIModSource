﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[DebuggerDisplay("{id}: {type} at distance {distance}")]
public class SpaceDestination
{
		private static global::Tuple<SimHashes, MathUtil.MinMax> GetRareElement(SimHashes id)
	{
		foreach (global::Tuple<SimHashes, MathUtil.MinMax> tuple in SpaceDestination.RARE_ELEMENTS)
		{
			if (tuple.first == id)
			{
				return tuple;
			}
		}
		return null;
	}

			public int OneBasedDistance
	{
		get
		{
			return this.distance + 1;
		}
	}

			public float CurrentMass
	{
		get
		{
			return (float)this.GetDestinationType().minimumMass + this.availableMass;
		}
	}

			public float AvailableMass
	{
		get
		{
			return this.availableMass;
		}
	}

		public SpaceDestination(int id, string type, int distance)
	{
		this.id = id;
		this.type = type;
		this.distance = distance;
		SpaceDestinationType destinationType = this.GetDestinationType();
		this.availableMass = (float)(destinationType.maxiumMass - destinationType.minimumMass);
		this.GenerateSurfaceElements();
		this.GenerateResearchOpportunities();
	}

		[OnDeserialized]
	private void OnDeserialized()
	{
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 9))
		{
			SpaceDestinationType destinationType = this.GetDestinationType();
			this.availableMass = (float)(destinationType.maxiumMass - destinationType.minimumMass);
		}
	}

		public SpaceDestinationType GetDestinationType()
	{
		return Db.Get().SpaceDestinationTypes.Get(this.type);
	}

		public SpaceDestination.ResearchOpportunity TryCompleteResearchOpportunity()
	{
		foreach (SpaceDestination.ResearchOpportunity researchOpportunity in this.researchOpportunities)
		{
			if (researchOpportunity.TryComplete(this))
			{
				return researchOpportunity;
			}
		}
		return null;
	}

		public void GenerateSurfaceElements()
	{
		foreach (KeyValuePair<SimHashes, MathUtil.MinMax> keyValuePair in this.GetDestinationType().elementTable)
		{
			this.recoverableElements.Add(keyValuePair.Key, UnityEngine.Random.value);
		}
	}

		public SpacecraftManager.DestinationAnalysisState AnalysisState()
	{
		return SpacecraftManager.instance.GetDestinationAnalysisState(this);
	}

		public void GenerateResearchOpportunities()
	{
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.UPPERATMO, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.LOWERATMO, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.MAGNETICFIELD, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SURFACE, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		this.researchOpportunities.Add(new SpaceDestination.ResearchOpportunity(UI.STARMAP.DESTINATIONSTUDY.SUBSURFACE, ROCKETRY.DESTINATION_RESEARCH.BASIC));
		float num = 0f;
		foreach (global::Tuple<float, int> tuple in SpaceDestination.RARE_ELEMENT_CHANCES)
		{
			num += tuple.first;
		}
		float num2 = UnityEngine.Random.value * num;
		int num3 = 0;
		foreach (global::Tuple<float, int> tuple2 in SpaceDestination.RARE_ELEMENT_CHANCES)
		{
			num2 -= tuple2.first;
			if (num2 <= 0f)
			{
				num3 = tuple2.second;
			}
		}
		for (int i = 0; i < num3; i++)
		{
			this.researchOpportunities[UnityEngine.Random.Range(0, this.researchOpportunities.Count)].discoveredRareResource = SpaceDestination.RARE_ELEMENTS[UnityEngine.Random.Range(0, SpaceDestination.RARE_ELEMENTS.Count)].first;
		}
		if (UnityEngine.Random.value < 0.33f)
		{
			int index = UnityEngine.Random.Range(0, this.researchOpportunities.Count);
			this.researchOpportunities[index].discoveredRareItem = SpaceDestination.RARE_ITEMS[UnityEngine.Random.Range(0, SpaceDestination.RARE_ITEMS.Count)].first;
		}
	}

		public float GetResourceValue(SimHashes resource, float roll)
	{
		if (this.GetDestinationType().elementTable.ContainsKey(resource))
		{
			return this.GetDestinationType().elementTable[resource].Lerp(roll);
		}
		if (SpaceDestinationTypes.extendedElementTable.ContainsKey(resource))
		{
			return SpaceDestinationTypes.extendedElementTable[resource].Lerp(roll);
		}
		return 0f;
	}

		public Dictionary<SimHashes, float> GetMissionResourceResult(float totalCargoSpace, float reservedMass, bool solids = true, bool liquids = true, bool gasses = true)
	{
		Dictionary<SimHashes, float> dictionary = new Dictionary<SimHashes, float>();
		float num = 0f;
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && gasses))
			{
				num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value);
			}
		}
		float num2 = Mathf.Min(this.CurrentMass + reservedMass - (float)this.GetDestinationType().minimumMass, totalCargoSpace);
		foreach (KeyValuePair<SimHashes, float> keyValuePair2 in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair2.Key).IsSolid && solids) || (ElementLoader.FindElementByHash(keyValuePair2.Key).IsLiquid && liquids) || (ElementLoader.FindElementByHash(keyValuePair2.Key).IsGas && gasses))
			{
				float value = num2 * (this.GetResourceValue(keyValuePair2.Key, keyValuePair2.Value) / num);
				dictionary.Add(keyValuePair2.Key, value);
			}
		}
		return dictionary;
	}

		public Dictionary<Tag, int> GetRecoverableEntities()
	{
		Dictionary<Tag, int> dictionary = new Dictionary<Tag, int>();
		Dictionary<string, int> recoverableEntities = this.GetDestinationType().recoverableEntities;
		if (recoverableEntities != null)
		{
			foreach (KeyValuePair<string, int> keyValuePair in recoverableEntities)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		return dictionary;
	}

		public Dictionary<Tag, int> GetMissionEntityResult()
	{
		return this.GetRecoverableEntities();
	}

		public float ReserveResources(CargoBay bay)
	{
		float num = 0f;
		if (bay != null)
		{
			Storage component = bay.GetComponent<Storage>();
			foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
			{
				if (this.HasElementType(bay.storageType))
				{
					num += component.capacityKg;
					this.availableMass = Mathf.Max(0f, this.availableMass - component.capacityKg);
					break;
				}
			}
		}
		return num;
	}

		public bool HasElementType(CargoBay.CargoType type)
	{
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && type == CargoBay.CargoType.Solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && type == CargoBay.CargoType.Liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && type == CargoBay.CargoType.Gasses))
			{
				return true;
			}
		}
		return false;
	}

		public void Replenish(float dt)
	{
		SpaceDestinationType destinationType = this.GetDestinationType();
		if (this.CurrentMass < (float)destinationType.maxiumMass)
		{
			this.availableMass += destinationType.replishmentPerSim1000ms;
		}
	}

		public float GetAvailableResourcesPercentage(CargoBay.CargoType cargoType)
	{
		float num = 0f;
		float totalMass = this.GetTotalMass();
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			if ((ElementLoader.FindElementByHash(keyValuePair.Key).IsSolid && cargoType == CargoBay.CargoType.Solids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsLiquid && cargoType == CargoBay.CargoType.Liquids) || (ElementLoader.FindElementByHash(keyValuePair.Key).IsGas && cargoType == CargoBay.CargoType.Gasses))
			{
				num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value) / totalMass;
			}
		}
		return num;
	}

		public float GetTotalMass()
	{
		float num = 0f;
		foreach (KeyValuePair<SimHashes, float> keyValuePair in this.recoverableElements)
		{
			num += this.GetResourceValue(keyValuePair.Key, keyValuePair.Value);
		}
		return num;
	}

		private const int MASS_TO_RECOVER_AMOUNT = 1000;

		private static List<global::Tuple<float, int>> RARE_ELEMENT_CHANCES = new List<global::Tuple<float, int>>
	{
		new global::Tuple<float, int>(1f, 0),
		new global::Tuple<float, int>(0.33f, 1),
		new global::Tuple<float, int>(0.03f, 2)
	};

		private static readonly List<global::Tuple<SimHashes, MathUtil.MinMax>> RARE_ELEMENTS = new List<global::Tuple<SimHashes, MathUtil.MinMax>>
	{
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Katairite, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Niobium, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Fullerene, new MathUtil.MinMax(1f, 10f)),
		new global::Tuple<SimHashes, MathUtil.MinMax>(SimHashes.Isoresin, new MathUtil.MinMax(1f, 10f))
	};

		private const float RARE_ITEM_CHANCE = 0.33f;

		private static readonly List<global::Tuple<string, MathUtil.MinMax>> RARE_ITEMS = new List<global::Tuple<string, MathUtil.MinMax>>
	{
		new global::Tuple<string, MathUtil.MinMax>("GeneShufflerRecharge", new MathUtil.MinMax(1f, 2f))
	};

		[Serialize]
	public int id;

		[Serialize]
	public string type;

		public bool startAnalyzed;

		[Serialize]
	public int distance;

		[Serialize]
	public float activePeriod = 20f;

		[Serialize]
	public float inactivePeriod = 10f;

		[Serialize]
	public float startingOrbitPercentage;

		[Serialize]
	public Dictionary<SimHashes, float> recoverableElements = new Dictionary<SimHashes, float>();

		[Serialize]
	public List<SpaceDestination.ResearchOpportunity> researchOpportunities = new List<SpaceDestination.ResearchOpportunity>();

		[Serialize]
	private float availableMass;

		[SerializationConfig(MemberSerialization.OptIn)]
	public class ResearchOpportunity
	{
				[OnDeserialized]
		private void OnDeserialized()
		{
			if (this.discoveredRareResource == (SimHashes)0)
			{
				this.discoveredRareResource = SimHashes.Void;
			}
			if (this.dataValue > 50)
			{
				this.dataValue = 50;
			}
		}

				public ResearchOpportunity(string description, int pointValue)
		{
			this.description = description;
			this.dataValue = pointValue;
		}

				public bool TryComplete(SpaceDestination destination)
		{
			if (!this.completed)
			{
				this.completed = true;
				if (this.discoveredRareResource != SimHashes.Void && !destination.recoverableElements.ContainsKey(this.discoveredRareResource))
				{
					destination.recoverableElements.Add(this.discoveredRareResource, UnityEngine.Random.value);
				}
				return true;
			}
			return false;
		}

				[Serialize]
		public string description;

				[Serialize]
		public int dataValue;

				[Serialize]
		public bool completed;

				[Serialize]
		public SimHashes discoveredRareResource = SimHashes.Void;

				[Serialize]
		public string discoveredRareItem;
	}
}
