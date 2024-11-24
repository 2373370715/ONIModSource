using System;

// Token: 0x020012D5 RID: 4821
public class Expectation
{
	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x06006302 RID: 25346 RVA: 0x000E0A5E File Offset: 0x000DEC5E
	// (set) Token: 0x06006303 RID: 25347 RVA: 0x000E0A66 File Offset: 0x000DEC66
	public string id { get; protected set; }

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06006304 RID: 25348 RVA: 0x000E0A6F File Offset: 0x000DEC6F
	// (set) Token: 0x06006305 RID: 25349 RVA: 0x000E0A77 File Offset: 0x000DEC77
	public string name { get; protected set; }

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06006306 RID: 25350 RVA: 0x000E0A80 File Offset: 0x000DEC80
	// (set) Token: 0x06006307 RID: 25351 RVA: 0x000E0A88 File Offset: 0x000DEC88
	public string description { get; protected set; }

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06006308 RID: 25352 RVA: 0x000E0A91 File Offset: 0x000DEC91
	// (set) Token: 0x06006309 RID: 25353 RVA: 0x000E0A99 File Offset: 0x000DEC99
	public Action<MinionResume> OnApply { get; protected set; }

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x0600630A RID: 25354 RVA: 0x000E0AA2 File Offset: 0x000DECA2
	// (set) Token: 0x0600630B RID: 25355 RVA: 0x000E0AAA File Offset: 0x000DECAA
	public Action<MinionResume> OnRemove { get; protected set; }

	// Token: 0x0600630C RID: 25356 RVA: 0x000E0AB3 File Offset: 0x000DECB3
	public Expectation(string id, string name, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove)
	{
		this.id = id;
		this.name = name;
		this.description = description;
		this.OnApply = OnApply;
		this.OnRemove = OnRemove;
	}
}
