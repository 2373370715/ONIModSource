using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x02001742 RID: 5954
public class RemoteChore : WorkChore<RemoteWorkTerminal>
{
	// Token: 0x06007A9F RID: 31391 RVA: 0x00319564 File Offset: 0x00317764
	public RemoteChore(RemoteWorkTerminal terminal) : base(Db.Get().ChoreTypes.RemoteOperate, terminal, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true)
	{
		this.terminal = terminal;
		this.AddPrecondition(RemoteChore.RemoteTerminalHasDock, terminal);
		this.AddPrecondition(RemoteChore.RemoteDockHasWorker, terminal);
		this.AddPrecondition(RemoteChore.RemoteDockAvailable, terminal);
		this.AddPrecondition(RemoteChore.RemoteTerminalHasChore, terminal);
		this.AddPrecondition(RemoteChore.RemoteDockOperational, terminal);
	}

	// Token: 0x06007AA0 RID: 31392 RVA: 0x003195DC File Offset: 0x003177DC
	public override void CollectChores(ChoreConsumerState duplicantState, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		Chore.Precondition.Context context = new Chore.Precondition.Context(this, duplicantState, is_attempting_override, null);
		context.RunPreconditions();
		if (!context.IsComplete())
		{
			incomplete_contexts.Add(context);
			return;
		}
		if (context.IsSuccess())
		{
			List<Chore.Precondition.Context> list = new List<Chore.Precondition.Context>();
			List<Chore.Precondition.Context> list2 = new List<Chore.Precondition.Context>();
			List<Chore.Precondition.Context> list3 = new List<Chore.Precondition.Context>();
			RemoteWorkerDock currentDock = this.terminal.CurrentDock;
			if (currentDock != null)
			{
				currentDock.CollectChores(duplicantState, list, list2, list3, is_attempting_override);
			}
			if (list2.Count > 0)
			{
				incomplete_contexts.Add(context);
				return;
			}
			foreach (Chore.Precondition.Context context2 in list)
			{
				succeeded_contexts.Add(new Chore.Precondition.Context(context.chore, context.consumerState, context.isAttemptingOverride, context2));
			}
			using (List<Chore.Precondition.Context>.Enumerator enumerator = list3.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Chore.Precondition.Context context3 = enumerator.Current;
					failed_contexts.Add(new Chore.Precondition.Context(context.chore, context.consumerState, context.isAttemptingOverride, context3));
				}
				return;
			}
		}
		failed_contexts.Add(context);
	}

	// Token: 0x06007AA1 RID: 31393 RVA: 0x000F08AC File Offset: 0x000EEAAC
	public override void Begin(Chore.Precondition.Context context)
	{
		base.Begin(context);
		RemoteWorkerDock currentDock = this.terminal.CurrentDock;
		if (currentDock == null)
		{
			return;
		}
		currentDock.SetNextChore(this.terminal, (Chore.Precondition.Context)context.data);
	}

	// Token: 0x04005C00 RID: 23552
	private static Chore.Precondition RemoteTerminalHasDock = new Chore.Precondition
	{
		id = "RemoteDockAssigned",
		description = DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_NO_REMOTE_DOCK,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((RemoteWorkTerminal)data).CurrentDock != null;
		},
		canExecuteOnAnyThread = true
	};

	// Token: 0x04005C01 RID: 23553
	private static Chore.Precondition RemoteDockOperational = new Chore.Precondition
	{
		id = "RemoteDockOperational",
		description = DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_DOCK_INOPERABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			RemoteWorkTerminal remoteWorkTerminal = (RemoteWorkTerminal)data;
			return remoteWorkTerminal.CurrentDock != null && remoteWorkTerminal.CurrentDock.IsOperational;
		},
		canExecuteOnAnyThread = true
	};

	// Token: 0x04005C02 RID: 23554
	private static Chore.Precondition RemoteDockHasWorker = new Chore.Precondition
	{
		id = "RemoteDockHasAvailableWorker",
		description = DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_NO_REMOTE_WORKER,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			RemoteWorkerDock currentDock = ((RemoteWorkTerminal)data).CurrentDock;
			return !(currentDock == null) && (currentDock.HasWorker() && currentDock.RemoteWorker.Available) && !currentDock.RemoteWorker.RequiresMaintnence;
		},
		canExecuteOnAnyThread = true
	};

	// Token: 0x04005C03 RID: 23555
	private static Chore.Precondition RemoteDockAvailable = new Chore.Precondition
	{
		id = "RemoteDockAvailable",
		description = DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_DOCK_UNAVAILABLE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			RemoteWorkTerminal remoteWorkTerminal = (RemoteWorkTerminal)data;
			RemoteWorkerDock currentDock = remoteWorkTerminal.CurrentDock;
			return !(currentDock == null) && currentDock.AvailableForWorkBy(remoteWorkTerminal);
		},
		canExecuteOnAnyThread = true
	};

	// Token: 0x04005C04 RID: 23556
	private static Chore.Precondition RemoteTerminalHasChore = new Chore.Precondition
	{
		id = "RemoteChorePreconditionsMet",
		description = DUPLICANTS.CHORES.PRECONDITIONS.REMOTE_CHORE_SUBCHORE_PRECONDITIONS,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			RemoteWorkTerminal remoteWorkTerminal = data as RemoteWorkTerminal;
			List<Chore.Precondition.Context> list = new List<Chore.Precondition.Context>();
			List<Chore.Precondition.Context> failed_contexts = new List<Chore.Precondition.Context>();
			if (remoteWorkTerminal != null)
			{
				RemoteWorkerDock currentDock = remoteWorkTerminal.CurrentDock;
				if (currentDock != null)
				{
					currentDock.CollectChores(context.consumerState, list, null, failed_contexts, false);
				}
			}
			return list.Count > 0;
		},
		canExecuteOnAnyThread = false
	};

	// Token: 0x04005C05 RID: 23557
	private RemoteWorkTerminal terminal;
}
