using System;
using UnityEngine;

// Token: 0x02000E7B RID: 3707
public class LonelyMinionMailbox : KMonoBehaviour
{
	// Token: 0x06004A95 RID: 19093 RVA: 0x0025BBE4 File Offset: 0x00259DE4
	public void Initialize(LonelyMinionHouse.Instance house)
	{
		this.House = house;
		SingleEntityReceptacle component = base.GetComponent<SingleEntityReceptacle>();
		component.occupyingObjectRelativePosition = base.transform.InverseTransformPoint(house.GetParcelPosition());
		component.occupyingObjectRelativePosition.z = -1f;
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
		StoryInstance storyInstance2 = storyInstance;
		storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
		this.OnStoryStateChanged(storyInstance.CurrentState);
	}

	// Token: 0x06004A96 RID: 19094 RVA: 0x000D035D File Offset: 0x000CE55D
	protected override void OnSpawn()
	{
		if (StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.LonelyMinion))
		{
			base.gameObject.AddOrGet<Deconstructable>().allowDeconstruction = true;
		}
	}

	// Token: 0x06004A97 RID: 19095 RVA: 0x0025BC74 File Offset: 0x00259E74
	protected override void OnCleanUp()
	{
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
		storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
	}

	// Token: 0x06004A98 RID: 19096 RVA: 0x0025BCC0 File Offset: 0x00259EC0
	private void OnStoryStateChanged(StoryInstance.State state)
	{
		QuestInstance quest = QuestManager.GetInstance(this.House.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
		if (state == StoryInstance.State.IN_PROGRESS)
		{
			base.Subscribe(-731304873, new Action<object>(this.OnStorageChanged));
			SingleEntityReceptacle singleEntityReceptacle = base.gameObject.AddOrGet<SingleEntityReceptacle>();
			singleEntityReceptacle.enabled = true;
			singleEntityReceptacle.AddAdditionalCriteria(delegate(GameObject candidate)
			{
				EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(candidate.GetComponent<KPrefabID>().PrefabTag.Name);
				int num = 0;
				return foodInfo != null && quest.DataSatisfiesCriteria(new Quest.ItemData
				{
					CriteriaId = LonelyMinionConfig.FoodCriteriaId,
					QualifyingTag = GameTags.Edible,
					CurrentValue = (float)foodInfo.Quality
				}, ref num);
			});
			RootMenu.Instance.Refresh();
			this.OnStorageChanged(singleEntityReceptacle.Occupant);
		}
		if (state == StoryInstance.State.COMPLETE)
		{
			base.Unsubscribe(-731304873, new Action<object>(this.OnStorageChanged));
			base.gameObject.AddOrGet<Deconstructable>().allowDeconstruction = true;
		}
	}

	// Token: 0x06004A99 RID: 19097 RVA: 0x000D038C File Offset: 0x000CE58C
	private void OnStorageChanged(object data)
	{
		this.House.MailboxContentChanged(data as GameObject);
	}

	// Token: 0x040033AA RID: 13226
	public LonelyMinionHouse.Instance House;
}
