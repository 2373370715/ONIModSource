using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001940 RID: 6464
[SerializationConfig(MemberSerialization.OptIn)]
public class Spacecraft
{
	// Token: 0x060086B3 RID: 34483 RVA: 0x000F82E1 File Offset: 0x000F64E1
	public Spacecraft(LaunchConditionManager launchConditions)
	{
		this.launchConditions = launchConditions;
	}

	// Token: 0x060086B4 RID: 34484 RVA: 0x000F8312 File Offset: 0x000F6512
	public Spacecraft()
	{
	}

	// Token: 0x170008E4 RID: 2276
	// (get) Token: 0x060086B5 RID: 34485 RVA: 0x000F833C File Offset: 0x000F653C
	// (set) Token: 0x060086B6 RID: 34486 RVA: 0x000F8349 File Offset: 0x000F6549
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

	// Token: 0x060086B7 RID: 34487 RVA: 0x000F8357 File Offset: 0x000F6557
	public void SetRocketName(string newName)
	{
		this.rocketName = newName;
		this.UpdateNameOnRocketModules();
	}

	// Token: 0x060086B8 RID: 34488 RVA: 0x000F8366 File Offset: 0x000F6566
	public string GetRocketName()
	{
		return this.rocketName;
	}

	// Token: 0x060086B9 RID: 34489 RVA: 0x0034DE14 File Offset: 0x0034C014
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

	// Token: 0x060086BA RID: 34490 RVA: 0x000F836E File Offset: 0x000F656E
	public bool HasInvalidID()
	{
		return this.id == -1;
	}

	// Token: 0x060086BB RID: 34491 RVA: 0x000F8379 File Offset: 0x000F6579
	public void SetID(int id)
	{
		this.id = id;
	}

	// Token: 0x060086BC RID: 34492 RVA: 0x000F8382 File Offset: 0x000F6582
	public void SetState(Spacecraft.MissionState state)
	{
		this.state = state;
	}

	// Token: 0x060086BD RID: 34493 RVA: 0x000F838B File Offset: 0x000F658B
	public void BeginMission(SpaceDestination destination)
	{
		this.missionElapsed = 0f;
		this.missionDuration = (float)destination.OneBasedDistance * ROCKETRY.MISSION_DURATION_SCALE / this.GetPilotNavigationEfficiency();
		this.SetState(Spacecraft.MissionState.Launching);
	}

	// Token: 0x060086BE RID: 34494 RVA: 0x0034DE84 File Offset: 0x0034C084
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

	// Token: 0x060086BF RID: 34495 RVA: 0x000F83B9 File Offset: 0x000F65B9
	public void ForceComplete()
	{
		this.missionElapsed = this.missionDuration;
	}

	// Token: 0x060086C0 RID: 34496 RVA: 0x0034E004 File Offset: 0x0034C204
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

	// Token: 0x060086C1 RID: 34497 RVA: 0x000F83C7 File Offset: 0x000F65C7
	public float GetTimeLeft()
	{
		return this.missionDuration - this.missionElapsed;
	}

	// Token: 0x060086C2 RID: 34498 RVA: 0x000F83D6 File Offset: 0x000F65D6
	public float GetDuration()
	{
		return this.missionDuration;
	}

	// Token: 0x060086C3 RID: 34499 RVA: 0x000F83DE File Offset: 0x000F65DE
	public void CompleteMission()
	{
		SpacecraftManager.instance.PushReadyToLandNotification(this);
		this.SetState(Spacecraft.MissionState.WaitingToLand);
		this.Land();
	}

	// Token: 0x060086C4 RID: 34500 RVA: 0x0034E078 File Offset: 0x0034C278
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

	// Token: 0x060086C5 RID: 34501 RVA: 0x0034E11C File Offset: 0x0034C31C
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

	// Token: 0x060086C6 RID: 34502 RVA: 0x000F83F8 File Offset: 0x000F65F8
	public void GenerateName()
	{
		this.SetRocketName(GameUtil.GenerateRandomRocketName());
	}

	// Token: 0x040065BA RID: 26042
	[Serialize]
	public int id = -1;

	// Token: 0x040065BB RID: 26043
	[Serialize]
	public string rocketName = UI.STARMAP.DEFAULT_NAME;

	// Token: 0x040065BC RID: 26044
	[Serialize]
	public float controlStationBuffTimeRemaining;

	// Token: 0x040065BD RID: 26045
	[Serialize]
	public Ref<LaunchConditionManager> refLaunchConditions = new Ref<LaunchConditionManager>();

	// Token: 0x040065BE RID: 26046
	[Serialize]
	public Spacecraft.MissionState state;

	// Token: 0x040065BF RID: 26047
	[Serialize]
	private float missionElapsed;

	// Token: 0x040065C0 RID: 26048
	[Serialize]
	private float missionDuration;

	// Token: 0x02001941 RID: 6465
	public enum MissionState
	{
		// Token: 0x040065C2 RID: 26050
		Grounded,
		// Token: 0x040065C3 RID: 26051
		Launching,
		// Token: 0x040065C4 RID: 26052
		Underway,
		// Token: 0x040065C5 RID: 26053
		WaitingToLand,
		// Token: 0x040065C6 RID: 26054
		Landing,
		// Token: 0x040065C7 RID: 26055
		Destroyed
	}
}
