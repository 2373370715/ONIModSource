using System;
using STRINGS;

// Token: 0x0200018F RID: 399
public class FixedCaptureStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>
{
	// Token: 0x060005A0 RID: 1440 RVA: 0x00159BA8 File Offset: 0x00157DA8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.capture;
		this.root.Exit("AbandonedCapturePoint", delegate(FixedCaptureStates.Instance smi)
		{
			smi.AbandonedCapturePoint();
		});
		this.capture.EventTransition(GameHashes.CapturePointNoLongerAvailable, null, null).DefaultState(this.capture.cheer);
		GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State state = this.capture.cheer.DefaultState(this.capture.cheer.pre);
		string name = CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.NAME;
		string tooltip = CREATURES.STATUSITEMS.EXCITED_TO_BE_RANCHED.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.capture.cheer.pre.ScheduleGoTo(0.9f, this.capture.cheer.cheer);
		this.capture.cheer.cheer.Enter("FaceRancher", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetComponent<Facing>().Face(smi.GetCapturePoint().transform.GetPosition());
		}).PlayAnim("excited_loop").OnAnimQueueComplete(this.capture.cheer.pst);
		this.capture.cheer.pst.ScheduleGoTo(0.2f, this.capture.move);
		GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State state2 = this.capture.move.DefaultState(this.capture.move.movetoranch);
		string name2 = CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
		string tooltip2 = CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
		string icon2 = "";
		StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
		NotificationType notification_type2 = NotificationType.Neutral;
		bool allow_multiples2 = false;
		main = Db.Get().StatusItemCategories.Main;
		state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, null, null, main);
		this.capture.move.movetoranch.Enter("Speedup", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed * 1.25f;
		}).MoveTo(new Func<FixedCaptureStates.Instance, int>(FixedCaptureStates.GetTargetCaptureCell), this.capture.move.waitforranchertobeready, null, false).Exit("RestoreSpeed", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetComponent<Navigator>().defaultSpeed = smi.originalSpeed;
		});
		this.capture.move.waitforranchertobeready.Enter("SetCreatureAtRanchingStation", delegate(FixedCaptureStates.Instance smi)
		{
			smi.GetCapturePoint().Trigger(-1992722293, null);
		}).EventTransition(GameHashes.RancherReadyAtCapturePoint, this.capture.ranching, null);
		GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State ranching = this.capture.ranching;
		string name3 = CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME;
		string tooltip3 = CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP;
		string icon3 = "";
		StatusItem.IconType icon_type3 = StatusItem.IconType.Info;
		NotificationType notification_type3 = NotificationType.Neutral;
		bool allow_multiples3 = false;
		main = Db.Get().StatusItemCategories.Main;
		ranching.ToggleStatusItem(name3, tooltip3, icon3, icon_type3, notification_type3, allow_multiples3, default(HashedString), 129022, null, null, main);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToGetCaptured, false);
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x000A85F4 File Offset: 0x000A67F4
	private static FixedCapturePoint.Instance GetCapturePoint(FixedCaptureStates.Instance smi)
	{
		return smi.GetSMI<FixedCapturableMonitor.Instance>().targetCapturePoint;
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x00159EB8 File Offset: 0x001580B8
	private static int GetTargetCaptureCell(FixedCaptureStates.Instance smi)
	{
		FixedCapturePoint.Instance capturePoint = FixedCaptureStates.GetCapturePoint(smi);
		return capturePoint.def.getTargetCapturePoint(capturePoint);
	}

	// Token: 0x04000417 RID: 1047
	private FixedCaptureStates.CaptureStates capture;

	// Token: 0x04000418 RID: 1048
	private GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State behaviourcomplete;

	// Token: 0x02000190 RID: 400
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000191 RID: 401
	public new class Instance : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.GameInstance
	{
		// Token: 0x060005A5 RID: 1445 RVA: 0x000A8609 File Offset: 0x000A6809
		public Instance(Chore<FixedCaptureStates.Instance> chore, FixedCaptureStates.Def def) : base(chore, def)
		{
			this.originalSpeed = base.GetComponent<Navigator>().defaultSpeed;
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToGetCaptured);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00159EE0 File Offset: 0x001580E0
		public FixedCapturePoint.Instance GetCapturePoint()
		{
			FixedCapturableMonitor.Instance smi = this.GetSMI<FixedCapturableMonitor.Instance>();
			if (smi == null)
			{
				return null;
			}
			return smi.targetCapturePoint;
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x000A863E File Offset: 0x000A683E
		public void AbandonedCapturePoint()
		{
			if (this.GetCapturePoint() != null)
			{
				this.GetCapturePoint().Trigger(-1000356449, null);
			}
		}

		// Token: 0x04000419 RID: 1049
		public float originalSpeed;
	}

	// Token: 0x02000192 RID: 402
	public class CaptureStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
	{
		// Token: 0x0400041A RID: 1050
		public FixedCaptureStates.CaptureStates.CheerStates cheer;

		// Token: 0x0400041B RID: 1051
		public FixedCaptureStates.CaptureStates.MoveStates move;

		// Token: 0x0400041C RID: 1052
		public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State ranching;

		// Token: 0x02000193 RID: 403
		public class CheerStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
		{
			// Token: 0x0400041D RID: 1053
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State pre;

			// Token: 0x0400041E RID: 1054
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State cheer;

			// Token: 0x0400041F RID: 1055
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State pst;
		}

		// Token: 0x02000194 RID: 404
		public class MoveStates : GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State
		{
			// Token: 0x04000420 RID: 1056
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State movetoranch;

			// Token: 0x04000421 RID: 1057
			public GameStateMachine<FixedCaptureStates, FixedCaptureStates.Instance, IStateMachineTarget, FixedCaptureStates.Def>.State waitforranchertobeready;
		}
	}
}
