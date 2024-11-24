using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using ImGuiNET;
using Klei;
using STRINGS;
using UnityEngine;

// Token: 0x02000BB5 RID: 2997
public class DevToolManager
{
	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06003964 RID: 14692 RVA: 0x000C521B File Offset: 0x000C341B
	public bool Show
	{
		get
		{
			return this.showImGui;
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06003965 RID: 14693 RVA: 0x000C5223 File Offset: 0x000C3423
	private bool quickDevEnabled
	{
		get
		{
			return DebugHandler.enabled && GenericGameSettings.instance.quickDevTools;
		}
	}

	// Token: 0x06003966 RID: 14694 RVA: 0x00220B04 File Offset: 0x0021ED04
	public DevToolManager()
	{
		DevToolManager.Instance = this;
		this.RegisterDevTool<DevToolSimDebug>("Debuggers/Sim Debug");
		this.RegisterDevTool<DevToolStateMachineDebug>("Debuggers/State Machine");
		this.RegisterDevTool<DevToolSaveGameInfo>("Debuggers/Save Game Info");
		this.RegisterDevTool<DevToolPerformanceInfo>("Debuggers/Performance Info");
		this.RegisterDevTool<DevToolPrintingPodDebug>("Debuggers/Printing Pod Debug");
		this.RegisterDevTool<DevToolBigBaseMutations>("Debuggers/Big Base Mutation Utilities");
		this.RegisterDevTool<DevToolNavGrid>("Debuggers/Nav Grid");
		this.RegisterDevTool<DevToolResearchDebugger>("Debuggers/Research");
		this.RegisterDevTool<DevToolStatusItems>("Debuggers/StatusItems");
		this.RegisterDevTool<DevToolUI>("Debuggers/UI");
		this.RegisterDevTool<DevToolUnlockedIds>("Debuggers/UnlockedIds List");
		this.RegisterDevTool<DevToolStringsTable>("Debuggers/StringsTable");
		this.RegisterDevTool<DevToolChoreDebugger>("Debuggers/Chore");
		this.RegisterDevTool<DevToolBatchedAnimDebug>("Debuggers/Batched Anim");
		this.RegisterDevTool<DevTool_StoryTraits_Reveal>("Debuggers/Story Traits Reveal");
		this.RegisterDevTool<DevTool_StoryTrait_CritterManipulator>("Debuggers/Story Trait - Critter Manipulator");
		this.RegisterDevTool<DevToolAnimEventManager>("Debuggers/Anim Event Manager");
		this.RegisterDevTool<DevToolSceneBrowser>("Scene/Browser");
		this.RegisterDevTool<DevToolSceneInspector>("Scene/Inspector");
		this.menuNodes.AddAction("Help/" + UI.FRONTEND.DEVTOOLS.TITLE.text, delegate
		{
			this.warning.ShouldDrawWindow = true;
		});
		this.RegisterDevTool<DevToolCommandPalette>("Help/Command Palette");
		this.RegisterAdditionalDevToolsByReflection();
	}

	// Token: 0x06003967 RID: 14695 RVA: 0x000C5238 File Offset: 0x000C3438
	public void Init()
	{
		this.UserAcceptedWarning = (KPlayerPrefs.GetInt("ShowDevtools", 0) == 1);
	}

	// Token: 0x06003968 RID: 14696 RVA: 0x00220C70 File Offset: 0x0021EE70
	private void RegisterDevTool<T>(string location) where T : DevTool, new()
	{
		this.menuNodes.AddAction(location, delegate
		{
			this.panels.AddPanelFor<T>();
		});
		this.dontAutomaticallyRegisterTypes.Add(typeof(T));
		this.devToolNameDict[typeof(T)] = Path.GetFileName(location);
	}

	// Token: 0x06003969 RID: 14697 RVA: 0x00220CC8 File Offset: 0x0021EEC8
	private void RegisterAdditionalDevToolsByReflection()
	{
		using (List<Type>.Enumerator enumerator = ReflectionUtil.CollectTypesThatInheritOrImplement<DevTool>(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Type type = enumerator.Current;
				if (!type.IsAbstract && !this.dontAutomaticallyRegisterTypes.Contains(type) && ReflectionUtil.HasDefaultConstructor(type))
				{
					this.menuNodes.AddAction("Debuggers/" + DevToolUtil.GenerateDevToolName(type), delegate
					{
						this.panels.AddPanelFor((DevTool)Activator.CreateInstance(type));
					});
				}
			}
		}
	}

	// Token: 0x0600396A RID: 14698 RVA: 0x00220D84 File Offset: 0x0021EF84
	public void UpdateShouldShowTools()
	{
		if (!DebugHandler.enabled)
		{
			this.showImGui = false;
			return;
		}
		bool flag = Input.GetKeyDown(KeyCode.BackQuote) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl));
		if (!this.toggleKeyWasDown && flag)
		{
			this.showImGui = !this.showImGui;
		}
		this.toggleKeyWasDown = flag;
	}

	// Token: 0x0600396B RID: 14699 RVA: 0x00220DEC File Offset: 0x0021EFEC
	public void UpdateTools()
	{
		if (!DebugHandler.enabled)
		{
			return;
		}
		if (this.showImGui)
		{
			if (this.warning.ShouldDrawWindow)
			{
				this.warning.DrawWindow(out this.warning.ShouldDrawWindow);
			}
			if (!this.UserAcceptedWarning)
			{
				this.warning.DrawMenuBar();
			}
			else
			{
				this.DrawMenu();
				this.panels.Render();
				if (this.showImguiState)
				{
					if (ImGui.Begin("ImGui state", ref this.showImguiState))
					{
						ImGui.Checkbox("ImGui.GetIO().WantCaptureMouse", ImGui.GetIO().WantCaptureMouse);
						ImGui.Checkbox("ImGui.GetIO().WantCaptureKeyboard", ImGui.GetIO().WantCaptureKeyboard);
					}
					ImGui.End();
				}
				if (this.showImguiDemo)
				{
					ImGui.ShowDemoWindow(ref this.showImguiDemo);
				}
			}
		}
		this.UpdateConsumingGameInputs();
		this.UpdateShortcuts();
	}

	// Token: 0x0600396C RID: 14700 RVA: 0x000C524E File Offset: 0x000C344E
	private void UpdateShortcuts()
	{
		if ((this.showImGui || this.quickDevEnabled) && this.UserAcceptedWarning)
		{
			this.<UpdateShortcuts>g__DoUpdate|26_0();
		}
	}

	// Token: 0x0600396D RID: 14701 RVA: 0x00220EC4 File Offset: 0x0021F0C4
	private void DrawMenu()
	{
		this.menuFontSize.InitializeIfNeeded();
		if (ImGui.BeginMainMenuBar())
		{
			this.menuNodes.Draw();
			this.menuFontSize.DrawMenu();
			if (ImGui.BeginMenu("IMGUI"))
			{
				ImGui.Checkbox("ImGui state", ref this.showImguiState);
				ImGui.Checkbox("ImGui Demo", ref this.showImguiDemo);
				ImGui.EndMenu();
			}
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x0600396E RID: 14702 RVA: 0x00220F34 File Offset: 0x0021F134
	private unsafe void UpdateConsumingGameInputs()
	{
		this.doesImGuiWantInput = false;
		if (this.showImGui)
		{
			this.doesImGuiWantInput = (*ImGui.GetIO().WantCaptureMouse || *ImGui.GetIO().WantCaptureKeyboard);
			if (!this.prevDoesImGuiWantInput && this.doesImGuiWantInput)
			{
				DevToolManager.<UpdateConsumingGameInputs>g__OnInputEnterImGui|28_0();
			}
			if (this.prevDoesImGuiWantInput && !this.doesImGuiWantInput)
			{
				DevToolManager.<UpdateConsumingGameInputs>g__OnInputExitImGui|28_1();
			}
		}
		if (this.prevShowImGui && this.prevDoesImGuiWantInput && !this.showImGui)
		{
			DevToolManager.<UpdateConsumingGameInputs>g__OnInputExitImGui|28_1();
		}
		this.prevShowImGui = this.showImGui;
		this.prevDoesImGuiWantInput = this.doesImGuiWantInput;
		KInputManager.devToolFocus = (this.showImGui && this.doesImGuiWantInput);
	}

	// Token: 0x06003971 RID: 14705 RVA: 0x00220FEC File Offset: 0x0021F1EC
	[CompilerGenerated]
	private void <UpdateShortcuts>g__DoUpdate|26_0()
	{
		if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.Space))
		{
			DevToolCommandPalette.Init();
			this.showImGui = true;
		}
		if (Input.GetKeyDown(KeyCode.Comma))
		{
			DevToolUI.PingHoveredObject();
			this.showImGui = true;
		}
	}

	// Token: 0x06003972 RID: 14706 RVA: 0x0022103C File Offset: 0x0021F23C
	[CompilerGenerated]
	internal static void <UpdateConsumingGameInputs>g__OnInputEnterImGui|28_0()
	{
		UnityMouseCatcherUI.SetEnabled(true);
		GameInputManager inputManager = Global.GetInputManager();
		for (int i = 0; i < inputManager.GetControllerCount(); i++)
		{
			inputManager.GetController(i).HandleCancelInput();
		}
	}

	// Token: 0x06003973 RID: 14707 RVA: 0x000C528A File Offset: 0x000C348A
	[CompilerGenerated]
	internal static void <UpdateConsumingGameInputs>g__OnInputExitImGui|28_1()
	{
		UnityMouseCatcherUI.SetEnabled(false);
	}

	// Token: 0x04002716 RID: 10006
	public const string SHOW_DEVTOOLS = "ShowDevtools";

	// Token: 0x04002717 RID: 10007
	public static DevToolManager Instance;

	// Token: 0x04002718 RID: 10008
	private bool toggleKeyWasDown;

	// Token: 0x04002719 RID: 10009
	private bool showImGui;

	// Token: 0x0400271A RID: 10010
	private bool prevShowImGui;

	// Token: 0x0400271B RID: 10011
	private bool doesImGuiWantInput;

	// Token: 0x0400271C RID: 10012
	private bool prevDoesImGuiWantInput;

	// Token: 0x0400271D RID: 10013
	private bool showImguiState;

	// Token: 0x0400271E RID: 10014
	private bool showImguiDemo;

	// Token: 0x0400271F RID: 10015
	public bool UserAcceptedWarning;

	// Token: 0x04002720 RID: 10016
	private DevToolWarning warning = new DevToolWarning();

	// Token: 0x04002721 RID: 10017
	private DevToolMenuFontSize menuFontSize = new DevToolMenuFontSize();

	// Token: 0x04002722 RID: 10018
	public DevPanelList panels = new DevPanelList();

	// Token: 0x04002723 RID: 10019
	public DevToolMenuNodeList menuNodes = new DevToolMenuNodeList();

	// Token: 0x04002724 RID: 10020
	public Dictionary<Type, string> devToolNameDict = new Dictionary<Type, string>();

	// Token: 0x04002725 RID: 10021
	private HashSet<Type> dontAutomaticallyRegisterTypes = new HashSet<Type>();
}
