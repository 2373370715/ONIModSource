using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020016D5 RID: 5845
public class OilEater : StateMachineComponent<OilEater.StatesInstance>
{
	// Token: 0x06007863 RID: 30819 RVA: 0x000EF1A8 File Offset: 0x000ED3A8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06007864 RID: 30820 RVA: 0x0031113C File Offset: 0x0030F33C
	public void Exhaust(float dt)
	{
		if (base.smi.master.wiltCondition.IsWilting())
		{
			return;
		}
		this.emittedMass += dt * this.emitRate;
		if (this.emittedMass >= this.minEmitMass)
		{
			int gameCell = Grid.PosToCell(base.transform.GetPosition() + this.emitOffset);
			PrimaryElement component = base.GetComponent<PrimaryElement>();
			SimMessages.AddRemoveSubstance(gameCell, SimHashes.CarbonDioxide, CellEventLogger.Instance.ElementEmitted, this.emittedMass, component.Temperature, byte.MaxValue, 0, true, -1);
			this.emittedMass = 0f;
		}
	}

	// Token: 0x04005A32 RID: 23090
	private const SimHashes srcElement = SimHashes.CrudeOil;

	// Token: 0x04005A33 RID: 23091
	private const SimHashes emitElement = SimHashes.CarbonDioxide;

	// Token: 0x04005A34 RID: 23092
	public float emitRate = 1f;

	// Token: 0x04005A35 RID: 23093
	public float minEmitMass;

	// Token: 0x04005A36 RID: 23094
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04005A37 RID: 23095
	[Serialize]
	private float emittedMass;

	// Token: 0x04005A38 RID: 23096
	[MyCmpReq]
	private WiltCondition wiltCondition;

	// Token: 0x04005A39 RID: 23097
	[MyCmpReq]
	private Storage storage;

	// Token: 0x04005A3A RID: 23098
	[MyCmpReq]
	private ReceptacleMonitor receptacleMonitor;

	// Token: 0x020016D6 RID: 5846
	public class StatesInstance : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.GameInstance
	{
		// Token: 0x06007866 RID: 30822 RVA: 0x000EF1D9 File Offset: 0x000ED3D9
		public StatesInstance(OilEater master) : base(master)
		{
		}
	}

	// Token: 0x020016D7 RID: 5847
	public class States : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater>
	{
		// Token: 0x06007867 RID: 30823 RVA: 0x003111DC File Offset: 0x0030F3DC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grow;
			GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State state = this.dead;
			string name = CREATURES.STATUSITEMS.DEAD.NAME;
			string tooltip = CREATURES.STATUSITEMS.DEAD.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Info;
			NotificationType notification_type = NotificationType.Neutral;
			bool allow_multiples = false;
			StatusItemCategory main = Db.Get().StatusItemCategories.Main;
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main).Enter(delegate(OilEater.StatesInstance smi)
			{
				GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.master.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
				smi.master.Trigger(1623392196, null);
				smi.master.GetComponent<KBatchedAnimController>().StopAndClear();
				UnityEngine.Object.Destroy(smi.master.GetComponent<KBatchedAnimController>());
				smi.Schedule(0.5f, delegate(object data)
				{
					GameObject gameObject = (GameObject)data;
					CreatureHelpers.DeselectCreature(gameObject);
					Util.KDestroyGameObject(gameObject);
				}, smi.master.gameObject);
			});
			this.blocked_from_growing.ToggleStatusItem(Db.Get().MiscStatusItems.RegionIsBlocked, null).EventTransition(GameHashes.EntombedChanged, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooColdWarning, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).EventTransition(GameHashes.TooHotWarning, this.alive, (OilEater.StatesInstance smi) => this.alive.ForceUpdateStatus(smi.master.gameObject)).TagTransition(GameTags.Uprooted, this.dead, false);
			this.grow.Enter(delegate(OilEater.StatesInstance smi)
			{
				if (smi.master.receptacleMonitor.HasReceptacle() && !this.alive.ForceUpdateStatus(smi.master.gameObject))
				{
					smi.GoTo(this.blocked_from_growing);
				}
			}).PlayAnim("grow_seed", KAnim.PlayMode.Once).EventTransition(GameHashes.AnimQueueComplete, this.alive, null);
			this.alive.InitializeStates(this.masterTarget, this.dead).DefaultState(this.alive.mature).Update("Alive", delegate(OilEater.StatesInstance smi, float dt)
			{
				smi.master.Exhaust(dt);
			}, UpdateRate.SIM_200ms, false);
			this.alive.mature.EventTransition(GameHashes.Wilt, this.alive.wilting, (OilEater.StatesInstance smi) => smi.master.wiltCondition.IsWilting()).PlayAnim("idle", KAnim.PlayMode.Loop);
			this.alive.wilting.PlayAnim("wilt1").EventTransition(GameHashes.WiltRecover, this.alive.mature, (OilEater.StatesInstance smi) => !smi.master.wiltCondition.IsWilting());
		}

		// Token: 0x04005A3B RID: 23099
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State grow;

		// Token: 0x04005A3C RID: 23100
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State blocked_from_growing;

		// Token: 0x04005A3D RID: 23101
		public OilEater.States.AliveStates alive;

		// Token: 0x04005A3E RID: 23102
		public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State dead;

		// Token: 0x020016D8 RID: 5848
		public class AliveStates : GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.PlantAliveSubState
		{
			// Token: 0x04005A3F RID: 23103
			public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State mature;

			// Token: 0x04005A40 RID: 23104
			public GameStateMachine<OilEater.States, OilEater.StatesInstance, OilEater, object>.State wilting;
		}
	}
}
