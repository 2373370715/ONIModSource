using STRINGS;
using UnityEngine;

public class ConduitSleepStates : GameStateMachine<ConduitSleepStates, ConduitSleepStates.Instance, IStateMachineTarget, ConduitSleepStates.Def>
{
	public class Def : BaseDef
	{
		public HashedString gulpSymbol = "gulp";
	}

	public new class Instance : GameInstance
	{
		[MyCmpReq]
		public KBatchedAnimController animController;

		[MyCmpReq]
		public Staterpillar staterpillar;

		[MyCmpAdd]
		private LoopingSounds loopingSounds;

		public bool gulpSymbolVisible;

		public float amountDeposited;

		public Instance(Chore<Instance> chore, Def def)
			: base((IStateMachineTarget)chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsConduitConnection);
		}

		public void SetGulpSymbolVisibility(bool state)
		{
			string sound = GlobalAssets.GetSound("PlugSlug_Charging_Gulp_LP");
			if (gulpSymbolVisible != state)
			{
				gulpSymbolVisible = state;
				animController.SetSymbolVisiblity(base.def.gulpSymbol, state);
				if (state)
				{
					loopingSounds.StartSound(sound);
				}
				else
				{
					loopingSounds.StopSound(sound);
				}
			}
		}
	}

	public class SleepStates : State
	{
		public State connected;

		public State noConnection;
	}

	public class DrowsyStates : State
	{
		public State loop;

		public State pst;
	}

	public class HasConnectorStates : State
	{
		public State moveToSleepLocation;

		public SleepStates sleep;

		public State noConnectionWake;

		public State connectedWake;
	}

	public DrowsyStates drowsy;

	public HasConnectorStates connector;

	public State behaviourcomplete;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = connector.moveToSleepLocation;
		root.EventTransition(GameHashes.NewDay, (Instance smi) => GameClock.Instance, behaviourcomplete).Exit(CleanUp);
		connector.moveToSleepLocation.ToggleStatusItem(CREATURES.STATUSITEMS.DROWSY.NAME, CREATURES.STATUSITEMS.DROWSY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).MoveTo(delegate(Instance smi)
		{
			ConduitSleepMonitor.Instance sMI = smi.GetSMI<ConduitSleepMonitor.Instance>();
			return sMI.sm.targetSleepCell.Get(sMI);
		}, drowsy, behaviourcomplete);
		drowsy.ToggleStatusItem(CREATURES.STATUSITEMS.DROWSY.NAME, CREATURES.STATUSITEMS.DROWSY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Enter(delegate(Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Ceiling);
		}).Enter(delegate(Instance smi)
		{
			if (GameClock.Instance.IsNighttime())
			{
				smi.GoTo(connector.sleep);
			}
		})
			.DefaultState(drowsy.loop);
		drowsy.loop.PlayAnim("drowsy_pre").QueueAnim("drowsy_loop", loop: true).EventTransition(GameHashes.Nighttime, (Instance smi) => GameClock.Instance, drowsy.pst, (Instance smi) => GameClock.Instance.IsNighttime());
		drowsy.pst.PlayAnim("drowsy_pst").OnAnimQueueComplete(connector.sleep);
		connector.sleep.ToggleStatusItem(CREATURES.STATUSITEMS.SLEEPING.NAME, CREATURES.STATUSITEMS.SLEEPING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).Enter(delegate(Instance smi)
		{
			if (!smi.staterpillar.IsConnectorBuildingSpawned())
			{
				smi.GoTo(behaviourcomplete);
			}
			else
			{
				smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Ceiling);
				smi.staterpillar.EnableConnector();
				if (smi.staterpillar.IsConnected())
				{
					smi.GoTo(connector.sleep.connected);
				}
				else
				{
					smi.GoTo(connector.sleep.noConnection);
				}
			}
		});
		connector.sleep.connected.Enter(delegate(Instance smi)
		{
			smi.animController.SetSceneLayer(GetSleepingLayer(smi));
		}).Exit(delegate(Instance smi)
		{
			smi.animController.SetSceneLayer(Grid.SceneLayer.Creatures);
		}).EventTransition(GameHashes.NewDay, (Instance smi) => GameClock.Instance, connector.connectedWake)
			.Transition(connector.sleep.noConnection, (Instance smi) => !smi.staterpillar.IsConnected())
			.PlayAnim("sleep_charging_pre")
			.QueueAnim("sleep_charging_loop", loop: true)
			.Update(UpdateGulpSymbol, UpdateRate.SIM_1000ms)
			.EventHandler(GameHashes.OnStorageChange, OnStorageChanged);
		connector.sleep.noConnection.PlayAnim("sleep_pre").QueueAnim("sleep_loop", loop: true).ToggleStatusItem(GetStatusItem)
			.EventTransition(GameHashes.NewDay, (Instance smi) => GameClock.Instance, connector.noConnectionWake)
			.Transition(connector.sleep.connected, (Instance smi) => smi.staterpillar.IsConnected());
		connector.connectedWake.QueueAnim("sleep_charging_pst").OnAnimQueueComplete(behaviourcomplete);
		connector.noConnectionWake.QueueAnim("sleep_pst").OnAnimQueueComplete(behaviourcomplete);
		behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsConduitConnection);
	}

	private static Grid.SceneLayer GetSleepingLayer(Instance smi)
	{
		return smi.staterpillar.conduitLayer switch
		{
			ObjectLayer.Wire => Grid.SceneLayer.SolidConduitBridges, 
			ObjectLayer.GasConduit => Grid.SceneLayer.Gas, 
			ObjectLayer.LiquidConduit => Grid.SceneLayer.GasConduitBridges, 
			_ => Grid.SceneLayer.SolidConduitBridges, 
		};
	}

	private static StatusItem GetStatusItem(Instance smi)
	{
		return smi.staterpillar.conduitLayer switch
		{
			ObjectLayer.Wire => Db.Get().BuildingStatusItems.NoWireConnected, 
			ObjectLayer.GasConduit => Db.Get().BuildingStatusItems.NeedGasOut, 
			ObjectLayer.LiquidConduit => Db.Get().BuildingStatusItems.NeedLiquidOut, 
			_ => Db.Get().BuildingStatusItems.Normal, 
		};
	}

	private static void OnStorageChanged(Instance smi, object obj)
	{
		GameObject gameObject = obj as GameObject;
		if (gameObject != null)
		{
			smi.amountDeposited += gameObject.GetComponent<PrimaryElement>().Mass;
		}
	}

	private static void UpdateGulpSymbol(Instance smi, float dt)
	{
		smi.SetGulpSymbolVisibility(smi.amountDeposited > 0f);
		smi.amountDeposited = 0f;
	}

	private static void CleanUp(Instance smi)
	{
		ConduitSleepMonitor.Instance sMI = smi.GetSMI<ConduitSleepMonitor.Instance>();
		sMI?.sm.targetSleepCell.Set(Grid.InvalidCell, sMI);
		smi.staterpillar.DestroyOrphanedConnectorBuilding();
	}
}
