using System.Collections.Generic;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;

public class MinionPersonalityPanel : DetailScreenTab
{
	private CollapsibleDetailContentPanel bioPanel;

	private CollapsibleDetailContentPanel traitsPanel;

	private CollapsibleDetailContentPanel resumePanel;

	private CollapsibleDetailContentPanel attributesPanel;

	private CollapsibleDetailContentPanel equipmentPanel;

	private CollapsibleDetailContentPanel amenitiesPanel;

	private SchedulerHandle updateHandle;

	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MinionIdentity>() != null;
	}

	protected override void OnSelectTarget(GameObject target)
	{
		base.OnSelectTarget(target);
		Refresh();
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		bioPanel = CreateCollapsableSection(UI.DETAILTABS.PERSONALITY.GROUPNAME_BIO);
		traitsPanel = CreateCollapsableSection(UI.DETAILTABS.STATS.GROUPNAME_TRAITS);
		attributesPanel = CreateCollapsableSection(UI.DETAILTABS.STATS.GROUPNAME_ATTRIBUTES);
		resumePanel = CreateCollapsableSection(UI.DETAILTABS.PERSONALITY.GROUPNAME_RESUME);
		amenitiesPanel = CreateCollapsableSection(UI.DETAILTABS.PERSONALITY.EQUIPMENT.GROUPNAME_ROOMS);
		equipmentPanel = CreateCollapsableSection(UI.DETAILTABS.PERSONALITY.EQUIPMENT.GROUPNAME_OWNABLE);
	}

	protected override void OnCleanUp()
	{
		updateHandle.ClearScheduler();
		base.OnCleanUp();
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Refresh();
		ScheduleUpdate();
	}

	private void ScheduleUpdate()
	{
		updateHandle = UIScheduler.Instance.Schedule("RefreshMinionPersonalityPanel", 1f, delegate
		{
			Refresh();
			ScheduleUpdate();
		});
	}

	private void Refresh()
	{
		if (base.gameObject.activeSelf && !(selectedTarget == null) && !(selectedTarget.GetComponent<MinionIdentity>() == null))
		{
			RefreshBioPanel(bioPanel, selectedTarget);
			RefreshTraitsPanel(traitsPanel, selectedTarget);
			RefreshAmenitiesPanel(amenitiesPanel, selectedTarget);
			RefreshEquipmentPanel(equipmentPanel, selectedTarget);
			RefreshResumePanel(resumePanel, selectedTarget);
			RefreshAttributesPanel(attributesPanel, selectedTarget);
		}
	}

	private static void RefreshBioPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		MinionIdentity component = targetEntity.GetComponent<MinionIdentity>();
		if (!component)
		{
			targetPanel.SetActive(active: false);
			return;
		}
		targetPanel.SetActive(active: true);
		targetPanel.SetLabel("name", string.Concat(DUPLICANTS.NAMETITLE, component.name), "");
		targetPanel.SetLabel("age", string.Concat(DUPLICANTS.ARRIVALTIME, GameUtil.GetFormattedCycles(((float)GameClock.Instance.GetCycle() - component.arrivalTime) * 600f, "F0", forceCycles: true)), string.Format(DUPLICANTS.ARRIVALTIME_TOOLTIP, component.arrivalTime + 1f, component.name));
		targetPanel.SetLabel("gender", string.Concat(DUPLICANTS.GENDERTITLE, string.Format(Strings.Get($"STRINGS.DUPLICANTS.GENDER.{component.genderStringKey.ToUpper()}.NAME"), component.gender)), "");
		targetPanel.SetLabel("personality", string.Format(Strings.Get($"STRINGS.DUPLICANTS.PERSONALITIES.{component.nameStringKey.ToUpper()}.DESC"), component.name), string.Format(Strings.Get(string.Format("STRINGS.DUPLICANTS.DESC_TOOLTIP", component.nameStringKey.ToUpper())), component.name));
		MinionResume component2 = targetEntity.GetComponent<MinionResume>();
		if (component2 != null && component2.AptitudeBySkillGroup.Count > 0)
		{
			targetPanel.SetLabel("interestHeader", string.Concat(UI.DETAILTABS.PERSONALITY.RESUME.APTITUDES.NAME, "\n"), string.Format(UI.DETAILTABS.PERSONALITY.RESUME.APTITUDES.TOOLTIP, targetEntity.name));
			foreach (KeyValuePair<HashedString, float> item in component2.AptitudeBySkillGroup)
			{
				if (item.Value != 0f)
				{
					SkillGroup skillGroup = Db.Get().SkillGroups.TryGet(item.Key);
					if (skillGroup != null)
					{
						targetPanel.SetLabel(skillGroup.Name, "  • " + skillGroup.Name, string.Format(DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION, skillGroup.Name, item.Value));
					}
				}
			}
		}
		targetPanel.Commit();
	}

	private static void RefreshTraitsPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (!targetEntity.GetComponent<MinionIdentity>())
		{
			targetPanel.SetActive(active: false);
			return;
		}
		targetPanel.SetActive(active: true);
		foreach (Trait trait in targetEntity.GetComponent<Traits>().TraitList)
		{
			if (!string.IsNullOrEmpty(trait.Name))
			{
				targetPanel.SetLabel(trait.Id, trait.Name, trait.GetTooltip());
			}
		}
		targetPanel.Commit();
	}

	private static void RefreshEquipmentPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		Equipment equipment = targetEntity.GetComponent<MinionIdentity>().GetEquipment();
		bool flag = false;
		foreach (AssignableSlotInstance slot in equipment.Slots)
		{
			if (!slot.slot.showInUI || !slot.IsAssigned())
			{
				continue;
			}
			flag = true;
			string text = slot.assignable.GetComponent<KSelectable>().GetName();
			string text2 = "";
			List<Descriptor> list = new List<Descriptor>(GameUtil.GetGameObjectEffects(slot.assignable.gameObject));
			if (list.Count > 0)
			{
				text2 += "\n";
				foreach (Descriptor item in list)
				{
					text2 = text2 + "  • " + item.IndentedText() + "\n";
				}
			}
			targetPanel.SetLabel(slot.slot.Name, $"{slot.slot.Name}: {text}", string.Format(UI.DETAILTABS.PERSONALITY.EQUIPMENT.ASSIGNED_TOOLTIP, text, text2, targetEntity.GetProperName()));
		}
		if (!flag)
		{
			targetPanel.SetLabel("NoSuitAssigned", UI.DETAILTABS.PERSONALITY.EQUIPMENT.NOEQUIPMENT, string.Format(UI.DETAILTABS.PERSONALITY.EQUIPMENT.NOEQUIPMENT_TOOLTIP, targetEntity.GetProperName()));
		}
		targetPanel.Commit();
	}

	private static void RefreshAmenitiesPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		Ownables soleOwner = targetEntity.GetComponent<MinionIdentity>().GetSoleOwner();
		bool flag = false;
		foreach (AssignableSlotInstance slot in soleOwner.Slots)
		{
			if (!slot.slot.showInUI || !slot.IsAssigned())
			{
				continue;
			}
			flag = true;
			string text = slot.assignable.GetComponent<KSelectable>().GetName();
			string text2 = "";
			List<Descriptor> list = new List<Descriptor>(GameUtil.GetGameObjectEffects(slot.assignable.gameObject));
			if (list.Count > 0)
			{
				text2 += "\n";
				foreach (Descriptor item in list)
				{
					text2 = text2 + "  • " + item.IndentedText() + "\n";
				}
			}
			targetPanel.SetLabel(slot.slot.Name, $"{slot.slot.Name}: {text}", string.Format(UI.DETAILTABS.PERSONALITY.EQUIPMENT.ASSIGNED_TOOLTIP, text, text2, targetEntity.GetProperName()));
		}
		if (!flag)
		{
			targetPanel.SetLabel("NothingAssigned", UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES, string.Format(UI.DETAILTABS.PERSONALITY.EQUIPMENT.NO_ASSIGNABLES_TOOLTIP, targetEntity.GetProperName()));
		}
		targetPanel.Commit();
	}

	private static void RefreshAttributesPanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		if (!targetEntity.GetComponent<MinionIdentity>())
		{
			targetPanel.SetActive(active: false);
			return;
		}
		List<AttributeInstance> list = new List<AttributeInstance>(targetEntity.GetAttributes().AttributeTable).FindAll((AttributeInstance a) => a.Attribute.ShowInUI == Attribute.Display.Skill);
		if (list.Count > 0)
		{
			foreach (AttributeInstance item in list)
			{
				targetPanel.SetLabel(item.Id, $"{item.Name}: {item.GetFormattedValue()}", item.GetAttributeValueTooltip());
			}
		}
		targetPanel.Commit();
	}

	private static void RefreshResumePanel(CollapsibleDetailContentPanel targetPanel, GameObject targetEntity)
	{
		MinionResume component = targetEntity.GetComponent<MinionResume>();
		targetPanel.SetTitle(string.Format(UI.DETAILTABS.PERSONALITY.GROUPNAME_RESUME, targetEntity.name.ToUpper()));
		List<Skill> list = new List<Skill>();
		foreach (KeyValuePair<string, bool> item2 in component.MasteryBySkillID)
		{
			if (item2.Value)
			{
				Skill item = Db.Get().Skills.Get(item2.Key);
				list.Add(item);
			}
		}
		targetPanel.SetLabel("mastered_skills_header", UI.DETAILTABS.PERSONALITY.RESUME.MASTERED_SKILLS, UI.DETAILTABS.PERSONALITY.RESUME.MASTERED_SKILLS_TOOLTIP);
		if (list.Count == 0)
		{
			targetPanel.SetLabel("no_skills", "  • " + UI.DETAILTABS.PERSONALITY.RESUME.NO_MASTERED_SKILLS.NAME, string.Format(UI.DETAILTABS.PERSONALITY.RESUME.NO_MASTERED_SKILLS.TOOLTIP, targetEntity.name));
		}
		else
		{
			foreach (Skill item3 in list)
			{
				string text = "";
				foreach (SkillPerk perk in item3.perks)
				{
					text = text + "  • " + perk.Name + "\n";
				}
				targetPanel.SetLabel(item3.Id, "  • " + item3.Name, item3.description + "\n" + text);
			}
		}
		targetPanel.Commit();
	}
}
