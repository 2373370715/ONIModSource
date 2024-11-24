using UnityEngine;

public class Dream : Resource
{
	public string BackgroundAnim;

	public Sprite[] Icons;

	public float secondPerImage = 2.4f;

	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names)
		: base(id, parent)
	{
		Icons = new Sprite[icons_sprite_names.Length];
		BackgroundAnim = background;
		for (int i = 0; i < icons_sprite_names.Length; i++)
		{
			Icons[i] = Assets.GetSprite(icons_sprite_names[i]);
		}
	}

	public Dream(string id, ResourceSet parent, string background, string[] icons_sprite_names, float durationPerImage)
		: this(id, parent, background, icons_sprite_names)
	{
		secondPerImage = durationPerImage;
	}
}
