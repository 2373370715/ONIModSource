using System;
using STRINGS;

// Token: 0x02001DF1 RID: 7665
public class CodexUnlockedMessage : Message
{
	// Token: 0x0600A04F RID: 41039 RVA: 0x001082E7 File Offset: 0x001064E7
	public CodexUnlockedMessage()
	{
	}

	// Token: 0x0600A050 RID: 41040 RVA: 0x00108345 File Offset: 0x00106545
	public CodexUnlockedMessage(string lock_id, string unlock_message)
	{
		this.lockId = lock_id;
		this.unlockMessage = unlock_message;
	}

	// Token: 0x0600A051 RID: 41041 RVA: 0x0010835B File Offset: 0x0010655B
	public string GetLockId()
	{
		return this.lockId;
	}

	// Token: 0x0600A052 RID: 41042 RVA: 0x001082A2 File Offset: 0x001064A2
	public override string GetSound()
	{
		return "AI_Notification_ResearchComplete";
	}

	// Token: 0x0600A053 RID: 41043 RVA: 0x00108363 File Offset: 0x00106563
	public override string GetMessageBody()
	{
		return UI.CODEX.CODEX_DISCOVERED_MESSAGE.BODY.Replace("{codex}", this.unlockMessage);
	}

	// Token: 0x0600A054 RID: 41044 RVA: 0x0010837A File Offset: 0x0010657A
	public override string GetTitle()
	{
		return UI.CODEX.CODEX_DISCOVERED_MESSAGE.TITLE;
	}

	// Token: 0x0600A055 RID: 41045 RVA: 0x00108386 File Offset: 0x00106586
	public override string GetTooltip()
	{
		return this.GetMessageBody();
	}

	// Token: 0x0600A056 RID: 41046 RVA: 0x000A65EC File Offset: 0x000A47EC
	public override bool IsValid()
	{
		return true;
	}

	// Token: 0x04007D6E RID: 32110
	private string unlockMessage;

	// Token: 0x04007D6F RID: 32111
	private string lockId;
}
