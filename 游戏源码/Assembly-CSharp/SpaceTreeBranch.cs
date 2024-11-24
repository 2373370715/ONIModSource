using Klei.AI;
using STRINGS;
using UnityEngine;

public class SpaceTreeBranch : GameStateMachine<SpaceTreeBranch, SpaceTreeBranch.Instance, IStateMachineTarget, SpaceTreeBranch.Def>
{
	public class AnimSet
	{
		public string[] meterTargets;

		public string[] meterAnimNames;

		public string undeveloped;

		public string spawn;

		public string spawn_pst;

		public string fill;

		public string ready_harvest;

		public string[] meterAnim_flowerWilted;

		public string wilted;

		public string wilted_short_trunk_healthy;

		public string wilted_short_trunk_wilted;

		public string hidden;

		public string die;

		public string manual_harvest_pre;

		public string manual_harvest_loop;

		public string manual_harvest_pst;
	}

	public class Def : BaseDef
	{
		public int OPTIMAL_LUX_LEVELS;

		public float GROWTH_RATE = 0.0016666667f;

		public float WILD_GROWTH_RATE = 0.00041666668f;
	}

	public class GrowingState : State
	{
		public State visible;

		public State hidden;
	}

	public class GrowingStates : PlantAliveSubState
	{
		public GrowingState wild;

		public GrowingState planted;
	}

	public class GrownStates : PlantAliveSubState
	{
		public State spawn;

		public State spawnPST;

		public HealthyStates healthy;

		public WiltStates trunkWilted;
	}

	public class GrowHaltState : PlantAliveSubState
	{
		public State wilted;

		public State trunkWilted;

		public State shaking;

		public State hidden;
	}

	public class WiltStates : State
	{
		public State wilted;

		public State shaking;
	}

	public class DieStates : State
	{
		public State entering;

		public State selfDelete;
	}

	public class ReadyForHarvest : State
	{
		public State idle;

		public State shaking;

		public ManualHarvestStates harvestInProgress;
	}

	public class ManualHarvestStates : State
	{
		public State pre;

		public State loop;

		public State pst;
	}

	public class HealthyStates : State
	{
		public State filling;

		public ReadyForHarvest trunkReadyForHarvest;
	}

	public new class Instance : GameInstance, IManageGrowingStates
	{
		[MyCmpGet]
		public WiltCondition wiltCondition;

		[MyCmpGet]
		public Crop crop;

		[MyCmpGet]
		public Harvestable harvestable;

		[MyCmpGet]
		public KBatchedAnimController animController;

		public AnimSet Animations = new AnimSet();

		private int cell;

		private float lastFillAmountRecorded;

		private AmountInstance maturity;

		public AttributeModifier baseGrowingRate;

		public AttributeModifier wildGrowingRate;

		public UnstableEntombDefense.Instance entombDefenseSMI;

		private MeterController[] flowerMeters;

		private PlantBranch.Instance branch;

		private KBatchedAnimController trunkAnimController;

		private PlantBranchGrower.Instance _trunk;

		public int CurrentAmountOfLux => Grid.LightIntensity[cell];

		public float Productivity
		{
			get
			{
				if (!IsBranchFullyGrown)
				{
					return 0f;
				}
				return Mathf.Clamp((float)CurrentAmountOfLux / (float)base.def.OPTIMAL_LUX_LEVELS, 0f, 1f);
			}
		}

		public bool IsTrunkHealthy
		{
			get
			{
				if (trunk != null)
				{
					return !trunk.HasTag(GameTags.Wilting);
				}
				return false;
			}
		}

		public bool IsTrunkWildPlanted
		{
			get
			{
				if (trunk != null)
				{
					return !trunk.GetComponent<ReceptacleMonitor>().Replanted;
				}
				return false;
			}
		}

		public bool IsEntombed
		{
			get
			{
				if (entombDefenseSMI != null)
				{
					return entombDefenseSMI.IsEntombed;
				}
				return false;
			}
		}

		public bool IsBranchFullyGrown => GetcurrentGrowthPercentage() >= 1f;

		private PlantBranchGrower.Instance trunk
		{
			get
			{
				if (_trunk == null)
				{
					_trunk = branch.GetTrunk();
					if (_trunk != null)
					{
						trunkAnimController = _trunk.GetComponent<KBatchedAnimController>();
					}
				}
				return _trunk;
			}
		}

		public void OverrideMaturityLevel(float percent)
		{
			float value = maturity.GetMax() * percent;
			maturity.SetValue(value);
		}

		public Instance(IStateMachineTarget master, Def def)
			: base(master, def)
		{
			cell = Grid.PosToCell(this);
			Amounts amounts = base.gameObject.GetAmounts();
			maturity = amounts.Get(Db.Get().Amounts.Maturity);
			baseGrowingRate = new AttributeModifier(maturity.deltaAttribute.Id, def.GROWTH_RATE, CREATURES.STATS.MATURITY.GROWING);
			wildGrowingRate = new AttributeModifier(maturity.deltaAttribute.Id, def.WILD_GROWTH_RATE, CREATURES.STATS.MATURITY.GROWINGWILD);
			Subscribe(1272413801, ResetGrowth);
		}

		public float GetcurrentGrowthPercentage()
		{
			return maturity.value / maturity.GetMax();
		}

		public void ResetGrowth(object data = null)
		{
			maturity.value = 0f;
			base.sm.HasSpawn.Set(value: false, this);
			base.smi.gameObject.Trigger(-254803949);
		}

		public override void StartSM()
		{
			branch = base.smi.GetSMI<PlantBranch.Instance>();
			entombDefenseSMI = base.smi.GetSMI<UnstableEntombDefense.Instance>();
			if (Animations.meterTargets != null)
			{
				CreateMeters(Animations.meterTargets, Animations.meterAnimNames);
			}
			base.StartSM();
		}

		public void CreateMeters(string[] meterTargets, string[] meterAnimNames)
		{
			flowerMeters = new MeterController[meterTargets.Length];
			for (int i = 0; i < flowerMeters.Length; i++)
			{
				flowerMeters[i] = new MeterController(animController, meterTargets[i], meterAnimNames[i], Meter.Offset.NoChange, Grid.SceneLayer.Building);
			}
		}

		public void RefreshAnimation()
		{
			if (flowerMeters == null && Animations.meterTargets != null)
			{
				CreateMeters(Animations.meterTargets, Animations.meterAnimNames);
			}
			KAnim.PlayMode mode = ((!IsInsideState(base.sm.grown.healthy)) ? KAnim.PlayMode.Once : KAnim.PlayMode.Loop);
			KBatchedAnimController component = GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				component.Play(GetAnimationForState(GetCurrentState()), mode);
			}
			if (IsInsideState(base.smi.sm.grown.healthy))
			{
				ActivateGlowFlowerMeter();
			}
			else
			{
				DeactivateGlowFlowerMeter();
			}
		}

		public HashedString GetCurrentTrunkAnim()
		{
			if (trunk != null && trunkAnimController != null)
			{
				return trunkAnimController.currentAnim;
			}
			return null;
		}

		public void SynchCurrentAnimWithTrunkAnim(HashedString trunkAnimNameToSynchTo)
		{
			if (trunk != null && trunkAnimController != null && trunkAnimController.currentAnim == trunkAnimNameToSynchTo)
			{
				float elapsedTime = trunkAnimController.GetElapsedTime();
				base.smi.animController.SetElapsedTime(elapsedTime);
			}
		}

		public string GetAnimationForState(BaseState state)
		{
			if (state == base.sm.growing.wild.visible)
			{
				return Animations.undeveloped;
			}
			if (state == base.sm.growing.planted.visible)
			{
				return Animations.undeveloped;
			}
			if (state == base.sm.growing.wild.hidden)
			{
				return Animations.hidden;
			}
			if (state == base.sm.growing.planted.hidden)
			{
				return Animations.hidden;
			}
			if (state == base.sm.grown.spawn)
			{
				return Animations.spawn;
			}
			if (state == base.sm.grown.spawnPST)
			{
				return Animations.spawn_pst;
			}
			if (state == base.sm.grown.healthy.filling)
			{
				return Animations.fill;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.idle)
			{
				return Animations.ready_harvest;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.pre)
			{
				return Animations.manual_harvest_pre;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.loop)
			{
				return Animations.manual_harvest_loop;
			}
			if (state == base.sm.grown.healthy.trunkReadyForHarvest.harvestInProgress.pst)
			{
				return Animations.manual_harvest_pst;
			}
			if (state == base.sm.grown.trunkWilted)
			{
				return Animations.wilted;
			}
			if (state == base.sm.halt.wilted)
			{
				return Animations.wilted_short_trunk_healthy;
			}
			if (state == base.sm.halt.trunkWilted)
			{
				return Animations.wilted_short_trunk_wilted;
			}
			if (state == base.sm.halt.shaking)
			{
				return Animations.hidden;
			}
			if (state == base.sm.halt.hidden)
			{
				return Animations.hidden;
			}
			if (state == base.sm.harvestedForWood)
			{
				return Animations.die;
			}
			if (state == base.sm.die.entering)
			{
				return Animations.die;
			}
			return Animations.spawn;
		}

		public string GetFillAnimNameForState(BaseState state)
		{
			string fill = Animations.fill;
			if (state == base.sm.grown.healthy.filling)
			{
				return Animations.fill;
			}
			if (state == base.sm.growing.wild.visible)
			{
				return Animations.undeveloped;
			}
			if (state == base.sm.growing.planted.visible)
			{
				return Animations.undeveloped;
			}
			if (state == base.sm.halt.wilted)
			{
				return Animations.wilted_short_trunk_healthy;
			}
			return fill;
		}

		public void PlayReadyForHarvestAnimation()
		{
			if (animController != null)
			{
				animController.Play(Animations.ready_harvest, KAnim.PlayMode.Loop);
			}
		}

		public void PlayFillAnimation()
		{
			PlayFillAnimation(lastFillAmountRecorded);
		}

		public void PlayFillAnimation(float fillLevel)
		{
			string fillAnimNameForState = GetFillAnimNameForState(base.smi.GetCurrentState());
			lastFillAmountRecorded = fillLevel;
			if ((!entombDefenseSMI.IsEntombed || !entombDefenseSMI.IsActive) && animController != null)
			{
				int num = Mathf.FloorToInt(fillLevel * 42f);
				if (animController.currentAnim != fillAnimNameForState)
				{
					animController.Play(fillAnimNameForState, KAnim.PlayMode.Once, 0f);
				}
				if (animController.currentFrame != num)
				{
					animController.SetPositionPercent(fillLevel);
				}
			}
		}

		public void ActivateGlowFlowerMeter()
		{
			if (flowerMeters != null)
			{
				for (int i = 0; i < flowerMeters.Length; i++)
				{
					flowerMeters[i].gameObject.SetActive(value: true);
					flowerMeters[i].meterController.Play(flowerMeters[i].meterController.currentAnim, KAnim.PlayMode.Loop);
				}
			}
		}

		public void PlayAnimOnFlower(string[] animNames, KAnim.PlayMode playMode)
		{
			if (flowerMeters != null)
			{
				for (int i = 0; i < flowerMeters.Length; i++)
				{
					flowerMeters[i].meterController.Play(animNames[i], playMode);
				}
			}
		}

		public void DeactivateGlowFlowerMeter()
		{
			if (flowerMeters != null)
			{
				for (int i = 0; i < flowerMeters.Length; i++)
				{
					flowerMeters[i].gameObject.SetActive(value: false);
				}
			}
		}

		public float TimeUntilNextHarvest()
		{
			return (maturity.GetMax() - maturity.value) / maturity.GetDelta();
		}

		public float PercentGrown()
		{
			return GetcurrentGrowthPercentage();
		}

		public Crop GetGropComponent()
		{
			return GetComponent<Crop>();
		}

		public float DomesticGrowthTime()
		{
			return maturity.GetMax() / base.smi.baseGrowingRate.Value;
		}

		public float WildGrowthTime()
		{
			return maturity.GetMax() / base.smi.wildGrowingRate.Value;
		}
	}

	public const int FILL_ANIM_FRAME_COUNT = 42;

	public const int SHAKE_ANIM_FRAME_COUNT = 54;

	public const float SHAKE_ANIM_DURATION = 1.8f;

	private BoolParameter HasSpawn;

	private GrowingStates growing;

	private GrowHaltState halt;

	private GrownStates grown;

	private State harvestedForWood;

	private DieStates die;

	public override void InitializeStates(out BaseState default_state)
	{
		base.serializable = SerializeType.ParamsOnly;
		default_state = growing;
		root.EventTransition(GameHashes.Uprooted, die).EventHandler(GameHashes.Wilt, UpdateFlowerOnWilt).EventHandler(GameHashes.WiltRecover, UpdateFlowerOnWiltRecover);
		growing.InitializeStates(masterTarget, die).EnterTransition(grown, IsBranchFullyGrown).Enter(DisableEntombDefenses)
			.Enter(DisableGlowFlowerMeter)
			.Enter(ForbidBranchToBeHarvestedForWood)
			.EventTransition(GameHashes.Wilt, halt, IsWiltedConditionReportingWilted)
			.EventTransition(GameHashes.RootHealthChanged, halt, GameStateMachine<SpaceTreeBranch, Instance, IStateMachineTarget, Def>.Not(IsTrunkHealthy))
			.EventTransition(GameHashes.PlanterStorage, growing.planted, GameStateMachine<SpaceTreeBranch, Instance, IStateMachineTarget, Def>.Not(IsTrunkWildPlanted))
			.EventTransition(GameHashes.PlanterStorage, growing.wild, IsTrunkWildPlanted)
			.ToggleStatusItem(Db.Get().CreatureStatusItems.Growing)
			.Update("CheckGrown", delegate(Instance smi, float dt)
			{
				if (smi.GetcurrentGrowthPercentage() >= 1f)
				{
					smi.gameObject.Trigger(-254803949);
					smi.GoTo(grown);
				}
			}, UpdateRate.SIM_4000ms);
		growing.wild.DefaultState(growing.wild.visible).EnterTransition(growing.planted, GameStateMachine<SpaceTreeBranch, Instance, IStateMachineTarget, Def>.Not(IsTrunkWildPlanted)).ToggleAttributeModifier("GrowingWild", (Instance smi) => smi.wildGrowingRate);
		growing.wild.visible.PlayAnim((Instance smi) => smi.GetAnimationForState(growing.wild.visible), KAnim.PlayMode.Paused).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, OnTrunkSyrupFullnessChanged).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, growing.wild.hidden)
			.Enter(PlayFillAnimationForThisState);
		growing.wild.hidden.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, growing.wild.visible, on_remove: true).PlayAnim((Instance smi) => smi.GetAnimationForState(growing.wild.hidden));
		growing.planted.DefaultState(growing.planted.visible).EnterTransition(growing.wild, IsTrunkWildPlanted).ToggleAttributeModifier("Growing", (Instance smi) => smi.baseGrowingRate);
		growing.planted.visible.PlayAnim((Instance smi) => smi.GetAnimationForState(growing.planted.visible), KAnim.PlayMode.Paused).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, OnTrunkSyrupFullnessChanged).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, growing.planted.hidden)
			.Enter(PlayFillAnimationForThisState);
		growing.planted.hidden.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, growing.planted.visible, on_remove: true).PlayAnim((Instance smi) => smi.GetAnimationForState(growing.planted.hidden));
		halt.InitializeStates(masterTarget, die).DefaultState(halt.wilted).EventHandlerTransition(GameHashes.RootHealthChanged, growing, (Instance smi, object o) => IsTrunkHealthy(smi) && !IsWiltedConditionReportingWilted(smi))
			.EventTransition(GameHashes.WiltRecover, growing)
			.Enter(DisableEntombDefenses)
			.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, halt.hidden);
		halt.wilted.PlayAnim((Instance smi) => smi.GetAnimationForState(halt.wilted), KAnim.PlayMode.Paused).EventTransition(GameHashes.RootHealthChanged, halt.trunkWilted, GameStateMachine<SpaceTreeBranch, Instance, IStateMachineTarget, Def>.Not(IsTrunkHealthy)).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, OnTrunkSyrupFullnessChanged)
			.Enter(PlayFillAnimationForThisState)
			.EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, halt.shaking, (Instance o, object smi) => true);
		halt.trunkWilted.EventTransition(GameHashes.RootHealthChanged, halt.wilted, IsTrunkHealthy).PlayAnim((Instance smi) => smi.GetAnimationForState(halt.trunkWilted)).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, halt.shaking, (Instance o, object smi) => true);
		halt.shaking.PlayAnim((Instance smi) => smi.GetAnimationForState(halt.shaking)).ScheduleGoTo(1.8f, halt.wilted);
		halt.hidden.PlayAnim((Instance smi) => smi.GetAnimationForState(halt.hidden)).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, halt.wilted, on_remove: true);
		grown.InitializeStates(masterTarget, die).Enter(EnableEntombDefenses).Enter(AllowItToBeHarvestForWood)
			.EventTransition(GameHashes.Harvest, harvestedForWood)
			.DefaultState(grown.spawn);
		grown.spawn.EventTransition(GameHashes.Wilt, grown.trunkWilted, GameStateMachine<SpaceTreeBranch, Instance, IStateMachineTarget, Def>.Not(IsTrunkHealthy)).EventTransition(GameHashes.RootHealthChanged, grown.trunkWilted, GameStateMachine<SpaceTreeBranch, Instance, IStateMachineTarget, Def>.Not(IsTrunkHealthy)).ParamTransition(HasSpawn, grown.healthy, GameStateMachine<SpaceTreeBranch, Instance, IStateMachineTarget, Def>.IsTrue)
			.Enter(DisableGlowFlowerMeter)
			.PlayAnim((Instance smi) => smi.GetAnimationForState(grown.spawn))
			.OnAnimQueueComplete(grown.spawnPST);
		grown.spawnPST.PlayAnim((Instance smi) => smi.GetAnimationForState(grown.spawnPST)).OnAnimQueueComplete(grown.healthy);
		grown.healthy.Enter(delegate(Instance smi)
		{
			HasSpawn.Set(value: true, smi);
		}).Enter(PlayFillAnimationForThisState).Enter(EnableGlowFlowerMeter)
			.Exit(DisableGlowFlowerMeter)
			.ToggleStatusItem(Db.Get().CreatureStatusItems.SpaceTreeBranchLightStatus)
			.DefaultState(grown.healthy.filling);
		grown.healthy.filling.EventHandler(GameHashes.EntombedChanged, PlayFillAnimationOnUnentomb).EventHandler(GameHashes.SpaceTreeInternalSyrupChanged, OnTrunkSyrupFullnessChanged).EventTransition(GameHashes.Wilt, grown.trunkWilted, GameStateMachine<SpaceTreeBranch, Instance, IStateMachineTarget, Def>.Not(IsTrunkHealthy))
			.EventTransition(GameHashes.RootHealthChanged, grown.trunkWilted, GameStateMachine<SpaceTreeBranch, Instance, IStateMachineTarget, Def>.Not(IsTrunkHealthy))
			.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, grown.healthy.trunkReadyForHarvest);
		grown.healthy.trunkReadyForHarvest.DefaultState(grown.healthy.trunkReadyForHarvest.idle).TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, grown.healthy.filling, on_remove: true);
		grown.healthy.trunkReadyForHarvest.idle.PlayAnim((Instance smi) => smi.GetAnimationForState(grown.healthy.trunkReadyForHarvest.idle), KAnim.PlayMode.Loop).EventHandler(GameHashes.EntombedChanged, PlayReadyForHarvestAnimationOnUnentomb).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, grown.healthy.trunkReadyForHarvest.shaking, (Instance o, object smi) => true)
			.EventTransition(GameHashes.SpaceTreeManualHarvestBegan, grown.healthy.trunkReadyForHarvest.harvestInProgress)
			.Update(delegate(Instance smi, float dt)
			{
				SynchAnimationWithTrunk(smi, "harvest_ready");
			});
		grown.healthy.trunkReadyForHarvest.harvestInProgress.DefaultState(grown.healthy.trunkReadyForHarvest.harvestInProgress.pre).EventTransition(GameHashes.SpaceTreeManualHarvestStopped, grown.healthy.trunkReadyForHarvest.idle);
		grown.healthy.trunkReadyForHarvest.harvestInProgress.pre.PlayAnim((Instance smi) => smi.GetAnimationForState(grown.healthy.trunkReadyForHarvest.harvestInProgress.pre)).Update(delegate(Instance smi, float dt)
		{
			SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_pre");
		}).Transition(grown.healthy.trunkReadyForHarvest.harvestInProgress.loop, TransitToManualHarvest_Loop);
		grown.healthy.trunkReadyForHarvest.harvestInProgress.loop.PlayAnim((Instance smi) => smi.GetAnimationForState(grown.healthy.trunkReadyForHarvest.harvestInProgress.loop), KAnim.PlayMode.Loop).Update(delegate(Instance smi, float dt)
		{
			SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_loop");
		}).Transition(grown.healthy.trunkReadyForHarvest.harvestInProgress.pst, TransitToManualHarvest_Pst);
		grown.healthy.trunkReadyForHarvest.harvestInProgress.pst.PlayAnim((Instance smi) => smi.GetAnimationForState(grown.healthy.trunkReadyForHarvest.harvestInProgress.pst)).Update(delegate(Instance smi, float dt)
		{
			SynchAnimationWithTrunk(smi, "syrup_harvest_trunk_pst");
		});
		grown.healthy.trunkReadyForHarvest.shaking.PlayAnim((Instance smi) => smi.entombDefenseSMI.UnentombAnimName).OnAnimQueueComplete(grown.healthy.trunkReadyForHarvest.idle);
		grown.trunkWilted.DefaultState(grown.trunkWilted.wilted).EventTransition(GameHashes.RootHealthChanged, grown.spawn, IsTrunkHealthy).EventTransition(GameHashes.WiltRecover, grown.spawn)
			.TagTransition(SpaceTreePlant.SpaceTreeReadyForHarvest, grown.healthy.trunkReadyForHarvest);
		grown.trunkWilted.wilted.PlayAnim((Instance smi) => smi.GetAnimationForState(grown.trunkWilted)).EventHandlerTransition(GameHashes.SpaceTreeUnentombDefenseTriggered, grown.trunkWilted.shaking, (Instance o, object smi) => true);
		grown.trunkWilted.shaking.PlayAnim((Instance smi) => smi.entombDefenseSMI.UnentombAnimName).OnAnimQueueComplete(grown.trunkWilted.wilted);
		harvestedForWood.PlayAnim((Instance smi) => smi.GetAnimationForState(harvestedForWood)).Enter(DisableEntombDefenses).Enter(SpawnWoodOnHarvest)
			.Enter(ForbidBranchToBeHarvestedForWood)
			.Exit(delegate(Instance smi)
			{
				smi.Trigger(113170146);
			})
			.OnAnimQueueComplete(growing);
		die.Enter(DisableEntombDefenses).DefaultState(die.entering);
		die.entering.PlayAnim((Instance smi) => smi.GetAnimationForState(die.entering)).Enter(SpawnWoodOnDeath).OnAnimQueueComplete(die.selfDelete)
			.ScheduleGoTo(2f, die.selfDelete);
		die.selfDelete.Enter(SelfDestroy);
	}

	public static bool TransitToManualHarvest_Loop(Instance smi)
	{
		if (smi.GetCurrentTrunkAnim() != null)
		{
			return smi.GetCurrentTrunkAnim() == "syrup_harvest_trunk_loop";
		}
		return false;
	}

	public static bool TransitToManualHarvest_Pst(Instance smi)
	{
		if (smi.GetCurrentTrunkAnim() != null)
		{
			return smi.GetCurrentTrunkAnim() == "syrup_harvest_trunk_pst";
		}
		return false;
	}

	public static bool IsWiltedConditionReportingWilted(Instance smi)
	{
		return smi.wiltCondition.IsWilting();
	}

	public static bool IsBranchFullyGrown(Instance smi)
	{
		return smi.IsBranchFullyGrown;
	}

	public static bool IsTrunkWildPlanted(Instance smi)
	{
		return smi.IsTrunkWildPlanted;
	}

	public static bool IsEntombed(Instance smi)
	{
		return smi.IsEntombed;
	}

	public static bool IsTrunkHealthy(Instance smi)
	{
		return smi.IsTrunkHealthy;
	}

	public static void PlayFillAnimationForThisState(Instance smi)
	{
		smi.PlayFillAnimation();
	}

	public static void OnTrunkSyrupFullnessChanged(Instance smi, object obj)
	{
		smi.PlayFillAnimation((float)obj);
	}

	public static void SynchAnimationWithTrunk(Instance smi, HashedString animName)
	{
		smi.SynchCurrentAnimWithTrunkAnim(animName);
	}

	public static void EnableGlowFlowerMeter(Instance smi)
	{
		smi.ActivateGlowFlowerMeter();
	}

	public static void DisableGlowFlowerMeter(Instance smi)
	{
		smi.DeactivateGlowFlowerMeter();
	}

	public static void UpdateFlowerOnWilt(Instance smi)
	{
		smi.PlayAnimOnFlower(smi.Animations.meterAnim_flowerWilted, KAnim.PlayMode.Loop);
	}

	public static void UpdateFlowerOnWiltRecover(Instance smi)
	{
		smi.PlayAnimOnFlower(smi.Animations.meterAnimNames, KAnim.PlayMode.Loop);
	}

	public static void EnableEntombDefenses(Instance smi)
	{
		smi.GetSMI<UnstableEntombDefense.Instance>().SetActive(active: true);
	}

	public static void DisableEntombDefenses(Instance smi)
	{
		smi.GetSMI<UnstableEntombDefense.Instance>().SetActive(active: false);
	}

	public static void AllowItToBeHarvestForWood(Instance smi)
	{
		smi.harvestable.SetCanBeHarvested(state: true);
	}

	public static void ForbidBranchToBeHarvestedForWood(Instance smi)
	{
		smi.harvestable.SetCanBeHarvested(state: false);
	}

	public static void SpawnWoodOnHarvest(Instance smi)
	{
		smi.crop.SpawnConfiguredFruit(null);
	}

	public static void SpawnWoodOnDeath(Instance smi)
	{
		if (smi.harvestable != null && smi.harvestable.CanBeHarvested)
		{
			smi.crop.SpawnConfiguredFruit(null);
		}
	}

	public static void SelfDestroy(Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	public static void PlayFillAnimationOnUnentomb(Instance smi)
	{
		if (!smi.IsEntombed)
		{
			PlayFillAnimationForThisState(smi);
		}
	}

	public static void PlayReadyForHarvestAnimationOnUnentomb(Instance smi)
	{
		if (!smi.IsEntombed)
		{
			smi.PlayReadyForHarvestAnimation();
		}
	}
}
