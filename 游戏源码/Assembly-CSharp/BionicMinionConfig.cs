using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public class BionicMinionConfig : IEntityConfig
{
	// Token: 0x060012B9 RID: 4793 RVA: 0x000AE6B8 File Offset: 0x000AC8B8
	public static string[] GetAttributes()
	{
		return BaseMinionConfig.BaseMinionAttributes();
	}

	// Token: 0x060012BA RID: 4794 RVA: 0x0018B170 File Offset: 0x00189370
	public static string[] GetAmounts()
	{
		return BaseMinionConfig.BaseMinionAmounts().Append(new string[]
		{
			Db.Get().Amounts.BionicOil.Id,
			Db.Get().Amounts.BionicGunk.Id,
			Db.Get().Amounts.BionicInternalBattery.Id,
			Db.Get().Amounts.BionicOxygenTank.Id
		});
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x000AE6BF File Offset: 0x000AC8BF
	public static AttributeModifier[] GetTraits()
	{
		return BaseMinionConfig.BaseMinionTraits(BionicMinionConfig.MODEL);
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x0018B1EC File Offset: 0x001893EC
	public GameObject CreatePrefab()
	{
		GameObject gameObject = BaseMinionConfig.BaseMinion(BionicMinionConfig.MODEL, BionicMinionConfig.GetAttributes(), BionicMinionConfig.GetAmounts(), BionicMinionConfig.GetTraits());
		Storage storage = gameObject.AddComponent<Storage>();
		storage.storageID = GameTags.StoragesIds.BionicBatteryStorage;
		storage.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Preserve,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		storage.storageFilters = new List<Tag>
		{
			GameTags.ChargedPortableBattery,
			GameTags.EmptyPortableBattery
		};
		storage.allowItemRemoval = false;
		storage.showInUI = false;
		Storage storage2 = gameObject.AddComponent<Storage>();
		storage2.storageID = GameTags.StoragesIds.BionicUpgradeStorage;
		storage2.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Preserve,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		storage2.storageFilters = new List<Tag>
		{
			GameTags.BionicUpgrade
		};
		storage2.allowItemRemoval = false;
		storage2.showInUI = false;
		Storage storage3 = gameObject.AddComponent<Storage>();
		storage3.capacityKg = BionicOxygenTankMonitor.OXYGEN_TANK_CAPACITY_KG;
		storage3.storageID = GameTags.StoragesIds.BionicOxygenTankStorage;
		storage3.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>
		{
			Storage.StoredItemModifier.Hide,
			Storage.StoredItemModifier.Preserve,
			Storage.StoredItemModifier.Seal,
			Storage.StoredItemModifier.Insulate
		});
		storage3.allowItemRemoval = false;
		storage3.showInUI = false;
		ManualDeliveryKG manualDeliveryKG = gameObject.AddComponent<ManualDeliveryKG>();
		manualDeliveryKG.SetStorage(storage);
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.FetchCritical.IdHash;
		manualDeliveryKG.capacity = 0f;
		manualDeliveryKG.refillMass = 0f;
		gameObject.AddOrGet<ReanimateBionicWorkable>();
		gameObject.AddOrGet<WarmBlooded>().complexity = WarmBlooded.ComplexityType.HomeostasisWithoutCaloriesImpact;
		return gameObject;
	}

	// Token: 0x060012BD RID: 4797 RVA: 0x0018B374 File Offset: 0x00189574
	public void OnPrefabInit(GameObject go)
	{
		BaseMinionConfig.BasePrefabInit(go, BionicMinionConfig.MODEL);
		AmountInstance amountInstance = Db.Get().Amounts.BionicOil.Lookup(go);
		amountInstance.value = amountInstance.GetMax();
		AmountInstance amountInstance2 = Db.Get().Amounts.BionicGunk.Lookup(go);
		amountInstance2.value = amountInstance2.GetMin();
	}

	// Token: 0x060012BE RID: 4798 RVA: 0x0018B3CC File Offset: 0x001895CC
	public void OnSpawn(GameObject go)
	{
		Sensors component = go.GetComponent<Sensors>();
		component.Add(new ClosestElectrobankSensor(component, true));
		component.Add(new ClosestOxygenCanisterSensor(component, false));
		component.Add(new ClosestOxyliteSensor(component, false));
		BaseMinionConfig.BaseOnSpawn(go, BionicMinionConfig.MODEL, this.RATIONAL_AI_STATE_MACHINES);
		BionicOxygenTankMonitor.Instance smi = go.GetSMI<BionicOxygenTankMonitor.Instance>();
		if (go.GetComponent<OxygenBreather>().GetGasProvider() == null)
		{
			go.GetComponent<OxygenBreather>().SetGasProvider(smi);
		}
		this.UnlockCraftingTable(go);
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x000AE6CB File Offset: 0x000AC8CB
	private void UnlockCraftingTable(GameObject instance)
	{
		GameScheduler.Instance.Schedule("BionicUnlockCraftingTable", 8f, delegate(object data)
		{
			TechItem techItem = Db.Get().TechItems.Get("CraftingTable");
			if (!techItem.IsComplete())
			{
				Notifier component = Game.Instance.GetComponent<Notifier>();
				Notification notification = new Notification(MISC.NOTIFICATIONS.BIONICRESEARCHUNLOCK.NAME, NotificationType.MessageImportant, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.BIONICRESEARCHUNLOCK.MESSAGEBODY.Replace("{0}", Assets.GetPrefab("CraftingTable").GetProperName()), Assets.GetPrefab("CraftingTable").GetProperName(), false, 0f, null, null, null, true, false, false);
				component.Add(notification, "");
				techItem.POIUnlocked();
			}
		}, null, null);
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x000A5F37 File Offset: 0x000A4137
	public string[] GetDlcIds()
	{
		return DlcManager.DLC3;
	}

	// Token: 0x060012C1 RID: 4801 RVA: 0x0018B440 File Offset: 0x00189640
	public BionicMinionConfig()
	{
		Func<RationalAi.Instance, StateMachine.Instance>[] array = BaseMinionConfig.BaseRationalAiStateMachines();
		Func<RationalAi.Instance, StateMachine.Instance>[] array2 = new Func<RationalAi.Instance, StateMachine.Instance>[9];
		array2[0] = ((RationalAi.Instance smi) => new BreathMonitor.Instance(smi.master)
		{
			canRecoverBreath = false
		});
		array2[1] = ((RationalAi.Instance smi) => new BionicSuffocationMonitor.Instance(smi.master, new BionicSuffocationMonitor.Def()));
		array2[2] = ((RationalAi.Instance smi) => new SteppedInMonitor.Instance(smi.master, new string[]
		{
			"CarpetFeet"
		}));
		array2[3] = ((RationalAi.Instance smi) => new BionicBatteryMonitor.Instance(smi.master, new BionicBatteryMonitor.Def()));
		array2[4] = ((RationalAi.Instance smi) => new BionicOilMonitor.Instance(smi.master, new BionicOilMonitor.Def()));
		array2[5] = ((RationalAi.Instance smi) => new GunkMonitor.Instance(smi.master, new GunkMonitor.Def()));
		array2[6] = ((RationalAi.Instance smi) => new BionicWaterDamageMonitor.Instance(smi.master, new BionicWaterDamageMonitor.Def()));
		array2[7] = ((RationalAi.Instance smi) => new BionicUpgradesMonitor.Instance(smi.master, new BionicUpgradesMonitor.Def()));
		array2[8] = ((RationalAi.Instance smi) => new BionicOxygenTankMonitor.Instance(smi.master, new BionicOxygenTankMonitor.Def()));
		this.RATIONAL_AI_STATE_MACHINES = array.Append(array2);
		base..ctor();
	}

	// Token: 0x04000CD4 RID: 3284
	public static Tag MODEL = GameTags.Minions.Models.Bionic;

	// Token: 0x04000CD5 RID: 3285
	public static string NAME = DUPLICANTS.MODEL.BIONIC.NAME;

	// Token: 0x04000CD6 RID: 3286
	public static string ID = BionicMinionConfig.MODEL.ToString();

	// Token: 0x04000CD7 RID: 3287
	public static string[] DEFAULT_BIONIC_TRAITS = new string[]
	{
		"BionicBaseline"
	};

	// Token: 0x04000CD8 RID: 3288
	public Func<RationalAi.Instance, StateMachine.Instance>[] RATIONAL_AI_STATE_MACHINES;
}
