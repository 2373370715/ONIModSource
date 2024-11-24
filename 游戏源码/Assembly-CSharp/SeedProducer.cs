using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020011DB RID: 4571
[AddComponentMenu("KMonoBehaviour/scripts/SeedProducer")]
public class SeedProducer : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06005D0A RID: 23818 RVA: 0x000DCBCF File Offset: 0x000DADCF
	public void Configure(string SeedID, SeedProducer.ProductionType productionType, int newSeedsProduced = 1)
	{
		this.seedInfo.seedId = SeedID;
		this.seedInfo.productionType = productionType;
		this.seedInfo.newSeedsProduced = newSeedsProduced;
	}

	// Token: 0x06005D0B RID: 23819 RVA: 0x000DCBF5 File Offset: 0x000DADF5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SeedProducer>(-216549700, SeedProducer.DropSeedDelegate);
		base.Subscribe<SeedProducer>(1623392196, SeedProducer.DropSeedDelegate);
		base.Subscribe<SeedProducer>(-1072826864, SeedProducer.CropPickedDelegate);
	}

	// Token: 0x06005D0C RID: 23820 RVA: 0x0029D260 File Offset: 0x0029B460
	private GameObject ProduceSeed(string seedId, int units = 1, bool canMutate = true)
	{
		if (seedId != null && units > 0)
		{
			Vector3 position = base.gameObject.transform.GetPosition() + new Vector3(0f, 0.5f, 0f);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag(seedId)), position, Grid.SceneLayer.Ore, null, 0);
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null)
			{
				MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
				bool flag = false;
				if (canMutate && component2 != null && component2.IsOriginal)
				{
					flag = this.RollForMutation();
				}
				if (flag)
				{
					component2.Mutate();
				}
				else
				{
					component.CopyMutationsTo(component2);
				}
			}
			PrimaryElement component3 = base.gameObject.GetComponent<PrimaryElement>();
			PrimaryElement component4 = gameObject.GetComponent<PrimaryElement>();
			component4.Temperature = component3.Temperature;
			component4.Units = (float)units;
			base.Trigger(472291861, gameObject);
			gameObject.SetActive(true);
			string text = gameObject.GetProperName();
			if (component != null)
			{
				text = component.GetSubSpeciesInfo().GetNameWithMutations(text, component.IsIdentified, false);
			}
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, text, gameObject.transform, 1.5f, false);
			return gameObject;
		}
		return null;
	}

	// Token: 0x06005D0D RID: 23821 RVA: 0x0029D394 File Offset: 0x0029B594
	public void DropSeed(object data = null)
	{
		if (this.droppedSeedAlready)
		{
			return;
		}
		if (this.seedInfo.newSeedsProduced <= 0)
		{
			return;
		}
		GameObject gameObject = this.ProduceSeed(this.seedInfo.seedId, this.seedInfo.newSeedsProduced, false);
		Uprootable component = base.GetComponent<Uprootable>();
		if (component != null && component.worker != null)
		{
			gameObject.Trigger(580035959, component.worker);
		}
		base.Trigger(-1736624145, gameObject);
		this.droppedSeedAlready = true;
	}

	// Token: 0x06005D0E RID: 23822 RVA: 0x000DCC30 File Offset: 0x000DAE30
	public void CropDepleted(object data)
	{
		this.DropSeed(null);
	}

	// Token: 0x06005D0F RID: 23823 RVA: 0x0029D41C File Offset: 0x0029B61C
	public void CropPicked(object data)
	{
		if (this.seedInfo.productionType == SeedProducer.ProductionType.Harvest)
		{
			WorkerBase completed_by = base.GetComponent<Harvestable>().completed_by;
			float num = 0.1f;
			if (completed_by != null)
			{
				num += completed_by.GetComponent<AttributeConverters>().Get(Db.Get().AttributeConverters.SeedHarvestChance).Evaluate();
			}
			int num2 = (UnityEngine.Random.Range(0f, 1f) <= num) ? 1 : 0;
			if (num2 > 0)
			{
				this.ProduceSeed(this.seedInfo.seedId, num2, true).Trigger(580035959, completed_by);
			}
		}
	}

	// Token: 0x06005D10 RID: 23824 RVA: 0x0029D4B0 File Offset: 0x0029B6B0
	public bool RollForMutation()
	{
		AttributeInstance attributeInstance = Db.Get().PlantAttributes.MaxRadiationThreshold.Lookup(this);
		int num = Grid.PosToCell(base.gameObject);
		float num2 = Mathf.Clamp(Grid.IsValidCell(num) ? Grid.Radiation[num] : 0f, 0f, attributeInstance.GetTotalValue()) / attributeInstance.GetTotalValue() * 0.8f;
		return UnityEngine.Random.value < num2;
	}

	// Token: 0x06005D11 RID: 23825 RVA: 0x0029D520 File Offset: 0x0029B720
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Assets.GetPrefab(new Tag(this.seedInfo.seedId)) != null;
		switch (this.seedInfo.productionType)
		{
		case SeedProducer.ProductionType.Hidden:
		case SeedProducer.ProductionType.DigOnly:
		case SeedProducer.ProductionType.Crop:
			return null;
		case SeedProducer.ProductionType.Harvest:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_HARVEST, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_HARVEST, Descriptor.DescriptorType.Lifecycle, true));
			list.Add(new Descriptor(string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.BONUS_SEEDS, GameUtil.GetFormattedPercent(10f, GameUtil.TimeSlice.None)), string.Format(UI.UISIDESCREENS.PLANTERSIDESCREEN.TOOLTIPS.BONUS_SEEDS, GameUtil.GetFormattedPercent(10f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false));
			break;
		case SeedProducer.ProductionType.Fruit:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_FRUIT, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_DIG_ONLY, Descriptor.DescriptorType.Lifecycle, true));
			break;
		case SeedProducer.ProductionType.Sterile:
			list.Add(new Descriptor(UI.GAMEOBJECTEFFECTS.MUTANT_STERILE, UI.GAMEOBJECTEFFECTS.TOOLTIPS.MUTANT_STERILE, Descriptor.DescriptorType.Effect, false));
			break;
		default:
			DebugUtil.Assert(false, "Seed producer type descriptor not specified");
			return null;
		}
		return list;
	}

	// Token: 0x040041D6 RID: 16854
	public SeedProducer.SeedInfo seedInfo;

	// Token: 0x040041D7 RID: 16855
	private bool droppedSeedAlready;

	// Token: 0x040041D8 RID: 16856
	private static readonly EventSystem.IntraObjectHandler<SeedProducer> DropSeedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>(delegate(SeedProducer component, object data)
	{
		component.DropSeed(data);
	});

	// Token: 0x040041D9 RID: 16857
	private static readonly EventSystem.IntraObjectHandler<SeedProducer> CropPickedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>(delegate(SeedProducer component, object data)
	{
		component.CropPicked(data);
	});

	// Token: 0x020011DC RID: 4572
	[Serializable]
	public struct SeedInfo
	{
		// Token: 0x040041DA RID: 16858
		public string seedId;

		// Token: 0x040041DB RID: 16859
		public SeedProducer.ProductionType productionType;

		// Token: 0x040041DC RID: 16860
		public int newSeedsProduced;
	}

	// Token: 0x020011DD RID: 4573
	public enum ProductionType
	{
		// Token: 0x040041DE RID: 16862
		Hidden,
		// Token: 0x040041DF RID: 16863
		DigOnly,
		// Token: 0x040041E0 RID: 16864
		Harvest,
		// Token: 0x040041E1 RID: 16865
		Fruit,
		// Token: 0x040041E2 RID: 16866
		Sterile,
		// Token: 0x040041E3 RID: 16867
		Crop
	}
}
