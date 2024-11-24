using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001AB0 RID: 6832
public class CharacterContainer : KScreen, ITelepadDeliverableContainer
{
	// Token: 0x06008EF4 RID: 36596 RVA: 0x000C9F3A File Offset: 0x000C813A
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x06008EF5 RID: 36597 RVA: 0x000FD745 File Offset: 0x000FB945
	public MinionStartingStats Stats
	{
		get
		{
			return this.stats;
		}
	}

	// Token: 0x06008EF6 RID: 36598 RVA: 0x00373770 File Offset: 0x00371970
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
		this.characterNameTitle.OnStartedEditing += this.OnStartedEditing;
		this.characterNameTitle.OnNameChanged += this.OnNameChanged;
		this.reshuffleButton.onClick += delegate()
		{
			this.Reshuffle(true);
		};
		List<IListableOption> list = new List<IListableOption>();
		foreach (SkillGroup item in new List<SkillGroup>(Db.Get().SkillGroups.resources))
		{
			list.Add(item);
		}
		this.archetypeDropDown.Initialize(list, new Action<IListableOption, object>(this.OnArchetypeEntryClick), new Func<IListableOption, IListableOption, object, int>(this.archetypeDropDownSort), new Action<DropDownEntry, object>(this.archetypeDropEntryRefreshAction), false, null);
		this.archetypeDropDown.CustomizeEmptyRow(Strings.Get("STRINGS.UI.CHARACTERCONTAINER_NOARCHETYPESELECTED"), this.noArchetypeIcon);
		List<IListableOption> contentKeys = new List<IListableOption>
		{
			new CharacterContainer.MinionModelOption(DUPLICANTS.MODEL.STANDARD.NAME, new List<Tag>
			{
				GameTags.Minions.Models.Standard
			}, Assets.GetSprite("ui_duplicant_minion_selection")),
			new CharacterContainer.MinionModelOption(DUPLICANTS.MODEL.BIONIC.NAME, new List<Tag>
			{
				GameTags.Minions.Models.Bionic
			}, Assets.GetSprite("ui_duplicant_bionicminion_selection"))
		};
		this.modelDropDown.Initialize(contentKeys, new Action<IListableOption, object>(this.OnModelEntryClick), new Func<IListableOption, IListableOption, object, int>(this.modelDropDownSort), new Action<DropDownEntry, object>(this.modelDropEntryRefreshAction), true, null);
		this.modelDropDown.CustomizeEmptyRow(UI.CHARACTERCONTAINER_ALL_MODELS, Assets.GetSprite(this.allModelSprite));
		base.StartCoroutine(this.DelayedGeneration());
	}

	// Token: 0x06008EF7 RID: 36599 RVA: 0x000FD74D File Offset: 0x000FB94D
	public void ForceStopEditingTitle()
	{
		this.characterNameTitle.ForceStopEditing();
	}

	// Token: 0x06008EF8 RID: 36600 RVA: 0x000FD501 File Offset: 0x000FB701
	public override float GetSortKey()
	{
		return 50f;
	}

	// Token: 0x06008EF9 RID: 36601 RVA: 0x000FD75A File Offset: 0x000FB95A
	private IEnumerator DelayedGeneration()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		this.GenerateCharacter(this.controller.IsStarterMinion, null);
		yield break;
	}

	// Token: 0x06008EFA RID: 36602 RVA: 0x000FD769 File Offset: 0x000FB969
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.animController != null)
		{
			this.animController.gameObject.DeleteObject();
			this.animController = null;
		}
	}

	// Token: 0x06008EFB RID: 36603 RVA: 0x000FD796 File Offset: 0x000FB996
	protected override void OnForcedCleanUp()
	{
		CharacterContainer.containers.Remove(this);
		base.OnForcedCleanUp();
	}

	// Token: 0x06008EFC RID: 36604 RVA: 0x0037394C File Offset: 0x00371B4C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.controller != null)
		{
			CharacterSelectionController characterSelectionController = this.controller;
			characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Remove(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
			CharacterSelectionController characterSelectionController2 = this.controller;
			characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Remove(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
			CharacterSelectionController characterSelectionController3 = this.controller;
			characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Remove(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		}
	}

	// Token: 0x06008EFD RID: 36605 RVA: 0x003739E4 File Offset: 0x00371BE4
	private void Initialize()
	{
		this.iconGroups = new List<GameObject>();
		this.traitEntries = new List<GameObject>();
		this.expectationLabels = new List<LocText>();
		this.aptitudeEntries = new List<GameObject>();
		if (CharacterContainer.containers == null)
		{
			CharacterContainer.containers = new List<CharacterContainer>();
		}
		CharacterContainer.containers.Add(this);
	}

	// Token: 0x06008EFE RID: 36606 RVA: 0x000FD7AA File Offset: 0x000FB9AA
	private void OnNameChanged(string newName)
	{
		this.stats.Name = newName;
		this.stats.personality.Name = newName;
		this.description.text = this.stats.personality.description;
	}

	// Token: 0x06008EFF RID: 36607 RVA: 0x000FD7E4 File Offset: 0x000FB9E4
	private void OnStartedEditing()
	{
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x06008F00 RID: 36608 RVA: 0x00373A3C File Offset: 0x00371C3C
	public void SetMinion(MinionStartingStats statsProposed)
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			this.DeselectDeliverable();
		}
		this.stats = statsProposed;
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.SetAnimator();
		this.SetInfoText();
		base.StartCoroutine(this.SetAttributes());
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.enabled = true;
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x06008F01 RID: 36609 RVA: 0x00373AF0 File Offset: 0x00371CF0
	public void GenerateCharacter(bool is_starter, string guaranteedAptitudeID = null)
	{
		int num = 0;
		do
		{
			this.stats = new MinionStartingStats(this.permittedModels, is_starter, guaranteedAptitudeID, null, false);
			num++;
		}
		while (this.IsCharacterInvalid() && num < 20);
		if (this.animController != null)
		{
			UnityEngine.Object.Destroy(this.animController.gameObject);
			this.animController = null;
		}
		this.SetAnimator();
		this.SetInfoText();
		base.StartCoroutine(this.SetAttributes());
		this.selectButton.ClearOnClick();
		if (!this.controller.IsStarterMinion)
		{
			this.selectButton.enabled = true;
			this.selectButton.onClick += delegate()
			{
				this.SelectDeliverable();
			};
		}
	}

	// Token: 0x06008F02 RID: 36610 RVA: 0x00373BA0 File Offset: 0x00371DA0
	private void SetAnimator()
	{
		if (this.animController == null)
		{
			this.animController = Util.KInstantiateUI(Assets.GetPrefab(GameTags.MinionSelectPreview), this.contentBody.gameObject, false).GetComponent<KBatchedAnimController>();
			this.animController.gameObject.SetActive(true);
			this.animController.animScale = this.baseCharacterScale;
		}
		BaseMinionConfig.ConfigureSymbols(this.animController.gameObject, true);
		this.stats.ApplyTraits(this.animController.gameObject);
		this.stats.ApplyRace(this.animController.gameObject);
		this.stats.ApplyAccessories(this.animController.gameObject);
		this.stats.ApplyOutfit(this.stats.personality, this.animController.gameObject);
		this.stats.ApplyJoyResponseOutfit(this.stats.personality, this.animController.gameObject);
		this.stats.ApplyExperience(this.animController.gameObject);
		HashedString idleAnim = this.GetIdleAnim(this.stats);
		this.idle_anim = Assets.GetAnim(idleAnim);
		if (this.idle_anim != null)
		{
			this.animController.AddAnimOverrides(this.idle_anim, 0f);
		}
		KAnimFile anim = Assets.GetAnim(new HashedString("crewSelect_fx_kanim"));
		if (anim != null)
		{
			this.animController.AddAnimOverrides(anim, 0f);
		}
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06008F03 RID: 36611 RVA: 0x00373D38 File Offset: 0x00371F38
	private HashedString GetIdleAnim(MinionStartingStats minionStartingStats)
	{
		List<HashedString> list = new List<HashedString>();
		foreach (KeyValuePair<HashedString, string[]> keyValuePair in CharacterContainer.traitIdleAnims)
		{
			foreach (Trait trait in minionStartingStats.Traits)
			{
				if (keyValuePair.Value.Contains(trait.Id))
				{
					list.Add(keyValuePair.Key);
				}
			}
			if (keyValuePair.Value.Contains(minionStartingStats.joyTrait.Id) || keyValuePair.Value.Contains(minionStartingStats.stressTrait.Id))
			{
				list.Add(keyValuePair.Key);
			}
		}
		if (list.Count > 0)
		{
			return list.ToArray()[UnityEngine.Random.Range(0, list.Count)];
		}
		return CharacterContainer.idleAnims[UnityEngine.Random.Range(0, CharacterContainer.idleAnims.Length)];
	}

	// Token: 0x06008F04 RID: 36612 RVA: 0x00373E64 File Offset: 0x00372064
	private void SetInfoText()
	{
		this.traitEntries.ForEach(delegate(GameObject tl)
		{
			UnityEngine.Object.Destroy(tl.gameObject);
		});
		this.traitEntries.Clear();
		this.characterNameTitle.SetTitle(this.stats.Name);
		this.traitHeaderLabel.SetText((this.stats.personality.model == GameTags.Minions.Models.Bionic) ? UI.CHARACTERCONTAINER_TRAITS_TITLE_BIONIC : UI.CHARACTERCONTAINER_TRAITS_TITLE);
		for (int i = 1; i < this.stats.Traits.Count; i++)
		{
			Trait trait = this.stats.Traits[i];
			LocText locText = trait.PositiveTrait ? this.goodTrait : this.badTrait;
			LocText locText2 = Util.KInstantiateUI<LocText>(locText.gameObject, locText.transform.parent.gameObject, false);
			locText2.gameObject.SetActive(true);
			locText2.text = this.stats.Traits[i].Name;
			locText2.color = (trait.PositiveTrait ? Constants.POSITIVE_COLOR : Constants.NEGATIVE_COLOR);
			locText2.GetComponent<ToolTip>().SetSimpleTooltip(trait.GetTooltip());
			for (int j = 0; j < trait.SelfModifiers.Count; j++)
			{
				GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject.SetActive(true);
				LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
				string format = (trait.SelfModifiers[j].Value > 0f) ? UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED : UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED;
				componentInChildren.text = string.Format(format, Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[j].AttributeId.ToUpper() + ".NAME"));
				trait.SelfModifiers[j].AttributeId == "GermResistance";
				Klei.AI.Attribute attribute = Db.Get().Attributes.Get(trait.SelfModifiers[j].AttributeId);
				string text = attribute.Description;
				text = string.Concat(new string[]
				{
					text,
					"\n\n",
					Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[j].AttributeId.ToUpper() + ".NAME"),
					": ",
					trait.SelfModifiers[j].GetFormattedString()
				});
				List<AttributeConverter> convertersForAttribute = Db.Get().AttributeConverters.GetConvertersForAttribute(attribute);
				for (int k = 0; k < convertersForAttribute.Count; k++)
				{
					string text2 = convertersForAttribute[k].DescriptionFromAttribute(convertersForAttribute[k].multiplier * trait.SelfModifiers[j].Value, null);
					if (text2 != "")
					{
						text = text + "\n    • " + text2;
					}
				}
				componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(text);
				this.traitEntries.Add(gameObject);
			}
			if (trait.disabledChoreGroups != null)
			{
				GameObject gameObject2 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject2.SetActive(true);
				LocText componentInChildren2 = gameObject2.GetComponentInChildren<LocText>();
				componentInChildren2.text = trait.GetDisabledChoresString(false);
				string text3 = "";
				string text4 = "";
				for (int l = 0; l < trait.disabledChoreGroups.Length; l++)
				{
					if (l > 0)
					{
						text3 += ", ";
						text4 += "\n";
					}
					text3 += trait.disabledChoreGroups[l].Name;
					text4 += trait.disabledChoreGroups[l].description;
				}
				componentInChildren2.GetComponent<ToolTip>().SetSimpleTooltip(string.Format(DUPLICANTS.TRAITS.CANNOT_DO_TASK_TOOLTIP, text3, text4));
				this.traitEntries.Add(gameObject2);
			}
			if (trait.ignoredEffects != null && trait.ignoredEffects.Length != 0)
			{
				GameObject gameObject3 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject3.SetActive(true);
				LocText componentInChildren3 = gameObject3.GetComponentInChildren<LocText>();
				componentInChildren3.text = trait.GetIgnoredEffectsString(false);
				string text5 = "";
				for (int m = 0; m < trait.ignoredEffects.Length; m++)
				{
					if (m > 0)
					{
						text5 += "\n";
					}
					text5 += string.Format(DUPLICANTS.TRAITS.IGNORED_EFFECTS_TOOLTIP, Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[m].ToUpper() + ".NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[m].ToUpper() + ".CAUSE"));
					if (m < trait.ignoredEffects.Length - 1)
					{
						text5 += ",";
					}
				}
				componentInChildren3.GetComponent<ToolTip>().SetSimpleTooltip(text5);
				this.traitEntries.Add(gameObject3);
			}
			StringEntry stringEntry;
			if (Strings.TryGet("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC", out stringEntry))
			{
				GameObject gameObject4 = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText.transform.parent.gameObject, false);
				gameObject4.SetActive(true);
				LocText componentInChildren4 = gameObject4.GetComponentInChildren<LocText>();
				componentInChildren4.text = stringEntry.String;
				componentInChildren4.GetComponent<ToolTip>().SetSimpleTooltip(Strings.Get("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC_TOOLTIP"));
				this.traitEntries.Add(gameObject4);
			}
			this.traitEntries.Add(locText2.gameObject);
		}
		this.aptitudeEntries.ForEach(delegate(GameObject al)
		{
			UnityEngine.Object.Destroy(al.gameObject);
		});
		this.aptitudeEntries.Clear();
		this.expectationLabels.ForEach(delegate(LocText el)
		{
			UnityEngine.Object.Destroy(el.gameObject);
		});
		this.expectationLabels.Clear();
		foreach (KeyValuePair<SkillGroup, float> keyValuePair in this.stats.skillAptitudes)
		{
			if (keyValuePair.Value != 0f)
			{
				SkillGroup skillGroup = Db.Get().SkillGroups.Get(keyValuePair.Key.IdHash);
				if (skillGroup == null)
				{
					global::Debug.LogWarningFormat("Role group not found for aptitude: {0}", new object[]
					{
						keyValuePair.Key
					});
				}
				else
				{
					GameObject gameObject5 = Util.KInstantiateUI(this.aptitudeEntry.gameObject, this.aptitudeEntry.transform.parent.gameObject, false);
					LocText locText3 = Util.KInstantiateUI<LocText>(this.aptitudeLabel.gameObject, gameObject5, false);
					locText3.gameObject.SetActive(true);
					locText3.text = skillGroup.Name;
					string simpleTooltip;
					if (skillGroup.choreGroupID != "")
					{
						ChoreGroup choreGroup = Db.Get().ChoreGroups.Get(skillGroup.choreGroupID);
						simpleTooltip = string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION_CHOREGROUP, skillGroup.Name, DUPLICANTSTATS.APTITUDE_BONUS, choreGroup.description);
					}
					else
					{
						simpleTooltip = string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION, skillGroup.Name, DUPLICANTSTATS.APTITUDE_BONUS);
					}
					locText3.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
					float num = (float)this.stats.StartingLevels[keyValuePair.Key.relevantAttributes[0].Id];
					LocText locText4 = Util.KInstantiateUI<LocText>(this.attributeLabelAptitude.gameObject, gameObject5, false);
					locText4.gameObject.SetActive(true);
					locText4.text = "+" + num.ToString() + " " + keyValuePair.Key.relevantAttributes[0].Name;
					string text6 = keyValuePair.Key.relevantAttributes[0].Description;
					text6 = string.Concat(new string[]
					{
						text6,
						"\n\n",
						keyValuePair.Key.relevantAttributes[0].Name,
						": +",
						num.ToString()
					});
					List<AttributeConverter> convertersForAttribute2 = Db.Get().AttributeConverters.GetConvertersForAttribute(keyValuePair.Key.relevantAttributes[0]);
					for (int n = 0; n < convertersForAttribute2.Count; n++)
					{
						text6 = text6 + "\n    • " + convertersForAttribute2[n].DescriptionFromAttribute(convertersForAttribute2[n].multiplier * num, null);
					}
					locText4.GetComponent<ToolTip>().SetSimpleTooltip(text6);
					gameObject5.gameObject.SetActive(true);
					this.aptitudeEntries.Add(gameObject5);
				}
			}
		}
		if (this.stats.stressTrait != null)
		{
			LocText locText5 = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
			locText5.gameObject.SetActive(true);
			locText5.text = string.Format(UI.CHARACTERCONTAINER_STRESSTRAIT, this.stats.stressTrait.Name);
			locText5.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.stressTrait.GetTooltip());
			this.expectationLabels.Add(locText5);
		}
		if (this.stats.joyTrait != null)
		{
			LocText locText6 = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
			locText6.gameObject.SetActive(true);
			locText6.text = string.Format(UI.CHARACTERCONTAINER_JOYTRAIT, this.stats.joyTrait.Name);
			locText6.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.joyTrait.GetTooltip());
			this.expectationLabels.Add(locText6);
		}
		this.description.text = this.stats.personality.description;
	}

	// Token: 0x06008F05 RID: 36613 RVA: 0x000FD7F0 File Offset: 0x000FB9F0
	private IEnumerator SetAttributes()
	{
		yield return null;
		this.iconGroups.ForEach(delegate(GameObject icg)
		{
			UnityEngine.Object.Destroy(icg);
		});
		this.iconGroups.Clear();
		List<AttributeInstance> list = new List<AttributeInstance>(this.animController.gameObject.GetAttributes().AttributeTable);
		list.RemoveAll((AttributeInstance at) => at.Attribute.ShowInUI != Klei.AI.Attribute.Display.Skill);
		list = (from at in list
		orderby at.Name
		select at).ToList<AttributeInstance>();
		for (int i = 0; i < list.Count; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.iconGroup.gameObject, this.iconGroup.transform.parent.gameObject, false);
			LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
			gameObject.SetActive(true);
			float totalValue = list[i].GetTotalValue();
			if (totalValue > 0f)
			{
				componentInChildren.color = Constants.POSITIVE_COLOR;
			}
			else if (totalValue == 0f)
			{
				componentInChildren.color = Constants.NEUTRAL_COLOR;
			}
			else
			{
				componentInChildren.color = Constants.NEGATIVE_COLOR;
			}
			componentInChildren.text = string.Format(UI.CHARACTERCONTAINER_SKILL_VALUE, GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f), list[i].Name);
			AttributeInstance attributeInstance = list[i];
			string text = attributeInstance.Description;
			if (attributeInstance.Attribute.converters.Count > 0)
			{
				text += "\n";
				foreach (AttributeConverter attributeConverter in attributeInstance.Attribute.converters)
				{
					AttributeConverterInstance converter = this.animController.gameObject.GetComponent<Klei.AI.AttributeConverters>().GetConverter(attributeConverter.Id);
					string text2 = converter.DescriptionFromAttribute(converter.Evaluate(), converter.gameObject);
					if (text2 != null)
					{
						text = text + "\n" + text2;
					}
				}
			}
			gameObject.GetComponent<ToolTip>().SetSimpleTooltip(text);
			this.iconGroups.Add(gameObject);
		}
		yield break;
	}

	// Token: 0x06008F06 RID: 36614 RVA: 0x00374910 File Offset: 0x00372B10
	public void SelectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.AddDeliverable(this.stats);
		}
		if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
		{
			MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 1f, true);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetActive();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.DeselectDeliverable();
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 0f, true);
			}
		};
		this.selectedBorder.SetActive(true);
		this.titleBar.color = this.selectedTitleColor;
		this.animController.Play("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Play("cheer_loop", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06008F07 RID: 36615 RVA: 0x003749F8 File Offset: 0x00372BF8
	public void DeselectDeliverable()
	{
		if (this.controller != null)
		{
			this.controller.RemoveDeliverable(this.stats);
		}
		this.selectButton.GetComponent<ImageToggleState>().SetInactive();
		this.selectButton.Deselect();
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
		this.selectedBorder.SetActive(false);
		this.titleBar.color = this.deselectedTitleColor;
		this.animController.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		this.animController.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06008F08 RID: 36616 RVA: 0x000FD7FF File Offset: 0x000FB9FF
	private void OnReplacedEvent(ITelepadDeliverable deliverable)
	{
		if (deliverable == this.stats)
		{
			this.DeselectDeliverable();
		}
	}

	// Token: 0x06008F09 RID: 36617 RVA: 0x00374AC0 File Offset: 0x00372CC0
	private void OnCharacterSelectionLimitReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		if (this.controller.AllowsReplacing)
		{
			this.selectButton.onClick += this.ReplaceCharacterSelection;
			return;
		}
		this.selectButton.onClick += this.CantSelectCharacter;
	}

	// Token: 0x06008F0A RID: 36618 RVA: 0x000FD595 File Offset: 0x000FB795
	private void CantSelectCharacter()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x06008F0B RID: 36619 RVA: 0x000FD810 File Offset: 0x000FBA10
	private void ReplaceCharacterSelection()
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.RemoveLast();
		this.SelectDeliverable();
	}

	// Token: 0x06008F0C RID: 36620 RVA: 0x00374B38 File Offset: 0x00372D38
	private void OnCharacterSelectionLimitUnReached()
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			return;
		}
		this.selectButton.ClearOnClick();
		this.selectButton.onClick += delegate()
		{
			this.SelectDeliverable();
		};
	}

	// Token: 0x06008F0D RID: 36621 RVA: 0x00374B8C File Offset: 0x00372D8C
	public void SetReshufflingState(bool enable)
	{
		this.reshuffleButton.gameObject.SetActive(enable);
		this.archetypeDropDown.gameObject.SetActive(enable);
		this.modelDropDown.transform.parent.gameObject.SetActive(enable && SaveLoader.Instance.IsDLCActiveForCurrentSave("DLC3_ID"));
	}

	// Token: 0x06008F0E RID: 36622 RVA: 0x00374BEC File Offset: 0x00372DEC
	public void Reshuffle(bool is_starter)
	{
		if (this.controller != null && this.controller.IsSelected(this.stats))
		{
			this.DeselectDeliverable();
		}
		if (this.fxAnim != null)
		{
			this.fxAnim.Play("loop", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.GenerateCharacter(is_starter, this.guaranteedAptitudeID);
	}

	// Token: 0x06008F0F RID: 36623 RVA: 0x00374C5C File Offset: 0x00372E5C
	public void SetController(CharacterSelectionController csc)
	{
		if (csc == this.controller)
		{
			return;
		}
		this.controller = csc;
		CharacterSelectionController characterSelectionController = this.controller;
		characterSelectionController.OnLimitReachedEvent = (System.Action)Delegate.Combine(characterSelectionController.OnLimitReachedEvent, new System.Action(this.OnCharacterSelectionLimitReached));
		CharacterSelectionController characterSelectionController2 = this.controller;
		characterSelectionController2.OnLimitUnreachedEvent = (System.Action)Delegate.Combine(characterSelectionController2.OnLimitUnreachedEvent, new System.Action(this.OnCharacterSelectionLimitUnReached));
		CharacterSelectionController characterSelectionController3 = this.controller;
		characterSelectionController3.OnReshuffleEvent = (Action<bool>)Delegate.Combine(characterSelectionController3.OnReshuffleEvent, new Action<bool>(this.Reshuffle));
		CharacterSelectionController characterSelectionController4 = this.controller;
		characterSelectionController4.OnReplacedEvent = (Action<ITelepadDeliverable>)Delegate.Combine(characterSelectionController4.OnReplacedEvent, new Action<ITelepadDeliverable>(this.OnReplacedEvent));
	}

	// Token: 0x06008F10 RID: 36624 RVA: 0x00374D1C File Offset: 0x00372F1C
	public void DisableSelectButton()
	{
		this.selectButton.soundPlayer.AcceptClickCondition = (() => false);
		this.selectButton.GetComponent<ImageToggleState>().SetDisabled();
		this.selectButton.soundPlayer.Enabled = false;
	}

	// Token: 0x06008F11 RID: 36625 RVA: 0x00374D7C File Offset: 0x00372F7C
	private bool IsCharacterInvalid()
	{
		return CharacterContainer.containers.Find((CharacterContainer container) => container != null && container.stats != null && container != this && container.stats.personality.Id == this.stats.personality.Id && container.stats.IsValid) != null || (SaveLoader.Instance != null && DlcManager.IsDlcId(this.stats.personality.requiredDlcId) && !SaveLoader.Instance.GameInfo.dlcIds.Contains(this.stats.personality.requiredDlcId)) || (this.stats.personality.model != GameTags.Minions.Models.Bionic && Components.LiveMinionIdentities.Items.Any((MinionIdentity id) => id.personalityResourceId == this.stats.personality.Id));
	}

	// Token: 0x06008F12 RID: 36626 RVA: 0x000FD612 File Offset: 0x000FB812
	public string GetValueColor(bool isPositive)
	{
		if (!isPositive)
		{
			return "<color=#ff2222ff>";
		}
		return "<color=green>";
	}

	// Token: 0x06008F13 RID: 36627 RVA: 0x000FD832 File Offset: 0x000FBA32
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.scroll_rect.mouseIsOver = true;
		base.OnPointerEnter(eventData);
	}

	// Token: 0x06008F14 RID: 36628 RVA: 0x000FD847 File Offset: 0x000FBA47
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.scroll_rect.mouseIsOver = false;
		base.OnPointerExit(eventData);
	}

	// Token: 0x06008F15 RID: 36629 RVA: 0x00374E34 File Offset: 0x00373034
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape) || e.IsAction(global::Action.MouseRight))
		{
			this.characterNameTitle.ForceStopEditing();
			this.controller.OnPressBack();
			this.archetypeDropDown.scrollRect.gameObject.SetActive(false);
		}
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
			return;
		}
		if (this.archetypeDropDown.scrollRect.activeInHierarchy)
		{
			KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
			Vector2 point = component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (component.rectTransform().rect.Contains(point))
			{
				component.mouseIsOver = true;
			}
			else
			{
				component.mouseIsOver = false;
			}
			component.OnKeyDown(e);
			return;
		}
		this.scroll_rect.OnKeyDown(e);
	}

	// Token: 0x06008F16 RID: 36630 RVA: 0x00374F04 File Offset: 0x00373104
	public override void OnKeyUp(KButtonEvent e)
	{
		if (!KInputManager.currentControllerIsGamepad)
		{
			e.Consumed = true;
			return;
		}
		if (this.archetypeDropDown.scrollRect.activeInHierarchy)
		{
			KScrollRect component = this.archetypeDropDown.scrollRect.GetComponent<KScrollRect>();
			Vector2 point = component.rectTransform().InverseTransformPoint(KInputManager.GetMousePos());
			if (component.rectTransform().rect.Contains(point))
			{
				component.mouseIsOver = true;
			}
			else
			{
				component.mouseIsOver = false;
			}
			component.OnKeyUp(e);
			return;
		}
		this.scroll_rect.OnKeyUp(e);
	}

	// Token: 0x06008F17 RID: 36631 RVA: 0x000FD85C File Offset: 0x000FBA5C
	protected override void OnCmpEnable()
	{
		base.OnActivate();
		if (this.stats == null)
		{
			return;
		}
		this.SetAnimator();
	}

	// Token: 0x06008F18 RID: 36632 RVA: 0x000FD873 File Offset: 0x000FBA73
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.characterNameTitle.ForceStopEditing();
	}

	// Token: 0x06008F19 RID: 36633 RVA: 0x00374F94 File Offset: 0x00373194
	private void OnArchetypeEntryClick(IListableOption skill, object data)
	{
		if (skill != null)
		{
			SkillGroup skillGroup = skill as SkillGroup;
			this.guaranteedAptitudeID = skillGroup.Id;
			this.selectedArchetypeIcon.sprite = Assets.GetSprite(skillGroup.archetypeIcon);
			this.Reshuffle(true);
			return;
		}
		this.guaranteedAptitudeID = null;
		this.selectedArchetypeIcon.sprite = this.dropdownArrowIcon;
		this.Reshuffle(true);
	}

	// Token: 0x06008F1A RID: 36634 RVA: 0x000FD887 File Offset: 0x000FBA87
	private int archetypeDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		if (b.Equals("Random"))
		{
			return -1;
		}
		return b.GetProperName().CompareTo(a.GetProperName());
	}

	// Token: 0x06008F1B RID: 36635 RVA: 0x00374FFC File Offset: 0x003731FC
	private void archetypeDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			SkillGroup skillGroup = entry.entryData as SkillGroup;
			entry.image.sprite = Assets.GetSprite(skillGroup.archetypeIcon);
		}
	}

	// Token: 0x06008F1C RID: 36636 RVA: 0x00375038 File Offset: 0x00373238
	private void OnModelEntryClick(IListableOption listItem, object data)
	{
		if (listItem == null)
		{
			this.permittedModels = this.allMinionModels;
			this.selectedModelIcon.sprite = Assets.GetSprite(this.allModelSprite);
			this.Reshuffle(true);
			return;
		}
		CharacterContainer.MinionModelOption minionModelOption = listItem as CharacterContainer.MinionModelOption;
		if (minionModelOption != null)
		{
			this.permittedModels = minionModelOption.permittedModels;
			this.selectedModelIcon.sprite = minionModelOption.sprite;
			this.Reshuffle(true);
		}
	}

	// Token: 0x06008F1D RID: 36637 RVA: 0x000FD8A9 File Offset: 0x000FBAA9
	private int modelDropDownSort(IListableOption a, IListableOption b, object targetData)
	{
		return a.GetProperName().CompareTo(b.GetProperName());
	}

	// Token: 0x06008F1E RID: 36638 RVA: 0x003750A8 File Offset: 0x003732A8
	private void modelDropEntryRefreshAction(DropDownEntry entry, object targetData)
	{
		if (entry.entryData != null)
		{
			CharacterContainer.MinionModelOption minionModelOption = entry.entryData as CharacterContainer.MinionModelOption;
			entry.image.sprite = minionModelOption.sprite;
		}
	}

	// Token: 0x04006BB9 RID: 27577
	[SerializeField]
	private GameObject contentBody;

	// Token: 0x04006BBA RID: 27578
	[SerializeField]
	private LocText characterName;

	// Token: 0x04006BBB RID: 27579
	[SerializeField]
	private EditableTitleBar characterNameTitle;

	// Token: 0x04006BBC RID: 27580
	[SerializeField]
	private LocText characterJob;

	// Token: 0x04006BBD RID: 27581
	[SerializeField]
	private LocText traitHeaderLabel;

	// Token: 0x04006BBE RID: 27582
	public GameObject selectedBorder;

	// Token: 0x04006BBF RID: 27583
	[SerializeField]
	private Image titleBar;

	// Token: 0x04006BC0 RID: 27584
	[SerializeField]
	private Color selectedTitleColor;

	// Token: 0x04006BC1 RID: 27585
	[SerializeField]
	private Color deselectedTitleColor;

	// Token: 0x04006BC2 RID: 27586
	[SerializeField]
	private KButton reshuffleButton;

	// Token: 0x04006BC3 RID: 27587
	private KBatchedAnimController animController;

	// Token: 0x04006BC4 RID: 27588
	[SerializeField]
	private GameObject iconGroup;

	// Token: 0x04006BC5 RID: 27589
	private List<GameObject> iconGroups;

	// Token: 0x04006BC6 RID: 27590
	[SerializeField]
	private LocText goodTrait;

	// Token: 0x04006BC7 RID: 27591
	[SerializeField]
	private LocText badTrait;

	// Token: 0x04006BC8 RID: 27592
	[SerializeField]
	private GameObject aptitudeEntry;

	// Token: 0x04006BC9 RID: 27593
	[SerializeField]
	private Transform aptitudeLabel;

	// Token: 0x04006BCA RID: 27594
	[SerializeField]
	private Transform attributeLabelAptitude;

	// Token: 0x04006BCB RID: 27595
	[SerializeField]
	private Transform attributeLabelTrait;

	// Token: 0x04006BCC RID: 27596
	[SerializeField]
	private LocText expectationRight;

	// Token: 0x04006BCD RID: 27597
	private List<LocText> expectationLabels;

	// Token: 0x04006BCE RID: 27598
	[SerializeField]
	private DropDown archetypeDropDown;

	// Token: 0x04006BCF RID: 27599
	[SerializeField]
	private Image selectedArchetypeIcon;

	// Token: 0x04006BD0 RID: 27600
	[SerializeField]
	private Sprite noArchetypeIcon;

	// Token: 0x04006BD1 RID: 27601
	[SerializeField]
	private Sprite dropdownArrowIcon;

	// Token: 0x04006BD2 RID: 27602
	private string guaranteedAptitudeID;

	// Token: 0x04006BD3 RID: 27603
	private List<GameObject> aptitudeEntries;

	// Token: 0x04006BD4 RID: 27604
	private List<GameObject> traitEntries;

	// Token: 0x04006BD5 RID: 27605
	[SerializeField]
	private LocText description;

	// Token: 0x04006BD6 RID: 27606
	[SerializeField]
	private Image selectedModelIcon;

	// Token: 0x04006BD7 RID: 27607
	[SerializeField]
	private DropDown modelDropDown;

	// Token: 0x04006BD8 RID: 27608
	private List<Tag> permittedModels = new List<Tag>
	{
		GameTags.Minions.Models.Standard,
		GameTags.Minions.Models.Bionic
	};

	// Token: 0x04006BD9 RID: 27609
	[SerializeField]
	private KToggle selectButton;

	// Token: 0x04006BDA RID: 27610
	[SerializeField]
	private KBatchedAnimController fxAnim;

	// Token: 0x04006BDB RID: 27611
	private string allModelSprite = "ui_duplicant_any_selection";

	// Token: 0x04006BDC RID: 27612
	private MinionStartingStats stats;

	// Token: 0x04006BDD RID: 27613
	private CharacterSelectionController controller;

	// Token: 0x04006BDE RID: 27614
	private static List<CharacterContainer> containers;

	// Token: 0x04006BDF RID: 27615
	private KAnimFile idle_anim;

	// Token: 0x04006BE0 RID: 27616
	[HideInInspector]
	public bool addMinionToIdentityList = true;

	// Token: 0x04006BE1 RID: 27617
	[SerializeField]
	private Sprite enabledSpr;

	// Token: 0x04006BE2 RID: 27618
	[SerializeField]
	private KScrollRect scroll_rect;

	// Token: 0x04006BE3 RID: 27619
	private static readonly Dictionary<HashedString, string[]> traitIdleAnims = new Dictionary<HashedString, string[]>
	{
		{
			"anim_idle_food_kanim",
			new string[]
			{
				"Foodie"
			}
		},
		{
			"anim_idle_animal_lover_kanim",
			new string[]
			{
				"RanchingUp"
			}
		},
		{
			"anim_idle_loner_kanim",
			new string[]
			{
				"Loner"
			}
		},
		{
			"anim_idle_mole_hands_kanim",
			new string[]
			{
				"MoleHands"
			}
		},
		{
			"anim_idle_buff_kanim",
			new string[]
			{
				"StrongArm"
			}
		},
		{
			"anim_idle_distracted_kanim",
			new string[]
			{
				"CantResearch",
				"CantBuild",
				"CantCook",
				"CantDig"
			}
		},
		{
			"anim_idle_coaster_kanim",
			new string[]
			{
				"HappySinger"
			}
		}
	};

	// Token: 0x04006BE4 RID: 27620
	private List<Tag> allMinionModels = new List<Tag>
	{
		GameTags.Minions.Models.Standard,
		GameTags.Minions.Models.Bionic
	};

	// Token: 0x04006BE5 RID: 27621
	private static readonly HashedString[] idleAnims = new HashedString[]
	{
		"anim_idle_healthy_kanim",
		"anim_idle_susceptible_kanim",
		"anim_idle_keener_kanim",
		"anim_idle_fastfeet_kanim",
		"anim_idle_breatherdeep_kanim",
		"anim_idle_breathershallow_kanim"
	};

	// Token: 0x04006BE6 RID: 27622
	public float baseCharacterScale = 0.38f;

	// Token: 0x02001AB1 RID: 6833
	[Serializable]
	public struct ProfessionIcon
	{
		// Token: 0x04006BE7 RID: 27623
		public string professionName;

		// Token: 0x04006BE8 RID: 27624
		public Sprite iconImg;
	}

	// Token: 0x02001AB2 RID: 6834
	private class MinionModelOption : IListableOption
	{
		// Token: 0x06008F29 RID: 36649 RVA: 0x000FD922 File Offset: 0x000FBB22
		public MinionModelOption(string name, List<Tag> permittedModels, Sprite sprite)
		{
			this.properName = name;
			this.permittedModels = permittedModels;
			this.sprite = sprite;
		}

		// Token: 0x06008F2A RID: 36650 RVA: 0x000FD93F File Offset: 0x000FBB3F
		public string GetProperName()
		{
			return this.properName;
		}

		// Token: 0x04006BE9 RID: 27625
		private string properName;

		// Token: 0x04006BEA RID: 27626
		public List<Tag> permittedModels;

		// Token: 0x04006BEB RID: 27627
		public Sprite sprite;
	}
}
