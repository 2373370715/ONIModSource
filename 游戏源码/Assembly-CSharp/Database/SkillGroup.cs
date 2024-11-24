using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x020021C1 RID: 8641
	public class SkillGroup : Resource, IListableOption
	{
		// Token: 0x0600B75A RID: 46938 RVA: 0x001161EB File Offset: 0x001143EB
		string IListableOption.GetProperName()
		{
			return Strings.Get("STRINGS.DUPLICANTS.SKILLGROUPS." + this.Id.ToUpper() + ".NAME");
		}

		// Token: 0x0600B75B RID: 46939 RVA: 0x00116211 File Offset: 0x00114411
		public SkillGroup(string id, string choreGroupID, string name, string icon, string archetype_icon) : base(id, name)
		{
			this.choreGroupID = choreGroupID;
			this.choreGroupIcon = icon;
			this.archetypeIcon = archetype_icon;
		}

		// Token: 0x040095BF RID: 38335
		public string choreGroupID;

		// Token: 0x040095C0 RID: 38336
		public List<Klei.AI.Attribute> relevantAttributes;

		// Token: 0x040095C1 RID: 38337
		public List<string> requiredChoreGroups;

		// Token: 0x040095C2 RID: 38338
		public string choreGroupIcon;

		// Token: 0x040095C3 RID: 38339
		public string archetypeIcon;
	}
}
