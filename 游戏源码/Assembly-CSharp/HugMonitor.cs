using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A15 RID: 2581
public class HugMonitor : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>
{
	// Token: 0x06002F38 RID: 12088 RVA: 0x001F7654 File Offset: 0x001F5854
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Update(new Action<HugMonitor.Instance, float>(this.UpdateHugEggCooldownTimer), UpdateRate.SIM_1000ms, false).ToggleBehaviour(GameTags.Creatures.WantsToTendEgg, (HugMonitor.Instance smi) => smi.UpdateHasTarget(), delegate(HugMonitor.Instance smi)
		{
			smi.hugTarget = null;
		});
		this.normal.DefaultState(this.normal.idle).ParamTransition<float>(this.hugFrenzyTimer, this.hugFrenzy, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsGTZero);
		this.normal.idle.ParamTransition<float>(this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms, false);
		this.normal.hugReady.ToggleReactable(new Func<HugMonitor.Instance, Reactable>(this.GetHugReactable));
		GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State state = this.normal.hugReady.passiveHug.ParamTransition<float>(this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms, false);
		string name = CREATURES.STATUSITEMS.HUGMINIONWAITING.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUGMINIONWAITING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.normal.hugReady.seekingHug.ToggleBehaviour(GameTags.Creatures.WantsAHug, (HugMonitor.Instance smi) => true, delegate(HugMonitor.Instance smi)
		{
			this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldownFailed, smi, false);
			smi.GoTo(this.normal.hugReady.passiveHug);
		});
		this.hugFrenzy.ParamTransition<float>(this.hugFrenzyTimer, this.normal, (HugMonitor.Instance smi, float p) => p <= 0f && !smi.IsHugging()).Update(new Action<HugMonitor.Instance, float>(this.UpdateHugFrenzyTimer), UpdateRate.SIM_1000ms, false).ToggleEffect((HugMonitor.Instance smi) => smi.frenzyEffect).ToggleLoopingSound(HugMonitor.soundPath, null, true, true, true).Enter(delegate(HugMonitor.Instance smi)
		{
			smi.hugParticleFx = Util.KInstantiate(EffectPrefabs.Instance.HugFrenzyFX, smi.master.transform.GetPosition() + smi.hugParticleOffset);
			smi.hugParticleFx.transform.SetParent(smi.master.transform);
			smi.hugParticleFx.SetActive(true);
		}).Exit(delegate(HugMonitor.Instance smi)
		{
			Util.KDestroyGameObject(smi.hugParticleFx);
			this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldown, smi, false);
		});
	}

	// Token: 0x06002F39 RID: 12089 RVA: 0x000BEA39 File Offset: 0x000BCC39
	private Reactable GetHugReactable(HugMonitor.Instance smi)
	{
		return new HugMinionReactable(smi.gameObject);
	}

	// Token: 0x06002F3A RID: 12090 RVA: 0x000BEA46 File Offset: 0x000BCC46
	private void UpdateWantsHugCooldownTimer(HugMonitor.Instance smi, float dt)
	{
		this.wantsHugCooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x06002F3B RID: 12091 RVA: 0x000BEA61 File Offset: 0x000BCC61
	private void UpdateHugEggCooldownTimer(HugMonitor.Instance smi, float dt)
	{
		this.hugEggCooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x06002F3C RID: 12092 RVA: 0x000BEA7C File Offset: 0x000BCC7C
	private void UpdateHugFrenzyTimer(HugMonitor.Instance smi, float dt)
	{
		this.hugFrenzyTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x04001FDA RID: 8154
	private static string soundPath = GlobalAssets.GetSound("Squirrel_hug_frenzyFX", false);

	// Token: 0x04001FDB RID: 8155
	private static Effect hugEffect;

	// Token: 0x04001FDC RID: 8156
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugFrenzyTimer;

	// Token: 0x04001FDD RID: 8157
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter wantsHugCooldownTimer;

	// Token: 0x04001FDE RID: 8158
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugEggCooldownTimer;

	// Token: 0x04001FDF RID: 8159
	public HugMonitor.NormalStates normal;

	// Token: 0x04001FE0 RID: 8160
	public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State hugFrenzy;

	// Token: 0x02000A16 RID: 2582
	public class HUGTUNING
	{
		// Token: 0x04001FE1 RID: 8161
		public const float HUG_EGG_TIME = 15f;

		// Token: 0x04001FE2 RID: 8162
		public const float HUG_DUPE_WAIT = 60f;

		// Token: 0x04001FE3 RID: 8163
		public const float FRENZY_EGGS_PER_CYCLE = 6f;

		// Token: 0x04001FE4 RID: 8164
		public const float FRENZY_EGG_TRAVEL_TIME_BUFFER = 5f;

		// Token: 0x04001FE5 RID: 8165
		public const float HUG_FRENZY_DURATION = 120f;
	}

	// Token: 0x02000A17 RID: 2583
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001FE6 RID: 8166
		public float hugsPerCycle = 2f;

		// Token: 0x04001FE7 RID: 8167
		public float scanningInterval = 30f;

		// Token: 0x04001FE8 RID: 8168
		public float hugFrenzyDuration = 120f;

		// Token: 0x04001FE9 RID: 8169
		public float hugFrenzyCooldown = 480f;

		// Token: 0x04001FEA RID: 8170
		public float hugFrenzyCooldownFailed = 120f;

		// Token: 0x04001FEB RID: 8171
		public float scanningIntervalFrenzy = 15f;

		// Token: 0x04001FEC RID: 8172
		public int maxSearchCost = 30;
	}

	// Token: 0x02000A18 RID: 2584
	public class HugReadyStates : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
	{
		// Token: 0x04001FED RID: 8173
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State passiveHug;

		// Token: 0x04001FEE RID: 8174
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State seekingHug;
	}

	// Token: 0x02000A19 RID: 2585
	public class NormalStates : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
	{
		// Token: 0x04001FEF RID: 8175
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State idle;

		// Token: 0x04001FF0 RID: 8176
		public HugMonitor.HugReadyStates hugReady;
	}

	// Token: 0x02000A1A RID: 2586
	public new class Instance : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.GameInstance
	{
		// Token: 0x06002F45 RID: 12101 RVA: 0x001F7938 File Offset: 0x001F5B38
		public Instance(IStateMachineTarget master, HugMonitor.Def def) : base(master, def)
		{
			this.frenzyEffect = Db.Get().effects.Get("HuggingFrenzy");
			this.RefreshSearchTime();
			if (HugMonitor.hugEffect == null)
			{
				HugMonitor.hugEffect = Db.Get().effects.Get("EggHug");
			}
			base.smi.sm.wantsHugCooldownTimer.Set(UnityEngine.Random.Range(base.smi.def.hugFrenzyCooldownFailed, base.smi.def.hugFrenzyCooldown), base.smi, false);
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x001F79D0 File Offset: 0x001F5BD0
		private void RefreshSearchTime()
		{
			if (this.hugTarget == null)
			{
				base.smi.sm.hugEggCooldownTimer.Set(this.GetScanningInterval(), base.smi, false);
				return;
			}
			base.smi.sm.hugEggCooldownTimer.Set(this.GetHugInterval(), base.smi, false);
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x000BEB10 File Offset: 0x000BCD10
		private float GetScanningInterval()
		{
			if (!this.IsHuggingFrenzy())
			{
				return base.def.scanningInterval;
			}
			return base.def.scanningIntervalFrenzy;
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000BEB31 File Offset: 0x000BCD31
		private float GetHugInterval()
		{
			if (this.IsHuggingFrenzy())
			{
				return 0f;
			}
			return 600f / base.def.hugsPerCycle;
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000BEB52 File Offset: 0x000BCD52
		public bool IsHuggingFrenzy()
		{
			return base.smi.GetCurrentState() == base.smi.sm.hugFrenzy;
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x000BEB71 File Offset: 0x000BCD71
		public bool IsHugging()
		{
			return base.smi.GetSMI<AnimInterruptMonitor.Instance>().anims != null;
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x001F7A34 File Offset: 0x001F5C34
		public bool UpdateHasTarget()
		{
			if (this.hugTarget == null)
			{
				if (base.smi.sm.hugEggCooldownTimer.Get(base.smi) > 0f)
				{
					return false;
				}
				this.FindEgg();
				this.RefreshSearchTime();
			}
			return this.hugTarget != null;
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x001F7A8C File Offset: 0x001F5C8C
		public void EnterHuggingFrenzy()
		{
			base.smi.sm.hugFrenzyTimer.Set(base.smi.def.hugFrenzyDuration, base.smi, false);
			base.smi.sm.hugEggCooldownTimer.Set(0f, base.smi, false);
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x001F7AE8 File Offset: 0x001F5CE8
		private void FindEgg()
		{
			int cell = Grid.PosToCell(base.gameObject);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			int num = base.def.maxSearchCost;
			this.hugTarget = null;
			if (cavityForCell != null)
			{
				foreach (KPrefabID kprefabID in cavityForCell.eggs)
				{
					if (!kprefabID.HasTag(GameTags.Creatures.ReservedByCreature) && !kprefabID.GetComponent<Effects>().HasEffect(HugMonitor.hugEffect))
					{
						int num2 = Grid.PosToCell(kprefabID);
						if (kprefabID.HasTag(GameTags.Stored))
						{
							GameObject gameObject;
							KPrefabID kprefabID2;
							if (!Grid.ObjectLayers[1].TryGetValue(num2, out gameObject) || !gameObject.TryGetComponent<KPrefabID>(out kprefabID2) || !kprefabID2.IsPrefabID("EggIncubator"))
							{
								continue;
							}
							num2 = Grid.PosToCell(gameObject);
							kprefabID = kprefabID2;
						}
						int navigationCost = this.navigator.GetNavigationCost(num2);
						if (navigationCost != -1 && navigationCost < num)
						{
							this.hugTarget = kprefabID;
							num = navigationCost;
						}
					}
				}
			}
		}

		// Token: 0x04001FF1 RID: 8177
		public GameObject hugParticleFx;

		// Token: 0x04001FF2 RID: 8178
		public Vector3 hugParticleOffset;

		// Token: 0x04001FF3 RID: 8179
		public Effect frenzyEffect;

		// Token: 0x04001FF4 RID: 8180
		public KPrefabID hugTarget;

		// Token: 0x04001FF5 RID: 8181
		[MyCmpGet]
		private Navigator navigator;
	}
}
