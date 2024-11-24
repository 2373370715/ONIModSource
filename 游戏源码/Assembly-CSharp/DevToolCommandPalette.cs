using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

public class DevToolCommandPalette : DevTool
{
	public class Command
	{
		public string display_name;

		public string[] tags;

		private System.Action m_on_select;

		public Command(string primary_tag, System.Action on_select)
			: this(new string[1] { primary_tag }, on_select)
		{
		}

		public Command(string primary_tag, string tag_a, System.Action on_select)
			: this(new string[2] { primary_tag, tag_a }, on_select)
		{
		}

		public Command(string primary_tag, string tag_a, string tag_b, System.Action on_select)
			: this(new string[3] { primary_tag, tag_a, tag_b }, on_select)
		{
		}

		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, System.Action on_select)
			: this(new string[4] { primary_tag, tag_a, tag_b, tag_c }, on_select)
		{
		}

		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, System.Action on_select)
			: this(new string[5] { primary_tag, tag_a, tag_b, tag_c, tag_d }, on_select)
		{
		}

		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, System.Action on_select)
			: this(new string[6] { primary_tag, tag_a, tag_b, tag_c, tag_d, tag_e }, on_select)
		{
		}

		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, string tag_f, System.Action on_select)
			: this(new string[7] { primary_tag, tag_a, tag_b, tag_c, tag_d, tag_e, tag_f }, on_select)
		{
		}

		public Command(string primary_tag, string[] additional_tags, System.Action on_select)
			: this(new string[1] { primary_tag }.Concat(additional_tags).ToArray(), on_select)
		{
		}

		public Command(string[] tags, System.Action on_select)
		{
			display_name = tags[0];
			this.tags = tags.Select((string t) => t.ToLowerInvariant()).ToArray();
			m_on_select = on_select;
		}

		public void Internal_Select()
		{
			m_on_select();
		}
	}

	private int m_selected_index;

	private StringSearchableList<Command> commands = new StringSearchableList<Command>(delegate(Command command, in string filter)
	{
		return !StringSearchableListUtil.DoAnyTagsMatchFilter(command.tags, in filter);
	});

	private bool m_should_focus_search = true;

	private bool shouldScrollToSelectedCommandFlag;

	public DevToolCommandPalette()
		: this(null)
	{
	}

	public DevToolCommandPalette(List<Command> commands = null)
	{
		drawFlags |= ImGuiWindowFlags.NoResize;
		drawFlags |= ImGuiWindowFlags.NoScrollbar;
		drawFlags |= ImGuiWindowFlags.NoScrollWithMouse;
		if (commands == null)
		{
			this.commands.allValues = DevToolCommandPaletteUtil.GenerateDefaultCommandPalette();
		}
		else
		{
			this.commands.allValues = commands;
		}
	}

	public static void Init()
	{
		InitWithCommands(DevToolCommandPaletteUtil.GenerateDefaultCommandPalette());
	}

	public static void InitWithCommands(List<Command> commands)
	{
		DevToolManager.Instance.panels.AddPanelFor(new DevToolCommandPalette(commands));
	}

	protected override void RenderTo(DevPanel panel)
	{
		Resize(panel);
		if (commands.allValues == null)
		{
			ImGui.Text("No commands list given");
			return;
		}
		if (commands.allValues.Count == 0)
		{
			ImGui.Text("Given command list is empty, no results to show.");
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			panel.Close();
			return;
		}
		if (!ImGui.IsWindowFocused(ImGuiFocusedFlags.ChildWindows))
		{
			panel.Close();
			return;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			m_selected_index--;
			shouldScrollToSelectedCommandFlag = true;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			m_selected_index++;
			shouldScrollToSelectedCommandFlag = true;
		}
		if (commands.filteredValues.Count > 0)
		{
			while (m_selected_index < 0)
			{
				m_selected_index += commands.filteredValues.Count;
			}
			m_selected_index %= commands.filteredValues.Count;
		}
		else
		{
			m_selected_index = 0;
		}
		Command command = null;
		if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && commands.filteredValues.Count > 0 && command == null)
		{
			command = commands.filteredValues[m_selected_index];
		}
		if (m_should_focus_search)
		{
			ImGui.SetKeyboardFocusHere();
		}
		if (ImGui.InputText("Filter", ref commands.filter, 30u) || m_should_focus_search)
		{
			commands.Refilter();
		}
		m_should_focus_search = false;
		ImGui.Separator();
		string text = "Up arrow & down arrow to navigate. Enter to select. ";
		if (commands.filteredValues.Count > 0 && commands.didUseFilter)
		{
			text += $"Found {commands.filteredValues.Count} Results";
		}
		ImGui.Text(text);
		ImGui.Separator();
		if (ImGui.BeginChild("ID_scroll_region"))
		{
			if (commands.filteredValues.Count <= 0)
			{
				ImGui.Text("Couldn't find anything that matches \"" + commands.filter + "\", maybe it hasn't been added yet?");
			}
			else
			{
				for (int i = 0; i < commands.filteredValues.Count; i++)
				{
					Command command2 = commands.filteredValues[i];
					bool flag = i == m_selected_index;
					bool flag2 = false;
					ImGui.PushID(i);
					flag2 = ((!flag) ? ImGui.Selectable("  " + command2.display_name, flag) : ImGui.Selectable("> " + command2.display_name, flag));
					ImGui.PopID();
					if (shouldScrollToSelectedCommandFlag && flag)
					{
						shouldScrollToSelectedCommandFlag = false;
						ImGui.SetScrollHereY(0.5f);
					}
					if (flag2 && command == null)
					{
						command = command2;
					}
				}
			}
		}
		ImGui.EndChild();
		if (command != null)
		{
			command.Internal_Select();
			panel.Close();
		}
	}

	private static void Resize(DevPanel devToolPanel)
	{
		float num = 800f;
		float num2 = 400f;
		Rect rect = new Rect(0f, 0f, Screen.width, Screen.height);
		Rect rect2 = default(Rect);
		rect2.x = rect.x + rect.width / 2f - num / 2f;
		rect2.y = rect.y + rect.height / 2f - num2 / 2f;
		rect2.width = num;
		rect2.height = num2;
		Rect rect3 = rect2;
		devToolPanel.SetPosition(rect3.position);
		devToolPanel.SetSize(rect3.size);
	}
}
