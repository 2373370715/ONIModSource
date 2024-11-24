using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200073F RID: 1855
public class StressShockChore : Chore<StressShockChore.StatesInstance>
{
	// Token: 0x06002109 RID: 8457 RVA: 0x001BE424 File Offset: 0x001BC624
	private static bool CheckBlocked(int sourceCell, int destinationCell)
	{
		HashSet<int> hashSet = new HashSet<int>();
		Grid.CollectCellsInLine(sourceCell, destinationCell, hashSet);
		bool result = false;
		foreach (int i in hashSet)
		{
			if (Grid.Solid[i])
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600210A RID: 8458 RVA: 0x000B5A1C File Offset: 0x000B3C1C
	public static void AddBatteryDrainModifier(StressShockChore.StatesInstance smi)
	{
		smi.SetDrainModifierActiveState(true);
	}

	// Token: 0x0600210B RID: 8459 RVA: 0x000B5A25 File Offset: 0x000B3C25
	public static void RemoveBatteryDrainModifier(StressShockChore.StatesInstance smi)
	{
		smi.SetDrainModifierActiveState(false);
	}

	// Token: 0x0600210C RID: 8460 RVA: 0x001BE490 File Offset: 0x001BC690
	public StressShockChore(ChoreType chore_type, IStateMachineTarget target, Notification notification, Action<Chore> on_complete = null) : base(Db.Get().ChoreTypes.StressShock, target, target.GetComponent<ChoreProvider>(), false, on_complete, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StressShockChore.StatesInstance(this, target.gameObject, notification);
	}

	// Token: 0x02000740 RID: 1856
	public class StatesInstance : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.GameInstance
	{
		// Token: 0x0600210D RID: 8461 RVA: 0x001BE4DC File Offset: 0x001BC6DC
		public StatesInstance(StressShockChore master, GameObject shocker, Notification notification) : base(master)
		{
			base.sm.shocker.Set(shocker, base.smi, false);
			this.notification = notification;
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x000B5A2E File Offset: 0x000B3C2E
		public void SetDrainModifierActiveState(bool draining)
		{
			if (draining)
			{
				this.batteryMonitor.AddOrUpdateModifier(this.powerDrainModifier, true);
				return;
			}
			this.batteryMonitor.RemoveModifier(this.powerDrainModifier.id, true);
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x001BE554 File Offset: 0x001BC754
		public void FindDestination()
		{
			int value = this.FindIdleCell();
			base.sm.targetMoveLocation.Set(value, base.smi, false);
			this.GoTo(base.sm.shocking.runAroundShockingStuff);
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x001BE598 File Offset: 0x001BC798
		private int FindIdleCell()
		{
			Navigator component = base.smi.master.GetComponent<Navigator>();
			MinionPathFinderAbilities minionPathFinderAbilities = (MinionPathFinderAbilities)component.GetCurrentAbilities();
			minionPathFinderAbilities.SetIdleNavMaskEnabled(true);
			IdleCellQuery idleCellQuery = PathFinderQueries.idleCellQuery.Reset(base.GetComponent<MinionBrain>(), UnityEngine.Random.Range(90, 180));
			component.RunQuery(idleCellQuery);
			minionPathFinderAbilities.SetIdleNavMaskEnabled(false);
			return idleCellQuery.GetResultCell();
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x001BE5F8 File Offset: 0x001BC7F8
		public void PickShockTarget(StressShockChore.StatesInstance smi)
		{
			int num = Grid.PosToCell(smi.master.gameObject);
			int worldId = (int)Grid.WorldIdx[num];
			List<GameObject> list = new List<GameObject>();
			float num2 = UnityEngine.Random.Range(0f, 2f);
			foreach (Health health in Components.Health.GetWorldItems(worldId, false))
			{
				if (!health.IsNullOrDestroyed() && !(health.gameObject == smi.master.gameObject))
				{
					int num3 = Grid.PosToCell(health);
					float num4 = Vector2.Distance(Grid.CellToPos2D(num), Grid.CellToPos2D(num3));
					if (num4 <= (float)STRESS.SHOCKER.SHOCK_RADIUS && num4 > num2 && !StressShockChore.CheckBlocked(num, num3))
					{
						list.Add(health.gameObject);
					}
				}
			}
			Vector2I vector2I = Grid.CellToXY(num);
			List<ScenePartitionerEntry> list2 = new List<ScenePartitionerEntry>();
			GameScenePartitioner.Instance.GatherEntries(vector2I.x - STRESS.SHOCKER.SHOCK_RADIUS, vector2I.y - STRESS.SHOCKER.SHOCK_RADIUS, STRESS.SHOCKER.SHOCK_RADIUS * 2, STRESS.SHOCKER.SHOCK_RADIUS * 2, GameScenePartitioner.Instance.objectLayers[42], list2);
			foreach (ScenePartitionerEntry scenePartitionerEntry in list2)
			{
				if (!StressShockChore.CheckBlocked(num, Grid.PosToCell(new Vector2((float)scenePartitionerEntry.x, (float)scenePartitionerEntry.y))) && scenePartitionerEntry.obj as GameObject != null)
				{
					list.Add(scenePartitionerEntry.obj as GameObject);
				}
			}
			if (list.Count == 0)
			{
				Vector2I vector2I2 = Grid.CellToXY(num);
				List<ScenePartitionerEntry> list3 = new List<ScenePartitionerEntry>();
				GameScenePartitioner.Instance.GatherEntries(vector2I2.x - STRESS.SHOCKER.SHOCK_RADIUS, vector2I2.y - STRESS.SHOCKER.SHOCK_RADIUS, STRESS.SHOCKER.SHOCK_RADIUS * 2, STRESS.SHOCKER.SHOCK_RADIUS * 2, GameScenePartitioner.Instance.completeBuildings, list3);
				foreach (ScenePartitionerEntry scenePartitionerEntry2 in list3)
				{
					if (!StressShockChore.CheckBlocked(num, Grid.PosToCell(new Vector2((float)scenePartitionerEntry2.x, (float)scenePartitionerEntry2.y))))
					{
						BuildingComplete buildingComplete = scenePartitionerEntry2.obj as BuildingComplete;
						if (buildingComplete != null)
						{
							list.Add(buildingComplete.gameObject);
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			GameObject random = list.GetRandom<GameObject>();
			GameObject gameObject = random;
			float num5 = float.MaxValue;
			foreach (GameObject gameObject2 in list)
			{
				if (list.Count <= 1 || !(gameObject2 == base.sm.previousTarget.Get(smi)))
				{
					float num6 = Vector2.Distance(base.transform.position, gameObject2.transform.position);
					if (num6 < num5)
					{
						num5 = num6;
						gameObject = gameObject2;
					}
				}
			}
			if (random != null && gameObject != null && UnityEngine.Random.Range(0, 100) > 50)
			{
				base.sm.beamTarget.Set(gameObject, smi, false);
				return;
			}
			base.sm.beamTarget.Set(gameObject, smi, false);
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x001BE990 File Offset: 0x001BCB90
		public void MakeBeam()
		{
			GameObject gameObject = new GameObject("shockFX");
			gameObject.SetActive(false);
			KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
			base.sm.beamFX.Set(kbatchedAnimController, base.smi, false);
			kbatchedAnimController.SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("bionic_dupe_stress_beam_fx_kanim")
			});
			gameObject.SetActive(true);
			bool flag;
			Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform("snapTo_hat", out flag).GetColumn(3);
			vector -= Vector3.up / 4f;
			vector.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront);
			gameObject.transform.position = vector;
			gameObject.transform.parent = base.transform;
			kbatchedAnimController.Play("lightning_beam_comp", KAnim.PlayMode.Loop, 1f, 0f);
			GameObject gameObject2 = new GameObject("impactFX");
			gameObject2.SetActive(false);
			KBatchedAnimController kbatchedAnimController2 = gameObject2.AddComponent<KBatchedAnimController>();
			base.sm.impactFX.Set(kbatchedAnimController2, base.smi, false);
			kbatchedAnimController2.SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("bionic_dupe_stress_beam_impact_fx_kanim")
			});
			gameObject2.SetActive(true);
			kbatchedAnimController2.Play("stress_beam_impact_fx", KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x001BEAF4 File Offset: 0x001BCCF4
		public void ClearBeam(int beamIdx)
		{
			base.sm.previousTarget.Set(base.sm.beamTarget.Get(base.smi), base.smi, false);
			base.sm.beamTarget.Set(null, base.smi, false);
			if (base.sm.beamFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.beamFX.Get(base.smi).gameObject);
				base.sm.beamFX.Set(null, base.smi, false);
			}
			if (base.sm.impactFX.Get(base.smi) != null)
			{
				Util.KDestroyGameObject(base.sm.impactFX.Get(base.smi).gameObject);
				base.sm.impactFX.Set(null, base.smi, false);
			}
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x001BEBF8 File Offset: 0x001BCDF8
		public void AimBeam(Vector3 targetPosition, int beamIdx)
		{
			Vector3 v = Vector3.Normalize(targetPosition - base.smi.sm.beamFX.Get(base.smi).transform.position);
			float rotation = MathUtil.AngleSigned(Vector3.up, v, Vector3.forward) + 90f;
			base.smi.sm.beamFX.Get(base.smi).Rotation = rotation;
			float animWidth = Vector2.Distance(base.smi.sm.beamTarget.Get(base.smi).transform.position, base.smi.sm.beamFX.Get(base.smi).transform.position) / 2.5f;
			base.smi.sm.beamFX.Get(base.smi).animWidth = animWidth;
			base.smi.sm.impactFX.Get(base.smi).transform.position = targetPosition;
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x001BED18 File Offset: 0x001BCF18
		public void ShowBeam(bool show)
		{
			base.smi.sm.impactFX.Get(base.smi).enabled = show;
			base.smi.sm.beamFX.Get(base.smi).enabled = show;
		}

		// Token: 0x040015AE RID: 5550
		public Notification notification;

		// Token: 0x040015AF RID: 5551
		[MySmiReq]
		public BionicBatteryMonitor.Instance batteryMonitor;

		// Token: 0x040015B0 RID: 5552
		public BionicBatteryMonitor.WattageModifier powerDrainModifier = new BionicBatteryMonitor.WattageModifier("StressShockChore", string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.STANDARD_ACTIVE_TEMPLATE, DUPLICANTS.TRAITS.STRESSSHOCKER.DRAIN_ATTRIBUTE, "<b>+</b>" + GameUtil.GetFormattedWattage(STRESS.SHOCKER.POWER_CONSUMPTION_RATE, GameUtil.WattageFormatterUnit.Automatic, true)), STRESS.SHOCKER.POWER_CONSUMPTION_RATE, STRESS.SHOCKER.POWER_CONSUMPTION_RATE);
	}

	// Token: 0x02000741 RID: 1857
	public class States : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore>
	{
		// Token: 0x06002116 RID: 8470 RVA: 0x001BED68 File Offset: 0x001BCF68
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.shocking.findDestination;
			base.serializable = StateMachine.SerializeType.Never;
			base.Target(this.shocker);
			this.shocking.DefaultState(this.shocking.findDestination).ToggleAnims("anim_loco_stressshocker_kanim", 0f).Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.MakeBeam();
			}).Exit(delegate(StressShockChore.StatesInstance smi)
			{
				smi.ClearBeam(0);
			});
			this.shocking.findDestination.Enter("FindDestination", delegate(StressShockChore.StatesInstance smi)
			{
				smi.ShowBeam(false);
				smi.FindDestination();
			});
			this.shocking.runAroundShockingStuff.MoveTo((StressShockChore.StatesInstance smi) => smi.sm.targetMoveLocation.Get(smi), this.shocking.findDestination, this.delay, false).ParamTransition<float>(this.powerConsumed, this.complete, (StressShockChore.StatesInstance smi, float p) => p >= STRESS.SHOCKER.MAX_POWER_USE).Toggle("BatteryDrain", new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.AddBatteryDrainModifier), new StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State.Callback(StressShockChore.RemoveBatteryDrainModifier)).Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.ShowBeam(true);
			}).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				smi.PickShockTarget(smi);
				float num = dt * STRESS.SHOCKER.POWER_CONSUMPTION_RATE;
				smi.sm.powerConsumed.Delta(-num, smi);
				smi.batteryMonitor.ConsumePower(-num);
				if (smi.sm.beamTarget.Get(smi) != null)
				{
					Health component = smi.sm.beamTarget.Get(smi).GetComponent<Health>();
					if (component != null)
					{
						component.Damage(dt * STRESS.SHOCKER.DAMAGE_RATE);
						return;
					}
					if (smi.sm.beamTarget.Get(smi).HasTag(GameTags.Wires))
					{
						BuildingHP component2 = smi.sm.beamTarget.Get(smi).GetComponent<BuildingHP>();
						if (component2 != null)
						{
							component2.DoDamage(Mathf.RoundToInt(dt * STRESS.SHOCKER.DAMAGE_RATE));
						}
					}
				}
			}, UpdateRate.SIM_200ms, false).Update(delegate(StressShockChore.StatesInstance smi, float dt)
			{
				if (smi.sm.beamTarget.Get(smi) != null)
				{
					Vector3 vector = smi.sm.beamTarget.Get(smi).transform.position + Vector3.up / 2f;
					if (!StressShockChore.CheckBlocked(Grid.PosToCell(smi.sm.beamFX.Get(smi).transform.position), Grid.PosToCell(vector)))
					{
						smi.AimBeam(vector, 0);
					}
				}
			}, UpdateRate.RENDER_EVERY_TICK, false);
			this.delay.ScheduleGoTo(0.5f, this.shocking);
			this.complete.Enter(delegate(StressShockChore.StatesInstance smi)
			{
				smi.StopSM("complete");
			});
		}

		// Token: 0x040015B1 RID: 5553
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.TargetParameter shocker;

		// Token: 0x040015B2 RID: 5554
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController[]> cosmeticBeamFXs;

		// Token: 0x040015B3 RID: 5555
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> beamFX;

		// Token: 0x040015B4 RID: 5556
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<KBatchedAnimController> impactFX;

		// Token: 0x040015B5 RID: 5557
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<GameObject> beamTarget;

		// Token: 0x040015B6 RID: 5558
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.ObjectParameter<GameObject> previousTarget;

		// Token: 0x040015B7 RID: 5559
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.IntParameter targetMoveLocation;

		// Token: 0x040015B8 RID: 5560
		public StateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.FloatParameter powerConsumed;

		// Token: 0x040015B9 RID: 5561
		public StressShockChore.States.ShockStates shocking;

		// Token: 0x040015BA RID: 5562
		public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State delay;

		// Token: 0x040015BB RID: 5563
		public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State complete;

		// Token: 0x02000742 RID: 1858
		public class ShockStates : GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State
		{
			// Token: 0x040015BC RID: 5564
			public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State findDestination;

			// Token: 0x040015BD RID: 5565
			public GameStateMachine<StressShockChore.States, StressShockChore.StatesInstance, StressShockChore, object>.State runAroundShockingStuff;
		}
	}
}
