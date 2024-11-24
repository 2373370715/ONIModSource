using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200047D RID: 1149
public class MinionConfig : IEntityConfig
{
	// Token: 0x0600141C RID: 5148 RVA: 0x000AECE6 File Offset: 0x000ACEE6
	public static string[] GetAttributes()
	{
		return BaseMinionConfig.BaseMinionAttributes().Append(new string[]
		{
			Db.Get().Attributes.FoodExpectation.Id,
			Db.Get().Attributes.ToiletEfficiency.Id
		});
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x0018FE44 File Offset: 0x0018E044
	public static string[] GetAmounts()
	{
		return BaseMinionConfig.BaseMinionAmounts().Append(new string[]
		{
			Db.Get().Amounts.Bladder.Id,
			Db.Get().Amounts.Stamina.Id,
			Db.Get().Amounts.Calories.Id
		});
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x0018FEA8 File Offset: 0x0018E0A8
	public static AttributeModifier[] GetTraits()
	{
		return BaseMinionConfig.BaseMinionTraits(MinionConfig.MODEL).Append(new AttributeModifier[]
		{
			new AttributeModifier(Db.Get().Attributes.FoodExpectation.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.FOOD_QUALITY_EXPECTATION, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.MAX_CALORIES, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.CALORIES_BURNED_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.STAMINA_USED_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.BLADDER_INCREASE_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Attributes.ToiletEfficiency.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.TOILET_EFFICIENCY, MinionConfig.NAME, false, false, true)
		});
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x000AED26 File Offset: 0x000ACF26
	public GameObject CreatePrefab()
	{
		return BaseMinionConfig.BaseMinion(MinionConfig.MODEL, MinionConfig.GetAttributes(), MinionConfig.GetAmounts(), MinionConfig.GetTraits());
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x00190030 File Offset: 0x0018E230
	public void OnPrefabInit(GameObject go)
	{
		BaseMinionConfig.BasePrefabInit(go, MinionConfig.MODEL);
		DUPLICANTSTATS statsFor = DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL);
		Db.Get().Amounts.Bladder.Lookup(go).value = UnityEngine.Random.Range(0f, 10f);
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(go);
		amountInstance.value = (statsFor.BaseStats.HUNGRY_THRESHOLD + statsFor.BaseStats.SATISFIED_THRESHOLD) * 0.5f * amountInstance.GetMax();
		AmountInstance amountInstance2 = Db.Get().Amounts.Stamina.Lookup(go);
		amountInstance2.value = amountInstance2.GetMax();
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x001900DC File Offset: 0x0018E2DC
	public void OnSpawn(GameObject go)
	{
		Sensors component = go.GetComponent<Sensors>();
		component.Add(new ToiletSensor(component));
		BaseMinionConfig.BaseOnSpawn(go, MinionConfig.MODEL, this.RATIONAL_AI_STATE_MACHINES);
		if (go.GetComponent<OxygenBreather>().GetGasProvider() == null)
		{
			go.GetComponent<OxygenBreather>().SetGasProvider(new GasBreatherFromWorldProvider());
		}
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x000A6F3E File Offset: 0x000A513E
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x00190128 File Offset: 0x0018E328
	public MinionConfig()
	{
		Func<RationalAi.Instance, StateMachine.Instance>[] array = BaseMinionConfig.BaseRationalAiStateMachines();
		Func<RationalAi.Instance, StateMachine.Instance>[] array2 = new Func<RationalAi.Instance, StateMachine.Instance>[9];
		array2[0] = ((RationalAi.Instance smi) => new BreathMonitor.Instance(smi.master));
		array2[1] = ((RationalAi.Instance smi) => new SteppedInMonitor.Instance(smi.master));
		array2[2] = ((RationalAi.Instance smi) => new Dreamer.Instance(smi.master));
		array2[3] = ((RationalAi.Instance smi) => new StaminaMonitor.Instance(smi.master));
		array2[4] = ((RationalAi.Instance smi) => new RationMonitor.Instance(smi.master));
		array2[5] = ((RationalAi.Instance smi) => new CalorieMonitor.Instance(smi.master));
		array2[6] = ((RationalAi.Instance smi) => new BladderMonitor.Instance(smi.master));
		array2[7] = ((RationalAi.Instance smi) => new HygieneMonitor.Instance(smi.master));
		array2[8] = ((RationalAi.Instance smi) => new TiredMonitor.Instance(smi.master));
		this.RATIONAL_AI_STATE_MACHINES = array.Append(array2);
		base..ctor();
	}

	// Token: 0x04000D98 RID: 3480
	public static Tag MODEL = GameTags.Minions.Models.Standard;

	// Token: 0x04000D99 RID: 3481
	public static string NAME = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x04000D9A RID: 3482
	public static string ID = MinionConfig.MODEL.ToString();

	// Token: 0x04000D9B RID: 3483
	public Func<RationalAi.Instance, StateMachine.Instance>[] RATIONAL_AI_STATE_MACHINES;
}
