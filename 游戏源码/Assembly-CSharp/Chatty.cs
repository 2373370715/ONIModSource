using System;
using System.Collections.Generic;

// Token: 0x02001069 RID: 4201
public class Chatty : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x060055B3 RID: 21939 RVA: 0x000D7E6B File Offset: 0x000D606B
	protected override void OnPrefabInit()
	{
		base.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		base.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		this.identity = base.GetComponent<MinionIdentity>();
	}

	// Token: 0x060055B4 RID: 21940 RVA: 0x0027F78C File Offset: 0x0027D98C
	private void OnStartedTalking(object data)
	{
		MinionIdentity minionIdentity = data as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		this.conversationPartners.Add(minionIdentity);
	}

	// Token: 0x060055B5 RID: 21941 RVA: 0x0027F7B8 File Offset: 0x0027D9B8
	public void SimEveryTick(float dt)
	{
		if (this.conversationPartners.Count == 0)
		{
			return;
		}
		for (int i = this.conversationPartners.Count - 1; i >= 0; i--)
		{
			MinionIdentity minionIdentity = this.conversationPartners[i];
			this.conversationPartners.RemoveAt(i);
			if (!(minionIdentity == this.identity))
			{
				minionIdentity.AddTag(GameTags.PleasantConversation);
			}
		}
		base.gameObject.AddTag(GameTags.PleasantConversation);
	}

	// Token: 0x04003C2C RID: 15404
	private MinionIdentity identity;

	// Token: 0x04003C2D RID: 15405
	private List<MinionIdentity> conversationPartners = new List<MinionIdentity>();
}
