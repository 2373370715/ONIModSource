using System;
using UnityEngine;

// Token: 0x020009F5 RID: 2549
public class CreatureFallMonitor : GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>
{
	// Token: 0x06002EDD RID: 11997 RVA: 0x000BE5F6 File Offset: 0x000BC7F6
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.grounded;
		this.grounded.ToggleBehaviour(GameTags.Creatures.Falling, (CreatureFallMonitor.Instance smi) => smi.ShouldFall(), null);
	}

	// Token: 0x04001F86 RID: 8070
	public static float FLOOR_DISTANCE = -0.065f;

	// Token: 0x04001F87 RID: 8071
	public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State grounded;

	// Token: 0x04001F88 RID: 8072
	public GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.State falling;

	// Token: 0x020009F6 RID: 2550
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04001F89 RID: 8073
		public bool canSwim;

		// Token: 0x04001F8A RID: 8074
		public bool checkHead = true;
	}

	// Token: 0x020009F7 RID: 2551
	public new class Instance : GameStateMachine<CreatureFallMonitor, CreatureFallMonitor.Instance, IStateMachineTarget, CreatureFallMonitor.Def>.GameInstance
	{
		// Token: 0x06002EE1 RID: 12001 RVA: 0x000BE654 File Offset: 0x000BC854
		public Instance(IStateMachineTarget master, CreatureFallMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x001F65D8 File Offset: 0x001F47D8
		public void SnapToGround()
		{
			Vector3 position = base.smi.transform.GetPosition();
			Vector3 position2 = Grid.CellToPosCBC(Grid.PosToCell(position), Grid.SceneLayer.Creatures);
			position2.x = position.x;
			base.smi.transform.SetPosition(position2);
			if (this.navigator.IsValidNavType(NavType.Floor))
			{
				this.navigator.SetCurrentNavType(NavType.Floor);
				return;
			}
			if (this.navigator.IsValidNavType(NavType.Hover))
			{
				this.navigator.SetCurrentNavType(NavType.Hover);
			}
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x001F6658 File Offset: 0x001F4858
		public bool ShouldFall()
		{
			if (this.kprefabId.HasTag(GameTags.Stored))
			{
				return false;
			}
			Vector3 position = base.smi.transform.GetPosition();
			int num = Grid.PosToCell(position);
			if (Grid.IsValidCell(num) && Grid.Solid[num])
			{
				return false;
			}
			if (this.navigator.IsMoving())
			{
				return false;
			}
			if (this.CanSwimAtCurrentLocation())
			{
				return false;
			}
			if (this.navigator.CurrentNavType != NavType.Swim)
			{
				if (this.navigator.NavGrid.NavTable.IsValid(num, this.navigator.CurrentNavType))
				{
					return false;
				}
				if (this.navigator.CurrentNavType == NavType.Ceiling)
				{
					return true;
				}
				if (this.navigator.CurrentNavType == NavType.LeftWall)
				{
					return true;
				}
				if (this.navigator.CurrentNavType == NavType.RightWall)
				{
					return true;
				}
			}
			Vector3 vector = position;
			vector.y += CreatureFallMonitor.FLOOR_DISTANCE;
			int num2 = Grid.PosToCell(vector);
			return !Grid.IsValidCell(num2) || !Grid.Solid[num2];
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x001F6760 File Offset: 0x001F4960
		public bool CanSwimAtCurrentLocation()
		{
			if (base.def.canSwim)
			{
				Vector3 position = base.transform.GetPosition();
				float num = 1f;
				if (!base.def.checkHead)
				{
					num = 0.5f;
				}
				position.y += this.collider.size.y * num;
				if (Grid.IsSubstantialLiquid(Grid.PosToCell(position), 0.35f))
				{
					if (!GameComps.Gravities.Has(base.gameObject))
					{
						return true;
					}
					if (GameComps.Gravities.GetData(GameComps.Gravities.GetHandle(base.gameObject)).velocity.magnitude < 2f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04001F8B RID: 8075
		public string anim = "fall";

		// Token: 0x04001F8C RID: 8076
		[MyCmpReq]
		private KPrefabID kprefabId;

		// Token: 0x04001F8D RID: 8077
		[MyCmpReq]
		private Navigator navigator;

		// Token: 0x04001F8E RID: 8078
		[MyCmpReq]
		private KBoxCollider2D collider;
	}
}
