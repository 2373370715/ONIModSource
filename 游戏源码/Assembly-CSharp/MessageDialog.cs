using System;

// Token: 0x02001DF9 RID: 7673
public abstract class MessageDialog : KMonoBehaviour
{
	// Token: 0x17000A69 RID: 2665
	// (get) Token: 0x0600A08F RID: 41103 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
	public virtual bool CanDontShowAgain
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600A090 RID: 41104
	public abstract bool CanDisplay(Message message);

	// Token: 0x0600A091 RID: 41105
	public abstract void SetMessage(Message message);

	// Token: 0x0600A092 RID: 41106
	public abstract void OnClickAction();

	// Token: 0x0600A093 RID: 41107 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void OnDontShowAgain()
	{
	}
}
