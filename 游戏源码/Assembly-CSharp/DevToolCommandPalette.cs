using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000B9E RID: 2974
public class DevToolCommandPalette : DevTool
{
	// Token: 0x06003901 RID: 14593 RVA: 0x000C4E09 File Offset: 0x000C3009
	public DevToolCommandPalette() : this(null)
	{
	}

	// Token: 0x06003902 RID: 14594 RVA: 0x0021D178 File Offset: 0x0021B378
	public DevToolCommandPalette(List<DevToolCommandPalette.Command> commands = null)
	{
		this.drawFlags |= ImGuiWindowFlags.NoResize;
		this.drawFlags |= ImGuiWindowFlags.NoScrollbar;
		this.drawFlags |= ImGuiWindowFlags.NoScrollWithMouse;
		if (commands == null)
		{
			this.commands.allValues = DevToolCommandPaletteUtil.GenerateDefaultCommandPalette();
			return;
		}
		this.commands.allValues = commands;
	}

	// Token: 0x06003903 RID: 14595 RVA: 0x000C4E12 File Offset: 0x000C3012
	public static void Init()
	{
		DevToolCommandPalette.InitWithCommands(DevToolCommandPaletteUtil.GenerateDefaultCommandPalette());
	}

	// Token: 0x06003904 RID: 14596 RVA: 0x000C4E1E File Offset: 0x000C301E
	public static void InitWithCommands(List<DevToolCommandPalette.Command> commands)
	{
		DevToolManager.Instance.panels.AddPanelFor(new DevToolCommandPalette(commands));
	}

	// Token: 0x06003905 RID: 14597 RVA: 0x0021D208 File Offset: 0x0021B408
	protected override void RenderTo(DevPanel panel)
	{
		DevToolCommandPalette.Resize(panel);
		if (this.commands.allValues == null)
		{
			ImGui.Text("No commands list given");
			return;
		}
		if (this.commands.allValues.Count == 0)
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
			this.m_selected_index--;
			this.shouldScrollToSelectedCommandFlag = true;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.m_selected_index++;
			this.shouldScrollToSelectedCommandFlag = true;
		}
		if (this.commands.filteredValues.Count > 0)
		{
			while (this.m_selected_index < 0)
			{
				this.m_selected_index += this.commands.filteredValues.Count;
			}
			this.m_selected_index %= this.commands.filteredValues.Count;
		}
		else
		{
			this.m_selected_index = 0;
		}
		DevToolCommandPalette.Command command = null;
		if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && this.commands.filteredValues.Count > 0 && command == null)
		{
			command = this.commands.filteredValues[this.m_selected_index];
		}
		if (this.m_should_focus_search)
		{
			ImGui.SetKeyboardFocusHere();
		}
		if (ImGui.InputText("Filter", ref this.commands.filter, 30U) || this.m_should_focus_search)
		{
			this.commands.Refilter();
		}
		this.m_should_focus_search = false;
		ImGui.Separator();
		string text = "Up arrow & down arrow to navigate. Enter to select. ";
		if (this.commands.filteredValues.Count > 0 && this.commands.didUseFilter)
		{
			text += string.Format("Found {0} Results", this.commands.filteredValues.Count);
		}
		ImGui.Text(text);
		ImGui.Separator();
		if (ImGui.BeginChild("ID_scroll_region"))
		{
			if (this.commands.filteredValues.Count <= 0)
			{
				ImGui.Text("Couldn't find anything that matches \"" + this.commands.filter + "\", maybe it hasn't been added yet?");
			}
			else
			{
				for (int i = 0; i < this.commands.filteredValues.Count; i++)
				{
					DevToolCommandPalette.Command command2 = this.commands.filteredValues[i];
					bool flag = i == this.m_selected_index;
					ImGui.PushID(i);
					bool flag2;
					if (flag)
					{
						flag2 = ImGui.Selectable("> " + command2.display_name, flag);
					}
					else
					{
						flag2 = ImGui.Selectable("  " + command2.display_name, flag);
					}
					ImGui.PopID();
					if (this.shouldScrollToSelectedCommandFlag && flag)
					{
						this.shouldScrollToSelectedCommandFlag = false;
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

	// Token: 0x06003906 RID: 14598 RVA: 0x0021D4F8 File Offset: 0x0021B6F8
	private static void Resize(DevPanel devToolPanel)
	{
		float num = 800f;
		float num2 = 400f;
		Rect rect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		Rect rect2 = new Rect
		{
			x = rect.x + rect.width / 2f - num / 2f,
			y = rect.y + rect.height / 2f - num2 / 2f,
			width = num,
			height = num2
		};
		devToolPanel.SetPosition(rect2.position, ImGuiCond.None);
		devToolPanel.SetSize(rect2.size, ImGuiCond.None);
	}

	// Token: 0x040026CE RID: 9934
	private int m_selected_index;

	// Token: 0x040026CF RID: 9935
	private StringSearchableList<DevToolCommandPalette.Command> commands = new StringSearchableList<DevToolCommandPalette.Command>(delegate(DevToolCommandPalette.Command command, in string filter)
	{
		return !StringSearchableListUtil.DoAnyTagsMatchFilter(command.tags, filter);
	});

	// Token: 0x040026D0 RID: 9936
	private bool m_should_focus_search = true;

	// Token: 0x040026D1 RID: 9937
	private bool shouldScrollToSelectedCommandFlag;

	// Token: 0x02000B9F RID: 2975
	public class Command
	{
		// Token: 0x06003907 RID: 14599 RVA: 0x000C4E36 File Offset: 0x000C3036
		public Command(string primary_tag, System.Action on_select) : this(new string[]
		{
			primary_tag
		}, on_select)
		{
		}

		// Token: 0x06003908 RID: 14600 RVA: 0x000C4E49 File Offset: 0x000C3049
		public Command(string primary_tag, string tag_a, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a
		}, on_select)
		{
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x000C4E60 File Offset: 0x000C3060
		public Command(string primary_tag, string tag_a, string tag_b, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b
		}, on_select)
		{
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x000C4E7C File Offset: 0x000C307C
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c
		}, on_select)
		{
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x000C4E9D File Offset: 0x000C309D
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d
		}, on_select)
		{
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x000C4EC3 File Offset: 0x000C30C3
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d,
			tag_e
		}, on_select)
		{
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x000C4EEE File Offset: 0x000C30EE
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, string tag_f, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d,
			tag_e,
			tag_f
		}, on_select)
		{
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x000C4F1E File Offset: 0x000C311E
		public Command(string primary_tag, string[] additional_tags, System.Action on_select) : this(new string[]
		{
			primary_tag
		}.Concat(additional_tags).ToArray<string>(), on_select)
		{
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x0021D5B0 File Offset: 0x0021B7B0
		public Command(string[] tags, System.Action on_select)
		{
			this.display_name = tags[0];
			this.tags = (from t in tags
			select t.ToLowerInvariant()).ToArray<string>();
			this.m_on_select = on_select;
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x000C4F3C File Offset: 0x000C313C
		public void Internal_Select()
		{
			this.m_on_select();
		}

		// Token: 0x040026D2 RID: 9938
		public string display_name;

		// Token: 0x040026D3 RID: 9939
		public string[] tags;

		// Token: 0x040026D4 RID: 9940
		private System.Action m_on_select;
	}
}
