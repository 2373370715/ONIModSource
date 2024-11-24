using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020007C3 RID: 1987
public class StickerBomber : GameStateMachine<StickerBomber, StickerBomber.Instance>
{
	// Token: 0x0600239C RID: 9116 RVA: 0x001C5BDC File Offset: 0x001C3DDC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false).Exit(delegate(StickerBomber.Instance smi)
		{
			smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB;
		});
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ToggleStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_StickerBombing, null);
		this.overjoyed.idle.Transition(this.overjoyed.place_stickers, (StickerBomber.Instance smi) => GameClock.Instance.GetTime() >= smi.nextStickerBomb, UpdateRate.SIM_200ms);
		this.overjoyed.place_stickers.Exit(delegate(StickerBomber.Instance smi)
		{
			smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB;
		}).ToggleReactable((StickerBomber.Instance smi) => smi.CreateReactable()).OnSignal(this.doneStickerBomb, this.overjoyed.idle);
	}

	// Token: 0x0400178C RID: 6028
	public StateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.Signal doneStickerBomb;

	// Token: 0x0400178D RID: 6029
	public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x0400178E RID: 6030
	public StickerBomber.OverjoyedStates overjoyed;

	// Token: 0x020007C4 RID: 1988
	public class OverjoyedStates : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x0400178F RID: 6031
		public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04001790 RID: 6032
		public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State place_stickers;
	}

	// Token: 0x020007C5 RID: 1989
	public new class Instance : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600239F RID: 9119 RVA: 0x000B7313 File Offset: 0x000B5513
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x000B731C File Offset: 0x000B551C
		public Reactable CreateReactable()
		{
			return new StickerBomber.Instance.StickerBombReactable(base.master.gameObject, base.smi);
		}

		// Token: 0x04001791 RID: 6033
		[Serialize]
		public float nextStickerBomb;

		// Token: 0x020007C6 RID: 1990
		private class StickerBombReactable : Reactable
		{
			// Token: 0x060023A1 RID: 9121 RVA: 0x001C5D24 File Offset: 0x001C3F24
			public StickerBombReactable(GameObject gameObject, StickerBomber.Instance stickerBomber) : base(gameObject, "StickerBombReactable", Db.Get().ChoreTypes.Build, 2, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
			{
				this.preventChoreInterruption = true;
				this.stickerBomber = stickerBomber;
			}

			// Token: 0x060023A2 RID: 9122 RVA: 0x001C5E04 File Offset: 0x001C4004
			public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
			{
				if (this.reactor != null)
				{
					return false;
				}
				if (new_reactor == null)
				{
					return false;
				}
				if (this.gameObject != new_reactor)
				{
					return false;
				}
				Navigator component = new_reactor.GetComponent<Navigator>();
				return !(component == null) && component.CurrentNavType != NavType.Tube && component.CurrentNavType != NavType.Ladder && component.CurrentNavType != NavType.Pole;
			}

			// Token: 0x060023A3 RID: 9123 RVA: 0x001C5E6C File Offset: 0x001C406C
			protected override void InternalBegin()
			{
				this.stickersToPlace = UnityEngine.Random.Range(4, 6);
				this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
				this.placementCell = this.FindPlacementCell();
				if (this.placementCell == 0)
				{
					base.End();
					return;
				}
				this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
				this.kbac.AddAnimOverrides(this.animset, 0f);
				this.kbac.Play(this.pre_anim, KAnim.PlayMode.Once, 1f, 0f);
				this.kbac.Queue(this.loop_anim, KAnim.PlayMode.Loop, 1f, 0f);
			}

			// Token: 0x060023A4 RID: 9124 RVA: 0x001C5F0C File Offset: 0x001C410C
			public override void Update(float dt)
			{
				this.STICKER_PLACE_TIMER -= dt;
				if (this.STICKER_PLACE_TIMER <= 0f)
				{
					this.PlaceSticker();
					this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
				}
				if (this.stickersPlaced >= this.stickersToPlace)
				{
					this.kbac.Play(this.pst_anim, KAnim.PlayMode.Once, 1f, 0f);
					base.End();
				}
			}

			// Token: 0x060023A5 RID: 9125 RVA: 0x001C5F78 File Offset: 0x001C4178
			protected override void InternalEnd()
			{
				if (this.kbac != null)
				{
					this.kbac.RemoveAnimOverrides(this.animset);
					this.kbac = null;
				}
				this.stickerBomber.sm.doneStickerBomb.Trigger(this.stickerBomber);
				this.stickersPlaced = 0;
			}

			// Token: 0x060023A6 RID: 9126 RVA: 0x001C5FD0 File Offset: 0x001C41D0
			private int FindPlacementCell()
			{
				int cell = Grid.PosToCell(this.reactor.transform.GetPosition() + Vector3.up);
				ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
				ListPool<int, PathFinder>.PooledList pooledList2 = ListPool<int, PathFinder>.Allocate();
				QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
				pooledQueue.Enqueue(new GameUtil.FloodFillInfo
				{
					cell = cell,
					depth = 0
				});
				GameUtil.FloodFillConditional(pooledQueue, this.canPlaceStickerCb, pooledList, pooledList2, 2);
				if (pooledList2.Count > 0)
				{
					int random = pooledList2.GetRandom<int>();
					pooledList.Recycle();
					pooledList2.Recycle();
					pooledQueue.Recycle();
					return random;
				}
				pooledList.Recycle();
				pooledList2.Recycle();
				pooledQueue.Recycle();
				return 0;
			}

			// Token: 0x060023A7 RID: 9127 RVA: 0x001C6074 File Offset: 0x001C4274
			private void PlaceSticker()
			{
				this.stickersPlaced++;
				Vector3 a = Grid.CellToPos(this.placementCell);
				int i = 10;
				while (i > 0)
				{
					i--;
					Vector3 position = a + new Vector3(UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), -2.5f);
					if (StickerBomb.CanPlaceSticker(StickerBomb.BuildCellOffsets(position)))
					{
						GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("StickerBomb".ToTag()), position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-this.tile_random_rotation, this.tile_random_rotation)), null, null, true, 0);
						StickerBomb component = gameObject.GetComponent<StickerBomb>();
						string stickerType = this.reactor.GetComponent<MinionIdentity>().stickerType;
						component.SetStickerType(stickerType);
						gameObject.SetActive(true);
						i = 0;
					}
				}
			}

			// Token: 0x060023A8 RID: 9128 RVA: 0x000A5E40 File Offset: 0x000A4040
			protected override void InternalCleanup()
			{
			}

			// Token: 0x04001792 RID: 6034
			private int stickersToPlace;

			// Token: 0x04001793 RID: 6035
			private int stickersPlaced;

			// Token: 0x04001794 RID: 6036
			private int placementCell;

			// Token: 0x04001795 RID: 6037
			private float tile_random_range = 1f;

			// Token: 0x04001796 RID: 6038
			private float tile_random_rotation = 90f;

			// Token: 0x04001797 RID: 6039
			private float TIME_PER_STICKER_PLACED = 0.66f;

			// Token: 0x04001798 RID: 6040
			private float STICKER_PLACE_TIMER;

			// Token: 0x04001799 RID: 6041
			private KBatchedAnimController kbac;

			// Token: 0x0400179A RID: 6042
			private KAnimFile animset = Assets.GetAnim("anim_stickers_kanim");

			// Token: 0x0400179B RID: 6043
			private HashedString pre_anim = "working_pre";

			// Token: 0x0400179C RID: 6044
			private HashedString loop_anim = "working_loop";

			// Token: 0x0400179D RID: 6045
			private HashedString pst_anim = "working_pst";

			// Token: 0x0400179E RID: 6046
			private StickerBomber.Instance stickerBomber;

			// Token: 0x0400179F RID: 6047
			private Func<int, bool> canPlaceStickerCb = (int cell) => !Grid.Solid[cell] && (!Grid.IsValidCell(Grid.CellLeft(cell)) || !Grid.Solid[Grid.CellLeft(cell)]) && (!Grid.IsValidCell(Grid.CellRight(cell)) || !Grid.Solid[Grid.CellRight(cell)]) && (!Grid.IsValidCell(Grid.OffsetCell(cell, 0, 1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, 1)]) && (!Grid.IsValidCell(Grid.OffsetCell(cell, 0, -1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, -1)]) && !Grid.IsCellOpenToSpace(cell);
		}
	}
}
