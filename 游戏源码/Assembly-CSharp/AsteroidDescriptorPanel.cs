using System;
using System.Collections.Generic;
using Database;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("KMonoBehaviour/scripts/AsteroidDescriptorPanel")]
public class AsteroidDescriptorPanel : KMonoBehaviour
{
	[Header("Destination Details")]
	[SerializeField]
	private GameObject customLabelPrefab;

	[SerializeField]
	private GameObject prefabTraitWidget;

	[SerializeField]
	private GameObject prefabTraitCategoryWidget;

	[SerializeField]
	private GameObject prefabParameterWidget;

	[SerializeField]
	private GameObject startingAsteroidRowContainer;

	[SerializeField]
	private GameObject nearbyAsteroidRowContainer;

	[SerializeField]
	private GameObject distantAsteroidRowContainer;

	[SerializeField]
	private LocText clusterNameLabel;

	[SerializeField]
	private LocText clusterDifficultyLabel;

	[SerializeField]
	public LocText headerLabel;

	[SerializeField]
	public MultiToggle clusterDetailsButton;

	[SerializeField]
	public GameObject storyTraitHeader;

	private List<GameObject> labels = new List<GameObject>();

	[Header("Selected Asteroid Details")]
	[SerializeField]
	private GameObject SpacedOutContentContainer;

	public Image selectedAsteroidIcon;

	public LocText selectedAsteroidLabel;

	public LocText selectedAsteroidDescription;

	[SerializeField]
	private GameObject prefabAsteroidLine;

	private Dictionary<ProcGen.World, GameObject> asteroidLines = new Dictionary<ProcGen.World, GameObject>();

	private List<GameObject> traitWidgets = new List<GameObject>();

	private List<GameObject> traitCategoryWidgets = new List<GameObject>();

	private List<GameObject> parameterWidgets = new List<GameObject>();

	public bool HasDescriptors()
	{
		return labels.Count > 0;
	}

	public void EnableClusterDetails(bool setActive)
	{
		clusterNameLabel.gameObject.SetActive(setActive);
		clusterDifficultyLabel.gameObject.SetActive(setActive);
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	public void SetClusterDetailLabels(ColonyDestinationAsteroidBeltData cluster)
	{
		Strings.TryGet(cluster.properName, out var result);
		clusterNameLabel.SetText((result == null) ? "" : string.Format(WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, result.String));
		int index = Mathf.Clamp(cluster.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
		Tuple<string, string, string> tuple = ColonyDestinationAsteroidBeltData.survivalOptions[index];
		string text = string.Format(WORLDS.SURVIVAL_CHANCE.TITLE, tuple.first, tuple.third);
		text = text.Trim('\n');
		clusterDifficultyLabel.SetText(text);
	}

	public void SetParameterDescriptors(IList<AsteroidDescriptor> descriptors)
	{
		for (int i = 0; i < parameterWidgets.Count; i++)
		{
			UnityEngine.Object.Destroy(parameterWidgets[i]);
		}
		parameterWidgets.Clear();
		for (int j = 0; j < descriptors.Count; j++)
		{
			GameObject gameObject = Util.KInstantiateUI(prefabParameterWidget, base.gameObject, force_active: true);
			gameObject.GetComponent<LocText>().SetText(descriptors[j].text);
			ToolTip component = gameObject.GetComponent<ToolTip>();
			if (!string.IsNullOrEmpty(descriptors[j].tooltip))
			{
				component.SetSimpleTooltip(descriptors[j].tooltip);
			}
			parameterWidgets.Add(gameObject);
		}
	}

	private void ClearTraitDescriptors()
	{
		for (int i = 0; i < traitWidgets.Count; i++)
		{
			UnityEngine.Object.Destroy(traitWidgets[i]);
		}
		traitWidgets.Clear();
		for (int j = 0; j < traitCategoryWidgets.Count; j++)
		{
			UnityEngine.Object.Destroy(traitCategoryWidgets[j]);
		}
		traitCategoryWidgets.Clear();
	}

	public void SetTraitDescriptors(IList<AsteroidDescriptor> descriptors, List<string> stories, bool includeDescriptions = true)
	{
		foreach (string story in stories)
		{
			WorldTrait storyTrait = Db.Get().Stories.Get(story).StoryTrait;
			string tooltip = (DlcManager.IsPureVanilla() ? Strings.Get(storyTrait.description + "_SHORT") : Strings.Get(storyTrait.description));
			descriptors.Add(new AsteroidDescriptor(Strings.Get(storyTrait.name).String, tooltip, Color.white, null, storyTrait.icon));
		}
		SetTraitDescriptors(new List<IList<AsteroidDescriptor>> { descriptors }, includeDescriptions);
		if (stories.Count != 0)
		{
			storyTraitHeader.rectTransform().SetSiblingIndex(storyTraitHeader.rectTransform().parent.childCount - stories.Count - 1);
			storyTraitHeader.SetActive(value: true);
		}
		else
		{
			storyTraitHeader.SetActive(value: false);
		}
	}

	public void SetTraitDescriptors(IList<AsteroidDescriptor> descriptors, bool includeDescriptions = true)
	{
		SetTraitDescriptors(new List<IList<AsteroidDescriptor>> { descriptors }, includeDescriptions);
	}

	public void SetTraitDescriptors(List<IList<AsteroidDescriptor>> descriptorSets, bool includeDescriptions = true, List<Tuple<string, Sprite>> headerData = null)
	{
		ClearTraitDescriptors();
		for (int i = 0; i < descriptorSets.Count; i++)
		{
			IList<AsteroidDescriptor> list = descriptorSets[i];
			GameObject parent = base.gameObject;
			if (descriptorSets.Count > 1)
			{
				Debug.Assert(headerData != null, "Asteroid Header data is null - traits wont have their world as contex in the selection UI");
				GameObject gameObject = Util.KInstantiate(prefabTraitCategoryWidget, base.gameObject);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				gameObject.transform.localScale = Vector3.one;
				StringEntry result;
				string text = (Strings.TryGet(headerData[i].first, out result) ? result.String : headerData[i].first);
				component.GetReference<LocText>("NameLabel").SetText(text);
				component.GetReference<Image>("Icon").sprite = headerData[i].second;
				gameObject.SetActive(value: true);
				parent = component.GetReference<RectTransform>("Contents").gameObject;
				traitCategoryWidgets.Add(gameObject);
			}
			for (int j = 0; j < list.Count; j++)
			{
				GameObject gameObject2 = Util.KInstantiate(prefabTraitWidget, parent);
				HierarchyReferences component2 = gameObject2.GetComponent<HierarchyReferences>();
				gameObject2.SetActive(value: true);
				component2.GetReference<LocText>("NameLabel").SetText("<b>" + list[j].text + "</b>");
				Image reference = component2.GetReference<Image>("Icon");
				reference.color = list[j].associatedColor;
				if (list[j].associatedIcon != null)
				{
					Sprite sprite = Assets.GetSprite(list[j].associatedIcon);
					if (sprite != null)
					{
						reference.sprite = sprite;
					}
				}
				if (gameObject2.GetComponent<ToolTip>() != null)
				{
					gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(list[j].tooltip);
				}
				LocText reference2 = component2.GetReference<LocText>("DescLabel");
				if (includeDescriptions && !string.IsNullOrEmpty(list[j].tooltip))
				{
					reference2.SetText(list[j].tooltip);
				}
				else
				{
					reference2.gameObject.SetActive(value: false);
				}
				gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject2.SetActive(value: true);
				traitWidgets.Add(gameObject2);
			}
		}
	}

	public void EnableClusterLocationLabels(bool enable)
	{
		startingAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
		nearbyAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
		distantAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
	}

	public void RefreshAsteroidLines(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel, List<string> storyTraits)
	{
		cluster.RemixClusterLayout();
		foreach (KeyValuePair<ProcGen.World, GameObject> asteroidLine in asteroidLines)
		{
			if (!asteroidLine.Value.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(asteroidLine.Value);
			}
		}
		asteroidLines.Clear();
		SpawnAsteroidLine(cluster.GetStartWorld, startingAsteroidRowContainer, cluster);
		for (int i = 0; i < cluster.worlds.Count; i++)
		{
			ProcGen.World world = cluster.worlds[i];
			WorldPlacement worldPlacement = null;
			for (int j = 0; j < cluster.Layout.worldPlacements.Count; j++)
			{
				if (cluster.Layout.worldPlacements[j].world == world.filePath)
				{
					worldPlacement = cluster.Layout.worldPlacements[j];
					break;
				}
			}
			SpawnAsteroidLine(world, (worldPlacement.locationType == WorldPlacement.LocationType.InnerCluster) ? nearbyAsteroidRowContainer : distantAsteroidRowContainer, cluster);
		}
		foreach (KeyValuePair<ProcGen.World, GameObject> line in asteroidLines)
		{
			MultiToggle component = line.Value.GetComponent<MultiToggle>();
			component.onClick = (System.Action)Delegate.Combine(component.onClick, (System.Action)delegate
			{
				SelectAsteroidInCluster(line.Key, cluster, selectedAsteroidDetailsPanel);
			});
		}
		SelectWholeClusterDetails(cluster, selectedAsteroidDetailsPanel, storyTraits);
	}

	private void SelectAsteroidInCluster(ProcGen.World asteroid, ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel)
	{
		selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(value: true);
		clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(0);
		foreach (KeyValuePair<ProcGen.World, GameObject> asteroidLine in asteroidLines)
		{
			asteroidLine.Value.GetComponent<MultiToggle>().ChangeState((asteroidLine.Key == asteroid) ? 1 : 0);
			if (asteroidLine.Key == asteroid)
			{
				SetSelectedAsteroid(asteroidLine.Key, selectedAsteroidDetailsPanel, cluster.GenerateTraitDescriptors(asteroidLine.Key));
			}
		}
	}

	public void SelectWholeClusterDetails(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel, List<string> stories)
	{
		selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(value: false);
		foreach (KeyValuePair<ProcGen.World, GameObject> asteroidLine in asteroidLines)
		{
			asteroidLine.Value.GetComponent<MultiToggle>().ChangeState(0);
		}
		SetSelectedCluster(cluster, selectedAsteroidDetailsPanel, stories);
		clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(1);
	}

	private void SpawnAsteroidLine(ProcGen.World asteroid, GameObject parentContainer, ColonyDestinationAsteroidBeltData cluster)
	{
		if (asteroidLines.ContainsKey(asteroid))
		{
			return;
		}
		GameObject gameObject = Util.KInstantiateUI(prefabAsteroidLine, parentContainer.gameObject, force_active: true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		Image reference = component.GetReference<Image>("Icon");
		LocText reference2 = component.GetReference<LocText>("Label");
		RectTransform reference3 = component.GetReference<RectTransform>("TraitsRow");
		LocText reference4 = component.GetReference<LocText>("TraitLabel");
		ToolTip component2 = gameObject.GetComponent<ToolTip>();
		Image component3 = gameObject.transform.Find("DlcBanner").GetComponent<Image>();
		Sprite uISprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
		reference.sprite = uISprite;
		reference2.SetText(asteroid.GetProperName());
		List<WorldTrait> worldTraits = cluster.GetWorldTraits(asteroid);
		reference4.gameObject.SetActive(worldTraits.Count == 0);
		reference4.SetText(UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS);
		RectTransform reference5 = component.GetReference<RectTransform>("TraitIconPrefab");
		foreach (WorldTrait item in worldTraits)
		{
			Image component4 = Util.KInstantiateUI(reference5.gameObject, reference3.gameObject, force_active: true).GetComponent<Image>();
			Sprite sprite = Assets.GetSprite(item.filePath.Substring(item.filePath.LastIndexOf("/") + 1));
			if (sprite != null)
			{
				component4.sprite = sprite;
			}
			component4.color = Util.ColorFromHex(item.colorHex);
		}
		string text = "";
		if (worldTraits.Count > 0)
		{
			for (int i = 0; i < worldTraits.Count; i++)
			{
				Strings.TryGet(worldTraits[i].name, out var result);
				Strings.TryGet(worldTraits[i].description, out var result2);
				text = text + "<color=#" + worldTraits[i].colorHex + ">" + result.String + "</color>\n" + result2.String;
				if (i != worldTraits.Count - 1)
				{
					text += "\n\n";
				}
			}
		}
		else
		{
			text = UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS;
		}
		if (DlcManager.IsDlcId(asteroid.dlcIdFrom))
		{
			text = text + "\n\n" + string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_DLC_CONTENT, DlcManager.GetDlcTitle(asteroid.dlcIdFrom));
		}
		component2.SetSimpleTooltip(text);
		if (DlcManager.IsDlcId(asteroid.dlcIdFrom))
		{
			component3.color = DlcManager.GetDlcBannerColor(asteroid.dlcIdFrom);
			component3.gameObject.SetActive(value: true);
		}
		else
		{
			component3.gameObject.SetActive(value: false);
		}
		asteroidLines.Add(asteroid, gameObject);
	}

	private void SetSelectedAsteroid(ProcGen.World asteroid, AsteroidDescriptorPanel detailPanel, List<AsteroidDescriptor> traitDescriptors)
	{
		detailPanel.SetTraitDescriptors(traitDescriptors);
		detailPanel.selectedAsteroidIcon.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
		detailPanel.selectedAsteroidIcon.gameObject.SetActive(value: true);
		detailPanel.selectedAsteroidLabel.SetText(asteroid.GetProperName());
		detailPanel.selectedAsteroidDescription.SetText(asteroid.GetProperDescription());
	}

	private void SetSelectedCluster(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel detailPanel, List<string> stories)
	{
		List<IList<AsteroidDescriptor>> list = new List<IList<AsteroidDescriptor>>();
		List<Tuple<string, Sprite>> list2 = new List<Tuple<string, Sprite>>();
		List<AsteroidDescriptor> list3 = cluster.GenerateTraitDescriptors(cluster.GetStartWorld, includeDefaultTrait: false);
		if (list3.Count != 0)
		{
			list2.Add(new Tuple<string, Sprite>(cluster.GetStartWorld.name, ColonyDestinationAsteroidBeltData.GetUISprite(cluster.GetStartWorld.asteroidIcon)));
			list.Add(list3);
		}
		foreach (ProcGen.World world in cluster.worlds)
		{
			List<AsteroidDescriptor> list4 = cluster.GenerateTraitDescriptors(world, includeDefaultTrait: false);
			if (list4.Count != 0)
			{
				list2.Add(new Tuple<string, Sprite>(world.name, ColonyDestinationAsteroidBeltData.GetUISprite(world.asteroidIcon)));
				list.Add(list4);
			}
		}
		list2.Add(new Tuple<string, Sprite>("STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER", Assets.GetSprite("codexIconStoryTraits")));
		List<AsteroidDescriptor> list5 = new List<AsteroidDescriptor>();
		foreach (string story2 in stories)
		{
			Story story = Db.Get().Stories.Get(story2);
			string icon = story.StoryTrait.icon;
			AsteroidDescriptor item = new AsteroidDescriptor(Strings.Get(story.StoryTrait.name).String, Strings.Get(story.StoryTrait.description).String, Color.white, null, icon);
			list5.Add(item);
		}
		list.Add(list5);
		detailPanel.SetTraitDescriptors(list, includeDescriptions: false, list2);
		detailPanel.selectedAsteroidIcon.gameObject.SetActive(value: false);
		string text = cluster.properName;
		if (Strings.TryGet(cluster.properName, out var result))
		{
			text = result.String;
		}
		detailPanel.selectedAsteroidLabel.SetText(text);
		detailPanel.selectedAsteroidDescription.SetText("");
	}
}
