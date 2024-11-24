using System;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using KSerialization;

// Token: 0x0200135E RID: 4958
public class GameplaySeasonManager : GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>
{
	// Token: 0x060065E1 RID: 26081 RVA: 0x002CD7A8 File Offset: 0x002CB9A8
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.root;
		this.root.Enter(delegate(GameplaySeasonManager.Instance smi)
		{
			smi.Initialize();
		}).Update(delegate(GameplaySeasonManager.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_4000ms, false);
	}

	// Token: 0x0200135F RID: 4959
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001360 RID: 4960
	public new class Instance : GameStateMachine<GameplaySeasonManager, GameplaySeasonManager.Instance, IStateMachineTarget, GameplaySeasonManager.Def>.GameInstance
	{
		// Token: 0x060065E4 RID: 26084 RVA: 0x000E2744 File Offset: 0x000E0944
		public Instance(IStateMachineTarget master, GameplaySeasonManager.Def def) : base(master, def)
		{
			this.activeSeasons = new List<GameplaySeasonInstance>();
		}

		// Token: 0x060065E5 RID: 26085 RVA: 0x002CD810 File Offset: 0x002CBA10
		public void Initialize()
		{
			this.activeSeasons.RemoveAll((GameplaySeasonInstance item) => item.Season == null);
			List<GameplaySeason> list = new List<GameplaySeason>();
			if (this.m_worldContainer != null)
			{
				ClusterGridEntity component = base.GetComponent<ClusterGridEntity>();
				using (List<string>.Enumerator enumerator = this.m_worldContainer.GetSeasonIds().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						GameplaySeason gameplaySeason = Db.Get().GameplaySeasons.TryGet(text);
						if (gameplaySeason == null)
						{
							Debug.LogWarning("world " + component.name + " has invalid season " + text);
						}
						else
						{
							if (gameplaySeason.type != GameplaySeason.Type.World)
							{
								Debug.LogWarning(string.Concat(new string[]
								{
									"world ",
									component.name,
									" has specified season ",
									text,
									", which is not a world type season"
								}));
							}
							list.Add(gameplaySeason);
						}
					}
					goto IL_146;
				}
			}
			Debug.Assert(base.GetComponent<SaveGame>() != null);
			list = (from season in Db.Get().GameplaySeasons.resources
			where season.type == GameplaySeason.Type.Cluster
			select season).ToList<GameplaySeason>();
			IL_146:
			foreach (GameplaySeason gameplaySeason2 in list)
			{
				if (SaveLoader.Instance.IsDLCActiveForCurrentSave(gameplaySeason2.dlcId) && gameplaySeason2.startActive && !this.SeasonExists(gameplaySeason2) && gameplaySeason2.events.Count > 0)
				{
					this.activeSeasons.Add(gameplaySeason2.Instantiate(this.GetWorldId()));
				}
			}
			foreach (GameplaySeasonInstance gameplaySeasonInstance in new List<GameplaySeasonInstance>(this.activeSeasons))
			{
				if (!list.Contains(gameplaySeasonInstance.Season) || !SaveLoader.Instance.IsDLCActiveForCurrentSave(gameplaySeasonInstance.Season.dlcId))
				{
					this.activeSeasons.Remove(gameplaySeasonInstance);
				}
			}
		}

		// Token: 0x060065E6 RID: 26086 RVA: 0x000E2759 File Offset: 0x000E0959
		private int GetWorldId()
		{
			if (this.m_worldContainer != null)
			{
				return this.m_worldContainer.id;
			}
			return -1;
		}

		// Token: 0x060065E7 RID: 26087 RVA: 0x002CDA70 File Offset: 0x002CBC70
		public void Update(float dt)
		{
			foreach (GameplaySeasonInstance gameplaySeasonInstance in this.activeSeasons)
			{
				if (gameplaySeasonInstance.ShouldGenerateEvents() && GameUtil.GetCurrentTimeInCycles() > gameplaySeasonInstance.NextEventTime)
				{
					int num = 0;
					while (num < gameplaySeasonInstance.Season.numEventsToStartEachPeriod && gameplaySeasonInstance.StartEvent(false))
					{
						num++;
					}
				}
			}
		}

		// Token: 0x060065E8 RID: 26088 RVA: 0x000E2776 File Offset: 0x000E0976
		public void StartNewSeason(GameplaySeason seasonType)
		{
			if (SaveLoader.Instance.IsDLCActiveForCurrentSave(seasonType.dlcId))
			{
				this.activeSeasons.Add(seasonType.Instantiate(this.GetWorldId()));
			}
		}

		// Token: 0x060065E9 RID: 26089 RVA: 0x002CDAF0 File Offset: 0x002CBCF0
		public bool SeasonExists(GameplaySeason seasonType)
		{
			return this.activeSeasons.Find((GameplaySeasonInstance e) => e.Season.IdHash == seasonType.IdHash) != null;
		}

		// Token: 0x04004C76 RID: 19574
		[Serialize]
		public List<GameplaySeasonInstance> activeSeasons;

		// Token: 0x04004C77 RID: 19575
		[MyCmpGet]
		private WorldContainer m_worldContainer;
	}
}
