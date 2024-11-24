using System;

// Token: 0x02001203 RID: 4611
public class Death : Resource
{
	// Token: 0x06005E2A RID: 24106 RVA: 0x000DD86C File Offset: 0x000DBA6C
	public Death(string id, ResourceSet parent, string name, string description, string pre_anim, string loop_anim) : base(id, parent, name)
	{
		this.preAnim = pre_anim;
		this.loopAnim = loop_anim;
		this.description = description;
	}

	// Token: 0x040042A9 RID: 17065
	public string preAnim;

	// Token: 0x040042AA RID: 17066
	public string loopAnim;

	// Token: 0x040042AB RID: 17067
	public string sound;

	// Token: 0x040042AC RID: 17068
	public string description;
}
