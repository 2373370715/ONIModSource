using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

public class DevToolStateMachineDebug : DevTool
{
	private int selectedStateMachine;

	private int selectedLog;

	private GameObject selectedGameObject;

	private Vector2 scrollPos;

	private bool lockSelection;

	private bool showSettings;

	private string stateMachineFilter = "";

	private void Update()
	{
		if (Application.isPlaying)
		{
			if (selectedGameObject == null)
			{
				lockSelection = false;
			}
			GameObject gameObject = SelectTool.Instance?.selected?.gameObject;
			if (!lockSelection && selectedGameObject != gameObject && gameObject != null && gameObject.GetComponentsInChildren<StateMachineController>().Length != 0)
			{
				selectedGameObject = gameObject;
				selectedStateMachine = 0;
			}
		}
	}

	public void ShowEditor(StateMachineDebuggerSettings.Entry entry)
	{
		ImGui.Text(entry.typeName);
		ImGui.SameLine();
		ImGui.PushID(entry.typeName);
		ImGui.PushID(1);
		ImGui.Checkbox("", ref entry.enableConsoleLogging);
		ImGui.PopID();
		ImGui.SameLine();
		ImGui.PushID(2);
		ImGui.Checkbox("", ref entry.breakOnGoTo);
		ImGui.PopID();
		ImGui.SameLine();
		ImGui.PushID(3);
		ImGui.Checkbox("", ref entry.saveHistory);
		ImGui.PopID();
		ImGui.PopID();
	}

	protected override void RenderTo(DevPanel panel)
	{
		Update();
		ImGui.InputText("Filter:", ref stateMachineFilter, 256u);
		if (showSettings = ImGui.CollapsingHeader("Debug Settings:"))
		{
			if (ImGui.Button("Reset"))
			{
				StateMachineDebuggerSettings.Get().Clear();
			}
			ImGui.Text("EnableConsoleLogging / BreakOnGoTo / SaveHistory");
			int num = 0;
			foreach (StateMachineDebuggerSettings.Entry item in StateMachineDebuggerSettings.Get())
			{
				if (string.IsNullOrEmpty(stateMachineFilter) || item.typeName.ToLower().IndexOf(stateMachineFilter) >= 0)
				{
					ShowEditor(item);
					num++;
				}
			}
		}
		if (!Application.isPlaying || !(selectedGameObject != null))
		{
			return;
		}
		StateMachineController[] componentsInChildren = selectedGameObject.GetComponentsInChildren<StateMachineController>();
		if (componentsInChildren.Length == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		List<StateMachine.Instance> list2 = new List<StateMachine.Instance>();
		List<StateMachine.BaseDef> list3 = new List<StateMachine.BaseDef>();
		StateMachineController[] array = componentsInChildren;
		foreach (StateMachineController stateMachineController in array)
		{
			foreach (StateMachine.Instance item2 in stateMachineController)
			{
				string text = stateMachineController.name + "." + item2.ToString();
				if (item2.isCrashed)
				{
					text = "(ERROR)" + text;
				}
				list.Add(text);
			}
		}
		List<string> list4 = ((stateMachineFilter != null && !(stateMachineFilter == "")) ? (from name in list
			where name.ToLower().Contains(stateMachineFilter)
			select name.ToLower()).ToList() : list.Select((string name) => name.ToLower()).ToList());
		array = componentsInChildren;
		foreach (StateMachineController stateMachineController2 in array)
		{
			foreach (StateMachine.Instance item3 in stateMachineController2)
			{
				string text2 = stateMachineController2.name + "." + item3.ToString();
				if (item3.isCrashed)
				{
					text2 = "(ERROR)" + text2;
				}
				if (list4.Contains(text2.ToLower()))
				{
					list2.Add(item3);
				}
			}
			foreach (StateMachine.BaseDef def in stateMachineController2.GetDefs<StateMachine.BaseDef>())
			{
				list3.Add(def);
			}
		}
		if (list4.Count == 0)
		{
			ImGui.LabelText("Defs", (list3.Count == 0) ? "(none)" : string.Join(", ", list3.Select((StateMachine.BaseDef d) => d.GetType().ToString())));
			array = componentsInChildren;
			foreach (StateMachineController controller in array)
			{
				ShowControllerLog(controller);
			}
			return;
		}
		selectedStateMachine = Math.Min(selectedStateMachine, list4.Count - 1);
		ImGui.LabelText("Defs", (list3.Count == 0) ? "(none)" : string.Join(", ", list3.Select((StateMachine.BaseDef d) => d.GetType().ToString())));
		ImGui.Checkbox("Lock selection", ref lockSelection);
		ImGui.Indent();
		ImGui.Combo("Select state machine", ref selectedStateMachine, list4.ToArray(), list4.Count);
		ImGui.Unindent();
		StateMachine.Instance instance = list2[selectedStateMachine];
		ShowStates(instance);
		ShowTags(instance);
		ShowDetails(instance);
		ShowLog(instance);
		ShowControllerLog(instance);
		ShowHistory(instance.GetMaster().GetComponent<StateMachineController>());
		ShowKAnimControllerLog();
	}

	private void ShowStates(StateMachine.Instance state_machine_instance)
	{
		StateMachine stateMachine = state_machine_instance.GetStateMachine();
		ImGui.Text(stateMachine.ToString() + ": ");
		ImGui.Checkbox("Break On GoTo: ", ref state_machine_instance.breakOnGoTo);
		ImGui.Checkbox("Console Logging: ", ref state_machine_instance.enableConsoleLogging);
		string value = "None";
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		if (currentState != null)
		{
			value = currentState.name;
		}
		string[] array = stateMachine.GetStateNames().Append("None");
		array[0] = array[0];
		int num = Array.IndexOf(array, value);
		int v = num;
		for (int i = 0; i < array.Length; i++)
		{
			ImGui.RadioButton(array[i], ref v, i);
		}
		if (v != num)
		{
			if (array[v] == "None")
			{
				state_machine_instance.StopSM("StateMachineEditor.StopSM");
			}
			else
			{
				state_machine_instance.GoTo(array[v]);
			}
		}
	}

	public void ShowTags(StateMachine.Instance state_machine_instance)
	{
		ImGui.Text("Tags:");
		ImGui.Indent();
		KPrefabID component = state_machine_instance.GetComponent<KPrefabID>();
		if (component != null)
		{
			foreach (Tag tag in component.Tags)
			{
				ImGui.Text(tag.Name);
			}
		}
		ImGui.Unindent();
	}

	private void ShowDetails(StateMachine.Instance state_machine_instance)
	{
		state_machine_instance.GetStateMachine();
		string text = "None";
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		if (currentState != null)
		{
			text = currentState.name;
		}
		ImGui.Text(text + ": ");
		ImGui.Indent();
		ShowParameters(state_machine_instance);
		ShowEvents(state_machine_instance);
		ShowTransitions(state_machine_instance);
		ShowEnterActions(state_machine_instance);
		ShowExitActions(state_machine_instance);
		ImGui.Unindent();
	}

	private void ShowParameters(StateMachine.Instance state_machine_instance)
	{
		ImGui.Text("Parameters:");
		ImGui.Indent();
		StateMachine.Parameter.Context[] parameterContexts = state_machine_instance.GetParameterContexts();
		for (int i = 0; i < parameterContexts.Length; i++)
		{
			parameterContexts[i].ShowDevTool(state_machine_instance);
		}
		ImGui.Unindent();
	}

	private void ShowEvents(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Events: ");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.events == null)
			{
				continue;
			}
			foreach (StateEvent @event in state.events)
			{
				ImGui.Text(@event.GetName());
			}
		}
		ImGui.Unindent();
	}

	private void ShowTransitions(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Transitions:");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.transitions != null)
			{
				for (int j = 0; j < state.transitions.Count; j++)
				{
					ImGui.Text(state.transitions[j].ToString());
				}
			}
		}
		ImGui.Unindent();
	}

	private void ShowExitActions(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Exit Actions: ");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.exitActions == null)
			{
				continue;
			}
			foreach (StateMachine.Action exitAction in state.exitActions)
			{
				ImGui.Text(exitAction.name);
			}
		}
		ImGui.Unindent();
	}

	private void ShowEnterActions(StateMachine.Instance state_machine_instance)
	{
		StateMachine.BaseState currentState = state_machine_instance.GetCurrentState();
		ImGui.Text("Enter Actions: ");
		if (currentState == null)
		{
			return;
		}
		ImGui.Indent();
		for (int i = 0; i < currentState.GetStateCount(); i++)
		{
			StateMachine.BaseState state = currentState.GetState(i);
			if (state.enterActions == null)
			{
				continue;
			}
			foreach (StateMachine.Action enterAction in state.enterActions)
			{
				ImGui.Text(enterAction.name);
			}
		}
		ImGui.Unindent();
	}

	private void ShowLog(StateMachine.Instance state_machine_instance)
	{
		ImGui.Text("Machine Log:");
	}

	private void ShowKAnimControllerLog()
	{
		_ = selectedGameObject.GetComponentInChildren<KAnimControllerBase>() == null;
	}

	private void ShowHistory(StateMachineController controller)
	{
		ImGui.Text("Logger disabled");
	}

	private void ShowControllerLog(StateMachineController controller)
	{
		ImGui.Text("Object Log:");
	}

	private void ShowControllerLog(StateMachine.Instance state_machine)
	{
		if (!state_machine.GetMaster().isNull)
		{
			ShowControllerLog(state_machine.GetMaster().GetComponent<StateMachineController>());
		}
	}
}
