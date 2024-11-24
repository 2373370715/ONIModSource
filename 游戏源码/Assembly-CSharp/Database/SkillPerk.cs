using System;

namespace Database
{
	// Token: 0x020021BC RID: 8636
	public class SkillPerk : Resource
	{
		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x0600B746 RID: 46918 RVA: 0x00116106 File Offset: 0x00114306
		// (set) Token: 0x0600B747 RID: 46919 RVA: 0x0011610E File Offset: 0x0011430E
		public Action<MinionResume> OnApply { get; protected set; }

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x0600B748 RID: 46920 RVA: 0x00116117 File Offset: 0x00114317
		// (set) Token: 0x0600B749 RID: 46921 RVA: 0x0011611F File Offset: 0x0011431F
		public Action<MinionResume> OnRemove { get; protected set; }

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x0600B74A RID: 46922 RVA: 0x00116128 File Offset: 0x00114328
		// (set) Token: 0x0600B74B RID: 46923 RVA: 0x00116130 File Offset: 0x00114330
		public Action<MinionResume> OnMinionsChanged { get; protected set; }

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x0600B74C RID: 46924 RVA: 0x00116139 File Offset: 0x00114339
		// (set) Token: 0x0600B74D RID: 46925 RVA: 0x00116141 File Offset: 0x00114341
		public bool affectAll { get; protected set; }

		// Token: 0x0600B74E RID: 46926 RVA: 0x0011614A File Offset: 0x0011434A
		public SkillPerk(string id_str, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove, Action<MinionResume> OnMinionsChanged, bool affectAll = false) : base(id_str, description)
		{
			this.OnApply = OnApply;
			this.OnRemove = OnRemove;
			this.OnMinionsChanged = OnMinionsChanged;
			this.affectAll = affectAll;
		}

		// Token: 0x0600B74F RID: 46927 RVA: 0x00116173 File Offset: 0x00114373
		public SkillPerk(string id_str, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove, Action<MinionResume> OnMinionsChanged, string[] requiredDlcIds = null, bool affectAll = false) : base(id_str, description)
		{
			this.OnApply = OnApply;
			this.OnRemove = OnRemove;
			this.OnMinionsChanged = OnMinionsChanged;
			this.affectAll = affectAll;
			this.requiredDlcIds = requiredDlcIds;
		}

		// Token: 0x04009567 RID: 38247
		public string[] requiredDlcIds;
	}
}
