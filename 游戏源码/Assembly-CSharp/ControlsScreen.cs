using System;
using UnityEngine.UI;

// Token: 0x02001C77 RID: 7287
public class ControlsScreen : KScreen
{
	// Token: 0x06009800 RID: 38912 RVA: 0x003AE6B8 File Offset: 0x003AC8B8
	protected override void OnPrefabInit()
	{
		BindingEntry[] bindingEntries = GameInputMapping.GetBindingEntries();
		string text = "";
		foreach (BindingEntry bindingEntry in bindingEntries)
		{
			text += bindingEntry.mAction.ToString();
			text += ": ";
			text += bindingEntry.mKeyCode.ToString();
			text += "\n";
		}
		this.controlLabel.text = text;
	}

	// Token: 0x06009801 RID: 38913 RVA: 0x00102D27 File Offset: 0x00100F27
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Help) || e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
	}

	// Token: 0x04007655 RID: 30293
	public Text controlLabel;
}
