﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Database;
using KSerialization;
using ProcGen;
using STRINGS;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SpacecraftManager")]
public class SpacecraftManager : KMonoBehaviour, ISim1000ms
{
	public static void DestroyInstance()
	{
		SpacecraftManager.instance = null;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SpacecraftManager.instance = this;
		if (this.savedSpacecraftDestinations == null)
		{
			this.savedSpacecraftDestinations = new Dictionary<int, int>();
		}
	}

	private void GenerateFixedDestinations()
	{
		SpaceDestinationTypes spaceDestinationTypes = Db.Get().SpaceDestinationTypes;
		if (this.destinations != null)
		{
			return;
		}
		this.destinations = new List<SpaceDestination>
		{
			new SpaceDestination(0, spaceDestinationTypes.CarbonaceousAsteroid.Id, 0),
			new SpaceDestination(1, spaceDestinationTypes.CarbonaceousAsteroid.Id, 0),
			new SpaceDestination(2, spaceDestinationTypes.MetallicAsteroid.Id, 1),
			new SpaceDestination(3, spaceDestinationTypes.RockyAsteroid.Id, 2),
			new SpaceDestination(4, spaceDestinationTypes.IcyDwarf.Id, 3),
			new SpaceDestination(5, spaceDestinationTypes.OrganicDwarf.Id, 4)
		};
	}

	private void GenerateRandomDestinations()
	{
		KRandom krandom = new KRandom(SaveLoader.Instance.clusterDetailSave.globalWorldSeed);
		SpaceDestinationTypes spaceDestinationTypes = Db.Get().SpaceDestinationTypes;
		List<List<string>> list = new List<List<string>>
		{
			new List<string>(),
			new List<string>
			{
				spaceDestinationTypes.OilyAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.Satellite.Id
			},
			new List<string>
			{
				spaceDestinationTypes.Satellite.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.CarbonaceousAsteroid.Id,
				spaceDestinationTypes.ForestPlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.CarbonaceousAsteroid.Id,
				spaceDestinationTypes.SaltDwarf.Id
			},
			new List<string>
			{
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.CarbonaceousAsteroid.Id,
				spaceDestinationTypes.IcyDwarf.Id,
				spaceDestinationTypes.OrganicDwarf.Id
			},
			new List<string>
			{
				spaceDestinationTypes.IcyDwarf.Id,
				spaceDestinationTypes.OrganicDwarf.Id,
				spaceDestinationTypes.DustyMoon.Id,
				spaceDestinationTypes.ChlorinePlanet.Id,
				spaceDestinationTypes.RedDwarf.Id
			},
			new List<string>
			{
				spaceDestinationTypes.DustyMoon.Id,
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.VolcanoPlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.GasGiant.Id,
				spaceDestinationTypes.IceGiant.Id,
				spaceDestinationTypes.RustPlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.GasGiant.Id,
				spaceDestinationTypes.IceGiant.Id,
				spaceDestinationTypes.HydrogenGiant.Id
			},
			new List<string>
			{
				spaceDestinationTypes.RustPlanet.Id,
				spaceDestinationTypes.VolcanoPlanet.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.MetallicAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.ShinyPlanet.Id,
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.RockyAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.GoldAsteroid.Id,
				spaceDestinationTypes.OrganicDwarf.Id,
				spaceDestinationTypes.ForestPlanet.Id,
				spaceDestinationTypes.ChlorinePlanet.Id
			},
			new List<string>
			{
				spaceDestinationTypes.IcyDwarf.Id,
				spaceDestinationTypes.MetallicAsteroid.Id,
				spaceDestinationTypes.DustyMoon.Id,
				spaceDestinationTypes.VolcanoPlanet.Id,
				spaceDestinationTypes.IceGiant.Id
			},
			new List<string>
			{
				spaceDestinationTypes.ShinyPlanet.Id,
				spaceDestinationTypes.RedDwarf.Id,
				spaceDestinationTypes.RockyAsteroid.Id,
				spaceDestinationTypes.GasGiant.Id
			},
			new List<string>
			{
				spaceDestinationTypes.HydrogenGiant.Id,
				spaceDestinationTypes.ForestPlanet.Id,
				spaceDestinationTypes.OilyAsteroid.Id
			},
			new List<string>
			{
				spaceDestinationTypes.GoldAsteroid.Id,
				spaceDestinationTypes.SaltDwarf.Id,
				spaceDestinationTypes.TerraPlanet.Id,
				spaceDestinationTypes.VolcanoPlanet.Id
			}
		};
		List<int> list2 = new List<int>();
		int num = 3;
		int minValue = 15;
		int maxValue = 25;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Count != 0)
			{
				for (int j = 0; j < num; j++)
				{
					list2.Add(i);
				}
			}
		}
		SpacecraftManager.<>c__DisplayClass12_0 CS$<>8__locals1;
		CS$<>8__locals1.nextId = this.destinations.Count;
		int num2 = krandom.Next(minValue, maxValue);
		List<SpaceDestination> list3 = new List<SpaceDestination>();
		for (int k = 0; k < num2; k++)
		{
			int index = krandom.Next(0, list2.Count - 1);
			int num3 = list2[index];
			list2.RemoveAt(index);
			List<string> list4 = list[num3];
			string type = list4[krandom.Next(0, list4.Count)];
			SpaceDestination item = new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), type, num3);
			list3.Add(item);
		}
		list2.ShuffleSeeded(krandom);
		List<SpaceDestination> list5 = new List<SpaceDestination>();
		foreach (string name in CustomGameSettings.Instance.GetCurrentDlcMixingIds())
		{
			foreach (DlcMixingSettings.SpaceDestinationMix spaceDestinationMix in SettingsCache.GetCachedDlcMixingSettings(name).spaceDesinations)
			{
				bool flag = false;
				if (list2.Count > 0)
				{
					for (int l = 0; l < list2.Count; l++)
					{
						int num4 = list2[l];
						if (num4 >= spaceDestinationMix.minTier && num4 <= spaceDestinationMix.maxTier)
						{
							SpaceDestination item2 = new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), spaceDestinationMix.type, num4);
							list5.Add(item2);
							list2.RemoveAt(l);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					for (int m = 0; m < list3.Count; m++)
					{
						SpaceDestination spaceDestination = list3[m];
						if (spaceDestination.distance >= spaceDestinationMix.minTier && spaceDestination.distance <= spaceDestinationMix.maxTier)
						{
							list3[m] = new SpaceDestination(spaceDestination.id, spaceDestinationMix.type, spaceDestination.distance);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					KCrashReporter.ReportDevNotification("Base game failed to mix a space destination", Environment.StackTrace, "", false, null);
					UnityEngine.Debug.LogWarning("Mixing: Unable to place destination '" + spaceDestinationMix.type + "'");
				}
			}
		}
		this.destinations.AddRange(list3);
		this.destinations.Add(new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), Db.Get().SpaceDestinationTypes.Earth.Id, 4));
		this.destinations.Add(new SpaceDestination(SpacecraftManager.<GenerateRandomDestinations>g__GetNextID|12_0(ref CS$<>8__locals1), Db.Get().SpaceDestinationTypes.Wormhole.Id, list.Count));
		this.destinations.AddRange(list5);
	}

	private void RestoreDestinations()
	{
		if (this.destinationsGenerated)
		{
			return;
		}
		this.GenerateFixedDestinations();
		this.GenerateRandomDestinations();
		this.destinations.Sort((SpaceDestination a, SpaceDestination b) => a.distance.CompareTo(b.distance));
		List<float> list = new List<float>();
		for (int i = 0; i < 10; i++)
		{
			list.Add((float)i / 10f);
		}
		for (int j = 0; j < 20; j++)
		{
			list.Shuffle<float>();
			int num = 0;
			foreach (SpaceDestination spaceDestination in this.destinations)
			{
				if (spaceDestination.distance == j)
				{
					num++;
					spaceDestination.startingOrbitPercentage = list[num];
				}
			}
		}
		this.destinationsGenerated = true;
	}

	public SpaceDestination GetSpacecraftDestination(LaunchConditionManager lcm)
	{
		Spacecraft spacecraftFromLaunchConditionManager = this.GetSpacecraftFromLaunchConditionManager(lcm);
		return this.GetSpacecraftDestination(spacecraftFromLaunchConditionManager.id);
	}

	public SpaceDestination GetSpacecraftDestination(int spacecraftID)
	{
		this.CleanSavedSpacecraftDestinations();
		if (this.savedSpacecraftDestinations.ContainsKey(spacecraftID))
		{
			return this.GetDestination(this.savedSpacecraftDestinations[spacecraftID]);
		}
		return null;
	}

	public List<int> GetSpacecraftsForDestination(SpaceDestination destination)
	{
		this.CleanSavedSpacecraftDestinations();
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, int> keyValuePair in this.savedSpacecraftDestinations)
		{
			if (keyValuePair.Value == destination.id)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	private void CleanSavedSpacecraftDestinations()
	{
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, int> keyValuePair in this.savedSpacecraftDestinations)
		{
			bool flag = false;
			using (List<Spacecraft>.Enumerator enumerator2 = this.spacecraft.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.id == keyValuePair.Key)
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2 = false;
			using (List<SpaceDestination>.Enumerator enumerator3 = this.destinations.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					if (enumerator3.Current.id == keyValuePair.Value)
					{
						flag2 = true;
						break;
					}
				}
			}
			if (!flag || !flag2)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (int key in list)
		{
			this.savedSpacecraftDestinations.Remove(key);
		}
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.spacecraftManager = this;
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			global::Debug.Assert(this.spacecraft == null || this.spacecraft.Count == 0);
			return;
		}
		this.RestoreDestinations();
	}

	public void SetSpacecraftDestination(LaunchConditionManager lcm, SpaceDestination destination)
	{
		Spacecraft spacecraftFromLaunchConditionManager = this.GetSpacecraftFromLaunchConditionManager(lcm);
		this.savedSpacecraftDestinations[spacecraftFromLaunchConditionManager.id] = destination.id;
	}

	public int GetSpacecraftID(ILaunchableRocket rocket)
	{
		foreach (Spacecraft spacecraft in this.spacecraft)
		{
			if (spacecraft.launchConditions.gameObject == rocket.LaunchableGameObject)
			{
				return spacecraft.id;
			}
		}
		return -1;
	}

	public SpaceDestination GetDestination(int destinationID)
	{
		foreach (SpaceDestination spaceDestination in this.destinations)
		{
			if (spaceDestination.id == destinationID)
			{
				return spaceDestination;
			}
		}
		global::Debug.LogErrorFormat("No space destination with ID {0}", new object[]
		{
			destinationID
		});
		return null;
	}

	public void RegisterSpacecraft(Spacecraft craft)
	{
		if (this.spacecraft.Contains(craft))
		{
			return;
		}
		if (craft.HasInvalidID())
		{
			craft.SetID(this.nextSpacecraftID);
			this.nextSpacecraftID++;
		}
		this.spacecraft.Add(craft);
	}

	public void UnregisterSpacecraft(LaunchConditionManager conditionManager)
	{
		Spacecraft spacecraftFromLaunchConditionManager = this.GetSpacecraftFromLaunchConditionManager(conditionManager);
		spacecraftFromLaunchConditionManager.SetState(Spacecraft.MissionState.Destroyed);
		this.spacecraft.Remove(spacecraftFromLaunchConditionManager);
	}

	public List<Spacecraft> GetSpacecraft()
	{
		return this.spacecraft;
	}

	public Spacecraft GetSpacecraftFromLaunchConditionManager(LaunchConditionManager lcm)
	{
		foreach (Spacecraft spacecraft in this.spacecraft)
		{
			if (spacecraft.launchConditions == lcm)
			{
				return spacecraft;
			}
		}
		return null;
	}

	public void Sim1000ms(float dt)
	{
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			return;
		}
		foreach (Spacecraft spacecraft in this.spacecraft)
		{
			spacecraft.ProgressMission(dt);
		}
		foreach (SpaceDestination spaceDestination in this.destinations)
		{
			spaceDestination.Replenish(dt);
		}
	}

	public void PushReadyToLandNotification(Spacecraft spacecraft)
	{
		Notification notification = new Notification(BUILDING.STATUSITEMS.SPACECRAFTREADYTOLAND.NOTIFICATION, NotificationType.Good, delegate(List<Notification> notificationList, object data)
		{
			string text = BUILDING.STATUSITEMS.SPACECRAFTREADYTOLAND.NOTIFICATION_TOOLTIP;
			foreach (Notification notification2 in notificationList)
			{
				text = text + "\n" + (string)notification2.tooltipData;
			}
			return text;
		}, "• " + spacecraft.rocketName, true, 0f, null, null, null, true, false, false);
		spacecraft.launchConditions.gameObject.AddOrGet<Notifier>().Add(notification, "");
	}

	private void SpawnMissionResults(Dictionary<SimHashes, float> results)
	{
		foreach (KeyValuePair<SimHashes, float> keyValuePair in results)
		{
			ElementLoader.FindElementByHash(keyValuePair.Key).substance.SpawnResource(PlayerController.GetCursorPos(KInputManager.GetMousePos()), keyValuePair.Value, 300f, 0, 0, false, false, false);
		}
	}

	public float GetDestinationAnalysisScore(SpaceDestination destination)
	{
		return this.GetDestinationAnalysisScore(destination.id);
	}

	public float GetDestinationAnalysisScore(int destinationID)
	{
		if (this.destinationAnalysisScores.ContainsKey(destinationID))
		{
			return this.destinationAnalysisScores[destinationID];
		}
		return 0f;
	}

	public void EarnDestinationAnalysisPoints(int destinationID, float points)
	{
		if (!this.destinationAnalysisScores.ContainsKey(destinationID))
		{
			this.destinationAnalysisScores.Add(destinationID, 0f);
		}
		SpaceDestination destination = this.GetDestination(destinationID);
		SpacecraftManager.DestinationAnalysisState destinationAnalysisState = this.GetDestinationAnalysisState(destination);
		Dictionary<int, float> dictionary = this.destinationAnalysisScores;
		dictionary[destinationID] += points;
		SpacecraftManager.DestinationAnalysisState destinationAnalysisState2 = this.GetDestinationAnalysisState(destination);
		if (destinationAnalysisState != destinationAnalysisState2)
		{
			int starmapAnalysisDestinationID = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
			if (starmapAnalysisDestinationID == destinationID)
			{
				if (destinationAnalysisState2 == SpacecraftManager.DestinationAnalysisState.Complete)
				{
					if (SpacecraftManager.instance.GetDestination(starmapAnalysisDestinationID).type == Db.Get().SpaceDestinationTypes.Earth.Id)
					{
						Game.Instance.unlocks.Unlock("earth", true);
					}
					if (SpacecraftManager.instance.GetDestination(starmapAnalysisDestinationID).type == Db.Get().SpaceDestinationTypes.Wormhole.Id)
					{
						Game.Instance.unlocks.Unlock("wormhole", true);
					}
					SpacecraftManager.instance.SetStarmapAnalysisDestinationID(-1);
				}
				base.Trigger(532901469, null);
			}
		}
	}

	public SpacecraftManager.DestinationAnalysisState GetDestinationAnalysisState(SpaceDestination destination)
	{
		if (destination.startAnalyzed)
		{
			return SpacecraftManager.DestinationAnalysisState.Complete;
		}
		float destinationAnalysisScore = this.GetDestinationAnalysisScore(destination);
		if (destinationAnalysisScore >= (float)ROCKETRY.DESTINATION_ANALYSIS.COMPLETE)
		{
			return SpacecraftManager.DestinationAnalysisState.Complete;
		}
		if (destinationAnalysisScore >= (float)ROCKETRY.DESTINATION_ANALYSIS.DISCOVERED)
		{
			return SpacecraftManager.DestinationAnalysisState.Discovered;
		}
		return SpacecraftManager.DestinationAnalysisState.Hidden;
	}

	public bool AreAllDestinationsAnalyzed()
	{
		foreach (SpaceDestination destination in this.destinations)
		{
			if (this.GetDestinationAnalysisState(destination) != SpacecraftManager.DestinationAnalysisState.Complete)
			{
				return false;
			}
		}
		return true;
	}

	public void DEBUG_RevealStarmap()
	{
		foreach (SpaceDestination spaceDestination in this.destinations)
		{
			this.EarnDestinationAnalysisPoints(spaceDestination.id, (float)ROCKETRY.DESTINATION_ANALYSIS.COMPLETE);
		}
	}

	public void SetStarmapAnalysisDestinationID(int id)
	{
		this.analyzeDestinationID = id;
		base.Trigger(532901469, id);
	}

	public int GetStarmapAnalysisDestinationID()
	{
		return this.analyzeDestinationID;
	}

	public bool HasAnalysisTarget()
	{
		return this.analyzeDestinationID != -1;
	}

	[CompilerGenerated]
	internal static int <GenerateRandomDestinations>g__GetNextID|12_0(ref SpacecraftManager.<>c__DisplayClass12_0 A_0)
	{
		int nextId = A_0.nextId;
		A_0.nextId = nextId + 1;
		return nextId;
	}

	public static SpacecraftManager instance;

	[Serialize]
	private List<Spacecraft> spacecraft = new List<Spacecraft>();

	[Serialize]
	private int nextSpacecraftID;

	public const int INVALID_DESTINATION_ID = -1;

	[Serialize]
	private int analyzeDestinationID = -1;

	[Serialize]
	public bool hasVisitedWormHole;

	[Serialize]
	public List<SpaceDestination> destinations;

	[Serialize]
	public Dictionary<int, int> savedSpacecraftDestinations;

	[Serialize]
	public bool destinationsGenerated;

	[Serialize]
	public Dictionary<int, float> destinationAnalysisScores = new Dictionary<int, float>();

	public enum DestinationAnalysisState
	{
		Hidden,
		Discovered,
		Complete
	}
}
