using System;
using KSerialization;
using STRINGS;

// Token: 0x02001E07 RID: 7687
public class WorldDetectedMessage : Message
{
	// Token: 0x0600A0EE RID: 41198 RVA: 0x001082E7 File Offset: 0x001064E7
	public WorldDetectedMessage()
	{
	}

	// Token: 0x0600A0EF RID: 41199 RVA: 0x00108A2B File Offset: 0x00106C2B
	public WorldDetectedMessage(WorldContainer world)
	{
		this.worldID = world.id;
	}

	// Token: 0x0600A0F0 RID: 41200 RVA: 0x001082A2 File Offset: 0x001064A2
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A0F1 RID: 41201 RVA: 0x003D652C File Offset: 0x003D472C
	public override string GetMessageBody()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
		return string.Format(MISC.NOTIFICATIONS.WORLDDETECTED.MESSAGEBODY, world.GetProperName());
	}

	// Token: 0x0600A0F2 RID: 41202 RVA: 0x00108A3F File Offset: 0x00106C3F
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.WORLDDETECTED.NAME;
	}

	// Token: 0x0600A0F3 RID: 41203 RVA: 0x003D6560 File Offset: 0x003D4760
	public override string GetTooltip()
	{
		WorldContainer world = ClusterManager.Instance.GetWorld(this.worldID);
		return string.Format(MISC.NOTIFICATIONS.WORLDDETECTED.TOOLTIP, world.GetProperName());
	}

	// Token: 0x0600A0F4 RID: 41204 RVA: 0x00108A4B File Offset: 0x00106C4B
	public override bool IsValid()
	{
		return this.worldID != 255;
	}

	// Token: 0x04007DA6 RID: 32166
	[Serialize]
	private int worldID;
}
