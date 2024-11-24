using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

public class DevToolBatchedAnimDebug : DevTool
{
	private GameObject Selection;

	private bool LockSelection;

	private string Filter = "";

	private int FrameIndex;

	public DevToolBatchedAnimDebug()
	{
		drawFlags = ImGuiWindowFlags.MenuBar;
	}

	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.BeginMenuBar())
		{
			ImGui.Checkbox("Lock selection", ref LockSelection);
			ImGui.EndMenuBar();
		}
		if (!LockSelection)
		{
			Selection = SelectTool.Instance?.selected?.gameObject;
		}
		if (Selection == null)
		{
			ImGui.Text("No selection.");
			return;
		}
		KBatchedAnimController component = Selection.GetComponent<KBatchedAnimController>();
		if (component == null)
		{
			ImGui.Text("No anim controller.");
			return;
		}
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(component.batchGroupID);
		SymbolOverrideController component2 = Selection.GetComponent<SymbolOverrideController>();
		ImGui.Text("Group: " + component.GetBatch().group.batchID.ToString() + ", Build: " + component.curBuild.name);
		if (!ImGui.BeginTabBar("##tabs", ImGuiTabBarFlags.None))
		{
			return;
		}
		if (ImGui.BeginTabItem("BatchGroup"))
		{
			KAnimBatchGroup group = component.GetBatch().group;
			ImGui.BeginChild("ScrollRegion", new Vector2(0f, 0f), border: true, ImGuiWindowFlags.None);
			ImGui.Text($"Group mesh.vertices.Count: {group.mesh.vertices.Count()}");
			ImGui.Text($"Group data.maxVisibleSymbols: {group.data.maxVisibleSymbols}");
			ImGui.Text($"Group maxGroupSize: {group.maxGroupSize}");
			ImGui.EndChild();
			ImGui.EndTabItem();
		}
		if (component2 != null && ImGui.BeginTabItem("SymbolOverrides"))
		{
			ImGui.InputText("Symbol Filter", ref Filter, 128u);
			int num = Hash.SDBMLower(Filter);
			ImGui.LabelText("Filter Hash", "0x" + num.ToString("X"));
			SymbolOverrideController.SymbolEntry[] getSymbolOverrides = component2.GetSymbolOverrides;
			ImGui.BeginChild("ScrollRegion", new Vector2(0f, 0f), border: true, ImGuiWindowFlags.None);
			for (int i = 0; i < getSymbolOverrides.Length; i++)
			{
				SymbolOverrideController.SymbolEntry symbolEntry = getSymbolOverrides[i];
				KAnim.Build.Symbol symbol = batchGroupData.GetSymbol(symbolEntry.targetSymbol);
				if (symbolEntry.targetSymbol.HashValue == num || symbolEntry.sourceSymbol.hash.HashValue == num || StringContains(symbolEntry.sourceSymbol.hash.ToString(), Filter) || StringContains(symbol.hash.ToString(), Filter))
				{
					ImGui.Text($"[{i}] source: {symbolEntry.sourceSymbol.hash}, {symbolEntry.sourceSymbol.build.name}, ({symbolEntry.sourceSymbol.build.GetTexture(0).name}), priority: {symbolEntry.priority}");
					ImGui.Text($"       firstFrameIdx = {symbolEntry.sourceSymbol.firstFrameIdx}, numFrames = {symbolEntry.sourceSymbol.numFrames}");
					if (symbol != null)
					{
						ImGui.Text($"   target: {symbol.hash}");
						ImGui.Text($"       firstFrameIdx = {symbol.firstFrameIdx}, numFrames = {symbol.numFrames}");
					}
					else
					{
						ImGui.Text($"   target: does not contain the symbol '{symbolEntry.sourceSymbol.hash}' to override");
					}
				}
			}
			ImGui.EndChild();
			ImGui.EndTabItem();
		}
		if (ImGui.BeginTabItem("Build Symbols"))
		{
			ImGui.InputText("Symbol Filter", ref Filter, 128u);
			int num2 = Hash.SDBMLower(Filter);
			ImGui.LabelText("Filter Hash", "0x" + num2.ToString("X"));
			ImGui.BeginChild("ScrollRegion", new Vector2(0f, 0f), border: true, ImGuiWindowFlags.None);
			KBatchGroupData data = component.GetBatch().group.data;
			for (int j = 0; j < data.GetSymbolCount(); j++)
			{
				KAnim.Build.Symbol symbol2 = data.GetSymbol(j);
				if (symbol2.hash.HashValue == num2 || StringContains(symbol2.hash.ToString(), Filter))
				{
					ImGui.Text($"[{symbol2.symbolIndexInSourceBuild}]: {symbol2.hash}");
				}
			}
			ImGui.EndChild();
			ImGui.EndTabItem();
		}
		if (ImGui.BeginTabItem("Anim Frame Data"))
		{
			ImGui.Text("Current anim: " + component.CurrentAnim.name);
			ImGui.Text("Current frame index: " + component.GetCurrentFrameIndex());
			ImGuiEx.InputIntRange("Frame Index", ref FrameIndex, 0, batchGroupData.GetAnimFrames().Count - 1);
			batchGroupData.TryGetFrame(FrameIndex, out var frame);
			ImGui.Text($"Frame [{FrameIndex}]: firstElementIdx= {frame.firstElementIdx} numElements= {frame.numElements}");
			ImGui.Text("Frame Elements: ");
			for (int k = 0; k < frame.numElements; k++)
			{
				KAnim.Anim.FrameElement frameElement = batchGroupData.GetFrameElement(frame.firstElementIdx + k);
				int symbolIndex = batchGroupData.GetSymbolIndex(frameElement.symbol);
				ImGui.Text($"FrameElement [{frame.firstElementIdx + k}]: symbolIdx= {symbolIndex} symbol= {frameElement.symbol}");
			}
			ImGui.EndTabItem();
		}
		if (ImGui.BeginTabItem("Texture atlases"))
		{
			ImGui.BeginChild("ScrollRegion", new Vector2(0f, 0f), border: true, ImGuiWindowFlags.None);
			List<Texture2D> list = new List<Texture2D>(component.GetBatch().atlases.GetTextures());
			int num3 = list.Count();
			if (component2 != null)
			{
				list.AddRange(component2.GetAtlasList().GetTextures());
			}
			for (int l = 0; l < list.Count; l++)
			{
				Texture2D texture2D = list[l];
				string text = ((l >= num3) ? "symbol override" : "base");
				ImGui.Text($"[{l}]: {texture2D.name}, [{texture2D.width},{texture2D.height}] ({text})");
				if (ImGui.IsItemHovered())
				{
					ImGui.BeginTooltip();
					ImGuiEx.Image(texture2D, new Vector2(texture2D.width, texture2D.height));
					ImGui.EndTooltip();
				}
			}
			ImGui.EndChild();
			ImGui.EndTabItem();
		}
		ImGui.EndTabBar();
	}

	private bool StringContains(string target, string query)
	{
		if (!(Filter == ""))
		{
			return target.IndexOf(query, 0, StringComparison.CurrentCultureIgnoreCase) != -1;
		}
		return true;
	}
}
