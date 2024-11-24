using System.Collections.Generic;
using System.Collections.Specialized;
using ImGuiNET;
using UnityEngine;

public class DevToolChoreDebugger : DevTool
{
	public class EditorPreconditionSnapshot
	{
		public struct EditorContext
		{
			public string Chore { get; set; }

			public string ChoreType { get; set; }

			public string FailedPrecondition { get; set; }

			public int WorldId { get; set; }
		}

		public List<EditorContext> SucceededContexts { get; set; }

		public List<EditorContext> FailedContexts { get; set; }
	}

	private string filter = "";

	private bool showLastSuccessfulPreconditionSnapshot;

	private bool lockSelection;

	private ChoreConsumer Consumer;

	private GameObject selectedGameObject;

	private OrderedDictionary columns = new OrderedDictionary
	{
		{ "BP", "" },
		{ "Id", "" },
		{ "Class", "" },
		{ "Type", "" },
		{ "PriorityClass", "" },
		{ "PersonalPriority", "" },
		{ "PriorityValue", "" },
		{ "Priority", "" },
		{ "PriorityMod", "" },
		{ "ConsumerPriority", "" },
		{ "Cost", "" },
		{ "Interrupt", "" },
		{ "Precondition", "" },
		{ "Override", "" },
		{ "Assigned To", "" },
		{ "Owner", "" },
		{ "Details", "" }
	};

	private int rowIndex;

	protected override void RenderTo(DevPanel panel)
	{
		Update();
	}

	public void Update()
	{
		if (Application.isPlaying && !(SelectTool.Instance == null) && !(SelectTool.Instance.selected == null) && !(SelectTool.Instance.selected.gameObject == null))
		{
			GameObject gameObject = SelectTool.Instance.selected.gameObject;
			if (Consumer == null || (!lockSelection && selectedGameObject != gameObject))
			{
				Consumer = gameObject.GetComponent<ChoreConsumer>();
				selectedGameObject = gameObject;
			}
			if (Consumer != null)
			{
				ImGui.InputText("Filter:", ref filter, 256u);
				DisplayAvailableChores();
				ImGui.Text("");
			}
		}
	}

	private void DisplayAvailableChores()
	{
		ImGui.Checkbox("Lock selection", ref lockSelection);
		ImGui.Checkbox("Show Last Successful Chore Selection", ref showLastSuccessfulPreconditionSnapshot);
		ImGui.Text("Available Chores:");
		ChoreConsumer.PreconditionSnapshot target_snapshot = Consumer.GetLastPreconditionSnapshot();
		if (showLastSuccessfulPreconditionSnapshot)
		{
			target_snapshot = Consumer.GetLastSuccessfulPreconditionSnapshot();
		}
		ShowChores(target_snapshot);
	}

	private void ShowChores(ChoreConsumer.PreconditionSnapshot target_snapshot)
	{
		ImGuiTableFlags flags = ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.ScrollX | ImGuiTableFlags.ScrollY;
		rowIndex = 0;
		if (!ImGui.BeginTable("Available Chores", columns.Count, flags))
		{
			return;
		}
		foreach (object key in columns.Keys)
		{
			ImGui.TableSetupColumn(key.ToString(), ImGuiTableColumnFlags.WidthFixed);
		}
		ImGui.TableHeadersRow();
		for (int num = target_snapshot.succeededContexts.Count - 1; num >= 0; num--)
		{
			ShowContext(target_snapshot.succeededContexts[num]);
		}
		if (target_snapshot.doFailedContextsNeedSorting)
		{
			target_snapshot.failedContexts.Sort();
			target_snapshot.doFailedContextsNeedSorting = false;
		}
		for (int num2 = target_snapshot.failedContexts.Count - 1; num2 >= 0; num2--)
		{
			ShowContext(target_snapshot.failedContexts[num2]);
		}
		ImGui.EndTable();
	}

	private void ShowContext(Chore.Precondition.Context context)
	{
		string text = "";
		Chore chore = context.chore;
		if (!context.IsSuccess())
		{
			text = context.chore.GetPreconditions()[context.failedPreconditionId].id;
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
		if (!Chore.Precondition.Context.ShouldFilter(filter, chore.GetType().ToString()) || !Chore.Precondition.Context.ShouldFilter(filter, chore.choreType.Id) || !Chore.Precondition.Context.ShouldFilter(filter, text) || !Chore.Precondition.Context.ShouldFilter(filter, text2) || !Chore.Precondition.Context.ShouldFilter(filter, text3) || !Chore.Precondition.Context.ShouldFilter(filter, text4))
		{
			columns["Id"] = chore.id.ToString();
			columns["Class"] = chore.GetType().ToString().Replace("`1", "");
			columns["Type"] = chore.choreType.Id;
			columns["PriorityClass"] = context.masterPriority.priority_class.ToString();
			columns["PersonalPriority"] = context.personalPriority.ToString();
			columns["PriorityValue"] = context.masterPriority.priority_value.ToString();
			columns["Priority"] = context.priority.ToString();
			columns["PriorityMod"] = context.priorityMod.ToString();
			columns["ConsumerPriority"] = context.consumerPriority.ToString();
			columns["Cost"] = context.cost.ToString();
			columns["Interrupt"] = context.interruptPriority.ToString();
			columns["Precondition"] = text;
			columns["Override"] = text3;
			columns["Assigned To"] = text2;
			columns["Owner"] = text4;
			columns["Details"] = "";
			ImGui.TableNextRow();
			ImGui.PushID($"ID_row_{rowIndex++}");
			for (int i = 0; i < columns.Count; i++)
			{
				ImGui.TableSetColumnIndex(i);
				ImGui.Text(columns[i].ToString());
			}
			ImGui.PopID();
		}
	}

	public void ConsumerDebugDisplayLog()
	{
	}
}
