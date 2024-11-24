using System;
using System.Collections.Generic;
using Klei.CustomSettings;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003B55 RID: 15189
	public class MeteorShowerEvent : GameplayEvent<MeteorShowerEvent.StatesInstance>
	{
		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x0600E9DE RID: 59870 RVA: 0x0013C48F File Offset: 0x0013A68F
		public bool canStarTravel
		{
			get
			{
				return this.clusterMapMeteorShowerID != null && DlcManager.FeatureClusterSpaceEnabled();
			}
		}

		// Token: 0x0600E9DF RID: 59871 RVA: 0x0013C4A0 File Offset: 0x0013A6A0
		public string GetClusterMapMeteorShowerID()
		{
			return this.clusterMapMeteorShowerID;
		}

		// Token: 0x0600E9E0 RID: 59872 RVA: 0x0013C4A8 File Offset: 0x0013A6A8
		public List<MeteorShowerEvent.BombardmentInfo> GetMeteorsInfo()
		{
			return new List<MeteorShowerEvent.BombardmentInfo>(this.bombardmentInfo);
		}

		// Token: 0x0600E9E1 RID: 59873 RVA: 0x004C83C4 File Offset: 0x004C65C4
		public MeteorShowerEvent(string id, float duration, float secondsPerMeteor, MathUtil.MinMax secondsBombardmentOff = default(MathUtil.MinMax), MathUtil.MinMax secondsBombardmentOn = default(MathUtil.MinMax), string clusterMapMeteorShowerID = null, bool affectedByDifficulty = true) : base(id, 0, 0)
		{
			this.allowMultipleEventInstances = true;
			this.clusterMapMeteorShowerID = clusterMapMeteorShowerID;
			this.duration = duration;
			this.secondsPerMeteor = secondsPerMeteor;
			this.secondsBombardmentOff = secondsBombardmentOff;
			this.secondsBombardmentOn = secondsBombardmentOn;
			this.affectedByDifficulty = affectedByDifficulty;
			this.bombardmentInfo = new List<MeteorShowerEvent.BombardmentInfo>();
			this.tags.Add(GameTags.SpaceDanger);
		}

		// Token: 0x0600E9E2 RID: 59874 RVA: 0x004C843C File Offset: 0x004C663C
		public MeteorShowerEvent AddMeteor(string prefab, float weight)
		{
			this.bombardmentInfo.Add(new MeteorShowerEvent.BombardmentInfo
			{
				prefab = prefab,
				weight = weight
			});
			return this;
		}

		// Token: 0x0600E9E3 RID: 59875 RVA: 0x0013C4B5 File Offset: 0x0013A6B5
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new MeteorShowerEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0600E9E4 RID: 59876 RVA: 0x0013C4BF File Offset: 0x0013A6BF
		public override bool IsAllowed()
		{
			return base.IsAllowed() && (!this.affectedByDifficulty || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers).id != "ClearSkies");
		}

		// Token: 0x0400E55A RID: 58714
		private List<MeteorShowerEvent.BombardmentInfo> bombardmentInfo;

		// Token: 0x0400E55B RID: 58715
		private MathUtil.MinMax secondsBombardmentOff;

		// Token: 0x0400E55C RID: 58716
		private MathUtil.MinMax secondsBombardmentOn;

		// Token: 0x0400E55D RID: 58717
		private float secondsPerMeteor = 0.33f;

		// Token: 0x0400E55E RID: 58718
		private float duration;

		// Token: 0x0400E55F RID: 58719
		private string clusterMapMeteorShowerID;

		// Token: 0x0400E560 RID: 58720
		private bool affectedByDifficulty = true;

		// Token: 0x02003B56 RID: 15190
		public struct BombardmentInfo
		{
			// Token: 0x0400E561 RID: 58721
			public string prefab;

			// Token: 0x0400E562 RID: 58722
			public float weight;
		}

		// Token: 0x02003B57 RID: 15191
		public class States : GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>
		{
			// Token: 0x0600E9E5 RID: 59877 RVA: 0x004C8470 File Offset: 0x004C6670
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.planning.Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.runTimeRemaining.Set(smi.gameplayEvent.duration, smi, false);
					this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi, false);
					this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi, false);
					if (smi.gameplayEvent.canStarTravel && smi.clusterTravelDuration > 0f)
					{
						smi.GoTo(smi.sm.starMap);
						return;
					}
					smi.GoTo(smi.sm.running);
				});
				this.starMap.Enter(new StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State.Callback(MeteorShowerEvent.States.CreateClusterMapMeteorShower)).DefaultState(this.starMap.travelling);
				this.starMap.travelling.OnSignal(this.OnClusterMapDestinationReached, this.starMap.arrive);
				this.starMap.arrive.GoTo(this.running.bombarding);
				this.running.DefaultState(this.running.snoozing).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.runTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.runTimeRemaining, this.finished, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
				this.running.bombarding.Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateBegins);
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					MeteorShowerEvent.States.TriggerMeteorGlobalEvent(smi, GameHashes.MeteorShowerBombardStateEnds);
				}).Enter(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					smi.StartBackgroundEffects();
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					smi.StopBackgroundEffects();
				}).Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.bombardTimeRemaining.Set(smi.GetBombardOnTime(), smi, false);
				}).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.bombardTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.bombardTimeRemaining, this.running.snoozing, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					smi.Bombarding(dt);
				}, UpdateRate.SIM_200ms, false);
				this.running.snoozing.Exit(delegate(MeteorShowerEvent.StatesInstance smi)
				{
					this.snoozeTimeRemaining.Set(smi.GetBombardOffTime(), smi, false);
				}).Update(delegate(MeteorShowerEvent.StatesInstance smi, float dt)
				{
					this.snoozeTimeRemaining.Delta(-dt, smi);
				}, UpdateRate.SIM_200ms, false).ParamTransition<float>(this.snoozeTimeRemaining, this.running.bombarding, GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.IsLTEZero);
				this.finished.ReturnSuccess();
			}

			// Token: 0x0600E9E6 RID: 59878 RVA: 0x0013C4F3 File Offset: 0x0013A6F3
			public static void TriggerMeteorGlobalEvent(MeteorShowerEvent.StatesInstance smi, GameHashes hash)
			{
				Game.Instance.Trigger((int)hash, smi.eventInstance.worldId);
			}

			// Token: 0x0600E9E7 RID: 59879 RVA: 0x004C86AC File Offset: 0x004C68AC
			public static void CreateClusterMapMeteorShower(MeteorShowerEvent.StatesInstance smi)
			{
				if (smi.sm.clusterMapMeteorShower.Get(smi) == null)
				{
					GameObject prefab = Assets.GetPrefab(smi.gameplayEvent.clusterMapMeteorShowerID.ToTag());
					float arrivalTime = smi.eventInstance.eventStartTime * 600f + smi.clusterTravelDuration;
					AxialI randomCellAtEdgeOfUniverse = ClusterGrid.Instance.GetRandomCellAtEdgeOfUniverse();
					GameObject gameObject = Util.KInstantiate(prefab, null, null);
					gameObject.GetComponent<ClusterMapMeteorShowerVisualizer>().SetInitialLocation(randomCellAtEdgeOfUniverse);
					ClusterMapMeteorShower.Def def = gameObject.AddOrGetDef<ClusterMapMeteorShower.Def>();
					def.destinationWorldID = smi.eventInstance.worldId;
					def.arrivalTime = arrivalTime;
					gameObject.SetActive(true);
					smi.sm.clusterMapMeteorShower.Set(gameObject, smi, false);
				}
				GameObject go = smi.sm.clusterMapMeteorShower.Get(smi);
				go.GetDef<ClusterMapMeteorShower.Def>();
				go.Subscribe(1796608350, new Action<object>(smi.OnClusterMapDestinationReached));
			}

			// Token: 0x0400E563 RID: 58723
			public MeteorShowerEvent.States.ClusterMapStates starMap;

			// Token: 0x0400E564 RID: 58724
			public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State planning;

			// Token: 0x0400E565 RID: 58725
			public MeteorShowerEvent.States.RunningStates running;

			// Token: 0x0400E566 RID: 58726
			public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State finished;

			// Token: 0x0400E567 RID: 58727
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.TargetParameter clusterMapMeteorShower;

			// Token: 0x0400E568 RID: 58728
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter runTimeRemaining;

			// Token: 0x0400E569 RID: 58729
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter bombardTimeRemaining;

			// Token: 0x0400E56A RID: 58730
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.FloatParameter snoozeTimeRemaining;

			// Token: 0x0400E56B RID: 58731
			public StateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.Signal OnClusterMapDestinationReached;

			// Token: 0x02003B58 RID: 15192
			public class ClusterMapStates : GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400E56C RID: 58732
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State travelling;

				// Token: 0x0400E56D RID: 58733
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State arrive;
			}

			// Token: 0x02003B59 RID: 15193
			public class RunningStates : GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400E56E RID: 58734
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State bombarding;

				// Token: 0x0400E56F RID: 58735
				public GameStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, object>.State snoozing;
			}
		}

		// Token: 0x02003B5B RID: 15195
		public class StatesInstance : GameplayEventStateMachine<MeteorShowerEvent.States, MeteorShowerEvent.StatesInstance, GameplayEventManager, MeteorShowerEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600E9F8 RID: 59896 RVA: 0x0013C5BE File Offset: 0x0013A7BE
			public float GetSleepTimerValue()
			{
				return Mathf.Clamp(GameplayEventManager.Instance.GetSleepTimer(this.gameplayEvent) - GameUtil.GetCurrentTimeInCycles(), 0f, float.MaxValue);
			}

			// Token: 0x0600E9F9 RID: 59897 RVA: 0x004C8818 File Offset: 0x004C6A18
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, MeteorShowerEvent meteorShowerEvent) : base(master, eventInstance, meteorShowerEvent)
			{
				this.world = ClusterManager.Instance.GetWorld(this.m_worldId);
				this.difficultyLevel = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
				this.m_worldId = eventInstance.worldId;
				Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
			}

			// Token: 0x0600E9FA RID: 59898 RVA: 0x0013C5E5 File Offset: 0x0013A7E5
			public void OnClusterMapDestinationReached(object obj)
			{
				base.smi.sm.OnClusterMapDestinationReached.Trigger(this);
			}

			// Token: 0x0600E9FB RID: 59899 RVA: 0x004C888C File Offset: 0x004C6A8C
			private void OnActiveWorldChanged(object data)
			{
				int first = ((global::Tuple<int, int>)data).first;
				if (this.activeMeteorBackground != null)
				{
					this.activeMeteorBackground.GetComponent<ParticleSystemRenderer>().enabled = (first == this.m_worldId);
				}
			}

			// Token: 0x0600E9FC RID: 59900 RVA: 0x0013C5FD File Offset: 0x0013A7FD
			public override void StopSM(string reason)
			{
				this.StopBackgroundEffects();
				base.StopSM(reason);
			}

			// Token: 0x0600E9FD RID: 59901 RVA: 0x0013C60C File Offset: 0x0013A80C
			protected override void OnCleanUp()
			{
				Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
				this.DestroyClusterMapMeteorShowerObject();
				base.OnCleanUp();
			}

			// Token: 0x0600E9FE RID: 59902 RVA: 0x004C88CC File Offset: 0x004C6ACC
			private void DestroyClusterMapMeteorShowerObject()
			{
				if (base.sm.clusterMapMeteorShower.Get(this) != null)
				{
					ClusterMapMeteorShower.Instance smi = base.sm.clusterMapMeteorShower.Get(this).GetSMI<ClusterMapMeteorShower.Instance>();
					if (smi != null)
					{
						smi.StopSM("Event is being aborted");
						Util.KDestroyGameObject(smi.gameObject);
					}
				}
			}

			// Token: 0x0600E9FF RID: 59903 RVA: 0x004C8924 File Offset: 0x004C6B24
			public void StartBackgroundEffects()
			{
				if (this.activeMeteorBackground == null)
				{
					this.activeMeteorBackground = Util.KInstantiate(EffectPrefabs.Instance.MeteorBackground, null, null);
					float x = (this.world.maximumBounds.x + this.world.minimumBounds.x) / 2f;
					float y = this.world.maximumBounds.y;
					float z = 25f;
					this.activeMeteorBackground.transform.SetPosition(new Vector3(x, y, z));
					this.activeMeteorBackground.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
				}
			}

			// Token: 0x0600EA00 RID: 59904 RVA: 0x004C89D8 File Offset: 0x004C6BD8
			public void StopBackgroundEffects()
			{
				if (this.activeMeteorBackground != null)
				{
					ParticleSystem component = this.activeMeteorBackground.GetComponent<ParticleSystem>();
					component.main.stopAction = ParticleSystemStopAction.Destroy;
					component.Stop();
					if (!component.IsAlive())
					{
						UnityEngine.Object.Destroy(this.activeMeteorBackground);
					}
					this.activeMeteorBackground = null;
				}
			}

			// Token: 0x0600EA01 RID: 59905 RVA: 0x004C8A2C File Offset: 0x004C6C2C
			public float TimeUntilNextShower()
			{
				if (base.IsInsideState(base.sm.running.bombarding))
				{
					return 0f;
				}
				if (!base.IsInsideState(base.sm.starMap))
				{
					return base.sm.snoozeTimeRemaining.Get(this);
				}
				float num = base.smi.eventInstance.eventStartTime * 600f + base.smi.clusterTravelDuration - GameUtil.GetCurrentTimeInCycles() * 600f;
				if (num >= 0f)
				{
					return num;
				}
				return 0f;
			}

			// Token: 0x0600EA02 RID: 59906 RVA: 0x004C8ABC File Offset: 0x004C6CBC
			public void Bombarding(float dt)
			{
				this.nextMeteorTime -= dt;
				while (this.nextMeteorTime < 0f)
				{
					if (this.GetSleepTimerValue() <= 0f)
					{
						this.DoBombardment(this.gameplayEvent.bombardmentInfo);
					}
					this.nextMeteorTime += this.GetNextMeteorTime();
				}
			}

			// Token: 0x0600EA03 RID: 59907 RVA: 0x004C8B1C File Offset: 0x004C6D1C
			private void DoBombardment(List<MeteorShowerEvent.BombardmentInfo> bombardment_info)
			{
				float num = 0f;
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in bombardment_info)
				{
					num += bombardmentInfo.weight;
				}
				num = UnityEngine.Random.Range(0f, num);
				MeteorShowerEvent.BombardmentInfo bombardmentInfo2 = bombardment_info[0];
				int num2 = 0;
				while (num - bombardmentInfo2.weight > 0f)
				{
					num -= bombardmentInfo2.weight;
					bombardmentInfo2 = bombardment_info[++num2];
				}
				Game.Instance.Trigger(-84771526, null);
				this.SpawnBombard(bombardmentInfo2.prefab);
			}

			// Token: 0x0600EA04 RID: 59908 RVA: 0x004C8BD0 File Offset: 0x004C6DD0
			private GameObject SpawnBombard(string prefab)
			{
				WorldContainer worldContainer = ClusterManager.Instance.GetWorld(this.m_worldId);
				float x = (float)(worldContainer.Width - 1) * UnityEngine.Random.value + (float)worldContainer.WorldOffset.x;
				float y = (float)(worldContainer.Height + worldContainer.WorldOffset.y - 1);
				float layerZ = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
				Vector3 position = new Vector3(x, y, layerZ);
				GameObject prefab2 = Assets.GetPrefab(prefab);
				if (prefab2 == null)
				{
					return null;
				}
				GameObject gameObject = Util.KInstantiate(prefab2, position, Quaternion.identity, null, null, true, 0);
				Comet component = gameObject.GetComponent<Comet>();
				if (component != null)
				{
					component.spawnWithOffset = true;
				}
				gameObject.SetActive(true);
				return gameObject;
			}

			// Token: 0x0600EA05 RID: 59909 RVA: 0x0013C635 File Offset: 0x0013A835
			public float BombardTimeRemaining()
			{
				return Mathf.Min(base.sm.bombardTimeRemaining.Get(this), base.sm.runTimeRemaining.Get(this));
			}

			// Token: 0x0600EA06 RID: 59910 RVA: 0x004C8C80 File Offset: 0x004C6E80
			public float GetBombardOffTime()
			{
				float num = this.gameplayEvent.secondsBombardmentOff.Get();
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 0.5f;
							}
						}
						else
						{
							num *= 1f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				return num;
			}

			// Token: 0x0600EA07 RID: 59911 RVA: 0x004C8D08 File Offset: 0x004C6F08
			public float GetBombardOnTime()
			{
				float num = this.gameplayEvent.secondsBombardmentOn.Get();
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 1f;
							}
						}
						else
						{
							num *= 1f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				return num;
			}

			// Token: 0x0600EA08 RID: 59912 RVA: 0x004C8D90 File Offset: 0x004C6F90
			private float GetNextMeteorTime()
			{
				float num = this.gameplayEvent.secondsPerMeteor;
				num *= 256f / (float)this.world.Width;
				if (this.gameplayEvent.affectedByDifficulty && this.difficultyLevel != null)
				{
					string id = this.difficultyLevel.id;
					if (!(id == "Infrequent"))
					{
						if (!(id == "Intense"))
						{
							if (id == "Doomed")
							{
								num *= 0.5f;
							}
						}
						else
						{
							num *= 0.8f;
						}
					}
					else
					{
						num *= 1.5f;
					}
				}
				return num;
			}

			// Token: 0x0400E576 RID: 58742
			public GameObject activeMeteorBackground;

			// Token: 0x0400E577 RID: 58743
			[Serialize]
			public float clusterTravelDuration = -1f;

			// Token: 0x0400E578 RID: 58744
			[Serialize]
			private float nextMeteorTime;

			// Token: 0x0400E579 RID: 58745
			[Serialize]
			private int m_worldId;

			// Token: 0x0400E57A RID: 58746
			private WorldContainer world;

			// Token: 0x0400E57B RID: 58747
			private SettingLevel difficultyLevel;
		}
	}
}
