using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ImGuiNET;
using UnityEngine;
using UnityEngine.EventSystems;

public class DevToolEntity_EyeDrop : DevTool
{
		public DevToolEntity_EyeDrop(Action<DevToolEntityTarget> onSelectionMadeFn, Func<DevToolEntityTarget, Option<string>> getErrorForCandidateTargetFn = null)
	{
		this.onSelectionMadeFn = onSelectionMadeFn;
		this.getErrorForCandidateTargetFn = getErrorForCandidateTargetFn;
	}

		protected override void RenderTo(DevPanel panel)
	{
		if (this.requestingNavBack)
		{
			this.requestingNavBack = false;
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
		DevToolEntity_EyeDrop.ImGuiInput_SampleScreenPosition(ref this.sampleAtScreenPosition);
		using (ListPool<DevToolEntityTarget, DevToolEntity_EyeDrop>.PooledList pooledList = PoolsFor<DevToolEntity_EyeDrop>.AllocateList<DevToolEntityTarget>())
		{
			Option<string> error = DevToolEntity_EyeDrop.CollectUIGameObjectHitsTo(pooledList, this.sampleAtScreenPosition);
			Option<string> error2 = DevToolEntity_EyeDrop.CollectWorldGameObjectHitsTo(pooledList, this.sampleAtScreenPosition);
			ValueTuple<Option<DevToolEntityTarget.ForSimCell>, Option<string>> simCellAt = DevToolEntity_EyeDrop.GetSimCellAt(this.sampleAtScreenPosition);
			Option<DevToolEntityTarget.ForSimCell> item = simCellAt.Item1;
			Option<string> item2 = simCellAt.Item2;
			if (item.IsSome())
			{
				pooledList.Add(item.Unwrap());
			}
			if (ImGui.TreeNode("Debug Info"))
			{
				DevToolEntity_EyeDrop.<RenderTo>g__DrawBullet|5_0("[UI GameObjects]", error);
				DevToolEntity_EyeDrop.<RenderTo>g__DrawBullet|5_0("[World GameObjects]", error2);
				DevToolEntity_EyeDrop.<RenderTo>g__DrawBullet|5_0("[Sim Cell]", item2);
				ImGui.TreePop();
			}
			ImGui.Separator();
			foreach (DevToolEntityTarget devToolEntityTarget in pooledList)
			{
				Option<string> option = (this.getErrorForCandidateTargetFn == null) ? Option.None : this.getErrorForCandidateTargetFn(devToolEntityTarget);
				Option<ValueTuple<Vector2, Vector2>> screenRect = devToolEntityTarget.GetScreenRect();
				bool flag = ImGuiEx.Button("Pick target \"" + devToolEntityTarget.GetDebugName() + "\"", option.IsNone());
				bool flag2 = ImGui.IsItemHovered();
				if (flag2)
				{
					ImGui.BeginTooltip();
					if (option.IsSome())
					{
						ImGui.Text("Error:");
						ImGui.Text(option.Unwrap());
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
				if (flag)
				{
					this.onSelectionMadeFn(devToolEntityTarget);
					this.requestingNavBack = true;
				}
				if (screenRect.IsSome())
				{
					DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), devToolEntityTarget.GetDebugName(), flag2);
				}
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
			foreach (RaycastResult raycastResult in pooledList)
			{
				if (!(raycastResult.gameObject.name == "ImGui Consume Input"))
				{
					targets.Add(new DevToolEntityTarget.ForUIGameObject(raycastResult.gameObject));
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
		ValueTuple<Option<DevToolEntityTarget.ForSimCell>, Option<string>> simCellAt = DevToolEntity_EyeDrop.GetSimCellAt(screenPosition);
		Option<DevToolEntityTarget.ForSimCell> item = simCellAt.Item1;
		Option<string> item2 = simCellAt.Item2;
		if (item2.IsSome())
		{
			return item2;
		}
		if (item.IsNone())
		{
			return "Couldn't find sim cell";
		}
		DevToolEntityTarget.ForSimCell forSimCell = item.Unwrap();
		Vector2 pos = main.ScreenToWorldPoint(screenPosition);
		using (ListPool<InterfaceTool.Intersection, DevToolEntity_EyeDrop>.PooledList pooledList = PoolsFor<DevToolEntity_EyeDrop>.AllocateList<InterfaceTool.Intersection>())
		{
			using (ListPool<ScenePartitionerEntry, DevToolEntity_EyeDrop>.PooledList pooledList2 = PoolsFor<DevToolEntity_EyeDrop>.AllocateList<ScenePartitionerEntry>())
			{
				int x_bottomLeft;
				int y_bottomLeft;
				Grid.CellToXY(forSimCell.cellIndex, out x_bottomLeft, out y_bottomLeft);
				Game.Instance.statusItemRenderer.GetIntersections(pos, pooledList);
				GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, GameScenePartitioner.Instance.collisionLayer, pooledList2);
				foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList2)
				{
					KCollider2D kcollider2D = scenePartitionerEntry.obj as KCollider2D;
					if (!kcollider2D.IsNullOrDestroyed() && kcollider2D.Intersects(pos) && !(kcollider2D.gameObject.name == "WorldSelectionCollider"))
					{
						targets.Add(new DevToolEntityTarget.ForWorldGameObject(kcollider2D.gameObject));
					}
				}
			}
		}
		return Option.None;
	}

		[return: TupleElementNames(new string[]
	{
		"target",
		"error"
	})]
	public static ValueTuple<Option<DevToolEntityTarget.ForSimCell>, Option<string>> GetSimCellAt(Vector3 screenPosition)
	{
		if (Game.Instance == null)
		{
			return new ValueTuple<Option<DevToolEntityTarget.ForSimCell>, Option<string>>(Option.None, "No Game instance found.");
		}
		Camera main = Camera.main;
		if (main.IsNullOrDestroyed())
		{
			return new ValueTuple<Option<DevToolEntityTarget.ForSimCell>, Option<string>>(Option.None, "No Main Camera found.");
		}
		Ray ray = main.ScreenPointToRay(screenPosition);
		float distance;
		if (!new Plane(new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, 1f)).Raycast(ray, out distance))
		{
			return new ValueTuple<Option<DevToolEntityTarget.ForSimCell>, Option<string>>(Option.None, "Ray from camera did not hit game plane.");
		}
		int num = Grid.PosToCell(ray.GetPoint(distance));
		if (num < 0 || Grid.CellCount <= num)
		{
			return new ValueTuple<Option<DevToolEntityTarget.ForSimCell>, Option<string>>(Option.None, string.Format("Found cell index {0} is out of range {1}..{2}", num, num, Grid.CellCount));
		}
		if (!Grid.IsValidCell(num))
		{
			return new ValueTuple<Option<DevToolEntityTarget.ForSimCell>, Option<string>>(Option.None, string.Format("Cell index {0} is invalid", num));
		}
		return new ValueTuple<Option<DevToolEntityTarget.ForSimCell>, Option<string>>(new DevToolEntityTarget.ForSimCell(num), Option.None);
	}

		public static void ImGuiInput_SampleScreenPosition(ref Vector2 unityScreenPosition)
	{
		float num = 4f;
		float num2 = 12f;
		float num3 = 4f;
		float num4 = 6f;
		float num5 = 2f;
		float num6 = num + num2 + num3;
		float d = num + num2 + num3 + num5 + num4;
		float rounding = num + 4f;
		Vector2 b = Vector2.one * num * 2f;
		Vector2 vector = Vector2.one * num6 * 2f;
		Vector2 b2 = Vector2.one * (num6 + num4) * 2f;
		Vector2 vector2 = Vector2.one * d * 2f;
		ImGuiWindowFlags flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoBackground | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.HorizontalScrollbar;
		Vector2 mousePos = ImGui.GetMousePos();
		Vector2 a = DevToolEntity_EyeDrop.posSampler_rectBasePos;
		if (DevToolEntity_EyeDrop.posSampler_dragStartPos.IsSome())
		{
			a += mousePos - DevToolEntity_EyeDrop.posSampler_dragStartPos.Unwrap();
		}
		ImGui.SetNextWindowPos(a - vector2 / 2f);
		ImGui.SetNextWindowSizeConstraints(Vector2.one, Vector2.one * -1f);
		ImGui.SetNextWindowSize(vector2);
		ImGui.PushStyleVar(ImGuiStyleVar.WindowMinSize, Vector2.zero);
		ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.zero);
		if (ImGui.Begin("###ID_EyeDropper", flags))
		{
			bool flag = ImGui.IsWindowHovered();
			bool flag2 = ImGui.IsWindowHovered() && ImGui.IsMouseDown(ImGuiMouseButton.Left);
			Color c;
			if (flag2)
			{
				c = Util.ColorFromHex("C5153B");
			}
			else if (flag)
			{
				c = Util.ColorFromHex("F498AC");
			}
			else
			{
				c = Util.ColorFromHex("EC4F71");
			}
			if (flag2 && DevToolEntity_EyeDrop.posSampler_dragStartPos.IsNone())
			{
				DevToolEntity_EyeDrop.posSampler_dragStartPos = mousePos;
			}
			if (ImGui.IsMouseReleased(ImGuiMouseButton.Left) && DevToolEntity_EyeDrop.posSampler_dragStartPos.IsSome())
			{
				DevToolEntity_EyeDrop.posSampler_rectBasePos += mousePos - DevToolEntity_EyeDrop.posSampler_dragStartPos.Unwrap();
				DevToolEntity_EyeDrop.posSampler_dragStartPos = Option.None;
			}
			ImDrawListPtr windowDrawList = ImGui.GetWindowDrawList();
			Vector2 vector3 = ImGui.GetCursorScreenPos() + Vector2.one * num5;
			Vector2 b3 = Vector2.one * num4;
			Vector2 vector4 = (vector - b) / 2f + b3;
			unityScreenPosition = new Vector2(vector3.x + vector4.x + num, -(vector3.y + vector4.y + num) + (float)Screen.height);
			windowDrawList.AddRectFilled(vector3, vector3 + b2, ImGui.GetColorU32(new Vector4(0f, 0f, 0f, 0.7f)), rounding);
			windowDrawList.AddRectFilled(vector3 + vector4, vector3 + vector4 + b, ImGui.GetColorU32(c), rounding);
			windowDrawList.AddRect(vector3 + b3, vector3 + b3 + vector, ImGui.GetColorU32(c), rounding, ImDrawFlags.None, num3);
			ImGui.End();
		}
		ImGui.PopStyleVar(2);
	}

		[CompilerGenerated]
	internal static void <RenderTo>g__DrawBullet|5_0(string groupName, Option<string> error)
	{
		ImGui.Bullet();
		ImGui.Text(groupName);
		ImGui.SameLine();
		if (error.IsSome())
		{
			ImGui.Text("[ERROR]");
			ImGui.SameLine();
			ImGui.Text(error.Unwrap());
			return;
		}
		ImGui.Text("No errors.");
	}

		private Vector2 sampleAtScreenPosition;

		private Action<DevToolEntityTarget> onSelectionMadeFn;

		private Func<DevToolEntityTarget, Option<string>> getErrorForCandidateTargetFn;

		private bool requestingNavBack;

		private static Vector2 posSampler_rectBasePos = new Vector2(200f, 200f);

		private static Option<Vector2> posSampler_dragStartPos = Option.None;
}
