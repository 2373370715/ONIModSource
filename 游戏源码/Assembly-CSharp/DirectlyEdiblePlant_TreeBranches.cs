using Klei.AI;
using TUNING;
using UnityEngine;

public class DirectlyEdiblePlant_TreeBranches : KMonoBehaviour, IPlantConsumptionInstructions
{
	private PlantBranchGrower.Instance trunk;

	protected override void OnSpawn()
	{
		trunk = base.gameObject.GetSMI<PlantBranchGrower.Instance>();
		base.OnSpawn();
	}

	public bool CanPlantBeEaten()
	{
		float num = 0.25f;
		if (GetMaxBranchMaturity() < num)
		{
			return false;
		}
		return true;
	}

	public float ConsumePlant(float desiredUnitsToConsume)
	{
		float maxBranchMaturity = GetMaxBranchMaturity();
		float num = Mathf.Min(desiredUnitsToConsume, maxBranchMaturity);
		GameObject mostMatureBranch = GetMostMatureBranch();
		if ((bool)mostMatureBranch)
		{
			Growing component = mostMatureBranch.GetComponent<Growing>();
			if ((bool)component)
			{
				Harvestable component2 = mostMatureBranch.GetComponent<Harvestable>();
				if (component2 != null)
				{
					component2.Trigger(2127324410, true);
				}
				component.ConsumeMass(num);
				return num;
			}
		}
		return 0f;
	}

	public float PlantProductGrowthPerCycle()
	{
		Crop crop = GetComponent<Crop>();
		float num = CROPS.CROP_TYPES.Find((Crop.CropVal m) => m.cropId == crop.cropId).cropDuration / 600f;
		return 1f / num;
	}

	public float GetMaxBranchMaturity()
	{
		float max_maturity = 0f;
		GameObject max_branch = null;
		trunk.ActionPerBranch(delegate(GameObject branch)
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

	private GameObject GetMostMatureBranch()
	{
		float max_maturity = 0f;
		GameObject max_branch = null;
		trunk.ActionPerBranch(delegate(GameObject branch)
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

	public string GetFormattedConsumptionPerCycle(float consumer_KGWorthOfCaloriesLostPerSecond)
	{
		float num = PlantProductGrowthPerCycle();
		return GameUtil.GetFormattedPlantGrowth(consumer_KGWorthOfCaloriesLostPerSecond * num * 100f, GameUtil.TimeSlice.PerCycle);
	}

	public CellOffset[] GetAllowedOffsets()
	{
		return null;
	}
}
