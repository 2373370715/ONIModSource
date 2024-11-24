using ImGuiNET;
using STRINGS;
using UnityEngine;

public class DevToolEntity : DevTool
{
	private Option<DevToolEntityTarget> currentTargetOpt;

	private bool shouldDrawBoundingBox = true;

	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.MenuItem("New Window"))
			{
				DevToolUtil.Open(new DevToolEntity());
			}
			ImGui.EndMenuBar();
		}
		ImGui.Text(currentTargetOpt.IsNone() ? "Pick target:" : "Change target:");
		ImGui.SameLine();
		if (ImGui.Button("Eyedrop"))
		{
			panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget result)
			{
				currentTargetOpt = result;
			}));
		}
		ImGui.SameLine();
		if (ImGui.Button("Search GameObjects (NOT implemented)"))
		{
			panel.PushDevTool(new DevToolEntity_SearchGameObjects(delegate(DevToolEntityTarget result)
			{
				currentTargetOpt = result;
			}));
		}
		if (GetInGameSelectedEntity().IsSome())
		{
			ImGui.SameLine();
			if (ImGui.Button("\"" + GetInGameSelectedEntity().Unwrap().name + "\""))
			{
				currentTargetOpt = new DevToolEntityTarget.ForWorldGameObject(GetInGameSelectedEntity().Unwrap());
			}
		}
		ImGui.Separator();
		ImGui.Spacing();
		if (currentTargetOpt.IsNone())
		{
			Name = "Entity";
			ImGui.Text("<nothing selected>");
		}
		else
		{
			Name = "Entity: " + currentTargetOpt.Unwrap().ToString();
			Name = "EntityType: " + currentTargetOpt.Unwrap().GetType().FullName.Substring("For".Length);
			ImGuiEx.SimpleField("Entity Name", currentTargetOpt.Unwrap().ToString());
		}
		ImGui.Spacing();
		ImGui.Separator();
		ImGui.Spacing();
		if (currentTargetOpt.IsNone())
		{
			return;
		}
		DevToolEntityTarget devToolEntityTarget = currentTargetOpt.Unwrap();
		Option<GameObject> option = ((!(devToolEntityTarget is DevToolEntityTarget.ForUIGameObject forUIGameObject)) ? ((!(devToolEntityTarget is DevToolEntityTarget.ForWorldGameObject forWorldGameObject)) ? ((Option<GameObject>)Option.None) : ((Option<GameObject>)forWorldGameObject.gameObject)) : ((Option<GameObject>)forUIGameObject.gameObject));
		if (ImGui.CollapsingHeader("Actions", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGui.Indent();
			ImGui.Checkbox("Draw Bounding Box", ref shouldDrawBoundingBox);
			if (option.IsSome())
			{
				GameObject gameObject = option.Unwrap();
				if (ImGui.Button($"Inspect GameObject in DevTools###ID_InspectInGame_{gameObject.GetInstanceID()}"))
				{
					DevToolSceneInspector.Inspect(gameObject);
				}
				WildnessMonitor.Instance sMI = gameObject.GetSMI<WildnessMonitor.Instance>();
				if (sMI.IsNullOrDestroyed())
				{
					ImGuiEx.Button("Taming: Covert to Tamed", "No WildnessMonitor.Instance found on the selected GameObject");
				}
				else
				{
					WildnessMonitor wildnessMonitor = (WildnessMonitor)sMI.GetStateMachine();
					if (sMI.GetCurrentState() != wildnessMonitor.tame)
					{
						if (ImGui.Button("Taming: Convert to Tamed"))
						{
							sMI.wildness.SetValue(0f);
							sMI.GoTo(wildnessMonitor.tame);
						}
					}
					else if (ImGui.Button("Taming: Convert to Untamed"))
					{
						sMI.wildness.value = sMI.wildness.GetMax();
						sMI.GoTo(wildnessMonitor.wild);
					}
				}
			}
			ImGui.Unindent();
		}
		ImGui.Spacing();
		if (ImGui.CollapsingHeader("Related DevTools", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGui.Indent();
			if (ImGuiEx.Button("Debug Status Items", DevToolStatusItems.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolStatusItems((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug Cavity", DevToolCavity.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolCavity((DevToolEntityTarget.ForSimCell)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug GoTo", DevToolEntity_DebugGoTo.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolEntity_DebugGoTo((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug RanchStation", DevToolEntity_RanchStation.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolEntity_RanchStation((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			ImGui.Unindent();
		}
		if (shouldDrawBoundingBox)
		{
			Option<(Vector2, Vector2)> screenRect = devToolEntityTarget.GetScreenRect();
			if (screenRect.IsSome())
			{
				DrawBoundingBox(screenRect.Unwrap(), devToolEntityTarget.GetDebugName(), ImGui.IsWindowFocused());
			}
		}
	}

	public Option<GameObject> GetInGameSelectedEntity()
	{
		if (SelectTool.Instance == null)
		{
			return Option.None;
		}
		KSelectable selected = SelectTool.Instance.selected;
		if (selected.IsNullOrDestroyed())
		{
			return Option.None;
		}
		return selected.gameObject;
	}

	public static string GetNameFor(GameObject gameObject)
	{
		if (gameObject.IsNullOrDestroyed())
		{
			return "<null or destroyed GameObject>";
		}
		return "\"" + UI.StripLinkFormatting(gameObject.name) + "\" [0x" + gameObject.GetInstanceID().ToString("X") + "]";
	}

	public static Vector2 GetPositionFor(GameObject gameObject)
	{
		if (Camera.main != null)
		{
			Camera main = Camera.main;
			Vector2 result = main.WorldToScreenPoint(gameObject.transform.position);
			result.y = (float)main.pixelHeight - result.y;
			return result;
		}
		return Vector2.zero;
	}

	public static Vector2 GetScreenPosition(Vector3 pos)
	{
		if (Camera.main != null)
		{
			Camera main = Camera.main;
			Vector2 result = main.WorldToScreenPoint(pos);
			result.y = (float)main.pixelHeight - result.y;
			return result;
		}
		return Vector2.zero;
	}

	public static void DrawBoundingBox((Vector2 cornerA, Vector2 cornerB) screenRect, string name, bool isFocused)
	{
		if (isFocused)
		{
			DrawScreenRect(screenRect, name, new Color(1f, 0f, 0f, 1f), new Color(1f, 0f, 0f, 0.3f));
		}
		else
		{
			DrawScreenRect(screenRect, Option.None, new Color(0.9f, 0f, 0f, 0.6f));
		}
	}

	public static void DrawScreenRect((Vector2 cornerA, Vector2 cornerB) screenRect, Option<string> text = default(Option<string>), Option<Color> outlineColor = default(Option<Color>), Option<Color> fillColor = default(Option<Color>), Option<DevToolUtil.TextAlignment> alignment = default(Option<DevToolUtil.TextAlignment>))
	{
		Vector2 vector = Vector2.Min(screenRect.cornerA, screenRect.cornerB);
		Vector2 vector2 = Vector2.Max(screenRect.cornerA, screenRect.cornerB);
		ImGui.GetBackgroundDrawList().AddRect(vector, vector2, ImGui.GetColorU32(outlineColor.UnwrapOr(Color.red)), 0f, ImDrawFlags.None, 4f);
		ImGui.GetBackgroundDrawList().AddRectFilled(vector, vector2, ImGui.GetColorU32(fillColor.UnwrapOr(Color.clear)));
		float font_size = 30f;
		if (!text.IsSome())
		{
			return;
		}
		Vector2 pos = new Vector2(vector2.x, vector.y) + new Vector2(15f, 0f);
		if (alignment.HasValue)
		{
			font_size = ImGui.GetFont().FontSize;
			Vector2 vector3 = ImGui.CalcTextSize(text.Unwrap());
			if (alignment == DevToolUtil.TextAlignment.Center)
			{
				Vector2 vector4 = vector2 - vector;
				pos.x = vector.x + (vector4.x - vector3.x) * 0.5f;
				pos.y = vector.y + (vector4.y - vector3.y) * 0.5f;
			}
		}
		ImGui.GetBackgroundDrawList().AddText(ImGui.GetFont(), font_size, pos, ImGui.GetColorU32(Color.white), text.Unwrap());
	}
}
