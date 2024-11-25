﻿using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Spacecraft
{
		public Spacecraft(LaunchConditionManager launchConditions)
	{
		this.launchConditions = launchConditions;
	}

		public Spacecraft()
	{
	}

				public LaunchConditionManager launchConditions
	{
		get
		{
			return this.refLaunchConditions.Get();
		}
		set
		{
			this.refLaunchConditions.Set(value);
		}
	}

		public void SetRocketName(string newName)
	{
		this.rocketName = newName;
		this.UpdateNameOnRocketModules();
	}

		public string GetRocketName()
	{
		return this.rocketName;
	}

		public void UpdateNameOnRocketModules()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
		{
			RocketModule component = gameObject.GetComponent<RocketModule>();
			if (component != null)
			{
				component.SetParentRocketName(this.rocketName);
			}
		}
	}

		public bool HasInvalidID()
	{
		return this.id == -1;
	}

		public void SetID(int id)
	{
		this.id = id;
	}

		public void SetState(Spacecraft.MissionState state)
	{
		this.state = state;
	}

		public void BeginMission(SpaceDestination destination)
	{
		this.missionElapsed = 0f;
		this.missionDuration = (float)destination.OneBasedDistance * ROCKETRY.MISSION_DURATION_SCALE / this.GetPilotNavigationEfficiency();
		this.SetState(Spacecraft.MissionState.Launching);
	}

		private float GetPilotNavigationEfficiency()
	{
		float num = 1f;
		if (!this.launchConditions.GetComponent<CommandModule>().robotPilotControlled)
		{
			List<MinionStorage.Info> storedMinionInfo = this.launchConditions.GetComponent<MinionStorage>().GetStoredMinionInfo();
			if (storedMinionInfo.Count < 1)
			{
				return 1f;
			}
			StoredMinionIdentity component = storedMinionInfo[0].serializedMinion.Get().GetComponent<StoredMinionIdentity>();
			string b = Db.Get().Attributes.SpaceNavigation.Id;
			using (Dictionary<string, bool>.Enumerator enumerator = component.MasteryBySkillID.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, bool> keyValuePair = enumerator.Current;
					foreach (SkillPerk skillPerk in Db.Get().Skills.Get(keyValuePair.Key).perks)
					{
						if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(skillPerk.requiredDlcIds))
						{
							SkillAttributePerk skillAttributePerk = skillPerk as SkillAttributePerk;
							if (skillAttributePerk != null && skillAttributePerk.modifier.AttributeId == b)
							{
								num += skillAttributePerk.modifier.Value;
							}
						}
					}
				}
				return num;
			}
		}
		RoboPilotModule component2 = this.launchConditions.GetComponent<RoboPilotModule>();
		if (component2 != null && component2.GetDataBanksStored() >= 1f)
		{
			num += component2.FlightEfficiencyModifier();
		}
		return num;
	}

		public void ForceComplete()
	{
		this.missionElapsed = this.missionDuration;
	}

		public void ProgressMission(float deltaTime)
	{
		if (this.state == Spacecraft.MissionState.Underway)
		{
			this.missionElapsed += deltaTime;
			if (this.controlStationBuffTimeRemaining > 0f)
			{
				this.missionElapsed += deltaTime * 0.20000005f;
				this.controlStationBuffTimeRemaining -= deltaTime;
			}
			else
			{
				this.controlStationBuffTimeRemaining = 0f;
			}
			if (this.missionElapsed > this.missionDuration)
			{
				this.CompleteMission();
			}
		}
	}

		public float GetTimeLeft()
	{
		return this.missionDuration - this.missionElapsed;
	}

		public float GetDuration()
	{
		return this.missionDuration;
	}

		public void CompleteMission()
	{
		SpacecraftManager.instance.PushReadyToLandNotification(this);
		this.SetState(Spacecraft.MissionState.WaitingToLand);
		this.Land();
	}

		private void Land()
	{
		this.launchConditions.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(this.id));
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.launchConditions.GetComponent<AttachableBuilding>()))
		{
			if (gameObject != this.launchConditions.gameObject)
			{
				gameObject.Trigger(-1165815793, SpacecraftManager.instance.GetSpacecraftDestination(this.id));
			}
		}
	}

		public void TemporallyTear()
	{
		SpacecraftManager.instance.hasVisitedWormHole = true;
		LaunchConditionManager launchConditions = this.launchConditions;
		for (int i = launchConditions.rocketModules.Count - 1; i >= 0; i--)
		{
			Storage component = launchConditions.rocketModules[i].GetComponent<Storage>();
			if (component != null)
			{
				component.ConsumeAllIgnoringDisease();
			}
			MinionStorage component2 = launchConditions.rocketModules[i].GetComponent<MinionStorage>();
			if (component2 != null)
			{
				List<MinionStorage.Info> storedMinionInfo = component2.GetStoredMinionInfo();
				for (int j = storedMinionInfo.Count - 1; j >= 0; j--)
				{
					component2.DeleteStoredMinion(storedMinionInfo[j].id);
				}
			}
			Util.KDestroyGameObject(launchConditions.rocketModules[i].gameObject);
		}
	}

		public void GenerateName()
	{
		this.SetRocketName(GameUtil.GenerateRandomRocketName());
	}

		[Serialize]
	public int id = -1;

		[Serialize]
	public string rocketName = UI.STARMAP.DEFAULT_NAME;

		[Serialize]
	public float controlStationBuffTimeRemaining;

		[Serialize]
	public Ref<LaunchConditionManager> refLaunchConditions = new Ref<LaunchConditionManager>();

		[Serialize]
	public Spacecraft.MissionState state;

		[Serialize]
	private float missionElapsed;

		[Serialize]
	private float missionDuration;

		public enum MissionState
	{
				Grounded,
				Launching,
				Underway,
				WaitingToLand,
				Landing,
				Destroyed
	}
}
