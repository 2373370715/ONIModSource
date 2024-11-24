using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000B9B RID: 2971
public class DevToolChoreDebugger : DevTool
{
	// Token: 0x060038ED RID: 14573 RVA: 0x000C4D9B File Offset: 0x000C2F9B
	protected override void RenderTo(DevPanel panel)
	{
		this.Update();
	}

	// Token: 0x060038EE RID: 14574 RVA: 0x0021CAEC File Offset: 0x0021ACEC
	public void Update()
	{
		if (!Application.isPlaying || SelectTool.Instance == null || SelectTool.Instance.selected == null || SelectTool.Instance.selected.gameObject == null)
		{
			return;
		}
		GameObject gameObject = SelectTool.Instance.selected.gameObject;
		if (this.Consumer == null || (!this.lockSelection && this.selectedGameObject != gameObject))
		{
			this.Consumer = gameObject.GetComponent<ChoreConsumer>();
			this.selectedGameObject = gameObject;
		}
		if (this.Consumer != null)
		{
			ImGui.InputText("Filter:", ref this.filter, 256U);
			this.DisplayAvailableChores();
			ImGui.Text("");
		}
	}

	// Token: 0x060038EF RID: 14575 RVA: 0x0021CBB4 File Offset: 0x0021ADB4
	private void DisplayAvailableChores()
	{
		ImGui.Checkbox("Lock selection", ref this.lockSelection);
		ImGui.Checkbox("Show Last Successful Chore Selection", ref this.showLastSuccessfulPreconditionSnapshot);
		ImGui.Text("Available Chores:");
		ChoreConsumer.PreconditionSnapshot target_snapshot = this.Consumer.GetLastPreconditionSnapshot();
		if (this.showLastSuccessfulPreconditionSnapshot)
		{
			target_snapshot = this.Consumer.GetLastSuccessfulPreconditionSnapshot();
		}
		this.ShowChores(target_snapshot);
	}

	// Token: 0x060038F0 RID: 14576 RVA: 0x0021CC14 File Offset: 0x0021AE14
	private void ShowChores(ChoreConsumer.PreconditionSnapshot target_snapshot)
	{
		ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersOuterH | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.ScrollX | ImGuiTableFlags.ScrollY;
		this.rowIndex = 0;
		if (ImGui.BeginTable("Available Chores", this.columns.Count, flags))
		{
			foreach (object obj in this.columns.Keys)
			{
				ImGui.TableSetupColumn(obj.ToString(), ImGuiTableColumnFlags.WidthFixed);
			}
			ImGui.TableHeadersRow();
			for (int i = target_snapshot.succeededContexts.Count - 1; i >= 0; i--)
			{
				this.ShowContext(target_snapshot.succeededContexts[i]);
			}
			if (target_snapshot.doFailedContextsNeedSorting)
			{
				target_snapshot.failedContexts.Sort();
				target_snapshot.doFailedContextsNeedSorting = false;
			}
			for (int j = target_snapshot.failedContexts.Count - 1; j >= 0; j--)
			{
				this.ShowContext(target_snapshot.failedContexts[j]);
			}
			ImGui.EndTable();
		}
	}

	// Token: 0x060038F1 RID: 14577 RVA: 0x0021CD18 File Offset: 0x0021AF18
	private void ShowContext(Chore.Precondition.Context context)
	{
		string text = "";
		Chore chore = context.chore;
		if (!context.IsSuccess())
		{
			text = context.chore.GetPreconditions()[context.failedPreconditionId].condition.id;
		}
		string text2 = "";
		if (chore.driver != null)
		{
			text2 = chore.driver.name;
		}
		string text3 = "";
		if (chore.overrideTarget != null)
		{
			text3 = chore.overrideTarget.name;
		}
		string text4 = "";
		if (!chore.isNull)
		{
			text4 = chore.gameObject.name;
		}
		if (Chore.Precondition.Context.ShouldFilter(this.filter, chore.GetType().ToString()) && Chore.Precondition.Context.ShouldFilter(this.filter, chore.choreType.Id) && Chore.Precondition.Context.ShouldFilter(this.filter, text) && Chore.Precondition.Context.ShouldFilter(this.filter, text2) && Chore.Precondition.Context.ShouldFilter(this.filter, text3) && Chore.Precondition.Context.ShouldFilter(this.filter, text4))
		{
			return;
		}
		this.columns["Id"] = chore.id.ToString();
		this.columns["Class"] = chore.GetType().ToString().Replace("`1", "");
		this.columns["Type"] = chore.choreType.Id;
		this.columns["PriorityClass"] = context.masterPriority.priority_class.ToString();
		this.columns["PersonalPriority"] = context.personalPriority.ToString();
		this.columns["PriorityValue"] = context.masterPriority.priority_value.ToString();
		this.columns["Priority"] = context.priority.ToString();
		this.columns["PriorityMod"] = context.priorityMod.ToString();
		this.columns["ConsumerPriority"] = context.consumerPriority.ToString();
		this.columns["Cost"] = context.cost.ToString();
		this.columns["Interrupt"] = context.interruptPriority.ToString();
		this.columns["Precondition"] = text;
		this.columns["Override"] = text3;
		this.columns["Assigned To"] = text2;
		this.columns["Owner"] = text4;
		this.columns["Details"] = "";
		ImGui.TableNextRow();
		string format = "ID_row_{0}";
		int num = this.rowIndex;
		this.rowIndex = num + 1;
		ImGui.PushID(string.Format(format, num));
		for (int i = 0; i < this.columns.Count; i++)
		{
			ImGui.TableSetColumnIndex(i);
			ImGui.Text(this.columns[i].ToString());
		}
		ImGui.PopID();
	}

	// Token: 0x060038F2 RID: 14578 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void ConsumerDebugDisplayLog()
	{
	}

	// Token: 0x040026C1 RID: 9921
	private string filter = "";

	// Token: 0x040026C2 RID: 9922
	private bool showLastSuccessfulPreconditionSnapshot;

	// Token: 0x040026C3 RID: 9923
	private bool lockSelection;

	// Token: 0x040026C4 RID: 9924
	private ChoreConsumer Consumer;

	// Token: 0x040026C5 RID: 9925
	private GameObject selectedGameObject;

	// Token: 0x040026C6 RID: 9926
	private OrderedDictionary columns = new OrderedDictionary
	{
		{
			"BP",
			""
		},
		{
			"Id",
			""
		},
		{
			"Class",
			""
		},
		{
			"Type",
			""
		},
		{
			"PriorityClass",
			""
		},
		{
			"PersonalPriority",
			""
		},
		{
			"PriorityValue",
			""
		},
		{
			"Priority",
			""
		},
		{
			"PriorityMod",
			""
		},
		{
			"ConsumerPriority",
			""
		},
		{
			"Cost",
			""
		},
		{
			"Interrupt",
			""
		},
		{
			"Precondition",
			""
		},
		{
			"Override",
			""
		},
		{
			"Assigned To",
			""
		},
		{
			"Owner",
			""
		},
		{
			"Details",
			""
		}
	};

	// Token: 0x040026C7 RID: 9927
	private int rowIndex;

	// Token: 0x02000B9C RID: 2972
	public class EditorPreconditionSnapshot
	{
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060038F4 RID: 14580 RVA: 0x000C4DA3 File Offset: 0x000C2FA3
		// (set) Token: 0x060038F5 RID: 14581 RVA: 0x000C4DAB File Offset: 0x000C2FAB
		public List<DevToolChoreDebugger.EditorPreconditionSnapshot.EditorContext> SucceededContexts { get; set; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060038F6 RID: 14582 RVA: 0x000C4DB4 File Offset: 0x000C2FB4
		// (set) Token: 0x060038F7 RID: 14583 RVA: 0x000C4DBC File Offset: 0x000C2FBC
		public List<DevToolChoreDebugger.EditorPreconditionSnapshot.EditorContext> FailedContexts { get; set; }

		// Token: 0x02000B9D RID: 2973
		public struct EditorContext
		{
			// Token: 0x1700029A RID: 666
			// (get) Token: 0x060038F9 RID: 14585 RVA: 0x000C4DC5 File Offset: 0x000C2FC5
			// (set) Token: 0x060038FA RID: 14586 RVA: 0x000C4DCD File Offset: 0x000C2FCD
			public string Chore { readonly get; set; }

			// Token: 0x1700029B RID: 667
			// (get) Token: 0x060038FB RID: 14587 RVA: 0x000C4DD6 File Offset: 0x000C2FD6
			// (set) Token: 0x060038FC RID: 14588 RVA: 0x000C4DDE File Offset: 0x000C2FDE
			public string ChoreType { readonly get; set; }

			// Token: 0x1700029C RID: 668
			// (get) Token: 0x060038FD RID: 14589 RVA: 0x000C4DE7 File Offset: 0x000C2FE7
			// (set) Token: 0x060038FE RID: 14590 RVA: 0x000C4DEF File Offset: 0x000C2FEF
			public string FailedPrecondition { readonly get; set; }

			// Token: 0x1700029D RID: 669
			// (get) Token: 0x060038FF RID: 14591 RVA: 0x000C4DF8 File Offset: 0x000C2FF8
			// (set) Token: 0x06003900 RID: 14592 RVA: 0x000C4E00 File Offset: 0x000C3000
			public int WorldId { readonly get; set; }
		}
	}
}
