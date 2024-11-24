using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000FE2 RID: 4066
public class Telepad : StateMachineComponent<Telepad.StatesInstance>
{
	// Token: 0x060052A2 RID: 21154 RVA: 0x00275A4C File Offset: 0x00273C4C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<Deconstructable>().allowDeconstruction = false;
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		if (num == 0)
		{
			global::Debug.LogError(string.Concat(new string[]
			{
				"Headquarters spawned at: (",
				num.ToString(),
				",",
				num2.ToString(),
				")"
			}));
		}
	}

	// Token: 0x060052A3 RID: 21155 RVA: 0x00275AC0 File Offset: 0x00273CC0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Telepads.Add(this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
		base.smi.StartSM();
	}

	// Token: 0x060052A4 RID: 21156 RVA: 0x000D5DD1 File Offset: 0x000D3FD1
	protected override void OnCleanUp()
	{
		Components.Telepads.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060052A5 RID: 21157 RVA: 0x00275B44 File Offset: 0x00273D44
	public void Update()
	{
		if (base.smi.IsColonyLost())
		{
			return;
		}
		if (Immigration.Instance.ImmigrantsAvailable && base.GetComponent<Operational>().IsOperational)
		{
			base.smi.sm.openPortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NewDuplicantsAvailable, this);
		}
		else
		{
			base.smi.sm.closePortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Wattson, this);
		}
		if (this.GetTimeRemaining() < -120f)
		{
			Messenger.Instance.QueueMessage(new DuplicantsLeftMessage());
			Immigration.Instance.EndImmigration();
		}
	}

	// Token: 0x060052A6 RID: 21158 RVA: 0x000D5DE4 File Offset: 0x000D3FE4
	public void RejectAll()
	{
		Immigration.Instance.EndImmigration();
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	// Token: 0x060052A7 RID: 21159 RVA: 0x00275C30 File Offset: 0x00273E30
	public void OnAcceptDelivery(ITelepadDeliverable delivery)
	{
		int cell = Grid.PosToCell(this);
		Immigration.Instance.EndImmigration();
		GameObject gameObject = delivery.Deliver(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
		if (component != null)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, GameClock.Instance.GetTimeSinceStartOfReport(), string.Format(UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, DUPLICANTS.CHORES.NOT_EXISTING_TASK), gameObject.GetProperName());
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.gameObject.GetComponent<KSelectable>().GetMyWorldId(), false))
			{
				minionIdentity.GetComponent<Effects>().Add("NewCrewArrival", true);
			}
			MinionResume component2 = component.GetComponent<MinionResume>();
			int num = 0;
			while ((float)num < this.startingSkillPoints)
			{
				component2.ForceAddSkillPoint();
				num++;
			}
			if (component.HasTag(GameTags.Minions.Models.Bionic))
			{
				GameScheduler.Instance.Schedule("BonusBatteryDelivery", 5f, delegate(object data)
				{
					base.Trigger(1982288670, null);
				}, null, null);
			}
		}
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	// Token: 0x060052A8 RID: 21160 RVA: 0x000D5E0C File Offset: 0x000D400C
	public float GetTimeRemaining()
	{
		return Immigration.Instance.GetTimeRemaining();
	}

	// Token: 0x040039B6 RID: 14774
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x040039B7 RID: 14775
	private MeterController meter;

	// Token: 0x040039B8 RID: 14776
	private const float MAX_IMMIGRATION_TIME = 120f;

	// Token: 0x040039B9 RID: 14777
	private const int NUM_METER_NOTCHES = 8;

	// Token: 0x040039BA RID: 14778
	private List<MinionStartingStats> minionStats;

	// Token: 0x040039BB RID: 14779
	public float startingSkillPoints;

	// Token: 0x040039BC RID: 14780
	public static readonly HashedString[] PortalBirthAnim = new HashedString[]
	{
		"portalbirth"
	};

	// Token: 0x02000FE3 RID: 4067
	public class StatesInstance : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.GameInstance
	{
		// Token: 0x060052AC RID: 21164 RVA: 0x000D5E4C File Offset: 0x000D404C
		public StatesInstance(Telepad master) : base(master)
		{
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x000D5E55 File Offset: 0x000D4055
		public bool IsColonyLost()
		{
			return GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver();
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x00275D74 File Offset: 0x00273F74
		public void UpdateMeter()
		{
			float timeRemaining = Immigration.Instance.GetTimeRemaining();
			float totalWaitTime = Immigration.Instance.GetTotalWaitTime();
			float positionPercent = Mathf.Clamp01(1f - timeRemaining / totalWaitTime);
			base.master.meter.SetPositionPercent(positionPercent);
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x000D5E70 File Offset: 0x000D4070
		public IEnumerator SpawnExtraPowerBanks()
		{
			int cellTarget = Grid.OffsetCell(Grid.PosToCell(base.gameObject), 1, 2);
			int count = 5;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, MISC.POPFX.EXTRA_POWERBANKS_BIONIC, base.gameObject.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("SandboxTool_Spawner", false));
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_BasicSingleHarvestPlant"), Grid.CellToPosCBC(cellTarget, Grid.SceneLayer.Front) - Vector3.right / 2f);
				gameObject.SetActive(true);
				Vector2 initial_velocity = new Vector2((-2.5f + 5f * ((float)i / 5f)) / 2f, 2f);
				if (GameComps.Fallers.Has(gameObject))
				{
					GameComps.Fallers.Remove(gameObject);
				}
				GameComps.Fallers.Add(gameObject, initial_velocity);
				yield return new WaitForSeconds(0.25f);
				num = i;
			}
			yield return 0;
			yield break;
		}
	}

	// Token: 0x02000FE5 RID: 4069
	public class States : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad>
	{
		// Token: 0x060052B6 RID: 21174 RVA: 0x00275F5C File Offset: 0x0027415C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.OnSignal(this.idlePortal, this.resetToIdle).EventTransition(GameHashes.BonusTelepadDelivery, this.bonusDelivery.pre, null);
			this.resetToIdle.GoTo(this.idle);
			this.idle.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.UpdateMeter();
			}).Update("TelepadMeter", delegate(Telepad.StatesInstance smi, float dt)
			{
				smi.UpdateMeter();
			}, UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).PlayAnim("idle").OnSignal(this.openPortal, this.opening);
			this.unoperational.PlayAnim("idle").Enter("StopImmigration", delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).EventTransition(GameHashes.OperationalChanged, this.idle, (Telepad.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.opening.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.open);
			this.open.OnSignal(this.closePortal, this.close).Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_loop", KAnim.PlayMode.Loop).Transition(this.close, (Telepad.StatesInstance smi) => smi.IsColonyLost(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OperationalChanged, this.close, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.close.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).PlayAnims((Telepad.StatesInstance smi) => Telepad.States.workingAnims, KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
			this.bonusDelivery.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.bonusDelivery.loop);
			this.bonusDelivery.loop.PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleAction("SpawnBonusDelivery", 1f, delegate(Telepad.StatesInstance smi)
			{
				smi.master.StartCoroutine(smi.SpawnExtraPowerBanks());
			}).ScheduleGoTo(3f, this.bonusDelivery.pst);
			this.bonusDelivery.pst.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
		}

		// Token: 0x040039C3 RID: 14787
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal openPortal;

		// Token: 0x040039C4 RID: 14788
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal closePortal;

		// Token: 0x040039C5 RID: 14789
		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal idlePortal;

		// Token: 0x040039C6 RID: 14790
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State idle;

		// Token: 0x040039C7 RID: 14791
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State resetToIdle;

		// Token: 0x040039C8 RID: 14792
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State opening;

		// Token: 0x040039C9 RID: 14793
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State open;

		// Token: 0x040039CA RID: 14794
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State close;

		// Token: 0x040039CB RID: 14795
		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State unoperational;

		// Token: 0x040039CC RID: 14796
		public Telepad.States.BonusDeliveryStates bonusDelivery;

		// Token: 0x040039CD RID: 14797
		private static readonly HashedString[] workingAnims = new HashedString[]
		{
			"working_loop",
			"working_pst"
		};

		// Token: 0x02000FE6 RID: 4070
		public class BonusDeliveryStates : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State
		{
			// Token: 0x040039CE RID: 14798
			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State pre;

			// Token: 0x040039CF RID: 14799
			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State loop;

			// Token: 0x040039D0 RID: 14800
			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State pst;
		}
	}
}
