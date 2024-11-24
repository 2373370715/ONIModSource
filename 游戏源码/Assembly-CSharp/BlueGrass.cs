using STRINGS;
using UnityEngine;

public class BlueGrass : StateMachineComponent<BlueGrass.StatesInstance>
{
	public class StatesInstance : GameStateMachine<States, StatesInstance, BlueGrass, object>.GameInstance
	{
		public StatesInstance(BlueGrass master)
			: base(master)
		{
		}
	}

	public class States : GameStateMachine<States, StatesInstance, BlueGrass>
	{
		public class AliveStates : PlantAliveSubState
		{
			public State growing;

			public State fullygrown;

			public State wilting;
		}

		public State grow;

		public State blocked_from_growing;

		public AliveStates alive;

		public State dead;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = grow;
			dead.ToggleStatusItem(CREATURES.STATUSITEMS.DEAD.NAME, CREATURES.STATUSITEMS.DEAD.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Enter(delegate(StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront).SetActive(value: true);
				smi.master.Trigger(1623392196);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, smi.master.DestroySelf);
			});
			blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked).EventTransition(GameHashes.EntombedChanged, alive, (StatesInstance smi) => alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, alive, (StatesInstance smi) => alive.ForceUpdateStatus(smi.master.gameObject))
				.EventTransition(GameHashes.TooHotWarning, alive, (StatesInstance smi) => alive.ForceUpdateStatus(smi.master.gameObject))
				.TagTransition(GameTags.Uprooted, dead);
			grow.Enter(delegate(StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(blocked_from_growing);
				}
				else
				{
					smi.GoTo(alive);
				}
			});
			alive.InitializeStates(masterTarget, dead).DefaultState(alive.growing).Enter(delegate(StatesInstance smi)
			{
				smi.master.SetConsumptionRate();
			});
			alive.growing.EventTransition(GameHashes.Wilt, alive.wilting, (StatesInstance smi) => smi.master.wiltCondition.IsWilting()).Enter(delegate(StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(enabled: true);
			}).Exit(delegate(StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(enabled: false);
			})
				.EventTransition(GameHashes.Grow, alive.fullygrown, (StatesInstance smi) => smi.master.growing.IsGrown());
			alive.fullygrown.EventTransition(GameHashes.Wilt, alive.wilting, (StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.HarvestComplete, alive.growing);
			alive.wilting.EventTransition(GameHashes.WiltRecover, alive.growing, (StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}
	}

	[MyCmpReq]
	private WiltCondition wiltCondition;

	[MyCmpReq]
	private ElementConsumer elementConsumer;

	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	[MyCmpReq]
	private Growing growing;

	private static readonly EventSystem.IntraObjectHandler<BlueGrass> OnReplantedDelegate = new EventSystem.IntraObjectHandler<BlueGrass>(delegate(BlueGrass component, object data)
	{
		component.OnReplanted(data);
	});

	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	protected override void OnPrefabInit()
	{
		Subscribe(1309017699, OnReplantedDelegate);
		base.OnPrefabInit();
	}

	private void OnReplanted(object data = null)
	{
		SetConsumptionRate();
	}

	public void SetConsumptionRate()
	{
		if (receptacleMonitor.Replanted)
		{
			elementConsumer.consumptionRate = 0.002f;
		}
		else
		{
			elementConsumer.consumptionRate = 0.0005f;
		}
	}
}
