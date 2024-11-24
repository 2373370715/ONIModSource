using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevToolSceneBrowser : DevTool
{
	private class StackItem
	{
		public bool SceneRoot;

		public GameObject Root;

		public string Filter;
	}

	private List<StackItem> Stack = new List<StackItem>();

	private int StackIndex;

	private static int SelectedIndex = -1;

	private static string SearchFilter = "";

	private static List<GameObject> SearchResults = new List<GameObject>();

	private static int SearchSelectedIndex = -1;

	public DevToolSceneBrowser()
	{
		drawFlags = ImGuiWindowFlags.MenuBar;
		StackItem item = new StackItem
		{
			SceneRoot = true,
			Filter = ""
		};
		Stack.Add(item);
	}

	private void PushGameObject(GameObject go)
	{
		if (StackIndex >= Stack.Count || !(go == Stack[StackIndex].Root))
		{
			if (Stack.Count > StackIndex + 1)
			{
				Stack.RemoveRange(StackIndex + 1, Stack.Count - (StackIndex + 1));
			}
			StackItem stackItem = new StackItem();
			stackItem.SceneRoot = go == null;
			stackItem.Root = go;
			stackItem.Filter = "";
			Stack.Add(stackItem);
			StackIndex++;
		}
	}

	protected override void RenderTo(DevPanel panel)
	{
		for (int num = Stack.Count - 1; num > 0; num--)
		{
			StackItem stackItem = Stack[num];
			if (!stackItem.SceneRoot && stackItem.Root.IsNullOrDestroyed())
			{
				Stack.RemoveAt(num);
				StackIndex = Math.Min(num - 1, StackIndex);
			}
		}
		bool flag = false;
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.BeginMenu("Utils"))
			{
				if (ImGui.MenuItem("Goto current selection") && SelectTool.Instance?.selected?.gameObject != null)
				{
					PushGameObject(SelectTool.Instance?.selected?.gameObject);
				}
				if (ImGui.MenuItem("Search All"))
				{
					flag = true;
				}
				ImGui.EndMenu();
			}
			ImGui.EndMenuBar();
		}
		if (ImGui.Button(" < ") && StackIndex > 0)
		{
			StackIndex--;
		}
		ImGui.SameLine();
		if (ImGui.Button(" ^ ") && StackIndex > 0 && !Stack[StackIndex].SceneRoot)
		{
			PushGameObject(Stack[StackIndex].Root.transform.parent?.gameObject);
		}
		ImGui.SameLine();
		if (ImGui.Button(" > ") && StackIndex + 1 < Stack.Count)
		{
			StackIndex++;
		}
		StackItem stackItem2 = Stack[StackIndex];
		if (!stackItem2.SceneRoot)
		{
			ImGui.SameLine();
			if (ImGui.Button("Inspect"))
			{
				DevToolSceneInspector.Inspect(stackItem2.Root);
			}
		}
		List<GameObject> list;
		if (stackItem2.SceneRoot)
		{
			ImGui.Text("Scene root");
			Scene activeScene = SceneManager.GetActiveScene();
			list = new List<GameObject>(activeScene.rootCount);
			activeScene.GetRootGameObjects(list);
		}
		else
		{
			ImGui.LabelText("Selected object", stackItem2.Root.name);
			list = new List<GameObject>();
			foreach (Transform item in stackItem2.Root.transform)
			{
				if (item.gameObject != stackItem2.Root)
				{
					list.Add(item.gameObject);
				}
			}
		}
		if (ImGui.Button("Clear"))
		{
			stackItem2.Filter = "";
		}
		ImGui.SameLine();
		ImGui.InputText("Filter", ref stackItem2.Filter, 64u);
		ImGui.BeginChild("ScrollRegion", new Vector2(0f, 0f), border: true, ImGuiWindowFlags.None);
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = list[i];
			if (!(stackItem2.Filter != "") || gameObject.name.IndexOf(stackItem2.Filter, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
			{
				if (ImGui.Selectable(gameObject.name, selected: false, ImGuiSelectableFlags.AllowDoubleClick) && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
				{
					PushGameObject(gameObject);
				}
				if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
				{
					SelectedIndex = i;
					ImGui.OpenPopup("RightClickMenu");
				}
			}
		}
		if (ImGui.BeginPopup("RightClickMenu"))
		{
			if (ImGui.MenuItem("Inspect"))
			{
				DevToolSceneInspector.Inspect(list[SelectedIndex]);
				SelectedIndex = -1;
			}
			ImGui.EndPopup();
		}
		ImGui.EndChild();
		if (flag)
		{
			ImGui.OpenPopup("SearchAll");
		}
		if (!ImGui.BeginPopupModal("SearchAll"))
		{
			return;
		}
		ImGui.Text("Search all objects in the scene:");
		ImGui.Separator();
		if (ImGui.Button("Clear"))
		{
			SearchFilter = "";
		}
		ImGui.SameLine();
		if (ImGui.InputText("Filter", ref SearchFilter, 64u))
		{
			SearchResults = (from go in UnityEngine.Object.FindObjectsOfType<GameObject>()
				where go.name.IndexOf(SearchFilter, 0, StringComparison.CurrentCultureIgnoreCase) != -1
				orderby go.name
				select go).ToList();
		}
		ImGui.BeginChild("ScrollRegion", new Vector2(0f, 200f), border: true, ImGuiWindowFlags.None);
		int num2 = 0;
		foreach (GameObject searchResult in SearchResults)
		{
			if (ImGui.Selectable(searchResult.name, SearchSelectedIndex == num2))
			{
				SearchSelectedIndex = num2;
			}
			num2++;
		}
		ImGui.EndChild();
		bool flag2 = false;
		if (ImGui.Button("Browse") && SearchSelectedIndex >= 0)
		{
			PushGameObject(SearchResults[SearchSelectedIndex]);
			flag2 = true;
		}
		ImGui.SameLine();
		if (ImGui.Button("Inspect") && SearchSelectedIndex >= 0)
		{
			DevToolSceneInspector.Inspect(SearchResults[SearchSelectedIndex]);
			flag2 = true;
		}
		ImGui.SameLine();
		if (ImGui.Button("Cancel"))
		{
			flag2 = true;
		}
		if (flag2)
		{
			SearchFilter = "";
			SearchResults.Clear();
			SearchSelectedIndex = -1;
			ImGui.CloseCurrentPopup();
		}
		ImGui.EndPopup();
	}
}
