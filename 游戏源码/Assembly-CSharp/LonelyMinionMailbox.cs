using System;
using UnityEngine;

public class LonelyMinionMailbox : KMonoBehaviour
{
	public LonelyMinionHouse.Instance House;

	public void Initialize(LonelyMinionHouse.Instance house)
	{
		House = house;
		SingleEntityReceptacle component = GetComponent<SingleEntityReceptacle>();
		component.occupyingObjectRelativePosition = base.transform.InverseTransformPoint(house.GetParcelPosition());
		component.occupyingObjectRelativePosition.z = -1f;
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
		storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(OnStoryStateChanged));
		OnStoryStateChanged(storyInstance.CurrentState);
	}

	protected override void OnSpawn()
	{
		if (StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.LonelyMinion))
		{
			base.gameObject.AddOrGet<Deconstructable>().allowDeconstruction = true;
		}
	}

	protected override void OnCleanUp()
	{
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
		storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(OnStoryStateChanged));
	}

	private void OnStoryStateChanged(StoryInstance.State state)
	{
		QuestInstance quest = QuestManager.GetInstance(House.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
		if (state == StoryInstance.State.IN_PROGRESS)
		{
			Subscribe(-731304873, OnStorageChanged);
			SingleEntityReceptacle singleEntityReceptacle = base.gameObject.AddOrGet<SingleEntityReceptacle>();
			singleEntityReceptacle.enabled = true;
			singleEntityReceptacle.AddAdditionalCriteria(delegate(GameObject candidate)
			{
				EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(candidate.GetComponent<KPrefabID>().PrefabTag.Name);
				int valueHandle = 0;
				return foodInfo != null && quest.DataSatisfiesCriteria(new Quest.ItemData
				{
					CriteriaId = LonelyMinionConfig.FoodCriteriaId,
					QualifyingTag = GameTags.Edible,
					CurrentValue = foodInfo.Quality
				}, ref valueHandle);
			});
			RootMenu.Instance.Refresh();
			OnStorageChanged(singleEntityReceptacle.Occupant);
		}
		if (state == StoryInstance.State.COMPLETE)
		{
			Unsubscribe(-731304873, OnStorageChanged);
			base.gameObject.AddOrGet<Deconstructable>().allowDeconstruction = true;
		}
	}

	private void OnStorageChanged(object data)
	{
		House.MailboxContentChanged(data as GameObject);
	}
}
