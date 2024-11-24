using System;
using UnityEngine;

// Token: 0x02000994 RID: 2452
public class AutoStorageDropper : GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>
{
	// Token: 0x06002C84 RID: 11396 RVA: 0x001EC12C File Offset: 0x001EA32C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.root.Update(delegate(AutoStorageDropper.Instance smi, float dt)
		{
			smi.UpdateBlockedStatus();
		}, UpdateRate.SIM_200ms, true);
		this.idle.EventTransition(GameHashes.OnStorageChange, this.pre_drop, null).OnSignal(this.checkCanDrop, this.pre_drop, (AutoStorageDropper.Instance smi) => !smi.GetComponent<Storage>().IsEmpty()).ParamTransition<bool>(this.isBlocked, this.blocked, GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.IsTrue);
		this.pre_drop.ScheduleGoTo((AutoStorageDropper.Instance smi) => smi.def.delay, this.dropping);
		this.dropping.Enter(delegate(AutoStorageDropper.Instance smi)
		{
			smi.Drop();
		}).GoTo(this.idle);
		this.blocked.ParamTransition<bool>(this.isBlocked, this.pre_drop, GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.OutputTileBlocked, null);
	}

	// Token: 0x04001DD8 RID: 7640
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State idle;

	// Token: 0x04001DD9 RID: 7641
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State pre_drop;

	// Token: 0x04001DDA RID: 7642
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State dropping;

	// Token: 0x04001DDB RID: 7643
	private GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.State blocked;

	// Token: 0x04001DDC RID: 7644
	private StateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.BoolParameter isBlocked;

	// Token: 0x04001DDD RID: 7645
	public StateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.Signal checkCanDrop;

	// Token: 0x02000995 RID: 2453
	public class DropperFxConfig
	{
		// Token: 0x04001DDE RID: 7646
		public string animFile;

		// Token: 0x04001DDF RID: 7647
		public string animName;

		// Token: 0x04001DE0 RID: 7648
		public Grid.SceneLayer layer = Grid.SceneLayer.FXFront;

		// Token: 0x04001DE1 RID: 7649
		public bool useElementTint = true;

		// Token: 0x04001DE2 RID: 7650
		public bool flipX;

		// Token: 0x04001DE3 RID: 7651
		public bool flipY;
	}

	// Token: 0x02000996 RID: 2454
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001DE4 RID: 7652
		public CellOffset dropOffset;

		// Token: 0x04001DE5 RID: 7653
		public bool asOre;

		// Token: 0x04001DE6 RID: 7654
		public SimHashes[] elementFilter;

		// Token: 0x04001DE7 RID: 7655
		public bool invertElementFilterInitialValue;

		// Token: 0x04001DE8 RID: 7656
		public bool blockedBySubstantialLiquid;

		// Token: 0x04001DE9 RID: 7657
		public AutoStorageDropper.DropperFxConfig neutralFx;

		// Token: 0x04001DEA RID: 7658
		public AutoStorageDropper.DropperFxConfig leftFx;

		// Token: 0x04001DEB RID: 7659
		public AutoStorageDropper.DropperFxConfig rightFx;

		// Token: 0x04001DEC RID: 7660
		public AutoStorageDropper.DropperFxConfig upFx;

		// Token: 0x04001DED RID: 7661
		public AutoStorageDropper.DropperFxConfig downFx;

		// Token: 0x04001DEE RID: 7662
		public Vector3 fxOffset = Vector3.zero;

		// Token: 0x04001DEF RID: 7663
		public float cooldown = 2f;

		// Token: 0x04001DF0 RID: 7664
		public float delay;
	}

	// Token: 0x02000997 RID: 2455
	public new class Instance : GameStateMachine<AutoStorageDropper, AutoStorageDropper.Instance, IStateMachineTarget, AutoStorageDropper.Def>.GameInstance
	{
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06002C88 RID: 11400 RVA: 0x000BCD27 File Offset: 0x000BAF27
		// (set) Token: 0x06002C89 RID: 11401 RVA: 0x000BCD2F File Offset: 0x000BAF2F
		public bool isInvertElementFilter { get; private set; }

		// Token: 0x06002C8A RID: 11402 RVA: 0x000BCD38 File Offset: 0x000BAF38
		public Instance(IStateMachineTarget master, AutoStorageDropper.Def def) : base(master, def)
		{
			this.isInvertElementFilter = def.invertElementFilterInitialValue;
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x000BCD4E File Offset: 0x000BAF4E
		public void SetInvertElementFilter(bool value)
		{
			base.smi.isInvertElementFilter = value;
			base.smi.sm.checkCanDrop.Trigger(base.smi);
		}

		// Token: 0x06002C8C RID: 11404 RVA: 0x001EC268 File Offset: 0x001EA468
		public void UpdateBlockedStatus()
		{
			int cell = Grid.PosToCell(base.smi.GetDropPosition());
			bool value = Grid.IsSolidCell(cell) || (base.def.blockedBySubstantialLiquid && Grid.IsSubstantialLiquid(cell, 0.35f));
			base.sm.isBlocked.Set(value, base.smi, false);
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x001EC2C8 File Offset: 0x001EA4C8
		private bool IsFilteredElement(SimHashes element)
		{
			for (int num = 0; num != base.def.elementFilter.Length; num++)
			{
				if (base.def.elementFilter[num] == element)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x001EC300 File Offset: 0x001EA500
		private bool AllowedToDrop(SimHashes element)
		{
			return base.def.elementFilter == null || base.def.elementFilter.Length == 0 || (!this.isInvertElementFilter && this.IsFilteredElement(element)) || (this.isInvertElementFilter && !this.IsFilteredElement(element));
		}

		// Token: 0x06002C8F RID: 11407 RVA: 0x001EC350 File Offset: 0x001EA550
		public void Drop()
		{
			bool flag = false;
			Element element = null;
			for (int i = this.m_storage.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = this.m_storage.items[i];
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (this.AllowedToDrop(component.ElementID))
				{
					if (base.def.asOre)
					{
						this.m_storage.Drop(gameObject, true);
						gameObject.transform.SetPosition(this.GetDropPosition());
						element = component.Element;
						flag = true;
					}
					else
					{
						Dumpable component2 = gameObject.GetComponent<Dumpable>();
						if (!component2.IsNullOrDestroyed())
						{
							component2.Dump(this.GetDropPosition());
							element = component.Element;
							flag = true;
						}
					}
				}
			}
			AutoStorageDropper.DropperFxConfig dropperAnim = this.GetDropperAnim();
			if (flag && dropperAnim != null && GameClock.Instance.GetTime() > this.m_timeSinceLastDrop + base.def.cooldown)
			{
				this.m_timeSinceLastDrop = GameClock.Instance.GetTime();
				Vector3 vector = Grid.CellToPosCCC(Grid.PosToCell(this.GetDropPosition()), dropperAnim.layer);
				vector += ((this.m_rotatable != null) ? this.m_rotatable.GetRotatedOffset(base.def.fxOffset) : base.def.fxOffset);
				KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect(dropperAnim.animFile, vector, null, false, dropperAnim.layer, false);
				kbatchedAnimController.destroyOnAnimComplete = false;
				kbatchedAnimController.FlipX = dropperAnim.flipX;
				kbatchedAnimController.FlipY = dropperAnim.flipY;
				if (dropperAnim.useElementTint)
				{
					kbatchedAnimController.TintColour = element.substance.colour;
				}
				kbatchedAnimController.Play(dropperAnim.animName, KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06002C90 RID: 11408 RVA: 0x001EC518 File Offset: 0x001EA718
		public AutoStorageDropper.DropperFxConfig GetDropperAnim()
		{
			CellOffset cellOffset = (this.m_rotatable != null) ? this.m_rotatable.GetRotatedCellOffset(base.def.dropOffset) : base.def.dropOffset;
			if (cellOffset.x < 0)
			{
				return base.def.leftFx;
			}
			if (cellOffset.x > 0)
			{
				return base.def.rightFx;
			}
			if (cellOffset.y < 0)
			{
				return base.def.downFx;
			}
			if (cellOffset.y > 0)
			{
				return base.def.upFx;
			}
			return base.def.neutralFx;
		}

		// Token: 0x06002C91 RID: 11409 RVA: 0x001EC5B8 File Offset: 0x001EA7B8
		public Vector3 GetDropPosition()
		{
			if (!(this.m_rotatable != null))
			{
				return base.transform.GetPosition() + base.def.dropOffset.ToVector3();
			}
			return base.transform.GetPosition() + this.m_rotatable.GetRotatedCellOffset(base.def.dropOffset).ToVector3();
		}

		// Token: 0x04001DF1 RID: 7665
		[MyCmpGet]
		private Storage m_storage;

		// Token: 0x04001DF2 RID: 7666
		[MyCmpGet]
		private Rotatable m_rotatable;

		// Token: 0x04001DF4 RID: 7668
		private float m_timeSinceLastDrop;
	}
}
