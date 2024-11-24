using System;
using Klei.CustomSettings;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02001256 RID: 4694
[AddComponentMenu("KMonoBehaviour/scripts/Durability")]
public class Durability : KMonoBehaviour
{
	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x0600602A RID: 24618 RVA: 0x000DEABB File Offset: 0x000DCCBB
	// (set) Token: 0x0600602B RID: 24619 RVA: 0x000DEAC3 File Offset: 0x000DCCC3
	public float TimeEquipped
	{
		get
		{
			return this.timeEquipped;
		}
		set
		{
			this.timeEquipped = value;
		}
	}

	// Token: 0x0600602C RID: 24620 RVA: 0x000DEACC File Offset: 0x000DCCCC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Durability>(-1617557748, Durability.OnEquippedDelegate);
		base.Subscribe<Durability>(-170173755, Durability.OnUnequippedDelegate);
	}

	// Token: 0x0600602D RID: 24621 RVA: 0x002ACFDC File Offset: 0x002AB1DC
	protected override void OnSpawn()
	{
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.Durability, base.gameObject);
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Durability);
		if (currentQualitySetting != null)
		{
			string id = currentQualitySetting.id;
			if (id == "Indestructible")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.INDESTRUCTIBLE_DURABILITY_MOD;
				return;
			}
			if (id == "Reinforced")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.REINFORCED_DURABILITY_MOD;
				return;
			}
			if (id == "Flimsy")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.FLIMSY_DURABILITY_MOD;
				return;
			}
			if (!(id == "Threadbare"))
			{
				return;
			}
			this.difficultySettingMod = EQUIPMENT.SUITS.THREADBARE_DURABILITY_MOD;
		}
	}

	// Token: 0x0600602E RID: 24622 RVA: 0x000DEAF6 File Offset: 0x000DCCF6
	private void OnEquipped()
	{
		if (!this.isEquipped)
		{
			this.isEquipped = true;
			this.timeEquipped = GameClock.Instance.GetTimeInCycles();
		}
	}

	// Token: 0x0600602F RID: 24623 RVA: 0x002AD088 File Offset: 0x002AB288
	private void OnUnequipped()
	{
		if (this.isEquipped)
		{
			this.isEquipped = false;
			float num = GameClock.Instance.GetTimeInCycles() - this.timeEquipped;
			this.DeltaDurability(num * this.durabilityLossPerCycle);
		}
	}

	// Token: 0x06006030 RID: 24624 RVA: 0x000DEB17 File Offset: 0x000DCD17
	private void DeltaDurability(float delta)
	{
		delta *= this.difficultySettingMod;
		this.durability = Mathf.Clamp01(this.durability + delta);
	}

	// Token: 0x06006031 RID: 24625 RVA: 0x002AD0C4 File Offset: 0x002AB2C4
	public void ConvertToWornObject()
	{
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.wornEquipmentPrefabID), Grid.SceneLayer.Ore, null, 0);
		gameObject.transform.SetPosition(base.transform.GetPosition());
		gameObject.GetComponent<PrimaryElement>().SetElement(base.GetComponent<PrimaryElement>().ElementID, false);
		gameObject.SetActive(true);
		EquippableFacade component = base.GetComponent<EquippableFacade>();
		if (component != null)
		{
			gameObject.GetComponent<RepairableEquipment>().facadeID = component.FacadeID;
		}
		Storage component2 = base.gameObject.GetComponent<Storage>();
		if (component2)
		{
			JetSuitTank component3 = base.gameObject.GetComponent<JetSuitTank>();
			if (component3)
			{
				component2.AddLiquid(SimHashes.Petroleum, component3.amount, base.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0, false, true);
			}
			component2.DropAll(false, false, default(Vector3), true, null);
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06006032 RID: 24626 RVA: 0x002AD1B0 File Offset: 0x002AB3B0
	public float GetDurability()
	{
		if (this.isEquipped)
		{
			float num = GameClock.Instance.GetTimeInCycles() - this.timeEquipped;
			return this.durability - num * this.durabilityLossPerCycle;
		}
		return this.durability;
	}

	// Token: 0x06006033 RID: 24627 RVA: 0x000DEB36 File Offset: 0x000DCD36
	public bool IsWornOut()
	{
		return this.GetDurability() <= 0f;
	}

	// Token: 0x04004436 RID: 17462
	private static readonly EventSystem.IntraObjectHandler<Durability> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Durability>(delegate(Durability component, object data)
	{
		component.OnEquipped();
	});

	// Token: 0x04004437 RID: 17463
	private static readonly EventSystem.IntraObjectHandler<Durability> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<Durability>(delegate(Durability component, object data)
	{
		component.OnUnequipped();
	});

	// Token: 0x04004438 RID: 17464
	[Serialize]
	private bool isEquipped;

	// Token: 0x04004439 RID: 17465
	[Serialize]
	private float timeEquipped;

	// Token: 0x0400443A RID: 17466
	[Serialize]
	private float durability = 1f;

	// Token: 0x0400443B RID: 17467
	public float durabilityLossPerCycle = -0.1f;

	// Token: 0x0400443C RID: 17468
	public string wornEquipmentPrefabID;

	// Token: 0x0400443D RID: 17469
	private float difficultySettingMod = 1f;
}
