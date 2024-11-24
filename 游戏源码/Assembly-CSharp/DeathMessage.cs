using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001DF2 RID: 7666
public class DeathMessage : TargetMessage
{
	// Token: 0x0600A057 RID: 41047 RVA: 0x0010838E File Offset: 0x0010658E
	public DeathMessage()
	{
	}

	// Token: 0x0600A058 RID: 41048 RVA: 0x001083A1 File Offset: 0x001065A1
	public DeathMessage(GameObject go, Death death) : base(go.GetComponent<KPrefabID>())
	{
		this.death.Set(death);
	}

	// Token: 0x0600A059 RID: 41049 RVA: 0x000CA99D File Offset: 0x000C8B9D
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x0600A05A RID: 41050 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public override bool PlayNotificationSound()
	{
		return false;
	}

	// Token: 0x0600A05B RID: 41051 RVA: 0x001083C6 File Offset: 0x001065C6
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.DUPLICANTDIED.NAME;
	}

	// Token: 0x0600A05C RID: 41052 RVA: 0x00108386 File Offset: 0x00106586
	public override string GetTooltip()
	{
		return this.GetMessageBody();
	}

	// Token: 0x0600A05D RID: 41053 RVA: 0x001083D2 File Offset: 0x001065D2
	public override string GetMessageBody()
	{
		return this.death.Get().description.Replace("{Target}", base.GetTarget().GetName());
	}

	// Token: 0x04007D70 RID: 32112
	[Serialize]
	private ResourceRef<Death> death = new ResourceRef<Death>();
}
