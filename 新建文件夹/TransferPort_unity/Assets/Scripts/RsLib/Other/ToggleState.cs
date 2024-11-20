using System;
using UnityEngine;

// Token: 0x02000C61 RID: 3169
[Serializable]
public struct ToggleState
{
	// Token: 0x04004548 RID: 17736
	public string Name;

	// Token: 0x04004549 RID: 17737
	public string on_click_override_sound_path;

	// Token: 0x0400454A RID: 17738
	public string on_release_override_sound_path;

	// Token: 0x0400454B RID: 17739
	public string sound_parameter_name;

	// Token: 0x0400454C RID: 17740
	public float sound_parameter_value;

	// Token: 0x0400454D RID: 17741
	public bool has_sound_parameter;

	// Token: 0x0400454E RID: 17742
	public Sprite sprite;

	// Token: 0x0400454F RID: 17743
	public Color color;

	// Token: 0x04004550 RID: 17744
	public Color color_on_hover;

	// Token: 0x04004551 RID: 17745
	public bool use_color_on_hover;

	// Token: 0x04004552 RID: 17746
	public bool use_rect_margins;

	// Token: 0x04004553 RID: 17747
	public Vector2 rect_margins;

	// Token: 0x04004554 RID: 17748
	public StatePresentationSetting[] additional_display_settings;
}
