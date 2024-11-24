using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001ECF RID: 7887
public class ResearchScreenSideBar : KScreen
{
	// Token: 0x0600A5E1 RID: 42465 RVA: 0x003EFA6C File Offset: 0x003EDC6C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.PopualteProjects();
		this.PopulateFilterButtons();
		this.RefreshCategoriesContentExpanded();
		this.RefreshWidgets();
		this.searchBox.OnValueChangesPaused = delegate()
		{
			this.UpdateCurrentSearch(this.searchBox.text);
		};
		KInputTextField kinputTextField = this.searchBox;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(delegate()
		{
			base.isEditing = true;
		}));
		this.searchBox.onEndEdit.AddListener(delegate(string value)
		{
			base.isEditing = false;
		});
		this.clearSearchButton.onClick += delegate()
		{
			this.ResetFilter();
		};
		this.ConfigCompletionFilters();
		base.ConsumeMouseScroll = true;
		Game.Instance.Subscribe(-107300940, new Action<object>(this.UpdateProjectFilter));
	}

	// Token: 0x0600A5E2 RID: 42466 RVA: 0x003EFB34 File Offset: 0x003EDD34
	private void Update()
	{
		for (int i = 0; i < Math.Min(this.QueuedActivations.Count, this.activationPerFrame); i++)
		{
			this.QueuedActivations[i].SetActive(true);
		}
		this.QueuedActivations.RemoveRange(0, Math.Min(this.QueuedActivations.Count, this.activationPerFrame));
		for (int j = 0; j < Math.Min(this.QueuedDeactivations.Count, this.activationPerFrame); j++)
		{
			this.QueuedDeactivations[j].SetActive(false);
		}
		this.QueuedDeactivations.RemoveRange(0, Math.Min(this.QueuedDeactivations.Count, this.activationPerFrame));
	}

	// Token: 0x0600A5E3 RID: 42467 RVA: 0x003EFBEC File Offset: 0x003EDDEC
	private void ConfigCompletionFilters()
	{
		MultiToggle multiToggle = this.allFilter;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All);
		}));
		MultiToggle multiToggle2 = this.completedFilter;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.Completed);
		}));
		MultiToggle multiToggle3 = this.availableFilter;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(delegate()
		{
			this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.Available);
		}));
		this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All);
	}

	// Token: 0x0600A5E4 RID: 42468 RVA: 0x003EFC78 File Offset: 0x003EDE78
	private void SetCompletionFilter(ResearchScreenSideBar.CompletionState state)
	{
		this.completionFilter = state;
		this.allFilter.GetComponent<MultiToggle>().ChangeState((this.completionFilter == ResearchScreenSideBar.CompletionState.All) ? 1 : 0);
		this.completedFilter.GetComponent<MultiToggle>().ChangeState((this.completionFilter == ResearchScreenSideBar.CompletionState.Completed) ? 1 : 0);
		this.availableFilter.GetComponent<MultiToggle>().ChangeState((this.completionFilter == ResearchScreenSideBar.CompletionState.Available) ? 1 : 0);
		this.UpdateProjectFilter(null);
	}

	// Token: 0x0600A5E5 RID: 42469 RVA: 0x0010BA50 File Offset: 0x00109C50
	public override float GetSortKey()
	{
		if (base.isEditing)
		{
			return 50f;
		}
		return 21f;
	}

	// Token: 0x0600A5E6 RID: 42470 RVA: 0x003EFCEC File Offset: 0x003EDEEC
	public override void OnKeyDown(KButtonEvent e)
	{
		if (this.researchScreen != null && this.researchScreen.canvas && !this.researchScreen.canvas.enabled)
		{
			return;
		}
		if (base.isEditing)
		{
			e.Consumed = true;
			return;
		}
		if (!e.Consumed)
		{
			Vector2 vector = base.transform.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (vector.x >= 0f && vector.x <= base.transform.rectTransform().rect.width)
			{
				if (e.TryConsume(global::Action.MouseRight))
				{
					return;
				}
				if (e.TryConsume(global::Action.MouseLeft))
				{
					return;
				}
				if (!KInputManager.currentControllerIsGamepad)
				{
					if (e.TryConsume(global::Action.ZoomIn))
					{
						return;
					}
					e.TryConsume(global::Action.ZoomOut);
					return;
				}
			}
		}
	}

	// Token: 0x0600A5E7 RID: 42471 RVA: 0x0010BA65 File Offset: 0x00109C65
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.RefreshWidgets();
	}

	// Token: 0x0600A5E8 RID: 42472 RVA: 0x003EFDBC File Offset: 0x003EDFBC
	private void UpdateCurrentSearch(string newValue)
	{
		if (base.isEditing)
		{
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.filterButtons)
			{
				this.filterStates[keyValuePair.Key] = false;
				keyValuePair.Value.GetComponent<MultiToggle>().ChangeState(0);
			}
		}
		this.currentSearchString = newValue;
		this.UpdateProjectFilter(null);
	}

	// Token: 0x0600A5E9 RID: 42473 RVA: 0x003EFE44 File Offset: 0x003EE044
	private void UpdateProjectFilter(object data = null)
	{
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectCategories)
		{
			dictionary.Add(keyValuePair.Key, false);
		}
		this.RefreshProjectsActive();
		foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.projectTechs)
		{
			if ((keyValuePair2.Value.activeSelf || this.QueuedActivations.Contains(keyValuePair2.Value)) && !this.QueuedDeactivations.Contains(keyValuePair2.Value))
			{
				dictionary[Db.Get().Techs.Get(keyValuePair2.Key).category] = true;
				this.categoryExpanded[Db.Get().Techs.Get(keyValuePair2.Key).category] = true;
			}
		}
		foreach (KeyValuePair<string, bool> keyValuePair3 in dictionary)
		{
			this.ChangeGameObjectActive(this.projectCategories[keyValuePair3.Key], keyValuePair3.Value);
		}
		this.RefreshCategoriesContentExpanded();
	}

	// Token: 0x0600A5EA RID: 42474 RVA: 0x003EFFC8 File Offset: 0x003EE1C8
	private void RefreshProjectsActive()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectTechs)
		{
			bool flag = this.CheckTechPassesFilters(keyValuePair.Key);
			this.ChangeGameObjectActive(keyValuePair.Value, flag);
			this.researchScreen.GetEntry(Db.Get().Techs.Get(keyValuePair.Key)).UpdateFilterState(flag);
			foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.projectTechItems[keyValuePair.Key])
			{
				bool flag2 = this.CheckTechItemPassesFilters(keyValuePair2.Key);
				HierarchyReferences component = keyValuePair2.Value.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").color = (flag2 ? Color.white : Color.grey);
				component.GetReference<Image>("Icon").color = (flag2 ? Color.white : new Color(1f, 1f, 1f, 0.5f));
			}
		}
	}

	// Token: 0x0600A5EB RID: 42475 RVA: 0x003F012C File Offset: 0x003EE32C
	private void RefreshCategoriesContentExpanded()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectCategories)
		{
			keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").gameObject.SetActive(this.categoryExpanded[keyValuePair.Key]);
			keyValuePair.Value.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").ChangeState(this.categoryExpanded[keyValuePair.Key] ? 1 : 0);
		}
	}

	// Token: 0x0600A5EC RID: 42476 RVA: 0x003F01E0 File Offset: 0x003EE3E0
	private void PopualteProjects()
	{
		List<global::Tuple<global::Tuple<string, GameObject>, int>> list = new List<global::Tuple<global::Tuple<string, GameObject>, int>>();
		for (int i = 0; i < Db.Get().Techs.Count; i++)
		{
			Tech tech = (Tech)Db.Get().Techs.GetResource(i);
			if (!this.projectCategories.ContainsKey(tech.category))
			{
				string categoryID = tech.category;
				GameObject gameObject = Util.KInstantiateUI(this.techCategoryPrefabAlt, this.projectsContainer, true);
				gameObject.name = categoryID;
				gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("Label").SetText(Strings.Get("STRINGS.RESEARCH.TREES.TITLE" + categoryID.ToUpper()));
				this.categoryExpanded.Add(categoryID, false);
				this.projectCategories.Add(categoryID, gameObject);
				gameObject.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("Toggle").onClick = delegate()
				{
					this.categoryExpanded[categoryID] = !this.categoryExpanded[categoryID];
					this.RefreshCategoriesContentExpanded();
				};
			}
			GameObject gameObject2 = this.SpawnTechWidget(tech.Id, this.projectCategories[tech.category]);
			list.Add(new global::Tuple<global::Tuple<string, GameObject>, int>(new global::Tuple<string, GameObject>(tech.Id, gameObject2), tech.tier));
			this.projectTechs.Add(tech.Id, gameObject2);
			gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(tech.desc);
			MultiToggle component = gameObject2.GetComponent<MultiToggle>();
			component.onEnter = (System.Action)Delegate.Combine(component.onEnter, new System.Action(delegate()
			{
				this.researchScreen.TurnEverythingOff();
				this.researchScreen.GetEntry(tech).OnHover(true, tech);
				this.soundPlayer.Play(1);
			}));
			MultiToggle component2 = gameObject2.GetComponent<MultiToggle>();
			component2.onExit = (System.Action)Delegate.Combine(component2.onExit, new System.Action(delegate()
			{
				this.researchScreen.TurnEverythingOff();
			}));
		}
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.projectTechs)
		{
			Transform reference = this.projectCategories[Db.Get().Techs.Get(keyValuePair.Key).category].GetComponent<HierarchyReferences>().GetReference<Transform>("Content");
			this.projectTechs[keyValuePair.Key].transform.SetParent(reference);
		}
		list.Sort((global::Tuple<global::Tuple<string, GameObject>, int> a, global::Tuple<global::Tuple<string, GameObject>, int> b) => a.second.CompareTo(b.second));
		foreach (global::Tuple<global::Tuple<string, GameObject>, int> tuple in list)
		{
			tuple.first.second.transform.SetAsLastSibling();
		}
	}

	// Token: 0x0600A5ED RID: 42477 RVA: 0x003F04F0 File Offset: 0x003EE6F0
	private void PopulateFilterButtons()
	{
		using (Dictionary<string, List<Tag>>.Enumerator enumerator = this.filterPresets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, List<Tag>> kvp = enumerator.Current;
				GameObject gameObject = Util.KInstantiateUI(this.filterButtonPrefab, this.searchFiltersContainer, true);
				this.filterButtons.Add(kvp.Key, gameObject);
				this.filterStates.Add(kvp.Key, false);
				MultiToggle toggle = gameObject.GetComponent<MultiToggle>();
				gameObject.GetComponentInChildren<LocText>().SetText(Strings.Get("STRINGS.UI.RESEARCHSCREEN.FILTER_BUTTONS." + kvp.Key.ToUpper()));
				MultiToggle toggle2 = toggle;
				toggle2.onClick = (System.Action)Delegate.Combine(toggle2.onClick, new System.Action(delegate()
				{
					foreach (KeyValuePair<string, GameObject> keyValuePair in this.filterButtons)
					{
						if (keyValuePair.Key != kvp.Key)
						{
							this.filterStates[keyValuePair.Key] = false;
							this.filterButtons[keyValuePair.Key].GetComponent<MultiToggle>().ChangeState(this.filterStates[keyValuePair.Key] ? 1 : 0);
						}
					}
					this.filterStates[kvp.Key] = !this.filterStates[kvp.Key];
					toggle.ChangeState(this.filterStates[kvp.Key] ? 1 : 0);
					if (this.filterStates[kvp.Key])
					{
						StringEntry stringEntry = Strings.Get("STRINGS.UI.RESEARCHSCREEN.FILTER_BUTTONS." + kvp.Key.ToUpper());
						this.searchBox.text = stringEntry.String;
						return;
					}
					this.searchBox.text = "";
				}));
			}
		}
	}

	// Token: 0x0600A5EE RID: 42478 RVA: 0x000A5E40 File Offset: 0x000A4040
	public void RefreshQueue()
	{
	}

	// Token: 0x0600A5EF RID: 42479 RVA: 0x003F05F8 File Offset: 0x003EE7F8
	private void RefreshWidgets()
	{
		List<TechInstance> researchQueue = Research.Instance.GetResearchQueue();
		using (Dictionary<string, GameObject>.Enumerator enumerator = this.projectTechs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, GameObject> kvp = enumerator.Current;
				if (Db.Get().Techs.Get(kvp.Key).IsComplete())
				{
					kvp.Value.GetComponent<MultiToggle>().ChangeState(2);
				}
				else if (researchQueue.Find((TechInstance match) => match.tech.Id == kvp.Key) != null)
				{
					kvp.Value.GetComponent<MultiToggle>().ChangeState(1);
				}
				else
				{
					kvp.Value.GetComponent<MultiToggle>().ChangeState(0);
				}
			}
		}
	}

	// Token: 0x0600A5F0 RID: 42480 RVA: 0x003F06DC File Offset: 0x003EE8DC
	private void RefreshWidgetProgressBars(string techID, GameObject widget)
	{
		HierarchyReferences component = widget.GetComponent<HierarchyReferences>();
		ResearchPointInventory progressInventory = Research.Instance.GetTechInstance(techID).progressInventory;
		int num = 0;
		for (int i = 0; i < Research.Instance.researchTypes.Types.Count; i++)
		{
			if (Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID.ContainsKey(Research.Instance.researchTypes.Types[i].id) && Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[i].id] > 0f)
			{
				HierarchyReferences component2 = component.GetReference<RectTransform>("BarRows").GetChild(1 + num).GetComponent<HierarchyReferences>();
				float num2 = progressInventory.PointsByTypeID[Research.Instance.researchTypes.Types[i].id] / Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[i].id];
				RectTransform rectTransform = component2.GetReference<Image>("Bar").rectTransform;
				rectTransform.sizeDelta = new Vector2(rectTransform.parent.rectTransform().rect.width * num2, rectTransform.sizeDelta.y);
				component2.GetReference<LocText>("Label").SetText(progressInventory.PointsByTypeID[Research.Instance.researchTypes.Types[i].id].ToString() + "/" + Research.Instance.GetTechInstance(techID).tech.costsByResearchTypeID[Research.Instance.researchTypes.Types[i].id].ToString());
				num++;
			}
		}
	}

	// Token: 0x0600A5F1 RID: 42481 RVA: 0x003F08E4 File Offset: 0x003EEAE4
	private GameObject SpawnTechWidget(string techID, GameObject parentContainer)
	{
		GameObject gameObject = Util.KInstantiateUI(this.techWidgetRootAltPrefab, parentContainer, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		gameObject.name = Db.Get().Techs.Get(techID).Name;
		component.GetReference<LocText>("Label").SetText(Db.Get().Techs.Get(techID).Name);
		if (!this.projectTechItems.ContainsKey(techID))
		{
			this.projectTechItems.Add(techID, new Dictionary<string, GameObject>());
		}
		RectTransform reference = component.GetReference<RectTransform>("UnlockContainer");
		System.Action <>9__0;
		foreach (TechItem techItem in Db.Get().Techs.Get(techID).unlockedItems)
		{
			if (SaveLoader.Instance.IsCorrectDlcActiveForCurrentSave(techItem.requiredDlcIds, techItem.forbiddenDlcIds))
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.techItemPrefab, reference.gameObject, true);
				gameObject2.GetComponentsInChildren<Image>()[1].sprite = techItem.UISprite();
				gameObject2.GetComponentsInChildren<LocText>()[0].SetText(techItem.Name);
				MultiToggle component2 = gameObject2.GetComponent<MultiToggle>();
				Delegate onClick = component2.onClick;
				System.Action b;
				if ((b = <>9__0) == null)
				{
					b = (<>9__0 = delegate()
					{
						this.researchScreen.ZoomToTech(techID);
					});
				}
				component2.onClick = (System.Action)Delegate.Combine(onClick, b);
				gameObject2.GetComponentsInChildren<Image>()[0].color = (this.evenRow ? this.evenRowColor : this.oddRowColor);
				this.evenRow = !this.evenRow;
				if (!this.projectTechItems[techID].ContainsKey(techItem.Id))
				{
					this.projectTechItems[techID].Add(techItem.Id, gameObject2);
				}
			}
		}
		MultiToggle component3 = gameObject.GetComponent<MultiToggle>();
		component3.onClick = (System.Action)Delegate.Combine(component3.onClick, new System.Action(delegate()
		{
			this.researchScreen.ZoomToTech(techID);
		}));
		return gameObject;
	}

	// Token: 0x0600A5F2 RID: 42482 RVA: 0x003F0B34 File Offset: 0x003EED34
	private void ChangeGameObjectActive(GameObject target, bool targetActiveState)
	{
		if (target.activeSelf != targetActiveState)
		{
			if (targetActiveState)
			{
				this.QueuedActivations.Add(target);
				if (this.QueuedDeactivations.Contains(target))
				{
					this.QueuedDeactivations.Remove(target);
					return;
				}
			}
			else
			{
				this.QueuedDeactivations.Add(target);
				if (this.QueuedActivations.Contains(target))
				{
					this.QueuedActivations.Remove(target);
				}
			}
		}
	}

	// Token: 0x0600A5F3 RID: 42483 RVA: 0x003F0B9C File Offset: 0x003EED9C
	private bool CheckTechItemPassesFilters(string techItemID)
	{
		TechItem techItem = Db.Get().TechItems.Get(techItemID);
		bool flag = true;
		switch (this.completionFilter)
		{
		case ResearchScreenSideBar.CompletionState.Available:
			flag = (flag && !techItem.IsComplete() && techItem.ParentTech.ArePrerequisitesComplete());
			break;
		case ResearchScreenSideBar.CompletionState.Completed:
			flag = (flag && techItem.IsComplete());
			break;
		}
		if (!flag)
		{
			return flag;
		}
		flag = (flag && ResearchScreen.TechItemPassesSearchFilter(techItemID, this.currentSearchString));
		foreach (KeyValuePair<string, bool> keyValuePair in this.filterStates)
		{
		}
		return flag;
	}

	// Token: 0x0600A5F4 RID: 42484 RVA: 0x003F0C5C File Offset: 0x003EEE5C
	private bool CheckTechPassesFilters(string techID)
	{
		Tech tech = Db.Get().Techs.Get(techID);
		bool flag = true;
		switch (this.completionFilter)
		{
		case ResearchScreenSideBar.CompletionState.Available:
			flag = (flag && !tech.IsComplete() && tech.ArePrerequisitesComplete());
			break;
		case ResearchScreenSideBar.CompletionState.Completed:
			flag = (flag && tech.IsComplete());
			break;
		}
		if (!flag)
		{
			return flag;
		}
		flag = (flag && ResearchScreen.TechPassesSearchFilter(techID, this.currentSearchString));
		foreach (KeyValuePair<string, bool> keyValuePair in this.filterStates)
		{
		}
		return flag;
	}

	// Token: 0x0600A5F5 RID: 42485 RVA: 0x003F0D18 File Offset: 0x003EEF18
	public void ResetFilter()
	{
		this.UpdateCurrentSearch("");
		this.searchBox.text = "";
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.filterButtons)
		{
			this.filterStates[keyValuePair.Key] = false;
			this.filterButtons[keyValuePair.Key].GetComponent<MultiToggle>().ChangeState(this.filterStates[keyValuePair.Key] ? 1 : 0);
		}
		this.SetCompletionFilter(ResearchScreenSideBar.CompletionState.All);
	}

	// Token: 0x04008226 RID: 33318
	[Header("Containers")]
	[SerializeField]
	private GameObject queueContainer;

	// Token: 0x04008227 RID: 33319
	[SerializeField]
	private GameObject projectsContainer;

	// Token: 0x04008228 RID: 33320
	[SerializeField]
	private GameObject searchFiltersContainer;

	// Token: 0x04008229 RID: 33321
	[Header("Prefabs")]
	[SerializeField]
	private GameObject headerTechTypePrefab;

	// Token: 0x0400822A RID: 33322
	[SerializeField]
	private GameObject filterButtonPrefab;

	// Token: 0x0400822B RID: 33323
	[SerializeField]
	private GameObject techWidgetRootPrefab;

	// Token: 0x0400822C RID: 33324
	[SerializeField]
	private GameObject techWidgetRootAltPrefab;

	// Token: 0x0400822D RID: 33325
	[SerializeField]
	private GameObject techItemPrefab;

	// Token: 0x0400822E RID: 33326
	[SerializeField]
	private GameObject techWidgetUnlockedItemPrefab;

	// Token: 0x0400822F RID: 33327
	[SerializeField]
	private GameObject techWidgetRowPrefab;

	// Token: 0x04008230 RID: 33328
	[SerializeField]
	private GameObject techCategoryPrefab;

	// Token: 0x04008231 RID: 33329
	[SerializeField]
	private GameObject techCategoryPrefabAlt;

	// Token: 0x04008232 RID: 33330
	[Header("Other references")]
	[SerializeField]
	private KInputTextField searchBox;

	// Token: 0x04008233 RID: 33331
	[SerializeField]
	private MultiToggle allFilter;

	// Token: 0x04008234 RID: 33332
	[SerializeField]
	private MultiToggle availableFilter;

	// Token: 0x04008235 RID: 33333
	[SerializeField]
	private MultiToggle completedFilter;

	// Token: 0x04008236 RID: 33334
	[SerializeField]
	private ResearchScreen researchScreen;

	// Token: 0x04008237 RID: 33335
	[SerializeField]
	private KButton clearSearchButton;

	// Token: 0x04008238 RID: 33336
	[SerializeField]
	private Color evenRowColor;

	// Token: 0x04008239 RID: 33337
	[SerializeField]
	private Color oddRowColor;

	// Token: 0x0400823A RID: 33338
	private ResearchScreenSideBar.CompletionState completionFilter;

	// Token: 0x0400823B RID: 33339
	private Dictionary<string, bool> filterStates = new Dictionary<string, bool>();

	// Token: 0x0400823C RID: 33340
	private Dictionary<string, bool> categoryExpanded = new Dictionary<string, bool>();

	// Token: 0x0400823D RID: 33341
	private string currentSearchString = "";

	// Token: 0x0400823E RID: 33342
	private Dictionary<string, GameObject> queueTechs = new Dictionary<string, GameObject>();

	// Token: 0x0400823F RID: 33343
	private Dictionary<string, GameObject> projectTechs = new Dictionary<string, GameObject>();

	// Token: 0x04008240 RID: 33344
	private Dictionary<string, GameObject> projectCategories = new Dictionary<string, GameObject>();

	// Token: 0x04008241 RID: 33345
	private Dictionary<string, GameObject> filterButtons = new Dictionary<string, GameObject>();

	// Token: 0x04008242 RID: 33346
	private Dictionary<string, Dictionary<string, GameObject>> projectTechItems = new Dictionary<string, Dictionary<string, GameObject>>();

	// Token: 0x04008243 RID: 33347
	private Dictionary<string, List<Tag>> filterPresets = new Dictionary<string, List<Tag>>
	{
		{
			"Oxygen",
			new List<Tag>()
		},
		{
			"Food",
			new List<Tag>()
		},
		{
			"Water",
			new List<Tag>()
		},
		{
			"Power",
			new List<Tag>()
		},
		{
			"Morale",
			new List<Tag>()
		},
		{
			"Ranching",
			new List<Tag>()
		},
		{
			"Filter",
			new List<Tag>()
		},
		{
			"Tile",
			new List<Tag>()
		},
		{
			"Transport",
			new List<Tag>()
		},
		{
			"Automation",
			new List<Tag>()
		},
		{
			"Medicine",
			new List<Tag>()
		},
		{
			"Rocket",
			new List<Tag>()
		}
	};

	// Token: 0x04008244 RID: 33348
	private List<GameObject> QueuedActivations = new List<GameObject>();

	// Token: 0x04008245 RID: 33349
	private List<GameObject> QueuedDeactivations = new List<GameObject>();

	// Token: 0x04008246 RID: 33350
	public ButtonSoundPlayer soundPlayer;

	// Token: 0x04008247 RID: 33351
	[SerializeField]
	private int activationPerFrame = 5;

	// Token: 0x04008248 RID: 33352
	private bool evenRow;

	// Token: 0x02001ED0 RID: 7888
	private enum CompletionState
	{
		// Token: 0x0400824A RID: 33354
		All,
		// Token: 0x0400824B RID: 33355
		Available,
		// Token: 0x0400824C RID: 33356
		Completed
	}
}
