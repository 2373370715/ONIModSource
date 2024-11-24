using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;

// Token: 0x02001B3E RID: 6974
public class SandboxStoryTraitToolHoverTextCard : HoverTextConfiguration
{
	// Token: 0x06009264 RID: 37476 RVA: 0x00386B34 File Offset: 0x00384D34
	public override void UpdateHoverElements(List<KSelectable> hoverObjects)
	{
		HoverTextScreen instance = HoverTextScreen.Instance;
		HoverTextDrawer hoverTextDrawer = instance.BeginDrawing();
		int num = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
		if (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId)
		{
			hoverTextDrawer.EndDrawing();
			return;
		}
		hoverTextDrawer.BeginShadowBar(false);
		Story story;
		TemplateContainer templateContainer;
		string error = base.GetComponent<SandboxStoryTraitTool>().GetError(PlayerController.GetCursorPos(KInputManager.GetMousePos()), out story, out templateContainer);
		if (story == null)
		{
			this.ToolName = UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.NAME;
		}
		else
		{
			this.ToolName = Strings.Get(story.StoryTrait.name);
		}
		base.DrawTitle(instance, hoverTextDrawer);
		base.DrawInstructions(instance, hoverTextDrawer);
		if (error != null)
		{
			hoverTextDrawer.NewLine(26);
			hoverTextDrawer.AddIndent(8);
			hoverTextDrawer.DrawText(error, this.HoverTextStyleSettings[1]);
		}
		hoverTextDrawer.EndShadowBar();
		hoverTextDrawer.EndDrawing();
	}
}
