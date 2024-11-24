using System.Collections.Generic;

public class Chatty : KMonoBehaviour, ISimEveryTick
{
	private MinionIdentity identity;

	private List<MinionIdentity> conversationPartners = new List<MinionIdentity>();

	protected override void OnPrefabInit()
	{
		GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse);
		Subscribe(-594200555, OnStartedTalking);
		identity = GetComponent<MinionIdentity>();
	}

	private void OnStartedTalking(object data)
	{
		MinionIdentity minionIdentity = data as MinionIdentity;
		if (!(minionIdentity == null))
		{
			conversationPartners.Add(minionIdentity);
		}
	}

	public void SimEveryTick(float dt)
	{
		if (conversationPartners.Count == 0)
		{
			return;
		}
		for (int num = conversationPartners.Count - 1; num >= 0; num--)
		{
			MinionIdentity minionIdentity = conversationPartners[num];
			conversationPartners.RemoveAt(num);
			if (!(minionIdentity == identity))
			{
				minionIdentity.AddTag(GameTags.PleasantConversation);
			}
		}
		base.gameObject.AddTag(GameTags.PleasantConversation);
	}
}
