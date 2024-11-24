using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001DFC RID: 7676
[AddComponentMenu("KMonoBehaviour/scripts/Messenger")]
public class Messenger : KMonoBehaviour
{
	// Token: 0x17000A6A RID: 2666
	// (get) Token: 0x0600A0A5 RID: 41125 RVA: 0x0010866F File Offset: 0x0010686F
	public int Count
	{
		get
		{
			return this.messages.Count;
		}
	}

	// Token: 0x0600A0A6 RID: 41126 RVA: 0x0010867C File Offset: 0x0010687C
	public IEnumerator<Message> GetEnumerator()
	{
		return this.messages.GetEnumerator();
	}

	// Token: 0x0600A0A7 RID: 41127 RVA: 0x00108689 File Offset: 0x00106889
	public static void DestroyInstance()
	{
		Messenger.Instance = null;
	}

	// Token: 0x17000A6B RID: 2667
	// (get) Token: 0x0600A0A8 RID: 41128 RVA: 0x00108691 File Offset: 0x00106891
	public SerializedList<Message> Messages
	{
		get
		{
			return this.messages;
		}
	}

	// Token: 0x0600A0A9 RID: 41129 RVA: 0x00108699 File Offset: 0x00106899
	protected override void OnPrefabInit()
	{
		Messenger.Instance = this;
	}

	// Token: 0x0600A0AA RID: 41130 RVA: 0x003D5EC0 File Offset: 0x003D40C0
	protected override void OnSpawn()
	{
		int i = 0;
		while (i < this.messages.Count)
		{
			if (this.messages[i].IsValid())
			{
				i++;
			}
			else
			{
				this.messages.RemoveAt(i);
			}
		}
		base.Trigger(-599791736, null);
	}

	// Token: 0x0600A0AB RID: 41131 RVA: 0x001086A1 File Offset: 0x001068A1
	public void QueueMessage(Message message)
	{
		this.messages.Add(message);
		base.Trigger(1558809273, message);
	}

	// Token: 0x0600A0AC RID: 41132 RVA: 0x003D5F10 File Offset: 0x003D4110
	public Message DequeueMessage()
	{
		Message result = null;
		if (this.messages.Count > 0)
		{
			result = this.messages[0];
			this.messages.RemoveAt(0);
		}
		return result;
	}

	// Token: 0x0600A0AD RID: 41133 RVA: 0x003D5F48 File Offset: 0x003D4148
	public void ClearAllMessages()
	{
		for (int i = this.messages.Count - 1; i >= 0; i--)
		{
			this.messages.RemoveAt(i);
		}
	}

	// Token: 0x0600A0AE RID: 41134 RVA: 0x001086BB File Offset: 0x001068BB
	public void RemoveMessage(Message m)
	{
		this.messages.Remove(m);
		base.Trigger(-599791736, null);
	}

	// Token: 0x04007D85 RID: 32133
	[Serialize]
	private SerializedList<Message> messages = new SerializedList<Message>();

	// Token: 0x04007D86 RID: 32134
	public static Messenger Instance;
}
