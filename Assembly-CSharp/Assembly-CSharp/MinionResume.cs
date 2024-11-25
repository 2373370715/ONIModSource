using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Database;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn), AddComponentMenu("KMonoBehaviour/scripts/MinionResume")]
public class MinionResume : IExperienceRecipient, ISaveLoadable, ISim200ms {
    public enum SkillMasteryConditions {
        SkillAptitude,
        StressWarning,
        UnableToLearn,
        NeedsSkillPoints,
        MissingPreviousSkill
    }

    [Serialize]
    public Dictionary<HashedString, float> AptitudeByRoleGroup = new Dictionary<HashedString, float>();

    [Serialize]
    public Dictionary<HashedString, float> AptitudeBySkillGroup = new Dictionary<HashedString, float>();

    public float DEBUG_ActiveExperienceGained;
    public float DEBUG_PassiveExperienceGained;
    public float DEBUG_SecondsAlive;

    [Serialize]
    public List<string> GrantedSkillIDs = new List<string>();

    private Notification lastSkillNotification;

    [Serialize]
    public Dictionary<string, bool> MasteryByRoleID = new Dictionary<string, bool>();

    [Serialize]
    public Dictionary<string, bool> MasteryBySkillID = new Dictionary<string, bool>();

    private readonly Dictionary<string, bool> ownedHats = new Dictionary<string, bool>();
    private          AttributeModifier        skillsMoraleExpectationModifier;
    private          AttributeModifier        skillsMoraleModifier;

    [field: MyCmpReq]
    public MinionIdentity GetIdentity { get; }

    [field: Serialize]
    public float TotalExperienceGained { get; private set; }

    public int TotalSkillPointsGained => CalculateTotalSkillPointsGained(TotalExperienceGained);

    public int SkillsMastered {
        get {
            var num = 0;
            foreach (var keyValuePair in MasteryBySkillID)
                if (keyValuePair.Value)
                    num++;

            return num;
        }
    }

    public int AvailableSkillpoints =>
        TotalSkillPointsGained - SkillsMastered + (GrantedSkillIDs == null ? 0 : GrantedSkillIDs.Count);

    [field: Serialize]
    public string CurrentRole { get; private set; } = "NoRole";

    [field: Serialize]
    public string CurrentHat { get; private set; }

    [field: Serialize]
    public string TargetHat { get; private set; }

    [field: Serialize]
    public string TargetRole { get; } = "NoRole";

    public void Sim200ms(float dt) {
        DEBUG_SecondsAlive += dt;
        if (!GetComponent<KPrefabID>().HasTag(GameTags.Dead)) {
            DEBUG_PassiveExperienceGained += dt * SKILLS.PASSIVE_EXPERIENCE_PORTION;
            AddExperience(dt * SKILLS.PASSIVE_EXPERIENCE_PORTION);
        }
    }

    public static int CalculateTotalSkillPointsGained(float experience) {
        return Mathf.FloorToInt(Mathf.Pow(experience / SKILLS.TARGET_SKILLS_CYCLE / 600f,
                                          1f         / SKILLS.EXPERIENCE_LEVEL_POWER) *
                                SKILLS.TARGET_SKILLS_EARNED);
    }

    [OnDeserialized]
    private void OnDeserializedMethod() {
        if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 7)) {
            foreach (var keyValuePair in MasteryByRoleID)
                if (keyValuePair.Value && keyValuePair.Key != "NoRole")
                    ForceAddSkillPoint();

            foreach (var keyValuePair2 in AptitudeByRoleGroup)
                AptitudeBySkillGroup[keyValuePair2.Key] = keyValuePair2.Value;
        }

        if (TotalSkillPointsGained > 1000 || TotalSkillPointsGained < 0) ForceSetSkillPoints(100);
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        Components.MinionResumes.Add(this);
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        GrantedSkillIDs.RemoveAll(x => Db.Get().Skills.TryGet(x) == null);
        var list = new List<string>();
        foreach (var text in MasteryBySkillID.Keys)
            if (Db.Get().Skills.TryGet(text) == null)
                list.Add(text);

        foreach (var key in list) MasteryBySkillID.Remove(key);
        if (GrantedSkillIDs == null) GrantedSkillIDs = new List<string>();
        var list2                                    = new List<string>();
        foreach (var keyValuePair in MasteryBySkillID)
            if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).deprecated)
                list2.Add(keyValuePair.Key);

        foreach (var skillId in list2) UnmasterSkill(skillId);
        foreach (var keyValuePair2 in MasteryBySkillID)
            if (keyValuePair2.Value) {
                var skill = Db.Get().Skills.Get(keyValuePair2.Key);
                foreach (var skillPerk in skill.perks)
                    if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(skillPerk.requiredDlcIds)) {
                        if (skillPerk.OnRemove != null) skillPerk.OnRemove(this);
                        if (skillPerk.OnApply  != null) skillPerk.OnApply(this);
                    }

                if (!ownedHats.ContainsKey(skill.hat)) ownedHats.Add(skill.hat, true);
            }

        UpdateExpectations();
        UpdateMorale();
        var component = GetComponent<KBatchedAnimController>();
        ApplyHat(CurrentHat, component);
        ShowNewSkillPointNotification();
    }

    public void RestoreResume(Dictionary<string, bool>        MasteryBySkillID,
                              Dictionary<HashedString, float> AptitudeBySkillGroup,
                              List<string>                    GrantedSkillIDs,
                              float                           totalExperienceGained) {
        this.MasteryBySkillID     = MasteryBySkillID;
        this.GrantedSkillIDs      = GrantedSkillIDs != null ? GrantedSkillIDs : new List<string>();
        this.AptitudeBySkillGroup = AptitudeBySkillGroup;
        TotalExperienceGained     = totalExperienceGained;
    }

    protected override void OnCleanUp() {
        Components.MinionResumes.Remove(this);
        if (lastSkillNotification != null) {
            Game.Instance.GetComponent<Notifier>().Remove(lastSkillNotification);
            lastSkillNotification = null;
        }

        base.OnCleanUp();
    }

    public bool HasMasteredSkill(string skillId) {
        return MasteryBySkillID.ContainsKey(skillId) && MasteryBySkillID[skillId];
    }

    public void UpdateUrge() {
        if (TargetHat != CurrentHat) {
            if (!gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
                gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);
        } else
            gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);
    }

    public void SetHats(string current, string target) {
        CurrentHat = current;
        TargetHat  = target;
    }

    public void SetCurrentRole(string role_id) { CurrentRole = role_id; }

    private void ApplySkillPerks(string skillId) {
        foreach (var skillPerk in Db.Get().Skills.Get(skillId).perks)
            if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(skillPerk.requiredDlcIds) && skillPerk.OnApply != null)
                skillPerk.OnApply(this);
    }

    private void RemoveSkillPerks(string skillId) {
        foreach (var skillPerk in Db.Get().Skills.Get(skillId).perks)
            if (SaveLoader.Instance.IsAllDlcActiveForCurrentSave(skillPerk.requiredDlcIds) &&
                skillPerk.OnRemove != null)
                skillPerk.OnRemove(this);
    }

    public bool IsAbleToLearnSkill(string skillId) {
        var skill        = Db.Get().Skills.Get(skillId);
        var choreGroupID = Db.Get().SkillGroups.Get(skill.skillGroup).choreGroupID;
        if (!string.IsNullOrEmpty(choreGroupID)) {
            var component = GetComponent<Traits>();
            if (component != null && component.IsChoreGroupDisabled(choreGroupID)) return false;
        }

        return true;
    }

    public bool BelowMoraleExpectation(Skill skill) {
        var num               = Db.Get().Attributes.QualityOfLife.Lookup(this).GetTotalValue();
        var totalValue        = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this).GetTotalValue();
        var moraleExpectation = skill.GetMoraleExpectation();
        if (AptitudeBySkillGroup.ContainsKey(skill.skillGroup) && AptitudeBySkillGroup[skill.skillGroup] > 0f)
            num += 1f;

        return totalValue + moraleExpectation <= num;
    }

    public bool HasMasteredDirectlyRequiredSkillsForSkill(Skill skill) {
        for (var i = 0; i < skill.priorSkills.Count; i++)
            if (!HasMasteredSkill(skill.priorSkills[i]))
                return false;

        return true;
    }

    public bool HasSkillPointsRequiredForSkill(Skill skill) { return AvailableSkillpoints >= 1; }

    public bool HasSkillAptitude(Skill skill) {
        return AptitudeBySkillGroup.ContainsKey(skill.skillGroup) && AptitudeBySkillGroup[skill.skillGroup] > 0f;
    }

    public bool HasBeenGrantedSkill(Skill skill) {
        return GrantedSkillIDs != null && GrantedSkillIDs.Contains(skill.Id);
    }

    public bool HasBeenGrantedSkill(string id) { return GrantedSkillIDs != null && GrantedSkillIDs.Contains(id); }

    public SkillMasteryConditions[] GetSkillMasteryConditions(string skillId) {
        var list  = new List<SkillMasteryConditions>();
        var skill = Db.Get().Skills.Get(skillId);
        if (HasSkillAptitude(skill)) list.Add(SkillMasteryConditions.SkillAptitude);
        if (!BelowMoraleExpectation(skill)) list.Add(SkillMasteryConditions.StressWarning);
        if (!IsAbleToLearnSkill(skillId)) list.Add(SkillMasteryConditions.UnableToLearn);
        if (!HasSkillPointsRequiredForSkill(skill)) list.Add(SkillMasteryConditions.NeedsSkillPoints);
        if (!HasMasteredDirectlyRequiredSkillsForSkill(skill)) list.Add(SkillMasteryConditions.MissingPreviousSkill);
        return list.ToArray();
    }

    public bool CanMasterSkill(SkillMasteryConditions[] masteryConditions) {
        return !Array.Exists(masteryConditions,
                             element => element == SkillMasteryConditions.UnableToLearn    ||
                                        element == SkillMasteryConditions.NeedsSkillPoints ||
                                        element == SkillMasteryConditions.MissingPreviousSkill);
    }

    public bool OwnsHat(string hatId) { return ownedHats.ContainsKey(hatId) && ownedHats[hatId]; }

    public void SkillLearned() {
        if (gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
            gameObject.GetComponent<ChoreConsumer>().RemoveUrge(Db.Get().Urges.LearnSkill);

        foreach (var key in ownedHats.Keys.ToList()) ownedHats[key] = true;
        if (TargetHat != null && CurrentHat != TargetHat) new PutOnHatChore(this, Db.Get().ChoreTypes.SwitchHat);
    }

    public void MasterSkill(string skillId) {
        if (!gameObject.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.LearnSkill))
            gameObject.GetComponent<ChoreConsumer>().AddUrge(Db.Get().Urges.LearnSkill);

        MasteryBySkillID[skillId] = true;
        ApplySkillPerks(skillId);
        UpdateExpectations();
        UpdateMorale();
        TriggerMasterSkillEvents();
        GameScheduler.Instance.Schedule("Morale Tutorial",
                                        2f,
                                        delegate {
                                            Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Morale);
                                        });

        if (!ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
            ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);

        if (AvailableSkillpoints == 0 && lastSkillNotification != null) {
            Game.Instance.GetComponent<Notifier>().Remove(lastSkillNotification);
            lastSkillNotification = null;
        }
    }

    public void UnmasterSkill(string skillId) {
        if (MasteryBySkillID.ContainsKey(skillId)) {
            MasteryBySkillID.Remove(skillId);
            RemoveSkillPerks(skillId);
            UpdateExpectations();
            UpdateMorale();
            TriggerMasterSkillEvents();
        }
    }

    public void GrantSkill(string skillId) {
        if (GrantedSkillIDs == null) GrantedSkillIDs = new List<string>();
        if (!HasBeenGrantedSkill(skillId)) {
            MasteryBySkillID[skillId] = true;
            ApplySkillPerks(skillId);
            GrantedSkillIDs.Add(skillId);
            UpdateExpectations();
            UpdateMorale();
            TriggerMasterSkillEvents();
            if (!ownedHats.ContainsKey(Db.Get().Skills.Get(skillId).hat))
                ownedHats.Add(Db.Get().Skills.Get(skillId).hat, false);
        }
    }

    public void UngrantSkill(string skillId) {
        if (GrantedSkillIDs != null) GrantedSkillIDs.RemoveAll(match => match == skillId);
        UnmasterSkill(skillId);
    }

    public Sprite GetSkillGrantSourceIcon(string skillID) {
        if (!GrantedSkillIDs.Contains(skillID)) return null;

        var smi = gameObject.GetSMI<BionicUpgradesMonitor.Instance>();
        if (smi != null)
            foreach (var upgradeComponentSlot in smi.upgradeComponentSlots)
                if (upgradeComponentSlot.HasUpgradeInstalled)
                    return Def.GetUISprite(upgradeComponentSlot.installedUpgradeComponent.gameObject).first;

        return Assets.GetSprite("skill_granted_trait");
    }

    private void TriggerMasterSkillEvents() {
        Trigger(540773776);
        Game.Instance.Trigger(-1523247426, this);
    }

    public void ForceSetSkillPoints(int points) { TotalExperienceGained = CalculatePreviousExperienceBar(points); }

    public void ForceAddSkillPoint() {
        AddExperience(CalculateNextExperienceBar(TotalSkillPointsGained) - TotalExperienceGained);
    }

    public static float CalculateNextExperienceBar(int current_skill_points) {
        return Mathf.Pow((current_skill_points + 1) / (float)SKILLS.TARGET_SKILLS_EARNED,
                         SKILLS.EXPERIENCE_LEVEL_POWER) *
               SKILLS.TARGET_SKILLS_CYCLE               *
               600f;
    }

    public static float CalculatePreviousExperienceBar(int current_skill_points) {
        return Mathf.Pow(current_skill_points / (float)SKILLS.TARGET_SKILLS_EARNED, SKILLS.EXPERIENCE_LEVEL_POWER) *
               SKILLS.TARGET_SKILLS_CYCLE                                                                          *
               600f;
    }

    private void UpdateExpectations() {
        var num = 0;
        foreach (var keyValuePair in MasteryBySkillID)
            if (keyValuePair.Value && !HasBeenGrantedSkill(keyValuePair.Key)) {
                var skill = Db.Get().Skills.Get(keyValuePair.Key);
                num += skill.tier + 1;
            }

        var attributeInstance = Db.Get().Attributes.QualityOfLifeExpectation.Lookup(this);
        if (skillsMoraleExpectationModifier != null) {
            attributeInstance.Remove(skillsMoraleExpectationModifier);
            skillsMoraleExpectationModifier = null;
        }

        if (num > 0) {
            skillsMoraleExpectationModifier
                = new AttributeModifier(attributeInstance.Id, num, DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_MOD_NAME);

            attributeInstance.Add(skillsMoraleExpectationModifier);
        }
    }

    private void UpdateMorale() {
        var num = 0;
        foreach (var keyValuePair in MasteryBySkillID)
            if (keyValuePair.Value && !HasBeenGrantedSkill(keyValuePair.Key)) {
                var skill = Db.Get().Skills.Get(keyValuePair.Key);
                var num2 = 0f;
                if (AptitudeBySkillGroup.TryGetValue(new HashedString(skill.skillGroup), out num2)) num += (int)num2;
            }

        var attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup(this);
        if (skillsMoraleModifier != null) {
            attributeInstance.Remove(skillsMoraleModifier);
            skillsMoraleModifier = null;
        }

        if (num > 0) {
            skillsMoraleModifier = new AttributeModifier(attributeInstance.Id,
                                                         num,
                                                         DUPLICANTS.NEEDS.QUALITYOFLIFE.APTITUDE_SKILLS_MOD_NAME);

            attributeInstance.Add(skillsMoraleModifier);
        }
    }

    private void OnSkillPointGained() {
        Game.Instance.Trigger(1505456302, this);
        ShowNewSkillPointNotification();
        if (PopFXManager.Instance != null) {
            var text = MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME.Replace("{Duplicant}", GetIdentity.GetProperName());
            PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus,
                                          text,
                                          transform,
                                          new Vector3(0f, 0.5f, 0f));
        }

        new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, -0.1f)).StartSM();
    }

    private void ShowNewSkillPointNotification() {
        if (AvailableSkillpoints == 1) {
            lastSkillNotification = new ManagementMenuNotification(Action.ManageSkills,
                                                                   NotificationValence.Good,
                                                                   GetIdentity.GetSoleOwner()
                                                                              .gameObject.GetInstanceID()
                                                                              .ToString(),
                                                                   MISC.NOTIFICATIONS.SKILL_POINT_EARNED.NAME
                                                                       .Replace("{Duplicant}",
                                                                                GetIdentity.GetProperName()),
                                                                   NotificationType.Good,
                                                                   GetSkillPointGainedTooltip,
                                                                   GetIdentity,
                                                                   true,
                                                                   0f,
                                                                   delegate {
                                                                       ManagementMenu.Instance.OpenSkills(GetIdentity);
                                                                   });

            GetComponent<Notifier>().Add(lastSkillNotification);
        }
    }

    private string GetSkillPointGainedTooltip(List<Notification> notifications, object data) {
        return MISC.NOTIFICATIONS.SKILL_POINT_EARNED.TOOLTIP.Replace("{Duplicant}",
                                                                     ((MinionIdentity)data).GetProperName());
    }

    public void SetAptitude(HashedString skillGroupID, float amount) { AptitudeBySkillGroup[skillGroupID] = amount; }

    public float GetAptitudeExperienceMultiplier(HashedString skillGroupId, float buildingFrequencyMultiplier) {
        var num = 0f;
        AptitudeBySkillGroup.TryGetValue(skillGroupId, out num);
        return 1f + num * SKILLS.APTITUDE_EXPERIENCE_MULTIPLIER * buildingFrequencyMultiplier;
    }

    public void AddExperience(float amount) {
        var num  = TotalExperienceGained;
        var num2 = CalculateNextExperienceBar(TotalSkillPointsGained);
        TotalExperienceGained += amount;
        if (isSpawned && TotalExperienceGained >= num2 && num < num2) OnSkillPointGained();
    }

    public override void AddExperienceWithAptitude(string skillGroupId, float amount, float buildingMultiplier) {
        var num = amount                                                            *
                  GetAptitudeExperienceMultiplier(skillGroupId, buildingMultiplier) *
                  SKILLS.ACTIVE_EXPERIENCE_PORTION;

        DEBUG_ActiveExperienceGained += num;
        AddExperience(num);
    }

    public bool HasPerk(HashedString perkId) {
        foreach (var keyValuePair in MasteryBySkillID)
            if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perkId))
                return true;

        return false;
    }

    public bool HasPerk(SkillPerk perk) {
        foreach (var keyValuePair in MasteryBySkillID)
            if (keyValuePair.Value && Db.Get().Skills.Get(keyValuePair.Key).GivesPerk(perk))
                return true;

        return false;
    }

    public void RemoveHat() { RemoveHat(GetComponent<KBatchedAnimController>()); }

    public static void RemoveHat(KBatchedAnimController controller) {
        var hat       = Db.Get().AccessorySlots.Hat;
        var component = controller.GetComponent<Accessorizer>();
        if (component != null) {
            var accessory = component.GetAccessory(hat);
            if (accessory != null) component.RemoveAccessory(accessory);
        } else
            controller.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride(hat.targetSymbolId, 4);

        controller.SetSymbolVisiblity(hat.targetSymbolId,                             false);
        controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
        controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId,    true);
    }

    public static void AddHat(string hat_id, KBatchedAnimController controller) {
        var hat       = Db.Get().AccessorySlots.Hat;
        var accessory = hat.Lookup(hat_id);
        if (accessory == null) Debug.LogWarning("Missing hat: " + hat_id);
        var component = controller.GetComponent<Accessorizer>();
        if (component != null) {
            var accessory2 = component.GetAccessory(Db.Get().AccessorySlots.Hat);
            if (accessory2 != null) component.RemoveAccessory(accessory2);
            if (accessory  != null) component.AddAccessory(accessory);
        } else {
            var component2 = controller.GetComponent<SymbolOverrideController>();
            component2.TryRemoveSymbolOverride(hat.targetSymbolId, 4);
            component2.AddSymbolOverride(hat.targetSymbolId, accessory.symbol, 4);
        }

        controller.SetSymbolVisiblity(hat.targetSymbolId,                             true);
        controller.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
        controller.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId,    false);
    }

    public void ApplyTargetHat() {
        var component = GetComponent<KBatchedAnimController>();
        ApplyHat(TargetHat, component);
        CurrentHat = TargetHat;
        TargetHat  = null;
    }

    public static void ApplyHat(string hat_id, KBatchedAnimController controller) {
        if (hat_id.IsNullOrWhiteSpace()) {
            RemoveHat(controller);
            return;
        }

        AddHat(hat_id, controller);
    }

    public string GetSkillsSubtitle() {
        return string.Format(DUPLICANTS.NEEDS.QUALITYOFLIFE.TOTAL_SKILL_POINTS, TotalSkillPointsGained);
    }

    public static bool AnyMinionHasPerk(string perk, int worldId = -1) {
        using (var enumerator
               = (from minion in worldId >= 0
                                     ? Components.MinionResumes.GetWorldItems(worldId, true)
                                     : Components.MinionResumes.Items
                  where !minion.HasTag(GameTags.Dead)
                  select minion).ToList()
                                .GetEnumerator()) {
            while (enumerator.MoveNext())
                if (enumerator.Current.HasPerk(perk))
                    return true;
        }

        return false;
    }

    public static bool AnyOtherMinionHasPerk(string perk, MinionResume me) {
        foreach (var minionResume in Components.MinionResumes.Items)
            if (!(minionResume == me) && minionResume.HasPerk(perk))
                return true;

        return false;
    }

    public void ResetSkillLevels(bool returnSkillPoints = true) {
        var list = new List<string>();
        foreach (var keyValuePair in MasteryBySkillID)
            if (keyValuePair.Value)
                list.Add(keyValuePair.Key);

        foreach (var skillId in list) UnmasterSkill(skillId);
    }
}