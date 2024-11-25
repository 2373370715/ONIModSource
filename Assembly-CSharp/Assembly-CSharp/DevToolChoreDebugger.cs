using System.Collections.Generic;
using System.Collections.Specialized;
using ImGuiNET;
using UnityEngine;

public class DevToolChoreDebugger : DevTool {
    private readonly OrderedDictionary columns = new OrderedDictionary {
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

    private            ChoreConsumer Consumer;
    private            string        filter = "";
    private            bool          lockSelection;
    private            int           rowIndex;
    private            GameObject    selectedGameObject;
    private            bool          showLastSuccessfulPreconditionSnapshot;
    protected override void          RenderTo(DevPanel panel) { Update(); }

    public void Update() {
        if (!Application.isPlaying                          ||
            SelectTool.Instance                     == null ||
            SelectTool.Instance.selected            == null ||
            SelectTool.Instance.selected.gameObject == null)
            return;

        var gameObject = SelectTool.Instance.selected.gameObject;
        if (Consumer == null || (!lockSelection && selectedGameObject != gameObject)) {
            Consumer           = gameObject.GetComponent<ChoreConsumer>();
            selectedGameObject = gameObject;
        }

        if (Consumer != null) {
            ImGui.InputText("Filter:", ref filter, 256U);
            DisplayAvailableChores();
            ImGui.Text("");
        }
    }

    private void DisplayAvailableChores() {
        ImGui.Checkbox("Lock selection",                       ref lockSelection);
        ImGui.Checkbox("Show Last Successful Chore Selection", ref showLastSuccessfulPreconditionSnapshot);
        ImGui.Text("Available Chores:");
        var target_snapshot                                         = Consumer.GetLastPreconditionSnapshot();
        if (showLastSuccessfulPreconditionSnapshot) target_snapshot = Consumer.GetLastSuccessfulPreconditionSnapshot();
        ShowChores(target_snapshot);
    }

    private void ShowChores(ChoreConsumer.PreconditionSnapshot target_snapshot) {
        var flags = ImGuiTableFlags.RowBg          |
                    ImGuiTableFlags.BordersInnerH  |
                    ImGuiTableFlags.BordersOuterH  |
                    ImGuiTableFlags.BordersInnerV  |
                    ImGuiTableFlags.BordersOuterV  |
                    ImGuiTableFlags.SizingFixedFit |
                    ImGuiTableFlags.ScrollX        |
                    ImGuiTableFlags.ScrollY;

        rowIndex = 0;
        if (ImGui.BeginTable("Available Chores", columns.Count, flags)) {
            foreach (var obj in columns.Keys) ImGui.TableSetupColumn(obj.ToString(), ImGuiTableColumnFlags.WidthFixed);
            ImGui.TableHeadersRow();
            for (var i = target_snapshot.succeededContexts.Count - 1; i >= 0; i--)
                ShowContext(target_snapshot.succeededContexts[i]);

            if (target_snapshot.doFailedContextsNeedSorting) {
                target_snapshot.failedContexts.Sort();
                target_snapshot.doFailedContextsNeedSorting = false;
            }

            for (var j = target_snapshot.failedContexts.Count - 1; j >= 0; j--)
                ShowContext(target_snapshot.failedContexts[j]);

            ImGui.EndTable();
        }
    }

    private void ShowContext(Chore.Precondition.Context context) {
        var text = "";
        var chore = context.chore;
        if (!context.IsSuccess()) text = context.chore.GetPreconditions()[context.failedPreconditionId].condition.id;
        var text2 = "";
        if (chore.driver != null) text2 = chore.driver.name;
        var text3 = "";
        if (chore.overrideTarget != null) text3 = chore.overrideTarget.name;
        var text4 = "";
        if (!chore.isNull) text4 = chore.gameObject.name;
        if (Chore.Precondition.Context.ShouldFilter(filter, chore.GetType().ToString()) &&
            Chore.Precondition.Context.ShouldFilter(filter, chore.choreType.Id)         &&
            Chore.Precondition.Context.ShouldFilter(filter, text)                       &&
            Chore.Precondition.Context.ShouldFilter(filter, text2)                      &&
            Chore.Precondition.Context.ShouldFilter(filter, text3)                      &&
            Chore.Precondition.Context.ShouldFilter(filter, text4))
            return;

        columns["Id"]               = chore.id.ToString();
        columns["Class"]            = chore.GetType().ToString().Replace("`1", "");
        columns["Type"]             = chore.choreType.Id;
        columns["PriorityClass"]    = context.masterPriority.priority_class.ToString();
        columns["PersonalPriority"] = context.personalPriority.ToString();
        columns["PriorityValue"]    = context.masterPriority.priority_value.ToString();
        columns["Priority"]         = context.priority.ToString();
        columns["PriorityMod"]      = context.priorityMod.ToString();
        columns["ConsumerPriority"] = context.consumerPriority.ToString();
        columns["Cost"]             = context.cost.ToString();
        columns["Interrupt"]        = context.interruptPriority.ToString();
        columns["Precondition"]     = text;
        columns["Override"]         = text3;
        columns["Assigned To"]      = text2;
        columns["Owner"]            = text4;
        columns["Details"]          = "";
        ImGui.TableNextRow();
        var format = "ID_row_{0}";
        var num    = rowIndex;
        rowIndex = num + 1;
        ImGui.PushID(string.Format(format, num));
        for (var i = 0; i < columns.Count; i++) {
            ImGui.TableSetColumnIndex(i);
            ImGui.Text(columns[i].ToString());
        }

        ImGui.PopID();
    }

    public void ConsumerDebugDisplayLog() { }

    public class EditorPreconditionSnapshot {
        public List<EditorContext> SucceededContexts { get; set; }
        public List<EditorContext> FailedContexts    { get; set; }

        public struct EditorContext {
            public string Chore              { readonly get; set; }
            public string ChoreType          { readonly get; set; }
            public string FailedPrecondition { readonly get; set; }
            public int    WorldId            { readonly get; set; }
        }
    }
}