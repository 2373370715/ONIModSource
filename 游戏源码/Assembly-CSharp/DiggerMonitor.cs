using System;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x02001160 RID: 4448
public class DiggerMonitor : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>
{
	// Token: 0x06005AD4 RID: 23252 RVA: 0x002957F0 File Offset: 0x002939F0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.EventTransition(GameHashes.BeginMeteorBombardment, (DiggerMonitor.Instance smi) => Game.Instance, this.dig, (DiggerMonitor.Instance smi) => smi.CanTunnel());
		this.dig.ToggleBehaviour(GameTags.Creatures.Tunnel, (DiggerMonitor.Instance smi) => true, null).GoTo(this.loop);
	}

	// Token: 0x04004011 RID: 16401
	public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State loop;

	// Token: 0x04004012 RID: 16402
	public GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.State dig;

	// Token: 0x02001161 RID: 4449
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06005AD6 RID: 23254 RVA: 0x000DB350 File Offset: 0x000D9550
		// (set) Token: 0x06005AD7 RID: 23255 RVA: 0x000DB358 File Offset: 0x000D9558
		public int depthToDig { get; set; }
	}

	// Token: 0x02001162 RID: 4450
	public new class Instance : GameStateMachine<DiggerMonitor, DiggerMonitor.Instance, IStateMachineTarget, DiggerMonitor.Def>.GameInstance
	{
		// Token: 0x06005AD9 RID: 23257 RVA: 0x00295898 File Offset: 0x00293A98
		public Instance(IStateMachineTarget master, DiggerMonitor.Def def) : base(master, def)
		{
			global::World instance = global::World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Combine(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			this.OnDestinationReachedDelegate = new Action<object>(this.OnDestinationReached);
			master.Subscribe(387220196, this.OnDestinationReachedDelegate);
			master.Subscribe(-766531887, this.OnDestinationReachedDelegate);
		}

		// Token: 0x06005ADA RID: 23258 RVA: 0x00295910 File Offset: 0x00293B10
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			global::World instance = global::World.Instance;
			instance.OnSolidChanged = (Action<int>)Delegate.Remove(instance.OnSolidChanged, new Action<int>(this.OnSolidChanged));
			base.master.Unsubscribe(387220196, this.OnDestinationReachedDelegate);
			base.master.Unsubscribe(-766531887, this.OnDestinationReachedDelegate);
		}

		// Token: 0x06005ADB RID: 23259 RVA: 0x000DB361 File Offset: 0x000D9561
		private void OnDestinationReached(object data)
		{
			this.CheckInSolid();
		}

		// Token: 0x06005ADC RID: 23260 RVA: 0x00295978 File Offset: 0x00293B78
		private void CheckInSolid()
		{
			Navigator component = base.gameObject.GetComponent<Navigator>();
			if (component == null)
			{
				return;
			}
			int cell = Grid.PosToCell(base.gameObject);
			if (component.CurrentNavType != NavType.Solid && Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Solid);
				return;
			}
			if (component.CurrentNavType == NavType.Solid && !Grid.IsSolidCell(cell))
			{
				component.SetCurrentNavType(NavType.Floor);
				base.gameObject.AddTag(GameTags.Creatures.Falling);
			}
		}

		// Token: 0x06005ADD RID: 23261 RVA: 0x000DB361 File Offset: 0x000D9561
		private void OnSolidChanged(int cell)
		{
			this.CheckInSolid();
		}

		// Token: 0x06005ADE RID: 23262 RVA: 0x002959EC File Offset: 0x00293BEC
		public bool CanTunnel()
		{
			int num = Grid.PosToCell(this);
			if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(num) == SubWorld.ZoneType.Space)
			{
				int num2 = num;
				while (Grid.IsValidCell(num2) && !Grid.Solid[num2])
				{
					num2 = Grid.CellAbove(num2);
				}
				if (!Grid.IsValidCell(num2))
				{
					return this.FoundValidDigCell();
				}
			}
			return false;
		}

		// Token: 0x06005ADF RID: 23263 RVA: 0x00295A44 File Offset: 0x00293C44
		private bool FoundValidDigCell()
		{
			int num = base.smi.def.depthToDig;
			int num2 = Grid.PosToCell(base.smi.master.gameObject);
			this.lastDigCell = num2;
			int cell = Grid.CellBelow(num2);
			while (this.IsValidDigCell(cell, null) && num > 0)
			{
				cell = Grid.CellBelow(cell);
				num--;
			}
			if (num > 0)
			{
				cell = GameUtil.FloodFillFind<object>(new Func<int, object, bool>(this.IsValidDigCell), null, num2, base.smi.def.depthToDig, false, true);
			}
			this.lastDigCell = cell;
			return this.lastDigCell != -1;
		}

		// Token: 0x06005AE0 RID: 23264 RVA: 0x00295AE0 File Offset: 0x00293CE0
		private bool IsValidDigCell(int cell, object arg = null)
		{
			if (Grid.IsValidCell(cell) && Grid.Solid[cell])
			{
				if (!Grid.HasDoor[cell] && !Grid.Foundation[cell])
				{
					ushort index = Grid.ElementIdx[cell];
					Element element = ElementLoader.elements[(int)index];
					return Grid.Element[cell].hardness < 150 && !element.HasTag(GameTags.RefinedMetal);
				}
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					return Grid.Element[cell].hardness < 150 && !component.Element.HasTag(GameTags.RefinedMetal);
				}
			}
			return false;
		}

		// Token: 0x04004014 RID: 16404
		[Serialize]
		public int lastDigCell = -1;

		// Token: 0x04004015 RID: 16405
		private Action<object> OnDestinationReachedDelegate;
	}
}
