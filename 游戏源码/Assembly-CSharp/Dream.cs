using System;
using UnityEngine;

// Token: 0x0200124A RID: 4682
public class Dream : Resource
{
	// Token: 0x06005FEA RID: 24554 RVA: 0x002AC068 File Offset: 0x002AA268
	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names) : base(id, parent, null)
	{
		this.Icons = new Sprite[icons_sprite_names.Length];
		this.BackgroundAnim = background;
		for (int i = 0; i < icons_sprite_names.Length; i++)
		{
			this.Icons[i] = Assets.GetSprite(icons_sprite_names[i]);
		}
	}

	// Token: 0x06005FEB RID: 24555 RVA: 0x000DE7F0 File Offset: 0x000DC9F0
	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names, float durationPerImage) : this(id, parent, background, icons_sprite_names)
	{
		this.secondPerImage = durationPerImage;
	}

	// Token: 0x04004402 RID: 17410
	public string BackgroundAnim;

	// Token: 0x04004403 RID: 17411
	public Sprite[] Icons;

	// Token: 0x04004404 RID: 17412
	public float secondPerImage = 2.4f;
}
