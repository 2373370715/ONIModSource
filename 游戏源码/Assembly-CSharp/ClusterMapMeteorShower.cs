using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

public class ClusterMapMeteorShower : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>
{
	public class Def : BaseDef, IGameObjectEffectDescriptor
	{
		public string name;

		public string description;

		public string description_Hidden;

		public string name_Hidden;

		public string eventID;

		public int destinationWorldID;

		public float arrivalTime;

		public List<Descriptor> GetDescriptors(GameObject go)
		{
			GameplayEvent gameplayEvent = Db.Get().GameplayEvents.Get(eventID);
			List<Descriptor> list = new List<Descriptor>();
			Instance sMI = go.GetSMI<Instance>();
			if (sMI != null && sMI.sm.IsIdentified.Get(sMI) && gameplayEvent is MeteorShowerEvent)
			{
				List<MeteorShowerEvent.BombardmentInfo> meteorsInfo = (gameplayEvent as MeteorShowerEvent).GetMeteorsInfo();
				float num = 0f;
				foreach (MeteorShowerEvent.BombardmentInfo item2 in meteorsInfo)
				{
					num += item2.weight;
				}
				foreach (MeteorShowerEvent.BombardmentInfo item3 in meteorsInfo)
				{
					GameObject prefab = Assets.GetPrefab(item3.prefab);
					string formattedPercent = GameUtil.GetFormattedPercent(Mathf.RoundToInt(item3.weight / num * 100f));
					string txt = prefab.GetProperName() + " " + formattedPercent;
					Descriptor item = new Descriptor(txt, UI.GAMEOBJECTEFFECTS.TOOLTIPS.METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP);
					list.Add(item);
				}
			}
			return list;
		}
	}

	public class TravelingState : State
	{
		public State unidentified;

		public State identified;
	}

	public new class Instance : GameInstance, ISidescreenButtonControl
	{
		[Serialize]
		public int DestinationWorldID = -1;

		[Serialize]
		public float ArrivalTime;

		[Serialize]
		private float Speed;

		[Serialize]
		private float identifyingProgress;

		public System.Action OnDestinationReached;

		[MyCmpGet]
		private InfoDescription descriptor;

		[MyCmpGet]
		private KSelectable selectable;

		[MyCmpGet]
		private ClusterMapMeteorShowerVisualizer visualizer;

		[MyCmpGet]
		private ClusterTraveler traveler;

		[MyCmpGet]
		private ClusterDestinationSelector destinationSelector;

		public WorldContainer World_Destination => ClusterManager.Instance.GetWorld(DestinationWorldID);

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

		public bool HasBeenIdentified => base.sm.IsIdentified.Get(this);

		public float IdentifyingProgress => identifyingProgress;

		public AxialI ClusterGridPosition()
		{
			return visualizer.Location;
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			traveler.getSpeedCB = GetSpeed;
			traveler.onTravelCB = OnTravellerMoved;
		}

		private void OnTravellerMoved()
		{
			Game.Instance.Trigger(-1975776133, this);
		}

		protected override void OnCleanUp()
		{
			visualizer.Deselect();
			base.OnCleanUp();
		}

		public void Identify()
		{
			if (!HasBeenIdentified)
			{
				identifyingProgress = 1f;
				base.sm.IsIdentified.Set(value: true, this);
				Game.Instance.Trigger(1427028915, this);
				RefreshVisuals(playIdentifyAnimationIfVisible: true);
				if (ClusterMapScreen.Instance.IsActive())
				{
					KFMOD.PlayUISound(GlobalAssets.GetSound("ClusterMapMeteor_Reveal"));
				}
			}
		}

		public void ProgressIdentifiction(float points)
		{
			if (!HasBeenIdentified)
			{
				identifyingProgress += points;
				identifyingProgress = Mathf.Clamp(identifyingProgress, 0f, 1f);
				if (identifyingProgress == 1f)
				{
					Identify();
				}
			}
		}

		public override void StartSM()
		{
			base.StartSM();
			if (DestinationWorldID < 0)
			{
				Setup(base.def.destinationWorldID, base.def.arrivalTime);
			}
			RefreshVisuals();
		}

		public void RefreshVisuals(bool playIdentifyAnimationIfVisible = false)
		{
			if (HasBeenIdentified)
			{
				selectable.SetName(base.def.name);
				descriptor.description = base.def.description;
				visualizer.PlayRevealAnimation(playIdentifyAnimationIfVisible);
			}
			else
			{
				selectable.SetName(base.def.name_Hidden);
				descriptor.description = base.def.description_Hidden;
				visualizer.PlayHideAnimation();
			}
			Trigger(1980521255);
		}

		public void Setup(int destinationWorldID, float arrivalTime)
		{
			DestinationWorldID = destinationWorldID;
			ArrivalTime = arrivalTime;
			AxialI location = World_Destination.GetComponent<ClusterGridEntity>().Location;
			destinationSelector.SetDestination(location);
			traveler.RevalidatePath(react_to_change: false);
			int count = traveler.CurrentPath.Count;
			float num = arrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f;
			Speed = (float)count / num * 600f;
		}

		public float GetSpeed()
		{
			return Speed;
		}

		public void DestinationReached()
		{
			OnDestinationReached?.Invoke();
		}

		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		public bool SidescreenEnabled()
		{
			return false;
		}

		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		public void OnSidescreenButtonPressed()
		{
			Identify();
		}

		public int HorizontalGroupID()
		{
			return -1;
		}

		public int ButtonSideScreenSortOrder()
		{
			return SORTORDER.KEEPSAKES;
		}
	}

	public BoolParameter IsIdentified;

	public TravelingState traveling;

	public State arrived;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = traveling;
		traveling.DefaultState(traveling.unidentified).EventTransition(GameHashes.ClusterDestinationReached, arrived);
		traveling.unidentified.ParamTransition(IsIdentified, traveling.identified, GameStateMachine<ClusterMapMeteorShower, Instance, IStateMachineTarget, Def>.IsTrue);
		traveling.identified.ParamTransition(IsIdentified, traveling.unidentified, GameStateMachine<ClusterMapMeteorShower, Instance, IStateMachineTarget, Def>.IsFalse).ToggleStatusItem(Db.Get().MiscStatusItems.ClusterMeteorRemainingTravelTime);
		arrived.Enter(DestinationReached);
	}

	public static void DestinationReached(Instance smi)
	{
		smi.DestinationReached();
		Util.KDestroyGameObject(smi.gameObject);
	}
}
