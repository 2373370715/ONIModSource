using System;
using UnityEngine;

// Token: 0x02001E8C RID: 7820
public class LegendEntry
{
	// Token: 0x0600A3FE RID: 41982 RVA: 0x003E42A0 File Offset: 0x003E24A0
	public LegendEntry(string name, string desc, Color colour, string desc_arg = null, Sprite sprite = null, bool displaySprite = true)
	{
		this.name = name;
		this.desc = desc;
		this.colour = colour;
		this.desc_arg = desc_arg;
		this.sprite = ((sprite == null) ? Assets.instance.LegendColourBox : sprite);
		this.displaySprite = displaySprite;
	}

	// Token: 0x0400801E RID: 32798
	public string name;

	// Token: 0x0400801F RID: 32799
	public string desc;

	// Token: 0x04008020 RID: 32800
	public string desc_arg;

	// Token: 0x04008021 RID: 32801
	public Color colour;

	// Token: 0x04008022 RID: 32802
	public Sprite sprite;

	// Token: 0x04008023 RID: 32803
	public bool displaySprite;
}
