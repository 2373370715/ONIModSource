﻿using System;
using STRINGS;
using UnityEngine;

public class EvilFlower : StateMachineComponent<EvilFlower.StatesInstance>
{
		protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<EvilFlower>(1309017699, EvilFlower.SetReplantedTrueDelegate);
	}

		protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

		protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

		[MyCmpReq]
	private WiltCondition wiltCondition;

		[MyCmpReq]
	private EntombVulnerable entombVulnerable;

		public bool replanted;

		public EffectorValues positive_decor_effect = new EffectorValues
	{
		amount = 1,
		radius = 5
	};

		public EffectorValues negative_decor_effect = new EffectorValues
	{
		amount = -1,
		radius = 5
	};

		private static readonly EventSystem.IntraObjectHandler<EvilFlower> SetReplantedTrueDelegate = new EventSystem.IntraObjectHandler<EvilFlower>(delegate(EvilFlower component, object data)
	{
		component.replanted = true;
	});

		public class StatesInstance : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.GameInstance
	{
				public StatesInstance(EvilFlower smi) : base(smi)
		{
		}
	}

		public class States : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower>
	{
				public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).TriggerOnEnter(GameHashes.BurstEmitDisease, null).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(EvilFlower.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (EvilFlower.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (EvilFlower.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (EvilFlower.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(EvilFlower.StatesInstance smi)
			{
				if (smi.master.replanted && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State state2 = this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.idle);
			string name2 = CREATURES.STATUSITEMS.IDLE.NAME;
			string tooltip2 = CREATURES.STATUSITEMS.IDLE.TOOLTIP;
			string icon2 = "";
			StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
			NotificationType notification_type2 = NotificationType.Neutral;
			bool allow_multiples2 = false;
			main = Db.Get().StatusItemCategories.Main;
			state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
			this.alive.idle.EventTransition(GameHashes.Wilt, this.alive.wilting, (EvilFlower.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop).Enter(delegate(EvilFlower.StatesInstance smi)
			{
				smi.master.GetComponent<DecorProvider>().SetValues(smi.master.positive_decor_effect);
				smi.master.GetComponent<DecorProvider>().Refresh();
				smi.master.AddTag(GameTags.Decoration);
			});
			this.alive.wilting.PlayAnim("wilt1", KAnim.PlayMode.Loop).EventTransition(GameHashes.WiltRecover, this.alive.idle, null).ToggleTag(GameTags.PreventEmittingDisease).Enter(delegate(EvilFlower.StatesInstance smi)
			{
				smi.master.GetComponent<DecorProvider>().SetValues(smi.master.negative_decor_effect);
				smi.master.GetComponent<DecorProvider>().Refresh();
				smi.master.RemoveTag(GameTags.Decoration);
			});
		}

				public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State grow;

				public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State blocked_from_growing;

				public EvilFlower.States.AliveStates alive;

				public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State dead;

				public class AliveStates : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.PlantAliveSubState
		{
						public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State idle;

						public EvilFlower.States.WiltingState wilting;
		}

				public class WiltingState : GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State
		{
						public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting_pre;

						public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting;

						public GameStateMachine<EvilFlower.States, EvilFlower.StatesInstance, EvilFlower, object>.State wilting_pst;
		}
	}
}
