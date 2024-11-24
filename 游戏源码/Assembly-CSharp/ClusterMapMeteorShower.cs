using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020018C5 RID: 6341
public class ClusterMapMeteorShower : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>
{
	// Token: 0x06008367 RID: 33639 RVA: 0x0033FA1C File Offset: 0x0033DC1C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.traveling;
		this.traveling.DefaultState(this.traveling.unidentified).EventTransition(GameHashes.ClusterDestinationReached, this.arrived, null);
		this.traveling.unidentified.ParamTransition<bool>(this.IsIdentified, this.traveling.identified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsTrue);
		this.traveling.identified.ParamTransition<bool>(this.IsIdentified, this.traveling.unidentified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsFalse).ToggleStatusItem(Db.Get().MiscStatusItems.ClusterMeteorRemainingTravelTime, null);
		this.arrived.Enter(new StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State.Callback(ClusterMapMeteorShower.DestinationReached));
	}

	// Token: 0x06008368 RID: 33640 RVA: 0x000F659B File Offset: 0x000F479B
	public static void DestinationReached(ClusterMapMeteorShower.Instance smi)
	{
		smi.DestinationReached();
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x040063B1 RID: 25521
	public StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.BoolParameter IsIdentified;

	// Token: 0x040063B2 RID: 25522
	public ClusterMapMeteorShower.TravelingState traveling;

	// Token: 0x040063B3 RID: 25523
	public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State arrived;

	// Token: 0x020018C6 RID: 6342
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600836A RID: 33642 RVA: 0x0033FADC File Offset: 0x0033DCDC
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			GameplayEvent gameplayEvent = Db.Get().GameplayEvents.Get(this.eventID);
			List<Descriptor> list = new List<Descriptor>();
			ClusterMapMeteorShower.Instance smi = go.GetSMI<ClusterMapMeteorShower.Instance>();
			if (smi != null && smi.sm.IsIdentified.Get(smi) && gameplayEvent is MeteorShowerEvent)
			{
				List<MeteorShowerEvent.BombardmentInfo> meteorsInfo = (gameplayEvent as MeteorShowerEvent).GetMeteorsInfo();
				float num = 0f;
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in meteorsInfo)
				{
					num += bombardmentInfo.weight;
				}
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo2 in meteorsInfo)
				{
					GameObject prefab = Assets.GetPrefab(bombardmentInfo2.prefab);
					string formattedPercent = GameUtil.GetFormattedPercent((float)Mathf.RoundToInt(bombardmentInfo2.weight / num * 100f), GameUtil.TimeSlice.None);
					string txt = prefab.GetProperName() + " " + formattedPercent;
					Descriptor item = new Descriptor(txt, UI.GAMEOBJECTEFFECTS.TOOLTIPS.METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP, Descriptor.DescriptorType.Effect, false);
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x040063B4 RID: 25524
		public string name;

		// Token: 0x040063B5 RID: 25525
		public string description;

		// Token: 0x040063B6 RID: 25526
		public string description_Hidden;

		// Token: 0x040063B7 RID: 25527
		public string name_Hidden;

		// Token: 0x040063B8 RID: 25528
		public string eventID;

		// Token: 0x040063B9 RID: 25529
		public int destinationWorldID;

		// Token: 0x040063BA RID: 25530
		public float arrivalTime;
	}

	// Token: 0x020018C7 RID: 6343
	public class TravelingState : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State
	{
		// Token: 0x040063BB RID: 25531
		public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State unidentified;

		// Token: 0x040063BC RID: 25532
		public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State identified;
	}

	// Token: 0x020018C8 RID: 6344
	public new class Instance : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x0600836D RID: 33645 RVA: 0x000F65BE File Offset: 0x000F47BE
		public WorldContainer World_Destination
		{
			get
			{
				return ClusterManager.Instance.GetWorld(this.DestinationWorldID);
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x0600836E RID: 33646 RVA: 0x000F65D0 File Offset: 0x000F47D0
		public string SidescreenButtonText
		{
			get
			{
				if (!base.smi.sm.IsIdentified.Get(base.smi))
				{
					return "Identify";
				}
				return "Dev Hide";
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600836F RID: 33647 RVA: 0x000F65FA File Offset: 0x000F47FA
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!base.smi.sm.IsIdentified.Get(base.smi))
				{
					return "Identifies the meteor shower";
				}
				return "Dev unidentify back";
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06008370 RID: 33648 RVA: 0x000F6624 File Offset: 0x000F4824
		public bool HasBeenIdentified
		{
			get
			{
				return base.sm.IsIdentified.Get(this);
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06008371 RID: 33649 RVA: 0x000F6637 File Offset: 0x000F4837
		public float IdentifyingProgress
		{
			get
			{
				return this.identifyingProgress;
			}
		}

		// Token: 0x06008372 RID: 33650 RVA: 0x000F663F File Offset: 0x000F483F
		public AxialI ClusterGridPosition()
		{
			return this.visualizer.Location;
		}

		// Token: 0x06008373 RID: 33651 RVA: 0x000F664C File Offset: 0x000F484C
		public Instance(IStateMachineTarget master, ClusterMapMeteorShower.Def def) : base(master, def)
		{
			this.traveler.getSpeedCB = new Func<float>(this.GetSpeed);
			this.traveler.onTravelCB = new System.Action(this.OnTravellerMoved);
		}

		// Token: 0x06008374 RID: 33652 RVA: 0x000F668B File Offset: 0x000F488B
		private void OnTravellerMoved()
		{
			Game.Instance.Trigger(-1975776133, this);
		}

		// Token: 0x06008375 RID: 33653 RVA: 0x000F669D File Offset: 0x000F489D
		protected override void OnCleanUp()
		{
			this.visualizer.Deselect();
			base.OnCleanUp();
		}

		// Token: 0x06008376 RID: 33654 RVA: 0x0033FC28 File Offset: 0x0033DE28
		public void Identify()
		{
			if (!this.HasBeenIdentified)
			{
				this.identifyingProgress = 1f;
				base.sm.IsIdentified.Set(true, this, false);
				Game.Instance.Trigger(1427028915, this);
				this.RefreshVisuals(true);
				if (ClusterMapScreen.Instance.IsActive())
				{
					KFMOD.PlayUISound(GlobalAssets.GetSound("ClusterMapMeteor_Reveal", false));
				}
			}
		}

		// Token: 0x06008377 RID: 33655 RVA: 0x0033FC90 File Offset: 0x0033DE90
		public void ProgressIdentifiction(float points)
		{
			if (!this.HasBeenIdentified)
			{
				this.identifyingProgress += points;
				this.identifyingProgress = Mathf.Clamp(this.identifyingProgress, 0f, 1f);
				if (this.identifyingProgress == 1f)
				{
					this.Identify();
				}
			}
		}

		// Token: 0x06008378 RID: 33656 RVA: 0x000F66B0 File Offset: 0x000F48B0
		public override void StartSM()
		{
			base.StartSM();
			if (this.DestinationWorldID < 0)
			{
				this.Setup(base.def.destinationWorldID, base.def.arrivalTime);
			}
			this.RefreshVisuals(false);
		}

		// Token: 0x06008379 RID: 33657 RVA: 0x0033FCE4 File Offset: 0x0033DEE4
		public void RefreshVisuals(bool playIdentifyAnimationIfVisible = false)
		{
			if (this.HasBeenIdentified)
			{
				this.selectable.SetName(base.def.name);
				this.descriptor.description = base.def.description;
				this.visualizer.PlayRevealAnimation(playIdentifyAnimationIfVisible);
			}
			else
			{
				this.selectable.SetName(base.def.name_Hidden);
				this.descriptor.description = base.def.description_Hidden;
				this.visualizer.PlayHideAnimation();
			}
			base.Trigger(1980521255, null);
		}

		// Token: 0x0600837A RID: 33658 RVA: 0x0033FD78 File Offset: 0x0033DF78
		public void Setup(int destinationWorldID, float arrivalTime)
		{
			this.DestinationWorldID = destinationWorldID;
			this.ArrivalTime = arrivalTime;
			AxialI location = this.World_Destination.GetComponent<ClusterGridEntity>().Location;
			this.destinationSelector.SetDestination(location);
			this.traveler.RevalidatePath(false);
			int count = this.traveler.CurrentPath.Count;
			float num = arrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f;
			this.Speed = (float)count / num * 600f;
		}

		// Token: 0x0600837B RID: 33659 RVA: 0x000F66E4 File Offset: 0x000F48E4
		public float GetSpeed()
		{
			return this.Speed;
		}

		// Token: 0x0600837C RID: 33660 RVA: 0x000F66EC File Offset: 0x000F48EC
		public void DestinationReached()
		{
			System.Action onDestinationReached = this.OnDestinationReached;
			if (onDestinationReached == null)
			{
				return;
			}
			onDestinationReached();
		}

		// Token: 0x0600837D RID: 33661 RVA: 0x000ABCB6 File Offset: 0x000A9EB6
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600837E RID: 33662 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		public bool SidescreenEnabled()
		{
			return false;
		}

		// Token: 0x0600837F RID: 33663 RVA: 0x000A65EC File Offset: 0x000A47EC
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x06008380 RID: 33664 RVA: 0x000F66FE File Offset: 0x000F48FE
		public void OnSidescreenButtonPressed()
		{
			this.Identify();
		}

		// Token: 0x06008381 RID: 33665 RVA: 0x000ABC75 File Offset: 0x000A9E75
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x06008382 RID: 33666 RVA: 0x000F6706 File Offset: 0x000F4906
		public int ButtonSideScreenSortOrder()
		{
			return SORTORDER.KEEPSAKES;
		}

		// Token: 0x040063BD RID: 25533
		[Serialize]
		public int DestinationWorldID = -1;

		// Token: 0x040063BE RID: 25534
		[Serialize]
		public float ArrivalTime;

		// Token: 0x040063BF RID: 25535
		[Serialize]
		private float Speed;

		// Token: 0x040063C0 RID: 25536
		[Serialize]
		private float identifyingProgress;

		// Token: 0x040063C1 RID: 25537
		public System.Action OnDestinationReached;

		// Token: 0x040063C2 RID: 25538
		[MyCmpGet]
		private InfoDescription descriptor;

		// Token: 0x040063C3 RID: 25539
		[MyCmpGet]
		private KSelectable selectable;

		// Token: 0x040063C4 RID: 25540
		[MyCmpGet]
		private ClusterMapMeteorShowerVisualizer visualizer;

		// Token: 0x040063C5 RID: 25541
		[MyCmpGet]
		private ClusterTraveler traveler;

		// Token: 0x040063C6 RID: 25542
		[MyCmpGet]
		private ClusterDestinationSelector destinationSelector;
	}
}
