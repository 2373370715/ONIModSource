using System;
using Database;

// Token: 0x02002002 RID: 8194
public class SkillListable : IListableOption
{
	// Token: 0x0600AE1A RID: 44570 RVA: 0x00416CEC File Offset: 0x00414EEC
	public SkillListable(string name)
	{
		this.skillName = name;
		Skill skill = Db.Get().Skills.TryGet(this.skillName);
		if (skill != null)
		{
			this.name = skill.Name;
			this.skillHat = skill.hat;
		}
	}

	// Token: 0x17000B23 RID: 2851
	// (get) Token: 0x0600AE1B RID: 44571 RVA: 0x0011153E File Offset: 0x0010F73E
	// (set) Token: 0x0600AE1C RID: 44572 RVA: 0x00111546 File Offset: 0x0010F746
	public string skillName { get; private set; }

	// Token: 0x17000B24 RID: 2852
	// (get) Token: 0x0600AE1D RID: 44573 RVA: 0x0011154F File Offset: 0x0010F74F
	// (set) Token: 0x0600AE1E RID: 44574 RVA: 0x00111557 File Offset: 0x0010F757
	public string skillHat { get; private set; }

	// Token: 0x0600AE1F RID: 44575 RVA: 0x00111560 File Offset: 0x0010F760
	public string GetProperName()
	{
		return this.name;
	}

	// Token: 0x040088D1 RID: 35025
	public LocString name;
}
