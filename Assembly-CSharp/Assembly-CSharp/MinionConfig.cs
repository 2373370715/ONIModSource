using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using Random = UnityEngine.Random;

public class MinionConfig : IEntityConfig {
    public static Tag                                                MODEL = GameTags.Minions.Models.Standard;
    public static string                                             NAME  = DUPLICANTS.MODEL.STANDARD.NAME;
    public static string                                             ID    = MODEL.ToString();
    public        Func<RationalAi.Instance, StateMachine.Instance>[] RATIONAL_AI_STATE_MACHINES;

    public MinionConfig() {
        var array  = BaseMinionConfig.BaseRationalAiStateMachines();
        var array2 = new Func<RationalAi.Instance, StateMachine.Instance>[9];
        array2[0]                  = smi => new BreathMonitor.Instance(smi.master);
        array2[1]                  = smi => new SteppedInMonitor.Instance(smi.master);
        array2[2]                  = smi => new Dreamer.Instance(smi.master);
        array2[3]                  = smi => new StaminaMonitor.Instance(smi.master);
        array2[4]                  = smi => new RationMonitor.Instance(smi.master);
        array2[5]                  = smi => new CalorieMonitor.Instance(smi.master);
        array2[6]                  = smi => new BladderMonitor.Instance(smi.master);
        array2[7]                  = smi => new HygieneMonitor.Instance(smi.master);
        array2[8]                  = smi => new TiredMonitor.Instance(smi.master);
        RATIONAL_AI_STATE_MACHINES = array.Append(array2);
        base..ctor();
    }

    public GameObject CreatePrefab() {
        return BaseMinionConfig.BaseMinion(MODEL, GetAttributes(), GetAmounts(), GetTraits());
    }

    public void OnPrefabInit(GameObject go) {
        BaseMinionConfig.BasePrefabInit(go, MODEL);
        var statsFor = DUPLICANTSTATS.GetStatsFor(MODEL);
        Db.Get().Amounts.Bladder.Lookup(go).value = Random.Range(0f, 10f);
        var amountInstance = Db.Get().Amounts.Calories.Lookup(go);
        amountInstance.value = (statsFor.BaseStats.HUNGRY_THRESHOLD + statsFor.BaseStats.SATISFIED_THRESHOLD) *
                               0.5f                                                                           *
                               amountInstance.GetMax();

        var amountInstance2 = Db.Get().Amounts.Stamina.Lookup(go);
        amountInstance2.value = amountInstance2.GetMax();
    }

    public void OnSpawn(GameObject go) {
        var component = go.GetComponent<Sensors>();
        component.Add(new ToiletSensor(component));
        BaseMinionConfig.BaseOnSpawn(go, MODEL, RATIONAL_AI_STATE_MACHINES);
        if (go.GetComponent<OxygenBreather>().GetGasProvider() == null)
            go.GetComponent<OxygenBreather>().SetGasProvider(new GasBreatherFromWorldProvider());
    }

    public string[] GetDlcIds() { return DlcManager.AVAILABLE_ALL_VERSIONS; }

    public static string[] GetAttributes() {
        return BaseMinionConfig.BaseMinionAttributes()
                               .Append(new[] {
                                   Db.Get().Attributes.FoodExpectation.Id, Db.Get().Attributes.ToiletEfficiency.Id
                               });
    }

    public static string[] GetAmounts() {
        return BaseMinionConfig.BaseMinionAmounts()
                               .Append(new[] {
                                   Db.Get().Amounts.Bladder.Id,
                                   Db.Get().Amounts.Stamina.Id,
                                   Db.Get().Amounts.Calories.Id
                               });
    }

    public static AttributeModifier[] GetTraits() {
        return BaseMinionConfig.BaseMinionTraits(MODEL)
                               .Append(new[] {
                                   new AttributeModifier(Db.Get().Attributes.FoodExpectation.Id,
                                                         DUPLICANTSTATS.GetStatsFor(MODEL)
                                                                       .BaseStats.FOOD_QUALITY_EXPECTATION,
                                                         NAME),
                                   new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id,
                                                         DUPLICANTSTATS.GetStatsFor(MODEL).BaseStats.MAX_CALORIES,
                                                         NAME),
                                   new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id,
                                                         DUPLICANTSTATS.GetStatsFor(MODEL)
                                                                       .BaseStats.CALORIES_BURNED_PER_SECOND,
                                                         NAME),
                                   new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id,
                                                         DUPLICANTSTATS.GetStatsFor(MODEL)
                                                                       .BaseStats.STAMINA_USED_PER_SECOND,
                                                         NAME),
                                   new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id,
                                                         DUPLICANTSTATS.GetStatsFor(MODEL)
                                                                       .BaseStats.BLADDER_INCREASE_PER_SECOND,
                                                         NAME),
                                   new AttributeModifier(Db.Get().Attributes.ToiletEfficiency.Id,
                                                         DUPLICANTSTATS.GetStatsFor(MODEL).BaseStats.TOILET_EFFICIENCY,
                                                         NAME)
                               });
    }
}