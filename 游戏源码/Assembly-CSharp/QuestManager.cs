using System.Collections.Generic;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class QuestManager : KMonoBehaviour
{
	private static QuestManager instance;

	[Serialize]
	private Dictionary<int, Dictionary<HashedString, QuestInstance>> ownerToQuests = new Dictionary<int, Dictionary<HashedString, QuestInstance>>();

	protected override void OnPrefabInit()
	{
		if (instance != null)
		{
			Object.Destroy(instance);
			return;
		}
		instance = this;
		base.OnPrefabInit();
	}

	public static QuestInstance InitializeQuest(Tag ownerId, Quest quest)
	{
		if (!TryGetQuest(ownerId.GetHash(), quest, out var qInst))
		{
			QuestInstance questInstance2 = (instance.ownerToQuests[ownerId.GetHash()][quest.IdHash] = new QuestInstance(quest));
			qInst = questInstance2;
		}
		qInst.Initialize(quest);
		return qInst;
	}

	public static QuestInstance InitializeQuest(HashedString ownerId, Quest quest)
	{
		if (!TryGetQuest(ownerId.HashValue, quest, out var qInst))
		{
			QuestInstance questInstance2 = (instance.ownerToQuests[ownerId.HashValue][quest.IdHash] = new QuestInstance(quest));
			qInst = questInstance2;
		}
		qInst.Initialize(quest);
		return qInst;
	}

	public static QuestInstance GetInstance(Tag ownerId, Quest quest)
	{
		TryGetQuest(ownerId.GetHash(), quest, out var qInst);
		return qInst;
	}

	public static QuestInstance GetInstance(HashedString ownerId, Quest quest)
	{
		TryGetQuest(ownerId.HashValue, quest, out var qInst);
		return qInst;
	}

	public static bool CheckState(HashedString ownerId, Quest quest, Quest.State state)
	{
		TryGetQuest(ownerId.HashValue, quest, out var qInst);
		if (qInst != null)
		{
			return qInst.CurrentState == state;
		}
		return false;
	}

	public static bool CheckState(Tag ownerId, Quest quest, Quest.State state)
	{
		TryGetQuest(ownerId.GetHash(), quest, out var qInst);
		if (qInst != null)
		{
			return qInst.CurrentState == state;
		}
		return false;
	}

	private static bool TryGetQuest(int ownerId, Quest quest, out QuestInstance qInst)
	{
		qInst = null;
		if (!instance.ownerToQuests.TryGetValue(ownerId, out var value))
		{
			Dictionary<HashedString, QuestInstance> dictionary2 = (instance.ownerToQuests[ownerId] = new Dictionary<HashedString, QuestInstance>());
			value = dictionary2;
		}
		if (!value.TryGetValue(quest.IdHash, out qInst))
		{
			return false;
		}
		return true;
	}
}
