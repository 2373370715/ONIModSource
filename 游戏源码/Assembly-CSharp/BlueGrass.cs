using System;
using STRINGS;
using UnityEngine;

// Token: 0x020016B1 RID: 5809
public class BlueGrass : StateMachineComponent<BlueGrass.StatesInstance>
{
	// Token: 0x060077CE RID: 30670 RVA: 0x000EE9FE File Offset: 0x000ECBFE
	protected void DestroySelf(object callbackParam)
	{
		CreatureHelpers.DeselectCreature(base.gameObject);
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x060077CF RID: 30671 RVA: 0x000EEA69 File Offset: 0x000ECC69
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060077D0 RID: 30672 RVA: 0x000EEA7C File Offset: 0x000ECC7C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060077D1 RID: 30673 RVA: 0x000EEA84 File Offset: 0x000ECC84
	protected override void OnPrefabInit()
	{
		base.Subscribe<BlueGrass>(1309017699, BlueGrass.OnReplantedDelegate);
		base.OnPrefabInit();
	}

	// Token: 0x060077D2 RID: 30674 RVA: 0x000EEA9D File Offset: 0x000ECC9D
	private void OnReplanted(object data = null)
	{
		this.SetConsumptionRate();
	}

	// Token: 0x060077D3 RID: 30675 RVA: 0x000EEAA5 File Offset: 0x000ECCA5
	public void SetConsumptionRate()
	{
		if (this.receptacleMonitor.Replanted)
		{
			this.elementConsumer.consumptionRate = 0.002f;
			return;
		}
		this.elementConsumer.consumptionRate = 0.0005f;
	}

	// Token: 0x04005995 RID: 22933
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04005996 RID: 22934
	[MyCmpReq]
	private ElementConsumer elementConsumer;

	// Token: 0x04005997 RID: 22935
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x04005998 RID: 22936
	[MyCmpReq]
	private Growing growing;

	// Token: 0x04005999 RID: 22937
	private static readonly EventSystem.IntraObjectHandler<BlueGrass> OnReplantedDelegate = new EventSystem.IntraObjectHandler<BlueGrass>(delegate(BlueGrass component, object data)
	{
		component.OnReplanted(data);
	});

	// Token: 0x020016B2 RID: 5810
	public class StatesInstance : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.GameInstance
	{
		// Token: 0x060077D6 RID: 30678 RVA: 0x000EEAF9 File Offset: 0x000ECCF9
		public StatesInstance(BlueGrass master) : base(master)
		{
		}
	}

	// Token: 0x020016B3 RID: 5811
	public class States : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass>
	{
		// Token: 0x060077D7 RID: 30679 RVA: 0x0030F3FC File Offset: 0x0030D5FC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, new Action<object>(smi.master.DestroySelf), null);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (BlueGrass.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(BlueGrass.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
					return;
				}
				smi.GoTo(this.alive);
			});
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.growing).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.SetConsumptionRate();
			});
			this.alive.growing.EventTransition(GameHashes.Wilt, this.alive.wilting, (BlueGrass.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).Enter(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(true);
			}).Exit(delegate(BlueGrass.StatesInstance smi)
			{
				smi.master.elementConsumer.EnableConsumption(false);
			}).EventTransition(GameHashes.Grow, this.alive.fullygrown, (BlueGrass.StatesInstance smi) => smi.master.growing.IsGrown());
			this.alive.fullygrown.EventTransition(GameHashes.Wilt, this.alive.wilting, (BlueGrass.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).EventTransition(GameHashes.HarvestComplete, this.alive.growing, null);
			this.alive.wilting.EventTransition(GameHashes.WiltRecover, this.alive.growing, (BlueGrass.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x0400599A RID: 22938
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State grow;

		// Token: 0x0400599B RID: 22939
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State blocked_from_growing;

		// Token: 0x0400599C RID: 22940
		public BlueGrass.States.AliveStates alive;

		// Token: 0x0400599D RID: 22941
		public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State dead;

		// Token: 0x020016B4 RID: 5812
		public class AliveStates : GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.PlantAliveSubState
		{
			// Token: 0x0400599E RID: 22942
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State growing;

			// Token: 0x0400599F RID: 22943
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State fullygrown;

			// Token: 0x040059A0 RID: 22944
			public GameStateMachine<BlueGrass.States, BlueGrass.StatesInstance, BlueGrass, object>.State wilting;
		}
	}
}
