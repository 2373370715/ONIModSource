using System;

namespace Database
{
	// Token: 0x020021BF RID: 8639
	public class SimpleSkillPerk : SkillPerk
	{
		// Token: 0x0600B757 RID: 46935 RVA: 0x001161CE File Offset: 0x001143CE
		public SimpleSkillPerk(string id, string description) : base(id, description, null, null, null, false)
		{
		}

		// Token: 0x0600B758 RID: 46936 RVA: 0x001161DC File Offset: 0x001143DC
		public SimpleSkillPerk(string id, string description, string[] requiredDlcIds) : base(id, description, null, null, null, requiredDlcIds, false)
		{
		}
	}
}
