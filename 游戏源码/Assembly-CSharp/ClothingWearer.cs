using System;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200108B RID: 4235
[AddComponentMenu("KMonoBehaviour/scripts/ClothingWearer")]
public class ClothingWearer : KMonoBehaviour
{
	// Token: 0x060056B9 RID: 22201 RVA: 0x00282D9C File Offset: 0x00280F9C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.decorProvider = base.GetComponent<DecorProvider>();
		if (this.decorModifier == null)
		{
			this.decorModifier = new AttributeModifier("Decor", 0f, DUPLICANTS.MODIFIERS.CLOTHING.NAME, false, false, false);
		}
		if (this.conductivityModifier == null)
		{
			AttributeInstance attributeInstance = base.gameObject.GetAttributes().Get("ThermalConductivityBarrier");
			this.conductivityModifier = new AttributeModifier("ThermalConductivityBarrier", ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, DUPLICANTS.MODIFIERS.CLOTHING.NAME, false, false, false);
			attributeInstance.Add(this.conductivityModifier);
		}
	}

	// Token: 0x060056BA RID: 22202 RVA: 0x00282E34 File Offset: 0x00281034
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.decorProvider.decor.Add(this.decorModifier);
		this.decorProvider.decorRadius.Add(new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, 3f, null, false, false, true));
		Traits component = base.GetComponent<Traits>();
		string format = UI.OVERLAYS.DECOR.CLOTHING;
		if (component != null)
		{
			if (component.HasTrait("DecorUp"))
			{
				format = UI.OVERLAYS.DECOR.CLOTHING_TRAIT_DECORUP;
			}
			else if (component.HasTrait("DecorDown"))
			{
				format = UI.OVERLAYS.DECOR.CLOTHING_TRAIT_DECORDOWN;
			}
		}
		this.decorProvider.overrideName = string.Format(format, base.gameObject.GetProperName());
		if (this.currentClothing == null)
		{
			this.ChangeToDefaultClothes();
		}
		else
		{
			this.ChangeClothes(this.currentClothing);
		}
		this.spawnApplyClothesHandle = GameScheduler.Instance.Schedule("ApplySpawnClothes", 2f, delegate(object obj)
		{
			base.GetComponent<CreatureSimTemperatureTransfer>().RefreshRegistration();
		}, null, null);
	}

	// Token: 0x060056BB RID: 22203 RVA: 0x000D8A54 File Offset: 0x000D6C54
	protected override void OnCleanUp()
	{
		this.spawnApplyClothesHandle.ClearScheduler();
		base.OnCleanUp();
	}

	// Token: 0x060056BC RID: 22204 RVA: 0x00282F3C File Offset: 0x0028113C
	public void ChangeClothes(ClothingWearer.ClothingInfo clothingInfo)
	{
		this.decorProvider.baseRadius = 3f;
		this.currentClothing = clothingInfo;
		this.conductivityModifier.Description = clothingInfo.name;
		this.conductivityModifier.SetValue(this.currentClothing.conductivityMod);
		this.decorModifier.SetValue((float)this.currentClothing.decorMod);
	}

	// Token: 0x060056BD RID: 22205 RVA: 0x000D8A67 File Offset: 0x000D6C67
	public void ChangeToDefaultClothes()
	{
		this.ChangeClothes(new ClothingWearer.ClothingInfo(ClothingWearer.ClothingInfo.BASIC_CLOTHING.name, ClothingWearer.ClothingInfo.BASIC_CLOTHING.decorMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.conductivityMod, ClothingWearer.ClothingInfo.BASIC_CLOTHING.homeostasisEfficiencyMultiplier));
	}

	// Token: 0x04003CB0 RID: 15536
	private DecorProvider decorProvider;

	// Token: 0x04003CB1 RID: 15537
	private SchedulerHandle spawnApplyClothesHandle;

	// Token: 0x04003CB2 RID: 15538
	private AttributeModifier decorModifier;

	// Token: 0x04003CB3 RID: 15539
	private AttributeModifier conductivityModifier;

	// Token: 0x04003CB4 RID: 15540
	[Serialize]
	public ClothingWearer.ClothingInfo currentClothing;

	// Token: 0x0200108C RID: 4236
	public class ClothingInfo
	{
		// Token: 0x060056C0 RID: 22208 RVA: 0x000D8AA9 File Offset: 0x000D6CA9
		public ClothingInfo(string _name, int _decor, float _temperature, float _homeostasisEfficiencyMultiplier)
		{
			this.name = _name;
			this.decorMod = _decor;
			this.conductivityMod = _temperature;
			this.homeostasisEfficiencyMultiplier = _homeostasisEfficiencyMultiplier;
		}

		// Token: 0x060056C1 RID: 22209 RVA: 0x00282FA0 File Offset: 0x002811A0
		public static void OnEquipVest(Equippable eq, ClothingWearer.ClothingInfo clothingInfo)
		{
			if (eq == null || eq.assignee == null)
			{
				return;
			}
			Ownables soleOwner = eq.assignee.GetSoleOwner();
			if (soleOwner == null)
			{
				return;
			}
			ClothingWearer component = (soleOwner.GetComponent<MinionAssignablesProxy>().target as KMonoBehaviour).GetComponent<ClothingWearer>();
			if (component != null)
			{
				component.ChangeClothes(clothingInfo);
				return;
			}
			global::Debug.LogWarning("Clothing item cannot be equipped to assignee because they lack ClothingWearer component");
		}

		// Token: 0x060056C2 RID: 22210 RVA: 0x00283008 File Offset: 0x00281208
		public static void OnUnequipVest(Equippable eq)
		{
			if (eq != null && eq.assignee != null)
			{
				Ownables soleOwner = eq.assignee.GetSoleOwner();
				if (soleOwner == null)
				{
					return;
				}
				MinionAssignablesProxy component = soleOwner.GetComponent<MinionAssignablesProxy>();
				if (component == null)
				{
					return;
				}
				GameObject targetGameObject = component.GetTargetGameObject();
				if (targetGameObject == null)
				{
					return;
				}
				ClothingWearer component2 = targetGameObject.GetComponent<ClothingWearer>();
				if (component2 == null)
				{
					return;
				}
				component2.ChangeToDefaultClothes();
			}
		}

		// Token: 0x060056C3 RID: 22211 RVA: 0x0014C1F0 File Offset: 0x0014A3F0
		public static void SetupVest(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes, false);
			Equippable equippable = go.GetComponent<Equippable>();
			if (equippable == null)
			{
				equippable = go.AddComponent<Equippable>();
			}
			equippable.SetQuality(global::QualityLevel.Poor);
			go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
		}

		// Token: 0x04003CB5 RID: 15541
		[Serialize]
		public string name = "";

		// Token: 0x04003CB6 RID: 15542
		[Serialize]
		public int decorMod;

		// Token: 0x04003CB7 RID: 15543
		[Serialize]
		public float conductivityMod;

		// Token: 0x04003CB8 RID: 15544
		[Serialize]
		public float homeostasisEfficiencyMultiplier;

		// Token: 0x04003CB9 RID: 15545
		public static readonly ClothingWearer.ClothingInfo BASIC_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.COOL_VEST.GENERICNAME, -5, 0.0025f, -1.25f);

		// Token: 0x04003CBA RID: 15546
		public static readonly ClothingWearer.ClothingInfo WARM_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.WARM_VEST.NAME, 0, 0.008f, -1.25f);

		// Token: 0x04003CBB RID: 15547
		public static readonly ClothingWearer.ClothingInfo COOL_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.COOL_VEST.NAME, -10, 0.0005f, 0f);

		// Token: 0x04003CBC RID: 15548
		public static readonly ClothingWearer.ClothingInfo FANCY_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.FUNKY_VEST.NAME, 30, 0.0025f, -1.25f);

		// Token: 0x04003CBD RID: 15549
		public static readonly ClothingWearer.ClothingInfo CUSTOM_CLOTHING = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME, 40, 0.0025f, -1.25f);

		// Token: 0x04003CBE RID: 15550
		public static readonly ClothingWearer.ClothingInfo SLEEP_CLINIC_PAJAMAS = new ClothingWearer.ClothingInfo(EQUIPMENT.PREFABS.CUSTOMCLOTHING.NAME, 40, 0.0025f, -1.25f);
	}
}
