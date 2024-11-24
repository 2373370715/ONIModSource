using System.Collections.Generic;
using Klei.CustomSettings;
using KSerialization;
using UnityEngine;

namespace Klei.AI;

public class MeteorShowerEvent : GameplayEvent<MeteorShowerEvent.StatesInstance>
{
	public struct BombardmentInfo
	{
		public string prefab;

		public float weight;
	}

	public class States : GameplayEventStateMachine<States, StatesInstance, GameplayEventManager, MeteorShowerEvent>
	{
		public class ClusterMapStates : State
		{
			public State travelling;

			public State arrive;
		}

		public class RunningStates : State
		{
			public State bombarding;

			public State snoozing;
		}

		public ClusterMapStates starMap;

		public State planning;

		public RunningStates running;

		public State finished;

		public TargetParameter clusterMapMeteorShower;

		public FloatParameter runTimeRemaining;

		public FloatParameter bombardTimeRemaining;

		public FloatParameter snoozeTimeRemaining;

		public Signal OnClusterMapDestinationReached;

		public override void InitializeStates(out BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = planning;
			base.serializable = SerializeType.Both_DEPRECATED;
			planning.Enter(delegate(StatesInstance smi)
			{
				runTimeRemaining.Set(smi.gameplayEvent.duration, smi);
				bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi);
				snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi);
				if (smi.gameplayEvent.canStarTravel && smi.clusterTravelDuration > 0f)
				{
					smi.GoTo(smi.sm.starMap);
				}
				else
				{
					smi.GoTo(smi.sm.running);
				}
			});
			starMap.Enter(CreateClusterMapMeteorShower).DefaultState(starMap.travelling);
			starMap.travelling.OnSignal(OnClusterMapDestinationReached, starMap.arrive);
			starMap.arrive.GoTo(running.bombarding);
			running.DefaultState(running.snoozing).Update(delegate(StatesInstance smi, float dt)
			{
				runTimeRemaining.Delta(0f - dt, smi);
			}).ParamTransition(runTimeRemaining, finished, GameStateMachine<States, StatesInstance, GameplayEventManager, object>.IsLTEZero);
			running.bombarding.Enter(delegate(StatesInstance smi)
			{
				TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateBegins);
			}).Exit(delegate(StatesInstance smi)
			{
				TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateEnds);
			}).Enter(delegate(StatesInstance smi)
			{
				smi.StartBackgroundEffects();
			})
				.Exit(delegate(StatesInstance smi)
				{
					smi.StopBackgroundEffects();
				})
				.Exit(delegate(StatesInstance smi)
				{
					bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi);
				})
				.Update(delegate(StatesInstance smi, float dt)
				{
					bombardTimeRemaining.Delta(0f - dt, smi);
				})
				.ParamTransition(bombardTimeRemaining, running.snoozing, GameStateMachine<States, StatesInstance, GameplayEventManager, object>.IsLTEZero)
				.Update(delegate(StatesInstance smi, float dt)
				{
					smi.Bombarding(dt);
				});
			running.snoozing.Exit(delegate(StatesInstance smi)
			{
				snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi);
			}).Update(delegate(StatesInstance smi, float dt)
			{
				snoozeTimeRemaining.Delta(0f - dt, smi);
			}).ParamTransition(snoozeTimeRemaining, running.bombarding, GameStateMachine<States, StatesInstance, GameplayEventManager, object>.IsLTEZero);
			finished.ReturnSuccess();
		}

		public static void TriggerMeteorGlobalEvent(StatesInstance smi, GameHashes hash)
		{
			Game.Instance.Trigger((int)hash, smi.eventInstance.worldId);
		}

		public static void CreateClusterMapMeteorShower(StatesInstance smi)
		{
			if (smi.sm.clusterMapMeteorShower.Get(smi) == null)
			{
				GameObject prefab = Assets.GetPrefab(smi.gameplayEvent.clusterMapMeteorShowerID.ToTag());
				float arrivalTime = smi.eventInstance.eventStartTime * 600f + smi.clusterTravelDuration;
				AxialI randomCellAtEdgeOfUniverse = ClusterGrid.Instance.GetRandomCellAtEdgeOfUniverse();
				GameObject gameObject = Util.KInstantiate(prefab);
				gameObject.GetComponent<ClusterMapMeteorShowerVisualizer>().SetInitialLocation(randomCellAtEdgeOfUniverse);
				ClusterMapMeteorShower.Def def = gameObject.AddOrGetDef<ClusterMapMeteorShower.Def>();
				def.destinationWorldID = smi.eventInstance.worldId;
				def.arrivalTime = arrivalTime;
				gameObject.SetActive(value: true);
				smi.sm.clusterMapMeteorShower.Set(gameObject, smi);
			}
			GameObject go = smi.sm.clusterMapMeteorShower.Get(smi);
			go.GetDef<ClusterMapMeteorShower.Def>();
			go.Subscribe(1796608350, smi.OnClusterMapDestinationReached);
		}
	}

	public class StatesInstance : GameplayEventStateMachine<States, StatesInstance, GameplayEventManager, MeteorShowerEvent>.GameplayEventStateMachineInstance
	{
		public GameObject activeMeteorBackground;

		[Serialize]
		public float clusterTravelDuration = -1f;

		[Serialize]
		private float nextMeteorTime;

		[Serialize]
		private int m_worldId;

		private WorldContainer world;

		private SettingLevel difficultyLevel;

		public float GetSleepTimerValue()
		{
			return Mathf.Clamp(GameplayEventManager.Instance.GetSleepTimer(gameplayEvent) - GameUtil.GetCurrentTimeInCycles(), 0f, float.MaxValue);
		}

		public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, MeteorShowerEvent meteorShowerEvent)
			: base(master, eventInstance, meteorShowerEvent)
		{
			world = ClusterManager.Instance.GetWorld(m_worldId);
			difficultyLevel = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
			m_worldId = eventInstance.worldId;
			Game.Instance.Subscribe(1983128072, OnActiveWorldChanged);
		}

		public void OnClusterMapDestinationReached(object obj)
		{
			base.smi.sm.OnClusterMapDestinationReached.Trigger(this);
		}

		private void OnActiveWorldChanged(object data)
		{
			int first = ((Tuple<int, int>)data).first;
			if (activeMeteorBackground != null)
			{
				activeMeteorBackground.GetComponent<ParticleSystemRenderer>().enabled = first == m_worldId;
			}
		}

		public override void StopSM(string reason)
		{
			StopBackgroundEffects();
			base.StopSM(reason);
		}

		protected override void OnCleanUp()
		{
			Game.Instance.Unsubscribe(1983128072, OnActiveWorldChanged);
			DestroyClusterMapMeteorShowerObject();
			base.OnCleanUp();
		}

		private void DestroyClusterMapMeteorShowerObject()
		{
			if (base.sm.clusterMapMeteorShower.Get(this) != null)
			{
				ClusterMapMeteorShower.Instance sMI = base.sm.clusterMapMeteorShower.Get(this).GetSMI<ClusterMapMeteorShower.Instance>();
				if (sMI != null)
				{
					sMI.StopSM("Event is being aborted");
					Util.KDestroyGameObject(sMI.gameObject);
				}
			}
		}

		public void StartBackgroundEffects()
		{
			if (activeMeteorBackground == null)
			{
				activeMeteorBackground = Util.KInstantiate(EffectPrefabs.Instance.MeteorBackground);
				float x = (world.maximumBounds.x + world.minimumBounds.x) / 2f;
				float y = world.maximumBounds.y;
				float z = 25f;
				activeMeteorBackground.transform.SetPosition(new Vector3(x, y, z));
				activeMeteorBackground.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
			}
		}

		public void StopBackgroundEffects()
		{
			if (activeMeteorBackground != null)
			{
				ParticleSystem component = activeMeteorBackground.GetComponent<ParticleSystem>();
				ParticleSystem.MainModule main = component.main;
				main.stopAction = ParticleSystemStopAction.Destroy;
				component.Stop();
				if (!component.IsAlive())
				{
					Object.Destroy(activeMeteorBackground);
				}
				activeMeteorBackground = null;
			}
		}

		public float TimeUntilNextShower()
		{
			if (IsInsideState(base.sm.running.bombarding))
			{
				return 0f;
			}
			if (IsInsideState(base.sm.starMap))
			{
				float num = base.smi.eventInstance.eventStartTime * 600f + base.smi.clusterTravelDuration - GameUtil.GetCurrentTimeInCycles() * 600f;
				if (!(num < 0f))
				{
					return num;
				}
				return 0f;
			}
			return base.sm.snoozeTimeRemaining.Get(this);
		}

		public void Bombarding(float dt)
		{
			nextMeteorTime -= dt;
			while (nextMeteorTime < 0f)
			{
				if (!(GetSleepTimerValue() > 0f))
				{
					DoBombardment(gameplayEvent.bombardmentInfo);
				}
				nextMeteorTime += GetNextMeteorTime();
			}
		}

		private void DoBombardment(List<BombardmentInfo> bombardment_info)
		{
			float num = 0f;
			foreach (BombardmentInfo item in bombardment_info)
			{
				num += item.weight;
			}
			num = Random.Range(0f, num);
			BombardmentInfo bombardmentInfo = bombardment_info[0];
			int num2 = 0;
			while (num - bombardmentInfo.weight > 0f)
			{
				num -= bombardmentInfo.weight;
				bombardmentInfo = bombardment_info[++num2];
			}
			Game.Instance.Trigger(-84771526);
			SpawnBombard(bombardmentInfo.prefab);
		}

		private GameObject SpawnBombard(string prefab)
		{
			WorldContainer worldContainer = ClusterManager.Instance.GetWorld(m_worldId);
			float x = (float)(worldContainer.Width - 1) * Random.value + (float)worldContainer.WorldOffset.x;
			float y = worldContainer.Height + worldContainer.WorldOffset.y - 1;
			float layerZ = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
			Vector3 position = new Vector3(x, y, layerZ);
			GameObject prefab2 = Assets.GetPrefab(prefab);
			if (prefab2 == null)
			{
				return null;
			}
			GameObject obj = Util.KInstantiate(prefab2, position, Quaternion.identity);
			Comet component = obj.GetComponent<Comet>();
			if (component != null)
			{
				component.spawnWithOffset = true;
			}
			obj.SetActive(value: true);
			return obj;
		}

		public float BombardTimeRemaining()
		{
			return Mathf.Min(base.sm.bombardTimeRemaining.Get(this), base.sm.runTimeRemaining.Get(this));
		}

		public float GetBombardOffTime()
		{
			float num = gameplayEvent.secondsBombardmentOff.Get();
			if (gameplayEvent.affectedByDifficulty && difficultyLevel != null)
			{
				switch (difficultyLevel.id)
				{
				case "Infrequent":
					num *= 1f;
					break;
				case "Intense":
					num *= 1f;
					break;
				case "Doomed":
					num *= 0.5f;
					break;
				}
			}
			return num;
		}

		public float GetBombardOnTime()
		{
			float num = gameplayEvent.secondsBombardmentOn.Get();
			if (gameplayEvent.affectedByDifficulty && difficultyLevel != null)
			{
				switch (difficultyLevel.id)
				{
				case "Infrequent":
					num *= 1f;
					break;
				case "Intense":
					num *= 1f;
					break;
				case "Doomed":
					num *= 1f;
					break;
				}
			}
			return num;
		}

		private float GetNextMeteorTime()
		{
			float secondsPerMeteor = gameplayEvent.secondsPerMeteor;
			secondsPerMeteor *= 256f / (float)world.Width;
			if (gameplayEvent.affectedByDifficulty && difficultyLevel != null)
			{
				switch (difficultyLevel.id)
				{
				case "Infrequent":
					secondsPerMeteor *= 1.5f;
					break;
				case "Intense":
					secondsPerMeteor *= 0.8f;
					break;
				case "Doomed":
					secondsPerMeteor *= 0.5f;
					break;
				}
			}
			return secondsPerMeteor;
		}
	}

	private List<BombardmentInfo> bombardmentInfo;

	private MathUtil.MinMax secondsBombardmentOff;

	private MathUtil.MinMax secondsBombardmentOn;

	private float secondsPerMeteor = 0.33f;

	private float duration;

	private string clusterMapMeteorShowerID;

	private bool affectedByDifficulty = true;

	public bool canStarTravel
	{
		get
		{
			if (clusterMapMeteorShowerID != null)
			{
				return DlcManager.FeatureClusterSpaceEnabled();
			}
			return false;
		}
	}

	public string GetClusterMapMeteorShowerID()
	{
		return clusterMapMeteorShowerID;
	}

	public List<BombardmentInfo> GetMeteorsInfo()
	{
		return new List<BombardmentInfo>(bombardmentInfo);
	}

	public MeteorShowerEvent(string id, float duration, float secondsPerMeteor, MathUtil.MinMax secondsBombardmentOff = default(MathUtil.MinMax), MathUtil.MinMax secondsBombardmentOn = default(MathUtil.MinMax), string clusterMapMeteorShowerID = null, bool affectedByDifficulty = true)
		: base(id, 0, 0)
	{
		allowMultipleEventInstances = true;
		this.clusterMapMeteorShowerID = clusterMapMeteorShowerID;
		this.duration = duration;
		this.secondsPerMeteor = secondsPerMeteor;
		this.secondsBombardmentOff = secondsBombardmentOff;
		this.secondsBombardmentOn = secondsBombardmentOn;
		this.affectedByDifficulty = affectedByDifficulty;
		bombardmentInfo = new List<BombardmentInfo>();
		tags.Add(GameTags.SpaceDanger);
	}

	public MeteorShowerEvent AddMeteor(string prefab, float weight)
	{
		bombardmentInfo.Add(new BombardmentInfo
		{
			prefab = prefab,
			weight = weight
		});
		return this;
	}

	public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
	{
		return new StatesInstance(manager, eventInstance, this);
	}

	public override bool IsAllowed()
	{
		if (base.IsAllowed())
		{
			if (affectedByDifficulty)
			{
				return CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers).id != "ClearSkies";
			}
			return true;
		}
		return false;
	}
}
