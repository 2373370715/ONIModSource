using System;
using UnityEngine;

// Token: 0x02001E02 RID: 7682
public class StandardMessageDialog : MessageDialog
{
	// Token: 0x0600A0D8 RID: 41176 RVA: 0x0010893D File Offset: 0x00106B3D
	public override bool CanDisplay(Message message)
	{
		return typeof(Message).IsAssignableFrom(message.GetType());
	}

	// Token: 0x0600A0D9 RID: 41177 RVA: 0x00108954 File Offset: 0x00106B54
	public override void SetMessage(Message base_message)
	{
		this.message = base_message;
		this.description.text = this.message.GetMessageBody();
	}

	// Token: 0x0600A0DA RID: 41178 RVA: 0x000A5E40 File Offset: 0x000A4040
	public override void OnClickAction()
	{
	}

	// Token: 0x04007D97 RID: 32151
	[SerializeField]
	private LocText description;

	// Token: 0x04007D98 RID: 32152
	private Message message;
}
