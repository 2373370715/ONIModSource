using System.Collections.Generic;
using Database;
using STRINGS;
using UnityEngine;

public class SandboxStoryTraitToolHoverTextCard : HoverTextConfiguration {
    public override void UpdateHoverElements(List<KSelectable> hoverObjects) {
        var instance        = HoverTextScreen.Instance;
        var hoverTextDrawer = instance.BeginDrawing();
        var num             = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
        if (!Grid.IsValidCell(num) || Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId) {
            hoverTextDrawer.EndDrawing();
            return;
        }

        hoverTextDrawer.BeginShadowBar();
        Story             story;
        TemplateContainer templateContainer;
        var error = GetComponent<SandboxStoryTraitTool>()
            .GetError(PlayerController.GetCursorPos(KInputManager.GetMousePos()), out story, out templateContainer);

        if (story == null)
            ToolName = UI.TOOLS.SANDBOX.SPAWN_STORY_TRAIT.NAME;
        else
            ToolName = Strings.Get(story.StoryTrait.name);

        DrawTitle(instance, hoverTextDrawer);
        DrawInstructions(instance, hoverTextDrawer);
        if (error != null) {
            hoverTextDrawer.NewLine();
            hoverTextDrawer.AddIndent(8);
            hoverTextDrawer.DrawText(error, HoverTextStyleSettings[1]);
        }

        hoverTextDrawer.EndShadowBar();
        hoverTextDrawer.EndDrawing();
    }
}