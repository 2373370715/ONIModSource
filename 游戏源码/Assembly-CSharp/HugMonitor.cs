using Klei.AI;
using STRINGS;
using UnityEngine;

public class HugMonitor : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>
{
	public class HUGTUNING
	{
		public const float HUG_EGG_TIME = 15f;

		public const float HUG_DUPE_WAIT = 60f;

		public const float FRENZY_EGGS_PER_CYCLE = 6f;

		public const float FRENZY_EGG_TRAVEL_TIME_BUFFER = 5f;

		public const float HUG_FRENZY_DURATION = 120f;
	}

	public class Def : BaseDef
	{
		public float hugsPerCycle = 2f;

		public float scanningInterval = 30f;

		public float hugFrenzyDuration = 120f;

		public float hugFrenzyCooldown = 480f;

		public float hugFrenzyCooldownFailed = 120f;

		public float scanningIntervalFrenzy = 15f;

		public int maxSearchCost = 30;
	}

	public class HugReadyStates : State
	{
		public State passiveHug;

		public State seekingHug;
	}

	public class NormalStates : State
	{
		public State idle;

		public HugReadyStates hugReady;
	}

	public new class Instance : GameInstance
	{
		public GameObject hugParticleFx;

		public Vector3 hugParticleOffset;

		public Effect frenzyEffect;

		public KPrefabID hugTarget;

		[MyCmpGet]
		private Navigator navigator;

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			frenzyEffect = Db.Get().effects.Get("HuggingFrenzy");
			RefreshSearchTime();
			if (hugEffect == null)
			{
				hugEffect = Db.Get().effects.Get("EggHug");
			}
			base.smi.sm.wantsHugCooldownTimer.Set(Random.Range(base.smi.def.hugFrenzyCooldownFailed, base.smi.def.hugFrenzyCooldown), base.smi);
		}

		private void RefreshSearchTime()
		{
			if (hugTarget == null)
			{
				base.smi.sm.hugEggCooldownTimer.Set(GetScanningInterval(), base.smi);
			}
			else
			{
				base.smi.sm.hugEggCooldownTimer.Set(GetHugInterval(), base.smi);
			}
		}

		private float GetScanningInterval()
		{
			if (!IsHuggingFrenzy())
			{
				return base.def.scanningInterval;
			}
			return base.def.scanningIntervalFrenzy;
		}

		private float GetHugInterval()
		{
			if (IsHuggingFrenzy())
			{
				return 0f;
			}
			return 600f / base.def.hugsPerCycle;
		}

		public bool IsHuggingFrenzy()
		{
			return base.smi.GetCurrentState() == base.smi.sm.hugFrenzy;
		}

		public bool IsHugging()
		{
			return base.smi.GetSMI<AnimInterruptMonitor.Instance>().anims != null;
		}

		public bool UpdateHasTarget()
		{
			if (hugTarget == null)
			{
				if (base.smi.sm.hugEggCooldownTimer.Get(base.smi) > 0f)
				{
					return false;
				}
				FindEgg();
				RefreshSearchTime();
			}
			return hugTarget != null;
		}

		public void EnterHuggingFrenzy()
		{
			base.smi.sm.hugFrenzyTimer.Set(base.smi.def.hugFrenzyDuration, base.smi);
			base.smi.sm.hugEggCooldownTimer.Set(0f, base.smi);
		}

		private void FindEgg()
		{
			int cell = Grid.PosToCell(base.gameObject);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			int num = base.def.maxSearchCost;
			hugTarget = null;
			if (cavityForCell == null)
			{
				return;
			}
			foreach (KPrefabID egg in cavityForCell.eggs)
			{
				KPrefabID kPrefabID = egg;
				if (kPrefabID.HasTag(GameTags.Creatures.ReservedByCreature) || kPrefabID.GetComponent<Effects>().HasEffect(hugEffect))
				{
					continue;
				}
				int num2 = Grid.PosToCell(kPrefabID);
				if (kPrefabID.HasTag(GameTags.Stored))
				{
					if (!Grid.ObjectLayers[1].TryGetValue(num2, out var value) || !value.TryGetComponent<KPrefabID>(out var component) || !component.IsPrefabID("EggIncubator"))
					{
						continue;
					}
					num2 = Grid.PosToCell(value);
					kPrefabID = component;
				}
				int navigationCost = navigator.GetNavigationCost(num2);
				if (navigationCost != -1 && navigationCost < num)
				{
					hugTarget = kPrefabID;
					num = navigationCost;
				}
			}
		}
	}

	private static string soundPath = GlobalAssets.GetSound("Squirrel_hug_frenzyFX");

	private static Effect hugEffect;

	private FloatParameter hugFrenzyTimer;

	private FloatParameter wantsHugCooldownTimer;

	private FloatParameter hugEggCooldownTimer;

	public NormalStates normal;

	public State hugFrenzy;

	public override void InitializeStates(out BaseState default_state)
	{
		default_state = normal;
		base.serializable = SerializeType.ParamsOnly;
		root.Update(UpdateHugEggCooldownTimer, UpdateRate.SIM_1000ms).ToggleBehaviour(GameTags.Creatures.WantsToTendEgg, (Instance smi) => smi.UpdateHasTarget(), delegate(Instance smi)
		{
			smi.hugTarget = null;
		});
		normal.DefaultState(normal.idle).ParamTransition(hugFrenzyTimer, hugFrenzy, GameStateMachine<HugMonitor, Instance, IStateMachineTarget, Def>.IsGTZero);
		normal.idle.ParamTransition(wantsHugCooldownTimer, normal.hugReady.seekingHug, GameStateMachine<HugMonitor, Instance, IStateMachineTarget, Def>.IsLTEZero).Update(UpdateWantsHugCooldownTimer, UpdateRate.SIM_1000ms);
		normal.hugReady.ToggleReactable(GetHugReactable);
		normal.hugReady.passiveHug.ParamTransition(wantsHugCooldownTimer, normal.hugReady.seekingHug, GameStateMachine<HugMonitor, Instance, IStateMachineTarget, Def>.IsLTEZero).Update(UpdateWantsHugCooldownTimer, UpdateRate.SIM_1000ms).ToggleStatusItem(CREATURES.STATUSITEMS.HUGMINIONWAITING.NAME, CREATURES.STATUSITEMS.HUGMINIONWAITING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, allow_multiples: false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		normal.hugReady.seekingHug.ToggleBehaviour(GameTags.Creatures.WantsAHug, (Instance smi) => true, delegate(Instance smi)
		{
			wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldownFailed, smi);
			smi.GoTo(normal.hugReady.passiveHug);
		});
		hugFrenzy.ParamTransition(hugFrenzyTimer, normal, (Instance smi, float p) => p <= 0f && !smi.IsHugging()).Update(UpdateHugFrenzyTimer, UpdateRate.SIM_1000ms).ToggleEffect((Instance smi) => smi.frenzyEffect)
			.ToggleLoopingSound(soundPath)
			.Enter(delegate(Instance smi)
			{
				smi.hugParticleFx = Util.KInstantiate(EffectPrefabs.Instance.HugFrenzyFX, smi.master.transform.GetPosition() + smi.hugParticleOffset);
				smi.hugParticleFx.transform.SetParent(smi.master.transform);
				smi.hugParticleFx.SetActive(value: true);
			})
			.Exit(delegate(Instance smi)
			{
				Util.KDestroyGameObject(smi.hugParticleFx);
				wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldown, smi);
			});
	}

	private Reactable GetHugReactable(Instance smi)
	{
		return new HugMinionReactable(smi.gameObject);
	}

	private void UpdateWantsHugCooldownTimer(Instance smi, float dt)
	{
		wantsHugCooldownTimer.DeltaClamp(0f - dt, 0f, float.MaxValue, smi);
	}

	private void UpdateHugEggCooldownTimer(Instance smi, float dt)
	{
		hugEggCooldownTimer.DeltaClamp(0f - dt, 0f, float.MaxValue, smi);
	}

	private void UpdateHugFrenzyTimer(Instance smi, float dt)
	{
		hugFrenzyTimer.DeltaClamp(0f - dt, 0f, float.MaxValue, smi);
	}
}
