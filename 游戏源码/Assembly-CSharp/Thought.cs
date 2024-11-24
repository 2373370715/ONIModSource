using System;
using UnityEngine;

// Token: 0x020019E5 RID: 6629
public class Thought : Resource
{
	// Token: 0x06008A18 RID: 35352 RVA: 0x0035A16C File Offset: 0x0035836C
	public Thought(string id, ResourceSet parent, Sprite icon, string mode_icon, string sound_name, string bubble, string speech_prefix, LocString hover_text, bool show_immediately = false, float show_time = 4f) : base(id, parent, null)
	{
		this.sprite = icon;
		if (mode_icon != null)
		{
			this.modeSprite = Assets.GetSprite(mode_icon);
		}
		this.bubbleSprite = Assets.GetSprite(bubble);
		this.sound = sound_name;
		this.speechPrefix = speech_prefix;
		this.hoverText = hover_text;
		this.showImmediately = show_immediately;
		this.showTime = show_time;
	}

	// Token: 0x06008A19 RID: 35353 RVA: 0x0035A1DC File Offset: 0x003583DC
	public Thought(string id, ResourceSet parent, string icon, string mode_icon, string sound_name, string bubble, string speech_prefix, LocString hover_text, bool show_immediately = false, float show_time = 4f) : base(id, parent, null)
	{
		this.sprite = Assets.GetSprite(icon);
		if (mode_icon != null)
		{
			this.modeSprite = Assets.GetSprite(mode_icon);
		}
		this.bubbleSprite = Assets.GetSprite(bubble);
		this.sound = sound_name;
		this.speechPrefix = speech_prefix;
		this.hoverText = hover_text;
		this.showImmediately = show_immediately;
		this.showTime = show_time;
	}

	// Token: 0x040067F1 RID: 26609
	public int priority;

	// Token: 0x040067F2 RID: 26610
	public Sprite sprite;

	// Token: 0x040067F3 RID: 26611
	public Sprite modeSprite;

	// Token: 0x040067F4 RID: 26612
	public string sound;

	// Token: 0x040067F5 RID: 26613
	public Sprite bubbleSprite;

	// Token: 0x040067F6 RID: 26614
	public string speechPrefix;

	// Token: 0x040067F7 RID: 26615
	public LocString hoverText;

	// Token: 0x040067F8 RID: 26616
	public bool showImmediately;

	// Token: 0x040067F9 RID: 26617
	public float showTime;
}
