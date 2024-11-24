using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001727 RID: 5927
[SerializationConfig(MemberSerialization.OptIn)]
public class QuestManager : KMonoBehaviour
{
	// Token: 0x06007A17 RID: 31255 RVA: 0x000F0334 File Offset: 0x000EE534
	protected override void OnPrefabInit()
	{
		if (QuestManager.instance != null)
		{
			UnityEngine.Object.Destroy(QuestManager.instance);
			return;
		}
		QuestManager.instance = this;
		base.OnPrefabInit();
	}

	// Token: 0x06007A18 RID: 31256 RVA: 0x003179D8 File Offset: 0x00315BD8
	public static QuestInstance InitializeQuest(Tag ownerId, Quest quest)
	{
		QuestInstance questInstance;
		if (!QuestManager.TryGetQuest(ownerId.GetHash(), quest, out questInstance))
		{
			questInstance = (QuestManager.instance.ownerToQuests[ownerId.GetHash()][quest.IdHash] = new QuestInstance(quest));
		}
		questInstance.Initialize(quest);
		return questInstance;
	}

	// Token: 0x06007A19 RID: 31257 RVA: 0x00317A2C File Offset: 0x00315C2C
	public static QuestInstance InitializeQuest(HashedString ownerId, Quest quest)
	{
		QuestInstance questInstance;
		if (!QuestManager.TryGetQuest(ownerId.HashValue, quest, out questInstance))
		{
			questInstance = (QuestManager.instance.ownerToQuests[ownerId.HashValue][quest.IdHash] = new QuestInstance(quest));
		}
		questInstance.Initialize(quest);
		return questInstance;
	}

	// Token: 0x06007A1A RID: 31258 RVA: 0x00317A80 File Offset: 0x00315C80
	public static QuestInstance GetInstance(Tag ownerId, Quest quest)
	{
		QuestInstance result;
		QuestManager.TryGetQuest(ownerId.GetHash(), quest, out result);
		return result;
	}

	// Token: 0x06007A1B RID: 31259 RVA: 0x00317AA0 File Offset: 0x00315CA0
	public static QuestInstance GetInstance(HashedString ownerId, Quest quest)
	{
		QuestInstance result;
		QuestManager.TryGetQuest(ownerId.HashValue, quest, out result);
		return result;
	}

	// Token: 0x06007A1C RID: 31260 RVA: 0x00317AC0 File Offset: 0x00315CC0
	public static bool CheckState(HashedString ownerId, Quest quest, Quest.State state)
	{
		QuestInstance questInstance;
		QuestManager.TryGetQuest(ownerId.HashValue, quest, out questInstance);
		return questInstance != null && questInstance.CurrentState == state;
	}

	// Token: 0x06007A1D RID: 31261 RVA: 0x00317AEC File Offset: 0x00315CEC
	public static bool CheckState(Tag ownerId, Quest quest, Quest.State state)
	{
		QuestInstance questInstance;
		QuestManager.TryGetQuest(ownerId.GetHash(), quest, out questInstance);
		return questInstance != null && questInstance.CurrentState == state;
	}

	// Token: 0x06007A1E RID: 31262 RVA: 0x00317B18 File Offset: 0x00315D18
	private static bool TryGetQuest(int ownerId, Quest quest, out QuestInstance qInst)
	{
		qInst = null;
		Dictionary<HashedString, QuestInstance> dictionary;
		if (!QuestManager.instance.ownerToQuests.TryGetValue(ownerId, out dictionary))
		{
			dictionary = (QuestManager.instance.ownerToQuests[ownerId] = new Dictionary<HashedString, QuestInstance>());
		}
		return dictionary.TryGetValue(quest.IdHash, out qInst);
	}

	// Token: 0x04005B9D RID: 23453
	private static QuestManager instance;

	// Token: 0x04005B9E RID: 23454
	[Serialize]
	private Dictionary<int, Dictionary<HashedString, QuestInstance>> ownerToQuests = new Dictionary<int, Dictionary<HashedString, QuestInstance>>();
}
