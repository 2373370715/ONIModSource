using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000BDB RID: 3035
public class DevToolUI : DevTool
{
	// Token: 0x06003A14 RID: 14868 RVA: 0x000C5823 File Offset: 0x000C3A23
	protected override void RenderTo(DevPanel panel)
	{
		this.RepopulateRaycastHits();
		this.DrawPingObject();
		this.DrawRaycastHits();
	}

	// Token: 0x06003A15 RID: 14869 RVA: 0x00226230 File Offset: 0x00224430
	private void DrawPingObject()
	{
		if (this.m_last_pinged_hit != null)
		{
			GameObject gameObject = this.m_last_pinged_hit.Value.gameObject;
			if (gameObject != null && gameObject)
			{
				ImGui.Text("Last Pinged: \"" + DevToolUI.GetQualifiedName(gameObject) + "\"");
				ImGui.SameLine();
				if (ImGui.Button("Inspect"))
				{
					DevToolSceneInspector.Inspect(gameObject);
				}
				ImGui.Spacing();
				ImGui.Spacing();
			}
			else
			{
				this.m_last_pinged_hit = null;
			}
		}
		ImGui.Text("Press \",\" to ping the top hovered ui object");
		ImGui.Spacing();
		ImGui.Spacing();
	}

	// Token: 0x06003A16 RID: 14870 RVA: 0x000C5837 File Offset: 0x000C3A37
	private void Internal_Ping(RaycastResult raycastResult)
	{
		GameObject gameObject = raycastResult.gameObject;
		this.m_last_pinged_hit = new RaycastResult?(raycastResult);
	}

	// Token: 0x06003A17 RID: 14871 RVA: 0x002262D0 File Offset: 0x002244D0
	public static void PingHoveredObject()
	{
		using (ListPool<RaycastResult, DevToolUI>.PooledList pooledList = PoolsFor<DevToolUI>.AllocateList<RaycastResult>())
		{
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			if (!(current == null) && current)
			{
				current.RaycastAll(new PointerEventData(current)
				{
					position = Input.mousePosition
				}, pooledList);
				DevToolUI devToolUI = DevToolManager.Instance.panels.AddOrGetDevTool<DevToolUI>();
				if (pooledList.Count > 0)
				{
					devToolUI.Internal_Ping(pooledList[0]);
				}
			}
		}
	}

	// Token: 0x06003A18 RID: 14872 RVA: 0x0022635C File Offset: 0x0022455C
	private void DrawRaycastHits()
	{
		if (this.m_raycast_hits.Count <= 0)
		{
			ImGui.Text("Didn't hit any ui");
			return;
		}
		ImGui.Text("Raycast Hits:");
		ImGui.Indent();
		for (int i = 0; i < this.m_raycast_hits.Count; i++)
		{
			RaycastResult raycastResult = this.m_raycast_hits[i];
			ImGui.BulletText(string.Format("[{0}] {1}", i, DevToolUI.GetQualifiedName(raycastResult.gameObject)));
		}
		ImGui.Unindent();
	}

	// Token: 0x06003A19 RID: 14873 RVA: 0x002263DC File Offset: 0x002245DC
	private void RepopulateRaycastHits()
	{
		this.m_raycast_hits.Clear();
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current == null || !current)
		{
			return;
		}
		current.RaycastAll(new PointerEventData(current)
		{
			position = Input.mousePosition
		}, this.m_raycast_hits);
	}

	// Token: 0x06003A1A RID: 14874 RVA: 0x00226430 File Offset: 0x00224630
	private static string GetQualifiedName(GameObject game_object)
	{
		KScreen componentInParent = game_object.GetComponentInParent<KScreen>();
		if (componentInParent != null)
		{
			return componentInParent.gameObject.name + " :: " + game_object.name;
		}
		return game_object.name ?? "";
	}

	// Token: 0x040027A3 RID: 10147
	private List<RaycastResult> m_raycast_hits = new List<RaycastResult>();

	// Token: 0x040027A4 RID: 10148
	private RaycastResult? m_last_pinged_hit;
}
