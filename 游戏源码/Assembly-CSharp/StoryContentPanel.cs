using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

public class StoryContentPanel : KMonoBehaviour
{
	private enum StoryState
	{
		Forbidden,
		Guaranteed,
		LENGTH
	}

	[SerializeField]
	private GameObject storyRowPrefab;

	[SerializeField]
	private GameObject storyRowContainer;

	private Dictionary<string, GameObject> storyRows = new Dictionary<string, GameObject>();

	public const int DEFAULT_RANDOMIZE_STORY_COUNT = 5;

	private Dictionary<string, StoryState> storyStates = new Dictionary<string, StoryState>();

	private string selectedStoryId = "";

	[SerializeField]
	private ColonyDestinationSelectScreen mainScreen;

	[Header("Trait Count")]
	[Header("SelectedStory")]
	[SerializeField]
	private Image selectedStoryImage;

	[SerializeField]
	private LocText selectedStoryTitleLabel;

	[SerializeField]
	private LocText selectedStoryDescriptionLabel;

	[SerializeField]
	private Sprite spriteForbidden;

	[SerializeField]
	private Sprite spritePossible;

	[SerializeField]
	private Sprite spriteGuaranteed;

	private StoryState _defaultStoryState;

	private List<string> storyTraitSettings = new List<string> { "None", "Few", "Lots" };

	public List<string> GetActiveStories()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, StoryState> storyState in storyStates)
		{
			if (storyState.Value == StoryState.Guaranteed)
			{
				list.Add(storyState.Key);
			}
		}
		return list;
	}

	public void Init()
	{
		SpawnRows();
		RefreshRows();
		RefreshDescriptionPanel();
		SelectDefault();
		CustomGameSettings.Instance.OnStorySettingChanged += OnStorySettingChanged;
	}

	public void Cleanup()
	{
		CustomGameSettings.Instance.OnStorySettingChanged -= OnStorySettingChanged;
	}

	private void OnStorySettingChanged(SettingConfig config, SettingLevel level)
	{
		storyStates[config.id] = ((level.id == "Guaranteed") ? StoryState.Guaranteed : StoryState.Forbidden);
		RefreshStoryDisplay(config.id);
	}

	private void SpawnRows()
	{
		foreach (Story story in Db.Get().Stories.resources)
		{
			GameObject gameObject = Util.KInstantiateUI(storyRowPrefab, storyRowContainer, force_active: true);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			component.GetReference<LocText>("Label").SetText(Strings.Get(story.StoryTrait.name));
			MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
			component2.onClick = (System.Action)Delegate.Combine(component2.onClick, (System.Action)delegate
			{
				SelectRow(story.Id);
			});
			storyRows.Add(story.Id, gameObject);
			component.GetReference<Image>("Icon").sprite = Assets.GetSprite(story.StoryTrait.icon);
			MultiToggle reference = component.GetReference<MultiToggle>("checkbox");
			reference.onClick = (System.Action)Delegate.Combine(reference.onClick, (System.Action)delegate
			{
				IncrementStorySetting(story.Id);
				RefreshStoryDisplay(story.Id);
			});
			storyStates.Add(story.Id, _defaultStoryState);
		}
		RefreshAllStoryStates();
		mainScreen.RefreshStoryLabel();
	}

	private void SelectRow(string id)
	{
		selectedStoryId = id;
		RefreshRows();
		RefreshDescriptionPanel();
	}

	public void SelectDefault()
	{
		foreach (KeyValuePair<string, StoryState> storyState in storyStates)
		{
			if (storyState.Value == StoryState.Guaranteed)
			{
				SelectRow(storyState.Key);
				return;
			}
		}
		using Dictionary<string, StoryState>.Enumerator enumerator = storyStates.GetEnumerator();
		if (enumerator.MoveNext())
		{
			SelectRow(enumerator.Current.Key);
		}
	}

	private void IncrementStorySetting(string storyId, bool forward = true)
	{
		int num = (int)storyStates[storyId];
		num += (forward ? 1 : (-1));
		if (num < 0)
		{
			num += 2;
		}
		num %= 2;
		SetStoryState(storyId, (StoryState)num);
		mainScreen.RefreshRowsAndDescriptions();
	}

	private void SetStoryState(string storyId, StoryState state)
	{
		storyStates[storyId] = state;
		SettingConfig config = CustomGameSettings.Instance.StorySettings[storyId];
		CustomGameSettings.Instance.SetStorySetting(config, storyStates[storyId] == StoryState.Guaranteed);
	}

	public void SelectRandomStories(int min = 5, int max = 5, bool useBias = false)
	{
		int num = UnityEngine.Random.Range(min, max);
		List<Story> list = new List<Story>(Db.Get().Stories.resources);
		List<Story> list2 = new List<Story>();
		list.Shuffle();
		for (int i = 0; i < num && list.Count - 1 >= i; i++)
		{
			list2.Add(list[i]);
		}
		float num2 = 0.7f;
		int num3 = list2.Count((Story x) => x.IsNew());
		if (useBias && num3 == 0 && UnityEngine.Random.value < num2)
		{
			List<Story> list3 = Db.Get().Stories.resources.Where((Story x) => x.IsNew()).ToList();
			list3.Shuffle();
			if (list3.Count > 0)
			{
				list2.RemoveAt(0);
				list2.Add(list3[0]);
			}
		}
		foreach (Story item in list)
		{
			SetStoryState(item.Id, list2.Contains(item) ? StoryState.Guaranteed : StoryState.Forbidden);
		}
		RefreshAllStoryStates();
		mainScreen.RefreshRowsAndDescriptions();
	}

	private void RefreshAllStoryStates()
	{
		foreach (string key in storyRows.Keys)
		{
			RefreshStoryDisplay(key);
		}
	}

	private void RefreshStoryDisplay(string id)
	{
		MultiToggle reference = storyRows[id].GetComponent<HierarchyReferences>().GetReference<MultiToggle>("checkbox");
		switch (storyStates[id])
		{
		case StoryState.Forbidden:
			reference.ChangeState(0);
			break;
		case StoryState.Guaranteed:
			reference.ChangeState(1);
			break;
		}
	}

	private void RefreshRows()
	{
		foreach (KeyValuePair<string, GameObject> storyRow in storyRows)
		{
			storyRow.Value.GetComponent<MultiToggle>().ChangeState((storyRow.Key == selectedStoryId) ? 1 : 0);
		}
	}

	private void RefreshDescriptionPanel()
	{
		if (selectedStoryId.IsNullOrWhiteSpace())
		{
			selectedStoryTitleLabel.SetText("");
			selectedStoryDescriptionLabel.SetText("");
			return;
		}
		WorldTrait storyTrait = Db.Get().Stories.GetStoryTrait(selectedStoryId, assertMissingTrait: true);
		selectedStoryTitleLabel.SetText(Strings.Get(storyTrait.name));
		selectedStoryDescriptionLabel.SetText(Strings.Get(storyTrait.description));
		string text = storyTrait.icon.Replace("_icon", "_image");
		selectedStoryImage.sprite = Assets.GetSprite(text);
	}

	public string GetTraitsString(bool tooltip = false)
	{
		int num = 0;
		int num2 = 5;
		foreach (KeyValuePair<string, StoryState> storyState in storyStates)
		{
			if (storyState.Value == StoryState.Guaranteed)
			{
				num++;
			}
		}
		string text = UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER;
		string text2 = "";
		text = text + ": " + num switch
		{
			0 => UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS, 
			1 => UI.FRONTEND.COLONYDESTINATIONSCREEN.SINGLE_TRAIT, 
			_ => string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.TRAIT_COUNT, num), 
		};
		if (num > num2)
		{
			text = text + " " + UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING;
		}
		if (tooltip)
		{
			foreach (KeyValuePair<string, StoryState> storyState2 in storyStates)
			{
				if (storyState2.Value == StoryState.Guaranteed)
				{
					WorldTrait storyTrait = Db.Get().Stories.Get(storyState2.Key).StoryTrait;
					text = text + "\n\n<b>" + Strings.Get(storyTrait.name).String + "</b>\n" + Strings.Get(storyTrait.description).String;
				}
			}
			if (num > num2)
			{
				text = text + "\n\n" + UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING_TOOLTIP;
			}
		}
		return text;
	}
}
