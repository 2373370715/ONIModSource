using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200109F RID: 4255
public class DirectlyEdiblePlant_TreeBranches : KMonoBehaviour, IPlantConsumptionInstructions
{
	// Token: 0x06005752 RID: 22354 RVA: 0x000D9045 File Offset: 0x000D7245
	protected override void OnSpawn()
	{
		this.trunk = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
		base.OnSpawn();
	}

	// Token: 0x06005753 RID: 22355 RVA: 0x000D905E File Offset: 0x000D725E
	public bool CanPlantBeEaten()
	{
		return this.GetMaxBranchMaturity() >= this.MinimumEdibleMaturity;
	}

	// Token: 0x06005754 RID: 22356 RVA: 0x0028556C File Offset: 0x0028376C
	public float ConsumePlant(float desiredUnitsToConsume)
	{
		float maxBranchMaturity = this.GetMaxBranchMaturity();
		float num = Mathf.Min(desiredUnitsToConsume, maxBranchMaturity);
		GameObject mostMatureBranch = this.GetMostMatureBranch();
		if (!mostMatureBranch)
		{
			return 0f;
		}
		Growing component = mostMatureBranch.GetComponent<Growing>();
		if (component)
		{
			Harvestable component2 = mostMatureBranch.GetComponent<Harvestable>();
			if (component2 != null)
			{
				component2.Trigger(2127324410, true);
			}
			component.ConsumeMass(num);
			return num;
		}
		mostMatureBranch.GetAmounts().Get(Db.Get().Amounts.Maturity.Id).ApplyDelta(-desiredUnitsToConsume);
		base.gameObject.Trigger(-1793167409, null);
		mostMatureBranch.Trigger(-1793167409, null);
		return desiredUnitsToConsume;
	}

	// Token: 0x06005755 RID: 22357 RVA: 0x00285624 File Offset: 0x00283824
	public float PlantProductGrowthPerCycle()
	{
		Crop component = base.GetComponent<Crop>();
		string cropID = component.cropId;
		if (this.overrideCropID != null)
		{
			cropID = this.overrideCropID;
		}
		float num = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == cropID).cropDuration / 600f;
		return 1f / num;
	}

	// Token: 0x06005756 RID: 22358 RVA: 0x00285688 File Offset: 0x00283888
	public float GetMaxBranchMaturity()
	{
		float max_maturity = 0f;
		GameObject max_branch = null;
		this.trunk.ActionPerBranch(delegate(GameObject branch)
		{
			if (branch != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
				if (amountInstance != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					if (num > max_maturity)
					{
						max_maturity = num;
						max_branch = branch;
					}
				}
			}
		});
		return max_maturity;
	}

	// Token: 0x06005757 RID: 22359 RVA: 0x002856CC File Offset: 0x002838CC
	private GameObject GetMostMatureBranch()
	{
		float max_maturity = 0f;
		GameObject max_branch = null;
		this.trunk.ActionPerBranch(delegate(GameObject branch)
		{
			if (branch != null)
			{
				AmountInstance amountInstance = Db.Get().Amounts.Maturity.Lookup(branch);
				if (amountInstance != null)
				{
					float num = amountInstance.value / amountInstance.GetMax();
					if (num > max_maturity)
					{
						max_maturity = num;
						max_branch = branch;
					}
				}
			}
		});
		return max_branch;
	}

	// Token: 0x06005758 RID: 22360 RVA: 0x00285710 File Offset: 0x00283910
	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		float num = this.PlantProductGrowthPerCycle();
		return GameUtil.GetFormattedPlantGrowth(consumer_KGWorthOfCaloriesLostPerSecond * num * 100f, GameUtil.TimeSlice.PerCycle);
	}

	// Token: 0x06005759 RID: 22361 RVA: 0x000AD332 File Offset: 0x000AB532
	public CellOffset[] GetAllowedOffsets()
	{
		return null;
	}

	// Token: 0x0600575A RID: 22362 RVA: 0x000A65EC File Offset: 0x000A47EC
	public Diet.Info.FoodType GetDietFoodType()
	{
		return Diet.Info.FoodType.EatPlantDirectly;
	}

	// Token: 0x04003CFA RID: 15610
	private PlantBranchGrower.Instance trunk;

	// Token: 0x04003CFB RID: 15611
	public float MinimumEdibleMaturity = 0.25f;

	// Token: 0x04003CFC RID: 15612
	public string overrideCropID;
}
