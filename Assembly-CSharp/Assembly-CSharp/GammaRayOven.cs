using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

public class GammaRayOven : ComplexFabricator, IGameObjectEffectDescriptor {
    [SerializeField]
    private readonly int diseaseCountKillRate = 100;

    public override List<Descriptor> GetDescriptors(GameObject go) {
        var descriptors = base.GetDescriptors(go);
        descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.REMOVES_DISEASE,
                                       UI.BUILDINGEFFECTS.TOOLTIPS.REMOVES_DISEASE));

        return descriptors;
    }

    protected override void OnPrefabInit() {
        base.OnPrefabInit();
        choreType            = Db.Get().ChoreTypes.Cook;
        fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
    }

    protected override void OnSpawn() {
        base.OnSpawn();
        this.workable.WorkerStatusItem              = Db.Get().DuplicantStatusItems.Cooking;
        this.workable.overrideAnims                 = new[] { Assets.GetAnim("anim_interacts_cookstation_kanim") };
        this.workable.AttributeConverter            = Db.Get().AttributeConverters.CookingSpeed;
        this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
        this.workable.SkillExperienceSkillGroup     = Db.Get().SkillGroups.Cooking.Id;
        this.workable.SkillExperienceMultiplier     = SKILLS.MOST_DAY_EXPERIENCE;
        var workable = this.workable;
        workable.OnWorkTickActions = (Action<WorkerBase, float>)Delegate.Combine(workable.OnWorkTickActions,
         new Action<WorkerBase, float>(delegate(WorkerBase worker, float dt) {
                                           Debug.Assert(worker != null, "How did we get a null worker?");
                                           if (diseaseCountKillRate > 0) {
                                               var component = GetComponent<PrimaryElement>();
                                               var num       = Math.Max(1, (int)(diseaseCountKillRate * dt));
                                               component.ModifyDiseaseCount(-num, "GammaRayOven");
                                           }
                                       }));

        GetComponent<Radiator>().emitter.enabled = false;
        Subscribe(824508782, UpdateRadiator);
    }

    private void UpdateRadiator(object data) { GetComponent<Radiator>().emitter.enabled = operational.IsActive; }

    protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe) {
        var list = base.SpawnOrderProduct(recipe);
        foreach (var gameObject in list) {
            var component = gameObject.GetComponent<PrimaryElement>();
            component.ModifyDiseaseCount(-component.DiseaseCount, "GammaRayOven.CompleteOrder");
        }

        GetComponent<Operational>().SetActive(false);
        return list;
    }
}