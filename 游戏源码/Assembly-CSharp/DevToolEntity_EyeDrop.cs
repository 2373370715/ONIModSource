using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;
using UnityEngine.EventSystems;

public class DevToolEntity_EyeDrop : DevTool
{
	private Vector2 sampleAtScreenPosition;

	private Action<DevToolEntityTarget> onSelectionMadeFn;

	private Func<DevToolEntityTarget, Option<string>> getErrorForCandidateTargetFn;

	private bool requestingNavBack;

	private static Vector2 posSampler_rectBasePos = new Vector2(200f, 200f);

	private static Option<Vector2> posSampler_dragStartPos = Option.None;

	public DevToolEntity_EyeDrop(Action<DevToolEntityTarget> onSelectionMadeFn, Func<DevToolEntityTarget, Option<string>> getErrorForCandidateTargetFn = null)
	{
		this.onSelectionMadeFn = onSelectionMadeFn;
		this.getErrorForCandidateTargetFn = getErrorForCandidateTargetFn;
	}

	protected override void RenderTo(DevPanel panel)
	{
		if (requestingNavBack)
		{
			requestingNavBack = false;
			panel.NavGoBack();
			return;
		}
		if (ImGuiEx.BeginHelpMarker())
		{
			ImGui.TextWrapped("This will do a raycast check against:");
			ImGui.Bullet();
			ImGui.SameLine();
			ImGui.TextWrapped("world gameobjects that have a KCollider2D component");
			ImGui.Bullet();
			ImGui.SameLine();
			ImGui.TextWrapped("ui gameobjects with a Graphic component that also have `raycastTarget` set to true");
			ImGui.Bullet();
			ImGui.SameLine();
			ImGui.TextWrapped("world sim cells");
			ImGui.TextWrapped("This means that some gameobjects that can be seen will not show up here.");
			ImGuiEx.EndHelpMarker();
		}
		ImGui.Separator();
		ImGuiInput_SampleScreenPosition(ref sampleAtScreenPosition);
		using (ListPool<DevToolEntityTarget, DevToolEntity_EyeDrop>.PooledList pooledList = PoolsFor<DevToolEntity_EyeDrop>.AllocateList<DevToolEntityTarget>())
		{
			Option<string> error2 = CollectUIGameObjectHitsTo(pooledList, sampleAtScreenPosition);
			Option<string> error3 = CollectWorldGameObjectHitsTo(pooledList, sampleAtScreenPosition);
			var (option, error4) = GetSimCellAt(sampleAtScreenPosition);
			if (option.IsSome())
			{
				pooledList.Add(option.Unwrap());
			}
			if (ImGui.TreeNode("Debug Info"))
			{
				DrawBullet("[UI GameObjects]", error2);
				DrawBullet("[World GameObjects]", error3);
				DrawBullet("[Sim Cell]", error4);
				ImGui.TreePop();
			}
			ImGui.Separator();
			foreach (DevToolEntityTarget item in pooledList)
			{
				Option<string> option2 = ((getErrorForCandidateTargetFn == null) ? ((Option<string>)Option.None) : getErrorForCandidateTargetFn(item));
				Option<(Vector2, Vector2)> screenRect = item.GetScreenRect();
				bool num = ImGuiEx.Button("Pick target \"" + item.GetDebugName() + "\"", option2.IsNone());
				bool flag = ImGui.IsItemHovered();
				if (flag)
				{
					ImGui.BeginTooltip();
					if (option2.IsSome())
					{
						ImGui.Text("Error:");
						ImGui.Text(option2.Unwrap());
						if (screenRect.IsSome())
						{
							ImGui.Separator();
							ImGui.Separator();
						}
					}
					if (screenRect.IsNone())
					{
						ImGui.Text("Error: Couldn't get screen rect to display.");
					}
					ImGui.EndTooltip();
				}
				if (num)
				{
					onSelectionMadeFn(item);
					requestingNavBack = true;
				}
				if (screenRect.IsSome())
				{
					DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), item.GetDebugName(), flag);
				}
			}
		}
		static void DrawBullet(string groupName, Option<string> error)
		{
			ImGui.Bullet();
			ImGui.Text(groupName);
			ImGui.SameLine();
			if (error.IsSome())
			{
				ImGui.Text("[ERROR]");
				ImGui.SameLine();
				ImGui.Text(error.Unwrap());
			}
			else
			{
				ImGui.Text("No errors.");
			}
		}
	}

	public static Option<string> CollectUIGameObjectHitsTo(IList<DevToolEntityTarget> targets, Vector3 screenPosition)
	{
		using (ListPool<RaycastResult, DevToolEntity_EyeDrop>.PooledList pooledList = PoolsFor<DevToolEntity_EyeDrop>.AllocateList<RaycastResult>())
		{
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			if (current.IsNullOrDestroyed())
			{
				return "No EventSystem found.";
			}
			current.RaycastAll(new PointerEventData(current)
			{
				position = screenPosition
			}, pooledList);
			foreach (RaycastResult item in pooledList)
			{
				if (!(item.gameObject.name == "ImGui Consume Input"))
				{
					targets.Add(new DevToolEntityTarget.ForUIGameObject(item.gameObject));
				}
			}
		}
		return Option.None;
	}

	public static Option<string> CollectWorldGameObjectHitsTo(IList<DevToolEntityTarget> targets, Vector3 screenPosition)
	{
		Camera main = Camera.main;
		if (main.IsNullOrDestroyed())
		{
			return "No Main Camera found.";
		}
		var (option, result) = GetSimCellAt(screenPosition);
		if (result.IsSome())
		{
			return result;
		}
		if (option.IsNone())
		{
			return "Couldn't find sim cell";
		}
		DevToolEntityTarget.ForSimCell forSimCell = option.Unwrap();
		Vector2 pos = main.ScreenToWorldPoint(screenPosition);
		using (ListPool<InterfaceTool.Intersection, DevToolEntity_EyeDrop>.PooledList intersections = PoolsFor<DevToolEntity_EyeDrop>.AllocateList<InterfaceTool.Intersection>())
		{
			using ListPool<ScenePartitionerEntry, DevToolEntity_EyeDrop>.PooledList pooledList = PoolsFor<DevToolEntity_EyeDrop>.AllocateList<ScenePartitionerEntry>();
			Grid.CellToXY(forSimCell.cellIndex, out var x, out var y);
			Game.Instance.statusItemRenderer.GetIntersections(pos, intersections);
			GameScenePartitioner.Instance.GatherEntries(x, y, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList);
			foreach (ScenePartitionerEntry item in pooledList)
			{
				KCollider2D kCollider2D = item.obj as KCollider2D;
				if (!kCollider2D.IsNullOrDestroyed() && kCollider2D.Intersects(pos) && !(kCollider2D.gameObject.name == "WorldSelectionCollider"))
				{
					targets.Add(new DevToolEntityTarget.ForWorldGameObject(kCollider2D.gameObject));
				}
			}
		}
		return Option.None;
	}

	public static (Option<DevToolEntityTarget.ForSimCell> target, Option<string> error) GetSimCellAt(Vector3 screenPosition)
	{
		if (Game.Instance == null)
		{
			return (target: Option.None, error: "No Game instance found.");
		}
		Camera main = Camera.main;
		if (main.IsNullOrDestroyed())
		{
			return (target: Option.None, error: "No Main Camera found.");
		}
		Ray ray = main.ScreenPointToRay(screenPosition);
		if (!new Plane(new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, 1f)).Raycast(ray, out var enter))
		{
			return (target: Option.None, error: "Ray from camera did not hit game plane.");
		}
		int num = Grid.PosToCell(ray.GetPoint(enter));
		if (num < 0 || Grid.CellCount <= num)
		{
			return (target: Option.None, error: $"Found cell index {num} is out of range {num}..{Grid.CellCount}");
		}
		if (!Grid.IsValidCell(num))
		{
			return (target: Option.None, error: $"Cell index {num} is invalid");
		}
		return (target: new DevToolEntityTarget.ForSimCell(num), error: Option.None);
	}

	public static void ImGuiInput_SampleScreenPosition(ref Vector2 unityScreenPosition)
	{
		float num = 4f;
		float num2 = 12f;
		float num3 = 4f;
		float num4 = 6f;
		float num5 = 2f;
		float num6 = num + num2 + num3;
		float num7 = num + num2 + num3 + num5 + num4;
		float rounding = num + 4f;
		Vector2 vector = Vector2.one * num * 2f;
		Vector2 vector2 = Vector2.one * num6 * 2f;
		Vector2 vector3 = Vector2.one * (num6 + num4) * 2f;
		Vector2 vector4 = Vector2.one * num7 * 2f;
		ImGuiWindowFlags flags = ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.HorizontalScrollbar;
		Vector2 mousePos = ImGui.GetMousePos();
		Vector2 vector5 = posSampler_rectBasePos;
		if (posSampler_dragStartPos.IsSome())
		{
			vector5 += mousePos - posSampler_dragStartPos.Unwrap();
		}
		ImGui.SetNextWindowPos(vector5 - vector4 / 2f);
		ImGui.SetNextWindowSizeConstraints(Vector2.one, Vector2.one * -1f);
		ImGui.SetNextWindowSize(vector4);
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Vector2.zero);
		ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.zero);
		int num8;
		Color color;
		if (ImGui.Begin("###ID_EyeDropper", flags))
		{
			bool flag = ImGui.IsWindowHovered();
			if (ImGui.IsWindowHovered())
			{
				num8 = (ImGui.IsMouseDown(ImGuiMouseButton.Left) ? 1 : 0);
				if (num8 != 0)
				{
					color = Util.ColorFromHex("C5153B");
					goto IL_017c;
				}
			}
			else
			{
				num8 = 0;
			}
			color = ((!flag) ? Util.ColorFromHex("EC4F71") : Util.ColorFromHex("F498AC"));
			goto IL_017c;
		}
		goto IL_02eb;
		IL_017c:
		if (num8 != 0 && posSampler_dragStartPos.IsNone())
		{
			posSampler_dragStartPos = mousePos;
		}
		if (ImGui.IsMouseReleased(ImGuiMouseButton.Left) && posSampler_dragStartPos.IsSome())
		{
			posSampler_rectBasePos += mousePos - posSampler_dragStartPos.Unwrap();
			posSampler_dragStartPos = Option.None;
		}
		ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
		Vector2 vector6 = ImGui.GetCursorScreenPos() + Vector2.one * num5;
		Vector2 vector7 = Vector2.one * num4;
		Vector2 vector8 = (vector2 - vector) / 2f + vector7;
		unityScreenPosition = new Vector2(vector6.x + vector8.x + num, 0f - (vector6.y + vector8.y + num) + (float)Screen.height);
		windowDrawList.AddRectFilled(vector6, vector6 + vector3, ImGui.GetColorU32(new Vector4(0f, 0f, 0f, 0.7f)), rounding);
		windowDrawList.AddRectFilled(vector6 + vector8, vector6 + vector8 + vector, ImGui.GetColorU32(color), rounding);
		windowDrawList.AddRect(vector6 + vector7, vector6 + vector7 + vector2, ImGui.GetColorU32(color), rounding, ImDrawFlags.None, num3);
		ImGui.End();
		goto IL_02eb;
		IL_02eb:
		ImGui.PopStyleVar(2);
	}
}
