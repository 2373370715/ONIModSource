using System;
using UnityEngine;

// Token: 0x02001DF0 RID: 7664
public class CodexMessageDialog : MessageDialog
{
	// Token: 0x0600A04A RID: 41034 RVA: 0x001082EF File Offset: 0x001064EF
	public override bool CanDisplay(Message message)
	{
		return typeof(CodexUnlockedMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x0600A04B RID: 41035 RVA: 0x00108306 File Offset: 0x00106506
	public override void SetMessage(Message base_message)
	{
		this.message = (CodexUnlockedMessage)base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x0600A04C RID: 41036 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void OnClickAction()
	{
	}

	// Token: 0x0600A04D RID: 41037 RVA: 0x0010832A File Offset: 0x0010652A
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.message.OnCleanUp();
	}

	// Token: 0x04007D6C RID: 32108
	[SerializeField]
	private LocText description;

	// Token: 0x04007D6D RID: 32109
	private CodexUnlockedMessage message;
}
