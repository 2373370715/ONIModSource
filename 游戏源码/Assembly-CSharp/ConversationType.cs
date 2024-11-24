using System;
using UnityEngine;

// Token: 0x02001112 RID: 4370
public class ConversationType
{
	// Token: 0x0600598B RID: 22923 RVA: 0x000A5E40 File Offset: 0x000A4040
	public virtual void NewTarget(MinionIdentity speaker)
	{
	}

	// Token: 0x0600598C RID: 22924 RVA: 0x000AD332 File Offset: 0x000AB532
	public virtual Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		return null;
	}

	// Token: 0x0600598D RID: 22925 RVA: 0x000AD332 File Offset: 0x000AB532
	public virtual Sprite GetSprite(string topic)
	{
		return null;
	}

	// Token: 0x04003F50 RID: 16208
	public string id;

	// Token: 0x04003F51 RID: 16209
	public string target;
}
