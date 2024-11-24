using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A52 RID: 2642
public class FallMonitor : GameStateMachine<FallMonitor, FallMonitor.Instance>
{
	// Token: 0x06003096 RID: 12438 RVA: 0x001FCA50 File Offset: 0x001FAC50
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.standing;
		this.root.TagTransition(GameTags.Stored, this.instorage, false).Update("CheckLanded", delegate(FallMonitor.Instance smi, float dt)
		{
			smi.UpdateFalling();
		}, UpdateRate.SIM_33ms, true);
		this.standing.ParamTransition<bool>(this.isEntombed, this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).ParamTransition<bool>(this.isFalling, this.falling_pre, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue);
		this.falling_pre.Enter("StopNavigator", delegate(FallMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		}).Enter("AttemptInitialRecovery", delegate(FallMonitor.Instance smi)
		{
			smi.AttemptInitialRecovery();
		}).GoTo(this.falling).ToggleBrain("falling_pre");
		this.falling.ToggleBrain("falling").PlayAnim("fall_pre").QueueAnim("fall_loop", true, null).ParamTransition<bool>(this.isEntombed, this.entombed, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsTrue).Transition(this.recoverladder, (FallMonitor.Instance smi) => smi.CanRecoverToLadder(), UpdateRate.SIM_33ms).Transition(this.recoverpole, (FallMonitor.Instance smi) => smi.CanRecoverToPole(), UpdateRate.SIM_33ms).ToggleGravity(this.landfloor);
		this.recoverinitialfall.ToggleBrain("recoverinitialfall").Enter("Recover", delegate(FallMonitor.Instance smi)
		{
			smi.Recover();
		}).EventTransition(GameHashes.DestinationReached, this.standing, null).EventTransition(GameHashes.NavigationFailed, this.standing, null).Exit(delegate(FallMonitor.Instance smi)
		{
			smi.RecoverEmote();
		});
		this.landfloor.Enter("Land", delegate(FallMonitor.Instance smi)
		{
			smi.LandFloor();
		}).GoTo(this.standing);
		this.recoverladder.ToggleBrain("recoverladder").PlayAnim("floor_ladder_0_0").Enter("MountLadder", delegate(FallMonitor.Instance smi)
		{
			smi.MountLadder();
		}).OnAnimQueueComplete(this.standing);
		this.recoverpole.ToggleBrain("recoverpole").PlayAnim("floor_pole_0_0").Enter("MountPole", delegate(FallMonitor.Instance smi)
		{
			smi.MountPole();
		}).OnAnimQueueComplete(this.standing);
		this.instorage.TagTransition(GameTags.Stored, this.standing, true);
		this.entombed.DefaultState(this.entombed.recovering);
		this.entombed.recovering.Enter("TryEntombedEscape", delegate(FallMonitor.Instance smi)
		{
			smi.TryEntombedEscape();
		});
		this.entombed.stuck.Enter("StopNavigator", delegate(FallMonitor.Instance smi)
		{
			smi.GetComponent<Navigator>().Stop(false, true);
		}).ToggleChore((FallMonitor.Instance smi) => new EntombedChore(smi.master, smi.entombedAnimOverride), this.standing).ParamTransition<bool>(this.isEntombed, this.standing, GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.IsFalse);
	}

	// Token: 0x040020CD RID: 8397
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State standing;

	// Token: 0x040020CE RID: 8398
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling_pre;

	// Token: 0x040020CF RID: 8399
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State falling;

	// Token: 0x040020D0 RID: 8400
	public FallMonitor.EntombedStates entombed;

	// Token: 0x040020D1 RID: 8401
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverladder;

	// Token: 0x040020D2 RID: 8402
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverpole;

	// Token: 0x040020D3 RID: 8403
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recoverinitialfall;

	// Token: 0x040020D4 RID: 8404
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State landfloor;

	// Token: 0x040020D5 RID: 8405
	public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State instorage;

	// Token: 0x040020D6 RID: 8406
	public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isEntombed;

	// Token: 0x040020D7 RID: 8407
	public StateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.BoolParameter isFalling;

	// Token: 0x02000A53 RID: 2643
	public class EntombedStates : GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040020D8 RID: 8408
		public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State recovering;

		// Token: 0x040020D9 RID: 8409
		public GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.State stuck;
	}

	// Token: 0x02000A54 RID: 2644
	public new class Instance : GameStateMachine<FallMonitor, FallMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003099 RID: 12441 RVA: 0x001FCE1C File Offset: 0x001FB01C
		public Instance(IStateMachineTarget master, bool shouldPlayEmotes, string entombedAnimOverride = null) : base(master)
		{
			this.navigator = base.GetComponent<Navigator>();
			this.shouldPlayEmotes = shouldPlayEmotes;
			this.entombedAnimOverride = entombedAnimOverride;
			Pathfinding.Instance.FlushNavGridsOnLoad();
			base.Subscribe(915392638, new Action<object>(this.OnCellChanged));
			base.Subscribe(1027377649, new Action<object>(this.OnMovementStateChanged));
			base.Subscribe(387220196, new Action<object>(this.OnDestinationReached));
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x001FCF1C File Offset: 0x001FB11C
		private void OnDestinationReached(object data)
		{
			int item = Grid.PosToCell(base.transform.GetPosition());
			if (!this.safeCells.Contains(item))
			{
				this.safeCells.Add(item);
				if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
				{
					this.safeCells.RemoveAt(0);
				}
			}
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x001FCF74 File Offset: 0x001FB174
		private void OnMovementStateChanged(object data)
		{
			if ((GameHashes)data == GameHashes.ObjectMovementWakeUp)
			{
				int item = Grid.PosToCell(base.transform.GetPosition());
				if (!this.safeCells.Contains(item))
				{
					this.safeCells.Add(item);
					if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
					{
						this.safeCells.RemoveAt(0);
					}
				}
			}
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x001FCFD8 File Offset: 0x001FB1D8
		private void OnCellChanged(object data)
		{
			int item = (int)data;
			if (!this.safeCells.Contains(item))
			{
				this.safeCells.Add(item);
				if (this.safeCells.Count > this.MAX_CELLS_TRACKED)
				{
					this.safeCells.RemoveAt(0);
				}
			}
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x001FD028 File Offset: 0x001FB228
		public void Recover()
		{
			int cell = Grid.PosToCell(this.navigator);
			foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
			{
				if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
				{
					int num = transition.IsValid(cell, this.navigator.NavGrid.NavTable);
					if (Grid.InvalidCell != num)
					{
						Vector2I vector2I = Grid.CellToXY(cell);
						Vector2I vector2I2 = Grid.CellToXY(num);
						this.flipRecoverEmote = (vector2I2.x < vector2I.x);
						this.navigator.BeginTransition(transition);
						return;
					}
				}
			}
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x001FD0E0 File Offset: 0x001FB2E0
		public void RecoverEmote()
		{
			if (!this.shouldPlayEmotes)
			{
				return;
			}
			if (UnityEngine.Random.Range(0, 9) == 8)
			{
				new EmoteChore(base.master.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.CloseCall_Fall, KAnim.PlayMode.Once, 1, this.flipRecoverEmote);
			}
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x000BF986 File Offset: 0x000BDB86
		public void LandFloor()
		{
			this.navigator.SetCurrentNavType(NavType.Floor);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x001FD140 File Offset: 0x001FB340
		public void AttemptInitialRecovery()
		{
			if (base.gameObject.HasTag(GameTags.Incapacitated))
			{
				return;
			}
			int cell = Grid.PosToCell(this.navigator);
			foreach (NavGrid.Transition transition in this.navigator.NavGrid.transitions)
			{
				if (transition.isEscape && this.navigator.CurrentNavType == transition.start)
				{
					int num = transition.IsValid(cell, this.navigator.NavGrid.NavTable);
					if (Grid.InvalidCell != num)
					{
						base.smi.GoTo(base.smi.sm.recoverinitialfall);
						return;
					}
				}
			}
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x001FD1F0 File Offset: 0x001FB3F0
		public bool CanRecoverToLadder()
		{
			int cell = Grid.PosToCell(base.master.transform.GetPosition());
			return this.navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !base.gameObject.HasTag(GameTags.Incapacitated);
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x000BF9B7 File Offset: 0x000BDBB7
		public void MountLadder()
		{
			this.navigator.SetCurrentNavType(NavType.Ladder);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x001FD244 File Offset: 0x001FB444
		public bool CanRecoverToPole()
		{
			int cell = Grid.PosToCell(base.master.transform.GetPosition());
			return this.navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole) && !base.gameObject.HasTag(GameTags.Incapacitated);
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x000BF9E8 File Offset: 0x000BDBE8
		public void MountPole()
		{
			this.navigator.SetCurrentNavType(NavType.Pole);
			base.GetComponent<Transform>().SetPosition(Grid.CellToPosCBC(Grid.PosToCell(base.GetComponent<Transform>().GetPosition()), Grid.SceneLayer.Move));
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x001FD298 File Offset: 0x001FB498
		public void UpdateFalling()
		{
			bool value = false;
			bool flag = false;
			if (!this.navigator.IsMoving() && this.navigator.CurrentNavType != NavType.Tube)
			{
				int num = Grid.PosToCell(base.transform.GetPosition());
				int num2 = Grid.CellAbove(num);
				bool flag2 = Grid.IsValidCell(num);
				bool flag3 = Grid.IsValidCell(num2);
				bool flag4 = this.IsValidNavCell(num);
				flag4 = (flag4 && (!base.gameObject.HasTag(GameTags.Incapacitated) || (this.navigator.CurrentNavType != NavType.Ladder && this.navigator.CurrentNavType != NavType.Pole)));
				flag = ((!flag4 && flag2 && Grid.Solid[num] && !Grid.DupePassable[num]) || (flag3 && Grid.Solid[num2] && !Grid.DupePassable[num2]) || (flag2 && Grid.DupeImpassable[num]) || (flag3 && Grid.DupeImpassable[num2]));
				value = (!flag4 && !flag);
				if ((!flag2 && flag3) || (flag3 && Grid.WorldIdx[num] != Grid.WorldIdx[num2] && Grid.IsWorldValidCell(num2)))
				{
					this.TeleportInWorld(num);
				}
			}
			base.sm.isFalling.Set(value, base.smi, false);
			base.sm.isEntombed.Set(flag, base.smi, false);
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x001FD418 File Offset: 0x001FB618
		private void TeleportInWorld(int cell)
		{
			int num = Grid.CellAbove(cell);
			WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[num]);
			if (world != null)
			{
				int safeCell = world.GetSafeCell();
				global::Debug.Log(string.Format("Teleporting {0} to {1}", this.navigator.name, safeCell));
				this.MoveToCell(safeCell, false);
				return;
			}
			global::Debug.LogError(string.Format("Unable to teleport {0} stuck on {1}", this.navigator.name, cell));
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x000BFA19 File Offset: 0x000BDC19
		private bool IsValidNavCell(int cell)
		{
			return this.navigator.NavGrid.NavTable.IsValid(cell, this.navigator.CurrentNavType) && !Grid.DupeImpassable[cell];
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x001FD498 File Offset: 0x001FB698
		public void TryEntombedEscape()
		{
			int num = Grid.PosToCell(base.transform.GetPosition());
			int backCell = base.GetComponent<Facing>().GetBackCell();
			int num2 = Grid.CellAbove(backCell);
			int num3 = Grid.CellBelow(backCell);
			foreach (int num4 in new int[]
			{
				backCell,
				num2,
				num3
			})
			{
				if (this.IsValidNavCell(num4) && !Grid.HasDoor[num4])
				{
					this.MoveToCell(num4, false);
					return;
				}
			}
			int cell = Grid.PosToCell(base.transform.GetPosition());
			foreach (CellOffset offset in this.entombedEscapeOffsets)
			{
				if (Grid.IsCellOffsetValid(cell, offset))
				{
					int num5 = Grid.OffsetCell(cell, offset);
					if (this.IsValidNavCell(num5) && !Grid.HasDoor[num5])
					{
						this.MoveToCell(num5, false);
						return;
					}
				}
			}
			for (int k = this.safeCells.Count - 1; k >= 0; k--)
			{
				int num6 = this.safeCells[k];
				if (num6 != num && this.IsValidNavCell(num6) && !Grid.HasDoor[num6])
				{
					this.MoveToCell(num6, false);
					return;
				}
			}
			foreach (CellOffset offset2 in this.entombedEscapeOffsets)
			{
				if (Grid.IsCellOffsetValid(cell, offset2))
				{
					int num7 = Grid.OffsetCell(cell, offset2);
					int num8 = Grid.CellAbove(num7);
					if (Grid.IsValidCell(num8) && !Grid.Solid[num7] && !Grid.Solid[num8] && !Grid.DupeImpassable[num7] && !Grid.DupeImpassable[num8] && !Grid.HasDoor[num7] && !Grid.HasDoor[num8])
					{
						this.MoveToCell(num7, true);
						return;
					}
				}
			}
			this.GoTo(base.sm.entombed.stuck);
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x001FD6AC File Offset: 0x001FB8AC
		private void MoveToCell(int cell, bool forceFloorNav = false)
		{
			base.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
			base.transform.GetComponent<Navigator>().Stop(false, true);
			if (base.gameObject.HasTag(GameTags.Incapacitated) || forceFloorNav)
			{
				base.transform.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
			}
			this.UpdateFalling();
			if (base.sm.isEntombed.Get(base.smi))
			{
				this.GoTo(base.sm.entombed.stuck);
				return;
			}
			this.GoTo(base.sm.standing);
		}

		// Token: 0x040020DA RID: 8410
		private CellOffset[] entombedEscapeOffsets = new CellOffset[]
		{
			new CellOffset(0, 1),
			new CellOffset(1, 0),
			new CellOffset(-1, 0),
			new CellOffset(1, 1),
			new CellOffset(-1, 1),
			new CellOffset(1, -1),
			new CellOffset(-1, -1)
		};

		// Token: 0x040020DB RID: 8411
		private Navigator navigator;

		// Token: 0x040020DC RID: 8412
		private bool shouldPlayEmotes;

		// Token: 0x040020DD RID: 8413
		public string entombedAnimOverride;

		// Token: 0x040020DE RID: 8414
		private List<int> safeCells = new List<int>();

		// Token: 0x040020DF RID: 8415
		private int MAX_CELLS_TRACKED = 3;

		// Token: 0x040020E0 RID: 8416
		private bool flipRecoverEmote;
	}
}
