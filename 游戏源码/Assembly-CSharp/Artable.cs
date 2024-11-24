using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using TUNING;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/Workable/Artable")]
public class Artable : Workable
{
	[Serialize]
	private string currentStage;

	[Serialize]
	private string userChosenTargetStage;

	private AttributeModifier artQualityDecorModifier;

	private string defaultArtworkId = "Default";

	public string defaultAnimName;

	private WorkChore<Artable> chore;

	public string CurrentStage => currentStage;

	protected Artable()
	{
		faceTargetWhenWorking = true;
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		workerStatusItem = Db.Get().DuplicantStatusItems.Arting;
		attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		skillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
		skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		requiredSkillPerk = Db.Get().SkillPerks.CanArt.Id;
		SetWorkTime(80f);
	}

	protected override void OnSpawn()
	{
		GetComponent<KPrefabID>().PrefabID();
		if (string.IsNullOrEmpty(currentStage) || currentStage == defaultArtworkId)
		{
			SetDefault();
		}
		else
		{
			SetStage(currentStage, skip_effect: true);
		}
		shouldShowSkillPerkStatusItem = false;
		base.OnSpawn();
	}

	[OnDeserialized]
	public void OnDeserialized()
	{
		if (Db.GetArtableStages().TryGet(currentStage) == null && currentStage != defaultArtworkId)
		{
			string id = $"{GetComponent<KPrefabID>().PrefabID().ToString()}_{currentStage}";
			if (Db.GetArtableStages().TryGet(id) == null)
			{
				Debug.LogWarning("Failed up to update " + currentStage + " to ArtableStages");
				currentStage = defaultArtworkId;
			}
			else
			{
				currentStage = id;
			}
		}
	}

	protected override void OnCompleteWork(Worker worker)
	{
		if (string.IsNullOrEmpty(userChosenTargetStage))
		{
			Db db = Db.Get();
			Tag prefab_id = GetComponent<KPrefabID>().PrefabID();
			List<ArtableStage> prefabStages = Db.GetArtableStages().GetPrefabStages(prefab_id);
			ArtableStatusItem artist_skill = db.ArtableStatuses.LookingUgly;
			MinionResume component = worker.GetComponent<MinionResume>();
			if (component != null)
			{
				if (component.HasPerk(db.SkillPerks.CanArtGreat.Id))
				{
					artist_skill = db.ArtableStatuses.LookingGreat;
				}
				else if (component.HasPerk(db.SkillPerks.CanArtOkay.Id))
				{
					artist_skill = db.ArtableStatuses.LookingOkay;
				}
			}
			prefabStages.RemoveAll((ArtableStage stage) => stage.statusItem.StatusType > artist_skill.StatusType || stage.statusItem.StatusType == ArtableStatuses.ArtableStatusType.AwaitingArting);
			prefabStages.Sort((ArtableStage x, ArtableStage y) => y.statusItem.StatusType.CompareTo(x.statusItem.StatusType));
			ArtableStatuses.ArtableStatusType highest_type = prefabStages[0].statusItem.StatusType;
			prefabStages.RemoveAll((ArtableStage stage) => stage.statusItem.StatusType < highest_type);
			prefabStages.RemoveAll((ArtableStage stage) => !stage.IsUnlocked());
			prefabStages.Shuffle();
			SetStage(prefabStages[0].id, skip_effect: false);
			if (prefabStages[0].cheerOnComplete)
			{
				new EmoteChore(worker.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer);
			}
			else
			{
				new EmoteChore(worker.GetComponent<ChoreProvider>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Disappointed);
			}
		}
		else
		{
			SetStage(userChosenTargetStage, skip_effect: false);
			userChosenTargetStage = null;
		}
		shouldShowSkillPerkStatusItem = false;
		UpdateStatusItem();
		Prioritizable.RemoveRef(base.gameObject);
	}

	public void SetDefault()
	{
		currentStage = defaultArtworkId;
		GetComponent<KBatchedAnimController>().SwapAnims(GetComponent<Building>().Def.AnimFiles);
		GetComponent<KAnimControllerBase>().Play(defaultAnimName);
		KSelectable component = GetComponent<KSelectable>();
		BuildingDef def = GetComponent<Building>().Def;
		component.SetName(def.Name);
		component.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().ArtableStatuses.AwaitingArting, this);
		this.GetAttributes().Remove(artQualityDecorModifier);
		shouldShowSkillPerkStatusItem = false;
		UpdateStatusItem();
		if (currentStage == defaultArtworkId)
		{
			shouldShowSkillPerkStatusItem = true;
			Prioritizable.AddRef(base.gameObject);
			chore = new WorkChore<Artable>(Db.Get().ChoreTypes.Art, this);
			chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, requiredSkillPerk);
		}
	}

	public virtual void SetStage(string stage_id, bool skip_effect)
	{
		ArtableStage artableStage = Db.GetArtableStages().Get(stage_id);
		if (artableStage == null)
		{
			Debug.LogError("Missing stage: " + stage_id);
			return;
		}
		currentStage = artableStage.id;
		GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[1] { Assets.GetAnim(artableStage.animFile) });
		GetComponent<KAnimControllerBase>().Play(artableStage.anim);
		this.GetAttributes().Remove(artQualityDecorModifier);
		if (artableStage.decor != 0)
		{
			artQualityDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, artableStage.decor, "Art Quality");
			this.GetAttributes().Add(artQualityDecorModifier);
		}
		KSelectable component = GetComponent<KSelectable>();
		component.SetName(artableStage.Name);
		component.SetStatusItem(Db.Get().StatusItemCategories.Main, artableStage.statusItem, this);
		base.gameObject.GetComponent<BuildingComplete>().SetDescriptionFlavour(artableStage.Description);
		shouldShowSkillPerkStatusItem = false;
		UpdateStatusItem();
	}

	public void SetUserChosenTargetState(string stageID)
	{
		SetDefault();
		userChosenTargetStage = stageID;
	}
}
