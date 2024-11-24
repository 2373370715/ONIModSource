using System;
using UnityEngine;

// Token: 0x02002064 RID: 8292
[Serializable]
public struct ToggleState
{
	// Token: 0x04008B63 RID: 35683
	public string Name;

	// Token: 0x04008B64 RID: 35684
	public string on_click_override_sound_path;

	// Token: 0x04008B65 RID: 35685
	public string on_release_override_sound_path;

	// Token: 0x04008B66 RID: 35686
	public string sound_parameter_name;

	// Token: 0x04008B67 RID: 35687
	public float sound_parameter_value;

	// Token: 0x04008B68 RID: 35688
	public bool has_sound_parameter;

	// Token: 0x04008B69 RID: 35689
	public Sprite sprite;

	// Token: 0x04008B6A RID: 35690
	public Color color;

	// Token: 0x04008B6B RID: 35691
	public Color color_on_hover;

	// Token: 0x04008B6C RID: 35692
	public bool use_color_on_hover;

	// Token: 0x04008B6D RID: 35693
	public bool use_rect_margins;

	// Token: 0x04008B6E RID: 35694
	public Vector2 rect_margins;

	// Token: 0x04008B6F RID: 35695
	public StatePresentationSetting[] additional_display_settings;
}
