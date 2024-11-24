using System;
using UnityEngine;

// Token: 0x02001E04 RID: 7684
public class TargetMessageDialog : MessageDialog
{
	// Token: 0x0600A0E0 RID: 41184 RVA: 0x0010899C File Offset: 0x00106B9C
	public override bool CanDisplay(Message message)
	{
		return typeof(TargetMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x0600A0E1 RID: 41185 RVA: 0x001089B3 File Offset: 0x00106BB3
	public override void SetMessage(Message base_message)
	{
		this.message = (TargetMessage)base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x0600A0E2 RID: 41186 RVA: 0x003D63BC File Offset: 0x003D45BC
	public override void OnClickAction()
	{
		MessageTarget target = this.message.GetTarget();
		SelectTool.Instance.SelectAndFocus(target.GetPosition(), target.GetSelectable());
	}

	// Token: 0x0600A0E3 RID: 41187 RVA: 0x001089D7 File Offset: 0x00106BD7
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.message.OnCleanUp();
	}

	// Token: 0x04007D9A RID: 32154
	[SerializeField]
	private LocText description;

	// Token: 0x04007D9B RID: 32155
	private TargetMessage message;
}
