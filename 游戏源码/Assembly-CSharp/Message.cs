using System;
using KSerialization;

// Token: 0x02001DF8 RID: 7672
[SerializationConfig(MemberSerialization.OptIn)]
public abstract class Message : ISaveLoadable
{
	// Token: 0x0600A083 RID: 41091
	public abstract string GetTitle();

	// Token: 0x0600A084 RID: 41092
	public abstract string GetSound();

	// Token: 0x0600A085 RID: 41093
	public abstract string GetMessageBody();

	// Token: 0x0600A086 RID: 41094
	public abstract string GetTooltip();

	// Token: 0x0600A087 RID: 41095 RVA: 0x000A65EC File Offset: 0x000A47EC
	public virtual bool ShowDialog()
	{
		return true;
	}

	// Token: 0x0600A088 RID: 41096 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void OnCleanUp()
	{
	}

	// Token: 0x0600A089 RID: 41097 RVA: 0x000A65EC File Offset: 0x000A47EC
	public virtual bool IsValid()
	{
		return true;
	}

	// Token: 0x0600A08A RID: 41098 RVA: 0x000A65EC File Offset: 0x000A47EC
	public virtual bool PlayNotificationSound()
	{
		return true;
	}

	// Token: 0x0600A08B RID: 41099 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void OnClick()
	{
	}

	// Token: 0x0600A08C RID: 41100 RVA: 0x000AD486 File Offset: 0x000AB686
	public virtual NotificationType GetMessageType()
	{
		return NotificationType.Messages;
	}

	// Token: 0x0600A08D RID: 41101 RVA: 0x000A65EC File Offset: 0x000A47EC
	public virtual bool ShowDismissButton()
	{
		return true;
	}
}
