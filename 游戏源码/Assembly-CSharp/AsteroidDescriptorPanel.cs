using System;
using System.Collections.Generic;
using Database;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C01 RID: 7169
[AddComponentMenu("KMonoBehaviour/scripts/AsteroidDescriptorPanel")]
public class AsteroidDescriptorPanel : KMonoBehaviour
{
	// Token: 0x060094E7 RID: 38119 RVA: 0x00100F6E File Offset: 0x000FF16E
	public bool HasDescriptors()
	{
		return this.labels.Count > 0;
	}

	// Token: 0x060094E8 RID: 38120 RVA: 0x00100F7E File Offset: 0x000FF17E
	public void EnableClusterDetails(bool setActive)
	{
		this.clusterNameLabel.gameObject.SetActive(setActive);
		this.clusterDifficultyLabel.gameObject.SetActive(setActive);
	}

	// Token: 0x060094E9 RID: 38121 RVA: 0x000B2F5A File Offset: 0x000B115A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060094EA RID: 38122 RVA: 0x003972B8 File Offset: 0x003954B8
	public void SetClusterDetailLabels(ColonyDestinationAsteroidBeltData cluster)
	{
		StringEntry stringEntry;
		Strings.TryGet(cluster.properName, out stringEntry);
		this.clusterNameLabel.SetText((stringEntry == null) ? "" : string.Format(WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, stringEntry.String));
		int index = Mathf.Clamp(cluster.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
		global::Tuple<string, string, string> tuple = ColonyDestinationAsteroidBeltData.survivalOptions[index];
		string text = string.Format(WORLDS.SURVIVAL_CHANCE.TITLE, tuple.first, tuple.third);
		text = text.Trim('\n');
		this.clusterDifficultyLabel.SetText(text);
	}

	// Token: 0x060094EB RID: 38123 RVA: 0x00397354 File Offset: 0x00395554
	public void SetParameterDescriptors(IList<AsteroidDescriptor> descriptors)
	{
		for (int i = 0; i < this.parameterWidgets.Count; i++)
		{
			UnityEngine.Object.Destroy(this.parameterWidgets[i]);
		}
		this.parameterWidgets.Clear();
		for (int j = 0; j < descriptors.Count; j++)
		{
			GameObject gameObject = global::Util.KInstantiateUI(this.prefabParameterWidget, base.gameObject, true);
			gameObject.GetComponent<LocText>().SetText(descriptors[j].text);
			ToolTip component = gameObject.GetComponent<ToolTip>();
			if (!string.IsNullOrEmpty(descriptors[j].tooltip))
			{
				component.SetSimpleTooltip(descriptors[j].tooltip);
			}
			this.parameterWidgets.Add(gameObject);
		}
	}

	// Token: 0x060094EC RID: 38124 RVA: 0x00397408 File Offset: 0x00395608
	private void ClearTraitDescriptors()
	{
		for (int i = 0; i < this.traitWidgets.Count; i++)
		{
			UnityEngine.Object.Destroy(this.traitWidgets[i]);
		}
		this.traitWidgets.Clear();
		for (int j = 0; j < this.traitCategoryWidgets.Count; j++)
		{
			UnityEngine.Object.Destroy(this.traitCategoryWidgets[j]);
		}
		this.traitCategoryWidgets.Clear();
	}

	// Token: 0x060094ED RID: 38125 RVA: 0x0039747C File Offset: 0x0039567C
	public void SetTraitDescriptors(IList<AsteroidDescriptor> descriptors, List<string> stories, bool includeDescriptions = true)
	{
		foreach (string id in stories)
		{
			WorldTrait storyTrait = Db.Get().Stories.Get(id).StoryTrait;
			string tooltip = DlcManager.IsPureVanilla() ? Strings.Get(storyTrait.description + "_SHORT") : Strings.Get(storyTrait.description);
			descriptors.Add(new AsteroidDescriptor(Strings.Get(storyTrait.name).String, tooltip, Color.white, null, storyTrait.icon));
		}
		this.SetTraitDescriptors(new List<IList<AsteroidDescriptor>>
		{
			descriptors
		}, includeDescriptions, null);
		if (stories.Count != 0)
		{
			this.storyTraitHeader.rectTransform().SetSiblingIndex(this.storyTraitHeader.rectTransform().parent.childCount - stories.Count - 1);
			this.storyTraitHeader.SetActive(true);
			return;
		}
		this.storyTraitHeader.SetActive(false);
	}

	// Token: 0x060094EE RID: 38126 RVA: 0x00100FA2 File Offset: 0x000FF1A2
	public void SetTraitDescriptors(IList<AsteroidDescriptor> descriptors, bool includeDescriptions = true)
	{
		this.SetTraitDescriptors(new List<IList<AsteroidDescriptor>>
		{
			descriptors
		}, includeDescriptions, null);
	}

	// Token: 0x060094EF RID: 38127 RVA: 0x00397594 File Offset: 0x00395794
	public void SetTraitDescriptors(List<IList<AsteroidDescriptor>> descriptorSets, bool includeDescriptions = true, List<global::Tuple<string, Sprite>> headerData = null)
	{
		this.ClearTraitDescriptors();
		for (int i = 0; i < descriptorSets.Count; i++)
		{
			IList<AsteroidDescriptor> list = descriptorSets[i];
			GameObject gameObject = base.gameObject;
			if (descriptorSets.Count > 1)
			{
				global::Debug.Assert(headerData != null, "Asteroid Header data is null - traits wont have their world as contex in the selection UI");
				GameObject gameObject2 = global::Util.KInstantiate(this.prefabTraitCategoryWidget, base.gameObject, null);
				HierarchyReferences component = gameObject2.GetComponent<HierarchyReferences>();
				gameObject2.transform.localScale = Vector3.one;
				StringEntry stringEntry;
				string text = Strings.TryGet(headerData[i].first, out stringEntry) ? stringEntry.String : headerData[i].first;
				component.GetReference<LocText>("NameLabel").SetText(text);
				component.GetReference<Image>("Icon").sprite = headerData[i].second;
				gameObject2.SetActive(true);
				gameObject = component.GetReference<RectTransform>("Contents").gameObject;
				this.traitCategoryWidgets.Add(gameObject2);
			}
			for (int j = 0; j < list.Count; j++)
			{
				GameObject gameObject3 = global::Util.KInstantiate(this.prefabTraitWidget, gameObject, null);
				HierarchyReferences component2 = gameObject3.GetComponent<HierarchyReferences>();
				gameObject3.SetActive(true);
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
				if (gameObject3.GetComponent<ToolTip>() != null)
				{
					gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(list[j].tooltip);
				}
				LocText reference2 = component2.GetReference<LocText>("DescLabel");
				if (includeDescriptions && !string.IsNullOrEmpty(list[j].tooltip))
				{
					reference2.SetText(list[j].tooltip);
				}
				else
				{
					reference2.gameObject.SetActive(false);
				}
				gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject3.SetActive(true);
				this.traitWidgets.Add(gameObject3);
			}
		}
	}

	// Token: 0x060094F0 RID: 38128 RVA: 0x003977F4 File Offset: 0x003959F4
	public void EnableClusterLocationLabels(bool enable)
	{
		this.startingAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
		this.nearbyAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
		this.distantAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
	}

	// Token: 0x060094F1 RID: 38129 RVA: 0x00397854 File Offset: 0x00395A54
	public void RefreshAsteroidLines(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel, List<string> storyTraits)
	{
		cluster.RemixClusterLayout();
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			if (!keyValuePair.Value.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(keyValuePair.Value);
			}
		}
		this.asteroidLines.Clear();
		this.SpawnAsteroidLine(cluster.GetStartWorld, this.startingAsteroidRowContainer, cluster);
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
			this.SpawnAsteroidLine(world, (worldPlacement.locationType == WorldPlacement.LocationType.InnerCluster) ? this.nearbyAsteroidRowContainer : this.distantAsteroidRowContainer, cluster);
		}
		using (Dictionary<ProcGen.World, GameObject>.Enumerator enumerator = this.asteroidLines.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ProcGen.World, GameObject> line = enumerator.Current;
				MultiToggle component = line.Value.GetComponent<MultiToggle>();
				component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
				{
					this.SelectAsteroidInCluster(line.Key, cluster, selectedAsteroidDetailsPanel);
				}));
			}
		}
		this.SelectWholeClusterDetails(cluster, selectedAsteroidDetailsPanel, storyTraits);
	}

	// Token: 0x060094F2 RID: 38130 RVA: 0x00397A64 File Offset: 0x00395C64
	private void SelectAsteroidInCluster(ProcGen.World asteroid, ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel)
	{
		selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(true);
		this.clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(0);
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == asteroid) ? 1 : 0);
			if (keyValuePair.Key == asteroid)
			{
				this.SetSelectedAsteroid(keyValuePair.Key, selectedAsteroidDetailsPanel, cluster.GenerateTraitDescriptors(keyValuePair.Key, true));
			}
		}
	}

	// Token: 0x060094F3 RID: 38131 RVA: 0x00397B14 File Offset: 0x00395D14
	public void SelectWholeClusterDetails(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel, List<string> stories)
	{
		selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(false);
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState(0);
		}
		this.SetSelectedCluster(cluster, selectedAsteroidDetailsPanel, stories);
		this.clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(1);
	}

	// Token: 0x060094F4 RID: 38132 RVA: 0x00397B98 File Offset: 0x00395D98
	private void SpawnAsteroidLine(ProcGen.World asteroid, GameObject parentContainer, ColonyDestinationAsteroidBeltData cluster)
	{
		if (this.asteroidLines.ContainsKey(asteroid))
		{
			return;
		}
		GameObject gameObject = global::Util.KInstantiateUI(this.prefabAsteroidLine, parentContainer.gameObject, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		Image reference = component.GetReference<Image>("Icon");
		LocText reference2 = component.GetReference<LocText>("Label");
		RectTransform reference3 = component.GetReference<RectTransform>("TraitsRow");
		LocText reference4 = component.GetReference<LocText>("TraitLabel");
		ToolTip component2 = gameObject.GetComponent<ToolTip>();
		Image component3 = gameObject.transform.Find("DlcBanner").GetComponent<Image>();
		Sprite uisprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
		reference.sprite = uisprite;
		reference2.SetText(asteroid.GetProperName());
		List<WorldTrait> worldTraits = cluster.GetWorldTraits(asteroid);
		reference4.gameObject.SetActive(worldTraits.Count == 0);
		reference4.SetText(UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS);
		RectTransform reference5 = component.GetReference<RectTransform>("TraitIconPrefab");
		foreach (WorldTrait worldTrait in worldTraits)
		{
			Image component4 = global::Util.KInstantiateUI(reference5.gameObject, reference3.gameObject, true).GetComponent<Image>();
			Sprite sprite = Assets.GetSprite(worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1));
			if (sprite != null)
			{
				component4.sprite = sprite;
			}
			component4.color = global::Util.ColorFromHex(worldTrait.colorHex);
		}
		string text = "";
		if (worldTraits.Count > 0)
		{
			for (int i = 0; i < worldTraits.Count; i++)
			{
				StringEntry stringEntry;
				Strings.TryGet(worldTraits[i].name, out stringEntry);
				StringEntry stringEntry2;
				Strings.TryGet(worldTraits[i].description, out stringEntry2);
				text = string.Concat(new string[]
				{
					text,
					"<color=#",
					worldTraits[i].colorHex,
					">",
					stringEntry.String,
					"</color>\n",
					stringEntry2.String
				});
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
			component3.gameObject.SetActive(true);
		}
		else
		{
			component3.gameObject.SetActive(false);
		}
		this.asteroidLines.Add(asteroid, gameObject);
	}

	// Token: 0x060094F5 RID: 38133 RVA: 0x00397E78 File Offset: 0x00396078
	private void SetSelectedAsteroid(ProcGen.World asteroid, AsteroidDescriptorPanel detailPanel, List<AsteroidDescriptor> traitDescriptors)
	{
		detailPanel.SetTraitDescriptors(traitDescriptors, true);
		detailPanel.selectedAsteroidIcon.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
		detailPanel.selectedAsteroidIcon.gameObject.SetActive(true);
		detailPanel.selectedAsteroidLabel.SetText(asteroid.GetProperName());
		detailPanel.selectedAsteroidDescription.SetText(asteroid.GetProperDescription());
	}

	// Token: 0x060094F6 RID: 38134 RVA: 0x00397ED8 File Offset: 0x003960D8
	private void SetSelectedCluster(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel detailPanel, List<string> stories)
	{
		List<IList<AsteroidDescriptor>> list = new List<IList<AsteroidDescriptor>>();
		List<global::Tuple<string, Sprite>> list2 = new List<global::Tuple<string, Sprite>>();
		List<AsteroidDescriptor> list3 = cluster.GenerateTraitDescriptors(cluster.GetStartWorld, false);
		if (list3.Count != 0)
		{
			list2.Add(new global::Tuple<string, Sprite>(cluster.GetStartWorld.name, ColonyDestinationAsteroidBeltData.GetUISprite(cluster.GetStartWorld.asteroidIcon)));
			list.Add(list3);
		}
		foreach (ProcGen.World world in cluster.worlds)
		{
			List<AsteroidDescriptor> list4 = cluster.GenerateTraitDescriptors(world, false);
			if (list4.Count != 0)
			{
				list2.Add(new global::Tuple<string, Sprite>(world.name, ColonyDestinationAsteroidBeltData.GetUISprite(world.asteroidIcon)));
				list.Add(list4);
			}
		}
		list2.Add(new global::Tuple<string, Sprite>("STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER", Assets.GetSprite("codexIconStoryTraits")));
		List<AsteroidDescriptor> list5 = new List<AsteroidDescriptor>();
		foreach (string id in stories)
		{
			Story story = Db.Get().Stories.Get(id);
			string icon = story.StoryTrait.icon;
			AsteroidDescriptor item = new AsteroidDescriptor(Strings.Get(story.StoryTrait.name).String, Strings.Get(story.StoryTrait.description).String, Color.white, null, icon);
			list5.Add(item);
		}
		list.Add(list5);
		detailPanel.SetTraitDescriptors(list, false, list2);
		detailPanel.selectedAsteroidIcon.gameObject.SetActive(false);
		string text = cluster.properName;
		StringEntry stringEntry;
		if (Strings.TryGet(cluster.properName, out stringEntry))
		{
			text = stringEntry.String;
		}
		detailPanel.selectedAsteroidLabel.SetText(text);
		detailPanel.selectedAsteroidDescription.SetText("");
	}

	// Token: 0x04007369 RID: 29545
	[Header("Destination Details")]
	[SerializeField]
	private GameObject customLabelPrefab;

	// Token: 0x0400736A RID: 29546
	[SerializeField]
	private GameObject prefabTraitWidget;

	// Token: 0x0400736B RID: 29547
	[SerializeField]
	private GameObject prefabTraitCategoryWidget;

	// Token: 0x0400736C RID: 29548
	[SerializeField]
	private GameObject prefabParameterWidget;

	// Token: 0x0400736D RID: 29549
	[SerializeField]
	private GameObject startingAsteroidRowContainer;

	// Token: 0x0400736E RID: 29550
	[SerializeField]
	private GameObject nearbyAsteroidRowContainer;

	// Token: 0x0400736F RID: 29551
	[SerializeField]
	private GameObject distantAsteroidRowContainer;

	// Token: 0x04007370 RID: 29552
	[SerializeField]
	private LocText clusterNameLabel;

	// Token: 0x04007371 RID: 29553
	[SerializeField]
	private LocText clusterDifficultyLabel;

	// Token: 0x04007372 RID: 29554
	[SerializeField]
	public LocText headerLabel;

	// Token: 0x04007373 RID: 29555
	[SerializeField]
	public MultiToggle clusterDetailsButton;

	// Token: 0x04007374 RID: 29556
	[SerializeField]
	public GameObject storyTraitHeader;

	// Token: 0x04007375 RID: 29557
	private List<GameObject> labels = new List<GameObject>();

	// Token: 0x04007376 RID: 29558
	[Header("Selected Asteroid Details")]
	[SerializeField]
	private GameObject SpacedOutContentContainer;

	// Token: 0x04007377 RID: 29559
	public Image selectedAsteroidIcon;

	// Token: 0x04007378 RID: 29560
	public LocText selectedAsteroidLabel;

	// Token: 0x04007379 RID: 29561
	public LocText selectedAsteroidDescription;

	// Token: 0x0400737A RID: 29562
	[SerializeField]
	private GameObject prefabAsteroidLine;

	// Token: 0x0400737B RID: 29563
	private Dictionary<ProcGen.World, GameObject> asteroidLines = new Dictionary<ProcGen.World, GameObject>();

	// Token: 0x0400737C RID: 29564
	private List<GameObject> traitWidgets = new List<GameObject>();

	// Token: 0x0400737D RID: 29565
	private List<GameObject> traitCategoryWidgets = new List<GameObject>();

	// Token: 0x0400737E RID: 29566
	private List<GameObject> parameterWidgets = new List<GameObject>();
}
