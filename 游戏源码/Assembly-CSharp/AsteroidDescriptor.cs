using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001C00 RID: 7168
public struct AsteroidDescriptor
{
	// Token: 0x060094E6 RID: 38118 RVA: 0x00100F47 File Offset: 0x000FF147
	public AsteroidDescriptor(string text, string tooltip, Color associatedColor, List<global::Tuple<string, Color, float>> bands = null, string associatedIcon = null)
	{
		this.text = text;
		this.tooltip = tooltip;
		this.associatedColor = associatedColor;
		this.bands = bands;
		this.associatedIcon = associatedIcon;
	}

	// Token: 0x04007364 RID: 29540
	public string text;

	// Token: 0x04007365 RID: 29541
	public string tooltip;

	// Token: 0x04007366 RID: 29542
	public List<global::Tuple<string, Color, float>> bands;

	// Token: 0x04007367 RID: 29543
	public Color associatedColor;

	// Token: 0x04007368 RID: 29544
	public string associatedIcon;
}
